using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Features;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using Ultimate.Structures;
using Ultimate.Enum;

namespace Ultimate.Game
{
    public enum PKMode : byte
    {
        PK = 0, Peace, Team, Capture
    }

    public enum StatusEffectEn : ulong
    {
        Normal = 0x0,
        BlueName = 0x1,
        Poisoned = 0x2,
        Gone = 0x4,//invisible
        XPStart = 0x10,
        TeamLeader = 0x40,
        Accuracy = 0x80,
        Shield = 0x100,
        Stigma = 0x200,
        Dead = 0x420,//0x420  sau 400
        FadeAway = 0x800,
        Invisible = 0x400000,
        RedName = 0x4000,
        BlackName = 0x8000,
        SuperMan = 0x40000,
        Cyclone = 0x800000,
        Dodge = 0x4000000,
        Fly = 0x8000000,
        Pray = 0x40000000,
        Blessing = 8589934592,
        TopGuildLeader = 17179869184,
        TopDeputyLeader = 34359738368,
        MonthlyPKChampion = 68719476736,
        WeeklyPKChampion = 137438953472,
        TopWarrior = 274877906944,
        TopTrojan = 549755813888,
        TopArcher = 1099511627776,
        TopWaterTaoist = 2199023255552,
        TopFireTaoist = 4398046511104,
        TopNinja = 8796093022208,
        ShurikenVortex = 70368744177664,
        FatalStrike = 140737488355328,
        Flashy = 281474976710656,
        Ride = 1125899906842624,
        Cursed = 1UL << 32,
        Top3FBSS = 1UL << 44,
        DragonWar = 1UL << 45,
        SparkleHalo = 1UL << 52,
        TopFBSS = 1UL << 53,
        Dazed = 1UL << 54,//no movement
        BlueRestoreAura = 1UL << 55,
        MoveSpeedRecovered = 1UL << 56,
        SuperShieldHalo = 1UL << 57,
        HUGEDazed = 1UL << 58,//no movement
        IceBlock = 1UL << 59, //no movement
        Confused = 1UL << 60//reverses movement
    }

    public enum Status : byte
    {
        HP = 0,
        MaxHP = 1,
        MP = 2,
        MaxMP = 3,
        Silvers = 4,
        Experience = 5,
        PKPoints = 6,
        Class = 7,
        Stamina = 8,
        WHMoney = 9,
        StatPoints = 10,
        Mesh = 11,
        Level = 12,
        Spirit = 13,
        Vitality = 14,
        Strength = 15,
        Agility = 16,
        BlessTime = 17,
        DoubleExpTime = 18,
        CurseTime = 20,
        RebirthCount = 22,
        Effect = 25,
        Hair = 26,
        XPCircle = 27,
        LuckyTime = 28,
        CPs = 29,
        EE1 = 30,
        OnlineTraining = 31,
        PotFromMentor = 36,
        Merchant = 38,
        VIPLevel = 39,
        QuizPts = 40,
        EnlightPoints = 41
    }
    //public class Location2
    //{
    //    Character C;
    //    ushort xX;
    //    ushort xY;
    //    ushort xMap;
    //    public ushort PreviousX;
    //    public ushort PreviousY;
    //    ushort xPreviousMap;
    //    public DateTime LastJump;
    //    public Location2(Character CC)
    //    {
    //        C = CC;
    //    }
    //    public ushort X
    //    {
    //        get { return xX; }
    //        set
    //        {
    //            xX = value;
    //        }
    //    }
    //    public ushort Y
    //    {
    //        get { return xY; }
    //        set
    //        {
    //            xY = value;
    //        }
    //    }
    //    public ushort Map
    //    {
    //        get { return xMap; }
    //        set
    //        {
    //            xMap = value;
    //        }
    //    }
    //    public ushort PreviousMap
    //    {
    //        get { return xPreviousMap; }
    //        set
    //        {
    //            xPreviousMap = value;
    //        }
    //    }
    //    public void Walk(byte Dir)
    //    {
    //        PreviousX = X;
    //        PreviousY = Y;

    //        if (Dir == 0)
    //            Y += 1;
    //        if (Dir == 2)
    //            X -= 1;
    //        if (Dir == 4)
    //            Y -= 1;
    //        if (Dir == 6)
    //            X += 1;
    //        if (Dir == 1)
    //        {
    //            X -= 1;
    //            Y += 1;
    //        }
    //        if (Dir == 3)
    //        {
    //            X -= 1;
    //            Y -= 1;
    //        }
    //        if (Dir == 5)
    //        {
    //            X += 1;
    //            Y -= 1;
    //        }
    //        if (Dir == 7)
    //        {
    //            X += 1;
    //            Y += 1;
    //        }
    //    }
    //    public bool AbleToJump(ushort NX, ushort NY, bool Speed)
    //    {
    //        if (MyMath.PointDistance(NX, NY, X, Y) < 22 && (DateTime.Now > LastJump.AddMilliseconds(450) || Speed))
    //        {
    //            DMap DM = ((DMap)DMaps.H_DMaps[Map]);
    //            if (DM != null)
    //            {
    //                DMapCell New = DM.GetCell(NX, NY);
    //                if (Map == 1038)
    //                {
    //                    DMapCell Old = DM.GetCell(X, Y);
    //                    if (New.High)
    //                        if (!Old.High)
    //                        { return false; }
    //                }
    //                else if (Map == 1844)
    //                {
    //                    if (Y >= 110 && Y <= 226)
    //                    {
    //                        if (X > 134 && X < 171 && NX <= 134)
    //                            return false;
    //                        else if (X > 172 && X < 210 && NX <= 172)
    //                            return false;
    //                        else if (X > 210 && X < 245 && NX <= 210)
    //                            return false;
    //                    }

    //                }
    //                if (!New.NoAccess)
    //                    return true;
    //            }
    //            else return true;
    //            return false;
    //        }
    //        return false;
    //    }
    //    public void Jump(ushort NX, ushort NY)
    //    {
    //        LastJump = DateTime.Now;
    //        PreviousX = X;
    //        PreviousY = Y;
    //        X = NX;
    //        Y = NY;
    //    }
    //    public static implicit operator Location(Location2 l)
    //    {
    //        Location L = new Location()
    //        {
    //            X = l.X,
    //            Y = l.Y,
    //            Map = l.Map,
    //            PreviousMap = l.PreviousMap,
    //            LastJump = l.LastJump,
    //            PreviousX = l.PreviousX,
    //            PreviousY = l.PreviousY
    //        };
    //        return L;
    //    }
    //}
    public struct StoreLoc
    {
        public ushort X;
        public ushort Y;
        public uint Map;
    }
    public struct SpawnLoc
    {
        public ushort XFrom;
        public ushort XTo;
        public ushort YFrom;
        public ushort Yto;
        public uint Map;
    }
    public struct Location
    {
        public ushort X;
        public ushort Y;
        public ushort OldX;
        public ushort OldY;
        public uint OldMap;
        //public ushort Map;
        public uint Map;
        public ushort PreviousX;
        public ushort PreviousY;
        //   public ushort Prev2X;
        // public ushort Prev2Y;
        public uint PreviousMap;
        public DateTime LastJump;

        public void Walk(byte Dir)
        {
            PreviousX = X;
            PreviousY = Y;

            if (Dir == 0)//sw
                Y += 1;
            if (Dir == 2)//nw
                X -= 1;
            if (Dir == 4)//ne
                Y -= 1;
            if (Dir == 6)//se
                X += 1;
            if (Dir == 1)//w
            {
                X -= 1;
                Y += 1;
            }
            if (Dir == 3)//n
            {
                X -= 1;
                Y -= 1;
            }
            if (Dir == 5)//e
            {
                X += 1;
                Y -= 1;
            }
            if (Dir == 7)//s
            {
                X += 1;
                Y += 1;
            }

        }
        public bool AbleToWalkGW(ushort X, ushort Y)
        {
            if (X >= 163 && X <= 166 && Y == 210 && !World.H_SOBs[GuildWars.TheLeftGate.EntityID].Opened)
                return false;
            else if (X == 222 && Y >= 177 && Y <= 180 && !World.H_SOBs[GuildWars.TheRightGate.EntityID].Opened)
                return false;
            return true;
        }
        public bool AbleToJump(ushort NX, ushort NY, bool Cyclone, bool DH)
        {
            int Dist = MyMath.PointDistance(NX, NY, X, Y);
            //  if ((Cyclone && DateTime.Now > LastJump.AddMilliseconds(Dist * 9)) || (DateTime.Now > LastJump.AddMilliseconds(Dist * 14)))
            if (Cyclone || DH ||/* ((Cyclone || DH) && DateTime.Now > LastJump.AddMilliseconds(Dist * 5)) || */DateTime.Now > LastJump.AddMilliseconds(Dist * 11))
                if (((Map != 1038 && Map != 1844) && Dist < 20) || Dist < 18 || Map == 1010)
                {
                    DMap DM = null;
                    if (DMaps.H_DMaps.ContainsKey(Map))
                        DM = DMaps.H_DMaps[Map];
                    if (DM != null)
                    {
                        DMapCell New = DM.GetCell(NX, NY);
                        if (Map == 1038)
                        {
                            if (New.High)
                            {
                                DMapCell Old = DM.GetCell(X, Y);

                                //  DMapCell Older = DM.GetCell(Prev2X, Prev2Y);
                                if (!Old.High)
                                { return false; }
                            }
                        }
                        else if (Map == 1010)
                        {
                            if (New.High)
                            {
                                DMapCell Old = DM.GetCell(X, Y);

                                //  DMapCell Older = DM.GetCell(Prev2X, Prev2Y);
                                if (!Old.High)
                                { return false; }
                            }
                        }
                        else if (Map == 1844)
                        {
                            if (Y >= 110 && Y <= 226)
                            {
                                if (X > 134 && X < 171 && NX <= 134)
                                    return false;
                                else if (X > 172 && X < 210 && NX <= 172)
                                    return false;
                                else if (X > 210 && X < 245 && NX <= 210)
                                    return false;
                            }
                        }
                        if (!New.NoAccess || Map == 1080 || Map == 1017 || Map == 1010 || Map == 1012 || Map == 1105)
                        {
                            return true;
                        }
                    }
                    else { return true; }
                    return false;
                }
            return false;
        }
        public void Jump(ushort NX, ushort NY)
        {
            LastJump = DateTime.Now;
            /*  if (Map == 1038)
              {

                  Prev2X = PreviousX;
                  Prev2Y = PreviousY;

              }*/
            PreviousX = X;
            PreviousY = Y;
            X = NX;
            Y = NY;
        }
    }
    public struct ItemIDManipulation
    {
        uint ID;
        public ItemIDManipulation(uint id)
        {
            ID = id;
        }

        public Item.ItemQuality Quality
        {
            get
            {
                return (Item.ItemQuality)Digit(6);
            }
        }
        public Item.ArmorColor Color
        {
            get
            {
                return (Item.ArmorColor)Digit(4);
            }
        }
        public void QualityChange(Item.ItemQuality Quality)
        {
            ChangeDigit(6, (byte)Quality);
        }
        public void ColorChange(Item.ArmorColor Col)
        {
            ChangeDigit(4, (byte)Col);
        }
        public uint Part(byte From, byte To)
        {
            string Item = Convert.ToString(ID);
            string type = Item.Remove(0, From);
            type = type.Remove(To - From, Item.Length - To);
            return uint.Parse(type);
        }
        public static uint Part(uint ID, byte From, byte To)
        {
            if (ID != 0)
            {
                string Item = Convert.ToString(ID);
                string type = Item.Remove(0, From);
                type = type.Remove(To - From, Item.Length - To);
                return uint.Parse(type);
            }
            return 0;
        }
        public byte Digit(byte Place)
        {
            return (byte)Part((byte)(Place - 1), Place);
        }
        public static byte Digit(uint ID, byte Place)
        {
            return (byte)Part(ID, (byte)(Place - 1), Place);
        }
        public void ChangeDigit(byte Place, byte To)
        {
            string Item = Convert.ToString(ID);
            string N = Item.Remove(Place - 1, Item.Length - Place + 1) + To.ToString();
            N += Item.Remove(0, Place);
            ID = uint.Parse(N);
        }
        public void LowestLevel(byte Pos)
        {
            ChangeDigit(4, 0);
            if (Pos == 1 || Pos == 2 || Pos == 3 || Digit(1) == 9)
                ChangeDigit(5, 0);
            else if (Pos == 8 || Pos == 6)
                ChangeDigit(5, 1);
            else
                ChangeDigit(5, 2);
        }
        public void IncreaseLevel()
        {
            if (ID != 0)
            {
                if (Database.DatabaseItems.ContainsKey(ID))
                {
                    DatabaseItem Item = (DatabaseItem)Database.DatabaseItems[ID];
                    byte Level = Item.LevReq;
                    string Type = Item.ID.ToString().Remove(2, Item.ID.ToString().Length - 2);
                    uint WeirdThing = Convert.ToUInt32(Type);
                    if (WeirdThing <= 60 && WeirdThing >= 42)//weapon
                    {
                        if (Level < 130)
                        {
                            if (Level >= 120)
                            {
                                Level++;
                                foreach (DatabaseItem I in Database.DatabaseItems.Values)
                                {
                                    if (I.ID / 1000 == Item.ID / 1000)
                                        if (I.ID % 10 == Item.ID % 10)
                                            if (I.LevReq == Level)
                                            { ID = I.ID; return; }
                                }
                            }
                            else
                            {
                            Again:
                                Level++;
                                foreach (DatabaseItem I in Database.DatabaseItems.Values)
                                {
                                    if (I.ID / 1000 == Item.ID / 1000)
                                        if (I.ID % 10 == Item.ID % 10)
                                            if (I.LevReq == Level)
                                            { ID = I.ID; return; }
                                }
                                goto Again;
                            }
                        }
                    }
                    else
                    {
                        if (WeirdThing == 20)
                            return;
                        Again:
                        Level++;
                        foreach (DatabaseItem I in Database.DatabaseItems.Values)
                        {
                            if (I.ID / 1000 == Item.ID / 1000)
                                if (I.ID % 10 == Item.ID % 10)
                                    if (I.LevReq == Level)
                                    { ID = I.ID; return; }
                        }
                        goto Again;
                    }
                }
            }
        }
        public void LowerLevel()
        {
            if (ID != 0)
            {
                if (Database.DatabaseItems.ContainsKey(ID))
                {
                    DatabaseItem Item = (DatabaseItem)Database.DatabaseItems[ID];
                    byte Level = Item.LevReq;
                    string Type = Item.ID.ToString().Remove(2, Item.ID.ToString().Length - 2);
                    uint WeirdThing = Convert.ToUInt32(Type);
                    if (WeirdThing <= 60 && WeirdThing >= 42)//weapon
                    {
                        if (Level <= 130)
                        {
                            if (Level >= 120)
                            {
                                Level--;
                                foreach (DatabaseItem I in Database.DatabaseItems.Values)
                                {
                                    if (I.ID / 1000 == Item.ID / 1000)
                                        if (I.ID % 10 == Item.ID % 10)
                                            if (I.LevReq == Level)
                                            { ID = I.ID; return; }
                                }
                            }
                            else
                            {
                            Again:
                                Level++;
                                foreach (DatabaseItem I in Database.DatabaseItems.Values)
                                {
                                    if (I.ID / 1000 == Item.ID / 1000)
                                        if (I.ID % 10 == Item.ID % 10)
                                            if (I.LevReq == Level)
                                            { ID = I.ID; return; }
                                }
                                goto Again;
                            }
                        }
                    }
                    else
                    {
                        if (WeirdThing == 20)
                            return;
                        Again:
                        Level--;
                        foreach (DatabaseItem I in Database.DatabaseItems.Values)
                        {
                            if (I.ID / 1000 == Item.ID / 1000)
                                if (I.ID % 10 == Item.ID % 10)
                                    if (I.LevReq == Level)
                                    { ID = I.ID; return; }
                        }
                        goto Again;
                    }
                }
            }
        }
        public uint ToID()
        {
            return ID;
        }
        public uint ToComposeID(byte EqPos)
        {
            uint id = ID;
            byte itemType = (byte)(ID / 10000);
            ushort itemType2 = (ushort)(ID / 1000);
            if (itemType == 14 && itemType2 != 142 && itemType2 != 141)//armors
            {
                ID = (uint)(
                            (((uint)(ID / 1000)) * 1000) + // [3] = 0
                            ((ID % 100) - (ID % 10)) // [5] = 0
                        );
            }
            else if (itemType == 13 || itemType == 90 || itemType == 11 || itemType2 == 123 || itemType == 30 || itemType == 20 || itemType == 12 || itemType == 15 || itemType == 16 || itemType == 50 || itemType2 == 421 || itemType2 == 601 || itemType2 == 141 || itemType2 == 142)//Necky bow bag
            {
                ID = (uint)(
                            ID - (ID % 10) // [5] = 0
                        );
            }
            else
            {
                byte head = (byte)(ID / 100000);
                ID = (uint)(
                        ((head * 100000) + (head * 10000) + (head * 1000)) + // [1] = [0], [2] = [0]
                        ((ID % 1000) - (ID % 10)) // [5] = 0
                    );
            }
            uint ret = ID;//incearca cu asta k
            ID = id;
            return ret;
        }
    }

    public class MEffect
    {
        private static uint ItemUIDStart = 1;
        private static uint ItemUIDFinish = uint.MaxValue - 1;
        private static uint ItemNextID
        {
            get
            {
                if (ItemUIDStart == ItemUIDFinish)
                    ItemUIDFinish = 1;
                return ++ItemUIDStart;
            }
        }
        public bool GenNewID = true;
        public uint ID;
        uint uID;
        public uint UID
        {
            get { if (ID == 0) return 0; return uID; }
            set
            {
                if (GenNewID)
                    uID = ItemNextID;
                else uID = value;
            }
        }

    }
    public enum Ranks : byte { Serf = 0, Knight = 1, Baron = 3, Earl = 5, Duke = 7, Prince = 9, King = 12 }

    public class Prof//struct
    {
        public ushort ID;
        public byte Lvl;
        public uint Exp;
        public byte PreviousLevel;

        public void WriteThis(System.IO.BinaryWriter I)
        {
            I.Write(ID);
            I.Write(Lvl);
            I.Write(Exp);
        }
        public void ReadThis(System.IO.BinaryReader I)
        {
            ID = I.ReadUInt16();
            Lvl = I.ReadByte();
            Exp = I.ReadUInt32();

        }
    }
    public class Skill
    {
        public ushort ID;
        public byte Lvl;
        public uint Exp;
        public byte PreviousLevel;

        public SkillsClass.SkillInfo Info
        {
            get
            {
                if (Features.SkillsClass.SkillInfos.ContainsKey(ID + " " + Lvl))
                    return (Features.SkillsClass.SkillInfo)Features.SkillsClass.SkillInfos[ID + " " + Lvl];
                return new SkillsClass.SkillInfo();
            }
        }
        public void WriteThis(System.IO.BinaryWriter I)
        {
            I.Write(ID);
            I.Write(Lvl);
            I.Write(Exp);
        }
        public void ReadThis(System.IO.BinaryReader I)
        {
            ID = I.ReadUInt16();
            Lvl = I.ReadByte();
            Exp = I.ReadUInt32();

        }
    }
    public struct Friend
    {
        public uint UID;
        string name;

        public void WriteThis(System.IO.BinaryWriter I)
        {
            I.Write(UID);
            I.Write(name);
        }
        public void ReadThis(System.IO.BinaryReader I)
        {
            UID = I.ReadUInt32();
            name = I.ReadString();

        }

