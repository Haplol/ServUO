using System;

namespace Server.Items
{
    public class HeartOfTheLionplus : PlateChest
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public HeartOfTheLionplus()
        {
			Name = "(Shiny) Heart Of The Lion";
            this.Hue = 0x501;
            this.Attributes.Luck = 300;
            this.Attributes.DefendChance = 15;
            this.ArmorAttributes.LowerStatReq = 100;

        }

        public HeartOfTheLionplus(Serial serial)
            : base(serial)
        {
        }


		public override int BasePhysicalResistance
        {
            get
            {
                return -50;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return -50;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return -50;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return -50;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return -50;
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