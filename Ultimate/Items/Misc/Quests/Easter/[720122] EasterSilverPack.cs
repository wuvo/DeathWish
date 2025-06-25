using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720122 : IItem
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
                int x = C.Rnd.Next(1, 5);
                if (x == 1)
                {
                    if (MyMath.ChanceSuccess(95))
                    {
                        if (C.Level < 130)
                            C.AddExp(1 * 2);
                        else if (MyMath.ChanceSuccess(10))
                        {
                            C.AddItem(700032);
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received a ref RainbowGem!");
                            World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and ref RainbowGem!", 2011, 0);
                        }
                        else
                        {
                            C.Silvers += 250000;
                            C.MyClient.LocalMessage(2005, "Congratulations! You have received 250,000 silvers!");
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
                    else if (MyMath.ChanceSuccess(10))
                    {
                        C.AddItem(700012);
                        C.MyClient.LocalMessage(2005, "Congratulations! You have received a ref DragonGem!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and ref DragonGem!", 2011, 0);
                    }
                    else
                    {
                        for (int a = 0; a < 3; a++)
                            C.AddItem(1088001);
                        if (C.Level < 130)
                            C.AddExp(1);
                        C.MyClient.LocalMessage(2005, "Congratulations you received 3 Meteors and the experience equivalent to 1 ExpBall!");
                    }
                }
                else if (x == 3)
                {
                    if (MyMath.ChanceSuccess(90))
                    {
                        C.VP += 3000;
                        if (C.Level < 130)
                            C.AddExp((1 / 2) + 1);

                        C.MyClient.LocalMessage(2005, "Congratulations you received 3,000 VirtuePoints and experience equivalent to 1.5 ExpBalls!");
                    }
                    else if (MyMath.ChanceSuccess(10))
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations! You have received a DragonBall");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and DragonBall!", 2011, 0);

                    }
                    else
                    {
                        C.VP += 2000;
                        if (C.Level < 130)
                            C.AddExp(1);

                        C.MyClient.LocalMessage(2005, "Congratulations you received 2,000 VirtuePoints and the experience equivalent to one ExpBall!");
                    }
                }
                else
                {
                    if (MyMath.ChanceSuccess(3))
                    {
                        C.AddItem(1088000);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a DragonBall!");
                        World.SendMsgToAll("[EVENT]", "Lucky Player " + C.Name + " has opened a " + I.DBInfo.Name + " and received a DragonBall!", 2011, 0);
                    }
                    else if (MyMath.ChanceSuccess(5))
                    {
                        C.AddItem(720027);
                        C.MyClient.LocalMessage(2005, "Congratulations! You have received a MeteorScroll!");
                    }
                    else if (MyMath.ChanceSuccess(3))
                    {
                        C.AddItem(720027);
                        C.MyClient.LocalMessage(2005, "Congratulations you received a MeteorScroll!");
                    }
                    else
                    {
                        C.AddItem(722700);
                        if (C.Level < 130)
                            C.AddExp(1);

                        C.MyClient.LocalMessage(2005, "Congratulations you received a 10MinExpPotion and the experience equivalent to 1 ExpBall!");
                    }
                }
            }
        }
    }
}