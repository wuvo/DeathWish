using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using Ultimate.Game;
using Ultimate.Structures;
using System.Configuration;

namespace Ultimate
{
    public struct CompanionInfo
    {
        public uint MinAttack;
        public uint MaxAttack;
        public byte Level;
        public uint SkillUses;
        public ushort HP;
        public uint Mesh;
        public string Name;
        public byte Dodge;
        public ushort Def;
    }
    public struct Shop
    {
        public uint ShopID;
        public byte Type;
        public byte MoneyType;
        public byte ItemAmount;
        public List<uint> Items;
    }
    public struct SkillLearn
    {
        public ushort ID;
        public byte Lvl;
        public bool XP;
        public byte LevelReq;

        public Skill ToSkill()
        {
            Skill S = new Skill();
            S.ID = ID;
            S.Lvl = Lvl;
            S.Exp = 0;
            return S;
        }
    }
    public struct DatabasePlusItem
    {
        public uint ID;
        public byte Plus;
        public ushort HP;
        public uint MinAtk;
        public uint MaxAtk;
        public ushort Defense;
        public ushort MAtk;
        public ushort MDef;
        public ushort Dex;//Vigor Add
        public byte Dodge;//Or Add Ride Speed

        public void ReadThis(string Line)
        {
            string[] Info = Line.Split(' ');
            ID = uint.Parse(Info[0]);
            Plus = byte.Parse(Info[1]);
            HP = ushort.Parse(Info[2]);
            MinAtk = uint.Parse(Info[3]);
            MaxAtk = uint.Parse(Info[4]);
            Defense = ushort.Parse(Info[5]);
            MAtk = ushort.Parse(Info[6]);
            MDef = ushort.Parse(Info[7]);
            Dex = ushort.Parse(Info[8]);
            Dodge = byte.Parse(Info[9]);
        }
    }
    public struct DatabaseItem
    {
        public uint ID;
        public string Name;
        public byte Class;
        public byte ProfReq;
        public byte LevReq;
        public byte GenderReq;
        public ushort StrNeed;
        public ushort AgiNeed;
        public uint Worth;
        public ushort MinAtk;
        public ushort MaxAtk;
        public uint Defense;
        public uint MagicDefense;
        public uint MagicAttack;
        public byte Dodge;
        public byte
            DexGives;
        public uint CPsWorth;
        public ushort Durability;
        public ushort HPAdd;
        public ushort MPAdd;
        public byte Dist;
        public void WriteThis(BinaryWriter BW)
        {
            BW.Write(ID);
            BW.Write(Name);
            BW.Write(Class);
            BW.Write(ProfReq);
            BW.Write(LevReq);
            BW.Write(GenderReq);
            BW.Write(StrNeed);
            BW.Write(AgiNeed);
            BW.Write(Worth);
            BW.Write(MinAtk);
            BW.Write(MaxAtk);
            BW.Write(Defense);
            BW.Write(MagicDefense);
            BW.Write(MagicAttack);
            BW.Write(Dodge);
            BW.Write(DexGives);
            BW.Write(CPsWorth);
            BW.Write(Durability);
        }
        public void ReadThis(BinaryReader BR)
        {
            ID = BR.ReadUInt32();
            Name = BR.ReadString();
            Class = BR.ReadByte();
            ProfReq = BR.ReadByte();
            LevReq = BR.ReadByte();
            GenderReq = BR.ReadByte();
            StrNeed = BR.ReadUInt16();
            AgiNeed = BR.ReadUInt16();
            Worth = BR.ReadUInt32();
            MinAtk = BR.ReadUInt16();
            MaxAtk = BR.ReadUInt16();
            Defense = BR.ReadUInt32();
            MagicDefense = BR.ReadUInt32();
            MagicAttack = BR.ReadUInt32();
            Dodge = BR.ReadByte();
            DexGives = BR.ReadByte();
            CPsWorth = BR.ReadUInt32();
            Durability = BR.ReadUInt16();
        }
    }
    public class Database
    {
        public static uint[][] RevPoints;
        public static uint[][] Portals;
        public static uint[] ProfExp;
        public static ulong[] LevelExp;
        public static ushort[] StonePts = new ushort[9] { 0, 10, 40, 120, 360, 1080, 3240, 9720, 29160 };
        public static ushort[] ComposePts = new ushort[10] { 20, 20, 80, 240, 720, 2160, 6480, 19440, 58320, 2700 };
        public static ushort[] SocPlusExtra = new ushort[9] { 6, 30, 70, 240, 740, 2240, 6670, 20000, 60000 };
        public static ConcurrentDictionary<uint, DatabaseItem> DatabaseItems;
        public static ConcurrentDictionary<string, DatabasePlusItem> DatabasePlusItems;
        public static ConcurrentDictionary<uint, Shop> Shops;
        public static ConcurrentDictionary<uint, Game.Vector2> DefaultCoords = new ConcurrentDictionary<uint, Vector2>();
        public static Dictionary<byte, List<SkillLearn>> SkillForLearning = new Dictionary<byte, List<SkillLearn>>();
        public static Dictionary<uint, CompanionInfo> CompanionInfos = new Dictionary<uint, CompanionInfo>();
        private static Dictionary<byte, string> ArcherStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> WarriorStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> TrojanStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> TaoistStats = new Dictionary<byte, string>();
        private static MEffect _mEffect;
        private static Location _location;

        public static void Dispose()
        {
            RevPoints = null;
            Portals = null;
            DatabaseItems = null;
            DatabasePlusItems = null;
            Shops = null;
            ProfExp = null;
            LevelExp = null;
            DefaultCoords = null;
            SkillForLearning = null;
            StonePts = null;
            ComposePts = null;
            SocPlusExtra = null;
            CompanionInfos = null;
            ArcherStats = null;
            WarriorStats = null;
            TrojanStats = null;
            TaoistStats = null;
        }

