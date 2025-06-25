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
    public class NPC_1368 : NPCBase
    {
        public NPC_1368(Main.GameClient _client)
            : base(_client)
        {
            ID = 1368;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Hello! I am the prize giver!");
                    AddOption("Claim My Donations!", 11);
                    AddOption("Just passing by.", 255);
                    break;
                case 11:
                    {
                        AddText("Hello! I am the prize giver! ");
                        if (GC.MyChar.DBScrolls > 0)
                        {
                            if (!World.LowRatedServer)
                            {
                                AddText("Have you come to receive your DBScrolls? ");
                                AddOption("Yes I want my DBScrolls!", 1);
                            }
                            else
                            {
                                AddText("Have you come to receive your DBs? ");
                                AddOption("Yes I want my DBs!", 1);
                            }
                        }
                        else if (GC.MyChar.GarmentToken > 2000 && GC.MyChar.GarmentToken < 2004)
                        {
                            if (!World.LowRatedServer)
                            {
                                AddText("Have you come to receive your Garments? ");
                                AddOption("Yes I want my Garments!", 5);
                            }
                            else
                            {
                                AddText("Have you come to receive your Garments? ");
                                AddOption("Yes I want my Garments!", 5);
                            }
                        }
                        else if (GC.MyChar.VIPLevelToReceive == 5)
                        {
                            AddText("Have you come to receive your VIP Card?");
                            AddOption("Yes I want my VIP Card!", 3);
                        }
                        else if (GC.MyChar.GarmentToken == 4)
                        {
                            AddText("Have you come to receive your Mining VIP Card Packet?");
                            AddOption("Yes I want my MiningVIP Card!", 7);
                        }
                        else if (GC.MyChar.GarmentToken == 5)
                        {
                            AddText("Have you come to receive your Mining VIP Card Packet?");
                            AddOption("Yes I want my MiningVIP Card!", 9);
                        }
                        else if (GC.MyChar.GarmentToken >= 2010 && GC.MyChar.GarmentToken <= 2029)
                        {
                            AddText("Have you come to receive your Accessory?");
                            AddOption("Yes I want my Accessory!", 20);
                        }
                        AddOption("Just passing by.", 255);

                        break;
                    }

                #region Accessory
                case 20:
                    {
                        if (GC.MyChar.GarmentToken == 2010)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2010)
                                    {
                                        GC.MyChar.GarmentToken -= 2010;
                                        GC.MyChar.AddItem(720170);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }

                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2011)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2011)
                                    {
                                        GC.MyChar.GarmentToken -= 2011;
                                        GC.MyChar.AddItem(720171);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2012)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2012)
                                    {
                                        GC.MyChar.GarmentToken -= 2012;
                                        GC.MyChar.AddItem(720172);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2013)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2013)
                                    {
                                        GC.MyChar.GarmentToken -= 2013;
                                        GC.MyChar.AddItem(720186);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2014)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2014)
                                    {
                                        GC.MyChar.GarmentToken -= 2014;
                                        GC.MyChar.AddItem(720173);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2015)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2015)
                                    {
                                        GC.MyChar.GarmentToken -= 2015;
                                        GC.MyChar.AddItem(720174);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2016)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2016)
                                    {
                                        GC.MyChar.GarmentToken -= 2016;
                                        GC.MyChar.AddItem(720175);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2017)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2017)
                                    {
                                        GC.MyChar.GarmentToken -= 2017;
                                        GC.MyChar.AddItem(720176);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2018)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2018)
                                    {
                                        GC.MyChar.GarmentToken -= 2018;
                                        GC.MyChar.AddItem(720177);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2019)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2019)
                                    {
                                        GC.MyChar.GarmentToken -= 2019;
                                        GC.MyChar.AddItem(720178);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2020)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2020)
                                    {
                                        GC.MyChar.GarmentToken -= 2020;
                                        GC.MyChar.AddItem(720179);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2021)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2021)
                                    {
                                        GC.MyChar.GarmentToken -= 2021;
                                        GC.MyChar.AddItem(720180);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2022)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2022)
                                    {
                                        GC.MyChar.GarmentToken -= 2022;
                                        GC.MyChar.AddItem(720181);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2023)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2023)
                                    {
                                        GC.MyChar.GarmentToken -= 2023;
                                        GC.MyChar.AddItem(720182);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2024)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2024)
                                    {
                                        GC.MyChar.GarmentToken -= 2024;
                                        GC.MyChar.AddItem(720183);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2025)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2025)
                                    {
                                        GC.MyChar.GarmentToken -= 2025;
                                        GC.MyChar.AddItem(720184);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2026)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2026)
                                    {
                                        GC.MyChar.GarmentToken -= 2026;
                                        GC.MyChar.AddItem(720185);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2027)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2027)
                                    {
                                        GC.MyChar.GarmentToken -= 2027;
                                        GC.MyChar.AddItem(720187);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2028)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2028)
                                    {
                                        GC.MyChar.GarmentToken -= 2028;
                                        GC.MyChar.AddItem(720188);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else if (GC.MyChar.GarmentToken == 2029)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029 && GC.MyChar.GarmentToken < 2009 || GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2029)
                                    {
                                        GC.MyChar.GarmentToken -= 2029;
                                        GC.MyChar.AddItem(720189);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, Accessory at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Accessory. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received Accessory.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }



                    }
                    break;


                #endregion

                #region Garments
                case 5:
                    {
                        if (GC.MyChar.GarmentToken == 2001)
                        {
                            int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                            if (FreeSpace > 0)
                            {
                                if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                {
                                    Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                    GC.MyChar.GarmentToken = 0;
                                }
                                if (!World.LowRatedServer)
                                {
                                    if (GC.MyChar.GarmentToken == 2001)
                                    {
                                        GC.MyChar.GarmentToken -= 2001;
                                        GC.MyChar.AddItem(720144);
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, GARMENTTOKEN1 at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received garment. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received GARMENTTOKEN1.");
                                        AddOption("I see.", 255);
                                    }
                                    else
                                    {
                                        AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("Your inventory is full.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            if (GC.MyChar.GarmentToken == 2002)
                            {
                                int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                                if (FreeSpace > 0)
                                {
                                    if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                    {
                                        Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                        GC.MyChar.GarmentToken = 0;
                                    }
                                    if (!World.LowRatedServer)
                                    {
                                        if (GC.MyChar.GarmentToken == 2002)
                                        {
                                            GC.MyChar.GarmentToken -= 2002;
                                            GC.MyChar.AddItem(720145);
                                            Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, GARMENTTOKEN2 at the time: " + DateTime.Now + " GMT -7 \r\n";
                                            //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received garment. You can also donate to contribute to the game!", 2000, 0);
                                            AddText("You received GARMENTTOKEN2.");
                                            AddOption("I see.", 255);
                                        }
                                        else
                                        {
                                            AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                            AddOption("I see.", 255);
                                        }
                                    }
                                }
                                else
                                {
                                    AddText("Your inventory is full.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                if (GC.MyChar.GarmentToken == 2003)
                                {
                                    int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                                    if (FreeSpace > 0)
                                    {
                                        if (GC.MyChar.GarmentToken < 2000 && GC.MyChar.GarmentToken > 2029)
                                        {
                                            Game.World.DebugAdd += "Critical error: Points on char: " + GC.MyChar.Name + " : " + GC.MyChar.GarmentToken;
                                            GC.MyChar.GarmentToken = 0;
                                        }
                                        if (!World.LowRatedServer)
                                        {
                                            if (GC.MyChar.GarmentToken == 2003)
                                            {
                                                GC.MyChar.GarmentToken -= 2003;
                                                GC.MyChar.AddItem(720146);
                                                Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, GARMENTTOKEN3 at the time: " + DateTime.Now + " GMT -7 \r\n";
                                                //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received garment. You can also donate to contribute to the game!", 2000, 0);
                                                AddText("You received GARMENTTOKEN3.");
                                                AddOption("I see.", 255);
                                            }
                                            else
                                            {
                                                AddText("Sorry you have only " + GC.MyChar.GarmentToken + " Points.");
                                                AddOption("I see.", 255);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AddText("Your inventory is full.");
                                        AddOption("I see.", 255);
                                    }
                                }



                            }

                        }
                        break;
                    }

                #endregion

                #region DBS
                case 1:
                    {
                        if (GC.MyChar.DBScrolls == 0)
                        {
                            if (!World.LowRatedServer)
                                AddText("You don't have any DBScrolls to claim!");
                            else AddText("You don't have any DBs to claim!");
                            AddOption("I see.", 255);
                        }
                        else
                        {
                            if (!World.LowRatedServer)
                                AddText("You have " + GC.MyChar.DBScrolls + " DBScrolls to receive.");
                            else AddText("You have " + GC.MyChar.DBScrolls + " DBs to receive.");
                            AddOption("Give me them.", 2);
                            AddOption("Not now.", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        int FreeSpace = 40 - GC.MyChar.Inventory.Count;
                        if (FreeSpace > 0)
                        {
                            if (GC.MyChar.DBScrolls > 400)
                            {
                                Game.World.DebugAdd += "Critical error: DBS on char: " + GC.MyChar.Name + " : " + GC.MyChar.DBScrolls;
                                GC.MyChar.DBScrolls = 0;
                            }
                            if (!World.LowRatedServer)
                            {
                                if (GC.MyChar.DBScrolls > FreeSpace)
                                {
                                    for (int i = 0; i < FreeSpace; i++)
                                    {
                                        GC.MyChar.AddItem(720028);
                                    }
                                    if (GC.MyChar.DBScrolls == 0)
                                    {

                                    }
                                    else
                                    {
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBScrolls " + FreeSpace + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received DBs. You can also donate to contribute to the game!", 2000, 0);
                                        GC.MyChar.DBScrolls = (ushort)(GC.MyChar.DBScrolls - FreeSpace);
                                        AddText("You received " + FreeSpace + " DBScrolls.");
                                        AddOption("I see.", 255);
                                    }

                                }
                                else
                                {
                                    for (int i = 0; i < GC.MyChar.DBScrolls; i++)
                                    {
                                        GC.MyChar.AddItem(720028);
                                    }
                                    if (GC.MyChar.DBScrolls == 0)
                                    {

                                    }
                                    else
                                    {
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBScrolls " + GC.MyChar.DBScrolls + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received DBs. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received " + GC.MyChar.DBScrolls + " DBScrolls.");
                                        AddOption("I see.", 255);
                                        GC.MyChar.DBScrolls = 0;
                                    }

                                }
                            }
                            else
                            {
                                ushort DBs = (ushort)(GC.MyChar.DBScrolls % 10);
                                ushort DBScrolls = (ushort)(GC.MyChar.DBScrolls / 10);
                                ushort SpaceNeeded = (ushort)(DBs + DBScrolls);
                                ushort DBsGiven = 0;
                                if (SpaceNeeded > FreeSpace)
                                {
                                    while (DBScrolls > 0 && GC.MyChar.Inventory.Count < 40)
                                    {
                                        GC.MyChar.AddItem(720028);
                                        DBScrolls -= 1;
                                        DBsGiven += 10;
                                    }
                                    while (DBs > 0 && GC.MyChar.Inventory.Count < 40)
                                    {
                                        GC.MyChar.AddItem(1088000);
                                        DBs -= 1;
                                        DBsGiven += 1;
                                    }

                                    if (GC.MyChar.DBScrolls == 0)
                                    {

                                    }
                                    else
                                    {
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBs " + DBsGiven + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received DBs. You can also donate to contribute to the game!", 2000, 0);
                                        GC.MyChar.DBScrolls = (ushort)(GC.MyChar.DBScrolls - DBsGiven);
                                        AddText("You received " + DBsGiven + " DBs.");
                                        AddOption("I see.", 255);
                                    }

                                }
                                else
                                {
                                    for (int i = 0; i < DBScrolls; i++)
                                    {
                                        GC.MyChar.AddItem(720028);
                                    }
                                    for (int i = 0; i < DBs; i++)
                                    {
                                        GC.MyChar.AddItem(1088000);
                                    }

                                    if (GC.MyChar.DBScrolls == 0)
                                    {

                                    }
                                    else
                                    {
                                        Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBs " + GC.MyChar.DBScrolls + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                        //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received DBs. You can also donate to contribute to the game!", 2000, 0);
                                        AddText("You received " + GC.MyChar.DBScrolls + " DBs.");
                                        AddOption("I see.", 255);
                                        GC.MyChar.DBScrolls = 0;
                                    }

                                }
                            }
                        }
                        else
                        {
                            AddText("Your inventory is full.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #region VIP
                case 3:
                    {
                        if (GC.MyChar.VIPLevelToReceive == 0)
                        {
                            AddText("You don't have any VIP Card to claim!");
                            AddOption("I see.", 255);
                        }
                        else if (GC.MyChar.VIPLevelToReceive == 3)
                        {
                            AddText("You have VIP Card Level : " + GC.MyChar.VIPLevelToReceive + "  Days: " + GC.MyChar.VIPDaysToReceive + " to receive.");
                            AddOption("Give it to me!", 6);
                            AddOption("Not now.", 255);
                        }
                        else
                        {
                            if (GC.MyChar.VIPDaysToReceive > 30)
                                AddText("You have VIP Cards Level : " + GC.MyChar.VIPLevelToReceive + "  Days: 30++ (you have multiple VIP cards waiting) to receive.");
                            else
                                AddText("You have VIP Card Level : " + GC.MyChar.VIPLevelToReceive + "  Days: " + GC.MyChar.VIPDaysToReceive + " to receive.");
                            AddOption("Give it to me!", 4);
                            AddOption("Not now.", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            Item I = new Item();
                            I.ID = 780001;
                            if (GC.MyChar.VIPDaysToReceive > 30)
                            {
                                I.Plus = 30;
                                I.Bless = GC.MyChar.VIPLevelToReceive;
                                GC.MyChar.VIPDaysToReceive -= 30;
                            }
                            else
                            {
                                I.Plus = GC.MyChar.VIPDaysToReceive;
                                I.Bless = GC.MyChar.VIPLevelToReceive;
                                GC.MyChar.VIPDaysToReceive = 0;
                                GC.MyChar.VIPLevelToReceive = 0;
                            }
                            I.MaxDur = I.DBInfo.Durability;
                            I.CurDur = I.MaxDur;
                            GC.MyChar.AddItem(I);

                            if (I.Bless == 0 && I.Plus == 0 && I.ID == 780001)
                            {
                                GC.MyChar.RemoveItem(I);
                            }
                            else
                            {
                                AddText("You received your VIP Card!");
                                AddOption("Thanks.", 255);
                                Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, VIP Card " + I.Bless + " , " + I.Plus + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received VIP. You can also donate to contribute to the game!", 2000, 0);
                            }
                            if (GC.MyChar.ClassicPoints < 90)
                            {
                                GC.MyChar.ClassicPoints += 10;
                                GC.LocalMessage(2000, "You received 10 Online Points for buying a VIP card!");
                            }

                        }
                        else
                        {
                            AddText("You don't have enough space in your inventory!");
                            AddOption("Ohh.", 255);
                        }
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.Inventory.Count < 40)
                        {
                            Item I = new Item();
                            I.ID = 780000;
                            if (GC.MyChar.VIPDaysToReceive > 30)
                            {
                                I.Plus = 30;
                                I.Bless = 3;
                                GC.MyChar.VIPDaysToReceive -= 30;
                            }
                            else
                            {
                                I.Plus = GC.MyChar.VIPDaysToReceive;
                                I.Bless = GC.MyChar.VIPLevelToReceive;
                                GC.MyChar.VIPDaysToReceive = 0;
                                GC.MyChar.VIPLevelToReceive = 0;
                            }
                            I.MaxDur = I.DBInfo.Durability;
                            I.CurDur = I.MaxDur;
                            GC.MyChar.AddItem(I);

                            if (I.Bless == 0 && I.Plus == 0 && I.ID == 780000)
                            {
                                GC.MyChar.RemoveItem(I);
                            }
                            else
                            {
                                AddText("You received your VIP Card!");
                                AddOption("Thanks.", 255);
                                Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, VIP Card " + I.Bless + " , " + I.Plus + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Mining VIP. You can also donate to contribute to the game!", 2000, 0);
                            }
                            if (GC.MyChar.ClassicPoints < 90)
                            {
                                GC.MyChar.ClassicPoints += 10;
                                GC.LocalMessage(2000, "You received 10 Online Points for buying a VIP card!");
                            }

                        }
                        else
                        {
                            AddText("You don't have enough space in your inventory!");
                            AddOption("Ohh.", 255);
                        }
                        break;
                    }
                #endregion

                #region Packet
                case 7:
                    {

                        if (GC.MyChar.GarmentToken == 4)
                            AddText("You have VIP Card Level : 3  Days: 30 to receive.");
                        AddOption("Give it to me!", 8);
                        AddOption("Not now.", 255);
                    }
                    break;


                case 8:
                    {
                        if (GC.MyChar.Inventory.Count < 29)
                        {
                            GC.MyChar.GarmentToken -= 4;
                            for (int a = 0; a < 5; a++)
                            {
                                Item I = new Item();
                                I.ID = 780000;
                                I.Plus = GC.MyChar.VIPDaysToReceive;
                                I.Bless = GC.MyChar.VIPLevelToReceive;
                                I.MaxDur = I.DBInfo.Durability;
                                I.CurDur = I.MaxDur;
                                GC.MyChar.AddItem(I);

                                if (I.Bless == 0 && I.Plus == 0 && I.ID == 780000)
                                {
                                    GC.MyChar.RemoveItem(I);
                                }

                                else
                                {
                                    AddText("You received your VIP Card!");
                                    AddOption("Thanks.", 255);
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, VIP Card at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Mining VIP. You can also donate to contribute to the game!", 2000, 0);
                                }
                            }
                            GC.MyChar.VIPDaysToReceive = 0;
                            GC.MyChar.VIPLevelToReceive = 0;

                        }
                        else
                        {
                            AddText("You don't have enough space in your inventory!");
                            AddOption("Ohh.", 255);
                        }
                        break;
                    }
                #endregion
                #region MiningVip
                case 9:
                    {

                        if (GC.MyChar.GarmentToken == 5)
                            AddText("You have VIP Card Level : 3  Days: 30 to receive.");
                        AddOption("Give it to me!", 10);
                        AddOption("Not now.", 255);
                    }
                    break;


                case 10:
                    {
                        if (GC.MyChar.Inventory.Count < 29)
                        {
                            GC.MyChar.GarmentToken -= 5;
                            for (int a = 0; a < 1; a++)
                            {
                                Item I = new Item();
                                I.ID = 780000;
                                I.Plus = GC.MyChar.VIPDaysToReceive;
                                I.Bless = GC.MyChar.VIPLevelToReceive;
                                I.MaxDur = I.DBInfo.Durability;
                                I.CurDur = I.MaxDur;
                                GC.MyChar.AddItem(I);

                                if (I.Bless == 0 && I.Plus == 0 && I.ID == 780000)
                                {
                                    GC.MyChar.RemoveItem(I);
                                }
                                else
                                {
                                    AddText("You received your VIP Card!");
                                    AddOption("Thanks.", 255);
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, VIP Card at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    //Game.World.SendMsgToAll("DONATE", GC.MyChar.Name + " donated to our game and received Mining VIP. You can also donate to contribute to the game!", 2000, 0);
                                }
                            }
                            GC.MyChar.VIPDaysToReceive = 0;
                            GC.MyChar.VIPLevelToReceive = 0;



                        }
                        else
                        {
                            AddText("You don't have enough space in your inventory!");
                            AddOption("Ohh.", 255);
                        }
                        break;
                    }
                    #endregion

            }

            AddFinish();
            Send();
        }
    }
}