//Semidar
using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
    public class AnimatedLegsoftheInsaneTinkerplus : PlateLegs
    {
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public AnimatedLegsoftheInsaneTinkerplus()
            : base()
        {
            this.Name = ("(Shiny) Animated Legs of the Insane Tinker (Breaks SSI)");
            this.Hue = 2310;
            this.Attributes.BonusDex = 10;
            this.Attributes.RegenStam = 5;
            this.Attributes.WeaponDamage = 20;
        }

        public AnimatedLegsoftheInsaneTinkerplus(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 15;
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
        public override int AosStrReq
        {
            get
            {
                return 200;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 200;
            }
        }
		
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); //version
        }
    }
}