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
    public class NPC_1366 : NPCBase
    {
        public NPC_1366(Main.GameClient _client)
            : base(_client)
        {
            ID = 1366;
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
                        AddText("Welcome! I am the PrizeGiver! ");
                        if (GC.MyChar.DBScrolls > 0)
                        {
                            if (!World.LowRatedServer)
                            {
                                AddText("Have you come to receive your DBScrolls? ");
                                AddOption("Yes! I want my DBScrolls!", 1);
                            }
                            else
                            {
                                AddText("Have you come to receive your DBs? ");
                                AddOption("Yes! I want my DBs!", 1);
                            }
                        }
                        if (GC.MyChar.VIPLevelToReceive > 0)
                        {
                            AddText("Have you come to receive your VIP Card?");
                            AddOption("Yes! I want my VIP Card!", 3);
                        }
                        AddOption("Just passing by.", 255);
                        break;
                    }
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
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBScrolls " + FreeSpace + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    GC.MyChar.DBScrolls = (ushort)(GC.MyChar.DBScrolls - FreeSpace);
                                    AddText("You received " + FreeSpace + " DBScrolls.");
                                    AddOption("I see.", 255);
                                }
                                else
                                {
                                    for (int i = 0; i < GC.MyChar.DBScrolls; i++)
                                    {
                                        GC.MyChar.AddItem(720028);
                                    }
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBScrolls " + GC.MyChar.DBScrolls + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    AddText("You received " + GC.MyChar.DBScrolls + " DBScrolls.");
                                    AddOption("I see.", 255);
                                    GC.MyChar.DBScrolls = 0;
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
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBs " + DBsGiven + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    GC.MyChar.DBScrolls = (ushort)(GC.MyChar.DBScrolls - DBsGiven);
                                    AddText("You received " + DBsGiven + " DBs.");
                                    AddOption("I see.", 255);
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
                                    Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, DBs " + GC.MyChar.DBScrolls + " at the time: " + DateTime.Now + " GMT -7 \r\n";
                                    AddText("You received " + GC.MyChar.DBScrolls + " DBs.");
                                    AddOption("I see.", 255);
                                    GC.MyChar.DBScrolls = 0;
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
                            if (GC.MyChar.ClassicPoints < 90)
                            {
                                GC.MyChar.ClassicPoints += 10;
                                GC.LocalMessage(2000, "You received 10 Online Points for buying a VIP card!");
                            }
                            AddText("You received your VIP Card!");
                            AddOption("Thanks.", 255);
                            Game.World.DonationAdd += GC.MyChar.Name + " has received from PRIZE NPC, VIP Card " + I.Bless + " , " + I.Plus + " at the time: " + DateTime.Now + " GMT -7 \r\n";
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