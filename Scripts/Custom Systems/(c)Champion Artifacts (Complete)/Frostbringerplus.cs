using System;

namespace Server.Items
{
    public class Frostbringerplus : Bow
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public Frostbringerplus()
        {
			Name = "(Shiny) Frostbringer";
            this.Hue = 0x4F2;
			this.Attributes.SpellChanneling = 1;
            this.WeaponAttributes.HitDispel = 100;
			this.Attributes.DefendChance = 15;
            this.Attributes.RegenStam = 10;
            this.Attributes.WeaponDamage = 50;
			this.CustomAttributes.HitHailstorm = 25;
        }

        public Frostbringerplus(Serial serial)
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
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = fire = pois = nrgy = chaos = direct = 0;
            cold = 100;
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