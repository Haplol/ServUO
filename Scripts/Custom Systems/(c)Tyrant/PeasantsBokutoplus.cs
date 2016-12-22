using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class PeasantsBokutoplus : Bokuto
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public PeasantsBokutoplus()
            : base()
        {
			Name = "(Shiny) Peasant's Bokuto";
			this.EngravedText = "Breaks SSI Cap";
            this.WeaponAttributes.SelfRepair = 3;
            this.Attributes.WeaponSpeed = 20;
			this.WeaponAttributes.HitLightning = 100;
			this.WeaponAttributes.HitFireball = 100;
			this.WeaponAttributes.HitMagicArrow = 100;
			this.WeaponAttributes.HitHarm = 100;
        }

        public PeasantsBokutoplus(Serial serial)
            : base(serial)
        {
        }
		
		public override int AosMinDamage
        {
            get
            {
                return 1;
            }
        }
		public override int AosMaxDamage
        {
            get
            {
                return 5;
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