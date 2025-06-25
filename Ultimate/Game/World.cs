using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using Ultimate.PacketHandling;

namespace Ultimate.Game
{
    public class ServerTime
    {
        #region Variables
        public static DateTime WorldTime = DateTime.Now;
        public static string WorldTimeString = Convert.ToString(WorldTime);
        public static string[] WorldTimeSplit = WorldTimeString.Split(' ');
        public static string[] i = WorldTimeSplit;

        public static string iSplit0 = i[0];
        public static string[] WorldDMY = iSplit0.Split('/');

        public static string iSplit1 = i[1];
        public static string[] WorldHMS = iSplit1.Split(':');

        public static byte Day = Convert.ToByte(WorldDMY[0]);

        public static DayOfWeek DayOfWeek = DateTime.Now.DayOfWeek;
        public static string DayName = Convert.ToString(DayOfWeek);

        public static byte Month = Convert.ToByte(WorldDMY[1]);
        public static ushort Year = Convert.ToUInt16(WorldDMY[2]);

        public static byte Hour = Convert.ToByte(WorldHMS[0]);
        public static byte Minute = Convert.ToByte(WorldHMS[1]);
        public static ushort Second = Convert.ToUInt16(WorldHMS[2]);

        public static string HourMin = Hour + ":" + Minute;

        public static string AMPM = i[2];
        #endregion
        public static void GetDate()
        {
            WorldTime = DateTime.Now;
            WorldTimeString = Convert.ToString(WorldTime);
            WorldTimeSplit = WorldTimeString.Split(' ');
            i = WorldTimeSplit;

            iSplit0 = i[0];
            WorldDMY = iSplit0.Split('/');

            iSplit1 = i[1];
            WorldHMS = iSplit1.Split(':');

            Day = Convert.ToByte(WorldDMY[0]);

            DayOfWeek = DateTime.Now.DayOfWeek;
            DayName = Convert.ToString(DayOfWeek);

            Month = Convert.ToByte(WorldDMY[1]);
            Year = Convert.ToUInt16(WorldDMY[2]);

            Hour = Convert.ToByte(WorldHMS[0]);
            Minute = Convert.ToByte(WorldHMS[1]);
            Second = Convert.ToUInt16(WorldHMS[2]);

            HourMin = Hour + ":" + Minute;

            AMPM = i[2];
        }
    }
    public struct MapInfo
    {
        public Vector2[] Spawns;
    }
    public struct BroadCastMessage
    {
        public string Name;
        public string Message;
        public byte Place;
    }
    public struct Vector2
    {
        public ushort X;
        public ushort Y;

