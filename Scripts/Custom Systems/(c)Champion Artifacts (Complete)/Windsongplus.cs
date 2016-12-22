using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class Windsongplus : MagicalShortbow
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public Windsongplus()
            : base()
        {
            this.Hue = 0xF7;
			Name = "(Shiny) Windsong";
			this.EngravedText = "Breaks SSI Cap";
            this.Attributes.WeaponDamage = 35;
            this.Attributes.WeaponSpeed = -25;
            this.WeaponAttributes.SelfRepair = 10;
            this.Velocity = 25;			
        }

        public Windsongplus(Serial serial)
            : base(serial)
        {
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

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
			
    }
}