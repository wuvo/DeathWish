using System;
using System.Collections.Generic;
using System.Text;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class Repair
    {
        public static void Handle(byte[] Data, Main.GameClient GC)
        {
            uint id = BitConverter.ToUInt32(Data, 4);
            Game.Item Repairing = GC.MyChar.FindInvItem(id);

            if (Repairing.CurDur == 0)
            {
                if (GC.MyChar.InventoryContains(1088001, 5))
                {
                    for (int i = 0; i < 5; i++)
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));

                    //GC.MyChar.RemoveItem(Repairing);
                    Repairing.MaxDur = Repairing.DBInfo.Durability;
                    Repairing.CurDur = Repairing.MaxDur;
                    GC.AddSend(Packets.UpdateItem(Repairing, 0));
                    //GC.MyChar.AddItem(Repairing);
                }
                else
                {
                    GC.LocalMessage(2005, "You don't have 5 meteors to repair this item!");
                }
            }
            else
            {
                if (Repairing.ID != 1050000 && Repairing.ID != 1050001 && Repairing.ID != 1050002 && Repairing.ID != 1051000)
                {
                    int nRecoverDurability = Math.Max(0, (Repairing.MaxDur - Repairing.CurDur));

                    if (nRecoverDurability == 0)
                        return;

                    uint nRepairCost = (uint)(Math.Max(1, (Repairing.DBInfo.Worth * nRecoverDurability / Repairing.MaxDur / 1.15)));
                    if (GC.MyChar.Silvers >= nRepairCost)
                    {
                        GC.MyChar.Silvers -= nRepairCost;
                        Repairing.MaxDur = Repairing.DBInfo.Durability;
                        Repairing.CurDur = Repairing.MaxDur;
                        GC.AddSend(Packets.UpdateItem(Repairing, 0));
                    }
                    else
                        GC.LocalMessage(2005, "You don`t have " + nRepairCost + " gold. Come back after you have enough!");
                }

            }
        }
        public static void HandleVipRepair(Main.GameClient GC)
        {
            GC.MyChar.Silvers = GC.MyChar.Silvers;//update the silvers
            int dbs = 0;
            int nRecoverDurability = 0;
            uint nRepairCost = 0;
            uint TotalCost = 0;
            Game.Item Repairing = new Game.Item();
            for (   byte i = 1; i < 10; i++)
            {
                if (i != 7)
                {
                    Repairing = GC.MyChar.Equips.Get(i);
                    if (Repairing.ID != 0 && Repairing.ID != 1050000 && Repairing.ID != 1050001 && Repairing.ID != 1050002 && Repairing.ID != 1051000)
                    {
                        nRecoverDurability = Math.Max(0, (Repairing.MaxDur - Repairing.CurDur));

                        if (nRecoverDurability == 0 || nRecoverDurability == Repairing.MaxDur)
                            continue;

                        nRepairCost = (uint)(Math.Max(1, (Repairing.DBInfo.Worth * nRecoverDurability / Repairing.MaxDur / 1.15)));
                        TotalCost += nRepairCost;
                        if (!Repairing.CanEquip(GC.MyChar))
                        {
                            if (nRecoverDurability >= 100 && nRecoverDurability <= 999)
                                dbs += 1;
                            else if (nRecoverDurability >= 1000 && nRecoverDurability <= 1999)
                                dbs += 3;
                            else if (nRecoverDurability >= 2000 && nRecoverDurability <= 2999)
                                dbs += 4;
                            else if (nRecoverDurability >= 3000 && nRecoverDurability <= 3999)
                                dbs += 5;
                            else if (nRecoverDurability >= 4000)
                                dbs += 6;
                        }
                       // uint RepairCost = (uint)(Repairing.DBInfo.Worth - Repairing.CurDur * Repairing.DBInfo.Worth / Repairing.DBInfo.Durability);
                    }
                    GC.AddSend(Packets.UpdateItem(Repairing, i));
                }
            }
            if (GC.MyChar.Silvers >= TotalCost)
            {
                if (dbs > 0)
                    if (GC.MyChar.InventoryContains(1088000, Convert.ToByte(dbs)))
                    {
                        for (byte db = 0; db < dbs; db++)
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                        GC.MyChar.Silvers -= TotalCost;
                        for (byte i = 1; i < 10; i++)
                        {
                            if (i != 7)
                            {
                                Repairing = GC.MyChar.Equips.Get(i);
                                nRecoverDurability = Math.Max(0, (Repairing.MaxDur - Repairing.CurDur));
                                if (Repairing.ID != 0 && Repairing.ID != 1050000 && Repairing.ID != 1050001 && Repairing.ID != 1050002 && Repairing.ID != 1051000)
                                {
                                    if (nRecoverDurability == 0 || nRecoverDurability == Repairing.MaxDur)
                                        continue;

                                    Repairing.MaxDur = Repairing.DBInfo.Durability;
                                    Repairing.CurDur = Repairing.MaxDur;
                                }
                                GC.AddSend(Packets.UpdateItem(Repairing, i));
                            }
                        }

                    }
                    else
                        GC.LocalMessage(2005, "You don`t have " + dbs + " DragonBalls. Try again when you have enough!");
                else
                {
                    GC.MyChar.Silvers -= TotalCost;
                    for (byte i = 1; i < 10; i++)
                    {
                        if (i != 7)
                        {
                            Repairing = GC.MyChar.Equips.Get(i);
                            if (Repairing.ID != 0 && Repairing.ID != 1050000 && Repairing.ID != 1050001 && Repairing.ID != 1050002 && Repairing.ID != 1051000)
                            {
                                nRecoverDurability = Math.Max(0, (Repairing.MaxDur - Repairing.CurDur));
                                if (nRecoverDurability == 0 || nRecoverDurability == Repairing.MaxDur)
                                    continue;

                                Repairing.MaxDur = Repairing.DBInfo.Durability;
                                Repairing.CurDur = Repairing.MaxDur;
                            }
                            GC.AddSend(Packets.UpdateItem(Repairing, i));
                        }
                    }
                }
            }
            else
                GC.LocalMessage(2005, "You don`t have " + TotalCost + " gold. Come back after you have enough!");
            
            GC.MyChar.Silvers = GC.MyChar.Silvers;//update the silvers
        }
    }
}