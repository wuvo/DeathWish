using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Structures
{
    public struct Equipment
    {
        public Item HeadGear;
        public Item Necklace;
        public Item Armor;
        public Item LeftHand;
        public Item RightHand;
        public Item Ring;
        public Item Garment;
        public Item Boots;
        public Item Gourd;
        public Item Fan;
        public Item Tower;
        public Item Steed;

        public void Open()
        {
            HeadGear = new Item();
            Necklace = new Item();
            Armor = new Item();
            LeftHand = new Item();
            RightHand = new Item();
            Ring = new Item();
            Garment = new Item();
            Boots = new Item();
            Gourd = new Item();
            Fan = new Item();
            Tower = new Item();
            Steed = new Item();
        }
        public byte GetSlot(uint UID)
        {
            if (HeadGear.UID == UID) return 1;
            if (Necklace.UID == UID) return 2;
            if (Armor.UID == UID) return 3;
            if (RightHand.UID == UID) return 4;
            if (LeftHand.UID == UID) return 5;
            if (Ring.UID == UID) return 6;
            if (Gourd.UID == UID) return 7;
            if (Boots.UID == UID) return 8;
            if (Garment.UID == UID) return 9;
            if (Fan.UID == UID) return 10;
            if (Tower.UID == UID) return 11;
            if (Steed.UID == UID) return 12;
            return 0;
        }
        public Item Get(byte Pos)
        {
            if (Pos == 1) return HeadGear;
            else if (Pos == 2) return Necklace;
            else if (Pos == 3) return Armor;
            else if (Pos == 4) return RightHand;
            else if (Pos == 5) return LeftHand;
            else if (Pos == 6) return Ring;
            else if (Pos == 7) return Gourd;
            else if (Pos == 8) return Boots;
            else if (Pos == 9) return Garment;
            else if (Pos == 10) return Fan;
            else if (Pos == 11) return Tower;
            else if (Pos == 12) return Steed;
            return new Item();
        }
        public void UnEquip(byte Pos, Character C)
        {
            C.MyClient.AddSend(Packets.ItemPacket(Get(Pos).UID, Pos, 6));

            if (Pos == 1) HeadGear = new Item();
            if (Pos == 2) Necklace = new Item();
            if (Pos == 3) Armor = new Item();
            if (Pos == 4) RightHand = new Item();
            if (Pos == 5) LeftHand = new Item();
            if (Pos == 6) Ring = new Item();
            if (Pos == 7) Gourd = new Item();
            if (Pos == 8) Boots = new Item();
            if (Pos == 9) Garment = new Item();
            if (Pos == 10) Fan = new Item();
            if (Pos == 11) Tower = new Item();
            if (Pos == 12) Steed = new Item();
        }
        public void Replace(byte Pos, Item I, Character C)
        {
            try
            {
                if (Pos == 1) HeadGear = I;
                else if (Pos == 2) Necklace = I;
                else if (Pos == 3) Armor = I;
                else if (Pos == 4) RightHand = I;
                else if (Pos == 5) LeftHand = I;
                else if (Pos == 6) Ring = I;
                else if (Pos == 7) Gourd = I;
                else if (Pos == 8) Boots = I;
                else if (Pos == 9) Garment = I;
                else if (Pos == 10) Fan = I;
                else if (Pos == 11) Tower = I;
                else if (Pos == 12) Steed = I;

                if (I.ID == 0)
                {
                    uint UID = Get(Pos).UID;
                    C.MyClient.AddSend(Packets.ItemPacket(UID, Pos, 6));
                    C.MyClient.AddSend(Packets.ItemPacket(UID, 0, 3));
                }
                else
                {
                    if (I.UID == 0)
                        I.UID = (uint)Program.Rnd.Next(10000000);
                    C.MyClient.AddSend(Packets.AddItem(I, Pos));
                    C.MyClient.AddSend(Packets.UpdateItem(I, Pos));
                }


            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }

        public void Send(Main.GameClient GC, bool Stats)
        {
            if (HeadGear.ID != 0) GC.AddSend(Packets.AddItem(HeadGear, 1));
            if (Necklace.ID != 0) GC.AddSend(Packets.AddItem(Necklace, 2));
            if (Armor.ID != 0) GC.AddSend(Packets.AddItem(Armor, 3));
            if (RightHand.ID != 0) GC.AddSend(Packets.AddItem(RightHand, 4));
            if (LeftHand.ID != 0) GC.AddSend(Packets.AddItem(LeftHand, 5));
            if (Ring.ID != 0) GC.AddSend(Packets.AddItem(Ring, 6));
            if (Gourd.ID != 0) GC.AddSend(Packets.AddItem(Gourd, 7));
            if (Boots.ID != 0) GC.AddSend(Packets.AddItem(Boots, 8));
            if (Garment.ID != 0) GC.AddSend(Packets.AddItem(Garment, 9));
            if (Fan.ID != 0) GC.AddSend(Packets.AddItem(Fan, 10));
            if (Tower.ID != 0) GC.AddSend(Packets.AddItem(Tower, 11));
            if (Steed.ID != 0) GC.AddSend(Packets.AddItem(Steed, 12));

            if (Stats)
            {
                GC.MyChar.EquipStats(1, true, false);
                GC.MyChar.EquipStats(2, true, false);
                GC.MyChar.EquipStats(3, true, false);
                GC.MyChar.EquipStats(4, true, false);
                GC.MyChar.EquipStats(5, true, false);
                GC.MyChar.EquipStats(6, true, false);
                GC.MyChar.EquipStats(7, true, false);
                GC.MyChar.EquipStats(8, true, false);
                GC.MyChar.EquipStats(9, true, false);
                GC.MyChar.EquipStats(10, true, false);
                GC.MyChar.EquipStats(11, true, false);
                GC.MyChar.EquipStats(12, true, false);
                GC.MyChar.LoadedEquipmentHPAdd = true;
            }
        }
        public void SendView(uint Viewed, Main.GameClient GC)
        {
            if (HeadGear.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, HeadGear, 1));
            if (Necklace.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Necklace, 2));
            if (Armor.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Armor, 3));
            if (RightHand.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, RightHand, 4));
            if (LeftHand.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, LeftHand, 5));
            if (Ring.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Ring, 6));
            if (Gourd.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Gourd, 7));
            if (Boots.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Boots, 8));
            if (Garment.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Garment, 9));
            if (Fan.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Fan, 10));
            if (Tower.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Tower, 11));
            if (Steed.ID != 0) GC.AddSend(Packets.AddViewItem(Viewed, Steed, 12));
        }
        public void WriteThis(System.IO.BinaryWriter BW)
        {
            HeadGear.WriteThis(BW);
            Necklace.WriteThis(BW);
            Armor.WriteThis(BW);
            RightHand.WriteThis(BW);
            LeftHand.WriteThis(BW);
            Ring.WriteThis(BW);
            Gourd.WriteThis(BW);
            Boots.WriteThis(BW);
            Garment.WriteThis(BW);
            Fan.WriteThis(BW);
            Tower.WriteThis(BW);
            Steed.WriteThis(BW);
        }
        public void ReadThis(System.IO.BinaryReader BR)
        {
            HeadGear = new Item();
            HeadGear.ReadThis(BR);
            Necklace = new Item();
            Necklace.ReadThis(BR);
            Armor = new Item();
            Armor.ReadThis(BR);
            RightHand = new Item();
            RightHand.ReadThis(BR);
            LeftHand = new Item();
            LeftHand.ReadThis(BR);
            Ring = new Item();
            Ring.ReadThis(BR);
            Gourd = new Item();
            Gourd.ReadThis(BR);
            Boots = new Item();
            Boots.ReadThis(BR);
            Garment = new Item();
            Garment.ReadThis(BR);
            Fan = new Item();
            Fan.ReadThis(BR);
            Tower = new Item();
            Tower.ReadThis(BR);
            Steed = new Item();
            Steed.ReadThis(BR);
        }
        public static implicit operator Item[] (Equipment e)
        {
            Item[] eqs = new Item[11];
            for (byte x = 0; x < 12; x++)
            {
                try
                {
                    eqs[x] = e.Get(x);
                }
                catch { }
            }
            return eqs;
        }
    }
}
