using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class SwitchMasteryGump : Gump
    {
        public SwitchMasteryGump(Mobile from) : base( 50, 50 )
        {
            PlayerMobile pm = (PlayerMobile)from;

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;

			AddPage(0);
			AddBackground(5, 9, 409, 281, 9380);
            AddLabel(156, 60, 0, @"Switch Mastery");

            if (pm.BardMastery != BardMastery.ProvocationMastery)
            {
                AddButton(68, 99, 4005, 4006, 1, GumpButtonType.Reply, 0);
                AddLabel(121, 100, 493, @"Provocation Mastery");
            }
            else
            {
                AddLabel(121, 100, 437, @"Provocation Mastery");
            }

            if (pm.BardMastery != BardMastery.PeacemakingMastery)
            {
                AddButton(68, 142, 4005, 4006, 2, GumpButtonType.Reply, 0);
                AddLabel(121, 143, 493, @"Peacemaking Mastery");
            }
            else
            {
                AddLabel(121, 143, 437, @"Peacemaking Mastery");
            }

            if (pm.BardMastery != BardMastery.DiscordMastery)
            {
                AddButton(68, 181, 4005, 4006, 3, GumpButtonType.Reply, 0);
                AddLabel(121, 182, 493, @"Discord Mastery");
            }
            else
            {
                AddLabel(121, 182, 437, @"Discord Mastery");
            }
            
			AddLabel(291, 100, 0, @"Tier: 3");
			AddLabel(291, 143, 0, @"Tier: 3");
			AddLabel(291, 181, 0, @"Tier: 3");			
        }       

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (!(sender.Mobile is PlayerMobile))
                return;
           
            PlayerMobile pm = sender.Mobile as PlayerMobile;          

            switch(info.ButtonID)
            {
                case 0: // Closed
                    {
                        break;
                    }
                case 1: // Provocation
                    {
                        if (pm.Skills[SkillName.Provocation].Value >= 90.0)
                        {
                            pm.BardMasteryChangeTime = TimeSpan.FromMinutes(10.0);
                            pm.BardMastery = BardMastery.ProvocationMastery;
                            pm.SendLocalizedMessage(1151949, "Provocation Mastery"); // You have changed to ~1_val~
                        }
                        else
                        {
                            pm.SendMessage("You need at least 90 Provocation skill to use that mastery.");
                        }

                        break;
                    }
                case 2: // Peacemaking
                    {
                        if (pm.Skills[SkillName.Peacemaking].Value >= 90.0)
                        {
                            pm.BardMasteryChangeTime = TimeSpan.FromMinutes(10.0);
                            pm.BardMastery = BardMastery.PeacemakingMastery;
                            pm.SendLocalizedMessage(1151949, "Peacemaking Mastery"); // You have changed to ~1_val~
                        }
                        else
                        {
                            pm.SendMessage("You need at least 90 Peacemaking skill to use that mastery.");
                        }

                        break;
                    }
                case 3: // Discord
                    {
                        if (pm.Skills[SkillName.Discordance].Value >= 90.0)
                        {
                            pm.BardMasteryChangeTime = TimeSpan.FromMinutes(10.0);
                            pm.BardMastery = BardMastery.DiscordMastery;
                            pm.SendLocalizedMessage(1151949, "Discord Mastery"); // You have changed to ~1_val~
                        }
                        else
                        {
                            pm.SendMessage("You need at least 90 Discordance skill to use that mastery.");
                        }

                        break;
                    }
            }
        }
    }
}