using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public class UnAlterItem
    {
        public static Dictionary<Type, Type> TypeList = new Dictionary<Type, Type>();
        public static void Configure()
        {
            //****BlackSmithy****//
            // Shields
            TypeList.Add(typeof(SmallPlateShield), typeof(Buckler) );
            TypeList.Add(typeof(LargePlateShield), typeof(HeaterShield) );
            TypeList.Add(typeof(MediumPlateShield), typeof(MetalShield) );
            TypeList.Add(typeof(GargishKiteShield), typeof(MetalKiteShield) );
            TypeList.Add(typeof(GargishWoodenShield), typeof(WoodenShield) );
            TypeList.Add(typeof(GargishChaosShield), typeof(ChaosShield) );
            TypeList.Add(typeof(GargishOrderShield), typeof(OrderShield) );

            //Platemail
            TypeList.Add(typeof(FemaleGargishPlateChest), typeof(FemalePlateChest) );
            TypeList.Add(typeof(GargishPlateChest), typeof(PlateChest) );
            TypeList.Add(typeof(GargishPlateArms), typeof(PlateArms) );
            TypeList.Add(typeof(GargishPlateLegs), typeof(PlateLegs) );
            
            // Weapons
            TypeList.Add(typeof(GargishBattleAxe), typeof(BattleAxe) );
            TypeList.Add(typeof(GargishBoneHarvester), typeof(BoneHarvester) );
            TypeList.Add(typeof(GargishKatana), typeof(Katana) );
            TypeList.Add(typeof(GargishTekagi), typeof(Tekagi) );
            TypeList.Add(typeof(GargishLance), typeof(Lance) );
            TypeList.Add(typeof(GargishPike), typeof(Pike) );
            TypeList.Add(typeof(GargishBardiche), typeof(Bardiche) );
            TypeList.Add(typeof(GargishDaisho), typeof(Daisho) );
            TypeList.Add(typeof(GargishScythe), typeof(Scythe) );
            TypeList.Add(typeof(GargishWarFork), typeof(WarFork) );
            TypeList.Add(typeof(GargishKryss), typeof(Kryss) );
            TypeList.Add(typeof(GargishWarHammer), typeof(WarHammer) );
            TypeList.Add(typeof(GargishMaul), typeof(Maul) );
            TypeList.Add(typeof(GargishTessen), typeof(Tessen) );
            TypeList.Add(typeof(GargishAxe), typeof(Axe) );
            TypeList.Add(typeof(GargishDagger), typeof(Dagger) );
            TypeList.Add(typeof(DreadSword), typeof(Broadsword) );
            TypeList.Add(typeof(GargishTalwar), typeof(Halberd) );
            TypeList.Add(typeof(StoneWarSword), typeof(VikingSword) );
            TypeList.Add(typeof(DualPointedSpear), typeof(Spear) );
            TypeList.Add(typeof(Shortblade), typeof(WarCleaver) );
            TypeList.Add(typeof(BloodBlade), typeof(Leafblade) );
            TypeList.Add(typeof(DualShortAxes), typeof(TwoHandedAxe) );
            TypeList.Add(typeof(DiscMace), typeof(WarMace) );

            //****Carpentry****//
            TypeList.Add(typeof(GargishGnarledStaff), typeof(GnarledStaff) );
            #region Need Implementation
            //****Tailoring****//
            //Misc
            /*  TypeList.Add(typeof(BodySash), typeof(GargishSash));
                TypeList.Add(typeof(HalfApron), typeof(GargoyleHalfApron));
                TypeList.Add(typeof(Kilt), typeof(GargishClothKilt));

                //Footwear
                TypeList.Add(typeof(FurBoots), typeof(LeatherTalons));
                TypeList.Add(typeof(Boots), typeof(LeatherTalons));
                TypeList.Add(typeof(ThighBoots), typeof(LeatherTalons));
                TypeList.Add(typeof(Shoes), typeof(LeatherTalons));
                TypeList.Add(typeof(Sandals), typeof(LeatherTalons));
                TypeList.Add(typeof(NinjaTabi), typeof(LeatherTalons));
                TypeList.Add(typeof(SamuraiTabi), typeof(LeatherTalons));
                TypeList.Add(typeof(Waraji), typeof(LeatherTalons));
                TypeList.Add(typeof(ElvenBoots), typeof(LeatherTalons));

                //Quivers & Cloaks
                TypeList.Add(typeof(ElvenQuiver), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfBlight), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfFire), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfIce), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfLightning), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfElements), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfRage), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(QuiverOfInfinity), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(Cloak), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(RewardCloak), typeof(GargishClothWingArmor));
                TypeList.Add(typeof(FurCape), typeof(GargishClothWingArmor)); */
            #endregion
//custom
TypeList.Add(typeof(GargishEarrings), typeof(GoldEarrings) );
TypeList.Add(typeof(GargishRing), typeof(GoldRing) );
TypeList.Add(typeof(GargishBracelet), typeof(GoldBracelet) );
TypeList.Add(typeof(GargishNecklace), typeof(Necklace) );
            //Leather Armor
            TypeList.Add(typeof(FemaleGargishLeatherChest), typeof(FemaleLeatherChest) );
            TypeList.Add(typeof(GargishLeatherChest), typeof(LeatherChest) );
            TypeList.Add(typeof(GargishLeatherLegs), typeof(LeatherLegs) );
            TypeList.Add(typeof(GargishLeatherArms), typeof(LeatherArms) );

            //****Tinker****//
            TypeList.Add(typeof(GargishButcherKnife), typeof(ButcherKnife) );
            TypeList.Add(typeof(GargishCleaver), typeof(Cleaver) );
                      
        }

        public static void BeginTarget(Mobile from, CraftSystem system, BaseTool tool)
        {
            from.Target = new UnAlterItemTarget(system, tool);
            from.SendLocalizedMessage(1094730); //Target the item to altar
        }

        public static bool TryToUnAlter(Mobile from, Item olditem)
        {
            if (!TypeList.ContainsKey(olditem.GetType()))
                return false;

            Item newitem = CreateItem(TypeList[olditem.GetType()]);

            if (newitem == null)
                return false;

            if (olditem is BaseWeapon && newitem is BaseWeapon)
            {
                BaseWeapon oldweapon = (BaseWeapon)olditem;
                BaseWeapon newweapon = (BaseWeapon)newitem;

                newweapon.Attributes = new AosAttributes(oldweapon, newweapon.Attributes);
				newweapon.NegativeAttributes = new NegativeAttributes(oldweapon, newweapon.NegativeAttributes);
                //newweapon.ElementDamages = new AosElementAttributes( oldweapon, newweapon.ElementDamages ); To Do
                newweapon.SkillBonuses = new AosSkillBonuses(oldweapon, newweapon.SkillBonuses);
                newweapon.WeaponAttributes = new AosWeaponAttributes(oldweapon, newweapon.WeaponAttributes);
                newweapon.AbsorptionAttributes = new SAAbsorptionAttributes(oldweapon, newweapon.AbsorptionAttributes);
            }
            else if (olditem is BaseArmor && newitem is BaseArmor)
            {
                BaseArmor oldarmor = (BaseArmor)olditem;
                BaseArmor newarmor = (BaseArmor)newitem;

                newarmor.Attributes = new AosAttributes(oldarmor, newarmor.Attributes);
				newarmor.NegativeAttributes = new NegativeAttributes(oldarmor, newarmor.NegativeAttributes);
                newarmor.ArmorAttributes = new AosArmorAttributes(oldarmor, newarmor.ArmorAttributes);
                newarmor.SkillBonuses = new AosSkillBonuses(oldarmor, newarmor.SkillBonuses);
                newarmor.AbsorptionAttributes = new SAAbsorptionAttributes(oldarmor, newarmor.AbsorptionAttributes);
            }
            else if (olditem is BaseShield && newitem is BaseShield)
            {
                BaseShield oldshield = (BaseShield)olditem;
                BaseShield newshield = (BaseShield)newitem;

                newshield.Attributes = new AosAttributes(oldshield, newshield.Attributes);
                newshield.NegativeAttributes = new NegativeAttributes(oldshield, newshield.NegativeAttributes);
                newshield.SkillBonuses = new AosSkillBonuses(oldshield, newshield.SkillBonuses);
                newshield.AbsorptionAttributes = new SAAbsorptionAttributes(oldshield, newshield.AbsorptionAttributes);
            }
            else if (olditem is BaseJewel && newitem is BaseJewel)
			{
                BaseJewel oldjewel = (BaseJewel)olditem;
                BaseJewel newjewel = (BaseJewel)newitem;

                newjewel.Attributes = new AosAttributes(oldjewel, newjewel.Attributes);
				newjewel.NegativeAttributes = new NegativeAttributes(oldjewel, newjewel.NegativeAttributes);
                newjewel.SkillBonuses = new AosSkillBonuses(oldjewel, newjewel.SkillBonuses);
                newjewel.AbsorptionAttributes = new SAAbsorptionAttributes(oldjewel, newjewel.AbsorptionAttributes);
            }
			 else if (olditem is BaseArmor && newitem is BaseJewel)
			{
                BaseArmor oldjewel2 = (BaseArmor)olditem;
                BaseJewel newjewel2 = (BaseJewel)newitem;

                newjewel2.Attributes = new AosAttributes(oldjewel2, newjewel2.Attributes);
				newjewel2.NegativeAttributes = new NegativeAttributes(oldjewel2, newjewel2.NegativeAttributes);
                newjewel2.SkillBonuses = new AosSkillBonuses(oldjewel2, newjewel2.SkillBonuses);
                newjewel2.AbsorptionAttributes = new SAAbsorptionAttributes(oldjewel2, newjewel2.AbsorptionAttributes);
            }
			else
            {
                return false;
            }

            olditem.Delete();
			olditem.OnAfterDuped(newitem);
			newitem.Parent = null;

            if (from.Backpack == null)
                newitem.MoveToWorld(from.Location, from.Map);
            else
                from.Backpack.DropItem(newitem);
				
			newitem.InvalidateProperties();

            return true;
        }

        public static Item CreateItem(Type t)
        {
            try
            {
                return (Item)Activator.CreateInstance(t);
            }
            catch
            {
                return null;
            }
        }
    }

    public class UnAlterItemTarget : Target
    {
        private readonly CraftSystem m_System;
        private readonly BaseTool m_Tool;
        /// mod
        public UnAlterItemTarget(CraftSystem system, BaseTool tool)
            : base(1, false, TargetFlags.None)
        {
            this.m_System = system;
            this.m_Tool = tool;// mod
        }

        protected override void OnTarget(Mobile from, object o)
        {
            if (!(o is Item))
            {
                from.SendMessage("You cannot convert people into gargoyles using this.");
            }
            else if (o is BaseWeapon)
            {
                BaseWeapon bw = (BaseWeapon)o;

                this.CheckResource(from, bw, bw.Resource);
            }
            else if (o is BaseArmor)
            {
                BaseArmor ba = (BaseArmor)o;

                this.CheckResource(from, ba, ba.Resource);
            }
            else if (o is BaseShield)
            {
                BaseShield bs = (BaseShield)o;

                this.CheckResource(from, bs, bs.Resource);
            }
        }

        private void CheckResource(Mobile from, Item item, CraftResource res)
        {
            bool completed = false;

            if (this.m_System is DefTailoring)//
            {
                switch (res)
                {
                    case CraftResource.RegularLeather:
                    case CraftResource.SpinedLeather:
                    case CraftResource.HornedLeather:
                    case CraftResource.BarbedLeather:

                        //default:
                        completed = UnAlterItem.TryToUnAlter(from, item);
                        break;
                }
            }
            else if (this.m_System is DefBlacksmithy)
            {
                switch (res)
                {
                    // default: // Not listing ores, it's the only logical remainder.
                    case CraftResource.Iron:
                    case CraftResource.DullCopper:
                    case CraftResource.ShadowIron:
                    case CraftResource.Copper:
                    case CraftResource.Bronze:
                    case CraftResource.Gold:
                    case CraftResource.Agapite:
                    case CraftResource.Verite:
                    case CraftResource.Valorite:

                        //if (m_Tool is SmithHammer)
                        completed = UnAlterItem.TryToUnAlter(from, item);
                        break;
                }
            }
            else if (this.m_System is DefCarpentry)
            {
                switch (res)
                {
                    case CraftResource.RegularWood:
                    case CraftResource.OakWood:
                    case CraftResource.AshWood:
                    case CraftResource.YewWood:
                    case CraftResource.Heartwood:
                    case CraftResource.Bloodwood:
                    case CraftResource.Frostwood:

                        //if (m_Tool is Hammer)
                        completed = UnAlterItem.TryToUnAlter(from, item);
                        break;
                }
            }
            else
            {
                from.SendMessage("You cannot use this to unalter that");
            }

            if (completed)
                from.SendMessage("The item has been turned into a human item.");
            else
                from.SendMessage("You cannot use this to unalter that");
        }
    }
}

namespace Server.Commands
{
    public class UnAlterItemCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("UnAlterItem", AccessLevel.Player, new CommandEventHandler(UnAlterItem_OnCommand));
        }

        [Description("Converts a human/elf item into a a gargoyle item.")]
        public static void UnAlterItem_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(10, false, TargetFlags.None, new TargetCallback(UnAlterItem_CallBack));
        }

        public static void UnAlterItem_CallBack(Mobile from, object targeted)
        {
            if (targeted is Item)
            {
                if (UnAlterItem.TryToUnAlter(from, (Item)targeted))
                    from.SendMessage("The item has been turned into a gargish item.");
                else
                    from.SendMessage("That could not be altered.");
            }
            else
                from.SendMessage("That is not an item.");
        }
    }
}