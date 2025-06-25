using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using NetFwTypeLib;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class Compose
    {
        public static void Handle( Main.GameClient GC, byte[] Data)
        {
                uint MainUID = BitConverter.ToUInt32(Data, 8);
                uint MinorUID1 = BitConverter.ToUInt32(Data, 12);

                Item MainI = GC.MyChar.FindInvItem(MainUID);
                Item MinorI = GC.MyChar.FindInvItem(MinorUID1);
                Item MainISec = MainI;
                Item MinorISec = MinorI;

            if (OfSameType(MainI.ID, MinorI.ID))
            {
                if (MainI.ID != 0 && MinorI.ID != 0 && MainI.Plus <= 8 && MinorI.Plus > 0)
                {
                    try
                    {
                        if (GC.MyChar.InventoryContains(MinorI.UID) && GC.MyChar.InventoryContains(MainI.UID))
                        {
                            if (MainI.UID != MinorI.UID)
                            {
                                uint Progress = MainI.Progress;
                                Progress += Database.StonePts[MinorI.Plus];

                                //GC.MyChar.RemoveItem(MainI);
                                // if (GC.MyChar.RemoveItem(MainI.UID))
                                {
                                    if (GC.MyChar.RemoveItem(ref MinorI))
                                    {

                                        while (Progress >= Database.ComposePts[MainI.Plus] && MainI.Plus <= 8)
                                        {
                                            Progress = (uint)(Progress - Database.ComposePts[MainI.Plus]);
                                            MainI.Plus++;
                                        }

                                        if (MainI.Plus >= 9)
                                        {
                                            Progress = 0;
                                            MainI.Plus = 9;
                                        }
                                        MainI.Progress = (ushort)Progress;
                                        GC.AddSend(Packets.UpdateItem(MainI, 0));
                                        Game.World.DebugAdd += GC.MyChar.Name + " #composecheck -- Just composed Main Item ID " + MainI.ID + " with minor item ID " + MinorI.ID + "! \r\n";
                                        //GC.MyChar.AddItem(MainI);

                                        if (!GC.MyChar.InventoryContains(MainI.UID))// || GC.MyChar.InventoryContains(MinorI.UID))
                                        {
                                            //if (!GC.MyChar.InventoryContains(MainI.UID))
                                            //   {
                                            Game.World.DebugAdd += GC.MyChar.Name + " doesn't have Main Item after compose (should be added) \r\n";
                                            Console.WriteLine(GC.MyChar.Name + " doesn't have Main Item after compose (should be added)");
                                            //  }
                                            /*  else// (GC.MyChar.Inventory.Contains(MinorI))
                                              {
                                                  Game.World.DebugAdd += GC.MyChar.Name + " has the minor item bofore compose (should be removed) \r\n";
                                                  Console.WriteLine(GC.MyChar.Name + " has the minor item bofore compose (should be removed)");
                                              }*/
                                            Game.World.DebugAdd += GC.MyChar.Name + " used main item: " + MainI.ID + "~" + MainI.Plus + "~" + MainI.Enchant + "~" + MainI.Bless + "~" + MainI.Soc1 + "~" + MainI.Soc2 + "~" + MainI.Progress + "~" + MainI.UID + " and minor: " + MinorI.ID + "~" + MinorI.Plus + "~" + MinorI.Enchant + "~" + MinorI.Bless + "~" + MinorI.Soc1 + "~" + MinorI.Soc2 + "~" + MinorI.Progress + "~" + MinorI.UID + "\r\n";
                                            Game.World.DebugAdd += GC.MyChar.Name + " used main (backup) item: " + MainISec.ID + "~" + MainISec.Plus + "~" + MainISec.Enchant + "~" + MainISec.Bless + "~" + MainISec.Soc1 + "~" + MainISec.Soc2 + "~" + MainISec.Progress + " and minor (backup): " + MinorISec.ID + "~" + MinorISec.Plus + "~" + MinorISec.Enchant + "~" + MinorISec.Bless + "~" + MinorISec.Soc1 + "~" + MinorISec.Soc2 + "~" + MinorISec.Progress + "\r\n";
                                        }
                                    }
                                    else Game.World.DebugAdd += GC.MyChar.Name + " used main item: (removed) " + MainI.ID + "~" + MainI.Plus + "~" + MainI.Enchant + "~" + MainI.Bless + "~" + MainI.Soc1 + "~" + MainI.Soc2 + "~" + MainI.Progress + "~" + MainI.UID + " and minor: (not removed) " + MinorI.ID + "~" + MinorI.Plus + "~" + MinorI.Enchant + "~" + MinorI.Bless + "~" + MinorI.Soc1 + "~" + MinorI.Soc2 + "~" + MinorI.Progress + "~" + MinorI.UID + "\r\n";
                                }
                                //else Game.World.DebugAdd += GC.MyChar.Name + " used main item: (not removed) " + MainI.ID + "~" + MainI.Plus + "~" + MainI.Enchant + "~" + MainI.Bless + "~" + MainI.Soc1 + "~" + MainI.Soc2 + "~" + MainI.Progress + "~" + MainI.UID;
                            }
                        }
                    }
                    catch (Exception Exc)
                    {
                        Game.World.ExcAdd += Exc.ToString() + "\r\n";
                        Game.World.DebugAdd += GC.MyChar.Name + " used main item: " + MainI.ID + "~" + MainI.Plus + "~" + MainI.Enchant + "~" + MainI.Bless + "~" + MainI.Soc1 + "~" + MainI.Soc2 + "~" + MainI.Progress + " and minor: " + MinorI.ID + "~" + MinorI.Plus + "~" + MinorI.Enchant + "~" + MinorI.Bless + "~" + MinorI.Soc1 + "~" + MinorI.Soc2 + "~" + MinorI.Progress + "\r\n";
                        Game.World.DebugAdd += GC.MyChar.Name + " used main (backup) item: " + MainISec.ID + "~" + MainISec.Plus + "~" + MainISec.Enchant + "~" + MainISec.Bless + "~" + MainISec.Soc1 + "~" + MainISec.Soc2 + "~" + MainISec.Progress + " and minor (backup): " + MinorISec.ID + "~" + MinorISec.Plus + "~" + MinorISec.Enchant + "~" + MinorISec.Bless + "~" + MinorISec.Soc1 + "~" + MinorISec.Soc2 + "~" + MinorISec.Progress + "\r\n";
                    }
                }
                if (MainI.ID == 0 || MinorI.ID == 0) // make sure your not jailing for a rare glitch (itemid = 0) (backup check)
                {
                    Game.Character C = Game.World.CharacterFromName(GC.MyChar.Name);
                    C.MyClient.LocalMessage(2005, "Compose Error(#2)! One of the Item ID's are invalid! Main Item ID: " + MainI.ID + " Minor item ID: " + MinorI.ID + " if this continues to happen, contact an admin!");
                }
            }
            else //auto jail cheaters
            {
                Game.Character C = Game.World.CharacterFromName(GC.MyChar.Name);
                if (MainI.ID == 0 || MinorI.ID == 0) // make sure your not jailing for a rare glitch (itemid = 0)
                {
                    C.MyClient.LocalMessage(2005, "Compose Error(#1)! One of the Item ID's are invalid! Main Item ID: " + MainI.ID + " Minor item ID: " + MinorI.ID + " if this continues to happen, contact an admin! One of the items you are using is fake!");
                }
                else
                {
                    //C.BOTJailed = true;
                    C.BOTJailedDays = 30;
                    C.Teleport(6003, 30, 72);
                    C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " days for attempting to compose exploit!");
                    C.MyClient.LocalMessage(2011, "If you feel this is a mistake, please contact the admins immediately.");
                    Game.World.DebugAdd += GC.MyChar.Name + " Was botjailed for " + C.BOTJailedDays + " days for attempting to compose item " + MainI.ID + " with minor item " + MinorI.ID + "! \r\n";
                    //Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " was auto botjailed for attempting to compose items that don't belong to the same type.", 2005, 0);
                }

                /*List<Item> Items = new List<Item>();
                foreach (Game.Item I in GC.MyChar.Inventory)
                    Items.Add(I);
                foreach (Game.Item I in Items)
                    GC.MyChar.RemoveItem(I);
                GC.LocalMessage(2005, "Inventory Cleared!"); */
            }

        }
    
        static bool OfSameType(uint ID1, uint ID2)
        {
            ushort Part1 = (ushort)ItemIDManipulation.Part(ID1, 0, 3);
            ushort Part2 = (ushort)ItemIDManipulation.Part(ID2, 0, 3);
            //ushort Part3 = (ushort)ItemIDManipulation.Part(ID1, 0, 2);
            //ushort Part4 = (ushort)ItemIDManipulation.Part(ID2, 0, 2);

            if (ItemIDManipulation.Digit(ID1, 1) == ItemIDManipulation.Digit(ID2, 1) && (ItemIDManipulation.Digit(ID1, 1) == 4 || ItemIDManipulation.Digit(ID1, 1) == 5 && Part2 != 500))
                return true; // 1 hander or 2 hander weapon, except bows
            else if (Part1 == Part2)
                return true; // both items are identical in type
            return false; // item types don't math server side, or missing something...
                
                
            //else if (ItemIDManipulation.Digit(ID1, 5) == ItemIDManipulation.Digit(ID1, 3) && Part1 == 112)
            //    return true;
            //else if (ItemIDManipulation.Digit(ID2, 5) == ItemIDManipulation.Digit(ID1, 3) && Part2 == 112)
            //    return true;
            //else if (Part1 >= 135 && Part1 <= 139 && Part2 == Part1 - 5)
            //    return true;
            //else if (Part2 >= 135 && Part2 <= 139 && Part1 == Part2 - 5)
            //    return true;
            
        }
    }
}