        public bool Online
        {
            get
            {
                return World.H_Chars.ContainsKey(UID);
            }
        }
        public Character Info
        {
            get
            {
                if (Online)
                    return World.H_Chars[UID];
                return null;
            }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    public struct Enemy
    {
        public uint UID;
        string name;

        public void WriteThis(System.IO.BinaryWriter I)
        {
            I.Write(UID);
            I.Write(name);
        }
        public void ReadThis(System.IO.BinaryReader I)
        {
            UID = I.ReadUInt32();
            name = I.ReadString();
        }
        public bool Online
        {
            get
            {
                return World.H_Chars.ContainsKey(UID);
            }
        }
        public Character Info
        {
            get
            {
                if (Online)
                    return World.H_Chars[UID];
                return null;
            }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }

    public class Nobility
    {
        private Character C;
        public ulong Donation;
        public int ListPlace;
        private Ranks NobilityID;
        public Nobility(Character c)
        {
            C = c;
        }
        public Ranks Rank
        {
            get { return NobilityID; }
            set
            {
                NobilityID = value;
                if (C.Loaded)
                    C.SendScreen(Packets.Donators(C));
            }
        }
    }
    public enum MerchantTypes : byte
    {
        No = 0,
        Ask = 1,
        Yes = 255
    }

    //public struct DataThread
    //{
    //    public Character[] Array;
    //    public bool Modified;
    //}
    //public struct CopiedChar
    //{
    //    string Acc;
    //    ushort Avatar;
    //    ushort Body;
    //    ushort Hair;
    //    // ushort LocMap;
    //    ushort LocX;
    //    ushort LocY;
    //    // ushort LocPreviousMap;
    //    byte Job;
    //    byte PreviousJob1;
    //    byte Level;
    //    ulong Experience;
    //    ushort Str;
    //    ushort Agi;
    //    ushort Vit;
    //    ushort Spi;
    //    ushort StatPoints;
    //    ulong NobilityDonation;
    //    uint Silvers;
    //    uint WHSilvers;
    //    ulong VP;
    //    ushort PKPoints;
    //    List<Item> Equips;
    //    List<Item> Inventory;
    //    List<Item> TCWarehouse;
    //    List<Item> PCWarehouse;
    //    List<Item> ACWarehouse;
    //    List<Item> DCWarehouse;
    //    List<Item> BIWarehouse;
    //    List<Item> SCWarehouse;
    //    List<Item> MAWarehouse;
    //    List<Item> MAWarehouse2;
    //    List<Item> HouseWH1;
    //    List<Item> HouseWH2;
    //    //Hashtable Skills;
    //    // Hashtable Profs;
    //    byte Reborns;
    //    Dictionary<ushort, Prof> ProfsBeforeReborn;
    //    Dictionary<ushort, Skill> SkillsBeforeReborn;

    //    public void CopyChar(Character C)
    //    {
    //        Acc = C.MyClient.AuthInfo.Account;
    //        Avatar = C.Avatar;
    //        Body = C.Body;
    //        Hair = C.Hair;//Hair
    //        //LocMap = C.Loc.Map;//Map
    //        LocX = C.Loc.X;//X
    //        LocY = C.Loc.Y;//Y
    //                       // LocPreviousMap = C.Loc.PreviousMap;//Previous Map
    //        Job = C.Job;
    //        PreviousJob1 = C.PreviousJob1;//Previous Job, 1st RB
    //        Level = C.Level;//Level
    //        Experience = C.Experience;//Experience        
    //        Str = C.Str;
    //        Agi = C.Agi;
    //        Vit = C.Vit;
    //        Spi = C.Spi;
    //        StatPoints = C.StatPoints;//Stat Points
    //        NobilityDonation = C.Nobility.Donation;
    //        Silvers = C.Silvers;//Silvers
    //        WHSilvers = C.WHSilvers;//Warehouse Silvers
    //        VP = C.VP;//Virtue Points
    //        PKPoints = C.PKPoints;//PK Points
    //        Equips = new List<Item>();
    //        Equips.Add(C.Equips.HeadGear);
    //        Equips.Add(C.Equips.Necklace);
    //        Equips.Add(C.Equips.Armor);
    //        Equips.Add(C.Equips.RightHand);
    //        Equips.Add(C.Equips.LeftHand);
    //        Equips.Add(C.Equips.Ring);
    //        Equips.Add(C.Equips.Boots);
    //        Equips.Add(C.Equips.Garment);
    //        Inventory = C.Inventory;
    //        TCWarehouse = C.Warehouses.TCWarehouse;
    //        PCWarehouse = C.Warehouses.PCWarehouse;
    //        ACWarehouse = C.Warehouses.ACWarehouse;
    //        DCWarehouse = C.Warehouses.DCWarehouse;
    //        BIWarehouse = C.Warehouses.BIWarehouse;
    //        SCWarehouse = C.Warehouses.SCWarehouse;
    //        MAWarehouse = C.Warehouses.MAWarehouse;
    //        MAWarehouse2 = C.Warehouses.MAWarehouse2;
    //        HouseWH1 = C.Warehouses.HouseWH1;
    //        HouseWH2 = C.Warehouses.HouseWH2;
    //        //Skills = C.Skills;
    //        // Profs = C.Profs;
    //        Reborns = C.Reborns;
    //        ProfsBeforeReborn = C.ProfsBeforeReborn;
    //        SkillsBeforeReborn = C.SkillsBeforeReborn;
    //    }
    //    public void SendChar(Character C2)
    //    {
    //        C2.MyClient.AuthInfo.Account = Acc;
    //        C2.Avatar = Avatar;
    //        C2.Body = Body;
    //        C2.Hair = Hair;//Hair
    //                       // C2.Loc.Map = LocMap;//Map
    //        C2.Loc.X = LocX;//X
    //        C2.Loc.Y = LocY;//Y
    //                        // C2.Loc.PreviousMap = LocPreviousMap;//Previous Map
    //        C2.Job = Job;
    //        C2.PreviousJob1 = PreviousJob1;//Previous Job, 1st RB
    //        C2.Level = Level;//Level
    //        C2.Experience = Experience;//Experience        
    //        C2.Str = Str;
    //        C2.Agi = Agi;
    //        C2.Vit = Vit;
    //        C2.Spi = Spi;
    //        C2.StatPoints = StatPoints;//Stat Points
    //        C2.Nobility.Donation = NobilityDonation;
    //        C2.Silvers = Silvers;//Silvers
    //        C2.WHSilvers = WHSilvers;//Warehouse Silvers
    //        C2.VP = VP;//Virtue Points
    //        C2.PKPoints = PKPoints;//PK Points
    //        C2.Equips.HeadGear = Equips[0];
    //        C2.Equips.Necklace = Equips[1];
    //        C2.Equips.Armor = Equips[2];
    //        C2.Equips.RightHand = Equips[3];
    //        C2.Equips.LeftHand = Equips[4];
    //        C2.Equips.RightHand = Equips[5];
    //        C2.Equips.Boots = Equips[6];
    //        C2.Equips.Garment = Equips[7];
    //        C2.Inventory = Inventory;
    //        C2.Warehouses.TCWarehouse = TCWarehouse;
    //        C2.Warehouses.PCWarehouse = PCWarehouse;
    //        C2.Warehouses.ACWarehouse = ACWarehouse;
    //        C2.Warehouses.DCWarehouse = DCWarehouse;
    //        C2.Warehouses.BIWarehouse = BIWarehouse;
    //        C2.Warehouses.SCWarehouse = SCWarehouse;
    //        C2.Warehouses.MAWarehouse = MAWarehouse;
    //        C2.Warehouses.MAWarehouse2 = MAWarehouse2;
    //        C2.Warehouses.HouseWH1 = HouseWH1;
    //        C2.Warehouses.HouseWH2 = HouseWH2;
    //        // C2.Skills = Skills;
    //        //C2.Profs = Profs;
    //        C2.Reborns = Reborns;
    //        C2.ProfsBeforeReborn = ProfsBeforeReborn;
    //        C2.SkillsBeforeReborn = SkillsBeforeReborn;
    //    }
    //}
    public class Character
    {
        public bool RemoveAfter = false;
        public DateTime RemoveStamp;
        public bool RemoveAfter1 = false;
        public DateTime RemoveStamp1;
        internal DateTime JumpingStamp;
        internal int CountSpeedHack;
        internal DateTime PreviousJump;
        public enum JobName : byte
        {
            InternTrojan = 10,
            Trojan = 11,
            VeteranTrojan = 12,
            TigerTrojan = 13,
            DragonTrojan = 14,
            TrojanMaster = 15,
            InternWarrior = 20,
            Warrior = 21,
            BrassWarrior = 22,
            SilverWarrior = 23,
            GoldWarrior = 24,
            WarriorMaster = 25,
            InternArcher = 40,
            Archer = 41,
            EagleArcher = 42,
            TigerArcher = 43,
            DragonArcher = 44,
            ArcherMaster = 45,
            InternNinja = 50,
            Ninja = 51,
            MiddleNinja = 52,
            DarkNinja = 53,
            MysticNinja = 54,
            NinjaMaster = 55,
            Taoist = 101,
            WaterTaoist = 132,
            WaterWizard = 133,
            WaterMaster = 134,
            WaterSaint = 135,
            FireTaoist = 142,
            FireWizard = 143,
            FireMaster = 144,
            FireSaint = 145
        }
        public bool TeamWhiteGarment = true;
        public byte WarriorDodge = 0;
        public bool LogOff = false;
        public bool Saving = false;
        public byte List = 0;
        public ushort DBScrolls = 0;
        public byte MetScrolls = 0;
        public byte DragonGems = 0;
        public byte PhoenixGems = 0;
        public byte RainbowGems = 0;
        public byte KylinGems = 0;
        public byte FuryGems = 0;
        public byte VioletGems = 0;
        public byte MoonGems = 0;
        public byte TortoiseGems = 0;
        public byte Dragonballs = 0;
        public byte Ultimates = 0;
        public ushort GarmentToken = 0;
        public bool Voted = false;
        public byte VotePoints = 0;
        public ushort PKTHits = 0;
        public bool Muted
        {
            get
            {
                if (MutedDays > 0)
                    return true;
                return false;
            }
            set
            {
                Muted = value;
            }
        }
        public byte MutedDays = 0;
        public ushort MutedRecord = 0;
        public int ClassicPoints = 0; //gump-------
        public byte BI_Quest = 0; // 0=not active 1=active
        public int BI_Quest_Kills = 0;
        public byte BI_Quest_Completed = 0; // soft max 3
        public bool AC_Quest_Hops = false;
        public bool AC_Quest_Hops_Completed = false;
        public bool DailyQuestActive = false;
        public bool DailyQuestCompleted = false;
        public int DailyQuestKills = 0;
        public DateTime DailyQuestDate = DateTime.Today;
        public bool AutoHuntEnabled = false;
        private DateTime nextAutoHunt = DateTime.Now;
        public bool VIPMiningSkipOres = false;
        public bool VIPAura = false;
        public bool CountEffect = true;
        public bool GemEffectsRemove = true;
        public bool skipdragongem = false;
        public bool skiphoenixgem = false;
        public bool skipkylingem = false;
        public bool skipvioletgem = false;
        public bool skiprainbowgem = false;
        public bool skipfurygem = false;
        public bool skipmoongem = false;
        public bool skipgreenegg = false;
        public bool skipredegg = false;
        public bool skipmeteor = false;
        public bool skipelite = false;
        public bool skipsuper = false;
        public bool skipallgems = false;
        //public bool BOTJailed = false;
        public bool BOTJailed
        {
            get
            {
                if (BOTJailedDays > 0)
                    return true;
                return false;
            }
        }
        public byte BOTJailedDays = 0;
        public byte VIPLevelToReceive = 0;
        public byte VIPDaysToReceive = 0;
        public ushort TreasurePoints = 0;
        public ushort CTBPoints = 0;
        public uint RemoveFurniture = 0;
        public byte LHouse = 0;
        public List<string> IPMuted = new List<string>();
        //public AI Opponent = null;
        //public AI Target = null;

        public int GetAvailableInventorySlots()
        {
            // Assuming inventory capacity is 40
            int maxInventorySlots = 40;
            int usedSlots = Inventory.Count;
            return maxInventorySlots - usedSlots;
        }

        //  public List<uint> ScreenChars;
        public ConcurrentDictionary<uint, Character> ScreenChars;
        public bool Warning = false;
        public bool Ghost = false;
        public bool PassiveSkills = true;
        public bool Mining = false;
        public bool CanReflect = false;
        public bool Flying = false;
        public ushort PumpkinPoints = 0;
        public Item VIPUsage;
        public DateTime LastMine = DateTime.Now;
        public DateTime ProtectTime = DateTime.Now;
        public bool CancelProtectTime = false;
        public int garment = 0;
        public DateTime LastBuffRemove = DateTime.Now;
        public DateTime LastWorldMsg = DateTime.Now;
        public DateTime LastUpgrade = DateTime.Now;
        public bool DH = false;
        public int DisToKill = 600;
        public DateTime ExpPotionUsed = DateTime.Now;
        //public DateTime LoggedOn = DateTime.Now;
        public DateTime LastLogin;// = DateTime.Now;
        public bool Invisible = false;
        public byte Roll = 0;
        public double TrainTimeLeft;
        public bool InOTG = false;
        public bool Lottery = false;
        public byte VIPDays = 0;
        public Companion MyCompanion;
        public byte ExperienceRate = 2;

        public uint Flowers = 0;
        public bool RecordAction = false;


        public bool Tank = true;
        public bool PrevTank = true;
        public bool CheckTank = true;
        public int TimeBuff = 0;
        public byte SuperGem = 0;
        public byte addBless = 0;
        public byte LotteryUsed = 0;
        private byte _Reborns;
        public bool Protection = false;
        public bool DoubleExp;
        public int DoubleExpLeft;
        public int BlessingLasts;
        public DateTime BlessingStarted;
        public DateTime VIPStarted;
        public DateTime VIP /*= new DateTime(1970, 1, 1)*/;
        public DateTime LastVIPMessage;
        public int PrayTimeLeft;
        public bool Online = false;
        public bool ExpPotUnder70 = false;
        public byte Pervade = 0;
        public bool HasBag = false;
        public bool RedTeam = false;
        public bool BlueTeam = false;
        public bool Invitations = false;
        public DateTime StigBow;
        public QualifierMatch ArenaQualifier;
        public Arena Arena;
        //public Events.PVPEvents EventBase;
        public Events.Events EventBase;
        public DateTime LoginTime = DateTime.Now;
        public DateTime _anticheat = DateTime.Now;
        public int Shots = 0;
        public int Hits = 0;
        public int Chains = 0;
        public int MaxChains = 0;
        public int PentaKill = 0;
        public bool Hit = false;
        public DateTime LastVote;
        public string OnBounty = "";
        public byte Page = 0;
        public bool KilledBounty = false;
        public int ExtraDex = 0;
        public byte TopFB = 0;
        public uint OnlineTime = 0;
        public ScreenSize Screen;
        public byte Range()
        {
            if (Screen == ScreenSize.Windowed)
                return 18;
            return 28;
        }
        public bool ToUpdate = false;
        public bool noobPlvl = false;
        public DateTime LastRequest;
        public uint noobID;
        public uint Inviting;
        public uint Dueler;
        public ushort CurrentKills;
        public Cloudsaint.MonsterType ToKill;
        public MsgDice MsgDice;
        public string Account;
        public string Class
        {
            get
            {
                switch (Job)
                {
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        return "Trojan";
                    //return "InternTrojan";
                    //return "VeteranTrojan";
                    //return "TigerTrojan";
                    //return "DragonTrojan";
                    //return "TrojanMaster";
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                        return "Warrior";
                    //return "InternWarrior";
                    //return "BrassWarrior";
                    //return "SilverWarrior";
                    //return "GoldWarrior";
                    //return "WarriorMaster";
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                        return "Archer";
                    //return "InternArcher";
                    //return "EagleArcher";
                    //return "TigerArcher";
                    //return "DragonArcher";
                    //return "ArcherMaster";
                    case 100:
                    case 101:
                        return "Taoist";
                    case 132:
                    case 133:
                    case 134:
                    case 135:
                        return "Water";
                    //return "WaterWizard";
                    //return "WaterMaster";
                    //return "WaterSaint";
                    case 142:
                    case 143:
                    case 144:
                    case 145:
                        return "Fire";
                        //return "FireWizard";
                        //return "FireMaster";
                        //return "FireSaint";
                }
                return "None";
            }
        }
        public uint CostumerPage = 0;
        public byte ArenaPage = 0;
        public uint Garment = 0;
        public bool Female = false;
        public ushort Version = 0;


        //public int ArenaPoints = 1500;
        public byte WinsToday = 0;
        public byte LossesToday = 0;
        public uint WinsTotal = 0;
        public uint LossesTotal = 0;
        public uint TotalHonor = 0;
        public uint CurrentHonor = 0;

        public Nobility Nobility;
        public int DisCityMobs = 0;

        public DateTime LastPts = DateTime.Now;

        public uint RequestFriends = 0;
        //Trade <<
        public uint TradingWith = 0;
        public uint OldTradingWith = 0;
        public string OldTradingWithName = "";
        public List<uint> TradeSide = new List<uint>(20);
        public uint TradingSilvers = 0;
        public bool Trading = false;
        public bool ClickedOK = false;
        public uint DragonDamage = 0;
        public uint DragonHeal = 0;
        // >>

        public byte ExpBallsUsedToday = 0;
        public Random Rnd = new Random();
        public int PrevXPKO = 0;

        public int TotalKO = 0;
        public int TotalKills = 0;
        public int Kills = 0;
        public int Deaths = 0;
        public bool Alive = true;
        public byte Direction = 0;
        public byte Action = 100;
        public uint EntityID;
        public Location Loc;
        public StoreLoc SLoc;
        public Intensify Intensify;
        public Transformation Transformation;
        public bool CHat2011 = false;
        public int TotalDemonBoxes = 0;
        public StatusEffect StatEff;
        public PKMode PKMode = PKMode.Capture;
        bool _BlueName = false;
        public int Top = 0;
        public Features.Team MyTeam;
        public bool TeamLeader = false;
        public AttackMemorise AtkMem;
        byte _Stamina = 0;
        //  ushort _Vigor = 0;
        public uint Donation;
        //public QuizShow.Info QuizShowInfo;
        public ushort GuildID
        {
            get
            {
                if (MyGuild == null)
                    return 0;
                else
                    return MyGuild.GuildID;
            }
        }
        public Guild MyGuild;
        public EquipStats EqStats;
        public Main.GameClient MyClient;
        public string Name;
        private string spouse = "None";
        public string Spouse
        {
            get { return spouse; }
            set
            {
                spouse = value;
            }
        }
        string wHPassword = "0";
        public string WHPassword
        {
            get { return wHPassword; }
            set
            {
                wHPassword = value;
            }

        }
        public string TempPass = "";
        public int WHErrors = 0;
        public bool WHOpen = false;
        public bool Loaded = false;
        public DateTime BlueNameStarted;
        public byte BlueNameLasts;
        public DateTime LastPKPLost = DateTime.Now;
        public DateTime LastXP = DateTime.Now;
        public DateTime LastStamina = DateTime.Now;
        public DateTime LastMove = DateTime.Now;
        public DateTime DeathHit = DateTime.Now;
        private uint _UniversityPoints = 0;
        byte _PreviousJob;
        public byte PreviousJob1
        {
            get { return _PreviousJob; }
            set { _PreviousJob = value; }
        }
        byte _PreviousJob2;
        public byte PreviousJob2
        {
            get { return _PreviousJob2; }
            set { _PreviousJob2 = value; }
        }
        public bool Reborn
        {
            get { return Reborns > 0; }
        }
        public ulong VP;

        byte viplevel = 0;
        public byte VipLevel
        {
            get { return viplevel; }
            set
            {
                viplevel = value;
                if (Loaded)
                {
                    //if (MyClient != null)
                    MyClient.AddSend(Packets.Status(EntityID, Status.VIPLevel, value));
                }
            }
        }
        ushort _Avatar;
        ushort _Body;
        ushort _Hair;
        public MerchantTypes Merchant;
        byte _Job;
        byte _Level;
        ulong _Experience;
        ushort _Str;
        ushort _Agi;
        ushort _Vit;
        ushort _Spi;
        ushort _StatPoints;
        ushort _CurHP;
        ushort _CurMP;
        uint _Silvers;
        uint _WHSilvers;
        ushort _PKPoints;
        uint _CPs;
        public uint LuckyTime = 0;
        public bool GettingLuckyTime = false;
        public bool Prayer = false;
        public Character ThePrayer;

        public DateTime PrayDT;
        public DateTime UnableToUseDrugs;
        public ushort UnableToUseDrugsFor;

        public PoisonType PoisonedInfo;

        public bool VortexOn = false;
        public DateTime LastVortexAttk = DateTime.Now;

        public bool LoadedEquipmentHPAdd = false;

        public uint GuildDonation;
        public GuildRank GuildRank;
        public MemberInfo MembInfo;

        public Equipment Equips;
        public List<Item> Inventory;
        public Banks Warehouses;
        public ConcurrentDictionary<ushort, Skill> Skills;
        public ConcurrentDictionary<ushort, Prof> Profs;
        //public Hashtable Profs;
        public Dictionary<ushort, Prof> ProfsBeforeReborn;
        public Dictionary<ushort, Skill> SkillsBeforeReborn;
        public Dictionary<uint, Friend> Friends;
        public Dictionary<uint, Enemy> Enemies;

        //public System.Collections.Concurrent.ConcurrentBag<Buff> Buffs;
        // public List<Buff> Buffs;
        public ConcurrentDictionary<Buff, ushort> Buffs;
        //public ConcurrentBag<Buff> BDelete;
        public ConcurrentDictionary<Buff, ushort> BDelete;

        public PersonalShops.Shop MyShop;

        public bool Superman
        {
            get
            {
                if (StatEff.Contains(StatusEffectEn.SuperMan))
                    return true;
                return false;
            }
        }

        public bool Cyclone
        {
            get
            {
                if (StatEff.Contains(StatusEffectEn.Cyclone))
                    return true;
                return false;
            }
        }

        public byte Reborns
        {
            get { return _Reborns; }
            set
            {
                _Reborns = value;
                if (Loaded)
                {
                    SendScreen(Packets.Status(EntityID, Status.RebirthCount, Reborns));
                }
            }
        }
        public void SendScreen(COPacket Data)
        {
            // Game.Character[] Chars = Game.World.H_Chars.Values.ToArray();   ---- maybe good?
            // Game.Character[] Chars = new Game.Character[Game.World.H_Chars.Count];
            //Game.World.H_Chars.Values.CopyTo(Chars, 0);
            /* foreach (Character C in World.H_Chars.Values)
                 if (MyMath.InBox(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y, 28))
                     C.MyClient.AddSend(Data);*/
            /* if (this != null)
                 if (ScreenChars != null)*/
            if (!Invisible)
                foreach (Character C in ScreenChars.Values)
                    C.MyClient.AddSend(Data);
        }

        /// <summary>
        /// Call this to move the player to interserver (through command or NPC)
        /// </summary>
        /// <param name="isNa"></param>
        //public void ConnectToInterserver(bool isNa = true)
        //{

        //    //Reminder: When using your interserver command, check what server they should be sent to. Make a gm command or something to toggle which is used.
        //    if (!Alive)
        //        return;//Check if they are somewhere they shouldn't exist such as events, war, etc, if so dont let them and tell them why


        //    if (!InterserverManager.Send(Ultimate.PacketHandling.WhoIsPacket.Create(this), isNa))
        //        return;//If we cannot connect to the interserver, dont disconnect us!

        //    //Tell the client all their inventory is gone
        //    foreach (Game.Item item in Inventory)
        //        MyClient.AddSend(Packets.ItemPacket(item.UID, 0, 3));//delete

        //    //We should probably also delete all their skills... but w/e lets leave it for now.

        //    //Tell the client all their equipped items are gone
        //    for (byte i = 0; i < 12; i++)
        //    {

        //        var item = Equips.Get(i);
        //        if (item != null && item.UID > 0)
        //        {
        //            MyClient.AddSend(Packets.ItemPacket(item.UID, i, 6));//unequip
        //            MyClient.AddSend(Packets.ItemPacket(item.UID, 0, 3));//delete
        //        }
        //    }

        //    MyClient.AddSend(Packets.GeneralData(EntityID, 0, 0, 0, 118));//Cancel Transformation

        //    MyClient.AddSend(Packets.Status(EntityID, Status.Effect, 0));//Remove ALL visible effects
        //    MyClient.AddSend(Packets.InterserverAuthentication(EntityID, 2, isNa ? InterserverManager.INTERSERVER_AMERICA : InterserverManager.INTERSERVER_EUROPE, 5819));
        //    MyClient.Disconnect();//This logs us out so there's no ghost copy for people to PK
        //}

        public ushort Potency
        {
            get
            {
                //string s = ((ushort)Equips.HeadGear.Soc1).ToString();
                int prePotency = 0;
                prePotency += Level + 5 * Reborns;
                for (byte x = 1; x < 12; x++)
                {
                    Item I = Equips.Get(x);
                    if (I.UID != 0)
                        prePotency += I.Pot;
                }
                prePotency += (byte)Nobility.Rank;
                return (ushort)prePotency;
            }
        }
        public byte LevReqForPromote
        {
            get
            {
                sbyte n = -1;
                if (Job >= 10 && Job <= 15)
                    n = (sbyte)(Job - 10);
                else if (Job >= 20 && Job <= 25)
                    n = (sbyte)(Job - 20);
                else if (Job >= 40 && Job <= 45)
                    n = (sbyte)(Job - 40);
                else if (Job >= 50 && Job <= 55)
                    n = (sbyte)(Job - 50);
                else if (Job >= 100)
                {
                    if (Job <= 101)
                        n = (sbyte)(Job - 100);
                    else if (Job >= 132 && Job <= 135)
                        n = (sbyte)(Job - 130);
                    else if (Job >= 142 && Job <= 145)
                        n = (sbyte)(Job - 140);
                }
                if (n == 0)
                    return 15;
                else if (n == 1)
                    return 40;
                else if (n == 2)
                    return 70;
                else if (n == 3)
                    return 100;
                else if (n == 4)
                    return 110;
                else
                    return 0;
            }
        }
        public uint UniversityPoints
        {
            get { return _UniversityPoints; }
            set
            {
                _UniversityPoints = value;
                if (Loaded)
                {
                    SendScreen(Packets.Status(EntityID, Status.QuizPts, _UniversityPoints));
                }
            }
        }
        byte xpko = 0;
        public byte XPKO
        {
            get { return xpko; }
            set
            {
                xpko = value;
                if (Loaded)
                {
                    if (xpko >= 100 && Alive)
                    {
                        XPKO = 0;
                        //  StatEff.Add(StatusEffectEn.XPStart);
                        Buff B = new Buff();
                        B.StEff = StatusEffectEn.XPStart;
                        B.Lasts = 20;
                        B.Started = DateTime.Now;
                        B.Eff = Features.SkillsClass.ExtraEffect.None;
                        B.Value = 20;
                        AddBuff(B);
                        //  Buffs.Add(new Buff() { StEff = StatusEffectEn.XPStart, Lasts = 20, Started = DateTime.Now, Eff = Features.SkillsClass.ExtraEffect.None });
                    }
                    MyClient.AddSend(Packets.Status(EntityID, Status.XPCircle, xpko));

                }
            }
        }
        public byte Stamina
        {
            get { return _Stamina; }
            set
            {
                _Stamina = value;
                if (_Stamina > 100) Stamina = 100;// go back to the current error

                if (Loaded)
                    MyClient.AddSend(Packets.Status(EntityID, Status.Stamina, _Stamina));
            }
        }
        /*   public ushort Vigor
           {
               get { return _Vigor; }
               set
               {
                   _Vigor = value;
                   if (_Vigor > MaxVigor) _Vigor = MaxVigor;
                   if (Loaded)
                       MyClient.AddSend(Packets.Vigor(_Vigor));
               }
           }
           public ushort MaxVigor
           {
               get { return (ushort)(30 + EqStats.AddVigor); }
           }*/
        public ushort Avatar
        {
            get { return _Avatar; }
            set
            {
                _Avatar = value;
                if (Loaded)
                {
                    SendScreen(Packets.Status(EntityID, Status.Mesh, uint.Parse(_Avatar.ToString() + _Body.ToString())));
                }
            }
        }
        public ushort Body
        {
            get { return _Body; }
            set
            {
                _Body = value; if (Loaded)
                {
                    SendScreen(Packets.Status(EntityID, Status.Mesh, uint.Parse(_Avatar.ToString() + _Body.ToString())));
                }
            }
        }
        public uint TransID = 0;
        public uint Mesh
        {
            get
            {
                //return uint.Parse(_Avatar.ToString() + _Body.ToString());
                if (Alive)
                    return (uint)(TransID * 10000000 + _Avatar * 10000 + _Body);
                else
                {
                    if (Body == 1003 || Body == 1004)
                        return uint.Parse(Convert.ToString(Avatar) + 1098.ToString());
                    else
                        return uint.Parse(Convert.ToString(Avatar) + 1099.ToString());
                }
            }
        }
        public ushort Hair
        {
            get { return _Hair; }
            set
            {
                _Hair = value;
                if (Loaded)
                {
                    SendScreen(Packets.Status(EntityID, Status.Hair, _Hair));
                }
            }
        }
        public byte Job
        {
            get { return _Job; }
            set
            {
                _Job = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Class, _Job));
                    if (!Reborn && Level <= 120)
                        Database.GetStats(this);
                }
            }
        }
        public byte Level
        {
            get { return _Level; }
            set
            {
                byte PrevLev = _Level;
                _Level = value;
                if (MembInfo != null)
                    MembInfo.Level = _Level;
                if (Loaded)
                {
                    World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 92).Get);
                    SendScreen(Packets.Status(EntityID, Status.Level, _Level));

                    if (!Reborn && PrevLev < 120)
                        Database.GetStats(this);
                }
            }
        }
        public ulong Experience
        {
            get { return _Experience; }
            set
            {
                _Experience = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Experience, _Experience));
                }
            }
        }
        public ushort Str
        {
            get { return _Str; }
            set
            {
                _Str = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Strength, _Str));
                }
            }
        }
        public ushort Agi
        {
            get { return _Agi; }
            set
            {
                _Agi = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Agility, _Agi));
                }
            }
        }
        public ushort Vit
        {
            get { return _Vit; }
            set
            {
                _Vit = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Vitality, _Vit));
                }
            }
        }
        public ushort Spi
        {
            get { return _Spi; }
            set
            {
                _Spi = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.Spirit, _Spi));
                }
            }
        }
        public ushort StatPoints
        {
            get { return _StatPoints; }
            set
            {
                _StatPoints = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.StatPoints, _StatPoints));
                }
            }
        }
        public ushort CurHP
        {
            get { return _CurHP; }
            set
            {
                _CurHP = value;
                if (LoadedEquipmentHPAdd)
                    if (_CurHP > MaxHP)
                        _CurHP = MaxHP;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.HP, _CurHP));
                    if (MyTeam != null)
                    {
                        if (MyTeam.Members.Count > 0)
                        {
                            //Character[] Members = null;
                            //if (MyTeam.Members.Count > 0)
                            //{
                            //    Members = new Character[MyTeam.Members.Count + 1];
                            //    MyTeam.Members.CopyTo(Members, 0);
                            //}
                            foreach (Character C in MyTeam.Members)
                            {
                                if (C != null)
                                {
                                    if (C.MyClient.Soc.Connected)
                                    {
                                        C.MyClient.AddSend(Packets.PlayerJoinsTeam(this));
                                        //MyClient.AddSend(Packets.PlayerJoinsTeam(C));
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        public ushort CurMP
        {
            get { return _CurMP; }
            set
            {
                _CurMP = value;
                if (LoadedEquipmentHPAdd)
                    if (_CurMP > MaxMP)
                        _CurMP = MaxMP;

                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.MP, _CurMP));
                }
            }
        }
        public ushort MaxHP
        {
            get
            {
                double Rt = 0;
                if (Transformation.Transformed)
                {
                    Rt = Transformation.HP;
                    MyClient.AddSend(Packets.Status(EntityID, Status.MaxHP, Transformation.HP));
                }
                else
                {
                    Rt = (double)(Vit * 24 + Str * 3 + Agi * 3 + Spi * 3);
                    if (Job == 11)
                        Rt *= 1.05;
                    if (Job == 12)
                        Rt *= 1.08;
                    if (Job == 13)
                        Rt *= 1.1;
                    if (Job == 14)
                        Rt *= 1.12;
                    if (Job == 15)
                        Rt *= 1.15;
                    Rt += EqStats.MaxHP;

                    if (Rt > 65530)
                        Rt = 65530;

                }
                return (ushort)Rt;
            }
        }
        public ushort MaxMP
        {
            get
            {
                ushort mp = 0;
                if (Job != 100 && Job != 101 && Job >= 130)
                    mp = (ushort)(Spi * 15);
                else
                    mp = (ushort)(Spi * 5);

                if (Job == 133 || Job == 143)
                    mp = (ushort)((double)mp * 4 / 3);
                if (Job == 134 || Job == 144)
                    mp = (ushort)((double)mp * 5 / 3);
                if (Job == 135 || Job == 145)
                    mp *= 2;

                return (ushort)(mp + EqStats.MaxMP);
            }
        }
        public uint Silvers
        {
            get { return _Silvers; }
            set
            {
                if (value > 2000000000)
                {
                    World.ExcAdd += Name + " had silvers: " + _Silvers + " with new silvers value: " + value + " and returned to 0\r\n";
                    _Silvers = 0;
                    MyClient.LocalMessage(2000, "Your gold got reset to 0! Please let the PMs know about this telling them server,name,date when happened!");
                }
                else _Silvers = value;
                //_Silvers = Math.Min(2000000000, _Silvers);
                if (Loaded)
                {
                    //if (MyClient != null)
                    MyClient.AddSend(Packets.Status(EntityID, Status.Silvers, _Silvers));
                }
            }

        }
        public uint CPs
        {
            get { return _CPs; }
            set
            {
                _CPs = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.CPs, _CPs));
                }
            }
        }
        public uint WHSilvers
        {
            get { return _WHSilvers; }
            set
            {
                _WHSilvers = value;
                if (Loaded)
                {
                    MyClient.AddSend(Packets.Status(EntityID, Status.WHMoney, _WHSilvers));
                }
            }
        }
        public ushort PKPoints
        {
            get { return _PKPoints; }
            set
            {
                _PKPoints = value;
                if (Loaded)
                {
                    if (_PKPoints >= 100)
                    {
                        if (StatEff.Contains(StatusEffectEn.RedName))
                            StatEff.Remove(StatusEffectEn.RedName);
                        StatEff.Add(StatusEffectEn.BlackName);
                    }
                    else if (_PKPoints >= 30 && _PKPoints <= 99)
                    {
                        if (StatEff.Contains(StatusEffectEn.BlackName))
                            StatEff.Remove(StatusEffectEn.BlackName);
                        StatEff.Add(StatusEffectEn.RedName);
                    }
                    else if (_PKPoints <= 29)
                    {
                        if (StatEff.Contains(StatusEffectEn.RedName))
                            StatEff.Remove(StatusEffectEn.RedName);
                    }

                    MyClient.AddSend(Packets.Status(EntityID, Status.PKPoints, _PKPoints));
                }
            }
        }
        public bool BlueName
        {
            get { return _BlueName; }
            set
            {
                _BlueName = value;
                if (_BlueName == true)
                {
                    StatEff.Add(StatusEffectEn.BlueName);
                    BlueNameStarted = DateTime.Now;
                }
                else
                    StatEff.Remove(StatusEffectEn.BlueName);
            }
        }
        public short AtkFrequence
        {
            get
            {
                short t = 1000;
                //   if (Transformation.Transformed)
                //     t -= Transformation.Dex;
                //  else
                // {
                int TransDex = 0;
                if (Transformation.Dex != 0 && ExtraDex != 0)
                    TransDex = Transformation.Dex;
                else if (Transformation.Dex == 0 && ExtraDex != 0)
                    TransDex = ExtraDex;

                t -= (short)((Agi + EqStats.ExtraDex + TransDex) * EqStats.GemExtraDex);
                if (StatEff.Contains(StatusEffectEn.Cyclone))
                    if (Job >= 40 && Job <= 45)
                        t /= 2;
                    else
                        t /= 10;
                if (StatEff.Contains(StatusEffectEn.SuperMan))
                    t /= 2;
                t = (short)Math.Max((int)t, 200);
                //}
                return t;
            }
        }

        public Character()
        {
            EqStats = new EquipStats();
            EqStats.GemExtraMAttack = 1;
            EqStats.GemExtraExp = 1;
            EqStats.GemExtraMExp = 1;
            EqStats.GemExtraProf = 1;
            EqStats.GemExtraAttack = 1;
            EqStats.GemExtraDex = 1;
            EqStats.WeaponExtraAttack = 1;
            EqStats.GemBless = 0;
            StatEff = new StatusEffect(this);
            AtkMem = new AttackMemorise();
            AtkMem.Attacking = false;
            AtkMem.LastAttack = DateTime.Now;
            AtkMem.Target = 0;
            Buffs = new ConcurrentDictionary<Buff, ushort>();
            //QuizShowInfo = new QuizShow.Info();
            //QuizShowInfo.QNo = 1;
            //QuizShowInfo.Score = 0;
            //QuizShowInfo.Time = 0;
            //QuizShowInfo.Answers = new byte[Features.QuizShow.Questions.Count];
            Nobility = new Nobility(this);
            BDelete = new ConcurrentDictionary<Buff, ushort>();
            PoisonedInfo = new PoisonType();

        }

        public DateTime LastSave = DateTime.Now;
        public DateTime LastSave2 = DateTime.Now;
        public void RebornCharacter(byte ToJob)
        {
            try
            {
                for (byte i = 1; i < 9; i++)
                    if (i != 7)
                    {
                        Item I = Equips.Get(i);
                        if (I.ID != 0)
                        {
                            EquipStats(i, false, false);
                            ItemIDManipulation IDM = new ItemIDManipulation(I.ID);
                            IDM.LowestLevel(i);
                            if (IDM.ToID() < I.ID)
                                I.ID = IDM.ToID();
                            Equips.Replace(i, I, this);
                            EquipStats(i, true, false);
                        }
                    }
                    else
                    {
                        Item I = Equips.Get(i);
                        Equips.Replace(i, I, this);
                    }
            }
            catch { }
            if (Equips.LeftHand.ID != 0)
            {
                Inventory.Add(Equips.Get(5));
                EquipStats(5, false, false);
                Game.World.Spawn(this, false);
                Equips.UnEquip(5, this);
            }
            Reborns++;
            byte ExtraStat = 0;
            if (Level >= 120)
                ExtraStat = (byte)((-120 + Level) * 3 + Reborns * 10 + 45);
            else
                ExtraStat = (byte)(Reborns * 10);
            StatPoints = ExtraStat;
            Level = 15;
            Experience = 0;
            bool ND = false;
            bool Dance2 = false, Dance3 = false, Dance4 = false, Dance5 = false, Dance6 = false, Dance7 = false, Dance8 = false;
            ProfsBeforeReborn = new Dictionary<ushort, Prof>();
            foreach (Prof P in Profs.Values)
            {

                ProfsBeforeReborn.Add(P.ID, P);
                MyClient.AddSend(Packets.GeneralData(EntityID, P.ID, 0, 0, 108));
            }
            SkillsBeforeReborn = new Dictionary<ushort, Skill>();
            foreach (Skill S in Skills.Values)
            {

                SkillsBeforeReborn.Add(S.ID, S);
                #region CheckSpells
                if (S.ID == 1360)
                    ND = true;
                else if (S.ID == 1380)
                    Dance2 = true;
                else if (S.ID == 1385)
                    Dance3 = true;
                else if (S.ID == 1390)
                    Dance4 = true;
                else if (S.ID == 1395)
                    Dance5 = true;
                else if (S.ID == 1400)
                    Dance6 = true;
                else if (S.ID == 1405)
                    Dance7 = true;
                else if (S.ID == 1410)
                    Dance8 = true;
                #endregion

                MyClient.AddSend(Packets.GeneralData(EntityID, S.ID, 0, 0, 109));
            }
            Skills = new ConcurrentDictionary<ushort, Skill>();
            Profs = new ConcurrentDictionary<ushort, Prof>();
            #region Trojan
            if (Job == 15)
            {
                PreviousJob1 = 15;
                if (ToJob == 41 || ToJob == 142 || ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 3050 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1270 });
                }

            }
            #endregion
            #region Warrior
            if (Job == 25)
            {
                PreviousJob1 = 25;
                if (ToJob == 41 || ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1015 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1320 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1025 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 3060 });
                }
            }
            #endregion
            #region WaterTao
            if (Job == 135)
            {
                PreviousJob1 = 135;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1055 });
                }
                else if (ToJob == 11 || ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 3090 });
                }
                NewSkill(new Skill() { ID = 1350 });
                NewSkill(new Skill() { ID = 1280 });
            }
            #endregion
            #region Archer
            if (Job == 45)
            {
                PreviousJob1 = 45;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 5000 });
                }
                else
                {
                    NewSkill(new Skill() { ID = 5002 });
                }
            }
            #endregion
            #region FireTao
            if (Job == 145)
            {
                PreviousJob1 = 145;
                if (ToJob == 11 || ToJob == 21 || ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 3080 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }
            #endregion

            #region CheckOldSpell
            if (ND)
                NewSkill(new Skill() { ID = 1360 });
            if (Dance2)
                NewSkill(new Skill() { ID = 1380 });
            if (Dance3)
                NewSkill(new Skill() { ID = 1385 });
            if (Dance4)
                NewSkill(new Skill() { ID = 1390 });
            if (Dance5)
                NewSkill(new Skill() { ID = 1395 });
            if (Dance6)
                NewSkill(new Skill() { ID = 1400 });
            if (Dance7)
                NewSkill(new Skill() { ID = 1405 });
            if (Dance8)
                NewSkill(new Skill() { ID = 1410 });
            #endregion
            NewSkill(new Skill() { ID = 4000 });
            Job = ToJob;
            Database.GetStats(this);
            MyClient.LocalMessage(2000, "Congratulations! You are now reborn. All your skills and proficiency are gone.");
            World.SendMsgToAll("SYSTEM", "Congratulations! " + Name + " has reborned!", 2005, 0);

        }
        public void RebornCharacter2(byte ToJob)
        {
            try
            {
                for (byte i = 1; i < 9; i++)
                    if (i != 7 || i != 8)
                    {
                        Item I = Equips.Get(i);
                        if (I.ID != 0)
                        {
                            EquipStats(i, false, false);
                            ItemIDManipulation IDM = new ItemIDManipulation(I.ID);
                            IDM.LowestLevel(i);
                            I.ID = IDM.ToID();
                            Equips.Replace(i, I, this);
                            EquipStats(i, true, false);
                        }
                    }
                    else
                    {
                        Item I = Equips.Get(i);
                        Equips.Replace(i, I, this);
                    }
            }
            catch { }
            if (Equips.RightHand.ID != 0)
            {
                Inventory.Add(Equips.Get(5));
                EquipStats(5, false, false);
                Game.World.Spawn(this, false);
                Equips.UnEquip(5, this);
            }
            if (Equips.Fan.ID != 0)
            {
                Inventory.Add(Equips.Get(5));
                EquipStats(5, false, false);
                Game.World.Spawn(this, false);
                Equips.UnEquip(5, this);
            }
            if (Equips.Tower.ID != 0)
            {
                Inventory.Add(Equips.Get(5));
                EquipStats(5, false, false);
                Game.World.Spawn(this, false);
                Equips.UnEquip(5, this);
            }
            Reborns++;
            byte ExtraStat = 0;
            if (Level >= 120)
                ExtraStat = (byte)((-120 + Level) * 3 + Reborns * 10 + 45);
            else
                ExtraStat = (byte)(Reborns * 10);
            StatPoints += ExtraStat;
            Level = 15;
            Experience = 0;
            foreach (Skill S in Skills.Values)
            {
                MyClient.AddSend(Packets.GeneralData(EntityID, S.ID, 0, 0, 109));
            }
            foreach (Prof P in Profs.Values)
            {
                MyClient.AddSend(Packets.GeneralData(EntityID, P.ID, 0, 0, 108));
            }

            Skills = new ConcurrentDictionary<ushort, Skill>();
            Profs = new ConcurrentDictionary<ushort, Prof>();

            #region Archer2
            #region Arch-Arch
            if (PreviousJob1 == 45 && Job == 45)
            {
                PreviousJob2 = 45;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 5000 });
                }
                else
                {
                    NewSkill(new Skill() { ID = 5002 });
                }
            }
            #endregion
            #region Arch-Fire
            else if (PreviousJob1 == 45 && Job == 145)
            {
                PreviousJob2 = 145;
                if (ToJob == 11 || ToJob == 21 || ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 5002 });
                }
                else if (ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 10010 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1120 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 3080 });
                }
            }
            #endregion
            #region Arch-Tro
            if (PreviousJob1 == 45 && Job == 15)
            {
                PreviousJob2 = 15;
                if (ToJob == 41 || ToJob == 132 || ToJob == 142 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 3050 });
                    NewSkill(new Skill() { ID = 5002 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 5100 });
                }

            }

            #endregion
            #region Arch-War
            if (PreviousJob1 == 45 && Job == 25)
            {
                PreviousJob2 = 25;
                if (ToJob == 41 || ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1015 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1320 });
                }
                else if (ToJob == 21 || ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1025 });
                }
            }


            #endregion
            #region Arch-Water
            if (PreviousJob1 == 45 && Job == 135)
            {
                PreviousJob2 = 135;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 11 || ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 5002 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                    NewSkill(new Skill() { ID = 3090 });
                }
                else if (ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 10010 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                }
            }
            #endregion
            #endregion
            #region Trojan2
            #region Tro-Arch
            if (PreviousJob1 == 15 && Job == 45)
            {
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 5000 });
                }
                else
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 5002 });
                }
            }

            #endregion
            #region Tro-Fire
            if (PreviousJob1 == 15 && Job == 145)
            {
                PreviousJob2 = 145;
                if (ToJob == 41 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 11 || ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 3080 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }
            #endregion
            #region Tro-Tro
            if (PreviousJob1 == 15 && Job == 15)
            {
                PreviousJob2 = 15;
                if (ToJob == 41 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3080 });
                }
                else if (ToJob == 11 || ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }


            #endregion
            #region Tro-War
            if (PreviousJob1 == 15 && Job == 25)
            {
                PreviousJob2 = 25;
                if (ToJob == 41 || ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1015 });
                    NewSkill(new Skill() { ID = 1320 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1025 });
                }
            }
            #endregion
            #region Tro-Water
            if (PreviousJob1 == 15 && Job == 135)
            {
                PreviousJob2 = 135;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1090 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1075 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1085 });
                }
                else if (ToJob == 21 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1090 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 3090 });
                }
            }
            #endregion
            #endregion
            #region Fire2
            #region Fire-Arch
            if (PreviousJob1 == 145 && Job == 45)
            {
                PreviousJob2 = 45;
                NewSkill(new Skill() { ID = 1000 });
                NewSkill(new Skill() { ID = 1001 });
                NewSkill(new Skill() { ID = 1005 });
                NewSkill(new Skill() { ID = 1195 });
                NewSkill(new Skill() { ID = 5002 });
            }
            #endregion
            #region Fire-Fire
            if (PreviousJob1 == 145 && Job == 145)
            {
                PreviousJob2 = 145;
                if (ToJob == 41 || ToJob == 11 || ToJob == 51 || ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 3080 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 3080 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 3080 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }
            #endregion
            #region Fire-Tro
            if (PreviousJob1 == 145 && Job == 15)
            {
                PreviousJob2 = 15;
                if (ToJob == 41 || ToJob == 142 || ToJob == 132 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3050 });
                }
            }
            #endregion
            #region Fire-War
            if (PreviousJob1 == 145 && Job == 25)
            {
                PreviousJob2 = 25;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1015 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1320 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1025 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                }
            }
            #endregion
            #region Fire-Water
            if (PreviousJob1 == 145 && Job == 135)
            {
                PreviousJob2 = 135;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1075 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1175 });
                }
                else if (ToJob == 11 || ToJob == 21 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });

                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 3090 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }
            #endregion
            #endregion
            #region War2
            #region War-Arch
            if (PreviousJob1 == 25 && Job == 45)
            {
                PreviousJob2 = 45;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 5000 });
                }
                else if (ToJob == 132 || ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 5002 });
                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 5002 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 5002 });
                }
            }
            #endregion
            #region War-Fire
            if (PreviousJob1 == 25 && Job == 145)
            {
                PreviousJob2 = 145;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3080 });
                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 25)
                {
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1120 });
                }
            }
            #endregion
            #region War-Tro
            if (PreviousJob1 == 25 && Job == 15)
            {
                PreviousJob2 = 15;
                if (ToJob == 41 || ToJob == 142 || ToJob == 132 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1320 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1320 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3050 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 5100 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1270 });
                }
            }
            #endregion
            #region War-War
            if (PreviousJob1 == 25 && Job == 25)
            {
                PreviousJob2 = 25;
                if (ToJob == 41 || ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1320 });
                    NewSkill(new Skill() { ID = 3060 });

                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1015 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1025 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1320 });
                    NewSkill(new Skill() { ID = 3060 });
                }
            }
            #endregion
            #region War-Water
            if (PreviousJob1 == 25 && Job == 135)
            {
                PreviousJob2 = 135;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1025 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3090 });
                    NewSkill(new Skill() { ID = 1280 });
                    NewSkill(new Skill() { ID = 1350 });
                }
                else if (ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                }
            }
            #endregion
            #endregion
            #region Water2
            #region Water-Arch
            if (PreviousJob1 == 135 && Job == 45)
            {
                PreviousJob2 = 45;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 5000 });
                    NewSkill(new Skill() { ID = 5002 });
                }
                else
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 5000 });
                    NewSkill(new Skill() { ID = 5002 });
                }
            }
            #endregion
            #region Water-Fire
            if (PreviousJob1 == 135 && Job == 145)
            {
                PreviousJob2 = 145;
                if (ToJob == 11 || ToJob == 21 | ToJob == 41 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1000 });
                    NewSkill(new Skill() { ID = 1001 });
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1195 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1120 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 3080 });
                }
            }

            #endregion
            #region Water-Tro
            if (PreviousJob1 == 135 && Job == 15)
            {
                PreviousJob2 = 15;
                if (ToJob == 41 || ToJob == 142 || ToJob == 132 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1190 });
                    NewSkill(new Skill() { ID = 1110 });
                    NewSkill(new Skill() { ID = 1270 });
                    NewSkill(new Skill() { ID = 5100 });
                }
                else if (ToJob == 11)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3050 });
                }
            }
            #endregion
            #region Water-War
            if (PreviousJob1 == 135 && Job == 25)
            {
                PreviousJob2 = 25;
                if (ToJob == 41)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1350 });
                    NewSkill(new Skill() { ID = 1280 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 11 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1015 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1320 });
                }
                else if (ToJob == 21)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3060 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 1020 });
                    NewSkill(new Skill() { ID = 1040 });
                    NewSkill(new Skill() { ID = 3060 });
                    NewSkill(new Skill() { ID = 1025 });
                }
            }
            #endregion
            #region Water-Water
            if (PreviousJob1 == 135 && Job == 135)
            {
                PreviousJob2 = 135;
                if (ToJob == 11 || ToJob == 21 || ToJob == 41 || ToJob == 51)
                {
                    NewSkill(new Skill() { ID = 1005 });
                    NewSkill(new Skill() { ID = 1085 });
                    NewSkill(new Skill() { ID = 1090 });
                    NewSkill(new Skill() { ID = 1095 });
                    NewSkill(new Skill() { ID = 1195 });
                    NewSkill(new Skill() { ID = 3090 });
                }
                else if (ToJob == 132)
                {
                    NewSkill(new Skill() { ID = 3090 });
                }
                else if (ToJob == 142)
                {
                    NewSkill(new Skill() { ID = 1050 });
                    NewSkill(new Skill() { ID = 1075 });
                    NewSkill(new Skill() { ID = 1055 });
                    NewSkill(new Skill() { ID = 1175 });
                    NewSkill(new Skill() { ID = 3090 });
                }
            }

            #endregion
            #endregion
            Job = ToJob;
            if (Reborns == 2)
                NewSkill(new Skill() { ID = 4000 });
            Database.GetStats(this);
            MyClient.LocalMessage(2000, "Congratulations! You are now reborn. All your skills and proficiency are gone.");
            World.SendMsgToAll("SYSTEM", Name + " has got " + Reborns.ToString() + "nd reborn!", 2011, 0);

        }
        public uint PromoteItems
        {
            get
            {
                uint e = 0;
                if (Job == 41)
                    e = 1072031;
                else
                {
                    sbyte n = 0;
                    if (Job >= 10 && Job <= 15)
                        n = (sbyte)(Job - 10);
                    else if (Job >= 20 && Job <= 25)
                        n = (sbyte)(Job - 20);
                    else if (Job >= 40 && Job <= 45)
                        n = (sbyte)(Job - 40);
                    else if (Job >= 50 && Job <= 55)
                        n = (sbyte)(Job - 50);
                    else if (Job >= 100)
                    {
                        if (Job <= 101)
                            n = (sbyte)(Job - 100);
                        else if (Job >= 132 && Job <= 135)
                            n = (sbyte)(Job - 130);
                        else if (Job >= 142 && Job <= 145)
                            n = (sbyte)(Job - 140);
                    }
                    if (n == 0 || n == 1) return 1;
                    if (n == 2)
                        e = 1080001;
                    else if (n == 3)
                        e = 1088001;
                    else if (n == 4)
                        e = 721080;
                    else
                        e = 0;
                }
                return e;
            }
        }
        public bool CanBeMeleed
        {
            get
            {
                //  if (StatEff.Contains(StatusEffectEn.Fly)/* || StatEff.Contains(StatusEffectEn.Invisible)*/)
                if (BuffOf(SkillsClass.ExtraEffect.Fly).Eff == SkillsClass.ExtraEffect.Fly)
                    return false;
                return true;
            }
        }
        public bool CanBeMeeledByMobs
        {
            get
            {
                // if (BuffOf(SkillsClass.ExtraEffect.Fly).Eff == SkillsClass.ExtraEffect.Fly || BuffOf(SkillsClass.ExtraEffect.Invisibility).Eff == SkillsClass.ExtraEffect.Invisibility)
                //foreach (Buff B in Buffs)
                //        if (B.Eff == SkillsClass.ExtraEffect.Fly || B.Eff == SkillsClass.ExtraEffect.Invisibility)
                //            return false;

                if (Buffs != null)
                {
                    if (Buffs.Count > 0)
                    {
                        //Buff[] Bufffs = null;
                        //if (Buffs.Count > 0)
                        //{
                        //    Bufffs = new Buff[Buffs.Count];
                        //    Buffs.CopyTo(Bufffs, 0);
                        //}
                        if (Buffs != null)
                            foreach (Buff B in Buffs.Keys)
                                if (B.Eff == SkillsClass.ExtraEffect.Fly || B.Eff == SkillsClass.ExtraEffect.Invisibility)
                                    return false;

                        //Bufffs = null;
                    }
                    //lock (ActiveBuffs.SyncRoot)
                    //{
                    //    foreach (Buff B in ActiveBuffs)
                    //        if (B.Eff == SkillsClass.ExtraEffect.Fly || B.Eff == SkillsClass.ExtraEffect.Invisibility)
                    //            return false;
                    //}
                }
                return true;
            }
        }
        public bool PKAble(PKMode PK, Character C)
        {
            if (PK == PKMode.PK)
            {
                if (Loc.Map == 1080 && Loc.Map == 1017)
                {
                    if (C.RedTeam && RedTeam)
                        return false;
                    else if (C.BlueTeam && BlueTeam)
                        return false;
                }
                //else if (C.EventBase != null && C.EventBase?.MapEvent != null && C.EventBase?.MapEvent == C.Loc.Map)
                //{
                //    if (C.EventBase?.MapEvent == C.Loc.Map)
                //    {
                //        if (!C.EventBase.FriendlyFire && C.EventBase.TeamOne.ContainsKey(C.EntityID) && C.EventBase.TeamOne.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamTwo.ContainsKey(C.EntityID) && C.EventBase.TeamTwo.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamThree.ContainsKey(C.EntityID) && C.EventBase.TeamThree.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamFour.ContainsKey(C.EntityID) && C.EventBase.TeamFour.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //    }
                //}
                else if (EventBase != null && EventBase.Stage == Events.EventStage.Fighting)
                {
                    if (!EventBase.FriendlyFire)
                        foreach (KeyValuePair<uint, Dictionary<uint, Character>> Team in EventBase.Teams)
                            if (Team.Value.ContainsKey(EntityID) && Team.Value.ContainsKey(C.EntityID))
                                return false;
                }
                return true;
            }
            else if (PK == PKMode.Capture)
                return (BlueName || PKPoints > 99);
            else if (PK == PKMode.Team)
            {
                /*if (MyTeam != null && MyGuild != null && Friends.Values != null)
                    return !MyTeam.Members.Contains(C) && !MyGuild.Members.Contains(C.EntityID) && !Friends.Contains(C.EntityID);
                else if (MyTeam != null && MyGuild != null && Friends.Values == null)
                    return !MyTeam.Members.Contains(C) && !MyGuild.Members.Contains(C.EntityID);
                else if (MyTeam != null && Friends.Values != null)
                    return !MyTeam.Members.Contains(C) && !Friends.Contains(C.EntityID);
                else if (MyGuild != null && Friends.Values != null)
                    return !MyGuild.Members.ContainsValue(C) && !Friends.Contains(C.EntityID);
                else if (MyGuild != null)
                    return !MyGuild.Members.Contains(C.EntityID);
                else if (MyTeam != null)
                    return !MyTeam.Members.Contains(C);
                else if (Friends.Values != null)
                    return !Friends.Contains(C.EntityID);
                else return true;*/
                if (Loc.Map >= 8002 && Loc.Map <= 8003)
                {
                    if (C.MyTeam != null)
                        if (C.MyTeam.Members.Contains(this))
                            return false;
                }
                else if (Loc.Map == 1080 && Loc.Map == 1017)
                {
                    if (C.RedTeam && RedTeam)
                        return false;
                    else if (C.BlueTeam && BlueTeam)
                        return false;
                }
                //else if (C.EventBase != null && C.EventBase?.MapEvent != null && C.EventBase?.MapEvent == C.Loc.Map)
                //{
                //    if (C.EventBase?.MapEvent == C.Loc.Map)
                //    {
                //        if (!C.EventBase.FriendlyFire && C.EventBase.TeamOne.ContainsKey(C.EntityID) && C.EventBase.TeamOne.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamTwo.ContainsKey(C.EntityID) && C.EventBase.TeamTwo.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamThree.ContainsKey(C.EntityID) && C.EventBase.TeamThree.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //        else if (!C.EventBase.FriendlyFire && C.EventBase.TeamFour.ContainsKey(C.EntityID) && C.EventBase.TeamFour.ContainsKey(EntityID) && C.EventBase?.Stage == Events.EventStage.Fighting)
                //            return false;
                //    }
                //}
                else if (EventBase != null && EventBase.Stage == Events.EventStage.Fighting)
                {
                    if (!EventBase.FriendlyFire)
                        foreach (KeyValuePair<uint, Dictionary<uint, Character>> Team in EventBase.Teams)
                            if (Team.Value.ContainsKey(EntityID) && Team.Value.ContainsKey(C.EntityID))
                                return false;
                }
                else
                {
                    if (C.MyTeam != null)
                        if (C.MyTeam.Members.Contains(this))
                            return false;
                    if (C.MyGuild != null && MyGuild != null)
                        if (C.MyGuild.GuildName != "" && MyGuild.GuildName != "")
                        {
                            if (C.MyGuild.GuildID == MyGuild.GuildID)
                                return false;
                            if (C.MyGuild.Allies.ContainsKey(MyGuild.GuildID))
                                return false;
                        }
                    if (C.Friends.ContainsKey(EntityID))
                        return false;
                }
                /* if (MyGuild != null)
                     foreach (Features.MemberInfo Memb in MyGuild.Members.Values)
                         if (Memb.MyGuildID == C.MyGuild.GuildID)
                             return false;*/
                /*if (MyGuild != null && C.MyGuild != null)
                    foreach (Hashtable Gld in MyGuild.Members.Values)
                        if (Gld.Contains(C.EntityID) && Gld.Contains(EntityID))
                            return false;*/
                return true;
            }
            return false;
        }
        /* public void Teleport(ushort Map, ushort X, ushort Y)
         {
             if (MyShop != null)
                 MyShop.Close();
             World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
             ScreenChars = new List<uint>(200);
             MyClient.AddSend(Packets.GeneralData(EntityID, Map, X, Y, 86));
             if (AtkMem.Attacking)
             {
                 AtkMem.Target = 0;
                 AtkMem.Attacking = false;
             }
             Loc.X = X;
             Loc.Y = Y;
             if (MyCompanion != null)
                 MyCompanion.Dissappear();
             if (Map != 700)
                 Loc.PreviousMap = Loc.Map;
             Loc.Map = Map;
             if (Loc.Map == 1036)
                 MyClient.AddSend(Packets.MapStatus(Loc.Map, 30));
             else
                 MyClient.AddSend(Packets.MapStatus(Loc.Map, 2080));
             World.Spawns(this, false);
         }*/
        public void Teleport(uint Map, ushort X, ushort Y)
        {
            if (MyShop != null)
                MyShop.Close();
            // World.Action(this, Packets.GeneralData(EntityID, Loc.Map,Loc.X,Loc.Y, 135).Get);
            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
            ScreenChars = new ConcurrentDictionary<uint, Character>();

            //ScreenItems = new ConcurrentDictionary<uint, DroppedItem>(300);
            if (DMaps.MapOwner.ContainsKey(Map))
                MyClient.AddSend(Packets.GeneralData(EntityID, (ushort)DMaps.MapOwner[Map], X, Y, 86));
            else if (DMaps.EventMaps.ContainsKey(Map))
                MyClient.AddSend(Packets.GeneralData(EntityID, (ushort)DMaps.EventMaps[Map], X, Y, 86));
            else
                MyClient.AddSend(Packets.GeneralData(EntityID, Map, X, Y, 86));
            if (AtkMem.Attacking)
            {
                AtkMem.Target = 0;
                AtkMem.Attacking = false;
            }
            Loc.X = X;
            Loc.Y = Y;
            if (MyCompanion != null)
                MyCompanion.Dissappear();
            //if (Map != 700 && Map != 701)
            Loc.PreviousMap = Loc.Map;
            Loc.Map = Map;
            if (Loc.Map == 1036 || Loc.Map == 1090 || Loc.Map == 2068)
                MyClient.AddSend(Packets.MapStatus(Loc.Map, 30));
            else
            {
                if (DMaps.MapOwner.ContainsKey(Loc.Map))
                    MyClient.AddSend(Packets.MapStatus((ushort)DMaps.MapOwner[Loc.Map], 2080));
                else if (DMaps.EventMaps.ContainsKey(Loc.Map))
                    MyClient.AddSend(Packets.MapStatus((ushort)DMaps.EventMaps[Loc.Map], 2080));
                else
                    MyClient.AddSend(Packets.MapStatus(Loc.Map, 2080));
            }
            if (Loc.Map != Loc.PreviousMap)
            {
                if (World.PlayersInMap.ContainsKey(Loc.PreviousMap))
                {
                    if (World.PlayersInMap[Loc.PreviousMap].ContainsKey(EntityID))
                        World.PlayersInMap[Loc.PreviousMap].Remove(EntityID);
                }
                if (World.PlayersInMap.ContainsKey(Loc.Map))
                {
                    if (!World.PlayersInMap[Loc.Map].ContainsKey(EntityID))
                        World.PlayersInMap[Loc.Map].TryAdd(EntityID, this);
                }
            }
            if (DateTime.Now.Month == 12 && DateTime.Now.Day > 9 && Loc.Map == 1002)
                MyClient.AddSend(Packets.Weather((uint)Features.Weather.CurrentWeather, Features.Weather.Intensity, Features.Weather.Appearence, Features.Weather.Direction));
            World.Spawns(this, false);
            CancelProtectTime = false;
            ProtectTime = DateTime.Now;
        }

        public void AddSkillExp(ushort ID, uint Amount)
        {
            if (Skills.ContainsKey(ID))
            {
                Skill S = (Skill)Skills[ID];
                Features.SkillsClass.SkillInfo Info = S.Info;
                if (Info.UpgReqExp != 0 && Info.ID == ID && Level >= Info.UpgReqLvl)
                {

                    byte TempExp = ExperienceRate;
                    if (TempExp > 3)
                        TempExp = 3;
                    if (World.LowRatedServer)
                        Amount = (uint)(Amount * EqStats.GemExtraMExp);
                    else Amount = (uint)(Amount * EqStats.GemExtraMExp * TempExp);
                    if (World.EventSkillExp)
                        Amount = (uint)(Amount * 1.30);
                    //if (S.Lvl <= 1)
                    //    Amount *= 2;
                    if (S.Info.ID == 1002)
                        Amount *= 10;
                    S.Exp += Amount;

                    if (S.Exp >= Info.UpgReqExp)
                    {
                        S.Lvl++;
                        S.Exp = 0;
                        MyClient.LocalMessage(2005, "Congratulations! Your skill level has increased.");
                    }
                    if (SkillsBeforeReborn.ContainsKey(ID))
                    {
                        Skill SS = (Skill)SkillsBeforeReborn[ID];
                        if (S.Lvl >= (byte)(SS.Lvl / 2) && SS.Lvl >= S.Lvl)
                        {
                            S.Lvl = SS.Lvl;
                            S.Exp = SS.Exp;
                            SkillsBeforeReborn.Remove(ID);
                            MyClient.LocalMessage(2011, "Your skill level jumped back to it's level before reborning!");
                        }
                    }
                    /*Skills.Remove(ID);
                    if (!Skills.ContainsKey(ID))
                        Skills.Add(ID, S);*/
                    MyClient.AddSend(Packets.Skill(S));
                }
            }
        }
        public Buff BuffOf(SkillsClass.ExtraEffect E)
        {
            if (Buffs.Count > 0)
            {
                //Buff[] Bufffs = null;
                //if (Buffs.Count > 0)
                //{
                //    Bufffs = new Buff[Buffs.Count + 1];
                //    Buffs.CopyTo(Bufffs, 0);
                //}

                foreach (Buff B in Buffs.Keys)
                    if (B.Eff == E)
                        return B;
            }
            return new Buff();
        }
        public void AddBuff(Buff B)
        {

            Buff ExBuff = BuffOf(B.Eff);
            if (ExBuff.Eff == B.Eff)
                Buffs.Remove(ExBuff);
            // if (Loc.Map != 1039)
            // {
            if (B.Eff == SkillsClass.ExtraEffect.Transform)
            {
                switch (B.Transform) // 361 (bomb) - 41 (gold chest)
                {
                    case 2000:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 282;
                        Transformation.MinDmg = 179;
                        Transformation.Def = 73;
                        Transformation.Dex = 50;
                        Transformation.Dodge = 9;
                        Transformation.MagicDef = 34;
                        Transformation.Dist = 3;
                        TransID = 214;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2001:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 395;
                        Transformation.MinDmg = 245;
                        Transformation.Def = 126;
                        Transformation.Dex = 55;
                        Transformation.Dodge = 12;
                        Transformation.MagicDef = 45;
                        Transformation.Dist = 3;
                        TransID = 214;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2002:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 616;
                        Transformation.MinDmg = 367;
                        Transformation.Def = 180;
                        Transformation.Dex = 60;
                        Transformation.Dodge = 15;
                        Transformation.MagicDef = 53;
                        Transformation.Dist = 3;
                        TransID = 214;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2003:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 724;
                        Transformation.MinDmg = 429;
                        Transformation.Def = 247;
                        Transformation.Dex = 65;
                        Transformation.Dodge = 15;
                        Transformation.MagicDef = 53;
                        Transformation.Dist = 3;
                        TransID = 214;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2010:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 1231;
                        Transformation.MinDmg = 704;
                        Transformation.Def = 499;
                        Transformation.Dex = 70;
                        Transformation.Dodge = 20;
                        Transformation.MagicDef = 53;
                        Transformation.Dist = 3;
                        TransID = 214;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2011:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 1673;
                        Transformation.MinDmg = 941;
                        Transformation.Def = 601;
                        Transformation.Dex = 70;
                        Transformation.Dodge = 25;
                        Transformation.MagicDef = 53;
                        Transformation.Dist = 3;
                        TransID = 214;//   374 - robot
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2012:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 1991;
                        Transformation.MinDmg = 1107;
                        Transformation.Def = 1029;
                        Transformation.Dex = 70;
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 55;
                        Transformation.Dist = 3;
                        TransID = 214;//   374 - robot
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2013:
                        Transformation.HP = 50000;
                        Transformation.MaxDmg = 2226;
                        Transformation.MinDmg = 1235;
                        Transformation.Def = 1029;
                        Transformation.Dex = 70;
                        Transformation.Dodge = 35;
                        Transformation.MagicDef = 55;
                        Transformation.Dist = 3;
                        TransID = 214;//   374 - robot
                                      //transValue = (ulong)2141000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2005:
                        Transformation.HP = 1048;
                        Transformation.MaxDmg = 930;
                        Transformation.MinDmg = 656;
                        Transformation.Def = 290;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.4));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 45;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2006:
                        Transformation.HP = 1130;
                        Transformation.MaxDmg = 1062;
                        Transformation.MinDmg = 750;
                        Transformation.Def = 320;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.45));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 46;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2007:
                        Transformation.HP = 1205;
                        Transformation.MaxDmg = 1292;
                        Transformation.MinDmg = 910;
                        Transformation.Def = 510;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.5));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 50;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2008:
                        Transformation.HP = 1279;
                        Transformation.MaxDmg = 1428;
                        Transformation.MinDmg = 1000;
                        Transformation.Def = 600;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.55));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 53;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2009:
                        Transformation.HP = 1476;
                        Transformation.MaxDmg = 1570;
                        Transformation.MinDmg = 1100;
                        Transformation.Def = 700;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.6));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 55;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2040:
                        Transformation.HP = 1629;
                        Transformation.MaxDmg = 1700;
                        Transformation.MinDmg = 1200;
                        Transformation.Def = 880;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.65));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 57;
                        Transformation.Dist = 3;
                        TransID = 213; // water elf
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2041:
                        Transformation.HP = 1803;
                        Transformation.MaxDmg = 1900;
                        Transformation.MinDmg = 1300;
                        Transformation.Def = 1540;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.7));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 59;
                        Transformation.Dist = 3;
                        TransID = 273;//373 water elf upgraded
                                      //transValue = (ulong)2131000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2042:
                        Transformation.HP = 1998;
                        Transformation.MaxDmg = 2100;
                        Transformation.MinDmg = 1500;
                        Transformation.Def = 1880;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.7));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 61;
                        Transformation.Dist = 3;
                        TransID = 273;//373 water elf upgraded
                                      //transValue = (ulong)2131000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2043:
                        Transformation.HP = 2088;
                        Transformation.MaxDmg = 2300;
                        Transformation.MinDmg = 1600;
                        Transformation.Def = 1970;
                        Transformation.Dex = Math.Max((ushort)80, (ushort)((Agi + EqStats.ExtraDex) * 0.7));
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 63;
                        Transformation.Dist = 3;
                        TransID = 273;//373 water elf upgraded
                                      //transValue = (ulong)2131000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2020:
                        Transformation.HP = 3000;
                        Transformation.MaxDmg = 182;
                        Transformation.MinDmg = 122;
                        Transformation.Def = 1300;
                        Transformation.Dex = 100;
                        Transformation.Dodge = 35;
                        Transformation.MagicDef = 94;
                        Transformation.Dist = 3;
                        TransID = 207;//divine hare
                        DH = true;
                        //MyClient.AddSend(Packets.Status(EntityID, Status.Mesh, Mesh));
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2021:
                        Transformation.HP = 3000;
                        Transformation.MaxDmg = 200;
                        Transformation.MinDmg = 134;
                        Transformation.Def = 1400;
                        Transformation.Dex = 100;
                        Transformation.Dodge = 40;
                        Transformation.MagicDef = 96;
                        Transformation.Dist = 3;
                        TransID = 207;//divine hare
                        DH = true;
                        //MyClient.AddSend(Packets.Status(EntityID, Status.Mesh, Mesh));
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2022:
                        Transformation.HP = 3000;
                        Transformation.MaxDmg = 240;
                        Transformation.MinDmg = 160;
                        Transformation.Def = 1500;
                        Transformation.Dex = 100;
                        Transformation.Dodge = 45;
                        Transformation.MagicDef = 97;
                        Transformation.Dist = 3;
                        TransID = 207;//divine hare
                        DH = true;
                        //MyClient.AddSend(Packets.Status(EntityID, Status.Mesh, Mesh));
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2023:
                        Transformation.HP = 3000;
                        Transformation.MaxDmg = 258;
                        Transformation.MinDmg = 172;
                        Transformation.Def = 1600;
                        Transformation.Dex = 100;
                        Transformation.Dodge = 50;
                        Transformation.MagicDef = 98;
                        Transformation.Dist = 3;
                        TransID = 267;//267 divine hare upgraded
                                      // transValue = (ulong)2071000000;
                        DH = true;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2024:
                        Transformation.HP = 3000;
                        Transformation.MaxDmg = 300;
                        Transformation.MinDmg = 200;
                        Transformation.Def = 1900;
                        Transformation.Dex = 100;
                        Transformation.Dodge = 55;
                        Transformation.MagicDef = 99;
                        Transformation.Dist = 3;
                        TransID = 267;//267 divine hare upgraded
                                      // transValue = (ulong)2071000000;
                        DH = true;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2030:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1215;
                        Transformation.MinDmg = 610;
                        Transformation.Def = 100;
                        Transformation.Dex = Math.Max((ushort)100, (ushort)((Agi + EqStats.ExtraDex) * 0.8));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 96;
                        Transformation.Dist = 6;
                        TransID = 277; // night devil
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2031:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1310;
                        Transformation.MinDmg = 650;
                        Transformation.Def = 400;
                        Transformation.Dex = Math.Max((ushort)100, (ushort)((Agi + EqStats.ExtraDex) * 0.85));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 97;
                        TransID = 277; // night devil
                        Transformation.Dist = 6;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2032:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1420;
                        Transformation.MinDmg = 710;
                        Transformation.Def = 650;
                        Transformation.Dex = Math.Max((ushort)100, (ushort)((Agi + EqStats.ExtraDex) * 0.9));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 98;
                        Transformation.Dist = 6;
                        TransID = 277; // night devil
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        break;
                    case 2033:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1555;
                        Transformation.MinDmg = 780;
                        Transformation.Def = 720;
                        Transformation.Dex = Math.Max((ushort)110, (ushort)((Agi + EqStats.ExtraDex) * 0.95));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 98;
                        Transformation.Dist = 6;
                        TransID = 217;//217 night devil upgraded
                                      //  transValue = (ulong)2171000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                    case 2034:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1660;
                        Transformation.MinDmg = 840;
                        Transformation.Def = 1200;
                        Transformation.Dex = Math.Max((ushort)120, (ushort)(Agi + EqStats.ExtraDex));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 99;
                        Transformation.Dist = 6;
                        TransID = 217;//217 night devil upgraded
                                      //  transValue = (ulong)2171000000;
                        DH = false;
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;

                    default:
                        Transformation.HP = 4000;
                        Transformation.MaxDmg = 1660;
                        Transformation.MinDmg = 840;
                        Transformation.Def = 1200;
                        Transformation.Dex = Math.Max((ushort)120, (ushort)(Agi + EqStats.ExtraDex));
                        Transformation.Dodge = 30;
                        Transformation.MagicDef = 99;
                        Transformation.Dist = 6;
                        TransID = B.Transform;
                        DH = false;
                        // transValue = (ulong)(B.Transform * 10000000 + Avatar * 10000 + Body);
                        Game.World.DebugAdd += "TransID" + TransID + "\r\n";
                        World.Action(this, Packets.Status(EntityID, Status.Mesh, Mesh).Get);
                        if (B.skillID == 0)
                            Game.World.Action(this, Packets.SkillUse(EntityID, EntityID, 0, 1280, 6, Loc.X, Loc.Y).Get);
                        else
                            Game.World.Action(this, Packets.SkillUse(EntityID, EntityID, 0, B.skillID, 6, Loc.X, Loc.Y).Get);
                        //SendScreen(Packets.Status(EntityID, Status.Mesh, transValue));
                        break;
                }
                ExtraDex = Transformation.Dex;
                /* transEffect = new CastEffect();
                 transEffect.countdown = B.Lasts * 10;
                 transEffect.B = B;
                 transEffect.C = this;
                 transEffect.transValue = transValue;
                 transEffect.executeEffect();*/
                double pc = (double)CurHP / MaxHP;

                Transformation.Transformed = true;
                CurHP = (ushort)(MaxHP * pc);
            }
            //}
            Buffs.TryAdd(B, B.Lasts);
            StatEff.Add(B.StEff);
        }
        public void RemoveBuff(Buff B)
        {
            if (Buffs.ContainsKey(B))
            {
                Buffs.Remove(B);
                StatEff.Remove(B.StEff);
                if (B.Eff == SkillsClass.ExtraEffect.Transform)
                {
                    TransID = 0;
                    double pc = (double)CurHP / MaxHP;
                    Transformation.Transformed = false;
                    Transformation.Dex = 0;
                    Transformation.Dist = 0;
                    CurHP = (ushort)(MaxHP * pc);
                    if (DH)
                        DH = false;
                    /* transEffect.timer.Dispose();
                     transEffect.countdown = 0;*/
                    Body = Body;
                    Hair = Hair;
                    Equips.Send(MyClient, false);
                }
                else if (B.StEff == StatusEffectEn.Fly)
                {
                    Flying = false;
                }
            }
        }
        public void LoadItem(Item I, ushort Location)
        {
            switch (Location)
            {
                case 0:
                    Inventory.Add(I); break;
                case 11:
                    Equips.HeadGear = I; break;
                case 12:
                    Equips.Necklace = I; break;
                case 13:
                    Equips.Armor = I; break;
                case 14:
                    Equips.RightHand = I; break;
                case 15:
                    Equips.LeftHand = I; break;
                case 16:
                    Equips.Ring = I; break;
                case 17:
                    Equips.Gourd = I; break;
                case 18:
                    Equips.Boots = I; break;
                case 19:
                    Equips.Garment = I; break;
                case 20:
                    Equips.Fan = I; break;
                case 21:
                    Equips.Tower = I; break;
                case 22:
                    Equips.Steed = I; break;
                default:
                    Warehouses.LoadItem(this, I, Location);
                    break;
            }
        }
        public void AddItem(Item I)
        {
            if (I.UID == 0)
            {
                I.UID = (uint)Rnd.Next(10000000);
            }

            if (Inventory.Count < Inventory.Capacity)
            {
                Inventory.Add(I);
                if (Loaded)
                    MyClient.AddSend(Packets.AddItem(I, 0));
            }
        }
        public void AddItem(ref Item I)
        {
            if (I.UID == 0)
            {
                I.UID = (uint)Rnd.Next(10000000);
            }

            if (Inventory.Count < Inventory.Capacity)
            {
                Inventory.Add(I);
                if (Loaded)
                    MyClient.AddSend(Packets.AddItem(I, 0));
            }
        }
        public void AddItem(uint ID)
        {
            Item I = new Item();
            I.ID = ID;
            I.UID = (uint)Rnd.Next(10000000);
            I.MaxDur = I.DBInfo.Durability;
            I.CurDur = I.MaxDur;
            I.Color = Item.ArmorColor.Orange;

            if (I.UID == 0)
            {
                I.UID = (uint)Rnd.Next(10000000);
            }
            Inventory.Add(I);
            if (MyClient != null)
                MyClient.AddSend(Packets.AddItem(I, 0));

        }
        public void AddItem(uint ID, byte Plus)
        {
            Item I = new Item();
            I.Plus = Plus;
            I.ID = ID;
            I.UID = (uint)Rnd.Next(10000000);
            I.MaxDur = I.DBInfo.Durability;
            I.CurDur = I.MaxDur;

            if (I.UID == 0)
            {
                I.UID = (uint)Rnd.Next(10000000);
            }

            Inventory.Add(I);
            MyClient.AddSend(Packets.AddItem(I, 0));

        }
        public Item FindInvItem(uint UID)
        {
            foreach (Item I in Inventory)
                if (I.UID == UID)
                    return I;
            return new Item { ID = 0 };
        }
        public void RemoveItem(Item I)
        {
            try
            {
                Inventory.Remove(I);
                MyClient.AddSend(Packets.ItemPacket(I.UID, 0, 3));
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public bool RemoveItem(ref Item I)
        {
            try
            {
                Inventory.Remove(I);
                MyClient.AddSend(Packets.ItemPacket(I.UID, 0, 3));
                foreach (Item II in Inventory)
                    if (II.UID == I.UID)
                        return false;
                return true;
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; return false; }
        }
        public bool RemoveItem(uint UID, bool ClientSend = true)
        {
            try
            {
                // Item Rem = new Item();
                foreach (Item I in Inventory)
                    if (I.UID == UID)
                    {
                        Inventory.Remove(I);
                        if (ClientSend)
                            MyClient.AddSend(Packets.ItemPacket(UID, 0, 3));
                        return true;
                    }
                /*if (Rem.ID != 0)
                {
                   
                }*/
                return false;
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; return false; }
        }
        public Item NextItem(uint ID)
        {
            for (int i = 0; i < Inventory.Count; i++)
                if (((Item)Inventory[i]).ID == ID)
                    return ((Item)Inventory[i]);
            return new Item();
        }
        public bool InventoryContains(uint ID, byte Count)
        {
            byte _Count = 0;
            for (int i = 0; i < Inventory.Count; i++)
                if (((Item)Inventory[i]).ID == ID)
                    _Count++;
            return _Count >= Count;
        }
        public bool InventoryContains(uint UID)
        {
            for (int i = 0; i < Inventory.Count; i++)
                if (((Item)Inventory[i]).UID == UID)
                    return true;
            return false;
        }
        public byte InventoryItemIDCount(uint ID)
        {
            byte _Count = 0;
            for (int i = 0; i < Inventory.Count; i++)
                if (((Item)Inventory[i]).ID == ID)
                    _Count++;
            return _Count;
        }

        public bool WeaponSkill(ushort AX, ushort AY, uint T)
        {
            if (Equips.LeftHand.ID != 0 || Equips.RightHand.ID != 0)
            {
                if (PassiveSkills)
                {
                    Buff B = BuffOf(SkillsClass.ExtraEffect.Transform);
                    if (!Buffs.ContainsKey(B))
                    {
                        bool WepSkill = MyMath.ChanceSuccess(80);
                        if (WepSkill)
                        {
                            ushort SkillID = 0;
                            ushort SkillID2 = 0;
                            #region SkillSELECTOR
                            if (Equips.RightHand.ID != 0)
                            {
                                if (Equips.LeftHand.ID != 0)
                                {
                                    switch (Game.ItemIDManipulation.Part(Equips.RightHand.ID, 0, 3))
                                    {
                                        case 420:                       //Sword
                                        case 421: SkillID = 5030; break;//Backsword
                                        case 430: SkillID = 7000; break;//Hook
                                        case 440: SkillID = 7040; break;//Whip
                                        case 450: SkillID = 7010; break;//Axe
                                        case 460: SkillID = 5040; break;//Hammer
                                        case 480: SkillID = 7020; break;//Club
                                        case 481: SkillID = 7030; break;//Scepter
                                        case 490: SkillID = 1290; break;//Dagger
                                        case 510: SkillID = 1250; break;//Glaive
                                        case 540: SkillID = 1300; break;//LongHammer
                                        case 560: SkillID = 1260; break;//Spear
                                        case 580: SkillID = 5020; break;//Halberd
                                        case 561: SkillID = 5010; break;//Wand
                                        case 530: SkillID = 5050; break;//Poleaxe
                                    }
                                    switch (ItemIDManipulation.Part(Equips.LeftHand.ID, 0, 3))
                                    {
                                        case 420:                       //Sword
                                        case 421: SkillID2 = 5030; break;//Backsword
                                        case 430: SkillID2 = 7000; break;//Hook
                                        case 440: SkillID2 = 7040; break;//Whip
                                        case 450: SkillID2 = 7010; break;//Axe
                                        case 460: SkillID2 = 5040; break;//Hammer
                                        case 480: SkillID2 = 7020; break;//Club
                                        case 481: SkillID2 = 7030; break;//Scepter
                                        case 490: SkillID2 = 1290; break;//Dagger
                                        case 510: SkillID2 = 1250; break;//Glaive
                                        case 540: SkillID2 = 1300; break;//LongHammer
                                        case 560: SkillID2 = 1260; break;//Spear
                                        case 580: SkillID2 = 5020; break;//Halberd
                                        case 561: SkillID2 = 5010; break;//Wand
                                        case 530: SkillID2 = 5050; break;//Poleaxe
                                    }
                                }
                                else
                                {
                                    switch (Game.ItemIDManipulation.Part(Equips.RightHand.ID, 0, 3))
                                    {
                                        case 420:                       //Sword
                                        case 421: SkillID = 5030; break;//Backsword
                                        case 430: SkillID = 7000; break;//Hook
                                        case 440: SkillID = 7040; break;//Whip
                                        case 450: SkillID = 7010; break;//Axe
                                        case 460: SkillID = 5040; break;//Hammer
                                        case 480: SkillID = 7020; break;//Club
                                        case 481: SkillID = 7030; break;//Scepter
                                        case 490: SkillID = 1290; break;//Dagger
                                        case 510: SkillID = 1250; break;//Glaive
                                        case 540: SkillID = 1300; break;//LongHammer
                                        case 560: SkillID = 1260; break;//Spear
                                        case 580: SkillID = 5020; break;//Halberd
                                        case 561: SkillID = 5010; break;//Wand
                                        case 530: SkillID = 5050; break;//Poleaxe
                                    }
                                }
                            }
                            #endregion
                            Skill Skill1 = null;
                            try
                            {
                                if (Skills.ContainsKey(SkillID))
                                    Skill1 = (Skill)Skills[SkillID];
                            }
                            catch { }
                            Skill Skill2 = null;
                            try
                            {
                                if (Skills.ContainsKey(SkillID2))
                                    Skill2 = (Skill)Skills[SkillID2];
                            }
                            catch { }
                            if (Skill1 == null && Skill2 == null)
                                return false;
                            int Skill1Chance = 0;
                            int Skill2Chance = 0;
                            if (Skill1 != null)
                            {
                                Skill1Chance = Skill1.Info.ActivationChance;
                                #region Calculate Weapon Skill Activation Rate
                                //switch (Skill1.ID)
                                //{
                                //    #region Sword/Backsword
                                //    case 5030:
                                //        switch (Skill1.Lvl)
                                //        {
                                //            case 0:
                                //                Skill1Chance = 33;
                                //                break;
                                //            case 1:
                                //                Skill1Chance = 38;
                                //                break;
                                //            case 2:
                                //                Skill1Chance = 43;
                                //                break;
                                //            case 3:
                                //                Skill1Chance = 48;
                                //                break;
                                //            case 4:
                                //                Skill1Chance = 53;
                                //                break;
                                //            case 5:
                                //                Skill1Chance = 58;
                                //                break;
                                //            case 6:
                                //                Skill1Chance = 63;
                                //                break;
                                //            case 7:
                                //                Skill1Chance = 68;
                                //                break;
                                //            case 8:
                                //                Skill1Chance = 73;
                                //                break;
                                //            case 9:
                                //                Skill1Chance = 78;
                                //                break;
                                //        }
                                //        break;
                                //    #endregion
                                //    #region Rage | Wand | Spear | Glaive | Poleaxe | Halberd | Whip | Longhammer
                                //    case 7020:
                                //    case 5010:
                                //    case 1260:
                                //    case 1250:
                                //    case 5050:
                                //    case 5020:
                                //    case 7040:
                                //    case 1300:
                                //        switch (Skill1.Lvl)
                                //        {
                                //            case 0:
                                //                Skill1Chance = 20;
                                //                break;
                                //            case 1:
                                //                Skill1Chance = 23;
                                //                break;
                                //            case 2:
                                //                Skill1Chance = 26;
                                //                break;
                                //            case 3:
                                //                Skill1Chance = 29;
                                //                break;
                                //            case 4:
                                //                Skill1Chance = 31;
                                //                break;
                                //            case 5:
                                //                Skill1Chance = 34;
                                //                break;
                                //            case 6:
                                //                Skill1Chance = 37;
                                //                break;
                                //            case 7:
                                //                Skill1Chance = 40;
                                //                break;
                                //            case 8:
                                //                Skill1Chance = 43;
                                //                break;
                                //            case 9:
                                //                Skill1Chance = 45;
                                //                break;
                                //        }
                                //        break;
                                //    #endregion
                                //    #region Dagger
                                //    case 1290:
                                //    case 7030:
                                //        switch (Skill1.Lvl)
                                //        {
                                //            case 0:
                                //                Skill1Chance = 10;
                                //                break;
                                //            case 1:
                                //                Skill1Chance = 10;
                                //                break;
                                //            case 2:
                                //                Skill1Chance = 11;
                                //                break;
                                //            case 3:
                                //                Skill1Chance = 11;
                                //                break;
                                //            case 4:
                                //                Skill1Chance = 12;
                                //                break;
                                //            case 5:
                                //                Skill1Chance = 12;
                                //                break;
                                //            case 6:
                                //                Skill1Chance = 13;
                                //                break;
                                //            case 7:
                                //                Skill1Chance = 13;
                                //                break;
                                //            case 8:
                                //                Skill1Chance = 14;
                                //                break;
                                //            case 9:
                                //                Skill1Chance = 15;
                                //                break;
                                //        }
                                //        break;
                                //    #endregion
                                //    #region Hammer | Hook | Axe
                                //    case 5040:
                                //    case 7000:
                                //    case 7010:
                                //        switch (Skill1.Lvl)
                                //        {
                                //            case 0:
                                //                Skill1Chance = 10;
                                //                break;
                                //            case 1:
                                //                Skill1Chance = 12;
                                //                break;
                                //            case 2:
                                //                Skill1Chance = 14;
                                //                break;
                                //            case 3:
                                //                Skill1Chance = 16;
                                //                break;
                                //            case 4:
                                //                Skill1Chance = 18;
                                //                break;
                                //            case 5:
                                //                Skill1Chance = 20;
                                //                break;
                                //            case 6:
                                //                Skill1Chance = 22;
                                //                break;
                                //            case 7:
                                //                Skill1Chance = 24;
                                //                break;
                                //            case 8:
                                //                Skill1Chance = 26;
                                //                break;
                                //            case 9:
                                //                Skill1Chance = 28;
                                //                break;
                                //        }
                                //        break;
                                //        #endregion
                                //}
                                #endregion
                            }
                            if (Skill2 != null)
                            {
                                Skill2Chance = Skill2.Info.ActivationChance;
                                #region Calculate Weapon Skill Activation Rate
                                //switch (Skill2.ID)
                                //{

                                //#region Sword/Backsword
                                //case 5030:
                                //    switch (Skill2.Lvl)
                                //    {
                                //        case 0:
                                //            Skill2Chance = 33;
                                //            break;
                                //        case 1:
                                //            Skill2Chance = 38;
                                //            break;
                                //        case 2:
                                //            Skill2Chance = 43;
                                //            break;
                                //        case 3:
                                //            Skill2Chance = 48;
                                //            break;
                                //        case 4:
                                //            Skill2Chance = 53;
                                //            break;
                                //        case 5:
                                //            Skill2Chance = 58;
                                //            break;
                                //        case 6:
                                //            Skill2Chance = 63;
                                //            break;
                                //        case 7:
                                //            Skill2Chance = 68;
                                //            break;
                                //        case 8:
                                //            Skill2Chance = 73;
                                //            break;
                                //        case 9:
                                //            Skill2Chance = 78;
                                //            break;
                                //    }
                                //    break;
                                //#endregion
                                //#region Rage | Wand | Spear | Glaive | Poleaxe | Halberd | Whip | Longhammer
                                //case 7020:
                                //case 5010:
                                //case 1260:
                                //case 1250:
                                //case 5050:
                                //case 5020:
                                //case 7040:
                                //case 1300:
                                //    switch (Skill2.Lvl)
                                //    {
                                //        case 0:
                                //            Skill2Chance = 20;
                                //            break;
                                //        case 1:
                                //            Skill2Chance = 23;
                                //            break;
                                //        case 2:
                                //            Skill2Chance = 26;
                                //            break;
                                //        case 3:
                                //            Skill2Chance = 29;
                                //            break;
                                //        case 4:
                                //            Skill2Chance = 31;
                                //            break;
                                //        case 5:
                                //            Skill2Chance = 34;
                                //            break;
                                //        case 6:
                                //            Skill2Chance = 37;
                                //            break;
                                //        case 7:
                                //            Skill2Chance = 40;
                                //            break;
                                //        case 8:
                                //            Skill2Chance = 43;
                                //            break;
                                //        case 9:
                                //            Skill2Chance = 45;
                                //            break;
                                //    }
                                //    break;
                                //#endregion
                                //#region Dagger
                                //case 1290:
                                //case 7030:
                                //    switch (Skill2.Lvl)
                                //    {
                                //        case 0:
                                //            Skill2Chance = 10;
                                //            break;
                                //        case 1:
                                //            Skill2Chance = 10;
                                //            break;
                                //        case 2:
                                //            Skill2Chance = 11;
                                //            break;
                                //        case 3:
                                //            Skill2Chance = 11;
                                //            break;
                                //        case 4:
                                //            Skill2Chance = 12;
                                //            break;
                                //        case 5:
                                //            Skill2Chance = 12;
                                //            break;
                                //        case 6:
                                //            Skill2Chance = 13;
                                //            break;
                                //        case 7:
                                //            Skill2Chance = 13;
                                //            break;
                                //        case 8:
                                //            Skill2Chance = 14;
                                //            break;
                                //        case 9:
                                //            Skill2Chance = 15;
                                //            break;
                                //    }
                                //    break;
                                //#endregion
                                //#region Hammer | Hook | Axe
                                //case 5040:
                                //case 7000:
                                //case 7010:
                                //    switch (Skill2.Lvl)
                                //    {
                                //        case 0:
                                //            Skill2Chance = 10;
                                //            break;
                                //        case 1:
                                //            Skill2Chance = 12;
                                //            break;
                                //        case 2:
                                //            Skill2Chance = 14;
                                //            break;
                                //        case 3:
                                //            Skill2Chance = 16;
                                //            break;
                                //        case 4:
                                //            Skill2Chance = 18;
                                //            break;
                                //        case 5:
                                //            Skill2Chance = 20;
                                //            break;
                                //        case 6:
                                //            Skill2Chance = 22;
                                //            break;
                                //        case 7:
                                //            Skill2Chance = 24;
                                //            break;
                                //        case 8:
                                //            Skill2Chance = 26;
                                //            break;
                                //        case 9:
                                //            Skill2Chance = 28;
                                //            break;
                                //    }
                                //    break;
                                //    #endregion
                                //}
                                #endregion
                            }
                            bool S1 = MyMath.ChanceSuccess(Skill1Chance);
                            bool S2 = MyMath.ChanceSuccess(Skill2Chance);
                            if (S1)
                            {
                                if (Skill1 != null)
                                {
                                    DoWeaponSkill(Skill1, T);
                                    return true;
                                }
                            }
                            else if (S2)
                            {
                                if (Skill2 != null)
                                {
                                    DoWeaponSkill(Skill2, T);
                                    return true;
                                }
                            }
                            return false;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        public void DoWeaponSkill(Skill Skill, uint Target)
        {
            if (Loc.Map == 1036 || Loc.Map == 1616 || Loc.Map == 1090 || Loc.Map == 2068)
                return;
            if (!Alive)
                return;
            if (EntityID == Target)
                return;

            ushort X = 0, Y = 0;
            Character CharacterTarget = null;
            if (World.H_Chars.ContainsKey(Target))
            { CharacterTarget = World.H_Chars[Target]; X = CharacterTarget.Loc.X; Y = CharacterTarget.Loc.Y; }
            Companion CompTarget = null;
            if (World.H_Companions.ContainsKey(Target))
            {
                CompTarget = (Companion)World.H_Companions[Target]; X = CompTarget.Loc.X; Y = CompTarget.Loc.Y;
            }
            Mob TargetMonster = null;
            if (World.H_Mobs.ContainsKey(Loc.Map))
            {
                if (World.H_Mobs[Loc.Map].ContainsKey(Target))
                {
                    TargetMonster = World.H_Mobs[Loc.Map][Target]; X = TargetMonster.Loc.X; Y = TargetMonster.Loc.Y;
                    //Console.WriteLine("TX: " + X + " TY: " + Y);
                }
            }
            /*foreach (Hashtable HMapMobs in World.H_Mobs.Values)
                if (HMapMobs.ContainsKey(Target))
                {
                    TargetMonster = (Mob)HMapMobs[Target]; X = TargetMonster.Loc.X; Y = TargetMonster.Loc.Y;
                    Console.WriteLine("TX: " + X + " TY: " + Y);
                }*/
            NPC TargetNpc = null;
            if (World.H_NPCs.ContainsKey(Loc.Map))
            {
                Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                if (MapNPC.ContainsKey(Target))
                { TargetNpc = (NPC)MapNPC[Target]; X = TargetNpc.Loc.X; Y = TargetNpc.Loc.Y; }
            }

            if (World.H_SOBs.ContainsKey(Target))
            {
                X = World.H_SOBs[Target].Loc.X;
                Y = World.H_SOBs[Target].Loc.Y;
            }
            #region unused
            //if (Target == GuildWars.ThePole.EntityID)
            //{
            //    X = GuildWars.ThePole.Loc.X;
            //    Y = GuildWars.ThePole.Loc.Y;
            //}
            //if (Target == GuildWars.TheLeftGate.EntityID)
            //{
            //    X = GuildWars.TheLeftGate.Loc.X;
            //    Y = GuildWars.TheLeftGate.Loc.Y;
            //}
            //if (Target == GuildWars.TheRightGate.EntityID)
            //{
            //    X = GuildWars.TheRightGate.Loc.X;
            //    Y = GuildWars.TheRightGate.Loc.Y;
            //}

            #region Counter Clock GW
            //if (Target == CounterClock.ThePole.EntityID)
            //{
            //    X = CounterClock.ThePole.Loc.X;
            //    Y = CounterClock.ThePole.Loc.Y;
            //}
            //if (Target == CounterClock.LG1.EntityID)
            //{
            //    X = CounterClock.LG1.Loc.X;
            //    Y = CounterClock.LG1.Loc.Y;
            //}
            //if (Target == CounterClock.LG2.EntityID)
            //{
            //    X = CounterClock.LG2.Loc.X;
            //    Y = CounterClock.LG2.Loc.Y;
            //}
            //if (Target == CounterClock.LG3.EntityID)
            //{
            //    X = CounterClock.LG3.Loc.X;
            //    Y = CounterClock.LG3.Loc.Y;
            //}
            //if (Target == CounterClock.LG4.EntityID)
            //{
            //    X = CounterClock.LG4.Loc.X;
            //    Y = CounterClock.LG4.Loc.Y;
            //}
            //if (Target == CounterClock.LG5.EntityID)
            //{
            //    X = CounterClock.LG5.Loc.X;
            //    Y = CounterClock.LG5.Loc.Y;
            //}
            //if (Target == CounterClock.LG6.EntityID)
            //{
            //    X = CounterClock.LG6.Loc.X;
            //    Y = CounterClock.LG6.Loc.Y;
            //}
            //if (Target == CounterClock.RG1.EntityID)
            //{
            //    X = CounterClock.RG1.Loc.X;
            //    Y = CounterClock.RG1.Loc.Y;
            //}
            //if (Target == CounterClock.RG2.EntityID)
            //{
            //    X = CounterClock.RG2.Loc.X;
            //    Y = CounterClock.RG2.Loc.Y;
            //}
            //if (Target == CounterClock.RG3.EntityID)
            //{
            //    X = CounterClock.RG3.Loc.X;
            //    Y = CounterClock.RG3.Loc.Y;
            //}
            //if (Target == CounterClock.RG4.EntityID)
            //{
            //    X = CounterClock.RG4.Loc.X;
            //    Y = CounterClock.RG4.Loc.Y;
            //}
            //if (Target == CounterClock.RG5.EntityID)
            //{
            //    X = CounterClock.RG5.Loc.X;
            //    Y = CounterClock.RG5.Loc.Y;
            //}
            //if (Target == CounterClock.RG6.EntityID)
            //{
            //    X = CounterClock.RG6.Loc.X;
            //    Y = CounterClock.RG6.Loc.Y;
            //}
            //if (Target == CounterClock.RG7.EntityID)
            //{
            //    X = CounterClock.RG7.Loc.X;
            //    Y = CounterClock.RG7.Loc.Y;
            //}
            //if (Target == CounterClock.RG8.EntityID)
            //{
            //    X = CounterClock.RG8.Loc.X;
            //    Y = CounterClock.RG8.Loc.Y;
            //}
            //if (Target == CounterClock.RG9.EntityID)
            //{
            //    X = CounterClock.RG9.Loc.X;
            //    Y = CounterClock.RG9.Loc.Y;
            //}
            //if (Target == CounterClock.RG10.EntityID)
            //{
            //    X = CounterClock.RG10.Loc.X;
            //    Y = CounterClock.RG10.Loc.Y;
            //}
            //if (Target == CounterClock.RG11.EntityID)
            //{
            //    X = CounterClock.RG11.Loc.X;
            //    Y = CounterClock.RG11.Loc.Y;
            //}
            //if (Target == CounterClock.RG12.EntityID)
            //{
            //    X = CounterClock.RG12.Loc.X;
            //    Y = CounterClock.RG12.Loc.Y;
            //}
            //if (Target == CounterClock.RG13.EntityID)
            //{
            //    X = CounterClock.RG13.Loc.X;
            //    Y = CounterClock.RG13.Loc.Y;
            //}
            //if (Target == CounterClock.RG14.EntityID)
            //{
            //    X = CounterClock.RG14.Loc.X;
            //    Y = CounterClock.RG14.Loc.Y;
            //}
            //if (Target == CounterClock.RG15.EntityID)
            //{
            //    X = CounterClock.RG15.Loc.X;
            //    Y = CounterClock.RG15.Loc.Y;
            //}
            //if (Target == CounterClock.RG16.EntityID)
            //{
            //    X = CounterClock.RG16.Loc.X;
            //    Y = CounterClock.RG16.Loc.Y;
            //}
            //if (Target == CounterClock.RG17.EntityID)
            //{
            //    X = CounterClock.RG17.Loc.X;
            //    Y = CounterClock.RG17.Loc.Y;
            //}
            #endregion
            #endregion

            Features.SkillsClass.SkillUse SU = new SkillsClass.SkillUse();
            SU.Init(this, Skill.ID, Skill.Lvl, X, Y);
            /* if (TargetMonster != null)
                 if (StatEff.Contains(Game.StatusEffectEn.FatalStrike))
                 {
                     Shift((ushort)(X), (ushort)(Y));
                 }*/
            try
            {
                switch (Skill.ID)
                {

                    #region WeaponSkill
                    #region MultipleTargets
                    #region Rage
                    case 7020:
                        {
                            ushort Dist = SU.Info.MaxDist;
                            if (World.H_Mobs.ContainsKey(Loc.Map))
                            {
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                {
                                    if (M.Alive)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= Dist)
                                            if (PKMode == PKMode.PK || !M.NeedsPKMode && !SU.MobTargets.ContainsKey(M))
                                                SU.MobTargets.Add(M, SU.GetDamage(M));
                                    }
                                }
                            }
                            if (!World.NoPKMaps.Contains(Loc.Map))
                                foreach (Character C in ScreenChars.Values)//World.H_Chars.Values
                                // for (int x = 0; x < Program.ThreadInfo[3].Array.Length; x++)
                                {

                                    // Character C = Program.ThreadInfo[3].Array[x];
                                    if (C != null)
                                        if (C.Alive && Loc.Map == C.Loc.Map)
                                        {
                                            if (C.EntityID != EntityID)
                                                if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                                    if (C.CanBeMeleed)
                                                        if (C.PKAble(PKMode, this) && !SU.PlayerTargets.ContainsKey(C))
                                                            SU.PlayerTargets.Add(C, SU.GetDamage(C));
                                        }
                                }
                            if (Loc.Map == 1039)
                            {
                                Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                                foreach (NPC C in MapNPC.Values)
                                {
                                    if ((C.Flags == 21 || C.Flags == 22) && Level >= C.Level)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                            if (!SU.NPCTargets.ContainsKey(C))
                                                SU.NPCTargets.Add(C, SU.GetDamage(C));
                                    }
                                }
                            }
                            foreach (SOB S in World.H_SOBs.Values)
                            {
                                if (Loc.Map == S.Loc.Map)
                                {
                                    if (S.IsPole())
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && S.War && MyGuild != null && (S.LastWinner == null || MyGuild.GuildID != S.LastWinner.GuildID))
                                            SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                    }

                                    else if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist)
                                        SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                }
                            }
                            #region unused
                            //else if (Loc.Map == 1038)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Dist && GuildWars.War && MyGuild != null && (GuildWars.LastWinner == null || MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                            //        SU.MiscTargets.Add(GuildWars.ThePole.EntityID, SU.GetDamage(GuildWars.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(GuildWars.TheRightGate.EntityID, SU.GetDamage(GuildWars.TheRightGate.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(GuildWars.TheLeftGate.EntityID, SU.GetDamage(GuildWars.TheLeftGate.CurHP));
                            //}
                            #region Count Clock GW
                            //else if (Loc.Map == 1844)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Dist && CounterClock.War && MyGuild != null && (CounterClock.LastWinner == null || MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                            //        SU.MiscTargets.Add(CounterClock.ThePole.EntityID, SU.GetDamage(CounterClock.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG1.EntityID, SU.GetDamage(CounterClock.LG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG2.EntityID, SU.GetDamage(CounterClock.LG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG3.EntityID, SU.GetDamage(CounterClock.LG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG4.EntityID, SU.GetDamage(CounterClock.LG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG5.EntityID, SU.GetDamage(CounterClock.LG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG6.EntityID, SU.GetDamage(CounterClock.LG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG1.EntityID, SU.GetDamage(CounterClock.RG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG2.EntityID, SU.GetDamage(CounterClock.RG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG3.EntityID, SU.GetDamage(CounterClock.RG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG4.EntityID, SU.GetDamage(CounterClock.RG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG5.EntityID, SU.GetDamage(CounterClock.RG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG6.EntityID, SU.GetDamage(CounterClock.RG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG7.EntityID, SU.GetDamage(CounterClock.RG7.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG8.EntityID, SU.GetDamage(CounterClock.RG8.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG9.EntityID, SU.GetDamage(CounterClock.RG9.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG10.EntityID, SU.GetDamage(CounterClock.RG10.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG11.EntityID, SU.GetDamage(CounterClock.RG11.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG12.EntityID, SU.GetDamage(CounterClock.RG12.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG13.EntityID, SU.GetDamage(CounterClock.RG13.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG14.EntityID, SU.GetDamage(CounterClock.RG14.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG15.EntityID, SU.GetDamage(CounterClock.RG15.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG16.EntityID, SU.GetDamage(CounterClock.RG16.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG17.EntityID, SU.GetDamage(CounterClock.RG17.CurHP));
                            //}
                            #endregion
                            #endregion
                            break;
                        }
                    #endregion
                    #region Snow
                    case 5010:
                        {
                            ushort Dist = SU.Info.MaxDist;
                            if (World.H_Mobs.ContainsKey(Loc.Map))
                            {
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                {
                                    if (M.Alive)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= Dist)
                                            if (PKMode == PKMode.PK || !M.NeedsPKMode && !SU.MobTargets.ContainsKey(M))
                                                SU.MobTargets.Add(M, SU.GetDamage(M));
                                    }
                                }
                            }
                            if (!World.NoPKMaps.Contains(Loc.Map))
                                foreach (Character C in ScreenChars.Values)//World.H_Chars.Values
                                {
                                    if (C != null)
                                        if (C.Alive && Loc.Map == C.Loc.Map)
                                        {
                                            if (C.EntityID != EntityID)
                                                if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist) if (C.CanBeMeleed)
                                                        if (C.PKAble(PKMode, this) && !SU.PlayerTargets.ContainsKey(C))
                                                            SU.PlayerTargets.Add(C, SU.GetDamage(C));
                                        }
                                }
                            if (Loc.Map == 1039)
                            {
                                Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                                foreach (NPC C in MapNPC.Values)
                                {
                                    if ((C.Flags == 21 || C.Flags == 22) && Level >= C.Level)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                            if (!SU.NPCTargets.ContainsKey(C))
                                                SU.NPCTargets.Add(C, SU.GetDamage(C));
                                    }
                                }
                            }
                            foreach (SOB S in World.H_SOBs.Values)
                            {
                                if (Loc.Map == S.Loc.Map)
                                {
                                    if (S.IsPole())
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && S.War && MyGuild != null && (S.LastWinner == null || MyGuild.GuildID != S.LastWinner.GuildID))
                                            SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                    }

                                    else if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist)
                                        SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                }
                            }
                            #region unused
                            //else if (Loc.Map == 1038)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Dist && GuildWars.War && MyGuild != null && (GuildWars.LastWinner == null || MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                            //        SU.MiscTargets.Add(GuildWars.ThePole.EntityID, SU.GetDamage(GuildWars.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(GuildWars.TheRightGate.EntityID, SU.GetDamage(GuildWars.TheRightGate.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(GuildWars.TheLeftGate.EntityID, SU.GetDamage(GuildWars.TheLeftGate.CurHP));
                            //}
                            #region Counter Clock GW
                            //else if (Loc.Map == 1844)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Dist && CounterClock.War && MyGuild != null && (CounterClock.LastWinner == null || MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                            //        SU.MiscTargets.Add(CounterClock.ThePole.EntityID, SU.GetDamage(CounterClock.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG1.EntityID, SU.GetDamage(CounterClock.LG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG2.EntityID, SU.GetDamage(CounterClock.LG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG3.EntityID, SU.GetDamage(CounterClock.LG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG4.EntityID, SU.GetDamage(CounterClock.LG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG5.EntityID, SU.GetDamage(CounterClock.LG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG6.EntityID, SU.GetDamage(CounterClock.LG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG1.EntityID, SU.GetDamage(CounterClock.RG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG2.EntityID, SU.GetDamage(CounterClock.RG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG3.EntityID, SU.GetDamage(CounterClock.RG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG4.EntityID, SU.GetDamage(CounterClock.RG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG5.EntityID, SU.GetDamage(CounterClock.RG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG6.EntityID, SU.GetDamage(CounterClock.RG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG7.EntityID, SU.GetDamage(CounterClock.RG7.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG8.EntityID, SU.GetDamage(CounterClock.RG8.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG9.EntityID, SU.GetDamage(CounterClock.RG9.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG10.EntityID, SU.GetDamage(CounterClock.RG10.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG11.EntityID, SU.GetDamage(CounterClock.RG11.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG12.EntityID, SU.GetDamage(CounterClock.RG12.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG13.EntityID, SU.GetDamage(CounterClock.RG13.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG14.EntityID, SU.GetDamage(CounterClock.RG14.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG15.EntityID, SU.GetDamage(CounterClock.RG15.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG16.EntityID, SU.GetDamage(CounterClock.RG16.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG17.EntityID, SU.GetDamage(CounterClock.RG17.CurHP));
                            //}
                            #endregion
                            #endregion
                            break;
                        }
                    #endregion
                    #region SpeedGun
                    case 1260:
                        {
                            ushort Dist = SU.Info.MaxDist;
                            InLineAlgorithm ila = new InLineAlgorithm(Loc.X,
                                       X, Loc.Y, Y, (byte)Dist, InLineAlgorithm.Algorithm.DDA);
                            if (World.H_Mobs.ContainsKey(Loc.Map))
                            {
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                {
                                    if (M.Alive)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= Dist)
                                            //if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y))
                                            if (ila.InLine(M.Loc.X, M.Loc.Y))
                                                if (PKMode == PKMode.PK || !M.NeedsPKMode && !SU.MobTargets.ContainsKey(M))
                                                    SU.MobTargets.Add(M, SU.GetDamage(M));
                                    }
                                }
                            }
                            if (!World.NoPKMaps.Contains(Loc.Map))
                                foreach (Character C in ScreenChars.Values)//World.H_Chars.Values
                                //  for (int x = 0; x < Program.ThreadInfo[3].Array.Length; x++)
                                {

                                    //Character C = Program.ThreadInfo[3].Array[x];
                                    if (C != null)
                                        if (C.Alive && Loc.Map == C.Loc.Map)
                                        {
                                            if (C.EntityID != EntityID)
                                                if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist) if (C.CanBeMeleed)
                                                        // if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y))
                                                        if (ila.InLine(C.Loc.X, C.Loc.Y))
                                                            if (C.PKAble(PKMode, this) && !SU.PlayerTargets.ContainsKey(C))
                                                                SU.PlayerTargets.Add(C, SU.GetDamage(C));
                                        }
                                }
                            if (Loc.Map == 1039)
                            {
                                Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                                foreach (NPC C in MapNPC.Values)
                                {

                                    if ((C.Flags == 21 || C.Flags == 22) && Level >= C.Level)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                            // if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y))
                                            if (ila.InLine(C.Loc.X, C.Loc.Y))
                                                if (!SU.NPCTargets.ContainsKey(C))
                                                    SU.NPCTargets.Add(C, SU.GetDamage(C));
                                    }
                                }
                            }
                            foreach (SOB S in World.H_SOBs.Values)
                            {
                                if (Loc.Map == S.Loc.Map)
                                {
                                    if (S.IsPole())
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && S.War && MyGuild != null && (S.LastWinner == null || MyGuild.GuildID != S.LastWinner.GuildID))
                                            if (ila.InLine(S.Loc.X, S.Loc.Y))
                                                SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                    }
                                    else if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && ila.InLine(S.Loc.X, S.Loc.Y))
                                        SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                }
                            }
                            #region unused
                            //else if (Loc.Map == 1038)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Dist && GuildWars.War && MyGuild != null && (GuildWars.LastWinner == null || MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                            //        // if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y))
                            //        if (ila.InLine(GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.ThePole.EntityID, SU.GetDamage(GuildWars.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Dist)
                            //        //if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y))
                            //        if (ila.InLine(GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.TheRightGate.EntityID, SU.GetDamage(GuildWars.TheRightGate.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Dist)
                            //        //if (MyMath.PointDirecton(Loc.X, Loc.Y, X, Y) == MyMath.PointDirecton(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y))
                            //        if (ila.InLine(GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.TheLeftGate.EntityID, SU.GetDamage(GuildWars.TheLeftGate.CurHP));
                            //}
                            #region Counter Clock GW
                            //else if (Loc.Map == 1844)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Dist && CounterClock.War && MyGuild != null && (CounterClock.LastWinner == null || MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                            //        SU.MiscTargets.Add(CounterClock.ThePole.EntityID, SU.GetDamage(CounterClock.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG1.EntityID, SU.GetDamage(CounterClock.LG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG2.EntityID, SU.GetDamage(CounterClock.LG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG3.EntityID, SU.GetDamage(CounterClock.LG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG4.EntityID, SU.GetDamage(CounterClock.LG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG5.EntityID, SU.GetDamage(CounterClock.LG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.LG6.EntityID, SU.GetDamage(CounterClock.LG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG1.EntityID, SU.GetDamage(CounterClock.RG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG2.EntityID, SU.GetDamage(CounterClock.RG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG3.EntityID, SU.GetDamage(CounterClock.RG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG4.EntityID, SU.GetDamage(CounterClock.RG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG5.EntityID, SU.GetDamage(CounterClock.RG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG6.EntityID, SU.GetDamage(CounterClock.RG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG7.EntityID, SU.GetDamage(CounterClock.RG7.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG8.EntityID, SU.GetDamage(CounterClock.RG8.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG9.EntityID, SU.GetDamage(CounterClock.RG9.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG10.EntityID, SU.GetDamage(CounterClock.RG10.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG11.EntityID, SU.GetDamage(CounterClock.RG11.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG12.EntityID, SU.GetDamage(CounterClock.RG12.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG13.EntityID, SU.GetDamage(CounterClock.RG13.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG14.EntityID, SU.GetDamage(CounterClock.RG14.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG15.EntityID, SU.GetDamage(CounterClock.RG15.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG16.EntityID, SU.GetDamage(CounterClock.RG16.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Dist)
                            //        SU.MiscTargets.Add(CounterClock.RG17.EntityID, SU.GetDamage(CounterClock.RG17.CurHP));
                            //}
                            #endregion
                            #endregion
                            break;
                        }
                    #endregion
                    #region FrontSpell
                    case 1250:
                    case 5050:
                    case 5020:
                    case 1300:
                    case 7040:
                        {
                            ushort Dist = SU.Info.MaxDist;
                            if (World.H_Mobs.ContainsKey(Loc.Map))
                            {
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                {
                                    if (M.Alive && M != null)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= Dist)
                                            // if (SU.InSector(M.Loc.X, M.Loc.Y))
                                            if (TargetMonster != null)
                                                if (SU.FunctiaCuUnghiVariabil(TargetMonster.Loc.X, TargetMonster.Loc.Y, Loc.X, Loc.Y, M.Loc.X, M.Loc.Y, SU.Info.SectorSize))
                                                    if (PKMode == PKMode.PK || !M.NeedsPKMode && !SU.MobTargets.ContainsKey(M))
                                                        SU.MobTargets.Add(M, SU.GetDamage(M));
                                    }
                                }
                            }
                            if (!World.NoPKMaps.Contains(Loc.Map))
                                foreach (Character C in ScreenChars.Values)//World.H_Chars.Values
                                //  for (int x = 0; x < Program.ThreadInfo[3].Array.Length; x++)
                                {

                                    // Character C = Program.ThreadInfo[3].Array[x];
                                    if (C != null)
                                        if (C.Alive && Loc.Map == C.Loc.Map)
                                        {
                                            if (C.EntityID != EntityID)
                                                if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                                    if (SU.InSector(C.Loc.X, C.Loc.Y)) if (C.CanBeMeleed)
                                                            if (C.PKAble(PKMode, this) && !SU.PlayerTargets.ContainsKey(C))
                                                                SU.PlayerTargets.Add(C, SU.GetDamage(C));
                                        }
                                }
                            if (Loc.Map == 1039)
                            {
                                Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                                foreach (NPC C in MapNPC.Values)
                                {
                                    if ((C.Flags == 21 || C.Flags == 22) && Level >= C.Level)
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= Dist)
                                            if (SU.InSector(C.Loc.X, C.Loc.Y))
                                                if (!SU.NPCTargets.ContainsKey(C))
                                                    SU.NPCTargets.Add(C, SU.GetDamage(C));
                                    }
                                }
                            }
                            foreach (SOB S in World.H_SOBs.Values)
                            {
                                if (Loc.Map == S.Loc.Map)
                                {
                                    if (S.IsPole())
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && S.War && MyGuild != null && (S.LastWinner == null || MyGuild.GuildID != S.LastWinner.GuildID))
                                            if (SU.InSector(S.Loc.X, S.Loc.Y))
                                                SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                    }

                                    else if (MyMath.PointDistance(Loc.X, Loc.Y, S.Loc.X, S.Loc.Y) <= Dist && SU.InSector(S.Loc.X, S.Loc.Y))
                                        SU.MiscTargets.Add(S.EntityID, SU.GetDamage(S));
                                }
                            }

                            #region unused
                            //else if (Loc.Map == 1038)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Dist && GuildWars.War && MyGuild != null && (GuildWars.LastWinner == null || MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                            //        if (SU.InSector(GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.ThePole.EntityID, SU.GetDamage(GuildWars.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Dist)
                            //        if (SU.InSector(GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.TheRightGate.EntityID, SU.GetDamage(GuildWars.TheRightGate.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Dist)
                            //        if (SU.InSector(GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y))
                            //            SU.MiscTargets.Add(GuildWars.TheLeftGate.EntityID, SU.GetDamage(GuildWars.TheLeftGate.CurHP));
                            //}
                            #region Counter Clock GW
                            //else if (Loc.Map == 1844)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Dist && CounterClock.War && MyGuild != null && (CounterClock.LastWinner == null || MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                            //        if (SU.InSector(CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.ThePole.EntityID, SU.GetDamage(CounterClock.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG1.EntityID, SU.GetDamage(CounterClock.LG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG2.EntityID, SU.GetDamage(CounterClock.LG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG3.EntityID, SU.GetDamage(CounterClock.LG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG4.EntityID, SU.GetDamage(CounterClock.LG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG5.EntityID, SU.GetDamage(CounterClock.LG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.LG6.EntityID, SU.GetDamage(CounterClock.LG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG1.EntityID, SU.GetDamage(CounterClock.RG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG2.EntityID, SU.GetDamage(CounterClock.RG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG3.EntityID, SU.GetDamage(CounterClock.RG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG4.EntityID, SU.GetDamage(CounterClock.RG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG5.EntityID, SU.GetDamage(CounterClock.RG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG6.EntityID, SU.GetDamage(CounterClock.RG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG7.EntityID, SU.GetDamage(CounterClock.RG7.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG8.EntityID, SU.GetDamage(CounterClock.RG8.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG9.EntityID, SU.GetDamage(CounterClock.RG9.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG10.EntityID, SU.GetDamage(CounterClock.RG10.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG11.EntityID, SU.GetDamage(CounterClock.RG11.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG12.EntityID, SU.GetDamage(CounterClock.RG12.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG13.EntityID, SU.GetDamage(CounterClock.RG13.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG14.EntityID, SU.GetDamage(CounterClock.RG14.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG15.EntityID, SU.GetDamage(CounterClock.RG15.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG16.EntityID, SU.GetDamage(CounterClock.RG16.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Dist)
                            //        if (SU.InSector(CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y))
                            //            SU.MiscTargets.Add(CounterClock.RG17.EntityID, SU.GetDamage(CounterClock.RG17.CurHP));
                            //}
                            #endregion
                            #endregion
                            break;
                        }
                    #endregion
                    #endregion
                    #region Single Target
                    case 5030:
                    case 1290:
                    case 5040:
                    case 7000:
                    case 7010:
                    case 7030:
                        {
                            #region Phoenix, Penetration, Boom,Seizer,Earthquake,Celestial
                            ushort Dist = SU.Info.MaxDist;

                            if (TargetMonster != null)
                            {
                                if (TargetMonster.Alive)
                                {
                                    if (MyMath.PointDistance(Loc.X, Loc.Y, X, Y) <= Dist)
                                    {
                                        if ((PKMode == PKMode.PK || !TargetMonster.NeedsPKMode) && !SU.MobTargets.ContainsKey(TargetMonster))
                                        {
                                            SU.MobTargets.Add(TargetMonster, SU.GetDamage(TargetMonster));
                                        }
                                    }
                                }
                            }
                            if (CharacterTarget != null)
                            {
                                if (CharacterTarget.Alive)
                                {
                                    if (MyMath.PointDistance(Loc.X, Loc.Y, X, Y) <= Dist)
                                    {
                                        if (CharacterTarget.PKAble(PKMode, this) && !SU.PlayerTargets.ContainsKey(CharacterTarget) && !World.NoPKMaps.Contains(Loc.Map))
                                            SU.PlayerTargets.Add(CharacterTarget, SU.GetDamage(CharacterTarget));
                                    }
                                }
                            }
                            if (TargetNpc != null)
                            {
                                if ((TargetNpc.Flags == 21 || TargetNpc.Flags == 22) && Level >= TargetNpc.Level)
                                {
                                    if (MyMath.PointDistance(Loc.X, Loc.Y, X, Y) <= Dist)
                                    {
                                        if (!SU.NPCTargets.ContainsKey(TargetNpc))
                                        {
                                            SU.NPCTargets.Add(TargetNpc, SU.GetDamage(TargetNpc));
                                        }
                                    }
                                }
                            }
                            if (World.H_SOBs.ContainsKey(Target))
                            {
                                if (Loc.Map == World.H_SOBs[Target].Loc.Map)
                                {
                                    if (World.H_SOBs[Target].IsPole())
                                    {
                                        if (MyMath.PointDistance(Loc.X, Loc.Y, World.H_SOBs[Target].Loc.X, World.H_SOBs[Target].Loc.Y) <= (Dist + 1) && World.H_SOBs[Target].War && MyGuild != null && (World.H_SOBs[Target].LastWinner == null || MyGuild.GuildID != World.H_SOBs[Target].LastWinner.GuildID))
                                            SU.MiscTargets.Add(World.H_SOBs[Target].EntityID, SU.GetDamage(World.H_SOBs[Target]));
                                    }

                                    else if (MyMath.PointDistance(Loc.X, Loc.Y, World.H_SOBs[Target].Loc.X, World.H_SOBs[Target].Loc.Y) <= (Dist + 1))
                                        SU.MiscTargets.Add(World.H_SOBs[Target].EntityID, SU.GetDamage(World.H_SOBs[Target]));
                                }
                            }
                            #region unused
                            //if (Loc.Map == 1038)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= (Dist + 1) && GuildWars.War && MyGuild != null && (GuildWars.LastWinner == null || MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                            //        if (GuildWars.ThePole.EntityID == Target)
                            //            SU.MiscTargets.Add(GuildWars.ThePole.EntityID, SU.GetDamage(GuildWars.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= (Dist + 1))
                            //        if (GuildWars.TheRightGate.EntityID == Target)
                            //            SU.MiscTargets.Add(GuildWars.TheRightGate.EntityID, SU.GetDamage(GuildWars.TheRightGate.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= (Dist + 1))
                            //        if (GuildWars.TheLeftGate.EntityID == Target)
                            //            SU.MiscTargets.Add(GuildWars.TheLeftGate.EntityID, SU.GetDamage(GuildWars.TheLeftGate.CurHP));
                            //}
                            #region Counter Clock GW
                            //else if (Loc.Map == 1844)
                            //{
                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Dist && CounterClock.War && MyGuild != null && (CounterClock.LastWinner == null || MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                            //        if (CounterClock.ThePole.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.ThePole.EntityID, SU.GetDamage(CounterClock.ThePole.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG1.Loc.X, CounterClock.LG1.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG1.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG1.EntityID, SU.GetDamage(CounterClock.LG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG2.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG2.EntityID, SU.GetDamage(CounterClock.LG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG3.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG3.EntityID, SU.GetDamage(CounterClock.LG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG4.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG4.EntityID, SU.GetDamage(CounterClock.LG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG5.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG5.EntityID, SU.GetDamage(CounterClock.LG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.LG6.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.LG6.EntityID, SU.GetDamage(CounterClock.LG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG1.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG1.EntityID, SU.GetDamage(CounterClock.RG1.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG2.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG2.EntityID, SU.GetDamage(CounterClock.RG2.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG3.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG3.EntityID, SU.GetDamage(CounterClock.RG3.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG4.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG4.EntityID, SU.GetDamage(CounterClock.RG4.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG5.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG5.EntityID, SU.GetDamage(CounterClock.RG5.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG6.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG6.EntityID, SU.GetDamage(CounterClock.RG6.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG7.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG7.EntityID, SU.GetDamage(CounterClock.RG7.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG8.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG8.EntityID, SU.GetDamage(CounterClock.RG8.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG9.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG9.EntityID, SU.GetDamage(CounterClock.RG9.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG10.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG10.EntityID, SU.GetDamage(CounterClock.RG10.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG11.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG11.EntityID, SU.GetDamage(CounterClock.RG11.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG12.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG12.EntityID, SU.GetDamage(CounterClock.RG12.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG13.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG13.EntityID, SU.GetDamage(CounterClock.RG13.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG14.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG14.EntityID, SU.GetDamage(CounterClock.RG14.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG15.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG15.EntityID, SU.GetDamage(CounterClock.RG15.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG16.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG16.EntityID, SU.GetDamage(CounterClock.RG16.CurHP));

                            //    if (MyMath.PointDistance(Loc.X, Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= (Dist + 1))
                            //        if (CounterClock.RG17.EntityID == Target)
                            //            SU.MiscTargets.Add(CounterClock.RG17.EntityID, SU.GetDamage(CounterClock.RG17.CurHP));
                            //}
                            #endregion
                            #endregion
                            break;
                            #endregion
                        }
                        #endregion

                        #endregion

                }
            }
            catch (Exception E) { Game.World.ExcAdd += E.ToString() + "\r\n"; }
            SU.Use();
        }
        public void Shift(byte Dir)//(ushort X, ushort Y)
        {
            //byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(X, Y, Loc.X, Loc.Y) / 45 % 8)) - 1 % 8);
            //byte Dir = (byte)((int)ToDir % 8);
            ushort X = Loc.X;
            ushort Y = Loc.Y;
            if (Dir == 0)
                Y += 1;
            if (Dir == 2)
                X -= 1;
            if (Dir == 4)
                Y -= 1;
            if (Dir == 6)
                X += 1;
            if (Dir == 1)
            {
                X -= 1;
                Y += 1;
            }
            if (Dir == 3)
            {
                X -= 1;
                Y -= 1;
            }
            if (Dir == 5)
            {
                X += 1;
                Y -= 1;
            }
            if (Dir == 7)
            {
                X += 1;
                Y += 1;
            }
            // if (MyMath.PointDistance(X, Y, Loc.X, Loc.Y) < 20)
            //{
            World.Action(this, Packets.GeneralData(EntityID, 0, X, Y, 0x9c).Get);
            Loc.X = X;
            Loc.Y = Y;
            Direction = Dir;

            World.Spawns(this, true);
            // }
        }

        public void AddExp(double Count)
        {
            if (Level < 130)
            {
                uint Amount = (uint)(ExpBallExp * Count);
                Amount = (uint)(Amount * EqStats.GemExtraExp /** ExperienceRate*/);
                ulong CurExp = Experience;
                byte CurLevel = Level;

                CurExp += Amount;
                while (CurLevel < 130 && CurExp > Database.LevelExp[CurLevel])
                {
                    CurExp -= Database.LevelExp[CurLevel];
                    CurLevel++;
                    if (CurLevel > 110)
                    {
                        World.InfoAdd += Name + " has reached level " + CurLevel + "\r\n";
                        World.SendMsgToAll("[SYSTEM]", "Congratulations! " + Name + " has reached level " + CurLevel + "!", 2005, 0);
                        if (Level < 15 && CurLevel >= 15)
                        {
                            MyClient.LocalMessage(2000, "You have reached level 15! Make sure you visit the Promotion Center and promote yourself to receive a set of items!");
                            MyClient.AddSend(Packets.ShowDialog(1, 1));
                            MyClient.AddSend(Packets.ShowDialog(21, 1));
                        }
                    }

                    if (CurLevel >= 3)
                    {
                        if (Job <= 55 && Job >= 50)
                            NewSkill(new Skill() { ID = 6011 });
                        else if (Job <= 45 && Job >= 40)
                            NewSkill(new Skill() { ID = 8002 });
                        else if (Job >= 10 && Job <= 15)
                            NewSkill(new Skill() { ID = 1110 });
                        else if (Job >= 20 && Job <= 25)
                            NewSkill(new Skill() { ID = 1025 });
                    }
                }
                if (CurLevel > Level)
                {
                    World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 92).Get);
                    if (Reborn || CurLevel >= 120)
                    {
                        if (Reborn)
                            StatPoints += (ushort)((CurLevel - Level) * 3);
                        else
                        {
                            if (Level < 120)
                                Level = 120;
                            StatPoints += (ushort)((CurLevel - Level) * 3);
                        }
                    }
                    CurHP = MaxHP;
                }
                if (CurLevel > Level)
                    Level = CurLevel;
                Experience = CurExp;
                World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "angelwing")).Get);
            }
        }
        public void IncreaseExp(uint Amount, bool isTeamExp, bool Killed, Character Attacker = null, byte MobLevel = 0)
        {

            if (Level < 130)
            {
                if (Loc.Map == 1039)
                {
                    if (Amount > Level / 2)
                        Amount = (uint)(Level / 2);
                }
                Amount = (uint)(Amount * EqStats.GemExtraExp * ExperienceRate);
                if (DoubleExp)
                    Amount *= 3;
                if (MyClient.MyChar.VipLevel == 6)
                    Amount *= (uint)(1.5);
                if (MyGuild != null)
                    Amount = (Amount * (100 + (MyGuild.Wins))) / 100;
                if ((World.ExpEvent || World.EREvent > DateTime.Now))
                    Amount *= 2;
                if (Level > 124)
                    Amount /= 2;

                bool noobexp = false;
                bool marrieexp100 = false;
                bool marrieexp20 = false;
                bool waterexp = false;
                byte Lev = Level;
                #region checkmyteam for extras
                if (MyTeam != null && isTeamExp)
                {
                    foreach (Character C in MyTeam.Members)
                    {
                        if (C != null)
                        {
                            if (EntityID != C.EntityID && C.Alive && C.Loc.Map == Loc.Map && MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= 32)
                            {
                                /* if (Level > C.Level + 30)
                                     noobexp = true;*/


                                if (Attacker != null)
                                {
                                    if (Level + 20 > MobLevel)
                                    {
                                        if (Spouse == Attacker.Name)
                                        {
                                            if (Job >= 132 && Job <= 135)
                                                marrieexp100 = true;
                                            else marrieexp20 = true;
                                        }
                                        if (Job >= 132 && Job <= 135)
                                            waterexp = true;
                                        if (C.Level + 20 <= MobLevel)
                                            noobexp = true;
                                    }

                                }
                            }
                        }
                    }
                }
                #endregion

                if (noobexp)
                    Amount *= 2;
                if (waterexp)
                    Amount *= 2;
                if (marrieexp100)
                    Amount *= 2;
                else if (marrieexp20)
                    Amount = (uint)(Amount * 1.2);

                #region Experience points Messages
                if (Killed)
                {
                    if (!noobexp && (!marrieexp100 && !marrieexp20) && !isTeamExp && Loc.Map != 1039)
                        MyClient.LocalMessage(2005, Amount + " extra experience points gained for killing.");
                    else if (!noobexp && (!marrieexp100 && !marrieexp20) && isTeamExp && Loc.Map != 1039)
                        MyClient.LocalMessage(2005, Amount + " team experience points gained.");
                    else if (noobexp && (!marrieexp100 && !marrieexp20) && isTeamExp && Loc.Map != 1039)
                        MyClient.LocalMessage(2005, "You gained  " + Amount + " team experience points with additional rewarding experience points due to low level teammates.");
                    else if (!noobexp && (marrieexp100 || marrieexp20) && isTeamExp && Loc.Map != 1039)
                        MyClient.LocalMessage(2005, "You gained  " + Amount + " team experience points with additional rewarding experience points due to marriage teammates.");
                    else if (noobexp && (marrieexp100 || marrieexp20) && isTeamExp && Loc.Map != 1039)
                        MyClient.LocalMessage(2005, "You gained  " + Amount + " team experience points with additional rewarding experience points due to low level and marriage teammates.");
                }
                #endregion

                ulong CurExp = Experience;
                byte CurLevel = Level;

                CurExp += Amount;
                while (CurLevel < 130 && CurExp > Database.LevelExp[CurLevel])
                {
                    CurExp -= Database.LevelExp[CurLevel];
                    CurLevel++;
                    if (CurLevel > 110)
                    {
                        World.InfoAdd += Name + " has reached level " + CurLevel + "\r\n";
                        World.SendMsgToAll("[SYSTEM]", "Congratulations! " + Name + " has reached level " + CurLevel + "!", 2005, 0);
                        if (Level < 15 && CurLevel >= 15)
                        {
                            MyClient.LocalMessage(2000, "You have reached level 15! Make sure you visit the Promotion Center and promote yourself to receive a set of items!");
                            MyClient.AddSend(Packets.ShowDialog(1, 1));
                            MyClient.AddSend(Packets.ShowDialog(21, 1));
                        }
                    }

                    if (CurLevel >= 3)
                    {
                        if (Job <= 55 && Job >= 50)
                            NewSkill(new Skill() { ID = 6011 });
                        else if (Job <= 45 && Job >= 40)
                            NewSkill(new Skill() { ID = 8002 });
                        else if (Job >= 10 && Job <= 15)
                            NewSkill(new Skill() { ID = 1110 });
                        else if (Job >= 20 && Job <= 25)
                            NewSkill(new Skill() { ID = 1025 });
                    }
                }
                if (CurLevel > Level)
                {
                    World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 92).Get);
                    if (Reborn || CurLevel >= 120)
                    {
                        if (Reborn)
                            StatPoints += (ushort)((CurLevel - Level) * 3);
                        else
                        {
                            if (Level < 120)
                                Level = 120;
                            StatPoints += (ushort)((CurLevel - Level) * 3);
                        }
                    }
                    CurHP = MaxHP;
                }
                if (CurLevel > Level)
                    Level = CurLevel;

                Experience = CurExp;
                if (MyTeam != null)
                    if (Attacker != null)
                    {
                        if (Attacker.MyTeam != null)
                        {
                            if (Attacker.MyTeam.Leader.Level >= 70 && Level <= 70)
                            {
                                for (; Lev < Level; Lev++)
                                {
                                    if (Attacker.MyTeam.Leader.Loc.Map == Attacker.Loc.Map && MyMath.PointDistance(Attacker.MyTeam.Leader.Loc.X, Attacker.MyTeam.Leader.Loc.Y, Attacker.Loc.X, Attacker.Loc.Y) <= 36)
                                    {
                                        uint VPAmount = (uint)Math.Max(1, Lev * 7 - 12);
                                        Attacker.MyTeam.Leader.VP += VPAmount;
                                        Attacker.MyTeam.Message(Packets.ChatMessage(45216, "SYSTEM", "ALL", Attacker.MyTeam.Leader.Name + " gained " + VPAmount + " virtue points.", 2003, 0));
                                    }
                                }
                            }
                        }
                    }
            }
        }

        public void AddProfExp(ushort Wep, uint Amount)
        {
            if (Wep != 105)
                if (Profs.ContainsKey(Wep))
                {
                    Prof P = (Prof)Profs[Wep];//ok mc..ma mai gandesc
                    if (P.Lvl < 20)
                    {
                        byte TempExp = ExperienceRate;
                        if (!World.LowRatedServer)
                        {
                            if (TempExp > 3)
                                TempExp = 3;
                        }
                        else
                        {
                            if (TempExp > 2)
                                TempExp = 2;
                        }
                        if (P.Lvl < 12)
                        {
                            Amount = (uint)(Amount * EqStats.GemExtraProf * TempExp); // gump
                        }
                        else
                        {
                            Amount = (uint)(Amount * EqStats.GemExtraProf * TempExp);
                        }
                        if (World.EventProfExp)
                            Amount = (uint)(Amount * 1.1);
                        if (P.Lvl < 12 && Level >= ((P.Lvl * 10) - 10))
                            P.Exp += Amount;
                        else if (P.Lvl >= 12 && Level >= 110)
                            P.Exp += Amount;
                        if (P.Exp >= Database.ProfExp[P.Lvl])
                        {
                            P.Lvl++;
                            P.Exp = 0;
                            MyClient.LocalMessage(2000, "Your proficiency level has increased.");
                        }
                        if (ProfsBeforeReborn.ContainsKey(Wep))
                        {
                            Prof PP = (Prof)ProfsBeforeReborn[Wep];
                            if (P.Lvl >= (byte)(PP.Lvl / 2) && PP.Lvl >= P.Lvl)
                            {
                                // P = PP;
                                P.Lvl = PP.Lvl;
                                P.Exp = PP.Exp;
                                ProfsBeforeReborn.Remove(Wep);

                                MyClient.LocalMessage(2011, "Your weapon proficiency jumped back to it's level before reborning!");
                            }
                        }
                        /* Profs.Remove(Wep);
                                if (!Profs.ContainsKey(Wep))
                                    Profs.Add(Wep, P);*/
                        MyClient.AddSend(Packets.Prof(P));
                    }
                }
                else
                {
                    Prof P = new Prof();
                    P.ID = Wep;
                    P.Lvl = 0;
                    P.Exp = 0;
                    NewProf(P);
                }
        }
        public void NewProf(Prof P)
        {
            if (!Profs.ContainsKey(P.ID))
            {
                Profs.TryAdd(P.ID, P);
                MyClient.AddSend(Packets.Prof(P));
            }
        }
        public void RWProf(Prof P)
        {
            if (Profs.ContainsKey(P.ID))
                Profs.Remove(P.ID);
            Profs.TryAdd(P.ID, P);
            MyClient.AddSend(Packets.Prof(P));
        }
        public void NewSkill(Skill S)
        {
            if (!Skills.ContainsKey(S.ID))
            {
                Skills.TryAdd(S.ID, S);
                MyClient.AddSend(Packets.Skill(S));
            }
            if (S.Lvl >= 1)
                RWSkill(S);
        }
        public void RWSkill(Skill S)
        {
            if (Skills.ContainsKey(S.ID))
                Skills.Remove(S.ID);
            Skills.TryAdd(S.ID, S);
            MyClient.AddSend(Packets.Skill(S));
        }
        public void InitAngry(bool _kind)
        {
            if (!MyClient.GM && MyClient.Soc.Connected)
            {
                Random _rand = new Random();
                // if (LastTrade.AddMilliseconds(5000) < DateTime.Now)
                // {
                #region Silver drop
                if (/*Loc.Map != 1038 && */!World.EventsMaps.Contains(Loc.Map) && !World.FreePKMaps.Contains(Loc.Map)/* && Loc.Map != 1005 && Loc.Map != 6000*/ && Loc.Map < 8000)
                {
                    int DropSilver = _rand.Next(0, (int)Silvers);
                    if (DropSilver > 0)
                    {
                        if (DropSilver > 1000)
                            DropSilver /= 10;
                        DroppedItem DI2 = new DroppedItem();
                        DI2.DropTime = DateTime.Now;
                        DI2.UID = (uint)Rnd.Next(10000000);
                        DI2.Loc = new Location();
                        DI2.Loc = Loc;
                        DI2.Loc.Map = Loc.Map;
                        DI2.Info = new Item();
                        DI2.UID = (uint)Rnd.Next(10000000);
                        DI2.Info.UID = DI2.UID;
                        if ((uint)DropSilver <= Silvers)
                        {
                            DI2.Silvers = (uint)DropSilver;
                            if (DI2.Silvers < 10)
                                DI2.Info.ID = 1090000;
                            else if (DI2.Silvers < 100)
                                DI2.Info.ID = 1090010;
                            else if (DI2.Silvers < 1000)
                                DI2.Info.ID = 1090020;
                            else if (DI2.Silvers < 3000)
                                DI2.Info.ID = 1091000;
                            else if (DI2.Silvers < 10000)
                                DI2.Info.ID = 1091010;
                            else
                                DI2.Info.ID = 1091020;
                            if (!World.H_Items.ContainsKey(Loc.Map))
                                World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            //  if (!DI2.FindPlace((ConcurrentDictionary<uint, DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                            World.DropAdd += "PK DROP (<30pk): " + Name + " has dropped silvers: " + DropSilver + " from total gold of: " + Silvers + " Map " + Loc.Map + " X " + Loc.X + " Y " + Loc.Y + " : " + DateTime.Now + "\r\n";
                            Silvers -= (uint)DropSilver;
                            //Silvers = (uint)(Silvers - DropSilver);

                            DI2.Drop();
                        }

                    }
                }
                #endregion
                #region Item drop
                if (Inventory.Count > 3)
                {
                    byte _val1 = (byte)_rand.Next(1, (int)(Inventory.Count / 2.5));
                    List<Item> _list1 = new List<Item>();
                    for (byte _val2 = 0; _val2 < _val1; _val2++)
                    {
                        byte _val3 = (byte)_rand.Next(0, (int)(Inventory.Count - 1));
                        Item _item = Inventory[_val3] as Item;
                        if (_item.FreeItem || _item.ID == 750000)
                            continue;
                        Game.DroppedItem DI = new Ultimate.Game.DroppedItem();
                        DI.Info = _item;
                        DI.DropTime = DateTime.Now;
                        DI.Loc = Loc;
                        DI.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                        DI.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                        DI.UID = (uint)Program.Rnd.Next(10000000);
                        if (DI.Info.ID != 780001 && (DI.Info.ID <= 721575 || DI.Info.ID >= 722721 || DI.Info.ID == 722384))
                            if (!DMaps.EventMaps.ContainsKey(Loc.Map) && ((!World.EventsMaps.Contains(Loc.Map) && !World.FreePKMaps.Contains(Loc.Map) && Loc.Map < 8000) || DI.Info.ID == 1000000 || DI.Info.ID == 1000010 || DI.Info.ID == 1000020 || DI.Info.ID == 1000030 || DI.Info.ID == 1001000 || DI.Info.ID == 1001010 || DI.Info.ID == 1001020 || DI.Info.ID == 1001030 || DI.Info.ID == 1001040 || DI.Info.ID == 1002000 || DI.Info.ID == 1002010 || DI.Info.ID == 1002020 || DI.Info.ID == 1002030 || DI.Info.ID == 1002040 || DI.Info.ID == 1002050 || DI.Info.ID == 1050000 || DI.Info.ID == 1050001 || DI.Info.ID == 1050002 || DI.Info.ID == 1051000))
                            {
                                if (!World.H_Items.ContainsKey(Loc.Map))
                                    World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                                if (DI.FindPlace(World.H_Items[Loc.Map]))
                                {
                                    if (RemoveItem(_item.UID))
                                        DI.Drop();
                                }
                            }

                    }
                }
                #endregion
                #region Equipment drop
                if (_kind)
                {
                    if (PKPoints > 29 && !World.FreePKMaps.Contains(Loc.Map) /*&& Loc.Map != 1038*/ && !World.EventsMaps.Contains(Loc.Map) /*&& Loc.Map != 1005 && Loc.Map != 6001*/ && Loc.Map < 8000)
                    {
                        byte _val1 = 0;
                        Item[] _equipment = Equips;
                        foreach (Item _equip in _equipment)
                        {
                            if (_val1 == 2)
                                return;
                            if (_equip.ID == 0)
                                continue;
                            if (_equip.FreeItem)
                                continue;
                            if (MyMath.ChanceSuccess(9))
                            {
                                EquipStats(Equips.GetSlot(_equip.UID), false, false);
                                Equips.UnEquip(Equips.GetSlot(_equip.UID), this);
                                MyClient.AddSend(Packets.ItemPacket(_equip.UID, 0, 3));

                                Game.DroppedItem DI = new Ultimate.Game.DroppedItem();
                                DI.Info = _equip;
                                DI.DropTime = DateTime.Now;
                                DI.Loc = Loc;
                                DI.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                                DI.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                                DI.UID = (uint)Program.Rnd.Next(10000000);
                                if (!World.H_Items.ContainsKey(Loc.Map))
                                    World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                                if (!DI.FindPlace(World.H_Items[Loc.Map])) continue;
                                DI.Drop();
                                if (DI.Info.IsWorth())
                                    World.DropAdd += "PK DROP (<30pk): " + Name + " has dropped " + DI.UID + "~" + DI.Info.ID + "~" + DI.Info.Plus + "~" + DI.Info.Bless + "~" + DI.Info.Enchant + "~" + (byte)DI.Info.Soc1 + "~" + (byte)DI.Info.Soc2 + "~" + DI.Info.Progress + " Map " + Loc.Map + " X " + Loc.X + " Y " + Loc.Y + " : " + DateTime.Now + "\r\n";
                                _val1++;
                                return;

                            }
                        }
                    }
                }
                #endregion
                //}
            }
        }
        public void TakeAttack(Mob Attacker, uint Damage, AttackType AT, bool AOE = false, bool IsSkill = false)
        {
            if (ProtectTime.AddMilliseconds(0) > DateTime.Now && !CancelProtectTime)
                return;
            if (Protection) Damage = 0;
            if (Damage != 0)
            {
                Extra.Durability.DefenceDurability(MyClient);

                /* if (BuffOf(SkillsClass.ExtraEffect.Scapegoat).Eff == SkillsClass.ExtraEffect.Scapegoat && MyMath.ChanceSuccess(30))
                 {
                     Buff B = BuffOf(SkillsClass.ExtraEffect.Scapegoat);
                     BDelete.Add(B);
                     uint Dmg = (uint)(PrepareAttack(2, false) * B.Value);
                     Attacker.TakeAttack(this, ref Dmg, AttackType.Scapegoat, false);
                     return;//Will not be damaged
                 }*/
                if (AT == AttackType.Melee)
                {
                    ushort Def;
                    if (!Transformation.Transformed)
                        Def = EqStats.defense;
                    else Def = Transformation.Def;
                    //ushort Def = EqStats.defense;
                    if (Job % 10 >= 3 && !Transformation.Transformed)
                        Def = (ushort)(Def * 1.3);
                    Buff Shield = BuffOf(SkillsClass.ExtraEffect.MagicShield);
                    if (Shield.Eff == SkillsClass.ExtraEffect.MagicShield)
                        if (Shield.Value == 2)
                            Def = (ushort)(Def * 3);
                        else Def = (ushort)(Def * Shield.Value);


                    /*  if (Def >= Damage)
                          Damage = 1;
                      else
                          Damage -= Def;*/
                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);
                    if (Def >= Damage)
                        Damage = 1;
                    else
                        Damage -= Def;
                    if (EqStats.MeleeDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MeleeDamageDecrease;
                }
                else if (AT == AttackType.Ranged)
                {
                    if (!Transformation.Transformed)
                        Damage = (uint)((double)Damage * (((double)(106 - EqStats.Dodge) / 100)));
                    else
                        Damage = (uint)((double)Damage * (((double)(106 - Transformation.Dodge) / 100)));
                    Damage *= 2 / 3;
                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);

                    if (EqStats.MeleeDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MeleeDamageDecrease;
                }
                else
                {
                    if (EqStats.MagicDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MagicDamageDecrease;

                    if (!Transformation.Transformed)
                        if (EqStats.MDef1 < 106)
                        {
                            Damage = (uint)((double)Damage * (((double)(106 - EqStats.MDef1) / 100)));
                        }
                        else if (Transformation.MagicDef < 106) { Damage = (uint)((double)Damage * (((double)(106 - Transformation.MagicDef) / 100))); }

                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);

                    if (EqStats.MDef2 >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MDef2;
                }
            }
            else
                Damage = 1;

            if (AT != AttackType.Magic && Action == 250)
            {
                if (Stamina > 30)
                    Stamina -= 30;
                else
                    Stamina = 0;
            }
            Action = 100;
            if (CanReflect)
            {
                if (MyMath.ChanceSuccess(5))
                {
                    if (Damage >= 2600)
                        Damage = 2600;

                    Attacker.GetReflect(ref Damage, AT);
                    World.Action(this, Packets.StringPacket(EntityID, StringType.Effect, "MagicReflect").Get);
                    Damage = 0;
                    return;
                }
            }
            if (Damage < CurHP)
            {
                CurHP = (ushort)(CurHP - Damage);
                if (AT == AttackType.Magic || IsSkill)
                {
                    if (Attacker.MagicSkill == 0)
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Melee).Get);
                    else
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                }
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                AtkMem.Attacking = false;
                AtkMem.Target = 0;
                DeathHit = DateTime.Now;
                if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    InitAngry(false);
                Alive = false;
                CurHP = 0;

                if (AT == AttackType.Magic)
                {
                    World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                }

                else
                {
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                    if (Attacker.MobID == 98)
                    {
                        Damage = 0;
                        Action = 100;
                        Stamina = 100;
                        Ghost = false;
                        BlueName = false;
                        CurHP = MaxHP;
                        if (MaxMP > 1)
                            CurMP = MaxMP;
                        Alive = true;
                        StatEff.Remove(StatusEffectEn.Dead);
                        StatEff.Remove(StatusEffectEn.BlueName);
                        Body = Body;
                        Hair = Hair;
                        XPKO = 0;
                        Equips.Send(MyClient, false);
                        World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e300")).Get);
                        CancelProtectTime = false;
                        ProtectTime = DateTime.Now.AddSeconds(0);
                        MyClient.LocalMessage(2001, "You have been revived by the Reviver Guard!");
                        return;
                    }
                }
                World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                /* List<Buff> BDelete = new List<Buff>();
                 foreach (Buff B in Buffs)
                     BDelete.Add(B);
                 foreach (Buff B in BDelete)
                     RemoveBuff(B); */
                foreach (Buff B in Buffs.Keys)
                    BDelete.TryAdd(B, B.Lasts);
                BlueName = false;
                StatEff.Add(StatusEffectEn.Dead);
                if (MyCompanion != null)
                    MyCompanion.Dissappear();

            }
            if (Attacker.MobID == 4152 && AOE)
                World.Action(this, Packets.StringPacket(EntityID, StringType.Effect, "change").Get);
        }
        public void TakeAttack(Companion Attacker, uint Damage, AttackType AT)
        {
            if (Level <= 6 && (Loc.Map == 1002 || Loc.Map == 1011 || Loc.Map == 1020 || Loc.Map == 1000 || Loc.Map == 1015 || Loc.Map == 1009))
                return;
            if (ProtectTime.AddMilliseconds(0) > DateTime.Now && !CancelProtectTime)
                return;
            if (World.NoPKMaps.Contains(Loc.Map))
                Damage = 0;
            if (Protection) Damage = 0;
            if (Damage != 0)
            {
                Damage = (uint)(Damage * 0.85);
                Extra.Durability.DefenceDurability(MyClient);
                if (EntityID != Attacker.Owner.EntityID)
                {
                    if (!BlueName && PKPoints < 100 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    {
                        Attacker.Owner.BlueName = true;
                        if (Attacker.Owner.BlueNameLasts < 15)
                            Attacker.Owner.BlueNameLasts = 15;
                    }
                }
                /* if (BuffOf(SkillsClass.ExtraEffect.Scapegoat).Eff == SkillsClass.ExtraEffect.Scapegoat && MyMath.ChanceSuccess(30))
                 {
                     Buff B = BuffOf(SkillsClass.ExtraEffect.Scapegoat);
                     BDelete.Add(B);
                     uint Dmg = (uint)(PrepareAttack(2, false) * B.Value);
                     Attacker.TakeAttack(this, ref Dmg, AttackType.Scapegoat, false);
                     return;//Will not be damaged
                 }*/
                if (AT == AttackType.Melee)
                {
                    ushort Def;
                    if (!Transformation.Transformed)
                        Def = EqStats.defense;
                    else Def = Transformation.Def;
                    if (Job % 10 >= 3 && !Transformation.Transformed)
                        Def = (ushort)(Def * 1.3);
                    Buff Shield = BuffOf(SkillsClass.ExtraEffect.MagicShield);
                    if (Shield.Eff == SkillsClass.ExtraEffect.MagicShield)
                        Def = (ushort)(Def * Shield.Value);


                    /* if (Def >= Damage)
                         Damage = 1;
                     else
                         Damage -= Def;*/
                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);
                    if (Def >= Damage)
                        Damage = 1;
                    else
                        Damage -= Def;
                    if (EqStats.MeleeDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MeleeDamageDecrease;
                }
                else if (AT == AttackType.Ranged)
                {
                    if (!Transformation.Transformed)
                        Damage = (uint)((double)Damage * (((double)(106 - EqStats.Dodge) / 100)));
                    else
                        Damage = (uint)((double)Damage * (((double)(106 - Transformation.Dodge) / 100)));
                    Damage *= 2 / 3;
                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);

                    if (EqStats.MeleeDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MeleeDamageDecrease;
                }
                else
                {


                    if (!Transformation.Transformed)
                    {
                        if (EqStats.MDef1 >= 106)
                            EqStats.MDef1 = 105;
                        Damage = (uint)((double)Damage * (((double)(106 - EqStats.MDef1) / 100)));
                    }
                    else Damage = (uint)((double)Damage * (((double)(106 - Transformation.MagicDef) / 100)));
                    Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                    Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);
                    if (EqStats.MagicDamageDecrease >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MagicDamageDecrease;
                    if (EqStats.MDef2 >= Damage)
                        Damage = 1;
                    else
                        Damage -= EqStats.MDef2;
                }
            }
            else
                Damage = 1;
            if (AT != AttackType.Magic && Action == 250)
            {
                if (Stamina > 30)
                    Stamina -= 30;
                else
                    Stamina = 0;
            }
            Action = 100;
            if (CanReflect)
            {
                if (MyMath.ChanceSuccess(10))
                {
                    if (Damage >= 2600)
                        Damage = 2600;

                    Attacker.GetReflect(Damage, AT);
                    World.Action(this, Packets.StringPacket(EntityID, StringType.Effect, "MagicReflect").Get);
                    Damage = 0;
                    return;
                }
            }
            if (Damage < CurHP)
            {
                CurHP = (ushort)(CurHP - Damage);
                if (AT == AttackType.Magic)
                    World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                InitAngry(false);
                Attacker.Owner.AtkMem.Attacking = false;
                Attacker.Owner.AtkMem.Target = 0;
                AtkMem.Attacking = false;
                AtkMem.Target = 0;
                DeathHit = DateTime.Now;
                Alive = false;
                CurHP = 0;
                if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                {
                    #region Attacker XP
                    if (Attacker.Level < 130)
                    {
                        if (Attacker.Level + 10 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 27) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 27) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 27) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 27) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 20 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 50) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 50) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 50) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 50) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 30 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 75) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 75) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 75) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 75) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 40 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 120) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 120) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 120) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 120) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 50 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 160) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 160) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 160) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 160) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 60 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 250) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 250) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 250) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 250) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else if (Attacker.Owner.Level + 70 > Level)
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 400) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 400) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 400) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 400) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        else
                        {
                            if (Reborns == 1 && (Job > 133 && Job < 135))
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 600) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 600) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                            else
                            {
                                if (Attacker.Owner.DoubleExp)
                                    Attacker.Owner.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 600) / Attacker.Owner.ExperienceRate) / 2), false, true);
                                else
                                    Attacker.Owner.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 600) / Attacker.Owner.ExperienceRate) / 2), false, true);
                            }
                        }
                        Experience = (ulong)(Experience / 1.125);
                    }
                    #endregion
                    if (Attacker.Owner.MyClient.GM)
                    {
                        Program.WriteCmds(Name + " got killed by " + Attacker.Owner.Name + " Map: " + Loc.Map + " at: " + DateTime.Now);
                    }
                    if (PKPoints >= 30 && !World.FreePKMaps.Contains(Loc.Map) && !MyClient.GM /*&& Loc.Map != 1038*/ && !World.EventsMaps.Contains(Loc.Map) /*&& Loc.Map != 1005 && Loc.Map != 6001*/)
                        LoseEquips();
                    if (!BlueName)
                    {
                        if (PoleWarTC.War && Loc.Map == 1002 || PoleWarPC.War && Loc.Map == 1011 || PoleWarAC.War && Loc.Map == 1020 || PoleWarDC.War && Loc.Map == 1000 || PoleWarBI.War && Loc.Map == 1015)
                        { }
                        else
                        {
                            Attacker.Owner.BlueNameLasts += 30;
                        if (PKPoints < 30)
                        {
                            if (Attacker.Owner.MyGuild != null && MyGuild != null)
                            {
                                if (Attacker.Owner.MyGuild.Enemies.ContainsKey(MyGuild.GuildID))
                                    Attacker.Owner.PKPoints += 3;
                                else if (Attacker.Owner.Enemies.ContainsKey(EntityID))
                                    Attacker.Owner.PKPoints += 5;
                                else
                                    Attacker.Owner.PKPoints += 10;
                            }
                            else if (Attacker.Owner.Enemies.ContainsKey(EntityID))
                                Attacker.Owner.PKPoints += 5;
                            else
                                Attacker.Owner.PKPoints += 10;

                            Attacker.Owner.PKPoints = Attacker.Owner.PKPoints > 30000 ? (ushort)30000 : Attacker.Owner.PKPoints;
                            //  Attacker.Owner.PKPoints = Math.Min((ushort)30000, Attacker.Owner.PKPoints);
                        }
                        }
                    }

                    if (!Enemies.ContainsKey(Attacker.Owner.EntityID) && Enemies.Count < 255)
                    {
                        Enemies.Add(Attacker.Owner.EntityID, new Enemy() { UID = Attacker.Owner.EntityID, Name = Attacker.Owner.Name });
                        MyClient.AddSend(Packets.FriendEnemyPacket(Attacker.Owner.EntityID, Attacker.Owner.Name, 19, 1));
                    }

                }

                if (AT != AttackType.Magic)
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                else World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);

                foreach (Buff B in Buffs.Keys)
                    BDelete.TryAdd(B, B.Lasts);

                BlueName = false;
                StatEff.Add(StatusEffectEn.Dead);
                if (MyCompanion != null)
                    MyCompanion.Dissappear();
                if (Attacker.Owner.ArenaQualifier != null && Attacker.Owner.ArenaQualifier.Status == Features.MatchStatus.Fighting)
                    Attacker.Owner.ArenaQualifier.RemovePlayer(this);

                if (EventBase != null)
                    if (EventBase.Stage == Events.EventStage.Fighting)
                        EventBase.Kill(Attacker, this);

                if (PKPoints >= 100 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                {
                    Teleport(6000, 28, 72);
                    World.SendMsgToAll("SYSTEM", Attacker.Owner.Name + " has captured " + Name + " and sent him to jail.", 2000, 0);
                }
            }
        }
        public void TakeAttack(uint Damage, SkillsClass.SkillUse S)
        {
            if (Damage < CurHP)
            {
                CurHP = (ushort)(CurHP - Damage);
                Game.World.Action(this, Packets.Traps(S, this).Get);
            }
            else
            {
                AtkMem.Attacking = false;
                AtkMem.Target = 0;
                DeathHit = DateTime.Now;
                if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    InitAngry(false);
                Alive = false;
                CurHP = 0;
                Game.World.Action(this, Packets.Traps(S, this).Get);

                foreach (Buff B in Buffs.Keys)
                    BDelete.TryAdd(B, B.Lasts);
                BlueName = false;
                StatEff.Add(StatusEffectEn.Dead);
                if (MyCompanion != null)
                    MyCompanion.Dissappear();
            }
        }
        public void TakeAttack(Mob Boss, uint Damage)
        {
            if (Damage < CurHP)
            {
                CurHP = (ushort)(CurHP - Damage);
                if (Boss != null)
                    World.Action(this, Packets.AttackPacket(Boss.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)2).Get);
                else
                    Game.World.Action(this, Packets.Traps(Damage, this).Get);
                //Game.World.Action(this, Packets.Traps(S, this).Get);
            }
            else
            {
                AtkMem.Attacking = false;
                AtkMem.Target = 0;
                DeathHit = DateTime.Now;
                if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    InitAngry(false);
                Alive = false;
                CurHP = 0;
                if (Boss != null)
                    World.Action(this, Packets.AttackPacket(Boss.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)14).Get);
                //Game.World.Action(this, Packets.Traps(S, this).Get);

                foreach (Buff B in Buffs.Keys)
                    BDelete.TryAdd(B, B.Lasts);
                BlueName = false;
                StatEff.Add(StatusEffectEn.Dead);
                if (MyCompanion != null)
                    MyCompanion.Dissappear();
            }
        }
        public void GetReflect(uint Damage, AttackType AT)
        {
            /* if (Damage > 4000)
                 Damage = 4000;*/
            if (Damage < CurHP)
            {
                CurHP -= (ushort)Damage;
                if (AT != AttackType.Magic)
                    World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                InitAngry(false);
                AtkMem.Attacking = false;
                AtkMem.Target = 0;
                Alive = false;
                CurHP = 0;
                DeathHit = DateTime.Now;
                if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                {
                    if (PKPoints >= 30)
                        LoseEquips();
                }

                if (AT != AttackType.Magic)
                    World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                foreach (Buff B in Buffs.Keys)
                    BDelete.TryAdd(B, B.Lasts);
                BlueName = false;
                StatEff.Add(StatusEffectEn.Dead);
                if (MyCompanion != null)
                    MyCompanion.Dissappear();
                if (ArenaQualifier != null && ArenaQualifier.Status == Features.MatchStatus.Fighting)
                    ArenaQualifier.RemovePlayer(this);

            }
        }
        public void TakeAttack(Character Attacker, ref uint Damage, AttackType AT, bool IsSkill, bool _poisonzap = false)
        {
            if (Invisible || Attacker.Invisible)
                return;
            if (Alive)
            {
                if (Loc.Map == 701)
                {
                    Attacker.Hits++;
                    Arena.DisplayScore();
                    return;
                }
                if (AT == AttackType.Melee && !IsSkill && !_poisonzap)
                    if (AT == AttackType.Melee || AT == AttackType.Ranged || AT == AttackType.FatalStrike || AT == AttackType.Magic && IsSkill)
                        if (Flying)
                            return;
                // if (BuffOf(SkillsClass.ExtraEffect.Fly).Eff == SkillsClass.ExtraEffect.Fly)
                if (Protection/* && Loc.Map != 1844 && Loc.Map != 1038 && Loc.Map != 1080 && Loc.Map != 1005 && Loc.Map != 700 && Loc.Map != 701 && Loc.Map != 1616*/)
                    return;
                if (Attacker.DH && Attacker.TransID != 0)
                    Damage /= 10;
                if (AT != AttackType.Magic /*&& !_poisonzap*/ && !IsSkill && Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                    Damage *= 3;
                if (World.NoPKMaps.Contains(Loc.Map))
                    return;
                if (Damage != 0)
                    Extra.Durability.DefenceDurability(MyClient);
                else
                    Damage = 1;
                if (EntityID != Attacker.EntityID)
                {
                    if (!BlueName && PKPoints < 100 && Loc.Map != 1039 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    {
                        Attacker.BlueName = true;
                        if (Attacker.BlueNameLasts < 15)
                            Attacker.BlueNameLasts = 15;
                    }
                }
                if (AT != AttackType.Magic && !IsSkill)
                {
                    ushort _Agi = (ushort)((Attacker.Agi + Attacker.EqStats.ExtraDex) * Attacker.EqStats.GemExtraDex);

                    Buff Accuracy = Attacker.BuffOf(SkillsClass.ExtraEffect.Accuracy);
                    if (Accuracy.Eff == SkillsClass.ExtraEffect.Accuracy)
                        _Agi = (ushort)(_Agi * Accuracy.Value);

                    double MissValue = Rnd.Next(_Agi - 15, _Agi + 35);
                    if (!Transformation.Transformed)
                    {
                        if (MissValue <= EqStats.Dodge || (AT == AttackType.Ranged && MyMath.ChanceSuccess(WarriorDodge)))
                            Damage = 0;
                    }
                    else
                    {
                        if (MissValue <= Transformation.Dodge || (AT == AttackType.Ranged && MyMath.ChanceSuccess(WarriorDodge)))
                            Damage = 0;
                    }
                }
                if (AT != AttackType.Magic && Action == 250)
                {
                    if (Stamina > 30)
                        Stamina -= 30;
                    else
                        Stamina = 0;
                }
                Action = 100;
                if (Attacker.Intensify.Active)
                {
                    if (Attacker.Intensify.Activated.AddMilliseconds(5300) < DateTime.Now)
                    {
                        if (Attacker.Intensify.X == Attacker.Loc.X && Attacker.Intensify.Y == Attacker.Loc.Y)
                        {
                            Attacker.Intensify.Active = false;
                            if (Attacker.Intensify.Level == 0)
                                Damage *= 2;
                            else if (Attacker.Intensify.Level == 1)
                                Damage = (uint)(Damage * 2.5);
                            else if (Attacker.Intensify.Level == 2)
                                Damage *= 3;
                            else Damage = (uint)(Damage * 3.5);
                        }
                        else
                            Attacker.Intensify.Active = false;
                    }
                    else Attacker.Intensify.Active = false;
                }
                if (Damage != 0 && !IsSkill && Loc.Map != 1080 && Loc.Map != 1017 && !(EventBase?.Stage == Events.EventStage.Fighting && EventBase.NoDamage))
                {
                    if (AT == AttackType.Melee)
                    {
                        ushort Def;
                        if (!Transformation.Transformed)
                            Def = EqStats.defense;
                        else Def = Transformation.Def;
                        //ushort Def = EqStats.defense;
                        Buff Shield = BuffOf(SkillsClass.ExtraEffect.MagicShield);
                        if (Shield.Eff == SkillsClass.ExtraEffect.MagicShield)
                            Def = (ushort)(Def * Shield.Value);

                        Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                        Damage = (uint)(Math.Floor((double)Damage * (100 - EqStats.TotalBless) / 100));

                        Damage += Attacker.EqStats.MeleeDamageIncrease;
                        if (Def >= Damage)
                            Damage = 1;
                        else
                            Damage -= Def;
                        if (EqStats.MeleeDamageDecrease >= Damage)
                            Damage = 1;
                        else
                            Damage -= EqStats.MeleeDamageDecrease;
                    }
                    else if (AT == AttackType.Ranged)
                    {
                        byte dodgev;
                        if (!Transformation.Transformed)
                            dodgev = EqStats.Dodge;
                        else dodgev = Transformation.Dodge;
                        if (dodgev > 95)
                            dodgev = 95;
                        Buff Dodge = BuffOf(SkillsClass.ExtraEffect.Dodge);
                        if (Dodge.Eff == SkillsClass.ExtraEffect.Dodge)
                            dodgev = (byte)(dodgev * Dodge.Value);
                        if (dodgev > 105)
                            dodgev = 105;
                        //Damage = (uint)((Damage * (((double)(200 - dodgev)) / 200)) / 13);
                        Damage = (uint)((Damage * (((double)(110 - dodgev)) / 110)) / 6);
                        //almost good   Damage = (uint)((Damage * (((double)(200 - EqStats.Dodge)) / 200)) / 13);
                        //Damage = (uint)((Damage * (((double)(304 - EqStats.Dodge)) / 300)) / 12);
                        /*  Damage = (uint)((double)Damage * ((double)(100 - EqStats.Dodge ) / 200));
                          if (Damage > EqStats.defense / 6)
                              Damage -= (uint)(EqStats.defense / 4);
                          else
                              Damage = 1;*/
                        Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                        Damage = (uint)(Math.Floor((double)Damage * (100 - EqStats.TotalBless) / 100));

                        Damage += Attacker.EqStats.MeleeDamageIncrease;

                        if (EqStats.MeleeDamageDecrease >= Damage)
                            Damage = 1;
                        else
                            Damage -= EqStats.MeleeDamageDecrease;

                        if (Damage > 1500) Damage = 1500;
                    }
                    else
                    {
                        ushort MDPC = EqStats.MDef1;
                        if (MDPC < 20)
                            MDPC = 20;
                        if (!Transformation.Transformed)
                            if (MDPC >= 110)
                                Damage = (uint)((double)Damage * (((double)1 / 100)));
                            else Damage = (uint)((double)Damage * (((double)(110 - MDPC) / 100)));
                        else Damage = (uint)((double)Damage * (((double)(110 - Transformation.MagicDef) / 100)));
                        if (EqStats.MDef2 >= Damage)
                            Damage = 1;
                        else
                            Damage -= EqStats.MDef2;
                        if (EqStats.MagicDamageDecrease >= Damage)
                            Damage = 1;
                        else
                            Damage -= EqStats.MagicDamageDecrease;
                        Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
                        Damage = (uint)(Math.Floor((double)Damage * (100 - EqStats.TotalBless) / 100));

                        Damage += Attacker.EqStats.MagicDamageIncrease;

                    }
                }

                if (CanReflect)
                {
                    if (MyMath.ChanceSuccess(10))
                    {
                        if (EventBase == null || (EventBase != null && EventBase.MapEvent != Loc.Map) || (EventBase.Reflect && EventBase.MapEvent == Loc.Map))
                        {
                            if (Arena == null || (Arena != null && Arena.MapID != Loc.Map))
                            {
                                if (Damage >= 2600)
                                    Damage = 2600;

                                Attacker.GetReflect(Damage, AT);
                                World.Action(this, Packets.StringPacket(EntityID, StringType.Effect, "MagicReflect").Get);
                                Damage = 0;
                                return;
                            }
                        }
                    }
                }
                if (Damage < CurHP)
                {
                    CurHP -= (ushort)Damage;
                    if (AT != AttackType.Magic && !IsSkill)
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                    if (Attacker?.EventBase != null)
                        if (Attacker.EventBase.Stage == Events.EventStage.Fighting)
                            Attacker?.EventBase?.Hit(Attacker, this);
                    if (Attacker?.Arena != null && Attacker.Arena.MapID == Attacker.Loc.Map)
                        Attacker?.Arena?.Hit(Attacker, this);
                }
                else
                {
                    InitAngry(false);
                    Attacker.AtkMem.Attacking = false;
                    Attacker.AtkMem.Target = 0;
                    AtkMem.Attacking = false;
                    AtkMem.Target = 0;
                    DeathHit = DateTime.Now;
                    Alive = false;
                    CurHP = 0;
                    if (Loc.Map == 1005)
                    {
                        Deaths++;
                        MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", " Kills : " + Kills + " Death : " + Deaths + "", 0x83c, 0));
                    }
                    if (StatEff.Contains(StatusEffectEn.Poisoned))
                    {
                        StatEff.Remove(StatusEffectEn.Poisoned);
                        PoisonedInfo.Poisoned = false;
                    }
                    if (Loc.Map == 1005)
                    {
                        Attacker.Kills += 1;
                        Attacker.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", " Kills : " + Attacker.Kills + " Death : " + Attacker.Deaths + "", 0x83c, 0));
                    }
                    if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    {
                        if (World.Bounty.ContainsKey(Name))
                        {
                            if (Attacker.OnBounty == Name)
                            {
                                if (!Attacker.KilledBounty)
                                {
                                    Attacker.KilledBounty = true;
                                    Attacker.MyClient.LocalMessage(2000, "Congratulations! You have killed a player that has a bounty on his/her head! Visit Sunfer NPC at TwinCity and claim your reward");
                                    World.SendMsgToAll("[SYSTEM]", "Ouch! " + Attacker.Name + " have killed a player that has a bounty on his/her head!", 2005, 0);
                                    //Features.Bounty.Delevel(this);
                                }
                                else
                                    Attacker.MyClient.LocalMessage(2000, "Please collect the bounty from the player you have killed!");
                            }
                        }
                        #region Attacker XP
                        uint Money = (uint)(Experience - Experience / 1.125) / 10;
                        if (Money == 0)
                            Money = 10000;
                        if (Attacker.Level < 130)
                        {
                            if ((MyGuild != null && GuildDonation < Money) || MyGuild == null)
                            {
                                if (Attacker.Level + 10 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 27) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 27) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 27) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 27) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 20 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 50) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 50) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 50) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 50) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 30 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 75) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 75) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 75) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 75) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 40 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 120) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 120) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 120) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 120) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 50 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 160) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 160) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 160) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 160) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 60 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 250) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 250) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 250) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 250) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else if (Attacker.Level + 70 > Level)
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 400) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 400) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 400) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 400) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }
                                else
                                {
                                    if (Reborns == 1 && (Job > 133 && Job < 135))
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)((((((Experience / 2) / 2) / EqStats.GemExtraExp) / 600) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 600) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                    else
                                    {
                                        if (Attacker.DoubleExp)
                                            Attacker.IncreaseExp((uint)(((((Experience / 2) / EqStats.GemExtraExp) / 600) / Attacker.ExperienceRate) / 2), false, true);
                                        else
                                            Attacker.IncreaseExp((uint)((((Experience / EqStats.GemExtraExp) / 600) / Attacker.ExperienceRate) / 2), false, true);
                                    }
                                }

                                Experience = (ulong)(Experience / 1.125);
                            }
                        }

                        if (MyGuild != null)
                        {
                            if (Level == 130)
                                Money = 13 * Money;
                            if (GuildRank == GuildRank.DeputyManager)
                                Money = Money * 2;
                            else if (GuildRank == GuildRank.GuildLeader)
                                Money = Money * 5;

                            if (GuildDonation > Money)
                                GuildDonation -= Money;
                            else
                                GuildDonation = 0;

                            if (MyGuild.Fund > Money / 2)
                            {
                                MyGuild.Fund -= Money / 2;
                                if (Attacker.MyGuild != null)
                                {
                                    Attacker.MyGuild.Fund += Money / 2;
                                    Attacker.GuildDonation += Money / 2;
                                }
                            }
                            else
                                MyGuild.Fund = 0;
                        }

                        #endregion
                        if (Attacker.MyClient.GM)
                        {
                            Program.WriteCmds(Name + " got killed by " + Attacker.Name + " Map: " + Loc.Map + " at: " + DateTime.Now);
                        }
                        if (PKPoints >= 30 && !MyClient.GM /*&& Loc.Map != 1038 && Loc.Map != 1005 && Loc.Map != 6001 && Loc.Map != 8001*/)
                            LoseEquips();
                        if (!BlueName)
                        {
                            if (PoleWarTC.War && Loc.Map == 1002 || PoleWarPC.War && Loc.Map == 1011 || PoleWarAC.War && Loc.Map == 1020 || PoleWarDC.War && Loc.Map == 1000 || PoleWarBI.War && Loc.Map == 1015)
                            { }
                            else
                            {
                                Attacker.BlueNameLasts += 30;
                            if (PKPoints < 30)
                            {
                                if (Attacker.MyGuild != null && MyGuild != null)
                                {
                                    if (Attacker.MyGuild.Enemies.ContainsKey(MyGuild.GuildID))
                                    {
                                        if (Attacker.PKPoints < 29998)
                                            Attacker.PKPoints += 3;
                                        else Attacker.PKPoints = 30000;

                                    }
                                    else if (Attacker.Enemies.ContainsKey(EntityID))
                                        if (Attacker.PKPoints < 29996)
                                            Attacker.PKPoints += 5;
                                        else Attacker.PKPoints = 30000;
                                    else
                                        if (Attacker.PKPoints < 29991)
                                        Attacker.PKPoints += 10;
                                    else
                                        Attacker.PKPoints = 30000;
                                }
                                else if (Attacker.Enemies.ContainsKey(EntityID))
                                    if (Attacker.PKPoints < 29996)
                                        Attacker.PKPoints += 5;
                                    else Attacker.PKPoints = 30000;
                                else
                                    if (Attacker.PKPoints < 29991)
                                    Attacker.PKPoints += 10;
                                else
                                    Attacker.PKPoints = 30000;

                                }
                            }
                        }

                        if (!Enemies.ContainsKey(Attacker.EntityID) && Enemies.Count < 255)
                        {
                            Enemies.Add(Attacker.EntityID, new Enemy() { UID = Attacker.EntityID, Name = Attacker.Name });
                            MyClient.AddSend(Packets.FriendEnemyPacket(Attacker.EntityID, Attacker.Name, 19, 1));
                        }

                    }

                    if (AT != AttackType.Magic && !IsSkill)
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);

                    foreach (Buff B in Buffs.Keys)
                        BDelete.TryAdd(B, B.Lasts);

                    BlueName = false;
                    StatEff.Add(StatusEffectEn.Dead);
                    if (MyCompanion != null)
                        MyCompanion.Dissappear();


                    if (PKPoints >= 100 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    {
                        Teleport(6000, 28, 72);
                        World.SendMsgToAll("SYSTEM", Attacker.Name + " has captured " + Name + " and sent him to jail.", 2000, 0);
                    }
                    if (Attacker?.EventBase != null)
                        if (Attacker.EventBase.Stage == Events.EventStage.Fighting)
                            Attacker?.EventBase?.Kill(Attacker, this);
                    if (Attacker?.ArenaQualifier != null /*&& Attacker?.Loc.Map == Attacker?.ArenaQualifier.MapID */&& Attacker?.ArenaQualifier.Status == Features.MatchStatus.Fighting)
                        Attacker?.ArenaQualifier.RemovePlayer(this);
                }

            }
            else
                Damage = 0;
        }
        //public void TakeAttack(AI Attacker, uint Damage, AttackType AT, bool AOE = false, bool IsSkill = false)
        //{
        //    if (ProtectTime.AddMilliseconds(0) > DateTime.Now && !CancelProtectTime)
        //        return;
        //    if (Protection) Damage = 0;
        //    if (Damage != 0)
        //    {
        //        Extra.Durability.DefenceDurability(MyClient);

        //        if (AT == AttackType.Melee)
        //        {
        //            ushort Def;
        //            if (!Transformation.Transformed)
        //                Def = EqStats.defense;
        //            else Def = Transformation.Def;
        //            if (Job % 10 >= 3 && !Transformation.Transformed)
        //                Def = (ushort)(Def * 1.3);
        //            Buff Shield = BuffOf(SkillsClass.ExtraEffect.MagicShield);
        //            if (Shield.Eff == SkillsClass.ExtraEffect.MagicShield)
        //                if (Shield.Value == 2)
        //                    Def = (ushort)(Def * 3);
        //                else Def = (ushort)(Def * Shield.Value);

        //            Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
        //            Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);
        //            if (Def >= Damage)
        //                Damage = 1;
        //            else
        //                Damage -= Def;
        //            if (EqStats.MeleeDamageDecrease >= Damage)
        //                Damage = 1;
        //            else
        //                Damage -= EqStats.MeleeDamageDecrease;
        //        }
        //        else if (AT == AttackType.Ranged)
        //        {
        //            if (!Transformation.Transformed)
        //                Damage = (uint)((double)Damage * (((double)(106 - EqStats.Dodge) / 100)));
        //            else
        //                Damage = (uint)((double)Damage * (((double)(106 - Transformation.Dodge) / 100)));
        //            Damage *= 2 / 3;
        //            Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
        //            Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);

        //            if (EqStats.MeleeDamageDecrease >= Damage)
        //                Damage = 1;
        //            else
        //                Damage -= EqStats.MeleeDamageDecrease;
        //        }
        //        else
        //        {
        //            if (EqStats.MagicDamageDecrease >= Damage)
        //                Damage = 1;
        //            else
        //                Damage -= EqStats.MagicDamageDecrease;

        //            if (!Transformation.Transformed)
        //                if (EqStats.MDef1 < 106)
        //                {
        //                    Damage = (uint)((double)Damage * (((double)(106 - EqStats.MDef1) / 100)));
        //                }
        //                else if (Transformation.MagicDef < 106) { Damage = (uint)((double)Damage * (((double)(106 - Transformation.MagicDef) / 100))); }

        //            Damage = (uint)(Math.Floor((double)Damage * (1 - ((EqStats.GemBless < .52) ? EqStats.GemBless : .52))));
        //            Damage = (uint)((double)Damage * (100 - EqStats.TotalBless) / 100);

        //            if (EqStats.MDef2 >= Damage)
        //                Damage = 1;
        //            else
        //                Damage -= EqStats.MDef2;
        //        }
        //    }
        //    else
        //        Damage = 1;

        //    if (AT != AttackType.Magic && Action == 250)
        //    {
        //        if (Stamina > 30)
        //            Stamina -= 30;
        //        else
        //            Stamina = 0;
        //    }
        //    Action = 100;
        //    if (CanReflect)
        //    {
        //        if (MyMath.ChanceSuccess(5))
        //        {
        //            if (Damage >= 2600)
        //                Damage = 2600;

        //            Attacker.GetReflect(ref Damage, AT);
        //            World.Action(this, Packets.StringPacket(EntityID, 10, "MagicReflect").Get);
        //            Damage = 0;
        //            return;
        //        }
        //    }
        //    if (Damage < CurHP)
        //    {
        //        CurHP = (ushort)(CurHP - Damage);
        //        if (AT == AttackType.Magic || IsSkill)
        //        {
        //            if (Attacker.attackSkill == 0)
        //                World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Melee).Get);
        //            else
        //                World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.attackSkill, Attacker.Skills[Attacker.attackSkill].Lvl, Loc.X, Loc.Y).Get);
        //        }
        //        else
        //            World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
        //    }
        //    else
        //    {
        //        AtkMem.Attacking = false;
        //        AtkMem.Target = 0;
        //        DeathHit = DateTime.Now;
        //        if (!World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
        //            InitAngry(false);
        //        Alive = false;
        //        CurHP = 0;

        //        if (AT == AttackType.Magic || IsSkill)
        //        {
        //            if (Attacker.attackSkill == 0)
        //                World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Melee).Get);
        //            else
        //                World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.attackSkill, Attacker.Skills[Attacker.attackSkill].Lvl, Loc.X, Loc.Y).Get);
        //        }
        //        else
        //            World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);


        //        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
        //        foreach (Buff B in Buffs.Keys)
        //            BDelete.TryAdd(B, B.Lasts);
        //        BlueName = false;
        //        StatEff.Add(StatusEffectEn.Dead);
        //        if (MyCompanion != null)
        //            MyCompanion.Dissappear();

        //    }
        //}

        public void TryEquipArrows()
        {
            if (Equips.LeftHand.CurDur > Equips.LeftHand.MaxDur)
                Equips.LeftHand.CurDur = Equips.LeftHand.MaxDur;

            if (Equips.LeftHand.CurDur >= 1 && Loc.Map == 1039)
                Equips.LeftHand.CurDur -= 0;
            else if (Equips.LeftHand.CurDur >= 1)
                Equips.LeftHand.CurDur -= 1;

            else
                Equips.LeftHand.CurDur = 0;

            if (Equips.LeftHand.CurDur == 0)
            {
                if (InventoryContains(1050000, 1))
                {
                    Equips.LeftHand = NextItem(1050000);
                    RemoveItem(NextItem(1050000));
                }
                else if (InventoryContains(1050001, 1) && Level >= 32)
                {
                    Equips.LeftHand = NextItem(1050001);
                    RemoveItem(NextItem(1050001));
                }
                else if (InventoryContains(1050002, 1) && Level >= 73)
                {
                    Equips.LeftHand = NextItem(1050002);
                    RemoveItem(NextItem(1050002));
                }
                else if (InventoryContains(1051000, 1) && Level >= 1)
                {
                    Equips.LeftHand = NextItem(1051000);
                    RemoveItem(NextItem(1051000));
                }
                else
                    MyClient.LocalMessage(2005, "You ran out of arrows!");
            }

            if (Equips.LeftHand.CurDur == 0)
            {
                MyClient.AddSend(Packets.ItemPacket(Equips.LeftHand.UID, 5, 6));
                MyClient.AddSend(Packets.ItemPacket(Equips.LeftHand.UID, 0, 3));
                Equips.LeftHand = new Game.Item();
            }
            else
                MyClient.AddSend(Packets.AddItem(Equips.LeftHand, 5));
        }

        public uint PrepareAttack(byte AtkType, bool ArrowCost, uint InfoDamage = 0)
        {
            AtkMem.LastAttack = DateTime.Now;
            AttackType A = (AttackType)AtkType;

            bool EnoughArrows = true;
            if (A == AttackType.Ranged && ArrowCost)
            {
                if (Loc.Map != 1039 || Loc.Map != 1616 || Loc.Map != 2068)
                {
                    if (Equips.LeftHand.ID != 0 && Item.IsArrow(Equips.LeftHand.ID))
                        TryEquipArrows();
                    else
                    {
                        AtkMem.Attacking = false;
                        EnoughArrows = false;
                    }
                }
            }


            if (EnoughArrows)
            {
                /*if (A == AttackType.Melee || A == AttackType.Ranged)
                {
                    uint Damage, BaseDamage;
                    if (EqStats.minatk > EqStats.maxatk)
                        BaseDamage = EqStats.minatk;
                    else
                        BaseDamage = (uint)Rnd.Next((int)EqStats.minatk, (int)EqStats.maxatk);

                    BaseDamage += Str;
                    Damage = BaseDamage;
                    Damage += (uint)(BaseDamage * (EqStats.GemExtraAttack - 1));
                    Damage += (uint)(BaseDamage * (EqStats.WeaponExtraAttack - 1));
                    Buff Stig = BuffOf(SkillsClass.ExtraEffect.Stigma);
                    if (Stig.Eff == SkillsClass.ExtraEffect.Stigma)
                        Damage += (uint)(Damage * (Stig.Value - 1));

                    return Damage;

                }*/
                if (A == AttackType.Melee || A == AttackType.Ranged)
                {

                    uint Damage;
                    if (Transformation.Transformed)
                        Damage = (uint)Rnd.Next(Transformation.MinDmg, Transformation.MaxDmg);

                    else
                    {
                        if (EqStats.minatk > EqStats.maxatk)
                            Damage = EqStats.minatk;
                        else
                            Damage = (uint)Rnd.Next((int)EqStats.minatk, (int)EqStats.maxatk);
                        Damage = (uint)((Damage + Str) * EqStats.GemExtraAttack);
                        Damage = (uint)(Damage * EqStats.WeaponExtraAttack);
                        Buff Stig = BuffOf(SkillsClass.ExtraEffect.Stigma);
                        if (Stig.Eff == SkillsClass.ExtraEffect.Stigma)
                            Damage = (uint)(Damage * Stig.Value);
                    }
                    if ((Equips.RightHand.Effect == Item.RebornEffect.Shield) && MyMath.ChanceSuccess(10) && StigBow < DateTime.Now)
                    {
                        World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "attackup40")).Get);
                        StigBow = DateTime.Now.AddSeconds(30);
                    }
                    if (StigBow > DateTime.Now)
                        Damage = (uint)(Damage * 1.2);


                    return Damage;
                }
                else
                {
                    uint Damage;
                    if (Job % 10 < 4)
                        Damage = (uint)((EqStats.matk + Spi) * EqStats.GemExtraMAttack);
                    else
                        Damage = (uint)((EqStats.matk + InfoDamage - EqStats.MDamage) * EqStats.GemExtraMAttack);
                    //Damage = (uint)((EqStats.matk + (Spi * 2)) * EqStats.GemExtraMAttack);
                    return Damage;
                }


            }
            return 0;
        }
        public uint ExpBallExp
        {
            get
            {
                if (Level < 25)
                    return (uint)(150000 + Level * 22000);
                else if (Level < 70)
                    return (uint)(150000 + Level * 25000);
                else if (Level < 80)
                    return (uint)(300000 + Level * 50000);
                else if (Level < 80)
                    return (uint)(300000 + Level * 60000);
                else if (Level < 100)
                    return (uint)(300000 + Level * 70000);
                else if (Level < 110)
                    return (uint)(300000 + Level * 90000);
                else if (Level < 120)
                    return (uint)(300000 + Level * 110000);
                else if (Level < 125)
                    return (uint)(300000 + Level * 150000);
                else if (Level < 130)
                    return (uint)(300000 + Level * 170000);
                else
                    return (uint)(37748736 + Level * 225000 + (7 - (137 - Level)) * 75497472);
            }
        }
        public void EquipStats(byte Pos, bool Equip, bool Force)
        {
            try
            {
                Item I = Equips.Get(Pos);
                if (I.ID != 0)
                {
                    DatabaseItem D1 = I.DBInfo;

                    ItemIDManipulation IMan = new ItemIDManipulation(I.ID);
                    uint ComposeID = IMan.ToComposeID(Pos);
                    DatabasePlusItem D2;
                    if (Database.DatabasePlusItems.ContainsKey(ComposeID.ToString() + I.Plus.ToString()))
                        D2 = (DatabasePlusItem)Database.DatabasePlusItems[ComposeID.ToString() + I.Plus.ToString()];
                    else
                        D2 = new DatabasePlusItem();

                    EquipStats E = new EquipStats();
                    if (Pos != 12)
                        E.Dodge = (byte)(D1.Dodge + D2.Dodge);
                    else
                        E.AddRideSpeed = D2.Dodge;

                    if (Pos != 12)
                        E.ExtraDex = (ushort)(D1.DexGives + D2.Dex);
                    /*  else
                          E.AddVigor = D2.Dex;*/
                    if (Pos == 12)
                        E.MaxHP += 100;
                    byte DodgeToWarrior = 0;
                    if (Pos == 5)
                    {
                        D1.MinAtk /= 2;
                        D1.MaxAtk /= 2;
                        if (Game.ItemIDManipulation.Part(D1.ID, 0, 3) == 900)
                        {
                            DodgeToWarrior = (byte)(D1.LevReq / 3);
                        }
                    }
                    if (Pos != 10 && Pos != 11)
                    {
                        E.defense = (ushort)(D1.Defense + D2.Defense);
                        E.matk = (uint)(D1.MagicAttack + D2.MAtk);
                        E.MDamage = (uint)D2.MAtk;
                        E.minatk = D2.MinAtk + D1.MinAtk;
                        E.maxatk = D2.MaxAtk + D1.MaxAtk;
                        E.MDef1 = (ushort)D1.MagicDefense;
                        E.MDef2 = D2.MDef;
                    }
                    else
                    {
                        E.MagicDamageIncrease = (uint)(D1.MagicAttack + D2.MAtk);
                        E.MagicDamageDecrease = (uint)(D1.MagicDefense + D2.MDef);
                        E.MeleeDamageIncrease = (uint)(D2.MinAtk + D1.MinAtk);
                        E.MeleeDamageDecrease = (uint)(D1.Defense + D2.Defense);
                    }
                    if (Profs.ContainsKey((ushort)ItemIDManipulation.Part(I.ID, 0, 3)))
                    {
                        Prof P = Profs[(ushort)ItemIDManipulation.Part(I.ID, 0, 3)];
                        Item.GetWeaponEffect(ref E, P);
                    }
                    Item.GetGemEffect(ref E, I.Soc1);
                    Item.GetGemEffect(ref E, I.Soc2);
                    if (Equips.Tower.ID != I.ID && Equips.Fan.ID != I.ID)
                        E.MaxHP += I.Enchant;
                    E.MaxHP += D2.HP;
                    E.eq_pot += I.Pot;
                    E.TotalBless += I.Bless;
                    /*   if (Pos == 1)
                       {
                           Console.WriteLine(I.DBInfo.Name);
                           Console.WriteLine("Def: " + D1.Defense);
                           Console.WriteLine("Def+: " + D2.Defense);
                           Console.WriteLine("Matk: " + (uint)(D1.MagicAttack + D2.MAtk));
                           Console.WriteLine("Minatk: " + (D2.MinAtk + D1.MinAtk));
                           Console.WriteLine("Maxatk: " + ( D2.MaxAtk + D1.MaxAtk));
                           Console.WriteLine("Mdef: " + (ushort)D1.MagicDefense);
                           Console.WriteLine("Mdef+: " + D2.MDef);
                           Console.WriteLine();
                       }*/
                    /*     if (I.ID == 137310 || I.ID == 137410 || I.ID == 137410 || I.ID == 137610 || I.ID == 137710 || I.ID == 137810 || I.ID == 137910)
                         {
                             E.MaxHP += 30000;
                             E.defense += 30000;
                             E.MDef1 += 30000;
                         } */










                    if (I.ID == 150000) // Some love forever ring lvl 20
                    {
                        E.MaxHP += 800;
                    }           
                    if (I.ID == 2100075) // Gold Prize 
                    {
                        E.MaxHP += 1500;
                        E.MaxMP += 1500;
                        E.defense += 1000;
                        E.maxatk += 1000;

                    }
                    if (I.ID == 2100065)  //SilverPrize 2100065
                    {
                        E.MaxHP += 1200;
                        E.MaxMP += 1200;
                    }
                    if (I.ID == 2100055)  //BronzePrize 2100055
                    {
                        E.MaxHP += 900;
                        E.MaxMP += 900;
                    }
                    if (I.ID == 2100025)  //MiraculousGourd
                    {
                        E.MaxHP += 800;
                        E.MaxMP += 800;
                    }
                        if (Equip && I.CurDur > I.MaxDur / 100)
                    {
                        EqStats += E;
                        if (DodgeToWarrior > 0)
                            WarriorDodge = DodgeToWarrior;
                    }
                    else if (I.CurDur <= I.MaxDur / 100 && I.CurDur > 0 && Equip)
                    {
                        MyClient.LocalMessage(2000, "Your " + I.DBInfo.Name + " durability is very low. Please repair it before reaching 0.");
                        EqStats += E;
                        if (DodgeToWarrior > 0)
                            WarriorDodge = DodgeToWarrior;
                    }
                    else if (I.CurDur == 0 && Equip)
                    {
                        MyClient.LocalMessage(2000, "Your " + I.DBInfo.Name + " is broken. Go and repair it otherwise the stats of the items will not count.");
                    }
                    else if ((I.CurDur > 0 || Force) && !Equip)
                    {
                        EqStats -= E;
                        if (DodgeToWarrior > 0)
                            WarriorDodge -= DodgeToWarrior;
                    }
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public void LoseEquips()
        {
            try
            {
                if (MyClient.AuthInfo.Status != "[GM]" && MyClient.AuthInfo.Status != "[PM]" && MyClient.Soc.Connected)
                {
                    int DroppedEqs = 0;
                    for (byte i = 0; i < 12; i++)
                    {
                        if (DroppedEqs < 3)
                        {
                            Item I = new Item();
                            if (i == 4 && Equips.Get(5).ID != 0)
                            {
                                I = Equips.Get(5);
                                i = 5;
                            }
                            else I = Equips.Get(i);
                            if (I.ID != 0 && !I.FreeItem)
                            {
                                byte ch = 0;
                                if (PKPoints >= 100)
                                    ch = 40;
                                if (MyMath.ChanceSuccess(30 + ch))
                                {
                                    bool _larger = false;
                                    EquipStats(i, false, false);
                                    DroppedItem D = new DroppedItem();
                                    D.Info = I;
                                    D.UID = (uint)(Rnd.Next(10000000));
                                    D.Loc = new Location();
                                    D.Loc.Map = Loc.Map;
                                    D.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                                    D.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                                    if (!World.H_Items.ContainsKey(Loc.Map))
                                        World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());
                                    if (!D.FindPlace(World.H_Items[Loc.Map]))
                                    {
                                        D.Loc.X = (ushort)(Loc.X + Rnd.Next(6) - Rnd.Next(6));
                                        D.Loc.Y = (ushort)(Loc.Y + Rnd.Next(6) - Rnd.Next(6));
                                        _larger = true;
                                    }
                                    if (_larger)
                                        if (!D.FindPlace(World.H_Items[Loc.Map])) return;

                                    Equips.Replace(i, new Item(), this);
                                    D.DropTime = DateTime.Now;
                                    DroppedEqs++;
                                    if (i == 5)
                                        i = 3;
                                    if (I.ID == 1050000 || I.ID == 1050001 || I.ID == 1050002 || I.ID == 1051000)
                                    {
                                        DroppedEqs--;
                                    }

                                    D.Drop();
                                    World.DropAdd += "PK DROP: " + Name + " has dropped " + D.Info.ID + "~" + D.Info.Plus + "~" + D.Info.Bless + "~" + D.Info.Enchant + "~" + (byte)D.Info.Soc1 + "~" + (byte)D.Info.Soc2 + "~" + D.Info.Progress + "\r\n";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public void SetTank()
        {
            bool TankRemoved = false;
            if (Loc.Map != 1039 && Loc.Map != 1036 && Loc.Map != 1090 && Loc.Map != 1004)
                foreach (Character C in ScreenChars.Values)//World.H_Chars.Values
                {
                    if (/*C.Loc.Map == Loc.Map && */C.Loc.X == Loc.X && C.Loc.Y == Loc.Y)
                    {
                        if (C.Tank)
                        {
                            C.PrevTank = true;
                            C.Tank = false;
                        }
                        else C.PrevTank = false;
                        TankRemoved = true;
                    }
                }




            if (TankRemoved)
                Tank = true;
            CheckTank = false;
        }

        private void AutoHuntStep()
        {
            if (!AutoHuntEnabled || !Alive)
                return;
            if (DateTime.Now < nextAutoHunt)
                return;
            nextAutoHunt = DateTime.Now.AddSeconds(1);

            if (!World.H_Mobs.ContainsKey(Loc.Map))
                return;

            Game.Mob target = null;
            int dist = int.MaxValue;
            foreach (var m in World.H_Mobs[Loc.Map].Values)
            {
                if (!m.Alive)
                    continue;
                int d = MyMath.PointDistance(Loc.X, Loc.Y, m.Loc.X, m.Loc.Y);
                if (d < dist)
                {
                    dist = d;
                    target = m;
                }
            }
            if (target == null)
                return;

            if (dist > 3)
            {
                ushort nx = (ushort)((Loc.X + target.Loc.X) / 2);
                ushort ny = (ushort)((Loc.Y + target.Loc.Y) / 2);
                if (AbleToJump(nx, ny, false, false))
                {
                    World.Action(this, Packets.GeneralData(EntityID, 0, nx, ny, 86).Get);
                    Jump(nx, ny);
                }
            }
            uint damage = PrepareAttack((byte)AttackType.Melee, true);
            World.Action(this, Packets.AttackPacket(EntityID, target.EntityID, target.Loc.X, target.Loc.Y, damage, (byte)AttackType.Melee).Get);
            target.TakeAttack(this, ref damage, AttackType.Melee, false);
        }
        public void Step()
        {
            try
            {
                DateTime TimeNow = DateTime.Now;
                if (DailyQuestDate.Date != DateTime.Today)
                {
                    DailyQuestDate = DateTime.Today;
                    DailyQuestActive = false;
                    DailyQuestCompleted = false;
                    DailyQuestKills = 0;
                }

                AutoHuntStep();

                if (RemoveAfter && DateTime.Now > RemoveStamp)
                {
                    MyClient.AddSend(Packets.ItemPacket(0, 9, 6));
                    MyClient.LocalMessage(2005, "Your garment test is over.");
                    RemoveAfter = false;
                }
                if (RemoveAfter1 && DateTime.Now > RemoveStamp1)
                {
                    MyClient.AddSend(Packets.ItemPacket(0, 4, 6));
                    MyClient.LocalMessage(2005, "Your accessory test is over.");
                    RemoveAfter1 = false;
                }

                //if (DateTime.Now >= _anticheat.AddSeconds(30) && !MyClient.GM)
                //{
                //    World.DebugAdd += Name + " has been disconnected by the anticheat.\r\n";
                //    MyClient.Disconnect();
                //    if (MyClient.Soc.Connected)
                //        MyClient.Soc.Disconnect(false);
                //}
                #region Captcha
                if (TimeNow < MyClient.LastAttack.AddSeconds(5) && MyClient != null)
                {
                    if (MyClient.MobsKilled > 13000 && (DateTime.Now > MyClient.LastSuccessCaptcha.AddMinutes(MyClient.NextCaptcha)))
                    {
                        if (TimeNow > MyClient.KillCountCaptchaStamp.AddSeconds(20))
                        {
                            if (!MyClient.WaitingKillCaptcha)
                            {
                                MyClient.KillCountCaptchaStamp = DateTime.Now;
                                MyClient.WaitingKillCaptcha = true;
                                MyClient.DialogNPC = 9999997;
                                MyClient.KillCountCaptcha = Program.Rnd.Next(10000, 50000).ToString();
                                MyClient.AddSend(Packets.NPCSay("Input the current text: " + MyClient.KillCountCaptcha + " to verify your humanity."));
                                MyClient.AddSend(Packets.NPCLink2("Captcha message:", (byte)MyClient.KillCountCaptcha.Length));
                                MyClient.AddSend(Packets.NPCLink("Just passing by", 255));
                                MyClient.AddSend(Packets.NPCSetFace(30));
                                MyClient.AddSend(Packets.NPCFinish());
                            }
                            else
                                MyClient.Disconnect();
                        }
                    }
                }
                #endregion
                if (DateTime.Now >= LoginTime.AddHours(1))
                {
                    if(ClassicPoints <= 250)
                    { 
                    LoginTime = DateTime.Now;
                    ClassicPoints++;
                    OnlineTime += 1;
                    MyClient.LocalMessage(2011, "You have earned 1 Online Point for being online! You can use your online points to exchange them for rewards.");
                    }
                    else
                    {
                        LoginTime = DateTime.Now;
                        MyClient.LocalMessage(2011, "Your online score has reached its maximum. Please go OnlinePoints NPC from market and exchange your points.");
                    }
                }
                if (EventBase != null)
                    if (EventBase.Stage == Events.EventStage.Fighting)
                        EventBase.CharacterChecks(this);
                //if (viplevel > 0 && (EventBase == null || EventBase != null && EventBase.MapEvent != Loc.Map || Loc.Map != 1616))
                //{
                //    if (VIP >= DateTime.Now && DateTime.Now >= LastVIPMessage.AddMilliseconds(1000))
                //    {
                //        MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", "VIP Time Remaining:", 0x83c, 0));
                //        var toDisplay = VIP.Subtract(DateTime.Now);
                //        MyClient.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", $"Days: { toDisplay.Days }", 0x83d, 0));
                //        MyClient.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", $"Time: { toDisplay.ToString(@"hh\:mm\:ss")}", 0x83d, 0));
                //        LastVIPMessage = DateTime.Now;
                //    }
                //}
                if (ToUpdate && DateTime.Now >= LastMove.AddMilliseconds(MyMath.PointDistance(Loc.X, Loc.Y, Loc.PreviousX, Loc.PreviousY) * 126))
                {
                    ToUpdate = false;
                    Game.World.Spawn(this, false);
                }
                if (TimeNow > LastXP.AddSeconds(3))
                {
                    LastXP = TimeNow;
                    XPKO++;
                }
                if (PoisonedInfo.Poisoned)
                {
                    if (TimeNow > PoisonedInfo.LastAttack.AddSeconds(2))
                    {
                        Features.Poison.AttackPoisonedCharacter(EntityID);
                    }
                }
                if (TimeNow > LastStamina.AddSeconds(1))
                {
                    LastStamina = TimeNow;
                    // Body = Body;

                    //CurHP = Math.Min(CurHP, MaxHP);
                    if (Alive && CurHP == 0)
                        CurHP = 1;

                    if (Action == 230)
                        PacketHandling.CoolEffect.ActiveCool(MyClient);

                    /* if (LuckyTime > 0)
                     {
                         LuckyTime = 0;
                         MyClient.AddSend(Packets.Status(EntityID, Status.LuckyTime, 0));//ulong value = LuckyTime * 1000
                     }*/
                    if (/*!StatEff.Contains(StatusEffectEn.Dead)*/ Alive && !Flying /*!StatEff.Contains(StatusEffectEn.Fly)*/ && !Transformation.Transformed && TimeNow > LastMove.AddMilliseconds(500) && !AtkMem.Attacking)
                    {
                        if (Action == 250)
                            Stamina += 15;
                        else
                            Stamina += 3;
                    }



                    /* List<Item> Remove = new List<Item>();
                     foreach (Item I in Inventory)
                         if (!Database.DatabaseItems.Contains(I.ID))
                             Remove.Add(I);
                     foreach (Item I in Remove)
                         Inventory.Remove(I);*/

                }

                if (MyTeam != null) MyTeam.LeaderCoords();

                if (Alive)
                {
                    if (TimeNow > LastMove.AddSeconds(3) && CheckTank)
                    {
                        SetTank();
                    }
                    try
                    {
                        if (AtkMem.FireCircle && DateTime.Now > AtkMem.LastAttack.AddMilliseconds(2200))
                        {
                            AtkMem.FireCircle = false;
                            if (Skills.ContainsKey((ushort)1120))
                            {
                                SkillsClass.SkillUse SU = new SkillsClass.SkillUse();
                                Skill S = Skills[(ushort)1120];
                                SU.Init(this, S.ID, S.Lvl, 0, 0);
                                SU.GetTargets();
                                SU.Use();
                            }
                        }
                    }
                    catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }
                }
                else
                {
                    if (AtkMem.FireCircle)
                        AtkMem.FireCircle = false;
                    if (!Ghost && TimeNow > DeathHit.AddSeconds(2))
                    {
                        Ghost = true;
                        StatEff.Add(StatusEffectEn.Dead);
                        MyClient.AddSend(Packets.Status(EntityID, Status.Hair, 0));
                        string Avt = "0";
                        if (Avatar.ToString().Length == 1)
                            Avt = "00" + Avatar.ToString();
                        else if (Avatar.ToString().Length == 2)
                            Avt = "0" + Avatar.ToString();
                        else Avt = Avatar.ToString();
                        if (Body == 1003 || Body == 1004)
                            MyClient.AddSend(Packets.Status(EntityID, Status.Mesh, uint.Parse("98" + Avt + Body.ToString())));
                        else
                            MyClient.AddSend(Packets.Status(EntityID, Status.Mesh, uint.Parse("99" + Avt + Body.ToString())));
                        World.Spawn(this, false);
                    }
                }
                if (TimeNow > LastBuffRemove.AddSeconds(1))
                {
                    LastBuffRemove = TimeNow;
                    //Buff[] Buff = null;
                    //if (Buffs.Count > 0)
                    //{
                    //    Buff = new Buff[Buffs.Count];
                    //    Buffs.CopyTo(Buff, 0);
                    //}
                    if (Buffs != null)
                    {
                        foreach (Buff B in Buffs.Keys)
                        {
                            ushort Time = B.Lasts;
                            if (B.Eff == SkillsClass.ExtraEffect.Cyclone || B.Eff == SkillsClass.ExtraEffect.Superman)
                            {
                                if (B.Started == World.CycloneEvent && B.Value == 90)
                                {
                                    if (DateTime.Now > B.Started.AddSeconds(90))
                                        BDelete.TryAdd(B, B.Lasts);
                                }
                                else if (EventBase != null && EventBase.EventTitle == "Speed Duel")
                                {
                                    if (DateTime.Now > B.Started.AddSeconds(Time))
                                    {
                                        if (!BDelete.ContainsKey(B))
                                            BDelete.TryAdd(B, B.Lasts);
                                    }
                                }
                                else
                                {
                                    if (TotalKO < PrevXPKO)
                                        PrevXPKO = 0;
                                    ushort CurXPKO = (ushort)(TotalKO - PrevXPKO);
                                    PrevXPKO = TotalKO;
                                    TimeBuff += CurXPKO;
                                    if (TimeBuff > 20)
                                        TimeBuff = 20;
                                    if (TimeBuff > 0)
                                        TimeBuff--;

                                    if (TimeBuff <= 0)
                                    {
                                        PrevXPKO = 0;
                                        World.NewKO(Name, TotalKO);
                                        TotalKO = 0;
                                        if (!BDelete.ContainsKey(B))
                                            BDelete.TryAdd(B, B.Lasts);
                                    }
                                }
                            }
                            else if (DateTime.Now > B.Started.AddSeconds(Time))
                            {
                                if (!BDelete.ContainsKey(B))
                                    BDelete.TryAdd(B, B.Lasts);
                            }
                        }
                    }
                    //bool had = false;
                    // bool stillhas = false;
                    try
                    {
                        //Buff[] BToDelete = null;
                        //if (BDelete.Count > 0)
                        //{
                        //    BToDelete = new Buff[BDelete.Count];
                        //    BDelete.CopyTo(BToDelete, 0);
                        //}
                        if (BDelete != null)
                        {
                            foreach (Buff B in BDelete.Keys)
                            {
                                RemoveBuff(B);
                                /*if (B.Eff == SkillsClass.ExtraEffect.Cyclone || B.Eff == SkillsClass.ExtraEffect.Superman)
                                {
                                    had = true;
                                    if (BuffOf(SkillsClass.ExtraEffect.Cyclone).Eff == SkillsClass.ExtraEffect.Cyclone || BuffOf(SkillsClass.ExtraEffect.Superman).Eff == SkillsClass.ExtraEffect.Superman)
                                        stillhas = true;
                                }*/
                            }
                        }
                        BDelete = new ConcurrentDictionary<Buff, ushort>();

                        /*if (had)
                        {
                            if (!stillhas)
                            {
                                World.NewKO(Name, TotalKO);
                                TotalKO = 0;
                            }
                        }*/

                    }
                    catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                }
                if (DoubleExp && (TimeNow > ExpPotionUsed.AddSeconds(DoubleExpLeft))) // double exp control
                {
                    DoubleExpLeft = 0;
                    DoubleExp = false;

                    if (!ExpPotUnder70)
                        MyClient.LocalMessage(2000, "VIP or potion double exp ended.");
                    else
                    {
                        MyClient.LocalMessage(2000, "Free newbie double exp ended.");
                        ExpPotUnder70 = false;
                    }
                }

                if (Mining)
                {
                    if ((TimeNow > LastMine.AddSeconds(5) && Inventory.Count < 40) || (TimeNow > LastMine.AddSeconds(4) && Inventory.Count < 40 && VipLevel > 4))
                    {
                        LastMine = TimeNow;
                        Features.Mining.Swing(this);
                    }
                    else if (Inventory.Count == 40)
                    {
                        Mining = false;
                    }
                }
                if (PKPoints > 0 && TimeNow > LastPKPLost.AddMinutes(3))//6
                {
                    PKPoints--;
                    LastPKPLost = TimeNow;
                }
                if (BlueName && TimeNow > BlueNameStarted.AddSeconds(BlueNameLasts))
                {
                    BlueName = false;
                    BlueNameLasts = 0;
                }
                if (Loaded)
                    if (TimeNow > LastSave.AddSeconds(120) && !Program.Reseting)
                    {
                        if (VIPDays > 0)
                            if (DateTime.Now > VIPStarted.AddHours(24))
                            {
                                VIPStarted = VIPStarted.AddHours(24);
                                VIPDays--;
                                if (VIPDays == 0)
                                    VipLevel = 0;
                            }
                        LastSave = TimeNow;
                        Database.SaveCharacter(this, MyClient.AuthInfo.Account);
                    }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }

        }
        public void Attack()
        {
            try
            {
                DateTime TimeNow = DateTime.Now;
                #region Attacking
                Action = 100;
                if (!Alive) return;
                if (ProtectTime.AddMilliseconds(0) > TimeNow && !CancelProtectTime)
                    return;
                if (StatEff.Contains(Game.StatusEffectEn.IceBlock))
                    return;
                Extra.Durability.AttackDurability(MyClient);
                if (AtkMem.AtkType != 21 && TimeNow > AtkMem.LastAttack.AddMilliseconds(AtkFrequence))
                {
                    if (AtkMem.AtkType == 2)
                        if (Flying)
                            return;
                    Game.Mob PossMob = null;
                    Game.Character PossChar = null;
                    Game.Companion PossComp = null;

                    if (Game.World.H_Mobs.ContainsKey(Loc.Map))
                    {
                        if (Game.World.H_Mobs[Loc.Map].ContainsKey(AtkMem.Target))
                            PossMob = (Game.Mob)Game.World.H_Mobs[Loc.Map][AtkMem.Target];
                        else if (Game.World.H_Chars.ContainsKey(AtkMem.Target))
                            PossChar = Game.World.H_Chars[AtkMem.Target];
                        else if (Game.World.H_Companions.ContainsKey(AtkMem.Target))
                            PossComp = (Companion)Game.World.H_Companions[AtkMem.Target];
                    }
                    else
                    {
                        if (Game.World.H_Chars.ContainsKey(AtkMem.Target))
                            PossChar = Game.World.H_Chars[AtkMem.Target];
                        else if (Game.World.H_Companions.ContainsKey(AtkMem.Target))
                            PossComp = (Companion)Game.World.H_Companions[AtkMem.Target];
                    }
                    if (PossChar != null)
                    {
                        if (!PossChar.PKAble(PKMode, this))
                            PossChar = null;
                        if (PossChar != null)
                        {
                            if (PossChar.ProtectTime.AddMilliseconds(0) > DateTime.Now && !CancelProtectTime)
                            {
                                PossChar = null;
                            }
                        }
                        if (PossChar != null)
                        {
                            if ((PossChar.Level <= 6 || Level <= 6) && (PossChar.Loc.Map == 1002 || PossChar.Loc.Map == 1011 || PossChar.Loc.Map == 1020 || PossChar.Loc.Map == 1000 || PossChar.Loc.Map == 1015 || PossChar.Loc.Map == 1009))
                            {
                                PossChar = null;
                                MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
                            }
                        }
                    }

                    byte Dist = Math.Max(Equips.RightHand.DBInfo.Dist, Transformation.Dist);
                    Dist = Math.Max((byte)2, Dist);
                    if (PossMob != null || PossChar != null || PossComp != null)
                    {
                        uint Damage = PrepareAttack((byte)AtkMem.AtkType, true);

                        if (EventBase != null)
                        {
                            if (EventBase.NoDamage && EventBase?.Stage == Events.EventStage.Fighting)
                                //if (EventBase?.MapEvent == Loc.Map)
                                Damage = EventBase.GetDamage(this, PossChar, (AttackType)AtkMem.AtkType);
                        }
                        // if (PossMob != null && PossMob.Alive && (MyMath.PointDistance(Loc.X, Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= Dist || AtkMem.AtkType == 28 && MyMath.PointDistance(Loc.X, Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= 15))
                        if (PossMob != null && PossMob.Alive && MyMath.PointDistance(Loc.X, Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= Dist)
                        {
                            if ((Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison && Equips.RightHand.Effect == Item.RebornEffect.Poison) && MyMath.ChanceSuccess(10))
                            {
                                PossMob.TakeAttack(this, ref Damage, AttackType.Melee, false, true);
                                //Console.WriteLine("Poisoned");
                            }
                            else if ((Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison || Equips.RightHand.Effect == Item.RebornEffect.Poison) && MyMath.ChanceSuccess(5))
                            {

                                PossMob.TakeAttack(this, ref Damage, AttackType.Melee, false, true);
                            }
                            if (!WeaponSkill(PossMob.Loc.X, PossMob.Loc.Y, PossMob.EntityID))
                            {
                                PossMob.TakeAttack(this, ref Damage, (Ultimate.Game.AttackType)AtkMem.AtkType, false);
                            }

                        }
                        //else if (PossChar != null && (PossChar.CanBeMeleed || AtkMem.AtkType != 2) && PossChar.MyClient.Soc.Connected && PossChar.Alive && (MyMath.PointDistance(Loc.X, Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= Dist || AtkMem.AtkType == 28 && MyMath.PointDistance(Loc.X, Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= 15))
                        else if (PossChar != null && (PossChar.CanBeMeleed || AtkMem.AtkType != 2) && PossChar.MyClient.Soc.Connected && PossChar.Alive && MyMath.PointDistance(Loc.X, Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= Dist)
                        {
                            if (!WeaponSkill(PossChar.Loc.X, PossChar.Loc.Y, PossChar.EntityID))
                                PossChar.TakeAttack(this, ref Damage, (Ultimate.Game.AttackType)AtkMem.AtkType, false);
                            if (!World.NoPKMaps.Contains(PossChar.Loc.Map) && Loc.Map != 1080 && Loc.Map != 1017)
                                if ((Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison && Equips.RightHand.Effect == Item.RebornEffect.Poison) && MyMath.ChanceSuccess(15))
                                {
                                    Features.Poison.PoisonCharacter(PossChar.EntityID, EntityID);
                                }
                                else if ((Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison || Equips.RightHand.Effect == Item.RebornEffect.Poison) && MyMath.ChanceSuccess(10))
                                {
                                    Features.Poison.PoisonCharacter(PossChar.EntityID, EntityID);
                                }
                        }
                        //else if (PossComp != null && PossComp.Owner.MyClient.Soc.Connected && (MyMath.PointDistance(Loc.X, Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= Dist || AtkMem.AtkType == 28 && MyMath.PointDistance(Loc.X, Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= 15))
                        else if (PossComp != null && PossComp.Owner.MyClient.Soc.Connected && MyMath.PointDistance(Loc.X, Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= Dist)
                        {
                            PossComp.TakeAttack(this, ref Damage, (Ultimate.Game.AttackType)AtkMem.AtkType, false);
                        }
                        else
                        {
                            AtkMem.Target = 0;
                            AtkMem.Attacking = false;
                        }
                    }
                    else if (World.H_SOBs.ContainsKey(AtkMem.Target))
                    {
                        AtkMem.LastAttack = TimeNow;
                        uint Damage = PrepareAttack((byte)(Ultimate.Game.AttackType)AtkMem.AtkType, true);

                        if (World.H_SOBs[AtkMem.Target].IsPole())
                        {
                            if (World.H_SOBs[AtkMem.Target].War && MyGuild != null && (World.H_SOBs[AtkMem.Target].LastWinner == null || MyGuild.GuildID != World.H_SOBs[AtkMem.Target].LastWinner.GuildID))
                            {
                                if (!WeaponSkill(World.H_SOBs[AtkMem.Target].Loc.X, World.H_SOBs[AtkMem.Target].Loc.Y, World.H_SOBs[AtkMem.Target].EntityID))
                                    World.H_SOBs[AtkMem.Target].TakeAttack(this, Damage, AtkMem.AtkType);
                            }
                            else
                            {
                                AtkMem.Target = 0;
                                AtkMem.Attacking = false;
                            }
                        }
                        else
                        {
                            if (!WeaponSkill(World.H_SOBs[AtkMem.Target].Loc.X, World.H_SOBs[AtkMem.Target].Loc.Y, World.H_SOBs[AtkMem.Target].EntityID))
                                World.H_SOBs[AtkMem.Target].TakeAttack(this, Damage, AtkMem.AtkType);
                        }
                        return;
                    }
                    if (PossChar == null && PossMob == null && PossComp == null)
                    {
                        NPC PossNPC = null;
                        if (World.H_NPCs.ContainsKey(Loc.Map))
                        {
                            Dictionary<uint, NPC> MapNPC = World.H_NPCs[Loc.Map];
                            if (MapNPC.ContainsKey(AtkMem.Target))
                            {
                                PossNPC = (NPC)MapNPC[AtkMem.Target];
                                // if (PossNPC != null && PossNPC.Flags == 21 && (MyMath.PointDistance(Loc.X, Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= Dist || AtkMem.AtkType == 28 && MyMath.PointDistance(Loc.X, Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= 15))
                                if (PossNPC.Flags == 21 && MyMath.PointDistance(Loc.X, Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= Dist)
                                {
                                    if (TimeNow > AtkMem.LastAttack.AddMilliseconds(AtkFrequence))
                                    {
                                        uint Damage = PrepareAttack((byte)AtkMem.AtkType, true);
                                        if (!WeaponSkill(PossNPC.Loc.X, PossNPC.Loc.Y, PossNPC.EntityID))
                                            PossNPC.TakeAttack(this, Damage, (Ultimate.Game.AttackType)AtkMem.AtkType, false);
                                    }
                                }
                            }
                        }
                    }

                }
                else if (TimeNow >= AtkMem.LastAttack.AddMilliseconds(1000) && (AtkMem.Skill != 1120 || TimeNow >= AtkMem.LastAttack.AddMilliseconds(2000)))
                {
                    if (AtkMem.Skill != 0 && Skills.ContainsKey(AtkMem.Skill))
                    {
                        AtkMem.LastAttack = TimeNow;
                        Skill S = (Skill)Skills[AtkMem.Skill];
                        if (Features.SkillsClass.SkillInfos.ContainsKey(S.ID + " " + S.Lvl))
                        {
                            Features.SkillsClass.SkillUse SU = new Ultimate.Features.SkillsClass.SkillUse();
                            SU.Init(this, S.ID, S.Lvl, AtkMem.SX, AtkMem.SY);
                            if (SU.Info.ID == 0)
                            {
                                AtkMem.Attacking = false;
                                return;
                            }
                            if (Loc.Map != 1039 && Loc.Map != 701 && Loc.Map != 1004)
                            {
                                if (SU.Info.ManaCost > CurMP || SU.Info.StaminaCost > Stamina && Pervade <= 0)
                                {
                                    AtkMem.Attacking = false;
                                    return;
                                }
                                if (SU.Info.ID != 3090)
                                {
                                    Mob M = null;
                                    if (World.H_Mobs.ContainsKey(Loc.Map))
                                        if (World.H_Mobs[Loc.Map].ContainsKey(AtkMem.Target))
                                            M = World.H_Mobs[Loc.Map][AtkMem.Target];
                                    Companion C = null;
                                    if (World.H_Companions.ContainsKey(AtkMem.Target))
                                        C = (Companion)World.H_Companions[AtkMem.Target];
                                    if (M == null && C == null && (AtkMem.Target < 6700 || AtkMem.Target > 6726))
                                    {
                                        AtkMem.Attacking = false;
                                        return;
                                    }
                                    if (M != null)
                                    {
                                        if (!M.Alive)
                                        {
                                            AtkMem.Attacking = false;
                                            return;
                                        }
                                    }
                                    if (C != null)
                                    {
                                        if (C.CurHP == 0)
                                        {
                                            AtkMem.Attacking = false;
                                            return;
                                        }
                                    }
                                }
                                CurMP -= SU.Info.ManaCost;
                                if (SU.Info.ID == 3090 && Pervade > 0)
                                {
                                    if (Pervade == 3 && SU.Info.Level > 3)
                                    {
                                        if (SU.Info.Level == 4)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e266")).Get);
                                        else if (SU.Info.Level == 5)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e267")).Get);
                                        else
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "tj")).Get);
                                    }
                                    else if (Pervade == 2 && SU.Info.Level <= 3)
                                    {
                                        if (SU.Info.Level == 0)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e263")).Get);
                                        else if (SU.Info.Level == 1)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e263")).Get);
                                        else if (SU.Info.Level == 2)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e264")).Get);
                                        else if (SU.Info.Level == 3)
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "zf2-e265")).Get);
                                        else
                                            World.Action(this, (Packets.StringPacket(EntityID, StringType.Effect, "tj")).Get);
                                    }


                                    Stamina -= 0;
                                    Pervade--;
                                }
                                else
                                    Stamina -= SU.Info.StaminaCost;


                                if (Equips.RightHand.Effect == Ultimate.Game.Item.RebornEffect.MP)//ManaBS
                                {
                                    if (MyMath.ChanceSuccess(30))
                                    {
                                        MyClient.AddSend(Packets.StringPacket(EntityID, StringType.Effect, "spilth1"));
                                        CurMP += 310;
                                    }
                                }
                                else if (Equips.RightHand.Effect == Ultimate.Game.Item.RebornEffect.HP)//HpBS
                                {
                                    if (MyMath.ChanceSuccess(30))
                                    {
                                        MyClient.AddSend(Packets.StringPacket(EntityID, StringType.Effect, "spilth"));
                                        if (EventBase != null && EventBase?.Stage == Events.EventStage.Fighting && EventBase.NoDamage)
                                            CurHP += 3;
                                        else
                                            CurHP += 310;
                                    }
                                }
                            }
                            else if (Loc.Map == 1039)
                            {
                                if (AtkMem.Skill == 1115)
                                {
                                    if (ItemIDManipulation.Part(Equips.LeftHand.ID, 0, 3) != ItemIDManipulation.Part(Equips.RightHand.ID, 0, 3))
                                        AtkMem.Attacking = false;
                                }
                                Character C = null;
                                if (C != null)
                                {
                                    if (World.H_Chars.ContainsKey(AtkMem.Target))
                                        C = (Character)World.H_Chars[AtkMem.Target];

                                    if (C.Loc.Map != 1039)
                                        AtkMem.Attacking = false;

                                    if (MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) >= 15)
                                        AtkMem.Attacking = false;
                                }
                            }

                            SU.GetTargets(AtkMem.Target);
                            SU.Use();
                        }
                    }
                }

                #endregion
            }
            catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
            // }
            //catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        internal string NewName = "";
        internal DateTime LastDLLCheck;
    }
}