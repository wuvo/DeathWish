using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.Features
{
    public class PoleWarDC
    {
        public static bool signup = false;
        public static bool PoleDcPrize = false;

        public class TCScore
        {
            public Guild TheGuild;
            public uint Score;
        }
        class Entry
        {
            public List<Guild> Guilds;
        }

        public static SOB ThePole;
        public static bool War;
        public static Dictionary<ushort, TCScore> Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;


        /// <summary>
        /// Called after the war has started - spawns all gates and pole
        /// </summary>
        public static void Init()
        {
            War = false;
            Scores = new Dictionary<ushort, TCScore>();
            LastScores = DateTime.Now;

            ThePole = new SOB() { EntityID = 6735, Type = Looks.Pole, Mesh = 1137, CurHP = 20000000, MaxHP = 20000000, Loc = new Location() { Map = 1000, X = 475, Y = 662 }, War = false, LastWinner = LastWinner };
            ThePole.AddSOB();





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
                TCScore S = new TCScore();
                S.Score = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                TCScore S = (TCScore)Scores[G.GuildID];
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

                foreach (KeyValuePair<ushort, TCScore> Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    TCScore Value = (TCScore)Score.Value;

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
                if (C.Loc.Map == 1000)
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

            loadstats();
            //World.SendMsgToAll("SYSTEM", "PoleWarDC has begun! Talk to ObscureWarrior in TwinCity to join!", 2011, 0);
            War = true;
        }

        /// <summary>
        /// Handles CCGW End, announces winner and teleports everyone back to Twin City
        /// </summary>
        public static void EndWarForGood()
        {
            War = false;
            World.H_SOBs[ThePole.EntityID].War = false;
            PoleDcPrize = true;
            if (LastWinner != null)
                LastWinner.Wins++;
            MySQL.MySqlCommand Cmd2;
            Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
            Cmd2.Update("guildwars").Set("winner", Features.PoleWarDC.LastWinner.GuildName).Where("id", 9).Execute();
            foreach (Character C in World.H_Chars.Values)
                if (C.Loc.Map == 1000)
                    ChangePKMode(C, PKMode.Capture);
            if (LastWinner != null)
            {
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the DC Pole War! Congratulations!", 2011, 0);
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the DC Pole War! Congratulations!", 2000, 0);
            }
            else
                World.SendMsgToAll("SYSTEM", "The DC Pole War has ended and there were no winners!", 2000, 0);
        }

        /// <summary>
        /// Saves CCGW stats
        /// </summary>
        public static void Savestats()
        {
            System.IO.MemoryStream FS = new System.IO.MemoryStream();

            System.IO.BinaryWriter BW = new System.IO.BinaryWriter(FS);
            BW.Write(PoleDcPrize);

            byte[] buffer = FS.ToArray();
            if (!World.LowRatedServer)
                System.IO.File.WriteAllBytes(@"C:\OldCODB\TCWarStats.gw", buffer);
            else System.IO.File.WriteAllBytes(@"C:\OldCODB\TCWarStatsNewServer.gw", buffer);
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
                if (System.IO.File.Exists(@"C:\OldCODB\TCWarStats.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\TCWarStats.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    PoleDcPrize = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\TCWarStatsNewServer.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\TCWarStatsNewServer.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    PoleDcPrize = BR.ReadBoolean();
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

            TCScore Highest = new TCScore();
            Highest.Score = 0;

            foreach (TCScore Score in Scores.Values)
                if (Score.Score > Highest.Score)
                    Highest = Score;

            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild; ThePole.ReSpawn();
                World.H_SOBs[ThePole.EntityID].LastWinner = Highest.TheGuild;
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " DC Pole War have won!", 2011, 0);
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " DC Pole War have won!", 2000, 0);
            }
            World.H_SOBs[ThePole.EntityID].CurHP = World.H_SOBs[ThePole.EntityID].MaxHP;
            World.H_SOBs[ThePole.EntityID].ReSpawn();
            Scores = new Dictionary<ushort, TCScore>();
            SendScores();
            Features.PoleWarDC.EndWarForGood();

        }
        public static void ChangePKMode(Character C, PKMode Mode)
        {
            C.PKMode = Mode;
            if (C.MyClient != null)
                C.MyClient.AddSend(Packets.GeneralData(C.EntityID, (uint)Mode, 0, 0, 96));
        }
    }
}
