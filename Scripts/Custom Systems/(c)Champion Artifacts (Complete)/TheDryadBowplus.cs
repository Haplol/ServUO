using System;

namespace Server.Items
{
    public class TheDryadBowplus : Bow
	{
		public override bool IsArtifact { get { return true; } }
        private static readonly SkillName[] m_PossibleBonusSkills = new SkillName[]
        {
            SkillName.Archery,
            SkillName.Healing,
            SkillName.MagicResist,
            SkillName.Peacemaking,
            SkillName.Chivalry,
            SkillName.Ninjitsu,
			SkillName.Anatomy,
			SkillName.Tactics
        };
        [Constructable]
        public TheDryadBowplus()
        {
			Name = "(Shiny) The Dryad Bow";
            this.Hue = 0x48F;
            this.SkillBonuses.SetValues(0, m_PossibleBonusSkills[Utility.Random(m_PossibleBonusSkills.Length)], (Utility.Random(4) == 0 ? 10.0 : 5.0));
            this.WeaponAttributes.SelfRepair = 5;
            this.Attributes.WeaponSpeed = 50;
            this.Attributes.WeaponDamage = 35;
            this.WeaponAttributes.ResistPoisonBonus = 30;
			this.CustomAttributes.HitPoisonStrike = 50;
        }

        public TheDryadBowplus(Serial serial)
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

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
                this.SkillBonuses.SetValues(0, m_PossibleBonusSkills[Utility.Random(m_PossibleBonusSkills.Length)], (Utility.Random(4) == 0 ? 10.0 : 5.0));
        }
    }
}