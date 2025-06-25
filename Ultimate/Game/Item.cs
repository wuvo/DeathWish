using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Structures;

namespace Ultimate.Game
{
    public class Item
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
        public enum ItemType
        {
            Helm = 0x1,
            Necklace = 0x2,
            Armor = 0x3,
            RightHand = 0x4,
            LeftHand = 0x5,
            Ring = 0x6,
            Boots = 0x8,
            Garment = 0x9,
        }
        public static void GetWeaponEffect(ref EquipStats E, Prof P)
        {
            if (P.Lvl > 12)
                E.WeaponExtraAttack += (P.Lvl - 12) / (double)100;
        }
        public static void GetGemEffect(ref EquipStats E, Gem G)
        {
            switch (G)
            {
                case Gem.NormalDragonGem:
                    {
                        E.GemExtraAttack += 0.05;
                        break;
                    }
                case Gem.RefinedDragonGem:
                    {
                        E.GemExtraAttack += 0.1;
                        break;
                    }
                case Gem.SuperDragonGem:
                    {
                        E.GemExtraAttack += 0.15;
                        break;
                    }
                case Gem.NormalRainbowGem:
                    {
                        E.GemExtraExp += 0.1;
                        break;
                    }
                case Gem.RefinedRainbowGem:
                    {
                        E.GemExtraExp += 0.15;
                        break;
                    }
                case Gem.SuperRainbowGem:
                    {
                        E.GemExtraExp += 0.25;
                        break;
                    }
                case Gem.NormalPhoenixGem:
                    {
                        E.GemExtraMAttack += 0.05;
                        break;
                    }
                case Gem.RefinedPhoenixGem:
                    {
                        E.GemExtraMAttack += 0.1;
                        break;
                    }
                case Gem.SuperPhoenixGem:
                    {
                        E.GemExtraMAttack += 0.15;
                        break;
                    }
                case Gem.NormalVioletGem:
                    {
                        E.GemExtraProf += 0.3;
                        break;
                    }
                case Gem.RefinedVioletGem:
                    {
                        E.GemExtraProf += 0.5;
                        break;
                    }
                case Gem.SuperVioletGem:
                    {
                        E.GemExtraProf += 1;
                        break;
                    }
                case Gem.NormalMoonGem:
                    {
                        E.GemExtraMExp += 0.15;
                        break;
                    }
                case Gem.RefinedMoonGem:
                    {
                        E.GemExtraMExp += 0.3;
                        break;
                    }
                case Gem.SuperMoonGem:
                    {
                        E.GemExtraMExp += 0.5;
                        break;
                    }
                case Gem.NormalFuryGem:
                    {
                        E.GemExtraDex += 0.05;
                        break;
                    }
                case Gem.RefinedFuryGem:
                    {
                        E.GemExtraDex += 0.1;
                        break;
                    }
                case Gem.SuperFuryGem:
                    {
                        E.GemExtraDex += 0.15;
                        break;
                    }
                case Gem.NormalTortoiseGem:
                    {
                        E.GemBless += 0.02;
                        break;
                    }
                case Gem.RefinedTortoiseGem:
                    {
                        E.GemBless += 0.03;
                        break;
                    }
                case Gem.SuperTortoiseGem:
                    {
                        E.GemBless += 0.04;
                        break;
                    }
            }
        }
        public enum ArmorColor
        {
            Black = 2,
            Orange,
            LightBlue,
            Red,
            Blue,
            Yellow,
            Purple,
            White
        }
        public enum GarmentColor
        {
            Black = 2,
            Orange,
            LightBlue,
            Red,
            Blue,
            Yellow,
            Purple,
            White
        }
        public enum ItemQuality
        {
            Fixed = 0,
            NoUpgrade = 1,
            Simple = 3,
            Poor = 4,
            Normal = 5,
            Refined = 6,
            Unique = 7,
            Elite = 8,
            Super = 9
        }
        public enum Gem : byte
        {
            NormalPhoenixGem = 1,
            RefinedPhoenixGem = 2,
            SuperPhoenixGem = 3,

            NormalDragonGem = 11,
            RefinedDragonGem = 12,
            SuperDragonGem = 13,

