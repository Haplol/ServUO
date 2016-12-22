using System;

namespace Server.Items
{
    public class RingOfTheElementsplus : GoldRing
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public RingOfTheElementsplus()
        {
			Name = "(Shiny) Ring of The Elements";
            this.Hue = 0x4E9;
            this.Attributes.Luck = 300;
            this.Resistances.Fire = 30;
            this.Resistances.Cold = 30;
            this.Resistances.Poison = 30;
            this.Resistances.Energy = 30;
			this.AbsorptionAttributes.EaterEnergy = 10;
            this.AbsorptionAttributes.EaterPoison = 10;
            this.AbsorptionAttributes.EaterCold = 10;
            this.AbsorptionAttributes.EaterFire = 10;
        }

        public RingOfTheElementsplus(Serial serial)
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