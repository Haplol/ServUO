#region Header
// **********
// ServUO - BardSpell.cs
// **********
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.PartySystem;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

#endregion

namespace Server.Spells.Bard
{

    public abstract class BardChant : Spell
    {
        protected BardChant(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        { }

        public virtual double RequiredSkill { get { return 90.0; } }
        public abstract int RequiredMana { get; }
        public override bool ClearHandsOnCast { get { return false; } }
        public override int CastRecoveryBase { get { return 8; } }
        public virtual Type SongType { get; set; }
        public virtual int UpkeepCost { get { return 5; } }

        public override bool CheckCast()
        {
            int mana = ScaleMana(RequiredMana);

            if (!base.CheckCast())
            {
                return false;
            }

            if (Caster.Mana < mana)
            {
                Caster.SendLocalizedMessage(1060174, mana.ToString()); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            return true;
        }

        public override void SayMantra()
        {
            Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, Info.Mantra, false);
        }

        public override void DoFizzle()
        {
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = Core.TickCount;
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
            {
                Caster.PlaySound(0x1D6);
            }
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 13, (int)(GetCastDelay().TotalSeconds * 28), 39, 3);
        }

        public override int GetMana()
        {
            return 0;
        }

        protected bool CanSing()
        {
            BardTimer timer = BardHelper.GetActiveSong(Caster, SongType);

            if (timer == null)
                return true;
            
            timer.EndSong();
            
            return false;
        }

        public static bool CheckInsturment(Mobile from)
        {
            if (from.Backpack == null)
                return false;

            BaseInstrument inst = (BaseInstrument)from.Backpack.FindItemByType(typeof(BaseInstrument));

            if (inst != null)
            {
                inst.ConsumeUse(from);
                return true;
            }
            else
                from.SendLocalizedMessage(1062488);

            return false;
        }
    }

    public class BardTimer : Timer
    {

        protected List<Mobile> m_Targets;

        protected readonly Mobile m_Caster;

        protected int m_TotalRounds = -1;
        protected int m_CurrentRound = 0;

        protected bool m_Beneficial;

        protected BardEffect m_BardEffect;

        public BardTimer(Mobile caster, bool beneficial, BardEffect effect)
            : this(caster, beneficial, effect, -1)
        {

        }

        public BardTimer(Mobile caster, bool beneficial, BardEffect effect, int rounds)
            : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2))
        {
            this.m_Targets = new List<Mobile>();
            this.m_Caster = caster;
            this.m_Beneficial = beneficial;
            this.m_BardEffect = effect;

            if (rounds > 0)
                m_TotalRounds = rounds;

            this.Priority = TimerPriority.OneSecond;
            ((PlayerMobile) caster).ActiveSongs.Add(this);
            this.Start();

        }

        public void EndSong()
        {
            m_Caster.SendLocalizedMessage(1115710); // Your spell song has been interrupted.
            CleanTargets(true);
            if (m_Caster is PlayerMobile)
                ((PlayerMobile) m_Caster).ActiveSongs.Remove(this);
            this.Stop();
        }

        protected void CleanTargets(bool cleanAll = false)
        {
            if (cleanAll)
            {
                foreach (Mobile m in m_Targets.ToList())
                {
                    EndEffect(m);
                }
            }
            else
            {
                foreach (Mobile m in m_Targets.ToList())
                {
                    /*if ((!m.InRange(m_Caster.Location, 8) || (m_Beneficial && (m_Caster.Party == null || (m_Caster.Party != null && m is PlayerMobile && !((Party)m_Caster.Party).Contains(m))))) && m != m_Caster)
                        EndEffect(m);*/					
					
					if ( m != m_Caster && !m.InRange(m_Caster.Location, 8) )
					{
						EndEffect(m);
					}
					else if ( m != m_Caster && m_Beneficial )
					{
						if ( m_Caster.Party == null )
						{
							EndEffect(m);
						}
						else if( m_Caster.Party != null && m is PlayerMobile )
						{
							Server.Engines.PartySystem.Party this_Party = (Server.Engines.PartySystem.Party)m_Caster.Party;
							
							if ( !this_Party.Contains(m))
								EndEffect(m);	
						}	
					}					
                }
            }
        }

        protected void AddTarget(Mobile target)
        {
            if (!m_Targets.Contains(target) && target.InRange(m_Caster.Location, 8) && BardHelper.HasEffect(target, m_BardEffect) == null && IsTarget(target))
                StartEffect(target);

        }

        protected override void OnTick()
        {
            CleanTargets();

            if (m_Beneficial)
            {
                Party party = Party.Get(m_Caster);

                if (party != null)
                {
                    foreach (PartyMemberInfo info in party.Members)
                    {
                        Mobile partyMember = info.Mobile;

                        AddTarget(partyMember);
                    }
                }
                else
                {
                    AddTarget(m_Caster);
                }
            }

            foreach (Mobile m in m_Targets)
                Effect(m);

            if (m_Targets.Count == 0 || m_Caster.Mana < BardHelper.GetUpkeepCost(m_Caster, m_BardEffect, m_Targets.Count))
                EndSong();
            else if (!m_Beneficial && ++m_CurrentRound >= m_TotalRounds)
                EndSong();
            else
                m_Caster.Mana -= BardHelper.GetUpkeepCost(m_Caster, m_BardEffect, m_Targets.Count);
        }

        protected virtual bool IsTarget(Mobile m)
        {
            return false;
        }

        protected virtual void Effect(Mobile m)
        {
            m.FixedEffect(0x376A, 10, (int)TimeSpan.FromSeconds(1).TotalSeconds * 28, 97, 3);
        }

        protected virtual void StartEffect(Mobile m)
        {
            if (m is PlayerMobile)
            {
                ((PlayerMobile)m).AddBuff(BardHelper.GenerateBuffInfo(m_BardEffect, m_Caster));
                m_Targets.Add(m);
            }
            else
                m_Targets.Add(m);
            
        }

        protected virtual void EndEffect(Mobile m)
        {
            if (m is PlayerMobile)
            {
                ((PlayerMobile)m).RemoveBuff(BardHelper.GenerateBuffInfo(m_BardEffect, m_Caster));
                m_Targets.Remove(m);
                m.SendLocalizedMessage(1149722); // Your spellsong has ended.
            }
            else
                m_Targets.Remove(m);
        }
       
    }

    public class BardTarget : Target
    {
        public BardTarget() 
            : base(8, false, TargetFlags.None)
        {
        }
    }
}