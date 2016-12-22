using System;

namespace Server.Items
{
    public class ArcticDeathDealerplus : WarMace
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public ArcticDeathDealerplus()
        {
			Name = "(Shiny) Arctic Death Dealer";
            this.Hue = 0x480;
			this.CustomAttributes.HitColdStrike = 25;
            this.WeaponAttributes.HitHarm = 100;
            this.WeaponAttributes.HitLowerAttack = 40;
            this.Attributes.WeaponSpeed = 40;
            this.Attributes.WeaponDamage = 20;
            this.WeaponAttributes.ResistColdBonus = 50;
        }

        public ArcticDeathDealerplus(Serial serial)
            : base(serial)
        {
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
            cold = 100;
            phys = 0;

            pois = fire = nrgy = chaos = direct = 0;
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