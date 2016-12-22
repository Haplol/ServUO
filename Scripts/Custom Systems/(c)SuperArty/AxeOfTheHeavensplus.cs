//Lord of Oaks
using System;
using Server.Spells;

namespace Server.Items


{
    public class AxeOfTheHeavensplus : DoubleAxe
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public AxeOfTheHeavensplus()
        {
			Name = "(Shiny) Axe of The Heavens";
            this.Hue = 0x4D5;
			this.CustomAttributes.HitChainLightning = 25;
			this.WeaponAttributes.HitLightning = 100;
            this.Attributes.AttackChance = 30;
            this.Attributes.WeaponDamage = 50;
			this.Attributes.WeaponSpeed = 20;
        }

        public AxeOfTheHeavensplus(Serial serial)
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