using System;

namespace Server.Items
{
    public class BoneCrusherplus : WarMace
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public BoneCrusherplus()
        {
 			Name = "(Shiny) Bone Crusher";
            this.Hue = 0x60C;
			this.Attributes.WeaponSpeed = -50;
            this.WeaponAttributes.HitLowerDefend = 100;
            this.Attributes.BonusStr = 30;
            this.Attributes.WeaponDamage = 30;
        }

        public BoneCrusherplus(Serial serial)
            : base(serial)
        {
        }

		public override int AosMinDamage
        {
            get
            {
                return 25;
            }
        }
		public override int AosMaxDamage
        {
            get
            {
                return 35;
            }
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

            if (this.Hue == 0x604)
                this.Hue = 0x60C;

        }
    }
}