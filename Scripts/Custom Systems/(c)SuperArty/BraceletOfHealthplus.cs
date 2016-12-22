//Rikktor
using System;

namespace Server.Items
{
    public class BraceletOfHealthplus : GoldBracelet
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public BraceletOfHealthplus()
        {
			Name = "(Shiny) Bracelet of Health";
            this.Hue = 0x21;
            this.Attributes.BonusHits = 50;
            this.Attributes.RegenHits = 10;
            this.Attributes.Luck = 100;

        }

        public BraceletOfHealthplus(Serial serial)
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