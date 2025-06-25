using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721542 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 37)
            {
                int j = 1;
                if (MyMath.ChanceSuccess(30))
                    j = 2;

                C.RemoveItem(I.UID);

                for (int l = 0; l < j; l++)
                {
                    Item Luck = new Item();
                    if (MyMath.ChanceSuccess(7))
                        Luck.ID = 720027;
                    else if (MyMath.ChanceSuccess(3.5))
                    {
                        Luck.ID = 1088000;
                        World.SendMsgToAll("LUCKY", C.Name + " has opened a WaningMoonBox and received a DragonBall!", 2005, 0);
                    }
                    else if (MyMath.ChanceSuccess(0.7))
                    {
                        Luck.ID = 1088000;
                        C.AddItem(1088000);
                        World.SendMsgToAll("LUCKY", C.Name + " has opened a WaningMoonBox and received 2 DragonBalls!", 2005, 0);
                    }
                    else
                        Luck.ID = 1088001;
                    C.AddItem(Luck);
                }

                if (MyMath.ChanceSuccess(90))
                {
                    Item I2 = new Item();
                    I2.UID = (uint)C.Rnd.Next(10000000);
                    Item.ItemQuality Q = Item.ItemQuality.Refined;
                    if (MyMath.ChanceSuccess(4.5))
                        Q = Item.ItemQuality.Super;
                    else if (MyMath.ChanceSuccess(9))
                        Q = Item.ItemQuality.Elite;
                    else if (MyMath.ChanceSuccess(25))
                        Q = Item.ItemQuality.Unique;

                    uint ItemID = 0;
                    List<uint> From = new List<uint>();
                    int Type = C.Rnd.Next(0, 340);
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
                    else if (Type < 175) Part = 410;
                    else if (Type < 185) Part = 420;
                    else if (Type < 195) Part = 421;
                    else if (Type < 205) Part = 430;
                    else if (Type < 215) Part = 440;
                    else if (Type < 225) Part = 450;
                    else if (Type < 235) Part = 460;
                    else if (Type < 245) Part = 480;
                    else if (Type < 255) Part = 481;
                    else if (Type < 265) Part = 490;
                    else if (Type < 275) Part = 500;
                    else if (Type < 285) Part = 510;
                    else if (Type < 295) Part = 530;
                    else if (Type < 305) Part = 540;
                    else if (Type < 315) Part = 560;
                    else if (Type < 325) Part = 561;
                    else if (Type < 335) Part = 580;
                    else if (Type < 340) Part = 900;

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
                            byte Tries = (byte)C.Rnd.Next(0, From.Count);
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

                        if (ItemIDManipulation.Digit(I2.ID, 1) == 4 || ItemIDManipulation.Digit(I2.ID, 1) == 5)
                        {
                            if (MyMath.ChanceSuccess(DropRates.OneSoc + 3))
                                I2.Soc1 = Item.Gem.EmptySocket;
                            if (MyMath.ChanceSuccess(DropRates.TwoSoc + 1))
                            {
                                I2.Soc1 = Item.Gem.EmptySocket;
                                I2.Soc2 = Item.Gem.EmptySocket;
                            }
                        }
                        else if (ItemIDManipulation.Digit(I2.ID, 1) == 1 || ItemIDManipulation.Digit(I2.ID, 1) == 2 || ItemIDManipulation.Digit(I2.ID, 1) == 3 || ItemIDManipulation.Digit(I2.ID, 1) == 6 || ItemIDManipulation.Digit(I2.ID, 1) == 8)
                        {
                            if (MyMath.ChanceSuccess(1.5))
                            {
                                I2.Soc1 = Item.Gem.EmptySocket;
                                World.SendMsgToAll("LUCKY", C.Name + " has opened a WaningMoonBox and received a socket " + I2.DBInfo.Name + "!", 2011, 0);
                                World.DebugAdd += C.Name + " obtained socket " + I2.DBInfo.Name + " from a WaningMoonBox! \r\n";
                            }
                        }
                        if (MyMath.ChanceSuccess(DropRates.PlusOne + 3.5))
                        {
                            I2.Plus = 1;
                            if (MyMath.ChanceSuccess(3.5))
                                I2.Plus = 2;
                        }
                        I2.MaxDur = I2.DBInfo.Durability;
                        I2.CurDur = I2.MaxDur;

                        C.AddItem(I2);

                    }
                }
                C.MyClient.LocalMessage(2005, "You have successfuly opened the WaningMoonBox! Check your inventory!");
            }
            else
                C.MyClient.LocalMessage(2005, "Please clear room in your inventory! You need at least 3 spaces!");
        }
    }
}