using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;
//using TinyMap;

namespace Ultimate
{
    public class DMaps
    {
        //public static List<ushort> MapsNeeded = new List<ushort>() { 601, 700, 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016, 1018, 1020, 1021, 1022, 1025, 1026, 1027, 1028, 1036, 1037, 1038, 1039, 1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1079, 1082, 1060, 1061, 1062, 1063, 1068, 1070, 1075, 1076, 1077, 1078, 1018, 1018, 1090, 1091, 1090, 1091, 1090, 1091, 1100, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1018, 1090, 1091, 1018, 1090, 1091, 1018, 1090, 1091, 1201, 1202, 1204, 1205, 1207, 1208, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1075, 1025, 1212, 1001, 1351, 1352, 1353, 1354, 1360, 1364, 1017, 1505, 1506, 1507, 1508, 1509, 1004, 1505, 1506, 1507, 1508, 1509, 1505, 1505, 1505, 1509, 1505, 1506, 1507, 1508, 1509, 1091, 1091, 1091, 1091, 1038, 1700, 1500, 1501, 1500, 1501, 1500, 1501, 1503, 1502, 1503, 1502, 1503, 1502, 1503, 1502, 1500, 1501, 1500, 1501, 1500, 1501, 1503, 1502, 1503, 1502, 1503, 1502, 1503, 1502, 1500, 1501, 1500, 1501, 1500, 1501, 1503, 1502, 1503, 1502, 1503, 1502, 1503, 1502, 2020, 2021, 2022, 2023, 2023, 2024, 1025, 6000, 6001, 6000, 6000, 1762, 7020 };
        public static Dictionary<uint, DMap> H_DMaps = new Dictionary<uint, DMap>();
       // public static Hashtable H_DMaps2 = new Hashtable();
        //public static Hashtable MapOwner = new Hashtable();
        public static Dictionary<uint, ushort> MapOwner = new Dictionary<uint, ushort>();
        public static System.Collections.Concurrent.ConcurrentDictionary<uint, ushort> EventMaps = new System.Collections.Concurrent.ConcurrentDictionary<uint, ushort>();
      //  public static TinyMapServer TMapServer = new TinyMapServer();
        public static bool Loaded = false;
        public static void Load()
        {
            if (!Loaded)
            {
                if (Directory.Exists(Program.ConquerPath))
                {
                    uint Time = Native.timeGetTime();
                    /*TMapServer.ConquerDirectory = Program.ConquerPath;
                    TMapServer.LoadHeight = true;
                    TMapServer.ShowOutput = true;
                    TMapServer.ExtractDMaps = true;
                    TMapServer.Load();*/
                    Game.World.DebugAdd +="Starting to load DMaps. \r\n";
                    Console.WriteLine("Starting to load DMaps.");
                    FileStream FS = new FileStream(Program.ConquerPath + @"GameMap.dat", FileMode.Open);
                    BinaryReader BR = new BinaryReader(FS);

                    uint MapCount = BR.ReadUInt32();
                    for (uint i = 0; i < MapCount; i++)
                    {
                        uint MapID = BR.ReadUInt32();
                        string Path = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                        // if (MapsNeeded.Contains((int)MapID))
                        // {
                        DMap D = new DMap(MapID, Path);
                        //Game.World.PlayersInMap.Add(MapID, new ThreadSafeList<uint>(400));
                        H_DMaps.Add(MapID, D);
                        //}
                        BR.ReadInt32();
                    }
                    BR.Close();
                    FS.Close();
                    Loaded = true;
                   // Game.World.DebugAdd +="DMaps loaded successfully in " + (Native.timeGetTime() - Time) + " milliseconds.");
                    Game.World.DebugAdd +=MapCount + " DMaps loaded successfully in " + (Native.timeGetTime() - Time) + " milliseconds. \r\n";
                    Console.WriteLine(MapCount + " DMaps loaded successfully in " + (Native.timeGetTime() - Time) + " milliseconds.");
                }
                else
                    Game.World.DebugAdd +="The specified Conquer Online folder doesn't exist. DMaps couldn't be loaded. \r\n";
            }
            else
                Game.World.DebugAdd +="Dmaps already loaded \r\n";
        }
        public static bool CreateDynamicMap(ushort mapneed, uint ownerid, bool Event)
        {
            bool addedmap = false;
            if (DMaps.MapOwner.ContainsKey(ownerid) || DMaps.EventMaps.ContainsKey(ownerid))
                return false;
            FileStream FS = new FileStream(Program.ConquerPath + @"GameMap.dat", FileMode.Open);
            BinaryReader BR = new BinaryReader(FS);
            uint MapCount = BR.ReadUInt32();
            for (uint i = 0; i < MapCount; i++)
            {
                ushort MapID = (ushort)BR.ReadUInt32();
                string Path = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                if (mapneed == MapID)
                {
                    uint NewMapID = ownerid;
                    
                    DMap D = new DMap(NewMapID, Path);
                    H_DMaps.Add(NewMapID, D);
                    if (!Event)
                        MapOwner.Add(ownerid, mapneed);
                    else
                        EventMaps.TryAdd(ownerid, mapneed);
                    addedmap = true;
                    break;
                }
                BR.ReadInt32();
            }
            BR.Close();
            FS.Close();
            return addedmap;
        }
        public static bool DeleteDynamicMap(/*ushort mapadd,*/ uint ownerid, bool Event)
        {
            bool deletedmap = false;
            //if (!DMaps.MapOwner.Contains(Convert.ToInt32(ownerid)))
            if (!Event)
            {
                if (!DMaps.MapOwner.ContainsKey(ownerid))
                    return false;
            }
            else if (!DMaps.EventMaps.ContainsKey(ownerid))
                    return false;


            uint NewMapID = ownerid;
            H_DMaps.Remove(NewMapID);
           // H_DMaps2.Remove(NewMapID);
           // MapOwner.Remove(Convert.ToInt32(ownerid));
            if (!Event)
                MapOwner.Remove(ownerid);
            else
                EventMaps.Remove(ownerid);
            deletedmap = true;

            return deletedmap;
        }
        public static uint GetHouseID(uint owner)
        {
           // int key = Convert.ToInt32(owner);
            if (MapOwner.ContainsKey(owner))//key
            {
               // return Convert.ToUInt16(MapOwner[owner]);
                return owner;
            }
            return 0;
        }
        public static byte HouseLevel(uint Map)
        {
           // ushort STHseID = Convert.ToUInt16(H_DMaps2[Map]);
            ushort STHseID = (ushort)MapOwner[Map];
            if (STHseID == 3024)
                return 5;
            else if (STHseID == 1765)
                return 4;
            else if (STHseID == 2080)
                return 3;
            else if (STHseID == 1099)
                return 2;
            else
                return 1;
        }
        public static void Save()
        {
            try
            {
                foreach (KeyValuePair<uint, ushort> Map in MapOwner)
                {
                    MySQL.MySqlCommand Maps = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    Maps.Insert("dynamicmaps").Insert("UID", Map.Key).Insert("BaseMap", Map.Value).Insert("house", "1").Execute();
                }
                foreach (KeyValuePair<uint, ushort> Map in EventMaps)
                {
                    MySQL.MySqlCommand Maps = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    Maps.Insert("dynamicmaps").Insert("UID", Map.Key).Insert("BaseMap", Map.Value).Insert("house", "0").Execute();
                }

                //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);

                //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                //    Connect_MySQL.Open();
                //string History = "";
                //MySqlCommand Cmd_MySQL;
                //foreach (KeyValuePair<uint, ushort> Map in MapOwner)
                //{
                //    History = "INSERT INTO dynamicmaps (UID,BaseMap,house) VALUES ('" + Map.Key + "','" + Map.Value + "','" + '1' + "') ON DUPLICATE KEY UPDATE BaseMap = '" + Map.Value + "'";
                //    Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                //    Cmd_MySQL.ExecuteNonQuery();
                //}
                //foreach (KeyValuePair<uint, ushort> Map in EventMaps)
                //{
                //    History = "INSERT INTO dynamicmaps (UID,BaseMap,House) VALUES ('" + Map.Key + "','" + Map.Value + "','" + '0' + "') ON DUPLICATE KEY UPDATE BaseMap = '" + Map.Value + "'";
                //    Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                //    Cmd_MySQL.ExecuteNonQuery();
                //}

                //Connect_MySQL.Close();
            }
            catch (Exception e)
            {
                Game.World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }

            //MemoryStream ms = new MemoryStream();
            
            //BinaryWriter BW = new BinaryWriter(ms);
            //BW.Write(MapOwner.Count);
            //// foreach (DictionaryEntry Map in MapOwner)
            //foreach (KeyValuePair<uint, ushort> Map in MapOwner)
            //{
            //    //BW.Write(Convert.ToUInt32(Map.Key));
            //    BW.Write(Convert.ToUInt32(Map.Key));
            //    BW.Write(Convert.ToUInt16(Map.Value));
            //    //BW.Write(false);
            //    //BW.Write(Convert.ToUInt16(H_DMaps2[Map]));//Map.Value
            //}
            //byte[] buffer = ms.ToArray();
            //ms.Close();
            //BW.Close();
            //if (!Game.World.LowRatedServer)
            //    File.WriteAllBytes(@"C:\OldCODB\DMapOwner.dat",buffer);
            //else
            //    File.WriteAllBytes(@"C:\OldCODB\DMapOwnerNewServer.dat", buffer);
            //ms = new MemoryStream();
            //BW = new BinaryWriter(ms);
            //BW.Write(EventMaps.Count);
            //foreach (KeyValuePair<uint, ushort> Map in EventMaps)
            //{
            //    BW.Write(Convert.ToUInt32(Map.Key));
            //    BW.Write(Convert.ToUInt16(Map.Value));
            //   // BW.Write(true);
            //}
            //buffer = ms.ToArray();
            
            ////BW.Flush();
            ////FS.Flush();
            //BW.Close();
            //ms.Close();
            //File.WriteAllBytes(@"C:\OldCODB\EventMaps.dat", buffer);
        }
        public static void LoadHouses()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("dynamicmaps");
                MySQL.MySqlReader Maps = new MySQL.MySqlReader(Cmd);
                
