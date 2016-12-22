using System;
using Server;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HuntersHeaddressplus : DeerMask
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public HuntersHeaddressplus()
        {
            this.Hue = 0x594;
			Name = "(Shiny) Hunter's Headdress";
            this.SkillBonuses.SetValues(0, SkillName.Archery, 40);
            this.Attributes.BonusDex = 40;
            this.Attributes.NightSight = 1;
            this.Attributes.AttackChance = 30;
        }

        public HuntersHeaddressplus(Serial serial)
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
        public override int BaseColdResistance
        {
            get
            {
                return 23;
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
            switch ( version )
            {
                case 0:
                    {
                        this.Resistances.Cold = 0;
                        break;
                    }
            }
        }
    }
}