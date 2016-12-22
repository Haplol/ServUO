using System;
using Reward = Server.Engines.Quests.BaseReward;

namespace Server.Items
{
    public class BaseCraftsmanSatchel : Backpack
    {
        public BaseCraftsmanSatchel()
            : base()
        {
            this.Hue = Reward.SatchelHue();
			
            int count = 2;
			
            if (0.055 > Utility.RandomDouble())
                count = 4;
			
            bool jewlery = false;
            bool talisman = false;
			
            while (this.Items.Count < count)
            { 
                if (0.50 > Utility.RandomDouble() && !talisman)
                {
                    this.DropItem(Loot.RandomTalisman());
                    talisman = true;					
                }
                else if (0.88 > Utility.RandomDouble() && !jewlery)
                {
                    this.DropItem(Reward.Jewlery());
                    jewlery = true;
                }
            }
        }

        public BaseCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public virtual Item RandomItem()
        {
            return null;
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

    public class FletcherCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public FletcherCraftsmanSatchel()
            : base()
        { 
			
            if (this.Items.Count < 4 && 0.25 > Utility.RandomDouble())
                this.DropItem(Reward.FletcherRecipe());
			if (0.2 > Utility.RandomDouble())
                this.DropItem(Reward.BagOfBoards());
			if (0.1 > Utility.RandomDouble())
                this.DropItem(Reward.BagOfTokens());
            if (0.05 > Utility.RandomDouble())
                this.DropItem(Reward.FletcherRunic());
        }

        public FletcherCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.RangedWeapon();
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

    public class TailorsCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public TailorsCraftsmanSatchel()
            : base()
        { 
            if (this.Items.Count < 2 && 0.5 > Utility.RandomDouble())
                this.DropItem(Reward.TailorRecipe());
        }

        public TailorsCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Armor();
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

    public class SmithsCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public SmithsCraftsmanSatchel()
            : base()
        { 
            if (this.Items.Count < 2 && 0.5 > Utility.RandomDouble())
                this.DropItem(Reward.SmithRecipe());
        }

        public SmithsCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
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

    public class TinkersCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public TinkersCraftsmanSatchel()
            : base()
        { 
            if (this.Items.Count < 2 && 0.5 > Utility.RandomDouble())
                this.DropItem(Reward.TinkerRecipe());
        }

        public TinkersCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
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

    public class CarpentersCraftsmanSatchel : BaseCraftsmanSatchel
    {
        [Constructable]
        public CarpentersCraftsmanSatchel()
            : base()
        { 
            if (this.Items.Count < 2 && 0.5 > Utility.RandomDouble())
                this.DropItem(Reward.CarpRecipe());				
			
            if (0.01 > Utility.RandomDouble())
                this.DropItem(Reward.CarpRunic());
        }

        public CarpentersCraftsmanSatchel(Serial serial)
            : base(serial)
        {
        }

        public override Item RandomItem()
        {
            return Reward.Weapon();
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