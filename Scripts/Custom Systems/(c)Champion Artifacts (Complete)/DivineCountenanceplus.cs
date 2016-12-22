using System;

namespace Server.Items
{
    public class DivineCountenanceplus : HornedTribalMask
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public DivineCountenanceplus()
        {
            this.Hue = 0x482;
			Name = "(Shiny) Divine Countenance";
            this.Attributes.BonusInt = 40;
            this.Attributes.RegenMana = 20;
            this.Attributes.LowerManaCost = 20;
			this.Attributes.LowerRegCost = 20;
			this.Attributes.SpellDamage = 30;


        }

        public DivineCountenanceplus(Serial serial)
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
                return 15;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 25;
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
                        this.Resistances.Fire = 0;
                        this.Resistances.Cold = 0;
                        this.Resistances.Energy = 0;
                        break;
                    }
            }
        }
    }
}