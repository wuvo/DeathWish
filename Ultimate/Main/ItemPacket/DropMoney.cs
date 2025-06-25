using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Ultimate.Game;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class DropMoney
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint HowMuch = BitConverter.ToUInt32(Data, 4);
            if (HowMuch > 100)
            {
                if (!GC.GM || GC.PM)
                    if (HowMuch <= GC.MyChar.Silvers)
                    {
                        if (GC.MyChar.MyShop == null)
                        {
                            Game.DroppedItem DI = new Ultimate.Game.DroppedItem();
                            DI.Info = new Game.Item();
                            DI.Silvers = HowMuch;
                            if (DI.Silvers < 10)
                                DI.Info.ID = 1090000;
                            else if (DI.Silvers < 100)
                                DI.Info.ID = 1090010;
                            else if (DI.Silvers < 1000)
                                DI.Info.ID = 1090020;
                            else if (DI.Silvers < 3000)
                                DI.Info.ID = 1091000;
                            else if (DI.Silvers < 10000)
                                DI.Info.ID = 1091010;
                            else
                                DI.Info.ID = 1091020;

                            DI.UID = (uint)Program.Rnd.Next(10000000);
                            DI.Info.UID = DI.UID;
                            DI.DropTime = DateTime.Now;
                            DI.Loc = GC.MyChar.Loc;
                            if (!World.H_Items.ContainsKey(GC.MyChar.Loc.Map))
                                World.H_Items.TryAdd(GC.MyChar.Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                            if (!DI.FindPlace(World.H_Items[GC.MyChar.Loc.Map])) return;
                            DI.Drop();
                            Game.World.DropAdd += GC.MyChar.Name + " dropped silvers: " + HowMuch + " of total gold: " + GC.MyChar.Silvers + "\r\n";
                            GC.MyChar.Silvers -= HowMuch;
                        }
                    }
            }
            /*
             if (I.ID != 0)
             {
                 if (GC.MyChar.MyShop == null)
                 {
                     if (!I.FreeItem)
                     {
                         Game.DroppedItem DI = new Ultimate.Game.DroppedItem();
                         DI.Info = I;
                         DI.DropTime = DateTime.Now;
                         DI.Loc = GC.MyChar.Loc;
                         DI.UID = (uint)Program.Rnd.Next(10000000);
                         if (!DI.FindPlace((Hashtable)Game.World.H_Items[GC.MyChar.Loc.Map])) return;
                         DI.Drop();
                         GC.MyChar.RemoveItem(I);
                     }
                     else
                         GC.LocalMessage(2005, "Cannot drop Free items.");
                 }
             }
             else
                 GC.AddSend(Packets.ItemPacket(ItemUID, 0, 3));
         */
        }
    }
}
