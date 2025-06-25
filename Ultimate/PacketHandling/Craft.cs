using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class Craft
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            List<uint> Slots = new List<uint>(Data[10]);
            List<Item> Items = new List<Item>(Data[10]);

            byte Size = 12;
            for (int a = 0; a < Data[10]; a++)
            {
                Slots.Add(BitConverter.ToUInt32(Data, Size));
                Size += 4;
            }

            foreach (uint UID in Slots)
                Items.Add(GC.MyChar.FindInvItem(UID));

            if (SingleItem(Items))
            {
                if (MainID(Items) != 0)
                {
                    if (UID(Items) != 0)
                    {
                        if (IsEquipment(MainID(Items)))
                        {
                            if (HasItems(GC.MyChar, Items))
                            {
                                Item I = GC.MyChar.FindInvItem(UID(Items));
                                if (I.Soc1 == 0 && I.Soc2 == 0)
                                {
                                    uint _price = Price(Items);
                                    if (GC.MyChar.Silvers >= _price)
                                    {
                                        GC.MyChar.Silvers -= _price;
                                        Items.Remove(GC.MyChar.FindInvItem(UID(Items)));
                                        double _chance = GetChance(Items);
                                        if (_chance > 5)
                                        {
                                            double _multiplier = Program.Rnd.NextDouble();
                                            if (_chance <= 15)
                                                _chance = _multiplier * (_chance - 5) + 5;
                                            else
                                                _chance = _multiplier * (15 - 5) + 5;
                                        }
                                        foreach (Item I2 in Items.ToList())
                                        {
                                            GC.MyChar.RemoveItem(I2);
                                            Items.Remove(I2);
                                        }
                                        if (Items.Count == 0)
                                        {
                                            if (MyMath.ChanceSuccess(_chance))
                                            {
                                                if (I.Soc1 == Game.Item.Gem.NoSocket)
                                                {
                                                    I.Soc1 = Game.Item.Gem.EmptySocket;
                                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "LuckyGuy")).Get);
                                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got first socket into his/her " + I.DBInfo.Name + " while crafting the item!", 2011, 0);
                                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket in " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) from crafting. \r\n";
                                                }
                                                GC.AddSend(Packets.UpdateItem(I, 0));
                                            }
                                            else
                                                GC.LocalMessage(2005, "What a shame! You weren't lucky enough to make a socket! Better luck next time!");
                                        }
                                        else
                                            World.DebugAdd += GC.MyChar.Name + " failed to craft an item!\r\n";
                                    }
                                    else
                                    {
                                        string x = _price.ToString("n0", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
                                        GC.LocalMessage(2005, "You don't have " + x + " silvers!");
                                    }
                                }
                                else
                                    GC.LocalMessage(2005, "Your " + I.DBInfo.Name + " is already socketed!");
                            }
                        }
                        else
                            GC.LocalMessage(2005, "Only equipments can be crafted!");
                    }
                }
            }
            else
                GC.LocalMessage(2005, "You can only craft one single equipment at a time!");
            
        }

        static bool SingleItem(List<Item> Items)
        {
            byte _amount = 0;
            foreach (Item I in Items)
            {
                if (I.ID != 1088000 && I.ID != 1088001 && I.ID != 1088002 && I.ID != 720027 && I.ID != 720028 && I.ID != 729912)
                    _amount++;
            }
            if (_amount != 1)
                return false;
            return true;
        }
        static double GetChance(List<Item> Items)
        {
            double _amount = 0;

            foreach (Item I in Items)
            {
                if (I.ID == 1088000)
                    _amount += 0.05;
                else if (I.ID == 1088001)
                    _amount += 0.02;
                else if (I.ID == 1088002)
                    _amount += 0.03;
                else if (I.ID == 720027)
                    _amount += 0.2;
                else if (I.ID == 720028)
                    _amount += 0.5;
                else if (I.ID == 729912)
                    _amount += 2;
            }

            return _amount;
        }
        static uint MainID(List<Item> Items)
        {
            foreach (Item I in Items)
            {
                if (I.ID != 1088000 && I.ID != 1088001 && I.ID != 1088002 && I.ID != 720027 && I.ID != 720028 && I.ID != 729912)
                    return I.ID;
            }
            return 0;
        }
        static uint UID(List<Item> Items)
        {
            foreach (Item I in Items)
            {
                if (I.ID != 1088000 && I.ID != 1088001 && I.ID != 1088002 && I.ID != 720027 && I.ID != 720028 && I.ID != 729912)
                    return I.UID;
            }
            return 0;
        }
        static bool IsEquipment(uint ID)
        {
            List<int> Parts = new List<int>() { 111, 113, 114, 117, 118, 120, 121, 130, 131, 133, 134, 141, 142, 150, 151, 152, 160, 410, 420, 421, 430, 440, 450, 460, 480, 481, 490, 500, 510, 530, 540, 560, 561, 580, 900 };
            foreach (int IDPart in Parts)
            {
                if (Game.ItemIDManipulation.Part(ID, 0, 3) == IDPart)
                    return true;
            }
            return false;
        }
        static bool HasItems(Character C, List<Item> Items)
        {
            foreach (Item I in Items)
                if (!C.InventoryContains(I.UID))
                    return false;
            return true;
        }
        static uint Price(List<Item> Items)
        {
            uint _price = 0;
            foreach (Item I in Items)
            {
                if (I.ID == 1088000)
                    _price += 25000;
                else if (I.ID == 1088001)
                    _price += 10000;
                else if (I.ID == 1088002)
                    _price += 15000;
                else if (I.ID == 720027)
                    _price += 100000;
                else if (I.ID == 720028)
                    _price += 250000;
                else if (I.ID == 729912)
                    _price += 1000000;
            }
            return _price;
        }
    }
}
