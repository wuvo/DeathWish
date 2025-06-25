using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_6671 : NPCBase
    {
        public NPC_6671(Main.GameClient _client)
            : base(_client)
        {
            ID = 6671;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Howdy! I'm pretty sure you already know this but you can get awesome rewards if you vote for our server. By voting you're helping us to increase the community and get rewards for it at the same time!");
                        AddOption("Check my vote points", 1);
                        AddOption("Exchange for rewards", 2);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Currently you currently have " + GC.MyChar.VotePoints + " Vote Points. Make sure you keep voting every 12 hours. This way you will win more Vote Points and our server will get more noticeable!");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Alright! I'm always happy to help out people on their quest! What would you like to receive in exchange for your Vote Points?");
                        AddOption("1 Hour Double Experience (1)", 3);
                        AddOption("GetPromed Items..", 15);
                        AddOption("1 Day VIP 5 (5)", 7);
                        AddOption("3 Days VIP 5 (12)", 9);
                        AddOption("1 Hour 25% Drop(7)", 16);
                        AddOption("1 Hour 25% Exp (7)", 17);
                        AddOption("Random Celestial/Elegance Garment (6)", 11);
                        AddOption("Random Garment - Except Elegances/Celestials (10)", 13);
                        //AddOption("Nevermind", 255);
                        break;
                    }

                case 16:
                    {
                        if (GC.MyChar.VotePoints >= 7)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                GC.MyChar.VotePoints -= 7;
                                World.DREvent = DateTime.Now.AddMinutes(60);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has made the final donation and activated global 25% higher drop rates for 1 hour! Enjoy!", 2011, 0);
                            }
                            else
                            {
                                GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                            }
                        }
                        else
                        {
                            AddText("You don't have 5 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }

                case 17:
                    {
                        if (GC.MyChar.VotePoints >= 7)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                GC.MyChar.VotePoints -= 7;
                                World.EREvent = DateTime.Now.AddMinutes(60);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has made the final donation and activated global 25% EXP Rate for 1 hour! Enjoy!", 2011, 0);
                            }
                            else
                            {
                                GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                            }
                        }
                        else
                        {
                            AddText("You don't have 7 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }


                case 15:
                    {
                        AddText("With the points you collected, you can get promed items from here..");
                        AddOption("Emerald (1)", 4);
                        AddOption("MoonBox (4)", 8);
                        AddOption("CleanWater (5)", 10);
                        AddOption("Nevermind", 255);
                        break;
                    }

                case 3:
                    {
                        if (GC.MyChar.VotePoints >= 1)
                        {
                            GC.MyChar.VotePoints -= 1;
                            GC.MyChar.ExpPotionUsed = DateTime.Now;
                            GC.MyChar.DoubleExp = true;
                            GC.MyChar.DoubleExpLeft = 3600;
                            GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));
                            AddText("Congratulations! You have received 1 Hour of Double Experience!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have 1 Vote Point.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.VotePoints >= 1)
                        {
                            if (GC.MyChar.Inventory.Count <= 39) // Emerald = 1080001
                            {
                                GC.MyChar.AddItem(1080001);
                                GC.MyChar.VotePoints -= 1;
                                AddText("Congratulations! You have received an Emerald!");
                                AddOption("Thanks", 255);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 1 Vote Point.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.VotePoints >= 2)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                GC.MyChar.AddItem(1088000);
                                GC.MyChar.VotePoints -= 2;
                                AddText("Congratulations! You have received a Dragonball!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                            }
                        }
                        else
                        {
                            AddText("You don't have 2 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.VotePoints >= 3)
                        {
                            if (GC.MyChar.Inventory.Count <= 37)
                            {
                                GC.MyChar.AddItem(721537);
                                GC.MyChar.AddItem(721538);
                                GC.MyChar.AddItem(721539);
                                GC.MyChar.VotePoints -= 3;
                                AddText("Congratulations! You have received a SkyToken, an EarthToken and a SoulToken!");
                                AddOption("Thanks", 255);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 3 free slots in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 3 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 7:
                    {
                        if (GC.MyChar.VipLevel != 3)
                        {
                            if (GC.MyChar.VotePoints >= 5)
                            {
                                if (GC.MyChar.VipLevel == 1)
                                    GC.MyChar.VIPDays /= 5;
                                else if (GC.MyChar.VipLevel == 2)
                                    GC.MyChar.VIPDays /= 4;
                                else if (GC.MyChar.VipLevel == 3)
                                    GC.MyChar.VIPDays /= 3;
                                else if (GC.MyChar.VipLevel == 4)
                                    GC.MyChar.VIPDays /= 2;
                                GC.MyChar.VotePoints -= 5;
                                if (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24) || GC.MyChar.VIPDays == 0)
                                    GC.MyChar.VIPStarted = DateTime.Now;
                                if (GC.MyChar.VipLevel != 6)
                                {
                                    GC.MyChar.VipLevel = 5;
                                }
                                GC.MyChar.VIPDays += 1;
                                AddText("Congratulations! You are now VIP for 1 Day!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("You don't have 5 Vote Points.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("Sorry mining characters cant buy vip.");
                            AddOption("I see", 255);
                        }

                        break;

                    }
                case 8:
                    {
                        if (GC.MyChar.VotePoints >= 4)
                        {
                            if (GC.MyChar.Inventory.Count <= 39) // Moonbox = 721080
                            {
                                GC.MyChar.AddItem(721080);
                                GC.MyChar.VotePoints -= 4;
                                AddText("Congratulations! You have received a MoonBox!");
                                AddOption("Thanks", 255);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 4 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 9:
                    {
                        if (GC.MyChar.VipLevel != 3)
                        {
                            if (GC.MyChar.VotePoints >= 12)
                            {
                                if (GC.MyChar.VipLevel == 1)
                                    GC.MyChar.VIPDays /= 5;
                                else if (GC.MyChar.VipLevel == 2)
                                    GC.MyChar.VIPDays /= 4;
                                else if (GC.MyChar.VipLevel == 3)
                                    GC.MyChar.VIPDays /= 3;
                                else if (GC.MyChar.VipLevel == 4)
                                    GC.MyChar.VIPDays /= 2;
                                GC.MyChar.VotePoints -= 12;
                                if (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24) || GC.MyChar.VIPDays == 0)
                                    GC.MyChar.VIPStarted = DateTime.Now;
                                if (GC.MyChar.VipLevel != 6)
                                {
                                    GC.MyChar.VipLevel = 5;
                                }
                                GC.MyChar.VIPDays += 3;
                                AddText("Congratulations! You are now VIP for 3 Days!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("You don't have 12 Vote Points.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("Sorry mining characters cant buy vip.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        if (GC.MyChar.VotePoints >= 5)
                        {
                            if (GC.MyChar.Inventory.Count <= 39) // CleanWater = 721258
                            {
                                GC.MyChar.AddItem(721258);
                                GC.MyChar.VotePoints -= 5;
                                AddText("Congratulations! You have received a CleanWater!");
                                AddOption("Thanks", 255);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 5 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 11:
                    {
                        if (GC.MyChar.VotePoints >= 6)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                GC.MyChar.VotePoints -= 6;
                                int _id = Program.Rnd.Next(0, 14);
                                uint toadd = 181925;
                                switch (_id)
                                {
                                    case 0:
                                        toadd = 181315;// WhiteElegance
                                        break;
                                    case 1:
                                        toadd = 181325;// WhiteCelestial
                                        break;
                                    case 2:
                                        toadd = 181415;// BrownElegance
                                        break;
                                    case 3:
                                        toadd = 181425;// BrownCelestial
                                        break;
                                    case 4:
                                        toadd = 181515;// BlackElegance
                                        break;
                                    case 5:
                                        toadd = 181525;// BlackCelestial
                                        break;
                                    case 6:
                                        toadd = 181615;// RedElegance
                                        break;
                                    case 7:
                                        toadd = 181625;// RedCelestial
                                        break;
                                    case 8:
                                        toadd = 181715;// GreenElegance
                                        break;
                                    case 9:
                                        toadd = 181725;// GreenCelestial
                                        break;
                                    case 10:
                                        toadd = 181815;// BlueElegance
                                        break;
                                    case 11:
                                        toadd = 181825;// BlueCelestial
                                        break;
                                    case 12:
                                        toadd = 181915;// PurpleElegance
                                        break;
                                    case 13:
                                        toadd = 181925;// PurpleCelestial
                                        break;
                                }

                                GC.MyChar.AddItem(toadd);
                            }
                            else
                            {
                                GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                            }
                        }
                        else
                        {
                            AddText("You don't have 6 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 12:
                    {
                        if (GC.MyChar.VotePoints >= 15)
                        {
                            if (GC.MyChar.Inventory.Count <= 39) // BlackTulip = 723584
                            {
                                GC.MyChar.AddItem(723584);
                                GC.MyChar.VotePoints -= 15;
                                AddText("Congratulations! You have received a BlackTulip!");
                                AddOption("Thanks", 255);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 15 Vote Point.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 13:
                    {
                        if (GC.MyChar.VotePoints >= 10)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                GC.MyChar.VotePoints -= 10;
                                int _id = Program.Rnd.Next(0, 30);
                                uint toadd = 181345;
                                switch (_id)
                                {
                                    case 0:
                                        toadd = 181345; // ColorfulDress
                                        break;
                                    case 1:
                                        toadd = 181365; // PrairieWind
                                        break;
                                    case 2:
                                        toadd = 181355; // DarkWizard
                                        break;
                                    case 3:
                                        toadd = 181385; // RoyalDignity
                                        break;
                                    case 4:
                                        toadd = 181395; // UglyDuck
                                        break;
                                    case 5:
                                        toadd = 181505; // BlackPhoenix
                                        break;
                                    case 6:
                                        toadd = 181555; // DarkWizard
                                        break;
                                    case 7:
                                        toadd = 191405; // Alternate Goodluck *rare*
                                        break;
                                    case 8:
                                        toadd = 181395; // Ugly Duck
                                        break;
                                    case 9:
                                        toadd = 181555; // Dark Wizard
                                        break;
                                    case 10:
                                        toadd = 191905; // Normal Goodluck 
                                        break;
                                    case 11:
                                        toadd = 191405; // Alternate Goodluck *rare*
                                        break;
                                    case 12:
                                        toadd = 182345; // MoonOrchid
                                        break;
                                    case 13:
                                        toadd = 181355; // DarkWizard
                                        break;
                                    case 14:
                                        toadd = 181385; // RoyalDignity
                                        break;
                                    case 15:
                                        toadd = 181705; // GreenPhoenix
                                        break;
                                    case 16:
                                        toadd = 181505; // BlackPhoenix
                                        break;
                                    case 17:
                                        toadd = 181395; // UglyDuck
                                        break;
                                    case 18:
                                        toadd = 182505; // SouthOfCloud
                                        break;
                                    case 19:
                                        toadd = 182535; // Blue Dream
                                        break;
                                    case 20:
                                        toadd = 182415; // Bonfire Night
                                        break;
                                    case 21:
                                        toadd = 181675; // SongOfTianshan
                                        break;
                                    case 22:
                                        toadd = 191605; // Normal Goodluck
                                        break;
                                    case 23:
                                        toadd = 191405; // Alternate Goodluck *rare*
                                        break;
                                    case 24:
                                        toadd = 182505; // SouthOfCloud
                                        break;
                                    case 25:
                                        toadd = 191605; // Normal Goodluck
                                        break;
                                    case 26:
                                        toadd = 182415; // Bonfire Night
                                        break;
                                    case 27:
                                        toadd = 181605; // RedPhoenix
                                        break;
                                    case 28:
                                        toadd = 191605; // Normal Goodluck
                                        break;
                                    case 29:
                                        toadd = 191405; // Alternate Goodluck *rare*
                                        break;
                                }
                                GC.MyChar.AddItem(toadd);
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 1 free slot in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have 10 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 14:
                    {
                        if (GC.MyChar.VotePoints >= 10 && GC.MyChar.Inventory.Count <= 38)
                        {
                            GC.MyChar.VotePoints -= 10;  // roll order: garment, cloudbox, clouddiamond, dbscroll, virtue pts, vote pts, metscroll
                            if (MyMath.ChanceSuccess(1))
                            {
                                uint Item;
                                List<uint> From = new List<uint>();
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                        From.Add(D.ID);
                                }
                                Item = (uint)From[Program.Rnd.Next(0, From.Count)];
                                GC.MyChar.AddItem(Item);
                                GC.LocalMessage(2005, "You gained a random Garment!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won a random Garment!", 2011, 0);
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                GC.MyChar.AddItem(1088000);
                                GC.LocalMessage(2005, "You gained a Dragonball!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won a Dragonball!", 2011, 0);
                            }
                            else if (MyMath.ChanceSuccess(8))
                            {
                                GC.MyChar.AddItem(721080);
                                GC.LocalMessage(2005, "You gained a MoonBox!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won a MoonBox!", 2005, 0);
                            }
                            else if (MyMath.ChanceSuccess(40))
                            {
                                GC.MyChar.AddItem(1088000);
                                GC.MyChar.AddItem(1088000);
                                GC.LocalMessage(2005, "You gained 2 Dragonballs!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won 2 Dragonballs!", 2005, 0);
                            }
                            else if (MyMath.ChanceSuccess(10))
                            {
                                GC.MyChar.AddItem(720028);
                                GC.LocalMessage(2005, "You gained a DBScroll!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won a DBScroll!", 2005, 0);
                            }
                            else if (MyMath.ChanceSuccess(4.25))
                            {
                                GC.MyChar.AddItem(723584);
                                GC.LocalMessage(2005, "You gained a BlackTulip!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won a BlackTulip!", 2005, 0);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.VP += 10000;
                                GC.LocalMessage(2005, "You gained 10,000 Virtue Points!");
                            }
                            else if (MyMath.ChanceSuccess(1))
                            {
                                GC.MyChar.ClassicPoints += 5;
                                GC.LocalMessage(2005, "You gained 3 Online Points!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " choose a random reward in exchange of his/her vote points and won 5 Online Points!", 2005, 0);
                            }
                            else if (MyMath.ChanceSuccess(12.5))
                            {
                                GC.MyChar.Silvers += 250000;
                                GC.LocalMessage(2005, "You gained 250,000 silvers!");
                            }
                            else if (MyMath.ChanceSuccess(5))
                            {
                                GC.MyChar.Silvers += 500000;
                                GC.LocalMessage(2005, "You gained 500,000 silvers!");
                            }
                            else if (MyMath.ChanceSuccess(1))
                            {
                                GC.MyChar.Silvers += 1000000;
                                GC.LocalMessage(2005, "You gained 1,000,000 silvers!");
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                GC.MyChar.AddItem(720027);
                                GC.MyChar.AddItem(720027);
                                GC.LocalMessage(2005, "You gained 2 MeteorScrolls!");
                            }
                            else
                            {
                                GC.MyChar.AddItem(720027);
                                GC.LocalMessage(2005, "You gained a MeteorScroll!");
                            }
                        }
                        else
                        {
                            AddText("You don't have 10 Vote Points.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}