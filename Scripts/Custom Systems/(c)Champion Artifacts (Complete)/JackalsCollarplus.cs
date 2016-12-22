using System;

namespace Server.Items
{
    public class JackalsCollarplus : PlateGorget
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public JackalsCollarplus()
        {
			Name = "(Shiny) Jackals Collar";
            this.Attributes.WeaponSpeed = 30;
            this.Hue = 0x6D1;
			this.Attributes.BonusHits = 10;
            this.Attributes.BonusDex = 40;
            this.Attributes.RegenHits = 10;
        }

        public JackalsCollarplus(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1061594;
            }
        }// Jackal's Collar
        public override int ArtifactRarity
        {
            get
            {
                return 11;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 23;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 17;
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

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
            {
                if (this.Hue == 0x54B)
                    this.Hue = 0x6D1;

                this.FireBonus = 0;
                this.ColdBonus = 0;
            }
        }
    }
}