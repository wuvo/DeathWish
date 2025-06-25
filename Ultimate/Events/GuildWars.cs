using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class GuildWars
    {
        public static bool GWPRIZE = false;
        public static byte GuildChests = 0;
        public static DateTime[] ChestTime = new DateTime[3];

        public class GWScore
        {
            public Guild TheGuild;
            public uint Score;
        }

        public static SOB ThePole;
        public static SOB TheLeftGate;
        public static SOB TheRightGate;

        public static bool War;
        public static Dictionary<ushort, GWScore> Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;

        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, GWScore>();
            LastScores = DateTime.Now;

            ThePole = new SOB();
            ThePole.EntityID = 6700;
            ThePole.Mesh = 1137;
            ThePole.Type = Game.Looks.Pole;
            ThePole.CurHP = 20000000;
            ThePole.MaxHP = 20000000;
            ThePole.Loc = new Location();
            ThePole.Loc.Map = 1038;
            ThePole.Loc.X = 84;
            ThePole.Loc.Y = 99;
            ThePole.War = false;
            ThePole.LastWinner = LastWinner;
            ThePole.AddSOB();

            TheLeftGate = new SOB();
            TheLeftGate.EntityID = 6701;
            TheLeftGate.Type = Game.Looks.LeftGate;
            TheLeftGate.Opened = false;
            TheLeftGate.MaxHP = 10000000;
            TheLeftGate.CurHP = 10000000;
            TheLeftGate.Loc = new Location();
            TheLeftGate.Loc.Map = 1038;
            TheLeftGate.Loc.X = 163;
            TheLeftGate.Loc.Y = 210;
            TheLeftGate.ReSpawn();
            TheLeftGate.AddSOB();

            TheRightGate = new SOB();
            TheRightGate.EntityID = 6702;
            TheRightGate.Type = Game.Looks.RightGate;
            TheRightGate.Opened = false;
            TheRightGate.MaxHP = 10000000;
            TheRightGate.CurHP = 10000000;
            TheRightGate.Loc = new Location();
            TheRightGate.Loc.Map = 1038;
            TheRightGate.Loc.X = 222;
            TheRightGate.Loc.Y = 177;
            TheRightGate.ReSpawn();
            TheRightGate.AddSOB();
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
        public static void SendScores()
        {
            LastScores = DateTime.Now;
            string[] ShuffledScores = ShuffleGuildScores();

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
            Init();
            World.SendMsgToAll("SYSTEM", "Guild War has begun!", 2011, 0);
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
            Cmd2.Update("guildwars").Set("winner", Features.GuildWars.LastWinner.GuildName).Where("id", 1).Execute();
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the GuildWar Congratulations!", 2011, 0);
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
            World.H_SOBs[TheLeftGate.EntityID].Opened = false;
            World.H_SOBs[TheRightGate.EntityID].Opened = false;

            World.H_SOBs[TheLeftGate.EntityID].CurHP = World.H_SOBs[TheLeftGate.EntityID].MaxHP;
            World.H_SOBs[TheRightGate.EntityID].CurHP = World.H_SOBs[TheRightGate.EntityID].MaxHP;
            World.H_SOBs[TheLeftGate.EntityID].ReSpawn();
            World.H_SOBs[TheRightGate.EntityID].ReSpawn();

            GWScore Highest = new GWScore();
            Highest.Score = 0;

            foreach (GWScore Score in Scores.Values)
            {
                if (Score.Score > Highest.Score)
                    Highest = Score;
            }

            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild;
                World.H_SOBs[ThePole.EntityID].LastWinner = Highest.TheGuild;
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "Guild War started!", 2000, 0);
            }
            World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            World.H_SOBs[ThePole.EntityID].ReSpawn();
            //World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            Scores = new Dictionary<ushort, GWScore>();
            SendScores();
        }
    }
}
