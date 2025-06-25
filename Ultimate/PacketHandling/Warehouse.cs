using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class Warehouse
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            if (!GC.MyChar.WHOpen && GC.MyChar.WHPassword != "0")
                return;

            uint NPC = BitConverter.ToUInt32(Data, 4);
            uint IUID = BitConverter.ToUInt32(Data, 12);
            byte Type = Data[8];

            List<Game.Item> Warehouse = null;

            switch (NPC)
            {
                case 8: { Warehouse = GC.MyChar.Warehouses.TCWarehouse; break; }
                case 10012: { Warehouse = GC.MyChar.Warehouses.PCWarehouse; break; }
                case 10028: { Warehouse = GC.MyChar.Warehouses.ACWarehouse; break; }
                case 10011: { Warehouse = GC.MyChar.Warehouses.DCWarehouse; break; }
                case 10027: { Warehouse = GC.MyChar.Warehouses.BIWarehouse; break; }
                case 44: { Warehouse = GC.MyChar.Warehouses.MAWarehouse; break; }
                case 46: { Warehouse = GC.MyChar.Warehouses.MAWarehouse2; break; }
                case 4101: { Warehouse = GC.MyChar.Warehouses.SCWarehouse; break; }
                case 2100: { Warehouse = GC.MyChar.Warehouses.HouseWH1; break; }
                case 2101: { Warehouse = GC.MyChar.Warehouses.HouseWH2; break; }
            }
            // if (Warehouse == null) return;
            if (!Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map) && GC.MyChar.VipLevel < 3)
                return;
            if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
            {
                Dictionary<uint, Game.NPC> MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                if (MapNPC != null)
                {
                    if (!MapNPC.ContainsKey(NPC) && GC.MyChar.VipLevel < 3)
                        return;
                    if (MapNPC.ContainsKey(NPC))
                    {
                        Game.NPC N = (Game.NPC)MapNPC[NPC];
                        if (GC.MyChar.VipLevel < 3)
                            if (MyMath.PointDistance(N.Loc.X, N.Loc.Y, GC.MyChar.Loc.X, GC.MyChar.Loc.Y) > 19)
                                return;
                    }
                }
            }
            if (Type == 1)
            {
                try
                {
                    if (Warehouse.Count < Warehouse.Capacity)//&& GC.MyChar.LastTrade.AddMilliseconds(5000) < DateTime.Now)//aici verificare pe timp
                    {
                        Game.Item I = GC.MyChar.FindInvItem(IUID);
                        if (I.ID != 0)
                        {
                            if (I.ID != 750000)
                            {
                                GC.MyChar.RemoveItem(I);
                                Warehouse.Add(I);
                                GC.AddSend(Packets.AddWHItem(GC.MyChar, (ushort)NPC, I));
                                return;
                            }
                            else
                                GC.LocalMessage(2005, "This item cannot be deposited.");
                        }
                        else
                            GC.AddSend(Packets.ItemPacket(IUID, 0, 3));

                    }
                }
                catch { }
                return;
            }
            else if (Type == 2)
            {
                try
                {
                    if (GC.MyChar.Inventory.Count < 40)
                    {
                        Game.Item I = new Game.Item();
                        foreach (Game.Item II in Warehouse)
                            if (II.UID == IUID)
                                I = II;

                        if (I.ID != 0 && I.UID != 0)
                        {
                            GC.AddSend(Packets.RemoveWHItem(GC.MyChar, (ushort)NPC, I));
                            Warehouse.Remove(I);
                            GC.MyChar.AddItem(I); return;
                        }
                    }
                }
                catch { return; }
                return;
            }

            switch (NPC)
            {
                case 8: { GC.MyChar.Warehouses.TCWarehouse = Warehouse; break; }
                case 10012: { GC.MyChar.Warehouses.PCWarehouse = Warehouse; break; }
                case 10028: { GC.MyChar.Warehouses.ACWarehouse = Warehouse; break; }
                case 10011: { GC.MyChar.Warehouses.DCWarehouse = Warehouse; break; }
                case 10027: { GC.MyChar.Warehouses.BIWarehouse = Warehouse; break; }
                case 44: { GC.MyChar.Warehouses.MAWarehouse = Warehouse; break; }
                case 46: { GC.MyChar.Warehouses.MAWarehouse2 = Warehouse; break; }
                case 4101: { GC.MyChar.Warehouses.SCWarehouse = Warehouse; break; }
                case 2100: { GC.MyChar.Warehouses.HouseWH1 = Warehouse; break; }
                case 2101: { GC.MyChar.Warehouses.HouseWH2 = Warehouse; break; }
            }
            if (Warehouse != null/* && Warehouse.Count > 0*/)
            {
                GC.AddSend(Packets.SendWarehouse(GC.MyChar, (ushort)NPC));
                if (Warehouse.Count > 20)
                    for (int x = 20; x < Warehouse.Count; x++)
                        GC.AddSend(Packets.AddWHItem(GC.MyChar, (ushort)NPC, (Game.Item)Warehouse[x]));
            }
        }

        public static void Deposit(Main.GameClient GC, byte[] Data)
        {
            uint NPC = BitConverter.ToUInt32(Data, 4);
            if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map) || GC.MyChar.VipLevel > 2)
            {
                Dictionary<uint, Game.NPC> MapNPC = null;
                if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                    MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                if (MapNPC != null)
                {
                    if (MapNPC.ContainsKey(NPC))
                    {
                        Game.NPC N = (Game.NPC)MapNPC[NPC];
                        if (MyMath.PointDistance(N.Loc.X, N.Loc.Y, GC.MyChar.Loc.X, GC.MyChar.Loc.Y) <= 19 || GC.MyChar.VipLevel > 2)
                        {
                            uint Amount = BitConverter.ToUInt32(Data, 8);
                            if (GC.MyChar.Silvers >= Amount)
                            {
                                if (GC.MyChar.WHSilvers + Amount <= 2000000000)
                                {
                                    GC.MyChar.Silvers -= Amount;
                                    GC.MyChar.WHSilvers += Amount;
                                }
                                else GC.LocalMessage(2005, "You can't store more than 2,000,000,000 gold in your warehouse!");
                            }
                        }
                    }
                    else if (GC.MyChar.VipLevel > 2)
                    {
                        uint Amount = BitConverter.ToUInt32(Data, 8);
                        if (GC.MyChar.Silvers >= Amount)
                        {
                            if (GC.MyChar.WHSilvers + Amount <= 2000000000)
                            {
                                GC.MyChar.Silvers -= Amount;
                                GC.MyChar.WHSilvers += Amount;
                            }
                            else GC.LocalMessage(2005, "You can't store more than 2,000,000,000 gold in your warehouse!");
                        }
                    }
                }
                else if (GC.MyChar.VipLevel > 2)
                {
                    uint Amount = BitConverter.ToUInt32(Data, 8);
                    if (GC.MyChar.Silvers >= Amount)
                    {
                        if (GC.MyChar.WHSilvers + Amount <= 2000000000)
                        {
                            GC.MyChar.Silvers -= Amount;
                            GC.MyChar.WHSilvers += Amount;
                        }
                        else GC.LocalMessage(2005, "You can't store more than 2,000,000,000 gold in your warehouse!");
                    }
                }
            }
        }

        public static void Withdraw(Main.GameClient GC, byte[] Data)
        {
            uint NPC = BitConverter.ToUInt32(Data, 4);
            if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map) || GC.MyChar.VipLevel > 2)
            {
                Dictionary<uint, Game.NPC> MapNPC = null;
                if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                    MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                if (MapNPC != null)
                {
                    if (MapNPC.ContainsKey(NPC))
                    {
                        Game.NPC N = (Game.NPC)MapNPC[NPC];
                        if (MyMath.PointDistance(N.Loc.X, N.Loc.Y, GC.MyChar.Loc.X, GC.MyChar.Loc.Y) <= 19 || GC.MyChar.VipLevel > 2)
                        {
                            uint Amount = BitConverter.ToUInt32(Data, 8);
                            if (GC.MyChar.WHSilvers >= Amount)
                            {
                                if (GC.MyChar.Silvers + Amount <= 2000000000)
                                {
                                    GC.MyChar.Silvers += Amount;
                                    GC.MyChar.WHSilvers -= Amount;
                                }
                                else GC.LocalMessage(2005, "You can't have more than 2,000,000,000 gold in your inventory.");
                            }
                        }
                    }
                    else if (GC.MyChar.VipLevel > 2)
                    {
                        uint Amount = BitConverter.ToUInt32(Data, 8);
                        if (GC.MyChar.WHSilvers >= Amount)
                        {
                            if (GC.MyChar.Silvers + Amount <= 2000000000)
                            {
                                GC.MyChar.Silvers += Amount;
                                GC.MyChar.WHSilvers -= Amount;
                            }
                            else GC.LocalMessage(2005, "You can't have more than 2,000,000,000 gold in your inventory.");
                        }
                    }
                }
                else if (GC.MyChar.VipLevel > 2)
                {
                    uint Amount = BitConverter.ToUInt32(Data, 8);
                    if (GC.MyChar.WHSilvers >= Amount)
                    {
                        if (GC.MyChar.Silvers + Amount <= 2000000000)
                        {
                            GC.MyChar.Silvers += Amount;
                            GC.MyChar.WHSilvers -= Amount;
                        }
                        else GC.LocalMessage(2005, "You can't have more than 2,000,000,000 gold in your inventory.");
                    }
                }
            }
        }
    }
}
