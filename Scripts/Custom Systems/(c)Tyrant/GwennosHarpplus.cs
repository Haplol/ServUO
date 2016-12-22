using System;

namespace Server.Items
{
    public class GwennosHarpplus : LapHarp
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public GwennosHarpplus()
        {
			Name = "(Shiny) Gwenno's Harp";
            this.Hue = 0x47E;
            this.Slayer = SlayerName.Repond;
            this.Slayer2 = SlayerName.ReptilianDeath;
			this.ReplenishesCharges = true;
        }

        public GwennosHarpplus(Serial serial)
            : base(serial)
        {
        }
        public override int InitMinUses
        {
            get
            {
                return 1600;
            }
        }
		  public override TimeSpan ChargeReplenishRate
        {
            get
            {
                return TimeSpan.FromSeconds(5.0);
            }
        }
        public override int InitMaxUses
        {
            get
            {
                return 1600;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}