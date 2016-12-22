using System;

namespace Server.Items
{
    public class ArmorOfFortuneplus : StuddedChest
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public ArmorOfFortuneplus()
        {
			Name = "(Shiny) Armor of Fortune";
            this.Hue = 0x501;
            this.Attributes.Luck = 500;
            this.Attributes.DefendChance = 30;
            this.Attributes.LowerRegCost = 100;
            this.ArmorAttributes.MageArmor = 1;
        }

        public ArmorOfFortuneplus(Serial serial)
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