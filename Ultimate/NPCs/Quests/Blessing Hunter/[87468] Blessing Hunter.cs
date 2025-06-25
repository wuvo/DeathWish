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
    public class NPC_87468 : NPCBase
    {
        public NPC_87468(Main.GameClient _client)
            : base(_client)
        {
            ID = 87468;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Are you the Warrior that will help me? This is a hard journey I would advise you to get some friends.");
                    AddOption("Yes", 1);
                    AddOption("I have what you need, make sure u have 5 empty slots.", 10);
                    AddOption("Just passing by", 255);
                    break;
                case 10:
                  //  if (DateTime.Now.Month == 2 && DateTime.Now.Day >= 17 && DateTime.Now.Day < 20)
                    {
                        if (GC.MyChar.InventoryContains(722343, 1) && GC.MyChar.InventoryContains(722344, 1) && GC.MyChar.InventoryContains(722345, 1) && GC.MyChar.InventoryContains(722346, 1) && GC.MyChar.InventoryContains(722347, 1) && GC.MyChar.InventoryContains(722348, 1) && GC.MyChar.InventoryContains(722349, 1) && GC.MyChar.InventoryContains(722350, 1) && GC.MyChar.InventoryContains(722351, 1) && GC.MyChar.InventoryContains(722352, 1))
                        {
                            AddText("Wow! Thank you for doing this i will reward you handsomly!");
                            AddOption("Thank you so much", 2);
                        }
                    }
                    break;
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(722343, 1) && GC.MyChar.InventoryContains(722344, 1) && GC.MyChar.InventoryContains(722345, 1) && GC.MyChar.InventoryContains(722346, 1) && GC.MyChar.InventoryContains(722347, 1) && GC.MyChar.InventoryContains(722348, 1) && GC.MyChar.InventoryContains(722349, 1) && GC.MyChar.InventoryContains(722350, 1) && GC.MyChar.InventoryContains(722351, 1) && GC.MyChar.InventoryContains(722352, 1)) // Quest Reward Items
                        {
                            //if (GC.MyChar.Inventory.Count < 39)
                            //{
                            //    AddText("You need 1 free spots.");
                            //    break;
                            //}
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722343));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722344));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722345));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722346));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722347));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722348));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722349));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722350));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722351));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722352));

                            Random Rnd = new Random();
                            switch (Rnd.Next(0, 3))
                            {
                                
                                #region Random GEM
                                case 1:
                                    {
                                        List<uint> From = new List<uint>() { 700002, 700012, 700032, 700042, 700052, 700062, 700072 };
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        GC.MyChar.AddItem((uint)From[Tries]);
                                        GC.LocalMessage(2000, "You got a Refined gem.");
                                        break;
                                    }
                                #endregion
                                #region Met~Scroll
                                case 2:
                                    {
                                        GC.MyChar.AddItem(720027);
                                        GC.MyChar.AddItem(720027);
                                        GC.MyChar.AddItem(720027);
                                        GC.LocalMessage(2000, "You got a 3 meteor scroll.");
                                        break;
                                    }
                                #endregion
                                #region DB~Scroll
                                case 3:
                                    {
                                        GC.MyChar.AddItem(720028);
                                        GC.MyChar.AddItem(720028);
                                        GC.LocalMessage(2000, "You got a 2 DB scroll.");
                                        break;
                                    }
                                #endregion
                                #region Rare Garment
                                case 4:
                                    {
                                        List<uint> From = new List<uint>() { 192435, 192895, 192755, 188955, 189200 };// add here all your ids and remove the ones i added
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        GC.MyChar.AddItem((uint)From[Tries]);
                                        GC.LocalMessage(2000, "You got a rare garment.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has complete Blessing quest and received a Rare Garment!", 2011, 0);
                                        break;
                                    }
                                #endregion
                                #region AncientDemonBox
                                case 5:
                                    {
                                        GC.MyChar.AddItem(720651);
                                        GC.LocalMessage(2000, "You got AncientDemonBox.");
                                        break;
                                    }
                                    #endregion

                            }

                            //for (int i = 0; i < 3; i++)
                            //    GC.MyChar.AddItem(720027); // Meteorscroll
                            GC.MyChar.Silvers += 1000000;
                            AddText("Thank fo your help. You have got your rewards.");
                        }
                        else
                        {
                            AddText("You dont have the required items..");
                        }
                    }
                    break;

                case 1: // Sob story
                    {
                        AddText("My family has been fighting these terrible monsters for over 1000 years and we cannot banish them. My great great GrandFather was the first of us to try.");
                        AddOption("I see", 3);
                        AddOption("I dont want to listen to your sob story", 255);
                    }
                    break;

                case 3: // Sob Story Part II
                    {
                        AddText("Since then many more of us have tried to defeat these terrible monsters but no one has done it. Are you sure you want to help me?");
                        AddOption("Yes! I am sure!", 4);
                        AddOption("No way!", 255);
                    }
                    break;

                case 4: // Special Map teleport I
                    {
                        AddText("I am going to teleport you to a special map, These monsters are everywhere! You will need to collect 10 different items from these monsters to prove that you have defeated them.");
                        AddOption("Sure", 5);
                        AddOption("I am too scared i dont want to do this now!", 255);
                    }
                    break;

                case 5: // Special Map teleport II
                    {
                        GC.MyChar.Teleport(1762, 63, 258);
                        AddText("Ok, Please find me these items");
                        AddText("Blessing 1 - Blessing 10, These items will drop from the monsters, It is a rare chance to find them though.");
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
    
}
