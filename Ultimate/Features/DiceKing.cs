using Ultimate.Game;
using Ultimate.Main;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Ultimate.Features
{
    public class DiceKing
    {
        public static ConcurrentDictionary<uint, GameClient> Players = new ConcurrentDictionary<uint, GameClient>();
        public static Dictionary<string, uint> Results = new Dictionary<string, uint>();
        public static Random Random = new Random();
        public static bool Finish = false;
        public static void Announce(byte[] Packet)
        {
            foreach (GameClient GC in Players.Values)
                    GC.AddSend(Packet);
        }
        public static void Announce(COPacket P, GameClient C)
        {
            foreach (GameClient GC in Players.Values)
            {
                if (C != null)
                {
                    if (GC.MyChar.EntityID != C.MyChar.EntityID)
                        GC.AddSend(P);
                }
                else
                    GC.AddSend(P);
            }

        }

        public static void EndDice()
        {
            if (World.DiceKing.Status == MsgDice.Stage.Betting /*&& World.DiceKing.Action == MsgDice.Mode.BeginGamble*/)
            {
                World.DiceKing.Action = MsgDice.Mode.EndGamble;
                World.DiceKing.Seconds = 0;
                Announce(Packets.MsgDice(World.DiceKing).Get);

                long minBet = 0;
                byte Number = 0;
                
                foreach (KeyValuePair<byte, MsgDice.Bets> Bet in World.DiceKing.Bet)
                {
                    if (Bet.Key != 0 && Bet.Key != 1)
                    {
                        if (minBet == 0 && Number == 0)
                        {
                            Number = Bet.Key;
                            minBet = (Bet.Value.Amount * Bet.Value.Times);
                            if ((Bet.Key <= 10 || Bet.Key > 18 && Bet.Key < 30) && World.DiceKing.Bet.ContainsKey(0))
                                minBet += World.DiceKing.Bet[0].Amount * 2;
                            else if ((Bet.Key > 10 && Bet.Key < 18 || Bet.Key > 30) && World.DiceKing.Bet.ContainsKey(1))
                                minBet += World.DiceKing.Bet[1].Amount * 2;
                        }
                        if (Bet.Key <= 10 || Bet.Key == 23 || Bet.Key == 26 || Bet.Key == 29)
                        {
                            long toAdd = 0;
                            if (Bet.Key > 10)
                                if (World.DiceKing.Bet.ContainsKey((byte)(Bet.Key - 20)))
                                    toAdd += World.DiceKing.Bet[(byte)(Bet.Key - 20)].Amount * World.DiceKing.Bet[(byte)(Bet.Key - 20)].Times;
                            if (World.DiceKing.Bet.ContainsKey(0))
                                toAdd += World.DiceKing.Bet[0].Amount * 2;

                            if (minBet > ((Bet.Value.Amount * Bet.Value.Times) + toAdd))
                            {
                                minBet = Bet.Value.Amount * Bet.Value.Times + toAdd;
                                Number = Bet.Key;
                                if (minBet == 0 && MyMath.ChanceSuccess(40))
                                    break;
                            }
                            else if (minBet == ((Bet.Value.Amount * Bet.Value.Times) + toAdd) && MyMath.ChanceSuccess(55))
                            {
                                minBet = Bet.Value.Amount * Bet.Value.Times + toAdd;
                                Number = Bet.Key;
                                if (minBet == 0 && MyMath.ChanceSuccess(40))
                                    break;
                            }
                        }
                        else
                        {
                            long toAdd = 0;
                            if (Bet.Key > 18)
                                if (World.DiceKing.Bet.ContainsKey((byte)(Bet.Key - 20)))
                                    toAdd += World.DiceKing.Bet[(byte)(Bet.Key - 20)].Amount * World.DiceKing.Bet[(byte)(Bet.Key - 20)].Times;
                            if (World.DiceKing.Bet.ContainsKey(1))
                                toAdd += World.DiceKing.Bet[1].Amount * 2;

                            if (minBet > ((Bet.Value.Amount * Bet.Value.Times) + toAdd))
                            {
                                minBet = Bet.Value.Amount * Bet.Value.Times + toAdd;
                                Number = Bet.Key;
                                if (minBet == 0 && MyMath.ChanceSuccess(40))
                                    break;
                            }
                            else if (minBet == ((Bet.Value.Amount * Bet.Value.Times) + toAdd) && MyMath.ChanceSuccess(55))
                            {
                                minBet = Bet.Value.Amount * Bet.Value.Times + toAdd;
                                Number = Bet.Key;
                                if (minBet == 0 && MyMath.ChanceSuccess(40))
                                    break;
                            }
                        }
                    }
                }

                //var _stopwatch = new System.Diagnostics.Stopwatch();
                //_stopwatch.Start();

                if (Number < 18)
                {
                    while (Number != (byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3))
                    {
                        World.DiceKing.Dice = (byte)Random.Next(1, 7);
                        World.DiceKing.Dice2 = (byte)Random.Next(1, 7);
                        World.DiceKing.Dice3 = (byte)Random.Next(1, 7);
                    }
                }
                else
                {
                    while (World.DiceKing.Dice3 != World.DiceKing.Dice2 || World.DiceKing.Dice3 != World.DiceKing.Dice || Number != ((byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3) + 20) || World.DiceKing.Dice == 0)
                    {
                        World.DiceKing.Dice = (byte)Random.Next(1, 7);
                        World.DiceKing.Dice2 = (byte)Random.Next(1, 7);
                        World.DiceKing.Dice3 = (byte)Random.Next(1, 7);
                    }
                }
               

                //_stopwatch.Stop();
                //TimeSpan T = _stopwatch.Elapsed;
                //Console.WriteLine(T);

                //if (World.DiceKing.Dice3 == World.DiceKing.Dice2 && World.DiceKing.Dice3 == World.DiceKing.Dice)
                //    World.DiceKing.Dice3 = (byte)Random.Next(1, 7);
                
                World.DiceKing.UnKnown = (byte)Random.Next(1, 7);
                World.DiceKing.Status = MsgDice.Stage.Results;
            }
            else if (World.DiceKing.Status == MsgDice.Stage.Results)
            {
                World.DiceKing.Status = MsgDice.Stage.End;
                World.DiceKing.Seconds = 1;
                World.DiceKing.Action = MsgDice.Mode.ResultGamble;
                Announce(Packets.MsgDice(World.DiceKing).Get);
                World.DiceKing.Seconds = 0;
                Results.Clear();

                foreach (var C in Players.Values)
                {
                    if (C.MyChar.Loc.Map == 1036)
                    {
                        if (C.MyChar.MsgDice != null)
                        {
                            uint Reward = 0;
                            if (World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3 <= 10)
                            {
                                if (C.MyChar.MsgDice.Bet.ContainsKey(0))
                                    Reward += C.MyChar.MsgDice.Bet[0].Amount * C.MyChar.MsgDice.Bet[0].Times;
                            }
                            else if (C.MyChar.MsgDice.Bet.ContainsKey(1))
                                Reward = C.MyChar.MsgDice.Bet[1].Amount * C.MyChar.MsgDice.Bet[1].Times;

                            if (C.MyChar.MsgDice.Bet.ContainsKey((byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3)))
                                Reward += C.MyChar.MsgDice.Bet[(byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3)].Amount * C.MyChar.MsgDice.Bet[(byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3)].Times;

                            if (World.DiceKing.Dice == World.DiceKing.Dice2 && World.DiceKing.Dice == World.DiceKing.Dice3)
                            {
                                var Sum = (byte)((byte)(World.DiceKing.Dice + World.DiceKing.Dice2 + World.DiceKing.Dice3) + 20);
                                if (C.MyChar.MsgDice.Bet.ContainsKey(Sum))
                                    Reward += C.MyChar.MsgDice.Bet[Sum].Amount * C.MyChar.MsgDice.Bet[Sum].Times;
                            }

                            Announce(Packets.StringPacket(Reward, StringType.DiceBonus, $"{C.MyChar.Name}{C.AuthInfo.Status}").Get);
                            Results.Add(C.MyChar.Name + C.AuthInfo.Status, Reward);
                            C.MyChar.Silvers += Reward;

                            if (!World.GoldSource.ContainsKey("DiceKing"))
                                World.GoldSource.Add("DiceKing", 0);

                            World.GoldSource["DiceKing"] += Reward;
                            World.DiceKingTurnOver -= Convert.ToInt32(Reward);
                            C.MyChar.MsgDice.Bet.Clear();
                        }
                    }
                    else
                    {
                        Players.Remove(C.MyChar.EntityID);
                        C.MyChar.MsgDice = null;
                    }
                }
                if (Results.Count > 0)
                {
                    var myList2 = new Dictionary<string, uint>();
                    foreach (KeyValuePair<string, uint> S in Results)
                        if (S.Value >= 100000)
                            myList2.Add(S.Key, S.Value);
                    if (myList2.Count > 0)
                    {
                        var myList = myList2.ToList();
                        myList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                        World.SendMsgToAll("SYSTEM", $"{myList.First().Key} has won {myList.First().Value.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture)} silvers from DiceKing at Market! Are you that lucky?", 2011, 0);
                    }
                }
            }
            else if (World.DiceKing.Status == MsgDice.Stage.End)
            {
                World.DiceKing.Bet.Clear();
                World.DiceKing.Seconds = 50;
                World.DiceKing.Action = MsgDice.Mode.BeginGamble;
                World.DiceKing.Dice = 0;
                World.DiceKing.Dice2 = 0;
                World.DiceKing.Dice3 = 0;
                Announce(Packets.MsgDice(World.DiceKing).Get);

                for (byte a = 4; a < 39; a++)
                {
                    if (a <= 17 || (a > 22 && a % 3 == 2))
                    {
                        MsgDice.Bets Bet = new MsgDice.Bets();
                        Bet.Amount = 0;
                        switch (a)
                        {
                            case 23:
                            case 26:
                            case 29:
                            case 32:
                            case 35:
                            case 38:
                                Bet.Times = 216;
                                break;

                            case 4:
                            case 17:
                                Bet.Times = 72;
                                break;
                            case 5:
                            case 16:
                                Bet.Times = 36;
                                break;
                            case 6:
                            case 15:
                                Bet.Times = 21;
                                break;
                            case 7:
                            case 14:
                                Bet.Times = 14;
                                break;
                            case 8:
                            case 13:
                                Bet.Times = 10;
                                break;
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                                Bet.Times = 8;
                                break;
                        }

                        World.DiceKing.Bet.TryAdd(a, Bet);
                    }
                }

                World.DiceKing.Status = MsgDice.Stage.Betting;
            }
        }
        public static void AddPlayer(GameClient GC, byte[] Data) 
        {
            GC.MyChar.MsgDice = new MsgDice();
            GC.MyChar.MsgDice.Seconds = World.DiceKing.Seconds;
            GC.MyChar.MsgDice.Action = MsgDice.Mode.BeginGamble;
            GC.AddSend(Packets.MsgDice(GC.MyChar.MsgDice));

            GC.MyChar.MsgDice.ID = BitConverter.ToUInt16(Data, 4);
            GC.AddSend(Data);

            foreach (KeyValuePair<byte, MsgDice.Bets> Bet in World.DiceKing.Bet)
            {
                var Type = Bet.Key;
                if (Type >= 23)
                    Type = (byte)((Type - 20) / 3 + 1);
                else if (Type > 1)
                    Type = (byte)(Type + 4);

                var Amount = Bet.Value.Amount;
                var TenK = Amount / 10000;
                Amount = Amount - (TenK * 10000);
                var OneK = Amount / 1000;
                Amount = Amount - (OneK * 1000);
                var Silvers = Amount / 100;

                for (int a = 0; a < TenK; a++)
                {
                    var Bet1 = new MsgDice();
                    Bet1.Seconds = 1;//byte 5
                    Bet1.ID = World.DiceKing.ID;//uint32 8
                    Bet1.Number = Type; //DiceType uint32  12
                    Bet1.Amount += 10000;//DiceData uint32 16

                    Bet1.Action = MsgDice.Mode.PlaceChip;
                    GC.AddSend(Packets.MsgDice(Bet1).Get);
                }
                for (int a = 0; a < OneK; a++)
                {
                    var Bet1 = new MsgDice();
                    Bet1.Seconds = 1;//byte 5
                    Bet1.ID = World.DiceKing.ID;//uint32 8
                    Bet1.Number = Type; //DiceType uint32  12
                    Bet1.Amount += 1000;//DiceData uint32 16

                    Bet1.Action = MsgDice.Mode.PlaceChip;
                    GC.AddSend(Packets.MsgDice(Bet1).Get);
                }
                for (int a = 0; a < Silvers; a++)
                {
                    var Bet1 = new MsgDice();
                    Bet1.Seconds = 1;//byte 5
                    Bet1.ID = World.DiceKing.ID;//uint32 8
                    Bet1.Number = Type; //DiceType uint32  12
                    Bet1.Amount += 100;//DiceData uint32 16

                    Bet1.Action = MsgDice.Mode.PlaceChip;
                    GC.AddSend(Packets.MsgDice(Bet1).Get);
                }
            }

            foreach (GameClient GC2 in Players.Values)
            {
                string Name = "";
                Name = GC2.MyChar.Name + GC2.AuthInfo.Status;
                GC.AddSend(Packets.StringPacket(GC.MyChar.MsgDice.ID, StringType.AddDicePlayer, Name));
            }

            string N = "";
            for (int a = 11; a < 11 + Data[10]; a++)
                N += Convert.ToChar(Data[a]);

            Announce(Packets.StringPacket(BitConverter.ToUInt16(Data, 4), StringType.AddDicePlayer, N).Get);
            foreach (KeyValuePair<string, uint> Win in Results)
                Announce(Packets.StringPacket(Win.Value, StringType.DiceBonus, $"{Win.Key}").Get);

            DiceKing.Players.TryAdd(GC.MyChar.EntityID, GC);
        }
        public static void RemovePlayer(GameClient GC, byte[] Data)
        {
            Announce(Data);
            if (Players.ContainsKey(GC.MyChar.EntityID))
                Players.Remove(GC.MyChar.EntityID);

            if (GC.MyChar.MsgDice == null)
                return;
            else
                GC.MyChar.MsgDice = null;

            //GC.AddSend(Data);

            //string Name = "";
            //int totalWords = Data[9], Length = 0, Count = 0;
            //for (int a = 10; a < 10 + Data[9] + Length; a++)
            //{
            //    if (Count == 0 && totalWords > 0)
            //    {
            //        Name += Convert.ToChar(Data[a]);
            //        Length = Data[a] + Length;
            //        Count = Data[a];
            //        totalWords--;
            //    }
            //    else if (Count > 0)
            //    {
            //        Name += Convert.ToChar(Data[a]);
            //        Count--;
            //    }
            //}
            //Announce(Packets.StringGuild(BitConverter.ToUInt16(Data, 4), Data[8], Name, Data[9]), GC);
        }
    }
    public class MsgDice
    {
        public MsgDice(bool World = false)
        {
            if (World)
            {
                for (byte a = 4; a < 39; a++)
                {
                    if (a <= 17 || (a > 22 && a % 3 == 2))
                    {
                        MsgDice.Bets _bet = new MsgDice.Bets();
                        _bet.Amount = 0;
                        switch (a)
                        {
                            case 23:
                            case 26:
                            case 29:
                            case 32:
                            case 35:
                            case 38:
                                _bet.Times = 216;
                                break;

                            case 4:
                            case 17:
                                _bet.Times = 72;
                                break;
                            case 5:
                            case 16:
                                _bet.Times = 36;
                                break;
                            case 6:
                            case 15:
                                _bet.Times = 21;
                                break;
                            case 7:
                            case 14:
                                _bet.Times = 14;
                                break;
                            case 8:
                            case 13:
                                _bet.Times = 10;
                                break;
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                                _bet.Times = 8;
                                break;
                        }

                       Bet.TryAdd(a, _bet);
                    }
                }
            }
        }
        public enum Mode : byte
        {
            PlaceChip,
            ConfirmPlaceChip,
            CancelChip,
            CancelChipConfirm,
            BeginGamble,
            EndGamble,
            ResultGamble,
        }
        public enum Stage
        {
            Betting,
            Results,
            End
        }
        public struct Bets
        {
            public uint Amount;
            public byte Times;
        }
        public Mode Action;
        public Stage Status;
        public byte Seconds;
        public ushort ID;
        public byte Number;
        public ushort Amount;
        public byte Dice;
        public byte Dice2;
        public byte Dice3;
        public byte UnKnown;
        public DateTime LastBet;
        public ConcurrentDictionary<byte, Bets> Bet = new ConcurrentDictionary<byte, Bets>();

        public static void Process(byte[] packet, Main.GameClient client)
        {
            if (World.DiceKing.Status != Stage.Betting)
            {
                client.LocalMessage(2005, "Bettings closed!");
                return;
            }
            if (client.MyChar.Silvers < BitConverter.ToUInt16(packet, 16))
                return;
            if (client.MyChar.MsgDice != null && client.MyChar.MsgDice.LastBet.AddMilliseconds(200) >= DateTime.Now)
            {
                client.LocalMessage(2005, "Please wait for a while!");
                return;
            }

            client.MyChar.Silvers -= BitConverter.ToUInt16(packet, 16);
            World.DiceKingTurnOver += BitConverter.ToUInt16(packet, 16);
            var Bet = new MsgDice();
            Bet.Action = Mode.ConfirmPlaceChip;//byte 4
            Bet.Seconds = 1;//byte 5
            Bet.ID = BitConverter.ToUInt16(packet, 8);//uint32 8
            Bet.Number = packet[12]; //DiceType uint32  12
            Bet.Amount = BitConverter.ToUInt16(packet, 16);//DiceData uint32 16
            client.AddSend(Packets.MsgDice(Bet));

            Bet.Action = Mode.PlaceChip;
            DiceKing.Announce(Packets.MsgDice(Bet), client);

            byte Number = 0;
            byte Times = 0;
            #region Get Right Number
            switch (packet[12])
            {
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    Number = (byte)((packet[12] - 1) * 3 + 20);
                    Times = 216;
                    break;
                case 1:
                    Number = 1;
                    Times = 2;
                    break;
                case 0:
                    Number = 0;
                    Times = 2;
                    break;
                default:
                    Number = packet[12];
                    Number -= 4;
                    break;
            }
            switch (Number)
            {
                case 4:
                case 17:
                    Times = 72;
                    break;
                case 5:
                case 16:
                    Times = 36;
                    break;
                case 6:
                case 15:
                    Times = 21;
                    break;
                case 7:
                case 14:
                    Times = 14;
                    break;
                case 8:
                case 13:
                    Times = 10;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    Times = 8;
                    break;
            }
            #endregion
            if (client.MyChar.MsgDice != null)
            {
                if (!client.MyChar.MsgDice.Bet.ContainsKey(Number))
                {
                    var MyBet = new Bets();
                    MyBet.Amount = BitConverter.ToUInt16(packet, 16);
                    MyBet.Times = Times;
                    client.MyChar.MsgDice.LastBet = DateTime.Now;

                    client.MyChar.MsgDice.Bet.TryAdd(Number, MyBet);
                    if (World.DiceKing.Bet.ContainsKey(Number))
                        World.DiceKing.Bet.Remove(Number);
                    World.DiceKing.Bet.TryAdd(Number, MyBet);
                }
                else
                {
                    uint oldBet = client.MyChar.MsgDice.Bet[Number].Amount;
                    var MyBet = new Bets();
                    MyBet.Amount = BitConverter.ToUInt16(packet, 16);
                    MyBet.Amount += oldBet;
                    MyBet.Times = Times;
                    client.MyChar.MsgDice.LastBet = DateTime.Now;

                    client.MyChar.MsgDice.Bet.Remove(Number);
                    client.MyChar.MsgDice.Bet.TryAdd(Number, MyBet);

                    World.DiceKing.Bet.Remove(Number);
                    World.DiceKing.Bet.TryAdd(Number, MyBet);
                }
            }
            
        }
    }
}