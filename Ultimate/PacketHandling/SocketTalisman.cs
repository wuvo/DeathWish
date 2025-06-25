using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.PacketHandling
{
    public class SocketTalisman
    {
        public static void HandleCPS(Main.GameClient GC, byte[] Data)
        {
            uint UID1 = BitConverter.ToUInt32(Data, 4);
            byte Slot = GC.MyChar.Equips.GetSlot(UID1);
            Game.Item Talisman = GC.MyChar.Equips.Get(Slot);
            if (Talisman.UID == UID1)
            {
                int Price = 0;
                if ((byte)Talisman.Soc1 == 0)
                {
                    decimal procent = 100 - (Talisman.TalismanProgress * 256 * 100 / 2048000);
                    if (100 - procent < 25)
                        return;
                    double price = (double)procent * 55;
                    Price = Convert.ToInt32(price);
                }
                else
                {
                    decimal procent = 100 - (Talisman.TalismanProgress * 256 * 100 / 5120000);
                    if (100 - procent < 25)
                        return;
                    double price = (double)procent * 110;
                    Price = Convert.ToInt32(price);
                }

                if (GC.MyChar.CPs >= Price)
                {
                    GC.MyChar.CPs -= (uint)Price;
                    if (Talisman.Soc1 == Ultimate.Game.Item.Gem.NoSocket)
                    {
                        Talisman.Soc1 = Ultimate.Game.Item.Gem.EmptySocket;
                        Talisman.TalismanProgress = 0;
                        GC.MyChar.Equips.Replace(Slot, Talisman, GC.MyChar); return;
                    }
                    if (Talisman.Soc1 != Ultimate.Game.Item.Gem.NoSocket)
                    {
                        Talisman.Soc2 = Ultimate.Game.Item.Gem.EmptySocket;
                        Talisman.TalismanProgress = 0;
                        GC.MyChar.Equips.Replace(Slot, Talisman, GC.MyChar); return;
                    }

                }
            }
        }

        public static void HandleItems(Main.GameClient GC, byte[] Data)
        {
            uint UID1 = BitConverter.ToUInt32(Data, 8);
            uint UID2 = BitConverter.ToUInt32(Data, 4);
            Game.Item UsedItem = GC.MyChar.FindInvItem(UID1);
            byte Slot = GC.MyChar.Equips.GetSlot(UID2);
            Game.Item Talisman = GC.MyChar.Equips.Get(Slot);

            if (UsedItem.UID == UID1 && Talisman.UID == UID2 && UsedItem.ID != 0 && Talisman.ID != 0)
            {
                ushort Points = 0;
                Game.ItemIDManipulation I = new Ultimate.Game.ItemIDManipulation(UsedItem.ID);
                if (I.Quality == Ultimate.Game.Item.ItemQuality.Refined)
                    Points += 5;
                else if (I.Quality == Ultimate.Game.Item.ItemQuality.Unique)
                    Points += 10;
                else if (I.Quality == Ultimate.Game.Item.ItemQuality.Elite)
                    Points += 40;
                else if (I.Quality == Ultimate.Game.Item.ItemQuality.Super)
                    Points += 1000;

                if (UsedItem.Plus > 0)
                    Points += Database.SocPlusExtra[UsedItem.Plus - 1];

                if (UsedItem.FreeItem)
                    return;
                if (UsedItem.ID / 1000 == Talisman.ID / 1000)
                    return;

                string Type = UsedItem.ID.ToString().Remove(2, UsedItem.ID.ToString().Length - 2);
                uint WeirdThing = Convert.ToUInt32(Type);

                if (WeirdThing <= 61 && WeirdThing >= 40)
                {
                    if (I.Quality == Ultimate.Game.Item.ItemQuality.Elite || I.Quality == Ultimate.Game.Item.ItemQuality.Super)
                    {
                        if ((byte)UsedItem.Soc1 != 0 && (byte)UsedItem.Soc2 == 0)
                            Points += 160;
                        if ((byte)UsedItem.Soc1 != 0 && (byte)UsedItem.Soc2 != 0)
                            Points += 960;
                    }
                }
                else
                {
                    if (I.Quality == Ultimate.Game.Item.ItemQuality.Elite || I.Quality == Ultimate.Game.Item.ItemQuality.Super)
                    {
                        if ((byte)UsedItem.Soc1 != 0 && (byte)UsedItem.Soc2 == 0)
                            Points += 2000;
                        if ((byte)UsedItem.Soc1 != 0 && (byte)UsedItem.Soc2 != 0)
                            Points += 8000;
                    }
                }

                Talisman.TalismanProgress += Points;
                if (Talisman.Soc1 == Ultimate.Game.Item.Gem.NoSocket && Talisman.TalismanProgress >= 8000)
                {
                    Talisman.Soc1 = Ultimate.Game.Item.Gem.EmptySocket;
                    Talisman.TalismanProgress -= 8000;
                }
                if (Talisman.Soc1 != Ultimate.Game.Item.Gem.NoSocket && Talisman.Soc2 == Ultimate.Game.Item.Gem.NoSocket && Talisman.TalismanProgress >= 20000)
                {
                    Talisman.Soc2 = Ultimate.Game.Item.Gem.EmptySocket;
                    Talisman.TalismanProgress = 0;
                }
                GC.MyChar.Equips.Replace(Slot, Talisman, GC.MyChar);
                GC.MyChar.RemoveItem(UsedItem);
            }
        }
    }
}
