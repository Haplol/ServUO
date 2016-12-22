using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Engines.ConPVP;

namespace Server.Spells.Bard
{
    public class DespairChant : BardChant
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
                    "Despair", "Kal Des Mani Tym",
                    -1,
                    9002
        );

        public DespairChant(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {            
        }

        public override Type SongType { get { return typeof (DespairUpkeepTimer); } }

        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.75); }
        }

        public override double RequiredSkill
        {
            get { return 90.0; }
        }
        public override int RequiredMana
        {
            get { return 20; }
        }

        public override int UpkeepCost
        {
            get { return 18; }
        }

        public override bool BlocksMovement
        {
            get { return false; }
        }

        public override bool DelayedDamage
        {
            get { return false; }
        }

        public override void OnCast()
        {
            if (CanSing())
            {
                if (BardChant.CheckInsturment(Caster))
                {
                    Caster.Target = new DespairTarget(Caster);
                    Caster.NextSkillTime = Core.TickCount + 21600000;
                }
                else
                {
                    BaseInstrument.PickInstrument(Caster, OnPickedInstrument);
                }
            }           
				
			this.FinishSequence();			
        }

        public static void OnPickedInstrument(Mobile from, BaseInstrument instrument)
        {
            from.RevealingAction();
            from.Target = new DespairTarget(from, instrument);
            from.NextSkillTime = Core.TickCount + 21600000;
        }
    }

    public class DespairTarget : Target
    {
        private readonly BaseInstrument m_Instrument;
        private readonly bool m_SetSkillTime = true;

        public DespairTarget(Mobile from, BaseInstrument instrument) : base(BaseInstrument.GetBardRange(from, SkillName.Discordance), false, TargetFlags.Harmful)
        {
            m_Instrument = instrument;
        }

        public DespairTarget(Mobile from) : base(BaseInstrument.GetBardRange(from, SkillName.Discordance), false, TargetFlags.Harmful)
        {
            m_Instrument = (BaseInstrument)from.Backpack.FindItemByType(typeof(BaseInstrument));
        }

        protected override void OnTargetFinish(Mobile @from)
        {
            if (m_SetSkillTime)
                from.NextSkillTime = Core.TickCount;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {

            from.RevealingAction();

            if (targeted is Mobile)
            {
                Mobile targ = (Mobile)targeted;

                if (!m_Instrument.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                }
                else
                {
                    m_Instrument.ConsumeUse(from);
                    from.DoHarmful(targ);
                    DespairUpkeepTimer song = new DespairUpkeepTimer(from, targ);
                }
            }           
        }
    }

    public class DespairUpkeepTimer : BardTimer
    {
        private readonly Mobile m_Target;

        public DespairUpkeepTimer(Mobile caster, Mobile target) : base(caster, false, BardEffect.Despair, 11)
        {
            m_Target = target;
            AddTarget(target);
        }

        protected override bool IsTarget(Mobile m)
        {
            return m_Target == m;
        }

        protected override void StartEffect(Mobile m)
        {
            BardHelper.AddEffect(m_Caster, m, BardEffect.Despair);
            m.AddStatMod(new StatMod(StatType.Str, "Despair", -1 * BardHelper.Scaler(m_Caster, 4, 16, 1), TimeSpan.FromHours(1)));

            base.StartEffect(m);
        }

        protected override void Effect(Mobile m)
        {		
			if ( m_Caster is PlayerMobile )
			{
				PlayerMobile pm = m_Caster as PlayerMobile;
				
				if ( m_Caster != m )
				{					
					if ( pm.BardDamageType == BardDamageType.PhysicalDamage )
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1), 100, 0, 0, 0, 0);
					}
					else if ( pm.BardDamageType == BardDamageType.FireDamage )
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1), 0, 100, 0, 0, 0);
					}					
					else if ( pm.BardDamageType == BardDamageType.ColdDamage )
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1), 0, 0, 100, 0, 0);
					}					
					else if ( pm.BardDamageType == BardDamageType.PoisonDamage )
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1), 0, 0, 0, 100, 0);
					}					
					else if ( pm.BardDamageType == BardDamageType.EnergyDamage )
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1), 0, 0, 0, 0, 100);
					}
					else
					{
						SpellHelper.Damage(TimeSpan.FromSeconds(2), m, m_Caster, BardHelper.Scaler(m_Caster, m, 9, 36, 1));
					}
				}
					
				base.Effect(m);
			}
        }

        protected override void EndEffect(Mobile m)
        {
            BardHelper.RemoveEffect(m, BardEffect.Despair);
            m.RemoveStatMod("Despair");

            base.EndEffect(m);
        }
    }
}