            NormalFuryGem = 21,
            RefinedFuryGem = 22,
            SuperFuryGem = 23,

            NormalRainbowGem = 31,
            RefinedRainbowGem = 32,
            SuperRainbowGem = 33,

            NormalKylinGem = 41,
            RefinedKylinGem = 42,
            SuperKylinGem = 43,

            NormalVioletGem = 51,
            RefinedVioletGem = 52,
            SuperVioletGem = 53,

            NormalMoonGem = 61,
            RefinedMoonGem = 62,
            SuperMoonGem = 63,

            NormalTortoiseGem = 71,
            RefinedTortoiseGem = 72,
            SuperTortoiseGem = 73,

            /* NormalGloryGem = 121,
             RefinedGloryGem = 122,
             SuperGloryGem = 123,

             NormalThunderGem = 101,
             RefinedThunderGem = 102,
             SuperThunderGem = 103,*/

            NoSocket = 0,
            EmptySocket = 255
        }
        public enum RebornEffect
        {
            None = 0,
            Poison = 0xC8,
            HP = 0xC9,
            MP = 0xCA,
            Shield = 0xCB,
            Horsie = 0x64
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
        public uint RestrainType;
        public byte Plus;
        public byte Bless;
        public byte Enchant;
        public Gem Soc1;
        public Gem Soc2;
        public ushort MaxDur;
        public ushort CurDur;
        public bool FreeItem;
        public uint TalismanProgress;
        public ushort Progress;
        public RebornEffect Effect;
        public ArmorColor Color;
        public byte Pot
        {
            get
            {
                byte pot = 0;
                byte Quality = ItemIDManipulation.Digit(ID, 6);
                if (Quality >= 6)//5
                    pot += (byte)(Quality - 5);
                // pot += (byte)(ItemIDManipulation.Digit(ID, 6) - 5);
                pot += Plus;
                if (Soc1 != Gem.NoSocket) pot++;
                if (Soc2 != Gem.NoSocket) pot++;
                byte soc1 = (byte)Soc1;
                byte soc2 = (byte)Soc2;
                if (soc1 % 10 == 3) pot++;
                if (soc2 % 10 == 3) pot++;
                if (ItemIDManipulation.Digit(ID, 1) == 5 || ItemIDManipulation.Part(ID, 0, 3) == 421) pot *= 2;

                return pot;
            }
        }
        public static bool IsArrow(uint ID)
        {
            /*  ID == 1050020 ||
                  ID == 1050021 ||
                  ID == 1050022 ||
                  ID == 1050023 ||
                  ID == 1050030 ||
                  ID == 1050031 ||
                  ID == 1050032 ||
                  ID == 1050033 ||
                  ID == 1050040 ||
                  ID == 1050041 ||
                  ID == 1050042 ||
                  ID == 1050043 ||
                  ID == 1050050 ||
                  ID == 1050051 ||
                  ID == 1050052 ||
                  ID == 1051000 ||
                  ID == 1051000)*/
            if (ID == 1050000 ||
                ID == 1051000 ||
                ID == 1050001 ||
                ID == 1050002)
                return true;
            return false;
        }
        public static bool IsArrow(string Name)
        {
            if (Name == "LuckyArrow" ||
                Name == "IronArrow" ||
                Name == "ThronePack" ||
                Name == "SpeedArrow")
                return true;
            return false;
        }
        public static bool EquipPassLvlReq(Item Item, Character C)
        {
            if (C.Level < Item.DBInfo.LevReq)
                return false;
            else
                return true;
        }
        public static bool EquipPassRbReq(Item Item, Character C)
        {
            if (C.Equips.RightHand != null)
                if (Item.DBInfo.LevReq < 71 && C.Reborns > 0 && C.Level >= 70 && (C.Job < 100 || Item.DBInfo.ID < 900000 || Item.DBInfo.ID > 900109))
                    return true;
                else
                    return false;
            else if (Item.DBInfo.LevReq < 71 && C.Reborns > 0 && C.Level >= 70)
                return true;
            else
                return false;
        }
        public static bool EquipPassStatsReq(Item Item, Character C)
        {
            if (C.Str >= Item.DBInfo.StrNeed && C.Agi >= Item.DBInfo.AgiNeed)
                return true;
            else
                return false;
        }
        public static bool EquipPassJobReq(Item Item, Character C)
        {
            switch (Item.DBInfo.Class)
            {
                #region Trojan
                case 10: if (C.Job <= 15 && C.Job >= 10) return true; break;
                case 11: if (C.Job <= 15 && C.Job >= 11) return true; break;
                case 12: if (C.Job <= 15 && C.Job >= 12) return true; break;
                case 13: if (C.Job <= 15 && C.Job >= 13) return true; break;
                case 14: if (C.Job <= 15 && C.Job >= 14) return true; break;
                case 15: if (C.Job == 15) return true; break;
                #endregion
                #region Warrior
                case 20: if (C.Job <= 25 && C.Job >= 20) return true; break;
                case 21: if (C.Job <= 25 && C.Job >= 21) return true; break;
                case 22: if (C.Job <= 25 && C.Job >= 22) return true; break;
                case 23: if (C.Job <= 25 && C.Job >= 23) return true; break;
                case 24: if (C.Job <= 25 && C.Job >= 24) return true; break;
                case 25: if (C.Job == 25) return true; break;
                #endregion
                #region Archer
                case 40: if (C.Job <= 45 && C.Job >= 40) return true; break;
                case 41: if (C.Job <= 45 && C.Job >= 41) return true; break;
                case 42: if (C.Job <= 45 && C.Job >= 42) return true; break;
                case 43: if (C.Job <= 45 && C.Job >= 43) return true; break;
                case 44: if (C.Job <= 45 && C.Job >= 44) return true; break;
                case 45: if (C.Job == 45) return true; break;
                #endregion
                #region Ninja
                case 50: if (C.Job <= 55 && C.Job >= 50) return true; break;
                case 51: if (C.Job <= 55 && C.Job >= 51) return true; break;
                case 52: if (C.Job <= 55 && C.Job >= 52) return true; break;
                case 53: if (C.Job <= 55 && C.Job >= 53) return true; break;
                case 54: if (C.Job <= 55 && C.Job >= 54) return true; break;
                case 55: if (C.Job == 55) return true; break;
                #endregion
                #region Taoist
                case 190: if (C.Job >= 100) return true; break;
                #endregion
                case 0: return true;
                default: return false;
            }
            return false;
        }
        public static bool EquipPassSexReq(Item Item, Character C)
        {
            int ClientSex = C.Body % 10000 < 1005 ? 1 : 2;
            if (Item.DBInfo.GenderReq == 2 && ClientSex == 2)
                return true;
            if (Item.DBInfo.GenderReq != 2)
                return true;
            return false;
        }
        public bool CanEquip(Character C)
        {
            bool pass = false;
            if (EquipPassRbReq(this, C))
                pass = true;
            else
                if (EquipPassJobReq(this, C))
                if (EquipPassStatsReq(this, C))
                    if (EquipPassLvlReq(this, C))
                        if (EquipPassSexReq(this, C)) pass = true;
            if (!pass)
                return false;



            DatabaseItem DI = DBInfo;
            ItemIDManipulation E = new ItemIDManipulation(ID);
            E.ChangeDigit(4, 0);
            uint ID2 = E.ToID();
            if (!EquipPassRbReq(this, C))
            {
                if (DI.ProfReq != 0)
                {
                    if (E.Digit(1) == 4 || E.Digit(1) == 5 || E.Digit(1) == 6)
                    {
                        if (C.Profs.ContainsKey((ushort)E.Part(0, 3)))
                        {
                            Prof P = (Prof)C.Profs[(ushort)E.Part(0, 3)];
                            if (P.Lvl < DI.ProfReq)
                                return false;
                        }
                        else
                            return false;
                    }
                }
            }

            if (C.MyClient.AuthInfo.Status == "[PM]")
                return true;
            if (ID2 == 137010)
                return false;

            return true;
        }
        public bool IsWorth()
        {
            if (Plus > 0)
                return true;
            else if (Bless > 0)
                return true;
            else if (Enchant > 0)
                return true;
            else if (Soc1 > 0)
                return true;
            else if (Soc2 > 0)
                return true;
            else if (Progress > 0)
                return true;
            else if (ID >= 1000000 && ID <= 1072059 || ID == 0)
                return false;
            else if (DBInfo.LevReq == 0)
                return true;
            return false;
        }
        //public void OpenSocket(Character C)
        //{
        //    if (Soc1 == Game.Item.Gem.NoSocket)
        //    {
        //        Soc1 = Game.Item.Gem.EmptySocket;
        //        Game.World.SendMsgToAll("SYSTEM", $"{C.Name} has got 1 socket into his/her {DBInfo.Name}!", 2011, 0);
        //        Game.World.Action(C, (Packets.StringPacket(C.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
        //        Game.World.DebugAdd += C.Name + " has got 1st socket from on " + DBInfo.Name + " ( " + ID + "~" + Plus + "~" + Bless + "~" + Soc1 + "~" + Soc2 + "~" + Progress + " ) \r\n";
        //    }
        //    else if (Soc2 == Game.Item.Gem.NoSocket)
        //    {
        //        Soc2 = Game.Item.Gem.EmptySocket;
        //        Game.World.SendMsgToAll("SYSTEM", C.Name + " has got second socket into his/her " + DBInfo.Name, 2011, 0);
        //        Game.World.Action(C, (Packets.StringPacket(C.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
        //        Game.World.DebugAdd += C.Name + " has got 2nd socket from on " + DBInfo.Name + " ( " + ID + "~" + Plus + "~" + Bless + "~" + Soc1 + "~" + Soc2 + "~" + Progress + " ) \r\n";
        //    }
        //}
        //public bool TwoHanded()
        //{
        //    if (ItemIDManipulation.Part(ID, 0, 3) >= 510 && ItemIDManipulation.Part(ID, 0, 3) <= 580)
        //        return true;
        //    return false;
        //}
        public DatabaseItem DBInfo
        {
            get
            {
                if (Database.DatabaseItems.ContainsKey(ID))
                    return (DatabaseItem)Database.DatabaseItems[ID];
                return new DatabaseItem();
            }
        }
        public void WriteThis(System.IO.BinaryWriter I)
        {
            if (ID != 0 && UID == 0) UID = (uint)Program.Rnd.Next(10000000);
            I.Write((byte)27);
            I.Write(UID);//check
            I.Write(ID);//check
            I.Write(Plus);//check
            I.Write(Bless);//check
            I.Write(Enchant);//check
            I.Write((byte)Soc1);//check
            I.Write((byte)Soc2);//check
            I.Write(MaxDur);//check
            I.Write(CurDur);//check
            I.Write(FreeItem);//check
            I.Write(TalismanProgress);//check
            I.Write(Progress);//check
            I.Write((byte)Color);//check
            I.Write((ushort)Effect);//check
        }
        public void ReadThis(System.IO.BinaryReader I)
        {
            byte Length = I.ReadByte();
            if (Length == 27)//25
            {
                UID = I.ReadUInt32();

                if (UID == 0) UID = (uint)Program.Rnd.Next(10000000);

                ID = I.ReadUInt32();
                if (Database.DatabaseItems.ContainsKey(ID))
                {
                    Plus = I.ReadByte();
                    Bless = I.ReadByte();
                    Enchant = I.ReadByte();
                    Soc1 = (Gem)I.ReadByte();
                    Soc2 = (Gem)I.ReadByte();
                    MaxDur = I.ReadUInt16();
                    CurDur = I.ReadUInt16();
                    FreeItem = I.ReadBoolean();
                    TalismanProgress = I.ReadUInt32();
                    Progress = I.ReadUInt16();
                    Color = (ArmorColor)I.ReadByte();
                    Effect = (RebornEffect)I.ReadUInt16();
                }
                else
                {
                    ID = 0;
                    I.ReadBytes(19);//17
                }
            }
            else I.ReadBytes(Length);
        }

        public void AddItem(Character C)
        {

        }
    }
}
