using System;

namespace Server.Items
{
    public class HolyKnightsBreastplateplus : PlateChest
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public HolyKnightsBreastplateplus()
        {
			Name = "(Shiny) Holy Knight's Breastplate";
            this.Hue = 0x47E;
			this.Attributes.DefendChance = 30;
            this.Attributes.BonusHits = 10;
            this.Attributes.ReflectPhysical = 15;
        }

        public HolyKnightsBreastplateplus(Serial serial)
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
        public override int BasePhysicalResistance
        {
            get
            {
                return 35;
            }
        }
		public override int BasePoisonResistance
        {
            get
            {
                return 35;
            }
        }
		public override int BaseFireResistance
        {
            get
            {
                return 35;
            }
        }
		public override int BaseColdResistance
        {
            get
            {
                return 35;
            }
        }
		public override int BaseEnergyResistance
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
                this.PhysicalBonus = 0;
        }
    }
}