using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Events
{
    public class GameOfThones
    {
        public enum WarStage
        {
            None,
            Inviting,
            Waiting,
            RoundOne,
            RoundOneBreak,
            RoundTwo,
            RoundTwoBreak,
            RoundThree
        }

        public static WarStage Stage;
        public static WarStage NextStage;
        public static bool CCPrize = false;

        public class Score
        {
            public uint EntityID;
            public Guild TheGuild;
            public uint GuildScore;
        }
        class Entry
        {
            public List<Guild> Guilds;
        }

        public static SOB StatueOne, StatueTwo, StatueThree;
        public static bool War;
        public static Dictionary<ushort, Score> Scores;
        public static List<uint> GuildOnNextRound;
        public static DateTime WaitingPeriod;
        public static DateTime LastScores;
        public static Guild LastWinner;
        public static int Break = 1;
        public static int RoundDuration = 3;

        /// <summary>
        /// Starts the event and enables invitations
        /// </summary>
        public static void Start()
        {
            Stage = WarStage.Inviting;
            Scores = new Dictionary<ushort, Score>();
            WaitingPeriod = DateTime.Now.AddMinutes(Break);
            World.SendMsgToAll("SYSTEM", $"Game Of Thones starts in {Break} minutes", 2000, 0);
        }

        public static void Shuffle()
        {
            if (Stage == WarStage.Waiting || Stage == WarStage.Inviting)
                NextRound();
            else if (Stage == WarStage.RoundOne || Stage == WarStage.RoundTwo)
                EndRound();
        }

        static void DropEffect(ushort x, ushort y)
        {
            Random Rnd = new Random();
            Game.MapEffect DI = new Game.MapEffect();
            DI.DropTime = DateTime.Now;
            DI.Loc = new Game.Location();
            DI.Loc.Map = 2071;
            DI.Info = new Game.MEffect();
            DI.Info.ID = 798;

            DI.UID = (uint)Rnd.Next(900000, 999999);
            DI.Info.UID = DI.UID;
            DI.Loc.X = (ushort)(x);
            DI.Loc.Y = (ushort)(y);
            if (!Game.World.H_Effects.ContainsKey(2071))
                World.H_Effects.Add(2071, new System.Collections.Concurrent.ConcurrentDictionary<uint, MapEffect>());
            //if (!DI.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.MapEffect>)Game.World.H_Effects[2071])) return;
            DI.Drop();
        }

        /// <summary>
        /// Sets the next round, teleports players to the event map
        /// </summary>
        public static void NextRound()
        {
            Random Rnd = new Random();
            DMap DM = (DMap)DMaps.H_DMaps[2071];
            int x;
            int y;
            if (Stage != WarStage.Inviting)
            {
                Stage = NextStage;
                World.SendMsgToAll("SYSTEM", $"{Stage} starting", 2000, 0);
            }
            else
            {
                WaitingPeriod = DateTime.Now.AddMinutes(RoundDuration);
                World.SendMsgToAll("SYSTEM", "Round One starting", 2000, 0);
                Stage = WarStage.RoundOne;
                for (int a = -15; a < 16; a++)
                {
                    int x2 = 127 + a;
                    int y2 = 127;
                    for (int b = -15; b < 16; b++)
                    {
                        y2 = 127 + b;
                        if ((x2 == 127 + 15 && y2 >= 127 - 15 && y2 <= 127 + 15) || (x2 == 127 - 15 && y2 >= 127 - 15 && y2 <= 127 + 15) || (y2 == 127 - 15 && x2 >= 127 - 15 && x2 <= 127 + 15) || (y2 == 127 + 15 && x2 >= 127 - 15 && x2 <= 127 + 15))
                            DropEffect((ushort)x2, (ushort)y2);
                    }
                }
                for (int a = -15; a < 16; a++)
                {
                    int x2 = 151 + a;
                    int y2 = 55;
                    for (int b = -15; b < 16; b++)
                    {
                        y2 = 55 + b;
                        if ((x2 == 151 + 15 && y2 >= 55 - 15 && y2 <= 55 + 15) || (x2 == 151 - 15 && y2 >= 55 - 15 && y2 <= 55 + 15) || (y2 == 55 - 15 && x2 >= 151 - 15 && x2 <= 151 + 15) || (y2 == 55 + 15 && x2 >= 151 - 15 && x2 <= 151 + 15))
                            DropEffect((ushort)x2, (ushort)y2);
                    }
                }
                for (int a = -15; a < 16; a++)
                {
                    int x2 = 103 + a;
                    int y2 = 199;
                    for (int b = -15; b < 16; b++)
                    {
                        y2 = 199 + b;
                        if ((x2 == 103 + 15 && y2 >= 199 - 15 && y2 <= 199 + 15) || (x2 == 103 - 15 && y2 >= 199 - 15 && y2 <= 199 + 15) || (y2 == 199 - 15 && x2 >= 103 - 15 && x2 <= 103 + 15) || (y2 == 199 + 15 && x2 >= 103 - 15 && x2 <= 103 + 15))
                            DropEffect((ushort)x2, (ushort)y2);
                    }
                }
            }

            if (Stage == WarStage.RoundOne || Stage == WarStage.RoundTwo)
            {
                foreach (Character C in World.H_Chars.Values)
                {
                    if (C.Loc.Map == 2068)
                    {
                        if (MyMath.ChanceSuccess(50))
                        {
                            x = Rnd.Next(36, 61);
                            y = Rnd.Next(125, 155);
                            while (DM.GetCell((ushort)x, (ushort)y).NoAccess)
                            {
                                x = Rnd.Next(36, 61);
                                y = Rnd.Next(125, 155);
                            }
                            C.Teleport(2071, (ushort)x, (ushort)y);
                        }
                        else
                        {
                            x = Rnd.Next(201, 225);
                            y = Rnd.Next(104, 134);
                            while (DM.GetCell((ushort)x, (ushort)y).NoAccess)
                            {
                                x = Rnd.Next(201, 225);
                                y = Rnd.Next(104, 134);
                            }
                            C.Teleport(2071, (ushort)x, (ushort)y);
                        }
                    }
                }
            }
            else if (Stage == WarStage.RoundThree)
            {
                foreach (Character C in World.H_Chars.Values)
                {
                    if (C.Loc.Map == 2068)
                    {
                        if (C.MyGuild.GuildID == GuildOnNextRound[0])
                        {
                            x = Rnd.Next(36, 61);
                            y = Rnd.Next(125, 155);
                            while (DM.GetCell((ushort)x, (ushort)y).NoAccess)
                            {
                                x = Rnd.Next(36, 61);
                                y = Rnd.Next(125, 155);
                            }
                            C.Teleport(2071, (ushort)x, (ushort)y);
                        }
                        else
                        {
                            x = Rnd.Next(201, 225);
                            y = Rnd.Next(104, 134);
                            while (DM.GetCell((ushort)x, (ushort)y).NoAccess)
                            {
                                x = Rnd.Next(201, 225);
                                y = Rnd.Next(104, 134);
                            }
                            C.Teleport(2071, (ushort)x, (ushort)y);
                        }
                    }
                }
                NextStage = WarStage.None;
            }
            World.SendMsgToAll("SYSTEM", $"Characters teleported to war map! Next stage: {WaitingPeriod}", 2000, 0);
        }

        /// <summary>
        /// Finishes the current round, teleports players to the waiting map and starts 5 minutes countdown
        /// </summary>
        public static void EndRound()
        {
            Random Rnd = new Random();
            DMap DM = (DMap)DMaps.H_DMaps[2068];
            int x;
            int y;
            if (Stage == WarStage.RoundOne)
            {
                NextStage = WarStage.RoundTwo;
                if (World.H_Effects.ContainsKey(2071))
                    foreach (Game.MapEffect M in World.H_Effects[2071].Values)
                        M.Dissappear();
            }
            else
                NextStage = WarStage.RoundThree;

            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 2071)
                {
                    x = Rnd.Next(0, 89);
                    y = Rnd.Next(0, 89);
                    while (DM.GetCell((ushort)x, (ushort)y).NoAccess)
                    {
                        x = Rnd.Next(0, 89);
                        y = Rnd.Next(0, 89);
                    }
                    C.Teleport(2068, (ushort)x, (ushort)y);
                }
            }
            Stage = WarStage.Waiting;
            WaitingPeriod = DateTime.Now.AddMinutes(Break);
            World.SendMsgToAll("SYSTEM", $"Characters teleported to waiting map! War starts at: {WaitingPeriod}", 2000, 0);

            SortedDictionary<ulong, Entry> List = new SortedDictionary<ulong, Entry>();
            foreach (KeyValuePair<ushort, Score> S in Scores)
            {
                ushort Key = (ushort)S.Key;

                if (!Guilds.AllTheGuilds.ContainsKey(Key))
                    continue;

                Score Value = (Score)S.Value;
                if (List.ContainsKey(Value.GuildScore))
                {
                    Entry e = List[Value.GuildScore];
                    e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                    List.Remove(Value.GuildScore);
                    List.Add(Value.GuildScore, e);
                }
                else
                {
                    Entry e = new Entry();
                    e.Guilds = new List<Guild>();
                    e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                    List.Add(Value.GuildScore, e);
                }
            }
            int Place = 0;
            GuildOnNextRound = new List<uint>();
            foreach (KeyValuePair<ulong, Entry> entries in List.Reverse())
            {
                foreach (Guild eGuild in entries.Value.Guilds)
                {
                    GuildOnNextRound.Add(eGuild.GuildID);
                    Place++;
                    if (Stage == WarStage.RoundOne && Place == 3)
                        break;
                    else if (Place == 1)
                        break;
                }
                if (Stage == WarStage.RoundOne && Place == 3)
                    break;
                else if (Place == 1)
                    break;
            }
            World.SendMsgToAll("SYSTEM", $"Guilds on next round: {string.Join(", ", GuildOnNextRound)}", 2000, 0);
        }

        public static void RoundOne()
        {
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 2071)
                {
                    //Dictionary<ushort, ushort> Cells = new Dictionary<ushort, ushort>();
                    //for (int x = 0; x < 20; x++)
                    //{
                    //    if (MyMath.PointDistance)
                    //}
                    //DMaps.H_DMaps[C.Loc.Map].GetCell()
                    //foreach ()
                    if (Circle(C.Loc.X, C.Loc.Y))
                    {
                        AddScore(C.MyGuild, 1);
                        World.SendMsgToAll("SYSTEM", $"{C.Name} added 1 Point to {C.MyGuild.GuildName}", 2000, 0);
                    }
                }
            }
        }

        static bool Circle(int x, int y)
        {
            if (MyMath.InBox(127, 127, x, y, 15))
                return true;
            else if (MyMath.InBox(151, 55, x, y, 15))
                return true;
            else if (MyMath.InBox(103, 199, x, y, 15))
                return true;
            //if (Math.Pow((x - 125), 2) + Math.Pow((y - 125), 2) < Math.Pow(10,2))
            //    return true;
            //else if (Math.Pow((x - 149), 2) + Math.Pow((y - 53), 2) < Math.Pow(10,2))
            //    return true;
            //else if (Math.Pow((x - 101), 2) + Math.Pow((y - 197), 2) < Math.Pow(10,2))
            //    return true;
            return false;
        }

        /// <summary>
        /// Called after the war has started - spawns all gates and pole
        /// </summary>
        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, Score>();
            LastScores = DateTime.Now;

            //ThePole = new SOB() { EntityID = 6726, Type = Looks.Pole, Mesh = 1137, CurHP = 15000000, MaxHP = 15000000, Loc = new Location() { Map = 1844, X = 114, Y = 163 }, War = false, LastWinner = LastWinner };
            //StatueTwo
            //ThePole.AddSOB();
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
                Score S = new Score();
                S.GuildScore = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                Score S = (Score)Scores[G.GuildID];
                S.GuildScore += Points;
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

                foreach (KeyValuePair<ushort, Score> Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    Score Value = (Score)Score.Value;

                    if (!Guilds.AllTheGuilds.ContainsKey(Key))
                        continue;

                    if (sortdict.ContainsKey(Value.GuildScore))
                    {
                        Entry e = sortdict[Value.GuildScore];
                        e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                        sortdict.Remove(Value.GuildScore);
                        sortdict.Add(Value.GuildScore, e);
                    }
                    else
                    {
                        Entry e = new Entry();
                        e.Guilds = new List<Guild>();
                        e.Guilds.Add(Guilds.AllTheGuilds[Key] as Guild);
                        sortdict.Add(Value.GuildScore, e);
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
            //World.H_SOBs[ThePole.EntityID].War = true;
            loadstats();
            World.SendMsgToAll("SYSTEM", "Counter Clock War has begun! Talk to ObscureWarrior in TwinCity to join!", 2011, 0);
            War = true;
        }

        /// <summary>
        /// Handles CCGW End, announces winner and teleports everyone back to Twin City
        /// </summary>
        public static void EndWarForGood()
        {
            War = false;
            //World.H_SOBs[ThePole.EntityID].War = false;
            CCPrize = true;
            LastWinner.Wins++;
            foreach (Character C in World.H_Chars.Values)
                if (C.Loc.Map == 1844)
                    C.Teleport(1002, 430, 378);
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the Counter Clock War! Congratulations!", 2011, 0);
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
        public static void PoleTakedown(uint EntityID)
        {
            Score Highest = new Score();
            Highest.GuildScore = 0;

            foreach (Score Score in Scores.Values)
            {
                if (Score.GuildScore > Highest.GuildScore)
                    Highest = Score;
            }

            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild; StatueOne.ReSpawn();
                World.H_SOBs[StatueOne.EntityID].LastWinner = Highest.TheGuild;
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "Counter Clock War started!", 2000, 0);
            }

            //World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            //World.H_SOBs[ThePole.EntityID].ReSpawn();
            Scores = new Dictionary<ushort, Score>();
            SendScores();
        }
    }
}