        public static void DeleteCharacter(string Charname, string AccName, uint UID = 0)
        {
            if (File.Exists(World.GlobalCharactersPath + Charname + ".chr"))
                File.Delete(World.GlobalCharactersPath + Charname + ".chr");

            if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", UID).ExecuteScalar() > 0)
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                Cmd.Delete("characters", "UID", UID).Execute();
            }
        }
        public static void LoadCompanions()
        {
            if (File.Exists(@"C:\OldCODB\Companions.txt"))
            {
                string[] Lines = File.ReadAllLines(@"C:\OldCODB\Companions.txt");

                foreach (string Line in Lines)
                {
                    string[] Info = Line.Split(' ');
                    CompanionInfo C = new CompanionInfo();
                    uint Type = uint.Parse(Info[0]);
                    C.MinAttack = uint.Parse(Info[1]);
                    C.MaxAttack = uint.Parse(Info[2]);
                    C.Level = byte.Parse(Info[3]);
                    C.SkillUses = uint.Parse(Info[4]);
                    C.HP = ushort.Parse(Info[5]);
                    C.Mesh = uint.Parse(Info[6]);
                    C.Name = Info[7];
                    C.Dodge = byte.Parse(Info[8]);
                    C.Def = ushort.Parse(Info[9]);
                    CompanionInfos.Add(Type, C);
                }
                World.DebugAdd += "Companions loaded\r\n";
            }
        }
        public static void AddSkills()
        {
            List<SkillLearn> Warrior = new List<SkillLearn>();
            Warrior.Add(new SkillLearn() { ID = (ushort)1015, XP = true, LevelReq = (byte)15 });
            Warrior.Add(new SkillLearn() { ID = (ushort)1020, XP = true, LevelReq = (byte)15 });
            Warrior.Add(new SkillLearn() { ID = (ushort)1025, XP = true, LevelReq = (byte)3 });
            Warrior.Add(new SkillLearn() { ID = (ushort)1040, XP = true, LevelReq = (byte)15 });
            Warrior.Add(new SkillLearn() { ID = (ushort)1051, LevelReq = (byte)63 });
            SkillForLearning.Add((byte)2, Warrior);

            List<SkillLearn> Trojan = new List<SkillLearn>();
            Trojan.Add(new SkillLearn() { ID = (ushort)1015, XP = true, LevelReq = (byte)15 });
            Trojan.Add(new SkillLearn() { ID = (ushort)1110, XP = true, LevelReq = (byte)3 });
            Trojan.Add(new SkillLearn() { ID = (ushort)1115, LevelReq = (byte)40 });
            Trojan.Add(new SkillLearn() { ID = (ushort)1190, LevelReq = (byte)40 });
            Trojan.Add(new SkillLearn() { ID = (ushort)1270, LevelReq = (byte)41 });
            SkillForLearning.Add((byte)1, Trojan);

            List<SkillLearn> Archer = new List<SkillLearn>();
            Archer.Add(new SkillLearn() { ID = (ushort)8002, XP = true, LevelReq = (byte)3 });
            Archer.Add(new SkillLearn() { ID = (ushort)8001, LevelReq = (byte)23 });
            Archer.Add(new SkillLearn() { ID = (ushort)8000, LevelReq = (byte)46 });
            Archer.Add(new SkillLearn() { ID = (ushort)8003, LevelReq = (byte)70 });
            Archer.Add(new SkillLearn() { ID = (ushort)8003, Lvl = 1, LevelReq = (byte)100 });
            Archer.Add(new SkillLearn() { ID = (ushort)8030, XP = true, LevelReq = (byte)70 });
            Archer.Add(new SkillLearn() { ID = (ushort)9000, LevelReq = (byte)71 });
            SkillForLearning.Add((byte)4, Archer);


            List<SkillLearn> WaterTaoist = new List<SkillLearn>();
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1055, LevelReq = (byte)40 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1195, LevelReq = (byte)44 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1085, LevelReq = (byte)45 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1090, LevelReq = (byte)50 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1095, LevelReq = (byte)55 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1075, LevelReq = (byte)60 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1100, LevelReq = (byte)70 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1175, LevelReq = (byte)81 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1170, LevelReq = (byte)94 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1050, XP = true, LevelReq = (byte)40 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1125, LevelReq = (byte)40 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)5001, LevelReq = (byte)70 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1280, LevelReq = (byte)50 });
            SkillForLearning.Add((byte)13, WaterTaoist);

            List<SkillLearn> FireTaoist = new List<SkillLearn>();
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1195, LevelReq = (byte)44 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1150, LevelReq = (byte)55 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1180, LevelReq = (byte)52 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1120, LevelReq = (byte)65 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1125, LevelReq = (byte)40 });
            FireTaoist.Add(new SkillLearn() { ID = (ushort)5001, LevelReq = (byte)70 });
            SkillForLearning.Add((byte)14, FireTaoist);

            List<SkillLearn> Taoist = new List<SkillLearn>();
            Taoist.Add(new SkillLearn() { ID = (ushort)1000, LevelReq = (byte)1 });
            Taoist.Add(new SkillLearn() { ID = (ushort)1005, LevelReq = (byte)5 });
            Taoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 });
            SkillForLearning.Add((byte)10, Taoist);
        }
        public static void SaveKOs()
        {
            MemoryStream FS = new MemoryStream();
            BinaryWriter BW = new BinaryWriter(FS);


            for (int i = 0; i < World.KOBoard.Length; i++)
                World.KOBoard[i].WriteThis(BW);
            byte[] buffer = FS.ToArray();
            BW.Close();
            FS.Close();

            if (!World.LowRatedServer)
                File.WriteAllBytes(@"C:\OldCODB\KOBoard.dat", buffer);
            else
                File.WriteAllBytes(@"C:\OldCODB\KOBoardNewServer.dat", buffer);


        }
        public static void LoadKOs()
        {
            if (!World.LowRatedServer)
            {
                if (System.IO.File.Exists(@"C:\OldCODB\KOBoard.dat"))
                {

                    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\KOBoard.dat");
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryReader BR = new BinaryReader(ms);

                    for (int i = 0; i < World.KOBoard.Length; i++)
                        World.KOBoard[i].ReadThis(BR);
                    BR.Close();
                    ms.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\KOBoardNewServer.dat"))
                {
                    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\KOBoardNewServer.dat");
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryReader BR = new BinaryReader(ms);

                    for (int i = 0; i < World.KOBoard.Length; i++)
                        World.KOBoard[i].ReadThis(BR);
                    BR.Close();
                    ms.Close();
                }
            }
        }
        public static void SaveEmpire()
        {
            MemoryStream FS = new MemoryStream();//ce deschide? nu deschide nimic :)), creaza un buffer iar bw o sa scrie in el, la sf copiaza bufferu si dupa ce inchide bw si ms, scrie cu File.Writeallbytes bufferu ca sa nu ai probleme cu alte procese :) ok mersi mult vezi daca merge :P ok
            BinaryWriter BW = new BinaryWriter(FS);

            for (int i = 0; i < World.EmpireBoard.Length; i++)
                World.EmpireBoard[i].WriteThis(BW);
            byte[] buffer = FS.ToArray();

            // BW.Flush();
            // FS.Flush();
            BW.Close();
            FS.Close();
            if (!World.LowRatedServer)
                File.WriteAllBytes(@"C:\OldCODB\Nobility.dat", buffer);
            else
                File.WriteAllBytes(@"C:\OldCODB\NobilityNewServer.dat", buffer);
        }
        public static void LoadEmpire()
        {
            if (!World.LowRatedServer)
            {
                //if (System.IO.File.Exists(@"C:\OldCODB\Nobility.dat"))
                //{
                //    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\Nobility.dat");
                //    MemoryStream ms = new MemoryStream(buffer);
                //    BinaryReader BR = new BinaryReader(ms);

                //    for (int i = 0; i < World.EmpireBoard.Length; i++)
                //        World.EmpireBoard[i].ReadThis(BR);
                //    BR.Close();
                //    ms.Close();
                //}
                string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
                foreach (string Path in Paths)
                {
                    if (Path.Remove(0, Path.Length - 4) == ".chr")
                    {
                        try
                        {
                            string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                            Character C;
                            C = World.CharacterFromName2(Name);
                            if (C == null)
                            {
                                string Account = "";
                                C = LoadCharacter(Name, ref Account);
                                if (C != null)
                                {
                                    if (DateTime.Now <= C.LastLogin.AddDays(14))
                                    {
                                        C.Nobility.ListPlace = -1;
                                        World.NewEmpire(C);
                                    }
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            World.ExcAdd += e.ToString() + "\r\n";
                        }

                    }
                }
            }
            else
            {
                if (System.IO.File.Exists(@"C:\OldCODB\NobilityNewServer.dat"))
                {
                    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\NobilityNewServer.dat");
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryReader BR = new BinaryReader(ms);

                    for (int i = 0; i < World.EmpireBoard.Length; i++)
                        World.EmpireBoard[i].ReadThis(BR);
                    BR.Close();
                    ms.Close();
                }
            }
        }
        public static void LoadShops()
        {
            Shops = new ConcurrentDictionary<uint, Shop>();

            IniFile I = new IniFile(@"C:\OldCODB\Shop.dat");
            int ShopAmount = I.ReadInt32("Header", "Amount");

            for (int i = 0; i < ShopAmount; i++)
            {
                Shop S = new Shop();
                S.ShopID = I.ReadUInt32("Shop" + i.ToString(), "ID");
                S.Type = I.ReadByte("Shop" + i.ToString(), "Type");
                S.MoneyType = I.ReadByte("Shop" + i.ToString(), "MoneyType");
                S.ItemAmount = I.ReadByte("Shop" + i.ToString(), "ItemAmount");
                S.Items = new List<uint>(S.ItemAmount);
                for (int e = 0; e < S.ItemAmount; e++)
                    S.Items.Add(I.ReadUInt32("Shop" + i.ToString(), "Item" + e.ToString()));

                Shops.TryAdd(S.ShopID, S);
            }
            I.Close();
        }
        public static void LoadLevelExp()
        {
            LevelExp = new ulong[130];
            LevelExp[0] = 0;
            byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\ExpNeed.dat");
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader BR = new BinaryReader(ms);
            for (byte i = 1; i < 130; i++)
                LevelExp[i] = BR.ReadUInt32();

            BR.Close();
            ms.Close();
        }
        public static void LoadPortals()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("portals");
                MySQL.MySqlReader Portals = new MySQL.MySqlReader(Cmd);

                while (Portals.Read())
                {
                    Dbase.Portal P = new Dbase.Portal();
                    P.PortalMapID = Portals.ReadUInt16("PortalMapID");
                    P.PortalX = Portals.ReadUInt16("PortalX");
                    P.PortalY = Portals.ReadUInt16("PortalY");
                    P.DestinationMapID = Portals.ReadUInt16("DestinationMapID");
                    P.DestinationX = Portals.ReadUInt16("DestinationX");
                    P.DestinationY = Portals.ReadUInt16("DestinationY");
                    World.Portals.Add(P);
                }

            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }

        public static void LoadProfExp()
        {
            ProfExp = new uint[20];
            ProfExp[0] = 0;
            ProfExp[1] = 1200;
            ProfExp[2] = 68000;
            ProfExp[3] = 250000;
            ProfExp[4] = 640000;
            ProfExp[5] = 1600000;
            ProfExp[6] = 4000000;
            ProfExp[7] = 10000000;
            ProfExp[8] = 22000000;
            ProfExp[9] = 40000000;
            ProfExp[10] = 90000000;
            ProfExp[11] = 95000000;
            ProfExp[12] = 142500000;
            ProfExp[13] = 213750000;
            ProfExp[14] = 320625000;
            ProfExp[15] = 480937500;
            ProfExp[16] = 721406250;
            ProfExp[17] = 1082109375;
            ProfExp[18] = 1623164063;
            ProfExp[19] = 2100000000;
        }
        public static void LoadRevPoints()
        {
            RevPoints = new uint[74][];
            RevPoints[0] = new uint[4] { 1002, 1002, 430, 378 };
            RevPoints[1] = new uint[4] { 1005, 1005, 50, 50 };
            RevPoints[2] = new uint[4] { 1006, 1002, 430, 378 };
            RevPoints[3] = new uint[4] { 1008, 1002, 430, 378 };
            RevPoints[4] = new uint[4] { 1009, 1002, 430, 378 };
            RevPoints[5] = new uint[4] { 1010, 1002, 430, 378 };
            RevPoints[6] = new uint[4] { 1007, 1002, 430, 378 };
            RevPoints[7] = new uint[4] { 1004, 1002, 430, 378 };
            RevPoints[8] = new uint[4] { 1028, 1002, 430, 378 };
            RevPoints[9] = new uint[4] { 1037, 1002, 430, 378 };
            RevPoints[10] = new uint[4] { 1038, 1002, 439, 398 };
            RevPoints[11] = new uint[4] { 1015, 1015, 717, 577 };
            RevPoints[12] = new uint[4] { 1001, 1000, 499, 650 };
            RevPoints[13] = new uint[4] { 1000, 1000, 499, 650 };
            RevPoints[14] = new uint[4] { 1013, 1011, 193, 266 };
            RevPoints[15] = new uint[4] { 1011, 1011, 193, 266 };
            RevPoints[16] = new uint[4] { 1076, 1011, 193, 266 };
            RevPoints[17] = new uint[4] { 1014, 1011, 193, 266 };
            RevPoints[18] = new uint[4] { 1020, 1020, 566, 562 };
            RevPoints[19] = new uint[4] { 1075, 1020, 566, 562 };
            RevPoints[20] = new uint[4] { 1012, 1020, 566, 656 };
            RevPoints[21] = new uint[4] { 6000, 6000, 29, 72 };
            RevPoints[22] = new uint[4] { 1505, 1002, 429, 378 };
            RevPoints[23] = new uint[4] { 1506, 1020, 567, 565 };
            RevPoints[24] = new uint[4] { 1507, 1015, 723, 573 };
            RevPoints[25] = new uint[4] { 1508, 1000, 500, 650 };
            RevPoints[26] = new uint[4] { 1509, 1011, 190, 271 };
            RevPoints[27] = new uint[4] { 1090, 1002, 438, 377 };
            RevPoints[28] = new uint[4] { 1003, 1002, 438, 377 };
            RevPoints[29] = new uint[4] { 1511, 1002, 438, 377 };
            RevPoints[30] = new uint[4] { 1018, 1002, 438, 377 };
            RevPoints[31] = new uint[4] { 1081, 1002, 438, 377 };
            RevPoints[32] = new uint[4] { 1762, 1002, 430, 380 };
            RevPoints[33] = new uint[4] { 1351, 1002, 430, 380 };
            RevPoints[34] = new uint[4] { 1352, 1002, 430, 380 };
            RevPoints[35] = new uint[4] { 1353, 1002, 430, 380 };
            RevPoints[36] = new uint[4] { 1354, 1002, 430, 380 };
            RevPoints[37] = new uint[4] { 1210, 1213, 450, 270 };
            RevPoints[38] = new uint[4] { 1207, 1213, 450, 270 };
            RevPoints[39] = new uint[4] { 1208, 1213, 450, 270 };
            RevPoints[40] = new uint[4] { 1211, 1213, 450, 270 };
            RevPoints[41] = new uint[4] { 1212, 1213, 450, 270 };
            RevPoints[42] = new uint[4] { 1214, 1213, 450, 270 };
            RevPoints[43] = new uint[4] { 1215, 1213, 450, 270 };
            RevPoints[44] = new uint[4] { 1051, 1015, 723, 573 };
            RevPoints[45] = new uint[4] { 1043, 1002, 438, 377 };
            RevPoints[46] = new uint[4] { 1044, 1002, 438, 377 };
            RevPoints[47] = new uint[4] { 1045, 1002, 438, 377 };
            RevPoints[48] = new uint[4] { 1046, 1002, 438, 377 };
            RevPoints[49] = new uint[4] { 1047, 1002, 438, 377 };
            RevPoints[50] = new uint[4] { 1048, 1002, 438, 377 };
            RevPoints[51] = new uint[4] { 1049, 1002, 438, 377 };
            RevPoints[52] = new uint[4] { 2021, 1020, 566, 564 };
            RevPoints[53] = new uint[4] { 2022, 1020, 566, 564 };
            RevPoints[54] = new uint[4] { 2023, 1020, 566, 564 };
            RevPoints[55] = new uint[4] { 2024, 1020, 566, 564 };
            RevPoints[56] = new uint[4] { 1027, 1000, 500, 650 };
            RevPoints[57] = new uint[4] { 1026, 1020, 566, 564 };
            RevPoints[58] = new uint[4] { 1025, 1011, 190, 271 };
            RevPoints[59] = new uint[4] { 1016, 1011, 190, 271 };
            RevPoints[60] = new uint[4] { 1029, 1213, 450, 270 };
            RevPoints[61] = new uint[4] { 1077, 1000, 499, 650 };
            RevPoints[62] = new uint[4] { 1075, 1213, 450, 270 };
            RevPoints[63] = new uint[4] { 1082, 1015, 717, 577 };
            RevPoints[64] = new uint[4] { 1785, 1000, 499, 650 };
            RevPoints[65] = new uint[4] { 1300, 1000, 499, 650 };
            RevPoints[66] = new uint[4] { 701, 1002, 430, 378 };
            RevPoints[67] = new uint[4] { 1105, 1002, 430, 378 };
            RevPoints[68] = new uint[4] { 8505, 1002, 429, 378 };
            RevPoints[69] = new uint[4] { 8506, 1020, 567, 565 };
            RevPoints[70] = new uint[4] { 8507, 1015, 723, 573 };
            RevPoints[71] = new uint[4] { 8508, 1000, 500, 650 };
            RevPoints[72] = new uint[4] { 8509, 1011, 190, 271 };
            RevPoints[73] = new uint[4] { 2020, 1213, 442, 269 };

            //RevPoints[64] = new uint[4] { 1616, 1002, 430, 378 };
            //RevPoints[65] = new uint[4] { 700, 1002, 430, 378 };
            ////RevPoints[66] = new uint[4] { 1763, 1002, 430, 378 };
            //RevPoints[67] = new uint[4] { 1780, 1000, 499, 650 };
        }
        public static void LoadPlusInfo()
        {
            DatabasePlusItems = new ConcurrentDictionary<string, DatabasePlusItem>();
            MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("itemadd");
            MySQL.MySqlReader R = new MySQL.MySqlReader(Cmd);

            while (R.Read())
            {
                DatabasePlusItem I = new DatabasePlusItem()
                {
                    ID = R.ReadUInt32("BaseID"),
                    Plus = R.ReadByte("Plus"),
                    HP = R.ReadUInt16("Health"),
                    MinAtk = R.ReadUInt32("MinimumDamage"),
                    MaxAtk = R.ReadUInt32("MaximumDamage"),
                    Defense = R.ReadUInt16("Defense"),
                    MAtk = R.ReadUInt16("MagicDamage"),
                    MDef = R.ReadUInt16("MagicDefense"),
                    Dex = R.ReadUInt16("Accuracy"),
                    Dodge = R.ReadByte("Dodge")
                };

                DatabasePlusItems.TryAdd(I.ID.ToString() + I.Plus.ToString(), I);
            }

            //Dictionary<uint, DatabasePlusItem> toIncrease = new Dictionary<uint, DatabasePlusItem>();
            //ushort PlusSeven = 0;
            //ushort PlusEight = 0;
            //foreach (DatabasePlusItem I in DatabasePlusItems.Values.OrderBy(p => p.ID))
            //{
            //    if (I.ID >= 152000 && I.ID <= 152259)
            //    {
            //        if (I.Plus >= 7 && I.Plus <= 9)
            //        {
            //            toIncrease.Add(I.ID * 10 + I.Plus, I);
            //        }
            //    }
            //}
            //foreach (KeyValuePair<uint, DatabasePlusItem> I in toIncrease.OrderBy(p => p.Key))
            //{
            //    if (I.Value.Plus == 7)
            //    {
            //        PlusSeven = I.Value.MAtk;
            //    }
            //    else if (I.Value.Plus == 8)
            //    {
            //        PlusEight = I.Value.MAtk;
            //    }
            //    else if (I.Value.Plus == 9)
            //    {
            //        Console.WriteLine($"+7: {PlusSeven}  +8: {PlusEight}  Old +9: {I.Value.MAtk}  New +9: {I.Value.MAtk + ((PlusEight - PlusSeven) * 2)}");

            //        Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE).Update("itemadd").Set("MagicDamage", I.Value.MAtk + ((PlusEight - PlusSeven) * 2)).Where("UniqueID", I.Value.ID.ToString() + I.Value.Plus.ToString());
            //        Cmd.Execute();
            //    }
            //}

            //string[] ItemAdd = File.ReadAllLines(@"C:\OldCODB\ItemAdd.ini");
            //DatabasePlusItems = new ConcurrentDictionary<string, DatabasePlusItem>();

            //foreach (string S in ItemAdd)
            //{
            //    DatabasePlusItem I = new DatabasePlusItem();
            //    I.ReadThis(S);
            //    DatabasePlusItems.TryAdd(I.ID.ToString() + I.Plus.ToString(), I);
            //}
            //foreach (DatabasePlusItem I in DatabasePlusItems.Values)
            //{
            //    MySQL.MySqlCommand Cmd;
            //    if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Count("itemadd").Where("UniqueID", I.ID * 10 + I.Plus).ExecuteScalar() == 0)
            //    {
            //        Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("itemadd");
            //        Cmd.Insert("UniqueID", I.ID * 10 + I.Plus).Insert("BaseID", I.ID).Insert("Plus", I.Plus).Insert("Health", I.HP).Insert("MinimumDamage", I.MaxAtk).Insert("MaximumDamage", I.MinAtk).Insert("Defense", I.Defense).Insert("MagicDamage", I.MAtk).Insert("MagicDefense", I.MDef).Insert("Accuracy", I.Dex).Insert("Dodge", I.Dodge);
            //        Cmd.Execute();
            //    }
            //    else
            //    {

            //    }
            //}
        }
        public static void LoadNPCs(bool Reload = false)
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("npcs");
                MySQL.MySqlReader NPC = new MySQL.MySqlReader(Cmd);

                while (NPC.Read())
                {
                    NPC N = new NPC()
                    {
                        EntityID = NPC.ReadUInt32("UID"),
                        Type = NPC.ReadUInt16("Type"),
                        Flags = NPC.ReadByte("Flags"),
                        Avatar = NPC.ReadByte("Face"),
                        Loc = new Location()
                        {
                            Map = NPC.ReadUInt16("Map"),
                            X = NPC.ReadUInt16("X"),
                            Y = NPC.ReadUInt16("Y")
                        }
                    };

                    if (N.Flags == 21)
                        N.Level = (byte)((N.Type - 427) / 6 + 20);
                    if (N.Flags == 22)
                        N.Level = (byte)((N.Type - 437) / 6 + 20);
                    if (N.Type == 1507 || N.Type == 1527)
                        N.Level = 125;
                    else if (N.Type == 1517 || N.Type == 1587)
                        N.Level = 130;
                    if (N.Flags == 21 || N.Flags == 22)
                    {
                        N.CurHP = 10000;
                        N.MaxHP = 10000;
                    }

                    byte Month = NPC.ReadByte("Month");
                    byte Begin = NPC.ReadByte("Begin");
                    byte End = NPC.ReadByte("End");

                    if ((Month == 0 && Begin == 0 && End == 0) || (Month == DateTime.Now.Month && DateTime.Now.Day >= Begin && DateTime.Now.Day <= End))
                    {
                        if (!World.H_NPCs.ContainsKey(N.Loc.Map))
                            World.H_NPCs.Add(N.Loc.Map, new Dictionary<uint, NPC>());
                        Dictionary<uint, NPC> MapNpcs = World.H_NPCs[N.Loc.Map];
                        if (Reload)
                        {
                            if (!MapNpcs.ContainsKey(N.EntityID))
                                MapNpcs.Add(N.EntityID, N);
                        }
                        else
                            MapNpcs.Add(N.EntityID, N);
                    }
                }

            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }
        public static void LoadNPCs(byte Type)
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("npcs").Where("Month", Type);
                MySQL.MySqlReader NPC = new MySQL.MySqlReader(Cmd);

                while (NPC.Read())
                {
                    NPC N = new NPC()
                    {
                        EntityID = NPC.ReadUInt32("UID"),
                        Type = NPC.ReadUInt16("Type"),
                        Flags = NPC.ReadByte("Flags"),
                        Avatar = NPC.ReadByte("Face"),
                        Loc = new Location()
                        {
                            Map = NPC.ReadUInt16("Map"),
                            X = NPC.ReadUInt16("X"),
                            Y = NPC.ReadUInt16("Y")
                        }
                    };

                    if (!World.H_NPCs.ContainsKey(N.Loc.Map))
                        World.H_NPCs.Add(N.Loc.Map, new Dictionary<uint, NPC>());
                    Dictionary<uint, NPC> MapNpcs = World.H_NPCs[N.Loc.Map];
                    MapNpcs.Add(N.EntityID, N);
                }

            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }
        }
        public static void LoadSquamas(MapEffect mapEffect, bool reload = false)
        {
            foreach (var line in File.ReadAllLines(@"C:\OldCODB\Squamas.txt"))
            {
                mapEffect.DropTime = DateTime.Now;
                _location = mapEffect.Loc = new Location();
                mapEffect.Loc.Map = ushort.Parse(line.Split(' ')[2]);
                _mEffect = mapEffect.Info = new MEffect();
                mapEffect.Info.ID = uint.Parse(line.Split(' ')[1]);

                mapEffect.UID = uint.Parse(line.Split(' ')[0]);
                mapEffect.Info.UID = mapEffect.UID;

                if (reload)
                    if (World.ActiveSquamas.Contains(mapEffect.UID))
                        continue;

                mapEffect.Loc.X = ushort.Parse(line.Split(' ')[3]);
                mapEffect.Loc.Y = ushort.Parse(line.Split(' ')[4]);
                if (World.H_Effects.ContainsKey(mapEffect.Loc.Map))
                    if (!mapEffect.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, MapEffect>)World.H_Effects[mapEffect.Loc.Map]))
                        continue;
                mapEffect.Drop();
            }
            //World.Squamaspawn = DateTime.Now;
        }
        public static void LoadMobs(bool Reload = false)
        {
            string[] FMobs;
            if (!World.LowRatedServer)
                FMobs = File.ReadAllLines(@"C:\OldCODB\MobInfos.txt");
            else FMobs = File.ReadAllLines(@"C:\OldCODB\MobInfosLowRates.txt");
            Dictionary<int, Mob> Mobs = new Dictionary<int, Mob>(FMobs.Length);
            for (int i = 0; i < FMobs.Length; i++)
            {
                if (FMobs[i][0] != '*')
                {
                    Mob M = new Mob(FMobs[i]);
                    Mobs.Add(M.MobID, M);
                }
            }
            int MobsCount = 0;
            if (Reload)
            {
                foreach (ConcurrentDictionary<uint, Mob> H in World.H_Mobs.Values)
                    H.Clear();
                World.H_Mobs.Clear();
            }
            string[] FSpawns = File.ReadAllLines(@"C:\OldCODB\MobSpawns.txt");
            foreach (string Spawn in FSpawns)
            {
                if (Spawn[0] == '*') return;
                string[] SpawnInfo = Spawn.Split(' ');
                int MobID = int.Parse(SpawnInfo[0]);
                int Count = int.Parse(SpawnInfo[1]);
                uint Map = uint.Parse(SpawnInfo[2]);//ushort
                ushort XFrom = ushort.Parse(SpawnInfo[3]);
                ushort YFrom = ushort.Parse(SpawnInfo[4]);
                ushort XTo = ushort.Parse(SpawnInfo[5]);
                ushort YTo = ushort.Parse(SpawnInfo[6]);
                if (!World.H_Mobs.ContainsKey(Map))
                {
                    World.H_Mobs.TryAdd(Map, new ConcurrentDictionary<uint, Mob>());
                    if (!Reload)//(!World.PlayersInMap.ContainsKey(Map))  //Valis fix for reloading mob spawns
                        World.PlayersInMap.Add(Map, new ConcurrentDictionary<uint, Character>());
                }

                ConcurrentDictionary<uint, Mob> MapMobs = (ConcurrentDictionary<uint, Mob>)World.H_Mobs[Map];
                if (!DMaps.H_DMaps.ContainsKey(Map))
                {
                    continue;
                }
                DMap D = (DMap)DMaps.H_DMaps[Map];

                for (int i = 0; i < Count; i++)
                {
                    Mob _Mob = new Mob((Mob)Mobs[MobID]);
                    _Mob.Loc = new Location();
                    _Mob.Loc.Map = Map;
                    _Mob.Loc.X = (ushort)Program.Rnd.Next(Math.Min(XFrom, XTo), Math.Max(XFrom, XTo));
                    _Mob.Loc.Y = (ushort)Program.Rnd.Next(Math.Min(YFrom, YTo), Math.Max(YFrom, YTo));

                    try
                    {
                        while (D != null && D.GetCell(_Mob.Loc.X, _Mob.Loc.Y).NoAccess && _Mob.Loc.Y > 0 && _Mob.Loc.Y < 1500)
                        {
                            _Mob.Loc.X = (ushort)Program.Rnd.Next(Math.Min(XFrom, XTo), Math.Max(XFrom, XTo));
                            _Mob.Loc.Y = (ushort)Program.Rnd.Next(Math.Min(YFrom, YTo), Math.Max(YFrom, YTo));
                        }
                    }
                    catch { World.ExcAdd += "Mobid: " + _Mob.MobID + " Mob.X: " + _Mob.Loc.X + " Mob.Y: " + _Mob.Loc.Y + " Mob.map: " + _Mob.Loc.Map + " MOBERROR! \r\n"; }
                    //_Mob.StartLoc = _Mob.Loc;
                    _Mob.StartLoc.XFrom = XFrom;
                    _Mob.StartLoc.XTo = XTo;
                    _Mob.StartLoc.YFrom = YFrom;
                    _Mob.StartLoc.Yto = YTo;
                    _Mob.StartLoc.Map = Map;
                    _Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                    while (MapMobs.ContainsKey(_Mob.EntityID))
                        _Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                    MapMobs.TryAdd(_Mob.EntityID, _Mob);
                    MobsCount++;
                    _Mob.LastTarget = DateTime.Now;
                    if (_Mob._UltimateBoss() || _Mob.MobID == 501 || _Mob.MobID == 701 || _Mob.MobID == 700 || _Mob.MobID == 150 || _Mob.MobID == 151 || _Mob.MobID == 500 || /*_Mob.MobID == 409 ||*/ _Mob.MobID == 4152 || _Mob.MobID == 8424 || _Mob.MobID == 8423) // ultimatepluto, expmob, *dbdevil*, waterdevilking, teratodragon
                    {
                        _Mob.Alive = false;
                        _Mob.CurrentHP = 0;
                    }
                }
            }
            if (Reload)
                World.SendMsgToAll("SYSTEM", "Monsters reloaded!", 2011, 0);
            World.DebugAdd += "Mobs loaded " + MobsCount.ToString() + "\r\n";
        }
        public static void CreateEquipsDrops()
        {
            StreamWriter SW = new StreamWriter(@"C:\OldCODB\EquipDrops.txt");
            foreach (DatabaseItem DBI in DatabaseItems.Values)
            {
                if (DBI.LevReq >= 1 && DBI.LevReq <= 120 && (ItemIDManipulation.Digit(DBI.ID, 6) == 3 || (ItemIDManipulation.Digit(DBI.ID, 6) == 1) && ItemIDManipulation.Digit(DBI.ID, 1) == 4 || ItemIDManipulation.Digit(DBI.ID, 6) == 5 || ItemIDManipulation.Digit(DBI.ID, 6) == 1 || ItemIDManipulation.Digit(DBI.ID, 6) == 6))
                    SW.WriteLine(DBI.LevReq.ToString() + " " + DBI.ID.ToString());
            }

            SW.Flush();
            SW.Close();
        }
        public static void GetStats(Character character)
        {
            string Job = "";
            switch (character.Job)
            {
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15: Job = "Trojan"; break;
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25: Job = "Warrior"; break;
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45: Job = "Archer"; break;
                default: Job = "Taoist"; break;
            }
            byte lvl = character.Level;
            if (lvl > 120)
                lvl = 120;

            string[] Data = null;
            if (Job == "Trojan")
                Data = TrojanStats[lvl].Split(',');
            else if (Job == "Warrior")
                Data = WarriorStats[lvl].Split(',');
            else if (Job == "Archer")
                Data = ArcherStats[lvl].Split(',');
            else if (Job == "Taoist")
                Data = TaoistStats[lvl].Split(',');

            character.Str = Convert.ToUInt16(Data[0]);
            character.Vit = Convert.ToUInt16(Data[1]);
            character.Agi = Convert.ToUInt16(Data[2]);
            character.Spi = Convert.ToUInt16(Data[3]);
        }
        public static void GetInitialStats(byte inJob, ref ushort Str, ref ushort Agi, ref ushort Vit, ref ushort Spi)
        {
            string Job = "";
            switch (inJob)
            {
                case 10: Job = "Trojan"; break;
                case 20: Job = "Warrior"; break;
                case 40: Job = "Archer"; break;
                default: Job = "Taoist"; break;
            }
            byte lvl = 1;
            string[] Data = null;
            if (Job == "Trojan")
                Data = TrojanStats[lvl].Split(',');
            else if (Job == "Warrior")
                Data = WarriorStats[lvl].Split(',');
            else if (Job == "Archer")
                Data = ArcherStats[lvl].Split(',');
            else if (Job == "Taoist")
                Data = TaoistStats[lvl].Split(',');

            Str = Convert.ToUInt16(Data[0]);
            Vit = Convert.ToUInt16(Data[1]);
            Agi = Convert.ToUInt16(Data[2]);
            Spi = Convert.ToUInt16(Data[3]);
        }
        public static void ReadAllCharacterStats()
        {
            IniFile F = new IniFile(@"C:\OldCODB\Stats.txt");
            for (byte lvl = 1; lvl < 122; lvl++)
            {
                string job = "Archer[" + lvl + "]";
                string Data = F.ReadString("Stats", job);
                ArcherStats.Add(lvl, Data);
                job = "Warrior[" + lvl + "]";
                Data = F.ReadString("Stats", job);
                WarriorStats.Add(lvl, Data);
                job = "Trojan[" + lvl + "]";
                Data = F.ReadString("Stats", job);
                TrojanStats.Add(lvl, Data);
                job = "Taoist[" + lvl + "]";
                Data = F.ReadString("Stats", job);
                TaoistStats.Add(lvl, Data);
            }
        }

        public static void LoadItems()
        {
            if (File.Exists(@"C:\OldCODB\Items.txt"))
            {
                TextReader TR = new StreamReader(@"C:\OldCODB\Items.txt");
                string Items = TR.ReadToEnd();
                TR.Close();
                DatabaseItems = new ConcurrentDictionary<uint, DatabaseItem>();
                Items = Items.Replace("\r", "");
                string[] AllItems = Items.Split('\n');
                foreach (string _item in AllItems)
                {
                    string _item_ = _item.Trim();
                    if (_item_.Length >= 2)
                    {
                        if (_item_.IndexOf("//", 0, 2) != 0)
                        {
                            string[] data = _item_.Split(' ');
                            if (data.Length >= 37)
                            {
                                DatabaseItem NewItem = new DatabaseItem();
                                NewItem.ID = Convert.ToUInt32(data[0]);
                                NewItem.Name = data[1].Trim();
                                NewItem.Class = Convert.ToByte(data[2]);
                                NewItem.ProfReq = Convert.ToByte(data[3]);
                                NewItem.LevReq = Convert.ToByte(data[4]);
                                NewItem.GenderReq = Convert.ToByte(data[5]);
                                NewItem.StrNeed = Convert.ToUInt16(data[6]);
                                NewItem.AgiNeed = Convert.ToUInt16(data[7]);
                                NewItem.Worth = Convert.ToUInt32(data[12]);
                                NewItem.MaxAtk = Convert.ToUInt16(data[14]);
                                NewItem.MinAtk = Convert.ToUInt16(data[15]);
                                NewItem.Defense = Convert.ToUInt32(data[16]);
                                NewItem.DexGives = Convert.ToByte(data[17]);
                                NewItem.Dodge = Convert.ToByte(data[18]);
                                NewItem.HPAdd = Convert.ToUInt16(data[19]);
                                NewItem.MPAdd = Convert.ToUInt16(data[20]);
                                NewItem.Durability = Convert.ToUInt16(data[22]);
                                NewItem.MagicAttack = Convert.ToUInt32(data[29]);
                                NewItem.MagicDefense = Convert.ToUInt32(data[30]);
                                if (ItemIDManipulation.Part(NewItem.ID, 0, 3) == 500)
                                    NewItem.Dist = Convert.ToByte(data[31]);
                                else //if (ItemIDManipulation.Part(NewItem.ID, 0, 3) >= 510 && ItemIDManipulation.Part(NewItem.ID, 0, 3) <= 580)
                                    NewItem.Dist = (byte)(Convert.ToByte(data[31]) * 2);
                                //data 32 - 2 hand or 1 hand
                                NewItem.CPsWorth = Convert.ToUInt32(data[36]);
                                DatabaseItems.TryAdd(NewItem.ID, NewItem);
                            }
                        }
                    }
                }
            }
        }
        public static Main.AuthWorker.AuthInfo Authenticate(string User, string Password)
        {
            Main.AuthWorker.AuthInfo Info = new Main.AuthWorker.AuthInfo();
            Info.Account = User;

            try
            {
                if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Username", User).ExecuteScalar() > 0)
                {
                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Username", User);
                    MySQL.MySqlReader Authentication = new MySQL.MySqlReader(Cmd);

                    while (Authentication.Read())
                    {
                        bool validPassword = Password == Authentication.ReadString("Password");
                        if (validPassword || validPassword)
                        {
                            int Status = Authentication.ReadInt32("Status");
                            Info.Status = "";
                            if (Status == 1)
                                Info.Status = "[GM]";
                            else if (Status == 3)
                                Info.Status = "[PM]";
                            else if (Status == 5)
                                Info.Status = "5";

                            Info.UID = Authentication.ReadUInt32("UID");
                            Info.Character = "";

                            if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", Info.UID).ExecuteScalar() > 0)
                            {
                                MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", Authentication.ReadUInt32("UID"));
                                MySQL.MySqlReader CharacterName = new MySQL.MySqlReader(Cmd2);

                                while (CharacterName.Read())
                                    Info.Character = CharacterName.ReadString("Name");
                            }

                            if (Info.Character.Length == 0)
                                Info.LogonType = 2;
                            else
                                Info.LogonType = 1;
                        }
                        else
                            Info.LogonType = 255;
                    }
                    return Info;
                }
                else
                    Info.LogonType = 255;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                World.ExcAdd += e + "\r\n";
            }
            return Info;

            //try
            //{
            //    while (File.Exists(World.GlobalAccountsPath + User + ".usr"))
            //    {
            //        byte[] buffer = File.ReadAllBytes(World.GlobalAccountsPath + User + ".usr");
            //        MemoryStream ms = new MemoryStream(buffer);
            //        BinaryReader BR = new BinaryReader(ms);
            //        string RealPassword = Main.PassCrypto.EncryptPassword(Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte())));
            //        string[] FileCaseSensitive = Directory.GetFiles(World.GlobalAccountsPath,User + ".usr");
            //        string RealAccount = Path.GetFileNameWithoutExtension(FileCaseSensitive[0]);
            //        if (RealPassword == Password && RealAccount == User)
            //        {
            //            Info.Status = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
            //            if (Info.Status !="[PM]" && Info.Status != "[GM]" && Info.Status !="[PH]")
            //                Info.Status = "";

            //            Info.Character = "";
            //            if (BR.BaseStream.Position != BR.BaseStream.Length)
            //            {
            //                byte len = BR.ReadByte();
            //                Info.Character = Encoding.ASCII.GetString(BR.ReadBytes(len));
            //            }

            //            if (Info.Character.Length == 0)
            //                Info.LogonType = 2;
            //            else
            //                Info.LogonType = 1;
            //        }
            //        else
            //            Info.LogonType = 255;
            //        BR.Close();
            //        ms.Close();

            //        return Info;
            //    }
            //    Info.LogonType = 255;
            //}
            //catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }
            //return Info;
        }
        public static void CreateAccount(string Name, string Password, string Status)
        {
            if (!File.Exists(World.GlobalAccountsPath + Name + ".usr"))
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter BW = new BinaryWriter(ms);
                BW.Write(Password);
                BW.Write(Status);
                byte[] buffer = ms.ToArray();
                BW.Close();
                ms.Close();
                File.WriteAllBytes(World.GlobalAccountsPath + Name + ".usr", buffer);
            }
        }
        public static void TopReset(object state)
        {
            Program.Reseting = true;
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C != null)
                        {
                            if (C.Top == 1 || C.Top == 2)
                            {
                                if (C.StatEff.Contains(StatusEffectEn.TopDeputyLeader))
                                    C.StatEff.Remove(StatusEffectEn.TopDeputyLeader);
                                else if (C.StatEff.Contains(StatusEffectEn.TopGuildLeader))
                                    C.StatEff.Remove(StatusEffectEn.TopGuildLeader);
                                C.Top = 0;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
                //System.Threading.Thread.Sleep(1);
            }
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void TopFBReset(object state)
        {
            Program.Reseting = true;
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        string Acc = "";
                        if (C != null)
                        {
                            if (C.TopFB == 1 || C.TopFB == 2)
                            {
                                if (C.StatEff.Contains(StatusEffectEn.TopFBSS))
                                    C.StatEff.Remove(StatusEffectEn.TopFBSS);
                                else if (C.StatEff.Contains(StatusEffectEn.Top3FBSS))
                                    C.StatEff.Remove(StatusEffectEn.Top3FBSS);
                                C.TopFB = 0;
                            }
                        }
                        else
                        {
                            Character C2 = LoadCharacter(Name, ref Acc);
                            if (C2 != null)
                            {
                                if (C2.TopFB != 0)
                                {
                                    C2.TopFB = 0;
                                    Database.SaveCharacter(C2, Acc);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
            }
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void AttributesReset(object state)
        {
            Console.WriteLine("Reseting attributes > 482....");
            Program.Reseting = true;
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C != null)
                        {
                            ushort TotalAtt = 0;
                            TotalAtt += C.StatPoints;
                            TotalAtt += C.Spi;
                            TotalAtt += C.Vit;
                            TotalAtt += C.Agi;
                            TotalAtt += C.Str;
                            TotalAtt -= 1;
                            if (TotalAtt > 482)
                            {
                                C.Spi = 0;
                                C.Vit = 1;
                                C.Agi = 0;
                                C.Str = 0;
                                C.StatPoints = 482;
                            }
                        }
                        else
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                ushort TotalAtt = 0;
                                TotalAtt += C.StatPoints;
                                TotalAtt += C.Spi;
                                TotalAtt += C.Vit;
                                TotalAtt += C.Agi;
                                TotalAtt += C.Str;
                                TotalAtt -= 1;
                                if (TotalAtt > 482)
                                {
                                    C.Spi = 0;
                                    C.Vit = 1;
                                    C.Agi = 0;
                                    C.Str = 0;
                                    C.StatPoints = 482;
                                    SaveCharacter(C, Account);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
                // System.Threading.Thread.Sleep(1);
            }
            Program.Reseting = false;
            Console.WriteLine("Attributes Reset");
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void GetMinus5Items(object state)
        {
            Program.Reseting = true;
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                ushort Plus8 = 0;
                                ushort Plus9 = 0;
                                foreach (Item I in C.Inventory)
                                {

                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }

                                }
                                foreach (Item I in C.Warehouses.TCWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse2)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.PCWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.ACWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.DCWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                foreach (Item I in C.Warehouses.BIWarehouse)
                                {
                                    if (I.Plus == 8 || I.Plus == 9)
                                    {
                                        if (I.Plus == 8)
                                            Plus8++;
                                        else Plus9++;
                                    }
                                }
                                if (Plus8 > 2 || Plus9 > 1)
                                    World.DebugAdd += C.Name + " had +8 items: " + Plus8 + " & +9 items: " + Plus9 + "\r\n";
                                SaveCharacter(C, Account);
                            }
                        }
                        else
                        {
                            ushort Plus8 = 0;
                            ushort Plus9 = 0;
                            foreach (Item I in C.Inventory)
                            {

                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }

                            }
                            foreach (Item I in C.Warehouses.TCWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse2)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.PCWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.ACWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.DCWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            foreach (Item I in C.Warehouses.BIWarehouse)
                            {
                                if (I.Plus == 8 || I.Plus == 9)
                                {
                                    if (I.Plus == 8)
                                        Plus8++;
                                    else Plus9++;
                                }
                            }
                            if (Plus8 > 2 || Plus9 > 1)
                                World.DebugAdd += C.Name + " had +8 items: " + Plus8 + " & +9 items: " + Plus9 + "\r\n";
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
                //System.Threading.Thread.Sleep(1);
            }
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort(state);
        }
        public static void Get2Sockets(object state)
        {
            Program.Reseting = true;
            ushort HeadGear1 = 0;
            ushort HeadGear2 = 0;
            ushort Neck1 = 0;
            ushort Neck2 = 0;
            ushort Ring1 = 0;
            ushort Ring2 = 0;
            ushort Armor2 = 0;
            ushort Boots1 = 0;
            ushort Boots2 = 0;
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                if (C.Equips.Armor.Soc2 != 0)
                                {
                                    Armor2++;
                                }
                                if (C.Equips.Boots.Soc1 != 0 || C.Equips.Boots.Soc2 != 0)
                                {
                                    if (C.Equips.Boots.Soc2 != 0)
                                        Boots2++;
                                    else Boots1++;
                                }
                                if (C.Equips.HeadGear.Soc1 != 0 || C.Equips.HeadGear.Soc2 != 0)
                                {
                                    if (C.Equips.HeadGear.Soc2 != 0)
                                        HeadGear2++;
                                    else HeadGear1++;
                                }
                                if (C.Equips.Necklace.Soc1 != 0 || C.Equips.Necklace.Soc2 != 0)
                                {
                                    if (C.Equips.Necklace.Soc2 != 0)
                                        Neck2++;
                                    else Neck1++;
                                }
                                if (C.Equips.Ring.Soc1 != 0 || C.Equips.Ring.Soc2 != 0)
                                {
                                    if (C.Equips.Ring.Soc2 != 0)
                                        Ring2++;
                                    else Ring1++;
                                }

                                foreach (Item I in C.Inventory)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }

                                }
                                foreach (Item I in C.Warehouses.TCWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse2)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.PCWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.ACWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.DCWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                foreach (Item I in C.Warehouses.BIWarehouse)
                                {
                                    if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                HeadGear2++;
                                            else HeadGear1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                    {
                                        if (I.Soc2 != 0)
                                            Armor2++;
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Neck2++;
                                            else Neck1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Ring2++;
                                            else Ring1++;
                                        }
                                    }
                                    else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                    {
                                        if (I.Soc1 != 0 || I.Soc2 != 0)
                                        {
                                            if (I.Soc2 != 0)
                                                Boots2++;
                                            else Boots1++;
                                        }
                                    }
                                }
                                SaveCharacter(C, Account);
                            }
                        }
                        else
                        {
                            if (C.Equips.Armor.Soc2 != 0)
                            {
                                Armor2++;
                            }
                            if (C.Equips.Boots.Soc1 != 0 || C.Equips.Boots.Soc2 != 0)
                            {
                                if (C.Equips.Boots.Soc2 != 0)
                                    Boots2++;
                                else Boots1++;
                            }
                            if (C.Equips.HeadGear.Soc1 != 0 || C.Equips.HeadGear.Soc2 != 0)
                            {
                                if (C.Equips.HeadGear.Soc2 != 0)
                                    HeadGear2++;
                                else HeadGear1++;
                            }
                            if (C.Equips.Necklace.Soc1 != 0 || C.Equips.Necklace.Soc2 != 0)
                            {
                                if (C.Equips.Necklace.Soc2 != 0)
                                    Neck2++;
                                else Neck1++;
                            }
                            if (C.Equips.Ring.Soc1 != 0 || C.Equips.Ring.Soc2 != 0)
                            {
                                if (C.Equips.Ring.Soc2 != 0)
                                    Ring2++;
                                else Ring1++;
                            }

                            foreach (Item I in C.Inventory)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.TCWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse2)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.PCWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.ACWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.DCWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                            foreach (Item I in C.Warehouses.BIWarehouse)
                            {
                                if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 118 || ItemIDManipulation.Part(I.ID, 0, 3) == 141 || ItemIDManipulation.Part(I.ID, 0, 3) == 142)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            HeadGear2++;
                                        else HeadGear1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 134)
                                {
                                    if (I.Soc2 != 0)
                                        Armor2++;
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 121)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Neck2++;
                                        else Neck1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 152)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Ring2++;
                                        else Ring1++;
                                    }
                                }
                                else if (Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 160)
                                {
                                    if (I.Soc1 != 0 || I.Soc2 != 0)
                                    {
                                        if (I.Soc2 != 0)
                                            Boots2++;
                                        else Boots1++;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
                //System.Threading.Thread.Sleep(1);
            }
            Console.WriteLine("-----1 Socket-----");
            Console.WriteLine("Headgears: " + HeadGear1);
            Console.WriteLine("Neck/Bags: " + Neck1);
            Console.WriteLine("Ring/Bracs: " + Ring1);
            Console.WriteLine("Boots: " + Boots1);
            Console.WriteLine("-----2 Socket-----");
            Console.WriteLine("Headgears: " + HeadGear2);
            Console.WriteLine("Neck/Bags: " + Neck2);
            Console.WriteLine("Ring/Bracs: " + Ring2);
            Console.WriteLine("Armors: " + Armor2);
            Console.WriteLine("Boots: " + Boots2);

            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort(state);
        }
        public static void GetMets(object state)
        {
            Program.Reseting = true;
            ulong Meteor = 0;
            ulong MeteorTear = 0;
            ulong Metscroll = 0;
            ulong MetscrollBag = 0;
            ulong MetPoints = 0;
            ulong VioletPoints = 0;
            ulong VirtuePoints = 0;
            ulong DragonPoints = 0;
            ulong PhoenixPoints = 0;
            ulong RainbowPoints = 0;
            ulong KylinPoints = 0;
            ulong FuryPoints = 0;
            ulong MoonPoints = 0;
            ulong TortoisePoints = 0;
            ulong DBPoints = 0;
            ulong UltimatePoints = 0;

            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                //720027 - Metscroll
                                //1088001 - Meteor
                                //1088002 - MeteorTear
                                //729912 - MetBag
                                if (DateTime.Now <= C.LastLogin.AddDays(30))
                                    continue;
                                VirtuePoints += C.VP;
                                DragonPoints += C.DragonGems;
                                PhoenixPoints += C.PhoenixGems;
                                RainbowPoints += C.RainbowGems;
                                KylinPoints += C.KylinGems;
                                FuryPoints += C.FuryGems;
                                VioletPoints += C.VioletGems;
                                MoonPoints += C.MoonGems;
                                TortoisePoints += C.TortoiseGems;
                                DBPoints += C.Dragonballs;
                                UltimatePoints += C.Ultimates;
                                MetPoints += C.MetScrolls;
                                foreach (Item I in C.Inventory)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.TCWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.MAWarehouse2)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.PCWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.ACWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.DCWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                foreach (Item I in C.Warehouses.BIWarehouse)
                                {
                                    if (I.ID == 1088001)
                                        Meteor++;
                                    else if (I.ID == 1088002)
                                        MeteorTear++;
                                    else if (I.ID == 720027)
                                        Metscroll++;
                                    else if (I.ID == 729912)
                                        MetscrollBag++;
                                }
                                SaveCharacter(C, Account);
                            }
                        }
                        else
                        {
                            foreach (Item I in C.Inventory)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.TCWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.MAWarehouse2)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.PCWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.ACWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.DCWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                            foreach (Item I in C.Warehouses.BIWarehouse)
                            {
                                if (I.ID == 1088001)
                                    Meteor++;
                                else if (I.ID == 1088002)
                                    MeteorTear++;
                                else if (I.ID == 720027)
                                    Metscroll++;
                                else if (I.ID == 729912)
                                    MetscrollBag++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
            }
            Console.WriteLine("-----Meteors-----");
            Console.WriteLine("Meteors: " + Meteor);
            Console.WriteLine("MeteorTears: " + MeteorTear);
            Console.WriteLine("MeteorScrolls: " + Metscroll);
            Console.WriteLine("MetScrollBags: " + MetscrollBag);
            Console.WriteLine("MetScrollPoints: " + MetPoints);
            Console.WriteLine("VirtuePoints: " + VirtuePoints);

            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort(state);
        }
        public static void UpdateChars(object state)
        {
            Console.WriteLine("Updating chars...");
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                SaveCharacter(C, Account);
                                Console.WriteLine(C.Name + " was updated successfully...");
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }
            Console.WriteLine("Finished updating chars.");

            Console.WriteLine("Updating banned chars...");
            string[] Paths2 = Directory.GetFiles(World.BannedChars);
            foreach (string Path in Paths2)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                SaveCharacter(C, Account);
                                Console.WriteLine("Banned character " + C.Name + " was updated successfully...");
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
            }
            Console.WriteLine("Finished updating banned chars.");

            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void UpdateBannedChars(object state)
        {
            Console.WriteLine("Updating banned chars...");
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.BannedChars);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                SaveCharacter(C, Account);
                                Console.WriteLine("Banned character " + C.Name + " was updated successfully...");
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }
            Console.WriteLine("Finished updating banned chars.");
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void CheckGoldOnPlayers(uint amount)
        {
            Console.WriteLine("Checking gold on chars...");
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            List<Character> Gold = new List<Character>();

            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                if (C.Silvers + C.WHSilvers >= amount)
                                {
                                    Gold.Add(C);
                                    /* World.DebugAdd += C.Name + " has silvers amount: " + (C.Silvers + C.WHSilvers) + "\r\n";
                                     if (C.TreasurePoints > 0)
                                         World.DebugAdd += C.Name + " has treasure points: " + C.TreasurePoints + "\r\n";
                                     if (C.Flowers > 0)
                                         World.DebugAdd += C.Name + " has flower points: " + C.Flowers + "\r\n";*/
                                }
                            }
                        }
                        else
                        {
                            if (C.Silvers + C.WHSilvers >= amount)
                                Gold.Add(C);
                            /* if (C.Silvers >= amount)
                                 World.DebugAdd += C.Name + " has silvers amount: " + C.Silvers + "\r\n"; */
                        }

                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }
            uint min; int pos_interchange;
            int n = Gold.Count;

            while (n > 0)
            {
                Character C2 = (Character)Gold[0];
                min = C2.Silvers + C2.WHSilvers;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)Gold[i];
                    if (C.Silvers + C.WHSilvers < min)
                    {
                        min = C.Silvers + C.WHSilvers;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                Gold[pos_interchange] = Gold[n - 1];
                Gold[n - 1] = C2;
                n--;
            }
            foreach (Character C in Gold)
            {
                World.GMChatAdd += C.Name + " has silvers amount: " + (C.Silvers + C.WHSilvers) + "\r\n";
            }
            Console.WriteLine("Finished checking gold on chars.");
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void LowerVote(object state)
        {
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                if (C.Voted)
                                {
                                    C.Voted = false;
                                    SaveCharacter(C, Account);
                                }
                            }
                        }
                        else
                        {
                            if (C.Voted)
                                C.Voted = false;
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }
            World.VotedIps = new List<string>();
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void RemoveAllNobility(object state)
        {
            Console.WriteLine("Reset nobility started!");
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            Program.Reseting = true;
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                if (C.Nobility.Donation >= 1)
                                {
                                    /* if (C.Nobility.Donation <= 150000000)
                                         if (C.Silvers + C.Nobility.Donation <= 2000000000)

                                             C.Silvers += (uint)C.Nobility.Donation;

                                         else World.DebugAdd += C.Name + " did not receive his nobility donation of : " + C.Nobility.Donation + "\r\n";
                                     else
                                     {
                                         if (C.Silvers + 150000000 <= 2000000000)
                                             C.Silvers += 150000000;
                                         else World.DebugAdd += C.Name + " did not receive his nobility donation of : " + 150000000 + "\r\n";
                                     }*/

                                    C.Nobility.Donation = 0;
                                    C.Nobility.ListPlace = -1;
                                    SaveCharacter(C, Account);
                                }
                            }
                        }
                        else
                        {
                            /* if (C.Nobility.Donation <= 150000000)
                                 if (C.Silvers + C.Nobility.Donation <= 2000000000)
                                     C.Silvers += (uint)C.Nobility.Donation;
                                 else World.DebugAdd += C.Name + " did not receive his nobility donation of : " + C.Nobility.Donation + "\r\n";
                             else
                             {
                                 if (C.Silvers + 150000000 <= 2000000000)
                                     C.Silvers += 150000000;
                                 else World.DebugAdd += C.Name + " did not receive his nobility donation of : " + 150000000 + "\r\n";
                             }*/

                            if (C.Nobility.Donation >= 1)
                            {
                                C.Nobility.Donation = 0;
                                C.Nobility.ListPlace = -1;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }
                }
                // System.Threading.Thread.Sleep(1);
            }
            Program.Reseting = false;
            World.EmpireBoard = new EmpireInfo[50];
            Console.WriteLine("Nobility reset finished");
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void Add7VIPDays(object state)
        {

            Console.WriteLine("Vip Bonus 1 Days started");
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            bool ToSave = false;
                            if (C != null)
                            {
                                if (C.VipLevel > 0 && C.VIPDays > 0)
                                {
                                    C.VIPDays += 1;
                                    ToSave = true;
                                }
                                if (ToSave)
                                    SaveCharacter(C, Account);
                            }
                        }
                        else
                        {
                            if (C.VipLevel > 0 && C.VIPDays > 0)
                            {
                                C.VIPDays += 1;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }

            Console.WriteLine("Vip Bonus 1 Days ended");
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void KillAllMonsters(object state)
        {
            foreach (ConcurrentDictionary<uint, Mob> H in World.H_Mobs.Values)
            {
                foreach (Mob M in H.Values)
                    if (M.Alive)
                    {
                        M.Died = DateTime.Now;
                        M.CurrentHP = 0;
                        M.Alive = false;
                        World.Action(M, Packets.Status(M.EntityID, Status.Effect, 2080).Get);
                        M.DropAnItem(0, 0);
                    }
                // System.Threading.Thread.Sleep(1);
            }

            Database.LoadMobs(true);
            System.Threading.Thread.CurrentThread.Abort();

        }
        public static void RemoveGarments(object state)
        {
            Console.WriteLine("Garment removal started!");
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            // bool ToSave = false;
                            if (C != null)
                            {
                                Item I = new Item();
                                I = C.Equips.Get(9);
                                if (I.ID != 0 && !Item.EquipPassSexReq(I, C))
                                {
                                    C.Equips.Garment = new Item();
                                    SaveCharacter(C, Account);

                                    World.InfoAdd += C.Name + " got his garment : " + I.ID + " removed!\r\n";
                                }
                            }
                        }
                        else
                        {
                            Item I = new Item();
                            I = C.Equips.Get(9);
                            if (I.ID != 0 && !Item.EquipPassSexReq(I, C))
                            {
                                C.EquipStats(9, false, false);
                                World.Spawn(C, false);
                                C.MyClient.AddSend(Packets.ItemPacket(I.UID, 9, 6));
                                C.Equips.Garment = new Item();
                                World.InfoAdd += C.Name + " got his garment : " + I.ID + " removed!\r\n";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }

            Console.WriteLine("Garment removal ended");
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void LowerVIPMuteBotVote(object state)
        {
            Program.Reseting = true;
            string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = LoadCharacter(Name, ref Account);
                            bool ToSave = false;
                            if (C != null)
                            {
                                if ((C.Top >= 3 && C.Top <= 7) || (DateTime.Now.DayOfWeek == DayOfWeek.Friday && (C.Top == 9 || C.Top == 1 || C.Top == 2)))
                                {
                                    C.Top = 0;
                                    ToSave = true;
                                }

                                /* if (DateTime.Now.Day == 1)
                                     if (C.Nobility.Donation >= 1)
                                     {

                                         C.Nobility.Donation = 0;
                                         C.Nobility.ListPlace = -1;
                                         ToSave = true;
                                     }*/
                                if (C.MutedDays > 0 || C.Muted)
                                {
                                    if (C.MutedDays > 0)
                                        C.MutedDays--;
                                    if (C.MutedDays <= 0)
                                    {
                                        //C.Muted = false;
                                        C.MutedDays = 0;
                                    }
                                    ToSave = true;
                                }
                                if (C.BOTJailedDays > 0 || C.BOTJailed)
                                {
                                    if (C.BOTJailedDays > 0)
                                        C.BOTJailedDays--;
                                    if (C.BOTJailedDays <= 0)
                                    {
                                        //C.BOTJailed = false;
                                        C.BOTJailedDays = 0;
                                    }
                                    ToSave = true;
                                }
                                if (C.Voted)
                                {
                                    C.Voted = false;
                                    ToSave = true;
                                }
                                if (!World.BanChars.Contains(C.Name))
                                {
                                    if (C.PKPoints >= 100)
                                        if (!World.TopPK.Contains(C))
                                            World.TopPK.Add(C);
                                    if (C.OnlineTime >= 50)
                                        if (!World.TopOnline.Contains(C))
                                            World.TopOnline.Add(C);
                                    if (C.Level >= 90 || C.Potency >= 190)
                                    {
                                        if (C.Job >= 10 && C.Job <= 15)
                                            World.TopTrojan.Add(C);
                                        else if (C.Job >= 20 && C.Job <= 25)
                                            World.TopWarrior.Add(C);
                                        else if (C.Job >= 40 && C.Job <= 45)
                                            World.TopArcher.Add(C);
                                        else if (C.Job >= 132 && C.Job <= 135)
                                            World.TopWaterTao.Add(C);
                                        else if (C.Job >= 142 && C.Job <= 145)
                                            World.TopFireTao.Add(C);
                                    }
                                    if (C.Silvers + C.WHSilvers >= 1000000)
                                    {
                                        World.TopGold.Add(C);
                                    }
                                    if (C.VP > 50000)
                                    {
                                        World.TopVPS.Add(C);
                                    }
                                }
                                if (ToSave)
                                    SaveCharacter(C, Account);
                            }
                        }
                        else
                        {
                            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                                C.Top = 0;

                            /*  if (DateTime.Now.Day == 1)
                                  if (C.Nobility.Donation >= 1)
                                  {
                                      C.Nobility.Donation = 0;
                                      C.Nobility.ListPlace = -1;
                                  }*/
                            if (C.MutedDays > 0 || C.Muted)
                            {
                                if (C.MutedDays > 0)
                                    C.MutedDays--;
                                if (C.MutedDays <= 0)
                                {
                                    C.MutedDays = 0;
                                    //C.Muted = false;
                                    C.MyClient.LocalMessage(2011, "You are free to talk now but don't insult others!!!");
                                }
                            }
                            if (C.BOTJailedDays > 0)
                            {
                                if (C.BOTJailedDays > 0)
                                    C.BOTJailedDays--;
                                if (C.BOTJailedDays <= 0)
                                {
                                    C.BOTJailedDays = 0;
                                    //C.BOTJailed = false;
                                    C.MyClient.LocalMessage(2011, "You are free now but don't use a bot ever again!!!");
                                }
                            }

                            if (C.Voted)
                                C.Voted = false;
                            if (!World.BanChars.Contains(C.Name) && C.MyClient.AuthInfo.Status != "[PM]")
                            {
                                if (C.PKPoints >= 100)
                                    if (!World.TopPK.Contains(C))
                                        World.TopPK.Add(C);
                                if (C.OnlineTime >= 50)
                                    if (!World.TopOnline.Contains(C))
                                        World.TopOnline.Add(C);
                                if (C.Level >= 90 || C.Potency >= 190)
                                {
                                    if (C.Job >= 10 && C.Job <= 15)
                                        World.TopTrojan.Add(C);
                                    else if (C.Job >= 20 && C.Job <= 25)
                                        World.TopWarrior.Add(C);
                                    else if (C.Job >= 40 && C.Job <= 45)
                                        World.TopArcher.Add(C);
                                    else if (C.Job >= 132 && C.Job <= 135)
                                        World.TopWaterTao.Add(C);
                                    else if (C.Job >= 142 && C.Job <= 145)
                                        World.TopFireTao.Add(C);
                                }
                                if (C.Silvers + C.WHSilvers >= 1000000)
                                {
                                    World.TopGold.Add(C);
                                }
                                if (C.VP > 50000)
                                {
                                    World.TopVPS.Add(C);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        World.ExcAdd += e.ToString() + "\r\n";
                    }

                }
                // System.Threading.Thread.Sleep(1);
            }
            //   if (DateTime.Now.Day == 1)
            //     World.EmpireBoard = new EmpireInfo[50];
            World.VotedIps = new List<string>();
            SortTops();
            Program.Reseting = false;
            System.Threading.Thread.CurrentThread.Abort();
        }
        public static void SortTops()
        {
            int n = World.TopVPS.Count;
            int pos_interchange;
            ulong min;// = ((Character)World.TopVPS[0]).VP;
            while (n > 0)
            {
                Character C2 = (Character)World.TopVPS[0];
                min = C2.VP;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopVPS[i];
                    if (C.VP < min)
                    {
                        min = C.VP;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopVPS[pos_interchange] = World.TopVPS[n - 1];
                World.TopVPS[n - 1] = C2;
                n--;
            }

            n = World.TopGold.Count;
            // pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopGold[0];
                min = C2.Silvers + C2.WHSilvers;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopGold[i];
                    if (C.Silvers + C.WHSilvers < min)
                    {
                        min = C.Silvers + C.WHSilvers;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopGold[pos_interchange] = World.TopGold[n - 1];
                World.TopGold[n - 1] = C2;
                n--;
            }

            n = World.TopArcher.Count;
            //  pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopArcher[0];
                min = C2.Potency;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopArcher[i];
                    if (C.Potency < min)
                    {
                        min = C.Potency;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopArcher[pos_interchange] = World.TopArcher[n - 1];
                World.TopArcher[n - 1] = C2;
                n--;
            }

            n = World.TopFireTao.Count;
            // pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopFireTao[0];
                min = C2.Potency;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopFireTao[i];
                    if (C.Potency < min)
                    {
                        min = C.Potency;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopFireTao[pos_interchange] = World.TopFireTao[n - 1];
                World.TopFireTao[n - 1] = C2;
                n--;
            }

            n = World.TopTrojan.Count;
            // pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopTrojan[0];
                min = C2.Potency;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopTrojan[i];
                    if (C.Potency < min)
                    {
                        min = C.Potency;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopTrojan[pos_interchange] = World.TopTrojan[n - 1];
                World.TopTrojan[n - 1] = C2;
                n--;
            }

            n = World.TopWarrior.Count;
            // pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopWarrior[0];
                min = C2.Potency;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopWarrior[i];
                    if (C.Potency < min)
                    {
                        min = C.Potency;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopWarrior[pos_interchange] = World.TopWarrior[n - 1];
                World.TopWarrior[n - 1] = C2;
                n--;
            }

            n = World.TopWaterTao.Count;
            //pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopWaterTao[0];
                min = C2.Potency;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopWaterTao[i];
                    if (C.Potency < min)
                    {
                        min = C.Potency;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopWaterTao[pos_interchange] = World.TopWaterTao[n - 1];
                World.TopWaterTao[n - 1] = C2;
                n--;
            }

            n = World.TopPK.Count;
            //  pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopPK[0];
                min = C2.PKPoints;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopPK[i];
                    if (C.PKPoints < min)
                    {
                        min = C.PKPoints;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopPK[pos_interchange] = World.TopPK[n - 1];
                World.TopPK[n - 1] = C2;
                n--;
            }
            n = World.TopOnline.Count;
            //  pos_interchange = 0;

            while (n > 0)
            {
                Character C2 = (Character)World.TopOnline[0];
                min = C2.OnlineTime;
                pos_interchange = 0;
                for (int i = 0; i < n; i++)
                {
                    Character C = (Character)World.TopOnline[i];
                    if (C.OnlineTime < min)
                    {
                        min = C.OnlineTime;
                        pos_interchange = i;
                        C2 = C;
                    }
                }
                World.TopOnline[pos_interchange] = World.TopOnline[n - 1];
                World.TopOnline[n - 1] = C2;
                n--;
            }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopKO.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopKONewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;

                List<string> Names = new List<string>();
                foreach (KOInfo KO in World.KOBoard)
                {
                    if (!Names.Contains(KO.Name))
                    {
                        Names.Add(KO.Name);
                        string PreText = i + ". " + KO.Name;
                        if (PreText.Length <= 7)
                            Text += PreText + " " + KO.KillCount.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                        else if (PreText.Length <= 15) Text += PreText + " " + KO.KillCount.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                        else Text += PreText + " " + KO.KillCount.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                        i++;
                    }
                }


                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopKO.ini was not written correctly!\r\n"; }

            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopVPS.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopVPSNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopVPS)
                {
                    if (i > 500)
                        break;
                    string PreText = i + ". " + C.Name;
                    if (PreText.Length <= 7)
                        Text += PreText + " " + C.VP.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                    else if (PreText.Length <= 15) Text += PreText + " " + C.VP.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                    else Text += PreText + " " + C.VP.ToString("#,#", System.Globalization.CultureInfo.InvariantCulture) + "\r\n";
                    i++;
                }
                World.TopVPS = new List<Character>();
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopVPS.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopGold.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopGoldNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopGold)
                {
                    if (i > 500)
                        break;
                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute(); i++;
                }
                World.TopGold = new List<Character>();
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopGold.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopPK.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopPKNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopPK)
                {
                    if (i > 500)
                        break;
                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute(); i++;
                }
                World.TopPK = new List<Character>();
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopPK.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopOnline.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopOnline.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopOnline)
                {
                    if (i > 500)
                        break;
                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute();
                    i++;
                }
                World.TopOnline = new List<Character>();
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopOnline.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopArch.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopArchNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopArcher)
                {
                    try
                    {
                        if (i == 1)
                        {
                            if (C != null)
                            {
                                if (C.MyClient == null)
                                {
                                    string acc = "";
                                    Character CC = LoadCharacter(C.Name, ref acc);
                                    if (CC != null)
                                    {
                                        CC.Top = 4;
                                        Database.SaveCharacter(CC, acc);
                                    }

                                }
                                else
                                {
                                    C.Top = 4;
                                }
                            }
                        }
                    }
                    catch { }
                    string PreText = i + ". " + C.Name;
                    string Nobility;
                    if (C.Nobility.Rank == Ranks.Duke)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Duke";
                        else
                            Nobility = "Duchess";
                    else if (C.Nobility.Rank == Ranks.Prince)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Prince";
                        else
                            Nobility = "Princess";
                    else if (C.Nobility.Rank == Ranks.King)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "King";
                        else
                            Nobility = "Queen";
                    else if (C.Nobility.Rank == Ranks.Knight)
                        Nobility = "Knight";
                    else if (C.Nobility.Rank == Ranks.Baron)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Baron";
                        else
                            Nobility = "Baroness";
                    else if (C.Nobility.Rank == Ranks.Earl)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Earl";
                        else
                            Nobility = "Countess";
                    else
                        Nobility = "Serf";

                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute(); i++;
                }
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopArch.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopTro.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopTroNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopTrojan)
                {
                    try
                    {
                        if (i == 1)
                        {
                            // Console.WriteLine(C.Name);
                            if (C != null)
                            {
                                if (C.MyClient == null)
                                {
                                    string acc = "";
                                    Character CC = LoadCharacter(C.Name, ref acc);
                                    if (CC != null)
                                    {
                                        CC.Top = 3;
                                        Database.SaveCharacter(CC, acc);
                                    }

                                }
                                else
                                {
                                    C.Top = 3;
                                }
                            }
                        }

                    }
                    catch { }
                    string PreText = i + ". " + C.Name;
                    string Nobility;
                    if (C.Nobility.Rank == Ranks.Duke)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Duke";
                        else
                            Nobility = "Duchess";
                    else if (C.Nobility.Rank == Ranks.Prince)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Prince";
                        else
                            Nobility = "Princess";
                    else if (C.Nobility.Rank == Ranks.King)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "King";
                        else
                            Nobility = "Queen";
                    else if (C.Nobility.Rank == Ranks.Knight)
                        Nobility = "Knight";
                    else if (C.Nobility.Rank == Ranks.Baron)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Baron";
                        else
                            Nobility = "Baroness";
                    else if (C.Nobility.Rank == Ranks.Earl)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Earl";
                        else
                            Nobility = "Countess";
                    else
                        Nobility = "Serf";

                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute(); i++;
                }
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopTro.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopWar.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopWarNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopWarrior)
                {
                    try
                    {
                        if (i == 1)
                        {
                            //Console.WriteLine(C.Name);
                            if (C != null)
                            {
                                if (C.MyClient == null)
                                {
                                    string acc = "";
                                    Character CC = LoadCharacter(C.Name, ref acc);
                                    if (CC != null)
                                    {
                                        CC.Top = 5;
                                        Database.SaveCharacter(CC, acc);
                                    }

                                }
                                else
                                {
                                    C.Top = 5;
                                }
                            }
                        }
                    }
                    catch { }
                    string PreText = i + ". " + C.Name;
                    string Nobility;
                    if (C.Nobility.Rank == Ranks.Duke)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Duke";
                        else
                            Nobility = "Duchess";
                    else if (C.Nobility.Rank == Ranks.Prince)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Prince";
                        else
                            Nobility = "Princess";
                    else if (C.Nobility.Rank == Ranks.King)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "King";
                        else
                            Nobility = "Queen";
                    else if (C.Nobility.Rank == Ranks.Knight)
                        Nobility = "Knight";
                    else if (C.Nobility.Rank == Ranks.Baron)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Baron";
                        else
                            Nobility = "Baroness";
                    else if (C.Nobility.Rank == Ranks.Earl)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Earl";
                        else
                            Nobility = "Countess";
                    else
                        Nobility = "Serf";

                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute(); i++;
                }
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopWar.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopWater.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopWaterNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopWaterTao)
                {
                    try
                    {
                        if (i == 1)
                        {
                            // Console.WriteLine(C.Name);
                            if (C != null)
                            {
                                if (C.MyClient == null)
                                {
                                    string acc = "";
                                    Character CC = LoadCharacter(C.Name, ref acc);
                                    if (CC != null)
                                    {
                                        CC.Top = 7;
                                        Database.SaveCharacter(CC, acc);
                                    }

                                }
                                else
                                {
                                    C.Top = 7;
                                }
                            }
                        }
                    }
                    catch { }
                    string PreText = i + ". " + C.Name;
                    string Nobility;
                    if (C.Nobility.Rank == Ranks.Duke)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Duke";
                        else
                            Nobility = "Duchess";
                    else if (C.Nobility.Rank == Ranks.Prince)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Prince";
                        else
                            Nobility = "Princess";
                    else if (C.Nobility.Rank == Ranks.King)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "King";
                        else
                            Nobility = "Queen";
                    else if (C.Nobility.Rank == Ranks.Knight)
                        Nobility = "Knight";
                    else if (C.Nobility.Rank == Ranks.Baron)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Baron";
                        else
                            Nobility = "Baroness";
                    else if (C.Nobility.Rank == Ranks.Earl)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Earl";
                        else
                            Nobility = "Countess";
                    else
                        Nobility = "Serf";

                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute();
                    i++;
                }
                System.IO.File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopWater.ini was not written correctly!\r\n"; }
            try
            {
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\TopFire.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\TopFireNewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                string Text = "";
                int i = 1;
                foreach (Character C in World.TopFireTao)
                {
                    try
                    {
                        if (i == 1)
                        {
                            Console.WriteLine(C.Name);
                            if (C != null)
                            {
                                if (C.MyClient == null)
                                {
                                    string acc = "";
                                    Character CC = LoadCharacter(C.Name, ref acc);
                                    if (CC != null)
                                    {
                                        CC.Top = 6;
                                        Database.SaveCharacter(CC, acc);
                                    }

                                }
                                else
                                {
                                    C.Top = 6;
                                }
                            }
                        }
                    }
                    catch { }

                    string PreText = i + ". " + C.Name;
                    string Nobility;
                    if (C.Nobility.Rank == Ranks.Duke)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Duke";
                        else
                            Nobility = "Duchess";
                    else if (C.Nobility.Rank == Ranks.Prince)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Prince";
                        else
                            Nobility = "Princess";
                    else if (C.Nobility.Rank == Ranks.King)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "King";
                        else
                            Nobility = "Queen";
                    else if (C.Nobility.Rank == Ranks.Knight)
                        Nobility = "Knight";
                    else if (C.Nobility.Rank == Ranks.Baron)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Baron";
                        else
                            Nobility = "Baroness";
                    else if (C.Nobility.Rank == Ranks.Earl)
                        if (C.Body == 1003 || C.Body == 1004)
                            Nobility = "Earl";
                        else
                            Nobility = "Countess";
                    else
                        Nobility = "Serf";

                    MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    Top.Update("CharStats").Set("Name", C.Name).Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Where("Name", C.Name).Execute();
                    i++;
                }

                File.WriteAllText(Path, Text);
            }
            catch { World.ExcAdd += "TopFire.ini was not written correctly!\r\n"; }
            try
            {
                string Top5 = "";

                Character C = (Character)World.TopArcher[0];
                Top5 += "Archer " + C.Name + " " + C.Level + " " + C.Potency + " " + C.Nobility.Rank + "\r\n";
                C = (Character)World.TopTrojan[0];
                Top5 += "Trojan " + C.Name + " " + C.Level + " " + C.Potency + " " + C.Nobility.Rank + "\r\n";
                C = (Character)World.TopWarrior[0];
                Top5 += "Warrior " + C.Name + " " + C.Level + " " + C.Potency + " " + C.Nobility.Rank + "\r\n";
                C = (Character)World.TopWaterTao[0];
                Top5 += "WaterTao " + C.Name + " " + C.Level + " " + C.Potency + " " + C.Nobility.Rank + "\r\n";
                C = (Character)World.TopFireTao[0];
                Top5 += "FireTao " + C.Name + " " + C.Level + " " + C.Potency + " " + C.Nobility.Rank;
                string Path = "";
                if (!World.LowRatedServer)
                    Path = @"C:\xampp\htdocs\leaderboard\Top5.ini";
                else Path = @"C:\xampp\htdocs\leaderboard\Top5NewServ.ini";
                if (!System.IO.File.Exists(Path))
                    System.IO.File.Create(Path).Close();
                else
                {
                    System.IO.File.Delete(Path);
                    System.IO.File.Create(Path).Close();
                }
                System.IO.File.WriteAllText(Path, Top5);
                World.TopArcher = new List<Character>();
                World.TopFireTao = new List<Character>();
                World.TopWarrior = new List<Character>();
                World.TopWaterTao = new List<Character>();
                World.TopTrojan = new List<Character>();
            }
            catch
            {
                World.TopArcher = new List<Character>();
                World.TopFireTao = new List<Character>();
                World.TopWarrior = new List<Character>();
                World.TopWaterTao = new List<Character>();
                World.TopTrojan = new List<Character>();
                World.ExcAdd += "Top5.ini was not written correctly!\r\n";
            }
        }
        public static Character LoadCharacterWithLogs(string Name, ref string Account, bool GenNewID = true, bool Load = false)
        {
            try
            {

                if (File.Exists(World.GlobalCharactersPath + Name + ".chr"))
                {
                    Character C = new Character();
                    byte[] buffer = File.ReadAllBytes(World.GlobalCharactersPath + Name + ".chr");
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryReader BR = new BinaryReader(ms);

                    C.Name = Name;
                    World.GMChatAdd += C.Name + " information:\r\n";
                    Account = BR.ReadString();
                    World.GMChatAdd += "Account : " + Account + "\r\n";
                    C.EntityID = BR.ReadUInt32();
                    C.Avatar = BR.ReadUInt16();
                    C.Body = BR.ReadUInt16();
                    C.Hair = BR.ReadUInt16();

                    C.Loc = new Location();
                    C.Loc.Map = BR.ReadUInt32();//here
                    C.Loc.X = BR.ReadUInt16();
                    C.Loc.Y = BR.ReadUInt16();
                    C.Loc.PreviousMap = BR.ReadUInt32();//here

                    C.Job = BR.ReadByte();
                    World.GMChatAdd += "Job : " + C.Job + "\r\n";
                    C.PreviousJob1 = BR.ReadByte();
                    World.GMChatAdd += "PreviousJob : " + C.PreviousJob1 + "\r\n";
                    C.Level = BR.ReadByte();
                    World.GMChatAdd += "Level : " + C.Level + "\r\n";
                    C.Experience = BR.ReadUInt64();

                    C.Str = BR.ReadUInt16();
                    C.Agi = BR.ReadUInt16();
                    C.Vit = BR.ReadUInt16();
                    C.Spi = BR.ReadUInt16();
                    C.StatPoints = BR.ReadUInt16();
                    C.CurHP = BR.ReadUInt16();
                    C.CurMP = BR.ReadUInt16();
                    C.Silvers = BR.ReadUInt32();
                    World.GMChatAdd += "Gold : " + C.Silvers + "\r\n";
                    C.WHSilvers = BR.ReadUInt32();
                    World.GMChatAdd += "WHGold : " + C.WHSilvers + "\r\n";
                    C.VP = BR.ReadUInt64();
                    World.GMChatAdd += "VP : " + C.VP + "\r\n";
                    //here
                    C.VipLevel = BR.ReadByte();
                    World.GMChatAdd += "VIPL : " + C.VipLevel + "\r\n";
                    C.VIPDays = BR.ReadByte();
                    World.GMChatAdd += "VIPD : " + C.VIPDays + "\r\n";
                    C.Reborns = BR.ReadByte();
                    World.GMChatAdd += "Reborns : " + C.Reborns + "\r\n";
                    C.DBScrolls = BR.ReadUInt16();//dbscrolls
                    World.GMChatAdd += "DBScrolls : " + C.DBScrolls + "\r\n";
                    C.VIPStarted = DateTime.FromBinary(BR.ReadInt64());
                    C.VIPLevelToReceive = BR.ReadByte();
                    C.VIPDaysToReceive = BR.ReadByte();
                    C.DoubleExp = BR.ReadBoolean();
                    C.DoubleExpLeft = BR.ReadInt32();
                    C.WHPassword = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                    C.PumpkinPoints = BR.ReadUInt16();//ushort
                    World.GMChatAdd += "PumpkinPts : " + C.PumpkinPoints + "\r\n";
                    C.CHat2011 = BR.ReadBoolean();//bool
                    C.TotalDemonBoxes = BR.ReadInt32();//int
                    World.GMChatAdd += "ChristmasPts : " + C.TotalDemonBoxes + "\r\n";
                    C.Flowers = BR.ReadUInt32();
                    World.GMChatAdd += "Flowers : " + C.Flowers + "\r\n";
                    C.TreasurePoints = BR.ReadUInt16();
                    World.GMChatAdd += "TreasurePts : " + C.TreasurePoints + "\r\n";
                    C.CTBPoints = BR.ReadUInt16();
                    World.GMChatAdd += "CTB Points : " + C.CTBPoints + "\r\n";
                    C.Warning = BR.ReadBoolean();
                    C.MetScrolls = BR.ReadByte();
                    World.GMChatAdd += "MetScrolls : " + C.MetScrolls + "\r\n";
                    C.DragonGems = BR.ReadByte();
                    World.GMChatAdd += "DragonGems : " + C.DragonGems + "\r\n";
                    C.PhoenixGems = BR.ReadByte();
                    World.GMChatAdd += "PhoenixGems : " + C.PhoenixGems + "\r\n";
                    C.RainbowGems = BR.ReadByte();
                    World.GMChatAdd += "RainbowGems : " + C.RainbowGems + "\r\n";
                    C.KylinGems = BR.ReadByte();
                    World.GMChatAdd += "KylinGems : " + C.KylinGems + "\r\n";
                    C.FuryGems = BR.ReadByte();
                    World.GMChatAdd += "FuryGems : " + C.FuryGems + "\r\n";
                    C.VioletGems = BR.ReadByte();
                    World.GMChatAdd += "VioletGems : " + C.VioletGems + "\r\n";
                    C.MoonGems = BR.ReadByte();
                    World.GMChatAdd += "MoonGems : " + C.MoonGems + "\r\n";
                    C.TortoiseGems = BR.ReadByte();
                    World.GMChatAdd += "TortoiseGems : " + C.TortoiseGems + "\r\n";
                    C.Dragonballs = BR.ReadByte();
                    World.GMChatAdd += "Dragonballs : " + C.Dragonballs + "\r\n";
                    C.Ultimates = BR.ReadByte();
                    World.GMChatAdd += "Ultimates : " + C.Ultimates + "\r\n";
                    C.GarmentToken = BR.ReadUInt16();//dbscrolls
                    World.GMChatAdd += "DBScrolls : " + C.GarmentToken + "\r\n";
                    //endhere

                    C.Nobility.Donation = BR.ReadUInt64();
                    World.GMChatAdd += "NobilityDon : " + C.Nobility.Donation + "\r\n";
                    C.Nobility.ListPlace = -1;
                    if (C.Nobility.Donation >= 3000000)
                    {
                        C.Nobility.ListPlace = 50;
                        for (int i = 49; i >= 0; i--)
                        {
                            if (C.Nobility.Donation >= World.EmpireBoard[i].Donation)
                                C.Nobility.ListPlace = i;
                            else break;
                        }
                        //if (C.Nobility.ListPlace < 50)
                        // {
                        if (C.Nobility.ListPlace >= 15 && C.Nobility.ListPlace <= 50)
                            C.Nobility.Rank = Ranks.Duke;
                        else if (C.Nobility.ListPlace >= 3 && C.Nobility.ListPlace <= 15)
                            C.Nobility.Rank = Ranks.Prince;
                        else if (C.Nobility.ListPlace < 3)
                            C.Nobility.Rank = Ranks.King;
                        else if (C.Nobility.Donation >= 30000000 && C.Nobility.Donation < 100000000)
                            C.Nobility.Rank = Ranks.Knight;
                        else if (C.Nobility.Donation >= 100000000 && C.Nobility.Donation < 200000000)
                            C.Nobility.Rank = Ranks.Baron;
                        else if (C.Nobility.Donation >= 200000000 && C.Nobility.Donation < 300000000)
                            C.Nobility.Rank = Ranks.Earl;
                        //}
                    }

                    C.CPs = BR.ReadUInt32();
                    C.PKPoints = BR.ReadUInt16();
                    World.GMChatAdd += "PKP : " + C.PKPoints + "\r\n";
                    ushort GID = BR.ReadUInt16();
                    if (Features.Guilds.AllTheGuilds.ContainsKey(GID))
                    {
                        C.MyGuild = (Features.Guild)Features.Guilds.AllTheGuilds[GID];

                        uint Don = BR.ReadUInt32(); ;
                        byte GR = BR.ReadByte();
                        if (C.EntityID != 0)
                            try
                            {
                                if ((C.MyGuild.Members[GR]).ContainsKey(C.EntityID))
                                {
                                    C.GuildDonation = Don;
                                    C.GuildRank = (Features.GuildRank)GR;

                                    C.MembInfo = (Features.MemberInfo)(C.MyGuild.Members[GR])[C.EntityID];
                                    C.MembInfo.Level = C.Level;
                                    C.GuildDonation = C.MembInfo.Donation;
                                    C.GuildRank = C.MembInfo.Rank;
                                }
                                else
                                    C.MyGuild = null;
                            }
                            catch (Exception E)
                            {
                                World.ExcAdd += E.ToString() + "\r\n";
                                C.GuildDonation = 0;
                                C.MyGuild = null;
                            }
                    }
                    else BR.ReadBytes(5);

                    C.Equips = new Equipment();
                    C.Equips.ReadThis(BR);
                    World.GMChatAdd += "Top gear: " + C.Equips.HeadGear.UID + "~" + C.Equips.HeadGear.ID + "~" + C.Equips.HeadGear.Plus + "~" + C.Equips.HeadGear.Bless + "~" + C.Equips.HeadGear.Enchant + "~" + (byte)C.Equips.HeadGear.Soc1 + "~" + (byte)C.Equips.HeadGear.Soc2 + "~" + C.Equips.HeadGear.Progress + "\r\n";
                    World.GMChatAdd += "Necklace: " + C.Equips.Necklace.UID + "~" + C.Equips.Necklace.ID + "~" + C.Equips.Necklace.Plus + "~" + C.Equips.Necklace.Bless + "~" + C.Equips.Necklace.Enchant + "~" + (byte)C.Equips.Necklace.Soc1 + "~" + (byte)C.Equips.Necklace.Soc2 + "~" + C.Equips.Necklace.Progress + "\r\n";
                    World.GMChatAdd += "Ring: " + C.Equips.Ring.UID + "~" + C.Equips.Ring.ID + "~" + C.Equips.Ring.Plus + "~" + C.Equips.Ring.Bless + "~" + C.Equips.Ring.Enchant + "~" + (byte)C.Equips.Ring.Soc1 + "~" + (byte)C.Equips.Ring.Soc2 + "~" + C.Equips.Ring.Progress + "\r\n";
                    World.GMChatAdd += "Right hand: " + C.Equips.RightHand.UID + "~" + C.Equips.RightHand.ID + "~" + C.Equips.RightHand.Plus + "~" + C.Equips.RightHand.Bless + "~" + C.Equips.RightHand.Enchant + "~" + (byte)C.Equips.RightHand.Soc1 + "~" + (byte)C.Equips.RightHand.Soc2 + "~" + C.Equips.RightHand.Progress + "\r\n";
                    World.GMChatAdd += "Left hand: " + C.Equips.LeftHand.UID + "~" + C.Equips.LeftHand.ID + "~" + C.Equips.LeftHand.Plus + "~" + C.Equips.LeftHand.Bless + "~" + C.Equips.LeftHand.Enchant + "~" + (byte)C.Equips.LeftHand.Soc1 + "~" + (byte)C.Equips.LeftHand.Soc2 + "~" + C.Equips.LeftHand.Progress + "\r\n";
                    World.GMChatAdd += "Armor: " + C.Equips.Armor.UID + "~" + C.Equips.Armor.ID + "~" + C.Equips.Armor.Plus + "~" + C.Equips.Armor.Bless + "~" + C.Equips.Armor.Enchant + "~" + (byte)C.Equips.Armor.Soc1 + "~" + (byte)C.Equips.Armor.Soc2 + "~" + C.Equips.Armor.Progress + "\r\n";
                    World.GMChatAdd += "Boots: " + C.Equips.Boots.UID + "~" + C.Equips.Boots.ID + "~" + C.Equips.Boots.Plus + "~" + C.Equips.Boots.Bless + "~" + C.Equips.Boots.Enchant + "~" + (byte)C.Equips.Boots.Soc1 + "~" + (byte)C.Equips.Boots.Soc2 + "~" + C.Equips.Boots.Progress + "\r\n";
                    World.GMChatAdd += "Garment: " + C.Equips.Garment.UID + "~" + C.Equips.Garment.ID + "\r\n";
                    C.Inventory = new List<Item>(40);
                    byte InventoryCount = BR.ReadByte();
                    World.GMChatAdd += "-Inventory-:\r\n";
                    for (byte i = 0; i < InventoryCount; i++)
                    {
                        Item I = new Item();
                        I.GenNewID = GenNewID;
                        I.ReadThis(BR);
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                        C.Inventory.Add(I);
                    }
                    C.Warehouses = new Banks();
                    C.Warehouses.ReadThis(BR);
                    World.GMChatAdd += "\r\nMA WH:";
                    foreach (Item I in C.Warehouses.MAWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nMA2 WH:";
                    foreach (Item I in C.Warehouses.MAWarehouse2)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nTC WH: ";
                    foreach (Item I in C.Warehouses.TCWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nPC WH: ";
                    foreach (Item I in C.Warehouses.PCWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nAC WH: ";
                    foreach (Item I in C.Warehouses.ACWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nDC WH: ";
                    foreach (Item I in C.Warehouses.DCWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    World.GMChatAdd += "\r\nBI WH: ";
                    foreach (Item I in C.Warehouses.BIWarehouse)
                        World.GMChatAdd += I.UID + "~" + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Enchant + "~" + (byte)I.Soc1 + "~" + (byte)I.Soc2 + "~" + I.Progress + "  ";
                    C.Skills = new ConcurrentDictionary<ushort, Skill>();
                    byte SkillCount = BR.ReadByte();
                    World.GMChatAdd += "\r\n-Skills-:\r\n";
                    for (byte i = 0; i < SkillCount; i++)
                    {
                        Skill S = new Skill();
                        S.ReadThis(BR);
                        if (!C.Skills.ContainsKey(S.ID))
                        {
                            World.GMChatAdd += S.ID + "~" + S.Lvl + "~" + S.Exp + "  ";
                            C.Skills.TryAdd(S.ID, S);
                            if (S.ID == 3060)
                                C.CanReflect = true;
                        }
                    }
                    World.GMChatAdd += "\r\n-Profs-:\r\n";
                    C.Profs = new ConcurrentDictionary<ushort, Prof>();
                    byte ProfCount = BR.ReadByte();
                    for (byte i = 0; i < ProfCount; i++)
                    {
                        Prof P = new Prof();
                        P.ReadThis(BR);
                        if (!C.Profs.ContainsKey(P.ID))
                        {
                            World.GMChatAdd += P.ID + "~" + P.Lvl + "~" + P.Exp + "  ";
                            C.Profs.TryAdd(P.ID, P);
                        }
                    }

                    C.Friends = new Dictionary<uint, Friend>();
                    byte FriendCount = BR.ReadByte();
                    for (byte i = 0; i < FriendCount; i++)
                    {
                        Friend F = new Friend();
                        F.ReadThis(BR);
                        if (!C.Friends.ContainsKey(F.UID))
                            C.Friends.Add(F.UID, F);
                    }

                    C.Enemies = new Dictionary<uint, Enemy>();
                    byte EnemyCount = BR.ReadByte();
                    for (byte i = 0; i < EnemyCount; i++)
                    {
                        Enemy E = new Enemy();
                        E.ReadThis(BR);
                        if (!C.Enemies.ContainsKey(E.UID))
                            C.Enemies.Add(E.UID, E);
                    }


                    // C.BlessingLasts = BR.ReadInt32();
                    // BR.ReadInt64();
                    // C.BlessingStarted = DateTime.FromBinary(BR.ReadInt64());
                    C.BlessingLasts = 0;
                    //C.BlessingStarted = DateTime.Now;
                    C.LuckyTime = 0;
                    C.ExpBallsUsedToday = 0;
                    C.Merchant = (MerchantTypes)(byte)255;//BR.ReadByte();//255
                    try
                    {
                        C.LastLogin = DateTime.FromBinary(BR.ReadInt64());
                    }
                    catch { C.LastLogin = DateTime.Now; }

                    C.LotteryUsed = 0;
                    C.TrainTimeLeft = 0;
                    C.InOTG = false;
                    try
                    {
                        C.Spouse = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                        if (!File.Exists(World.GlobalCharactersPath + C.Spouse + ".chr"))
                            C.Spouse = "None";
                    }
                    catch
                    {
                        C.Spouse = "None";
                    }
                    try
                    {
                        //C.UniversityPoints = BR.ReadUInt32();
                        C.UniversityPoints = 0;
                        C.Top = BR.ReadInt32();
                    }
                    catch
                    {
                        C.Top = 0;
                        // C.UniversityPoints = 0;
                    }
                    try
                    {
                        C.PreviousJob2 = BR.ReadByte();
                    }
                    catch
                    {
                        C.PreviousJob2 = 0;
                    }
                    try
                    {
                        BR.ReadBoolean();
                        //C.BOTJailed = BR.ReadBoolean();
                    }
                    catch
                    {
                        //C.BOTJailed = false;
                    }
                    try
                    {
                        C.Voted = BR.ReadBoolean();
                    }
                    catch
                    {
                        C.Voted = false;
                    }
                    try
                    {
                        C.PassiveSkills = BR.ReadBoolean();
                    }
                    catch
                    {
                        C.PassiveSkills = true;
                    }
                    try
                    {
                        C.BOTJailedDays = BR.ReadByte();
                    }
                    catch
                    {
                        C.BOTJailedDays = 0;
                    }
                    try
                    {
                        C.Muted = BR.ReadBoolean();
                    }
                    catch
                    {
                        C.Muted = false;
                    }
                    try
                    {
                        C.MutedDays = BR.ReadByte();
                    }
                    catch
                    {
                        C.MutedDays = 0;
                    }
                    C.ProfsBeforeReborn = new Dictionary<ushort, Prof>();
                    try
                    {
                        ProfCount = BR.ReadByte();
                    }
                    catch
                    {
                        ProfCount = 0;
                    }
                    for (byte i = 0; i < ProfCount; i++)
                    {
                        Prof P = new Prof();
                        P.ReadThis(BR);
                        if (!C.ProfsBeforeReborn.ContainsKey(P.ID))
                            C.ProfsBeforeReborn.Add(P.ID, P);
                    }
                    try
                    {
                        C.VotePoints = BR.ReadByte();
                        World.GMChatAdd += "\r\nVotePts: " + C.VotePoints + "\r\n";
                    }
                    catch
                    { C.VotePoints = 0; }
                    C.SkillsBeforeReborn = new Dictionary<ushort, Skill>();
                    try
                    {
                        SkillCount = BR.ReadByte();
                    }
                    catch { SkillCount = 0; }
                    for (byte i = 0; i < SkillCount; i++)
                    {
                        Skill S = new Skill();
                        S.ReadThis(BR);
                        if (!C.SkillsBeforeReborn.ContainsKey(S.ID))
                            C.SkillsBeforeReborn.Add(S.ID, S);
                    }

                    /*  try
                      {
                          C.TradeReverse = new Hashtable(20);
                          byte TradeRevCount = BR.ReadByte();
                          for (int i =0;i<TradeRevCount;i++)
                          {
                              Item I = new Item();
                              I.GenNewID = false;
                              I.ReadThis(BR);
                              C.TradeReverse.Add(I.UID, I);
                          }
                          C.GetRevertedItems = BR.ReadBoolean();
                          C.TradedGold = BR.ReadUInt32();
                      }
                      catch { Console.WriteLine("Error in TradeReversal stuff loading!"); }*/
                    //  ms.Flush();
                    //gump ------------
                    try
                    {
                        C.ClassicPoints = BR.ReadByte();
                    }
                    catch
                    {
                        C.ClassicPoints = 0;
                    }
                    World.GMChatAdd += "Classic Points: " + C.ClassicPoints + "\r\n";

                    World.GMChatAdd += "Loaded Quest data.\r\n";

                    BR.Close();
                    ms.Close();
                    C.Loaded = Load;

                    return C;
                }
                return null;

            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; World.ExcAdd += "Bugged Char: " + Name + " Acc: " + Account + "\r\n"; return null; }
        }
        public static void ChangeNames()
        {
            string[] Paths = Directory.GetFiles(@"C:\OldCODB\UsersNewServer\Characters\");
            foreach (string Path in Paths)
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                    ChangeCharacterNamePlusAcc(Name, "_" + Name);
                }
            }
        }
        public static string ChangeCharacterNamePlusAcc(string Name, string NewName)
        {//done now done hehe
            string CharsPath = @"C:\OldCODB\UsersNewServer\Characters\";
            string AccsPath = @"C:\OldCODB\UsersNewServer\";
            if (!File.Exists(CharsPath + NewName + ".chr"))
            {
                Character C = World.CharacterFromName(Name);
                if (C != null)
                {
                    C.MyClient.LocalMessage(2000, "In order to change your character name you have to be offline!");
                    return "Error Character Online";
                }
                else
                {
                    string Acc = "";
                    C = LoadCharacter(Name, ref Acc);
                    if (C != null)
                    {
                        C.Name = NewName;
                        if (C.MyGuild != null)
                            if (C.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                Features.MemberInfo M = C.MyGuild.MembOfName(Name);
                                C.MyGuild.Creator.MembName = NewName;
                                if (M != null)
                                {
                                    M.MembName = NewName;
                                    C.MyGuild.Creator = M;
                                    (C.MyGuild.Members[(byte)100]).Remove(M.MembID);
                                    (C.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                }
                            }
                        // File.Create(World.GlobalCharactersPath + NewName + ".chr");
                        File.Copy(CharsPath + Name + ".chr", CharsPath + NewName + ".chr");
                        SaveCharacter(C, "_" + Acc);
                        File.Delete(CharsPath + Name + ".chr");
                        MemoryStream ms = null;
                        BinaryWriter BW = null;
                        if (File.Exists(AccsPath + Acc + ".usr"))
                        {
                            byte[] buffer = File.ReadAllBytes(AccsPath + Acc + ".usr");
                            ms = new MemoryStream(buffer);
                            BinaryReader BR = new BinaryReader(ms);
                            string RealPassword = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                            string Status = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                            if (BR.BaseStream.Position != BR.BaseStream.Length)
                            {
                                byte len = BR.ReadByte();
                                Encoding.ASCII.GetString(BR.ReadBytes(len));
                            }

                            BR.Close();
                            ms.Close();
                            ms = new MemoryStream();
                            BW = new BinaryWriter(ms);
                            BW.Write(RealPassword);
                            BW.Write(Status);
                            BW.Write((byte)NewName.Length);
                            BW.Write(Encoding.ASCII.GetBytes(NewName));
                            buffer = ms.ToArray();
                            File.WriteAllBytes(AccsPath + "_" + Acc + ".usr", buffer);
                            File.Delete(AccsPath + Acc + ".usr");
                        }
                        if (File.Exists(AccsPath + Acc + ".txt"))
                        {
                            File.Copy(AccsPath + Acc + ".txt", AccsPath + "_" + Acc + ".txt");
                            File.Delete(AccsPath + Acc + ".txt");
                        }

                        if (BW != null)
                            BW.Close();
                        if (ms != null)
                            ms.Close();
                        return "OK";
                    }
                    else return "Error Character " + Name + " Does Not Exist";
                }
            }
            else return "Error New Name Character Exists";
            // return "Error";
        }
        public static Character LoadCharacter(string Name, ref string Account, bool GenNewID = true, bool Load = false)
        {
            try
            {
                if (File.Exists(World.GlobalCharactersPath + Name + ".chr"))
                {
                    Character C = new Character();
                    byte[] buffer = File.ReadAllBytes(World.GlobalCharactersPath + Name + ".chr");
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryReader BR = new BinaryReader(ms);
                    //C.LastLogin = DateTime.Now;

                    //63 bytes
                    C.Name = Name;//check
                    Account = BR.ReadString();//check - accounts table
                    C.Account = Account;//check
                    //BR.ReadBytes(63);
                    C.EntityID = BR.ReadUInt32();//check
                    C.Avatar = BR.ReadUInt16();//check
                    C.Body = BR.ReadUInt16();//check
                    C.Hair = BR.ReadUInt16();//check

                    C.Loc = new Location();
                    C.Loc.Map = BR.ReadUInt32();//here - check
                    C.Loc.X = BR.ReadUInt16();//check
                    C.Loc.Y = BR.ReadUInt16();//check
                    C.Loc.PreviousMap = BR.ReadUInt32();//here - check?

                    C.Job = BR.ReadByte();//check
                    C.PreviousJob1 = BR.ReadByte();//check
                    C.Level = BR.ReadByte();//check
                    C.Experience = BR.ReadUInt64();//check
                    

                    C.Str = BR.ReadUInt16();//check
                    C.Agi = BR.ReadUInt16();//check
                    C.Vit = BR.ReadUInt16();//check
                    C.Spi = BR.ReadUInt16();//check
                    C.StatPoints = BR.ReadUInt16();//check
                    C.CurHP = BR.ReadUInt16();//check
                    C.CurMP = BR.ReadUInt16();//check
                    C.Silvers = BR.ReadUInt32();//check
                    C.WHSilvers = BR.ReadUInt32();//check
                    C.VP = BR.ReadUInt64();//check
                    //here
                   
                    
                    C.VipLevel = BR.ReadByte();// change it to date time VIP variable only
                    C.VIPDays = BR.ReadByte();// change it to date time VIP variable only
                    C.Reborns = BR.ReadByte();// - get { Previous Reborn > 0 return 1}
                    C.DBScrolls = BR.ReadUInt16();//dbscrolls //check
                    C.VIPStarted = DateTime.FromBinary(BR.ReadInt64());// change it to date time VIP variable only
                    C.VIPLevelToReceive = BR.ReadByte();//check
                    C.VIPDaysToReceive = BR.ReadByte();//check
                    C.DoubleExp = BR.ReadBoolean();//get { DoubleExp Left > 0 return true }
                    C.DoubleExpLeft = BR.ReadInt32();//check
                    C.WHPassword = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));//check
                    C.PumpkinPoints = BR.ReadUInt16();//ushort //check
                    C.CHat2011 = BR.ReadBoolean();//bool  NOT NEEDED
                    C.TotalDemonBoxes = BR.ReadInt32();//int   NOT NEEDED
                    C.Flowers = BR.ReadUInt32();// NOT NEEDED
                    C.TreasurePoints = BR.ReadUInt16();//check
                    C.CTBPoints = BR.ReadUInt16();//check
                    C.Warning = BR.ReadBoolean();// NOT NEEDED
                    C.MetScrolls = BR.ReadByte();//check
                    C.DragonGems = BR.ReadByte();//check
                    C.PhoenixGems = BR.ReadByte();//check
                    C.RainbowGems = BR.ReadByte();//check
                    C.KylinGems = BR.ReadByte();//check
                    C.FuryGems = BR.ReadByte();//check
                    C.VioletGems = BR.ReadByte();//check
                    C.MoonGems = BR.ReadByte();//check
                    C.TortoiseGems = BR.ReadByte();//check
                    C.Dragonballs = BR.ReadByte();//check
                    C.Ultimates = BR.ReadByte();//check
                    C.GarmentToken = BR.ReadUInt16();//dbscrolls //check
                    C.TopFB = BR.ReadByte();// NEW TABLE WITH TOPS
                    C.OnlineTime = BR.ReadUInt32();//check
                    C.CurrentKills = BR.ReadUInt16();//check
                    C.VIP = DateTime.FromBinary(BR.ReadInt64());//check
                    C.Nobility.Donation = BR.ReadUInt64();//check
                    //endhere

                    C.Nobility.ListPlace = -1;
                    if (C.Nobility.Donation >= 3000000)
                    {
                        C.Nobility.ListPlace = 50;
                        for (int i = 49; i >= 0; i--)
                        {
                            if (C.Nobility.Donation > World.EmpireBoard[i].Donation)
                                C.Nobility.ListPlace = i;
                            else if (C.Nobility.Donation == World.EmpireBoard[i].Donation && C.Nobility.ListPlace <= 49)
                            {
                                //int c = string.Compare(C.Name, EmpireBoard[i].Name);
                                //c = C.Name.CompareTo(EmpireBoard[i].Name);
                                if (C.Name.CompareTo(World.EmpireBoard[i].Name) > 0)
                                    break;
                                else
                                    C.Nobility.ListPlace = i;
                            }
                            else
                                break;
                        }
                        if (C.Nobility.ListPlace < 50)
                        {
                            if (C.Nobility.ListPlace >= 15 && C.Nobility.ListPlace <= 50)
                                C.Nobility.Rank = Ranks.Duke;
                            else if (C.Nobility.ListPlace >= 3 && C.Nobility.ListPlace <= 15)
                                C.Nobility.Rank = Ranks.Prince;
                            else if (C.Nobility.ListPlace < 3)
                                C.Nobility.Rank = Ranks.King;
                        }
                        else if (C.Nobility.Donation >= 30000000 && C.Nobility.Donation <= 100000000)
                            C.Nobility.Rank = Ranks.Knight;
                        else if (C.Nobility.Donation >= 100000000 && C.Nobility.Donation <= 200000000)
                            C.Nobility.Rank = Ranks.Baron;
                        else if (C.Nobility.Donation >= 200000000 && C.Nobility.Donation <= 300000000)
                            C.Nobility.Rank = Ranks.Earl;
                    }

                    C.CPs = BR.ReadUInt32();//check
                    C.PKPoints = BR.ReadUInt16();//check

                    //ushort GID = BR.ReadUInt16();
                    //if (Features.Guilds.AllTheGuilds.ContainsKey(GID))
                    //{
                    //    C.MyGuild = (Features.Guild)Features.Guilds.AllTheGuilds[GID];

                    //    uint Don = BR.ReadUInt32(); ;
                    //    byte GR = BR.ReadByte();
                    //    if (C.EntityID != 0)
                    //        try
                    //        {
                    //            if ((C.MyGuild.Members[GR]).ContainsKey(C.EntityID))
                    //            {
                    //                C.GuildDonation = Don;
                    //                C.GuildRank = (Features.GuildRank)GR;

                    //                C.MembInfo = (Features.MemberInfo)(C.MyGuild.Members[GR])[C.EntityID];
                    //                C.MembInfo.Level = C.Level;
                    //                C.GuildDonation = C.MembInfo.Donation;
                    //                C.GuildRank = C.MembInfo.Rank;
                    //            }
                    //            else if (GR != 100)
                    //                C.MyGuild = null;
                    //        }
                    //        catch (Exception E)
                    //        {
                    //            World.ExcAdd += E.ToString() + "\r\n";
                    //            C.GuildDonation = 0;
                    //            C.MyGuild = null;
                    //        }
                    //}
                    //else
                    //    BR.ReadBytes(5);

                    BR.ReadBytes(7);

                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT);
                    Cmd.Select("characters").Where("UID", C.EntityID);
                    MySQL.MySqlReader Character = new MySQL.MySqlReader(Cmd);

                    ushort GuildID = 0;
                    while (Character.Read())
                    {
                        GuildID = Character.ReadUInt16("GuildID");
                        C.Version = Character.ReadUInt16("Version");
                        C.MutedRecord = Character.ReadUInt16("MutedRecord");
                    }
                    if (Features.Guilds.AllTheGuilds.ContainsKey(GuildID) && GuildID != 0)
                    {
                        C.MyGuild = Features.Guilds.AllTheGuilds[GuildID];
                        try
                        {
                            bool contains = false;
                            foreach (KeyValuePair<byte, Dictionary<uint, Features.MemberInfo>> List in C.MyGuild.Members)
                            {
                                if (List.Value.ContainsKey(C.EntityID))
                                {
                                    C.MembInfo = C.MyGuild.Members[List.Key][C.EntityID];
                                    C.MembInfo.Level = C.Level;
                                    C.GuildDonation = C.MembInfo.Donation;
                                    C.GuildRank = C.MembInfo.Rank;
                                    contains = true;
                                    break;
                                }
                            }
                            if (!contains)
                                C.MyGuild = null;
                        }
                        catch (Exception E)
                        {
                            World.ExcAdd += E.ToString() + "\r\n";
                            C.GuildDonation = 0;
                            C.MyGuild = null;
                        }
                    }

                    C.Equips = new Equipment();
                    C.Equips.ReadThis(BR);
                    C.Inventory = new List<Item>(40);
                    byte InventoryCount = BR.ReadByte();
                    for (byte i = 0; i < InventoryCount; i++)
                    {
                        Item I = new Item();
                        I.GenNewID = GenNewID;
                        I.ReadThis(BR);
                        if (I.ID != 0)
                            C.Inventory.Add(I);
                    }
                    C.Warehouses = new Banks();
                    C.Warehouses.ReadThis(BR);

                    C.Skills = new ConcurrentDictionary<ushort, Skill>();
                    byte SkillCount = BR.ReadByte();
                    for (byte i = 0; i < SkillCount; i++)
                    {
                        Skill S = new Skill();
                        S.ReadThis(BR);
                        if (!C.Skills.ContainsKey(S.ID))
                        {
                            C.Skills.TryAdd(S.ID, S);
                            if (S.ID == 3060)
                                C.CanReflect = true;
                        }
                    }

                    C.Profs = new ConcurrentDictionary<ushort, Prof>();
                    byte ProfCount = BR.ReadByte();
                    for (byte i = 0; i < ProfCount; i++)
                    {
                        Prof P = new Prof();
                        P.ReadThis(BR);
                        if (!C.Profs.ContainsKey(P.ID))
                            C.Profs.TryAdd(P.ID, P);
                    }
                    C.Friends = new Dictionary<uint, Friend>();
                    byte FriendCount = BR.ReadByte();
                    for (byte i = 0; i < FriendCount; i++)
                    {
                        Friend F = new Friend();
                        F.ReadThis(BR);
                        if (!C.Friends.ContainsKey(F.UID) && World.EIDS.Contains(F.UID))
                            C.Friends.Add(F.UID, F);
                    }
                    C.Enemies = new Dictionary<uint, Enemy>();
                    byte EnemyCount = BR.ReadByte();
                    for (byte i = 0; i < EnemyCount; i++)
                    {
                        Enemy E = new Enemy();
                        E.ReadThis(BR);
                        if (!C.Enemies.ContainsKey(E.UID) && World.EIDS.Contains(E.UID))
                            C.Enemies.Add(E.UID, E);
                    }
                    C.BlessingLasts = 0;
                    C.LuckyTime = 0;
                    C.ExpBallsUsedToday = 0;
                    C.Merchant = (MerchantTypes)(byte)255;//BR.ReadByte();//255
                    try
                    {
                        C.LastLogin = DateTime.FromBinary(BR.ReadInt64());
                    }
                    catch { C.LastLogin = DateTime.Now; }
                    C.LotteryUsed = 0;
                    C.TrainTimeLeft = 0;
                    C.InOTG = false;
                    try
                    {
                        C.Spouse = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                        if (!File.Exists(World.GlobalCharactersPath + C.Spouse + ".chr"))
                            C.Spouse = "None";
                    }
                    catch
                    {
                        C.Spouse = "None";
                    }
                    try
                    {
                        C.UniversityPoints = 0;
                        C.Top = BR.ReadInt32();
                    }
                    catch
                    {
                        C.Top = 0;
                    }
                    try
                    {
                        C.PreviousJob2 = BR.ReadByte();//check
                    }
                    catch
                    {
                        C.PreviousJob2 = 0;
                    }
                    try
                    {
                        BR.ReadBoolean();
                        //C.BOTJailed = BR.ReadBoolean();//get { return botjaileddays}
                    }
                    catch
                    {
                        //C.BOTJailed = false;
                    }
                    try
                    {
                        C.Voted = BR.ReadBoolean();//get { return last vote}
                    }
                    catch
                    {
                        C.Voted = false;
                    }
                    try
                    {
                        C.PassiveSkills = BR.ReadBoolean();
                    }
                    catch
                    {
                        C.PassiveSkills = true;
                    }
                    try
                    {
                        C.BOTJailedDays = BR.ReadByte();//check
                    }
                    catch
                    {
                        C.BOTJailedDays = 0;
                    }
                    try
                    {
                        BR.ReadBoolean();
                        //C.Muted = BR.ReadBoolean();//get { return muteddays}
                    }
                    catch
                    {
                        //C.Muted = false;
                    }
                    try
                    {
                        C.MutedDays = BR.ReadByte();
                    }
                    catch
                    {
                        C.MutedDays = 0;
                    }
                    C.ProfsBeforeReborn = new Dictionary<ushort, Prof>();
                    try
                    {
                        ProfCount = BR.ReadByte();
                    }
                    catch
                    {
                        ProfCount = 0;
                    }
                    for (byte i = 0; i < ProfCount; i++)
                    {
                        Prof P = new Prof();
                        P.ReadThis(BR);
                        if (!C.ProfsBeforeReborn.ContainsKey(P.ID))
                            C.ProfsBeforeReborn.Add(P.ID, P);
                    }
                    try
                    {
                        C.VotePoints = BR.ReadByte();
                    }
                    catch
                    { C.VotePoints = 0; }
                    C.SkillsBeforeReborn = new Dictionary<ushort, Skill>();
                    try
                    {
                        SkillCount = BR.ReadByte();
                    }
                    catch { SkillCount = 0; }
                    for (byte i = 0; i < SkillCount; i++)
                    {
                        Skill S = new Skill();
                        S.ReadThis(BR);
                        if (!C.SkillsBeforeReborn.ContainsKey(S.ID))
                            C.SkillsBeforeReborn.Add(S.ID, S);
                    }
                    try
                    {
                        C.ClassicPoints = BR.ReadByte();//check
                    }
                    catch
                    {
                        C.ClassicPoints = 0;
                    }


                    BR.Close();
                    ms.Close();
                    C.Loaded = Load;

                    return C;

                    //C.UniversityPoints = BR.ReadUInt32();
                    // C.BlessingLasts = BR.ReadInt32();
                    // BR.ReadInt64();
                    // C.BlessingStarted = DateTime.FromBinary(BR.ReadInt64());
                    //C.BlessingStarted = DateTime.Now;


                    /*  try
                      {
                          C.TradeReverse = new Hashtable(20);
                          byte TradeRevCount = BR.ReadByte();
                          for (int i =0;i<TradeRevCount;i++)
                          {
                              Item I = new Item();
                              I.GenNewID = false;
                              I.ReadThis(BR);
                              C.TradeReverse.Add(I.UID, I);
                          }
                          C.GetRevertedItems = BR.ReadBoolean();
                          C.TradedGold = BR.ReadUInt32();
                      }
                      catch { Console.WriteLine("Error in TradeReversal stuff loading!"); }*/
                    //  ms.Flush();

                    //gump----------
                }
                return null;

            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; World.ExcAdd += "Bugged Char: " + Name + " Acc: " + Account + "\r\n"; return null; }
        }
        public static Character LoadCharacter(string Name, bool Loaded)
        {
            try
            {

                Character C = new Character();
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("Name", Name);
                MySQL.MySqlReader Username = new MySQL.MySqlReader(Cmd);

                while (Username.Read())
                    C.EntityID = Username.ReadUInt32("UID");

                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("UID", C.EntityID);
                Username = new MySQL.MySqlReader(Cmd);

                while (Username.Read())
                    C.Account = Username.ReadString("Username");

                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", C.EntityID);
                MySQL.MySqlReader Character = new MySQL.MySqlReader(Cmd);

                ushort GuildID = 0;
                while (Character.Read())
                {
                    C.EntityID = Character.ReadUInt32("UID");
                    C.Name = Character.ReadString("Name");
                    C.Level = Character.ReadByte("Level");
                    C.Experience = Character.ReadUInt64("Experience");
                    C.Spouse = Character.ReadString("Spouse");
                    C.Body = Character.ReadUInt16("Body");
                    C.Avatar = Character.ReadUInt16("Face");
                    C.Hair = Character.ReadUInt16("Hair");
                    C.Silvers = Character.ReadUInt32("Silvers");
                    C.WHSilvers = Character.ReadUInt32("WHSilvers");
                    C.CPs = Character.ReadUInt32("CPs");
                    GuildID = Character.ReadUInt16("GuildID");
                    C.Version = Character.ReadUInt16("Version");
                    C.Loc = new Location()
                    {
                        Map = Character.ReadUInt32("Map"),
                        X = Character.ReadUInt16("X"),
                        Y = Character.ReadUInt16("Y")
                    };
                    C.Job = Character.ReadByte("Job");
                    C.PreviousJob1 = Character.ReadByte("PreviousJob1");
                    C.PreviousJob2 = Character.ReadByte("PreviousJob2");
                    C.Str = Character.ReadUInt16("Strength");
                    C.Agi = Character.ReadUInt16("Agility");
                    C.Vit = Character.ReadUInt16("Vitality");
                    C.Spi = Character.ReadUInt16("Spirit");
                    C.StatPoints = Character.ReadUInt16("ExtraStats");
                    C.CurHP = Character.ReadUInt16("Life");
                    C.CurMP = Character.ReadUInt16("Mana");
                    C.VP = Character.ReadUInt64("VirtuePoints");
                    C.DBScrolls = Character.ReadUInt16("DBScrolls");
                    C.VIPLevelToReceive = (byte)Character.ReadUInt16("VIPLevelToReceive");
                    C.VIPDaysToReceive = (byte)Character.ReadUInt16("VIPDaysToReceive");
                    C.DoubleExpLeft = Character.ReadInt32("DoubleExp");
                    C.WHPassword = Character.ReadString("WHPassword");
                    C.VIP = Character.ReadDatetime("VIP");
                    C.PumpkinPoints = Character.ReadUInt16("PumpkinPoints");
                    C.TreasurePoints = Character.ReadUInt16("TreasurePoints");
                    C.CTBPoints = Character.ReadUInt16("CTBPoints");
                    C.MetScrolls = (byte)Character.ReadUInt16("MetScrolls");
                    C.DragonGems = (byte)Character.ReadUInt16("DragonGems");
                    C.PhoenixGems = (byte)Character.ReadUInt16("PhoenixGems");
                    C.RainbowGems = (byte)Character.ReadUInt16("RainbowGems");
                    C.KylinGems = (byte)Character.ReadUInt16("KylinGems");
                    C.FuryGems = (byte)Character.ReadUInt16("FuryGems");
                    C.VioletGems = (byte)Character.ReadUInt16("VioletGems");
                    C.MoonGems = (byte)Character.ReadUInt16("MoonGems");
                    C.TortoiseGems = (byte)Character.ReadUInt16("TortoiseGems");
                    C.Dragonballs = (byte)Character.ReadUInt16("Dragonballs");
                    C.Ultimates = (byte)Character.ReadUInt16("Ultimates");
                    C.GarmentToken = Character.ReadUInt16("GarmentToken");
                    C.OnlineTime = Character.ReadUInt32("OnlineTime");
                    C.CurrentKills = Character.ReadUInt16("CurrentKills");
                    C.Nobility.Donation = Character.ReadUInt64("Nobility");
                    C.PKPoints = Character.ReadUInt16("PKPoints");
                    C.LastLogin = Character.ReadDatetime("LastLogin");
                    C.BOTJailedDays = Character.ReadByte("BotJailedDays");
                    C.MutedDays = Character.ReadByte("MutedDays");
                    C.VotePoints = Character.ReadByte("VotePoints");
                    C.ClassicPoints = Character.ReadInt32("ClassicPoints");
                    C.LastVote = Character.ReadDatetime("LastVote");

                    C.Loaded = Loaded;
                }

                //Guild
                if (Features.Guilds.AllTheGuilds.ContainsKey(GuildID) && GuildID != 0)
                {
                    C.MyGuild = Features.Guilds.AllTheGuilds[GuildID];
                    try
                    {
                        foreach (KeyValuePair<byte, Dictionary<uint, Features.MemberInfo>> List in C.MyGuild.Members)
                        {
                            if (List.Value.ContainsKey(C.EntityID))
                            {
                                C.MembInfo = C.MyGuild.Members[List.Key][C.EntityID];
                                C.MembInfo.Level = C.Level;
                                C.GuildDonation = C.MembInfo.Donation;
                                C.GuildRank = C.MembInfo.Rank;
                                break;
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        World.ExcAdd += E.ToString() + "\r\n";
                        C.GuildDonation = 0;
                        C.MyGuild = null;
                    }
                }

                C.Equips = new Equipment();
                C.Equips.Open();
                C.Warehouses = new Banks();
                C.Warehouses.Open();
                C.Inventory = new List<Item>(40);

                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("items").Where("Owner", C.EntityID);
                Character = new MySQL.MySqlReader(Cmd);
                while (Character.Read())
                {
                    Item I = new Item();
                    I.UID = Character.ReadUInt32("UID");
                    I.ID = Character.ReadUInt32("StaticID");
                    I.Plus = Character.ReadByte("Plus");
                    I.Bless = Character.ReadByte("Bless");
                    I.Enchant = Character.ReadByte("Enchant");
                    I.Soc1 = (Item.Gem)Character.ReadByte("Gem1");
                    I.Soc2 = (Item.Gem)Character.ReadByte("Gem2");
                    I.MaxDur = Character.ReadUInt16("MaxDura");
                    I.CurDur = Character.ReadUInt16("CurDura");
                    I.Color = (Item.ArmorColor)Character.ReadByte("Color");
                    I.Effect = (Item.RebornEffect)Character.ReadByte("Effect");
                    I.Progress = Character.ReadUInt16("Progress");
                    I.TalismanProgress = Character.ReadUInt32("TalismanProgress");
                    I.FreeItem = Character.ReadBoolean("FreeItem");
                    I.RestrainType = Character.ReadUInt32("RestrainType");
                    C.LoadItem(I, Character.ReadUInt16("Location"));
                }

                C.Skills = new ConcurrentDictionary<ushort, Skill>();
                C.SkillsBeforeReborn = new Dictionary<ushort, Skill>(); // need to delete this and add a "previous level" field to skill
                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("skills").Where("Owner", C.EntityID);
                Character = new MySQL.MySqlReader(Cmd);
                while (Character.Read())
                {
                    Skill S = new Skill();
                    S.ID = Character.ReadUInt16("ID");
                    S.Lvl = Character.ReadByte("Level");
                    S.Exp = Character.ReadUInt32("Experience");
                    S.PreviousLevel = Character.ReadByte("PreviousLevel");
                    if (!C.Skills.ContainsKey(S.ID))
                    {
                        C.Skills.TryAdd(S.ID, S);
                        if (S.ID == 3060)
                            C.CanReflect = true;
                    }

                    if (S.PreviousLevel > 0)
                    {
                        Skill S2 = S;
                        S2.Lvl = S.PreviousLevel;
                        S.Exp = 0;
                        C.SkillsBeforeReborn.Add(S.ID, S);
                    }
                }

                C.Profs = new ConcurrentDictionary<ushort, Prof>();
                C.ProfsBeforeReborn = new Dictionary<ushort, Prof>();
                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("proficiencies").Where("Owner", C.EntityID);
                Character = new MySQL.MySqlReader(Cmd);
                while (Character.Read())
                {
                    Prof S = new Prof();
                    S.ID = Character.ReadUInt16("ID");
                    S.Lvl = Character.ReadByte("Level");
                    S.Exp = Character.ReadUInt32("Experience");
                    S.PreviousLevel = Character.ReadByte("PreviousLevel");
                    if (!C.Profs.ContainsKey(S.ID))
                    {
                        C.Profs.TryAdd(S.ID, S);
                    }

                    if (S.PreviousLevel > 0)
                    {
                        Prof S2 = S;
                        S2.Lvl = S.PreviousLevel;
                        S.Exp = 0;
                        C.ProfsBeforeReborn.Add(S.ID, S);
                    }
                }

                C.Friends = new Dictionary<uint, Friend>();
                C.Enemies = new Dictionary<uint, Enemy>();
                Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("associates").Where("UID", C.EntityID);
                Character = new MySQL.MySqlReader(Cmd);
                while (Character.Read())
                {
                    byte Type = Character.ReadByte("Type");
                    if (Type == 0)
                    {
                        Enemy E = new Enemy();
                        E.UID = Character.ReadUInt32("AssociateID");
                        E.Name = "";
                        MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).COUNT("characters").Where("UID", E.UID);
                        MySQL.MySqlReader Associate = new MySQL.MySqlReader(Cmd2);
                        while (Associate.Read())
                            E.Name = Associate.ReadString("Name");

                        if (!C.Enemies.ContainsKey(E.UID) && !World.BannedChars.Contains(E.Name) && E.Name != "")
                            C.Enemies.Add(E.UID, E);
                    }
                    else
                    {
                        Friend F = new Friend();
                        F.UID = Character.ReadUInt32("AssociateID");

                        F.Name = "";
                        MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).COUNT("characters").Where("UID", F.UID);
                        MySQL.MySqlReader Associate = new MySQL.MySqlReader(Cmd2);
                        while (Associate.Read())
                            F.Name = Associate.ReadString("Name");

                        if (!C.Friends.ContainsKey(F.UID) && !World.BannedChars.Contains(F.Name) && F.Name != "")
                            C.Friends.Add(F.UID, F);
                    }
                }
                return C;
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\nBugged Char: " + Name + "\r\n";
                return null;
            }
        }

        public static void SaveCharacter(Character C, string Acc)
        {
            try
            {
                if (C != null && File.Exists(World.GlobalCharactersPath + C.Name + ".chr"))
                {
                    if (C.Saving)
                        return;
                    C.Saving = true;
                    MemoryStream FS = new MemoryStream();//ce deschide? nu deschide nimic :)), creaza un buffer iar bw o sa scrie in el, la sf copiaza bufferu si dupa ce inchide bw si ms, scrie cu File.Writeallbytes bufferu ca sa nu ai probleme cu alte procese :) ok mersi mult vezi daca merge :P ok
                    BinaryWriter BW = new BinaryWriter(FS);
                    int DoubleExp = C.DoubleExpLeft;
                    if (C.DoubleExp)
                        DoubleExp -= (int)(DateTime.Now - C.ExpPotionUsed).TotalSeconds;

                    BW.Write(Acc);
                    BW.Write(C.EntityID);
                    BW.Write(C.Avatar);
                    BW.Write(C.Body);
                    BW.Write(C.Hair);//Hair
                    BW.Write(C.Loc.Map);//Map
                    BW.Write(C.Loc.X);//X
                    BW.Write(C.Loc.Y);//Y
                    BW.Write(C.Loc.PreviousMap);//Previous Map
                    BW.Write(C.Job);
                    BW.Write(C.PreviousJob1);//Previous Job, 1st RB
                    BW.Write(C.Level);//Level
                    BW.Write(C.Experience);//Experience        
                    BW.Write(C.Str);
                    BW.Write(C.Agi);
                    BW.Write(C.Vit);
                    BW.Write(C.Spi);
                    BW.Write(C.StatPoints);//Stat Points
                    BW.Write(C.CurHP);
                    BW.Write(C.CurMP);//MP
                    BW.Write(C.Silvers);//Silvers
                    BW.Write(C.WHSilvers);//Warehouse Silvers
                    BW.Write(C.VP);//Virtue Points
                    //here-------
                    BW.Write(C.VipLevel);//
                    BW.Write(C.VIPDays);
                    BW.Write(C.Reborns);
                    BW.Write(C.DBScrolls);//ushort
                    BW.Write(C.VIPStarted.ToBinary());//long
                    BW.Write(C.VIPLevelToReceive);
                    BW.Write(C.VIPDaysToReceive);
                    BW.Write(C.DoubleExp);
                    BW.Write(DoubleExp);
                    BW.Write(C.WHPassword);
                    BW.Write(C.PumpkinPoints);//ushort
                    BW.Write(C.CHat2011);//bool
                    BW.Write(C.TotalDemonBoxes);//int
                    BW.Write(C.Flowers);
                    BW.Write(C.TreasurePoints);
                    BW.Write(C.CTBPoints);
                    BW.Write(C.Warning);
                    BW.Write(C.MetScrolls);
                    BW.Write(C.DragonGems);
                    BW.Write(C.PhoenixGems);
                    BW.Write(C.RainbowGems);
                    BW.Write(C.KylinGems);
                    BW.Write(C.FuryGems);
                    BW.Write(C.VioletGems);
                    BW.Write(C.MoonGems);
                    BW.Write(C.TortoiseGems);
                    BW.Write(C.Dragonballs);
                    BW.Write(C.Ultimates);
                    BW.Write(C.GarmentToken);//ushort
                    BW.Write(C.TopFB);
                    BW.Write(C.OnlineTime);
                    BW.Write(C.CurrentKills);//CurrentKills Cloudsaint
                    BW.Write(C.VIP.ToBinary());//long
                    //endhere-------
                    BW.Write(C.Nobility.Donation);
                    BW.Write(C.CPs);
                    BW.Write(C.PKPoints);//PK Points
                    if (C.MyGuild != null)
                    {
                        BW.Write(C.MyGuild.GuildID);//Guild
                        BW.Write(C.GuildDonation);//Guild Donation
                        BW.Write((byte)C.GuildRank);//Guild Rank
                    }
                    else BW.Write(new byte[7]);
                    C.Equips.WriteThis(BW);
                    BW.Write((byte)C.Inventory.Count);
                    List<Item> TempInv = C.Inventory;
                    foreach (Item I in TempInv)
                        I.WriteThis(BW);
                    C.Warehouses.WriteThis(BW);
                    BW.Write((byte)C.Skills.Count);
                    // Hashtable TempSkills = C.Skills;
                    foreach (Skill I in C.Skills.Values)
                        I.WriteThis(BW);
                    BW.Write((byte)C.Profs.Count);
                    //  Hashtable TempProfs = C.Profs;
                    foreach (Prof I in C.Profs.Values)
                        I.WriteThis(BW);
                    BW.Write((byte)C.Friends.Count);//C.Friends.Count
                    Dictionary<uint, Friend> TempFriends = C.Friends;
                    foreach (Friend I in TempFriends.Values)
                        I.WriteThis(BW);
                    BW.Write((byte)C.Enemies.Count);//C.Enemies.Count
                    Dictionary<uint, Enemy> TempEnemies = C.Enemies;
                    foreach (Enemy I in TempEnemies.Values)
                        I.WriteThis(BW);


                    //rem     BW.Write((int)0);//C.BlessingLasts
                    //rem     BW.Write((long)0);//C.BlessingStarted.Ticks
                    /*if (C.GettingLuckyTime)
                    {
                        if (!C.Prayer)
                            C.LuckyTime += (uint)(DateTime.Now - C.PrayDT).TotalSeconds;
                        else
                            C.LuckyTime += (uint)(DateTime.Now - C.PrayDT).TotalSeconds * 3;
                        C.PrayDT = DateTime.Now;
                    }*/
                    //rem       BW.Write((uint)0);//C.LuckyTime
                    //rem       BW.Write((byte)0);//C.ExpBallsUsedToday

                    //BW.Write((byte)C.Merchant);// 
                    //rem        BW.Write((byte)255);

                    BW.Write(C.LastLogin.ToBinary());//When logged off, for otg feature purposes  C.LastLogin (loading)
                                                     //rem        BW.Write((ushort)0);//Train time left (in minutes)
                                                     //rem        BW.Write(false);//C.InOTG
                                                     //rem        BW.Write((byte)0);//C.LotteryUsed

                    BW.Write(C.Spouse);
                    //rem         BW.Write((uint)0);//Quiz Pts    C.UniversityPoints 
                    BW.Write(C.Top);
                    BW.Write(C.PreviousJob2);

                    BW.Write(C.BOTJailed);
                    BW.Write(C.Voted);
                    BW.Write(C.PassiveSkills);
                    BW.Write(C.BOTJailedDays);
                    BW.Write(C.Muted);
                    BW.Write(C.MutedDays);
                    BW.Write((byte)C.ProfsBeforeReborn.Count);
                    foreach (Prof P in C.ProfsBeforeReborn.Values)
                        P.WriteThis(BW);
                    BW.Write(C.VotePoints);
                    BW.Write((byte)C.SkillsBeforeReborn.Count);
                    foreach (Skill S in C.SkillsBeforeReborn.Values)
                        S.WriteThis(BW);

                    /*  BW.Write((byte)C.TradeReverse.Count);
                      foreach (Item I in C.TradeReverse.Values)
                          I.WriteThis(BW);
                      BW.Write(C.GetRevertedItems);
                      BW.Write(C.TradedGold);*/

                    //gump----
                    BW.Write(C.ClassicPoints);

                    //end gump----
                    byte[] buffer = FS.ToArray();
                    // BW.Flush();
                    // FS.Flush();
                    BW.Close();
                    FS.Close();
                    try
                    {

                        File.WriteAllBytes(World.GlobalCharactersPath + C.Name + ".chr", buffer);//Gata:P woot stai asa
                    }
                    catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
                    C.Saving = false;

                    MySQL.MySqlCommand UpdateChar = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    UpdateChar.Update("characters").Set("Face", C.Avatar).Set("GuildID", C.GuildID).Set("Version", (ushort)Game.World._serverVersion).Set("MutedRecord", C.MutedRecord).Where("UID", C.EntityID).Execute();
                }
            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; C.Saving = false; }
        }
        public static string CreateCharacter(string Account, string Name, ushort Body, byte Job, uint UID = 0)
        {
            try
            {
                //if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Username", Account).ExecuteScalar() > 0 && !File.Exists(World.GlobalCharactersPath + Name + ".chr"))
                //{

                //}
                //if (File.Exists(World.GlobalAccountsPath + Account + ".usr") && !File.Exists(World.GlobalCharactersPath + Name + ".chr"))
                if (new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Username", Account).ExecuteScalar() > 0 && !File.Exists(World.GlobalCharactersPath + Name + ".chr"))
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        BinaryWriter BW = new BinaryWriter(ms);
                        BW.Write(Account);
                        //uint Eid = (uint)Program.Rnd.Next(1000001, 19999999);
                        //while (World.EIDS.Contains(Eid))
                        //{
                        //   Eid = (uint)Program.Rnd.Next(1000001, 19999999);
                        //}
                        //World.EIDS.Add(Eid);
                        //BW.Write(Eid);

                        if (!World.EIDS.Contains(UID))
                            World.EIDS.Add(UID);

                        BW.Write(UID);

                        if (Body == 1003 || Body == 1004)
                            BW.Write((ushort)1);//Avatar
                        else
                            BW.Write((ushort)201);//Avatar

                        BW.Write(Body);
                        BW.Write((ushort)(410 + (Program.Rnd.Next(5) * 100)));//Hair
                        BW.Write((uint)1010);//Map
                        BW.Write((ushort)61);//X
                        BW.Write((ushort)109);//Y
                        BW.Write((uint)0);//Previous Map
                        BW.Write(Job);
                        BW.Write((byte)0);//Previous Job, 1st RB
                        BW.Write((byte)1);//Level
                        BW.Write((ulong)0);//Experience

                        ushort Str = 0, Agi = 0, Vit = 0, Spi = 0;
                        GetInitialStats(Job, ref Str, ref Agi, ref Vit, ref Spi);

                        BW.Write(Str);
                        BW.Write(Agi);
                        BW.Write(Vit);
                        BW.Write(Spi);
                        BW.Write((ushort)0);//Stat Points
                        ushort HP = (ushort)(Vit * 24 + Str * 3 + Agi * 3 + Spi * 3);
                        BW.Write(HP);
                        BW.Write((ushort)(Spi * 5));//MP
                        BW.Write((uint)40000);//Silvers
                        BW.Write((uint)0);//Warehouse Silvers
                        BW.Write((ulong)0);//Virtue Points
                        //here
                        BW.Write((byte)0);//vip level
                        BW.Write((byte)0);//vip days left
                        BW.Write((byte)0);//reborns
                        BW.Write((ushort)0);//Prize NPC DBScrolls
                        BW.Write(DateTime.Now.ToBinary());//time vip started
                        BW.Write((byte)0);//vip level to get
                        BW.Write((byte)0);//vip days to get
                        BW.Write(false);
                        BW.Write((int)0);
                        BW.Write("0");
                        BW.Write((ushort)0);//pumpkin points halloween
                        BW.Write(false);//christmas hat received
                        BW.Write((int)0);//christmas points
                        BW.Write((uint)0);//flower points 
                        BW.Write((ushort)0);//treasure points
                        BW.Write((ushort)0);//ctb points
                        BW.Write(false);//warning
                        BW.Write((byte)0);//metscrolls stored
                        BW.Write((byte)0);//DragonGems stored
                        BW.Write((byte)0);//PhoenixGems stored
                        BW.Write((byte)0);//RainbowGems stored
                        BW.Write((byte)0);//KylinGems stored
                        BW.Write((byte)0);//FuryGems stored
                        BW.Write((byte)0);//VioletGems stored
                        BW.Write((byte)0);//MoonGems stored
                        BW.Write((byte)0);//TortoiseGems stored
                        BW.Write((byte)0);//Dragonballs stored
                        BW.Write((byte)0);//Ultimates stored
                        BW.Write((ushort)0);//Prize NPC GarmentToken
                        BW.Write((byte)0);//TopFB
                        BW.Write((uint)0);//TopOnline
                        BW.Write((ushort)0);//CurrentKills Cloudsaint
                        BW.Write(new DateTime(1970, 1, 1).ToBinary());//VIP Time
                        //endhere
                        BW.Write((ulong)0);//Donation
                        BW.Write((uint)0);//CPs
                        BW.Write((ushort)0);//PK Points
                        BW.Write((ushort)0);//Guild
                        BW.Write((uint)0);//Guild Donation
                        BW.Write((byte)0);//Guild Rank
                        Equipment Eq = new Equipment();
                        Eq.Open();
                        Eq.WriteThis(BW);
                        #region Beginner Items
                        if (Job == 100) // TAO
                        {
                            BW.Write((byte)12); // Inventory Count
                            Item I = new Item();
                            I.ID = 421301;
                            I.ID = 723753; // StarterPack
                            I.MaxDur = ((DatabaseItem)DatabaseItems[(uint)421301]).Durability;
                            I.CurDur = I.MaxDur;
                            I.UID = (uint)Program.Rnd.Next(10000000);
                            I.WriteThis(BW); // 1 item
                            I.ID = 1001000;
                            for (int i = 0; i < 5; i++)
                            {
                                I.UID = (uint)Program.Rnd.Next(10000000);
                                I.WriteThis(BW); // 5 items
                            }
                        }
                        else if (Job == 20) // WARR
                        {
                            BW.Write((byte)7); // Inventory Count
                            Item I = new Item();
                            I.ID = 410301;
                            I.ID = 723754;// StarterPack
                            I.MaxDur = ((DatabaseItem)DatabaseItems[(uint)410301]).Durability;
                            I.CurDur = I.MaxDur;
                            I.UID = (uint)Program.Rnd.Next(10000000);
                            I.WriteThis(BW); // 1 item
                        }
                        else if (Job == 10) // TRO
                        {
                            BW.Write((byte)7); // Inventory Count
                            Item I = new Item();
                            I.ID = 410301;
                            I.ID = 723755;// StarterPack
                            I.MaxDur = ((DatabaseItem)DatabaseItems[(uint)410301]).Durability;
                            I.CurDur = I.MaxDur;
                            I.UID = (uint)Program.Rnd.Next(10000000);
                            I.WriteThis(BW); // 1 item
                        }
                        else // ARCHER
                        {
                            BW.Write((byte)12); // Inventory Count
                            Item I = new Item();
                            I.ID = 500301;
                            I.ID = 723756;// StarterPack
                            I.MaxDur = ((DatabaseItem)DatabaseItems[(uint)500301]).Durability;
                            I.CurDur = I.MaxDur;
                            I.UID = (uint)Program.Rnd.Next(10000000);
                            I.WriteThis(BW); // 1 item
                            I.ID = 1050000;
                            I.MaxDur = ((DatabaseItem)DatabaseItems[(uint)1050000]).Durability;
                            I.CurDur = I.MaxDur;
                            for (int i = 0; i < 5; i++)
                            {
                                I.UID = (uint)Program.Rnd.Next(10000000);
                                I.WriteThis(BW); // 5 items
                            }
                        }
                        Item Armor = new Item();
                        Armor.ID = 132004;
                        Armor.Color = (Item.ArmorColor)(Program.Rnd.Next(3, 9));
                        Armor.MaxDur = ((DatabaseItem)DatabaseItems[(uint)132004]).Durability;
                        Armor.CurDur = Armor.MaxDur;
                        Armor.UID = (uint)Program.Rnd.Next(10000000);
                        Armor.WriteThis(BW); // 1 item
                        Item Stancher = new Item();
                        Stancher.ID = 1000000;
                        for (int i = 0; i < 5; i++) // 5 items
                        {
                            Stancher.UID = (uint)Program.Rnd.Next(10000000);
                            Stancher.WriteThis(BW);
                        }
                        #endregion
                        for (int n = 0; n < 9; n++)
                        {
                            BW.Write((byte)0);//WH[n] Count
                            //Warehouse[n]
                        }
                        BW.Write((byte)0);//WH[6] Count
                        //Warehouse[6]

                        BW.Write((byte)0);//Prof Count

                        if (Job != 100)
                            BW.Write((byte)0);//SkillCount
                        else
                        {
                            BW.Write((byte)2);//SkillCount
                            Skill S = new Skill() { ID = 1000 };
                            S.WriteThis(BW);
                            S = new Skill() { ID = 1005 };
                            S.WriteThis(BW);
                        }

                        BW.Write((byte)0);//Friend Count
                        BW.Write((byte)0);//Enemy Count


                        //   BW.Write(false);
                        //   BW.Write((int)0);
                        //   BW.Write((int)0);
                        //   BW.Write((long)0);
                        //   BW.Write((int)0);
                        //   BW.Write((byte)0);
                        //   BW.Write((byte)0);
                        //   BW.Write((byte)255);//Merchant
                        BW.Write(DateTime.Now.ToBinary());
                        //   BW.Write((ushort)0);
                        //   BW.Write(false);
                        //   BW.Write((byte)0);//lottery uses today
                        //   BW.Write("0");//WH Pass
                        BW.Write("None");//Spouse Name
                                         //    BW.Write((uint)0);//Quiz Pts     
                        BW.Write((int)0);//TopEffect
                        BW.Write((byte)0);//job before any rb
                        BW.Write(false);//botjailed
                        BW.Write(false);//voted
                        BW.Write(true);//passive skills
                        BW.Write((byte)0);//botjailed days
                        BW.Write(false);//muted?
                        BW.Write((byte)0);//muted days
                        BW.Write((byte)0);//prof count before reborning
                        BW.Write((byte)0);//vote points
                        BW.Write((byte)0);//skill count before rb

                        //  BW.Write((byte)0);//trade reverse items
                        //  BW.Write(false);//get reversed items

                        BW.Write((int)0);//Classic Points
                        BW.Write((byte)0);//BI_Quest active flag
                        BW.Write((int)0);//BI_Quest_Kills killed
                        BW.Write((byte)0);//BI_Quest_Completed count


                        byte[] buffer = ms.ToArray();
                        BW.Close();
                        ms.Close();
                        File.WriteAllBytes(World.GlobalCharactersPath + Name + ".chr", buffer);
                        try
                        {
                            MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY).Insert("characters").Insert("UID", UID).Insert("Name", Name);
                            Cmd.Execute();
                            MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY).Insert("CharStats").Insert("Name", Name);
                            Cmd2.Execute();
                        }
                        catch (Exception e)
                        {
                            World.ExcAdd += e + "\r\n";
                            Console.WriteLine(e);
                        }
                        //FileStream FS = new FileStream(World.GlobalAccountsPath + Account + ".usr", FileMode.Append);
                        //BW = new BinaryWriter(FS);
                        //BW.Write((byte)Name.Length);
                        //BW.Write(Encoding.ASCII.GetBytes(Name));

                        //BW.Close();
                        //FS.Close();
                        Character C = LoadCharacter(Name, ref Account);
                        if (C != null)
                        {
                            SaveCharacter(C, Account);
                        }
                    }
                    catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; return "Error! Try again."; }
                    return "ANSWER_OK";
                }
                return "Error: Character already exists!";

            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; return "Failed to create the character."; }
        }
        //public static void RetrieveAccsOfEmail(string email)
        //{
        //    string[] Paths = Directory.GetFiles(World.GlobalAccountsPath);
        //    string Accs = "Accounts on e-mail " + email + " :";
        //    foreach (string Path in Paths)
        //    {

        //        try
        //        {
        //            if (Path.Remove(0, Path.Length - 4) == ".txt")
        //            {
        //                string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                string TxtEmail = File.ReadAllText(World.GlobalAccountsPath + Name + ".txt");
        //                if (TxtEmail == email)
        //                    Accs += Name + "\r\n";
        //            }
        //        }
        //        catch (Exception E)
        //        {
        //            World.ExcAdd += E.ToString() + "\r\n";
        //        }
        //        // System.Threading.Thread.Sleep(1);
        //    }
        //    Program.WriteGMChatLine(Accs);
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
        //public static void DeleteUnusedAccounts(object state)
        //{
        //    string[] Paths = Directory.GetFiles(World.GlobalAccountsPath);
        //    foreach (string Path in Paths)
        //    {
        //        string Name = "";
        //        try
        //        {
        //            if (Path.Remove(0, Path.Length - 4) == ".usr")
        //            {
        //                Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                byte[] buffer = File.ReadAllBytes(World.GlobalAccountsPath + Name + ".usr");
        //                MemoryStream ms = new MemoryStream(buffer);
        //                BinaryReader BR = new BinaryReader(ms);
        //                Main.PassCrypto.EncryptPassword(Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte())));
        //                Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));

        //                string Character = "";
        //                if (BR.BaseStream.Position != BR.BaseStream.Length)
        //                {
        //                    byte len = BR.ReadByte();
        //                    Character = Encoding.ASCII.GetString(BR.ReadBytes(len));
        //                }
        //                BR.Close();
        //                ms.Close();
        //                if (Character.Length == 0)
        //                {
        //                    try
        //                    {

        //                        File.Delete(World.GlobalAccountsPath + Name + ".usr");
        //                        if (File.Exists(World.GlobalAccountsPath + Name + ".txt"))
        //                            File.Delete(World.GlobalAccountsPath + Name + ".txt");
        //                        Console.WriteLine(Name + " deleted!");
        //                    }
        //                    catch { Console.WriteLine(Name + " not deleted!"); }


        //                }


        //            }
        //        }
        //        catch (Exception E)
        //        {
        //            World.ExcAdd += E.ToString() + "\r\n";
        //            try
        //            {

        //                File.Delete(World.GlobalAccountsPath + Name + ".usr");
        //                if (File.Exists(World.GlobalAccountsPath + Name + ".txt"))
        //                    File.Delete(World.GlobalAccountsPath + Name + ".txt");
        //                Console.WriteLine(Name + " deleted (bugged acc)!");
        //            }
        //            catch { Console.WriteLine(Name + " not deleted!"); }
        //            Console.WriteLine(E.ToString());
        //        }
        //        // System.Threading.Thread.Sleep(1);
        //    }
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
        //public static void Fill_EntityID_And_Fix(object state)
        //{
        //    Program.Reseting = true;
        //    string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
        //    World.EIDS = new List<uint>();
        //    Console.WriteLine("Fill EntityIDs started");
        //    foreach (string Path in Paths)
        //    {
        //        if (Path.Remove(0, Path.Length - 4) == ".chr")
        //        {
        //            try
        //            {
        //                string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                Character C;
        //                C = World.CharacterFromName2(Name);
        //                bool save = false;
        //                if (C == null)
        //                {
        //                    string Account = "";
        //                    C = LoadCharacter(Name, ref Account);
        //                    if (C != null)
        //                    {
        //                        while (World.EIDS.Contains(C.EntityID))
        //                        {
        //                            C.EntityID = (uint)Program.Rnd.Next(1000001, 19999999);
        //                            save = true;
        //                        }
        //                        World.EIDS.Add(C.EntityID);
        //                        if (save)
        //                            SaveCharacter(C, Account);
        //                    }
        //                }
        //                else
        //                {
        //                    while (World.EIDS.Contains(C.EntityID))
        //                    {
        //                        C.EntityID = (uint)Program.Rnd.Next(1000001, 19999999);
        //                    }
        //                    World.EIDS.Add(C.EntityID);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                World.ExcAdd += e.ToString() + "\r\n";
        //            }

        //        }
        //        //   System.Threading.Thread.Sleep(1);
        //    }
        //    string EIds = "";
        //    foreach (uint I in World.EIDS)
        //        EIds += I + " ";
        //    EIds = EIds.Remove(EIds.Length - 1);
        //    if (!System.IO.File.Exists("entityids.txt"))
        //        System.IO.File.Create("entityids.txt").Close();
        //    else
        //    {
        //        System.IO.File.Delete("entityids.txt");
        //        System.IO.File.Create("entityids.txt").Close();
        //    }
        //    System.IO.File.WriteAllText("entityids.txt", EIds);
        //    Program.Reseting = false;
        //    Console.WriteLine("Fill EntityIDs ended!");
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
        /*public static Main.AuthWorker.AuthInfo Authenticate(string User, string Password)
        {
            Main.AuthWorker.AuthInfo Info = new Main.AuthWorker.AuthInfo();
            Info.Account = User;
            try
            {
                IniFile File = new IniFile("C:\\OldCODB\\Users\\" + User + ".usr");
                string RealAccount = File.ReadString("User", "account");
                if (User == RealAccount)
                {
                    string RealPassword = File.ReadString("User", "password");
                    RealPassword = Main.PassCrypto.EncryptPassword(RealPassword);
                    if (RealPassword == Password)
                    {
                        Info.Status = File.ReadString("User", "status");
                        Info.Character = File.ReadString("User", "character");
                        if (Info.Character == "")
                            Info.LogonType = 2;
                        else
                            Info.LogonType = 1;
                    }
                    else
                        Info.LogonType = 255;
                }
                else
                    Info.LogonType = 255;
                File.Close();
            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }
            return Info;
        }*/
        /* public static string ChangeCharacterName(string Name, string NewName)
         {
             if (!File.Exists(World.GlobalCharactersPath + NewName + ".chr"))
             {
                 Character C = World.CharacterFromName(Name);
                 if (C != null)
                 {
                     C.MyClient.LocalMessage(2000, "In order to change your character name you have to be offline!");
                     return "Error Character Online";
                 }
                 else
                 {
                     string Acc="";
                     C = LoadCharacter(Name,ref Acc);
                     if (C != null)
                     {
                         C.Name = NewName;
                         if (C.MyGuild != null)
                             if (C.GuildRank == Features.GuildRank.GuildLeader)
                             {
                                 Features.MemberInfo M = C.MyGuild.MembOfName(Name);
                                 C.MyGuild.Creator.MembName = NewName;
                                 if (M != null)
                                 {
                                     M.MembName = NewName;
                                     C.MyGuild.Creator = M;
                                     ((Hashtable)C.MyGuild.Members[(byte)100]).Remove(M.MembID);
                                     ((Hashtable)C.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                 }
                             }
                        // File.Create(World.GlobalCharactersPath + NewName + ".chr");
                         File.Copy(World.GlobalCharactersPath + Name + ".chr", World.GlobalCharactersPath + NewName + ".chr");
                         SaveCharacter(C, Acc);
                         File.Delete(World.GlobalCharactersPath + Name + ".chr");
                         byte[] buffer = File.ReadAllBytes(World.GlobalAccountsPath + Acc + ".usr");
                         MemoryStream ms = new MemoryStream(buffer);
                         BinaryReader BR = new BinaryReader(ms);
                         string RealPassword = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                         string Status = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                         string Character = "";
                         if (BR.BaseStream.Position != BR.BaseStream.Length)
                         {
                             byte len = BR.ReadByte();
                             Character = Encoding.ASCII.GetString(BR.ReadBytes(len));
                         }
                         BR.Close();
                         ms.Close();
                         ms = new MemoryStream();
                         BinaryWriter BW = new BinaryWriter(ms);
                         BW.Write(RealPassword);
                         BW.Write(Status);
                         BW.Write((byte)NewName.Length);
                         BW.Write(Encoding.ASCII.GetBytes(NewName));
                         buffer = ms.ToArray();
                         File.WriteAllBytes(World.GlobalAccountsPath + Acc + ".usr", buffer);

                         BW.Close();
                         ms.Close();
                         return "OK";
                     }
                     else return "Error Character " + Name + " Does Not Exist"; 
                 }
             }
             else return "Error New Name Character Exists";
            // return "Error";
         }*/
        //public static void CheckFlowersOnPlayers(uint amount)
        //{
        //    Console.WriteLine("Checking flowers on chars...");
        //    Program.Reseting = true;
        //    string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
        //    List<Character> Gold = new List<Character>();

        //    foreach (string Path in Paths)
        //    {
        //        if (Path.Remove(0, Path.Length - 4) == ".chr")
        //        {
        //            try
        //            {
        //                string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                Character C;
        //                C = World.CharacterFromName2(Name);
        //                if (C == null)
        //                {
        //                    string Account = "";
        //                    C = LoadCharacter(Name, ref Account);
        //                    if (C != null)
        //                    {
        //                        if (C.Flowers >= amount)
        //                        {
        //                            Gold.Add(C);
        //                            /* World.DebugAdd += C.Name + " has silvers amount: " + (C.Silvers + C.WHSilvers) + "\r\n";
        //                             if (C.TreasurePoints > 0)
        //                                 World.DebugAdd += C.Name + " has treasure points: " + C.TreasurePoints + "\r\n";
        //                             if (C.Flowers > 0)
        //                                 World.DebugAdd += C.Name + " has flower points: " + C.Flowers + "\r\n";*/
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (C.Flowers >= amount)
        //                        Gold.Add(C);
        //                    /* if (C.Silvers >= amount)
        //                         World.DebugAdd += C.Name + " has silvers amount: " + C.Silvers + "\r\n"; */
        //                }

        //            }
        //            catch (Exception e)
        //            {
        //                World.ExcAdd += e.ToString() + "\r\n";
        //            }

        //        }
        //        // System.Threading.Thread.Sleep(1);
        //    }
        //    uint min; int pos_interchange;
        //    int n = Gold.Count;

        //    while (n > 0)
        //    {
        //        Character C2 = (Character)Gold[0];
        //        min = C2.Flowers;
        //        pos_interchange = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            Character C = (Character)Gold[i];
        //            if (C.Flowers < min)
        //            {
        //                min = C.Flowers;
        //                pos_interchange = i;
        //                C2 = C;
        //            }
        //        }
        //        Gold[pos_interchange] = Gold[n - 1];
        //        Gold[n - 1] = C2;
        //        n--;
        //    }
        //    foreach (Character C in Gold)
        //    {

        //        World.GMChatAdd += C.Name + " has flower points: " + C.Flowers + "\r\n";
        //    }
        //    Console.WriteLine("Finished checking flowers on chars.");
        //    Program.Reseting = false;
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
        //public static void ResetGold(object state)
        //{
        //    Console.WriteLine("Reseting gold on chars...");
        //    Program.Reseting = true;
        //    string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
        //    foreach (string Path in Paths)
        //    {
        //        if (Path.Remove(0, Path.Length - 4) == ".chr")
        //        {
        //            try
        //            {
        //                string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                Character C;
        //                C = World.CharacterFromName2(Name);
        //                if (C == null)
        //                {
        //                    string Account = "";
        //                    C = LoadCharacter(Name, ref Account);
        //                    if (C != null)
        //                    {
        //                        C.Silvers = 10000;
        //                        C.WHSilvers = 0;
        //                        if (C.VipLevel <= 3)
        //                            C.VipLevel = 4;
        //                        C.VIPDays += 7;
        //                        C.VIPStarted = DateTime.Now;

        //                        SaveCharacter(C, Account);
        //                    }
        //                }

        //            }
        //            catch (Exception e)
        //            {
        //                World.ExcAdd += e.ToString() + "\r\n";
        //            }

        //        }
        //        // System.Threading.Thread.Sleep(1);
        //    }
        //    Console.WriteLine("Finished reseting gold on chars.");
        //    Program.Reseting = false;
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
        //public static void CheckTreasureOnPlayers(ushort amount)
        //{
        //    Console.WriteLine("Checking treasure points on chars...");
        //    Program.Reseting = true;
        //    string[] Paths = Directory.GetFiles(World.GlobalCharactersPath);
        //    List<Character> Gold = new List<Character>();

        //    foreach (string Path in Paths)
        //    {
        //        if (Path.Remove(0, Path.Length - 4) == ".chr")
        //        {
        //            try
        //            {
        //                string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
        //                Character C;
        //                C = World.CharacterFromName2(Name);
        //                if (C == null)
        //                {
        //                    string Account = "";
        //                    C = LoadCharacter(Name, ref Account);
        //                    if (C != null)
        //                    {
        //                        if (C.TreasurePoints >= amount)
        //                        {
        //                            Gold.Add(C);
        //                            /* World.DebugAdd += C.Name + " has silvers amount: " + (C.Silvers + C.WHSilvers) + "\r\n";
        //                             if (C.TreasurePoints > 0)
        //                                 World.DebugAdd += C.Name + " has treasure points: " + C.TreasurePoints + "\r\n";
        //                             if (C.Flowers > 0)
        //                                 World.DebugAdd += C.Name + " has flower points: " + C.Flowers + "\r\n";*/
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (C.TreasurePoints >= amount)
        //                        Gold.Add(C);
        //                    /* if (C.Silvers >= amount)
        //                         World.DebugAdd += C.Name + " has silvers amount: " + C.Silvers + "\r\n"; */
        //                }

        //            }
        //            catch (Exception e)
        //            {
        //                World.ExcAdd += e.ToString() + "\r\n";
        //            }

        //        }
        //        // System.Threading.Thread.Sleep(1);
        //    }
        //    uint min; int pos_interchange;
        //    int n = Gold.Count;

        //    while (n > 0)
        //    {
        //        Character C2 = (Character)Gold[0];
        //        min = C2.TreasurePoints;
        //        pos_interchange = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            Character C = (Character)Gold[i];
        //            if (C.TreasurePoints < min)
        //            {
        //                min = C.TreasurePoints;
        //                pos_interchange = i;
        //                C2 = C;
        //            }
        //        }
        //        Gold[pos_interchange] = Gold[n - 1];
        //        Gold[n - 1] = C2;
        //        n--;
        //    }
        //    foreach (Character C in Gold)
        //    {
        //        World.GMChatAdd += C.Name + " has treasure points: " + (C.TreasurePoints) + "\r\n";
        //    }
        //    Console.WriteLine("Finished checking treasure points on chars.");
        //    Program.Reseting = false;
        //    System.Threading.Thread.CurrentThread.Abort();
        //}
    }
    public class IniFile
    {
        public string path;
        public IniFile(string INIPath)
        {
            path = INIPath;
            if (File.Exists(path))
            {
                Read();
            }
        }
        public void Read()
        {
            #region IniSectionSelect
            string[] Lines = File.ReadAllLines(path);
            string Ssection = "";
            foreach (string Line in Lines)
            {
                if (Line.Length > 0)
                {
                    if (Line[0] == '[' && Line[Line.Length - 1] == ']')
                    {
                        Ssection = Line;
                        IniSectionStructure Section = new IniSectionStructure();
                        Section.SectionName = Ssection;
                        Section.Variables = new Dictionary<string, IniValueStructure>();
                        Sections.Add(Ssection, Section);
                    }
                    else if (Line[0] == '/' && Line[1] == '/')
                        continue;
                    else
                    {
                        IniValueStructure IvS = new IniValueStructure();
                        IvS.Variable = Line.Split('=')[0];
                        IvS.Value = Line.Split('=')[1];
                        IniSectionStructure Section = null;
                        Sections.TryGetValue(Ssection, out Section);
                        if (Section != null)
                        {
                            if (!Section.Variables.ContainsKey(IvS.Variable))
                                Section.Variables.Add(IvS.Variable, IvS);
                        }
                    }
                }
            }
            #endregion
        }
        Dictionary<string, IniSectionStructure> Sections = new Dictionary<string, IniSectionStructure>();
        public void Close()
        {
            Sections.Clear();
        }
        public void Save()
        {
            string Text = "";
            foreach (IniSectionStructure Section in Sections.Values)
            {
                Text += Section.SectionName + "\r\n";
                foreach (IniValueStructure IVS in Section.Variables.Values)
                {
                    Text += IVS.Variable + "=" + IVS.Value + "\r\n";
                }
            }
            if (File.Exists(path))
            {
                File.Delete(path);
                File.Create(path).Close();
                File.WriteAllText(path, Text);
            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, Text);
            }
        }
        class IniValueStructure
        {
            public string Variable;
            public string Value;
        }
        class IniSectionStructure
        {
            public Dictionary<string, IniValueStructure> Variables;
            public string SectionName;
        }
        private void IniWriteValue(string ssection, string Key, string Value)
        {
            string section = "[" + ssection + "]";
            IniSectionStructure _Section = null;
            Sections.TryGetValue(section, out _Section);
            if (_Section != null)
            {
                IniValueStructure IVS = null;
                _Section.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                {
                    if (IVS.Variable == Key)
                    {
                        IVS.Value = Value;
                    }
                }
                else
                {
                    _Section.Variables.Add(Key, new IniValueStructure() { Value = Value, Variable = Key });
                }
            }
            else
            {
                _Section = new IniSectionStructure() { SectionName = section, Variables = new Dictionary<string, IniValueStructure>() };
                Sections.Add(section, _Section);
                IniValueStructure IVS = null;
                _Section.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                {
                    if (IVS.Variable == Key)
                    {
                        IVS.Value = Value;
                    }
                }
                else
                {
                    _Section.Variables.Add(Key, new IniValueStructure() { Value = Value, Variable = Key });
                }
            }
        }

        #region Read
        public byte ReadByte(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return byte.Parse(IVS.Value);
            }
            return 0;
        }
        public sbyte ReadSbyte(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return sbyte.Parse(IVS.Value);
            }
            return 0;
        }
        public short ReadInt16(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return short.Parse(IVS.Value);
            }
            return 0;
        }
        public int ReadInt32(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return int.Parse(IVS.Value);
            }
            return 0;
        }
        public long ReadInt64(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return long.Parse(IVS.Value);
            }
            return 0;
        }
        public ushort ReadUInt16(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return ushort.Parse(IVS.Value);
            }
            return 0;
        }
        public uint ReadUInt32(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return uint.Parse(IVS.Value);
            }
            return 0;
        }
        public ulong ReadUInt64(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return ulong.Parse(IVS.Value);
            }
            return 0;
        }
        public double ReadDouble(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return double.Parse(IVS.Value);
            }
            return 0;
        }
        public float ReadFloat(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return float.Parse(IVS.Value);
            }
            return 0;
        }
        public string ReadString(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return IVS.Value;
            }
            return "";
        }
        public bool ReadBoolean(string Section, string Key)
        {
            string section = "[" + Section + "]";
            IniSectionStructure ISS = null;
            Sections.TryGetValue(section, out ISS);
            if (ISS != null)
            {
                IniValueStructure IVS = null;
                ISS.Variables.TryGetValue(Key, out IVS);
                if (IVS != null)
                    return byte.Parse(IVS.Value) == 1 ? true : false; ;
            }
            return false;
        }
        #endregion
        #region Write
        public void WriteString(string Section, string Key, string Value)
        {
            IniWriteValue(Section, Key, Value);
        }
        public void WriteInteger(string Section, string Key, byte Value)
        {
            IniWriteValue(Section, Key, Value.ToString());
        }
        public void WriteInteger(string Section, string Key, ulong Value)
        {
            IniWriteValue(Section, Key, Value.ToString());
        }
        public void WriteInteger(string Section, string Key, double Value)
        {
            IniWriteValue(Section, Key, Value.ToString());
        }
        public void WriteInteger(string Section, string Key, long Value)
        {
            IniWriteValue(Section, Key, Value.ToString());
        }
        public void WriteInteger(string Section, string Key, float Value)
        {
            IniWriteValue(Section, Key, Value.ToString());
        }
        public void WriteBoolean(string Section, string Key, bool Value)
        {
            IniWriteValue(Section, Key, (Value == true ? 1 : 0).ToString());
        }
        #endregion
    }
}