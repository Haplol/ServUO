using System;

namespace Server.Items
{
    public class QuiverOfInfinityplus : BaseQuiver, ITokunoDyable
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public QuiverOfInfinityplus()
            : base(0x2B02)
        {
            this.LootType = LootType.Blessed;
            this.Weight = 8.0;

			Name = "(Shiny) Quiver of Infinity";
            this.WeightReduction = 100;
            this.LowerAmmoCost = 100;

            this.Attributes.DefendChance = 10;
        }

        public QuiverOfInfinityplus(Serial serial)
            : base(serial)
        {
        }

        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(2); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if (version < 1 && this.DamageIncrease == 0)
                this.DamageIncrease = 10;

            if (version < 2 && this.Attributes.WeaponDamage == 10)
                this.Attributes.WeaponDamage = 0;
        }
    }
}