using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class PersonalShops
    {
        public struct ItemValue
        {
            public byte MoneyType;
            public uint Value;
        }
        public class Shop
        {
            public Character Owner;
            public string Hawk = "";
            public Dictionary<uint, ItemValue> Items;
            // public SortedDictionary<uint, ItemValue> Items;
            public NPC NPCInfo;
            public uint UID;

            public Shop(Character C, uint Time)
            {
                Owner = C;
                Items = new Dictionary<uint, ItemValue>();
                // Items = new SortedDictionary<uint, ItemValue>();

                NPCInfo = new NPC();
                NPCInfo.EntityID = (uint)Program.Rnd.Next(102000, 106000);
                Dictionary<uint, NPC> MapNPC = World.H_NPCs[C.Loc.Map];
                while (MapNPC.ContainsKey(NPCInfo.EntityID) || World.H_PShops.ContainsKey(NPCInfo.EntityID)/* || World.H_Chars.ContainsKey(NPCInfo.EntityID)*/)
                    NPCInfo.EntityID = (uint)Program.Rnd.Next(102000, 106000);
                NPCInfo.Type = 400;
                NPCInfo.Flags = 14;
                NPCInfo.Loc = C.Loc;
                NPCInfo.Loc.X++;
                NPCInfo.Direction = 6;
                NPCInfo.Avatar = 0;

                UID = NPCInfo.EntityID;

                C.MyClient.AddSend(Packets.SpawnNamedNPC2(NPCInfo, Name));
                C.MyClient.AddSend(Packets.GeneralData(C.EntityID, UID, NPCInfo.Loc.X, NPCInfo.Loc.Y, 111, 6));
                //  C.MyClient.EndSend();

                World.H_PShops.TryAdd(UID, this);

                World.Spawn(this);
            }
            public void Close()
            {
                World.H_PShops.Remove(UID);
                World.Action(Owner, Packets.GeneralData(UID, 0, 0, 0, 135).Get);

                Owner.MyShop = null;
                MySQL.MySqlCommand dshop;
                dshop = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("shop", "seller", Owner.Name);
                dshop.Execute();
            }
            public bool AddItem(uint UID, uint Value, byte MoneyType)
            {
                // if (Owner.LastTrade.AddMilliseconds(5000) < DateTime.Now)
                // {
                Item I = Owner.FindInvItem(UID);

                Game.ItemIDManipulation I1 = new Ultimate.Game.ItemIDManipulation(I.ID);


                if (I.UID == UID && !Items.ContainsKey(UID) && Items.Count < 20)
                {
                    if (I1.Quality == Ultimate.Game.Item.ItemQuality.Normal)
                    {
                        string Normal = "Normal";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Normal).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    else if (I1.Quality == Ultimate.Game.Item.ItemQuality.Refined)
                    {
                        string Refined = "Refined";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Refined).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    else if (I1.Quality == Ultimate.Game.Item.ItemQuality.Unique)
                    {
                        string Unique = "Unique";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Unique).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    else if (I1.Quality == Ultimate.Game.Item.ItemQuality.Elite)
                    {
                        string Elite = "Elite";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Elite).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    else if (I1.Quality == Ultimate.Game.Item.ItemQuality.Super)
                    {
                        string Super = "Super";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Super).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    else
                    {
                        string Super = "Normal";
                        MySQL.MySqlCommand Vote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Vote.Insert("shop").Insert("uid", UID).Insert("plus", I.Plus).Insert("bless", I.Bless).Insert("quality", Super).Insert("itemid", I.ID).Insert("itemname", I.DBInfo.Name).Insert("soc1", (byte)I.Soc1).Insert("soc2", (byte)I.Soc2).Insert("price", Value).Insert("seller", Owner.Name.ToString().Split(':')[0].ToString()).Execute();
                    }
                    if (!I.FreeItem && (I.ID <= 721575 || I.ID >= 722721 || I.ID == 722384))
                    {
                        Items.Add(UID, new ItemValue() { Value = Value, MoneyType = 1 });
                        return true;
                    }
                    else
                    {
                        Owner.MyClient.LocalMessage(2005, "[Shop]You can't sell this item.");
                        return false;
                    }
                }
                //}
                return false;
            }
            public void Buy(uint UID, Character C)
            {
                if (Owner.MyClient.Soc.Connected)
                {
                    if (Owner != C && Items.ContainsKey(UID) && C.Inventory.Count < 40)
                    {
                        Item I = Owner.FindInvItem(UID);
                        if (I.ID != 0)
                        {
                            ItemValue Val = (ItemValue)Items[UID];
                            uint Costs = Val.Value;

                            if (Val.MoneyType == 1 && C.Silvers >= Costs)
                            {
                                if (Owner.Silvers + Costs <= 2000000000)
                                {
                                    C.Silvers -= Costs;
                                    Owner.Silvers += Costs;
                                    Owner.MyClient.AddSend(Packets.ChatMessage(13000, "[SHOP][INFO]", Owner.Name, C.Name + " has bought " + I.DBInfo.Name + " for " + Costs + " gold from you.", 2003, 0x7d3));
                                    //Owner.MyClient.LocalMessage(2001, C.Name + " has bought " + I.DBInfo.Name + " for " + Costs + " gold from you.");
                                    RemoveItem(UID, Packets.ItemPacket(UID, this.UID, 23).Get);
                                    MySQL.MySqlCommand dshop;
                                    dshop = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("shop", "uid", UID);
                                    dshop.Execute();

                                    if (!Owner.RemoveItem(ref I))//if (!Owner.RemoveItem(UID))
                                        Game.World.TradeAdd += Owner.Name + " cheated on shopping item uid: " + UID + " and was not sent to: " + C.Name + "\r\n";

                                    else
                                    {
                                        C.AddItem(ref I);
                                        Game.World.TradeAdd += "SHOP: " + C.Name + " has bought " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " for " + Costs + " gold from: " + Owner.Name + "\r\n";
                                        //     Game.World.TradeAdd += Owner.Name + " shopped " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " and accepted a trade with " + C.Name + "\r\n";
                                    }
                                }
                                else
                                {
                                    C.MyClient.LocalMessage(2005, "The seller can't hold more than 2kkk silvers.");
                                    Owner.MyClient.AddSend(Packets.ChatMessage(13000, "[SHOP][INFO]", Owner.Name, "You can't sell anymore items because you might get more than 2kkk silvers!.", 2003, 0x7d3));
                                    Owner.MyClient.LocalMessage(2005, "You can't sell anymore items because you might get more than 2kkk silvers!.");
                                }
                                //  Owner.RemoveItem(I);
                                //  C.AddItem(I);
                                //  Game.World.TradeAdd += "SHOP: " + C.Name + " has bought " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " for " + Costs + " gold from: " + Owner.Name + "\r\n";

                            }
                        }
                    }
                }
                else
                {
                    C.MyClient.LocalMessage(2000, "The shop doesn't exist!");
                    Owner.MyShop.Close();
                }
            }
            public void RemoveItem(uint UID, byte[] Data)
            {

                if (Items.ContainsKey(UID))
                    Items.Remove(UID);

                MySQL.MySqlCommand dshop;
                dshop = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("shop", "uid", UID);
                dshop.Execute();

                foreach (Character C in Owner.ScreenChars.Values)//World.H_Chars.Values
                                                                 // if (C.Loc.Map == Owner.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Owner.Loc.X, Owner.Loc.Y, 28))
                    C.MyClient.AddSend(Data);
            }
            public void SendItems(Main.GameClient C)
            {
                foreach (KeyValuePair<uint, ItemValue> DE in Items)
                // for (int i = 0; i < Items.Count; i ++)
                {
                    //KeyValuePair<uint,ItemValue> DE = Items.
                    Item I = Owner.FindInvItem((uint)DE.Key);
                    if (I.ID != 0)
                        C.AddSend(Packets.AddStallItem(I, (ItemValue)DE.Value, UID));
                    MySQL.MySqlCommand dshop;
                    dshop = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("shop", "uid", UID);
                    dshop.Execute();
                }
            }
            public string Name
            {
                get
                {
                    return Owner.Name;
                }
            }
        }
    }
}
