using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class Trade
    {
        static bool Cancel = true;
        static void CancelTrade(Main.GameClient C)
        {
            if (C.MyChar.Trading)
            {
                Character Who = null;
                if (World.H_Chars.ContainsKey(C.MyChar.TradingWith))
                    Who = World.H_Chars[C.MyChar.TradingWith];

                if (Who != null)
                {
                    Who.MyClient.AddSend(Packets.TradePacket(C.MyChar.TradingWith, 5));
                    Who.Trading = false;
                    Who.TradingWith = 0;
                    Who.TradeSide = new System.Collections.Generic.List<uint>(20);
                    Who.TradingSilvers = 0;
                    Who.ClickedOK = false;
                    Who.Silvers = Who.Silvers;//update the silvers
                    Who.CPs = Who.CPs;//update the cps
                    Who.MyClient.AddSend(Packets.ChatMessage(Who.MyClient.MessageID, "SYSTEM", Who.Name, "Trading failed!", 2005, 0));
                    Game.World.TradeAdd += Who.Name + " Trading failed! " + C.MyChar.Name + " ~ " + Who.Loc.Map + " ~ " + Who.Loc.X + " ~ " + Who.Loc.Y + " ~ " + DateTime.Now + "\r\n";
                }
                C.AddSend(Packets.TradePacket(C.MyChar.TradingWith, 5));
                C.MyChar.Trading = false;
                C.MyChar.TradingWith = 0;
                C.MyChar.TradeSide = new System.Collections.Generic.List<uint>(20);
                C.MyChar.TradingSilvers = 0;
                C.MyChar.ClickedOK = false;
                C.MyChar.CPs = C.MyChar.CPs;//update the cps
                C.MyChar.Silvers = C.MyChar.Silvers;//update the silvers
                C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "Trading failed!", 2005, 0));
                Game.World.TradeAdd += C.MyChar.Name + " Trading failed! " + Who.Name + " ~ " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";


            }
        }
        public static void Handle(Main.GameClient C, byte[] Data)
        {
            uint UID = BitConverter.ToUInt32(Data, 4);
            byte Type = Data[8];

            switch (Type)
            {
                case 1:
                    {
                        Character Who = null; if (World.H_Chars.ContainsKey(UID)) Who = World.H_Chars[UID];
                        if (Who != null)
                            //if (!C.GM || C.PM || (C.GM && Who.MyClient.GM))
                                if (!Who.Trading)
                                {
                                    if (!C.MyChar.Trading)
                                    {
                                        // DateTime Now = DateTime.Now;
                                        // if (C.MyChar.LastTrade.AddMilliseconds(5000) < Now)
                                        //{
                                        // if (Who.LastTrade.AddMilliseconds(5000) < Now)
                                        //{
                                        if (Who.EntityID != C.MyChar.TradingWith)
                                        {
                                            C.MyChar.TradingWith = UID;
                                            if (Who.EntityID == C.MyChar.TradingWith && Who.TradingWith == C.MyChar.EntityID)
                                            {
                                                Who.MyClient.AddSend(Packets.TradePacket(C.MyChar.EntityID, 3));
                                                C.AddSend(Packets.TradePacket(Who.EntityID, 3));
                                                C.MyChar.Trading = true;
                                                Who.Trading = true;
                                                break;
                                            }
                                            else
                                            {
                                                C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]Request for trading has been sent out.", 2005, 0));
                                            Game.World.TradeAdd += C.MyChar.Name + " Request for trading has been sent out. " + Who.Name + " ~ " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";
                                              Who.MyClient.AddSend(Packets.TradePacket(C.MyChar.EntityID, 1));
                                            }
                                        }
                                        if (Who.EntityID == C.MyChar.TradingWith && Who.TradingWith == C.MyChar.EntityID)//
                                        {
                                            Who.MyClient.AddSend(Packets.TradePacket(C.MyChar.EntityID, 3));
                                            C.AddSend(Packets.TradePacket(Who.EntityID, 3));
                                            C.MyChar.Trading = true;
                                            Who.Trading = true;
                                        }
                                        // }
                                        //else C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]The target has recently ended a trade please wait a few seconds.", 2005, 0));
                                        //  }
                                        // else C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]Wait a few seconds between trades.", 2005, 0));
                                    }
                                    else
                                        C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]Close the current trade before you take another one.", 2005, 0));
                            }
                                else
                                    C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]The target is trading with someone else.", 2005, 0));
                     
                        break;
                    }
                case 2:
                    {
                        if (Cancel)
                            CancelTrade(C);
                        break;
                    }
                case 6:
                    {
                        Character Who = null;
                        if (World.H_Chars.ContainsKey(C.MyChar.TradingWith))
                            Who = World.H_Chars[C.MyChar.TradingWith];
                        if (Who != null)
                        {
                            if (C.MyChar.TradeSide.Count < 20)
                            {
                                if (Who.Inventory.Count + C.MyChar.TradeSide.Count < 40)
                                {
                                    Game.Item I = C.MyChar.FindInvItem(UID);
                                    if (!I.FreeItem && I.ID != 750000 && (I.ID <= 721575 || I.ID >= 722721 || I.ID == 722384))
                                    {
                                        Who.MyClient.AddSend(Packets.TradeItem(I));
                                        C.MyChar.TradeSide.Add(I.UID);
                                    }
                                    else
                                    {
                                        C.AddSend(Packets.TradePacket(UID, 11));
                                        C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]This item cannot be traded.", 2005, 0));
                                        C.LocalMessage(2005, "[Trade]This item cannot be traded.");
                                        Game.World.TradeAdd += C.MyChar.Name + " This item cannot be traded. " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";

                                    }
                                    // Program.WriteTrade(C.MyChar.Name + " added " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " in trade menu with " + Who.Name);

                                }
                                else
                                {
                                    C.AddSend(Packets.TradePacket(UID, 11));
                                    C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]Your trade partner can't hold any more items.", 2005, 0));
                                    Who.MyClient.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "[Trade]The one your trading with cant add anymore items on the table because you have no room in your inventory.", 2005, 0));
                                    Game.World.TradeAdd += C.MyChar.Name + " inventory full gibi. " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";
                                }
                            }

                        }
                        break;
                    }
                case 7:
                    {
                        if (UID <= C.MyChar.Silvers)
                        {
                            C.MyChar.TradingSilvers = UID;
                            Character Who = null; if (World.H_Chars.ContainsKey(C.MyChar.TradingWith)) Who = World.H_Chars[C.MyChar.TradingWith];
                            if (Who != null)
                                Who.MyClient.AddSend(Packets.TradePacket(UID, 8));
                            //  Program.WriteTrade(C.MyChar.Name + " added " + UID + " gold in trade menu with " + Who.Name);
                        }

                        break;
                    }
                case 13:
                    {
                        //cps
                        break;
                    }
                case 10:
                    {

                        Character Who = null; if (World.H_Chars.ContainsKey(C.MyChar.TradingWith)) Who = World.H_Chars[C.MyChar.TradingWith];
                        if (C.Soc.Connected)
                            if (Who != null)
                                if (Who.MyClient.Soc.Connected)
                                    if (Who.ClickedOK)
                                    {
                                        if (C.MyChar.Silvers >= C.MyChar.TradingSilvers && Who.Silvers >= Who.TradingSilvers)
                                        {
                                            Cancel = false;
                                            Who.MyClient.AddSend(Packets.TradePacket(C.MyChar.TradingWith, 5));
                                            C.AddSend(Packets.TradePacket(C.MyChar.TradingWith, 5));
                                            foreach (uint Id in C.MyChar.TradeSide)
                                            {
                                                Game.Item I = C.MyChar.FindInvItem(Id);
                                                if (I.ID == 0)
                                                {
                                                    Cancel = true;
                                                    CancelTrade(C);

                                                }
                                            }
                                            if (!Cancel)
                                            {
                                                foreach (uint Id in Who.TradeSide)
                                                {
                                                    /*i++;
                                                    if (i == Who.TradeSide.Count)
                                                        T2 = true;*/
                                                    Game.Item I = Who.FindInvItem(Id);
                                                    if (I.ID == 0)
                                                    {
                                                        Cancel = true;
                                                        CancelTrade(C);


                                                    }
                                                }
                                                if (!Cancel)
                                                {
                                                    if (C.MyChar.Silvers < C.MyChar.TradingSilvers || Who.Silvers < Who.TradingSilvers)
                                                    {
                                                        Cancel = true;
                                                        CancelTrade(C);
                                                        Game.World.TradeAdd += "SILVER TRADE PROBLEM BETWEEN: " + C.MyChar.Name + " AND " + Who.Name + "C.Silvers: " + C.MyChar.Silvers + "C.TradingSilvers: " + C.MyChar.TradingSilvers + " Who.Silvers: " + Who.Silvers + " Who.TradingSilvers: " + Who.TradingSilvers + "\r\n";
                                                    }
                                                    if (!Cancel)
                                                    {
                                                        if ((Who.Silvers - Who.TradingSilvers + C.MyChar.TradingSilvers <= 2000000000) && (C.MyChar.Silvers - C.MyChar.TradingSilvers + Who.TradingSilvers <= 2000000000))
                                                        {
                                                            if (C.MyChar.TradingSilvers > 0)
                                                                Game.World.TradeAdd += C.MyChar.Name + " added " + C.MyChar.TradingSilvers + " gold in trade menu and accepted with " + Who.Name + " ~ " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";
                                                     
                                                            if (Who.TradingSilvers > 0)
                                                                Game.World.TradeAdd += Who.Name + " added " + Who.TradingSilvers + " gold in trade menu and accepted with " + C.MyChar.Name + " ~ " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";

                                                            Who.Silvers -= Who.TradingSilvers;
                                                            C.MyChar.Silvers -= C.MyChar.TradingSilvers;
                                                            Who.Silvers += C.MyChar.TradingSilvers;
                                                            C.MyChar.Silvers += Who.TradingSilvers;
                                                            // Who.TradedGold = Who.TradingSilvers;
                                                            // C.MyChar.TradedGold = C.MyChar.TradingSilvers;

                                                            //  Who.TradeReverse = new System.Collections.Hashtable(20);
                                                            // C.MyChar.TradeReverse = new System.Collections.Hashtable(20);

                                                            // bool T1 = false;
                                                            // bool T2 = false;
                                                            // int i = 0;
                                                            foreach (uint Id in C.MyChar.TradeSide)
                                                            {
                                                                Game.Item I = C.MyChar.FindInvItem(Id);
                                                                /*if (!C.MyChar.RemoveItem(Id))
                                                                    Game.World.TradeAdd += C.MyChar.Name + " cheated on trading item uid: " + Id + " and was not sent to: " + Who.Name + "\r\n";
                                                                else
                                                                {
                                                                    Who.AddItem(ref I);
                                                                    Game.World.TradeAdd += C.MyChar.Name + " added " + I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + " and accepted a trade with " + Who.Name + "\r\n";
                                                                }*/
                                                                if (C.MyChar.RemoveItem(ref I))
                                                                {
                                                                    Who.AddItem(ref I);
                                                                    Game.World.TradeAdd += C.MyChar.Name + " added " + I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + " and accepted a trade with " + Who.Name + " ~ "  + DateTime.Now + "\r\n";
                                                                }
                                                                else Game.World.TradeAdd += C.MyChar.Name + " cheated on trading item uid: " + Id + " and was not sent to: " + Who.Name + "\r\n";
                                                            }
                                                            // i = 0;
                                                            foreach (uint Id in Who.TradeSide)
                                                            {
                                                                Game.Item I = Who.FindInvItem(Id);
                                                                /* if (!Who.RemoveItem(Id))
                                                                     Game.World.TradeAdd += Who.Name + " cheated on trading item uid: " + Id + " and was not sent to: " + C.MyChar.Name + "\r\n";
                                                                 else
                                                                 {
                                                                     C.MyChar.AddItem(ref I);
                                                                     Game.World.TradeAdd += Who.Name + " added " + I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + " and accepted a trade with " + C.MyChar.Name + "\r\n";
                                                                 }*/
                                                                if (Who.RemoveItem(ref I))
                                                                {
                                                                    C.MyChar.AddItem(ref I);
                                                                    Game.World.TradeAdd += Who.Name + " added " + I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + " and accepted a trade with " + C.MyChar.Name + " ~ " + DateTime.Now + "\r\n";
                                                                }
                                                                else Game.World.TradeAdd += Who.Name + " cheated on trading item uid: " + Id + " and was not sent to: " + C.MyChar.Name + "\r\n";
                                                            }
                                                            /* if (C.MyChar.TradeSide.Count > 0 || Who.TradeSide.Count > 0 || C.MyChar.TradingSilvers > 0 || Who.TradingSilvers > 0)
                                                             {
                                                                 Who.GetRevertedItems = false;
                                                                 C.MyChar.GetRevertedItems = false;
                                                                 Who.LastTrade = DateTime.Now;
                                                                 C.MyChar.LastTrade = DateTime.Now;
                                                             }*/
                                                            Who.Trading = false;
                                                            Who.OldTradingWith = Who.TradingWith;
                                                            Who.OldTradingWithName = C.MyChar.Name;
                                                            Who.TradingWith = 0;
                                                            Who.TradeSide = new List<uint>(20);
                                                            Who.TradingSilvers = 0;
                                                            Who.ClickedOK = false;
                                                            Who.MyClient.AddSend(Packets.ChatMessage(Who.MyClient.MessageID, "SYSTEM", Who.Name, "Trading succeeded!", 2005, 0));
                                                            Game.World.TradeAdd += Who.Name + " Trading succeeded! " + C.MyChar.Name + " ~ " + Who.Loc.Map + " ~ " + Who.Loc.X + " ~ " + Who.Loc.Y + " ~ " + DateTime.Now + "\r\n";

                                                            C.MyChar.Trading = false;
                                                            C.MyChar.OldTradingWith = C.MyChar.TradingWith;
                                                            C.MyChar.OldTradingWithName = Who.Name;
                                                            C.MyChar.TradingWith = 0;
                                                            C.MyChar.TradeSide = new List<uint>(20);
                                                            C.MyChar.TradingSilvers = 0;
                                                            C.MyChar.ClickedOK = false;
                                                            C.AddSend(Packets.ChatMessage(C.MessageID, "SYSTEM", C.MyChar.Name, "Trading succeeded!", 2005, 0));
                                                            Game.World.TradeAdd += C.MyChar.Name + " Trading succeeded! " + Who.Name + " ~ " + C.MyChar.Loc.Map + " ~ " + C.MyChar.Loc.X + " ~ " + C.MyChar.Loc.Y + " ~ " + DateTime.Now + "\r\n";
                                                            Cancel = true;
                                                        }
                                                        else
                                                        {
                                                            C.LocalMessage(2005, "You can't hold more than 2,000,000,000 silvers in your inventory!");
                                                            Who.MyClient.LocalMessage(2005, "You can't hold more than 2,000,000,000 silvers in your inventory!");
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        C.MyChar.ClickedOK = true;
                                        Who.MyClient.AddSend(Packets.TradePacket(0, 10));
                                    }

                        break;
                    }
            }
        }
    }
}
