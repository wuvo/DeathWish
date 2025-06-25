using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using System.Collections;

namespace Ultimate.Items
{
    public class Item_720123 : IItem
    {
        public override void Run(Character C, Item I)
        {
            Main.CryptoRandom Rnd = new Main.CryptoRandom();
            if (C.Inventory.Count > 35)
            {
                C.MyClient.LocalMessage(2005, "Please make some room in your inventory!");
            }
            else
            {
                C.RemoveItem(C.NextItem(I.ID));
                int x = C.Rnd.Next(1, 5);
                if (x == 1)
                {
                    if (MyMath.ChanceSuccess(99))
                    {
                        if (C.Level < 130)
                        {
                            C.AddExp(1);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received the experience equivalent to 1 ExpBall!");
                        }
                        else if (MyMath.ChanceSuccess(15))
                        {
                            C.AddItem(700002);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a ref PhoenixGem!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and ref PhoenixGem!", 2011, 0);
                        }
                        else if (MyMath.ChanceSuccess(50))
                        {
                            C.Silvers += 1000000;
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received 1,000,000 silvers!");
                        }
                        else
                        {
                            C.AddItem(720032);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a " + I.DBInfo.Name + "!");
                        }
                    }
                    else
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                    }
                }
                else if (x == 2)
                {
                    if (MyMath.ChanceSuccess(3))
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                    }
                    else if (MyMath.ChanceSuccess(15))
                    {
                        C.AddItem(700062);
                        C.MyClient.LocalMessage(2005, "Congratulations! You have received a ref MoonGem!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and ref MoonGem!", 2011, 0);
                    }
                    else
                    {
                        C.AddItem(700062);
                        C.MyClient.LocalMessage(2005, "Congratulations! You have received a ref MoonGem!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and ref MoonGem!", 2011, 0);
                    }
                }
                else if (x == 3)
                {
                    if (MyMath.ChanceSuccess(95))
                    {
                        C.VP += 1000;
                        C.AddItem(1088000);
                        if (C.Level < 130)
                            C.AddExp((1 / 2) + 1);

                        C.MyClient.LocalMessage(2005, "Congratulations you received 1,000 VirtuePoints, the experience equivalent to 1 ExpBall and some DragonBall!");
                    }
                    else if (MyMath.ChanceSuccess(15))
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                    }
                    else
                    {
                        C.VP += 2000;
                        C.AddItem(1088000);
                        if (C.Level < 130)
                            C.AddExp(1);

                        C.MyClient.LocalMessage(2005, "Congratulations you received 2,000 VirtuePoints, a LifeFruit and the experience equivalent to one Dragonball!");
                    }
                }
                else
                {
                    if (MyMath.ChanceSuccess(10))
                    {
                        {

                            top:
                            Item I2 = new Item();
                            I2.UID = (uint)Rnd.Next(10000000);
                            Item.ItemQuality Q = Item.ItemQuality.Normal;

                            uint ItemID = 0;
                            ArrayList From = new ArrayList();
                            int Type = Rnd.Next(1, 165);
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
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
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

                                C.AddItem(I2);


                            }
                            else goto top;

                            C.MyClient.LocalMessage(2005, "You received a random +1 non weapon item!");

                        }
                    }
                    else if (MyMath.ChanceSuccess(15))
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                    }
                    else if (MyMath.ChanceSuccess(15))
                    {
                        C.ExpPotionUsed = DateTime.Now;
                        C.DoubleExp = true;
                        C.DoubleExpLeft += 360;
                        C.MyClient.AddSend(Packets.Status(C.EntityID, Status.DoubleExpTime, (ulong)C.DoubleExpLeft));
                        C.MyClient.LocalMessage(2005, "Congratulations you received 10 minutes of Double Experience!");
                    }
                    else if (MyMath.ChanceSuccess(25))
                    {
                        C.LuckyTime += 300;
                        C.MyClient.AddSend(Packets.Status(C.EntityID, Status.LuckyTime, (ulong)C.LuckyTime));
                        C.AddItem(720031);
                        C.MyClient.LocalMessage(2005, "Congratulations you received 5 Minutes of better drops and Fireworks!");
                    }
                    else
                    {
                        C.AddItem(723726);
                        if (C.Level < 130)
                            C.AddExp(1 / 3);

                        C.MyClient.LocalMessage(2005, "Congratulations you received a LifeFruit and the experience equivalent to a third of an ExpBall!");
                    }
                }
            }
        }
    }
}