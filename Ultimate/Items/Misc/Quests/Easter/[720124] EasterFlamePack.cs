using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720124 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count > 35)
            {
                C.MyClient.LocalMessage(2005, "Please make some room in your inventory!");
            }
            else
            {
                C.RemoveItem(C.NextItem(I.ID));
                int x = C.Rnd.Next(1, 7);
                switch (x)
                {
                    case 1:
                        if (MyMath.ChanceSuccess(90))
                        {
                            if (C.Level < 130)
                            {
                                C.Silvers += 1000000;
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received 1,000,000 silvers!");
                            }
                            else if (MyMath.ChanceSuccess(5))
                            {
                                C.AddItem(721258);
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received a CleanWater!");
                            }
                            else
                            {
                                C.Silvers += 500000;
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received 500,000 silvers!");
                            }
                        }
                        else
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                        }
                        break;

                    case 2:
                        if (MyMath.ChanceSuccess(90))
                        {
                            if (C.Level < 130)
                            {
                                C.AddItem(1088000);
                                C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                                World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                            }
                            else if (MyMath.ChanceSuccess(5))
                            {
                                C.AddItem(721258);
                                C.MyClient.LocalMessage(2005, "Congratulations! You have received a CleanWater!");
                            }
                            else
                            {
                                C.AddItem(1088000);
                                C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                                World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                            }
                        }
                        else
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                        }
                        break;

                    case 3:
                        if (C.Level < 130)
                        {
                            C.ExpPotionUsed = DateTime.Now;
                            C.DoubleExp = true;
                            C.DoubleExpLeft = 3600;
                            C.MyClient.AddSend(Packets.Status(C.EntityID, Status.DoubleExpTime, (ulong)C.DoubleExpLeft));
                            C.MyClient.LocalMessage(2005, "Congratulations you received one hour of Double Experience!");
                        }
                        else if (MyMath.ChanceSuccess(5))
                        {
                            C.AddItem(721954);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a Transformation Candy!");
                        }
                        else
                        {
                            for (int a = 0; a < 1; a++)
                                C.AddItem(721170);

                            C.MyClient.LocalMessage(2005, "Congratulations! You have received HousePermit!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a HousePermit!", 2011, 0);
                        }
                        break;
                    case 4:
                        if (MyMath.ChanceSuccess(10))
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                        }
                        else if (MyMath.ChanceSuccess(5))
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                        }
                        else if (MyMath.ChanceSuccess(10))
                        {
                            for (int a = 0; a < 5; a++)
                                C.AddItem(720027);
                            C.MyClient.LocalMessage(2005, "Congratulations you received 5 MeteorScrolls!");
                        }
                        else
                        {
                            if (C.Level < 130)
                            {
                                for (int a = 0; a < 5; a++)
                                    C.AddItem(720027);
                                C.MyClient.LocalMessage(2005, "Congratulations you received 5 MeteorScrolls!");
                            }
                            else
                            {
                                C.AddItem(1088000);
                                C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                                World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                            }
                        }
                        break;
                    case 5:
                        if (MyMath.ChanceSuccess(20))
                        {
                            C.AddItem(720650);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a DemonBox!");
                        }
                        else if (MyMath.ChanceSuccess(20))
                        {
                            C.AddItem(720651);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received an AncientDemonBox!");
                        }
                        else
                        {
                            C.AddItem(1088000);
                            C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                        }
                        break;
                    case 6:
                        if (MyMath.ChanceSuccess(30))
                        {
                            C.AddItem(722384);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a ProficiencyToken!");
                        }
                        else if (MyMath.ChanceSuccess(30))
                        {
                            C.AddItem(721246);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a CCGWBomb!");
                        }
                        else if (MyMath.ChanceSuccess(30))
                        {
                            C.AddItem(721261);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a Bomb!");
                        }
                        break;
                }
            }
        }
    }
}