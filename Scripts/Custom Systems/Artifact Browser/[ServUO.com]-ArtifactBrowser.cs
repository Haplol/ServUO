/**************************** ArtifactBrowser.cs **************************
 *  
 *  Creates every Artifact on the Server as an object in your pack,
 *      divided into convenient bags.
 *
 *  Also, using [ARTB or [ArtBrowser command, lets you view the details
 *      about each Artifact
 *
 *  Requires: Latest ServUO (April 2016) and OpenPropertyList.cs
 *  
/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using Server.Gumps;
using Server.Commands;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Custom
{
	public class ArtBro
    {
		private static List<Item> mArtifactList;
		public static List<Item> ArtifactList { get { return mArtifactList; } set { mArtifactList = value; } }
		private static int numLoaded;
		
		private static List<Item> mWeaponList;
		private static List<Item> mArmorList;
		private static List<Item> mClothingList;
		private static List<Item> mDecorationList;
		private static List<Item> mInstrumentList;
		private static List<Item> mGlassesList;
		private static List<Item> mSpellbookList;
		private static List<Item> mQuiverList;
		private static List<Item> mJewelList;
        private static List<Item> mTalismanList;
        private static List<Item> mMiscList;
		private static Dictionary<SetItem, List<Item>> mSetList;
		
		public static List<Item> WeaponList { get { return mWeaponList; } set { mWeaponList = value; } }
		public static List<Item> ArmorList { get { return mArmorList; } set { mArmorList = value; } }
		public static List<Item> ClothingList { get { return mClothingList; } set { mClothingList = value; } }
		public static List<Item> DecorationList { get { return mDecorationList; } set { mDecorationList = value; } }
		public static List<Item> InstrumentList { get { return mInstrumentList; } set { mInstrumentList = value; } }
		public static List<Item> GlassesList { get { return mGlassesList; } set { mGlassesList = value; } }
		public static List<Item> SpellbookList { get { return mSpellbookList; } set { mSpellbookList = value; } }
		public static List<Item> QuiverList { get { return mQuiverList; } set { mQuiverList = value; } }
		public static List<Item> JewelList { get { return mJewelList; } set { mJewelList = value; } }
        public static List<Item> TalismanList { get { return mTalismanList; } set { mTalismanList = value; } }
        public static List<Item> MiscList { get { return mMiscList; } set { mMiscList = value; } }
		public static Dictionary<SetItem, List<Item>> SetList { get { return mSetList; } set { mSetList = value; } }

        public static void Initialize()
        {
			CommandSystem.Register("ArtifactBrowser", AccessLevel.Owner, new CommandEventHandler(BrowseArtifacts_OnCommand));
			CommandSystem.Register("ArtB", AccessLevel.Owner, new CommandEventHandler(BrowseArtifacts_OnCommand));
			CommandSystem.Register("RelArt", AccessLevel.Owner, new CommandEventHandler(ReloadArtifacts_OnCommand));
			numLoaded = -1;
        }

        [Usage("ArtifactBrowser")]
        [Aliases("ArtB")]
        [Description("Shows the BrowseArtifacts Gump.")]
        public static void BrowseArtifacts_OnCommand(CommandEventArgs e)
        {
			if (e.Mobile is PlayerMobile)
				numLoaded = LoadArtifacts(e.Mobile as PlayerMobile);
			if (numLoaded >= 0)
				e.Mobile.SendGump(new BrowseArtifacts(0));
			else if (numLoaded == -1)
			{
				e.Mobile.SendMessage("Null mobile detected. (Where did you go?)");
			}
			else if (numLoaded == -2)
			{
				e.Mobile.SendMessage("You have no backpack?");
			}
			else
				e.Mobile.SendMessage("Unknown Error...");
        }

        [Usage("RelArt")]
        [Description("Reloads the Artifacts in the Owner's pack.")]
        public static void ReloadArtifacts_OnCommand(CommandEventArgs e)
        {
			if (e.Mobile is PlayerMobile)
				numLoaded = LoadArtifacts(e.Mobile as PlayerMobile);
			if (numLoaded > 0)
				e.Mobile.SendMessage("{0} Artifacts reloaded.", numLoaded);
			else
				e.Mobile.SendMessage("No Artifacts reloaded.");
        }
		
		public static SetItem GetSetItem(Item i)
		{
			if (i is BaseArmor && ((BaseArmor)i).IsSetItem) return ((BaseArmor)i).SetID;
			else if (i is BaseClothing &&((BaseClothing)i).IsSetItem) return ((BaseClothing)i).SetID;
			else if (i is BaseJewel &&((BaseJewel)i).IsSetItem) return ((BaseJewel)i).SetID;
			else if (i is BaseQuiver &&((BaseQuiver)i).IsSetItem) return ((BaseQuiver)i).SetID;
			else if (i is BaseWeapon &&((BaseWeapon)i).IsSetItem) return ((BaseWeapon)i).SetID;
			
			return SetItem.None;
		}
		
		// Items passed to this method are assumed to be Artifacts already...
		private static void AddToLists(Item i)
		{
			SetItem set = GetSetItem(i);
			if (set > SetItem.None) 
			{
				if (mSetList.ContainsKey(set))
				{
					List<Item> list = mSetList[set];
					if (list.Contains(i) == false)
					{
						list.Add(i);
					}
				}
				else
				{
					List<Item> list = new List<Item>();
					list.Add(i);
					mSetList.Add(set, list);
				}
			}
			if (i is BaseJewel) mJewelList.Add(i);
            else if (i is Glasses) mGlassesList.Add(i);
            else if (i is BaseArmor) mArmorList.Add(i);
			else if (i is BaseClothing) mClothingList.Add(i);
			else if (i is BaseWeapon) mWeaponList.Add(i);
			else if (i is BaseQuiver) mQuiverList.Add(i);
            else if (i is BaseTalisman) mTalismanList.Add(i);
            else if (i is Spellbook) mSpellbookList.Add(i);
			else if (i is BaseDecorationArtifact) mDecorationList.Add(i);
			else if (i is BaseInstrument) mInstrumentList.Add(i);
			else mMiscList.Add(i);
		}

        private static int LoadArtifacts(PlayerMobile m)
        {
			mArtifactList = new List<Item>();
			mWeaponList = new List<Item>();
			mArmorList = new List<Item>();
			mClothingList = new List<Item>();
			mDecorationList = new List<Item>();
			mInstrumentList = new List<Item>();
			mGlassesList = new List<Item>();
			mSpellbookList = new List<Item>();
			mQuiverList = new List<Item>();
			mJewelList = new List<Item>();
            mTalismanList = new List<Item>();
            mMiscList = new List<Item>();
			mSetList = new Dictionary<SetItem, List<Item>>();
			
			if (m != null)
			{
				Container pack = m.Backpack;
				
				if (pack == null) return -2;

				List<Type> typeList = new List<Type>();
				List<Item> bagList = new List<Item>();
				
				foreach (var assembly in ScriptCompiler.Assemblies)
				{
					foreach (var type in assembly.GetTypes())
					{
						// Skip types that don't have parameterless constructor
						var constructor = type.GetConstructor(Type.EmptyTypes);
						if (constructor == null) continue;
						
						try
						{
							// Skip types who did not override the "IsArtifact" property
							var property = type.GetProperty("IsArtifact").GetGetMethod(false);
							var def = property.GetBaseDefinition();
							if (def == property) continue;
							if (def.DeclaringType == property.DeclaringType) continue;
							
							if (type.IsSubclassOf(typeof(Item)) && !typeList.Contains(type))
							{
								Item item = (Item)Activator.CreateInstance(type);
								if (item != null)
								{
									if (item.IsArtifact)
									{
										typeList.Add(type);
										mArtifactList.Add(item);
										AddToLists(item);
										Item found = pack.FindItemByType(type);
										if (found == null) bagList.Add(item);
									}
									else
									{
										item.Delete();
									}
								}
							}
						}
						catch {  }
					}  				// END foreach (var type
				} 				// END foreach (var assembly
				
				if (bagList.Count > 0)
				{
					ArtifactBag wbag = new ArtifactBag();
					wbag.Name = "Bag of Weapon Artifacts";
					ArtifactBag axebag = new ArtifactBag();
					axebag.Name = "Bag of Axe Artifacts";
					ArtifactBag bashingbag = new ArtifactBag();
					bashingbag.Name = "Bag of Bashing Artifacts";
					ArtifactBag knifebag = new ArtifactBag();
					knifebag.Name = "Bag of Knife Artifacts";
					ArtifactBag polebag = new ArtifactBag();
					polebag.Name = "Bag of PoleArm Artifacts";
					ArtifactBag rangedbag = new ArtifactBag();
					rangedbag.Name = "Bag of Ranged Artifacts";
					ArtifactBag spearbag = new ArtifactBag();
					spearbag.Name = "Bag of Spear Artifacts";
					ArtifactBag staffbag = new ArtifactBag();
					staffbag.Name = "Bag of Staff Artifacts";
					ArtifactBag swordbag = new ArtifactBag();
					swordbag.Name = "Bag of Sword Artifacts";
					ArtifactBag thrownbag = new ArtifactBag();
					thrownbag.Name = "Bag of Thrown Artifacts";
					ArtifactBag wandbag = new ArtifactBag();
					wandbag.Name = "Bag of Wand Artifacts";
					ArtifactBag abag = new ArtifactBag();
					abag.Name = "Bag of Armor Artifacts";
					ArtifactBag shieldbag = new ArtifactBag();
					shieldbag.Name = "Bag of Shield Armor Artifacts";
					ArtifactBag headbag = new ArtifactBag();
					headbag.Name = "Bag of Head Armor Artifacts";
					ArtifactBag legbag = new ArtifactBag();
					legbag.Name = "Bag of Leg Armor Artifacts";
					ArtifactBag chestbag = new ArtifactBag();
					chestbag.Name = "Bag of Chest Armor Artifacts";
					ArtifactBag armbag = new ArtifactBag();
					armbag.Name = "Bag of Arm Armor Artifacts";
					ArtifactBag glovebag = new ArtifactBag();
					glovebag.Name = "Bag of Glove Armor Artifacts";
					ArtifactBag gorgetbag = new ArtifactBag();
					gorgetbag.Name = "Bag of Gorget Armor Artifacts";
					ArtifactBag asetbag = new ArtifactBag();
					asetbag.Name = "Bag of Artifact Armor Sets";
					ArtifactBag acolytebag = new ArtifactBag();
					acolytebag.Name = "Bag of Acolyte Artifacts";
					ArtifactBag assassinbag = new ArtifactBag();
					assassinbag.Name = "Bag of Assassin Artifacts";
					ArtifactBag darkwoodbag = new ArtifactBag();
					darkwoodbag.Name = "Bag of Darkwood Artifacts";
					ArtifactBag grizzlebag = new ArtifactBag();
					grizzlebag.Name = "Bag of Grizzle Artifacts";
					ArtifactBag hunterbag = new ArtifactBag();
					hunterbag.Name = "Bag of Hunter Artifacts";
					ArtifactBag juggernautbag = new ArtifactBag();
					juggernautbag.Name = "Bag of Juggernaut Artifacts";
					ArtifactBag magebag = new ArtifactBag();
					magebag.Name = "Bag of Mage Artifacts";
					ArtifactBag marksmanbag = new ArtifactBag();
					marksmanbag.Name = "Bag of Marksman Artifacts";
					ArtifactBag myrmidonbag = new ArtifactBag();
					myrmidonbag.Name = "Bag of Myrmidon Artifacts";
					ArtifactBag necromancerbag = new ArtifactBag();
					necromancerbag.Name = "Bag of Necromancer Artifacts";
					ArtifactBag paladinbag = new ArtifactBag();
					paladinbag.Name = "Bag of Paladin Artifacts";
					ArtifactBag virtuebag = new ArtifactBag();
					virtuebag.Name = "Bag of Virtue Artifacts";
					ArtifactBag luckbag = new ArtifactBag();
					luckbag.Name = "Bag of Luck Artifacts";
					ArtifactBag knightsbag = new ArtifactBag();
					knightsbag.Name = "Bag of Knights Artifacts";
					ArtifactBag scoutbag = new ArtifactBag();
					scoutbag.Name = "Bag of Scout Artifacts";
					ArtifactBag sorcererbag = new ArtifactBag();
					sorcererbag.Name = "Bag of Sorcerer Artifacts";
					ArtifactBag initiationbag = new ArtifactBag();
					initiationbag.Name = "Bag of Initiation Artifacts";
					ArtifactBag jbag = new ArtifactBag();
					jbag.Name = "Bag of Jewel Artifacts";
                    ArtifactBag tbag = new ArtifactBag();
                    tbag.Name = "Bag of Talisman Artifacts";
                    ArtifactBag dbag = new ArtifactBag();
					dbag.Name = "Bag of Decoration Artifacts";
					ArtifactBag cbag = new ArtifactBag();
					cbag.Name = "Bag of Clothing Artifacts";
					ArtifactBag ibag = new ArtifactBag();
					ibag.Name = "Bag of Instrument Artifacts";
					ArtifactBag qbag = new ArtifactBag();
					qbag.Name = "Bag of Quiver Artifacts";
					ArtifactBag sbag = new ArtifactBag();
					sbag.Name = "Bag of Spellbook Artifacts";
					ArtifactBag gbag = new ArtifactBag();
					gbag.Name = "Bag of Glasses Artifacts";
					ArtifactBag miscbag = new ArtifactBag();
					miscbag.Name = "Bag of Miscellaneous Artifacts";
					SetItem set = SetItem.None;
					// Go through the list and drop them in appropriate bags
					foreach (Item i in bagList)
					{
						set = GetSetItem(i);
						if (set > SetItem.None)
						{
							if (set == SetItem.Acolyte) acolytebag.DropItem(i);
							else if (set == SetItem.Assassin) assassinbag.DropItem(i);
							else if (set == SetItem.Darkwood) darkwoodbag.DropItem(i);
							else if (set == SetItem.Grizzle) grizzlebag.DropItem(i);
							else if (set == SetItem.Hunter) hunterbag.DropItem(i);
							else if (set == SetItem.Juggernaut) juggernautbag.DropItem(i);
							else if (set == SetItem.Mage) magebag.DropItem(i);
							else if (set == SetItem.Marksman) marksmanbag.DropItem(i);
							else if (set == SetItem.Myrmidon) myrmidonbag.DropItem(i);
							else if (set == SetItem.Necromancer) necromancerbag.DropItem(i);
							else if (set == SetItem.Paladin) paladinbag.DropItem(i);
							else if (set == SetItem.Virtue) virtuebag.DropItem(i);
							else if (set == SetItem.Luck) luckbag.DropItem(i);
							else if (set == SetItem.Knights) knightsbag.DropItem(i);
							else if (set == SetItem.Scout) scoutbag.DropItem(i);
							else if (set == SetItem.Sorcerer) sorcererbag.DropItem(i);
							else if (set == SetItem.Initiation) initiationbag.DropItem(i);
						}
						else if (i is BaseArmor)
                        {
						    if (i is Glasses) gbag.DropItem(i);
                            else switch ( ((BaseArmor)i).BodyPosition )
							{
								case ArmorBodyType.Gorget: gorgetbag.DropItem(i); break;
								case ArmorBodyType.Shield: shieldbag.DropItem(i); break;
								case ArmorBodyType.Gloves: glovebag.DropItem(i); break;
								case ArmorBodyType.Helmet: headbag.DropItem(i); break;
								case ArmorBodyType.Arms: armbag.DropItem(i); break;
								case ArmorBodyType.Legs: legbag.DropItem(i); break;
								case ArmorBodyType.Chest: chestbag.DropItem(i); break;
								default: abag.DropItem(i); break;
							}
						}
						else if (i is BaseClothing) cbag.DropItem(i);
						else if (i is BaseJewel) jbag.DropItem(i);
						else if (i is BaseWeapon) 
						{
							if (i is BaseAxe) axebag.DropItem(i);
							else if (i is BaseBashing) bashingbag.DropItem(i);
							else if (i is BaseKnife) knifebag.DropItem(i);
							else if (i is BasePoleArm) polebag.DropItem(i);
							else if (i is BaseRanged) rangedbag.DropItem(i);
							else if (i is BaseSpear) spearbag.DropItem(i);
							else if (i is BaseStaff) staffbag.DropItem(i);
							else if (i is BaseSword) swordbag.DropItem(i);
							else if (i is BaseThrown) thrownbag.DropItem(i);
							else if (i is BaseWand) wandbag.DropItem(i);
							else wbag.DropItem(i);
						}
						else if (i is BaseDecorationArtifact) dbag.DropItem(i);
						else if (i is BaseInstrument) ibag.DropItem(i);
						else if (i is BaseQuiver) qbag.DropItem(i);
                        else if (i is BaseTalisman) tbag.DropItem(i);
                        else if (i is Spellbook) sbag.DropItem(i);
						else miscbag.DropItem(i);
					}
					
					// For each bag, if there are items in it, drop them to the user, otherwise delete the bag
					if (cbag.Items.Count > 0) m.AddToBackpack(cbag); else cbag.Delete();
					if (jbag.Items.Count > 0) m.AddToBackpack(jbag); else jbag.Delete();
					if (gorgetbag.Items.Count > 0) PlaceItemIn(abag, 30, 35, gorgetbag); else gorgetbag.Delete();
					if (shieldbag.Items.Count > 0) PlaceItemIn(abag, 60, 35, shieldbag); else shieldbag.Delete();
					if (glovebag.Items.Count > 0) PlaceItemIn(abag, 90, 35, glovebag); else glovebag.Delete();
					if (headbag.Items.Count > 0) PlaceItemIn(abag, 30, 68, headbag); else headbag.Delete();
					if (armbag.Items.Count > 0) PlaceItemIn(abag, 45, 68, armbag); else armbag.Delete();
					if (legbag.Items.Count > 0) PlaceItemIn(abag, 75, 68, legbag); else legbag.Delete();
					if (chestbag.Items.Count > 0) PlaceItemIn(abag, 90, 68, chestbag); else chestbag.Delete();
					if (abag.Items.Count > 0) m.AddToBackpack(abag); else abag.Delete();
					if (acolytebag.Items.Count > 0) PlaceItemIn(asetbag, 30, 35, acolytebag); else acolytebag.Delete();
					if (assassinbag.Items.Count > 0) PlaceItemIn(asetbag, 60, 35, assassinbag); else assassinbag.Delete();
					if (darkwoodbag.Items.Count > 0) PlaceItemIn(asetbag, 90, 35, darkwoodbag); else darkwoodbag.Delete();
					if (grizzlebag.Items.Count > 0) PlaceItemIn(asetbag, 30, 65, grizzlebag); else grizzlebag.Delete();
					if (hunterbag.Items.Count > 0) PlaceItemIn(asetbag, 45, 65, hunterbag); else hunterbag.Delete();
					if (juggernautbag.Items.Count > 0) PlaceItemIn(asetbag, 75, 65, juggernautbag); else juggernautbag.Delete();
					if (magebag.Items.Count > 0) PlaceItemIn(asetbag, 90, 65, magebag); else magebag.Delete();
					if (marksmanbag.Items.Count > 0) PlaceItemIn(asetbag, 30, 50, marksmanbag); else marksmanbag.Delete();
					if (myrmidonbag.Items.Count > 0) PlaceItemIn(asetbag, 60, 50, myrmidonbag); else myrmidonbag.Delete();
					if (necromancerbag.Items.Count > 0) PlaceItemIn(asetbag, 90, 50, necromancerbag); else necromancerbag.Delete();
					if (paladinbag.Items.Count > 0) PlaceItemIn(asetbag, 45, 50, paladinbag); else paladinbag.Delete();
					if (virtuebag.Items.Count > 0) PlaceItemIn(asetbag, 30, 80, virtuebag); else virtuebag.Delete();
					if (luckbag.Items.Count > 0) PlaceItemIn(asetbag, 45, 80, luckbag); else luckbag.Delete();
					if (knightsbag.Items.Count > 0) PlaceItemIn(asetbag, 60, 80, knightsbag); else knightsbag.Delete();
					if (scoutbag.Items.Count > 0) PlaceItemIn(asetbag, 75, 80, scoutbag); else scoutbag.Delete();
					if (sorcererbag.Items.Count > 0) PlaceItemIn(asetbag, 30, 95, sorcererbag); else sorcererbag.Delete();
					if (initiationbag.Items.Count > 0) PlaceItemIn(asetbag, 45, 95, initiationbag); else initiationbag.Delete();
					if (asetbag.Items.Count > 0) m.AddToBackpack(asetbag); else asetbag.Delete();
					if (axebag.Items.Count > 0) PlaceItemIn(wbag, 30, 35, axebag); else axebag.Delete();
					if (bashingbag.Items.Count > 0) PlaceItemIn(wbag, 45, 35, bashingbag); else bashingbag.Delete();
					if (knifebag.Items.Count > 0) PlaceItemIn(wbag, 60, 35, knifebag); else knifebag.Delete();
					if (polebag.Items.Count > 0) PlaceItemIn(wbag, 75, 35, polebag); else polebag.Delete();
					if (rangedbag.Items.Count > 0) PlaceItemIn(wbag, 90, 35, rangedbag); else rangedbag.Delete();
					if (spearbag.Items.Count > 0) PlaceItemIn(wbag, 30, 50, spearbag); else spearbag.Delete();
					if (staffbag.Items.Count > 0) PlaceItemIn(wbag, 45, 50, staffbag); else staffbag.Delete();
					if (swordbag.Items.Count > 0) PlaceItemIn(wbag, 60, 50, swordbag); else swordbag.Delete();
					if (thrownbag.Items.Count > 0) PlaceItemIn(wbag, 75, 50, thrownbag); else thrownbag.Delete();
					if (wandbag.Items.Count > 0) PlaceItemIn(wbag, 90, 50, wandbag); else wandbag.Delete();
					if (wbag.Items.Count > 0) m.AddToBackpack(wbag); else wbag.Delete();
					if (dbag.Items.Count > 0) m.AddToBackpack(dbag); else dbag.Delete();
					if (ibag.Items.Count > 0) m.AddToBackpack(ibag); else ibag.Delete();
					if (qbag.Items.Count > 0) m.AddToBackpack(qbag); else qbag.Delete();
                    if (tbag.Items.Count > 0) m.AddToBackpack(tbag); else tbag.Delete();
                    if (sbag.Items.Count > 0) m.AddToBackpack(sbag); else sbag.Delete();
					if (gbag.Items.Count > 0) m.AddToBackpack(gbag); else gbag.Delete();
					if (miscbag.Items.Count > 0) m.AddToBackpack(miscbag); else miscbag.Delete();
				}
				
				// Delete the items that were created but not put in new bags...
				foreach (Item dupItem in mArtifactList) { if (!bagList.Contains(dupItem)) dupItem.Delete(); }

				return (bagList.Count);
			}

            return -1;
        }

        private static void PlaceItemIn(Container parent, int x, int y, Item item)
        {
            parent.AddItem(item);
            item.Location = new Point3D(x, y, 0);
        }
	}

    public class ArtifactBag : Bag
    {
        [Constructable]
        public ArtifactBag()
        {
        }

        public ArtifactBag(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
	
    public class BrowseArtifacts : CG
	{
		private int m_Image;
		private bool m_Page;
		private Pages m_PageID;
		private bool m_Sets;
		private SetItem m_SetItem;

        public BrowseArtifacts(int image) : this(image, false, Pages.All, false, SetItem.None)
		{
		}

        public BrowseArtifacts(int image, bool page) : this(image, page, Pages.All, false, SetItem.None)
		{
		}

        public BrowseArtifacts(int image, bool page, Pages pageID, bool sets, SetItem setItem) : 
            base(30, 30)
		{
			m_Image = image;
			m_Page = page;
			m_PageID = pageID;
			m_Sets = sets;
			m_SetItem = setItem;
			
            GumpButtonType Reply = GumpButtonType.Reply;
			
            AddBackground(0, 0, 449, 28, 9300); // Title Background
            AddBackground(0, 29, 449, 471, 9300); // Main Background
            AddBackground(450, 0, 150, 500, 9300); // Menu Background
			
			AddLabel(140, 5, 0, "Artifact Browser (by Lokai)");
			
			List<Item> currentList = GetList(page, pageID, sets, setItem);
			if (currentList != null)
			{
				int count = currentList.Count;
				if (count > 0)
				{
					Item item = currentList[m_Image];
					AddLabel(121, 36, 0, String.Format(
						"Displaying {0} of {1} : {2} Artifacts", m_Image + 1, count, 
							page ? m_PageID.ToString() : m_SetItem.ToString()));
					AddHtmlLocalized(121, 60, 200, 20, item.LabelNumber, 0x24E5, false, false );
					AddLabel(121, 80, 0, String.Format("Name: {0}", item.Name));
					AddItem(21, 80, item.ItemID, item.Hue);
					AddLabel(121, 100, 0, String.Format("Weight: {0}", item.Weight));
					SetItem set = ArtBro.GetSetItem(item);
					if (set > SetItem.None)
						AddLabel(121, 120, 0, String.Format("Part of the {0} Set", set));
					AddItemProperty(item.Serial);
					
					if (item is BaseClothing) DisplayClothing(item as BaseClothing);
					else if (item is BaseJewel) DisplayJewel(item as BaseJewel);
                    else if (item is Glasses) DisplayGlasses(item as Glasses);
                    else if (item is BaseArmor) DisplayArmor(item as BaseArmor);
					else if (item is BaseWeapon) DisplayWeapon(item as BaseWeapon);
					else if (item is BaseDecorationArtifact) DisplayDecoration(item as BaseDecorationArtifact);
					else if (item is BaseInstrument) DisplayInstrument(item as BaseInstrument);
					else if (item is BaseQuiver) DisplayQuiver(item as BaseQuiver);
					else if (item is Spellbook) DisplaySpellbook(item as Spellbook);
                    else if (item is BaseTalisman) DisplayTalisman(item as BaseTalisman);
					else DisplayMisc(item);
			
					if (m_Image < count - 1)
					{
						AddButton(388, 35, 0x15E1, 0x15E5, 2, Reply, 0); // Next Item
					}
					if (m_Image > 0)
					{
						AddButton(31, 35, 0x15E3, 0x15E7, 3, Reply, 0); // Previous Item
					}
				}
				else
				{
					AddLabel(121, 120, 0, "There are no Artifacts in that List.");
				}
			}
			else
			{
                AddLabel(121, 120, 0, "There is no List for that type.");
			}
			
			AddLabel(465, 5, 0, "Navigation Menu");
			AddLabel(465, 19, 0, "---------------");
			bool yes;
			
			yes = (m_PageID == Pages.All);
			AddButton(470, 45, yes?2511:2510, 2510, (int)Pages.All, Reply, 0); // All
			AddLabel(495, 45, 0, "Show All.");
			
			if (ArtBro.SetList.Count > 0) 
			{
				AddButton(470, 70, m_Sets?2511:2510, 2510, 700, Reply, 0); // Artifact Sets
				AddLabel(495, 70, 0, "Artifact Sets");
			}
			
			if (m_Sets)
			{
				AddBackground(601, 0, 150, 500, 9300); // Submenu Background
				AddLabel(615, 5, 0, "Sets Submenu");
				AddLabel(615, 19, 0, "-------------");
				int x = 701;
				for (int i = 0;i < 17;i++)
				{
					SetItem set = (SetItem)(i + 1);
					// Set Selection List
					if (ArtBro.SetList.ContainsKey(set))
					{
						yes = (m_SetItem == set);
						AddButton(620, 45 + (i*20), yes?2511:2510, 2510, (int)(x + i), Reply, 0);
						AddLabel(645, 45 + (i*20), 0, string.Format("{0}", set));
					}
				}
			}
			
			if (ArtBro.WeaponList.Count > 0) 
			{
				yes = (m_PageID >= Pages.Weapon && m_PageID < Pages.Armor);
				AddButton(470, 115, yes?2511:2510, 2510, (int)Pages.Weapon, Reply, 0); // WeaponList
				AddLabel(495, 115, 0, "Weapons");
				
				if (yes)
				{
					AddBackground(601, 0, 150, 500, 9300); // Submenu Background
					AddLabel(615, 5, 0, "Weapons Submenu");
					AddLabel(615, 19, 0, "-----------------");
					
					yes = (m_PageID == Pages.Weapon);
					AddButton(620, 45, yes?2511:2510, 2510, 200, Reply, 0);
					AddLabel(645, 45, 0, "All");
						
					for (int i = 0; i < 10; i++)
					{
						Pages wPage = (Pages)(i + 201);
						yes = (m_PageID == wPage);
						AddButton(620, 65 + (i*20), yes?2511:2510, 2510, 201+i, Reply, 0);
						AddLabel(645, 65 + (i*20), 0, string.Format("{0}", wPage));
					}
				}
			}
			if (ArtBro.ArmorList.Count > 0) 
			{
				yes = (m_PageID >= Pages.Armor && m_PageID < Pages.Clothing);
				AddButton(470, 135, yes?2511:2510, 2510, (int)Pages.Armor, Reply, 0); // ArmorList
				AddLabel(495, 135, 0, "Armor");
				
				if (yes)
				{
					AddBackground(601, 0, 150, 500, 9300); // Submenu Background
					AddLabel(615, 5, 0, "Armor Submenu");
					AddLabel(615, 19, 0, "--------------");
					
					yes = (m_PageID == Pages.Armor);
					AddButton(620, 45, yes?2511:2510, 2510, 300, Reply, 0);
					AddLabel(645, 45, 0, "All");
						
					for (int i = 0; i < 7; i++)
					{
						Pages aPage = (Pages)(i + 301);
						yes = (m_PageID == aPage);
						AddButton(620, 65 + (i*20), yes?2511:2510, 2510, 301+i, Reply, 0);
						AddLabel(645, 65 + (i*20), 0, string.Format("{0}", aPage));
					}
				}
			}
			if (ArtBro.ClothingList.Count > 0) 
			{
				yes = (m_PageID == Pages.Clothing);
				AddButton(470, 155, yes?2511:2510, 2510, (int)Pages.Clothing, Reply, 0); // ClothingList
				AddLabel(495, 155, 0, "Clothing");
			}
			if (ArtBro.DecorationList.Count > 0) 
			{
				yes = (m_PageID == Pages.Decoration);
				AddButton(470, 175, yes?2511:2510, 2510, (int)Pages.Decoration, Reply, 0); // DecorationList
				AddLabel(495, 175, 0, "Decorations");
			}
			if (ArtBro.InstrumentList.Count > 0) 
			{
				yes = (m_PageID == Pages.Instrument);
				AddButton(470, 195, yes?2511:2510, 2510, (int)Pages.Instrument, Reply, 0); // InstrumentList
				AddLabel(495, 195, 0, "Instruments");
			}
			if (ArtBro.GlassesList.Count > 0) 
			{
				yes = (m_PageID == Pages.Glasses);
				AddButton(470, 215, yes?2511:2510, 2510, (int)Pages.Glasses, Reply, 0); // GlassesList
				AddLabel(495, 215, 0, "Glasses");
			}
			if (ArtBro.SpellbookList.Count > 0) 
			{
				yes = (m_PageID == Pages.Spellbook);
				AddButton(470, 235, yes?2511:2510, 2510, (int)Pages.Spellbook, Reply, 0); // SpellbookList
				AddLabel(495, 235, 0, "Spellbooks");
			}
			if (ArtBro.QuiverList.Count > 0) 
			{
				yes = (m_PageID == Pages.Quiver);
				AddButton(470, 255, yes?2511:2510, 2510, (int)Pages.Quiver, Reply, 0); // QuiverList
				AddLabel(495, 255, 0, "Quivers");
			}
			if (ArtBro.JewelList.Count > 0) 
			{
				yes = (m_PageID == Pages.Jewel);
				AddButton(470, 275, yes?2511:2510, 2510, (int)Pages.Jewel, Reply, 0); // JewelList
				AddLabel(495, 275, 0, "Jewels");
            }
            if (ArtBro.TalismanList.Count > 0)
            {
                yes = (m_PageID == Pages.Talisman);
                AddButton(470, 295, yes ? 2511 : 2510, 2510, (int)Pages.Talisman, Reply, 0); // TalismanList
                AddLabel(495, 295, 0, "Talismans");
            }
            if (ArtBro.MiscList.Count > 0) 
			{
				yes = (m_PageID == Pages.Misc);
				AddButton(470, 315, yes?2511:2510, 2510, (int)Pages.Misc, Reply, 0); // MiscList
				AddLabel(495, 315, 0, "Miscellaneous");
			}
		}
		
		private static ArmorBodyType[] armorTypes = { ArmorBodyType.Gorget, ArmorBodyType.Shield, 
			ArmorBodyType.Gloves, ArmorBodyType.Helmet, ArmorBodyType.Arms, ArmorBodyType.Legs, 
			ArmorBodyType.Chest };
			
		private bool IsOfType(Item i, Pages page)
		{
			switch(page)
			{
				case Pages.Axe: return (i is BaseAxe);
				case Pages.Bashing: return (i is BaseBashing);
				case Pages.Knife: return (i is BaseKnife);
				case Pages.PoleArm: return (i is BasePoleArm);
				case Pages.Ranged: return (i is BaseRanged);
				case Pages.Spear: return (i is BaseSpear);
				case Pages.Staff: return (i is BaseStaff);
				case Pages.Sword: return (i is BaseSword);
				case Pages.Thrown: return (i is BaseThrown);
				case Pages.Wand: return (i is BaseWand);
			}
			return false;
		}
		
		private List<Item> GetList(bool page, Pages pageID, bool sets, SetItem setItem)
		{
			if (sets) return ArtBro.SetList[setItem];
			else if (page) 
			{
				if (pageID > Pages.Weapon && pageID < Pages.Armor)
				{
					List<Item> weapList = new List<Item>();
					foreach(Item i in ArtBro.WeaponList) { if (IsOfType(i, pageID)) { weapList.Add(i); } }
					return weapList;
				}
				else if (pageID > Pages.Armor && pageID < Pages.Clothing)
				{
					List<Item> armorList = new List<Item>();
					foreach(Item i in ArtBro.ArmorList)
					{
						if (i is BaseArmor && ((BaseArmor)i).BodyPosition == armorTypes[(int)pageID - 301])
						{
							armorList.Add(i);
						}
					}
					return armorList;
				}
				switch (pageID)
				{
					case Pages.Weapon: return ArtBro.WeaponList;
					case Pages.Armor: return ArtBro.ArmorList;
					case Pages.Clothing: return ArtBro.ClothingList;
					case Pages.Decoration: return ArtBro.DecorationList;
					case Pages.Instrument: return ArtBro.InstrumentList;
					case Pages.Glasses: return ArtBro.GlassesList;
					case Pages.Spellbook: return ArtBro.SpellbookList;
					case Pages.Quiver: return ArtBro.QuiverList;
                    case Pages.Talisman: return ArtBro.TalismanList;
                    case Pages.Jewel: return ArtBro.JewelList;
					case Pages.Misc: return ArtBro.MiscList;
					default:
					case Pages.All: return ArtBro.ArtifactList;
				}
			}
			else return ArtBro.ArtifactList;
		}
		
		private void DisplayDecoration(BaseDecorationArtifact deco)
		{
			AddLabel(121, 140, 0, "Decoration Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(deco);
            DisplayProperties(opl.PropList);
        }

        private void DisplayProperties(List<OpenProperty> propList)
        {
            int y = 200;
            if (propList.Count > 0)
            {
                int height = propList.Count > 15 ? (propList.Count * 20) + 30 : 330;
                AddBackground(0, 170, 449, height, 9300); // Extension Background
            }
            AddHtml(101, 180, 200, 20, "Properties:", 9400, false, false);
            foreach (OpenProperty prop in propList)
            {
                AddHtmlLocalized(121, y, 320, 20, prop.Number, prop.Arguments, 8200, false, false);
                y += 20;
            }
        }
		
		private void DisplayClothing(BaseClothing clothing)
        {
            AddLabel(121, 140, 0, "Clothing Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(clothing);
            DisplayProperties(opl.PropList);
		}
		
		private void DisplayJewel(BaseJewel jewel)
		{
            AddLabel(121, 140, 0, "Jewel Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(jewel);
            DisplayProperties(opl.PropList);
		}
		
		private void DisplayArmor(BaseArmor armor)
        {
            AddLabel(121, 140, 0, "Armor Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(armor);
            DisplayProperties(opl.PropList);
		}
		
		private void DisplayWeapon(BaseWeapon weapon)
		{
			AddLabel(121, 140, 0, "Weapon Artifact");
			AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(weapon);
            DisplayProperties(opl.PropList);
		}
		
		private void DisplayInstrument(BaseInstrument instrument)
		{
			AddLabel(121, 140, 0, "Instrument Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(instrument);
            DisplayProperties(opl.PropList);
        }
		
		private void DisplayQuiver(BaseQuiver quiver)
		{
			AddLabel(121, 140, 0, "Quiver Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(quiver);
            DisplayProperties(opl.PropList);
        }

        private void DisplayTalisman(BaseTalisman talisman)
        {
            AddLabel(121, 140, 0, "Talisman Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(talisman);
            DisplayProperties(opl.PropList);
        }

        private void DisplaySpellbook(Spellbook spellbook)
		{
			AddLabel(121, 140, 0, "Spellbook Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(spellbook);
            DisplayProperties(opl.PropList);
        }
		
		private void DisplayGlasses(Glasses glasses)
		{
			AddLabel(121, 140, 0, "Glasses Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(glasses);
            DisplayProperties(opl.PropList);
        }
		
		private void DisplayMisc(Item miscitem)
		{
			AddLabel(121, 140, 0, "Miscellaneous Artifact");
            AllPropertyList apl = new AllPropertyList();
            OpenPropertyList opl = apl.GetOpenProperties(miscitem);
            DisplayProperties(opl.PropList);
        }
		
        public override void OnResponse(NetState sender, RelayInfo info)
        {
			sender.Mobile.CloseGump(typeof(BrowseArtifacts));
			if (info.ButtonID == 2) // Forward 
			{
				m_Image += 1;
				sender.Mobile.SendGump(new BrowseArtifacts(m_Image, m_Page, m_PageID, m_Sets, m_SetItem));
			}
			else if (info.ButtonID == 3) // Back 
			{
				m_Image -= 1;
				sender.Mobile.SendGump(new BrowseArtifacts(m_Image, m_Page, m_PageID, m_Sets, m_SetItem));
			}
			else  if (info.ButtonID >= (int)SetPages.Acolyte) // Set Pages > 700
			{
				sender.Mobile.SendGump(
					new BrowseArtifacts(0, false, Pages.Sets, true, (SetItem)(info.ButtonID - 700)));
			}
			else if (info.ButtonID == 700) // Switch to Sets
			{
				sender.Mobile.SendGump(
					new BrowseArtifacts(0, false, (Pages)info.ButtonID, true, SetItem.Acolyte));
			}
			else  if (info.ButtonID >= (int)Pages.All) // Pages >= 100
			{
				sender.Mobile.SendGump(
					new BrowseArtifacts(0, true, (Pages)info.ButtonID, false, SetItem.None));
			}
		}
	}
	
	public enum SetPages
	{
		Acolyte = 701, Assassin, Darkwood, Grizzle, Hunter, Juggernaut, Mage, Marksman, Myrmidon, 
			Necromancer, Paladin, Virtue, Luck, Knights, Scout, Sorcerer, Initiation 
	}
		
	public enum Pages 
	{
		All = 100,
		Weapon = 200, Axe, Bashing, Knife, PoleArm, Ranged, Spear, Staff, Sword, Thrown, Wand,
		Armor = 300, Gorget, Shield, Gloves, Helmet, Arms, Legs, Chest, 
		Clothing = 400, Decoration, Instrument, Glasses, Spellbook, Quiver, Jewel, Talisman, Misc, 
		Sets = 500
	}
}
