using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Ultimate.Game;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class DropAnItem
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint ItemUID = BitConverter.ToUInt32(Data, 4);
            Game.Item I = GC.MyChar.FindInvItem(ItemUID);
            if (I.ID != 780001)
                if (I.ID != 0)
                {
                    if (GC.MyChar.MyShop == null)
                    {
                        if (!GC.GM || GC.PM) 
                            if (!I.FreeItem && I.ID != 750000 && (I.ID <= 721575 || I.ID >= 722721 || I.ID == 722384))
                            {
                                if (!DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))//aici sa nu poata sa dea drop dupa trade // GC.MyChar.LastTrade.AddMilliseconds(5000) < DateTime.Now
                                {
                                    Game.DroppedItem DI = new Ultimate.Game.DroppedItem();
                                    DI.Info = I;
                                    DI.DropTime = DateTime.Now;
                                    DI.Loc = GC.MyChar.Loc;
                                    DI.UID = (uint)Program.Rnd.Next(10000000);
                                    if (!World.H_Items.ContainsKey(GC.MyChar.Loc.Map))
                                        World.H_Items.TryAdd(GC.MyChar.Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                                    if (!DI.FindPlace(World.H_Items[GC.MyChar.Loc.Map])) return;
                                    if (GC.MyChar.RemoveItem(ItemUID))
                                    {
                                        DI.Drop();
                                        if (I.IsWorth())
                                            Game.World.DropAdd += GC.MyChar.Name + " has dropped " + DI.UID + "~" + DI.Info.ID + "~" + DI.Info.Plus + "~" + DI.Info.Bless + "~" + DI.Info.Enchant + "~" + (byte)DI.Info.Soc1 + "~" + (byte)DI.Info.Soc2 + "~" + DI.Info.Progress + " Map " + GC.MyChar.Loc.Map + " X " + GC.MyChar.Loc.X + " Y " + GC.MyChar.Loc.Y + " : " + DateTime.Now + "\r\n";
                                    }
                                }
                                else
                                    GC.LocalMessage(2005, "You can't drop items in this map!");

                            }
                            else
                                GC.MyChar.RemoveItem(I.UID);
                    }
                }
                else
                    GC.AddSend(Packets.ItemPacket(ItemUID, 0, 3));
        }
    }
}
