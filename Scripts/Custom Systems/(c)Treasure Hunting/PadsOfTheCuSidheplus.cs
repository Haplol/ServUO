using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Menus.ItemLists;
using Server.Menus.Questions;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Targets;

namespace Server.Items
{
    public class PadsOfTheCuSidheplus : FurBoots
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public PadsOfTheCuSidheplus()
            : base(0x47E)
        {
		Name = "(Shiny) Footpads Of The CuSidhe";

        }

        public PadsOfTheCuSidheplus(Serial serial)
            : base(serial)
        {
        }
		public override bool OnEquip( Mobile from )
		{
			from.Send(SpeedControl.MountSpeed);
			return base.OnEquip( from );
		}

        public override void OnRemoved( object parent )
        {
			 base.OnRemoved( parent ); 
			 if ( parent is Mobile && !Deleted)
            {
                Mobile m = (Mobile) parent;               
                m.Send(SpeedControl.Disable);
               
            }
           
        }        

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
		
    }
}