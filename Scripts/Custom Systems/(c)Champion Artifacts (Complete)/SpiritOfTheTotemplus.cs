using System;

namespace Server.Items
{
    public class SpiritOfTheTotemplus : BearMask
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public SpiritOfTheTotemplus()
        {
            this.Hue = 0x455;
			Name = "(Shiny) Spirit of The Totem";
            this.Attributes.BonusStr = 50;
            this.Attributes.ReflectPhysical = 15;
            this.Attributes.AttackChance = 30;
        }

        public SpiritOfTheTotemplus(Serial serial)
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
                return 40;
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

            switch ( version )
            {
                case 0:
                    {
                        this.Resistances.Physical = 0;
                        break;
                    }
            }
        }
    }
}