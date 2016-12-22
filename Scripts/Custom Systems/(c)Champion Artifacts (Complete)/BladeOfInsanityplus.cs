using System;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BladeOfInsanityplus : Katana
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public BladeOfInsanityplus()
        {
			Name = "(Shiny) Blade of Insanity";
            this.Hue = 0x76D;
			this.WeaponAttributes.HitLeechMana = 50;
			this.WeaponAttributes.HitLeechStam = 300;
            this.WeaponAttributes.HitLeechHits = 50;
            this.Attributes.RegenStam = 5;
            this.Attributes.WeaponSpeed = 75;
            this.Attributes.WeaponDamage = 50;
			this.WeaponAttributes.HitHarm = 100;
			XmlAttach.AttachTo(this, 
                        new XmlCustomAttacks( 
                            new XmlCustomAttacks.SpecialAttacks [] 
                            { 
                            XmlCustomAttacks.SpecialAttacks.TripleSlash
                            }
                        )
                    );
        }

        public BladeOfInsanityplus(Serial serial)
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

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (this.Hue == 0x44F)
                this.Hue = 0x76D;
        }
    }
}