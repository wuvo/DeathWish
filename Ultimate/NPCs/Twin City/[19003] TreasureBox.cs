using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;
using Ultimate.Structures;

namespace Ultimate.NPCs
{
    public class NPC_19003 : NPCBase
    {
        public NPC_19003(Main.GameClient _client)
            : base(_client)
        {
            ID = 19003;
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
                        AddText("Treasure Hunt is currently offline.");
                        AddText("Howdy! As you have probably noticed, monsters have invaded our world and they stole some of our most precious ");
                        AddText("treasures. Some of these treasures were lost and we are organizing expedictions to find them every day. Be aware tho,");
                        AddText("monsters have corrupted some of the treasures and you might find something you won't like!");
                        AddOption("Tell me more about it!", 20);
                        AddOption("Join the Treasure Hunt!", 1);
                        AddOption("Check Treasure Points and prizes!", 2);
                        AddOption("Okay", 255);
                        break;
                    }
                case 20:
                    {
                        AddText("Treasure hunt expedictions are being held from Monday to Friday on two different schedules. On Monday, Wednesday and Friday ");
                        AddText("they start at 08:00 and on Tuesdays and Thursdays they start at 18:00. Once you join, you'll be sent to a random map and you'll ");
                        AddText("have to hunt treasures. Beware tho, some of them and traps.");
                        AddOption("What can I win from it?", 21);
                        AddOption("I see", 255);
                        break;
                    }
                case 21:
                    {
                        AddText("Once you find a TreasureChest you'll receive Treasure Points which can be exchanged for awesome rewards such as Meteors or Dragonballs later on. ");
                        AddText("If you capture a legit TreasureChest you'll receive 1 Treasure Point, if you capture a TreasureTrap, you'll lose two. If you capture a TreasureChance you'll have the chance of ");
                        AddText("winning 2 Treasure Points and the small chance of winning 5. However, there's also a chance you'll lose 2 Treasure Points!");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 1:
                    {
                        if (World.TreasureHunt)
                        {
                            if (World.TreasureMap == 8004)
                                GC.MyChar.Teleport(8004, 57, 58);
                            else if (World.TreasureMap == 8005)
                                GC.MyChar.Teleport(8005, 542, 538);
                            else GC.MyChar.Teleport(8006, 192, 227);
                            Buff S = GC.MyChar.BuffOf(Features.SkillsClass.ExtraEffect.Cyclone);
                            if (S.Eff == Features.SkillsClass.ExtraEffect.Cyclone)
                                if (!GC.MyChar.BDelete.ContainsKey(S))
                                    GC.MyChar.BDelete.TryAdd(S, S.Lasts);
                            Buff B = GC.MyChar.BuffOf(Features.SkillsClass.ExtraEffect.Transform);
                            if (B.Eff == Features.SkillsClass.ExtraEffect.Transform)
                                if (!GC.MyChar.BDelete.ContainsKey(B))
                                    GC.MyChar.BDelete.TryAdd(S, S.Lasts);
                            GC.LocalMessage(2005, "Good luck hunting these precious treasures and be careful with the traps! To get out of here teleport or log out!");
                        }
                        else
                        {
                            AddText("The Treasure Hunt event is not on-going!");
                            AddOption("Thanks.", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        AddText("You currently have " + GC.MyChar.TreasurePoints + " Treasure Points!");
                        AddOption("Spend Treasure Points", 3);
                        AddOption("Thanks.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("You currently have " + GC.MyChar.TreasurePoints + " Treasure Points!");
                        // AddOption("Emerald, 10 points.", 5);
                        AddOption("MeteorScroll, 25 points.", 6);
                        AddOption("DragonBall, 50 points.", 7);
                        AddOption("MoonBox, 350 points.", 8);
                        AddOption("Random +2 item non weapon, 80 points", 9);
                        AddOption("Random +3 item non weapon, 200 points", 11);
                        AddOption("Thanks.", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("You currently have " + GC.MyChar.TreasurePoints + " Treasure Points!");


                        //AddOption("Random garment, 750 points", 10);
                        AddOption("Previous Page", 3);
                        AddOption("Thanks.", 255);
                        break;
                    }
                #region Prizes
                #region Emerald
                case 5:
                    {
                        if (GC.MyChar.TreasurePoints >= 10)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 10;
                                GC.MyChar.AddItem(1080001);
                                AddText("You received an emerald!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 10 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region MetScroll
                case 6:
                    {
                        if (GC.MyChar.TreasurePoints >= 25)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 25;
                                GC.MyChar.AddItem(720027);
                                AddText("You received a MeteorScroll!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 25 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region DB
                case 7:
                    {
                        if (GC.MyChar.TreasurePoints >= 50)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 50;
                                GC.MyChar.AddItem(1088000);
                                AddText("You received a DragonBall!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 50 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region MB
                case 8:
                    {
                        if (GC.MyChar.TreasurePoints >= 350)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 350;
                                GC.MyChar.AddItem(721080);
                                AddText("You received a MoonBox!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 350 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region Random +2 item
                case 9:
                    {
                        if (GC.MyChar.TreasurePoints >= 250)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 250;
                                PlusItemReward(2, 1);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 250 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region Random +3 item
                case 11:
                    {
                        if (GC.MyChar.TreasurePoints >= 600)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 600;
                                PlusItemReward(3, 1);
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 600 treasure points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                #region Garment
                case 10:
                    {
                        if (GC.MyChar.TreasurePoints >= 750)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.TreasurePoints -= 750;
                                uint Item;
                                List<uint> From = new List<uint>();
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                        From.Add(D.ID);
                                }
                                Item = (uint)From[Program.Rnd.Next(0, From.Count)];
                                GC.MyChar.AddItem(Item);
                                AddText("You received a random garment!");
                                AddOption("Thanks", 255);
                                break;
                            }
                            else
                            {
                                AddText("Your inventory is full!");
                                AddOption("I see", 255);
                                break;
                            }
                        }
                        else
                        {
                            AddText("You don't have 750 treasure points!");
                            AddOption("I see", 255);
                            break;
                        }
                    }
                    #endregion
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
}