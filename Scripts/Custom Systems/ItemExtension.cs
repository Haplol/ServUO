using System;
using Server;
using System.Collections.Generic;
using System.Linq;

 
namespace Server.Items
{
    public static class ItemExtensions
    {
        public static bool IsEquipped(this Item item, Mobile m)
        {
            if (m != null)
            {
				  for( int i = 0; i < 25; ++i )
					{
						
					Item tocheck = m.FindItemOnLayer((Layer)i );
                if (tocheck == item)return true;
					}
            }
            return false;
        }
        public static bool HasEquipped(this Mobile m, Item item)
        {
            return item.IsEquipped(m);
        }
        public static bool IsEquipped(this Object obj, Mobile m, Item item)
        {
            return item.IsEquipped(m);
        }
        public static bool IsEquipped(this Type itemtype, Mobile m)
        {
            if (m != null && itemtype != null)
            {
				for( int i = 0; i < 25; ++i )
					{
                try
                {
                	
						
					Item tocheck = m.FindItemOnLayer((Layer)i );
                    if (tocheck.GetType() == itemtype)
					{
                        return true;
					}
					
					
                }catch(Exception)
                { /* If for example you looked for a 1 handed weapon but use a 2 handed weapon instead */ }
                finally
                {
                }
				}
            }
            return false;
        }
        public static bool HasEquipped(this Mobile m, Type itemtype)
        {
            return itemtype.IsEquipped(m);
        }
        public static bool IsEquipped(this Object obj, Mobile m, Type itemtype)
        {
            return itemtype.IsEquipped(m);
        }
    }
}