                while (Maps.Read())
                {
                    uint UID = Maps.ReadUInt32("UID");
                    if (CreateDynamicMap(Maps.ReadUInt16("BaseMap"), UID, !Maps.ReadBoolean("House")) && Maps.ReadBoolean("House"))
                    {
                        if (DMaps.MapOwner.ContainsKey(UID))
                        {
                            Features.HouseTable.AddFurniture(UID, "2100", "8206", "2", "20", "40", true);
                            if (HouseLevel(UID) == 2)
                                Features.HouseTable.AddFurniture(UID, "2101", "8206", "2", "32", "38", true);
                        }
                    }
                }

                //MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);

                //if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
                //    Connect_MySQL.Open();

                //MySqlCommand Cmd_MySQL;
                //MySqlDataReader DataRead_MySQL;

                //string nr = "Select COUNT(*) FROM dynamicmaps";
                //Cmd_MySQL = new MySqlCommand(nr, Connect_MySQL);

                //var nrLines = Convert.ToInt32(Cmd_MySQL.ExecuteScalar().ToString());
                //if (nrLines == 0)
                //{
                //    Cmd_MySQL.Dispose();
                //    return;
                //}
                //else
                //    Cmd_MySQL.Dispose();

                //string History = "SELECT UID,BaseMap,House FROM dynamicmaps";
                //Cmd_MySQL = new MySqlCommand(History, Connect_MySQL);
                //DataRead_MySQL = Cmd_MySQL.ExecuteReader();

