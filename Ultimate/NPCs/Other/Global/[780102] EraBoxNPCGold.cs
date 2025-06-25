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
    public class NPC_780102 : NPCBase
    {
        public NPC_780102(Main.GameClient _client)
            : base(_client)
        {
            ID = 780102;
            Face = 1;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to open EraBox[Gold] with you Key ?");
                        AddOption("Yes! open the Box please !", 1);
                        AddOption("I'll keep it", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721876, 1) && GC.MyChar.Inventory.Count <= 36)
                        {
                            int j = 1;
                            if (MyMath.ChanceSuccess(30))
                                j = 2;

                            //  GC.MyChar.RemoveItem(721750);
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721619));//EraGoldBox
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721876));//KEY

                            for (int l = 0; l < j; l++)
                            {
                                Item Luck = new Item();
                                if (MyMath.ChanceSuccess(10))
                                    Luck.ID = 720027;
                                else if (MyMath.ChanceSuccess(55))
                                {
                                    Luck.ID = 1088000;
                                    World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has opened an EraBox[Gold] and received a DragonBall!", 2005, 0);
                                }
                                else if (MyMath.ChanceSuccess(20))
                                {
                                    Luck.ID = 720028;
                                    World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has opened an EraBox[Gold] and received a DBScroll!", 2005, 0);
                                }
                                else if (MyMath.ChanceSuccess(100))
                                    Luck.ID = 1088001;
                                GC.MyChar.AddItem(Luck);
                            }

                            if (MyMath.ChanceSuccess(1))
                            {
                                GC.MyChar.AddItem(723584);
                            }

                            if (MyMath.ChanceSuccess(100))
                            {
                                Item I2 = new Item();
                                I2.UID = (uint)GC.MyChar.Rnd.Next(10000000);
                                Item.ItemQuality Q = Item.ItemQuality.Refined;
                                if (MyMath.ChanceSuccess(15))
                                    Q = Item.ItemQuality.Super;
                                else if (MyMath.ChanceSuccess(20))
                                    Q = Item.ItemQuality.Elite;
                                else if (MyMath.ChanceSuccess(50))
                                    Q = Item.ItemQuality.Unique;

                                uint ItemID = 255;
                                List<uint> From = new List<uint>();
                                int Type = GC.MyChar.Rnd.Next(255, 340);
                                uint Part = 255;
                                //if (Type < 10) Part = 111;
                                //else if (Type < 20) Part = 113;
                                //else if (Type < 30) Part = 114;
                                //else if (Type < 40) Part = 117;
                                //else if (Type < 50) Part = 118;
                                //else if (Type < 60) Part = 120;
                                //else if (Type < 70) Part = 121;
                                //else if (Type < 80) Part = 130;
                                //else if (Type < 90) Part = 131;
                                //else if (Type < 100) Part = 133;
                                //else if (Type < 110) Part = 134;
                                //else if (Type < 120) Part = 141;
                                ///////////////////////////////////
                                //if (Type < 130) Part = 142;
                                //else if (Type < 140) Part = 150;
                                //else if (Type < 150) Part = 151;
                                //else if (Type < 160) Part = 152;
                                //else if (Type < 165) Part = 160;
                                //else if (Type < 175) Part = 410;
                                //else if (Type < 185) Part = 420;
                                //else if (Type < 195) Part = 421;
                                //else if (Type < 205) Part = 430;
                                //else if (Type < 215) Part = 440;
                                //else if (Type < 225) Part = 450;
                                //else if (Type < 235) Part = 460;
                                //else if (Type < 245) Part = 480;
                                //////////////////////////////////////
                                 if (Type < 255) Part = 481;
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
                                        byte Tries = (byte)GC.MyChar.Rnd.Next(0, From.Count);
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
                                        if (MyMath.ChanceSuccess(5))
                                        {
                                            I2.Soc1 = Item.Gem.EmptySocket;
                                            World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has opened an EraBox[Gold] and received a socket " + I2.DBInfo.Name + "!", 2011, 0);
                                            World.DebugAdd += GC.MyChar.Name + " obtained socket " + I2.DBInfo.Name + " from an EraBox[Gold]! \r\n";
                                        }
                                    }
                                    if (MyMath.ChanceSuccess(DropRates.PlusOne + 4.5))
                                    {
                                        I2.Plus = 1;
                                        if (MyMath.ChanceSuccess(6.5))
                                            I2.Plus = 2;
                                    }
                                    I2.MaxDur = I2.DBInfo.Durability;
                                    I2.CurDur = I2.MaxDur;

                                    GC.MyChar.AddItem(I2);

                                }
                            }
                            GC.MyChar.MyClient.LocalMessage(2005, "You have successfuly opened the EraBox[Gold]! Check your inventory!");
                        }
                        else
                        {
                            GC.MyChar.MyClient.LocalMessage(2005, "Unable to open EraBox[Gold] . Either you don't have EraKey or you don't have at least 3 spaces in your inventory!");
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}