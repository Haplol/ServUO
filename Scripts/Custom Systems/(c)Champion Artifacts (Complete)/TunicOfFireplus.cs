using System;
using Server;

namespace Server.Items
{
    public class TunicOfFireplus : ChainChest
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public TunicOfFireplus() : base()
        {
			Name = "(Shiny) Tunic Of Fire";
            this.Hue = 0x54F;
			this.Attributes.RegenMana = 10;
            this.ArmorAttributes.SelfRepair = 10;
            this.Attributes.NightSight = 1;
            this.Attributes.ReflectPhysical = 50;		
        }

        public TunicOfFireplus(Serial serial) 
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
                return 24;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 54;
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
                if (this.Hue == 0x54E)
                    this.Hue = 0x54F;

                if (this.Attributes.NightSight == 0)
                    this.Attributes.NightSight = 1;

                this.PhysicalBonus = 0;
                this.FireBonus = 0;
            }
        }
    }
}