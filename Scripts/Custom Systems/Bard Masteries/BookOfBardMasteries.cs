using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Prompts;
using Server.Network;

namespace Server.Items
{
    public class BookOfBardMasteries : Spellbook
    {		
		private Mobile m_Owner;
		
		[CommandProperty( AccessLevel.GameMaster )]
        public Mobile Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }			
		
        [Constructable]
        public BookOfBardMasteries()
            : this((ulong)0x3F)
        {
        }		

        [Constructable]
        public BookOfBardMasteries(ulong content)
            : base(content, 0x225a)
        {
            this.Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
        }

        public BookOfBardMasteries(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType { get { return SpellbookType.Bard; } }

        public override int BookOffset { get { return 700; } }

        public override int BookCount { get { return 6; } }

        public override void Serialize(GenericWriter writer)
        {
           base.Serialize( writer ); 
 
			writer.Write( (int) 0 ); // version 
 
			writer.Write( (Mobile) m_Owner ); 
        }
		
		 public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt(); 
 
			switch ( version ) 
			{ 
				case 0: 
				{ 
					m_Owner = reader.ReadMobile(); 
					break; 
				} 
			} 

            if (version == 0 && Core.ML)
                this.Layer = Layer.OneHanded;
        }
		
		public override bool DropToMobile(Mobile from, Mobile target, Point3D p)
        {
            bool ret = base.DropToMobile(from, target, p);
			
            if (ret)
			{
                m_Owner = target;				
				InvalidateProperties();
			}
			
            return ret;
        }
		
		public override bool DropToItem(Mobile from, Item target, Point3D p)
        {
            bool ret = base.DropToItem(from, target, p);

            if (ret)
			{
                m_Owner = from;				
				InvalidateProperties();
			}
			
            return ret;
        }
		
		public override bool OnEquip( Mobile from )
		{
			m_Owner = from;
			
			return base.OnEquip(from);
		}
		
		public override bool DropToWorld( Mobile from, Point3D p )
        {
            bool ret = base.DropToWorld( from, p );
               
            if ( ret )
                m_Owner = null;
               
            return ret;
        }		
		
		public override bool AllowSecureTrade( Mobile from, Mobile to, Mobile newOwner, bool accepted )
		{
			m_Owner = to;

			return base.AllowSecureTrade( from, to, newOwner, accepted );
		}		
		
		private class SwitchMasteryEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;

            public SwitchMasteryEntry(Mobile from) : base( 1151948 ) // Switch Mastery 
            {
                m_Mobile = from;
                PlayerMobile pm = (PlayerMobile)from;

                if (pm == null)
                    return;

                if (pm.BardMasteryChangeTime != TimeSpan.Zero)
                    Flags |= CMEFlags.Disabled;
            }

