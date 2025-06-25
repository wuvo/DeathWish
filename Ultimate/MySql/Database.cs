using System;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using NewestCOServer.Game;
using System.Data;

namespace NewestCOServer
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
    }
    public struct Shop
    {
        public uint ShopID;
        public byte Type;
        public byte MoneyType;
        public byte ItemAmount;
        public ArrayList Items;
    }
    public struct SkillLearn
    {
        public ushort ID;
        public byte Lvl;
        public bool XP;
        public byte LevelReq;

        public Game.Skill ToSkill()
        {
            Game.Skill S = new Game.Skill();
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
        public byte DexGives;
        public uint CPsWorth;
        public ushort Durability;
        public ushort HPAdd;
        public ushort MPAdd;
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
        public static void CreareConnecxion(string Pass, string user, string Database)
        {
            mysqlUser = user;
            mysqlPassword = Pass;
            mysqlDatabase = Database;
        }
        
        public static string mysqlUser = "";
        public static string mysqlPassword = "";
        public static string mysqlDatabase = "";
    
        public static MySqlConnection MySqlConnection
        {
            get
            {
                return new MySqlConnection("Server=localhost;Database='" + mysqlDatabase + "';Username='" + mysqlUser + "';Password='" + mysqlPassword + "';");
            }
        }

        public static ushort[][] RevPoints;
        public static ushort[][] Portals;
        public static Hashtable DatabaseItems;
        public static Hashtable DatabasePlusItems;
        public static Hashtable Shops;
        public static uint[] ProfExp;
        public static ulong[] LevelExp;
        public  static int npcss =0;
        public  static int monst =0;
        public static Hashtable DefaultCoords = new Hashtable();
        public static Hashtable SkillForLearning = new Hashtable();
        public static ushort[] StonePts = new ushort[9] { 0, 10, 40, 120, 360, 1080, 3240, 9720, 29160 };
        public static ushort[] ComposePts = new ushort[13] { 20, 20, 80, 240, 720, 2160, 6480, 19440, 58320, 2700, 5500, 9000, 33000 };
        public static ushort[] SocPlusExtra = new ushort[9] { 6, 30, 70, 240, 740, 2240, 6670, 20000, 60000 };
        public static ArrayList GWOn = new ArrayList() { 0, 3, 6, 9, 12, 15, 18, 21 };
        public static Hashtable CompanionInfos = new Hashtable();
        private static Dictionary<byte, string> ArcherStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> NinjaStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> WarriorStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> TrojanStats = new Dictionary<byte, string>();
        private static Dictionary<byte, string> TaoistStats = new Dictionary<byte, string>();
        public static void Dispose()
        {
            RevPoints = null;
            Portals = null;
            DatabaseItems.Clear();
            DatabasePlusItems.Clear();
            Shops.Clear();
            ProfExp = null;
            LevelExp = null;
            DefaultCoords.Clear();
            SkillForLearning.Clear();
            StonePts = null;
            ComposePts = null;
            SocPlusExtra = null;
            GWOn.Clear();
            CompanionInfos.Clear();
            ArcherStats.Clear();
            NinjaStats.Clear();
            WarriorStats.Clear();
            TrojanStats.Clear();
            TaoistStats.Clear();
            Features.QuizShow.AllQuestions.Clear();
        }
        public static void LoadQuestions()
        {
            {
                string[] AllLines = System.IO.File.ReadAllLines("OldCODB\\QuizShow.txt");
                int questionscount = AllLines.Length;
                int quizcount = 0;
                for (int x = 0; x < questionscount; x++)
                {
                    string[] Info = AllLines[x].Split('#');
                    string Question = Info[0];
                    Features.QuizShow.Answer[] Answers = new ConquerSx.Features.QuizShow.Answer[4];
                    Answers[0] = new ConquerSx.Features.QuizShow.Answer(Info[1].Split(':')[0], ushort.Parse(Info[1].Split(':')[1]));
                    Answers[1] = new ConquerSx.Features.QuizShow.Answer(Info[2].Split(':')[0], ushort.Parse(Info[2].Split(':')[1]));
                    Answers[2] = new ConquerSx.Features.QuizShow.Answer(Info[3].Split(':')[0], ushort.Parse(Info[3].Split(':')[1]));
                    Answers[3] = new ConquerSx.Features.QuizShow.Answer(Info[4].Split(':')[0], ushort.Parse(Info[4].Split(':')[1]));
                    Features.QuizShow.Question Q = new ConquerSx.Features.QuizShow.Question(Question, Answers);
                    Features.QuizShow.AllQuestions.Add((ushort)x, Q);
                    quizcount++;
                }
                Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] QuizQuestion loading " + quizcount.ToString());
            }
        }
        public static void LoadCompanions()
        {
            if (File.Exists(@"OldCODB\Companions.txt"))
            {
                string[] Lines = File.ReadAllLines(@"OldCODB\Companions.txt");

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
                    CompanionInfos.Add(Type, C);
                }
            }
        }
        public static void AddSkills()
        {
            int skillcount = 0;
            ArrayList Warrior = new ArrayList();
            Warrior.Add(new SkillLearn() { ID = (ushort)1015, XP = true, LevelReq = (byte)15 }); skillcount++;
            Warrior.Add(new SkillLearn() { ID = (ushort)1020, XP = true, LevelReq = (byte)15 }); skillcount++;
            Warrior.Add(new SkillLearn() { ID = (ushort)1025, XP = true, LevelReq = (byte)3 }); skillcount++;
            Warrior.Add(new SkillLearn() { ID = (ushort)1040, XP = true, LevelReq = (byte)15 }); skillcount++;
            Warrior.Add(new SkillLearn() { ID = (ushort)1051, LevelReq = (byte)63 }); skillcount++;

            SkillForLearning.Add((byte)2, Warrior);

            ArrayList Trojan = new ArrayList();
            Trojan.Add(new SkillLearn() { ID = (ushort)1015, XP = true, LevelReq = (byte)15 }); skillcount++;
            Trojan.Add(new SkillLearn() { ID = (ushort)1110, XP = true, LevelReq = (byte)3 }); skillcount++;
            Trojan.Add(new SkillLearn() { ID = (ushort)1115, LevelReq = (byte)40 }); skillcount++;
            Trojan.Add(new SkillLearn() { ID = (ushort)1190, LevelReq = (byte)40 }); skillcount++;
            Trojan.Add(new SkillLearn() { ID = (ushort)1270, LevelReq = (byte)41 });
            Trojan.Add(new SkillLearn() { ID = (ushort)1270, LevelReq = (byte)41 }); skillcount++;
            SkillForLearning.Add((byte)1, Trojan);

            ArrayList Archer = new ArrayList();
            Archer.Add(new SkillLearn() { ID = (ushort)8002, XP = true, LevelReq = (byte)3 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)8001, LevelReq = (byte)23 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)8000, LevelReq = (byte)46 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)8003, LevelReq = (byte)70 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)8003, Lvl = 1, LevelReq = 70 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)8030, XP = true, LevelReq = (byte)70 }); skillcount++;
            Archer.Add(new SkillLearn() { ID = (ushort)9000, LevelReq = (byte)71 }); skillcount++;
            SkillForLearning.Add((byte)4, Archer);

            ArrayList Ninja = new ArrayList();
            Ninja.Add(new SkillLearn() { ID = (ushort)6011, LevelReq = (byte)15 }); skillcount++;
            Ninja.Add(new SkillLearn() { ID = (ushort)6000, XP = true, LevelReq = (byte)40 }); skillcount++;
            Ninja.Add(new SkillLearn() { ID = (ushort)6001, LevelReq = (byte)70 }); skillcount++;
            Ninja.Add(new SkillLearn() { ID = (ushort)6010, LevelReq = (byte)40 }); skillcount++;
            Ninja.Add(new SkillLearn() { ID = (ushort)6004, LevelReq = (byte)110 }); skillcount++;
            Ninja.Add(new SkillLearn() { ID = (ushort)6003, LevelReq = (byte)130 }); skillcount++;
            SkillForLearning.Add((byte)5, Ninja);


            ArrayList WaterTaoist = new ArrayList();
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1055, LevelReq = (byte)40 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1195, LevelReq = (byte)44 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1280, LevelReq = (byte)50 });
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1085, LevelReq = (byte)45 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1090, LevelReq = (byte)50 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1095, LevelReq = (byte)55 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1075, LevelReq = (byte)60 }); skillcount++;
            
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1100, LevelReq = (byte)70 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1175, LevelReq = (byte)81 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1170, LevelReq = (byte)94 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1050, XP = true, LevelReq = (byte)40 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1125, LevelReq = (byte)40 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)5001, LevelReq = (byte)70 }); skillcount++;
            WaterTaoist.Add(new SkillLearn() { ID = (ushort)1280, LevelReq = (byte)50 }); skillcount++;
            SkillForLearning.Add((byte)13, WaterTaoist);

            ArrayList FireTaoist = new ArrayList();
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1195, LevelReq = (byte)44 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1150, LevelReq = (byte)55 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1180, LevelReq = (byte)52 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1120, LevelReq = (byte)65 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)1125, LevelReq = (byte)40 }); skillcount++;
            FireTaoist.Add(new SkillLearn() { ID = (ushort)5001, LevelReq = (byte)70 }); skillcount++;
            SkillForLearning.Add((byte)14, FireTaoist);

            ArrayList Taoist = new ArrayList();
            Taoist.Add(new SkillLearn() { ID = (ushort)1010, LevelReq = (byte)15 }); skillcount++;
            SkillForLearning.Add((byte)10, Taoist);
            Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] Skills loading " + skillcount.ToString());

        }
        public static void SaveKOs()
        {
            FileStream FS = new FileStream(@"OldCODB\KOBoard.dat", FileMode.OpenOrCreate);
            BinaryWriter BW = new BinaryWriter(FS);

            for (int i = 0; i < Game.World.KOBoard.Length; i++)
                Game.World.KOBoard[i].WriteThis(BW);

            BW.Close();
            FS.Close();
        }
        public static void LoadKOs()
        {
            if (System.IO.File.Exists(@"OldCODB\KOBoard.dat"))
            {
                FileStream FS = new FileStream(@"OldCODB\KOBoard.dat", FileMode.Open);
                BinaryReader BR = new BinaryReader(FS);

                for (int i = 0; i < Game.World.KOBoard.Length; i++)
                    Game.World.KOBoard[i].ReadThis(BR);
                BR.Close();
                FS.Close();
            }
        }
        public static void SaveEmpire()
        {
            //    MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
           //     cmd1.Update("nobility").Set("Don", (long)don).Set
           //         ("Name",C.Name).Where("IdEntity", C.EntityID);
           //     cmd1.Execute();
            
            try
            {
                FileStream FS = new FileStream(@"OldCODB\Nobility.dat", FileMode.OpenOrCreate);
                BinaryWriter BW = new BinaryWriter(FS);

                for (int i = 0; i < Game.World.EmpireBoard.Length; i++)
                    Game.World.EmpireBoard[i].WriteThis(BW);

                BW.Close();
                FS.Close();
            }
            catch { }
        }
     /*  public static void teste()
        {
            try
            {
                File.WriteAllText(@"OldCODB\Nobility.dat", string.Empty);
                ((new FileInfo(@"OldCODB\Nobility.dat")).Open(FileMode.Truncate)).Close();
            }
            catch { }
        }*/

        public static void LoadEmpire()
        {

   //         MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
   //         cmd.Select("nobility");
   //         MySqlReader r = new MySqlReader(cmd);
   //         int i = 0;
   //         while (r.Read())
   //         {
   //            Game.World.EmpireBoard[i].Name = r.ReadString("Name");
   //            Game.World.EmpireBoard[i].Donation = r.ReadUInt64("Don");
   //             Game.World.EmpireBoard[i].ID = r.ReadUInt32("IdEntity");
   //             i++;
   //         }
   //         Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] Nobility loading " +i);


            try
            {
                if (System.IO.File.Exists(@"OldCODB\Nobility.dat"))
                {
                    FileStream FS = new FileStream(@"OldCODB\Nobility.dat", FileMode.Open);
                    BinaryReader BR = new BinaryReader(FS);
                    int nobiliticount = 0;

                    for (int i = 0; i < Game.World.EmpireBoard.Length; i++)
                        Game.World.EmpireBoard[i].ReadThis(BR);
                    nobiliticount++;
                    BR.Close();
                    FS.Close();
                    Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] Nobility loading " + nobiliticount.ToString());
                }
            }
            catch { }
        }
        public static void LoadShops()
        {
            Shops = new Hashtable();

            IniFile I = new IniFile(@"OldCODB\Shop.dat");
            int ShopAmount = I.ReadInt32("Header", "Amount");

            for (int i = 0; i < ShopAmount; i++)
            {
                Shop S = new Shop();
                S.ShopID = I.ReadUInt32("Shop" + i.ToString(), "ID");
                S.Type = I.ReadByte("Shop" + i.ToString(), "Type");
                S.MoneyType = I.ReadByte("Shop" + i.ToString(), "MoneyType");
                S.ItemAmount = I.ReadByte("Shop" + i.ToString(), "ItemAmount");
                S.Items = new ArrayList(S.ItemAmount);
                for (int e = 0; e < S.ItemAmount; e++)
                    S.Items.Add(I.ReadUInt32("Shop" + i.ToString(), "Item" + e.ToString()));

                Shops.Add(S.ShopID, S);
            }
            I.Close();
        }
        public static void LoadLevelExp()
        {
            LevelExp = new ulong[138];
            LevelExp[0] = 0;
            FileStream FS = new FileStream(@"OldCODB\ExpNeed.dat", FileMode.Open);
            BinaryReader BR = new BinaryReader(FS);
            for (byte i = 1; i < 130; i++)
                LevelExp[i] = BR.ReadUInt32();

            LevelExp[130] = 8589134588;
            LevelExp[131] = 25767403764;
            LevelExp[132] = 77302211292;
            LevelExp[133] = 231906633876;
            LevelExp[134] = 347859950814;
            LevelExp[135] = 347859950814;
            LevelExp[136] = 782684889332;
            LevelExp[137] = 1174027333998;
           // LevelExp[138] = 7826868000000;
           // LevelExp[139] = 78268080;
           // LevelExp[140] = 78268080;
           // LevelExp[141] = 78268080;
           // LevelExp[142] = 74120545;
           // LevelExp[143] = 41002500;
           // LevelExp[144] = 401052002;
           // LevelExp[145] = 52100220;
           // LevelExp[146] = 651515151;
           // LevelExp[147] = 125408452;
           // LevelExp[148] = 1548482030;
           // LevelExp[149] = 21578010;
           // LevelExp[150] = 485478101;
            BR.Close();
            FS.Close();
        }
        public static void LoadProfExp()
        {
            ProfExp = new uint[20];
            ProfExp[0] = 0;
            ProfExp[1] = 100;
            ProfExp[2] = 600;
            ProfExp[3] = 250;
            ProfExp[4] = 6400;
            ProfExp[5] = 1600;
            ProfExp[6] = 4000;
            ProfExp[7] = 1000;
            ProfExp[8] = 22000;
            ProfExp[9] = 4000;
            ProfExp[10] = 10000;
            ProfExp[11] = 95000;
            ProfExp[12] = 14000;
            ProfExp[13] = 21000;
            ProfExp[14] = 32500;
            ProfExp[15] = 48000;
            ProfExp[16] = 721250;
            ProfExp[17] = 108275;
            ProfExp[18] = 162363;
            ProfExp[19] = 210000;
        }
        public static void LoadRevPoints()
        {
            RevPoints = new ushort[28][];
            RevPoints[0] = new ushort[4] { 1002, 1002, 430, 380 };
            RevPoints[1] = new ushort[4] { 1005, 1005, 50, 50 };
            RevPoints[2] = new ushort[4] { 1006, 1002, 430, 380 };
            RevPoints[3] = new ushort[4] { 1008, 1002, 430, 380 };
            RevPoints[4] = new ushort[4] { 1009, 1002, 430, 380 };
            RevPoints[5] = new ushort[4] { 1010, 1002, 430, 380 };
            RevPoints[6] = new ushort[4] { 1007, 1002, 430, 380 };
            RevPoints[7] = new ushort[4] { 1004, 1002, 430, 380 };
            RevPoints[8] = new ushort[4] { 1028, 1002, 430, 380 };
            RevPoints[9] = new ushort[4] { 1037, 1002, 430, 380 };
            RevPoints[10] = new ushort[4] { 1038, 1002, 438, 398 };
            RevPoints[11] = new ushort[4] { 1015, 1015, 717, 577 };
            RevPoints[12] = new ushort[4] { 1001, 1000, 499, 650 };
            RevPoints[13] = new ushort[4] { 1000, 1000, 499, 650 };
            RevPoints[14] = new ushort[4] { 1013, 1011, 193, 266 };
            RevPoints[15] = new ushort[4] { 1011, 1011, 193, 266 };
            RevPoints[16] = new ushort[4] { 1076, 1011, 193, 266 };
            RevPoints[17] = new ushort[4] { 1014, 1011, 193, 266 };
            RevPoints[18] = new ushort[4] { 1020, 1020, 566, 562 };
            RevPoints[19] = new ushort[4] { 1075, 1020, 566, 656 };
            RevPoints[20] = new ushort[4] { 1012, 1020, 566, 656 };
            RevPoints[21] = new ushort[4] { 6000, 6000, 29, 73 };

            RevPoints[22] = new ushort[4] { 1730, 1002, 430, 380 };
            RevPoints[23] = new ushort[4] { 1731, 1002, 430, 380 };
            RevPoints[24] = new ushort[4] { 1732, 1002, 430, 380 };
            RevPoints[25] = new ushort[4] { 1733, 1002, 430, 380 };
            RevPoints[26] = new ushort[4] { 1734, 1002, 430, 380 };
            RevPoints[27] = new ushort[4] { 1735, 1002, 430, 380 };
        }
        public static void LoadPlusInfo()
        {
            string[] ItemAdd = File.ReadAllLines(@"OldCODB\ItemAdd.ini");
            DatabasePlusItems = new Hashtable();
            int countplus = 0;

            foreach (string S in ItemAdd)
            {
                DatabasePlusItem I = new DatabasePlusItem();
                I.ReadThis(S);
                DatabasePlusItems.Add(I.ID.ToString() + I.Plus.ToString(), I);
                countplus++;
            }
            Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] PlusInfo loading " + countplus.ToString());
        }
        public static void LoadFlowers(Game.Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("cq_flowers").Where("charuid", C.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {
                Struct.Flowers F = C.Flowers;
                F.Lilies = r.ReadInt16("lilies");
                F.Lilies2day = r.ReadInt16("liliestoday");
                F.Orchads = r.ReadInt16("orchads");
                F.Orchads2day = r.ReadInt16("orchadstoday");
                F.RedRoses = r.ReadInt16("redroses");
                F.RedRoses2day = r.ReadInt16("redrosestoday");
                F.Tulips = r.ReadInt16("tulips");
                F.Tulips2day = r.ReadInt16("tulipstoday");
                C.FlowerExist = true;
            }

        }
        public static void SaveFlowerRank(Game.Character C)
        {
            Struct.Flowers F = C.Flowers;
            if(C.FlowerExist)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("cq_flowers").Set("redroses", F.RedRoses).Set("redrosestoday", F.RedRoses2day).Set("lilies", F.Lilies).Set("liliestoday", F.Lilies2day).Set("tulips", F.Tulips).Set("tulipstoday", F.Tulips2day).Set("orchads", F.Orchads).Set("orchadstoday", F.Orchads2day).Where("charuid", C.EntityID).Execute();
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("cq_flowers").Insert("Names", C.Name).Insert("charuid", C.EntityID).Insert("redroses", F.RedRoses).Insert("redrosestoday", F.RedRoses2day).Insert("lilies", F.Lilies).Insert("liliestoday", F.Lilies2day).Insert("tulips", F.Tulips).Insert("tulipstoday", F.Tulips2day).Insert("orchads", F.Orchads).Insert("orchadstoday", F.Orchads2day).Execute();
            }
        }
       /* public static void Loadconfig()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("config");
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {
                Game.World.Server.ExperienceRate = r.ReadUInt32("ExpRate");
                Game.World.Server.ProfExpRate = r.ReadUInt32("ExpProf");
                Game.World.Server.SkillExpRate = r.ReadUInt32("ExpSkill");
                Game.World.Server.WebSite = r.ReadString("WebSite");
                Game.World.Server.ServerName = r.ReadString("ServerName");
     
            }
        }
          public static void LoadNPCs()// working
          {
              lock (WriteConnection)
              {
                  MySqlCommand Cmd = new MySqlCommand("SELECT * FROM `npc`", WriteConnection);

                  MySqlDataReader DR = Cmd.ExecuteReader();
                  while (DR.Read())
                  {
                      Game.NPC Npc = new ConquerSx.Game.NPC();
                      Npc.EntityID = Convert.ToUInt32(DR["EntityID"]);
                      Npc.Type = Convert.ToUInt16(DR["Type"]);
                      Npc.Flags = Convert.ToByte(DR["Flags"]);
                      Npc.Avatar = Convert.ToByte(DR["Avatar"]);
                      Npc.Loc = new Game.Location();
                      Npc.Loc.Map = Convert.ToUInt16(DR["Map"]);
                      Npc.Loc.X = Convert.ToUInt16(DR["X"]);
                      Npc.Loc.Y = Convert.ToUInt16(DR["Y"]);



                      if (Npc.Flags == 21)
                          Npc.Level = (byte)((Npc.Type - 427) / 6 + 20);
                      if (Npc.Flags == 22)
                          Npc.Level = (byte)((Npc.Type - 437) / 6 + 20);
                      if (Npc.Type == 1500)
                          Npc.Level = 125;
                      if (Npc.Type == 1520)
                          Npc.Level = 125;

                      if (Npc.Flags == 21 || Npc.Flags == 22)
                      {
                          Npc.CurHP = 10000;
                          Npc.MaxHP = 10000;
                      }
                      Game.World.H_NPCs.Add(Npc.EntityID, Npc);

                  }
                  DR.Close();
              }
          
          }
          */
        public static void LoadNPCs()
        {
            string[] FNPCs = File.ReadAllLines(@"OldCODB\NPCs.txt");
            foreach (string Line in FNPCs)
            {
                Game.NPC N = new ConquerSx.Game.NPC(Line);
                Game.World.H_NPCs.Add(N.EntityID, N);
                npcss++;
                //string[] npc = Line.Split(' ');
                //Console.WriteLine("{0} {1} {2} {3} {4} {5}", npc[0], npc[1], npc[2], npc[3], npc[4], npc[5]);


            }
            Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] Npc loading " + npcss.ToString());
            FNPCs = null;
        }
        public static void LoadMobs()
        {
            string[] FMobs = File.ReadAllLines(@"OldCODB\MobInfos.txt");
            Hashtable Mobs = new Hashtable(FMobs.Length);
            for (int i = 0; i < FMobs.Length; i++)
            {
                if (FMobs[i][0] != '*')
                {
                    Game.Mob M = new ConquerSx.Game.Mob(FMobs[i]);
                    Mobs.Add(M.MobID, M);
                }
            }
            string[] FSpawns = File.ReadAllLines(@"OldCODB\MobSpawns.txt");
            foreach (string Spawn in FSpawns)
            {
                if (Spawn[0] == '*') return;
                string[] SpawnInfo = Spawn.Split(' ');
                int MobID = int.Parse(SpawnInfo[0]);
                int Count = int.Parse(SpawnInfo[1]);
                ushort Map = ushort.Parse(SpawnInfo[2]);
                ushort XFrom = ushort.Parse(SpawnInfo[3]);
                ushort YFrom = ushort.Parse(SpawnInfo[4]);
                ushort XTo = ushort.Parse(SpawnInfo[5]);
                ushort YTo = ushort.Parse(SpawnInfo[6]);

                if (!Game.World.H_Mobs.Contains(Map))
                    Game.World.H_Mobs.Add(Map, new Hashtable());
                Hashtable MapMobs = (Hashtable)Game.World.H_Mobs[Map];

                DMap D = (DMap)DMaps.H_DMaps[Map];

                for (int i = 0; i < Count; i++)
                {
                    Game.Mob _Mob = new ConquerSx.Game.Mob((Game.Mob)Mobs[MobID]);
                    _Mob.Loc = new ConquerSx.Game.Location();
                    _Mob.Loc.Map = Map;
                    _Mob.Loc.X = (ushort)Program.Rnd.Next(Math.Min(XFrom, XTo), Math.Max(XFrom, XTo));
                    _Mob.Loc.Y = (ushort)Program.Rnd.Next(Math.Min(YFrom, YTo), Math.Max(YFrom, YTo));

                    while (D != null && D.GetCell(_Mob.Loc.X, _Mob.Loc.Y).NoAccess)
                    {
                        _Mob.Loc.X = (ushort)Program.Rnd.Next(Math.Min(XFrom, XTo), Math.Max(XFrom, XTo));
                        _Mob.Loc.Y = (ushort)Program.Rnd.Next(Math.Min(YFrom, YTo), Math.Max(YFrom, YTo));
                    }
                    _Mob.StartLoc = _Mob.Loc;
                    _Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                    while (Game.World.H_Chars.Contains(_Mob.EntityID) || MapMobs.Contains(_Mob.EntityID))
                        _Mob.EntityID = (uint)Program.Rnd.Next(400000, 500000);

                    MapMobs.Add(_Mob.EntityID, _Mob);
                    monst++;
                }
            }
            Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Mobs loaded " + monst.ToString());
        }
        public static void CreateEquipsDrops()
        {
            StreamWriter SW = new StreamWriter(@"OldCODB\EquipDrops.txt");
            foreach (DatabaseItem DBI in DatabaseItems.Values)
            {
                if (DBI.LevReq >= 1 && DBI.LevReq <= 120 && (Game.ItemIDManipulation.Digit(DBI.ID, 6) == 3 || (Game.ItemIDManipulation.Digit(DBI.ID, 6) == 1) && Game.ItemIDManipulation.Digit(DBI.ID, 1) == 4 || Game.ItemIDManipulation.Digit(DBI.ID, 6) == 5 || Game.ItemIDManipulation.Digit(DBI.ID, 6) == 1 || Game.ItemIDManipulation.Digit(DBI.ID, 6) == 6))
                    SW.WriteLine(DBI.LevReq.ToString() + " " + DBI.ID.ToString());
            }

            SW.Flush();
            SW.Close();
        }

        public static void GetStats(Game.Character character)
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
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55: Job = "Ninja"; break;
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
            else if (Job == "Ninja")
                Data = NinjaStats[lvl].Split(',');
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
                case 50: Job = "Ninja"; break;
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
            else if (Job == "Ninja")
                Data = NinjaStats[lvl].Split(',');
            else if (Job == "Taoist")
                Data = TaoistStats[lvl].Split(',');

            Str = Convert.ToUInt16(Data[0]);
            Vit = Convert.ToUInt16(Data[1]);
            Agi = Convert.ToUInt16(Data[2]);
            Spi = Convert.ToUInt16(Data[3]);
        }
        public static void ReadAllCharacterStats()
        {
            IniFile F = new IniFile(@"OldCODB\Stats.txt");
            for (byte lvl = 1; lvl < 122; lvl++)
            {
                string job = "Archer[" + lvl + "]";
                string Data = F.ReadString("Stats", job);
                ArcherStats.Add(lvl, Data);
                job = "Ninja[" + lvl + "]";
                Data = F.ReadString("Stats", job);
                NinjaStats.Add(lvl, Data);
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
            if (File.Exists(@"OldCODB\Items.txt"))
            {
                int itemcount = 0;
                int start = System.Environment.TickCount;
                TextReader TR = new StreamReader(@"OldCODB\Items.txt");
                string Items = TR.ReadToEnd();
                TR.Close();
                DatabaseItems = new Hashtable();
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
                                NewItem.CPsWorth = Convert.ToUInt32(data[36]);
                                DatabaseItems.Add(NewItem.ID, NewItem);
                                itemcount++;
                            }
                        }
                    }
                }
                Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] Items loading " + itemcount.ToString());
            }
        }
        public static Main.AuthWorker.AuthInfo Authenticate(string User, string Password)
        {
            Main.AuthWorker.AuthInfo Info = new ConquerSx.Main.AuthWorker.AuthInfo();
            Info.Account = User;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);


                cmd.Select("Accounts").Where("AccountID", User);

                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {


                    string RealAccount = r.ReadString("AccountID");
                    if (User == RealAccount)
                    {
                        string RealPassword = r.ReadString("Password");
                        if (RealPassword == "")
                        {
                            MySqlCommand cms = new MySqlCommand(MySqlCommandType.UPDATE);
                            cms.Update("accounts").Set("Password", Password).Where("AccountID", RealAccount).Execute();
                            Info.Status = (ConquerSx.Main.AuthWorker.AuthInfo.AccountState)r.ReadByte("Status");
                            Info.Character = r.ReadString("Character");
                            if (Info.Character == "")
                                Info.LogonType = 2;
                            else
                                Info.LogonType = 1;
                            return Info;
                        }
                        if (RealPassword == Password)
                        {
                            Info.Status = (ConquerSx.Main.AuthWorker.AuthInfo.AccountState)r.ReadByte("Status");
                            Info.Character = r.ReadString("Character");
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
                }
                else
                    Info.LogonType = 255;
            }
            catch { Info.LogonType = 255; }
            return Info;
        }


        public static void ResetTopTrojan()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopTrojan);
                    Chaar.TopTrojan = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopTrojan", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopTrojan", 0).Where("TopDeputyLeader", 1).Execute();

        }

        public static void ResetTopSpouse()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopSpouse);
                    Chaar.TopSpouse = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopSpouse", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopSpouse", 0).Where("TopDeputyLeader", 1).Execute();

        }
        public static void ResetTopWar()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopWarrior);
                    Chaar.TopWarrior = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopWarrior", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopWarrior", 0).Where("TopDeputyLeader", 1).Execute();

        }
        public static void ResetTopArcher()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopArcher);
                    Chaar.TopArcher = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopArcher", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopArcher", 0).Where("TopDeputyLeader", 1).Execute();

        }
        public static void ResetTopWater()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopWaterTaoist);
                    Chaar.TopWaterTaoist = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopWaterTaoist", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopWaterTaoist", 0).Where("TopDeputyLeader", 1).Execute();
        }
        public static void ResetTopFire()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopFireTaoist);
                    Chaar.TopFireTaoist = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopFireTaoist", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopFireTaoist", 0).Where("TopDeputyLeader", 1).Execute();
        }
        public static void ResetTopNinja()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopNinja);
                    Chaar.TopNinja = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopNinja", 0).Where("TopDeputyLeader", 0).Execute();
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("TopNinja", 0).Where("TopDeputyLeader", 1).Execute();
        }
        public static void TopGuildReset()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopDeputyLeader);
                    Chaar.StatEff.Remove(ConquerSx.Game.StatusEffectEn.TopGuildLeader);
                    Chaar.TopDeputyLeader = 0;
                    Chaar.TopGuildLeader = 0;
                }
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopGuildLeader", 0).Set("TopDeputyLeader", 0).Where("TopDeputyLeader", 1).Execute();
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd2.Update("characters").Set("TopGuildLeader", 0).Set("TopDeputyLeader", 0).Where("TopGuildLeader", 1).Execute();
        }
        public static void CreateAccount(string Name, string Password, string Status)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("accounts").Insert("AcountID", Name).Insert("Password", Password).Insert("Status", Status);cmd.Execute();
        }
        public static void UnBanned(string UnBanned, string IDName)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("banned", 0).Set("BanBy", UnBanned).Where("Name", IDName).Execute();
        }
        public static void ExpBallReset()
        {
            foreach (DictionaryEntry DE in Game.World.H_Chars)
            {
                Game.Character Chaar = (Game.Character)DE.Value;
                {
                    Chaar.questtcnr = 0;
                    Chaar.LotteryUsed = 0;
                    Chaar.DbUsedToday = 0;
                    Chaar.ExpBallsUsedToday = 0;
                    Chaar.ElighemPoints = 0;
                    Chaar.ElightenAdd = 1;
                    Chaar.EnhligtehnRequest = 0;
                }
            }
            MySqlCommand cmd1 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd1.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 10).Execute();
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd2.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 9).Execute();
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 8).Execute();
            MySqlCommand cmd4 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd4.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 7).Execute();
            MySqlCommand cmd5 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd5.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 6).Execute();
            MySqlCommand cmd6 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd6.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 5).Execute();
            MySqlCommand cmd7 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd7.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 4).Execute();
            MySqlCommand cmd8 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd8.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 3).Execute();
            MySqlCommand cmd9 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd9.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 2).Execute();
            MySqlCommand cmd10 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd10.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 1).Execute();
            MySqlCommand cmd11 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd11.Update("characters").Set("ExpBallsUsedToday", 0).Set("ElightenAdd", 1).Set("EnhligtehnRequest", 0).Set("ElighemPoints", 0).Set("DbUsedToday", 0).Set("questtcnr", 0).Set("LotteryUsed", 0).Where("ExpBallsUsedToday", 0).Execute();
            MySqlCommand cmdVot = new MySqlCommand(MySqlCommandType.DELETE);
            cmdVot.Delete("VoteIp", "Count", 1).Execute();
            Game.World.VotePoolUid.Clear();
            Game.World.VotePool.Clear();
        }
        public static Game.Character LoadCharacter(string Name, ref string Account)
        {
            Game.Character C = new ConquerSx.Game.Character();
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("Characters").Where("Name", Name);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    C.Name = Name;
                    Account = r.ReadString("Account");
                    C.EntityID = r.ReadUInt32("EntityID");
                    C.Avatar = r.ReadUInt16("Avatar");
                    C.Body = r.ReadUInt16("Body");
                    C.Hair = r.ReadUInt16("Hair");

                    C.Loc = new ConquerSx.Game.Location();
                    C.Loc.Map = r.ReadUInt16("Map");
                    C.Loc.X = r.ReadUInt16("X");
                    C.Loc.Y = r.ReadUInt16("Y");
                    C.Loc.PreviousMap = r.ReadUInt16("PreviousMap");

                    C.Job = r.ReadByte("Job");
                    C.PreviousJob1 = r.ReadByte("PreviousJob");
                    C.PreviousJob2 = r.ReadByte("PreviousJob2");
                    C.Level = r.ReadByte("Level");
                    C.DisKO = r.ReadUInt16("DisKO");
                    C.Experience = r.ReadUInt64("Experience");

                    C.Str = r.ReadUInt16("Str");
                    C.Agi = r.ReadUInt16("Agi");
                    C.House = r.ReadUInt16("House");
                    C.Vit = r.ReadUInt16("Vit");
                    C.Spi = r.ReadUInt16("Spi");
                    C.StatPoints = r.ReadUInt16("StatPoints");
                    C.TopTrojan = r.ReadUInt16("TopTrojan");
                    C.TopSpouse = r.ReadUInt16("TopSpouse");

                    C.TopWarrior = r.ReadUInt16("TopWarrior");
                    C.TopNinja = r.ReadUInt16("TopNinja");
                    C.TopWaterTaoist = r.ReadUInt16("TopWaterTaoist");
                    C.TopArcher = r.ReadUInt16("TopArcher");
                    C.TopGuildLeader = r.ReadUInt16("TopGuildLeader");
                    C.TopFireTaoist = r.ReadUInt16("TopFireTaoist");
                    C.TopDeputyLeader = r.ReadUInt16("TopDeputyLeader");
                    C.WeeklyPKChampion = r.ReadUInt16("WeeklyPKChampion");


                    C.BoxLeft = r.ReadUInt16("Boxss");
                    C.HideQuest = r.ReadUInt16("HideQuest");
                    C.FreeGear = r.ReadUInt16("FreeGear");
                    C.CurHP = r.ReadUInt16("CurHP");
                    C.CurMP = r.ReadUInt16("CurMP");
                    C.HonorPoints = r.ReadUInt16("HonorPoints");
                    C.DragonPoints = r.ReadUInt16("DragonPoints");
                    C.Nobility.Donation = r.ReadUInt64("NobilityDonation");
                    C.Nobility.ListPlace = -1;
                    if (C.Nobility.Donation >= 3000000)
                    {
                        C.Nobility.ListPlace = 50;
                        for (int i = 49; i >= 0; i--)
                        {
                            if (C.Nobility.Donation >= Game.World.EmpireBoard[i].Donation)
                                C.Nobility.ListPlace--;
                        }
                        if (C.Nobility.ListPlace < 50)
                        {
                            if (C.Nobility.Donation >= 30000000 && C.Nobility.Donation <= 100000000)
                                C.Nobility.Rank = Game.Ranks.Knight;
                            else if (C.Nobility.Donation >= 100000000 && C.Nobility.Donation <= 200000000)
                                C.Nobility.Rank = Game.Ranks.Baron;
                            else if (C.Nobility.Donation >= 200000000 && C.Nobility.Donation <= 300000000)
                                C.Nobility.Rank = Game.Ranks.Earl;
                            else if (C.Nobility.ListPlace >= 15 && C.Nobility.ListPlace <= 50)
                                C.Nobility.Rank = Game.Ranks.Duke;
                            else if (C.Nobility.ListPlace >= 3 && C.Nobility.ListPlace <= 15)
                                C.Nobility.Rank = Game.Ranks.Prince;
                            else if (C.Nobility.ListPlace <= 3)
                                C.Nobility.Rank = Game.Ranks.King;
                        }
                    }
                    Game.EmpireInfo G = new EmpireInfo();
                    G.Donation = C.Donation;
                    Database.SaveEmpire();
                    C.Silvers = r.ReadUInt32("Silvers");
                    C.CPs = r.ReadUInt32("CPs");
                    C.WHSilvers = r.ReadUInt32("WHSilvers");
                    C.WHCPs = r.ReadUInt32("WHCPs");

                    C.VP = r.ReadUInt64("VP");
                    C.PKPoints = r.ReadUInt16("PKPoints");
                    ushort GID = r.ReadUInt16("GID");
                    if (Features.Guilds.AllTheGuilds.ContainsKey(GID))
                    {
                        C.MyGuild = (Features.Guild)Features.Guilds.AllTheGuilds[GID];

                        uint Don = r.ReadUInt32("Don"); ;
                        byte GR = r.ReadByte("GR");
                        if (((Hashtable)C.MyGuild.Members[GR]).Contains(C.EntityID))
                        {
                            C.GuildDonation = Don;
                            C.GuildRank = (Features.GuildRank)GR;

                            C.MembInfo = (Features.MemberInfo)((Hashtable)C.MyGuild.Members[GR])[C.EntityID];
                            C.MembInfo.Level = C.Level;
                            C.GuildDonation = C.MembInfo.Donation;
                            C.GuildRank = C.MembInfo.Rank;
                        }
                        else
                            C.MyGuild = null;
                    }

                    C.Equips = new ConquerSx.Game.Equipment();
                    string Equipment = r.ReadString("Equips").ToString();
                    if (Equipment == null)
                       Equipment = "";
                    C.Equips.ReadThis(Equipment);
                   string Warehouses = r.ReadString("Warehouses").ToString();
                    if (Warehouses == null)
                      Warehouses = "";
                   C.Warehouses = new ConquerSx.Game.Banks();
                   C.Warehouses.ReadThis(Warehouses);
                    C.QuestTc = r.ReadUInt16("QuestTc");
                    C.DoubleExpLeft = r.ReadInt32("DoubleExpLeft");
                    C.BlessingLasts = r.ReadInt32("BlessingLasts");
                    C.BlessingStarted = DateTime.FromBinary(r.ReadInt64("BlessingStarted"));
                    C.LuckyTime = r.ReadUInt32("LuckyTime");
                    C.ExpBallsUsedToday = r.ReadByte("ExpBallsUsedToday");
                    C.Reborns = r.ReadByte("Reborns");
                    C.Merchant = (ConquerSx.Game.MerchantTypes)r.ReadByte("Merchant");
                    C.VipLevel = r.ReadByte("VipLevel");
                    C.DbUsedToday = r.ReadByte("DbUsedToday");
                    C.Flori = r.ReadUInt16("Flori");
                    try
                    {
                        C.LastLogin = DateTime.FromBinary(r.ReadInt64("LastLogin"));
                    }
                    catch { C.LastLogin = DateTime.Now; }
                    C.TrainTimeLeft = r.ReadUInt16("TrainTimeLeft");
                    C.InOTG = r.ReadBoolean("InOTG");
                    C.MonsterHunter = r.ReadUInt16("MonsterHunter");
                    C.UniversityPoints = r.ReadUInt32("UniversityPoints");
                    C.WHPassword = r.ReadString("WHPassword");
                    C.Spouse = r.ReadString("Spouse").ToString();
                    C.BanBy = r.ReadString("BanBy");
                    C.flames = r.ReadUInt16("flames");
                    C.LotteryUsed = r.ReadByte("LotteryUsed");
                    C.queststatictc = r.ReadUInt16("queststatictc");
                    C.questtcnr = r.ReadUInt16("questtcnr");
                    C.DoubleExp = r.ReadBoolean("DoubleExp");
                    C.rebornquest = r.ReadUInt16("rebornquest");
                    C.banned = r.ReadInt16("banned");
                    C.quest1 = r.ReadInt16("quest1");
                    C.oldlevel = r.ReadByte("OLDLev");
                    C.ElightenAdd = r.ReadByte("ElightenAdd");
                    C.EnhligtehnRequest = r.ReadByte("EnhligtehnRequest");
                    C.ElighemPoints = r.ReadUInt64("ElighemPoints");
                    C.Loaded = true;

                }
            }
            catch { }
            return C;
        }
        public static void LoadEnemy(Character C)
        {
            C.Enemies = new Hashtable();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("Enemy").Where("EntityID", C.EntityID);
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Game.Enemy F = new Game.Enemy();
                F.UID = d.ReadUInt32("UID");
                F.Name = d.ReadString("EnemyName");
                if (!C.Enemies.Contains(F.UID))
                    C.Enemies.Add(F.UID, F);
            }
        }
        public static void LoadFrends(Character C)
        {
            C.Friends = new Hashtable();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("Friends").Where("EntityID", C.EntityID);
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Game.Friend F = new Game.Friend();
                F.UID = d.ReadUInt32("UID");
                F.Name = d.ReadString("FrendName");
                if (!C.Friends.Contains(F.UID))
                    C.Friends.Add(F.UID, F);
            }
            
        }
        public static void LoadPartners(Character C)
        {
         //   C.Partners = new Hashtable();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("TradePartner").Where("CharID", C.EntityID);
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Game.TradePartner TP = new Game.TradePartner();
                TP.UID = d.ReadUInt32("PartenerID");
                TP.Name = d.ReadString("PartenerName");
                long date = d.ReadInt64("TimeStart");
                C.TimePartner = date;
                TP.ProbationStartedOn =DateTime.FromBinary(date);
                if (!C.Partners.ContainsKey(TP.UID))
                    C.Partners.Add(TP.UID, TP);
            }

        }
        public static void LoadSkills(Character C)
        {
            C.Skills = new Hashtable();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("skills").Where("EntityID", C.EntityID).And("Type", "spell");
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Game.Skill SC = new Game.Skill();
                SC.ID = d.ReadUInt16("ID");
                SC.Lvl = d.ReadByte("Level");
                SC.Exp = d.ReadUInt32("Experience");
                if (!C.Skills.ContainsKey(SC.ID))
                {
                    C.Skills.Add(SC.ID, SC);
                }
            }
        }
        public static void LoadPlayersSlots()
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("slot");
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Main.PlayersPool Slot = new ConquerSx.Main.PlayersPool();
                Slot.name = d.ReadString("name");
                Slot.Uid = d.ReadUInt32("uid");
                if (!Game.World.PlayersPool.ContainsKey(Slot.name))
                    Game.World.PlayersPool.Add(Slot.name, Slot);

            }
        }
        public static void LoadPlayersVots()
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("VoteIp");
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Main.PlayersVot Vot = new ConquerSx.Main.PlayersVot();
                Vot.Uid = d.ReadUInt32("ID");
                Vot.AdressIp = d.ReadString("IP");
                if (!World.VotePool.ContainsKey(Vot.AdressIp))
                    World.VotePool.Add(Vot.AdressIp, Vot);
                if (!Game.World.VotePoolUid.ContainsKey(Vot.Uid))
                    Game.World.VotePoolUid.Add(Vot.Uid, Vot);
            }
        }
        public static void LoadPortals()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("portals");
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                PacketHandling.Struct1.Portal Port = new PacketHandling.Struct1.Portal();
                Port.ID = d.ReadInt32("PortalID");
                Port.StartMap = d.ReadInt32("StartMap");
                Port.StartX = d.ReadInt32("StartX");
                Port.StartY = d.ReadInt32("StartY");
                Port.EndMap = d.ReadInt32("EndMap");
                Port.EndX = d.ReadInt32("EndX");
                Port.EndY = d.ReadInt32("EndY");
                string PID = Port.StartX + "," + Port.StartY + "," + Port.StartMap;
                if (!World.Portals.ContainsKey(PID))
                    World.Portals.Add(PID, Port);
                else
                {
                    int a = 0;
                    while (a < 10)
                    {
                        //Console.WriteLine("Portal ID " + Port.ID + " use 2 variabile");
                    }
                }
            }
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] Loaded " + World.Portals.Count + " portals into the world.");
        }
        public static void LoadProfs(Character C)
        {
            C.Profs = new Hashtable();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("profs").Where("EntityID", C.EntityID).And("Type", "prof");
            MySqlReader d = new MySqlReader(cmd);
            while (d.Read())
            {
                Game.Prof SC = new Game.Prof();
                SC.ID = d.ReadUInt16("ID");
                SC.Lvl = d.ReadByte("Level");
                SC.Exp = d.ReadUInt32("Experience");
                if (!C.Profs.Contains(SC.ID))
                    C.Profs.Add(SC.ID, SC);
            }
        }
        public static void savebody(Game.Character C,uint body)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Body", body).Where("EntityID", C.EntityID).Execute();
        }
        public static void savevip(Game.Character C, byte levelvip)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("VipLevel", levelvip).Where("EntityID", C.EntityID).Execute();
        }
        public static void savelife(Game.Character C, uint hp)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("CurHP",hp).Where("EntityID", C.EntityID).Execute();
        }
        public static void savemana(Game.Character C, uint mp)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("CurMP", mp).Where("EntityID", C.EntityID).Execute();
        }
        public static void savemap(Game.Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Map", C.Loc.Map).Set("X", C.Loc.X).Set("Y", C.Loc.Y).Set("PreviousMap", C.Loc.PreviousMap).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveHair(Game.Character C,ushort hair)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Hair",hair).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveJob(Game.Character C, byte job)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Job", job).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveLevel(Game.Character C, byte level)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Level", level).Where("EntityID", C.EntityID).Execute();
        }

        public static void SaveStr(Game.Character C, ushort str)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Str", str).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveAgi(Game.Character C, ushort Agi)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Agi", Agi).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveVit(Game.Character C, ushort vit)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Vit", vit).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveSpi(Game.Character C, ushort spi)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Spi", spi).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveCharStatus(Game.Character C, ushort status)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("StatPoints", status).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveAvatar(Game.Character C, ushort avatar)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Avatar", avatar).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveSilver(Game.Character C,uint silver)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Silvers", silver).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveCps(Game.Character C, uint cps)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("CPs", cps).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveWhSilver(Game.Character C, uint whsilver)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("WHSilvers", whsilver).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveWHCPs(Game.Character C, uint WHCP)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("WHCPs", WHCP).Where("EntityID", C.EntityID).Execute();
        }
        public static void SavePkPoints(Game.Character C, ushort pkp)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("PKPoints", pkp).Where("EntityID", C.EntityID).Execute();
        }
        public static void SavePreviousjob1(Game.Character C, ushort job1)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("PreviousJob", job1).Where("EntityID", C.EntityID).Execute();
        }
        public static void SavePreviousjob2(Game.Character C, ushort job2)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("PreviousJob2",job2).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveReborn(Game.Character C, byte reborns)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("Reborns", reborns).Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveTop(Game.Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("TopTrojan", C.TopTrojan).Set("TopSpouse", C.TopSpouse).Set("TopWarrior", C.TopWarrior).Set("TopNinja", C.TopNinja)
                        .Set("TopWaterTaoist", C.TopWaterTaoist).Set("TopArcher", C.TopArcher).Set("TopGuildLeader", C.TopGuildLeader)
                        .Set("TopFireTaoist", C.TopFireTaoist).Set("TopDeputyLeader", C.TopDeputyLeader).Set("WeeklyPKChampion", C.WeeklyPKChampion)
                        .Where("EntityID", C.EntityID).Execute();
        }
        public static void SaveCharacter(Game.Character C, string Acc)
        {
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                    int DoubleExp = C.DoubleExpLeft;
                    if (C.DoubleExp)
                        DoubleExp -= (int)(DateTime.Now - C.ExpPotionUsed).TotalSeconds;
                    int Boxss = C.BoxLeft;
                    Boxss -= (int)(DateTime.Now - C.BoxUsed).TotalSeconds;

                    int gid = 0;
                    if (C.MyGuild != null)
                        gid = C.MyGuild.GuildID;
                    if (C.GettingLuckyTime)
                    {
                        if (!C.Prayer)
                            C.LuckyTime += (uint)(DateTime.Now - C.PrayDT).TotalSeconds;
                        else
                            C.LuckyTime += (uint)(DateTime.Now - C.PrayDT).TotalSeconds * 3;
                        C.PrayDT = DateTime.Now;
                    }

       
                    //.Set("Equips", C.Equips.WriteThis())
                    cmd.Update("characters")
                        .Set("Map", C.Loc.Map).Set("X", C.Loc.X).Set("Y", C.Loc.Y).Set("PreviousMap", C.Loc.PreviousMap)
                        .Set("Experience", C.Experience)
       
                        .Set("DisKO", C.DisKO)
                        .Set("House", C.House)
                        .Set("HideQuest", C.HideQuest).Set("Boxss", C.BoxLeft).Set("FreeGear", C.FreeGear).Set("HonorPoints", C.HonorPoints).Set("DragonPoints", C.DragonPoints)
                        .Set("NobilityDonation", C.Nobility.Donation)
                        .Set("VP", C.VP).Set("GID", gid).Set("Don", C.GuildDonation).Set("GR", (byte)C.GuildRank)
                        .Set("ElighemPoints", C.ElighemPoints).Set("ElightenAdd", C.ElightenAdd).Set("EnhligtehnRequest", C.EnhligtehnRequest)
                        .Set("Equips", C.Equips.WriteThis())
                       .Set("Warehouses", C.Warehouses.WriteThis())
                        .Set("QuestTc", C.QuestTc)
                        .Set("DoubleExpLeft", DoubleExp).Set("BlessingLasts", C.BlessingLasts).Set("BlessingStarted", C.BlessingStarted.Ticks)
                        .Set("LuckyTime", C.LuckyTime).Set("ExpBallsUsedToday", C.ExpBallsUsedToday).Set("Merchant", (byte)C.Merchant)
                        .Set("DbUsedToday", C.DbUsedToday).Set("Flori", C.Flori).Set("LastLogin", DateTime.Now.Ticks)
                        .Set("TrainTimeLeft", (ushort)(C.TrainTimeLeft + ((DateTime.Now - C.LoggedOn).TotalMinutes * 10)))
                        .Set("InOTG", (C.InOTG == true ? "1" : "0")).Set("MonsterHunter", C.MonsterHunter)
                        .Set("WHPassword", C.WHPassword).Set("Spouse", C.Spouse).Set("BanBy", C.BanBy).Set("flames", C.flames).Set("LotteryUsed", C.LotteryUsed)
                        .Set("queststatictc", C.queststatictc).Set("questtcnr", C.questtcnr).Set("DoubleExp", (C.DoubleExp == true ? "1" : "0"))
                        .Set("rebornquest", C.rebornquest).Set("banned", C.banned).Set("quest1", C.quest1).Set("OLDLev", C.oldlevel)
                        .Where("EntityID", C.EntityID).Execute();

                    
                  


                }
                catch (Exception Exc) { Console.WriteLine(Exc); }
            }
        }
        public static void SaveUniversity(Game.Character C,uint puncte)
        {                 
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("characters").Set("UniversityPoints", puncte).Where("EntityID", C.EntityID).Execute();
        }
        public static void CreateGuild(Game.Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("guildenemies").Insert("GuildID", C.MyGuild.GuildID).Execute();
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.INSERT);
            cmd2.Insert("guildallies").Insert("GuildID", C.MyGuild.GuildID).Execute();
        }
        public static void DismisGuild(Game.Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("guildenemies", "GuildID", C.MyGuild.GuildID).Execute();
            MySqlCommand Cmd2 = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd2.Delete("guildallies", "GuildID", C.MyGuild.GuildID).Execute();
        }
        public static void AddAllisToGuild(Game.Character C, Features.Guild G)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd2.Select("guildallies").Where("GuildID", C.MyGuild.GuildID);
            MySqlReader r = new MySqlReader(cmd2);
            if (r.Read())
            {
                if (r.ReadUInt16("AlliesId1") == 0)
                {
                    cmd3.Update("guildallies").Set("AlliesId1", G.GuildID).Set("AlliesName1", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("AlliesId2") == 0)
                {
                    cmd3.Update("guildallies").Set("AlliesId2", G.GuildID).Set("AlliesName2", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("AlliesId3") == 0)
                {
                    cmd3.Update("guildallies").Set("AlliesId3", G.GuildID).Set("AlliesName3", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("AlliesId4") == 0)
                {
                    cmd3.Update("guildallies").Set("AlliesId4", G.GuildID).Set("AlliesName4", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("AlliesId5") == 0)
                {
                    cmd3.Update("guildallies").Set("AlliesId5", G.GuildID).Set("AlliesName5", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
            }
        }
        public static void DeleteAllisDusman(Game.Character C, Features.Guild G)
        {
            MySqlCommand cmd4 = new MySqlCommand(MySqlCommandType.UPDATE);
            MySqlCommand cmd5 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd5.Select("guildallies").Where("GuildID", G.GuildID);
            MySqlReader s = new MySqlReader(cmd5);
            if (s.Read())
            {
                try
                {
                    if (s.ReadUInt16("AlliesId1") == C.MyGuild.GuildID)
                    {
                        cmd4.Update("guildallies").Set("AlliesId1", 0).Set("AlliesName1", null).Where("GuildID", G.GuildID).Execute();
                    }
                    else if (s.ReadUInt16("AlliesId2") == C.MyGuild.GuildID)
                    {
                        cmd4.Update("guildallies").Set("AlliesId2", 0).Set("AlliesName2", null).Where("GuildID", G.GuildID).Execute();
                    }
                    else if (s.ReadUInt16("AlliesId3") == C.MyGuild.GuildID)
                    {
                        cmd4.Update("guildallies").Set("AlliesId3", 0).Set("AlliesName3", null).Where("GuildID", G.GuildID).Execute();
                    }
                    else if (s.ReadUInt16("AlliesId4") == C.MyGuild.GuildID)
                    {
                        cmd4.Update("guildallies").Set("AlliesId4", 0).Set("AlliesName4", null).Where("GuildID", G.GuildID).Execute();
                    }
                    else if (s.ReadUInt16("AlliesId5") == C.MyGuild.GuildID)
                    {
                        cmd4.Update("guildallies").Set("AlliesId5", 0).Set("AlliesName5", null).Where("GuildID", G.GuildID).Execute();
                    }
                }
                catch { Console.WriteLine("ERORRRRRRRRRRRRRRRRRRRRRR ALLIES"); }
            }
        }
        public static void DeleteAllis(Game.Character C, Features.Guild G)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd2.Select("guildallies").Where("GuildID", C.MyGuild.GuildID);
            MySqlReader r = new MySqlReader(cmd2);
            if (r.Read()) 
            {
                try
                {
                    if (r.ReadUInt16("AlliesId1") == G.GuildID)
                    {
                        cmd3.Update("guildallies").Set("AlliesId1", 0).Set("AlliesName1", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                    }
                    else if (r.ReadUInt16("AlliesId2") == G.GuildID)
                    {
                        cmd3.Update("guildallies").Set("AlliesId2", 0).Set("AlliesName2", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                    }
                    else if (r.ReadUInt16("AlliesId3") == G.GuildID)
                    {
                        cmd3.Update("guildallies").Set("AlliesId3", 0).Set("AlliesName3", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                    }
                    else if (r.ReadUInt16("AlliesId4") == G.GuildID)
                    {
                        cmd3.Update("guildallies").Set("AlliesId4", 0).Set("AlliesName4", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                    }
                    else if (r.ReadUInt16("AlliesId5") == G.GuildID)
                    {
                        cmd3.Update("guildallies").Set("AlliesId5", 0).Set("AlliesName5", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                    }
                }
                catch { Console.WriteLine("ERORRRRRRRRRRRRRRRRRRRRRR ALLIES"); }
            }
        }
        public static void AddEnemiesGuild(Game.Character C, Features.Guild G)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd2.Select("guildenemies").Where("GuildID", C.MyGuild.GuildID);
            MySqlReader r = new MySqlReader(cmd2);
            if (r.Read())
            {
                if (r.ReadUInt16("EnemiesId1") == 0)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId1", G.GuildID).Set("EnemiesName1", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId2") == 0)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId2", G.GuildID).Set("EnemiesName2", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId3") == 0)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId3", G.GuildID).Set("EnemiesName3", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId4") == 0)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId4", G.GuildID).Set("EnemiesName4", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId5") == 0)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId5", G.GuildID).Set("EnemiesName5", G.GuildName).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
            }
           //MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
           //cmd.Update("guildenemies").Set("EnemiesId1", C.MyGuild.Enemiesid1).Set("EnemiesName1", C.MyGuild.Enemies1)
           //    .Set("EnemiesId2", C.MyGuild.Enemiesid2).Set("EnemiesName2", C.MyGuild.Enemies2)
           //    .Set("EnemiesId3", C.MyGuild.Enemiesid3).Set("EnemiesName3", C.MyGuild.Enemies3)
           //    .Set("EnemiesId4", C.MyGuild.Enemiesid4).Set("EnemiesName4", C.MyGuild.Enemies4)
           //    .Set("EnemiesId5", C.MyGuild.Enemiesid5).Set("EnemiesName5", C.MyGuild.Enemies5).Where("GuildID",C.MyGuild.GuildID)
           //    .Execute();
        }
        public static void LoadEnemiesGuildaaa(Game.Character C, Features.Guild G)
        {
            ushort Guildid = 0;
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd2.Select("Characters").Where("Name", C.Name);
            MySqlReader r = new MySqlReader(cmd2);
            if (r.Read())
            {
                Guildid = r.ReadUInt16("GID");
            }

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("guildenemies").Where("GuildID", Guildid);
            MySqlReader d = new MySqlReader(cmd);
            if (d.Read())
            {
                if (d.ReadUInt16("EnemiesId1") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId1") == TGuild.GuildID)
                        {
                            //Console.WriteLine("logate ceva?");
                            C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId2") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId2") == TGuild.GuildID)
                        {
                            C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId3") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId3") == TGuild.GuildID)
                        {
                            C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId4") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId4") == TGuild.GuildID)
                        {
                            C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId5") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId5") == TGuild.GuildID)
                        {
                            C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
            }
        }
        public static void LoadEnemiesGuildss(Features.Guild G)
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("guildenemies").Where("GuildID", G.GuildID);
            MySqlReader d = new MySqlReader(cmd);
            if (d.Read())
            {
                if (d.ReadUInt16("EnemiesId1") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId1") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId2") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId2") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId3") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId3") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId4") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId4") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("EnemiesId5") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("EnemiesId5") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Enemies.ContainsKey(TGuild.GuildID))
                                G.Enemies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
            }
        }
        public static void LoadGuildAllis(Features.Guild G)
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("guildallies").Where("GuildID", G.GuildID);
            MySqlReader d = new MySqlReader(cmd);
            if (d.Read())
            {
                if (d.ReadUInt16("AlliesId1") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("AlliesId1") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Allies.ContainsKey(TGuild.GuildID))
                                G.Allies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("AlliesId2") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("AlliesId2") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Allies.ContainsKey(TGuild.GuildID))
                                G.Allies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("AlliesId3") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("AlliesId3") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Allies.ContainsKey(TGuild.GuildID))
                                G.Allies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("AlliesId4") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("AlliesId4") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Allies.ContainsKey(TGuild.GuildID))
                                G.Allies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
                if (d.ReadUInt16("AlliesId5") != 0)
                {
                    foreach (KeyValuePair<uint, Features.Guild> Guilds in Features.Guilds.AllTheGuilds)
                    {

                        Features.Guild TGuild = Guilds.Value;
                        if (d.ReadUInt16("AlliesId5") == TGuild.GuildID)
                        {
                            //C.MyClient.SendPacket(Packets.SendGuild(TGuild.GuildID, 9));
                            if (!G.Allies.ContainsKey(TGuild.GuildID))
                                G.Allies.Add(TGuild.GuildID, TGuild);
                        }
                    }
                }
            }
        }
        public static void DeleteEnemies(Game.Character C, Features.Guild G)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.SELECT);
            cmd2.Select("guildenemies").Where("GuildID", C.MyGuild.GuildID);
            MySqlReader r = new MySqlReader(cmd2);
            if (r.Read())
            {
                if (r.ReadUInt16("EnemiesId1") == G.GuildID)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId1", 0).Set("EnemiesName1", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId2") == G.GuildID)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId2", 0).Set("EnemiesName2", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId3") == G.GuildID)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId3", 0).Set("EnemiesName3", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId4") == G.GuildID)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId4", 0).Set("EnemiesName4", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
                else if (r.ReadUInt16("EnemiesId5") == G.GuildID)
                {
                    cmd3.Update("guildenemies").Set("EnemiesId5", 0).Set("EnemiesName5", null).Where("GuildID", C.MyGuild.GuildID).Execute();
                }
            }
            //MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            //cmd.Update("guildenemies").Set("EnemiesId1", C.MyGuild.Enemiesid1).Set("EnemiesName1", C.MyGuild.Enemies1)
            //    .Set("EnemiesId2", C.MyGuild.Enemiesid2).Set("EnemiesName2", C.MyGuild.Enemies2)
            //    .Set("EnemiesId3", C.MyGuild.Enemiesid3).Set("EnemiesName3", C.MyGuild.Enemies3)
            //    .Set("EnemiesId4", C.MyGuild.Enemiesid4).Set("EnemiesName4", C.MyGuild.Enemies4)
            //    .Set("EnemiesId5", C.MyGuild.Enemiesid5).Set("EnemiesName5", C.MyGuild.Enemies5).Where("GuildID",C.MyGuild.GuildID)
            //    .Execute();
        }
        public static void SaveSlots(Main.PlayersPool SlotPlayer)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("Slot").Insert("name", SlotPlayer.name).Insert("uid", SlotPlayer.Uid).Execute();

        }
        public static void SavePlayersVot(Main.PlayersVot PlayerVot)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("VoteIp").Insert("ID", PlayerVot.Uid).Insert("IP", PlayerVot.AdressIp).Execute();

        }
        public static void RemoveSlots(Main.PlayersPool players)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("slot", "uid", players.Uid).And("name", players.name).Execute();

        }

        public static void SavePartner(uint uid, string name, long timestart, Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("TradePartner").Insert("CharID", C.EntityID).Insert("PartenerName", name).Insert("PartenerID", uid).Insert("TimeStart",timestart).Execute();
           
        }
        public static void DelPartner(uint UID, uint IDYou)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("TradePartner", "CharID", IDYou).And("PartenerID", UID).Execute();
        }
        public static void SaveEnemy(uint uid,string name, Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("Enemy").Insert("EntityID", C.EntityID).Insert("EnemyName", name).Insert("UID", uid).Execute();
        }
        public static void DelEnemy(uint UID,Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("Enemy", "EntityID", C.EntityID).And("UID", UID).Execute();
        }
        public static void SaveFrends(Friend F, Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("Friends").Insert("EntityID", C.EntityID).Insert("FrendName", F.Name).Insert("UID", F.UID).Execute();
        
        }
        public static void DelFrends(Friend F,Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("Friends", "EntityID", F.UID).And("UID",C.EntityID).Execute();
            MySqlCommand Cmds = new MySqlCommand(MySqlCommandType.DELETE);
            Cmds.Delete("Friends", "EntityID", C.EntityID).And("UID", F.UID).Execute();
        }
        public static void DelProfs(Prof p, Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("profs", "EntityID", C.EntityID).And("ID", p.ID).Execute();
        }
        public static void DelSkills(Skill p, Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("skills", "EntityID", C.EntityID).And("ID", p.ID).Execute();
        }
        public static void deleteprofreborn(Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("profs", "EntityID", C.EntityID).Execute();

        }
        public static void deletepsekillreborn(Character C)
        {
            MySqlCommand Cmd = new MySqlCommand(MySqlCommandType.DELETE);
            Cmd.Delete("skills", "EntityID", C.EntityID).Execute();
        }
        /*public static void UpgradeServerStatus()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("config").Set("PlayersOnline", Game.World.H_Chars.Count).Where("ServerName", Game.World.Server.ServerName).Execute();

        }*/
        public static void SaveProfs(Prof p,Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("profs").Set("Level", p.Lvl).Set("Experience", p.Exp).Where("EntityID", C.EntityID).And("ID",p.ID).Execute();
        }
        public static void SaveSkill(Skill p, Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("skills").Set("Level", p.Lvl).Set("Experience", p.Exp).Where("EntityID", C.EntityID).And("ID",p.ID).Execute();
        }
        public static void AddProfs(ushort ID, byte lev, uint exp, Character Gc)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("profs").Insert("Level", lev).Insert("Experience", exp).Insert("EntityID", (long)Gc.EntityID)
                .Insert("Type", "prof").Insert("ID", ID).Execute();
        }
        public static void AddSkills(ushort ID, byte lev, uint exp, Character Gc)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("skills").Insert("Level", lev).Insert("Experience", exp).Insert("EntityID", (long)Gc.EntityID)
                .Insert("Type", "spell").Insert("ID", ID).Execute();
        }
        public static bool GetItems(Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("items").Where("CharID", C.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                {
                    Game.Item Item = new Game.Item();
                    Item.Bless = r.ReadByte("Minus");
                    Item.CurDur = r.ReadUInt16("Dura");
                    Item.ID = r.ReadUInt32("ItemID");
                    Item.MaxDur = r.ReadUInt16("MaxDura");
                    Item.Plus = r.ReadByte("Plus");
                    Item.Position = r.ReadByte("Position");
                    Item.Soc1 = (Item.Gem)r.ReadByte("Soc1");
                    Item.Soc2 = (Item.Gem)r.ReadByte("Soc2");
                    Item.UID = r.ReadUInt32("ItemUID");
                    Item.Enchant = r.ReadByte("Enchant");
                    Item.Color = (Item.ArmorColor)r.ReadByte("Color");
                    Item.Suspicious = r.ReadInt16("Suspicious");
                    Item.FreeItem = r.ReadBoolean("Free");
                    Item.Locked = r.ReadByte("Locked");
                    Item.Progress = r.ReadUInt16("Progress");
                    Item.LockedDays = r.ReadUInt32("LockedDay");
                    Item.TalismanProgress = r.ReadUInt32("SocketProgress");
                    //C.Equips.Armor = Item;
                    if (Item.ID == 300000)
                    {
                        Item.RBG[0] = r.ReadByte("X");
                        Item.RBG[1] = r.ReadByte("Y");
                        Item.RBG[2] = r.ReadByte("Z");
                        Item.RBG[3] = r.ReadByte("floor");
                        Item.TalismanProgress = BitConverter.ToUInt32(Item.RBG, 0);
                    }
                    Item.Effect = (Item.RebornEffect)r.ReadByte("Effect");
                 
                    C.Inventory.Add(Item.UID, Item);
                    C.MyClient.SendPacket(Packets.AddItem(Item, 0));
                    if (Item.Locked == 2)
                    {

                        int myDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                        C.MyClient.SendPacket(Packets.ItemLock(Item.UID, 1, 3, (uint)Item.LockedDays));
                        if (myDate >= (int)Item.LockedDays)
                        {
                            Item.LockedDays = 0;
                            Item.Locked = 0;
                            Database.SaveItems(Item, C);

                            C.MyClient.LocalMessage(2000, System.Drawing.Color.Red, "Congratulations! successful Unlocked " + Item.DBInfo.Name + "");
                        }
                    }

                }
            }
            return true;
        }

        public static bool Reward(Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("CReward").Where("CharID", C.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                {
                    Game.Item Item = new Game.Item();
                    Item.Bless = r.ReadByte("Minus");
                    Item.CurDur = r.ReadUInt16("Dura");
                    Item.ID = r.ReadUInt32("ItemID");
                    Item.MaxDur = r.ReadUInt16("MaxDura");
                    Item.Plus = r.ReadByte("Plus");
                    Item.Position = r.ReadByte("Position");
                    Item.Soc1 = (Item.Gem)r.ReadByte("Soc1");
                    Item.Soc2 = (Item.Gem)r.ReadByte("Soc2");
                    Item.UID = r.ReadUInt32("ItemUID");
                    Item.Enchant = r.ReadByte("Enchant");
                    Item.Color = (Item.ArmorColor)r.ReadByte("Color");
                    Item.Suspicious = r.ReadInt16("Suspicious");
                    Item.FreeItem = r.ReadBoolean("Free");
                    Item.Locked = r.ReadByte("Locked");
                    Item.Progress = r.ReadUInt16("Progress");
                    Item.LockedDays = r.ReadUInt32("LockedDay");
                    Item.TalismanProgress = r.ReadUInt32("SocketProgress");
                    //C.Equips.Armor = Item;
                    if (Item.ID == 300000)
                    {
                        Item.RBG[0] = r.ReadByte("X");
                        Item.RBG[1] = r.ReadByte("Y");
                        Item.RBG[2] = r.ReadByte("Z");
                        Item.RBG[3] = r.ReadByte("floor");
                        Item.TalismanProgress = BitConverter.ToUInt32(Item.RBG, 0);
                    }
                    Item.Effect = (Item.RebornEffect)r.ReadByte("Effect");
                    Item.NameReward = r.ReadString("Name1");
                    C.ConfiscatorReward.Add(Item.UID, Item);

                    C.MyClient.SendPacket(Packets.ConfiscatorReward(Item, C, (ushort)PacketHandling.Confiscator.CpsItem(Item),Item.NameReward));

                }
            }
            return true;
        }
        public static void UpSac(uint UID, Game.Character C)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Update("CClain").Set("Position",1).Where("ItemUID", UID).And("CharID", C.EntityID).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static void DeleteReward(uint UID, Game.Character C)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Delete("CReward", "ItemUID", UID).And("CharID", C.EntityID).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static void DeleteClain(uint UID, Game.Character C)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Delete("CClain", "ItemUID", UID).And("CharID", C.EntityID).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static bool Clain(Character C)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("CClain").Where("CharID", C.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                {
                    Game.Item Item = new Game.Item();
                    Item.Bless = r.ReadByte("Minus");
                    Item.CurDur = r.ReadUInt16("Dura");
                    Item.ID = r.ReadUInt32("ItemID");
                    Item.MaxDur = r.ReadUInt16("MaxDura");
                    Item.Plus = r.ReadByte("Plus");
                    Item.Position = r.ReadByte("Position");
                    Item.Soc1 = (Item.Gem)r.ReadByte("Soc1");
                    Item.Soc2 = (Item.Gem)r.ReadByte("Soc2");
                    Item.UID = r.ReadUInt32("ItemUID");
                    Item.Enchant = r.ReadByte("Enchant");
                    Item.Color = (Item.ArmorColor)r.ReadByte("Color");
                    Item.Suspicious = r.ReadInt16("Suspicious");
                    Item.FreeItem = r.ReadBoolean("Free");
                    Item.Locked = r.ReadByte("Locked");
                    Item.Progress = r.ReadUInt16("Progress");
                    Item.LockedDays = r.ReadUInt32("LockedDay");
                    Item.TalismanProgress = r.ReadUInt32("SocketProgress");
                    //C.Equips.Armor = Item;
                    if (Item.ID == 300000)
                    {
                        Item.RBG[0] = r.ReadByte("X");
                        Item.RBG[1] = r.ReadByte("Y");
                        Item.RBG[2] = r.ReadByte("Z");
                        Item.RBG[3] = r.ReadByte("floor");
                        Item.TalismanProgress = BitConverter.ToUInt32(Item.RBG, 0);
                    }
                    Item.Effect = (Item.RebornEffect)r.ReadByte("Effect");
                    Item.NameClain = r.ReadString("Name1");
                    C.ConfiscatorClain.Add(Item.UID, Item);
                    if (Item.Position == 1)
                        C.MyClient.SendPacket(Packets.Sac(Item, C, (ushort)PacketHandling.Confiscator.CpsItem(Item), Item.NameClain));
                    else
                        C.MyClient.SendPacket(Packets.ConfiscatorClain(Item, C, (ushort)PacketHandling.Confiscator.CpsItem(Item), Item.NameClain));

                }
            }
            return true;
        }
        public static bool ConfiscatorReward(Game.Item Item, Game.Character GC)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("CReward").Insert("ItemUID", Item.UID).Insert("Position", Item.Position).Insert("CharID", GC.EntityID).Insert("ItemID", Item.ID).Insert("Plus", Item.Plus).Insert("Minus", Item.Bless).Insert("Enchant", Item.Enchant).Insert("Soc1", (byte)Item.Soc1).Insert("Soc2", (byte)Item.Soc2).Insert("Dura", Item.CurDur).Insert("MaxDura", Item.MaxDur).Insert("LockedDay", Item.LockedDays).Insert("Color", (byte)Item.Color).Insert("Suspicious", Item.Suspicious).Insert("Free", Item.FreeItem).Insert("Locked", Item.Locked).Insert("Progress", Item.Progress).Insert("SocketProgress", Item.TalismanProgress).Insert("X", Item.RBG[0])
                    .Insert("Y", Item.RBG[1]).Insert("Z", Item.RBG[2]).Insert("floor", Item.RBG[3]).Insert("Effect", (byte)Item.Effect).Insert("Time",Item.Time).Insert("Name1",Item.NameReward);
                cmd.Execute();
            }
            catch
            {
                return false;

            }
            return true;
        }

        public static bool ConfiscatorClain(Game.Item Item, Game.Character GC)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("CClain").Insert("ItemUID", Item.UID).Insert("Position", Item.Position).Insert("CharID", GC.EntityID).Insert("ItemID", Item.ID).Insert("Plus", Item.Plus).Insert("Minus", Item.Bless).Insert("Enchant", Item.Enchant).Insert("Soc1", (byte)Item.Soc1).Insert("Soc2", (byte)Item.Soc2).Insert("Dura", Item.CurDur).Insert("MaxDura", Item.MaxDur).Insert("LockedDay", Item.LockedDays).Insert("Color", (byte)Item.Color).Insert("Suspicious", Item.Suspicious).Insert("Free", Item.FreeItem).Insert("Locked", Item.Locked).Insert("Progress", Item.Progress).Insert("SocketProgress", Item.TalismanProgress).Insert("X", Item.RBG[0])
                    .Insert("Y", Item.RBG[1]).Insert("Z", Item.RBG[2]).Insert("floor", Item.RBG[3]).Insert("Effect", (byte)Item.Effect).Insert("Time", Item.Time).Insert("Name1", Item.NameClain); 
                cmd.Execute();
            }
            catch
            {
                return false;

            }
            return true;
        }





        public static bool NewItem(Game.Item Item, Game.Character GC)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("items").Insert("ItemUID", Item.UID).Insert("Position",Item.Position).Insert("CharID", GC.EntityID).Insert("ItemID", Item.ID).Insert("Plus", Item.Plus).Insert("Minus", Item.Bless).Insert("Enchant", Item.Enchant).Insert("Soc1", (byte)Item.Soc1).Insert("Soc2", (byte)Item.Soc2).Insert("Dura", Item.CurDur).Insert("MaxDura", Item.MaxDur).Insert("LockedDay",Item.LockedDays).Insert("Color", (byte)Item.Color).Insert("Suspicious", Item.Suspicious).Insert("Free", Item.FreeItem).Insert("Locked", Item.Locked).Insert("Progress", Item.Progress).Insert("SocketProgress", Item.TalismanProgress).Insert("X", Item.RBG[0])
                    .Insert("Y", Item.RBG[1]).Insert("Z", Item.RBG[2]).Insert("floor", Item.RBG[3]).Insert("Effect", (byte)Item.Effect);
                cmd.Execute();
            }
            catch
            {
                return false;

            }
            return true;
        }

        public static uint ItemUID
        {
            get
            {
                uint uid = 0;
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("items").Where("CharID", 0);
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    uid = r.ReadUInt32("ItemID");
                }
                uint uid2 = uid + 1;
                MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd2.Update("items").Set("ItemID", uid2).Where("CharID", 0).Execute();
                return uid;
            }
        }
        public static void DeleteItem(uint UID,Game.Character C)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Delete("items", "ItemUID", UID).And("CharID",C.EntityID).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static void DeleteItemBank(uint UID,uint pos, Game.Character C)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Delete("items", "ItemUID", UID).And("CharID", C.EntityID).And("Position",pos).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static void Fixinventory(uint UID)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
                cmd.Delete("items", "CharID", UID).Execute();
                MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.DELETE);
                cmd2.Delete("items", "ItemUID", UID).Execute();
            }
            catch (Exception Exe)
            {
                Console.WriteLine(Exe);
            }
        }
        public static void SaveItems(Game.Item Item,Character C)
        {
            try
            {
                MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd2.Update("items").Set("Position", Item.Position).Set("ItemID", Item.ID).Set("Plus", Item.Plus).Set("Progress", Item.Progress).Set("Minus", Item.Bless).Set("Enchant", Item.Enchant).Set("Soc1", (byte)Item.Soc1).Set("Soc2", (byte)Item.Soc2).Set("Dura", Item.CurDur).Set("MaxDura", Item.MaxDur).Set("Color", (byte)Item.Color).Set("Locked", Item.Locked).Set("Free", Item.FreeItem).Set("SocketProgress", Item.TalismanProgress).Set("Suspicious", Item.Suspicious).Set("LockedDay", Item.LockedDays)
                    .Set("X", Item.RBG[0])
                    .Set("Y", Item.RBG[1]).Set("Z", Item.RBG[2]).Set("floor", Item.RBG[3]).Set("Effect", (byte)Item.Effect)
                    .Where("ItemUID", Item.UID).And("CharID", C.EntityID).Execute();
            }
            catch (Exception Exe) { Program.WriteLine(Exe); }
        }
        public static void LoadLottoItems()
        {
            string[] Lotto = System.IO.File.ReadAllLines(@"OldCODB\Lotto.txt");
            int lotericount = 0;
            for (short Cur = 0; Cur < Lotto.Length; Cur++)
            {
                if (Lotto[Cur] != null && Lotto[Cur] != "")
                {
                    string[] Item = Lotto[Cur].Split(',');
                    Game.Item TheItem = new ConquerSx.Game.Item();
                    TheItem.ID = uint.Parse(Item[0]);
                    TheItem.Plus = byte.Parse(Item[1]);
                    TheItem.Soc1 = (ConquerSx.Game.Item.Gem)byte.Parse(Item[2]);
                    TheItem.Soc2 = (ConquerSx.Game.Item.Gem)byte.Parse(Item[3]);
                    if (DatabaseItems.ContainsKey(TheItem.ID))
                    {
                        DatabaseItem DI = (DatabaseItem)DatabaseItems[TheItem.ID];
                        TheItem.CurDur = TheItem.MaxDur = DI.Durability;
                        Game.World.H_LottoItems.Add(Cur, TheItem);
                        lotericount++;
                    }
                }
            }
            Program.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [GameServer] LoteryItems loading " + lotericount.ToString());
        }
        public static string CreateCharacter(string Account, string Name, ushort Body, byte Job)
        {
            try
            {
                Game.Character GC = new ConquerSx.Game.Character();
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                    cmd.Select("Characters").Where("Name", Name);
                    MySqlReader r = new MySqlReader(cmd);
                    if (r.Read())
                    {
                        return "Name in use.";
                    }
                }
                catch { }
                int CPs = 500;
                int Silvers = 2000000;
                ushort Str = 0, Agi = 0, Vit = 0, Spi = 0;
                GetInitialStats(Job, ref Str, ref Agi, ref Vit, ref Spi);
                ushort HP = (ushort)(Vit * 24 + Str * 3 + Agi * 3 + Spi * 3);
                byte Avatar = 0;
                if (Body == 1003 || Body == 1004 || Body == 2003 || Body == 2004)
                { Avatar = 1; }
                else if (Body == 1001 || Body == 1002 || Body == 2001 || Body == 2002)
                { Avatar = 201; }
                MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.INSERT);
                cmd2.Insert("characters").Insert("Account", Account).Insert("Name", Name).Insert("Avatar", Avatar).Insert("Body", Body).Insert("Hair", (410 + (Program.Rnd.Next(5) * 100))).Insert("Job", Job).Insert("Str", Str).Insert("Agi", Agi).Insert("Vit", Vit).Insert("Spi", Spi).Insert("CurHP", HP).Insert("CPs", CPs).Insert("Silvers", Silvers);
                cmd2.Execute();
                MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd3.Update("accounts").Set("Character", Name).Where("AccountID", Account).Execute();
                uint carid = 0;
                Game.Item Item = new Game.Item();
                MySqlCommand cmd4 = new MySqlCommand(MySqlCommandType.SELECT);
                cmd4.Select("characters").Where("Name", Name);
                MySqlReader rs = new MySqlReader(cmd4);
                if (rs.Read())
                {
                    carid = rs.ReadUInt32("EntityID");
                }
         //       MySqlCommand cmd2222 = new MySqlCommand(MySqlCommandType.INSERT);
         //       cmd2222.Insert("nobility").Insert("Don", 1).Insert("IdEntity", carid).Insert("Name", Name).Execute();
         //       Database.LoadEmpire();
                #region Beginner Items
                if (Job == 100)
                {
                    MySqlCommand cmd6 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd6.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 421301).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                else if (Job == 10)
                {
                    MySqlCommand cmd7 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd7.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 410901).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                else if (Job == 20)
                {
                    MySqlCommand cmd8 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd8.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 410901).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                else if (Job == 40)
                {
                    MySqlCommand cmd9 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd9.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 410901).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                else if (Job == 50)
                {
                    MySqlCommand cmda = new MySqlCommand(MySqlCommandType.INSERT);
                    cmda.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 601301).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                else
                {
                    MySqlCommand cmdb = new MySqlCommand(MySqlCommandType.INSERT);
                    cmdb.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 410301).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                }
                MySqlCommand cmdc = new MySqlCommand(MySqlCommandType.INSERT);
                cmdc.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 132004).Insert("CharID", carid).Insert("Color", Program.Rnd.Next(3, 9)).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();

                MySqlCommand cmdd = new MySqlCommand(MySqlCommandType.INSERT);
                cmdd.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 1000000).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();

                MySqlCommand cmdd2 = new MySqlCommand(MySqlCommandType.INSERT);
                cmdd2.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 1000000).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();

                MySqlCommand cmdd3 = new MySqlCommand(MySqlCommandType.INSERT);
                cmdd3.Insert("items").Insert("ItemUID", Item.UID).Insert("ItemID", 1000000).Insert("CharID", carid).Insert("Dura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Insert("MaxDura", ((DatabaseItem)DatabaseItems[(uint)421301]).Durability).Execute();
                #endregion
                if (Job == 100)
                {
                    MySqlCommand cmdsss5 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmdsss5.Insert("skills").Insert("Level", 0).Insert("Experience", 0).Insert("EntityID", carid)
                        .Insert("Type", "spell").Insert("ID", 1000).Execute();
                    MySqlCommand cmdsss6 = new MySqlCommand(MySqlCommandType.INSERT);
                    cmdsss6.Insert("skills").Insert("Level", 0).Insert("Experience", 0).Insert("EntityID", carid)
                        .Insert("Type", "spell").Insert("ID", 1005).Execute();
                }
            }
            catch { return "Error! Try again."; }
            return "ANSWER_OK";
        }
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