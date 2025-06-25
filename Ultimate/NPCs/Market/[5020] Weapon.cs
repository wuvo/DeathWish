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

    public class NPC_5020 : NPCBase
    {

        public NPC_5020(Main.GameClient _client)
                : base(_client)
        {
            //12 and 108
            ID = 5020;
            Face = 112;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {

            Responses = new List<COPacket>();
            AddAvatar();
            try
            {
                switch (_linkback)
                {
                    case 0:
                        AddText("What to do if you don't have an accessory ticket ?\n");
                        AddText("1-Buy Accessory Ticket from Our web page.\n");
                        AddText("2-Choose a Accessory from Our Accessory Page\n");
                        AddText("3-Choose whichever weapon you want to change and continue.\n");
                        AddOption("I have an accessory ticket", 8);
                        //AddOption("Show Accessories", 9);
                        //AddOption("Store Page", 10);
                        AddOption("Try Accessories", 35);
                        AddOption("Thanks", 255);

                        break;

                    case 35:
                        GC.MyChar.MyClient.DialogNPC = 5018;
                        NPCs.NPCHandler.Handle(GC.MyChar.MyClient, null, 5018, 0);
                        break;

                    case 8:

                        AddText("which weapon do you want to change?");
                        AddOption("Club", 1);
                        AddOption("Sword", 2);
                        AddOption("Blade", 3);
                        AddOption("Dagger", 4);
                        AddOption("BackSword", 5);
                        AddOption("Bow", 6);
                        AddOption("Spear", 7);
                        AddOption("Thanks", 255);
                        break;


                    case 1:
                        AddText("You can choose any of the following Accessories");
                        AddOption("FreezeClub", 11);
                        AddOption("GodOfClubs", 12);
                        AddOption("LongestClub", 13);
                        AddOption("Thanks", 255);
                        break;

                    case 2:
                        AddText("You can choose any of the following Accessories");
                        AddOption("DemonSword", 14);
                        AddOption("BuriedSword", 15);
                        AddOption("MeteorSword", 16);
                        AddOption("Thanks", 255);
                        break;

                    case 3:
                        AddText("You can choose any of the following Accessories");
                        AddOption("FreezeBlade", 17);
                        AddOption("DragonBlade", 18);
                        AddOption("FlatBlade", 19);
                        AddOption("GodsBlade", 20);
                        AddOption("Thanks", 255);
                        break;

                    case 4:
                        AddText("You can choose any of the following Accessories");
                        AddOption("GoldenDagger", 21);
                        AddOption("Thanks", 255);
                        break;

                    case 5:
                        AddText("You can choose any of the following Accessories");
                        AddOption("BrightEarth", 22);
                        AddOption("MagicHammer", 23);
                        AddOption("MagicSword", 24);
                        AddOption("Thanks", 255);
                        break;


                    case 6:
                        AddText("You can choose any of the following Accessories");
                        AddOption("SwipeBow", 25);
                        AddOption("FireBow", 26);
                        AddOption("GodsBow", 27);
                        AddOption("EnchantedBow", 28);
                        AddOption("Thanks", 255);
                        break;

                    case 7:
                        AddText("You can choose any of the following Accessories");
                        AddOption("PoseidonSpear", 29);
                        AddOption("XerxesSpear", 30);
                        AddOption("Thanks", 255);
                        break;

                    case 9:
                        GC.LocalMessage(2105, "https://www.Ultimateconquer.com/Ultimate/accessories/");
                        break;

                    case 10:
                        GC.LocalMessage(2105, "http://www.Ultimateconquer.com/Ultimate/Store");
                        break;

                    #region Club
                    case 11:
                        if (GC.MyChar.InventoryContains(720177, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 480))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 480339 || GC.MyChar.Equips.RightHand.ID == 480349 || GC.MyChar.Equips.RightHand.ID == 480359 || GC.MyChar.Equips.RightHand.ID == 480369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720177));
                                    GC.MyChar.Equips.RightHand.ID = 480349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your club needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }

                            }
                            else
                            {
                                AddText("Sorry you dont wear a club on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 12:
                        if (GC.MyChar.InventoryContains(720178, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 480))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 480339 || GC.MyChar.Equips.RightHand.ID == 480349 || GC.MyChar.Equips.RightHand.ID == 480359 || GC.MyChar.Equips.RightHand.ID == 480369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720178));
                                    GC.MyChar.Equips.RightHand.ID = 480359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your club needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }

                            }
                            else
                            {
                                AddText("Sorry you dont wear a club on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 13:
                        if (GC.MyChar.InventoryContains(720179, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 480))
                            {

                                if (GC.MyChar.Equips.RightHand.ID == 480339 || GC.MyChar.Equips.RightHand.ID == 480349 || GC.MyChar.Equips.RightHand.ID == 480359 || GC.MyChar.Equips.RightHand.ID == 480369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720179));
                                    GC.MyChar.Equips.RightHand.ID = 480369;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your club needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }

                            }
                            else
                            {
                                AddText("Sorry you dont wear a club on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;
                    #endregion
                    #region sword
                    case 14:
                        if (GC.MyChar.InventoryContains(720173, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 420))
                            {

                                if (GC.MyChar.Equips.RightHand.ID == 420339 || GC.MyChar.Equips.RightHand.ID == 420349 || GC.MyChar.Equips.RightHand.ID == 420359 || GC.MyChar.Equips.RightHand.ID == 420369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720173));
                                    GC.MyChar.Equips.RightHand.ID = 420349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your sword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }

                            }
                            else
                            {
                                AddText("Sorry you dont wear a sword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 15:
                        if (GC.MyChar.InventoryContains(720174, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 420))
                            {

                                if (GC.MyChar.Equips.RightHand.ID == 420339 || GC.MyChar.Equips.RightHand.ID == 420349 || GC.MyChar.Equips.RightHand.ID == 420359 || GC.MyChar.Equips.RightHand.ID == 420369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720174));
                                    GC.MyChar.Equips.RightHand.ID = 420359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your sword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }

                            }
                            else
                            {
                                AddText("Sorry you dont wear a sword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 16:
                        if (GC.MyChar.InventoryContains(720175, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 420))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 420339 || GC.MyChar.Equips.RightHand.ID == 420349 || GC.MyChar.Equips.RightHand.ID == 420359 || GC.MyChar.Equips.RightHand.ID == 420369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720175));
                                    GC.MyChar.Equips.RightHand.ID = 420369;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your sword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a sword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;
                    #endregion
                    #region blade
                    case 17:
                        if (GC.MyChar.InventoryContains(720170, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 410))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 410339 || GC.MyChar.Equips.RightHand.ID == 410349 || GC.MyChar.Equips.RightHand.ID == 410359 || GC.MyChar.Equips.RightHand.ID == 410369 || GC.MyChar.Equips.RightHand.ID == 410379)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720170));
                                    GC.MyChar.Equips.RightHand.ID = 410349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your blade needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a blade on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 18:
                        if (GC.MyChar.InventoryContains(720171, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 410))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 410339 || GC.MyChar.Equips.RightHand.ID == 410349 || GC.MyChar.Equips.RightHand.ID == 410359 || GC.MyChar.Equips.RightHand.ID == 410369 || GC.MyChar.Equips.RightHand.ID == 410379)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720171));
                                    GC.MyChar.Equips.RightHand.ID = 410359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your blade needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a blade on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 19:
                        if (GC.MyChar.InventoryContains(720172, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 410))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 410339 || GC.MyChar.Equips.RightHand.ID == 410349 || GC.MyChar.Equips.RightHand.ID == 410359 || GC.MyChar.Equips.RightHand.ID == 410369 || GC.MyChar.Equips.RightHand.ID == 410379)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720172));
                                    GC.MyChar.Equips.RightHand.ID = 410369;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your blade needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a blade on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;

                    case 20:
                        if (GC.MyChar.InventoryContains(720186, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 410))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 410339 || GC.MyChar.Equips.RightHand.ID == 410349 || GC.MyChar.Equips.RightHand.ID == 410359 || GC.MyChar.Equips.RightHand.ID == 410369 || GC.MyChar.Equips.RightHand.ID == 410379)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720186));
                                    GC.MyChar.Equips.RightHand.ID = 410379;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your blade needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a blade on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }
                        break;
                    #endregion

                    #region Dagger
                    case 21:
                        if (GC.MyChar.InventoryContains(720176, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 490))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 490339 || GC.MyChar.Equips.RightHand.ID == 490349)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720176));
                                    GC.MyChar.Equips.RightHand.ID = 490349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your dagger needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a dagger on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;
                    #endregion

                    #region BackSword
                    case 22:
                        if (GC.MyChar.InventoryContains(720180, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 421))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 421339 || GC.MyChar.Equips.RightHand.ID == 421349 || GC.MyChar.Equips.RightHand.ID == 421359 || GC.MyChar.Equips.RightHand.ID == 421369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720180));
                                    GC.MyChar.Equips.RightHand.ID = 421349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your backsword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a backsword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 23:
                        if (GC.MyChar.InventoryContains(720181, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 421))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 421339 || GC.MyChar.Equips.RightHand.ID == 421349 || GC.MyChar.Equips.RightHand.ID == 421359 || GC.MyChar.Equips.RightHand.ID == 421369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720181));
                                    GC.MyChar.Equips.RightHand.ID = 421359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your backsword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a backsword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 24:
                        if (GC.MyChar.InventoryContains(720182, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 421))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 421339 || GC.MyChar.Equips.RightHand.ID == 421349 || GC.MyChar.Equips.RightHand.ID == 421359 || GC.MyChar.Equips.RightHand.ID == 421369)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720182));
                                    GC.MyChar.Equips.RightHand.ID = 421369;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your backsword needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a backsword on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;
                    #endregion

                    #region bow
                    case 25:
                        if (GC.MyChar.InventoryContains(720183, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 500))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 500329 || GC.MyChar.Equips.RightHand.ID == 500339 || GC.MyChar.Equips.RightHand.ID == 500349 || GC.MyChar.Equips.RightHand.ID == 500359 || GC.MyChar.Equips.RightHand.ID == 500409)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720183));
                                    GC.MyChar.Equips.RightHand.ID = 500339;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your bow needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a bow on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 26:
                        if (GC.MyChar.InventoryContains(720184, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 500))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 500329 || GC.MyChar.Equips.RightHand.ID == 500339 || GC.MyChar.Equips.RightHand.ID == 500349 || GC.MyChar.Equips.RightHand.ID == 500359 || GC.MyChar.Equips.RightHand.ID == 500409)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720184));
                                    GC.MyChar.Equips.RightHand.ID = 500349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your bow needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a bow on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 27:
                        if (GC.MyChar.InventoryContains(720185, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 500))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 500329 || GC.MyChar.Equips.RightHand.ID == 500339 || GC.MyChar.Equips.RightHand.ID == 500349 || GC.MyChar.Equips.RightHand.ID == 500359 || GC.MyChar.Equips.RightHand.ID == 500409)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720185));
                                    GC.MyChar.Equips.RightHand.ID = 500359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your bow needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a bow on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 28:
                        if (GC.MyChar.InventoryContains(720187, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 500))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 500329 || GC.MyChar.Equips.RightHand.ID == 500339 || GC.MyChar.Equips.RightHand.ID == 500349 || GC.MyChar.Equips.RightHand.ID == 500359 || GC.MyChar.Equips.RightHand.ID == 500409)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720187));
                                    GC.MyChar.Equips.RightHand.ID = 500409;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your bow needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a bow on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;
                    #endregion

                    #region Spear
                    case 29:
                        if (GC.MyChar.InventoryContains(720188, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 560))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 560339 || GC.MyChar.Equips.RightHand.ID == 560349 || GC.MyChar.Equips.RightHand.ID == 560359)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720188));
                                    GC.MyChar.Equips.RightHand.ID = 560349;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your spear needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a spear on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;

                    case 30:
                        if (GC.MyChar.InventoryContains(720189, 1))
                        {
                            if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 560))
                            {
                                if (GC.MyChar.Equips.RightHand.ID == 560339 || GC.MyChar.Equips.RightHand.ID == 560349 || GC.MyChar.Equips.RightHand.ID == 560359)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720189));
                                    GC.MyChar.Equips.RightHand.ID = 560359;
                                    GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                }
                                else
                                {
                                    AddText("Sorry your spear needs to be level 130");
                                    AddOption("Thanks.", 254);
                                }
                            }
                            else
                            {
                                AddText("Sorry you dont wear a spear on your right-hand");
                                AddOption("Thanks.", 254);
                            }
                        }
                        else
                        {
                            AddText("Sorry you dont have AccessoriesToken. Do you want to buy?");
                            AddOption("Yes, i want buy.", 10);
                            AddOption("Thanks.", 254);

                        }

                        break;
                        #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            AddFinish();
            Send();
        }
    }
}