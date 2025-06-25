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

    public class NPC_5018 : NPCBase
    {

        public NPC_5018(Main.GameClient _client)
                : base(_client)
        {
            //12 and 108
            IsGlobal = true;
            ID = 5018;
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
                        AddText("which weapon do you want to try?");
                        AddOption("Club", 1);
                        AddOption("Sword", 2);
                        AddOption("Blade", 3);
                        AddOption("Dagger", 4);
                        AddOption("BackSword", 5);
                        AddOption("Bow", 6);
                        AddOption("Spear", 7);
                        AddOption("Thanks", 255);
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


                    #region Club
                    case 11:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((480349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 12:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((480359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 13:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((480369)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion
                    #region sword
                    case 14:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((420349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    case 15:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((420359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 16:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((420369)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion
                    #region blade
                    case 17:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((410349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 18:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((410359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 19:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((410369)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 20:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((410379)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion

                    #region Dagger
                    case 21:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((490349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion

                    #region BackSword
                    case 22:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((421349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 23:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((421359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 24:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((421369)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion

                    #region bow
                    case 25:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((500339)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 26:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((500349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 27:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((500359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;

                    case 28:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((500409)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion

                    #region Spear
                    case 29:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((560349)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    case 30:
                        if (GC.MyChar.Equips.RightHand.ID == 0 && GC.MyChar.Equips.LeftHand.ID == 0)
                        {
                            GC.AddSend(Packets.OverwriteWeapon((560359)));
                            GC.MyChar.RemoveStamp1 = DateTime.Now.AddSeconds(5);
                            GC.MyChar.RemoveAfter1 = true;
                        }
                        else
                        {
                            AddText("Sorry, you have to take off your weapons.");
                            AddOption("Thanks", 255);
                        }
                        break;
                    #endregion


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
                        GC.LocalMessage(2105, "https://www.Ultimateconquer.com/Ultimate/garments/");
                        break;
                    case 34:
                        GC.LocalMessage(2105, "http://www.Ultimateconquer.com/Ultimate/Store");
                        break;
                    case 33:

                        uint itemid1 = Convert.ToUInt32(ReadString(_data));
                        {
                            if (GC.MyChar.Equips.Garment.ID == 0)
                            {
                                if (Database.DatabaseItems.ContainsKey(itemid1))
                                {
                                    if (itemid1 == 193025 || itemid1 == 192505 || itemid1 == 192525 || itemid1 == 192535 || itemid1 == 192545 || itemid1 == 193035 || itemid1 == 183465 || itemid1 == 183225 ||
                                        itemid1 == 183315 || itemid1 == 183335 || itemid1 == 183345 || itemid1 == 183385 || itemid1 == 183395 || itemid1 == 183415 || itemid1 == 184305 || itemid1 == 183315 ||
                                        itemid1 == 184345 || itemid1 == 184355 || itemid1 == 184365 || itemid1 == 187665 || itemid1 == 193055 || itemid1 == 193075 || itemid1 == 193085 || itemid1 == 193105 ||
                                        itemid1 == 193300 || itemid1 == 183305 || itemid1 == 188265 || itemid1 == 192675 || itemid1 == 192685 || itemid1 == 193385 || itemid1 == 193395 || itemid1 == 192655 ||
                                        itemid1 == 192665 || itemid1 == 193365 || itemid1 == 193375 || itemid1 == 183485 || itemid1 == 187305 || itemid1 == 187775 || itemid1 == 191405 || itemid1 == 191505 ||
                                        itemid1 == 192125 || itemid1 == 192135 || itemid1 == 192200 || itemid1 == 192435 || itemid1 == 192495 || itemid1 == 192565 || itemid1 == 192605 || itemid1 == 192615 ||
                                        itemid1 == 192745 || itemid1 == 192755 || itemid1 == 194210 || itemid1 == 194320 || itemid1 == 188295 || itemid1 == 183325 || itemid1 == 183375 || itemid1 == 183635 ||
                                        itemid1 == 184315 || itemid1 == 184325 || itemid1 == 184335 || itemid1 == 188165 || itemid1 == 188175 || itemid1 == 188885 || itemid1 == 188255 || itemid1 == 188495 ||
                                        itemid1 == 188575 || itemid1 == 188655 || itemid1 == 188675 || itemid1 == 193095 || itemid1 == 193345 || itemid1 == 193355 || itemid1 == 193335 || itemid1 == 183425 ||
                                        itemid1 == 183475 || itemid1 == 192250 || itemid1 == 192310 || itemid1 == 192345 || itemid1 == 187775 || itemid1 == 183275 || itemid1 == 193255 || itemid1 == 188915 ||
                                        itemid1 == 193315 || itemid1 == 187475 || itemid1 == 188155 || itemid1 == 188285 || itemid1 == 188965 || itemid1 == 192625 || itemid1 == 192425 || itemid1 == 192575 ||
                                        itemid1 == 192635 || itemid1 == 192785 || itemid1 == 193065 || itemid1 == 193115 || itemid1 == 193245 || itemid1 == 187315 || itemid1 == 193625 || itemid1 == 194310 ||
                                        itemid1 == 194350 || itemid1 == 194330 || itemid1 == 194370 || itemid1 == 192325 || itemid1 == 188140 || itemid1 == 188190 || itemid1 == 188180 || itemid1 == 189685 ||
                                        itemid1 == 193725 || itemid1 == 194795 || itemid1 == 192465 || itemid1 == 181100 || itemid1 == 199415 || itemid1 == 194995 || itemid1 == 194360)

                                    {
                                        GC.AddSend(Packets.OverwriteGarment((itemid1)));
                                        GC.MyChar.RemoveStamp = DateTime.Now.AddSeconds(5);
                                        GC.MyChar.RemoveAfter = true;

                                        AddText("it was very nice. I think you should buy.");
                                        AddOption("Yeah.", 34);
                                        AddOption("Thanks.", 254);

                                    }
                                    else
                                    {
                                        AddText("Wrong Garment id. Can you check again Garment ID from our site ?");
                                        AddOption("Show Garment ID's.", 32);
                                        AddOption("Thanks.", 254);

                                    }
                                }
                                else
                                {
                                    AddText("Wrong Garment id. Can you check again Garment ID from our site ?");
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

                    case 36:
                        GC.MyChar.Invisible = false;
                        GC.MyChar.Teleport(1036, 205, 206);
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