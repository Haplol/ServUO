using System;

namespace Server.Items
{
    public class PolarBearMaskplus : BearMask
	{
		public override bool IsArtifact { get { return true; } }
		private int m_BodyInit;

        [CommandProperty( AccessLevel.Administrator )]
        public int BodyInit
		{ 
            get 
            { 
                return m_BodyInit;
            }
            set 
            { 
                m_BodyInit = value;
                InvalidateProperties();
            }
        }
        [Constructable]
        public PolarBearMaskplus()
        {
			      
			Name = "(Shiny) Polar Bear Mask";
            this.Hue = 0x481;
			
			this.Attributes.BonusHits = 20;
            this.Attributes.RegenHits = 10;
            this.Attributes.NightSight = 1;
			this.Attributes.BonusStr = 20;
			this.Attributes.BonusDex = 20;
        }
		 public override bool OnEquip( Mobile from )
	{
		if(BodyInit != 0xD5);{
            BodyInit = from.BodyMod;
		}
            from.BodyMod = 0xD5;

	    return base.OnEquip( from );
	}

        public override void OnRemoved( object parent )
        {
            base.OnRemoved( parent );

            if ( parent is Mobile && !Deleted)
            {
                Mobile m = (Mobile) parent;               
                
                m.BodyMod = BodyInit;
            }
        }        

        public PolarBearMaskplus(Serial serial)
            : base(serial)
        {
        }

      	
		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 10; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 10; } }
        public override int InitMinHits{ get{ return 255; } }
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
			writer.Write( (int) m_BodyInit );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);        
            int version = reader.ReadInt();
			m_BodyInit = reader.ReadInt();

        }
    }
}