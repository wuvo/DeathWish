using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class TCGuildWars
    {
        public static bool GWPRIZE = false;
        public static byte GuildChests = 0;
        public static DateTime[] ChestTime = new DateTime[3];

        public class TCGWScore
        {
            public Guild TheGuild;
            public uint Score;
        }

        public static SOB ThePole;
        public static SOB TheLeftGate;
        public static SOB TheRightGate;
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
                    if (EntityID == 67001 || EntityID == 67005 || EntityID == 67006)//Left Gate
                    {
                        if (value) Mesh = 250;
                        else Mesh = 240;
                    }
                    else if (EntityID == 67002 || EntityID == 67003 || EntityID == 67004)//Right Gate
                    {
                        if (value) Mesh = 280;
                        else Mesh = 270;
                    }
                }
                get
                {
                    if (EntityID == 67001 || EntityID == 67005 || EntityID == 67006)//Left Gate
                    {
                        if (Mesh == 250) return true;
                        else return false;
                    }
                    else if (EntityID == 67002 || EntityID == 67003 || EntityID == 67004)//Right Gate
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
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 18) || !Check))
                    C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void ReSpawn()
            {

                //ThreadSafeDictionary<uint,Character> Map = (ThreadSafeDictionary<uint,Character>)World.PlayersInMap[Loc.Map];
                // System.Collections.Concurrent.ConcurrentBag<uint> Map = (System.Collections.Concurrent.ConcurrentBag<uint>)World.PlayersInMap[Loc.Map];
                // foreach (Character C in Map.Values)
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 18))
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

        public static Gate[] ProtectGates;


        public static bool War;
        public static Dictionary<ushort, TCGWScore> Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;

        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, TCGWScore>();
            LastScores = DateTime.Now;

            ThePole = new SOB();
            ThePole.EntityID = 67000;
            ThePole.Mesh = 1137;
            ThePole.Type = Game.Looks.Pole;
            ThePole.CurHP = 20000000;
            ThePole.MaxHP = 20000000;
            ThePole.Loc = new Location();
            ThePole.Loc.Map = 10200;
            ThePole.Loc.X = 310 + 128;
            ThePole.Loc.Y = 277 + 100;
            ThePole.War = false;
            ThePole.LastWinner = LastWinner;
            ThePole.AddSOB();

            TheLeftGate = new SOB();
            TheLeftGate.EntityID = 67001;
            TheLeftGate.Type = Game.Looks.LeftGate;
            TheLeftGate.Opened = false;
            TheLeftGate.MaxHP = 10000000;
            TheLeftGate.CurHP = 10000000;
            TheLeftGate.Loc = new Location();
            TheLeftGate.Loc.Map = 10200;
            TheLeftGate.Loc.X = 309 + 128;
            TheLeftGate.Loc.Y = 326 + 100;
            TheLeftGate.ReSpawn();
            TheLeftGate.AddSOB();

            TheRightGate = new SOB();
            TheRightGate.EntityID = 67002;
            TheRightGate.Type = Game.Looks.RightGate;
            TheRightGate.Opened = false;
            TheRightGate.MaxHP = 10000000;
            TheRightGate.CurHP = 10000000;
            TheRightGate.Loc = new Location();
            TheRightGate.Loc.Map = 10200;
            TheRightGate.Loc.X = 375 + 128;
            TheRightGate.Loc.Y = 254 + 100;
            TheRightGate.ReSpawn();
            TheRightGate.AddSOB();
            ProtectGates = new Gate[4];
            for (int i = 0; i < 4; i++)
            {
                ProtectGates[i].EntityID = (uint)(67002 + i + 1);
                ProtectGates[i].Opened = false;
                ProtectGates[i].MaxHP = 10000000;
                ProtectGates[i].CurHP = 10000000;
                ProtectGates[i].Loc = new Location();
                ProtectGates[i].Loc.Map = 10200;
                switch (i)
                {
                    case 0:
                        ProtectGates[i].Loc.X = 592;
                        ProtectGates[i].Loc.Y = 591;
                        break;
                    case 1:
                        ProtectGates[i].Loc.X = 592;
                        ProtectGates[i].Loc.Y = 583;
                        break;
                    case 2:
                        ProtectGates[i].Loc.X = 561;
                        ProtectGates[i].Loc.Y = 613;
                        break;
                    case 3:
                        ProtectGates[i].Loc.X = 571;
                        ProtectGates[i].Loc.Y = 613;
                        break;
                }

                ProtectGates[i].ReSpawn();
            }
        }
        public static void AddScore(Guild G, uint Points)
        {
            if (!Scores.ContainsKey(G.GuildID))
            {
                TCGWScore S = new TCGWScore();
                S.Score = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                TCGWScore S = (TCGWScore)Scores[G.GuildID];
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

                foreach (KeyValuePair<ushort, TCGWScore> Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    TCGWScore Value = (TCGWScore)Score.Value;

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
        public static void SendScores()
        {
            LastScores = DateTime.Now;
            string[] ShuffledScores = ShuffleGuildScores();

            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 10200)
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
            Init();
            World.SendMsgToAll("SYSTEM", "TCGuild War has begun!", 2011, 0);
            War = true;
            loadstats();
            World.H_SOBs[ThePole.EntityID].War = true;
            World.H_SOBs[TheLeftGate.EntityID].ReSpawn();
            World.H_SOBs[TheRightGate.EntityID].ReSpawn();
            if (World.H_SOBs[TheLeftGate.EntityID].CurHP > 0)
                World.H_SOBs[TheLeftGate.EntityID].Opened = false;
            if (World.H_SOBs[TheRightGate.EntityID].CurHP > 0)
                World.H_SOBs[TheRightGate.EntityID].Opened = false;

            //TheLeftGate.ReSpawn();
            //TheRightGate.ReSpawn();
            //if (GuildChests == 0) - // Must Uncomment
            //{
            //    GuildChests = (byte)Program.Rnd.Next(1, 4);
            //    if (GuildChests == 1)
            //            ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.Now.ToBinary(), DateTime.Now.AddHours(41).ToBinary()));
            //    else if (GuildChests == 2)
            //    {
            //        ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.Now.ToBinary(), DateTime.Now.AddHours(20).ToBinary()));
            //        ChestTime[1] = DateTime.FromBinary(LongRandom(DateTime.Now.AddHours(21).ToBinary(), DateTime.Now.AddHours(41).ToBinary()));
            //    }
            //    else if (GuildChests == 3)
            //    {
            //        ChestTime[0] = DateTime.FromBinary(LongRandom(DateTime.Now.ToBinary(), DateTime.Now.AddHours(13).ToBinary()));
            //        ChestTime[1] = DateTime.FromBinary(LongRandom(DateTime.Now.AddHours(14).ToBinary(), DateTime.Now.AddHours(26).ToBinary()));
            //        ChestTime[2] = DateTime.FromBinary(LongRandom(DateTime.Now.AddHours(27).ToBinary(), DateTime.Now.AddHours(41).ToBinary()));
            //    }
            //}

        }
        public static void EndWarForGood()
        {
            War = false;
            World.H_SOBs[ThePole.EntityID].War = false;
            GWPRIZE = true;
            LastWinner.Wins++;
            MySQL.MySqlCommand Cmd2;
            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
            Cmd2.Update("guildwars").Set("winner", Features.CityWarAc.LastWinner.GuildName).Where("id", 8).Execute();
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the TCGuildWar Congratulations!", 2011, 0);
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the TCGuildWar War! Congratulations!", 2000, 0);
            World.H_SOBs[TheLeftGate.EntityID].Opened = false;
            World.H_SOBs[TheRightGate.EntityID].Opened = false;

            World.H_SOBs[TheLeftGate.EntityID].CurHP = World.H_SOBs[TheLeftGate.EntityID].MaxHP;
            World.H_SOBs[TheRightGate.EntityID].CurHP = World.H_SOBs[TheRightGate.EntityID].MaxHP;
            World.H_SOBs[TheLeftGate.EntityID].ReSpawn();
            World.H_SOBs[TheRightGate.EntityID].ReSpawn();
            GuildChests = 0;

        }
        public static void Savestats()
        {
            System.IO.MemoryStream FS = new System.IO.MemoryStream();

            System.IO.BinaryWriter BW = new System.IO.BinaryWriter(FS);
            BW.Write(World.H_SOBs[ThePole.EntityID].CurHP);
            BW.Write(World.H_SOBs[TheLeftGate.EntityID].CurHP);
            BW.Write(World.H_SOBs[TheRightGate.EntityID].CurHP);
            BW.Write(GWPRIZE);
            BW.Write(GuildChests);
            for (int i = 0; i < GuildChests; i++)
                BW.Write(ChestTime[i].ToBinary());

            byte[] buffer = FS.ToArray();
            if (!World.LowRatedServer)
                System.IO.File.WriteAllBytes(@"C:\OldCODB\TCGuildwarStats.gw", buffer);
            else System.IO.File.WriteAllBytes(@"C:\OldCODB\TCGuildwarStatsNewServer.gw", buffer);
            BW.Close();
            FS.Close();
        }
        public static void loadstats()
        {
            if (!World.LowRatedServer)
            {
                if (System.IO.File.Exists(@"C:\OldCODB\TCGuildwarStats.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\TCGuildwarStats.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    World.H_SOBs[ThePole.EntityID].CurHP = BR.ReadUInt32();
                    World.H_SOBs[TheLeftGate.EntityID].CurHP = BR.ReadUInt32();
                    World.H_SOBs[TheRightGate.EntityID].CurHP = BR.ReadUInt32();
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
                if (System.IO.File.Exists(@"C:\OldCODB\TCGuildwarStatsNewServer.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\TCGuildwarStatsNewServer.gw");
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
            World.H_SOBs[TheLeftGate.EntityID].Opened = false;
            World.H_SOBs[TheRightGate.EntityID].Opened = false;

            World.H_SOBs[TheLeftGate.EntityID].CurHP = World.H_SOBs[TheLeftGate.EntityID].MaxHP;
            World.H_SOBs[TheRightGate.EntityID].CurHP = World.H_SOBs[TheRightGate.EntityID].MaxHP;
            World.H_SOBs[TheLeftGate.EntityID].ReSpawn();
            World.H_SOBs[TheRightGate.EntityID].ReSpawn();

            TCGWScore Highest = new TCGWScore();
            Highest.Score = 0;

            foreach (TCGWScore Score in Scores.Values)
            {
                if (Score.Score > Highest.Score)
                    Highest = Score;
            }

            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild;
                World.H_SOBs[ThePole.EntityID].LastWinner = Highest.TheGuild;
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "TCGuild War started!", 2000, 0);
            }
            World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            World.H_SOBs[ThePole.EntityID].ReSpawn();
            //World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            Scores = new Dictionary<ushort, TCGWScore>();
            SendScores();
        }
    }
}
