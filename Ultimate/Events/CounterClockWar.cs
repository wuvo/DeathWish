using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.Features
{
    public class CounterClock
    {
        public static bool signup = false;
        public static bool CCPrize = false;

        public class CCScore
        {
            public Guild TheGuild;
            public uint Score;
        }
        class Entry
        {
            public List<Guild> Guilds;
        }

        public static SOB ThePole, RG19, RG18, RG17, RG16, RG15, RG14, RG13, RG12, RG11, RG10, RG9, RG8, RG7, RG6, RG5, RG4, RG3, RG2, RG1, LG6, LG5, LG4, LG3, LG2, LG1;
        public static bool War;
        public static Dictionary<ushort, CCScore> Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;


        /// <summary>
        /// Called after the war has started - spawns all gates and pole
        /// </summary>
        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, CCScore>();
            LastScores = DateTime.Now;

            ThePole = new SOB() { EntityID = 6726, Type = Looks.Pole, Mesh = 1137, CurHP = 15000000, MaxHP = 15000000, Loc = new Location() { Map = 1844, X = 114, Y = 163 }, War = false, LastWinner = LastWinner };
            ThePole.AddSOB();

            #region Central Gates
            #region Pole
            RG17 = new SOB() { EntityID = 6725, Type = Looks.RightGate, Opened = false, MaxHP = 2500000, CurHP = 2500000, Loc = new Location() { Map = 1844, X = 134, Y = 167, }, DependantGates = new List<uint>() { 6724, 6723 } };
            RG17.ReSpawn();
            RG17.AddSOB();

            RG16 = new SOB() { EntityID = 6724, Type = Looks.RightGate, Opened = false, MaxHP = 2500000, CurHP = 2500000, Loc = new Location() { Map = 1844, X = 134, Y = 163 }, DependantGates = new List<uint>() { 6725, 6723 } };
            RG16.ReSpawn();
            RG16.AddSOB();

            RG15 = new SOB() { EntityID = 6723, Type = Looks.RightGate, Opened = false, MaxHP = 2500000, CurHP = 2500000, Loc = new Location() { Map = 1844, X = 134, Y = 159 }, DependantGates = new List<uint>() { 6724, 6725 } };
            RG15.ReSpawn();
            RG15.AddSOB();
            #endregion

            #region Mid
            RG14 = new SOB() { EntityID = 6722, Type = Looks.RightGate, Opened = false, MaxHP = 1500000, CurHP = 1500000, Loc = new Location() { Map = 1844, X = 172, Y = 167 }, DependantGates = new List<uint>() { 6721, 6720 } };
            RG14.ReSpawn();
            RG14.AddSOB();

            RG13 = new SOB() { EntityID = 6721, Type = Looks.RightGate, Opened = false, MaxHP = 1500000, CurHP = 1500000, Loc = new Location() { Map = 1844, X = 172, Y = 163 }, DependantGates = new List<uint>() { 6722, 6720 } };
            RG13.ReSpawn();
            RG13.AddSOB();

            RG12 = new SOB() { EntityID = 6720, Type = Looks.RightGate, Opened = false, MaxHP = 1500000, CurHP = 1500000, Loc = new Location() { Map = 1844, X = 172, Y = 159 }, DependantGates = new List<uint>() { 6721, 6722 } };
            RG12.ReSpawn();
            RG12.AddSOB();
            #endregion

            #region Entrance
            RG11 = new SOB() { EntityID = 6719, Type = Looks.RightGate, Opened = false, MaxHP = 1000000, CurHP = 1000000, Loc = new Location() { Map = 1844, X = 210, Y = 167 }, DependantGates = new List<uint>() { 6718, 6717 } };
            RG11.ReSpawn();
            RG11.AddSOB();

            RG10 = new SOB() { EntityID = 6718, Type = Looks.RightGate, Opened = false, MaxHP = 1000000, CurHP = 1000000, Loc = new Location() { Map = 1844, X = 210, Y = 163 }, DependantGates = new List<uint>() { 6719, 6717 } };
            RG10.ReSpawn();
            RG10.AddSOB();

            RG9 = new SOB() { EntityID = 6717, Type = Looks.RightGate, Opened = false, MaxHP = 1000000, CurHP = 1000000, Loc = new Location() { Map = 1844, X = 210, Y = 159 }, DependantGates = new List<uint>() { 6718, 6719 } };
            RG9.ReSpawn();
            RG9.AddSOB();
            #endregion
            #endregion

            #region RGs

            #region Last lane
            RG8 = new SOB() { EntityID = 6716, Type = Looks.RightGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 210, Y = 124 } };
            RG8.ReSpawn();
            RG8.AddSOB();

            RG7 = new SOB() { EntityID = 6715, Type = Looks.RightGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 210, Y = 128 } };
            RG7.ReSpawn();
            RG7.AddSOB();

            RG6 = new SOB() { EntityID = 6714, Type = Looks.RightGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 210, Y = 207 } };
            RG6.ReSpawn();
            RG6.AddSOB();

            RG5 = new SOB() { EntityID = 6713, Type = Looks.RightGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 210, Y = 203 } };
            RG5.ReSpawn();
            RG5.AddSOB();
            #endregion

            #region First lane
            RG4 = new SOB() { EntityID = 6712, Type = Looks.RightGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 172, Y = 207 } };
            RG4.ReSpawn();
            RG4.AddSOB();

            RG3 = new SOB() { EntityID = 6711, Type = Looks.RightGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 172, Y = 128 } };
            RG3.ReSpawn();
            RG3.AddSOB();

            RG2 = new SOB() { EntityID = 6710, Type = Looks.RightGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 172, Y = 124 } };
            RG2.ReSpawn();
            RG2.AddSOB();


            RG1 = new SOB() { EntityID = 6709, Type = Looks.RightGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 172, Y = 203 } };
            RG1.ReSpawn();
            RG1.AddSOB();
            #endregion

            #endregion
            #region LGs
            LG6 = new SOB() { EntityID = 6708, Type = Looks.LeftGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 149, Y = 145 } };//third Square right
            LG6.ReSpawn();
            LG6.AddSOB();

            LG5 = new SOB() { EntityID = 6707, Type = Looks.LeftGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 187, Y = 145 } };//second Square right
            LG5.ReSpawn();
            LG5.AddSOB();

            LG4 = new SOB() { EntityID = 6706, Type = Looks.LeftGate, Opened = false, MaxHP = 2000000, CurHP = 2000000, Loc = new Location() { Map = 1844, X = 225, Y = 145 } };//first Square right
            LG4.ReSpawn();
            LG4.AddSOB();

            LG3 = new SOB() { EntityID = 6705, Type = Looks.LeftGate, Opened = false, MaxHP = 5000000, CurHP = 5000000, Loc = new Location() { Map = 1844, X = 149, Y = 186 } };//third Square left
            LG3.ReSpawn();
            LG3.AddSOB();

            LG2 = new SOB() { EntityID = 6704, Type = Looks.LeftGate, Opened = false, MaxHP = 3000000, CurHP = 3000000, Loc = new Location() { Map = 1844, X = 187, Y = 186 } };//second Square left
            LG2.ReSpawn();
            LG2.AddSOB();

            LG1 = new SOB() { EntityID = 6703, Type = Looks.LeftGate, Opened = false, MaxHP = 2000000, CurHP = 2000000, Loc = new Location() { Map = 1844, X = 225, Y = 186 } };//first Square left
            LG1.ReSpawn();
            LG1.AddSOB();
            #endregion
        }

        /// <summary>
        /// Adds score to a guild when a player attacks the pole
        /// </summary>
        /// <param name="G"></param>
        /// <param name="Points"></param>
        public static void AddScore(Guild G, uint Points)
        {
            if (!Scores.ContainsKey(G.GuildID))
            {
                CCScore S = new CCScore();
                S.Score = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                CCScore S = (CCScore)Scores[G.GuildID];
                S.Score += Points;
            }
        }

        /// <summary>
        /// Organizes the guilds' scores by the total damage dealt to pole
        /// </summary>
        /// <returns></returns>
        public static string[] ShuffleGuildScores()
        {
            try
            {
                List<string> ret = new List<string>();

                SortedDictionary<ulong, Entry> sortdict = new SortedDictionary<ulong, Entry>();

                foreach (KeyValuePair<ushort, CCScore> Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    CCScore Value = (CCScore)Score.Value;

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

        /// <summary>
        /// Sends current scores to all players inside the map
        /// </summary>
        public static void SendScores()
        {
            LastScores = DateTime.Now;
            string[] ShuffledScores = ShuffleGuildScores();

            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 1844)
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

        /// <summary>
        /// Handles CCGW Start, spawns all gates, loads last stats and announces it has started
        /// </summary>
        public static void StartWar()
        {
            Init();
            World.H_SOBs[ThePole.EntityID].War = true;
            World.H_SOBs[LG1.EntityID].ReSpawn();
            World.H_SOBs[LG2.EntityID].ReSpawn();
            World.H_SOBs[LG3.EntityID].ReSpawn();
            World.H_SOBs[LG4.EntityID].ReSpawn();
            World.H_SOBs[LG5.EntityID].ReSpawn();
            World.H_SOBs[LG6.EntityID].ReSpawn();
            World.H_SOBs[RG1.EntityID].ReSpawn();
            World.H_SOBs[RG2.EntityID].ReSpawn();
            World.H_SOBs[RG3.EntityID].ReSpawn();
            World.H_SOBs[RG4.EntityID].ReSpawn();
            World.H_SOBs[RG5.EntityID].ReSpawn();
            World.H_SOBs[RG6.EntityID].ReSpawn();
            World.H_SOBs[RG7.EntityID].ReSpawn();
            World.H_SOBs[RG8.EntityID].ReSpawn();
            World.H_SOBs[RG9.EntityID].ReSpawn();
            World.H_SOBs[RG10.EntityID].ReSpawn();
            World.H_SOBs[RG11.EntityID].ReSpawn();
            World.H_SOBs[RG12.EntityID].ReSpawn();
            World.H_SOBs[RG13.EntityID].ReSpawn();
            World.H_SOBs[RG14.EntityID].ReSpawn();
            World.H_SOBs[RG15.EntityID].ReSpawn();
            World.H_SOBs[RG16.EntityID].ReSpawn();
            loadstats();
            World.SendMsgToAll("SYSTEM", "Counter Clock War has begun! Talk to CounterClock NPC in TwinCity to join!", 2011, 0);
            War = true;
        }

        /// <summary>
        /// Handles CCGW End, announces winner and teleports everyone back to Twin City
        /// </summary>
        public static void EndWarForGood()
        {
            War = false;
            World.H_SOBs[ThePole.EntityID].War = false;
            CCPrize = true;
            if (LastWinner != null)
                LastWinner.Wins++;
            MySQL.MySqlCommand Cmd2;
            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
            Cmd2.Update("guildwars").Set("winner", Features.CounterClock.LastWinner.GuildName).Where("id", 7).Execute();
            foreach (Character C in World.H_Chars.Values)
                if (C.Loc.Map == 1844)
                    C.Teleport(1002, 430, 378);
            if (LastWinner != null)
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the Counter Clock War! Congratulations!", 2011, 0);
            else
                World.SendMsgToAll("SYSTEM", "The Counter Clock War has ended and there were no winners!", 2011, 0);
        }

        /// <summary>
        /// Saves CCGW stats
        /// </summary>
        public static void Savestats()
        {
            System.IO.MemoryStream FS = new System.IO.MemoryStream();

            System.IO.BinaryWriter BW = new System.IO.BinaryWriter(FS);
            BW.Write(CCPrize);

            byte[] buffer = FS.ToArray();
            if (!World.LowRatedServer)
                System.IO.File.WriteAllBytes(@"C:\OldCODB\CCWarStats.gw", buffer);
            else System.IO.File.WriteAllBytes(@"C:\OldCODB\CCWarStatsNewServer.gw", buffer);
            BW.Close();
            FS.Close();
        }

        /// <summary>
        /// Loads last CCGW stats
        /// </summary>
        public static void loadstats()
        {
            if (!World.LowRatedServer)
            {
                if (System.IO.File.Exists(@"C:\OldCODB\CCWarStats.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\CCWarStats.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    CCPrize = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\CCWarStatsNewServer.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\CCWarStatsNewServer.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    CCPrize = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
        }

        /// <summary>
        /// Handles the event of a Pole being killed - closes central and some of the lateral gates and respawn pole
        /// </summary>
        public static void PoleTakedown()
        {
            #region Pole
            World.H_SOBs[RG17.EntityID].Opened = false;
            World.H_SOBs[RG16.EntityID].Opened = false;
            World.H_SOBs[RG15.EntityID].Opened = false;

            World.H_SOBs[RG17.EntityID].CurHP = World.H_SOBs[RG17.EntityID].MaxHP;
            World.H_SOBs[RG16.EntityID].CurHP = World.H_SOBs[RG16.EntityID].MaxHP;
            World.H_SOBs[RG15.EntityID].CurHP = World.H_SOBs[RG15.EntityID].MaxHP;

            World.H_SOBs[RG17.EntityID].ReSpawn();
            World.H_SOBs[RG16.EntityID].ReSpawn();
            World.H_SOBs[RG15.EntityID].ReSpawn();
            #endregion
            #region Mid
            World.H_SOBs[RG14.EntityID].Opened = false;
            World.H_SOBs[RG13.EntityID].Opened = false;
            World.H_SOBs[RG12.EntityID].Opened = false;

            World.H_SOBs[RG14.EntityID].CurHP = World.H_SOBs[RG14.EntityID].MaxHP;
            World.H_SOBs[RG13.EntityID].CurHP = World.H_SOBs[RG13.EntityID].MaxHP;
            World.H_SOBs[RG12.EntityID].CurHP = World.H_SOBs[RG12.EntityID].MaxHP;

            World.H_SOBs[RG14.EntityID].ReSpawn();
            World.H_SOBs[RG13.EntityID].ReSpawn();
            World.H_SOBs[RG12.EntityID].ReSpawn();
            #endregion
            #region MidLeft
            World.H_SOBs[RG1.EntityID].Opened = false;
            World.H_SOBs[RG4.EntityID].Opened = false;

            World.H_SOBs[RG1.EntityID].CurHP = World.H_SOBs[RG1.EntityID].MaxHP;
            World.H_SOBs[RG4.EntityID].CurHP = World.H_SOBs[RG4.EntityID].MaxHP;

            World.H_SOBs[RG1.EntityID].ReSpawn();
            World.H_SOBs[RG4.EntityID].ReSpawn();
            #endregion
            #region MidRight
            World.H_SOBs[RG2.EntityID].Opened = false;
            World.H_SOBs[RG3.EntityID].Opened = false;

            World.H_SOBs[RG2.EntityID].CurHP = World.H_SOBs[RG2.EntityID].MaxHP;
            World.H_SOBs[RG3.EntityID].CurHP = World.H_SOBs[RG3.EntityID].MaxHP;

            World.H_SOBs[RG2.EntityID].ReSpawn();
            World.H_SOBs[RG3.EntityID].ReSpawn();
            #endregion

            World.H_SOBs[LG2.EntityID].Opened = false;
            World.H_SOBs[LG5.EntityID].Opened = false;

            World.H_SOBs[LG2.EntityID].CurHP = World.H_SOBs[LG2.EntityID].MaxHP;
            World.H_SOBs[LG5.EntityID].CurHP = World.H_SOBs[LG5.EntityID].MaxHP;
            World.H_SOBs[LG2.EntityID].ReSpawn();
            World.H_SOBs[LG5.EntityID].ReSpawn();

            CCScore Highest = new CCScore();
            Highest.Score = 0;

            foreach (CCScore Score in Scores.Values)
                if (Score.Score > Highest.Score)
                    Highest = Score;

            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild; ThePole.ReSpawn();
                World.H_SOBs[ThePole.EntityID].LastWinner = Highest.TheGuild;
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "Counter Clock War started!", 2000, 0);
            }

            World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            World.H_SOBs[ThePole.EntityID].ReSpawn();
            Scores = new Dictionary<ushort, CCScore>();
            SendScores();
        }
    }
}
