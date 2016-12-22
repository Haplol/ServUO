//Neira
using System;
using System.Collections;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class NoxRangersHeavyCrossbowplus : HeavyCrossbow
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public NoxRangersHeavyCrossbowplus()
        {
			Name = "(Shiny) Nox Ranger's Heavy Crossbow";
			this.EngravedText = "Low SSI Cap";
            this.Hue = 0x58C;
            this.Attributes.SpellChanneling = 1;
            this.Attributes.WeaponSpeed = -75;
            this.Attributes.WeaponDamage = 100;
			this.WeaponAttributes.HitPoisonArea = 20;

			                    XmlAttach.AttachTo(this, 
                        new XmlCustomAttacks( 
                            new XmlCustomAttacks.SpecialAttacks [] 
                            { 
                            XmlCustomAttacks.SpecialAttacks.SnipingShot
                            }
                        )
                    );
        }

        public NoxRangersHeavyCrossbowplus(Serial serial)
            : base(serial)
        {
        }
		public override int AosMinDamage
        {
            get
            {
                return 25;
            }
        }
		public override int AosMaxDamage
        {
            get
            {
                return 35;
            }
        }

        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            pois = 100;

            phys = fire = cold = nrgy = chaos = direct = 0;
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}