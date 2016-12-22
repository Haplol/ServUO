using System;

namespace Server.Items
{
    public class GauntletsOfAngerplus : PlateGloves
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public GauntletsOfAngerplus()
        {
            this.Hue = 0x29b;
			Name = "(Shiny) Gauntlets Of Anger";
            this.Attributes.BonusHits = 10;
            this.Attributes.RegenHits = 4;
            this.Attributes.DefendChance = -10;
        }

        public GauntletsOfAngerplus(Serial serial)
            : base(serial)
        {
        }
        public override int BasePhysicalResistance
        {
            get
            {
                return -10;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return -10;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return -10;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return -10;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return -10;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 150;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 150;
            }
        }
        public override bool CanFortify
        {
            get
            {
                return false;
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