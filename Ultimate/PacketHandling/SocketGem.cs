using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.PacketHandling
{
    public class SocketGem
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint MainUID = BitConverter.ToUInt32(Data, 8);
            uint GemUID = BitConverter.ToUInt32(Data, 12);

            Game.Item MainItem = GC.MyChar.FindInvItem(MainUID);
            Game.Item Gem = GC.MyChar.FindInvItem(GemUID);

            if (MainItem.ID != 0)
            {
                byte Mode = Data[18];
                byte Slot = Data[16];

                if (Mode == 0)
                {
                    if (Gem.ID != 0)
                    {
                        if (Slot == 1)
                        {
                            if (MainItem.Soc1 == Game.Item.Gem.EmptySocket)
                            {
                                //if (GC.MyChar.RemoveItem(MainUID))
                                {
                                    if (GC.MyChar.RemoveItem(Gem.UID))
                                    {
                                        if (Gem.ID > 700000 && Gem.ID < 700090)
                                        {
                                            MainItem.Soc1 = (Game.Item.Gem)(Gem.ID - 700000);
                                            GC.AddSend(Packets.UpdateItem(MainItem, 0));
                                            //GC.MyChar.AddItem(MainItem);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "Invalid Gem ID detected.");
                                            Game.Character C = Game.World.CharacterFromName(GC.MyChar.Name);
                                            //C.BOTJailed = true;
                                            C.BOTJailedDays = 1;
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " day for attempting to gem socket exploit!");
                                            Game.World.DebugAdd += GC.MyChar.Name + " Was botjailed for " + C.BOTJailedDays + " day for attempting to gem socket exploit! Invalid GEMID used:" + Gem.ID + " \r\n";
                                        }

                                    }
                                }
                            }
                        }
                        else if (Slot == 2)
                        {
                            if (MainItem.Soc2 == Game.Item.Gem.EmptySocket)
                            {
                                //if (GC.MyChar.RemoveItem(MainUID))
                                {
                                    if (GC.MyChar.RemoveItem(Gem.UID))
                                    {
                                        if (Gem.ID > 700000 && Gem.ID < 700090)
                                        {
                                            MainItem.Soc2 = (Game.Item.Gem)(Gem.ID - 700000);
                                            GC.AddSend(Packets.UpdateItem(MainItem, 0));
                                            // GC.MyChar.AddItem(MainItem);
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2000, "Invalid Gem ID detected.");
                                            Game.Character C = Game.World.CharacterFromName(GC.MyChar.Name);
                                            //C.BOTJailed = true;
                                            C.BOTJailedDays = 1;
                                            C.Teleport(6003, 30, 72);
                                            C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " day for attempting to gem socket exploit!");
                                            Game.World.DebugAdd += GC.MyChar.Name + " Was botjailed for " + C.BOTJailedDays + " day for attempting to gem socket exploit! Invalid GEMID used:" + Gem.ID + " \r\n";
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //if (GC.MyChar.RemoveItem(MainUID))
                    {
                        if (Slot == 1)
                            if (MainItem.Soc1 != Game.Item.Gem.NoSocket)
                            {
                                MainItem.Soc1 = Game.Item.Gem.EmptySocket;
                                GC.AddSend(Packets.UpdateItem(MainItem, 0));
                                // GC.MyChar.AddItem(MainItem);
                            }
                        if (Slot == 2)
                            if (MainItem.Soc2 != Game.Item.Gem.NoSocket)
                            {
                                MainItem.Soc2 = Game.Item.Gem.EmptySocket;
                                GC.AddSend(Packets.UpdateItem(MainItem, 0));
                                //GC.MyChar.AddItem(MainItem);
                            }

                    }
                }
            }
        }
    }
}
