using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [3002] Shirley
    /// </summary>
    public class NPC_3002 : NPCBase
    {
        public NPC_3002(Main.GameClient _client)
            : base(_client)
        {
            ID = 3002;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Hurray ! Ultimate Conquer is celebrating its anniversary ! Aren't you happy? I'm helding a party but I need some letters to decorate my house !\n");
                    AddText("Go out there and find me enough letters to write Ultimate and I will reward you some goodies !");
                    AddOption("I have the letters", 1);
                    AddOption("Just passing by", 255);
                    break;
                case 1:
                   if (DateTime.Now.Month == 2 && DateTime.Now.Day >= 17 && DateTime.Now.Day < 20)
                    {
                        if (GC.MyChar.InventoryContains(711210, 1) && GC.MyChar.InventoryContains(711211, 1) && GC.MyChar.InventoryContains(711212, 3) && GC.MyChar.InventoryContains(711213, 2) && GC.MyChar.InventoryContains(711214, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(711210));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(711211));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(711214));

                            for (int a = 0; a < 3; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(711212));
                            for (int a = 0; a < 2; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(711213));

                            if (Features.Anniversary.AnniversaryQuest.ContainsKey(GC.MyChar.Name))
                                Features.Anniversary.AnniversaryQuest[GC.MyChar.Name]++;
                            else
                                Features.Anniversary.AnniversaryQuest.Add(GC.MyChar.Name, 1);

                            AddText($"Hurray! You have delivered all the letters and received your reward! Check your ranking at the Anniversary Quest Ranking and see if you can get that amazing UltimateSpirit!");
                            AddOption("Thanks", 255);

                            Random Rnd = new Random();
                            switch (Rnd.Next(0, 31))
                            {
                                case 0:
                                    GC.MyChar.AddItem(720650);
                                    break;
                                case 1:
                                    GC.MyChar.AddItem(722700);
                                    break;
                                case 2:
                                    GC.MyChar.AddItem(720651);
                                    break;
                                case 3:
                                    GC.MyChar.AddItem(720654);
                                    break;
                                case 4:
                                    if (GC.MyChar.Level < 130)
                                        GC.MyChar.AddExp(2);
                                    else
                                        GC.MyChar.AddItem(720670);
                                    break;
                                case 5:
                                    GC.MyChar.AddItem(721541);
                                    break;
                                case 6:
                                    GC.MyChar.AddItem(721542);
                                    break;
                                case 7:
                                    GC.MyChar.AddItem(721543);
                                    break;
                                case 8:
                                    GC.MyChar.AddItem(721544);
                                    break;
                                case 9:
                                    GC.MyChar.AddItem(722384);
                                    break;
                                case 10:
                                    GC.MyChar.AddItem(720664);
                                    break;
                                case 11:
                                    GC.MyChar.AddItem(720658);
                                    break;
                                case 12:
                                    GC.MyChar.AddItem(721261);
                                    break;
                                case 13:
                                    GC.MyChar.AddItem(1088000);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has found all the letters for Shirley and received a DragonBall in return!", 2011, 0);
                                    break;
                                case 14:
                                    GC.MyChar.AddItem(723017);
                                    break;
                                case 15:
                                    GC.MyChar.AddItem(720027);
                                    break;
                                case 16:
                                    for (int a = 0; a < 5; a++)
                                        GC.MyChar.AddItem(1088001);
                                    break;
                                case 17:
                                    GC.MyChar.AddItem(722114);
                                    break;
                                case 18:
                                    GC.MyChar.AddItem(722107);
                                    break;
                                case 19:
                                    GC.MyChar.AddItem(722108);
                                    break;
                                case 20:
                                    GC.MyChar.AddItem(722109);
                                    break;
                                case 21:
                                    GC.MyChar.AddItem(722110);
                                    break;
                                case 22:
                                    GC.MyChar.AddItem(722111);
                                    break;
                                case 23:
                                    GC.MyChar.AddItem(722113);
                                    break;
                                case 24:
                                    if (GC.MyChar.Level < 130)
                                        GC.MyChar.AddExp(1);
                                    else
                                        GC.MyChar.AddItem(720658);
                                    break;
                                case 25:
                                    if (GC.MyChar.Level < 130)
                                        GC.MyChar.AddExp(3);
                                    else
                                        GC.MyChar.AddItem(720664);
                                    break;
                                case 26:
                                    if (GC.MyChar.Level < 130)
                                        GC.MyChar.AddExp(4);
                                    else
                                        GC.MyChar.AddItem(720658);
                                    break;
                                case 27:
                                    if (GC.MyChar.Level < 130)
                                        GC.MyChar.AddExp(5);
                                    else
                                        GC.MyChar.AddItem(720664);
                                    break;
                                case 28:
                                    GC.MyChar.VotePoints++;
                                    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!");
                                    break;
                                case 29:
                                    GC.MyChar.AddItem(721954);
                                    break;
                                case 30:
                                    GC.MyChar.AddItem(721246);
                                    break;
                            }
                        }
                        else
                        {
                            AddText("You don't have enough letters to write down our server's name! Go find some for me!");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("I'm sorry but I already have enough letters! Make sure you don't miss it next year!");
                        AddOption("Alright", 255);
                    }
                    break;

            }

            AddFinish();
            Send();
        }
    }
    
}