        public Vector2(ushort X, ushort Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    public struct EmpireInfo
    {
        public string Name;
        public ulong Donation;
        public uint ID;
        public byte Gender;
        public void WriteThis(System.IO.BinaryWriter BW)
        {
            if (Name == null)
                Name = "";
            BW.Write(Name.Length);
            BW.Write(Encoding.ASCII.GetBytes(Name));
            BW.Write(Donation);
            BW.Write(ID);
            BW.Write(Gender);
        }
        public void ReadThis(System.IO.BinaryReader BR)
        {
            try
            {
                Name = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                Donation = BR.ReadUInt64();
                ID = BR.ReadUInt32();
                Gender = BR.ReadByte();
            }
            catch
            {
                Name = "";
                Donation = 0;
                ID = 0;
                Gender = 0;
            }
        }
    }
    public struct KOInfo
    {
        public string Name;
        public int KillCount;
        public uint KOID;

        public void WriteThis(System.IO.BinaryWriter BW)
        {
            if (Name == null)
                Name = "";
            BW.Write(Name.Length);
            BW.Write(Encoding.ASCII.GetBytes(Name));
            BW.Write(KillCount);
            BW.Write(KOID);
        }
        public void ReadThis(System.IO.BinaryReader BR)
        {
            try
            {
                Name = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                KillCount = BR.ReadInt32();
                KOID = BR.ReadUInt32();
            }
            catch
            {
                Name = "";
                KillCount = 0;
                KOID = 0;
            }
        }
    }
    public enum StringType
    {
        None = 0,
        Fireworks = 1,
        CreateGuild = 2,
        GuildName = 3,
        ChangeTitle = 4,
        DeleteRole = 5,
        Spouse = 6,
        QueryNpc = 7,
        Wanted = 8,
        MapEffect = 9,
        Effect = 10,
        MemberList = 11,
        KickoutGuildMember = 12,
        QueryWanted = 13,
        QueryPoliceWanted = 14,
        PoliceWanted = 15,
        ViewEquipSpouse = 16,
        AddDicePlayer = 17,
        DeleteDicePlayer = 18,
        DiceBonus = 19,
        Sound = 20,
        GuildAllies = 21,
        GuildEnemies = 22,
        WhisperWindowInfo = 26
    }

    public struct MapEffect
    {
        public MEffect Info;
        public Location Loc;
        public uint Silvers;
        public uint Owner;
        public DateTime DropTime;
        public DateTime LastDrop;
        public uint UID;

        public void Drop()
        {
            if (Info.ID != 0 && UID != 0)// && Info.DBInfo.ID != 0)
            {
                if (Info.UID == 0)
                    Info.UID = (uint)World.Rnd.Next(900000, 999999);
                if (!World.H_Effects.ContainsKey(Loc.Map))
                    World.H_Effects.Add(Loc.Map, new ConcurrentDictionary<uint, MapEffect>());

                ConcurrentDictionary<uint, MapEffect> Map = (ConcurrentDictionary<uint, MapEffect>)World.H_Effects[Loc.Map];

                for (byte i = 0; i < 10; i++)
                {
                    if (Map.ContainsKey(UID))
                        UID = (uint)World.Rnd.Next(900000, 999999);
                    else break;
                }
                if (!Map.ContainsKey(UID))
                {
                    if (!World.ActiveSquamas.Contains(UID))
                        World.ActiveSquamas.Add(UID);
                    else
                        for (byte i = 0; i < 10; i++)
                        {
                            if (Map.ContainsKey(UID))
                                UID = (uint)World.Rnd.Next(900000, 999999);
                            else break;
                        }
                    World.Action(this, Packets.MapEffect(this).Get);
                    if (!Map.TryAdd(UID, this)) Map.TryAdd(UID, this);
                }
            }
        }

        //public bool FindPlace(Hashtable Map)
        //{
        //    if (Map == null) return true;
        //    DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
        //    bool FoundPlace = true;
        //    short X = (short)Loc.X;
        //    short Y = (short)Loc.Y;
        //    for (short x = -1; x < 2; x++)
        //    {
        //        for (short y = -1; y < 2; y++)
        //        {
        //            try
        //            {
        //                FoundPlace = true;
        //                if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
        //                    FoundPlace = false;
        //                else
        //                {
        //                    foreach (MapEffect D in Map.Values)
        //                    {
        //                        //FoundPlace = true;
        //                        if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
        //                        {
        //                            FoundPlace = false;
        //                            break;

        //                        }
        //                    }
        //                }


        //            }
        //            catch { FoundPlace = false; }
        //            if (FoundPlace)
        //            {
        //                Loc.X = (ushort)(X + x);
        //                Loc.Y = (ushort)(Y + y);
        //                break;
        //            }
        //        }
        //        if (FoundPlace)
        //            break;
        //    }

        //    if (!FoundPlace)
        //        return false;
        //    return true;
        //}
        //public bool FindPlace(ConcurrentDictionary<uint, MapEffect> Map)
        //{
        //    if (Map == null) return true;
        //    DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
        //    bool FoundPlace = true;
        //    short X = (short)Loc.X;
        //    short Y = (short)Loc.Y;
        //    for (short x = -1; x < 2; x++)
        //    {
        //        for (short y = -1; y < 2; y++)
        //        {
        //            try
        //            {
        //                FoundPlace = true;
        //                if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
        //                    FoundPlace = false;
        //                else
        //                {
        //                    foreach (MapEffect D in Map.Values)
        //                    {
        //                        //FoundPlace = true;
        //                        if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
        //                        {
        //                            FoundPlace = false;
        //                            break;

        //                        }
        //                    }
        //                }


        //            }
        //            catch { FoundPlace = false; }
        //            if (FoundPlace)
        //            {
        //                Loc.X = (ushort)(X + x);
        //                Loc.Y = (ushort)(Y + y);
        //                break;
        //            }
        //        }
        //        if (FoundPlace)
        //            break;
        //    }

        //    if (!FoundPlace)
        //        return false;
        //    return true;
        //}

        public bool FindPlace(ConcurrentDictionary<uint, MapEffect> Map)
        {
            if (Map == null) return true;
            DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
            if (!DMaps.H_DMaps.ContainsKey(Loc.Map))
                Console.WriteLine("Does not contain map: " + Loc.Map);
            //foreach (MapEffect M in Map.Values)
            //    if (Loc.X == M.Loc.X && Loc.Y == M.Loc.Y)
            //        return false;
            bool FoundPlace = true;
            short X = (short)Loc.X;
            short Y = (short)Loc.Y;
            for (short x = -1; x < 2; x++)               // x = -1; x < 2     y = -1; y < 2
            {
                for (short y = -1; y < 2; y++)
                {
                    try
                    {
                        FoundPlace = true;
                        if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
                            FoundPlace = false;
                        else
                        {
                            foreach (MapEffect D in Map.Values)
                            {
                                //FoundPlace = true;
                                if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
                                {
                                    FoundPlace = false;
                                    break;

                                }
                            }
                        }


                    }
                    catch { FoundPlace = false; }
                    if (FoundPlace)
                    {
                        Loc.X = (ushort)(X + x);
                        Loc.Y = (ushort)(Y + y);
                        //bool f = true;
                        //foreach (NPC N in Game.World.H_NPCs.Values)
                        //    if (N.Loc.Map == Loc.Map && N.Loc.X == Loc.X && N.Loc.Y == Loc.Y)
                        //    {
                        //        f = false;
                        //        break;
                        //    }
                        //if (!f)
                        // continue;
                        break;
                    }
                }
                if (FoundPlace)
                    break;
            }

            if (!FoundPlace)
                return false;


            return true;
        }
        public void Dissappear()
        {
            if (((ConcurrentDictionary<uint, MapEffect>)World.H_Effects[Loc.Map]).ContainsKey(UID))
            {
                if (World.ActiveSquamas.Contains(UID))
                    World.ActiveSquamas.Remove(UID);
                World.Action(this, Packets.MapEffectRemove(UID, Info.ID, Loc.X, Loc.Y).Get);
                MapEffect d;
                if (!((ConcurrentDictionary<uint, MapEffect>)World.H_Effects[Loc.Map]).TryRemove(UID, out d)) ((ConcurrentDictionary<uint, MapEffect>)World.H_Effects[Loc.Map]).TryRemove(UID, out d);
            }
        }
    }
    public struct DroppedItem
    {
        public Item Info;
        public Location Loc;
        public uint Silvers;
        public uint Owner;
        public DateTime DropTime;
        public uint UID;

        public void Drop()
        {
            if (Info.ID != 0 && UID != 0)// && Info.DBInfo.ID != 0)
            {
                if (Info.UID == 0)
                    Info.UID = (uint)World.Rnd.Next(10000000);
                if (!World.H_Items.ContainsKey(Loc.Map))
                    //                    World.H_Items.Add(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                    World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());

                // World.H_Items.Add(Loc.Map, new Hashtable());

                ConcurrentDictionary<uint, DroppedItem> Map = (ConcurrentDictionary<uint, DroppedItem>)World.H_Items[Loc.Map];

                for (byte i = 0; i < 10; i++)
                {
                    if (Map.ContainsKey(UID))
                        UID = (uint)World.Rnd.Next(10000000);
                    else break;
                }
                if (!Map.ContainsKey(UID))
                {
                    // if (Map.Count < 5990)
                    // {
                    World.Action(this, Packets.ItemDrop(this).Get);
                    //  Map.Add(UID, this);
                    if (!Map.TryAdd(UID, this)) Map.TryAdd(UID, this);
                    // }
                    // else World.ExcAdd += "Items on ground count : " + Map.Count + "\r\n";
                }
            }
        }

        public bool FindPlace(Hashtable Map)
        {
            if (Map == null) return true;
            DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
            bool FoundPlace = true;
            short X = (short)Loc.X;
            short Y = (short)Loc.Y;
            for (short x = -1; x < 2; x++)
            {
                for (short y = -1; y < 2; y++)
                {
                    try
                    {
                        FoundPlace = true;
                        if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
                            FoundPlace = false;
                        else
                        {
                            foreach (DroppedItem D in Map.Values)
                            {
                                //FoundPlace = true;
                                if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
                                {
                                    FoundPlace = false;
                                    break;

                                }
                            }
                        }


                    }
                    catch { FoundPlace = false; }
                    if (FoundPlace)
                    {
                        Loc.X = (ushort)(X + x);
                        Loc.Y = (ushort)(Y + y);
                        break;
                    }
                }
                if (FoundPlace)
                    break;
            }

            if (!FoundPlace)
                return false;
            return true;
        }
        public bool FindPlace(ThreadSafeDictionary<uint, DroppedItem> Map)
        {
            if (Map == null) return true;
            DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
            bool FoundPlace = true;
            short X = (short)Loc.X;
            short Y = (short)Loc.Y;
            for (short x = -1; x < 2; x++)
            {
                for (short y = -1; y < 2; y++)
                {
                    try
                    {
                        FoundPlace = true;
                        if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
                            FoundPlace = false;
                        else
                        {
                            foreach (DroppedItem D in Map.Values)
                            {
                                //FoundPlace = true;
                                if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
                                {
                                    FoundPlace = false;
                                    break;

                                }
                            }
                        }


                    }
                    catch { FoundPlace = false; }
                    if (FoundPlace)
                    {
                        Loc.X = (ushort)(X + x);
                        Loc.Y = (ushort)(Y + y);
                        break;
                    }
                }
                if (FoundPlace)
                    break;
            }

            if (!FoundPlace)
                return false;
            return true;
        }


        public bool FindPlace(ConcurrentDictionary<uint, DroppedItem> Map)
        {
            if (Map == null) return true;
            DMap DM = (DMap)DMaps.H_DMaps[Loc.Map];
            bool FoundPlace = true;
            short X = (short)Loc.X;
            short Y = (short)Loc.Y;
            for (short x = -1; x < 2; x++)               // x = -1; x < 2     y = -1; y < 2
            {
                for (short y = -1; y < 2; y++)
                {
                    try
                    {
                        FoundPlace = true;
                        if (DM.GetCell((ushort)(X + x), (ushort)(Y + y)).NoAccess)
                            FoundPlace = false;
                        else
                        {
                            foreach (DroppedItem D in Map.Values)
                            {
                                //FoundPlace = true;
                                if ((D.Loc.X == (ushort)(X + x) && D.Loc.Y == (ushort)(Y + y)))
                                {
                                    FoundPlace = false;
                                    break;

                                }
                            }
                        }


                    }
                    catch { FoundPlace = false; }
                    if (FoundPlace)
                    {
                        Loc.X = (ushort)(X + x);
                        Loc.Y = (ushort)(Y + y);
                        //bool f = true;
                        //foreach (NPC N in Game.World.H_NPCs.Values)
                        //    if (N.Loc.Map == Loc.Map && N.Loc.X == Loc.X && N.Loc.Y == Loc.Y)
                        //    {
                        //        f = false;
                        //        break;
                        //    }
                        //if (!f)
                        // continue;
                        break;
                    }
                }
                if (FoundPlace)
                    break;
            }

            if (!FoundPlace)
                return false;


            return true;
        }
        public void Dissappear()
        {
            if (((ConcurrentDictionary<uint, DroppedItem>)World.H_Items[Loc.Map]).ContainsKey(UID))
            {
                World.Action(this, Packets.ItemDropRemove(UID, Info.ID, Loc.X, Loc.Y).Get);
                DroppedItem d;
                if (!((ConcurrentDictionary<uint, DroppedItem>)World.H_Items[Loc.Map]).TryRemove(UID, out d)) ((ConcurrentDictionary<uint, DroppedItem>)World.H_Items[Loc.Map]).TryRemove(UID, out d);
            }
        }
    }
    public struct IPLog
    {
        public ushort Logs;
        public DateTime LogDate;
    }
    public struct MessageBoard
    {
        public string Name;
        public string Msg;
        public DateTime Time;
    }
    public class World
    {
        #region Event Variables
        public static bool EventMet = false;
        public static bool EventDB = false;
        public static bool EventPlus = false;
        public static bool EventSuper = false;
        public static bool EventElite = false;
        public static bool EventGem = false;
        public static bool EventProfExp = false;
        public static bool EventSkillExp = false;
        public static bool DropEvent = false;
        #endregion
        public static bool SaveGuilds = false;
        public static bool WorldChat = true;
        public static bool ChatForm = false;
        public static uint MapN = 1002;
        public static ushort Xn = 300;
        public static ushort Yn = 300;
        public static string M = "";
        public static string Hint1 = "";
        public static string Hint2 = "";
        public static bool Gano = false;
        public static bool Titan = false;
        public static byte Dis2 = 0;
        public static byte Dis3 = 0;
        public static byte LeftFlank = 0;
        public static byte RightFlank = 0;
        public static bool PlutoKilled = false, SnakeKingAgain = false, ExpMob = false, Ball = false, Dragon = false, BossByPM = false, Raikou = false, Capricorn = false, ThrillingSpook = false, Tash = false;
        public static string CurrentBoss = "";
        public static bool AncientDevil = false;
        public static bool GuildBeast = false;
        public static bool GuildBeastByPM = false;
        public static ArrayList PKTIPs = new ArrayList();
        public static ArrayList PKTList = new ArrayList();
        public static bool CCMob = false;
        //public static bool DBDevil = true;
        public static bool Drawing = false;
        public static bool Found = true;
        public static int DRPts = 1000;
        public static int ERPts = 1000;
        public static DateTime DREvent;
        public static DateTime EREvent;
        public static bool LastWeek = false;
        //public static DateTime Squamaspawn;
        public static bool HodorEvent = false;
        //public static byte Interserver = 0;
        public static ulong demonBoxesCur = 0;
        public static int _serverVersion = 0;
        public static ushort UnlimitedStaminaMap = 0;
        public static Features.MsgDice DiceKing = new Features.MsgDice(true) { ID = 9999 };

        public static long DiceKingTurnOver = 0;
        public static bool IgnoreNull = false;
        public static Dictionary<int, CustomDialog> Dialogs = new Dictionary<int, CustomDialog>();
        public static int Snowballs = 1000;
        public static DateTime DemonBoxStarted;
        public static DateTime GarmentStarted;
        public static bool DemonBoxes = false;
        public static bool SafeBool = false;
        public static bool GOTWar = false;
        //  public static bool BOTSEND = false;
        /* public static ConcurrentDictionary<uint, MessageBoard> TradeBoard = new ConcurrentDictionary<uint, MessageBoard>(200);
         public static ConcurrentDictionary<uint, MessageBoard> FriendBoard = new ConcurrentDictionary<uint, MessageBoard>(200);
         public static ConcurrentDictionary<uint, MessageBoard> TeamBoard = new ConcurrentDictionary<uint, MessageBoard>(200);
         public static ConcurrentDictionary<uint, MessageBoard> GuildBoard = new ConcurrentDictionary<uint, MessageBoard>(200);
         public static ConcurrentDictionary<uint, MessageBoard> OthersBoard = new ConcurrentDictionary<uint, MessageBoard>(200);*/
        public static Dictionary<string, Game.IPLog> SpammIps = new Dictionary<string, IPLog>();
        public static Dictionary<uint, Character> DragonDamage = new Dictionary<uint, Character>();
        public static Dictionary<uint, Character> DragonHeal = new Dictionary<uint, Character>();
        public static Dictionary<int, Dictionary<uint, uint>> BossesDamage = new Dictionary<int, Dictionary<uint, uint>>();
        public static Character DragonTank;
        public static Dictionary<string, uint> Bounty = new Dictionary<string, uint>();
        public static ConcurrentDictionary<uint, Character> Archers = new ConcurrentDictionary<uint, Character>();
        public static bool PKTourny = false;
        public static bool MobsStart = false;
        //public static bool DevilKing = false;
        public static int LeftKills = 0;
        public static bool TestXP = false;
        public static uint TestXPP = 0;
        public static bool TestCmds = Main.AuthWorker.GameIP == "121.99.242.180";
        public static string GlobalAccountsPath = "";
        public static byte AccPathCount = 0;
        public static byte CharPathCount = 0;
        public static string GlobalCharactersPath = "";
        public static string BannedChars = "";
        public static string GlobalAccountsPath2Slashes = "";
        public static string GlobalCharactersPath2Slashes = "";
        public static string Blowfish = "";
        public static bool LowRatedServer = false;
        public static string InfoAdd = "";
        public static string ChatAdd = "";
        public static string GMChatAdd = "";
        public static string TradeAdd = "";
        public static string HourlyEvent = "";
        public static string DropAdd = "";
        public static string ExcAdd = "";
        public static string DebugAdd = "";
        public static string DonationAdd = "";
        public static string AntiCheatAdd = "";
        public static Dictionary<string, uint> Anticheat = new Dictionary<string, uint>();
        public static string PacketAdd = "";
        public static string Actions = "";
        public static int RightKills = 0;
        public static byte Syrens = 1;
        public static bool Exit = false;
        public static bool DisCityON = false;
        public static bool ExpEvent;
        public static int WorldEvent = 0;
        public static bool TreasureHunt = false;
        public static ushort TreasureMap = 0;
        public static bool Pluto = false;
        //public static bool SaveServer = false;
        // public static byte Connections = 8;
        public static bool Firewall = false;
        //  public static CopiedChar BackupChar;
        public static MyRandom Rnd = new MyRandom();
        public static List<string> BanChars = new List<string>();
        public static Dictionary<string, uint> ToBanIPList = new Dictionary<string, uint>();
        public static List<string> VotedIps = new List<string>();
        public static List<Dbase.Portal> Portals = new List<Dbase.Portal>();
        public static List<uint> EIDS = new List<uint>();
        public static List<Character> TopPK = new List<Character>();
        public static List<Character> TopGold = new List<Character>();
        public static List<Character> TopVPS = new List<Character>();
        public static List<Character> TopArcher = new List<Character>();
        public static List<Character> TopWarrior = new List<Character>();
        public static List<Character> TopTrojan = new List<Character>();
        public static List<Character> TopWaterTao = new List<Character>();
        public static List<Character> TopFireTao = new List<Character>();
        public static List<Character> TopOnline = new List<Character>();
        public static Dictionary<string, uint> GoldSource = new Dictionary<string, uint>();
        public static List<uint> NoPKMaps = new List<uint>() { (uint)1036, (uint)1090, (uint)1039, (uint)2068, (uint)1004, (uint)2023, (uint)1059, (uint)8004, (uint)8005, (uint)8006, (uint)2068 };
        public static List<uint> FreePKMaps = new List<uint>() { (uint)6000, (uint)6001, (uint)6003, (uint)1038, (uint)1080, (uint)1017, (uint)1005, (uint)1763, (uint)1509, (uint)2024, (uint)700, (uint)701, (uint)1844, (uint)8001 };
        public static List<uint> EventsMaps = new List<uint>() { (uint)700, (uint)701, (uint)1763, (uint)1080, (uint)1017, (uint)1844, (uint)1505, (uint)1506, (uint)1507, (uint)1508 };
        public static List<Events.Events> Events = new List<Ultimate.Events.Events>();
        public static ConcurrentDictionary<uint, Character> H_Chars = new ConcurrentDictionary<uint, Character>();
        public static Dictionary<uint, ConcurrentDictionary<uint, Character>> PlayersInMap = new Dictionary<uint, ConcurrentDictionary<uint, Character>>();
        public static Dictionary<uint, Main.GameClient> H_Clients = new Dictionary<uint, Main.GameClient>();
        public static Dictionary<uint, Character> H_LeftFlank = new Dictionary<uint, Character>();
        public static Dictionary<uint, Character> H_RightFlank = new Dictionary<uint, Character>();
        public static Dictionary<uint, Character> H_CharsDrawing = new Dictionary<uint, Character>();
        public static ConcurrentDictionary<uint, ConcurrentDictionary<uint, Mob>> H_Mobs = new ConcurrentDictionary<uint, ConcurrentDictionary<uint, Mob>>();
        public static ConcurrentDictionary<uint, SOB> H_SOBs = new ConcurrentDictionary<uint, SOB>();
        public static ConcurrentDictionary<uint, ConcurrentDictionary<uint, DroppedItem>> H_Items = new ConcurrentDictionary<uint, ConcurrentDictionary<uint, DroppedItem>>();
        public static Dictionary<uint, ConcurrentDictionary<uint, MapEffect>> H_Effects = new Dictionary<uint, ConcurrentDictionary<uint, MapEffect>>();
        public static List<uint> ActiveSquamas = new List<uint>();
        public static Dictionary<uint, Dictionary<uint, NPC>> H_NPCs = new Dictionary<uint, Dictionary<uint, NPC>>();
        public static ConcurrentDictionary<uint, Features.PersonalShops.Shop> H_PShops = new ConcurrentDictionary<uint, Features.PersonalShops.Shop>();
        public static ConcurrentDictionary<uint, Companion> H_Companions = new ConcurrentDictionary<uint, Companion>();
        public static List<uint> Furnitures = new List<uint>();
        public static KOInfo[] KOBoard = new KOInfo[500];
        public static EmpireInfo[] EmpireBoard = new EmpireInfo[50];
        public static BroadCastMessage[] BroadCasts = new BroadCastMessage[100];
        public static BroadCastMessage CurrentBC = new BroadCastMessage();
        public static byte BroadCastCount = 0;
        public static DateTime CycloneEvent = DateTime.Now;
        public static DateTime LastBroadCast = DateTime.Now;
        public static uint ScreenColor = 0;

        public static void NewKO(string Name, int KO)
        {
            try
            {
                if (KO > 0)
                {
                    int MyPlace = 500;
                    int PrevPlace = 500;
                    for (int i = 499; i >= 0; i--)
                    {
                        if (KO >= KOBoard[i].KillCount)
                        {
                            MyPlace--;
                        }
                        if (Name == KOBoard[i].Name)
                            PrevPlace = i;
                    }
                    if (MyPlace < 500 && MyPlace <= PrevPlace)
                    {
                        if (MyPlace != PrevPlace)
                        {
                            for (int i = PrevPlace; i < 499; i++)
                            {
                                KOBoard[i] = KOBoard[i + 1];
                            }
                            for (int i = 498; i >= MyPlace; i--)
                                KOBoard[i + 1] = KOBoard[i];
                        }
                        KOInfo K = new KOInfo();
                        K.Name = Name;
                        K.KillCount = KO;
                        K.KOID = (uint)Rnd.Next(10000000);
                        KOBoard[MyPlace] = K;
                        SendMsgToAll("SYSTEM", Name + " has killed " + KO + " monsters with XP skills and ranks " + (MyPlace + 1) + " on the KO board.", 2000, 0);

                        MySQL.MySqlCommand Koboard = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                        Koboard.Insert("koboard").Insert("Name", K.Name).Insert("KO", K.KillCount.ToString().Split(':')[0].ToString()).Execute();
              
                    }
                }
            }
            catch { }
        }
        public static void NewEmpire(Character C, bool load = true)
        {
            try
            {
                if (C.Nobility.Donation >= 3000000)
                {
                    int MyPlace = 50;
                    for (int i = 49; i >= 0; i--)
                    {
                        if (C.Nobility.Donation > EmpireBoard[i].Donation)
                        {
                            MyPlace--;
                        }
                        else if (C.Nobility.Donation == EmpireBoard[i].Donation && MyPlace <= 49)
                        {
                            //int c = string.Compare(C.Name, EmpireBoard[i].Name);
                            //c = C.Name.CompareTo(EmpireBoard[i].Name);
                            if (C.Name.CompareTo(EmpireBoard[i].Name) < 0)
                                MyPlace--;
                        }
                        else break;
                    }

                    if (!load)
                    {
                        string _rank = "";
                        if (MyPlace < 3 && C.Nobility.ListPlace >= 3 && C.Nobility.ListPlace < 15 && C.Nobility.Rank != Ranks.King)
                        {
                            if (C.Body == 1003 || C.Body == 1004)
                                _rank = "King";
                            else
                                _rank = "Queen";
                        }
                        else if (MyPlace >= 3 && MyPlace < 15 && C.Nobility.ListPlace >= 15 && C.Nobility.ListPlace < 50 && C.Nobility.Rank != Ranks.Prince)
                        {
                            if (C.Body == 1003 || C.Body == 1004)
                                _rank = "Prince";
                            else
                                _rank = "Princess";
                        }
                        else if (MyPlace >= 15 && MyPlace < 50 && (C.Nobility.ListPlace >= 50 || C.Nobility.ListPlace == -1) && C.Nobility.Rank != Ranks.Duke)
                        {
                            if (C.Body == 1003 || C.Body == 1004)
                                _rank = "Duke";
                            else
                                _rank = "Duchess";
                        }
                        else if (C.Nobility.Donation >= 200000000 && C.Nobility.Donation <= 300000000 && C.Nobility.Rank == Ranks.Baron)
                        {
                            if (C.Body == 1003 || C.Body == 1004)
                                _rank = "Earl";
                            else
                                _rank = "Countess";
                        }
                        else if (C.Nobility.Donation >= 100000000 && C.Nobility.Donation < 200000000 && C.Nobility.Rank == Ranks.Knight)
                        {
                            if (C.Body == 1003 || C.Body == 1004)
                                _rank = "Baron";
                            else
                                _rank = "Baroness";
                        }
                        else if (C.Nobility.Donation >= 30000000 && C.Nobility.Donation < 100000000 && C.Nobility.Rank == Ranks.Serf)
                            _rank = "Knight";
                        if (_rank != "")
                            World.SendMsgToAll("SYSTEM", "All hail the new " + _rank + ", " + C.Name + "! May he/she have a lucky life!", 2011, 0);
                    }

                    if (MyPlace < 3)
                        C.Nobility.Rank = Ranks.King;
                    else if (MyPlace >= 3 && MyPlace < 15)
                        C.Nobility.Rank = Ranks.Prince;
                    else if (MyPlace >= 15 && MyPlace < 50)
                        C.Nobility.Rank = Ranks.Duke;
                    else if (C.Nobility.Donation >= 200000000 && C.Nobility.Donation <= 300000000)
                        C.Nobility.Rank = Ranks.Earl;
                    else if (C.Nobility.Donation >= 100000000 && C.Nobility.Donation < 200000000)
                        C.Nobility.Rank = Ranks.Baron;
                    else if (C.Nobility.Donation >= 30000000 && C.Nobility.Donation < 100000000)
                        C.Nobility.Rank = Ranks.Knight;

                    if (MyPlace < 50)
                    {
                        for (int i = 0; i < 50; i++)
                            if (EmpireBoard[i].ID == C.EntityID)
                            {
                                C.Nobility.ListPlace = i;
                                break;
                            }
                        if (C.Nobility.ListPlace != MyPlace)
                        {
                            if (C.Nobility.ListPlace != -1)//if the player already exists in the top
                            {
                                // for (int i = C.Nobility.ListPlace - 1; i >= MyPlace; i--)
                                for (int i = C.Nobility.ListPlace; i <= 48; i++)
                                    EmpireBoard[i] = EmpireBoard[i + 1];//then just push everyone back who WERE before me
                            }
                            for (int i = 48; i >= MyPlace; i--)
                                EmpireBoard[i + 1] = EmpireBoard[i];
                        }
                        EmpireBoard[MyPlace].ID = C.EntityID;
                        EmpireBoard[MyPlace].Donation = C.Nobility.Donation;
                        EmpireBoard[MyPlace].Name = C.Name /*+ C.MyClient.AuthInfo.Status*/;
                        if (C.Body == 1003 || C.Body == 1004)
                            EmpireBoard[MyPlace].Gender = 0;
                        else
                            EmpireBoard[MyPlace].Gender = 1;

                        C.Nobility.ListPlace = MyPlace;
                        // C.Nobility.Rank = C.Nobility.Rank;
                    }
                }
            }
            catch { }
        }
        public static NPC NPCFromLoc(Location Loc)
        {
            try
            {
                if (H_NPCs.ContainsKey(Loc.Map))
                {
                    Dictionary<uint, NPC> MapNPC = H_NPCs[Loc.Map];
                    foreach (NPC N in MapNPC.Values)
                        if (N.Loc.X == Loc.X && N.Loc.Y == Loc.Y)
                            return N;
                }
                return null;
            }
            catch { return null; }
        }
        //public static ushort shit = 0;
        //public static ushort shit2 = 0;
        //public static ushort shit3 = 2;
        //public static uint UID;
        //public static void RunPackets(object key)
        //{
        //    try
        //    {
        //        if (shit == 74)
        //            shit++;
        //        if (shit == 104)
        //            shit++;
        //        Character CC = (Character)Game.World.H_Chars[UID];
        //        CC.MyClient.AddSend(Packets.GeneralData(1, 1, (ushort)(CC.Loc.X + 1), (ushort)(CC.Loc.Y + 1), shit, 6));
        //        CC.MyClient.LocalMessage(2000, "General Data with dir type: " + shit);
        //        shit = (ushort)(shit + 1);

        //    }
        //    catch (Exception E) { Console.WriteLine(E.ToString()); }
        //}
        //public static void RunPackets2(object key)
        //{
        //    try
        //    {
        //        if (shit2 == 74)
        //            shit2++;
        //        if (shit2 == 104)
        //            shit2++;
        //        Character CC = (Character)Game.World.H_Chars[UID];
        //        CC.MyClient.AddSend(Packets.GeneralData(CC.Loc.Map, CC.Loc.Map, (ushort)(CC.Loc.X + 1), (ushort)(CC.Loc.Y + 1), shit2));
        //        CC.MyClient.LocalMessage(2000, "General Data type: " + shit2);
        //        shit2 = (ushort)(shit2 + 1);

        //    }
        //    catch (Exception E) { Console.WriteLine(E.ToString()); }
        //}
        //public static void RunPackets3(object key)
        //{
        //    try
        //    {
        //        //if (shit3 >= 0)
        //        //    shit3++;

        //        Character CC = (Character)Game.World.H_Chars[UID];
        //        CC.MyClient.AddSend(Packets.GeneralData(CC.EntityID, shit3, (ushort)(CC.Loc.X + 1), (ushort)(CC.Loc.Y + 1), 126));
        //        CC.MyClient.LocalMessage(2000, "General Data type: " + shit3);

        //    }
        //    catch (Exception E) { Console.WriteLine(E.ToString()); }
        //}
        public static void SendMsgToAll(string Name, string Message, ushort ChatType, uint Mesh, uint Map = 0)
        {
            try
            {
                //foreach (KeyValuePair<uint, Character> DE in H_Chars)
                if (Map == 0)
                    foreach (Character C in H_Chars.Values)
                    {
                        C.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, Name, "ALL", Message, ChatType, Mesh));
                    }
                else
                    foreach (Character C in H_Chars.Values)
                    {
                        if (C.Loc.Map == Map)
                            C.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, Name, "ALL", Message, ChatType, Mesh));
                    }
            }
            catch (Exception E) { ExcAdd += E.ToString() + "\r\n"; }
        }
        public static Character CharacterFromName(string Name)
        {
            foreach (Character C in H_Chars.Values)
                if ((C.Name + C.MyClient.AuthInfo.Status).ToLower() == Name.ToLower())
                    return C;

            return null;
        }
        public static Character CharacterFromName2(string Name)
        {
            foreach (Character C in H_Chars.Values)
                if (C.Name.ToLower() == Name.ToLower())
                    return C;
            return null;
        }
        public static void Chat(Character C, ushort Type, string From, string To, string Message)
        {
            if (Message.Contains("playconquer.se") || Message.Contains("immortals-co.com") || Message.Contains("unitedgenerals.com") || Message.Contains("monksoul.com") || Message.Contains("ourconquer.com") || Message.Contains("darkmagik-co.com") || Message.Contains("dragon-conquer.com") || Message.Contains("kingdom-co.com") || Message.Contains("classic-conquer-veterans.com") || Message.Contains("classic-conquer.com") || Message.Contains("hellconquer.com") || Message.Contains("undergroundco.net") || Message.Contains("project-throwback.com") || Message.Contains("evocatusco.com") || Message.Contains("eraconquer.cf") || Message.Contains("conquerheroes.com") || Message.Contains("eternityco-online.com") || Message.Contains("classicconqueronline.com") || Message.Contains("eraconquer.cf"))
                Message = "*****";

            if (World.HodorEvent)
                Message = "HODOR";
            if (Type == 2021)
            {
                string test = Message.ToLower();
                if ((test.Contains("baligya") || test.Contains("palit") || test.Contains("paligya") || test.Contains("bligya")) && (test.Contains("php") || test.Contains("illigan")))
                {
                    //C.BOTJailed = true;
                    C.BOTJailedDays = 2;
                    C.Teleport(6003, 30, 72);
                    C.MyClient.LocalMessage(2011, "You are now botjailed for " + C.BOTJailedDays + " day for trying to buy/sell items for real money!");
                    World.SendMsgToAll("SYSTEM", C.Name + " was botjailed for trying to buy/sell items for real money!", 2000, 0);
                    Program.WriteCmds(C.Name + " was botjailed " + C.Name + " for " + C.BOTJailedDays + " at: " + DateTime.Now + " for trying to sell items for real money!");
                }
            }
            try
            {
                if (Type == 2000 || Type == 2104 || Type == 2013)
                {
                    foreach (Character CC in C.ScreenChars.Values)
                    {
                        if (C.EntityID != CC.EntityID)
                            if (!CC.IPMuted.Contains(C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()))
                                CC.MyClient.AddSend(Packets.ChatMessage(CC.MyClient.MessageID, From, To, Message, Type, C.Mesh));
                    }
                    if (C.MyClient.GM)
                        Game.World.GMChatAdd += "Talk: " + C.Name + " : " + Message + "\r\n";
                    Game.World.ChatAdd += "Talk: " + C.Name + " : " + Message + "\r\n";
                }
                else if (Type == 2021)//world chat
                {
                    if (!C.Muted)
                    {
                        if (DateTime.Now > C.LastWorldMsg.AddMinutes(1) || C.MyClient.PM)
                        {
                            foreach (Character CC in World.H_Chars.Values)
                                if (CC != null)
                                    if (C.EntityID != CC.EntityID)
                                        if (!CC.IPMuted.Contains(C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()))
                                            CC.MyClient.AddSend(Packets.ChatMessage(CC.MyClient.MessageID, From, To, Message, Type, C.Mesh));
                            C.LastWorldMsg = DateTime.Now;
                            if (C.MyClient.GM)
                                Game.World.GMChatAdd += "World: " + C.Name + " : " + Message + "\r\n";
                            Game.World.ChatAdd += "World: " + C.Name + " : " + Message + "\r\n";
                        }
                        else C.MyClient.LocalMessage(2005, "You have to wait 1 minute before posting another world chat message");
                    }
                    else C.MyClient.LocalMessage(2005, "You are muted you can't use chat until you get unmuted");

                }
                else if (Type == 2001) // Whisper
                {
                    Character C2 = CharacterFromName(To);
                    if (C2 != null)
                    {
                        string guildName = C.MyGuild != null ? C.MyGuild.GuildName : "None";
                        string mystring = $"{C.EntityID}~{C.Level}~{C.Potency}~#{guildName}~#~{C.Spouse}~0~";
                        if (C.Body % 10 < 3)
                            mystring += $"1";
                        else
                            mystring += $"0";
                        mystring = $"{C.Name}{C.MyClient.AuthInfo.Status} {mystring}";

                        C.MyClient.AddSend(Packets.StringPacket(C2.MyClient.MessageID, StringType.WhisperWindowInfo, mystring, true));

                        if (!C2.IPMuted.Contains(C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString()))
                        {
                            C2.MyClient.AddSend(Packets.ChatMessage(C2.MyClient.MessageID, From, To, Message, Type, C.Mesh));
                            C2.MyClient.AddSend(Packets.StringPacket(C2.MyClient.MessageID, StringType.WhisperWindowInfo, mystring, true));
                        }
                        //C.MyClient.AddSend(Packets.ChatMessage(C2.MyClient.MessageID, From, To, Message, Type, C.Mesh));
                        if (C.MyClient.GM)
                            Game.World.GMChatAdd += "Whisper: " + C.Name + " to " + C2.Name + " : " + Message + "\r\n";
                        Game.World.ChatAdd += "Whisper: " + C.Name + " to " + C2.Name + " : " + Message + "\r\n";
                        //  C.MyClient.AddSend(Packets.SpawnViewed(C2, 2));


                    }
                    else
                    {
                        C.MyClient.LocalMessage(2000, "Character " + To + " is not online or doesn't exist.");
                    }
                }
                else if (Type == 2009)//Friend
                {
                    foreach (Friend F in C.Friends.Values)
                        if (F.Online)
                        {
                            F.Info.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0x7d9));
                        }
                    if (C.MyClient.GM)
                        Game.World.GMChatAdd += "Friend: " + C.Name + " : " + Message + "\r\n";
                    Game.World.ChatAdd += "Friend: " + C.Name + " : " + Message + "\r\n";
                }
                else if (Type == 2004)//Guild
                {
                    if (C.MyGuild != null)
                    {
                        if (C.MyGuild.Fund >= 50000)
                        {
                            if (Message.Substring(0, 1) == ".")
                            {
                                foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                                {
                                    if (C.MyGuild.Allies.ContainsKey(G.GuildID))
                                    {
                                        string _message = Message.Substring(1);
                                        G.GuildMsg(Packets.ChatMessage(C.MyClient.MessageID, From, To, "[" + C.MyGuild.GuildName + "] " + _message, Type, 0), C.EntityID);
                                    }
                                }
                            }

                            C.MyGuild.GuildMsg(Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0), C.EntityID);
                            if (C.MyClient.GM)
                                Game.World.GMChatAdd += "Guild: " + C.MyGuild.GuildName + " : " + C.Name + " : " + Message + "\r\n";
                            Game.World.ChatAdd += "Guild: " + C.MyGuild.GuildName + " : " + C.Name + " : " + Message + "\r\n";

                            //foreach (Character Ally in World.H_Chars.Values)
                            //    if (Ally.MyGuild != null)
                            //        if (C.MyGuild.Allies.ContainsKey(Ally.MyGuild.GuildID))
                            //            if (!Ally.LogOff)
                            //                if (Message.Substring(0, 1) == ".")
                            //                {
                            //                    string _message = Message.Substring(1);
                            //                    Ally.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, From, To, "[" + C.MyGuild.GuildName + "] " + _message, Type, C.Mesh));
                            //                }

                        }
                        else
                            C.MyClient.LocalMessage(2004, "Your Guild Fund is too low. You have to donate some gold to use the guild chat!");
                    }
                }
                else if (Type == 2003) // Team
                {
                    if (C.MyTeam != null)
                    {
                        C.MyTeam.Message(C, Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0x7d3));
                        if (C.MyClient.GM)
                            Game.World.GMChatAdd += "Team: " + C.Name + " : " + Message + "\r\n";
                        Game.World.ChatAdd += "Team: " + C.Name + " : " + Message + "\r\n";
                    }

                }
                else if (Type == 2111)//Guild
                {
                    if (C.MyGuild != null && C.GuildRank == Features.GuildRank.GuildLeader)
                    {
                        C.MyGuild.NewBulletin(Message);
                        if (C.MyClient.GM)
                            Game.World.GMChatAdd += "Guild bulletin: " + C.MyGuild.GuildName + " : " + C.Name + " : " + Message + "\r\n";
                        Game.World.ChatAdd += "Guild bulletin: " + C.MyGuild.GuildName + " : " + C.Name + " : " + Message + "\r\n";
                    }
                }
                /* else if (Type == 2201)
                 {
                     if (TradeBoard.ContainsKey(C.EntityID))
                     {
                         TradeBoard.Remove(C.EntityID);
                     }
                     TradeBoard.Add(C.EntityID, new MessageBoard() { Msg = Message, Time = DateTime.Now, Name = C.Name });
                 }*/
                /* else if (Type == 2201)//trade board
                 {
                     NewTradeBoardMessage(Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0), Message);
                 } 
                 else if (Type == 2202)//friend board
                 {
                     foreach (Friend F in C.Friends.Values)
                         if (F.Online)
                         {
                             F.Info.MyClient.AddSend(Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0));
                         }
                 }
                 else if (Type == 2203)//team board
                 {
                     if (C.MyTeam != null)
                         C.MyTeam.Message(C, Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0));
                 }
                 else if (Type == 2204)//guild board
                 {
                     if (C.MyGuild != null)
                         C.MyGuild.GuildMsg(Packets.ChatMessage(C.MyClient.MessageID, From, To, Message, Type, 0), C.EntityID);
                 }
                 else if (Type == 2205)//others board
                 {
                     foreach (Character CC in H_Chars.Values)
                         if (C != CC)
                             CC.MyClient.AddSend(Packets.ChatMessage(CC.MyClient.MessageID, From, To, Message, Type, 0));
                 }*/
                else
                {
                    if (C.MyClient.GM)
                        C.MyClient.LocalMessage(2001, "Unknown chat type: " + Type);
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public static void Spawn(Companion M, bool Check)
        {
            try
            {
                COPacket P = Packets.SpawnEntity(M);
                // ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[M.Loc.Map];
                //foreach(Character CC in Map.Values)
                foreach (Character CC in H_Chars.Values)
                    if (CC.Loc.Map == M.Loc.Map && MyMath.InBox(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y, 18) && (!MyMath.InBox(M.Loc.PreviousX, M.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, 18) || !Check))
                        // if (CC.Loc.Map == M.Loc.Map && MyMath.InBox(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y, CC.Range()) && (!MyMath.InBox(M.Loc.PreviousX, M.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, CC.Range()) || !Check))
                        // if (CC.Loc.Map == M.Loc.Map && MyMath.PointDistance(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && (MyMath.PointDistance(M.Loc.PreviousX, M.Loc.PreviousY, CC.Loc.X, CC.Loc.Y) > 18 || !Check))
                        CC.MyClient.AddSend(P);
            }
            catch { }
        }
        public static void Spawn(Mob M, bool Check)
        {
            try
            {
                COPacket P = Packets.SpawnEntity(M);
                foreach (Character CC in World.PlayersInMap[M.Loc.Map].Values)
                    //foreach (Character CC in H_Chars.Values)
                    if (/*CC.Loc.Map == M.Loc.Map &&*/ MyMath.InBox(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y, 18) && (!MyMath.InBox(M.Loc.PreviousX, M.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, 18) || !Check))
                        //if (CC.Loc.Map == M.Loc.Map && MyMath.PointDistance(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && (MyMath.PointDistance(M.Loc.PreviousX, M.Loc.PreviousY, CC.Loc.X, CC.Loc.Y) > 18 || !Check))
                        CC.MyClient.AddSend(P);
            }
            catch { }
        }
        public static void Spawn(NPC M)
        {
            try
            {
                COPacket P;
                if (M.CurHP == 0)
                    P = Packets.SpawnNPC(M);
                else if (M.PlayerEvent)
                    P = Packets.SpawnNPCWithHP(M.EntityID, M.Type, M.Flags, M.Loc, true, M.Name, M.CurHP, M.MaxHP);
                else
                    P = Packets.SpawnNPCWithHP(M);
                // ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[M.Loc.Map];
                //foreach (Character CC in Map.Values)
                foreach (Character CC in H_Chars.Values)
                    if (CC.Loc.Map == M.Loc.Map && MyMath.InBox(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    // if (CC.Loc.Map == M.Loc.Map && MyMath.PointDistance(M.Loc.X, M.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                    {
                        CC.MyClient.AddSend(P);
                    }
            }
            catch { }
        }
        public static void Spawn(Features.PersonalShops.Shop M)
        {
            try
            {
                COPacket P = Packets.SpawnNamedNPC(M.NPCInfo, M.Name);
                COPacket P2 = Packets.ChatMessage(26514, M.Owner.Name, "ALL", M.Hawk, 2104, 0);
                // ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[M.NPCInfo.Loc.Map];
                //   foreach (Character CC in Map.Values)
                foreach (Character CC in H_Chars.Values)
                    if (CC != M.Owner && CC.Loc.Map == M.NPCInfo.Loc.Map && MyMath.InBox(M.NPCInfo.Loc.X, M.NPCInfo.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    // if (CC != M.Owner && CC.Loc.Map == M.NPCInfo.Loc.Map && MyMath.PointDistance(M.NPCInfo.Loc.X, M.NPCInfo.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                    {
                        CC.MyClient.AddSend(P);
                        if (M.Hawk != null)
                            if (M.Hawk != "")
                                CC.MyClient.AddSend(P2);
                    }
            }
            catch { }
        }
        public static void Spawn(Character C, bool Check)
        {
            try
            {
                //  COPacket P = Packets.SpawnEntity(C);
                // Console.WriteLine("Spawn!");
                if (!C.Invisible)
                    foreach (Character CC in C.ScreenChars.Values)//H_Chars.Values
                    // if (CC.Loc.Map == C.Loc.Map)
                    {
                        //

                        //if (MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, 28) || !Check))
                        //{
                        // CC.MyClient.AddSend(Packets.SpawnEntity(C));// P
                        // if (C.MyGuild != null)
                        // CC.MyClient.AddSend(Packets.StringPacket(C.MyGuild.GuildID, StringType.GuildName, C.MyGuild.GuildName));
                        //}


                        //to test
                        //  if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y) > 18 || !C.ScreenChars.ContainsKey(CC.EntityID)))
                        //  {
                        //     if (!CC.ScreenChars.ContainsKey(C.EntityID))//not
                        //      CC.ScreenChars.Add(C.EntityID, C);//not

                        if (C.MyGuild != null)
                            CC.MyClient.AddSend(Packets.StringPacket(C.MyGuild.GuildID, StringType.GuildName, C.MyGuild.GuildName));

                        CC.MyClient.AddSend(Packets.SpawnEntity(C));// P


                        //   }
                        /*      else if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) > 18)
                         * 
                              {
                                  if (C.ScreenChars.ContainsKey(CC.EntityID))
                                  {
                                      C.ScreenChars.Remove(CC.EntityID);
                                      C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                                      if (CC.ScreenChars.ContainsKey(C.EntityID))
                                      {
                                          CC.ScreenChars.Remove(C.EntityID);
                                          CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                      }
                                  }
                                  else if (CC.ScreenChars.ContainsKey(C.EntityID))
                                  {
                                      CC.ScreenChars.Remove(C.EntityID);
                                      CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                  }
                              }*/
                    }
                /*    else
                    {
                        if (C.ScreenChars.ContainsKey(CC.EntityID))
                        {
                            C.ScreenChars.Remove(CC.EntityID);
                            C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                            if (CC.ScreenChars.ContainsKey(C.EntityID))
                            {
                                CC.ScreenChars.Remove(C.EntityID);
                                CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                            }
                        }
                        else if (CC.ScreenChars.ContainsKey(C.EntityID))
                        {
                            CC.ScreenChars.Remove(C.EntityID);
                            CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                        }
                    } */
            }
            catch { }
        }
        /*public static void Spawn(DroppedItem C, byte[] Data)
        {
            try
            {
                COPacket P = new COPacket(Data);
                foreach (Character CC in H_Chars.Values)
                {
                    // if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 28))
                    if (CC.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && !CC.ScreenItems.ContainsKey(C.UID))
                    {
                        CC.MyClient.AddSend(P);
                        CC.ScreenItems.Add(C.UID, C);
                    }
                }
            }
            catch { }
        }*/
        public static void Spawns(Character C, bool Check, bool GetPlayers = true)
        {
            try
            {
                /*    foreach (Character CC in H_Chars.Values)
                    {
                        if (C.EntityID != CC.EntityID)
                            if (CC.Loc.Map == C.Loc.Map)
                            {

                                if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && !C.ScreenChars.Contains(CC.EntityID))
                                //if (MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, 28) /*|| !Check*/
                // || !C.ScreenChars.Contains(CC.EntityID)))
                /*      {
                          C.ScreenChars.Add(CC.EntityID);
                           if (!CC.ScreenChars.Contains(C.EntityID))
                           {
                               CC.ScreenChars.Add(C.EntityID);
                               CC.MyClient.AddSend(Packets.SpawnEntity(C));
                               if (C.MyGuild!= null)
                                   CC.MyClient.AddSend(Packets.StringPacket(C.MyGuild.GuildID, StringType.GuildName, C.MyGuild.GuildName));
                           }

                       }
                       else if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) > 18 && C.ScreenChars.Contains(CC.EntityID))
                       {
                           C.ScreenChars.Remove(CC.EntityID);
                           C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                           if (CC.ScreenChars.Contains(C.EntityID))
                           {
                               CC.ScreenChars.Remove(C.EntityID);
                               CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                           }
                       }
                   }
                   else if (C.ScreenChars.Contains(CC.EntityID))
                   {
                       C.ScreenChars.Remove(CC.EntityID);
                       C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                       if (CC.ScreenChars.Contains(C.EntityID))
                       {
                           CC.ScreenChars.Remove(C.EntityID);
                           CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                       }
                   }*/
                #region KeepSafe
                if (GetPlayers)
                {
                    //ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[C.Loc.Map];
                    /*  foreach (Character CC in Map.Values)*/
                    foreach (Character CC in H_Chars.Values)
                    {
                        if (CC != null)
                        {
                            if (CC.Loc.Map == C.Loc.Map)
                            {

                                // if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                                // if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y) > 18 || !C.ScreenChars.ContainsKey(CC.EntityID)))// || !Check InBox
                                if (MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y, 18) || !C.ScreenChars.ContainsKey(CC.EntityID)))// || !Check InBox
                                                                                                                                                                                                                   // if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, CC.Loc.X, CC.Loc.Y) > 18 || !C.ScreenChars.ContainsKey(CC.EntityID)))
                                {
                                    if (!CC.Invisible)
                                    {
                                        if (CC.MyGuild != null)
                                            C.MyClient.AddSend(Packets.StringPacket(CC.MyGuild.GuildID, StringType.GuildName, CC.MyGuild.GuildName));
                                        C.MyClient.AddSend(Packets.SpawnEntity(CC));
                                    }
                                    if (!C.Invisible)
                                    {
                                        if (C.MyGuild != null)
                                            CC.MyClient.AddSend(Packets.StringPacket(C.MyGuild.GuildID, StringType.GuildName, C.MyGuild.GuildName));
                                        CC.MyClient.AddSend(Packets.SpawnEntity(C));
                                    }

                                    if (!C.ScreenChars.ContainsKey(CC.EntityID))
                                    {
                                        C.ScreenChars.TryAdd(CC.EntityID, CC);

                                        if (!CC.ScreenChars.ContainsKey(C.EntityID))
                                        {
                                            CC.ScreenChars.TryAdd(C.EntityID, C);
                                        }
                                    }
                                    else if (!CC.ScreenChars.ContainsKey(C.EntityID))
                                    {
                                        CC.ScreenChars.TryAdd(C.EntityID, C);
                                    }

                                }
                                //else if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) > 18)
                                else if (!MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                                {
                                    if (C.ScreenChars.ContainsKey(CC.EntityID))
                                    {
                                        C.ScreenChars.Remove(CC.EntityID);
                                        // C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, CC.Loc.Map,CC.Loc.X,CC.Loc.Y, 135).Get);
                                        C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                                        if (CC.ScreenChars.ContainsKey(C.EntityID))
                                        {
                                            CC.ScreenChars.Remove(C.EntityID);
                                            //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map,C.Loc.X,C.Loc.Y, 135).Get);
                                            CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                        }
                                    }
                                    else if (CC.ScreenChars.ContainsKey(C.EntityID))
                                    {
                                        CC.ScreenChars.Remove(C.EntityID);
                                        //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID,C.Loc.Map,C.Loc.X,C.Loc.Y, 135).Get);
                                        CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                    }
                                }
                            }
                            else
                            {
                                if (C.ScreenChars.ContainsKey(CC.EntityID))
                                {
                                    C.ScreenChars.Remove(CC.EntityID);
                                    // C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, CC.Loc.Map, CC.Loc.X,CC.Loc.Y, 135).Get);
                                    C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                                    if (CC.ScreenChars.ContainsKey(C.EntityID))
                                    {
                                        CC.ScreenChars.Remove(C.EntityID);
                                        //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map,C.Loc.X,C.Loc.Y, 135).Get);
                                        CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                    }
                                }
                                else if (CC.ScreenChars.ContainsKey(C.EntityID))
                                {
                                    CC.ScreenChars.Remove(C.EntityID);
                                    //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                    CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                }
                            }
                        }
                    }
                    /* List<Character> Chrs = new List<Character>();
                     foreach (Character CC in C.ScreenChars.Values)
                         if (CC.Loc.Map != C.Loc.Map)
                             if (C.ScreenChars.ContainsKey(CC.EntityID))
                             {
                                 // C.ScreenChars.Remove(CC.EntityID);
                                 Chrs.Add(CC.EntityID);
                                 // C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, CC.Loc.Map, CC.Loc.X,CC.Loc.Y, 135).Get);
                                 C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                                 if (CC.ScreenChars.ContainsKey(C.EntityID))
                                 {
                                     CC.ScreenChars.Remove(C.EntityID);
                                     //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map,C.Loc.X,C.Loc.Y, 135).Get);
                                     CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                 }
                             }
                             else if (CC.ScreenChars.ContainsKey(C.EntityID))
                             {
                                 CC.ScreenChars.Remove(C.EntityID);
                                 //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                 CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                             }
                     foreach (uint Key in Chrs)
                         C.ScreenChars.Remove(Key);*/
                }
                #endregion

                // }
                /*    foreach (uint CCID in C.ScreenChars)
                    {
                        Character CC = World.H_Chars[CCID];
                        C.MyClient.AddSend(Packets.SpawnEntity(CC));
                        if (CC.MyGuild != null)
                            C.MyClient.AddSend(Packets.StringPacket(CC.MyGuild.GuildID, StringType.GuildName, CC.MyGuild.GuildName));
                    }*/
                if (H_Mobs.ContainsKey(C.Loc.Map))
                {
                    {
                        foreach (Mob M in H_Mobs[C.Loc.Map].Values)
                            if (M != null)
                                if (M.Alive && MyMath.InBox(C.Loc.X, C.Loc.Y, M.Loc.X, M.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, M.Loc.X, M.Loc.Y, 18) || !Check))
                                    C.MyClient.AddSend(Packets.SpawnEntity(M));
                        //Mob[] Mobs = null;
                        //if (MapMobs.Count > 0)
                        //{
                        //    Mobs = new Mob[MapMobs.Count + 1];
                        //    MapMobs.Values.CopyTo(Mobs, 0);
                        //}
                        //if (MapMobs.Count > 0)
                        //{
                        //    foreach (Mob M in Mobs)
                        //        if (M != null)
                        //        if (M.Alive && MyMath.InBox(C.Loc.X, C.Loc.Y, M.Loc.X, M.Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, M.Loc.X, M.Loc.Y, 28) || !Check))
                        //            C.MyClient.AddSend(Packets.SpawnEntity(M));

                        //}
                        //lock (MapMobs)
                        //{
                        //    foreach (Mob M in MapMobs.Values)
                        //        if (M.Alive && MyMath.InBox(C.Loc.X, C.Loc.Y, M.Loc.X, M.Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, M.Loc.X, M.Loc.Y, 28) || !Check))
                        //            //if (M.Alive && MyMath.PointDistance(C.Loc.X, C.Loc.Y, M.Loc.X, M.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, M.Loc.X, M.Loc.Y) > 18 || !Check))
                        //            C.MyClient.AddSend(Packets.SpawnEntity(M));
                        //}
                        //MapMobs.Clear();
                    }
                }

                if (H_NPCs.ContainsKey(C.Loc.Map))
                {
                    {
                        Dictionary<uint, NPC> MapNPC = H_NPCs[C.Loc.Map];
                        //NPC[] NPCs = null;
                        //if (MapNPC.Count > 0)
                        //{
                        //    NPCs = new NPC[MapNPC.Count + 1];
                        //    MapNPC.Values.CopyTo(NPCs, 0);
                        //}
                        if (MapNPC.Count > 0)
                        {
                            foreach (NPC N in MapNPC.Values)
                                if (N != null)
                                    if (MyMath.InBox(C.Loc.X, C.Loc.Y, N.Loc.X, N.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, N.Loc.X, N.Loc.Y, 18) || !Check))
                                    //  if (N.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, N.Loc.X, N.Loc.Y) <= 18&& (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, N.Loc.X, N.Loc.Y) > 18 || !Check))
                                    {
                                        if (N.MaxHP == 0)
                                            C.MyClient.AddSend(Packets.SpawnNPC(N));
                                        else if (N.PlayerEvent)
                                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(N.EntityID, N.Type, N.Flags, N.Loc, true, N.Name, N.CurHP, N.MaxHP));
                                        else
                                            C.MyClient.AddSend(Packets.SpawnNPCWithHP(N));
                                    }
                        }
                    }
                }

                foreach (Features.PersonalShops.Shop S in H_PShops.Values)
                    // if (S.NPCInfo.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, S.NPCInfo.Loc.X, S.NPCInfo.Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, S.NPCInfo.Loc.X, S.NPCInfo.Loc.Y, 28) || !Check))
                    if (S.NPCInfo.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, S.NPCInfo.Loc.X, S.NPCInfo.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, S.NPCInfo.Loc.X, S.NPCInfo.Loc.Y) > 18 || !Check))
                    {
                        C.MyClient.AddSend(Packets.SpawnNamedNPC(S.NPCInfo, S.Name));
                        if (S.Hawk != null)
                            if (S.Hawk != "")
                                C.MyClient.AddSend(Packets.ChatMessage(26514, S.Owner.Name, "ALL", S.Hawk, 2104, 0));
                    }
                foreach (Companion Cmp in H_Companions.Values)
                    //    if (Cmp.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Cmp.Loc.X, Cmp.Loc.Y, C.Range()) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Cmp.Loc.X, Cmp.Loc.Y, C.Range()) || !Check) && C.Alive)
                    if (Cmp.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Cmp.Loc.X, Cmp.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Cmp.Loc.X, Cmp.Loc.Y, 18) || !Check) && C.Alive)
                        C.MyClient.AddSend(Packets.SpawnEntity(Cmp));
                if (H_Items.ContainsKey(C.Loc.Map))
                {
                    ConcurrentDictionary<uint, DroppedItem> MapItems = (ConcurrentDictionary<uint, DroppedItem>)H_Items[C.Loc.Map];
                    // lock (MapItems.SyncRoot)
                    //if (System.Threading.Monitor.TryEnter(((Hashtable)H_Items[C.Loc.Map]).SyncRoot, 1))
                    {
                        // Hashtable MapItems = Hashtable.Synchronized((Hashtable)H_Items[C.Loc.Map]);
                        // Hashtable MapItems = (Hashtable)H_Items[C.Loc.Map];
                        //Console.WriteLine("Mapitems sync: " + MapItems.IsSynchronized);

                        foreach (DroppedItem DI in MapItems.Values)
                        {
                            if (MyMath.InBox(C.Loc.X, C.Loc.Y, DI.Loc.X, DI.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, DI.Loc.X, DI.Loc.Y, 18) || !Check))
                            //  if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, DI.Loc.X, DI.Loc.Y) <= 18 && (MyMath.PointDistance(C.Loc.PreviousX, C.Loc.PreviousY, DI.Loc.X, DI.Loc.Y) > 18 || !Check))//||!Check
                            {
                                C.MyClient.AddSend(Packets.ItemDrop(DI));
                                /* if (!C.ScreenItems.ContainsKey(DI.UID))
                                     C.ScreenItems.Add(DI.UID, DI);*/
                            }
                            /*  else if (MyMath.PointDistance(C.Loc.X, C.Loc.Y, DI.Loc.X, DI.Loc.Y) > 18 && C.ScreenItems.ContainsKey(DI.UID))
                              {
                                  C.MyClient.AddSend(Packets.ItemDropRemove(DI.UID, DI.Info.ID, DI.Loc.X, DI.Loc.Y).Get);
                                  C.ScreenItems.Remove(DI.UID);
                              }*/
                        }
                    }
                }
                if (H_Effects.ContainsKey(C.Loc.Map))
                {
                    ConcurrentDictionary<uint, MapEffect> MapEffects = (ConcurrentDictionary<uint, MapEffect>)H_Effects[C.Loc.Map];
                    {

                        foreach (MapEffect DI in MapEffects.Values)
                        {
                            if (MyMath.InBox(C.Loc.X, C.Loc.Y, DI.Loc.X, DI.Loc.Y, 18) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, DI.Loc.X, DI.Loc.Y, 18) || !Check))
                            {
                                C.MyClient.AddSend(Packets.MapEffect(DI));
                            }
                        }
                    }
                }

                foreach (SOB S in H_SOBs.Values)
                    if (S.Loc.Map == C.Loc.Map)
                        S.Spawn(C, Check);

                //Features.GuildWars.ThePole.Spawn(C, Check);
                //Features.GuildWars.TheLeftGate.Spawn(C, Check);
                //Features.GuildWars.TheRightGate.Spawn(C, Check);

                //if (Features.CounterClock.War)
                //{
                //    Features.CounterClock.ThePole.Spawn(C, Check);
                //    Features.CounterClock.RG19.Spawn(C, Check);
                //    Features.CounterClock.RG18.Spawn(C, Check);
                //    Features.CounterClock.RG17.Spawn(C, Check);
                //    Features.CounterClock.RG16.Spawn(C, Check);
                //    Features.CounterClock.RG15.Spawn(C, Check);
                //    Features.CounterClock.RG14.Spawn(C, Check);
                //    Features.CounterClock.RG13.Spawn(C, Check);
                //    Features.CounterClock.RG12.Spawn(C, Check);
                //    Features.CounterClock.RG11.Spawn(C, Check);
                //    Features.CounterClock.RG10.Spawn(C, Check);
                //    Features.CounterClock.RG9.Spawn(C, Check);
                //    Features.CounterClock.RG8.Spawn(C, Check);
                //    Features.CounterClock.RG7.Spawn(C, Check);
                //    Features.CounterClock.RG6.Spawn(C, Check);
                //    Features.CounterClock.RG5.Spawn(C, Check);
                //    Features.CounterClock.RG4.Spawn(C, Check);
                //    Features.CounterClock.RG3.Spawn(C, Check);
                //    Features.CounterClock.RG2.Spawn(C, Check);
                //    Features.CounterClock.RG1.Spawn(C, Check);
                //    Features.CounterClock.LG6.Spawn(C, Check);
                //    Features.CounterClock.LG5.Spawn(C, Check);
                //    Features.CounterClock.LG4.Spawn(C, Check);
                //    Features.CounterClock.LG3.Spawn(C, Check);
                //    Features.CounterClock.LG2.Spawn(C, Check);
                //    Features.CounterClock.LG1.Spawn(C, Check);
                //}

            }
            catch (Exception E) { ExcAdd += E.ToString() + "\r\n"; Console.WriteLine(E); }
        }
        public static void Action(NPC C, byte[] Data)
        {
            try
            {
                // ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[C.Loc.Map];
                //foreach (Character CC in Map.Values)
                //ThreadSafeList<uint> Map = (ThreadSafeList<uint>)World.PlayersInMap[C.Loc.Map];
                // foreach (uint k in Map.Keys)
                foreach (Character CC in H_Chars.Values)
                {
                    //Character CC = World.H_Chars[k];
                    // if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, CC.Range()))
                    if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    {
                        CC.MyClient.AddSend(Data);
                    }
                }
            }
            catch { }
        }
        public static void Action(SOB C, byte[] Data)
        {
            try
            {
                foreach (Character CC in H_Chars.Values)
                {
                    //if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, CC.Range()))
                    if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                        CC.MyClient.AddSend(Data);

                }
            }
            catch { }
        }
        public static void Action(Companion C, byte[] Data)
        {
            try
            {
                //ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[C.Loc.Map];
                //foreach (Character CC in Map.Values)
                foreach (Character CC in H_Chars.Values)
                    // if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, CC.Range()))
                    if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                        CC.MyClient.AddSend(Data);
            }
            catch { }
        }
        public static void Action(Character C, byte[] Data)
        {
            try
            {
                if (!C.Invisible)
                    foreach (Character CC in C.ScreenChars.Values)//H_Chars.Values
                    {
                        // if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 28))
                        // if (CC.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                        //  {

                        CC.MyClient.AddSend(Data);

                        //}


                    }
            }
            catch { }
        }
        //public static void Action(AI C, byte[] Data)
        //{
        //    try
        //    {
        //        foreach (Character CC in C.ScreenChars.Values)//H_Chars.Values
        //            CC.MyClient.AddSend(Data);
        //    }
        //    catch { }
        //}
        public static void Action(Mob C, byte[] Data)
        {
            try
            {
                COPacket P = new COPacket(Data);
                foreach (Character CC in World.PlayersInMap[C.Loc.Map].Values)
                // foreach (Character CC in H_Chars.Values)
                {
                    //Character CC = World.H_Chars[k];
                    if (/*CC.Loc.Map == C.Loc.Map && */MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    // if (CC.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                    {
                        CC.MyClient.AddSend(P);
                    }
                }
            }
            catch { }
        }
        public static void Action(DroppedItem C, byte[] Data)
        {
            try
            {
                COPacket P = new COPacket(Data);
                //  ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[C.Loc.Map];
                // foreach (Character CC in Map.Values)
                foreach (Character CC in H_Chars.Values)
                {
                    if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    // if (CC.Loc.Map == C.Loc.Map && MyMath.PointDistance(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y) <= 18)
                    {
                        CC.MyClient.AddSend(P);
                    }
                }
            }
            catch { }
        }
        public static void Action(MapEffect C, byte[] Data)
        {
            try
            {
                COPacket P = new COPacket(Data);
                foreach (Character CC in H_Chars.Values)
                {
                    if (CC.Loc.Map == C.Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, CC.Loc.X, CC.Loc.Y, 18))
                    {
                        CC.MyClient.AddSend(P);
                    }
                }
            }
            catch { }
        }
        public static void Action(byte[] Data)
        {
            try
            {
                COPacket P = new COPacket(Data);
                foreach (Character CC in H_Chars.Values)
                    CC.MyClient.AddSend(P);
            }
            catch { }
        }
    }
}
