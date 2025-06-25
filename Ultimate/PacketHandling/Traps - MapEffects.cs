using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using System.Collections.Concurrent;

namespace Ultimate.PacketHandling
{
    public class Traps
    {
        public static void Handle(Character C, MapEffect I)
        {
            if (I.Loc.Map == C.Loc.Map && I.Loc.X == C.Loc.X && I.Loc.Y == C.Loc.Y && I.Info.ID == 11)
            {
                if (C.Inventory.Count > 35)
                {
                    C.MyClient.LocalMessage(2000, "Please make some room in your inventory first!");
                }
                else
                {
                    I.Dissappear();
                    int a = Program.Rnd.Next(29);
                    switch (a)
                    {
                        #region Rewards
                        case 0:
                            {
                                C.Silvers += 5000;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 5,000 silvers!");
                                break;
                            }
                        case 1:
                            {
                                C.Silvers += 10000;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 10,000 silvers!");
                                break;
                            }
                        case 2:
                            {
                                C.Silvers += 25000;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 25,000 silvers!");
                                break;
                            }
                        case 3:
                            {
                                C.Silvers += 50000;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 50,000 silvers!");
                                break;
                            }
                        case 4:
                            {
                                C.Silvers += 75000;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 75,000 silvers!");
                                break;
                            }
                        case 5:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.Silvers += 3333;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 3,333 silvers!");
                                }
                                else
                                {
                                    C.Silvers += 100000;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 100,000 silvers!");
                                }
                                break;
                            }
                        case 6:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.Silvers += 6666;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 6,666 silvers!");
                                }
                                else
                                {
                                    C.Silvers += 125000;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 125,000 silvers!");
                                }
                                break;
                            }
                        case 7:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.Silvers += 11111;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 11,111 silvers!");
                                }
                                else
                                {
                                    C.Silvers += 150000;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 150,000 silvers!");
                                }
                                break;
                            }
                        case 8:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.Silvers += 33333;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 33,333 silvers!");
                                }
                                else
                                {
                                    C.Silvers += 200000;
                                    World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received 200,000 silvers!", 2011, 0);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 200,000 silvers!");
                                }
                                break;
                            }
                        case 9:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.Silvers += 66666;
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 66,666 silvers!");
                                }
                                else
                                {
                                    C.Silvers += 250000;
                                    World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received 250,000 silvers!", 2011, 0);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 250,000 silvers!");
                                }
                                break;
                            }
                        case 10:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    C.AddItem(1088001);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received a meteor!");
                                }
                                else
                                {
                                    C.Silvers += 300000;
                                    World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received 300,000 silvers!", 2011, 0);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 300,000 silvers!");
                                }
                                break;
                            }
                        case 11:
                            {
                                if (MyMath.ChanceSuccess(50))
                                {
                                    for (int q = 0; q < 3; q++)
                                        C.AddItem(1088001);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received three meteors!");
                                }
                                else
                                {
                                    C.Silvers += 500000;
                                    World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received 500,000 silvers!", 2011, 0);
                                    C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 500,000 silvers!");
                                }
                                break;
                            }
                        case 12:
                            {
                                C.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received a DragonBall!", 2011, 0);
                                break;
                            }
                        case 13:
                            {
                                C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received a meteor!");
                                break;
                            }
                        case 14:
                            {
                                for (int q = 0; q < 3; q++)
                                    C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received three meteors!");
                                break;
                            }
                        case 15:
                            {
                                for (int q = 0; q < 5; q++)
                                    C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received five meteors!");
                                break;
                            }
                        case 16:
                            {
                                C.AddItem(720027);
                                World.SendMsgToAll("SYSTEM", "Congratulations! " + C.Name + " has found a Squama and received a MeteorScroll!", 2011, 0);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received a MeteorScroll!");
                                break;
                            }
                        case 17:
                            {
                                C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received a meteor!");
                                break;
                            }
                        case 18:
                            {
                                C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received a meteor!");
                                break;
                            }
                        case 19:
                            {
                                for (int q = 0; q < 5; q++)
                                    C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received five meteors!");
                                break;
                            }
                        case 20:
                            {
                                for (int q = 0; q < 3; q++)
                                    C.AddItem(1088001);
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received three meteors!");
                                break;
                            }
                        case 21:
                            {
                                C.Silvers += 3333;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 3,333 silvers!");
                                break;
                            }
                        case 22:
                            {
                                C.Silvers += 6666;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 6,666 silvers!");
                                break;
                            }
                        case 23:
                            {
                                C.Silvers += 9999;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 9,999 silvers!");
                                break;
                            }
                        case 24:
                            {
                                C.Silvers += 16666;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 16,666 silvers!");
                                break;
                            }
                        case 25:
                            {
                                C.Silvers += 7777;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 7,777 silvers!");
                                break;
                            }
                        case 26:
                            {
                                C.Silvers += 22222;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 22,222 silvers!");
                                break;
                            }
                        case 27:
                            {
                                C.Silvers += 33333;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 33,333 silvers!");
                                break;
                            }
                        case 28:
                            {
                                C.Silvers += 66666;
                                C.MyClient.LocalMessage(2001, "Congratulations! You have found a Squama and received 66,666 silvers!");
                                break;
                            }
                            #endregion
                    }
                }
            }
        }
    }
}
