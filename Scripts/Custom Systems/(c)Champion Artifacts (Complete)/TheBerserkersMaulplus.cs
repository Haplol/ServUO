using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class TheBerserkersMaulplus : Maul
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public TheBerserkersMaulplus()
        {

			Name = "(Shiny) The Berserkers Maul";
            this.Hue = 0x21;
			this.EngravedText = "Breaks SSI Cap";
            this.Attributes.WeaponSpeed = -25;
            this.Attributes.WeaponDamage = 50;
        }

        public TheBerserkersMaulplus(Serial serial)
            : base(serial)
        {
        }

        public override int ArtifactRarity
        {
            get
            {
                return 11;
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