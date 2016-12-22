using System;

namespace Server.Items
{
    public class TheTaskmasterplus : WarFork
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public TheTaskmasterplus()
        {
			Name = "(Shiny) The Taskmaster";
            this.Hue = 0x4F8;
            this.WeaponAttributes.HitPoisonArea = 100;
			this.CustomAttributes.HitPoisonStrike = 50;
            this.Attributes.BonusDex = 30;
            this.Attributes.AttackChance = 30;
            this.Attributes.WeaponDamage = 50;
        }

        public TheTaskmasterplus(Serial serial)
            : base(serial)
        {
        }

        public override int ArtifactRarity
        {
            get
            {
                return 10;
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
            phys = fire = cold = nrgy = chaos = direct = 0;
            pois = 100;
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