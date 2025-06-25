using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Game;

namespace NewestCOServer.Features
{
    public class GuildWars
    {
        public static bool GWPRIZE = false;
        public static byte GuildChests = 0;
        public static DateTime[] ChestTime = new DateTime[3];
       // static byte Week = 1;
       // static byte Month = 0;
        public class GWScore
        {
            public Guild TheGuild;
            public uint Score;
        }
        public struct Pole
        {
            public Location Loc;
            public uint MaxHP;
            public uint CurHP;
            public uint Mesh;
            public uint EntityID;

            public void Spawn(Character C, bool Check)
            {
                //if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 16) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 16) || !Check))
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 28) || !Check))
                {
                    if (LastWinner == null)
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Pole", CurHP, MaxHP));
                    else
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, LastWinner.GuildName, CurHP, MaxHP));
                }
            }
            public void ReSpawn()
            {
               // ThreadSafeDictionary<uint, Character> Map = (ThreadSafeDictionary<uint, Character>)World.PlayersInMap[Loc.Map];
               //foreach (Character C in Map.Values)
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28))
                    {
                        if (LastWinner == null)
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Pole", CurHP, MaxHP));
                        else
                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, LastWinner.GuildName, CurHP, MaxHP));
                    }
            }
            public void TakeAttack(Character C, uint Damage, byte AtkType)
            {
                if (War && C.MyGuild != null && (LastWinner == null || C.MyGuild.GuildID != LastWinner.GuildID))
                {
                    if (AtkType != 21)
                        World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
                    if (LastWinner != null)
                        if (LastWinner.Fund == 0)
                            Damage *= 2;
                    if (Damage >= CurHP)
                    {
                        if (LastWinner != null)
                        {

                            if (LastWinner.Fund > CurHP / 45)
                                LastWinner.Fund -= (CurHP / 45);
                                else
                                    LastWinner.Fund = 0;
                            if (Damage >= 500)
                                C.Silvers += 10;
                            //    C.Nobility.Donation += 50000;
                                World.NewEmpire(C);
                              //  C.Silvers += (Damage - CurHP) / 45;
                                C.MyGuild.Fund += (CurHP / 50);
                                C.GuildDonation += (CurHP / 50);
                            
                        }
                        C.MyClient.AddSend(Packets.GuildInfo(C.MyGuild, C));
                        AddScore(C.MyGuild, CurHP);
                        C.AtkMem.Attacking = false;
                        C.AtkMem.Target = 0;
                        PoleTakedown();
                        World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, 14).Get);
                        
                    }
                    else
                    {
                        if (LastWinner != null)
                        {
                            
                                if (LastWinner.Fund > Damage / 45)
                                    LastWinner.Fund -= Damage / 45;
                                else
                                    LastWinner.Fund = 0;
                               // C.Nobility.Donation += Damage / 20;
                            if (Damage >= 500)
                             //   C.Silvers += 10;
                                C.Silvers += Damage / 45;
                                C.MyGuild.Fund += Damage / 50;
                                C.GuildDonation += Damage / 50; 
                            
                        }
                        C.MyClient.AddSend(Packets.GuildInfo(C.MyGuild, C));
                        uint CurHP2 = CurHP;
                        if (CurHP > Damage)
                            CurHP -= Damage;
                        else CurHP = 0;
                        if (CurHP > 20000000)
                        {
                            World.ExcAdd += "Pole HP: " + CurHP + "\r\n";
                            Console.WriteLine("GW PROBLEM! Pole HP: " + CurHP);
                            if (CurHP2 < 20000000)
                                CurHP = CurHP2;
                            else CurHP = 1000000;
                        }
                        AddScore(C.MyGuild, Damage);
                    }
                }
            }
        }
        public struct Gate
        {
            public Location Loc;
            public uint MaxHP;
            public uint CurHP;
            public uint EntityID;
            public uint Mesh;

            public bool Opened
            {
                set
                {
                    if (EntityID == 6701)//Left Gate
                    {
                        if (value) Mesh = 250;
                        else Mesh = 240;
                    }
                    else if (EntityID == 6702)//Right Gate
                    {
                        if (value) Mesh = 280;
                        else Mesh = 270;
                    }
                }
                get
                {
                    if (EntityID == 6701)//Left Gate
                    {
                        if (Mesh == 250) return true;
                        else return false;
                    }
                    else if (EntityID == 6702)//Right Gate
                    {
                        if (Mesh == 280) return true;
                        else return false;
                    }
                    return false;
                }
            }
            public void Spawn(Character C, bool Check)
            {
               // if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 16) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 16) || !Check))
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 28) || !Check))
                    C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void ReSpawn()
            {
               
                //ThreadSafeDictionary<uint,Character> Map = (ThreadSafeDictionary<uint,Character>)World.PlayersInMap[Loc.Map];
               // System.Collections.Concurrent.ConcurrentBag<uint> Map = (System.Collections.Concurrent.ConcurrentBag<uint>)World.PlayersInMap[Loc.Map];
               // foreach (Character C in Map.Values)
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28))
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void TakeAttack(Character C, uint Damage, byte AtkType)
            {
                if (AtkType != 21)
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
                if (Damage >= CurHP)
                {
                    /*C.AtkMem.Attacking = false;
                    C.AtkMem.Target = 0;*/
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, (byte)AttackType.Kill).Get);
                    CurHP = 0;
                    if (!Opened)
                    {
                        Opened = true;
                        ReSpawn();
                    }
                }
                else
                    CurHP -= Damage;
            }
            public void TakeAttack(Companion C, uint Damage, byte AtkType)
            {
                if (AtkType != 21)
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
                if (Damage >= CurHP)
                {
                    CurHP = 0;
                    C.Owner.AtkMem.Attacking = false;
                    C.Owner.AtkMem.Target = 0;
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, 14).Get);
                    Opened = true;
                    ReSpawn();
                }
                else
                    CurHP -= Damage;
            }
        }

        public static Pole ThePole;
        public static Gate TheLeftGate;
        public static Gate TheRightGate;

        public static bool War;
        public static Dictionary<ushort, GWScore> Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;

        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, GWScore>();
            LastScores = DateTime.UtcNow;

            ThePole = new Pole();
            ThePole.EntityID = 6700;
            ThePole.Mesh = 1137;
            ThePole.CurHP = 20000000;
            ThePole.MaxHP = 20000000;
            ThePole.Loc = new Location();
            ThePole.Loc.Map = 1038;
            ThePole.Loc.X = 84;
            ThePole.Loc.Y = 99;

            TheLeftGate = new Gate();
            TheLeftGate.EntityID = 6701;
            TheLeftGate.Opened = false;
            TheLeftGate.MaxHP = 10000000;
            TheLeftGate.CurHP = 10000000;
            TheLeftGate.Loc = new Location();
            TheLeftGate.Loc.Map = 1038;
            TheLeftGate.Loc.X = 163;
            TheLeftGate.Loc.Y = 210;
            TheLeftGate.ReSpawn();

            TheRightGate = new Gate();
            TheRightGate.EntityID = 6702;
            TheRightGate.Opened = false;
            TheRightGate.MaxHP = 10000000;
            TheRightGate.CurHP = 10000000;
            TheRightGate.Loc = new Location();
            TheRightGate.Loc.Map = 1038;
            TheRightGate.Loc.X = 222;
            TheRightGate.Loc.Y = 177;
            TheRightGate.ReSpawn();
        }
        public static void AddScore(Guild G, uint Points)
        {
            if (!Scores.ContainsKey(G.GuildID))
            {
                GWScore S = new GWScore();
                S.Score = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                GWScore S = (GWScore)Scores[G.GuildID];
                S.Score += Points;
            }
        }
        class Entry
        {
            public List<Guild> Guilds;
        }
        public static string[] ShuffleGuildScores()
        {
            try
            {
                List<string> ret = new List<string>();

                SortedDictionary<ulong, Entry> sortdict = new SortedDictionary<ulong, Entry>();

                foreach (KeyValuePair<ushort, GWScore> Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    GWScore Value = (GWScore)Score.Value;

                    if (!Guilds.AllTheGuilds.ContainsKey(Key))
                        continue;

                    if (sortdict.ContainsKey(Value.Score))
                    {
                        Entry e = sortdict[Value.Score];
                        e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                        sortdict.Remove(Value.Score);
                        sortdict.Add(Value.Score, e);
                    }
                    else
                    {
                        Entry e = new Entry();
                        e.Guilds = new List<Guild>();
                        e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                        sortdict.Add(Value.Score, e);
                    }
                }
                int Place = 0;
                foreach (KeyValuePair<ulong, Entry> entries in sortdict.Reverse())
                {
                    foreach (Guild eGuild in entries.Value.Guilds)
                    {
                        string str = "No  " + (Place + 1).ToString() + ": " + eGuild.GuildName + "(" + entries.Key + ")";
                        ret.Add(str);
                        Place++;
                        if (Place == 4)
                            break;
                    }
                    if (Place == 4)
                        break;
                }

                return ret.ToArray();
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; return null; }
        }
       /* public static string[] ShuffleGuildScores()
        {
            try
            {
                string[] ret = new string[5];
                DictionaryEntry[] Vals = new DictionaryEntry[5];

                for (sbyte i = 0; i < 5; i++)
                {
                    Vals[i] = new DictionaryEntry();
                    Vals[i].Key = (ushort)0;
                    Vals[i].Value = (uint)0;
                }

                foreach (DictionaryEntry Score in Scores)
                {
                    sbyte Pos = -1;
                    for (sbyte i = 0; i < 5; i++)
                    {
                        if (((GWScore)Score.Value).Score > (uint)Vals[i].Value)
                        {
                            Pos = i;
                            break;
                        }
                    }
                    if (Pos == -1)
                        continue;

                    for (sbyte i = 4; i > Pos; i--)
                        Vals[i] = Vals[i - 1];

                    Vals[Pos] = Score;
                }

                for (sbyte i = 0; i < 5; i++)
                {
                    if ((ushort)Vals[i].Key == 0)
                    {
                        ret[i] = "";
                        continue;
                    }
                    Features.Guild eGuild = (Features.Guild)Features.Guilds.AllTheGuilds[(ushort)Vals[i].Key];
                    ret[i] = "No  " + (i + 1).ToString() + ": " + eGuild.GuildName + "(" + ((GWScore)Vals[i].Value).Score + ")";
                }

                return ret;
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; Scores = new Hashtable(); return null; }
        }*/
        public static void SendScores()
        {
            LastScores = DateTime.UtcNow;
            string[] ShuffledScores = ShuffleGuildScores();

          //  ThreadSafeDictionary<uint, Character> Map = (ThreadSafeDictionary<uint, Character>)World.PlayersInMap[ThePole.Loc.Map];
           // foreach (Character C in Map.Values)
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 1038)
                {
                    byte c = 0;
                    foreach (string t in ShuffledScores)
                    {
                        if (t != "")
                        {
                            if (c == 0)
                                C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", t, 0x83c, 0));
                            else
                                C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", t, 0x83d, 0));
                        }
                        c++;
                    }
                }
            }
        }
        static long LongRandom(long min, long max)
        {
            byte[] buf = new byte[8];
            Program.Rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
        public static void StartWar()
        {
           // System.Threading.Thread TopReset = new System.Threading.Thread(Database.TopReset);
           // TopReset.Start();
            Init();
            World.SendMsgToAll("SYSTEM", "Guild War has begun!", 2011, 0);
            War = true;
            loadstats();
            TheLeftGate.ReSpawn();
            TheRightGate.ReSpawn();
            //if (GuildChests == 0) - // Must Uncomment
            //{
            //    GuildChests = (byte)Program.Rnd.Next(1, 4);
            //    if (GuildChests == 1)
            //            ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.ToBinary(), DateTime.UtcNow.AddHours(41).ToBinary()));
            //    else if (GuildChests == 2)
            //    {
            //        ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.ToBinary(), DateTime.UtcNow.AddHours(20).ToBinary()));
            //        ChestTime[1] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.AddHours(21).ToBinary(), DateTime.UtcNow.AddHours(41).ToBinary()));
            //    }
            //    else if (GuildChests == 3)
            //    {
            //        ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.ToBinary(), DateTime.UtcNow.AddHours(13).ToBinary()));
            //        ChestTime[1] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.AddHours(14).ToBinary(), DateTime.UtcNow.AddHours(26).ToBinary()));
            //        ChestTime[2] = DateTime.FromBinary(LongRandom(DateTime.UtcNow.AddHours(27).ToBinary(), DateTime.UtcNow.AddHours(41).ToBinary()));
            //    }
            //}
                
        }
        public static void EndWarForGood()
        {
            War = false;
            GWPRIZE = true;
            LastWinner.Wins++;
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the GuildWar Congratulations!", 2011, 0);
            TheLeftGate.Opened = false;
            TheRightGate.Opened = false;

            TheLeftGate.CurHP = TheLeftGate.MaxHP;
            TheRightGate.CurHP = TheRightGate.MaxHP;
            TheLeftGate.ReSpawn();
            TheRightGate.ReSpawn();
            GuildChests = 0;
         
        }
        public static void Savestats()
        {
            System.IO.MemoryStream FS = new System.IO.MemoryStream();
           
            System.IO.BinaryWriter BW = new System.IO.BinaryWriter(FS);
            BW.Write(ThePole.CurHP);
            BW.Write(TheLeftGate.CurHP);
            BW.Write(TheRightGate.CurHP);
            BW.Write(GWPRIZE);
            BW.Write(GuildChests);
            for (int i = 0; i < GuildChests; i++)
                BW.Write(ChestTime[i].ToBinary());

            byte[] buffer = FS.ToArray();
            if (!World.LowRatedServer)
                System.IO.File.WriteAllBytes(@"C:\OldCODB\GuildwarStats.gw", buffer);
            else System.IO.File.WriteAllBytes(@"C:\OldCODB\GuildwarStatsNewServer.gw", buffer);
            BW.Close();
            FS.Close();
        }
        public static void loadstats()
        {
            if (!World.LowRatedServer)
            {
                if (System.IO.File.Exists(@"C:\OldCODB\GuildwarStats.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\GuildwarStats.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    ThePole.CurHP = BR.ReadUInt32();
                    TheLeftGate.CurHP = BR.ReadUInt32();
                    TheRightGate.CurHP = BR.ReadUInt32();
                    GWPRIZE = BR.ReadBoolean();
                   GuildChests = BR.ReadByte();
                    for (int i = 0; i < GuildChests; i++)
                        ChestTime[i] = DateTime.FromBinary(BR.ReadInt64());
                    BR.Close();
                    FS.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\GuildwarStatsNewServer.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\GuildwarStatsNewServer.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    ThePole.CurHP = BR.ReadUInt32();
                    TheLeftGate.CurHP = BR.ReadUInt32();
                    TheRightGate.CurHP = BR.ReadUInt32();
                    GWPRIZE = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
        }

        public static void PoleTakedown()
        {
            TheLeftGate.Opened = false;
            TheRightGate.Opened = false;

            TheLeftGate.CurHP = TheLeftGate.MaxHP;
            TheRightGate.CurHP = TheRightGate.MaxHP;
            TheLeftGate.ReSpawn();
            TheRightGate.ReSpawn();

            GWScore Highest = new GWScore();
            Highest.Score = 0;

            foreach (GWScore Score in Scores.Values)
            {
                if (Score.Score > Highest.Score)
                    Highest = Score;
            }

            ThePole.CurHP = ThePole.MaxHP;
            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild;                ThePole.ReSpawn();
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "Guild War started!", 2000, 0);
            }
            Scores = new Dictionary<ushort, GWScore>();
            SendScores();
        }
    }
}