                //uint UID = 0;
                //ushort BaseMap = 0;
                //bool House = false;
                //while (DataRead_MySQL.Read())
                //{
                //    UID = Convert.ToUInt32(DataRead_MySQL.GetString(0));
                //    BaseMap = Convert.ToUInt16(DataRead_MySQL.GetString(1));
                //    House = Convert.ToBoolean(Convert.ToByte(DataRead_MySQL.GetString(2)));
                //    bool Success;
                //    if (House)
                //        Success = CreateDynamicMap(BaseMap, UID, false);
                //    else
                //        Success = CreateDynamicMap(BaseMap, UID, true);
                //    if (House && Success)
                //    {
                //        if (DMaps.MapOwner.ContainsKey(UID))
                //        {
                //            Features.HouseTable.AddFurniture(UID, "2100", "8206", "2", "20", "40", true);
                //            if (HouseLevel(UID) == 2)
                //                Features.HouseTable.AddFurniture(UID, "2101", "8206", "2", "32", "38", true);
                //        }
                //    }
                //}


                //DataRead_MySQL.Close();
                //Connect_MySQL.Close();
            }
            catch (Exception e)
            {
                Game.World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }

            //if (System.IO.File.Exists(@"C:\OldCODB\EventMaps.dat"))
            //{
            //    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\EventMaps.dat");
            //    MemoryStream ms = new MemoryStream(buffer);
            //    BinaryReader BR = new BinaryReader(ms);
            //    try
            //    {
            //        int MapCount = BR.ReadInt32();

