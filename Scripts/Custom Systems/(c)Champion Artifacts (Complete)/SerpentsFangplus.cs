using System;

namespace Server.Items
{
    public class SerpentsFangplus : Kryss
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public SerpentsFangplus()
        {
			Name = "(Shiny) Serpents Fang";
            this.Hue = 0x488;
            this.WeaponAttributes.HitPoisonArea = 100;
            this.WeaponAttributes.ResistPoisonBonus = 20;
			this.CustomAttributes.HitPoisonStrike = 50;
            this.Attributes.AttackChance = 15;
            this.Attributes.WeaponDamage = 50;
        }

        public SerpentsFangplus(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1061601;
            }
        }// Serpent's Fang
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
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            fire = cold = nrgy = chaos = direct = 0;
            phys = 25;
            pois = 75;
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

            if (this.ItemID == 0x1401)
                this.ItemID = 0x1400;
        }
    }
}