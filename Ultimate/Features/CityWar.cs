using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Game;
using System.Threading;

namespace NewestCOServer.Features
{
    public class CityWar
    {
        public static ArrayList Players = new ArrayList();
        public static bool signup = false;
        public static bool CCPrize = false;
        public static DateTime[] ChestTime = new DateTime[3];

        public class CWScore
        {
            public Guild TheGuild;
            public uint Score;
        }
        public struct Poles
        {
            public Location Loc;
            public uint MaxHP;
            public uint CurHP;
            public uint Mesh;
            public uint EntityID;

            public void Spawn(Character C, bool Check)
            {
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

                            if (Damage >= 500)
                                C.Silvers += Damage / 45;

                            C.MyGuild.Fund += Damage / 50;
                            C.GuildDonation += Damage / 50;

                        }
                        C.MyClient.AddSend(Packets.GuildInfo(C.MyGuild, C));
                        uint CurHP2 = CurHP;
                        if (CurHP > Damage)
                            CurHP -= Damage;
                        else CurHP = 0;
                        if (CurHP > 15000000)
                        {
                            World.ExcAdd += "Pole HP: " + CurHP + "\r\n";
                            Console.WriteLine("CW PROBLEM! Pole HP: " + CurHP);
                            if (CurHP2 < 15000000)
                                CurHP = CurHP2;
                            else CurHP = 7500000;
                        }
                        AddScore(C.MyGuild, Damage);
                    }
                }
            }
        }

        public struct Gates
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
                    if (EntityID >= 6703 && EntityID <= 6708)//Left Gate
                    {
                        if (value) Mesh = 250;
                        else Mesh = 240;
                    }
                    else if (EntityID >= 6709 && EntityID <= 6725)//Right Gate
                    {
                        if (value) Mesh = 280;
                        else Mesh = 270;
                    }
                }
                get
                {
                    if (EntityID >= 6703 && EntityID <= 6708)//Left Gate
                    {
                        if (Mesh == 250) return true;
                        else return false;
                    }
                    else if (EntityID >= 6709 && EntityID <= 6725)//Right Gate
                    {
                        if (Mesh == 280) return true;
                        else return false;
                    }
                    return false;
                }
            }
            public void Spawn(Character C, bool Check)
            {
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 28) || !Check))
                    C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void ReSpawn()
            {
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
                    if (!Opened)
                    {
                        Opened = true;
                        ReSpawn();
                    }
                }
                else
                    CurHP -= Damage;
            }
        }
        public static Poles PC;
        public static Poles AC;
        public static Poles DC;
        public static Poles BI;
        public static Gates RGPC;
        public static Gates RGAC;
        public static Gates RGDC;
        public static Gates RGBI;
        public static Gates LGPC;
        public static Gates LGAC;
        public static Gates LGDC;
        public static Gates LGBI;

        public static bool War;
        public static Hashtable Scores;
        public static DateTime LastScores;
        public static Guild LastWinner;

        public static void Init()
        {
            War = false;
            Scores = new Hashtable();
            LastScores = DateTime.UtcNow;

            ThePole = new Poles();
            ThePole.EntityID = 6726;
            ThePole.Mesh = 1137;
            ThePole.CurHP = 15000000;
            ThePole.MaxHP = 15000000;
            ThePole.Loc = new Location();
            ThePole.Loc.Map = 1844;
            ThePole.Loc.X = 114;
            ThePole.Loc.Y = 163;

            #region Central Gates
            #region Pole
            RG17 = new Gates();
            RG17.EntityID = 6725;
            RG17.Opened = false;
            RG17.MaxHP = 2500000;
            RG17.CurHP = 2500000;
            RG17.Loc = new Location();
            RG17.Loc.Map = 1844;
            RG17.Loc.X = 134;
            RG17.Loc.Y = 167;
            RG17.ReSpawn();

            RG16 = new Gates();
            RG16.EntityID = 6724;
            RG16.Opened = false;
            RG16.MaxHP = 2500000;
            RG16.CurHP = 2500000;
            RG16.Loc = new Location();
            RG16.Loc.Map = 1844;
            RG16.Loc.X = 134;
            RG16.Loc.Y = 163;
            RG16.ReSpawn();

            RG15 = new Gates();
            RG15.EntityID = 6723;
            RG15.Opened = false;
            RG15.MaxHP = 2500000;
            RG15.CurHP = 2500000;
            RG15.Loc = new Location();
            RG15.Loc.Map = 1844;
            RG15.Loc.X = 134;
            RG15.Loc.Y = 159;
            RG15.ReSpawn();
            #endregion

            #region Mid
            RG14 = new Gates();
            RG14.EntityID = 6722;
            RG14.Opened = false;
            RG14.MaxHP = 1500000;
            RG14.CurHP = 1500000;
            RG14.Loc = new Location();
            RG14.Loc.Map = 1844;
            RG14.Loc.X = 172;
            RG14.Loc.Y = 167;
            RG14.ReSpawn();

            RG13 = new Gates();
            RG13.EntityID = 6721;
            RG13.Opened = false;
            RG13.MaxHP = 1500000;
            RG13.CurHP = 1500000;
            RG13.Loc = new Location();
            RG13.Loc.Map = 1844;
            RG13.Loc.X = 172;
            RG13.Loc.Y = 163;
            RG13.ReSpawn();

            RG12 = new Gates();
            RG12.EntityID = 6720;
            RG12.Opened = false;
            RG12.MaxHP = 1500000;
            RG12.CurHP = 1500000;
            RG12.Loc = new Location();
            RG12.Loc.Map = 1844;
            RG12.Loc.X = 172;
            RG12.Loc.Y = 159;
            RG12.ReSpawn();
            #endregion

            #region Entrance
            RG11 = new Gates();
            RG11.EntityID = 6719;
            RG11.Opened = false;
            RG11.MaxHP = 1000000;
            RG11.CurHP = 1000000;
            RG11.Loc = new Location();
            RG11.Loc.Map = 1844;
            RG11.Loc.X = 210;
            RG11.Loc.Y = 167;
            RG11.ReSpawn();

            RG10 = new Gates();
            RG10.EntityID = 6718;
            RG10.Opened = false;
            RG10.MaxHP = 1000000;
            RG10.CurHP = 1000000;
            RG10.Loc = new Location();
            RG10.Loc.Map = 1844;
            RG10.Loc.X = 210;
            RG10.Loc.Y = 163;
            RG10.ReSpawn();

            RG9 = new Gates();
            RG9.EntityID = 6717;
            RG9.Opened = false;
            RG9.MaxHP = 1000000;
            RG9.CurHP = 1000000;
            RG9.Loc = new Location();
            RG9.Loc.Map = 1844;
            RG9.Loc.X = 210;
            RG9.Loc.Y = 159;
            RG9.ReSpawn();
            #endregion
            #endregion

            #region RGs

            #region Last lane
            RG8 = new Gates();
            RG8.EntityID = 6716;
            RG8.Opened = false;
            RG8.MaxHP = 3000000;
            RG8.CurHP = 3000000;
            RG8.Loc = new Location();
            RG8.Loc.Map = 1844;
            RG8.Loc.X = 210;
            RG8.Loc.Y = 124;
            RG8.ReSpawn();

            RG7 = new Gates();
            RG7.EntityID = 6715;
            RG7.Opened = false;
            RG7.MaxHP = 3000000;
            RG7.CurHP = 3000000;
            RG7.Loc = new Location();
            RG7.Loc.Map = 1844;
            RG7.Loc.X = 210;
            RG7.Loc.Y = 128;
            RG7.ReSpawn();

            RG6 = new Gates();
            RG6.EntityID = 6714;
            RG6.Opened = false;
            RG6.MaxHP = 3000000;
            RG6.CurHP = 3000000;
            RG6.Loc = new Location();
            RG6.Loc.Map = 1844;
            RG6.Loc.X = 210;
            RG6.Loc.Y = 207;
            RG6.ReSpawn();

            RG5 = new Gates();
            RG5.EntityID = 6713;
            RG5.Opened = false;
            RG5.MaxHP = 3000000;
            RG5.CurHP = 3000000;
            RG5.Loc = new Location();
            RG5.Loc.Map = 1844;
            RG5.Loc.X = 210;
            RG5.Loc.Y = 203;
            RG5.ReSpawn();
            #endregion

            #region First lane
            RG4 = new Gates();//left side right gate
            RG4.EntityID = 6712;
            RG4.Opened = false;
            RG4.MaxHP = 5000000;
            RG4.CurHP = 5000000;
            RG4.Loc = new Location();
            RG4.Loc.Map = 1844;
            RG4.Loc.X = 172;
            RG4.Loc.Y = 207;
            RG4.ReSpawn();

            RG3 = new Gates();
            RG3.EntityID = 6711;
            RG3.Opened = false;
            RG3.MaxHP = 5000000;
            RG3.CurHP = 5000000;
            RG3.Loc = new Location();
            RG3.Loc.Map = 1844;
            RG3.Loc.X = 172;
            RG3.Loc.Y = 128;
            RG3.ReSpawn();

            RG2 = new Gates();
            RG2.EntityID = 6710;
            RG2.Opened = false;
            RG2.MaxHP = 5000000;
            RG2.CurHP = 5000000;
            RG2.Loc = new Location();
            RG2.Loc.Map = 1844;
            RG2.Loc.X = 172;
            RG2.Loc.Y = 124;
            RG2.ReSpawn();

            RG1 = new Gates(); //left side right gate
            RG1.EntityID = 6709;
            RG1.Opened = false;
            RG1.MaxHP = 5000000;
            RG1.CurHP = 5000000;
            RG1.Loc = new Location();
            RG1.Loc.Map = 1844;
            RG1.Loc.X = 172;
            RG1.Loc.Y = 203;
            RG1.ReSpawn();
            #endregion

            #endregion
            #region LGs
            LG6 = new Gates();
            LG6.EntityID = 6708;
            LG6.Opened = false;
            LG6.MaxHP = 5000000;
            LG6.CurHP = 5000000;
            LG6.Loc = new Location();
            LG6.Loc.Map = 1844;
            LG6.Loc.X = 149;
            LG6.Loc.Y = 145;
            LG6.ReSpawn();

            LG5 = new Gates();
            LG5.EntityID = 6707;
            LG5.Opened = false;
            LG5.MaxHP = 3000000;
            LG5.CurHP = 3000000;
            LG5.Loc = new Location();
            LG5.Loc.Map = 1844;
            LG5.Loc.X = 187;
            LG5.Loc.Y = 145;
            LG5.ReSpawn();

            LG4 = new Gates();
            LG4.EntityID = 6706;
            LG4.Opened = false;
            LG4.MaxHP = 2000000;
            LG4.CurHP = 2000000;
            LG4.Loc = new Location();
            LG4.Loc.Map = 1844;
            LG4.Loc.X = 225;
            LG4.Loc.Y = 145;
            LG4.ReSpawn();

            LG3 = new Gates();
            LG3.EntityID = 6705;
            LG3.Opened = false;
            LG3.MaxHP = 5000000;
            LG3.CurHP = 5000000;
            LG3.Loc = new Location();
            LG3.Loc.Map = 1844;
            LG3.Loc.X = 149;
            LG3.Loc.Y = 186;
            LG3.ReSpawn();

            LG2 = new Gates();
            LG2.EntityID = 6704;
            LG2.Opened = false;
            LG2.MaxHP = 3000000;
            LG2.CurHP = 3000000;
            LG2.Loc = new Location();
            LG2.Loc.Map = 1844;
            LG2.Loc.X = 187;
            LG2.Loc.Y = 186;
            LG2.ReSpawn();

            LG1 = new Gates();
            LG1.EntityID = 6703;
            LG1.Opened = false;
            LG1.MaxHP = 2000000;
            LG1.CurHP = 2000000;
            LG1.Loc = new Location();
            LG1.Loc.Map = 1844;
            LG1.Loc.X = 225;
            LG1.Loc.Y = 186;
            LG1.ReSpawn();
            #endregion
        }
        public static void AddScore(Guild G, uint Points)
        {
            if (!Scores.Contains(G.GuildID))
            {
                CWScore S = new CWScore();
                S.Score = Points;
                S.TheGuild = G;
                Scores.Add(G.GuildID, S);
            }
            else
            {
                CWScore S = (CWScore)Scores[G.GuildID];
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

                foreach (DictionaryEntry Score in Scores)
                {
                    ushort Key = (ushort)Score.Key;
                    CWScore Value = (CWScore)Score.Value;

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
            LastScores = DateTime.UtcNow;
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
        static long LongRandom(long min, long max)
        {
            byte[] buf = new byte[8];
            Program.Rnd.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
        public static void StartWar(DMap)
        {
            signup = false;
            Init();
            LG1.ReSpawn();
            LG2.ReSpawn();
            LG3.ReSpawn();
            LG4.ReSpawn();
            LG5.ReSpawn();
            LG6.ReSpawn();
            RG1.ReSpawn();
            RG2.ReSpawn();
            RG3.ReSpawn();
            RG4.ReSpawn();
            RG5.ReSpawn();
            RG6.ReSpawn();
            RG7.ReSpawn();
            RG8.ReSpawn();
            RG9.ReSpawn();
            RG10.ReSpawn();
            RG11.ReSpawn();
            RG12.ReSpawn();
            RG13.ReSpawn();
            RG14.ReSpawn();
            RG15.ReSpawn();
            RG16.ReSpawn();
            loadstats();
            World.SendMsgToAll("SYSTEM", "City War has begun!", 2011, 0);
            War = true;
            //loadstats();


        }
        public static void EndWarForGood()
        {
            War = false;
            CCPrize = true;
            LastWinner.Wins++;
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 1844 || C.Loc.Map == 1090)
                {
                    C.Teleport(1002, 430, 378);
                }
            }
            World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won the Counter Clock War! Congratulations!", 2011, 0);
        }
        public static void Savestats()
        {
            System.IO.MemoryStream FS = new System.IO.MemoryStream();

            System.IO.BinaryWriter BW = new System.IO.BinaryWriter(FS);
            BW.Write(CCPrize);

            byte[] buffer = FS.ToArray();
            if (!World.LowRatedServer)
                System.IO.File.WriteAllBytes(@"C:\OldCODB\CityWarStats.gw", buffer);
            else System.IO.File.WriteAllBytes(@"C:\OldCODB\CityWarStatsNewServer.gw", buffer);
            BW.Close();
            FS.Close();
        }
        public static void loadstats()
        {
            if (!World.LowRatedServer)
            {
                if (System.IO.File.Exists(@"C:\OldCODB\CityWarStats.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\CityWarStats.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    CCPrize = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\CityWarStatsNewServer.gw"))
                {
                    byte[] buffer = System.IO.File.ReadAllBytes(@"C:\OldCODB\CityWarStatsNewServer.gw");
                    System.IO.MemoryStream FS = new System.IO.MemoryStream(buffer);
                    System.IO.BinaryReader BR = new System.IO.BinaryReader(FS);
                    CCPrize = BR.ReadBoolean();
                    BR.Close();
                    FS.Close();
                }
            }
        }

        public static void PoleTakedown()
        {
            #region Pole
            RG17.Opened = false;
            RG16.Opened = false;
            RG15.Opened = false;

            RG17.CurHP = RG17.MaxHP;
            RG16.CurHP = RG16.MaxHP;
            RG15.CurHP = RG15.MaxHP;

            RG17.ReSpawn();
            RG16.ReSpawn();
            RG15.ReSpawn();
            #endregion
            #region Mid
            RG14.Opened = false;
            RG13.Opened = false;
            RG12.Opened = false;

            RG14.CurHP = RG14.MaxHP;
            RG13.CurHP = RG13.MaxHP;
            RG12.CurHP = RG12.MaxHP;

            RG14.ReSpawn();
            RG13.ReSpawn();
            RG12.ReSpawn();
            #endregion
            #region MidLeft
            RG1.Opened = false;
            RG4.Opened = false;

            RG1.CurHP = RG1.MaxHP;
            RG4.CurHP = RG4.MaxHP;

            RG1.ReSpawn();
            RG4.ReSpawn();
            #endregion
            #region MidRight
            RG2.Opened = false;
            RG3.Opened = false;

            RG2.CurHP = RG2.MaxHP;
            RG3.CurHP = RG3.MaxHP;

            RG2.ReSpawn();
            RG3.ReSpawn();
            #endregion

            LG2.Opened = false;
            RG5.Opened = false;

            LG2.CurHP = LG2.MaxHP;
            LG5.CurHP = LG5.MaxHP;
            LG2.ReSpawn();
            LG5.ReSpawn();

            CWScore Highest = new CWScore();
            Highest.Score = 0;

            foreach (CWScore Score in Scores.Values)
            {
                if (Score.Score > Highest.Score)
                    Highest = Score;
            }

            ThePole.CurHP = ThePole.MaxHP;
            if (Highest.TheGuild != null)
            {
                LastWinner = Highest.TheGuild; ThePole.ReSpawn();
                World.SendMsgToAll("SYSTEM", LastWinner.GuildName + " have won!", 2000, 0);
                World.SendMsgToAll("SYSTEM", "Counter Clock War started!", 2000, 0);
            }
            Scores = new Hashtable();
            SendScores();
        }
    }
}
