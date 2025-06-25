using Ultimate.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class Shops
    {
        public static void SellHandle(Main.GameClient GC, byte[] Data)
        {
            uint NPCID = BitConverter.ToUInt32(Data, 4);
            uint ItemUID = BitConverter.ToUInt32(Data, 8);
            if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
            {
                Dictionary<uint, Game.NPC> MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                if (MapNPC.ContainsKey(NPCID) && Database.Shops.ContainsKey(NPCID))
                {
                    Game.NPC N = (Game.NPC)MapNPC[NPCID];
                    if (MyMath.PointDistance(N.Loc.X, N.Loc.Y, GC.MyChar.Loc.X, GC.MyChar.Loc.Y) < 18)
                    {
                        Game.Item I = GC.MyChar.FindInvItem(ItemUID);
                        if (I.ID != 780001)
                        {
                            if (I.MaxDur > 0)
                                if (GC.MyChar.Silvers + (I.DBInfo.Worth * I.CurDur / I.MaxDur / 3) <= 2000000000)
                                {
                                    GC.MyChar.RemoveItem(I);
                                    GC.MyChar.Silvers += I.DBInfo.Worth * I.CurDur / I.MaxDur / 3;
                                    if (!World.GoldSource.ContainsKey("Shops"))
                                        World.GoldSource.Add("Shops", 0);
                                    World.GoldSource["Shops"] += I.DBInfo.Worth * I.CurDur / I.MaxDur / 3;
                                }
                                else GC.LocalMessage(2005, "You can't have more than 2kkk gold in your inventory.");
                        }
                    }
                }
            }
        }
        public static void BuyHandle(Main.GameClient GC, byte[] Data)
        {
            try
            {
                uint NPCID = BitConverter.ToUInt32(Data, 4);
                uint ItemID = BitConverter.ToUInt32(Data, 8);
                byte Amount = Data[20];
                if (Amount == 0)
                    Amount = 1;
                if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                {
                    Dictionary<uint, Game.NPC> MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                    if (MapNPC.ContainsKey(NPCID) || NPCID == 2888 && Database.Shops.ContainsKey(NPCID))
                    {
                        Game.NPC N = (Game.NPC)MapNPC[NPCID];
                        Shop S = (Shop)Database.Shops[NPCID];

                        if ((N != null && N.Loc.Map == GC.MyChar.Loc.Map && MyMath.PointDistance(N.Loc.X, N.Loc.Y, GC.MyChar.Loc.X, GC.MyChar.Loc.Y) < 18 || NPCID == 2888) && S.Items.Contains(ItemID))
                        {
                            DatabaseItem DBI = (DatabaseItem)Database.DatabaseItems[ItemID];
                            if (DBI.ID == 0)
                            {
                                Game.World.DebugAdd += "Error Database item = null, item id = " + ItemID + "\r\n";
                                return;
                            }
                            for (byte i = 0; i < Amount; i++)
                            {
                                if (GC.MyChar.Inventory.Count < 40)
                                {
                                    if (S.MoneyType == 0 && GC.MyChar.Silvers >= DBI.Worth)
                                    {
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.ID = ItemID;
                                        I.UID = (uint)Game.World.Rnd.Next(10000000);
                                        try
                                        {
                                            I.MaxDur = DBI.Durability;
                                            I.CurDur = I.MaxDur;
                                        }
                                        catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                        Game.ItemIDManipulation e = new Ultimate.Game.ItemIDManipulation(I.ID);
                                        if (e.Part(0, 2) == 11 || e.Part(0, 2) == 13 || e.Part(0, 3) == 123 || e.Part(0, 3) == 141 || e.Part(0, 3) == 142)
                                            I.Color = Ultimate.Game.Item.ArmorColor.Orange;
                                        GC.MyChar.AddItem(I);
                                        GC.MyChar.Silvers -= DBI.Worth;
                                    }
                                    else if (S.MoneyType == 1 && GC.MyChar.CPs >= DBI.CPsWorth)
                                    {
                                        Game.Item I = new Ultimate.Game.Item();
                                        I.ID = ItemID;
                                        I.UID = (uint)Game.World.Rnd.Next(10000000);
                                        try
                                        {
                                            I.MaxDur = DBI.Durability;
                                            I.CurDur = I.MaxDur;
                                        }
                                        catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }

                                        if (Game.ItemIDManipulation.Part(I.ID, 0, 2) == 73)
                                            I.Plus = Game.ItemIDManipulation.Digit(I.ID, 6);
                                        GC.MyChar.AddItem(I);
                                        GC.MyChar.CPs -= DBI.CPsWorth;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}
