using System;

namespace Server.Items
{
    public class GauntletsOfNobilityplus : RingmailGloves
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public GauntletsOfNobilityplus()
        {
			Name = "(Shiny) Gauntlets of Nobility";
			this.Hue = 0x4FE;
            this.Attributes.Luck = 1000;
            this.Attributes.WeaponDamage = -25;
			this.Attributes.WeaponSpeed = -25;
			this.Attributes.SpellDamage = -25;
        }

        public GauntletsOfNobilityplus(Serial serial)
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
                return 18;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 20;
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
                if (this.Hue == 0x562)
                    this.Hue = 0x4FE;

                this.PhysicalBonus = 0;
                this.PoisonBonus = 0;
            }
        }
    }
}