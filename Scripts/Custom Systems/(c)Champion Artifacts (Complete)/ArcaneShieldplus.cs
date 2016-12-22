using System;

namespace Server.Items
{
    public class ArcaneShieldplus : WoodenKiteShield
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public ArcaneShieldplus()
        {
			Name = "(Shiny) Arcane Shield";
            this.Hue = 0x556;
            this.Attributes.NightSight = 1;
            this.Attributes.SpellChanneling = 1;
            this.Attributes.DefendChance = 15;
            this.Attributes.CastSpeed = 2;
			this.Attributes.CastRecovery = 3;
			this.Attributes.SpellDamage = 20;
            this.Attributes.LowerManaCost = 40;
        }

        public ArcaneShieldplus(Serial serial)
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

            if (this.Attributes.NightSight == 0)
                this.Attributes.NightSight = 1;
        }
    }
}