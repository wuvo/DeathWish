using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Structures
{
    public struct Banks
    {
        public List<Item> TCWarehouse;
        public List<Item> PCWarehouse;
        public List<Item> ACWarehouse;
        public List<Item> DCWarehouse;
        public List<Item> BIWarehouse;
        public List<Item> SCWarehouse;
        public List<Item> MAWarehouse;
        public List<Item> MAWarehouse2;
        public List<Item> HouseWH1;
        public List<Item> HouseWH2;

        public void Open()
        {
            TCWarehouse = new List<Item>(20);
            PCWarehouse = new List<Item>(20);
            ACWarehouse = new List<Item>(20);
            DCWarehouse = new List<Item>(20);
            BIWarehouse = new List<Item>(20);
            SCWarehouse = new List<Item>(20);
            MAWarehouse = new List<Item>(40);
            MAWarehouse2 = new List<Item>(40);
            HouseWH1 = new List<Item>(20);
            HouseWH2 = new List<Item>(20);
    }
        public void LoadItem(Character C, Item I, ushort Location)
        {
            List<Item> Warehouse = new List<Item>();
            switch (Location)
            {
                case 8: { Warehouse = C.Warehouses.TCWarehouse; break; }
                case 10012: { Warehouse = C.Warehouses.PCWarehouse; break; }
                case 10028: { Warehouse = C.Warehouses.ACWarehouse; break; }
                case 10011: { Warehouse = C.Warehouses.DCWarehouse; break; }
                case 10027: { Warehouse = C.Warehouses.BIWarehouse; break; }
                case 44: { Warehouse = C.Warehouses.MAWarehouse; break; }
                case 46: { Warehouse = C.Warehouses.MAWarehouse2; break; }
                case 4101: { Warehouse = C.Warehouses.SCWarehouse; break; }
                case 2100: { Warehouse = C.Warehouses.HouseWH1; break; }
                case 2101: { Warehouse = C.Warehouses.HouseWH2; break; }
            }
            Warehouse.Add(I);
        }
        public void WriteThis(System.IO.BinaryWriter BW)
        {
            BW.Write((byte)TCWarehouse.Count);
            foreach (Item I in TCWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)PCWarehouse.Count);
            foreach (Item I in PCWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)ACWarehouse.Count);
            foreach (Item I in ACWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)DCWarehouse.Count);
            foreach (Item I in DCWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)BIWarehouse.Count);
            foreach (Item I in BIWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)MAWarehouse.Count);
            foreach (Item I in MAWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)MAWarehouse2.Count);
            foreach (Item I in MAWarehouse2)
                I.WriteThis(BW);
            BW.Write((byte)SCWarehouse.Count);
            foreach (Item I in SCWarehouse)
                I.WriteThis(BW);
            BW.Write((byte)HouseWH1.Count);
            foreach (Item I in HouseWH1)
                I.WriteThis(BW);
            BW.Write((byte)HouseWH2.Count);
            foreach (Item I in HouseWH2)
                I.WriteThis(BW);
        }
        public void ReadThis(System.IO.BinaryReader BR)
        {
            TCWarehouse = new List<Item>(20);
            byte Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !TCWarehouse.Any(x => x.UID == I.UID))
                    TCWarehouse.Add(I);
            }

            PCWarehouse = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !PCWarehouse.Any(x => x.UID == I.UID))
                    PCWarehouse.Add(I);
            }

            ACWarehouse = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !ACWarehouse.Any(x => x.UID == I.UID))
                    ACWarehouse.Add(I);
            }

            DCWarehouse = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !DCWarehouse.Any(x => x.UID == I.UID))
                    DCWarehouse.Add(I);
            }

            BIWarehouse = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !BIWarehouse.Any(x => x.UID == I.UID))
                    BIWarehouse.Add(I);
            }

            MAWarehouse = new List<Item>(40);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !MAWarehouse.Any(x => x.UID == I.UID))
                    MAWarehouse.Add(I);
            }
            MAWarehouse2 = new List<Item>(40);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !MAWarehouse2.Any(x => x.UID == I.UID))
                    MAWarehouse2.Add(I);
            }
            SCWarehouse = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !SCWarehouse.Any(x => x.UID == I.UID))
                    SCWarehouse.Add(I);
            }
            HouseWH1 = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !HouseWH1.Any(x => x.UID == I.UID))
                    HouseWH1.Add(I);
            }
            HouseWH2 = new List<Item>(20);
            Count = BR.ReadByte();
            for (byte i = 0; i < Count; i++)
            {
                Item I = new Item();
                I.ReadThis(BR);
                if (I.ID != 0 && !HouseWH2.Any(x => x.UID == I.UID))
                    HouseWH2.Add(I);
            }
        }
    }
}
