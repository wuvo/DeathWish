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
    public class NPC_6666 : NPCBase
    {
        public NPC_6666(Main.GameClient _client)
            : base(_client)
        {
            ID = 6666;
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
                        AddText("As you have probably heard already, active players will have benefits. You will recieve online points at random times for being online. Later on, the points can be exchanged by rewards! How cool is that?");
                        AddOption("Check my Online Points", 1);
                        AddOption("Exchange for rewards", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Currently you have " + GC.MyChar.ClassicPoints + " Online Points! Make sure you keep your account logged in so you can exchange them for rewards!");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("What would you like to exchange your Online Points for?");
                        AddOption("Double Experience (10)", 3);
                        //AddOption("Better drops (3)", 4);
                        AddOption("2,000 Virtue Points (5)", 5);
                        AddOption("Random reward (6)", 6);
                        AddOption("Broadcast (2)", 7);
                        //AddOption("MoonBox (10)", 9);
                        AddOption("1 Day Vip (50)", 10);
                        AddOption("4 Day Vip (150)", 11);
                        //AddOption("ExpMob (200)", 15);
                        AddOption("Nevermind", 255);
                        break;
                    }
                //case 15:
                //    {
                //        if (GC.MyChar.ClassicPoints >= 200)
                //        {
                //            GC.MyChar.ClassicPoints -= 200;
                //            Game.World.ExpMob = true;
                //            AddText("Congratulations! ExpMob spawn in GetPromed Map.!");
                //            AddOption("Thanks", 255);
                //        }
                //        else
                //        {
                //            AddText("You don't have enough Online Points!");
                //            AddOption("I see", 255);
                //        }

                //    }
                //    break;

                case 10:
                    {
                        if (GC.MyChar.VipLevel != 3)
                        {
                            if (GC.MyChar.ClassicPoints >= 50)
                            {
                                GC.MyChar.ClassicPoints -= 50;
                                if (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24) || GC.MyChar.VIPDays == 0)
                                    GC.MyChar.VIPStarted = DateTime.Now;
                                if (GC.MyChar.VipLevel != 6)
                                {
                                    GC.MyChar.VipLevel = 5;
                                }
                                GC.MyChar.VIPDays += 1;
                                AddText("Congratulations! You now have VIP for 1 day!");
                                AddOption("Thanks", 255);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " exchange 50 Online Points for 1 Day Vip!1 day of VIP!", 2011, 0);
                            }
                            else
                            {
                                AddText("You don't have enough Online Points!");
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

                case 11:
                    {
                        if (GC.MyChar.VipLevel != 3)
                        {
                            if (GC.MyChar.ClassicPoints >= 150)
                            {
                                GC.MyChar.ClassicPoints -= 150;
                                if (DateTime.Now > GC.MyChar.VIPStarted.AddHours(24) || GC.MyChar.VIPDays == 0)
                                    GC.MyChar.VIPStarted = DateTime.Now;
                                if (GC.MyChar.VipLevel != 6)
                                {
                                    GC.MyChar.VipLevel = 5;
                                }
                                GC.MyChar.VIPDays += 4;
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " exchanged 150 Online Points for 4 days of VIP!", 2011, 0);
                                AddText("Congratulations! You now have VIP for 4 days.");
                                AddOption("Thanks", 255);



                            }
                            else
                            {
                                AddText("You don't have enough Online Points!");
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

                case 9:
                    {
                        if (GC.MyChar.ClassicPoints >= 10)
                        {
                            GC.MyChar.ClassicPoints -= 10;
                            GC.MyChar.AddItem(721080);

                            AddText("Congratulations! You have received MoonBox!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.ClassicPoints >= 10)
                        {
                            GC.MyChar.ClassicPoints -= 10;
                            GC.MyChar.ExpPotionUsed = DateTime.Now;
                            GC.MyChar.DoubleExp = true;
                            GC.MyChar.DoubleExpLeft = 3600;
                            GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));
                            AddText("Congratulations! You have received 1 Hour of Double Experience!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.ClassicPoints >= 3)
                        {
                            GC.MyChar.ClassicPoints -= 3;
                            GC.MyChar.LuckyTime = 3600;
                            GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.LuckyTime, (ulong)GC.MyChar.LuckyTime));
                            AddText("Congratulations! Your drops are increased for the next hour! Make sure you make the best out of it!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.ClassicPoints >= 5)
                        {
                            GC.MyChar.ClassicPoints -= 5;
                            GC.MyChar.VP += 2000;
                            AddText("Congratulations! You have received 2,000 Virtue Points!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #region Online Points Random Reward
                case 6:
                    {
                        if (GC.MyChar.ClassicPoints >= 6)
                        {
                            if (GC.MyChar.Inventory.Count <= 35)
                            {
                                if (MyMath.ChanceSuccess(30))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.AddItem(720027);
                                    GC.MyChar.VP += 2000;
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " exchanged 6 Online Points and received a MeteorScroll & 2000 Virtue Points in return!", 2005, 0);
                                }
                                else if (MyMath.ChanceSuccess(10))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.AddItem(720027);
                                    GC.MyChar.VP += 2000;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 2,000 Virtue Points and a MeteorScroll as a reward!");
                                }
                                else if (MyMath.ChanceSuccess(2))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.Silvers += 250000;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 250,000 silvers as reward!");
                                }
                                else if (MyMath.ChanceSuccess(5))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.Silvers += 250000;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 250,000 silvers as reward!");
                                }
                                else if (MyMath.ChanceSuccess(1))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.AddItem(721258);
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received a CleanWater as reward!");
                                }
                                else if (MyMath.ChanceSuccess(7.5))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.VotePoints++;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 1 Vote Point as reward!");
                                }
                                else if (MyMath.ChanceSuccess(2.5))
                                {
                                    GC.MyChar.ClassicPoints -= 6;
                                    GC.MyChar.VotePoints++;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 1 Vote Points as reward!");
                                }
                                else if (MyMath.ChanceSuccess(5))
                                {
                                    #region +1 Item
                                    GC.MyChar.ClassicPoints -= 6;
                                top:
                                    Item I2 = new Item();
                                    I2.UID = (uint)Program.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Elite;
                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = Program.Rnd.Next(0, 165);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
                                        {
                                            if (D.LevReq != 0)
                                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                    From.Add(D.ID);
                                        }
                                    }
                                    if (From != null)
                                    {
                                        if (From.Count > 0)
                                        {
                                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }
                                        I2.Color = Item.ArmorColor.Orange;
                                        I2.Plus = 1;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;
                                        GC.MyChar.AddItem(I2);
                                        GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received a +1" + I2.DBInfo.Name + " as reward!");
                                    }
                                    else goto top;
                                    #endregion
                                }
                                else
                                {
                                    GC.MyChar.ClassicPoints -= 6;

                                    GC.MyChar.Silvers += 250000;
                                    GC.LocalMessage(2000, "Congratulations! You exchanged 6 Online Points and received 250,000 silvers as reward!");
                                }
                            }
                            else GC.LocalMessage(2000, "Please make sure you have 5 free slots in your inventory.");
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                #endregion
                case 7:
                    {
                        if (GC.MyChar.ClassicPoints >= 2)
                        {
                            AddText("Please type in the message you would like to broadcast!");
                            AddInput("Message: ", 8);
                        }
                        else
                        {
                            AddText("You don't have enough Online Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 8:
                    {
                        GC.MyChar.ClassicPoints -= 2;
                        Game.BroadCastMessage B = new Ultimate.Game.BroadCastMessage();
                        B.Name = GC.MyChar.Name + GC.AuthInfo.Status;
                        B.Message = ReadString(_data);
                        B.Place = Game.World.BroadCastCount;
                        Game.World.BroadCasts[Game.World.BroadCastCount] = B;
                        Game.World.BroadCastCount++;
                        AddText("Your message has been sent! It will appear in a matter of seconds!");
                        AddOption("Thanks", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}