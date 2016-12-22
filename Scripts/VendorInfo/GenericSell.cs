using System;
using System.Collections.Generic;
using Server.Items;
using daat99;

namespace Server.Mobiles
{
	public class GenericSellInfo : IShopSellInfo
	{
		private Dictionary<Type, int> m_Table = new Dictionary<Type, int>();
		private Type[] m_Types;

		public GenericSellInfo()
		{
		}

		public void Add( Type type, int price )
		{
			m_Table[type] = price;
			m_Types = null;
		}

		public int GetSellPriceFor( Item item )
		{
			int price = 0;
			m_Table.TryGetValue( item.GetType(), out price );

			if ( item is BaseArmor ) {
				BaseArmor armor = (BaseArmor)item;

				if ( armor.Quality == ArmorQuality.Low )
					price = (int)( price * 0.60 );
				else if ( armor.Quality == ArmorQuality.Exceptional )
					price = (int)( price * 1.25 );

				price += 100 * (int)armor.Durability;

				price += 100 * (int)armor.ProtectionLevel;

				//daat99 OWLTR start - resources cost more
				if ( OWLTROptionsManager.IsEnabled(OWLTROptionsManager.OPTIONS_ENUM.RESOURCE_COST) )
				{
					double d_Mult = (int)armor.Resource;
					if ( d_Mult > 1 && d_Mult < 101 )
						d_Mult = 10+(d_Mult - 1);
					else if ( d_Mult > 101 && d_Mult < 201 )
						d_Mult = 10+(d_Mult - 101);
					else if ( d_Mult > 201 && d_Mult < 300 )
						d_Mult = 10+(d_Mult - 201);
					else
						d_Mult = 10;
					price = (int)((d_Mult*price)/10);
				}
				//daat99 OWLTR end - resources cost more

				if ( price < 1 )
					price = 1;
			}
			else if ( item is BaseWeapon ) {
				BaseWeapon weapon = (BaseWeapon)item;

				if ( weapon.Quality == WeaponQuality.Low )
					price = (int)( price * 0.60 );
				else if ( weapon.Quality == WeaponQuality.Exceptional )
					price = (int)( price * 1.25 );

				price += 100 * (int)weapon.DurabilityLevel;

				//daat99 OWLTR start - resources cost more
				if ( OWLTROptionsManager.IsEnabled(OWLTROptionsManager.OPTIONS_ENUM.RESOURCE_COST) )
				{
					double d_Mult = (int)weapon.Resource;
					if ( d_Mult > 1 && d_Mult < 101 )
						d_Mult = 10+(d_Mult - 1);
					else if ( d_Mult > 300 && d_Mult < 400 )
						d_Mult = 10+(d_Mult - 300);
					else
						d_Mult = 10;
					price = (int)((d_Mult*price)/10);
				}
				//daat99 OWLTR end - resources cost more

				if ( price < 1 )
					price = 1;
			}
			else if ( item is BaseBeverage ) {
				int price1 = price, price2 = price;

				if ( item is Pitcher )
				{ price1 = 3; price2 = 5; }
				else if ( item is BeverageBottle )
				{ price1 = 3; price2 = 3; }
				else if ( item is Jug )
				{ price1 = 6; price2 = 6; }

				BaseBeverage bev = (BaseBeverage)item;

				if ( bev.IsEmpty || bev.Content == BeverageType.Milk )
					price = price1;
				else
					price = price2;
			}

			return price;
		}

		public int GetBuyPriceFor( Item item )
		{
			return (int)( 1.90 * GetSellPriceFor( item ) );
		}

		public Type[] Types
		{
			get
			{
				if ( m_Types == null )
				{
					m_Types = new Type[m_Table.Keys.Count];
					m_Table.Keys.CopyTo( m_Types, 0 );
				}

				return m_Types;
			}
		}

		public string GetNameFor( Item item )
		{
			if ( item.Name != null )
				return item.Name;
			else
				return item.LabelNumber.ToString();
		}

		public bool IsSellable( Item item )
		{
			if ( item.Nontransferable )
				return false;

			//if ( item.Hue != 0 )
				//return false;

			return IsInList( item.GetType() );
		}
	 
		public bool IsResellable( Item item )
		{
			if ( item.Nontransferable )
				return false;

			//if ( item.Hue != 0 )
				//return false;

			return IsInList( item.GetType() );
		}

		public bool IsInList( Type type )
		{
			return m_Table.ContainsKey( type );
		}
	}
}
