using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class Mining
    {
        public static void Swing(Game.Character MyChar)
        {
            if (!MyChar.Alive)
            { MyChar.Mining = false; return; }
            if (MyChar.Equips.RightHand.ID == 0)
            { MyChar.Mining = false; return; }
            if (MyChar.Equips.LeftHand.ID != 0)
            { MyChar.Mining = false; return; }
            if (MyChar.Equips.RightHand.DBInfo.Name != "Hoe" && MyChar.Equips.RightHand.DBInfo.Name != "PickAxe")
            { MyChar.Mining = false; return; }
            Game.World.Action(MyChar, Packets.GeneralData(MyChar.EntityID, 0, MyChar.Loc.X, MyChar.Loc.Y, 99).Get);
            MyChar.Action = 100;

            switch (MyChar.Loc.Map)
            {
                // case 6001://jail war mine
                case 6000://jails
                    {
                        Mine(700001, 700011, 700031, 700041, 1072010, 1072050, 1072020, 0, MyChar);
                        break;
                    }
                case 1028://twincity minecave
                    {
                        if (MyMath.ChanceSuccess(50))
                        {
                            Mine(700041, 700001, 700021, 700071, 1072010, 1072031, 1072050, 0, MyChar);
                        }
                        else
                        {
                            Mine(700011, 700031, 700021, 700041, 1072010, 1072031, 0, 0, MyChar);
                        }
                        break;
                    }
                case 1025://pc mine //metzone
                    {
                        Mine(700031, 700001, 700041, 700011, 1072010, 0, 1072020, 0, MyChar);
                        break;
                    }
                case 1027://DesertMine
                case 1026://ApeMine
                    {
                        Mine(700051, 700061, 0, 0, 1072020, 1072050, 1072040, 1072010, MyChar);
                        break;
                    }
                case 1029:
                    {
                        Mine(700001, 700011, 700031, 700061, 1072020, 1072050, 1072040, 1072010, MyChar);
                        break;
                    }
                default:
                    {
                        MyChar.MyClient.LocalMessage(2005, "Unable to mine here.");
                        MyChar.Mining = false;
                        break;
                    }
            }
        }

        static void Mine(uint GemID, uint GemID2, uint GemID3, uint GemID4, uint Ore1, uint Ore2, uint Ore3, uint Ore4, Game.Character MyChar)
        {
            if (MyChar.Inventory.Count <= 39)
            {
                Random Rnd = new Random();
                double i = 0;

                bool super = false;
                bool refined = false;

                if (MyMath.ChanceSuccess(DropRates.DragonBall * 2))
                {
                    if (MyMath.ChanceSuccess(50))
                    {
                        MyChar.Equips.RightHand.Progress += 10;

                        if (MyChar.Equips.RightHand.Progress >= Database.ComposePts[MyChar.Equips.RightHand.Plus] && MyChar.Equips.RightHand.Plus <= 8)
                        {
                            MyChar.Equips.RightHand.Progress = (ushort)(MyChar.Equips.RightHand.Progress - Database.ComposePts[MyChar.Equips.RightHand.Plus]);
                            MyChar.Equips.RightHand.Plus++;
                        }
                    }

                    if (MyMath.ChanceSuccess(50))
                    {
                        if (MyChar.Equips.RightHand.Soc1 == Item.Gem.NoSocket && MyChar.Equips.RightHand.Soc2 == Item.Gem.NoSocket)
                        {
                            MyChar.Equips.RightHand.Soc1 = Item.Gem.EmptySocket;
                        }
                        else if (MyChar.Equips.RightHand.Soc1 != Item.Gem.NoSocket && MyChar.Equips.RightHand.Soc2 == Item.Gem.NoSocket)
                        {
                            MyChar.Equips.RightHand.Soc2 = Item.Gem.EmptySocket;
                        }
                    }

                    MyChar.Equips.Replace(4, MyChar.Equips.RightHand, MyChar);
                    MyChar.EquipStats(4, true, false);
                }

                if (MyChar.Loc.Map == 1029)
                    i = 0.02;
                if (World.EventGem)
                    i += 0.05;
                if (GemID != 0 && MyMath.ChanceSuccess(DropRates.Gem + i))//GEM TYPE 1
                {
                    if (Database.DatabaseItems.ContainsKey(GemID))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        if (MyMath.ChanceSuccess(DropRates.GemSup))
                        {
                            super = true;
                            GemID += 2;
                        }

                        else if (MyMath.ChanceSuccess(DropRates.GemRef))
                        {
                            refined = true;
                            GemID++;
                        }

                        I.ID = GemID;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        if (super)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Super" + I.DBInfo.Name + "!", 2011, 0);
                        else if (refined)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Refined" + I.DBInfo.Name + "!", 2005, 0);
                        else
                        {
                            if (MyChar.skiphoenixgem && I.ID == 700001)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700001));
                            }
                            else if (MyChar.skipdragongem && I.ID == 700011)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700011));
                            }
                            else if (MyChar.skipfurygem && I.ID == 700021)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700021));
                            }
                            else if (MyChar.skiprainbowgem && I.ID == 700031)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700031));
                            }
                            else if (MyChar.skipkylingem && I.ID == 700041)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700041));
                            }
                            else if (MyChar.skipvioletgem && I.ID == 700051)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700051));
                            }
                            else if (MyChar.skipmoongem && I.ID == 700061)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700061));
                            }
                            else
                            {
                                MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                            }
                            return;
                        }



                    }
                }
                else if (GemID2 != 0 && MyMath.ChanceSuccess(DropRates.Gem + i))//GEM TYPE 2
                {
                    if (Database.DatabaseItems.ContainsKey(GemID2))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        if (MyMath.ChanceSuccess(DropRates.GemSup))
                        {
                            GemID2 += 2;
                            super = true;
                        }
                        else if (MyMath.ChanceSuccess(DropRates.GemRef))
                        {
                            refined = true;
                            GemID2++;
                        }

                        I.ID = GemID2;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        if (super)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Super" + I.DBInfo.Name + "!", 2011, 0);
                        else if (refined)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Refined" + I.DBInfo.Name + "!", 2005, 0);
                        else
                        {
                            if (MyChar.skiphoenixgem && I.ID == 700001)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700001));
                            }
                            else if (MyChar.skipdragongem && I.ID == 700011)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700011));
                            }
                            else if (MyChar.skipfurygem && I.ID == 700021)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700021));
                            }
                            else if (MyChar.skiprainbowgem && I.ID == 700031)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700031));
                            }
                            else if (MyChar.skipkylingem && I.ID == 700041)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700041));
                            }
                            else if (MyChar.skipvioletgem && I.ID == 700051)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700051));
                            }
                            else if (MyChar.skipmoongem && I.ID == 700061)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700061));
                            }
                            else
                            {
                                MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                            }
                            return;
                        }
                    }
                }
                else if (GemID3 != 0 && MyMath.ChanceSuccess(DropRates.Gem + i))//GEM TYPE 3
                {
                    if (Database.DatabaseItems.ContainsKey(GemID3))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        if (MyMath.ChanceSuccess(DropRates.GemSup))
                        {
                            GemID3 += 2;
                            super = true;
                        }
                        else if (MyMath.ChanceSuccess(DropRates.GemRef))
                        {
                            GemID3++;
                            refined = true;
                        }

                        I.ID = GemID3;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        if (super)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Super" + I.DBInfo.Name + "!", 2011, 0);
                        else if (refined)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Refined" + I.DBInfo.Name + "!", 2005, 0);
                        else
                        {
                            if (MyChar.skiphoenixgem && I.ID == 700001)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700001));
                            }
                            else if (MyChar.skipdragongem && I.ID == 700011)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700011));
                            }
                            else if (MyChar.skipfurygem && I.ID == 700021)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700021));
                            }
                            else if (MyChar.skiprainbowgem && I.ID == 700031)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700031));
                            }
                            else if (MyChar.skipkylingem && I.ID == 700041)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700041));
                            }
                            else if (MyChar.skipvioletgem && I.ID == 700051)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700051));
                            }
                            else if (MyChar.skipmoongem && I.ID == 700061)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700061));
                            }
                            else
                            {
                                MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                            }
                            return;
                        }
                    }
                }
                else if (GemID4 != 0 && MyMath.ChanceSuccess(DropRates.Gem + i))//GEM TYPE 4
                {
                    if (Database.DatabaseItems.ContainsKey(GemID4))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        if (MyMath.ChanceSuccess(DropRates.GemSup))
                        {
                            GemID4 += 2;
                            super = true;
                        }
                        else if (MyMath.ChanceSuccess(DropRates.GemRef))
                        {
                            GemID4++;
                            refined = true;
                        }

                        I.ID = GemID4;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        if (super)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Super" + I.DBInfo.Name + "!", 2011, 0);
                        else if (refined)
                            World.SendMsgToAll("LUCKY", MyChar.Name + " has mined a Refined" + I.DBInfo.Name + "!", 2005, 0);
                        else
                        {
                            if (MyChar.skiphoenixgem && I.ID == 700001)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700001));
                            }
                            else if (MyChar.skipdragongem && I.ID == 700011)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700011));
                            }
                            else if (MyChar.skipfurygem && I.ID == 700021)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700021));
                            }
                            else if (MyChar.skiprainbowgem && I.ID == 700031)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700031));
                            }
                            else if (MyChar.skipkylingem && I.ID == 700041)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700041));
                            }
                            else if (MyChar.skipvioletgem && I.ID == 700051)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700051));
                            }
                            else if (MyChar.skipmoongem && I.ID == 700061)
                            {
                                MyChar.RemoveItem(MyChar.NextItem(700061));
                            }
                            else
                            {
                                MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                            }
                            return;
                        }
                    }
                }
                if (MyMath.ChanceSuccess(DropRates.DragonBall))//Dragonball
                {
                    MyChar.AddItem(1088000);
                    World.SendMsgToAll("LUCKY", MyChar.Name + " has found a Dragonball while mining!", 2011, 0);
                    MyChar.MyClient.LocalMessage(2005, "You have gained a Dragonball!");
                    return;
                }
                if (Ore1 != 0 && MyMath.ChanceSuccess(25))//ores type 1
                {
                    if (Ore1 != 1072031) { Random rnd = new Random(); Ore1 += (uint)rnd.Next(0, 9); }
                    if (MyChar.VipLevel >= 3 && MyChar.VIPMiningSkipOres)
                    {
                        if (Ore1 != 1072031)
                        {
                            return;
                        }
                    }
                    if (Database.DatabaseItems.ContainsKey(Ore1))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        I.ID = Ore1;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                        return;
                    }
                }
                i = 0;
                if (DateTime.Now.Second == 10 || DateTime.Now.Second == 20 || DateTime.Now.Second == 30 || DateTime.Now.Minute % 2 == 1) // Added Gumpshot for eux ore
                {
                    if (MyMath.ChanceSuccess(33))
                    {
                        Ore2 = 1072031;
                    }

                }
                if (Ore2 == 1072031)
                    i += 0.55;
                if (Ore2 != 0 && MyMath.ChanceSuccess(0.45 + i))//ores type 2
                {
                    if (Ore2 != 1072031) { Random rnd = new Random(); Ore2 += (uint)rnd.Next(0, 9); }
                    if (MyChar.VipLevel >= 3 && MyChar.VIPMiningSkipOres)
                    {
                        if (Ore1 != 1072031)
                        {
                            return;
                        }
                    }
                    if (Database.DatabaseItems.ContainsKey(Ore2))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        I.ID = Ore2;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                        return;
                    }
                }
                if (Ore3 != 0 && MyMath.ChanceSuccess(10))//ores type 3
                {
                    if (Ore3 != 1072031) { Random rnd = new Random(); Ore3 += (uint)rnd.Next(0, 9); }
                    if (MyChar.VipLevel >= 3 && MyChar.VIPMiningSkipOres)
                    {
                        if (Ore1 != 1072031)
                        {
                            return;
                        }
                    }
                    if (Database.DatabaseItems.ContainsKey(Ore3))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        I.ID = Ore3;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                        return;
                    }
                }
                if (Ore4 != 0 && MyMath.ChanceSuccess(0.42))//ores type 4
                {
                    if (Ore4 != 1072031) { Random rnd = new Random(); Ore4 += (uint)rnd.Next(0, 9); }
                    if (MyChar.VipLevel >= 3 && MyChar.VIPMiningSkipOres)
                    {
                        if (Ore1 != 1072031)
                        {
                            return;
                        }
                    }
                    if (Database.DatabaseItems.ContainsKey(Ore4))
                    {
                        Item I = new Item();
                        I.UID = (uint)Rnd.Next(10000000);
                        I.ID = Ore4;
                        I.MaxDur = 1;
                        I.CurDur = 1;
                        MyChar.AddItem(I);
                        MyChar.MyClient.LocalMessage(2005, "You have gained a " + I.DBInfo.Name + ".");
                        return;
                    }
                }
            }
        }
    }
}
