using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class SongWovenMantleplus : LeafArms
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public SongWovenMantleplus()
        {
			Name = "(Shiny) Song Woven Mantle (Speeds Up Provocation)";
            this.Hue = 0x493;
            this.SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
            this.Attributes.Luck = 400;
            this.Attributes.DefendChance = 20;
        }

        public SongWovenMantleplus(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 14;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 14;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 16;
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