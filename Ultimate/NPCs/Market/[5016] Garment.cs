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





    public class NPC_5016 : NPCBase
    {

        public NPC_5016(Main.GameClient _client)
                : base(_client)
        {
            //12 and 108
            ID = 5016;
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
                        if (GC.MyChar.InventoryContains(720144, 1) || GC.MyChar.InventoryContains(720145, 1) || GC.MyChar.InventoryContains(720146, 1))
                        {
                            AddText("Which Garment Ticket Do You Have?");
                            AddOption("GarmentTicket1", 1);
                            AddOption("GarmentTicket2", 2);
                            AddOption("GarmentTicket3", 3);
                            AddOption("Nevermind", 254);
                        }
                        else
                        {
                            AddText("You don't have any garment tickets. Please come back when you do.\n");
                            //AddText("1-Buy Garment Ticket from Our web page.\n");
                            //AddText("2-Choose a garment and note garment id.\n");
                            //AddText("3-Then give me your ticket and tell me your garment id.\n");
                            AddOption("Show Garments", 9);
                            AddOption("Store Page", 10);
                            AddOption("Try Garment", 31);
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 1:
                        AddText("First you need to know the garment ID numbers.. Check our website for the IDs.");
                        AddInput("Garment ID:", 4);
                        AddOption("Show Garments ID", 7);
                        break;


                    case 2:
                        AddText("First you need to know GarmentTicket2 id numbers.. if you dont know you can see our site.");
                        AddInput("Garment ID:", 5);
                        AddOption("Show Garments ID", 8);
                        break;

                    case 3:
                        AddText("First you need to know GarmentTicket3 id numbers.. if you dont know you can see our site.");
                        AddInput("Garment ID:", 6);
                        AddOption("Show Garments ID", 9);
                        break;

                    case 7:
                        GC.LocalMessage(2105, "https://www.Ultimate-conquer.com/Ultimate/garments/");
                        break;

                    case 8:
                        GC.LocalMessage(2105, "https://www.Ultimate-conquer.com/Ultimate/garments/");
                        break;

                    case 9:
                        GC.LocalMessage(2105, "https://www.Ultimate-conquer.com/Ultimate/garments/");
                        break;

                    case 10:
                        GC.LocalMessage(2105, "http://www.Ultimateconquer.com/shop.php");
                        break;

                    case 4:

                        uint itemid1 = Convert.ToUInt32(ReadString(_data));
                        {
                            if (Database.DatabaseItems.ContainsKey(itemid1))
                            {
                                if (itemid1 == 193025 || itemid1 == 192505 || itemid1 == 192525 || itemid1 == 192535 || itemid1 == 192545 || itemid1 == 193035 || itemid1 == 183465 || itemid1 == 183225 ||
                                    itemid1 == 183315 || itemid1 == 183335 || itemid1 == 183345 || itemid1 == 183385 || itemid1 == 183395 || itemid1 == 183415 || itemid1 == 184305 || itemid1 == 183315 ||
                                    itemid1 == 184345 || itemid1 == 184355 || itemid1 == 184365 || itemid1 == 187665 || itemid1 == 193055 || itemid1 == 193075 || itemid1 == 193085 || itemid1 == 193105 ||
                                    itemid1 == 193300 || itemid1 == 183305 || itemid1 == 188265 || itemid1 == 192675 || itemid1 == 192685 || itemid1 == 193385 || itemid1 == 193395 || itemid1 == 192655 ||
                                    itemid1 == 192665 || itemid1 == 193365 || itemid1 == 193375)
                                {
                                    if (GC.MyChar.InventoryContains(720144, 1))
                                    {
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(720144));
                                        GC.MyChar.AddItem(itemid1);

                                        AddText("Feel happy while using it, we are waiting again sir.. Good Games..");
                                        AddOption("Thanks.", 254);

                                    }
                                    else
                                    {
                                        AddText("Sorry you dont have any purchases for garment. Do you want to buy new garment ?");
                                        AddOption("Yes, I want buy.", 10);
                                        AddOption("Thanks.", 254);

                                    }
                                }
                                else
                                {
                                    AddText("Wrong Garment ID. Please use correct garment ID.");
                                    AddOption("Show Garment ID's.", 7);
                                    AddOption("Thanks.", 254);

                                }
                            }
                            else
                            {
                                AddText("Wrong Garment ID. Please use correct garment ID.");
                                AddOption("Show Garment ID's.", 7);
                                AddOption("Thanks.", 254);

                            }

                        }
                        break;


                    case 5:

                        uint itemid2 = Convert.ToUInt32(ReadString(_data));
                        {
                            if (Database.DatabaseItems.ContainsKey(itemid2))
                            {
                                if (itemid2 == 183485 || itemid2 == 187305 || itemid2 == 187775 || itemid2 == 191405 || itemid2 == 191505 || itemid2 == 192125 || itemid2 == 192135 || itemid2 == 192200 ||
                                    itemid2 == 192435 || itemid2 == 192495 || itemid2 == 192565 || itemid2 == 192605 || itemid2 == 192615 || itemid2 == 192745 || itemid2 == 192755 || itemid2 == 194210 ||
                                    itemid2 == 194320 || itemid2 == 188295 || itemid2 == 183325 || itemid2 == 183375 || itemid2 == 184315 || itemid2 == 184325 || itemid2 == 184335 ||
                                    itemid2 == 188165 || itemid2 == 188175 || itemid2 == 188885 || itemid2 == 188255 || itemid2 == 188495 || itemid2 == 188575 || itemid2 == 188655 || itemid2 == 188675 ||
                                    itemid2 == 193095 || itemid2 == 193345 || itemid2 == 193355 || itemid2 == 193335 || itemid2 == 183425 || itemid2 == 183475 || itemid2 == 192250 || itemid2 == 192310 ||
                                    itemid2 == 192345)
                                {
                                    if (GC.MyChar.InventoryContains(720145, 1))
                                    {
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(720145));
                                        GC.MyChar.AddItem(itemid2);

                                        AddText("Feel happy while using it, we are waiting again sir.. Good Games..");
                                        AddOption("Thanks.", 254);

                                    }
                                    else
                                    {
                                        AddText("Sorry you dont have any purchases for garment. Do you want to buy new garment ?");
                                        AddOption("Yes, i want buy.", 10);
                                        AddOption("Thanks.", 254);

                                    }
                                }
                                else
                                {
                                    AddText("Wrong Garment ID. Please use correct garment ID.");
                                    AddOption("Show Garment ID's.", 8);
                                    AddOption("Thanks.", 254);

                                }
                            }
                            else
                            {
                                AddText("Wrong Garment ID. Please use correct garment ID.");
                                AddOption("Show Garment ID's.", 8);
                                AddOption("Thanks.", 254);

                            }

                        }
                        break;


                    case 6:

                        uint itemid3 = Convert.ToUInt32(ReadString(_data));
                        {
                            if (Database.DatabaseItems.ContainsKey(itemid3))
                            {
                                if (itemid3 == 183275 || itemid3 == 193255 || itemid3 == 188915 || itemid3 == 193315 || itemid3 == 187475 || itemid3 == 188155 || itemid3 == 188285 || itemid3 == 188965 ||
                                    itemid3 == 192625 || itemid3 == 192425 || itemid3 == 192575 || itemid3 == 192635 || itemid3 == 192785 || itemid3 == 193065 || itemid3 == 193115 || itemid3 == 193245 ||
                                    itemid3 == 187315 || itemid3 == 193625 || itemid3 == 194310 || itemid3 == 194350 || itemid3 == 194330 || itemid3 == 194370 || itemid3 == 192325 || itemid3 == 188140 ||
                                    itemid3 == 188190 || itemid3 == 188180 || itemid3 == 189685 || itemid3 == 193725 || itemid3 == 194795 || itemid3 == 192465 || itemid3 == 181100 || itemid3 == 199415 ||
                                    itemid3 == 194995 || itemid3 == 194360 || itemid3 == 191020 || itemid3 == 183635)
                                {
                                    if (GC.MyChar.InventoryContains(720146, 1))
                                    {
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(720146));
                                        GC.MyChar.AddItem(itemid3);

                                        AddText("Feel happy while using it, we are waiting again sir.. Good Games..");
                                        AddOption("Thanks.", 254);

                                    }
                                    else
                                    {
                                        AddText("Sorry you dont have any purchases for garment. Do you want to buy new garment ?");
                                        AddOption("Yes, i want buy.", 10);
                                        AddOption("Thanks.", 254);

                                    }
                                }
                                else
                                {
                                    AddText("Wrong Garment ID. Please use correct garment ID.");
                                    AddOption("Show Garment ID's.", 9);
                                    AddOption("Thanks.", 254);

                                }
                            }
                            else
                            {
                                AddText("Wrong Garment ID. Please use correct garment ID.");
                                AddOption("Show Garment ID's.", 9);
                                AddOption("Thanks.", 254);

                            }

                        }
                        break;

                    case 31:
                        if (GC.MyChar.Equips.Garment.ID == 0)
                        {
                            AddText("First you need to know GarmentTicket id numbers.. if you dont know you can see our site.");
                            AddInput("Garment ID:", 33);
                            AddOption("Show Garments ID", 32);
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your garment.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    case 32:
                        GC.LocalMessage(2105, "https://www.Ultimate-conquer.com/Ultimate/garments/");
                        break;
                    case 34:
                        GC.LocalMessage(2105, "http://www.Ultimate-conquer.comshop.php");
                        break;
                    case 33:

                        uint itemid4 = Convert.ToUInt32(ReadString(_data));
                        {
                            if (GC.MyChar.Equips.Garment.ID == 0)
                            {
                                if (Database.DatabaseItems.ContainsKey(itemid4))
                                {
                                    if (itemid4 == 193025 || itemid4 == 192505 || itemid4 == 192525 || itemid4 == 192535 || itemid4 == 192545 || itemid4 == 193035 || itemid4 == 183465 || itemid4 == 183225 ||
                                        itemid4 == 183315 || itemid4 == 183335 || itemid4 == 183345 || itemid4 == 183385 || itemid4 == 183395 || itemid4 == 183415 || itemid4 == 184305 || itemid4 == 183315 ||
                                        itemid4 == 184345 || itemid4 == 184355 || itemid4 == 184365 || itemid4 == 187665 || itemid4 == 193055 || itemid4 == 193075 || itemid4 == 193085 || itemid4 == 193105 ||
                                        itemid4 == 193300 || itemid4 == 183305 || itemid4 == 188265 || itemid4 == 192675 || itemid4 == 192685 || itemid4 == 193385 || itemid4 == 193395 || itemid4 == 192655 ||
                                        itemid4 == 192665 || itemid4 == 193365 || itemid4 == 193375 || itemid4 == 183485 || itemid4 == 187305 || itemid4 == 187775 || itemid4 == 191405 || itemid4 == 191505 ||
                                        itemid4 == 192125 || itemid4 == 192135 || itemid4 == 192200 || itemid4 == 192435 || itemid4 == 192495 || itemid4 == 192565 || itemid4 == 192605 || itemid4 == 192615 ||
                                        itemid4 == 192745 || itemid4 == 192755 || itemid4 == 194210 || itemid4 == 194320 || itemid4 == 188295 || itemid4 == 183325 || itemid4 == 183375 || itemid4 == 183635 ||
                                        itemid4 == 184315 || itemid4 == 184325 || itemid4 == 184335 || itemid4 == 188165 || itemid4 == 188175 || itemid4 == 188885 || itemid4 == 188255 || itemid4 == 188495 ||
                                        itemid4 == 188575 || itemid4 == 188655 || itemid4 == 188675 || itemid4 == 193095 || itemid4 == 193345 || itemid4 == 193355 || itemid4 == 193335 || itemid4 == 183425 ||
                                        itemid4 == 183475 || itemid4 == 192250 || itemid4 == 192310 || itemid4 == 192345 || itemid4 == 187775 || itemid4 == 183275 || itemid4 == 193255 || itemid4 == 188915 ||
                                        itemid4 == 193315 || itemid4 == 187475 || itemid4 == 188155 || itemid4 == 188285 || itemid4 == 188965 || itemid4 == 192625 || itemid4 == 192425 || itemid4 == 192575 ||
                                        itemid4 == 192635 || itemid4 == 192785 || itemid4 == 193065 || itemid4 == 193115 || itemid4 == 193245 || itemid4 == 187315 || itemid4 == 193625 || itemid4 == 194310 ||
                                        itemid4 == 194350 || itemid4 == 194330 || itemid4 == 194370 || itemid4 == 192325 || itemid4 == 188140 || itemid4 == 188190 || itemid4 == 188180 || itemid4 == 189685 ||
                                        itemid4 == 193725 || itemid4 == 194795 || itemid4 == 192465 || itemid4 == 181100 || itemid4 == 199415 || itemid4 == 194995 || itemid4 == 194360)

                                    {
                                        GC.AddSend(Packets.OverwriteGarment((itemid4)));
                                        GC.MyChar.RemoveStamp = DateTime.Now.AddSeconds(5);
                                        GC.MyChar.RemoveAfter = true;

                                        AddText("it was very nice. I think you should buy.");
                                        AddOption("Yeah.", 34);
                                        AddOption("Thanks.", 254);

                                    }
                                    else
                                    {
                                        AddText("Wrong Garment ID. Please use correct garment ID.");
                                        AddOption("Show Garment ID's.", 32);
                                        AddOption("Thanks.", 254);

                                    }
                                }
                                else
                                {
                                    AddText("Wrong Garment ID. Please use correct garment ID.");
                                    AddOption("Show Garment ID's.", 32);
                                    AddOption("Thanks.", 254);

                                }
                            }
                            else
                            {
                                AddText("Sorry, you have to take off your garment.");
                                AddOption("Thanks", 255);
                            }

                        }
                        break;

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