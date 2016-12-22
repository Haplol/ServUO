using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
namespace Server.Items
{
    public class AutoResPotion : Item
    {
		private static Dictionary<Mobile, AutoResPotion> m_ResList;
		
        private int m_Charges;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        private Timer m_Timer;
        private static TimeSpan m_Delay = TimeSpan.FromSeconds( 5.0 ); /*TimeSpan.Zero*/

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay { get { return m_Delay; } set { m_Delay = value; } }
	
        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_Death);
        }
		
        [Constructable]
        public AutoResPotion() : this( 1 )
        { }

        [Constructable]
        public AutoResPotion(int charges) : base(0xF04) 
        {
            m_Charges = charges;

            Name = "Potion Of Rebirth";
            LootType = LootType.Blessed;
            
            /*Stackable = true;*/
            Weight = 1.0;
            /*Amount = amount;*/
        }

        public AutoResPotion(Serial serial)
            : base(serial)
        { }
		
		public override void OnDoubleClick( Mobile from )
		{
			if(!from.Alive)
				return;
			if(m_ResList == null)
				m_ResList = new Dictionary<Mobile, AutoResPotion>();
			
			if( !m_ResList.ContainsKey(from))
			{
				if(!m_ResList.ContainsValue(this))
				{
					m_ResList.Add(from, this);
					from.SendMessage("You feel the spirits watching you, awaiting to send you back to your body.");
				}
				else
					from.SendMessage("The spirits of this potion are watching another");
			}
			else
				from.SendMessage("The spirits watch you already.");
		}
		
		private static void EventSink_Death(PlayerDeathEventArgs e)
        {
            PlayerMobile owner = e.Mobile as PlayerMobile;

            if (owner != null && !owner.Deleted)
            {
                if (owner.Alive)
                    return;
				
				if(m_ResList != null && m_ResList.ContainsKey(owner))
				{
					AutoResPotion arp = m_ResList[owner];
					if(arp == null || arp.Deleted)
					{
						m_ResList.Remove(owner);
						return;
					}
					arp.m_Timer = Timer.DelayCall(m_Delay, new TimerStateCallback(Resurrect_OnTick), new object[] { owner, arp });
					m_ResList.Remove(owner);
				}
            }
        }

        private static void Resurrect_OnTick(object state)
        {
            object[] states = (object[])state;
            PlayerMobile owner = (PlayerMobile)states[0];
			AutoResPotion arp = (AutoResPotion)states[1];
            if (owner != null && !owner.Deleted && arp != null && !arp.Deleted)
            {
                if (owner.Alive || arp.m_Charges < 1)
                    return;

                owner.SendMessage("You died under the watch of the spirits, they have offered you another chance at life.");
                owner.Resurrect();

                arp.m_Charges--;

                arp.InvalidateProperties();

                if (arp.m_Charges < 1)
                    arp.Delete();
            }
        }

        public override void GetProperties( ObjectPropertyList list )
        {
            base.GetProperties( list );

            list.Add(String.Format("{0} Charges", m_Charges));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write( (int) 0 ); // version

            writer.Write( (TimeSpan) m_Delay );
            writer.Write( (int) m_Charges );            
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0: 
                {
                     m_Delay = reader.ReadTimeSpan();
                     m_Charges = reader.ReadInt();
                } break;
            }
        }
    }
}