            public override void OnClick()
            {
                PlayerMobile pm = (PlayerMobile)m_Mobile;

                if (pm.BardMasteryChangeTime == TimeSpan.Zero)
                {
                    m_Mobile.CloseGump(typeof(SwitchMasteryGump));
                    m_Mobile.SendGump(new SwitchMasteryGump(m_Mobile));
                }
            }
        }
		
		private class PhysicalDamageEntry : ContextMenuEntry
        {           
            private Mobile m_Mobile;
			private BookOfBardMasteries m_Item;
 
            public PhysicalDamageEntry( Mobile from, BookOfBardMasteries item ) : base( 1151800 ) // Physical Damage
            {               
                m_Mobile = from;
				m_Item = item;
				
				PlayerMobile pm = m_Mobile as PlayerMobile;
				
				if (pm.BardDamageType == BardDamageType.PhysicalDamage )
                    Flags |= CMEFlags.Disabled;
            }
 
            public override void OnClick()
            {				
				if ( m_Mobile is PlayerMobile )
				{
					PlayerMobile pm = m_Mobile as PlayerMobile;
					
					if (pm.BardDamageType != BardDamageType.PhysicalDamage)					
					    pm.BardDamageType = BardDamageType.PhysicalDamage;
				}
            }
        }
		
		private class FireDamageEntry : ContextMenuEntry
        {           
            private Mobile m_Mobile;
			private BookOfBardMasteries m_Item;
 
            public FireDamageEntry( Mobile from, BookOfBardMasteries item ) : base( 1151801 ) // Fire Damage
            {               
                m_Mobile = from;
				m_Item = item;
				
				PlayerMobile pm = m_Mobile as PlayerMobile;
				
				if (pm.BardDamageType == BardDamageType.FireDamage )
                    Flags |= CMEFlags.Disabled;
            }
 
            public override void OnClick()
            {				
				if ( m_Mobile is PlayerMobile )
				{
					PlayerMobile pm = m_Mobile as PlayerMobile;
					
					if (pm.BardDamageType != BardDamageType.FireDamage)				
						pm.BardDamageType = BardDamageType.FireDamage;
				}
            }
        }
		
		private class ColdDamageEntry : ContextMenuEntry
        {           
            private Mobile m_Mobile;
			private BookOfBardMasteries m_Item;
 
            public ColdDamageEntry( Mobile from, BookOfBardMasteries item ) : base( 1151802 ) // Cold Damage
            {               
                m_Mobile = from;
				m_Item = item;
				
				PlayerMobile pm = m_Mobile as PlayerMobile;
				
				if (pm.BardDamageType == BardDamageType.ColdDamage )
                    Flags |= CMEFlags.Disabled;
            }
 
            public override void OnClick()
            {
				if ( m_Mobile is PlayerMobile )
				{
					PlayerMobile pm = m_Mobile as PlayerMobile;
					
					if (pm.BardDamageType != BardDamageType.ColdDamage)				
						pm.BardDamageType = BardDamageType.ColdDamage;
				}
            }
        }
		
		private class PoisonDamageEntry : ContextMenuEntry
        {           
            private Mobile m_Mobile;
			private BookOfBardMasteries m_Item;
 
            public PoisonDamageEntry( Mobile from, BookOfBardMasteries item ) : base( 1151803 ) // Poison Damage
            {               
                m_Mobile = from;
				m_Item = item;
				
				PlayerMobile pm = m_Mobile as PlayerMobile;
				
				if (pm.BardDamageType == BardDamageType.PoisonDamage )
                    Flags |= CMEFlags.Disabled;
            }
 
            public override void OnClick()
            {
				if ( m_Mobile is PlayerMobile )
				{
					PlayerMobile pm = m_Mobile as PlayerMobile;
					
					if (pm.BardDamageType != BardDamageType.PoisonDamage)				
						pm.BardDamageType = BardDamageType.PoisonDamage;
				}
            }
        }
		
		private class EnergyDamageEntry : ContextMenuEntry
        {           
            private Mobile m_Mobile;
			private BookOfBardMasteries m_Item;
 
            public EnergyDamageEntry( Mobile from, BookOfBardMasteries item ) : base( 1151804 ) // Energy Damage
            {               
                m_Mobile = from;
				m_Item = item;
				
				PlayerMobile pm = m_Mobile as PlayerMobile;
				
				if (pm.BardDamageType == BardDamageType.EnergyDamage )
                    Flags |= CMEFlags.Disabled;
            }
 
            public override void OnClick()
            {
				if ( m_Mobile is PlayerMobile )
				{
					PlayerMobile pm = m_Mobile as PlayerMobile;
					
					if (pm.BardDamageType != BardDamageType.EnergyDamage)					
						pm.BardDamageType = BardDamageType.EnergyDamage;
				}
            }
        }
 
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
			
			PlayerMobile pm = from as PlayerMobile;

            if (m_Owner == from && from.CheckAlive())
			{
                list.Add(new SwitchMasteryEntry(from));

                if ( pm.BardMastery == BardMastery.DiscordMastery )
				{
					list.Add( new PhysicalDamageEntry( from, this ) );	
					list.Add( new FireDamageEntry( from, this ) );							
					list.Add( new ColdDamageEntry( from, this ) );					
					list.Add( new PoisonDamageEntry( from, this ) );
					list.Add( new EnergyDamageEntry( from, this ) );
				}	
			}
        }
		
		public override void AddNameProperties( ObjectPropertyList list )
        {			
            base.AddNameProperties( list );		

			if ( m_Owner is PlayerMobile )
			{
				PlayerMobile pm = m_Owner as PlayerMobile;
				
				if (pm.CheckAlive() && this.RootParentEntity == m_Owner)
				{
					if ( pm.BardMastery == BardMastery.ProvocationMastery )
						list.Add( 1151946 ); // Provocation Mastery
					
					if ( pm.BardMastery == BardMastery.PeacemakingMastery )
						list.Add( 1151947 ); // Peacemaking Mastery
						
					if ( pm.BardMastery == BardMastery.DiscordMastery )
					{
						list.Add( 1151945 ); // Discord Mastery
						
						if ( pm.BardDamageType == BardDamageType.PhysicalDamage )
							list.Add( 1151800 ); // Physical Damage
						
						if ( pm.BardDamageType == BardDamageType.FireDamage )
							list.Add( 1151801 ); // Fire Damage
						
						if ( pm.BardDamageType == BardDamageType.ColdDamage )
							list.Add( 1151802 ); // Cold Damage
						
						if ( pm.BardDamageType == BardDamageType.PoisonDamage )
							list.Add( 1151803 ); // Poison Damage
						
						if ( pm.BardDamageType == BardDamageType.EnergyDamage )
							list.Add( 1151804 ); // Energy Damage
					}	
				}				
			}			
		}       
    }
}