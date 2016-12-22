using System;
using System.Collections.Generic;
using Server.Targeting;

namespace Server.Spells.Seventh
{
    public class MeteorSwarmSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Meteor Swarm", "Flam Kal Des Ylem",
            233,
            9042,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk);
        public MeteorSwarmSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Seventh;
            }
        }
        public override bool DelayedDamage
        {
            get
            {
                return true;
            }
        }
        public override void OnCast()
        {
            this.Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!this.Caster.CanSee(p))
            {
                this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, this.Caster) && this.CheckSequence())
            {
                SpellHelper.Turn(this.Caster, p);

                if (p is Item)
                    p = ((Item)p).GetWorldLocation();

                List<Mobile> targets = new List<Mobile>();

                Map map = this.Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 2);

                    foreach (Mobile m in eable)
                    {
                        if (this.Caster != m && SpellHelper.ValidIndirectTarget(this.Caster, m) && this.Caster.CanBeHarmful(m, false))
                        {
                            if (Core.AOS && !this.Caster.InLOS(m))
                                continue;

                            targets.Add(m);
                        }
                    }

                    eable.Free();
                }

                double damage;

                if (targets.Count > 0)
                {
                    Effects.PlaySound(p, this.Caster.Map, 0x160);

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = targets[i];

                        if (Core.AOS)
                            damage = this.GetNewAosDamage(51, 1, 5, m.Player, m);
                        else
                            damage = Utility.Random(27, 22);

                      

                        if (!Core.AOS && this.CheckResisted(m))
                        {
                            damage *= 0.5;

                            m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                        }

                        damage *= this.GetDamageScalar(m);
                        this.Caster.DoHarmful(m);
                        SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);

                        this.Caster.MovingParticles(m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100);
                    }
                }
            }

            this.FinishSequence();
        }

        private class InternalTarget : Target
        {
            private readonly MeteorSwarmSpell m_Owner;
            public InternalTarget(MeteorSwarmSpell owner)
                : base(Core.ML ? 10 : 12, true, TargetFlags.None)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    this.m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}