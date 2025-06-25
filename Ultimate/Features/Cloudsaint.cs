using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class Cloudsaint
    {
        /// <summary>
        /// Types of monsters to be displayed in the jar and used for the quest
        /// </summary>
        public enum MonsterType
        {
            None = 0,
            Pheasant = 1,
            Turtledove = 2,
            Robin = 3,
            Apparition = 4,
            Poltergeist = 5,
            WingedSnake = 6,
            Bandit = 7,
            Ratling = 8,
            FireSpirit = 9,
            Macaque = 10,
            GiantApe = 11,
            ThunderApe = 12,
            Snakeman = 13,
            SandMonster = 14,
            HillMonster = 15,
            RockMonster = 16,
            BladeGhost = 17,
            Birdman = 18,
            HawKing = 19,
            TombBat = 20,
            BanditL97 = 55,
            BloodyBat = 56,
            BullMonster = 57,
            RedDevilL117 = 58
        }

        /// <summary>
        /// Called by the city Captains to see which mobs can be chosen
        /// </summary>
        /// <param name="Captain"></param>
        /// <returns></returns>
        public static Dictionary<byte, MonsterType> Available(byte Captain)
        {
            Dictionary<byte, MonsterType> Mobs = null;
            switch (Captain)
            {
                case 1:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {1, MonsterType.Pheasant},
                        {3, MonsterType.Turtledove},
                        {12, MonsterType.Robin},
                        {17, MonsterType.Apparition},
                        {22, MonsterType.Poltergeist}
                    };
                    break;
                case 2:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {27, MonsterType.WingedSnake},
                        {32, MonsterType.Bandit},
                        {37, MonsterType.Ratling},
                        {42, MonsterType.FireSpirit}
                    };
                    break;
                case 3:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {47, MonsterType.Macaque},
                        {52, MonsterType.GiantApe},
                        {57, MonsterType.ThunderApe},
                        {62, MonsterType.Snakeman}
                    };
                    break;
                case 4:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {67, MonsterType.SandMonster},
                        {72, MonsterType.HillMonster},
                        {77, MonsterType.RockMonster},
                        {82, MonsterType.BladeGhost},
                    };
                    break;
                case 5:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {87, MonsterType.Birdman},
                        {92, MonsterType.HawKing},
                        {97, MonsterType.BanditL97},
                    };
                    break;
                case 6:
                    Mobs = new Dictionary<byte, MonsterType>()
                    {
                        {102, MonsterType.TombBat},
                        {107, MonsterType.BloodyBat},
                        {112, MonsterType.BullMonster},
                        {117, MonsterType.RedDevilL117}
                    };
                    break;
            }
            return Mobs;
        }

        /// <summary>
        /// Retrives the IDs of the monsters counting on each jar
        /// </summary>
        /// <param name="Mob"></param>
        /// <returns></returns>
        public static List<int> MonsterIDs(MonsterType Mob)
        {
            List<int> Mobs = new List<int>();
            switch (Mob)
            {
                case MonsterType.Pheasant:
                    Mobs = new List<int>() { 1, 2 };
                    break;
                case MonsterType.Turtledove:
                    Mobs = new List<int>() { 3, 4 };
                    break;
                case MonsterType.Robin:
                    Mobs = new List<int>() { 5, 6 };
                    break;
                case MonsterType.Apparition:
                    Mobs = new List<int>() { 9 };
                    break;
                case MonsterType.Poltergeist:
                    Mobs = new List<int>() { 10, 12, 108 };
                    break;
                case MonsterType.WingedSnake:
                    Mobs = new List<int>() { 14, 15, 110, 111, 112 };
                    break;
                case MonsterType.Bandit:
                    Mobs = new List<int>() { 17, 18 };
                    break;
                case MonsterType.Ratling:
                    Mobs = new List<int>() { 20, 21 };
                    break;
                case MonsterType.FireSpirit:
                    Mobs = new List<int>() { 22, 23 };
                    break;
                case MonsterType.Macaque:
                    Mobs = new List<int>() { 27, 28 };
                    break;
                case MonsterType.GiantApe:
                    Mobs = new List<int>() { 29, 30 };
                    break;
                case MonsterType.ThunderApe:
                    Mobs = new List<int>() { 31, 32 };
                    break;
                case MonsterType.Snakeman:
                    Mobs = new List<int>() { 36, 37 };
                    break;
                case MonsterType.SandMonster:
                    Mobs = new List<int>() { 40, 41 };
                    break;
                case MonsterType.HillMonster:
                    Mobs = new List<int>() { 43, 44 };
                    break;
                case MonsterType.RockMonster:
                    Mobs = new List<int>() { 45, 46 };
                    break;
                case MonsterType.BladeGhost:
                    Mobs = new List<int>() { 49, 50 };
                    break;
                case MonsterType.Birdman:
                    Mobs = new List<int>() { 52, 53 };
                    break;
                case MonsterType.HawKing:
                    Mobs = new List<int>() { 54, 55 };
                    break;
                case MonsterType.TombBat:
                    Mobs = new List<int>() { 63, 64 };
                    break;
                case MonsterType.BanditL97:
                    Mobs = new List<int>() { 58, 59 };
                    break;
                case MonsterType.BloodyBat:
                    Mobs = new List<int>() { 66, 67 };
                    break;
                case MonsterType.BullMonster:
                    Mobs = new List<int>() { 70, 95 };
                    break;
                case MonsterType.RedDevilL117:
                    Mobs = new List<int>() { 71, 72 };
                    break;
            }
            return Mobs;
        }

        /// <summary>
        /// Called when a player wants to take a cloudsaint jar and start the quest
        /// </summary>
        /// <param name="C"></param>
        /// <param name="_monsterType"></param>
        public static bool SelectMonster(Character C, byte monsterType)
        {
            if (!C.InventoryContains(750000, 1))
            {
                Item I = new Item();
                I.ID = 750000;
                I.UID = 0;
                I.MaxDur = monsterType;
                I.CurDur = SelectCount(monsterType);
                I.Color = Item.ArmorColor.Orange;
                C.AddItem(I);
                C.ToKill = (MonsterType)monsterType;
                C.CurrentKills = 0;
                C.MyClient.AddSend(Packets.UpdateCloudSaintJar(C.EntityID, monsterType, C.CurrentKills));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Called when we're choosing which jar to give the player, it selects the total kills amount
        /// </summary>
        /// <param name="monsterType"></param>
        /// <returns></returns>
        public static ushort SelectCount(byte monsterType)
        {
            switch (monsterType)
            {
                case 1:
                    return 50;
                case 2:
                case 3:
                    return 100;
                case 4:
                case 5:
                    return 100;
                case 6:
                case 7:
                    return 150;
                case 8:
                case 9:
                    return 200;
                case 10:
                case 11:
                case 12:
                case 13:
                    return 250;
                default:
                    return 300;
            }
        }


        /// <summary>
        /// Called by the NPC when player delivers the jar
        /// </summary>
        /// <param name="C"></param>
        /// <param name="monsterType"></param>
        /// <returns></returns>
        public static bool Award(Character C, byte monsterType)
        {
            if (C.InventoryContains(750000, 1) && C.Inventory.Count < 36 && C.NextItem(750000).MaxDur == (byte)C.ToKill && C.CurrentKills >= C.NextItem(750000).CurDur)
            {
                C.RemoveItem(C.NextItem(750000));
                C.CurrentKills = 0;
                C.ToKill = MonsterType.None;
                switch (monsterType)
                {
                    case 1:
                        //C.AddExp(1 / 10.0);
                        if (C.Job < 100)
                        {
                            C.AddItem(720027);
                            C.AddItem(120007);
                            C.AddItem(150007);
                            if (C.Job < 40 || C.Job > 45)
                                C.AddItem(410036);
                            else
                            {
                                C.AddItem(133017);
                                C.AddItem(500037);
                            }
                        }
                        else
                        {
                            C.AddItem(121007);
                            C.AddItem(421007);
                            C.AddItem(134017);
                        }
                        C.AddItem(160016);
                        C.ExpPotionUsed = DateTime.Now;
                        C.DoubleExp = true;
                        C.DoubleExpLeft = 1200;
                        C.Silvers += 1000;
                        C.MyClient.AddSend(Packets.Status(C.EntityID, Status.DoubleExpTime, (ulong)C.DoubleExpLeft));
                        C.AddExp(1 * 6.0);
                        if (C.Job >= 40 && C.Job <= 45)
                            for (int a = 0; a < 5; a++)
                                if (C.Inventory.Count < 40)
                                    C.AddItem(1050000);
                        //C.VIP = DateTime.Now.AddMinutes(5);
                        break;
                    case 2:
                        C.AddExp(1 * 10.0);
                        C.AddItem(150018);
                        C.Silvers += 5000;
                        if (C.Job >= 40 && C.Job <= 45)
                            for (int a = 0; a < 5; a++)
                                if (C.Inventory.Count < 40)
                                    C.AddItem(1050000);
                        break;
                    case 3:
                    case 4:
                    case 5:
                        C.Silvers += 5000;
                        C.AddItem(720027);
                        C.AddExp(1 * 10.0);
                        break;
                    case 6:
                    case 7:
                        if (C.Level > 20 && C.Level < 41)
                        {
                            if (C.Job < 100)
                            {
                                C.AddItem(120088);
                                C.AddItem(150098);
                                if (C.Job < 40 || C.Job > 45)
                                    C.AddItem(410078);
                                else
                                {
                                    C.AddItem(133028);
                                    C.AddItem(500078);
                                }
                            }
                            else
                            {
                                C.AddItem(121088);
                                C.AddItem(421078);
                                C.AddItem(134028);
                            }
                            C.AddItem(160098);
                        }
                        C.Silvers += 10000;
                        C.AddItem(720027);
                        C.AddExp(1 * 15.0);

                        break;
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        C.AddItem(720027);
                        C.AddItem(720027);
                        C.AddItem(720027);
                        C.AddItem(720027);
                        C.AddItem(720027);
                        C.Silvers += 50000;
                        C.AddExp(1 * 30.0);

                        break;
                    default:
                        C.AddExp(1 * 30.0);
                        if (C.Level > 60 && C.Level < 83)
                        {
                            if (C.Job < 100)
                            {
                                C.AddItem(120159);
                                C.AddItem(150159);
                                if (C.Job < 40 || C.Job > 45)
                                    C.AddItem(410159);
                                else
                                {
                                    C.AddItem(133059);
                                    C.AddItem(500159);
                                }
                            }
                            else
                            {
                                C.AddItem(121159);
                                C.AddItem(421159);
                                C.AddItem(134069);
                            }
                            C.AddItem(160159);
                        }
                        C.AddItem(720027);
                        C.Silvers += 15000;

                        break;

                }
                if (monsterType > 1 && !AddItem(C))
                {
                    if (MyMath.ChanceSuccess(100))
                    {
                        C.AddItem(722384);
                        C.AddItem(720650);
                        C.AddItem(720027);
                        C.Silvers = 250000;
                        C.AddItem(1088000);
                    }
                    else if (MyMath.ChanceSuccess(2))
                        C.AddItem(720650);
                    else if (MyMath.ChanceSuccess(0.7))
                        C.AddItem(720027);
                    else if (MyMath.ChanceSuccess(3))
                        C.Silvers = 25000;
                    else
                    {
                        for (int a = 0; a < 3; a++)
                            if (MyMath.ChanceSuccess(100))
                                C.AddItem(720027);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds an item to the character
        /// </summary>
        /// <param name="c"></param>
        /// <param name="quality"></param>
        public static bool AddItem(Character c)
        {
            bool addItem = false;
            byte quality = 0;
            bool plusOne = false;
            if (c.Level <= 70)
            {
                if (MyMath.ChanceSuccess(10))
                    quality = 7;
                else if (MyMath.ChanceSuccess(50))
                    quality = 8;
                else if (MyMath.ChanceSuccess(50))
                    quality = 9;
                if (MyMath.ChanceSuccess(2.5))
                    plusOne = true;
            }
            else if (c.Level <= 100)
            {
                if (MyMath.ChanceSuccess(10))
                    quality = 7;
                else if (MyMath.ChanceSuccess(50))
                    quality = 8;
                else if (MyMath.ChanceSuccess(50))
                    quality = 9;
                if (MyMath.ChanceSuccess(1.5))
                    plusOne = true;
            }
            else if (c.Level <= 120)
            {
                if (MyMath.ChanceSuccess(10))
                    quality = 7;
                else if (MyMath.ChanceSuccess(50))
                    quality = 8;
                else if (MyMath.ChanceSuccess(50))
                    quality = 9;
                if (MyMath.ChanceSuccess(1.5))
                    plusOne = true;
            }
            if (quality > 0)
                addItem = true;
            if (addItem)
            {
                var I = new Item();
                var rnd = new Random();
            Top:
                var from = new List<uint>();
                var type = rnd.Next(0, 330);
                var part = 0;
                if (type < 10) part = 111;
                else if (type < 20) part = 113;
                else if (type < 30) part = 114;
                else if (type < 40) part = 117;
                else if (type < 50) part = 118;
                else if (type < 60) part = 120;
                else if (type < 70) part = 121;
                else if (type < 80) part = 130;
                else if (type < 90) part = 131;
                else if (type < 100) part = 133;
                else if (type < 110) part = 134;
                else if (type < 120) part = 141;
                else if (type < 130) part = 142;
                else if (type < 140) part = 150;
                else if (type < 150) part = 151;
                else if (type < 160) part = 152;
                else if (type < 165) part = 160;
                else if (type < 175) part = 410;
                else if (type < 185) part = 420;
                else if (type < 195) part = 421;
                else if (type < 203) part = 430;
                else if (type < 211) part = 440;
                else if (type < 219) part = 450;
                else if (type < 229) part = 460;
                else if (type < 239) part = 480;
                else if (type < 247) part = 481;
                else if (type < 255) part = 490;
                else if (type < 265) part = 500;
                else if (type < 275) part = 510;
                else if (type < 285) part = 530;
                else if (type < 295) part = 540;
                else if (type < 305) part = 560;
                else if (type < 315) part = 561;
                else if (type < 325) part = 580;
                else if (type < 330) part = 900;

                foreach (var d in Database.DatabaseItems.Values)
                {
                    if (c.Level > 115)
                    {
                        if (d.LevReq < 106 || d.LevReq > 126) continue;
                        if (d.LevReq == 0) continue;
                        if (ItemIDManipulation.Part(d.ID, 0, 3) == part)
                            from.Add(d.ID);
                    }
                    else
                    {
                        if (d.LevReq + 15 <= c.Level || d.LevReq - 10 > c.Level) continue;
                        if (d.LevReq == 0) continue;
                        if (ItemIDManipulation.Part(d.ID, 0, 3) != part) continue;
                        from.Add(d.ID);
                    }
                }
                if (from.Count > 0)
                    I.ID = from[(byte)rnd.Next(0, from.Count)];
                else
                    goto Top;
                if (I.ID != 0)
                {
                    if (I.DBInfo.LevReq != 1)
                    {
                        var e = new ItemIDManipulation(I.ID);
                        e.QualityChange((Item.ItemQuality)quality);
                        I.ID = e.ToID();
                    }
                    I.Color = Item.ArmorColor.Orange;
                    if (plusOne)
                        I.Plus = 1;

                    I.MaxDur = I.DBInfo.Durability;
                    I.CurDur = I.MaxDur;
                }
                c.AddItem(I);
                return true;
            }
            return false;
        }
    }
}