//Mephitis
using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    //Verison 1

    public class RingOfTheVileplus : GoldRing
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public RingOfTheVileplus()
        {
			Name = "(Shiny) Ring of The Vile (Poison Immunity)";
            this.Hue = 0x4F7;
            this.Attributes.BonusDex = 8;
            this.Attributes.RegenStam = 6;
            this.Attributes.AttackChance = 15;
            this.Resistances.Poison = 20;
        }

        public RingOfTheVileplus(Serial serial)
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

            if (this.Hue == 0x4F4)
                this.Hue = 0x4F7;
        }
        
        
    }
}