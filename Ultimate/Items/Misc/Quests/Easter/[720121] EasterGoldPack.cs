using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using System.Collections;

namespace Ultimate.Items
{
    public class Item_720121 : IItem
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
                int x = C.Rnd.Next(1, 6);
                switch (x)
                {
                    case 1:
                        if (MyMath.ChanceSuccess(90))
                        {
                            if (C.Level < 130)
                            {
                                C.AddExp(1 * 5);
                                C.MyClient.LocalMessage(2005, "Congratulations you received the experience equivalent to 5 ExpBalls!");
                            }
                            else if (MyMath.ChanceSuccess(3))
                            {
                                C.AddItem(1088000);
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received a DragonBall!");
                            }
                            else
                            {
                                C.Silvers += 200000;
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received 200,000 silvers!");
                            }
                        }
                        else
                        {
                            C.AddItem(720027);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a MeteorScroll!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a MeteorScroll!", 2011, 0);
                        }
                        break;
                    case 2:
                        if (MyMath.ChanceSuccess(5))
                        {
                            C.AddItem(720027);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a MeteorScroll!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a MeteorScroll!", 2011, 0);
                        }
                        else if (MyMath.ChanceSuccess(3))
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a DragonBall!");
                        }
                        else
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

                                        I2.Plus = 2;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                       C.AddItem(I2);


                                    }
                                    else goto top;

                            C.MyClient.LocalMessage(2005, "You received a random +1 non weapon item!");

                        }
                        break;
                    case 3:
                        if (MyMath.ChanceSuccess(50))
                        {
                            C.VP += 5000;
                            C.AddItem(1088000);

                            C.MyClient.LocalMessage(2005, "Congratulations you received 5,000 VirtuePoints and experience equivalent to 2 ExpBalls!");
                        }
                        else if (MyMath.ChanceSuccess(3))
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a DragonBall!");
                        }
                        else
                        {
                            C.VP += 2500;
                            if (C.Level < 130)
                                C.AddExp(1 * 1);

                            C.MyClient.LocalMessage(2005, "Congratulations you received 2,500 VirtuePoints and  a DragonBall!");
                        }
                        break;
                    case 4:
                        if (MyMath.ChanceSuccess(5))
                        {
                            C.AddItem(720028);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DBScroll!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DBScroll!", 2011, 0);
                        }
                        else if (MyMath.ChanceSuccess(3))
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a DragonBall!");
                        }
                        else if (MyMath.ChanceSuccess(15))
                        {
                            for (int a = 0; a < 5; a++)
                                C.AddItem(1088001);
                            C.MyClient.LocalMessage(2005, "Congratulations you received 5 Meteors!");
                        }
                        else
                        {
                            C.AddItem(722700);
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a 10MinExpPotion and  a DragonBall!");
                        }
                        break;
                    case 5:
                        if (MyMath.ChanceSuccess(10))
                        {
                            C.AddItem(720650);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a DemonBox!");
                        }
                        else if (MyMath.ChanceSuccess(10))
                        {
                            C.AddItem(720651);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received an AncientDemonBox!");
                        }
                        else if (MyMath.ChanceSuccess(3))
                        {
                            if (C.VipLevel == 1)
                                C.VIPDays /= 5;
                            else if (C.VipLevel == 2)
                                C.VIPDays /= 4;
                            else if (C.VipLevel == 3)
                                C.VIPDays /= 3;
                            else if (C.VipLevel == 4)
                                C.VIPDays /= 2;
                            C.VotePoints -= 4;
                            if (DateTime.Now > C.VIPStarted.AddHours(24) || C.VIPDays == 0)
                                C.VIPStarted = DateTime.Now;
                            C.VipLevel = 5;
                            C.VIPDays += 1;
                            C.MyClient.LocalMessage(2005, "Congratulations! You are now VIP for 1 Day!");
                        }
                        else
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
                        break;
                }
            }
        }
    }
}