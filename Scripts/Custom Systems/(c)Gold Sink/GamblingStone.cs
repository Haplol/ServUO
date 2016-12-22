using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using daat99;

namespace Server.Items
{
    public class GamblingStone : Item
    {
		 private static readonly Type[] m_GambleArtifact = new Type[]
        {
            typeof(ArcaneShieldplus),
            typeof(TheTaskmasterplus),
            typeof(ArmorOfFortuneplus),
            typeof(HolyKnightsBreastplateplus),
            typeof(JackalsCollarplus),
            typeof(LeggingsOfBaneplus),
            typeof(TunicOfFireplus),
            typeof(RingOfTheElementsplus),
            typeof(BladeOfInsanityplus),
            typeof(BoneCrusherplus),
            typeof(Frostbringerplus),
            typeof(SerpentsFangplus),
            typeof(StaffOfTheMagiplus),
            typeof(TheBerserkersMaulplus),
            typeof(TheDryadBowplus),
            typeof(DivineCountenanceplus),
            typeof(HuntersHeaddressplus),
            typeof(SpiritOfTheTotemplus),
			typeof(QuiverOfInfinityplus),
			typeof(Windsongplus),
			typeof( AlchemistsBaubleplus ), 
			typeof( GwennosHarpplus ), 
			typeof( ArcticDeathDealerplus ), 
			typeof( PeasantsBokutoplus ),
			typeof( EyesOfHateplus ), 
			typeof( PolarBearMaskplus )
			
        };
		public static bool TakePlayerGold(PlayerMobile player, int amount, bool informPlayer)
		{
			return MasterStorageUtils.TakeTypeFromPlayer(player, typeof(Gold), amount, informPlayer);
		}

        [Constructable]
        public GamblingStone()
            : base(0xED4)
        {
            this.Movable = false;
            this.Hue = 0x56;
        }

        public GamblingStone(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "a gambling stone";
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("500,000 Gold to spin");
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            base.LabelTo(from, "500,000 Gold to spin");
        }

        public override void OnDoubleClick(Mobile from)
        {
            Container pack = from.Backpack;
			Container bank = from.FindBankNoCreate();
					

            if ((pack != null && pack.ConsumeTotal(typeof(Gold), 500000)) || (bank != null && bank.ConsumeTotal(typeof(Gold), 500000 )) || (TakePlayerGold(from as PlayerMobile, 500000, true)))
            {
                this.InvalidateProperties();

                int roll = Utility.Random(1200);

                if (roll == 0) // Jackpot
                {                 

                from.SendMessage(0x35, "You win an Artifact!");
				Item i = null;

                try
                {
                    i = Activator.CreateInstance(m_GambleArtifact[Utility.Random(m_GambleArtifact.Length)]) as Item;
					from.PlaceInBackpack(i);
                }
                catch
                {
                }
				}
				 else if (roll <= 10) // Chance for a regbag
                {
                    from.SendMessage(0x35, "You win a One Handed Deed");
                    from.AddToBackpack(new OneHandedDeed());
                }
				
                
                else if (roll <= 20) // Chance for a regbag
                {
                    from.SendMessage(0x35, "You win a Relayer");
                    from.AddToBackpack(new RelayerDeed());
                }
                else // Loser!
                {
                    from.SendMessage(0x22, "You lose!");
                }
            }
            else
            {
                from.SendMessage(0x22, "You need at least 500,000gp in your backpack or bank to use this.");
            }
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
}