            //        for (int i = 0; i < MapCount; i++)
            //        {
            //            uint Owner = BR.ReadUInt32();
            //            //ushort Mapid = BR.ReadUInt16();
            //            ushort StMapid = BR.ReadUInt16();
            //            // bool Event = BR.ReadBoolean();
            //            bool Success = CreateDynamicMap(/*Mapid,*/ StMapid, Owner, true);

            //        }

            //    }
            //    catch { }
            //    BR.Close();
            //    ms.Close();
            //}
            //if (!Game.World.LowRatedServer)
            //{
            //    if (File.Exists(@"C:\OldCODB\DMapOwner.dat"))
            //    {
            //        byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\DMapOwner.dat");
            //        MemoryStream ms = new MemoryStream(buffer);
            //        BinaryReader BR = new BinaryReader(ms);

            //        try
            //        {
            //            int MapCount = BR.ReadInt32();
            //            string[] Record = new string[MapCount];
            //            for (int i = 0; i < MapCount; i++)
            //            {
            //                uint Owner = BR.ReadUInt32();
            //                //ushort Mapid = BR.ReadUInt16();
            //                ushort StMapid = BR.ReadUInt16();
            //                // bool Event = BR.ReadBoolean();
            //                bool Success = CreateDynamicMap(/*Mapid,*/ StMapid, Owner, false);
            //                Record[i] = Owner + " " + StMapid;
            //                if (Success)
            //                {
            //                    if (DMaps.MapOwner.ContainsKey(Owner))
            //                    {
            //                        Features.HouseTable.AddFurniture(Owner, "2100", "8206", "2", "20", "40", true);
            //                        if (HouseUpgrade(Owner))
            //                            Features.HouseTable.AddFurniture(Owner, "2101", "8206", "2", "32", "38", true);
            //                    }
            //                }
            //            }
            //            File.WriteAllLines(@"C:\OldCODB\HousesOwners.txt", Record);
            //        }
            //        catch { }
            //        BR.Close();
            //        ms.Close();
            //    }
            //}
        }
    }
    public struct DMapCell
    {
        private Boolean _noAccess;
        public Boolean High;
        public DMapCell(Boolean noAccess)
        {
            _noAccess = noAccess;
            High = false;
        }

        public Boolean NoAccess
        {
            get
            {
                return _noAccess;
            }

            internal set
            {
                _noAccess = value;
            }
        }       
    }
    public class DMap
    {        
        private Int32 Width;
        private Int32 Height;
        private DMapCell[,] Cells;

       /* public DMap(ushort MapID, string Path)
        {
            Game.World.DebugAdd +="Loading " + MapID.ToString() + " : " + Program.ConquerPath + Path + "\r\n";
            if (File.Exists(Program.ConquerPath + Path))
            {
                FileStream FS = new FileStream(Program.ConquerPath + Path, FileMode.Open);
                BinaryReader BR = new BinaryReader(FS);
                BR.ReadBytes(268);
                Width = BR.ReadInt32();
                Height = BR.ReadInt32();
                Cells = new DMapCell[Width, Height];

                byte[] cell_data = BR.ReadBytes(((6 * Width) + 4) * Height);
                int offset = 0;

                for (int y = 0; y < Width; y++)
                {
                    for (int x = 0; x < Height; x++)
                    {
                        Boolean noAccess = BitConverter.ToBoolean(cell_data, offset) != false;

                        if (MapID == 1002)
                        {
                            if (x >= 606 && x <= 641)
                                if (y >= 674 && y <= 680)
                                    noAccess = false;
                            if (x >= 148 && x <= 194)
                                if (y >= 541 && y <= 546)
                                    noAccess = false;
                        }
                        Cells[x, y] = new DMapCell(noAccess);
                        if (MapID == 1038)
                        {
                            if (x <= 119)
                                Cells[x, y].High = true;
                            if (x >= 120 && x <= 222 && y <= 210)
                                Cells[x, y].High = true;
                        }
                        offset += 6;
                        
                    }
                    offset += 4;
                }
                BR.Close();
                FS.Close();
            }
        } */
        public DMap(uint MapID, string Path)
        {
            Game.World.DebugAdd += "Loading " + MapID.ToString() + " : " + Program.ConquerPath + Path + "\r\n";
            if (File.Exists(Program.ConquerPath + Path))
            {
                FileStream FS = new FileStream(Program.ConquerPath + Path, FileMode.Open);
                BinaryReader BR = new BinaryReader(FS);
                BR.ReadBytes(268);
                Width = BR.ReadInt32();
                Height = BR.ReadInt32();
                Cells = new DMapCell[Width, Height];

                byte[] cell_data = BR.ReadBytes(((6 * Width) + 4) * Height);
                int offset = 0;

                for (ushort y = 0; y < Width; y++)
                {
                    for (ushort x = 0; x < Height; x++)
                    {
                        Boolean noAccess = BitConverter.ToBoolean(cell_data, offset) != false;

                        if (MapID == 1002)
                        {
                            if (x >= 606 && x <= 641)
                                if (y >= 674 && y <= 680)
                                    noAccess = false;
                            if (x >= 148 && x <= 194)
                                if (y >= 541 && y <= 546)
                                    noAccess = false;
                        }
                        if (MapID == 1207) // 1207 bridges
                        {
                            if (x >= 910 && x <= 916)
                                if (y >= 919 && y <= 968)
                                    noAccess = false;
                            if (x >= 342 && x <= 394)
                                if (y >= 209 && y <= 215)
                                    noAccess = false;
                        }
                        if (MapID == 1208) // 1208 bridge
                        {
                            if (x >= 396 && x <= 437)
                                if (y >= 349 && y <= 355)
                                    noAccess = false;
                        }
                        if (!noAccess)
                        {
                            if (Game.World.H_NPCs.ContainsKey(MapID))
                            {
                                Dictionary<uint, Game.NPC> MapNPC = Game.World.H_NPCs[MapID];
                                foreach (Game.NPC N in MapNPC.Values)
                                {
                                    if (N.Loc.Map == 1039)
                                    {
                                        if (x == N.Loc.X && y == N.Loc.Y)
                                        {
                                            noAccess = true;
                                            break;
                                        }
                                    }
                                    else if (N.Type == 1086 || (N.Type >= 420 && N.Type <= 1049))
                                    {
                                        if (MapID == N.Loc.Map && x >= N.Loc.X && x <= N.Loc.X && y >= N.Loc.Y && y <= N.Loc.Y)
                                        {
                                            noAccess = true;
                                            break;
                                        }
                                    }
                                    else if (MapID == N.Loc.Map && x >= N.Loc.X - 1 && x <= N.Loc.X + 1 && y >= N.Loc.Y - 1 && y <= N.Loc.Y + 1)
                                    {
                                        noAccess = true;
                                        break;
                                    }
                                }
                            }
                        }
                        Cells[x, y] = new DMapCell(noAccess);
                        if (MapID == 1038)
                        {
                            if (x <= 119)
                                Cells[x, y].High = true;
                            if (x >= 120 && x <= 222 && y <= 210)
                                Cells[x, y].High = true;
                        }
                        //else if (MapID == 1844)
                        //{
                        //    if (y >= 110 && y <= 226 && x >= 173 && x <= 210)
                        //        Cells[x, y].High = true;
                        //    else if (y >= 110 && y <= 226 && x >= 136 && x <= 172)
                        //        Cells[x, y].High = true;
                        //    else if (y >= 110 && y <= 226 && x >= 95 && x <= 135)
                        //        Cells[x, y].High = true;
                        //}
                        offset += 6;

                    }
                    offset += 4;
                }
                BR.Close();
                FS.Close();
            }
        }

        public DMapCell GetCell(ushort X, ushort Y)
        {
            
            return Cells[X, Y];
        }
    }
}
