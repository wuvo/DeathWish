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
    public class NPC_2000 : NPCBase
    {
        public NPC_2000(Main.GameClient _client)
            : base(_client)
        {
            ID = 2000;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if ((DateTime.Now.Month == 3 && DateTime.Now.Day >= 17) || (DateTime.Now.Month == 4 && DateTime.Now.Day <= 3))
                        {
                            if (GC.MyChar.InventoryContains(729922, 1) &&
                                GC.MyChar.InventoryContains(729923, 1) &&
                                GC.MyChar.InventoryContains(729924, 1) &&
                                GC.MyChar.InventoryContains(729925, 1) &&
                                GC.MyChar.InventoryContains(729926, 1))
                            {
                                if (GC.MyChar.Inventory.Count < 39)
                                {
                                    #region Rewards
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(729922));
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(729923));
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(729924));
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(729925));
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(729926));
                                    if (GC.MyChar.InventoryContains(729921))
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(729921));
                                    if (GC.MyChar.Level < 70)
                                    {
                                        if (MyMath.ChanceSuccess(10))
                                        {
                                            GC.MyChar.AddItem(1088000);
                                            GC.LocalMessage(2005, "You has received a Dragonball!");
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the egg fragments and received a DragonBall from Commander!", 2011, 0);
                                        }
                                        else if (MyMath.ChanceSuccess(10))
                                        {
                                            for (int a = 0; a < 3; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.LocalMessage(2005, "You has received 3 Meteors!");
                                        }
                                        else if (MyMath.ChanceSuccess(40))
                                        {
                                            GC.MyChar.AddExp(1 * 2);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 2 ExpBalls!");
                                        }
                                        else if (MyMath.ChanceSuccess(20))
                                        {
                                            for (int a = 0; a < 2; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.MyChar.AddExp(1);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1 ExpBall and 2 Meteors!");
                                        }
                                        else
                                        {
                                            GC.MyChar.AddExp(1);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1 ExpBall!");
                                        }
                                    }
                                    else if (GC.MyChar.Level >= 70 && GC.MyChar.Level < 110)
                                    {
                                        if (MyMath.ChanceSuccess(8))
                                        {
                                            GC.MyChar.AddItem(1088000);
                                            GC.LocalMessage(2005, "You has received a Dragonball!");
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the egg fragments and received a DragonBall from Commander!", 2011, 0);
                                        }
                                        else if (MyMath.ChanceSuccess(30))
                                        {
                                            for (int a = 0; a < 3; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.LocalMessage(2005, "You have received 3 Meteors!");
                                        }
                                        else if (MyMath.ChanceSuccess(40))
                                        {
                                            GC.MyChar.AddExp(1 * 2);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 2 ExpBalls!");
                                        }
                                        else if (MyMath.ChanceSuccess(20))
                                        {
                                            for (int a = 0; a < 2; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.MyChar.AddExp(1);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1 ExpBall and 2 Meteors!");
                                        }
                                        else if (MyMath.ChanceSuccess(30))
                                            PlusItemReward(1, 1);

                                        else if (MyMath.ChanceSuccess(5))
                                        {
                                            GC.MyChar.AddItem(721170);
                                            GC.LocalMessage(2005, "You have received a HousePermit!");
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the egg fragments and received a HousePermit from Commander!", 2011, 0);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddExp(1);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1 ExpBall!");
                                        }
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(1))
                                        {
                                            GC.MyChar.AddItem(1088000);
                                            GC.LocalMessage(2005, "You have received a Dragonball!");
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the egg fragments and received a DragonBall from Commander!", 2011, 0);
                                        }
                                        else if (MyMath.ChanceSuccess(30))
                                        {
                                            for (int a = 0; a < 3; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.LocalMessage(2005, "You have received 3 Meteors!");
                                        }
                                        else if (MyMath.ChanceSuccess(40) && GC.MyChar.Level < 130)
                                        {
                                            GC.MyChar.AddExp(1);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1 ExpBalls!");
                                        }
                                        else if (MyMath.ChanceSuccess(20) && GC.MyChar.Level < 130)
                                        {
                                            for (int a = 0; a < 2; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.MyChar.AddExp(1.5);
                                            GC.LocalMessage(2005, "You have received the experience equivalent to 1.5 ExpBalls and 2 Meteors!");
                                        }
                                        else if (MyMath.ChanceSuccess(20))
                                        {
                                            GC.MyChar.AddItem(721261);
                                            GC.LocalMessage(2005, "You have received a Bomb!");
                                        }
                                        else if (MyMath.ChanceSuccess(15))
                                        {
                                            GC.MyChar.AddItem(721541);
                                            GC.LocalMessage(2005, "You have received a SunBox!");
                                        }
                                        else if (MyMath.ChanceSuccess(20))
                                        {
                                            GC.MyChar.AddItem(721246);
                                            GC.LocalMessage(2005, "You have received a CCGWBomb!");
                                        }
                                        else if (MyMath.ChanceSuccess(15))
                                            PlusItemReward(1, 2);
                                        else if (MyMath.ChanceSuccess(5))
                                        {
                                            GC.MyChar.AddItem(721170);
                                            GC.LocalMessage(2005, "You have received a HousePermit!");
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the egg fragments and received a HousePermit from Commander!", 2011, 0);
                                        }
                                        else if (MyMath.ChanceSuccess(20))
                                        {
                                            for (int a = 0; a < 2; a++)
                                                GC.MyChar.AddItem(1088001);
                                            GC.LocalMessage(2005, "You have received 2 Meteors!");
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(722700);
                                            GC.LocalMessage(2005, "You have received a 10 Min ExpPotion!");
                                        }
                                    }
                                    #endregion
                                    AddText("Seems like you've found all the egg fragments! Here's your reward!");
                                    AddOption("Thanks", 255);
                                }
                                else
                                {
                                    AddText("Please make some room in your inventory first!");
                                    AddOption("Alright", 255);
                                }
                            }
                            else
                            {
                                AddText("Help! Hey, if you have some free time, I'll reward you? My precious EpicEgg was broken into fragments and scattered across the land upon my recent trip to Twin City. ");
                                AddText(" A Demon with incredible power shot with me with a bow, but I was protected by the Egg. It split into five fragments and one landed in each city. Just don't tell the Easter Bunny...");
                                AddOption("Tell me more about it", 1);
                                AddOption("Yeah right...", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like the Easter Quest already ended! Please come back next year!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Oh, Well lets just say, I'll give you whatever is in my bag! Meteors, DragonBalls, +n Items, my precious garments... What ever it takes.");
                        AddText(" All of it just to make sure that we get those Eggs safe without Easter Bunny knowing about it!");
                        AddOption("Alright I'll help you", 2);
                        AddOption("I don't have time for that", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Think about it, I'd go but my leg got shot with an arrow... looking foward to seeing you again!");
                        AddText(" You just need to go to each city and look for the Easter Eggs! They contain fragments to make the ultimate EpicEgg!");
                        AddOption("Let's save Easter then", 3);
                        AddOption("Maybe later", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(729921, 1))
                        {
                            AddText("It seems like I already gave you a bag were you can keep the eggs fragments! What ");
                            AddText("are you still doing here? The damn rabbit is going to know I lost the eggs!");
                            AddOption("I'm on my way!", 255);
                        }
                        else if (GC.MyChar.Inventory.Count < 39)
                        {
                            GC.MyChar.AddItem(729921);
                            AddText("Off you go! Make sure you come back in time with those Eggs so that the Bunny won't notice! Take this Feed with you ");
                            AddText("It will help you grabbing the Eggs! Some crazy guys told me they moved!");
                            AddOption("Let's go then!", 255);
                        }
                        else
                        {
                            AddText("Please make up some room in your inventory first! I need to give you something!");
                            AddOption("Alright", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}