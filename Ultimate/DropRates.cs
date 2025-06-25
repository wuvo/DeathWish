using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ultimate
{
    public class DropRates
    {
        public static double
        GreenEgg = 0.004,
        RedEgg = 0.004,
        EggPacket = 0.001,
        DragonBall = 0.008,
        PowerEXPBall = 0.096,
        plusMeteor = 0.145,
        Meteor = 0.110,
        PlusOne = 0.400,
        Refined = 2,
        Unique = 1.1,
        Elite = 0.07,
        Super = 0.008,
        OneSoc = 12,
        TwoSoc = 6,
        Silver = 12,
        SilverDrop = 1.5,
        SoulStone = 0.8,
        Gem = 0.2,
        GemRef = 1.5,
        GemSup = 0.130,
        CleanWater = 0.18,
        Item = 15,
        MeteorSock1 = 0.17,
        MeteorSock2 = 0.13,

        Meteor1SpamSock1 = 0.25,
        Meteor1SpamSock2 = 0.15,

        Meteor5SpamSock1 = 0.70,
        Meteor5SpamSock2 = 0.55,

        Meteor10SpamSock1 = 5.00,
        Meteor10SpamSock2 = 3.00,
        DBSock1 = 0.8,
        DBSock2 = 0.5,
        LabBoss1 = 2,
        LabBoss2 = 0.6;




        public static Dictionary<int, List<RateItemInfo>> Specifics = new Dictionary<int, List<RateItemInfo>>();
        public static Dictionary<byte, List<uint>> EquipDrops = new Dictionary<byte, List<uint>>();

        public struct RateItemInfo
        {
            public int MonsterID;
            public uint ID;
            public byte Sockets;
            public byte Plus;
            public byte Bless;
            public double DropChance;
        }
        public static void Load()
        {
            //IniFile iNi = new IniFile(@"C:\OldCODB\DropRates.ini");
            //MeteorSock1 = iNi.ReadDouble("Sockets", "1Meteor");
            //MeteorSock2 = iNi.ReadDouble("Sockets", "2Meteor");
            //DBSock1 = iNi.ReadDouble("Sockets", "1DB");
            //DBSock2 = iNi.ReadDouble("Sockets", "2DB");
            //LabBoss1 = iNi.ReadDouble("Sockets", "1LabBoss");
            //LabBoss2 = iNi.ReadDouble("Sockets", "2LabBoss");
            //Meteor1SpamSock1 = iNi.ReadDouble("Sockets", "Meteor1SpamSock1");
            //Meteor1SpamSock2 = iNi.ReadDouble("Sockets", "Meteor1SpamSock2");
            //Meteor5SpamSock1 = iNi.ReadDouble("Sockets", "Meteor5SpamSock1");
            //Meteor5SpamSock2 = iNi.ReadDouble("Sockets", "Meteor5SpamSock2");
            //Meteor10SpamSock1 = iNi.ReadDouble("Sockets", "Meteor10SpamSock1");
            //Meteor10SpamSock2 = iNi.ReadDouble("Sockets", "Meteor10SpamSock2");
            //Gem = iNi.ReadDouble("Mining", "Gem");
            //GemRef = iNi.ReadDouble("Mining", "Refined");
            //GemSup = iNi.ReadDouble("Mining", "Super");
            //if (!Game.World.LowRatedServer)
            //{
            //    DragonBall = iNi.ReadDouble("Rates", "DragonBall");
            //    PowerEXPBall = iNi.ReadDouble("Rates", "PowerEXPBall");
            //    Meteor = iNi.ReadDouble("Rates", "Meteor");
            //    PlusOne = iNi.ReadDouble("Rates", "PlusOne");
            //    Refined = iNi.ReadDouble("Rates", "Refined");
            //    Unique = iNi.ReadDouble("Rates", "Unique");
            //    Elite = iNi.ReadDouble("Rates", "Elite");
            //    Super = iNi.ReadDouble("Rates", "Super");
            //    OneSoc = iNi.ReadDouble("Rates", "OneSoc");
            //    TwoSoc = iNi.ReadDouble("Rates", "TwoSoc");
            //    Silver = iNi.ReadDouble("Rates", "Silver");
            //    SilverDrop = iNi.ReadDouble("Rates", "SilverDrop");
            //    SoulStone = iNi.ReadDouble("Rates", "SoulStone");
            //    CleanWater = iNi.ReadDouble("Rates", "CleanWater");
            //    Item = iNi.ReadDouble("Rates", "Item");
            //}
            //else
            //{
            //    DragonBall = iNi.ReadDouble("LowRates", "DragonBall");
            //    PowerEXPBall = iNi.ReadDouble("LowRates", "PowerEXPBall");
            //    Meteor = iNi.ReadDouble("LowRates", "Meteor");
            //    PlusOne = iNi.ReadDouble("LowRates", "PlusOne");
            //    Refined = iNi.ReadDouble("LowRates", "Refined");
            //    Unique = iNi.ReadDouble("LowRates", "Unique");
            //    Elite = iNi.ReadDouble("LowRates", "Elite");
            //    Super = iNi.ReadDouble("LowRates", "Super");
            //    OneSoc = iNi.ReadDouble("LowRates", "OneSoc");
            //    TwoSoc = iNi.ReadDouble("LowRates", "TwoSoc");
            //    Silver = iNi.ReadDouble("LowRates", "Silver");
            //    SilverDrop = iNi.ReadDouble("LowRates", "SilverDrop");
            //    SoulStone = iNi.ReadDouble("LowRates", "SoulStone");
            //    CleanWater = iNi.ReadDouble("LowRates", "CleanWater");
            //    Item = iNi.ReadDouble("LowRates", "Item");
            //}

            string[] Lines = File.ReadAllLines(@"C:\OldCODB\DropRates.txt");
            int Current = 0;
            List<RateItemInfo> CurArr = null;

            foreach (string Line in Lines)
            {
                if (Line.Length > 0)
                {
                    string[] E = Line.Split(' ');
                    if (E.Length == 1)
                    {
                        Current = int.Parse(E[0]);
                        if (!Specifics.ContainsKey(Current))
                        {
                            CurArr = new List<RateItemInfo>();
                            Specifics.Add(Current, CurArr);
                        }
                    }
                    else
                    {
                        RateItemInfo R = new RateItemInfo();
                        R.MonsterID = Current;
                        R.ID = uint.Parse(E[0]);
                        R.Plus = byte.Parse(E[1]);
                        R.Sockets = byte.Parse(E[2]);
                        R.Bless = byte.Parse(E[3]);
                        R.DropChance = double.Parse(E[4]);
                        CurArr.Add(R);
                    }
                }
            }
            Lines = File.ReadAllLines(@"C:\OldCODB\EquipDrops.txt");
            foreach (string Line in Lines)
            {
                string[] E = Line.Split(' ');
                byte Lev = byte.Parse(E[0]);
                if (!EquipDrops.ContainsKey(Lev))
                    EquipDrops.Add(Lev, new List<uint>());
                uint ID = uint.Parse(E[1]);
                (EquipDrops[Lev]).Add(ID);
            }
        }
    }
}
