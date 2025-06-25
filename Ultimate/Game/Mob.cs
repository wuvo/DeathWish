using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Ultimate.Features;
using Ultimate.Structures;
using Ultimate.MysqlDB;

namespace Ultimate.Game
{
    public enum AttackType : byte
    {
        Melee = 2,
        Ranged = 28,
        Magic = 21,
        Kill = 14,
        FatalStrike = 45,
        Scapegoat = 43
    }
    public enum MobBehaveour : byte
    {
        HuntPlayers = 1,
        HuntMobs = 2,
        HuntMobsAndPlayers = 3,
        HuntBlueNames = 4,//
        HuntMobsAndBlue = 6//
    }
    public class Mob
    {
        public static readonly Dictionary<uint, MapEffect> DropsEffects = new Dictionary<uint, MapEffect>();
        public DateTime LastMove;
        public byte Direction = 0;
        public byte Action = 0;
        public uint EntityID;
        public Location Loc;
        public SpawnLoc StartLoc;
        public bool Alive = true;
        public bool Dropped = false;
        public bool Dissappeared = false;
        public DateTime Died;
        public DateTime Respawned = DateTime.Now;
        public uint RandomTime = 30;
        public static MyRandom Rnd = new MyRandom();
        public DateTime LastTarget = DateTime.Now;

        Companion CompTarget;
        Character PlayerTarget;
        Mob MobTarget;
        public NPC NPCTarget;
        public int MobID;
        public ushort Mesh;
        public byte Level;
        public uint MaxHP;
        public uint CurrentHP;
        public ushort Defense;
        public ushort MDef;
        public ushort MAttack;
        public ushort MinAttack;
        public ushort MaxAttack;
        public string Name;
        public MobBehaveour Type;
        public byte DmgReduceTimes;
        public AttackType AtkType;
        public byte Dodge;
        public bool Gives;
        public byte AttackDist;
        public ushort MagicSkill = 0;
        public byte MagicLvl = 0;
        public int MinSilvers;
        public int MaxSilvers;
        public uint SpawnSpeed = 0;
        public ushort MoveSpeed = 0;
        bool LevDifDmg = true;
        public Features.PoisonType PoisonedInfo = null;
        public Mob(string Line)
        {
            LastMove = DateTime.Now;
            string[] Info = Line.Split(' ');
            MobID = int.Parse(Info[0]);
            Name = Info[1];
            Type = (MobBehaveour)byte.Parse(Info[2]);
            Mesh = ushort.Parse(Info[3]);
            Level = byte.Parse(Info[4]);
            MaxHP = uint.Parse(Info[5]);
            Defense = ushort.Parse(Info[6]);
            MDef = ushort.Parse(Info[7]);
            MAttack = ushort.Parse(Info[8]);
            MinAttack = ushort.Parse(Info[9]);
            MaxAttack = ushort.Parse(Info[10]);
            DmgReduceTimes = byte.Parse(Info[11]);
            Dodge = byte.Parse(Info[12]);
            AtkType = (AttackType)byte.Parse(Info[13]);
            if (AtkType == AttackType.Magic)
            {
                MagicSkill = ushort.Parse(Info[14]);
                MagicLvl = byte.Parse(Info[15]);
                Gives = bool.Parse(Info[16]);
                AttackDist = byte.Parse(Info[17]);
                MinSilvers = int.Parse(Info[18]);
                MaxSilvers = int.Parse(Info[19]);
                MoveSpeed = ushort.Parse(Info[20]);
                SpawnSpeed = uint.Parse(Info[21]);
                LevDifDmg = bool.Parse(Info[22]);
            }
            else
            {
                Gives = bool.Parse(Info[14]);
                AttackDist = byte.Parse(Info[15]);
                MinSilvers = int.Parse(Info[16]);
                MaxSilvers = int.Parse(Info[17]);
                MoveSpeed = ushort.Parse(Info[18]);
                SpawnSpeed = uint.Parse(Info[19]);
                LevDifDmg = bool.Parse(Info[20]);
            }

            CurrentHP = MaxHP;
        }
        public Mob(Mob M)
        {
            LastMove = DateTime.Now;
            MobID = M.MobID;
            Mesh = M.Mesh;
            Level = M.Level;
            MaxHP = M.MaxHP;
            CurrentHP = M.CurrentHP;
            Defense = M.Defense;
            MDef = M.MDef;
            MAttack = M.MAttack;
            MinAttack = M.MinAttack;
            MaxAttack = M.MaxAttack;
            Name = M.Name;
            Type = M.Type;
            DmgReduceTimes = M.DmgReduceTimes;
            Dodge = M.Dodge;
            AtkType = M.AtkType;
            Gives = M.Gives;
            AttackDist = M.AttackDist;
            MagicSkill = M.MagicSkill;
            MagicLvl = M.MagicLvl;
            MinSilvers = M.MinSilvers;
            MaxSilvers = M.MaxSilvers;
            MoveSpeed = M.MoveSpeed;
            SpawnSpeed = M.SpawnSpeed;
            LevDifDmg = M.LevDifDmg;
        }
        public Mob()
        { }

        uint PrepareAttack()
        {
            if (AtkType == AttackType.Melee || AtkType == AttackType.Ranged)
                return (uint)Rnd.Next(MinAttack, MaxAttack);
            else
                return MAttack;
        }
        public bool NeedsPKMode
        {
            get
            {
                if (Type == MobBehaveour.HuntBlueNames || Type == MobBehaveour.HuntMobsAndBlue || MobID == 8422)
                    return true;
                return false;
            }
        }
        public uint TakeAttack(Character Attacker, ref uint Damage, AttackType AT, bool IsSkill, bool Poison = false)
        {
            if (AT != AttackType.Magic && Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                Damage = (uint)(Damage * 10);
            double e = 1;
            if (Level + 4 < Attacker.Level)
                e = 0.1;
            if (Level + 4 >= Attacker.Level)
                e = 1;
            if (Level >= Attacker.Level)
                e = 1.1;
            if (Level - 4 > Attacker.Level)
                e = 1.4;

            if (Type == MobBehaveour.HuntBlueNames || Type == MobBehaveour.HuntMobsAndBlue)
            {
                Attacker.BlueName = true;
                if (Attacker.BlueNameLasts < 60)
                    Attacker.BlueNameLasts = 15;
            }
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
            }
            if (AT != AttackType.Magic && !IsSkill)
            {
                short _Agi = (short)((Attacker.Agi + Attacker.EqStats.ExtraDex) * Attacker.EqStats.GemExtraDex);

                Buff Accuracy = Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Accuracy);
                if (Accuracy.Eff == Features.SkillsClass.ExtraEffect.Accuracy)
                    _Agi = (short)(_Agi * Accuracy.Value);
                Buff SM = Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Superman);
                if (SM.StEff == StatusEffectEn.SuperMan)
                    _Agi *= 2;
                /*double MissValue = Rnd.Next(Dodge - 10, _Agi + Dodge + 20);
                Console.WriteLine("Agi: " + _Agi);
                Console.WriteLine("Dodge: " + Dodge);
                Console.WriteLine("MissValue: " + MissValue);
                if (MissValue <= Dodge && !Poison)
                    Damage = 0;*/
                double HitValue = _Agi - 15;
                if (HitValue < 0)
                    HitValue = 0;
                if (Dodge > HitValue)
                {
                    HitValue = Rnd.Next((int)Dodge - 20, (int)Dodge + 55 + _Agi);
                }
                else if (Dodge == HitValue)
                    HitValue = Rnd.Next((int)HitValue, (int)HitValue + 2);
                if (HitValue <= Dodge && !Poison)
                    Damage = 0;
                else
                {
                    if (LevDifDmg) Damage = (uint)(Damage * MyMath.LevelDifference(Attacker.Level, Level));
                }
            }
            if (!IsSkill && Damage != 0)
            {
                if (AT == AttackType.Melee)
                {
                    if (!Poison)
                        if (Defense >= Damage)
                            Damage = 1;
                        else
                            Damage -= Defense;

                    Damage += Attacker.EqStats.MeleeDamageIncrease;
                }
                else if (AT == AttackType.Ranged)
                {
                    Damage = (uint)((double)Damage * ((double)(155 - Dodge) / 100));
                    Damage += Attacker.EqStats.MeleeDamageIncrease;
                }
                else
                {
                    if (MDef >= Damage)
                        Damage = 1;
                    else
                        Damage -= MDef;

                    Damage += Attacker.EqStats.MagicDamageIncrease;
                }
                Damage = (uint)(Damage / DmgReduceTimes);
            }
            uint Exp = 0;
            if (Damage < CurrentHP)
            {
                CurrentHP -= Damage;
                if (MobID == 4152)
                {
                    if (World.DragonDamage.ContainsKey(Attacker.EntityID))
                    {
                        Attacker.DragonDamage += Damage;
                    }
                    else
                    {
                        World.DragonDamage.Add(Attacker.EntityID, Attacker);
                        Attacker.DragonDamage += Damage;
                    }
                }
                else if (_UltimateBoss() && MobID != 4152)
                {
                    if (World.BossesDamage.ContainsKey(MobID))
                    {
                        if (World.BossesDamage[MobID].ContainsKey(Attacker.EntityID))
                            World.BossesDamage[MobID][Attacker.EntityID] += Damage;
                        else
                            World.BossesDamage[MobID].Add(Attacker.EntityID, Damage);
                    }
                }
                if (!IsSkill)
                {
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                }
                if (Gives)
                {
                    Exp = (uint)(Damage * e);
                    if (!IsSkill)
                    {
                        if (AT == AttackType.Ranged || AT == AttackType.Melee || AT == AttackType.FatalStrike)
                        {
                            uint DamageExpMob = Damage;
                            if (MobID == 150)
                                DamageExpMob *= 3;

                            if (Attacker.Equips.RightHand.ID != 0 && Attacker.Equips.LeftHand.ID != 0)
                            {
                                if ((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3) == (ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3))
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageExpMob + (uint)(DamageExpMob * 0.5));
                                else
                                {
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageExpMob);
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(DamageExpMob * 0.5));
                                }
                            }
                            else
                            {
                                if (Attacker.Equips.RightHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageExpMob);
                                if (Attacker.Equips.LeftHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(DamageExpMob * 0.5));
                                if (Attacker.Equips.RightHand.ID == 0 && Attacker.Equips.LeftHand.ID == 0)
                                    Attacker.AddProfExp((ushort)000, DamageExpMob);//damage /8
                            }
                            uint DamageBall = Damage;
                            if (MobID == 151)
                                DamageBall *= 3;

                            if (Attacker.Equips.RightHand.ID != 0 && Attacker.Equips.LeftHand.ID != 0)
                            {
                                if ((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3) == (ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3))
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageBall + (uint)(DamageBall * 0.5));
                                else
                                {
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageBall);
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(DamageBall * 0.5));
                                }
                            }
                            else
                            {
                                if (Attacker.Equips.RightHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), DamageBall);
                                if (Attacker.Equips.LeftHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(DamageBall * 0.5));
                                if (Attacker.Equips.RightHand.ID == 0 && Attacker.Equips.LeftHand.ID == 0)
                                    Attacker.AddProfExp((ushort)000, DamageBall);//damage /8
                            }


                        }
                    }
                }
            }
            else
            {
                #region Terato
                if (MobID == 4152)
                {
                    World.DragonTank = null;
                    if (World.DragonDamage.ContainsKey(Attacker.EntityID))
                    {
                        Attacker.DragonDamage += Damage;
                    }
                    else
                    {
                        World.DragonDamage.Add(Attacker.EntityID, Attacker);
                        Attacker.DragonDamage += Damage;
                    }
                    List<Character> Winners = new List<Character>();
                    List<Character> WinnersHeal = new List<Character>();
                    uint MaxDmg = 0;
                    uint MaxHeal = 0;
                    foreach (Character C in World.DragonDamage.Values)
                    {
                        if (C.DragonDamage > MaxDmg)
                        {
                            MaxDmg = C.DragonDamage;
                            if (Winners.Count > 0)
                                Winners.Clear();
                            Winners.Add(C);
                        }
                        else if (C.DragonDamage == MaxDmg)
                            Winners.Add(C);
                    }
                    foreach (Character C in World.DragonHeal.Values)
                    {

                        if (C.DragonHeal > MaxHeal)
                        {
                            MaxHeal = C.DragonHeal;
                            if (WinnersHeal.Count > 0)
                                WinnersHeal.Clear();
                            WinnersHeal.Add(C);
                        }
                        else if (C.DragonHeal == MaxHeal)
                            WinnersHeal.Add(C);
                    }
                    World.DragonDamage.Clear();
                    World.DragonHeal.Clear();
                    foreach (Character C in Winners)
                    {
                        uint Item;
                        List<uint> From = new List<uint>();
                        foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                From.Add(D.ID);

                        Item = (uint)From[Rnd.Next(0, From.Count)];
                        if (C.Inventory.Count < 40)
                            C.AddItem(Item);
                        else Game.World.DebugAdd += C.Name + " didn't get prize from TeratoDragon: " + Item + "\r\n";
                        World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest damage of: " + C.DragonDamage, 2005, 0);
                        World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest damage of: " + C.DragonDamage, 2000, 0);
                    }
                    foreach (Character C in WinnersHeal)
                    {

                        uint Item;
                        List<uint> From = new List<uint>();
                        foreach (DatabaseItem D in Database.DatabaseItems.Values)
                        {

                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                From.Add(D.ID);
                        }
                        Item = (uint)From[Rnd.Next(0, From.Count)];
                        if (C.Inventory.Count < 40)
                            C.AddItem(Item);
                        else Game.World.DebugAdd += C.Name + " didn't get prize from TeratoDragon: " + Item + "\r\n";
                        World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest heal of: " + C.DragonHeal, 2005, 0);
                        World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest heal of: " + C.DragonHeal, 2000, 0);
                    }
                    Winners.Clear();
                    WinnersHeal.Clear();
                }
                #endregion
                #region Bosses
                else if (_UltimateBoss())
                {
                    if (MobID == 3821 || MobID == 3822 || MobID == 3823 || MobID == 4172)
                    {
                        if (World.BossesDamage.ContainsKey(MobID))
                        {
                            if (MyMath.ChanceSuccess(50))
                            {
                                Dictionary<Character, uint> Winner = new Dictionary<Character, uint>();
                                foreach (uint UID in World.BossesDamage[MobID].Keys)
                                    if (World.H_Chars.ContainsKey(UID))
                                        Winner.Add(World.H_Chars[UID], World.BossesDamage[MobID][UID]);

                                uint Item;
                                List<uint> From = new List<uint>();
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 183 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                        From.Add(D.ID);

                                Item = (uint)From[Rnd.Next(0, From.Count)];
                                if (Winner.OrderByDescending(s => s.Value).First().Key.Inventory.Count < 40)
                                    Winner.OrderByDescending(s => s.Value).First().Key.AddItem(Item);

                                else Game.World.DebugAdd += Winner.OrderByDescending(s => s.Value).First().Key.Name + " didn't get prize from TeratoDragon: " + Item + "\r\n";
                                World.SendMsgToAll("SYSTEM", Winner.OrderByDescending(s => s.Value).First().Key.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at " + Name + " with highest damage of: " + Winner.OrderByDescending(s => s.Value).First().Value, 2005, 0);
                                World.SendMsgToAll("SYSTEM", Winner.OrderByDescending(s => s.Value).First().Key.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at " + Name + " with highest damage of: " + Winner.OrderByDescending(s => s.Value).First().Value, 2000, 0);
                            }
                            World.BossesDamage[MobID].Clear();
                        }
                    }
                }
                #endregion
                #region DisCity
                if (World.PlutoKilled && (Attacker.Loc.Map == 2021 || Attacker.Loc.Map == 2022 || Attacker.Loc.Map == 2023))
                {
                    Attacker.Teleport(1020, 566, 564);
                    Attacker.MyClient.LocalMessage(2011, "Dis City has been won by somebody. Better luck next time!");
                }
                if (Attacker.Loc.Map == 2022)
                {
                    if (MobID == 402)
                    {
                        Attacker.DisCityMobs += 3;
                        Attacker.MyClient.LocalMessage(2005, "You have killed " + Attacker.DisCityMobs + " monsters out of " + Attacker.DisToKill + "! Hurry up, only the first 30 players can enter the next stage!");
                    }
                    else
                    {
                        Attacker.DisCityMobs++;
                        Attacker.MyClient.LocalMessage(2005, "You have killed " + Attacker.DisCityMobs + " monsters out of " + Attacker.DisToKill + "! Hurry up, only the first 30 players can enter the next stage!");
                    }
                }
                if (Attacker.Loc.Map == 2023)
                {
                    if (World.H_RightFlank.ContainsKey(Attacker.EntityID))
                        World.RightKills++;
                    else World.LeftKills++;
                    //(World.H_LeftFlank.Contains(Attacker.EntityID))
                }
                if (World.RightKills > 0)
                {
                    foreach (Character C in World.H_RightFlank.Values)
                    {
                        if (C != null && C.Loc.Map == 2023)
                        {
                            C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------Third Stage---------", 0x83c, 0));
                            C.MyClient.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", $"Right Flank: " + (600 - World.RightKills) + " kills left", 0x83d, 0));
                            C.MyClient.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", $"Left Flank: " + (600 - World.LeftKills) + " kills left", 0x83d, 0));
                        }
                    }
                }
                else if (World.LeftKills > 0)
                {
                    foreach (Character C in World.H_LeftFlank.Values)
                    {
                        if (C != null && C.Loc.Map == 2023)
                        {
                            C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------Third Stage---------", 0x83c, 0));
                            C.MyClient.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", $"Left Flank: " + (600 - World.LeftKills) + " kills left", 0x83d, 0));
                            C.MyClient.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", $"Right Flank: " + (600 - World.RightKills) + " kills left", 0x83d, 0));
                        }
                    }
                }
                if (World.LeftKills >= 600)
                {
                    foreach (Character C in World.H_Chars.Values)
                    {
                        if (C.Loc.Map == 2021 || C.Loc.Map == 2022)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "There was somebody who reached stage 4 already. Better luck next time!");
                            C.AtkMem.Target = 0;
                            C.AtkMem.Attacking = false;
                        }
                    }
                    foreach (Character C in World.H_LeftFlank.Values)
                    {
                        if (C != null)
                        {
                            if (C.Inventory.Count < 39)
                                C.AddItem(1088000);
                            else
                                C.MyClient.LocalMessage(2000, "Your inventory was full and so you didn't receive the Dragonball for reaching this stage!");
                            C.Teleport(2024, 150, 284);
                            C.MyClient.LocalMessage(2011, "Congratulations your flank advanced to the final stage!");
                        }
                    }
                    foreach (Character C in World.H_RightFlank.Values)
                    {
                        if (C != null)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "I'm sorry but the other flank killed 600 monsters before you. Better luck next time!");
                            C.AtkMem.Target = 0;
                            C.AtkMem.Attacking = false;
                        }
                    }
                    World.LeftKills = 0;
                    World.RightKills = 0;
                }
                else if (World.RightKills >= 600)
                {
                    foreach (Character C in World.H_Chars.Values)
                    {
                        if (C.Loc.Map == 2021 || C.Loc.Map == 2022)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "There was somebody who reached stage 4 already. Better luck next time!");
                            C.AtkMem.Target = 0;
                            C.AtkMem.Attacking = false;
                        }
                    }
                    foreach (Character C in World.H_LeftFlank.Values)
                    {
                        if (C != null)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "I'm sorry but the other flank killed 600 monsters before you. Better luck next time!");
                            C.AtkMem.Target = 0;
                            C.AtkMem.Attacking = false;
                        }
                    }
                    foreach (Character C in World.H_RightFlank.Values)
                    {
                        if (C != null)
                        {
                            if (C.Inventory.Count < 39)
                                C.AddItem(1088000);
                            else
                                C.MyClient.LocalMessage(2000, "Your inventory was full and so you didn't receive the Dragonball for reaching this stage!");
                            C.Teleport(2024, 150, 284);
                            C.MyClient.LocalMessage(2011, "Congratulations your flank advanced to the final stage!");
                        }
                    }
                    World.LeftKills = 0;
                    World.RightKills = 0;
                }
                if (MobID == 700)
                    if (World.Syrens > 0)
                        World.Syrens--;
                if (MobID == 701)
                {
                    World.PlutoKilled = true;
                    World.DebugAdd += Attacker.Name + " has killed UltimatePluto \r\n";
                    World.SendMsgToAll("DisCity", Attacker.Name + " has defeated the UltimatePluto and obtained the DarkHorn!", 2011, 0);
                }
                #endregion
                #region TreasureHunt
                if (MobID == 702)
                {
                    Attacker.TreasurePoints++;
                    Attacker.MyClient.LocalMessage(2005, "You gained 1 Treasure Point!");
                }
                else if (MobID == 703)
                {
                    if (MyMath.ChanceSuccess(70))
                    {
                        if (Attacker.TreasurePoints > 2)
                            Attacker.TreasurePoints -= 2;
                        else
                            Attacker.TreasurePoints = 0;
                        Attacker.MyClient.LocalMessage(2005, "You lost 2 Treasure Point!");
                    }
                    else if (MyMath.ChanceSuccess(20))
                    {
                        Attacker.TreasurePoints += 3;
                        Attacker.MyClient.LocalMessage(2005, "You gained 3 Treasure Point!");
                    }
                    else
                    {
                        Attacker.TreasurePoints += 1;
                        Attacker.MyClient.LocalMessage(2005, "You gained 1 Treasure Point!");
                    }
                }
                else if (MobID == 704)
                {
                    if (MyMath.ChanceSuccess(25))
                    {
                        if (Attacker.TreasurePoints > 2)
                            Attacker.TreasurePoints -= 2;
                        else
                            Attacker.TreasurePoints = 0;
                        Attacker.MyClient.LocalMessage(2005, "You lost 2 Treasure Point!");
                    }
                    else if (MyMath.ChanceSuccess(20))
                    {
                        Attacker.TreasurePoints += 5;
                        Attacker.MyClient.LocalMessage(2005, "You gained 5 Treasure Point!");
                    }
                    else
                    {
                        Attacker.TreasurePoints += 2;
                        Attacker.MyClient.LocalMessage(2005, "You gained 2 Treasure Point!");
                    }
                }
                #endregion
                Attacker.XPKO++;
                if (Attacker.Superman || Attacker.Cyclone)
                    Attacker.TotalKO++;
                if (Attacker.TotalKills < 100000)
                    Attacker.TotalKills++;
                PlayerTarget = null;
                Alive = false;
                uint Benefit = CurrentHP;
                CurrentHP = 0;
                PoisonedInfo = null;
                Died = DateTime.Now;
                if (!IsSkill)
                {
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                }
                if (Attacker.Superman || Attacker.Cyclone)
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, (uint)(65536 * Attacker.TotalKO), (byte)AttackType.Kill).Get);
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, (uint)(1), (byte)AttackType.Kill).Get);

                World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                if (EntityID == Attacker.AtkMem.Target)
                {
                    Attacker.AtkMem.Attacking = false;
                    Attacker.AtkMem.Target = 0;
                }

                if (Gives && (!Attacker.MyClient.GM || Attacker.MyClient.PM))
                {
                    Exp = (uint)(Benefit * e);
                    if (Attacker.MyTeam != null)
                    {
                        //Character[] Chars = null;
                        //lock (Attacker.MyTeam.Members)
                        //{
                        //    Chars = new Character[Attacker.MyTeam.Members.Count];
                        //    Attacker.MyTeam.Members.CopyTo(Chars, 0);
                        //}

                        if (Attacker.MyTeam.Members != null)
                            foreach (Character C in Attacker.MyTeam.Members)
                            {
                                if (C.EntityID != Attacker.EntityID && C.Alive && MyMath.PointDistance(C.Loc.X, C.Loc.Y, Attacker.Loc.X, Attacker.Loc.Y) <= 36 && C.Loc.Map == Attacker.Loc.Map)
                                {
                                    byte Lev = C.Level;
                                    if (C.Level + 20 > Level)
                                    {
                                        uint Amount;
                                        if (C.Level + 5 > Level)
                                        {
                                            Amount = (uint)Math.Floor((double)MaxHP / 20);
                                        }
                                        else if (C.Level + 10 > Level)
                                        {
                                            Amount = (uint)Math.Floor(Math.Floor((double)MaxHP / 20) * 1.2);
                                        }
                                        else
                                        {
                                            Amount = (uint)Math.Floor(Math.Floor((double)MaxHP / 20) * 1.3);
                                        }

                                        C.IncreaseExp(Amount, true, true, Attacker, Level);
                                        /* if (C.Level >= 110)
                                             C.IncreaseExp(MaxHP / 18, true, true, Attacker);
                                         else C.IncreaseExp(MaxHP / 12, true, true, Attacker);*/
                                    }
                                    else
                                    {
                                        //uint Amount = (uint)(156 + (C.Level * 20));
                                        uint Amount = (uint)(C.Level * 30);
                                        /*  if (C.Level >= 110)
                                              Amount = (uint)(Amount *0.9);*/
                                        C.IncreaseExp(Amount, true, true, Attacker, Level);
                                    }
                                    /*  */
                                }
                                /*  if (C != Attacker && C.Alive && MyMath.InBox(C.Loc.X, C.Loc.Y, Attacker.Loc.X, Attacker.Loc.Y, 28) && C.Loc.Map == Attacker.Loc.Map)
                                  {
                                      if (C.Level + 20 > Attacker.Level)
                                      {
                                          uint Amount = (uint)(156 + (C.Level * 20));

                                          if (C.Level >= 120)
                                              Amount = (uint)(Amount / 1.5);
                                          C.IncreaseExp(Amount, true, true);

                                      }
                                      else
                                      {
                                          byte Lev = C.Level;
                                          if (C.Level >= 120)
                                              C.IncreaseExp(MaxHP / 20, true, true);
                                          else C.IncreaseExp(MaxHP / 12, true, true);
                                         // C.IncreaseExp(Amount, true, true);

                                          if (Attacker.MyTeam.Leader.Level >= 70 && C.Level <= 70)
                                          {
                                              for (; Lev < C.Level; Lev++)
                                              {
                                                  if (Attacker.MyTeam.Leader.Loc.Map == Attacker.Loc.Map && MyMath.InBox(Attacker.MyTeam.Leader.Loc.X, Attacker.MyTeam.Leader.Loc.Y, Attacker.Loc.X, Attacker.Loc.Y, 28))
                                                  {
                                                      uint VPAmount = (uint)Math.Max(1, Lev * 9 - 17);
                                                      Attacker.MyTeam.Leader.VP += VPAmount;
                                                      Attacker.MyTeam.Message(Packets.ChatMessage(45216, "SYSTEM", "ALL", Attacker.MyTeam.Leader.Name + " gained " + VPAmount + " virtue points.", 2003, 0));
                                                  }
                                              }
                                          }
                                      }
                                  }*/
                            }

                    }
                    if (MobID != 150)
                        Attacker.IncreaseExp(MaxHP / 10, false, true, Attacker);
                    if (!IsSkill)
                    {
                        if (AT == AttackType.Ranged || AT == AttackType.Melee || AT == AttackType.FatalStrike)
                        {
                            if (Attacker.Equips.RightHand.ID != 0 && Attacker.Equips.LeftHand.ID != 0)
                            {
                                if (Attacker.Equips.RightHand.ID / 1000 == Attacker.Equips.LeftHand.ID / 1000)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit + (uint)(Benefit * 0.5));
                                else
                                {
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit);
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(Benefit * 0.5));
                                }
                            }
                            else
                            {
                                if (Attacker.Equips.RightHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit);
                                if (Attacker.Equips.LeftHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(Benefit * 0.5));
                                if (Attacker.Equips.RightHand.ID == 0 && Attacker.Equips.LeftHand.ID == 0)
                                    Attacker.AddProfExp((ushort)000, Benefit);//damage /8
                            }
                        }
                    }
                    if (MobID != 151)
                        Attacker.IncreaseExp(MaxHP / 10, false, true, Attacker);
                    if (!IsSkill)
                    {
                        if (AT == AttackType.Ranged || AT == AttackType.Melee || AT == AttackType.FatalStrike)
                        {
                            if (Attacker.Equips.RightHand.ID != 0 && Attacker.Equips.LeftHand.ID != 0)
                            {
                                if (Attacker.Equips.RightHand.ID / 1000 == Attacker.Equips.LeftHand.ID / 1000)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit + (uint)(Benefit * 0.5));
                                else
                                {
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit);
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(Benefit * 0.5));
                                }
                            }
                            else
                            {
                                if (Attacker.Equips.RightHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), Benefit);
                                if (Attacker.Equips.LeftHand.ID != 0)
                                    Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), (uint)(Benefit * 0.5));
                                if (Attacker.Equips.RightHand.ID == 0 && Attacker.Equips.LeftHand.ID == 0)
                                    Attacker.AddProfExp((ushort)000, Benefit);//damage /8
                            }
                        }
                    }
                    DropAnItem(Attacker.EntityID, Attacker.Level);
                }
                else if (MobID == 500)
                    DropAnItem(Attacker.EntityID, Attacker.Level);

                if (Attacker.InventoryContains(750000, 1) && Cloudsaint.MonsterIDs(Attacker.ToKill).Contains(MobID))
                {
                    Attacker.CurrentKills++;
                    Attacker.MyClient.AddSend(Packets.UpdateCloudSaintJar(Attacker.EntityID, (byte)Attacker.ToKill, Attacker.CurrentKills));
                    //Attacker.MyClient.LocalMessage(2000, $"Kill Count: {Attacker.CurrentKills} Monster UID: {EntityID} Monster Location: {Loc.X},{Loc.Y}");
                    if (Attacker.CurrentKills >= Cloudsaint.SelectCount((byte)Attacker.ToKill))
                        Attacker.MyClient.LocalMessage(2005, "You have enough monster souls inside your jar! Please deliver it to the city Captain!");
                }
            }

            if (!IsSkill)
            {
                uint ExpExp = Exp;
                if (!World.LowRatedServer)
                {
                    if (MobID == 150)
                        ExpExp *= 6;
                }
                else
                {
                    if (MobID == 150)
                        ExpExp *= 3;
                }
                if (!World.LowRatedServer)
                {
                    if (MobID == 151)
                        ExpExp *= 6;
                }
                else
                {
                    if (MobID == 151)
                        ExpExp *= 3;
                }
                if (Damage < CurrentHP)
                    Attacker.IncreaseExp(ExpExp, false, false, Attacker);
                else
                    Attacker.IncreaseExp(ExpExp, false, true, Attacker);
            }
            return Exp;
        }
        public void UpdateKills()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE stats SET mobs=mobs+1");
                t.ExecuteUpdate();
            }
        }
        public void GetReflect(ref uint Damage, AttackType AT)
        {
            if (Damage > 6000)
                Damage = 6000;
            if (Damage < CurrentHP)
            {
                CurrentHP -= Damage;
                World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                Alive = false;
                uint Benefit = CurrentHP;
                CurrentHP = 0;
                PlayerTarget = null;
                Died = DateTime.Now;

                World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);

                World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                if (Gives)
                    DropAnItem(0, Level);
            }
        }
        public void TakeAttack(Mob Attacker, uint Damage, AttackType AT)
        {
            try
            {
                if (LevDifDmg) Damage = (uint)(Damage * MyMath.LevelDifference(Attacker.Level, Level));
                Damage = (uint)(Damage / DmgReduceTimes);
                if (AT == AttackType.Melee)
                {
                    if (Defense >= Damage)
                        Damage = 1;
                    else
                        Damage -= Defense;
                }
                else if (AT == AttackType.Ranged)
                    Damage = (uint)((double)Damage * ((double)Dodge / 100));
                else if (AT == AttackType.Magic)
                {
                    if (MDef >= Damage)
                        Damage = 1;
                    else
                        Damage -= MDef;
                }

                if (Damage < CurrentHP)
                {
                    CurrentHP -= Damage;
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                }
                else
                {
                    PoisonedInfo = null;
                    PlayerTarget = null;
                    Alive = false;
                    uint Benefit = CurrentHP;
                    CurrentHP = 0;
                    Died = DateTime.Now;
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                    World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                    if (Gives)
                        DropAnItem(0, Attacker.Level);
                }
            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public void TakeAttack(Companion Attacker, uint Damage, AttackType AT)
        {
            try
            {
                if (LevDifDmg) Damage = (uint)(Damage * MyMath.LevelDifference(Attacker.Level, Level));
                Damage = (uint)(Damage / DmgReduceTimes);
                if (AT == AttackType.Melee)
                {
                    if (Defense >= Damage)
                        Damage = 1;
                    else
                        Damage -= Defense;
                }
                else if (AT == AttackType.Ranged)
                    Damage = (uint)((double)Damage * ((double)Dodge / 100));
                else if (AT == AttackType.Magic)
                {
                    if (MDef >= Damage)
                        Damage = 1;
                    else
                        Damage -= MDef;
                }

                if (Damage < CurrentHP)
                {
                    CurrentHP -= Damage;
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);

                    if (Gives)
                        Attacker.Owner.IncreaseExp(Damage, false, false, Attacker.Owner);
                }
                else
                {
                    #region Terato
                    if (MobID == 4152)
                    {
                        World.DragonTank = null;
                        /*  if (World.DragonDamage.Contains(Attacker.Owner.EntityID))
                          {
                              Attacker.Owner.DragonDamage += Damage;
                          }
                          else
                          {
                              World.DragonDamage.Add(Attacker.Owner.EntityID, Attacker);
                              Attacker.Owner.DragonDamage += Damage;
                          }*/
                        List<Character> Winners = new List<Character>();
                        List<Character> WinnersHeal = new List<Character>();
                        uint MaxDmg = 0;
                        uint MaxHeal = 0;
                        foreach (Character C in World.DragonDamage.Values)
                        {
                            if (C.DragonDamage > MaxDmg)
                            {
                                MaxDmg = C.DragonDamage;
                                if (Winners.Count > 0)
                                    Winners.Clear();
                                Winners.Add(C);
                            }
                            else if (C.DragonDamage == MaxDmg)
                                Winners.Add(C);
                        }
                        foreach (Character C in World.DragonHeal.Values)
                        {

                            if (C.DragonHeal > MaxHeal)
                            {
                                MaxHeal = C.DragonHeal;
                                if (WinnersHeal.Count > 0)
                                    WinnersHeal.Clear();
                                WinnersHeal.Add(C);
                            }
                            else if (C.DragonHeal == MaxHeal)
                                WinnersHeal.Add(C);
                        }
                        World.DragonDamage.Clear();
                        World.DragonHeal.Clear();
                        foreach (Character C in Winners)
                        {

                            uint Item;
                            /*    byte x = (byte)Rnd.Next(0, 2);
                               byte Multiplier;
                               if (x == 0)
                               {
                                   Item = (uint)Rnd.Next(1813, 1820);
                                   Multiplier = (byte)Rnd.Next(0, 10);
                               }
                               else
                               {
                                   Item = (uint)Rnd.Next(1823, 1830);
                                   Multiplier = (byte)Rnd.Next(0, 9);
                               } 
                              Item = (uint)(Item * 100 + (Multiplier * 10) + 5);
                             */
                            List<uint> From = new List<uint>();
                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {

                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                    From.Add(D.ID);
                            }
                            Item = (uint)From[Rnd.Next(0, From.Count)];
                            if (C.Inventory.Count < 40)
                                C.AddItem(Item);
                            else Game.World.DebugAdd += C.Name + " didn't get prize from TeratoDragon: " + Item + "\r\n";
                            World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest damage of: " + C.DragonDamage, 2005, 0);
                            World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest damage of: " + C.DragonDamage, 2000, 0);
                        }
                        foreach (Character C in WinnersHeal)
                        {

                            uint Item;
                            /* byte x = (byte)Rnd.Next(0, 2);
                             byte Multiplier;
                             if (x == 0)
                             {
                                 Item = (uint)Rnd.Next(1813, 1820);
                                 Multiplier = (byte)Rnd.Next(0, 10);
                             }
                             else
                             {
                                 Item = (uint)Rnd.Next(1823, 1830);
                                 Multiplier = (byte)Rnd.Next(0, 9);
                             }

                             Item = (uint)(Item * 100 + (Multiplier * 10) + 5);*/
                            List<uint> From = new List<uint>();
                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {

                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                    From.Add(D.ID);
                            }
                            Item = (uint)From[Rnd.Next(0, From.Count)];
                            if (C.Inventory.Count < 40)
                                C.AddItem(Item);
                            else Game.World.DebugAdd += C.Name + " didn't get prize from TeratoDragon: " + Item + "\r\n";
                            World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest heal of: " + C.DragonHeal, 2005, 0);
                            World.SendMsgToAll("SYSTEM", C.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at TeratoDragon with highest heal of: " + C.DragonHeal, 2000, 0);
                        }
                        Winners.Clear();
                        WinnersHeal.Clear();

                    }
                    #endregion
                    #region Bosses
                    else if (_UltimateBoss())
                    {
                        if (MobID == 3821 || MobID == 3822 || MobID == 3823 || MobID == 4172)
                        {
                            if (World.BossesDamage.ContainsKey(MobID))
                            {
                                if (MyMath.ChanceSuccess(30))
                                {
                                    Dictionary<Character, uint> Winner = new Dictionary<Character, uint>();
                                    foreach (uint UID in World.BossesDamage[MobID].Keys)
                                        if (World.H_Chars.ContainsKey(UID))
                                            Winner.Add(World.H_Chars[UID], World.BossesDamage[MobID][UID]);

                                    uint Item;
                                    List<uint> From = new List<uint>();
                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                            From.Add(D.ID);

                                    Item = (uint)From[Rnd.Next(0, From.Count)];
                                    if (Winner.OrderByDescending(s => s.Value).First().Key.Inventory.Count < 40)
                                        Winner.OrderByDescending(s => s.Value).First().Key.AddItem(Item);

                                    else Game.World.DebugAdd += Winner.OrderByDescending(s => s.Value).First().Key.Name + " didn't get prize from " + Name + ": " + Item + "\r\n";
                                    World.SendMsgToAll("SYSTEM", Winner.OrderByDescending(s => s.Value).First().Key.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at " + Name + " with highest damage of: " + Winner.OrderByDescending(s => s.Value).First().Value, 2005, 0);
                                    World.SendMsgToAll("SYSTEM", Winner.OrderByDescending(s => s.Value).First().Key.Name + " won " + ((DatabaseItem)Database.DatabaseItems[Item]).Name + " at " + Name + " with highest damage of: " + Winner.OrderByDescending(s => s.Value).First().Value, 2000, 0);

                                }
                                World.BossesDamage[MobID].Clear();
                            }
                        }
                    }
                    #endregion
                    #region DisCity
                    if (World.PlutoKilled && (Attacker.Owner.Loc.Map == 2021 || Attacker.Owner.Loc.Map == 2022 || Attacker.Owner.Loc.Map == 2023))
                    {
                        Attacker.Owner.Teleport(1020, 566, 564);
                        Attacker.Owner.MyClient.LocalMessage(2011, "Dis City has been won by somebody. Better luck next time!");
                    }
                    if (Attacker.Owner.Loc.Map == 2022)
                    {
                        if (MobID == 402)
                            Attacker.Owner.DisCityMobs += 3;
                        else Attacker.Owner.DisCityMobs++;
                    }
                    if (Attacker.Loc.Map == 2023)
                    {
                        if (World.H_RightFlank.ContainsKey(Attacker.Owner.EntityID))
                            World.RightKills++;
                        else if (World.H_LeftFlank.ContainsKey(Attacker.Owner.EntityID))
                            World.LeftKills++; //
                    }
                    if (World.LeftKills >= 600)
                    {
                        foreach (Character C in World.H_Chars.Values)
                        {
                            if (C.Loc.Map == 2021 || C.Loc.Map == 2022)
                            {
                                C.Teleport(1020, 566, 564);
                                C.MyClient.LocalMessage(2011, "There was somebody who reached stage 4 already. Better luck next time!");
                                C.AtkMem.Target = 0;
                                C.AtkMem.Attacking = false;
                            }
                        }
                        foreach (Character C in World.H_LeftFlank.Values)
                        {
                            if (C != null)
                            {
                                C.Teleport(2024, 150, 284);
                                C.MyClient.LocalMessage(2011, "Congratulations your flank advanced to the final stage!");
                            }
                        }
                        foreach (Character C in World.H_RightFlank.Values)
                        {
                            if (C != null)
                            {
                                C.Teleport(1020, 566, 564);
                                C.MyClient.LocalMessage(2011, "I'm sorry but the other flank killed 600 monsters before you. Better luck next time!");
                                C.AtkMem.Target = 0;
                                C.AtkMem.Attacking = false;
                            }
                        }
                        World.LeftKills = 0;
                        World.RightKills = 0;
                    }
                    if (World.RightKills >= 600)
                    {
                        foreach (Character C in World.H_Chars.Values)
                        {
                            if (C.Loc.Map == 2021 || C.Loc.Map == 2022)
                            {
                                C.Teleport(1020, 566, 564);
                                C.MyClient.LocalMessage(2011, "There was somebody who reached stage 4 already. Better luck next time!");
                                C.AtkMem.Target = 0;
                                C.AtkMem.Attacking = false;
                            }
                        }
                        foreach (Character C in World.H_LeftFlank.Values)
                        {
                            if (C != null)
                            {
                                C.Teleport(1020, 566, 564);
                                C.MyClient.LocalMessage(2011, "I'm sorry but the other flank killed 600 monsters before you. Better luck next time!");
                                C.AtkMem.Target = 0;
                                C.AtkMem.Attacking = false;
                            }
                        }
                        foreach (Character C in World.H_RightFlank.Values)
                        {
                            if (C != null)
                            {
                                C.Teleport(2024, 150, 284);
                                C.MyClient.LocalMessage(2011, "Congratulations your flank advanced to the final stage!");
                            }
                        }
                        World.LeftKills = 0;
                        World.RightKills = 0;
                    }
                    if (MobID == 700)
                        if (World.Syrens > 0)
                            World.Syrens--;
                    if (MobID == 701)
                    {
                        World.PlutoKilled = true;
                        World.DebugAdd += Attacker.Owner.Name + " has killed UltimatePluto \r\n";
                        World.SendMsgToAll("DisCity", Attacker.Owner.Name + " has defeated the UltimatePluto and obtained the DarkHorn!", 2011, 0);
                    }
                    #endregion
                    PoisonedInfo = null;
                    PlayerTarget = null;
                    Alive = false;
                    uint Benefit = CurrentHP;
                    CurrentHP = 0;
                    Died = DateTime.Now;
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                    World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                    if (Gives && (!Attacker.Owner.MyClient.GM || Attacker.Owner.MyClient.PM))
                    {
                        DropAnItem(Attacker.Owner.EntityID, Attacker.Level);
                        Attacker.Owner.IncreaseExp(Benefit, false, true, Attacker.Owner);
                    }
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        bool IsBoss()
        {
            if (MobID >= 107 && MobID <= 121 || MobID == 56)
                return true;
            return false;
        }
        public bool _UltimateBoss()
        {
            if (MobID >= 3000 && MobID <= 4500)
                return true;
            return false;
        }

        public void DropAnItem(uint Owner, byte OwnerLevel)//unde verifica slotu, adik ring/neck
        {
            try
            {
                if (!Dropped)
                {
                    Dropped = true;
                    if (!World.H_Items.ContainsKey(Loc.Map))
                        World.H_Items.TryAdd(Loc.Map, new ConcurrentDictionary<uint, DroppedItem>());

                    if (DropRates.Specifics.ContainsKey(MobID))
                    {
                        //List<Item> Arr = (List<Item>)DropRates.Specifics[MobID];
                        List<DropRates.RateItemInfo> Arr = DropRates.Specifics[MobID];
                        if (Arr != null)
                        {
                            foreach (DropRates.RateItemInfo R in Arr)
                            {
                                if (MyMath.ChanceSuccess(R.DropChance))
                                {
                                    DroppedItem DI = new DroppedItem();
                                    DI.DropTime = DateTime.Now;
                                    DI.UID = (uint)Rnd.Next(10000000);
                                    DI.Loc = new Location();
                                    DI.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                                    DI.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                                    DI.Loc.Map = Loc.Map;
                                    DI.Info = new Item();
                                    DI.Info.ID = R.ID;
                                    DI.Info.UID = (uint)Rnd.Next(10000000);
                                    DI.Info.Plus = R.Plus;
                                    DI.Info.Bless = R.Bless;
                                    if (R.Sockets >= 1)
                                        DI.Info.Soc1 = Item.Gem.EmptySocket;
                                    if (R.Sockets >= 2)
                                        DI.Info.Soc2 = Item.Gem.EmptySocket;
                                    try
                                    {
                                        DI.Info.MaxDur = DI.Info.DBInfo.Durability;
                                        DI.Info.CurDur = DI.Info.MaxDur;
                                    }
                                    catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
                                    DI.Owner = Owner;
                                    if (!DI.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI.Drop();
                                }
                            }
                        }
                    }

                    Game.Character Char = null;
                    if (Game.World.H_Chars.ContainsKey(Owner))
                        Char = (Character)Game.World.H_Chars[Owner];
                    else if (Game.World.H_Companions.ContainsKey(Owner))
                        Char = ((Companion)World.H_Companions[Owner]).Owner;

                    DroppedItem DI2 = new DroppedItem();
                    DI2.DropTime = DateTime.Now;
                    DI2.UID = (uint)Rnd.Next(10000000);
                    DI2.Loc = new Location();
                    DI2.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                    DI2.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                    DI2.Loc.Map = Loc.Map;
                    DI2.Info = new Item();
                    DI2.Info.UID = (uint)Rnd.Next(10000000);
                    DI2.Owner = Owner;

                    Game.MapEffect DI3 = new Game.MapEffect();
                    DI3.DropTime = DateTime.Now;
                    DI3.UID = (uint)Rnd.Next(10000000);
                    DI3.Loc = new Location();
                    DI3.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                    DI3.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
                    DI3.Loc.Map = Loc.Map;
                    DI3.Info = new Game.MEffect();
                    DI3.Info.UID = (uint)Rnd.Next(10000000);
                    DI3.Owner = Owner;

                    double ExpChances = 0;
                    double NoobRate = 0;
                    double OnlineRate = World.H_Chars.Count / 1000;
                    if (Char != null && Char.MyClient != null)
                    {
                        Char.MyClient.MobsKilled++;
                    }
                    if (Char != null)
                    {

                        if (World.LowRatedServer)
                        {
                            if (Char.Job >= 40 && Char.Job <= 45)
                            {
                                ExpChances = Char.TotalKills * 0.000001;
                            }
                            else ExpChances = Char.TotalKills * 0.000008;
                            if (ExpChances > 0.1 || Char.Level < 70)
                                ExpChances = 0.1;
                        }
                        else
                        {
                            if (Char.Job >= 40 && Char.Job <= 45)
                            {
                                ExpChances = Char.TotalKills * 0.000005;
                            }
                            else ExpChances = Char.TotalKills * 0.00001;

                            if (ExpChances > 0.1 || Char.Level < 70)
                                ExpChances = 0.1;
                            if (Char.Level < 40 && !Char.Reborn)
                                NoobRate = 0.01;
                        }
                    }
                    #region Gold Drop
                    if (Loc.Map != 1214 && Loc.Map != 1210 && Loc.Map != 1211 && Loc.Map != 1212 && Loc.Map != 1215)
                    {
                        if (IsBoss() || MyMath.ChanceSuccess(DropRates.Silver + (ExpChances * 10)))
                        {
                            if (Loc.Map == 1300 && MyMath.ChanceSuccess(50))
                                return;
                            double x = Rnd.Next(200, 650) / 100; // multiplier factor
                            int t = Rnd.Next(1, 100);
                            int n;    // drop count

                            if (t <= 50) { n = 1; }
                            else if (t <= 75) { n = 2; }
                            else if (t <= 89) { n = 3; }
                            else if (t <= 96) { n = 4; }
                            else if (t <= 99) { n = 5; }
                            else { n = 6; }
                            if (Name.Length > 4)
                            {
                                // string newname = Name.Remove(0, Name.Length - 4);
                                if (IsBoss())
                                {
                                    x = Rnd.Next(400, 650) / 100;
                                    n = Math.Max(4, n);
                                }
                            }

                            double y;  // drop value
                            /*   for (int i = 0; i < n; i++)
                               {

                               }*/
                            // int DropTimes = Rnd.Next(1, 4); /// Silver drop times per kill.
                            for (int i = 0; i < n; i++)
                            {
                                bool Drop2 = true;
                                DI2 = Drop(Owner);
                                //y = Rnd.Next(x - 1, x + 1);
                                //  y = Rnd.Next((int)(100 * (x - 1)), (int)(100 * (x + 1))) / 100;
                                if (World.LowRatedServer)
                                {
                                    y = (double)Rnd.Next((int)(100 * (x - 1)), (int)(100 * (x + 1))) / 100;
                                    DI2.Silvers = (uint)(y * Level * DropRates.SilverDrop); // L = monster lvl
                                    if (MinSilvers == 0 && MaxSilvers == 0)
                                        DI2.Silvers = 0;
                                }
                                else
                                {
                                    DI2.Silvers = (uint)(Rnd.Next(MinSilvers, MaxSilvers) * DropRates.SilverDrop);
                                    //if (ServerTime.Month == 3 && ServerTime.Day < 18)
                                    //    DI2.Silvers = (uint)(DI2.Silvers * (2.3 - (Level / (double)100)));
                                    if (Loc.Map == 1351 || Loc.Map == 1352 || Loc.Map == 1353 || Loc.Map == 1354)
                                        DI2.Silvers /= 2;
                                }
                                DI2.Silvers = (DI2.Silvers > 25000) ? 25000 : DI2.Silvers;
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

                                if (Char != null)
                                {

                                    if (Char.VipLevel > 0 && Char.VipLevel <= 6)
                                    {
                                        double pc = 100;
                                        if (World.LowRatedServer)
                                        {
                                            if (Char.Job >= 40 && Char.Job <= 45)
                                                pc = 37;
                                            else pc = 67;
                                        }
                                        else
                                        {
                                            if (Char.Job >= 40 && Char.Job <= 45)
                                                pc = 75;
                                            else pc = 90;
                                        }
                                        if (MyMath.ChanceSuccess(pc))
                                        {
                                            if (Char.VipLevel >= 4)
                                            {
                                                if (DI2.Silvers > 0 && DI2.Silvers <= 700)
                                                {
                                                    //Char.MyClient.LocalMessage(2005, "You received " + DI2.Silvers + " gold from the drops.");
                                                    if (!World.GoldSource.ContainsKey(Name + " VIP"))
                                                        World.GoldSource.Add(Name + " VIP", 0);
                                                    World.GoldSource[Name + " VIP"] += DI2.Silvers;
                                                    Char.Silvers += DI2.Silvers;
                                                    Drop2 = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (Drop2)
                                {

                                    if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                                        return;
                                    if (DI2.Silvers > 0)
                                        DI2.Drop();

                                    if (!World.GoldSource.ContainsKey(Name))
                                        World.GoldSource.Add(Name, 0);
                                    World.GoldSource[Name] += DI2.Silvers;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (MyMath.ChanceSuccess(0.1))
                        {
                            // bool Drop2 = true;
                            DI2 = Drop(Owner);
                            DI2.Silvers = (uint)Rnd.Next(2000, 14000);
                            DI2.Silvers = (DI2.Silvers > 14000) ? 14000 : DI2.Silvers;
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


                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            if (DI2.Silvers > 0)
                                DI2.Drop();
                            if (!World.GoldSource.ContainsKey(Name))
                                World.GoldSource.Add(Name, 0);
                            World.GoldSource[Name] += DI2.Silvers;
                            // }
                        }
                    }
                    #endregion
                    #region DragonBall
                    if (DropRates.DragonBall != 0)
                    {
                        bool Drop2 = true;
                        double i = 0;
                        if (Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1215 || Loc.Map == 1211 || Loc.Map == 1212)
                            i += 0.0002;
                        if (Char != null && Char.VipLevel == 6)
                            i += DropRates.DragonBall * 0.05;
                        if (World.EventDB)
                            i += DropRates.DragonBall * 0.3;
                        if (World.DropEvent)
                            i += DropRates.DragonBall * 0.25;
                        if (Char != null && Char.LuckyTime > 0 && i > 0)
                            i *= 1.1;
                        if (World.DREvent > DateTime.Now)
                            i += DropRates.DragonBall * 0.2;

                        //if ((Char.MyGuild != null && CityWarTc.LastWinner != Char.MyGuild) && Char.Loc.Map != 1002)
                        //if (Features.CityWarTc.LastWinner == Char.MyGuild)
                        //if (CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                        if (Char.MyGuild != null && CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                            i += DropRates.DragonBall * 0.25;
                        else if (Char.MyGuild != null && CityWarPc.LastWinner == Char.MyGuild && Loc.Map == 1011)
                            i += DropRates.DragonBall * 0.25;
                        else if (Char.MyGuild != null && CityWarAc.LastWinner == Char.MyGuild && Loc.Map == 1020)
                            i += DropRates.DragonBall * 0.25;
                        else if (Char.MyGuild != null && CityWarDc.LastWinner == Char.MyGuild && Loc.Map == 1000)
                            i += DropRates.DragonBall * 0.25;
                        else if (Char.MyGuild != null && CityWarBi.LastWinner == Char.MyGuild && Loc.Map == 1015)
                            i += DropRates.DragonBall * 0.25;

                        if (Name.Length > 4)
                        {
                            //string newname = Name.Remove(0, Name.Length - 4);
                            if (IsBoss())
                            {
                                if (World.LowRatedServer)
                                    i += (DropRates.DragonBall * 120);
                                else i += (DropRates.DragonBall * 30);
                            }
                        }
                        if (MobID == 6056 || MobID == 6061 || MobID == 6064 || Loc.Map == 1070) // Gumparoo, Sfinxos, SarasMinion or GW hunters map
                        {
                            if (MyMath.ChanceSuccess(51)) // only roll for DB's 51% as often as normal
                            {
                                if (MyMath.ChanceSuccess(DropRates.DragonBall + (ExpChances / 400) + i + (NoobRate / 4)) || MobID == 409)
                                {
                                    if (Char != null)
                                    {
                                        if (Char.VipLevel == 5 || Char.VipLevel == 6)
                                        {
                                            if (Char.Inventory.Count < 40)
                                            {
                                                Char.AddItem(1088000);
                                                Char.MyClient.LocalMessage(2005, "You received a DragonBall from the drops.");
                                                World.SendMsgToAll("SYSTEM", "A DragonBall has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0/*, Loc.Map*/);
                                                Drop2 = false;
                                            }
                                        }
                                    }
                                    if (Drop2)
                                    {

                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = 1088000;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        if (Char != null)
                                        {
                                            World.SendMsgToAll("SYSTEM", "A DragonBall has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0/*, Loc.Map*/);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(DropRates.DragonBall + (ExpChances / 400) + i + (NoobRate / 5) + (OnlineRate / 10)) || MobID == 409)
                            {
                                if (Char != null)
                                {
                                    if (Char.VipLevel == 5 || Char.VipLevel == 6)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            Char.AddItem(1088000);
                                            Char.MyClient.LocalMessage(2005, "You received a DragonBall from the drops.");
                                            World.SendMsgToAll("SYSTEM", "A DragonBall has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0/*, Loc.Map*/);
                                            Drop2 = false;
                                        }
                                    }
                                }
                                if (Drop2)
                                {

                                    DI2 = Drop(Owner);
                                    DI2.Info.ID = 1088000;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;

                                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI2.Drop();
                                    if (Char != null)
                                    {
                                        World.SendMsgToAll("SYSTEM", "A DragonBall has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0/*, Loc.Map*/);
                                    }

                                }
                            }
                        }

                    }
                    #endregion
                    #region GreenEgg
                    if (DropRates.GreenEgg != 0)
                    {
                        Item Luck = new Item();
                        if (MyMath.ChanceSuccess(30))
                            Luck.ID = 711001;
                        else if (MyMath.ChanceSuccess(30))
                        {
                            Luck.ID = 711002;
                        }
                        else if (MyMath.ChanceSuccess(100))
                            Luck.ID = 711003;

                        bool Drop2 = true;
                        double i = 0;
                        if (Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1215 || Loc.Map == 1211 || Loc.Map == 1212)
                            i = 0.02;
                        if (Char != null && Char.VipLevel == 5)
                            i += DropRates.GreenEgg * 0.05;
                        if (Char != null && Char.Loc.Map != 1015)
                            i += DropRates.GreenEgg * 0.05;
                        if (Char.MyGuild != null && CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                            i += DropRates.GreenEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarPc.LastWinner == Char.MyGuild && Loc.Map == 1011)
                            i += DropRates.GreenEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarAc.LastWinner == Char.MyGuild && Loc.Map == 1020)
                            i += DropRates.GreenEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarDc.LastWinner == Char.MyGuild && Loc.Map == 1000)
                            i += DropRates.GreenEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarBi.LastWinner == Char.MyGuild && Loc.Map == 1015)
                            i += DropRates.GreenEgg * 0.25;


                        if (World.DropEvent)
                            i += DropRates.GreenEgg * 0.25;
                        if (Char.LuckyTime > 0 && i > 0)
                            i *= 0.2;
                        if (World.DREvent > DateTime.Now)
                            i += DropRates.GreenEgg * 0.2;

                        if (Name.Length > 4)
                        {
                            //string newname = Name.Remove(0, Name.Length - 4);
                            if (IsBoss())
                            {
                                i += (DropRates.GreenEgg * 30);
                            }
                        }
                        if (MobID == 6056 || MobID == 6061 || MobID == 6064 || Loc.Map == 1070) // Gumparoo, Sfinxos, SarasMinion and GW hunters map
                        {
                            if (MyMath.ChanceSuccess(51)) // only roll for met 51% of the time
                            {
                                if (MyMath.ChanceSuccess(DropRates.GreenEgg + i + (ExpChances / 2) + NoobRate))
                                {
                                    if (Drop2)
                                    {
                                        if (Char.VipLevel == 5)
                                        {
                                            if (!Char.skipgreenegg)
                                            {
                                                if (Char.Inventory.Count < 40)
                                                {
                                                    Char.AddItem(Luck);

                                                    Char.MyClient.LocalMessage(2001, "You dropped a Fruit, you can exchange it with a nice reward!");
                                                }

                                            }
                                            else
                                            {
                                                DI2 = Drop(Owner);
                                                DI2.Info.ID = (uint)Rnd.Next(711001, 711003);
                                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                                DI2.Drop();
                                                Char.MyClient.LocalMessage(2001, "You dropped a fruit, you can exchange it with a nice reward!");
                                            }

                                        }
                                        else
                                        {

                                            DI2 = Drop(Owner);
                                            DI2.Info.ID = (uint)Rnd.Next(711001, 711003);
                                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                            DI2.Info.CurDur = DI2.Info.MaxDur;
                                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                            DI2.Drop();
                                            //Context = new ConquerDataContext();
                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(DropRates.GreenEgg + i + (ExpChances / 2) + NoobRate + OnlineRate))
                            {
                                if (Drop2)
                                {
                                    if (Char.VipLevel == 5)
                                    {
                                        if (!Char.skipgreenegg)
                                        {
                                            if (Char.Inventory.Count < 40)
                                            {
                                                Char.AddItem(Luck);
                                                Char.MyClient.LocalMessage(2001, "You dropped a Fruit, you can exchange it with a nice reward!");
                                            }

                                        }
                                        else
                                        {
                                            DI2 = Drop(Owner);
                                            DI2.Info.ID = (uint)Rnd.Next(711001, 711003);
                                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                            DI2.Info.CurDur = DI2.Info.MaxDur;
                                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                            DI2.Drop();
                                            Char.MyClient.LocalMessage(2001, "You dropped a Fruit, you can exchange it with a nice reward!");
                                        }
                                    }
                                    else
                                    {
                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = (uint)Rnd.Next(711001, 711003);
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;
                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        //Context = new ConquerDataContext();
                                    }


                                }
                            }

                        }

                    }
                    #endregion
                    #region RedEgg
                    if (DropRates.RedEgg != 0)
                    {
                        Item Luck1 = new Item();
                        if (MyMath.ChanceSuccess(30))
                            Luck1.ID = 711004;
                        else if (MyMath.ChanceSuccess(30))
                        {
                            Luck1.ID = 711005;
                        }
                        else if (MyMath.ChanceSuccess(100))
                            Luck1.ID = 711005;
                        bool Drop2 = true;
                        double i = 0;
                        if (Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1215 || Loc.Map == 1211 || Loc.Map == 1212)
                            i = 0.02;
                        if (Char != null && Char.VipLevel == 5)
                            i += DropRates.RedEgg * 0.05;
                        if (Char != null && Char.Loc.Map != 1015)
                            i += DropRates.RedEgg * 0.05;
                        if (Char.MyGuild != null && CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                            i += DropRates.RedEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarPc.LastWinner == Char.MyGuild && Loc.Map == 1011)
                            i += DropRates.RedEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarAc.LastWinner == Char.MyGuild && Loc.Map == 1020)
                            i += DropRates.RedEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarDc.LastWinner == Char.MyGuild && Loc.Map == 1000)
                            i += DropRates.RedEgg * 0.25;
                        else if (Char.MyGuild != null && CityWarBi.LastWinner == Char.MyGuild && Loc.Map == 1015)
                            i += DropRates.RedEgg * 0.25;

                        if (World.DropEvent)
                            i += DropRates.RedEgg * 0.25;
                        if (Char.LuckyTime > 0 && i > 0)
                            i *= 0.2;
                        if (World.DREvent > DateTime.Now)
                            i += DropRates.RedEgg * 0.2;

                        if (Name.Length > 4)
                        {
                            //string newname = Name.Remove(0, Name.Length - 4);
                            if (IsBoss())
                            {
                                i += (DropRates.RedEgg * 30);
                            }
                        }
                        if (MobID == 6056 || MobID == 6061 || MobID == 6064 || Loc.Map == 1070) // Gumparoo, Sfinxos, SarasMinion and GW hunters map
                        {
                            if (MyMath.ChanceSuccess(51)) // only roll for met 51% of the time
                            {
                                if (MyMath.ChanceSuccess(DropRates.RedEgg + i + (ExpChances / 2) + NoobRate))
                                {
                                    if (Drop2)
                                    {
                                        if (Char.VipLevel == 5)
                                        {
                                            if (!Char.skipgreenegg)
                                            {
                                                if (Char.Inventory.Count < 40)
                                                {
                                                    Char.AddItem(Luck1);
                                                    Char.MyClient.LocalMessage(2001, "You dropped a Rare Fruit, you can exchange it with a nice reward!");
                                                }
                                            }
                                            else
                                            {
                                                DI2 = Drop(Owner);
                                                DI2.Info.ID = (uint)Rnd.Next(711004, 711006);
                                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                                DI2.Drop();
                                                Char.MyClient.LocalMessage(2001, "You dropped a Rare Fruit, you can exchange it with a nice reward!");
                                                //Context = new ConquerDataContext();
                                            }
                                        }
                                        else
                                        {
                                            DI2 = Drop(Owner);
                                            DI2.Info.ID = (uint)Rnd.Next(711004, 711006);
                                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                            DI2.Info.CurDur = DI2.Info.MaxDur;
                                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                            DI2.Drop();
                                            //Context = new ConquerDataContext();
                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(DropRates.RedEgg + i + (ExpChances / 2) + NoobRate + OnlineRate))
                            {
                                if (Drop2)
                                {
                                    if (Char.VipLevel == 5)
                                    {
                                        if (!Char.skipgreenegg)
                                        {
                                            if (Char.Inventory.Count < 40)
                                            {
                                                Char.AddItem(Luck1);
                                                Char.MyClient.LocalMessage(2001, "You dropped a Rare Fruit, you can exchange it with a nice reward!");
                                            }
                                        }
                                        else
                                        {
                                            DI2 = Drop(Owner);
                                            DI2.Info.ID = (uint)Rnd.Next(711004, 711006);
                                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                            DI2.Info.CurDur = DI2.Info.MaxDur;
                                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                            DI2.Drop();
                                            Char.MyClient.LocalMessage(2001, "You dropped a Rare Fruit, you can exchange it with a nice reward!");
                                            //Context = new ConquerDataContext();
                                        }
                                    }
                                    else
                                    {
                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = (uint)Rnd.Next(711004, 711006);
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;
                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        //Context = new ConquerDataContext();
                                    }


                                }
                            }

                        }

                    }
                    #endregion
                    #region EggPacket
                    if (DropRates.EggPacket != 0)
                    {
                        bool Drop2 = true;
                        double i = 0;
                        if (Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1215 || Loc.Map == 1211 || Loc.Map == 1212)
                            i += 0.0001;
                        if (Char != null && Char.VipLevel == 5)
                            i += DropRates.EggPacket * 0.05;
                        if (Char.MyGuild != null && CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                            i += DropRates.EggPacket * 0.25;
                        else if (Char.MyGuild != null && CityWarPc.LastWinner == Char.MyGuild && Loc.Map == 1011)
                            i += DropRates.EggPacket * 0.25;
                        else if (Char.MyGuild != null && CityWarAc.LastWinner == Char.MyGuild && Loc.Map == 1020)
                            i += DropRates.EggPacket * 0.25;
                        else if (Char.MyGuild != null && CityWarDc.LastWinner == Char.MyGuild && Loc.Map == 1000)
                            i += DropRates.EggPacket * 0.25;
                        else if (Char.MyGuild != null && CityWarBi.LastWinner == Char.MyGuild && Loc.Map == 1015)
                            i += DropRates.EggPacket * 0.25;
                        if (Char != null && Char.Loc.Map != 1015)
                            i += DropRates.GreenEgg * 0.05;


                        if (World.DropEvent)
                            i += DropRates.EggPacket * 0.25;
                        if (Char != null && Char.LuckyTime > 0 && i > 0)
                            i *= 0.2;
                        if (World.DREvent > DateTime.Now)
                            i += DropRates.EggPacket * 0.2;
                        if (Name.Length > 4)
                        {
                            //string newname = Name.Remove(0, Name.Length - 4);
                            if (IsBoss())
                            {
                                if (World.LowRatedServer)
                                    i += (DropRates.EggPacket * 120);
                                else i += (DropRates.EggPacket * 30);
                            }
                        }
                        if (MobID == 6056 || MobID == 6061 || MobID == 6064 || Loc.Map == 1070) // Gumparoo, Sfinxos, SarasMinion or GW hunters map
                        {
                            if (MyMath.ChanceSuccess(51)) // only roll for DB's 51% as often as normal
                            {
                                if (MyMath.ChanceSuccess(DropRates.EggPacket + (ExpChances / 400) + i + (NoobRate / 4)) || MobID == 409)
                                {
                                    if (Char != null)
                                    {
                                        if (Char.VipLevel == 5)
                                        {
                                            if (Char.Inventory.Count < 40)
                                            {
                                                Char.AddItem(720142);
                                                Char.MyClient.LocalMessage(2005, "You dropped a Fruit Packet, you can exchange it with a 5 Fruit!");
                                                World.SendMsgToAll("SYSTEM", "Fruit Packet dropped from " + Name + " killed by " + Char.Name + "!", 2011, 0/*, Loc.Map*/);
                                                Drop2 = false;
                                                //BaseStats.dbs++;
                                                //Context = new ConquerDataContext();

                                            }
                                        }
                                    }
                                    if (Drop2)
                                    {

                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = 720142;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        //BaseStats.dbs++;
                                        //Context = new ConquerDataContext();

                                        if (Char != null)
                                        {
                                            World.SendMsgToAll("SYSTEM", "Fruit Packet dropped from " + Name + " killed by " + Char.Name + "!", 2011, 0/*, Loc.Map*/);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(DropRates.EggPacket + (ExpChances / 400) + i + (NoobRate / 5) + (OnlineRate / 10)) || MobID == 409)
                            {
                                if (Char != null)
                                {
                                    if (Char.VipLevel == 5)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            if (MyMath.ChanceSuccess(40))
                                            {
                                                Char.AddItem(720142);
                                                //BaseStats.dbs++;
                                                //Context = new ConquerDataContext();

                                                Char.MyClient.LocalMessage(2005, "You dropped a Fruit Packet, you can exchange it with a 5 Fruit!");
                                                World.SendMsgToAll("SYSTEM", "Fruit Packet dropped from " + Name + " killed by " + Char.Name + "!", 2011, 0/*, Loc.Map*/);
                                                Drop2 = false;
                                            }
                                        }
                                    }
                                }
                                if (Drop2)
                                {
                                    if (MyMath.ChanceSuccess(40))
                                    {
                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = 720142;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;
                                        //BaseStats.dbs++;
                                        //Context = new ConquerDataContext();

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        if (Char != null)
                                        {

                                            World.SendMsgToAll("SYSTEM", "Fruit Packet dropped from " + Name + " killed by " + Char.Name + "!", 2011, 0/*, Loc.Map*/);
                                        }
                                    }

                                }
                            }
                        }

                    }
                    #endregion
                    #region Meteor
                    if (DropRates.Meteor != 0)
                    {
                        bool Drop2 = true;
                        double i = 0;
                        if (Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1215 || Loc.Map == 1211 || Loc.Map == 1212)
                            i = 0.03;
                        if (Char != null && Char.VipLevel == 6)
                            i += DropRates.Meteor * 0.05;
                        if (World.EventMet)
                            i += 0.2;
                        if (World.DropEvent)
                            i += DropRates.Meteor * 0.25;
                        //if (Char.LuckyTime > 0 && i > 0)
                        //    i *= 1.1;
                        if (World.DREvent > DateTime.Now)
                            i += DropRates.Meteor * 0.2;

                        if (Char.MyGuild != null && CityWarTc.LastWinner == Char.MyGuild && Loc.Map == 1002)
                            i += DropRates.Meteor * 0.25;
                        else if (Char.MyGuild != null && CityWarPc.LastWinner == Char.MyGuild && Loc.Map == 1011)
                            i += DropRates.Meteor * 0.25;
                        else if (Char.MyGuild != null && CityWarAc.LastWinner == Char.MyGuild && Loc.Map == 1020)
                            i += DropRates.Meteor * 0.25;
                        else if (Char.MyGuild != null && CityWarDc.LastWinner == Char.MyGuild && Loc.Map == 1000)
                            i += DropRates.Meteor * 0.25;
                        else if (Char.MyGuild != null && CityWarBi.LastWinner == Char.MyGuild && Loc.Map == 1015)
                            i += DropRates.Meteor * 0.25;

                        if (Name.Length > 4)
                        {
                            //string newname = Name.Remove(0, Name.Length - 4);
                            if (IsBoss())
                            {
                                i += (DropRates.Meteor * 30);
                            }
                        }
                        if (MobID == 6056 || MobID == 6061 || MobID == 6064 || Loc.Map == 1070) // Gumparoo, Sfinxos, SarasMinion and GW hunters map
                        {
                            if (MyMath.ChanceSuccess(51)) // only roll for met 51% of the time
                            {
                                if (MyMath.ChanceSuccess(DropRates.Meteor + i + (ExpChances / 2) + NoobRate))
                                {
                                    if (Char != null)
                                    {
                                        if (Char.VipLevel >= 5)
                                        {
                                            if (!Char.skipmeteor)
                                            {
                                                if (Char.Inventory.Count < 40)
                                                {
                                                    Char.AddItem(1088001);
                                                    Char.MyClient.LocalMessage(2005, "You received a Meteor from the drops.");
                                                    Drop2 = false;
                                                }
                                            }

                                        }
                                    }
                                    if (Drop2)
                                    {

                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = 1088001;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(DropRates.Meteor + i + (ExpChances / 2) + NoobRate + OnlineRate))
                            {
                                if (Char != null)
                                {
                                    if (Char.VipLevel >= 5)
                                    {
                                        if (!Char.skipmeteor)
                                        {
                                            if (Char.Inventory.Count < 40)
                                            {
                                                Char.AddItem(1088001);
                                                Char.MyClient.LocalMessage(2005, "You received a Meteor from the drops.");
                                                Drop2 = false;
                                            }
                                        }

                                    }
                                }
                                if (Drop2)
                                {

                                    DI2 = Drop(Owner);
                                    DI2.Info.ID = 1088001;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;

                                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI2.Drop();

                                }
                            }

                        }

                    }
                    #endregion

                    #region +1Stone Drops 
                    if (Char.VipLevel == 0) //VIP ONLY +1 Stone Drops AUTO LOOTED
                    {
                        double plusonestone = DropRates.PlusOne;
                        if (Char != null)
                        {
                            if (Char.Job >= 40 && Char.Job <= 45)
                            {
                                plusonestone = DropRates.PlusOne * 0.2;
                            }
                        }

                        int I = Rnd.Next(0, 4);
                        if (MyMath.ChanceSuccess(plusonestone))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = (730001);
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            Char.MyClient.LocalMessage(2001, "A +1Stone dropped on the floor at (" + DI2.Loc.X + "," + DI2.Loc.Y + ")");
                            if (!DI2.FindPlace((ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Char.VipLevel == 5 || Char.VipLevel == 6)


                    {
                        double plusonestone = DropRates.PlusOne;
                        if (Char != null)
                        {
                            if (Char.Job >= 40 && Char.Job <= 45)
                            {
                                plusonestone = DropRates.PlusOne * 0.22; // 
                            }
                        }

                        int I = Rnd.Next(0, 4);
                        if (MyMath.ChanceSuccess(plusonestone))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = (730001);
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            Char.AddItem(DI2.Info);
                            Char.MyClient.LocalMessage(2001, "A +1Stone has been added to your inventory.");

                        }
                    }
                    #endregion
                    #region Items in normal maps
                    if (DropRates.Item != 0 && Game.World.H_Items.ContainsKey(Loc.Map) && Loc.Map != 1214 && Loc.Map != 2020 && Loc.Map != 1215 && Loc.Map != 1210 && Loc.Map != 1211 && Loc.Map != 1212 && Loc.Map != 1043 && Loc.Map != 1044 && Loc.Map != 1045 && Loc.Map != 1046 && Loc.Map != 1047 && Loc.Map != 1048 && Loc.Map != 1049 && Loc.Map != 1351 && Loc.Map != 1352 && Loc.Map != 1353 && Loc.Map != 1354 && Loc.Map != 2021 && Loc.Map != 2022 && Loc.Map != 2023 && Loc.Map != 2024 && Loc.Map != 1051)
                    {
                        bool Drop2 = true;
                        DI2 = Drop(Owner);
                        if (DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            bool boss = IsBoss();
                            if (boss || MyMath.ChanceSuccess(DropRates.Item + (ExpChances * 20) + NoobRate + OnlineRate))
                            {
                                Item.ItemQuality Q = Item.ItemQuality.Simple;
                                double super = 0;
                                double elite = 0;
                                if (World.EventSuper)
                                    super = DropRates.Super / 2;
                                else if (World.EventElite)
                                    elite = DropRates.Elite / 2;
                                if (boss)
                                {
                                    Q = Item.ItemQuality.Refined;
                                    super += DropRates.Super * 25;
                                    elite += DropRates.Elite * 50;
                                }
                                if (MyMath.ChanceSuccess(DropRates.Super + super + (ExpChances / 5)))
                                    Q = Item.ItemQuality.Super;
                                else if (MyMath.ChanceSuccess(DropRates.Elite + elite + (ExpChances / 2) + NoobRate))
                                    Q = Item.ItemQuality.Elite;
                                else if (MyMath.ChanceSuccess(DropRates.Unique + (ExpChances * 4)))
                                    Q = Item.ItemQuality.Unique;
                                else if (MyMath.ChanceSuccess(DropRates.Refined + (ExpChances * 10)))
                                    Q = Item.ItemQuality.Refined;
                                uint ItemID = 0;
                            Top:
                                List<uint> From = new List<uint>();
                                int Type = Rnd.Next(0, 330);
                                uint Part = 0;
                                if (Type < 10) Part = 111;
                                else if (Type < 20) Part = 113;
                                else if (Type < 30) Part = 114;
                                else if (Type < 40) Part = 117;
                                else if (Type < 50) Part = 118;
                                else if (Type < 60) Part = 120;
                                else if (Type < 70) Part = 121;
                                else if (Type < 80) Part = 130;
                                else if (Type < 90) Part = 131;
                                else if (Type < 100) Part = 133;
                                else if (Type < 110) Part = 134;
                                else if (Type < 120) Part = 141;
                                else if (Type < 130) Part = 142;
                                else if (Type < 140) Part = 150;
                                else if (Type < 150) Part = 151;
                                else if (Type < 160) Part = 152;
                                else if (Type < 165) Part = 160;
                                else if (Type < 175) Part = 410;
                                else if (Type < 185) Part = 420;
                                else if (Type < 195) Part = 421;
                                else if (Type < 203) Part = 430;
                                else if (Type < 211) Part = 440;
                                else if (Type < 219) Part = 450;
                                else if (Type < 229) Part = 460;
                                else if (Type < 239) Part = 480;
                                else if (Type < 247) Part = 481;
                                else if (Type < 255) Part = 490;
                                else if (Type < 265) Part = 500;
                                else if (Type < 275) Part = 510;
                                else if (Type < 285) Part = 530;
                                else if (Type < 295) Part = 540;
                                else if (Type < 305) Part = 560;
                                else if (Type < 315) Part = 561;
                                else if (Type < 325) Part = 580;
                                else if (Type < 330) Part = 900;
                                /*  if (Type < 20) Part = 111;
                                  else if (Type < 40) Part = (uint)Rnd.Next(113, 115);
                                  else if (Type < 60) Part = (uint)Rnd.Next(117, 119);
                                  else if (Type < 80) Part = (uint)Rnd.Next(120, 122);
                                  else if (Type < 100) Part = (uint)Rnd.Next(130, 132);
                                  else if (Type < 120) Part = (uint)Rnd.Next(133, 135);
                                  else if (Type < 140) Part = (uint)Rnd.Next(141, 143);
                                  else if (Type < 160) Part = (uint)Rnd.Next(150, 154);
                                  else if (Type < 170) Part = 160;
                                  else if (Type < 190) Part = 410;
                                  else if (Type < 210) Part = 420;
                                  else if (Type < 230) Part = 421;
                                  else if (Type < 250) Part = 430;
                                  else if (Type < 270) Part = 440;
                                  else if (Type < 290) Part = 450;
                                  else if (Type < 310) Part = 460;
                                  else if (Type < 330) Part = (uint)Rnd.Next(480, 482);
                                  else if (Type < 350) Part = 490;
                                  else if (Type < 370) Part = 500;
                                  else if (Type < 390) Part = 510;
                                  else if (Type < 410) Part = 530;
                                  else if (Type < 430) Part = 540;
                                  else if (Type < 450) Part = (uint)Rnd.Next(560, 562);
                                  else if (Type < 470) Part = 580;
                                  else if (Type < 475) Part = 900;*/
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {

                                    if (Level <= 115)
                                    {
                                        if (D.LevReq + 15 > Level && D.LevReq - 10 <= Level)//ai vreo idee?..nush dc nu merge :-?? habarnam
                                            if (D.LevReq != 0)//nush plm..sry lasa vdem alta data
                                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                    From.Add(D.ID);
                                    }
                                    else if (D.LevReq >= 106 && D.LevReq <= 126)
                                        if (D.LevReq != 0)
                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                From.Add(D.ID);
                                }
                                if (From != null)
                                {
                                    if (From.Count > 0)
                                    {
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        ItemID = (uint)From[Tries];
                                    }
                                    else goto Top;
                                }
                                if (ItemID != 0)
                                {
                                    DI2.Info.ID = ItemID;
                                    if (DI2.Info.DBInfo.LevReq != 1)
                                    {
                                        ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                        E.QualityChange(Q);
                                        DI2.Info.ID = E.ToID();
                                    }

                                    DI2.Info.Color = Item.ArmorColor.Orange;
                                    if (ItemIDManipulation.Digit(DI2.Info.ID, 1) == 4 || ItemIDManipulation.Digit(DI2.Info.ID, 1) == 5)
                                    {
                                        if (boss || MyMath.ChanceSuccess(DropRates.OneSoc + (ExpChances * 10)))
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                        if (MyMath.ChanceSuccess(DropRates.TwoSoc + (ExpChances * 10)))
                                        {
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                            DI2.Info.Soc2 = Item.Gem.EmptySocket;
                                        }
                                    }
                                    double plus = 0;
                                    if (World.EventPlus)
                                        plus = DropRates.PlusOne / 3;
                                    if (boss)
                                        plus += DropRates.PlusOne * 10;
                                    if (Char != null && Char.VipLevel == 6)
                                        plus += DropRates.PlusOne * 0.05;
                                    if (ItemIDManipulation.Digit(DI2.Info.ID, 1) == 4 || ItemIDManipulation.Digit(DI2.Info.ID, 1) == 5)
                                    {
                                        if (MyMath.ChanceSuccess((DropRates.PlusOne * 0.45) + plus + ExpChances))
                                            DI2.Info.Plus = 1;
                                    }
                                    else if (MyMath.ChanceSuccess(DropRates.PlusOne + plus + ExpChances + (OnlineRate / 5)))
                                        DI2.Info.Plus = 1;

                                    if (MyMath.ChanceSuccess(0.0099 + ExpChances))
                                        if (MyMath.ChanceSuccess(3))
                                            DI2.Info.Bless = 5;
                                        else if (MyMath.ChanceSuccess(10))
                                            DI2.Info.Bless = 3;
                                    //else DI2.Info.Bless = 1;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    if (DI2.Info.MaxDur != 0)
                                        if (DI2.Info.DBInfo.LevReq < 40)
                                            DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.5), (int)(DI2.Info.MaxDur * 0.8));
                                        else if (DI2.Info.ID % 10 > 5)
                                            DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.15), (int)(DI2.Info.MaxDur * 0.35));
                                        else DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.01), (int)(DI2.Info.MaxDur * 0.1));
                                    else DI2.Info.CurDur = DI2.Info.MaxDur;
                                }
                                if (Char != null)
                                {

                                    if (Char.VipLevel > 0 && Char.VipLevel <= 6)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            if (Char.VipLevel >= 3)
                                            {

                                                if (Q == Item.ItemQuality.Elite)
                                                {
                                                    if (Char.skipelite)
                                                        Drop2 = true;
                                                    else
                                                    {
                                                        Drop2 = false;
                                                        Char.AddItem(DI2.Info);
                                                        Char.MyClient.LocalMessage(2005, "You received an elite item from the drops.");
                                                    }
                                                }
                                                else if (Q == Item.ItemQuality.Super)
                                                {
                                                    if (Char.skipsuper)
                                                        Drop2 = true;
                                                    else
                                                    {
                                                        Drop2 = false;
                                                        Char.AddItem(DI2.Info);
                                                        Char.MyClient.LocalMessage(2005, "You received a super item from the drops.");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Q == Item.ItemQuality.Super)
                                        World.SendMsgToAll("LUCKY", "A Super" + DI2.Info.DBInfo.Name + " has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);

                                }
                                if (Drop2)  //+1 Stone Drop Normal MAPS
                                {
                                    if (Char != null)
                                    {
                                        if (DI2.Info.Plus == 1 && Char.VipLevel >= 0)

                                        {
                                            Char.MyClient.LocalMessage(2001, "A " + DI2.Info.DBInfo.Name + " +1 item dropped at " + DI2.Loc.X + "," + DI2.Loc.Y + "!");

                                            World.Action(DI2, Packets.StringPacket(DI2.Loc.X, DI2.Loc.Y, StringType.MapEffect, "colorstart1").Get);



                                        }
                                        if (DI2.Info.Bless != 0 && Char.VipLevel >= 0)
                                        {
                                            Char.MyClient.LocalMessage(2001, "A -" + DI2.Info.Bless + " Blessed " + DI2.Info.DBInfo.Name + " dropped at " + DI2.Loc.X + ", " + DI2.Loc.Y + "!");

                                            World.Action(DI2, Packets.StringPacket(DI2.Loc.X, DI2.Loc.Y, StringType.MapEffect, "colorstart3").Get);
                                        }
                                    }
                                    if (ItemID != 150000 && ItemID != 150310 && ItemID != 150320) // gumpfix to prevent loveforever rings from dropping
                                    {
                                        DI2.Drop();
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Items in lab
                    else if (Loc.Map == 1351 || Loc.Map == 1352 || Loc.Map == 1353 || Loc.Map == 1354)
                    {
                        bool Drop2 = true;
                        DI2 = Drop(Owner);
                        if (DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            if (MyMath.ChanceSuccess(DropRates.Item + (ExpChances * 20)))
                            {
                                Item.ItemQuality Q = Item.ItemQuality.Simple;
                                double super = 0;
                                double elite = 0;
                                if (World.EventSuper)
                                    super = DropRates.Super / 2;
                                else if (World.EventElite)
                                    elite = DropRates.Elite / 2;
                                if (MyMath.ChanceSuccess(DropRates.Super + super))
                                    Q = Item.ItemQuality.Super;
                                else if (MyMath.ChanceSuccess(DropRates.Elite + elite))
                                    Q = Item.ItemQuality.Elite;
                                else if (MyMath.ChanceSuccess(DropRates.Unique))
                                    Q = Item.ItemQuality.Unique;
                                else if (MyMath.ChanceSuccess(DropRates.Refined))
                                    Q = Item.ItemQuality.Refined;
                                uint ItemID = 0;
                                List<uint> From = new List<uint>();
                                int Type;
                                if (MobID == 85 || MobID == 88 || MobID == 91)
                                    Type = Rnd.Next(0, 165);
                                else Type = Rnd.Next(0, 335);
                                uint Part = 0;
                                if (Type < 10) Part = 111;
                                else if (Type < 20) Part = 113;
                                else if (Type < 30) Part = 114;
                                else if (Type < 40) Part = 117;
                                else if (Type < 50) Part = 118;
                                else if (Type < 60) Part = 120;
                                else if (Type < 70) Part = 121;
                                else if (Type < 80) Part = 130;
                                else if (Type < 90) Part = 131;
                                else if (Type < 100) Part = 133;
                                else if (Type < 110) Part = 134;
                                else if (Type < 120) Part = 141;
                                else if (Type < 130) Part = 142;
                                else if (Type < 140) Part = 150;
                                else if (Type < 150) Part = 151;
                                else if (Type < 160) Part = 152;
                                else if (Type < 165) Part = 160;
                                else if (Type < 175) Part = 410;
                                else if (Type < 185) Part = 420;
                                else if (Type < 195) Part = 421;
                                else if (Type < 205) Part = 430;
                                else if (Type < 215) Part = 440;
                                else if (Type < 225) Part = 450;
                                else if (Type < 235) Part = 460;
                                else if (Type < 245) Part = 480;
                                else if (Type < 255) Part = 481;
                                else if (Type < 265) Part = 490;
                                else if (Type < 275) Part = 500;
                                else if (Type < 285) Part = 510;
                                else if (Type < 295) Part = 530;
                                else if (Type < 305) Part = 540;
                                else if (Type < 315) Part = 560;
                                else if (Type < 325) Part = 561;
                                else if (Type < 335) Part = 580;

                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (D.LevReq <= 15)
                                    {
                                        if (D.LevReq != 0)
                                            if (ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                From.Add(D.ID);
                                    }
                                }
                                if (From != null)
                                {
                                    if (From.Count > 0)
                                    {
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        ItemID = (uint)From[Tries];
                                    }
                                }
                                if (ItemID != 0)
                                {
                                    DI2.Info.ID = ItemID;
                                    if (DI2.Info.DBInfo.LevReq != 1)
                                    {
                                        ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                        E.QualityChange(Q);
                                        DI2.Info.ID = E.ToID();
                                    }

                                    DI2.Info.Color = Item.ArmorColor.Orange;
                                    if (MobID != 91 && MobID != 85 && MobID != 88)//not ( talon gibbon nagalord )
                                    {
                                        if (ItemIDManipulation.Digit(DI2.Info.ID, 1) == 4 || ItemIDManipulation.Digit(DI2.Info.ID, 1) == 5)
                                        {
                                            if (MyMath.ChanceSuccess(DropRates.OneSoc + 0.5 + (ExpChances * 10)))
                                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                            if (MyMath.ChanceSuccess(DropRates.TwoSoc + 0.2 + (ExpChances * 10)))
                                            {
                                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                                DI2.Info.Soc2 = Item.Gem.EmptySocket;
                                            }
                                        }
                                    }

                                    double plus = 0;
                                    if (World.EventPlus)
                                        plus = DropRates.PlusOne / 3;
                                    if (Char != null && Char.VipLevel == 6)
                                        plus += DropRates.PlusOne * 0.05;
                                    if (MyMath.ChanceSuccess(DropRates.PlusOne + plus + 0.1 + (ExpChances / 2)))
                                        DI2.Info.Plus = 1;
                                    if (MyMath.ChanceSuccess(0.0065 + ExpChances))
                                        if (MyMath.ChanceSuccess(3))
                                            DI2.Info.Bless = 5;
                                        else if (MyMath.ChanceSuccess(10))
                                            DI2.Info.Bless = 3;
                                    //else DI2.Info.Bless = 1;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;


                                    if (DI2.Info.MaxDur != 0)
                                        if (DI2.Info.DBInfo.LevReq < 40)
                                            DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.5), (int)(DI2.Info.MaxDur * 0.8));
                                        else if (DI2.Info.ID % 10 > 5)
                                            DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.15), (int)(DI2.Info.MaxDur * 0.35));
                                        else DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.01), (int)(DI2.Info.MaxDur * 0.1));
                                    else DI2.Info.CurDur = DI2.Info.MaxDur;
                                }
                                if (Char != null)
                                {

                                    if (Char.VipLevel > 0 && Char.VipLevel <= 6)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            if (Char.VipLevel >= 3)
                                            {
                                                if (Q == Item.ItemQuality.Elite)
                                                {
                                                    Drop2 = false;
                                                    Char.AddItem(DI2.Info);
                                                    Char.MyClient.LocalMessage(2005, "You received an elite item from the drops.");
                                                }
                                                else if (Q == Item.ItemQuality.Super)
                                                {
                                                    Drop2 = false;
                                                    Char.AddItem(DI2.Info);
                                                    Char.MyClient.LocalMessage(2005, "You received a super item from the drops.");
                                                }
                                            }
                                        }
                                    }

                                }
                                if (Drop2) // +1 Stone Drops in LAB
                                {
                                    if (Char != null)
                                    {
                                        if (DI2.Info.Plus == 1 && Char.VipLevel <= 6)
                                        {
                                            Char.MyClient.LocalMessage(2001, "A " + DI2.Info.DBInfo.Name + " +1 item dropped at " + DI2.Loc.X + "," + DI2.Loc.Y + "!");
                                            World.Action(DI2, Packets.StringPacket(DI2.Loc.X, DI2.Loc.Y, StringType.MapEffect, "colorstart1").Get);
                                        }
                                        if (DI2.Info.Bless != 0 && Char.VipLevel <= 6)
                                        {
                                            Char.MyClient.LocalMessage(2001, "A -" + DI2.Info.Bless + " Blessed " + DI2.Info.DBInfo.Name + " dropped at " + DI2.Loc.X + ", " + DI2.Loc.Y + "!");
                                            World.Action(DI2, Packets.StringPacket(DI2.Loc.X, DI2.Loc.Y, StringType.MapEffect, "colorstart3").Get);
                                        }
                                    }
                                    if (ItemID != 150000 && ItemID != 150310 && ItemID != 150320) // gumpfix to prevent loveforever rings from dropping
                                    {
                                        DI2.Drop();
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Items in advanced zone
                    else if (Loc.Map == 2020 || Loc.Map == 1214 || Loc.Map == 1210 || Loc.Map == 1211 || Loc.Map == 1212 || Loc.Map == 1215)
                    {
                        bool Drop2 = true;
                        DI2 = Drop(Owner);
                        if (DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            if (MyMath.ChanceSuccess(0.012))
                            {
                                Item.ItemQuality Q = Item.ItemQuality.Refined;
                                if (MyMath.ChanceSuccess(6))
                                    Q = Item.ItemQuality.Super;
                                else if (MyMath.ChanceSuccess(12))
                                    Q = Item.ItemQuality.Elite;
                                else if (MyMath.ChanceSuccess(50))
                                    Q = Item.ItemQuality.Unique;
                                uint ItemID = 0;

                                List<uint> From = new List<uint>();
                                int Type = Rnd.Next(0, 330);
                                uint Part = 0;
                                if (Type < 10) Part = 111;
                                else if (Type < 20) Part = 113;
                                else if (Type < 30) Part = 114;
                                else if (Type < 40) Part = 117;
                                else if (Type < 50) Part = 118;
                                else if (Type < 60) Part = 120;
                                else if (Type < 70) Part = 121;
                                else if (Type < 80) Part = 130;
                                else if (Type < 90) Part = 131;
                                else if (Type < 100) Part = 133;
                                else if (Type < 110) Part = 134;
                                else if (Type < 120) Part = 141;
                                else if (Type < 130) Part = 142;
                                else if (Type < 140) Part = 150;
                                else if (Type < 150) Part = 151;
                                else if (Type < 160) Part = 152;
                                else if (Type < 165) Part = 160;
                                else if (Type < 175) Part = 410;
                                else if (Type < 185) Part = 420;
                                else if (Type < 195) Part = 421;
                                else if (Type < 203) Part = 430;
                                else if (Type < 211) Part = 440;
                                else if (Type < 219) Part = 450;
                                else if (Type < 229) Part = 460;
                                else if (Type < 239) Part = 480;
                                else if (Type < 247) Part = 481;
                                else if (Type < 255) Part = 490;
                                else if (Type < 265) Part = 500;
                                else if (Type < 275) Part = 510;
                                else if (Type < 285) Part = 530;
                                else if (Type < 295) Part = 540;
                                else if (Type < 305) Part = 560;
                                else if (Type < 315) Part = 561;
                                else if (Type < 325) Part = 580;
                                else if (Type < 330) Part = 900;

                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (D.LevReq >= (Level / 2) && D.LevReq <= 82)
                                    {
                                        if (D.LevReq != 0)
                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                From.Add(D.ID);
                                    }
                                }
                                if (From != null)
                                {
                                    if (From.Count > 0)
                                    {
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        ItemID = (uint)From[Tries];
                                    }
                                }
                                if (ItemID != 0)
                                {
                                    DI2.Info.ID = ItemID;
                                    if (DI2.Info.DBInfo.LevReq != 1)
                                    {
                                        ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                        E.QualityChange(Q);
                                        DI2.Info.ID = E.ToID();
                                    }

                                    DI2.Info.Color = Item.ArmorColor.Orange;
                                    if (ItemIDManipulation.Digit(DI2.Info.ID, 1) == 4 || ItemIDManipulation.Digit(DI2.Info.ID, 1) == 5)
                                    {
                                        if (MyMath.ChanceSuccess(18))
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                        if (MyMath.ChanceSuccess(8))
                                        {
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                            DI2.Info.Soc2 = Item.Gem.EmptySocket;
                                        }
                                    }
                                    if (MyMath.ChanceSuccess(0.0075))
                                        if (MyMath.ChanceSuccess(5))
                                            DI2.Info.Bless = 5;
                                        else if (MyMath.ChanceSuccess(15))
                                            DI2.Info.Bless = 3;
                                        else DI2.Info.Bless = 1;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;

                                    if (DI2.Info.MaxDur != 0)
                                        DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.15), (int)(DI2.Info.MaxDur * 0.35));
                                    else DI2.Info.CurDur = DI2.Info.MaxDur;
                                }
                                if (Char != null)
                                {
                                    if (Char.VipLevel > 0 && Char.VipLevel <= 4)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            if (Char.VipLevel >= 3)
                                            {
                                                if (Q == Item.ItemQuality.Elite)
                                                {
                                                    Char.MyClient.LocalMessage(2005, "You received an elite item from the drops.");
                                                    Drop2 = false;
                                                    Char.AddItem(DI2.Info);
                                                }
                                                else if (Q == Item.ItemQuality.Super)
                                                {
                                                    Char.MyClient.LocalMessage(2005, "You received a super item from the drops.");
                                                    Drop2 = false;
                                                    Char.AddItem(DI2.Info);
                                                }
                                            }


                                        }
                                    }
                                    if (Q == Item.ItemQuality.Super)
                                        World.SendMsgToAll("LUCKY", "A Super" + DI2.Info.DBInfo.Name + " has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);
                                }
                                if (Drop2)
                                {
                                    if (DI2.Info.Bless != 0 && Char.VipLevel >= 5) // VIP 5 & 6 Get Bless notifications on ground
                                    {
                                        Char.MyClient.LocalMessage(2001, "A -" + DI2.Info.Bless + " Blessed " + DI2.Info.DBInfo.Name + " dropped!");
                                    }
                                    if (ItemID != 150000 && ItemID != 150310 && ItemID != 150320) // gumpfix to prevent loveforever rings from dropping
                                    {
                                        DI2.Drop();
                                    }
                                }
                            }
                            if (MyMath.ChanceSuccess(1.1))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 1060102; // StoneCity
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            else if (MyMath.ChanceSuccess(1.1))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 1002040; // ChantPill
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            else if (MyMath.ChanceSuccess(1.1))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 1002050; // Milginseng
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                    }
                    #endregion
                    #region DanceBooks
                    if (MyMath.ChanceSuccess(0.0045))
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = (uint)Rnd.Next(725018, 725025);
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                        if (Char != null)
                            if (Char.VipLevel == 4 || Char.VipLevel == 6)
                                Char.MyClient.LocalMessage(2000, "A DanceBook magic scroll dropped from a monster that you killed!");
                    }
                    #endregion
                    #region a silly place to check for quest kills
                    #region BI Quests
                    if (Char != null)
                    {
                        if (Char.BI_Quest == 1)
                        {
                            if (MobID == 52 || MobID == 53 || MobID == 54)
                            {
                                if (Char.BI_Quest_Kills >= 15000)
                                {
                                    Char.MyClient.LocalMessage(2000, "That's enough! You've killed 15,000 Monsters now! Report to Cathy in Bird Island.");
                                }
                                else
                                {
                                    Char.BI_Quest_Kills += 1;
                                }
                            }
                        }
                    }
                    #endregion
                    if (Char != null)
                    {
                        if (Char.AC_Quest_Hops)
                        {
                            if (Char.InventoryContains(729933, 5))
                            {
                                Char.AC_Quest_Hops_Completed = true;
                                Char.MyClient.LocalMessage(2000, "You are finished the Hops quest! Go see the Breeder in Ape City (550,598)");
                            }
                            else if (MobID == 27 || MobID == 28 || MobID == 29 || MobID == 30 || MobID == 31 || MobID == 32)
                            {
                                if (MyMath.ChanceSuccess(0.133))
                                {
                                    DI2 = Drop(Owner);
                                    DI2.Info.ID = 729933; //Hops
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;
                                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI2.Drop();
                                    if (Char.VipLevel >= 4)
                                    {
                                        Char.MyClient.LocalMessage(2000, "A Quest item (Hops) dropped from a monster that you killed!");
                                    }
                                }
                            }
                        }
                        if (Char.DailyQuestActive && !Char.DailyQuestCompleted)
                        {
                            Char.DailyQuestKills += 1;
                            if (Char.DailyQuestKills >= 100)
                            {
                                Char.DailyQuestCompleted = true;
                                Char.MyClient.LocalMessage(2000, "You have completed the Daily Quest! Return to the Daily Quest Master.");
                            }
                        }
                    }
                    #endregion
                    #region Anniversary Quest
                    if (DateTime.Now.Month == 2 && DateTime.Now.Day >= 17 && DateTime.Now.Day < 20)
                    {
                        if (MyMath.ChanceSuccess(DropRates.Meteor * 7)) // roll for S.H.A.N.N.A.R.A letters
                        {
                            Random Rdn = new Random();
                            uint ID = 711210 + (uint)(Rdn.Next(0, 5));
                            DI2 = Drop(Owner);
                            DI2.Info.ID = ID;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region ElementalSymbols
                    if (MyMath.ChanceSuccess(DropRates.Meteor * Level / 20) && Loc.Map == 3030)
                    {
                        Random Rnd = new Random();
                        uint ID = 730700 + (uint)(Rnd.Next(0, 5));
                        DI2 = Drop(Owner);
                        DI2.Info.ID = ID;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                        Char.MyClient.LocalMessage(2000, "an element (" + DI2.Info.DBInfo.Name + ") has been dropped next to you !");
                    }
                    #endregion
                    //#region EasterEggs Quest
                    //if (DateTime.Now.AddDays(5) >= NPCs.NPC_7.EasterSunday(DateTime.Now.Year) && DateTime.Now <= NPCs.NPC_7.EasterSunday(DateTime.Now.Year).AddDays(5))
                    //{
                    //    if (MyMath.ChanceSuccess(0.918)) // roll for EasterEggs
                    //    {
                    //        Random Rdn = new Random();
                    //        uint ID = 710060 + (uint)(Rdn.Next(0, 4));
                    //        DI2 = Drop(Owner);
                    //        DI2.Info.ID = ID; // EasterEggs
                    //        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //        DI2.Drop();
                    //    }
                    //    else if (MyMath.ChanceSuccess(DropRates.DragonBall * 6)) // Roll for StrippedEggs
                    //    {
                    //        Random Rdn = new Random();
                    //        uint ID = 710065 + (uint)(Rdn.Next(0, 7));
                    //        DI2 = Drop(Owner);
                    //        DI2.Info.ID = ID; // StrippedEggs
                    //        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //        DI2.Drop();
                    //        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a " + DI2.Info.DBInfo.Name + "!", 2000, 0, Loc.Map);
                    //    }
                    //}
                    //#endregion
                    #region Halloween Quest
                    if ((DateTime.Now.Month == 10 && DateTime.Now.Day >= 28) || (DateTime.Now.Month == 11 && DateTime.Now.Day <= 6))
                    {
                        if (MyMath.ChanceSuccess(0.918)) // roll for Pumpkins
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 722176; // Pumpkin
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        else if (MyMath.ChanceSuccess(DropRates.DragonBall * 6)) // Roll for PumpkinSeeds
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 710587; // PumpkinSeed
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    if (MobID == 809 && ((DateTime.Now.Month == 10 && DateTime.Now.Day >= 28) || (DateTime.Now.Month == 11 && DateTime.Now.Day <= 6)))
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 721970; // PumpkinBox
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    #endregion
                    #region ChristmasQuest
                    if (DateTime.Now.Month == 12)
                    {
                        if (((!World.SafeBool && MyMath.ChanceSuccess(DropRates.Meteor * 4)) || (World.SafeBool && MyMath.ChanceSuccess(DropRates.Meteor * 2))) && DateTime.Now.Day >= 9) // Roll for Snowballs
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 720163; // PumpkinSeed
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        if (MyMath.ChanceSuccess(DropRates.Meteor / 3) && DateTime.Now.Day >= 18)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = Convert.ToUInt32(Rnd.Next(720151, 720156)); // PumpkinSeed
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region SoulStone
                    if (DropRates.SoulStone != 0)
                    {
                        if (DI2.Loc.Map == 2021)
                        {
                            double i = 0;
                            if (MobID == 297)
                                i += 0.4;
                            if (Char != null)
                                if (Char.Job >= 40 && Char.Job <= 45)
                                    i -= 0.4;
                            if (MyMath.ChanceSuccess(DropRates.SoulStone + i + 0.5)) // Dis City drop
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 723085; // SoulStone
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                    }
                    #endregion
                    #region CleanWater
                    if (MobID == 266)
                    {
                        if (Loc.Map == 1212)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721258;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region CommandTokens
                    else if (Loc.Map == 1043)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721010;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Loc.Map == 1044)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721011;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Loc.Map == 1045)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721012;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Loc.Map == 1046)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721013;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Loc.Map == 1047)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721014;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (Loc.Map == 1048)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721015;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region SnakeKing
                    else if (Loc.Map == 1051)
                    {
                        if (MobID == 300)
                        {
                            bool Drop2 = true;
                            DI2 = Drop(Owner);
                            uint ItemX = 1088000;
                            byte x = (byte)Rnd.Next(1, 4);
                            bool M = false;
                            if (x == 1)
                            {
                                if (MyMath.ChanceSuccess(10))
                                {
                                    if (Drop2)
                                    {
                                        for (int i = 0; i < 3; i++)
                                        {
                                            DI2 = Drop(Owner);
                                            DI2.Info.ID = ItemX;
                                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                            DI2.Info.CurDur = DI2.Info.MaxDur;

                                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                            DI2.Drop();
                                            World.SendMsgToAll("LUCKY", "3x Dragonball has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);
                                        }
                                        M = true;
                                    }
                                }
                            }
                            if (x == 2)
                            {
                                if (MyMath.ChanceSuccess(60))
                                {
                                    if (Drop2)
                                    {

                                        ItemX = 1088000;
                                        DI2.Info.ID = ItemX;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        World.SendMsgToAll("LUCKY", "A Dragonball has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);

                                        M = true;

                                    }
                                }
                            }
                            if (x == 3)
                            {
                                if (MyMath.ChanceSuccess(25))
                                {
                                    byte G = (byte)Rnd.Next(0, 8);
                                    ItemX = (uint)(700000 + G * 10 + Rnd.Next(2, 4));

                                    DI2.Info.ID = ItemX;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;

                                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI2.Drop();
                                    M = true;
                                }
                            }
                            if (x == 4)
                            {
                                Item.ItemQuality Q = Item.ItemQuality.Refined;
                                if (MyMath.ChanceSuccess(45))
                                    Q = Item.ItemQuality.Super;
                                else if (MyMath.ChanceSuccess(55))
                                    Q = Item.ItemQuality.Elite;
                                else if (MyMath.ChanceSuccess(70))
                                    Q = Item.ItemQuality.Unique;
                                uint ItemID = 0;

                                List<uint> From = new List<uint>();
                                int Type = Rnd.Next(0, 255);
                                uint Part = 0;
                                if (Type < 10) Part = 111;
                                else if (Type < 20) Part = 113;
                                else if (Type < 30) Part = 114;
                                else if (Type < 40) Part = 117;
                                else if (Type < 50) Part = 118;
                                else if (Type < 60) Part = 120;
                                else if (Type < 70) Part = 121;
                                else if (Type < 80) Part = 130;
                                else if (Type < 90) Part = 131;
                                else if (Type < 100) Part = 133;
                                else if (Type < 110) Part = 134;
                                else if (Type < 120) Part = 141;
                                else if (Type < 130) Part = 142;
                                else if (Type < 140) Part = 150;
                                else if (Type < 150) Part = 151;
                                else if (Type < 160) Part = 152;
                                else if (Type < 165) Part = 160;
                                else if (Type < 175) Part = 410;
                                else if (Type < 185) Part = 420;
                                else if (Type < 195) Part = 480;
                                else if (Type < 205) Part = 481;
                                else if (Type < 215) Part = 500;
                                else if (Type < 225) Part = 530;
                                else if (Type < 235) Part = 560;
                                else if (Type < 245) Part = 561;
                                else if (Type < 255) Part = 900;
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (D.LevReq >= 60 && D.LevReq <= 120)
                                    {
                                        if (D.LevReq != 0)
                                            if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                From.Add(D.ID);
                                    }
                                }
                                if (From != null)
                                {
                                    if (From.Count > 0)
                                    {
                                        byte Tries = (byte)Rnd.Next(0, From.Count);
                                        ItemID = (uint)From[Tries];
                                    }
                                }
                                if (ItemID != 0)
                                {
                                    DI2.Info.ID = ItemID;
                                    if (DI2.Info.DBInfo.LevReq != 1)
                                    {
                                        ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                        E.QualityChange(Q);
                                        DI2.Info.ID = E.ToID();
                                    }

                                    DI2.Info.Color = Item.ArmorColor.Orange;
                                    if (ItemIDManipulation.Digit(DI2.Info.ID, 1) == 4 || ItemIDManipulation.Digit(DI2.Info.ID, 1) == 5)
                                    {
                                        if (MyMath.ChanceSuccess(25))
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                        if (MyMath.ChanceSuccess(8))
                                        {
                                            DI2.Info.Soc1 = Item.Gem.EmptySocket;
                                            DI2.Info.Soc2 = Item.Gem.EmptySocket;
                                        }
                                    }
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;

                                    if (DI2.Info.MaxDur != 0)
                                        DI2.Info.CurDur = (ushort)Rnd.Next(1, (int)(DI2.Info.MaxDur / 4));
                                    else DI2.Info.CurDur = DI2.Info.MaxDur;
                                }
                                if (Char != null)
                                {
                                    if (Char.VipLevel >= 5)
                                    {
                                        if (Char.Inventory.Count < 40)
                                        {
                                            if (Char.VipLevel >= 3)
                                            {
                                                if (Q == Item.ItemQuality.Elite)
                                                    Char.AddItem(DI2.Info);
                                                if (Q == Item.ItemQuality.Super)
                                                    Char.AddItem(DI2.Info);
                                            }
                                            if (Q == Item.ItemQuality.Elite)
                                            {
                                                Char.MyClient.LocalMessage(2005, "You received an elite item from the drops.");
                                                Drop2 = false;
                                            }
                                            if (Q == Item.ItemQuality.Super)
                                            {
                                                Char.MyClient.LocalMessage(2005, "You received a super item from the drops.");
                                                World.SendMsgToAll("LUCKY", "A Super" + DI2.Info.DBInfo.Name + " has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);
                                                Drop2 = false;
                                            }
                                        }
                                    }
                                }
                                if (Drop2)
                                {

                                    if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                                        return;
                                    DI2.Drop();
                                    M = true;
                                }
                            }
                            if (M == false)
                            {
                                if (Char != null)
                                {
                                    if (Char.VipLevel >= 3 && Char.VipLevel <= 6)
                                    {
                                        if (Char.Inventory.Count < 37)
                                        {
                                            for (int l = 0; l < 2; l++)
                                                Char.AddItem(1088000);
                                            Drop2 = false;
                                            World.SendMsgToAll("LUCKY", "2x Dragonball has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);
                                        }
                                    }
                                }
                                if (Drop2)
                                {

                                    for (int i = 0; i < 3; i++)
                                    {
                                        DI2 = Drop(Owner);
                                        DI2.Info.ID = ItemX;
                                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                        DI2.Info.CurDur = DI2.Info.MaxDur;

                                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                        DI2.Drop();
                                        World.SendMsgToAll("LUCKY", "3x Dragonball has dropped from the " + Name + " killed by " + Char.Name + "!", 2005, 0);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Emerald
                    else if (MobID == 44 || MobID == 43)
                    {
                        if (MyMath.ChanceSuccess(1.4 + ExpChances + NoobRate))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1080001;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region SkyToken
                    else if (MobID == 84)
                    {
                        if (MyMath.ChanceSuccess(0.7))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721537;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region EarthToken
                    else if (MobID == 87)
                    {
                        if (MyMath.ChanceSuccess(0.65))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721538;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region SoulToken
                    else if (MobID == 90)
                    {
                        if (MyMath.ChanceSuccess(0.62))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721539;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region Lab Diamonds
                    else if (MobID == 83)
                    {
                        if (MyMath.ChanceSuccess(1.0))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721533;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 86)
                    {
                        if (MyMath.ChanceSuccess(1.0))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721534;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 89)
                    {
                        if (MyMath.ChanceSuccess(0.9))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721535;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 92)
                    {
                        if (MyMath.ChanceSuccess(0.8))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721536;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region ExpPot
                    else if (MobID == 244)
                    {
                        if (MyMath.ChanceSuccess(75))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 723017;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        if (MyMath.ChanceSuccess(30))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 722384;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 247)
                    {
                        if (MyMath.ChanceSuccess(85))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 723017;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        if (MyMath.ChanceSuccess(30))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 722384;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;

                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 151)
                    {

                        if (MyMath.ChanceSuccess(100))//change 100 to what ever u want the droprate to be
                        {
                            Char.StatEff.Add(StatusEffectEn.SparkleHalo);
                        }
                    }
                    #endregion

                    #region AncientDevil Amulets
                    else if (Name == "WarriorDevil")
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710016;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;

                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (Name == "TrojanDevil")
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710017;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;

                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (Name == "FireDevil")
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710018;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;

                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (Name == "WaterDevil" && Loc.Map == 1082)
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710019;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;

                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (Name == "ArcherDevil")
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710020;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;

                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    #endregion
                    #region AncientDevil Rewards
                    else if (MobID == 8423)
                    {
                        #region Drops
                        for (int a = 0; a < 20; a++)
                        {
                            if (MyMath.ChanceSuccess(15))
                            {
                                DI2 = Drop(Owner, true);
                                DI2.Info.ID = (uint)(721330); //MilGis
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(15))
                            {
                                DI2 = Drop(Owner, true);
                                DI2.Info.ID = (uint)(721331); // Mil.Ginseng
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(15))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(721333); //ChanTP
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(15))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(721332); //ChanTP
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(5))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(725018 + (uint)(Rnd.Next(1, 7))); //Dance
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(20) && a < 5)
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1088000); // DBs
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                //if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(65) && a < 10)
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1088001); // Meteors
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;

                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                //if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                        #endregion
                        #region Gold
                        int Gold;
                        for (int i = 0, j = 0; i < 30 && j < 50; i++, j++)
                        {
                            Gold = Rnd.Next(425000, 1625000);
                            Gold /= 100;
                            DI2 = Drop(0, true);

                            if (MyMath.ChanceSuccess(75))
                            {
                                DI2.Silvers = (uint)Gold;
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

                                if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                                {
                                    i--;
                                    continue;
                                }
                                DI2.Drop();
                                if (!World.GoldSource.ContainsKey(Name))
                                    World.GoldSource.Add(Name, 0);
                                World.GoldSource[Name] += DI2.Silvers;
                            }
                        }

                        #endregion
                    }
                    #endregion
                    #region GuildBeast Rewards
                    else if (MobID == 501)
                    {
                        #region Drops
                        for (int a = 0; a < 20; a++)
                        {
                            if (MyMath.ChanceSuccess(70))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1088001); // Meteors
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(70) && a < 10)
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1088000); // DBs
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                        #endregion
                        #region Gold
                        int Gold;
                        for (int i = 0, j = 0; i < 30 && j < 50; i++, j++)
                        {
                            Gold = Rnd.Next(425000, 1625000);
                            Gold /= 100;
                            DI2 = Drop(0, true);
                            if (DI2.FindPlace(World.H_Items[Loc.Map]))
                            {
                                if (MyMath.ChanceSuccess(75))
                                {
                                    DI2.Silvers = (uint)Gold;
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
                                    DI2.Drop();

                                    if (!World.GoldSource.ContainsKey(Name))
                                        World.GoldSource.Add(Name, 0);
                                    World.GoldSource[Name] += DI2.Silvers;
                                }
                            }
                            else i--;
                        }

                        #endregion
                    }
                    #endregion
                    #region Bomb,FOH
                    else if (MobID == 54 || MobID == 55)
                    {
                        //1060101 FOH
                        //1060100 Bomb
                        if (MyMath.ChanceSuccess(0.09))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1060101; //FoH
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                            if (Char != null)
                                if (Char.VipLevel == 4 || Char.VipLevel == 6)
                                    Char.MyClient.LocalMessage(2000, "A FireOfHell magic scroll dropped from a monster that you killed!");
                        }
                        else if (MyMath.ChanceSuccess(0.11))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1060100; //Bomb
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                            if (Char != null)
                                if (Char.VipLevel == 4 || Char.VipLevel == 6)
                                    Char.MyClient.LocalMessage(2000, "A BombScroll dropped from a monster that you killed!");
                        }
                    }
                    #endregion
                    #region Real MeteorDove
                    else if (MobID == 231)
                    {
                        bool Drop2 = true;
                        DI2 = Drop(Owner);

                        if (MyMath.ChanceSuccess(5))
                        {
                            if (Drop2)
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 1088000;
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                                if (Char != null)
                                    World.SendMsgToAll("SYSTEM", "A DragonBall was dropped by the monster killed by " + Char.Name + "!", 2005, 0/*, Loc.Map*/);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (Drop2)
                                {
                                    DI2 = Drop(Owner);
                                    DI2.Info.ID = 1088001;
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;
                                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                    DI2.Drop();
                                }
                            }
                        }
                    }
                    #endregion
                    #region DarkHorn
                    else if (MobID == 701)
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 790001;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    #endregion
                    #region HealthWine
                    else if (MobID == 71 || MobID == 72)
                    {
                        if (MyMath.ChanceSuccess(1.5))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 723030;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region Sulphur Cateran & ArmyToken
                    else if (MobID == 19)
                    {
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721263;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        if (MyMath.ChanceSuccess(1))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721117;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region NightDevil
                    else if ((MobID == 12 || MobID == 265))
                    {
                        if (MyMath.ChanceSuccess(0.0032))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = (uint)(725016);
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                            if (Char.VipLevel == 4 || Char.VipLevel == 6)
                            {
                                Char.MyClient.LocalMessage(2000, "A NightDevil magic scroll dropped from a monster that you killed!");
                            }
                        }
                    }
                    else if (MobID == 600)
                    {
                        if (MyMath.ChanceSuccess(0.074))
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = (uint)(725016);
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                            if (Char.VipLevel == 4 || Char.VipLevel == 6)
                                Char.MyClient.LocalMessage(2000, "A NightDevil magic scroll dropped from a monster that you killed!");
                        }
                    }
                    #endregion
                    #region TeratoDragon
                    else if (MobID == 4152)
                    {
                        #region Drops
                        for (int a = 0; a < 25; a++)//Max DBs
                        {
                            if (a < 7 && MyMath.ChanceSuccess(50))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = (uint)(720028); // DBScrolls
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                //if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (a < 15 && MyMath.ChanceSuccess(85))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(720027); //metscrolls
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (a < 10 && MyMath.ChanceSuccess(58))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1080001); // emeralds
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (a < 10 && MyMath.ChanceSuccess(62))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(721080); //Moonbox
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (MyMath.ChanceSuccess(70))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(1088000); // DragonBalls
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (a < 3 && MyMath.ChanceSuccess(15))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = (uint)(725016); //nd
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                            if (a < 5 && MyMath.ChanceSuccess(20))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = (uint)(721259); //CelestialStone
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                        #endregion
                        #region Gold
                        int Gold;
                        //   Gold /= 10;
                        for (int i = 0, j = 0; i < 80 && j < 120; i++, j++)
                        {
                            Gold = Rnd.Next(425000, 1625000);
                            Gold /= 10;
                            DI2 = Drop(0, true);
                            if (DI2.FindPlace(World.H_Items[Loc.Map]))
                            {
                                if (MyMath.ChanceSuccess(75))
                                {
                                    DI2.Silvers = (uint)Gold;
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
                                    DI2.Drop();

                                    if (!World.GoldSource.ContainsKey(Name))
                                        World.GoldSource.Add(Name, 0);
                                    World.GoldSource[Name] += DI2.Silvers;
                                }
                            }
                            else i--;
                        }

                        #endregion
                    }
                    #endregion
                    #region Bosses
                    else if (MobID == 3821 || MobID == 3822 || MobID == 3823 || MobID == 4172)
                    {
                        #region Drops
                        List<uint> Garments = new List<uint>();
                        foreach (DatabaseItem D in Database.DatabaseItems.Values)
                        {
                            if (MobID == 3821)
                            {
                                if (ItemIDManipulation.Part(D.ID, 0, 3) == 195 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 196 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 197 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 198)
                                    if (ItemIDManipulation.Part(D.ID, 3, 4) != 2)
                                        if (ItemIDManipulation.Part(D.ID, 4, 6) == 20 ||
                                            ItemIDManipulation.Part(D.ID, 4, 6) == 30)
                                            Garments.Add(D.ID);
                            }
                            else if (MobID == 3822)
                            {
                                if (ItemIDManipulation.Part(D.ID, 0, 3) == 195 ||
                                       ItemIDManipulation.Part(D.ID, 0, 3) == 196 ||
                                       ItemIDManipulation.Part(D.ID, 0, 3) == 197 ||
                                       ItemIDManipulation.Part(D.ID, 0, 3) == 198)
                                {
                                    if (ItemIDManipulation.Part(D.ID, 3, 4) != 2)
                                        if (ItemIDManipulation.Part(D.ID, 4, 6) == 10)
                                            Garments.Add(D.ID);
                                }
                                else if (ItemIDManipulation.Part(D.ID, 0, 3) == 199)
                                    if (ItemIDManipulation.Part(D.ID, 4, 6) == 00)
                                        Garments.Add(D.ID);
                            }
                            else if (MobID == 3823)
                            {
                                if (ItemIDManipulation.Part(D.ID, 0, 3) == 195 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 196 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 197 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 198)
                                    if (ItemIDManipulation.Part(D.ID, 3, 4) != 2)
                                        if (ItemIDManipulation.Part(D.ID, 4, 6) == 60 ||
                                            ItemIDManipulation.Part(D.ID, 4, 6) == 70)
                                            Garments.Add(D.ID);
                            }
                            else if (MobID == 4172)
                            {
                                if (ItemIDManipulation.Part(D.ID, 0, 3) == 195 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 196 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 197 ||
                                        ItemIDManipulation.Part(D.ID, 0, 3) == 198)
                                    if (ItemIDManipulation.Part(D.ID, 3, 4) != 2)
                                        if (ItemIDManipulation.Part(D.ID, 4, 6) == 80 ||
                                            ItemIDManipulation.Part(D.ID, 4, 6) == 90)
                                            Garments.Add(D.ID);
                            }
                        }
                        if (Garments != null)
                        {
                            List<string> Rewards = new List<string>();
                            if (Garments.Count > 0)
                                for (byte a = 0; a < 2; a++)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garments.Count);
                                    DI2 = Drop(0, true);
                                    DI2.Info.ID = (uint)((uint)Garments[Tries]); // Garment
                                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                    DI2.Info.CurDur = DI2.Info.MaxDur;
                                    if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) continue;
                                    DI2.Drop();
                                    Rewards.Add(DI2.Info.DBInfo.Name);
                                }
                            if (Rewards.Count == 1)
                                World.SendMsgToAll("SYSTEM", Name + " was killed and it dropped a " + Rewards[0] + " Armor-Garment! Make sure you join the next boss!", 2011, 0);
                            else if (Rewards.Count == 2)
                                World.SendMsgToAll("SYSTEM", Name + " was killed and it dropped a " + Rewards[0] + " and a " + Rewards[1] + " Armor-Garments! Make sure you join the next boss!", 2011, 0);
                        }
                        for (byte a = 0; a < 5; a++)
                        {
                            if (a <= 2 || (a > 2 && MyMath.ChanceSuccess(70)))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(720028); // DragonBalls
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) continue;
                                DI2.Drop();
                            }
                        }
                        for (byte a = 0; a < 5; a++)
                        {
                            if (a == 0 || (a > 0 && MyMath.ChanceSuccess(50)))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = (uint)(721259); //CelestialStone
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) continue;
                                DI2.Drop();
                            }
                        }
                        for (byte a = 0; a < 5; a++)
                        {
                            if (a == 0 || (a > 0 && MyMath.ChanceSuccess(50)))
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = (uint)(721080); //Moonbox
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) continue;
                                DI2.Drop();
                            }
                        }
                        for (byte a = 0; a < 25; a++)
                        {
                            if (a < 13 || a >= 13 && MyMath.ChanceSuccess(60))
                            {
                                DI2 = Drop(0, true);
                                DI2.Info.ID = (uint)(72027); //Meteors
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace((System.Collections.Concurrent.ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[Loc.Map])) continue;
                                DI2.Drop();
                            }
                        }
                        #endregion
                        #region Gold
                        int Gold;
                        //   Gold /= 10;
                        for (int i = 0, j = 0; i < 80 && j < 120; i++, j++)
                        {
                            Gold = Rnd.Next(10000, 50000);
                            Gold /= 10;
                            DI2 = Drop(0, true);
                            if (DI2.FindPlace(World.H_Items[Loc.Map]))
                            {
                                if (MyMath.ChanceSuccess(75))
                                {
                                    DI2.Silvers = (uint)Gold;
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
                                    DI2.Drop();

                                    if (!World.GoldSource.ContainsKey(Name))
                                        World.GoldSource.Add(Name, 0);
                                    World.GoldSource[Name] += DI2.Silvers;
                                }
                            }
                            else i--;
                        }

                        #endregion
                    }
                    #endregion
                    #region GuildChest //Commented
                    //else if (MobID == 500)
                    //{
                    //    int Typ = Rnd.Next(3);
                    //    //0-Normal  1-Super 2-KillAll

                    //    //0- Gold  1- Items 2- Mets 3- DBs 4- Garments 5- Nothing
                    //    if (Typ == 2)
                    //    {
                    //        #region KillAll
                    //        foreach (Character C in World.H_Chars.Values)
                    //        {

                    //            if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28))
                    //            {
                    //                C.AtkMem.Attacking = false;
                    //                C.AtkMem.Target = 0;
                    //                C.DeathHit = DateTime.Now;
                    //                C.Alive = false;
                    //                C.CurHP = 0;
                    //                World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "change").Get);
                    //                Game.World.Action(C, Packets.AttackPacket(0, C.EntityID, C.Loc.X, C.Loc.Y, 0, (byte)Game.AttackType.Kill).Get);

                    //                foreach (Buff B in C.Buffs.Keys)
                    //                    C.BDelete.TryAdd(B, B.Lasts);
                    //                C.BlueName = false;
                    //                C.StatEff.Add(StatusEffectEn.Dead);
                    //                C.PoisonedInfo.Times = 0;
                    //            }
                    //        }
                    //        World.SendMsgToAll("SYSTEM", "GuildChest was a trap! Everybody near it got killed!", 2011, 0, Loc.Map);
                    //        return;
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        int x;
                    //        bool Super = false;
                    //        if (Typ == 0)
                    //        {
                    //            x = Rnd.Next(4);
                    //            if (MyMath.ChanceSuccess(10))
                    //                x = 4;
                    //        }
                    //        else { x = Rnd.Next(4); Super = true; }
                    //        if (x == 0)
                    //        {
                    //            #region Gold
                    //            int Gold;
                    //            if (Super)
                    //                Gold = Rnd.Next(17000000, 27000000);
                    //            else Gold = Rnd.Next(7000000, 17000000);
                    //            Gold /= 70;
                    //            for (int i = 0, j = 0; i < 70 && j < 100; i++, j++)
                    //            {
                    //                DI2 = Drop(0, true);
                    //                if (DI2.FindPlace(World.H_Items[Loc.Map]))
                    //                {
                    //                    DI2.Silvers = (uint)Gold;
                    //                    if (DI2.Silvers < 10)
                    //                        DI2.Info.ID = 1090000;
                    //                    else if (DI2.Silvers < 100)
                    //                        DI2.Info.ID = 1090010;
                    //                    else if (DI2.Silvers < 1000)
                    //                        DI2.Info.ID = 1090020;
                    //                    else if (DI2.Silvers < 3000)
                    //                        DI2.Info.ID = 1091000;
                    //                    else if (DI2.Silvers < 10000)
                    //                        DI2.Info.ID = 1091010;
                    //                    else
                    //                        DI2.Info.ID = 1091020;
                    //                    DI2.Drop();
                    //                }
                    //                else i--;
                    //            }
                    //            return;
                    //            #endregion
                    //        }
                    //        else if (x == 1)
                    //        {
                    //            #region Items
                    //            int times;
                    //            if (Super)
                    //                times = 50;
                    //            else times = 35;
                    //            for (int i = 0, j = 0; i < times && j < 80; i++, j++)
                    //            {
                    //                DI2 = Drop(0, true);
                    //                if (DI2.FindPlace(World.H_Items[Loc.Map]))
                    //                {

                    //                    Item.ItemQuality Q;
                    //                    if (Super)
                    //                        Q = Item.ItemQuality.Unique;
                    //                    else Q = Item.ItemQuality.Refined;
                    //                    double super = 0;
                    //                    double elite = 0;
                    //                    if (Super)
                    //                    {
                    //                        super = DropRates.Super / 2;
                    //                        elite = DropRates.Elite / 2;
                    //                    }
                    //                    if (MyMath.ChanceSuccess(DropRates.Super + super))
                    //                        Q = Item.ItemQuality.Super;
                    //                    else if (MyMath.ChanceSuccess(DropRates.Elite + elite))
                    //                        Q = Item.ItemQuality.Elite;
                    //                    uint ItemID = 0;
                    //                    List<uint> From = new List<uint>();
                    //                    int Type = Rnd.Next(0, 170);
                    //                    uint Part = 0;
                    //                    if (Type < 10) Part = 111;
                    //                    else if (Type < 20) Part = 113;
                    //                    else if (Type < 30) Part = 114;
                    //                    else if (Type < 40) Part = 117;
                    //                    else if (Type < 50) Part = 118;
                    //                    else if (Type < 60) Part = 120;
                    //                    else if (Type < 70) Part = 121;
                    //                    else if (Type < 80) Part = 130;
                    //                    else if (Type < 90) Part = 131;
                    //                    else if (Type < 100) Part = 133;
                    //                    else if (Type < 110) Part = 134;
                    //                    else if (Type < 120) Part = 141;
                    //                    else if (Type < 130) Part = 142;
                    //                    else if (Type < 140) Part = 150;
                    //                    else if (Type < 150) Part = 151;
                    //                    else if (Type < 160) Part = 152;
                    //                    else if (Type < 165) Part = 160;
                    //                    else if (Type < 170) Part = 900;

                    //                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                    //                    {
                    //                        if (D.LevReq >= 10 && D.LevReq <= 60)
                    //                        {
                    //                            if (D.LevReq != 0)
                    //                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                    //                                    From.Add(D.ID);
                    //                        }
                    //                    }
                    //                    if (From != null)
                    //                    {
                    //                        if (From.Count > 0)
                    //                        {
                    //                            byte Tries = (byte)Rnd.Next(0, From.Count);
                    //                            ItemID = (uint)From[Tries];
                    //                        }
                    //                    }
                    //                    if (ItemID != 0)
                    //                    {
                    //                        DI2.Info.ID = ItemID;
                    //                        if (DI2.Info.DBInfo.LevReq != 1)
                    //                        {
                    //                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                    //                            E.QualityChange(Q);
                    //                            DI2.Info.ID = E.ToID();
                    //                        }

                    //                        DI2.Info.Color = Item.ArmorColor.Orange;

                    //                        if (Super)
                    //                        {
                    //                            if (MyMath.ChanceSuccess(0.1))
                    //                            {
                    //                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                    //                                DI2.Info.Soc2 = Item.Gem.EmptySocket;
                    //                            }
                    //                            else if (MyMath.ChanceSuccess(0.4))
                    //                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                    //                        }
                    //                        else
                    //                        {
                    //                            if (MyMath.ChanceSuccess(0.05))
                    //                            {
                    //                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                    //                                DI2.Info.Soc2 = Item.Gem.EmptySocket;
                    //                            }
                    //                            else if (MyMath.ChanceSuccess(0.2))
                    //                                DI2.Info.Soc1 = Item.Gem.EmptySocket;
                    //                        }

                    //                        double plus = 0;
                    //                        if (Super)
                    //                            plus = DropRates.PlusOne / 3;
                    //                        if (MyMath.ChanceSuccess(DropRates.PlusOne + plus + 0.1))
                    //                            DI2.Info.Plus = 1;
                    //                        if (MyMath.ChanceSuccess(0.05))
                    //                            if (MyMath.ChanceSuccess(10))
                    //                                DI2.Info.Bless = 5;
                    //                            else if (MyMath.ChanceSuccess(30))
                    //                                DI2.Info.Bless = 3;
                    //                        //else DI2.Info.Bless = 1;
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;


                    //                        if (DI2.Info.MaxDur != 0)
                    //                            if (DI2.Info.DBInfo.LevReq < 40)
                    //                                DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.5), (int)(DI2.Info.MaxDur * 0.8));
                    //                            else if (DI2.Info.ID % 10 > 5)
                    //                                DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.15), (int)(DI2.Info.MaxDur * 0.35));
                    //                            else DI2.Info.CurDur = (ushort)Rnd.Next((int)(DI2.Info.MaxDur * 0.01), (int)(DI2.Info.MaxDur * 0.1));
                    //                        else DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    }
                    //                    DI2.Drop();
                    //                }
                    //                else i--;
                    //            }
                    //            return;
                    //            #endregion
                    //        }
                    //        else if (x == 2 || x == 3)
                    //        {
                    //            #region Gems||DBs
                    //            int times;
                    //            uint id = 2;
                    //            if (x == 2)
                    //            {
                    //                if (Super)
                    //                    times = 25;
                    //                else times = 15;
                    //            }
                    //            else
                    //            {
                    //                if (World.LowRatedServer)
                    //                    if (Super)
                    //                        times = 20;
                    //                    else
                    //                        times = 10;
                    //                else
                    //                    if (Super) times = 30;
                    //                else times = 20;
                    //                id = 1088000;
                    //            }
                    //            for (int i = 0, j = 0; i < times && j < 200; i++, j++)
                    //            {
                    //                DI2 = Drop(0, true);
                    //                if (DI2.FindPlace(World.H_Items[Loc.Map]))
                    //                {
                    //                    while (id % 10 == 2)
                    //                        id = (uint)Rnd.Next(0, 7);
                    //                    if (id < 7)
                    //                        id = 700002 + (id * 10);
                    //                    DI2.Info.ID = id;
                    //                    DI2.Drop();
                    //                }
                    //                else i--;

                    //            }
                    //            return;
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            World.SendMsgToAll("SYSTEM", "GuildChest was a fake one! It didn't drop anything but luckily it was not a trap!", 2011, 0, Loc.Map);
                    //            return;
                    //        }
                    //    }
                    //}
                    #endregion
                    #region Lab Bosses
                    else if (MobID == 85) //Gibon 1 SunBox and 1 DBs
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 721541;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();

                        for (int i = 0; i < 3; i++)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1088000;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710212;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();


                    }
                    else if (MobID == 88) //NagaLord 1 WaningMoonBox and 1 DBs
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 721542;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();

                        for (int i = 0; i < 3; i++)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1088000;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710212;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (MobID == 91) //Talon 1 StarBox and 1 DBs
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 721543;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();

                        for (int i = 0; i < 3; i++)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1088000;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 710212;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();
                    }
                    else if (MobID == 93) //Howler 1 AncestorBox and 1 DBs
                    {
                        DI2 = Drop(Owner);
                        DI2.Info.ID = 721540;
                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                        DI2.Drop();

                        for (int i = 0; i < 2; i++)
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 1088000;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region Desunoto
                    else if (MobID == 8424)
                    {

                        if (Char != null)
                        {
                            World.SendMsgToAll("CCGW", Char.Name + " has defeated the Desunoto and obtained the CCGWBomb! Be careful now!", 2011, 0, 1844);
                            if (Char.Inventory.Count <= 39)
                                Char.AddItem(721246);
                            else
                            {
                                DI2 = Drop(Owner);
                                DI2.Info.ID = 721246;
                                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                DI2.Info.CurDur = DI2.Info.MaxDur;
                                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                DI2.Drop();
                            }
                        }
                        else
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721246;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region SkyPass
                    else if (MobID == 8425)
                    {
                        if (MyMath.ChanceSuccess(1.12))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721100;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 8426)
                    {
                        if (MyMath.ChanceSuccess(0.92))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721101;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 8427)
                    {
                        if (MyMath.ChanceSuccess(0.82))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721102;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 8428)
                    {
                        if (MyMath.ChanceSuccess(0.77))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721103;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    else if (MobID == 8429)
                    {
                        if (MyMath.ChanceSuccess(0.75))//change 100 to what ever u want the droprate to be
                        {
                            DI2 = Drop(Owner);
                            DI2.Info.ID = 721108;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region RatFang Quest //MobID >= 20 && MobID <= 23
                    else if (MobID >= 20 && MobID <= 23)
                    {
                        if (MyMath.ChanceSuccess(DropRates.Meteor / 3))
                        {
                            DI2.Info.ID = 721120;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }

                    }
                    #endregion
                    #region MoonSpring Quest //MobID == 122
                    else if (MobID == 122)
                    {
                        if (MyMath.ChanceSuccess(3))
                        {
                            DI2.Info.ID = 721128;
                            DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                            DI2.Info.CurDur = DI2.Info.MaxDur;
                            if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                            DI2.Drop();
                        }
                    }
                    #endregion
                    #region DemonBoxes
                    else if (MobID == 800)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 50000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 1350000 && MyMath.ChanceSuccess(0.4))
                            {
                                if (World.demonBoxesCur * 0.7 >= 2700000 && MyMath.ChanceSuccess(40))
                                {
                                    DI2.Info.ID = 720657;
                                    World.demonBoxesCur -= 2700000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a DemonBox and found a JoyGoldPack(2,700,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720656;
                                    World.demonBoxesCur -= 1350000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a DemonBox and found a DreamGoldPack(1,350,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 70))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a DemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720658;
                                World.demonBoxesCur -= 5000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 100000)
                            {
                                DI2.Info.ID = 720655;
                                World.demonBoxesCur -= 100000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720654;
                                World.demonBoxesCur -= 50000;
                            }
                            else
                            {
                                DI2.Info.ID = 720653;
                                World.demonBoxesCur -= 25000;
                            }
                        }
                        else
                            DI2.Info.ID = 720658;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 801)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 100000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 3000000 && MyMath.ChanceSuccess(0.4))
                            {
                                if (World.demonBoxesCur * 0.7 >= 5400000 && MyMath.ChanceSuccess(5))
                                {
                                    DI2.Info.ID = 720663;
                                    World.demonBoxesCur -= 5400000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AncientDemonBox and found a MysticGoldPack(5,400,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720662;
                                    World.demonBoxesCur -= 3000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AncientDemonBox and found a PureGoldPack(3,000,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 65))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AncientDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720664;
                                World.demonBoxesCur -= 10000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 200000)
                            {
                                DI2.Info.ID = 720661;
                                World.demonBoxesCur -= 200000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720660;
                                World.demonBoxesCur -= 100000;
                            }
                            else
                            {
                                DI2.Info.ID = 720659;
                                World.demonBoxesCur -= 50000;
                            }
                        }
                        else
                            DI2.Info.ID = 720664;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 802)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 500000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 13500000 && MyMath.ChanceSuccess(0.4))
                            {
                                if (World.demonBoxesCur * 0.7 >= 27000000 && MyMath.ChanceSuccess(5))
                                {
                                    DI2.Info.ID = 720669;
                                    World.demonBoxesCur -= 27000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a FloodDemonBox and found a FantasyGoldPack(27,000,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720668;
                                    World.demonBoxesCur -= 13500000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a FloodDemonBox and found a StarGoldPack(13,500,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 60))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a FloodDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720670;
                                World.demonBoxesCur -= 50000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 1000000)
                            {
                                DI2.Info.ID = 720667;
                                World.demonBoxesCur -= 1000000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720666;
                                World.demonBoxesCur -= 500000;
                            }
                            else
                            {
                                DI2.Info.ID = 720665;
                                World.demonBoxesCur -= 250000;
                            }
                        }
                        else
                            DI2.Info.ID = 720670;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 803)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 1000000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 27000000 && MyMath.ChanceSuccess(0.4))
                            {
                                if (World.demonBoxesCur * 0.7 >= 54000000 && MyMath.ChanceSuccess(5))
                                {
                                    DI2.Info.ID = 720679;
                                    World.demonBoxesCur -= 54000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a HeavenDemonBox and found a FrostGoldPack(54,000,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720678;
                                    World.demonBoxesCur -= 27000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a HeavenDemonBox and found a LifeGoldPack(27,000,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 55))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a HeavenDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720680;
                                World.demonBoxesCur -= 100000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 2000000)
                            {
                                DI2.Info.ID = 720677;
                                World.demonBoxesCur -= 2000000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720676;
                                World.demonBoxesCur -= 1000000;
                            }
                            else
                            {
                                DI2.Info.ID = 720675;
                                World.demonBoxesCur -= 500000;
                            }
                        }
                        else
                            DI2.Info.ID = 720680;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 804)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 5000000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 135000000 && MyMath.ChanceSuccess(0.2))
                            {
                                if (World.demonBoxesCur * 0.7 >= 270000000 && MyMath.ChanceSuccess(3))
                                {
                                    DI2.Info.ID = 720685;
                                    World.demonBoxesCur -= 270000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a ChaosDemonBox and found a NimbusGoldPack(270,000,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720684;
                                    World.demonBoxesCur -= 135000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a ChaosDemonBox and found a ButterflyGoldPack(135,000,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 50))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a ChaosDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720686;
                                World.demonBoxesCur -= 500000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 10000000)
                            {
                                DI2.Info.ID = 720683;
                                World.demonBoxesCur -= 10000000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720682;
                                World.demonBoxesCur -= 5000000;
                            }
                            else
                            {
                                DI2.Info.ID = 720681;
                                World.demonBoxesCur -= 2500000;
                            }
                        }
                        else
                            DI2.Info.ID = 720686;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 805)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 10000000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 270000000 && MyMath.ChanceSuccess(0.2))
                            {
                                if (World.demonBoxesCur * 0.7 >= 540000000 && MyMath.ChanceSuccess(3))
                                {
                                    DI2.Info.ID = 720691;
                                    World.demonBoxesCur -= 540000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a SacredDemonBox and found a KylinGoldPack(540,000,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720690;
                                    World.demonBoxesCur -= 270000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a SacredDemonBox and found a RainbowGoldPack(270,000,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 45))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a SacredDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720692;
                                World.demonBoxesCur -= 1000000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 20000000)
                            {
                                DI2.Info.ID = 720689;
                                World.demonBoxesCur -= 20000000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720688;
                                World.demonBoxesCur -= 10000000;
                            }
                            else
                            {
                                DI2.Info.ID = 720687;
                                World.demonBoxesCur -= 5000000;
                            }
                        }
                        else
                            DI2.Info.ID = 720692;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    else if (MobID == 806)
                    {
                        DI2 = Drop(Owner);
                        if (World.demonBoxesCur > 20000000)
                        {
                            if (World.demonBoxesCur * 0.7 >= 540000000 && MyMath.ChanceSuccess(0.2))
                            {
                                if (World.demonBoxesCur * 0.7 >= 1080000000 && MyMath.ChanceSuccess(3))
                                {
                                    DI2.Info.ID = 720697;
                                    World.demonBoxesCur -= 1080000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AuroraDemonBox and found a OpalGoldPack(1,080,000,000 Silvers)!", 2011, 0);
                                }
                                else
                                {
                                    DI2.Info.ID = 720696;
                                    World.demonBoxesCur -= 540000000;
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AuroraDemonBox and found a RainbowGoldPack(540,000,000 Silvers)!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(1 / 40))
                            {
                                List<uint> Garms = new List<uint>() { 187315, 187345, 187355, 187365, 187465, 187455, 187505, 187605, 187635, 187665, 188165, 188545, 188755, 192125, 192185, 192325 };
                                if (Garms.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, Garms.Count);
                                    DI2.Info.ID = (uint)((uint)Garms[Tries]); // Garment
                                    if (Char != null)
                                        World.SendMsgToAll("SYSTEM", Char.Name + " has used a AuroraDemonBox and found a " + DI2.Info.DBInfo.Name + "!", 2011, 0);
                                }
                            }
                            else if (MyMath.ChanceSuccess(25))
                            {
                                DI2.Info.ID = 720698;
                                World.demonBoxesCur -= 2000000;
                            }
                            else if (MyMath.ChanceSuccess(50) && World.demonBoxesCur > 40000000)
                            {
                                DI2.Info.ID = 720695;
                                World.demonBoxesCur -= 40000000;
                            }
                            else if (MyMath.ChanceSuccess(50))
                            {
                                DI2.Info.ID = 720694;
                                World.demonBoxesCur -= 20000000;
                            }
                            else
                            {
                                DI2.Info.ID = 720693;
                                World.demonBoxesCur -= 10000000;
                            }
                        }
                        else
                            DI2.Info.ID = 720698;

                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                        DI2.Info.CurDur = DI2.Info.MaxDur;
                        if (!DI2.FindPlace(World.H_Items[Loc.Map]))
                        {
                            DI2.Loc.X = Loc.X;
                            DI2.Loc.Y = Loc.Y;
                        }
                        DI2.Drop();
                    }
                    #endregion

                    //Disabled
                    #region Lilies (disabled)
                    /*      if (Loc.Map == 1002 && DateTime.Now.Month == 2 && DateTime.Now.Day <= 20*)
                          {
                              double flower1 = 0.3;
                              double flower3 = 0.2;
                              double flower9 =0.1;
                              double flower99 =0.08;
                              double flower999 = 0.01;
                              if (Char != null)
                              {
                                  if (Char.Job >= 40 && Char.Job <= 45)
                                  {
                                      flower1 = 0.15;
                                      flower3 = 0.1;
                                      flower9 = 0.05;
                                      flower99 = 0.03;
                                      flower999 = 0.005;
                                  }
                              }
                              int I = Rnd.Next(0, 4);
                              if (MyMath.ChanceSuccess(flower999))
                              {
                                  uint[] IDS = new uint[4] {752999, 753999, 754999, 751999};
                                  DI2 = Drop(Owner);
                                  DI2.Info.ID = (uint)(IDS[I]);// 753999  754999  751999 
                                  DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                  DI2.Info.CurDur = DI2.Info.MaxDur;
                                  if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                  DI2.Drop();
                              }
                              else if (MyMath.ChanceSuccess(flower99))
                              {
                                  uint[] IDS = new uint[4] { 751099, 752099, 753099, 754099 };
                                  //751099  752099  753099  754099 
                                  DI2 = Drop(Owner);
                                  DI2.Info.ID = (uint)(IDS[I]);// 753999  754999  751999 
                                  DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                  DI2.Info.CurDur = DI2.Info.MaxDur;
                                  if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                  DI2.Drop();
                              }
                              else if (MyMath.ChanceSuccess(flower9))
                              {
                                  uint[] IDS = new uint[4] { 752009, 751009, 754009, 753009 };
                                  //752009  751009 754009  753009 
                                  DI2 = Drop(Owner);
                                  DI2.Info.ID = (uint)(IDS[I]);// 753999  754999  751999 
                                  DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                  DI2.Info.CurDur = DI2.Info.MaxDur;
                                  if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                  DI2.Drop();
                              }
                              else if (MyMath.ChanceSuccess(flower3))
                              {
                                  uint[] IDS = new uint[4] { 751003, 752003, 753003, 754003 };
                                  //751003  752003  753003  754003 
                                  DI2 = Drop(Owner);
                                  DI2.Info.ID = (uint)(IDS[I]);// 753999  754999  751999 
                                  DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                  DI2.Info.CurDur = DI2.Info.MaxDur;
                                  if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                  DI2.Drop();
                              }
                              else if (MyMath.ChanceSuccess(flower1))
                              {
                                  uint[] IDS = new uint[4] { 751001, 752001, 753001, 754001 };
                                  //751001  752001  753001  754001 
                                  DI2 = Drop(Owner);
                                  DI2.Info.ID = (uint)(IDS[I]);// 753999  754999  751999 
                                  DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                                  DI2.Info.CurDur = DI2.Info.MaxDur;
                                  if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                                  DI2.Drop();
                              }
                          } */
                    #endregion
                    #region Gump Drops 01 (CloudDiamonds & CloudBoxes) - Disabled by Joao
                    //if (MyMath.ChanceSuccess(45))
                    //{
                    //    if (MobID >= 1 && MobID <= 26) // Pheasants to PC mobs 
                    //    {
                    //        if (MyMath.ChanceSuccess(0.0038)) // roll for CloudDiamond
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(721536);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!" , 2005, 0);
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 721536; // CloudDiamond
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //            }

                    //        }
                    //        else if (MyMath.ChanceSuccess(0.00016)) // Roll for CloudBox (Pheasants to PC Mobs) (Only roll if CloudDiamond fail) 
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(722685);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 722685; // CloudBox
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //            }
                    //        }
                    //    }
                    //    else if (MobID >= 27 && MobID <= 5999) // AC mobs, BI mobs and all other mobs (excluding new gump mobs) 
                    //    {
                    //        if (MyMath.ChanceSuccess(0.0045)) 
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(721536);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 721536; // CloudDiamond
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //            }
                    //        }
                    //        else if (MyMath.ChanceSuccess(0.00028)) // Roll for CloudBox (AC to BI + all other) (Only roll if CloudDiamond fail)
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(722685);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 722685; // CloudBox
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //            }
                    //        }
                    //    }
                    //    else if (MobID == 6059 || MobID == 6058 || MobID == 257) // American Rooster, EliteBandit, HugeSnake 0.007
                    //    {
                    //        if (Loc.Map != 1070)
                    //        {
                    //            if (MyMath.ChanceSuccess(0.0074))
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(721536);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 721536; // CloudDiamond
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else if (MyMath.ChanceSuccess(0.00045)) // Roll for CloudBox (AmericanRooster, EliteBandit, HugeSnake) (Only roll if CloudDiamond fail)
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(722685);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 722685; // CloudBox
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //        }
                    //        else // location == gw hunters map
                    //        {
                    //            if (MyMath.ChanceSuccess(0.0004))
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(721536);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 721536; // CloudDiamond
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else if (MyMath.ChanceSuccess(0.00005)) // Roll for CloudBox (AmericanRooster, EliteBandit, HugeSnake) (Only roll if CloudDiamond fail)
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(722685);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 722685; // CloudBox
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //        }

                    //    }
                    //    else if (MobID == 6056 || MobID == 6057 || MobID == 6061 || MobID == 6060 || MobID == 6061 || MobID == 6062) // Gumparoo, ZinryusZibbon, SfiNxos, SeniorSandElf, CursedWarrior, ThirstyBasilisk 0.0095
                    //    {
                    //        if (Loc.Map != 1070)
                    //        {
                    //            if (MyMath.ChanceSuccess(0.0099))
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(721536);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 721536; // CloudDiamond
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else if (MyMath.ChanceSuccess(0.0009)) // Roll for CloudBox (ZinryuZibbon, Gumparoo to basilisk + misc) (Only roll if CloudDiamond fail)
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(722685);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 722685; // CloudBox
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //        }
                    //        else // location GW hunters map
                    //        {
                    //            if (MyMath.ChanceSuccess(0.0009))
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(721536);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 721536; // CloudDiamond
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else if (MyMath.ChanceSuccess(0.0001)) // Roll for CloudBox (ZinryuZibbon, Gumparoo to basilisk + misc) (Only roll if CloudDiamond fail)
                    //            {
                    //                if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //                {
                    //                    if (Char.Inventory.Count < 40)
                    //                    {
                    //                        Char.AddItem(722685);
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                    else
                    //                    {
                    //                        DI2 = Drop(Owner);
                    //                        DI2.Info.ID = 722685; // CloudBox
                    //                        DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                        DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                        if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                        DI2.Drop();
                    //                        Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                        World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                        Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //        }

                    //    }
                    //    else if (MobID == 6064) // SarasMinion
                    //    {
                    //        if (MyMath.ChanceSuccess(0.0113)) 
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(721536);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudDiamond from the drops.");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721536; // CloudDiamond
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 721536; // CloudDiamond
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudDiamond!", 2005, 0);
                    //            }
                    //        }
                    //        else if (MyMath.ChanceSuccess(0.0008)) // Roll for CloudBox (SarasMinion) (Only roll if CloudDiamond fail)
                    //        {
                    //            if (Char.VipLevel == 4 || Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(722685);
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 722685; // CloudBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    Char.MyClient.LocalMessage(2005, "You received a CloudBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 722685; // CloudBox
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a CloudBox!", 2011, 0);
                    //                Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a CloudBox! \r\n";
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    #region ClassicBox Drop (GUMP - Disabled by Joao)
                    //else if (MyMath.ChanceSuccess(40))
                    //{
                    //    if (Loc.Map != 1070)
                    //    {
                    //        if (MyMath.ChanceSuccess(0.00033))
                    //        {
                    //            if (Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(721851);
                    //                    Char.MyClient.LocalMessage(2005, "You received a ClassicBox from the drops!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721851; // ClassicBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    Char.MyClient.LocalMessage(2005, "You received a ClassicBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 721851; // ClassicBox
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (MyMath.ChanceSuccess(0.0001))
                    //        {
                    //            if (Char.VipLevel == 5)
                    //            {
                    //                if (Char.Inventory.Count < 40)
                    //                {
                    //                    Char.AddItem(721851);
                    //                    Char.MyClient.LocalMessage(2005, "You received a ClassicBox from the drops!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //                }
                    //                else
                    //                {
                    //                    DI2 = Drop(Owner);
                    //                    DI2.Info.ID = 721851; // ClassicBox
                    //                    DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                    DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                    if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                    DI2.Drop();
                    //                    Char.MyClient.LocalMessage(2005, "You received a ClassicBox from the drops!!! YOUR INVENTORY IS FULL!! IT DROPPED ON THE GROUND!!!");
                    //                    World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                    Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //                }
                    //            }
                    //            else
                    //            {
                    //                DI2 = Drop(Owner);
                    //                DI2.Info.ID = 721851; // ClassicBox
                    //                DI2.Info.MaxDur = DI2.Info.DBInfo.Durability;
                    //                DI2.Info.CurDur = DI2.Info.MaxDur;
                    //                if (!DI2.FindPlace(World.H_Items[Loc.Map])) return;
                    //                DI2.Drop();
                    //                World.SendMsgToAll("SYSTEM", Char.Name + " just killed a " + Name + " and it dropped a ClassicBox!", 2011, 0);
                    //                Game.World.DebugAdd += Char.Name + " just killed a " + Name + " and it dropped a ClassicBox! \r\n";
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }

        public DroppedItem Drop(uint Owner, bool Large = false)
        {
            DroppedItem DI2 = new DroppedItem();
            DI2.DropTime = DateTime.Now;
            DI2.UID = (uint)Rnd.Next(10000000);
            DI2.Loc = new Location();
            if (!Large)
            {
                DI2.Loc.X = (ushort)(Loc.X + Rnd.Next(4) - Rnd.Next(4));
                DI2.Loc.Y = (ushort)(Loc.Y + Rnd.Next(4) - Rnd.Next(4));
            }
            else
            {
                DI2.Loc.X = (ushort)(Loc.X + Rnd.Next(20) - Rnd.Next(20));
                DI2.Loc.Y = (ushort)(Loc.Y + Rnd.Next(20) - Rnd.Next(20));
            }
            DI2.Loc.Map = Loc.Map;
            DI2.Info = new Item();
            DI2.Info.UID = DI2.UID;
            DI2.Owner = Owner;
            return DI2;
        }
        public bool AddMob()
        {
            if (!World.H_Mobs.ContainsKey(Loc.Map))
                World.H_Mobs.TryAdd(Loc.Map, new ConcurrentDictionary<uint, Mob>());
            if (!World.PlayersInMap.ContainsKey(Loc.Map))
            {
                World.PlayersInMap.Add(Loc.Map, new ConcurrentDictionary<uint, Character>());
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map)
                        World.PlayersInMap[Loc.Map].TryAdd(C.EntityID, C);
            }

            EntityID = (uint)Program.Rnd.Next(400000, 500000);
            for (int a = 0; a < 10; a++)
            {
                if (World.H_Mobs[Loc.Map].ContainsKey(EntityID))
                    EntityID = (uint)Program.Rnd.Next(400000, 500000);
                else
                    break;
            }
            if (World.H_Mobs[Loc.Map].ContainsKey(EntityID))
                return false;
            World.H_Mobs[Loc.Map].TryAdd(EntityID, this);
            Alive = true;
            Respawn();
            if (Dropped)
                return false;
            return true;
        }
        public void Respawn()
        {
            try
            {
                Loc.X = (ushort)Program.Rnd.Next(Math.Min(StartLoc.XFrom, StartLoc.XTo), Math.Max(StartLoc.XFrom, StartLoc.XTo));
                Loc.Y = (ushort)Program.Rnd.Next(Math.Min(StartLoc.YFrom, StartLoc.Yto), Math.Max(StartLoc.YFrom, StartLoc.Yto));
                DMap D = (DMap)DMaps.H_DMaps[Loc.Map];

                //  try
                // {
                var count = 0;
                while (D != null && D.GetCell(Loc.X, Loc.Y).NoAccess)
                {
                    Loc.X = (ushort)Program.Rnd.Next(Math.Min(StartLoc.XFrom, StartLoc.XTo), Math.Max(StartLoc.XFrom, StartLoc.XTo));
                    Loc.Y = (ushort)Program.Rnd.Next(Math.Min(StartLoc.YFrom, StartLoc.Yto), Math.Max(StartLoc.YFrom, StartLoc.Yto));
                    count++;
                    if (count > 200 && count < 220 && MobID >= 800 && MobID <= 809)
                    {
                        World.ExcAdd += "Mob Line 5196 took more than 200 tries to find coordinate \r\n";
                        Console.WriteLine("Mob Line 5196 took more than 200 tries to find coordinate");
                        Dropped = true;
                        return;
                    }
                }
                // }
                // catch { Game.World.ExcAdd += "Mobid: " + MobID + " Mob.X: " + Loc.X + " Mob.Y: " + Loc.Y + " Mob.map: " + Loc.Map + " MOBERROR! \r\n"; }
                //  Loc = StartLoc;
                LastTarget = DateTime.Now;
                Alive = true;
                CurrentHP = MaxHP;
                Action = 100;
                World.Spawn(this, false);
                World.Action(this, Packets.StringPacket(EntityID, StringType.Effect, "MBStandard").Get);
                Dissappeared = false;
                Dropped = false;
            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        public void Step()
        {
            try
            {
                if (!_UltimateBoss() && MobID != 501 && MobID != 244 && MobID != 247 && MobID != 700 && MobID != 701 && MobID != 8423 && MobID != 8424 && MobID != 704 && MobID != 300 && MobID != 150 && MobID != 500 &&/* MobID != 409 &&*/ MobID != 4152 && MobID != 702 && MobID != 703 && SpawnSpeed > 0)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                        RandomTime = (uint)(Rnd.Next(8, 20));
                    }
                    if (DateTime.Now > Died.AddSeconds(SpawnSpeed + RandomTime))//changed back to 30 by Ricardo
                    {
                        Respawn();
                    }
                }
                else if (SpawnSpeed == 0)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                        World.H_Mobs[Loc.Map].Remove(EntityID);

                    }
                }
                #region Syrens DisCity
                if (MobID == 700 && !Alive)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                        RandomTime = (uint)(Rnd.Next(10, 30));
                    }
                    if (World.Syrens == 8)//changed back to 30 by Ricardo
                        Respawn();
                }
                #endregion
                #region UltimatePluto
                else if (MobID == 701)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if ((World.Syrens == 0 || World.Syrens > 8) && !World.Pluto)//changed back to 30 by Ricardo
                    {
                        Respawn();
                        World.Pluto = true;
                        World.DebugAdd += "UltimatePluto Spawned! \r\n";
                        foreach (Character C in World.PlayersInMap[2024].Values)
                            C.MyClient.LocalMessage(2011, "Ultimate Pluto has spawned hurry up to defeat him!");

                    }
                }
                #endregion
                #region Ganoderma Special
                else if (MobID == 244)
                {
                    if (!Alive && DateTime.Now > Died.AddSeconds(90)) //added sanity check for respawn
                        World.Gano = false;

                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (DateTime.Now.Minute == 14 && !World.Gano)//changed back to 14 by Joao
                    {
                        World.SendMsgToAll("SYSTEM", "Warning! Ganoderma has appeared in the forest!", 2011, 0);
                        World.Gano = true;
                        Respawn();
                    }
                }
                #endregion
                #region Titan Special
                else if (MobID == 247)
                {
                    if (!Alive && DateTime.Now > Died.AddSeconds(90))
                        World.Titan = false;

                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (DateTime.Now.Minute == 16 && !World.Titan)//changed back to 16 by Joao
                    {
                        World.SendMsgToAll("SYSTEM", "Warning! Titan has appeared in the canyon!", 2011, 0);
                        World.Titan = true;
                        Respawn();
                    }

                }
                #endregion
                #region Desunoto
                else if (MobID == 8424)
                {/*
                    if (Features.CounterClock.War)
                    {
                        if (!Alive && DateTime.Now > Died.AddSeconds(90)) //added sanity check for respawn
                            World.CCMob = false;

                        if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                        {
                            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                            Dissappeared = true;
                        }
                        if ((DateTime.Now.Minute % 5 == 0) && !World.CCMob)//changed back to 14 by Joao
                        {
                            World.SendMsgToAll("SYSTEM", "The Desunoto has appeared in the Counter Clock Guild War Map! Kill it before it's too late!", 2011, 0, 1844);
                            World.CCMob = true;
                            Respawn();
                        }
                    }*/
                }
                #endregion
                #region GuildBeast
                else if (MobID == 501)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (((DateTime.Now.Hour == 20 && DateTime.Now.Minute == 00 && World.GuildBeast) || World.GuildBeastByPM) && !Alive)
                    {
                        World.SendMsgToAll("SYSTEM", "Warning! The GuildBeast has appeared in the GuildCastle! Everyone shall gather their weapons and fight it!", 2011, 0);
                        World.GuildBeast = false;
                        Respawn();
                    }
                }
                #endregion
                #region SnakeKing Special
                else if (MobID == 300)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                        RandomTime = (uint)(Rnd.Next(10, 30));
                    }
                    if (Alive && DateTime.Now > Respawned.AddSeconds(SpawnSpeed + RandomTime) && !World.SnakeKingAgain)//changed back to 30 by Ricardo
                    {
                        World.SnakeKingAgain = true;
                    }
                    else if (!Alive && DateTime.Now > Died.AddSeconds(SpawnSpeed + RandomTime) && !World.SnakeKingAgain)
                    {
                        Respawn();
                        Respawned = DateTime.Now;
                    }
                    else if (!Alive && World.SnakeKingAgain)
                    {
                        Respawned = DateTime.Now;
                        World.SnakeKingAgain = false;
                        Respawn();
                    }
                }
                #endregion
                #region ExpMob Special
                else if (MobID == 150)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.ExpMob && !Alive)
                    {
                        //Game.World.SendMsgToAll("EVENT", "EXP Mob has appeared inside the Promotion Center!", 2011, 0);
                        Game.World.SendMsgToAll("EVENT", "EXP Mob has appeared inside the Promotion Center!", 2005, 0);
                        Game.World.SendMsgToAll("EVENT", "EXP Mob has appeared inside the Promotion Center!", 2000, 0);
                        World.ExpMob = false;
                        Respawn();
                    }
                }
                #endregion
                #region Ball Special
                else if (MobID == 151)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.Ball && !Alive)
                    {
                        //Game.World.SendMsgToAll("EVENT", "Ball Mob has appeared inside the Ball Center!", 2011, 0);
                        //Game.World.SendMsgToAll("EVENT", "Ball has appeared inside the Ball Center!", 2005, 0);
                        //Game.World.SendMsgToAll("EVENT", "Ball Mob has appeared inside the Ball Center!", 2000, 0);
                        World.Ball = false;
                        Respawn();
                    }
                }
                #endregion
                #region DBDevil Commented
                /*else if (MobID == 409 && !Alive)
                    {
                        if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                        {
                            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                            Dissappeared = true;
                        }
                        if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && World.DBDevil)
                        {
                            World.DBDevil = false;
                            Respawn();
                        }
                        else if (!World.DBDevil && DateTime.Now.Hour == 1)
                            World.DBDevil = true;
                    }*/
                #endregion
                #region TeratoDragon Special
                else if (MobID == 4152)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.Dragon && !Alive)
                    {
                        World.SendMsgToAll("SYSTEM", "Terato Dragon has spawned in TwinCity at 565, 794 near Ape City portal!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Terato Dragon has spawned in TwinCity at 565, 794 near Ape City portal!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Terato Dragon has spawned in TwinCity at 565, 794 near Ape City portal!", 2000, 0);
                        World.Dragon = false;
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region Raikou
                else if (MobID == 3822)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.Raikou && !Alive)
                    {
                        if (!World.BossesDamage.ContainsKey(MobID))
                            World.BossesDamage.Add(MobID, new Dictionary<uint, uint>());
                        else
                            World.BossesDamage[MobID].Clear();
                        //World.SendMsgToAll("SYSTEM", "Raikou has spawned inside the Moon Platform at 131, 98 near the big tree!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Raikou has spawned inside the Moon Platform at 131, 98 near the big tree!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Raikou has spawned inside the Moon Platform at 131, 98 near the big tree!", 2000, 0);
                        World.Raikou = false;
                        World.CurrentBoss = "";
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }

                #endregion
                #region Capricorn
                else if (MobID == 3821)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.Capricorn && !Alive)
                    {
                        if (!World.BossesDamage.ContainsKey(MobID))
                            World.BossesDamage.Add(MobID, new Dictionary<uint, uint>());
                        else
                            World.BossesDamage[MobID].Clear();
                        //World.SendMsgToAll("SYSTEM", "Capricorn has spawned in Phoenix Castle at 804, 477 near the Village!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Capricorn has spawned in Phoenix Castle at 804, 477 near the Village!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Capricorn has spawned in Phoenix Castle at 804, 477 near the Village!", 2000, 0);
                        World.Capricorn = false;
                        World.CurrentBoss = "";
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region ThrillingSpook
                else if (MobID == 4172)
                {

                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.ThrillingSpook && !Alive)
                    {
                        if (!World.BossesDamage.ContainsKey(MobID))
                            World.BossesDamage.Add(MobID, new Dictionary<uint, uint>());
                        else
                            World.BossesDamage[MobID].Clear();
                        World.SendMsgToAll("SYSTEM", "Thrilling Spook has spawned in BirdIsland at 710, 925 near the abandoned city!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Thrilling Spook has spawned in BirdIsland at 710, 925 near the abandoned city!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Thrilling Spook has spawned in BirdIsland at 710, 925 near the abandoned city!", 2000, 0);
                        World.ThrillingSpook = false;
                        World.CurrentBoss = "";
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region Tash
                else if (MobID == 3823)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.Tash && !Alive)
                    {
                        if (!World.BossesDamage.ContainsKey(MobID))
                            World.BossesDamage.Add(MobID, new Dictionary<uint, uint>());
                        else
                            World.BossesDamage[MobID].Clear();
                        World.SendMsgToAll("SYSTEM", "Tash has spawned in Desert City at 493, 272 near the abandoned city!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Tash has spawned in Desert City at 493, 272 near the abandoned city!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Tash has spawned in Desert City at 493, 272 near the abandoned city!", 2000, 0);
                        World.Tash = false;
                        World.CurrentBoss = "";
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region SwordMaster
                else if (MobID == 4170)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.ThrillingSpook && !Alive)
                    {
                        World.SendMsgToAll("SYSTEM", "SwordMaster has spawned in Desert City at 493, 272 near the abandoned city!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "SwordMaster has spawned in Desert City at 493, 272 near the abandoned city!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "SwordMaster has spawned in Desert City at 493, 272 near the abandoned city!", 2000, 0);
                        World.ThrillingSpook = false;
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region SnowBanshee
                else if (MobID == 4171)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.ThrillingSpook && !Alive)
                    {
                        World.SendMsgToAll("SYSTEM", "SnowBanshee has spawned in Desert City at 493, 272 near the abandoned city!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "SnowBanshee has spawned in Desert City at 493, 272 near the abandoned city!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "SnowBanshee has spawned in Desert City at 493, 272 near the abandoned city!", 2000, 0);
                        World.ThrillingSpook = false;
                        Respawn();
                        World.Action(this, Packets.ShakeScreen(EntityID).Get);
                    }
                }
                #endregion
                #region AncientDevil
                else if (MobID == 8423)
                {
                    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2) && !Alive)
                    {
                        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                        Dissappeared = true;
                    }
                    if (World.AncientDevil)
                    {
                        World.SendMsgToAll("SYSTEM", "The AncientDevil has broken its seal and appeared near BirdIsland! Seal it and win rewards!", 2011, 0);
                        World.AncientDevil = false;
                        Respawn();
                    }
                }
                #endregion
                #region TreasureChest/Trap
                else if (MobID == 702 || MobID == 703 || MobID == 704)
                {
                    if (Loc.Map == World.TreasureMap && World.TreasureHunt)
                    {
                        if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                        {
                            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                            Dissappeared = true;
                            RandomTime = (uint)(Rnd.Next(7, 15));
                        }
                        if (DateTime.Now > Died.AddSeconds(SpawnSpeed + RandomTime))//changed back to 30 by Ricardo
                        {
                            Respawn();
                        }
                    }
                }
                #endregion
                #region DungeonMobs
                //if (MobID >= 1000 && MobID <= 3000 && !Alive)
                //{
                //    if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                //    {
                //        World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                //        Dissappeared = true;
                //        RandomTime = (uint)(Rnd.Next(10, 30));
                //    }
                //}
                #endregion
                #region GuildChest // Must Uncomment
                //else if (MobID == 500)
                //{
                //    if (Features.GuildWars.GuildChests > 0)
                //    {
                //        //Console.WriteLine("Chests OK " + Features.GuildWars.GuildChests);
                //        if (!Dissappeared && DateTime.Now > Died.AddSeconds(2))
                //        {
                //            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                //            Dissappeared = true;
                //            RandomTime = (uint)(Rnd.Next(8, 20));
                //        }
                //        for (int i = 0; i < Features.GuildWars.GuildChests; i++)
                //            if (DateTime.Now > Features.GuildWars.ChestTime[i])//changed back to 30 by Ricardo
                //            {
                //                Respawn();
                //                World.SendMsgToAll("SYSTEM", "GuildChest spawned in Guild Area!", 2011, 0);
                //                Features.GuildWars.ChestTime[i] = DateTime.Now.AddDays(7);
                //                break;
                //            }

                //    }
                //}
                #endregion
            }
            catch (Exception Exc) { World.ExcAdd += Exc.ToString() + "\r\n"; }




        }
        public void Attack()
        {
            LastMove = DateTime.Now.AddMilliseconds(Rnd.Next(500, 1500));
            List<Character> PlayerTargets = new List<Character>();
            if ((Type == MobBehaveour.HuntPlayers || Type == MobBehaveour.HuntMobsAndBlue || Type == MobBehaveour.HuntMobsAndPlayers) && PlayerTarget == null && MobTarget == null && CompTarget == null)
            {
                byte NDist = 14;//these 2 
                byte MaxDist = 14;
                bool CheckedTank = false;
                try
                {
                    if (World.PlayersInMap[Loc.Map].Count > 0)
                        foreach (Companion C in World.H_Companions.Values)
                        {
                            if (C != null && MobID != 98)
                            {
                                if (C.Loc.Map == Loc.Map)
                                {
                                    if (C.CurHP > 0 && Alive)
                                    {
                                        int Dst = MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);
                                        if (Dst <= MaxDist && Dst < NDist)//wants to find the nearest target
                                        {
                                            if (Type != MobBehaveour.HuntMobsAndBlue || C.Owner.BlueName)
                                            {
                                                NDist = (byte)MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);//and that
                                                CompTarget = C;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }

            Label:
                {
                    try
                    {
                        //6
                        //  for (int x = 0; x < Program.ThreadInfo.Array.Length; x++)
                        bool foundprevtank = false;
                        bool AOE = MyMath.ChanceSuccess(9);
                        foreach (Character C in World.PlayersInMap[Loc.Map].Values)//foreach (Character C in Game.World.H_Chars.Values)
                        {
                            if (C != null)
                            {
                                if (C.Loc.Map == Loc.Map)
                                {
                                    if (C.Alive && MobID != 98 || !C.Alive && MobID == 98)
                                    {
                                        if (Alive)
                                        {
                                            if ((C.CanBeMeeledByMobs || Type == MobBehaveour.HuntMobsAndBlue) || AtkType != AttackType.Melee || _UltimateBoss())
                                            {
                                                int Dst = MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);
                                                if (Dst <= MaxDist)
                                                {
                                                    if (MobID == 4152 && AOE)
                                                    {
                                                        PlayerTargets.Add(C);
                                                        PlayerTarget = C;
                                                    }
                                                    else if (Dst < NDist)//wants to find the nearest target
                                                    {
                                                        if (C.PrevTank)
                                                            foundprevtank = true;
                                                        if (C.Tank || CheckedTank)
                                                            if (Type != MobBehaveour.HuntMobsAndBlue || C.BlueName)
                                                            {
                                                                if (foundprevtank && CheckedTank)
                                                                {
                                                                    if (C.PrevTank)
                                                                    {
                                                                        NDist = (byte)MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);//and that
                                                                        PlayerTarget = C;
                                                                        if (MobID == 4152)
                                                                            World.DragonTank = C;
                                                                        C.Tank = true;
                                                                        C.PrevTank = false;
                                                                    }
                                                                }
                                                                else if (C.Tank || CheckedTank)
                                                                {
                                                                    NDist = (byte)MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);//and that
                                                                    PlayerTarget = C;
                                                                    if (MobID == 4152)
                                                                        World.DragonTank = C;
                                                                }
                                                            }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }





                    if (PlayerTarget == null && CompTarget == null && CheckedTank == false)
                    {
                        CheckedTank = true;
                        goto Label;
                    }
                }
            }
            if (_UltimateBoss())
            {
                foreach (Character C in World.PlayersInMap[Loc.Map].Values)//foreach (Character C in Game.World.H_Chars.Values)
                    if (C != null)
                        if (C.Loc.Map == Loc.Map)
                            if (C.Alive)
                                if (this != null)
                                    if (Alive)
                                    {
                                        if (!(C.Job == 135 && C.CanBeMeeledByMobs))
                                        {
                                            int Dst = MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y);
                                            if (Dst <= 24)
                                                PlayerTargets.Add(C);
                                        }
                                    }
            }
            if (Type == MobBehaveour.HuntMobsAndPlayers || Type == MobBehaveour.HuntMobs || Type == MobBehaveour.HuntMobsAndBlue && MobTarget == null && PlayerTarget == null && CompTarget == null)
            {
                byte NDist = 12;
                byte MaxDist = 10;
                foreach (Mob M in (World.H_Mobs[Loc.Map]).Values)
                    if (M != this && (M.Type == MobBehaveour.HuntPlayers || M.Type == MobBehaveour.HuntMobsAndPlayers) && M.Alive && MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= MaxDist && MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) < NDist)
                    {
                        NDist = (byte)MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y);
                        MobTarget = M;
                    }
            }
            if (Loc.Map == 3976)
            {
                foreach (NPC N in World.H_NPCs[Loc.Map].Values)
                    if (N.EntityID == 6730)
                        NPCTarget = N;
            }
            try
            {
                #region Player Target
                if (PlayerTarget != null && World.H_Chars.ContainsKey(PlayerTarget.EntityID) && PlayerTarget.MyClient != null && !PlayerTarget.LogOff)
                {
                    if (Alive)
                    {
                        if ((PlayerTarget.Alive && MobID != 98 || !PlayerTarget.Alive && MobID == 98) &&
                        ((PlayerTarget.CanBeMeeledByMobs || Type == MobBehaveour.HuntMobsAndBlue) || AtkType != AttackType.Melee || MobID == 4152) &&
                        MyMath.PointDistance(Loc.X, Loc.Y, PlayerTarget.Loc.X, PlayerTarget.Loc.Y) < Math.Max(16, (int)AttackDist) &&
                        (Type != MobBehaveour.HuntMobsAndBlue || PlayerTarget.BlueName))
                        {
                            #region Inside If
                            if (MyMath.PointDistance(Loc.X, Loc.Y, PlayerTarget.Loc.X, PlayerTarget.Loc.Y) >= AttackDist)
                            {
                                byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(Loc.X, Loc.Y, PlayerTarget.Loc.X, PlayerTarget.Loc.Y) / 45 % 8)) - 1 % 8);
                                Direction = (byte)((int)ToDir % 8);

                                Location eLoc = Loc;
                                eLoc.Walk(Direction);
                                bool PlaceFree = true;
                                if (DMaps.Loaded)
                                {
                                    if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;
                                }
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                    if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                                    {
                                        PlaceFree = false;
                                        break;
                                    }
                                if (PlaceFree)
                                {
                                    World.Action(this, Packets.Movement(EntityID, Direction).Get);
                                    World.Spawn(this, true);
                                    Loc.Walk(Direction);
                                }
                                else
                                {
                                    for (int i = 0; i < 7; i++)
                                    {
                                        PlaceFree = true;
                                        eLoc = Loc;
                                        Direction = (byte)((Direction + 1) % 8);
                                        eLoc.Walk(Direction);

                                        if (DMaps.Loaded)
                                            if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                                        foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                            if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                                            {
                                                PlaceFree = false;
                                                break;
                                            }
                                        if (PlaceFree)
                                        {
                                            World.Action(this, Packets.Movement(EntityID, Direction).Get);
                                            World.Spawn(this, true);
                                            Loc.Walk(Direction);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (DateTime.Now >= LastTarget.AddMilliseconds(1000) && DateTime.Now >= PlayerTarget.Loc.LastJump.AddMilliseconds(700))
                            {
                                byte dodge;
                                if (PlayerTarget.Transformation.Transformed)
                                {
                                    dodge = Math.Max((byte)50, (byte)(PlayerTarget.Transformation.Dodge + 20));
                                    dodge = Math.Min((byte)85, (byte)dodge);
                                }
                                else
                                {
                                    dodge = Math.Max((byte)50, (byte)(PlayerTarget.EqStats.Dodge));
                                    dodge = Math.Min((byte)85, (byte)dodge);
                                }
                                if ((AtkType != AttackType.Magic && MyMath.ChanceSuccess(dodge)) || Type == MobBehaveour.HuntMobsAndBlue)
                                    PlayerTarget.TakeAttack(this, PrepareAttack(), AtkType);
                                else if (AtkType == AttackType.Magic && (MyMath.ChanceSuccess(15) || MobID == 98))
                                    PlayerTarget.TakeAttack(this, PrepareAttack(), AtkType);

                                else PlayerTarget.TakeAttack(this, 0, AtkType);
                                LastTarget = DateTime.Now;
                            }
                            if (PlayerTargets.Count > 0)
                                Bosses.BossHandler.Handle(PlayerTarget, this, PrepareAttack() / 5, PlayerTargets);
                            #endregion
                        }
                        else
                            PlayerTarget = null;
                    }
                    else
                        PlayerTarget = null;
                }
                else
                    PlayerTarget = null;

                #endregion
            }
            catch (Exception E) { PlayerTarget = null; World.ExcAdd += E.ToString() + "\r\n"; }
            #region Mob Target
            if (MobTarget != null && MobTarget.Alive && MyMath.PointDistance(Loc.X, Loc.Y, MobTarget.Loc.X, MobTarget.Loc.Y) < 13)
            {
                if (MyMath.PointDistance(Loc.X, Loc.Y, MobTarget.Loc.X, MobTarget.Loc.Y) >= AttackDist)
                {
                    byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(Loc.X, Loc.Y, MobTarget.Loc.X, MobTarget.Loc.Y) / 45 % 8)) - 1 % 8);
                    Direction = (byte)((int)ToDir % 8);

                    Location eLoc = Loc;
                    eLoc.Walk(Direction);
                    bool PlaceFree = true;

                    if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                    foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                        if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                        {
                            PlaceFree = false;
                            break;
                        }
                    if (PlaceFree)
                    {
                        World.Action(this, Packets.Movement(EntityID, Direction).Get);
                        World.Spawn(this, true);
                        Loc.Walk(Direction);
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            PlaceFree = true;
                            eLoc = Loc;
                            Direction = (byte)((Direction + 1) % 8);
                            eLoc.Walk(Direction);

                            if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                            foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                                {
                                    PlaceFree = false;
                                    break;
                                }
                            if (PlaceFree)
                            {
                                World.Action(this, Packets.Movement(EntityID, Direction).Get);
                                World.Spawn(this, true);
                                Loc.Walk(Direction);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MobTarget.TakeAttack(this, PrepareAttack(), AtkType);
                }
            }
            else MobTarget = null;
            #endregion
            try
            {
                #region Companion Target
                if (CompTarget != null && Alive && CompTarget.Owner != null)
                {
                    if (World.H_Companions.ContainsKey(CompTarget.EntityID))
                    {
                        if (MobID != 150 && MobID != 8424 && MobID != 8423 && CompTarget.CurHP > 0 && MyMath.PointDistance(Loc.X, Loc.Y, CompTarget.Loc.X, CompTarget.Loc.Y) < Math.Max(16, (int)AttackDist) && (Type != MobBehaveour.HuntMobsAndBlue || CompTarget.Owner.BlueName))
                        {
                            if (MyMath.PointDistance(Loc.X, Loc.Y, CompTarget.Loc.X, CompTarget.Loc.Y) >= AttackDist)
                            {
                                byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(Loc.X, Loc.Y, CompTarget.Loc.X, CompTarget.Loc.Y) / 45 % 8)) - 1 % 8);
                                Direction = (byte)((int)ToDir % 8);

                                Location eLoc = Loc;
                                eLoc.Walk(Direction);
                                bool PlaceFree = true;
                                if (DMaps.Loaded)
                                {
                                    if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;
                                }
                                foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                    if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                                    {
                                        PlaceFree = false;
                                        break;
                                    }
                                if (PlaceFree)
                                {
                                    World.Action(this, Packets.Movement(EntityID, Direction).Get);
                                    World.Spawn(this, true);
                                    Loc.Walk(Direction);
                                }
                                else
                                {
                                    for (int i = 0; i < 7; i++)
                                    {
                                        PlaceFree = true;
                                        eLoc = Loc;
                                        Direction = (byte)((Direction + 1) % 8);
                                        eLoc.Walk(Direction);

                                        if (DMaps.Loaded)
                                            if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                                        foreach (Mob M in World.H_Mobs[Loc.Map].Values)
                                            if (M != this && M.Loc.X == eLoc.X && M.Loc.Y == eLoc.Y && M.Alive)
                                            {
                                                PlaceFree = false;
                                                break;
                                            }
                                        if (PlaceFree)
                                        {
                                            World.Action(this, Packets.Movement(EntityID, Direction).Get);
                                            World.Spawn(this, true);
                                            Loc.Walk(Direction);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (DateTime.Now >= LastTarget.AddMilliseconds(1000))
                            {
                                //if ((AtkType != AttackType.Magic && MyMath.ChanceSuccess(100 - CompTarget.Dodge)) || Type == MobBehaveour.HuntMobsAndBlue)
                                LastTarget = DateTime.Now;
                                CompTarget.TakeAttack(this, PrepareAttack(), AtkType);
                                // else if (AtkType == AttackType.Magic && MyMath.ChanceSuccess(15))
                                // CompTarget.TakeAttack(this, PrepareAttack(), AtkType);
                                // else CompTarget.TakeAttack(this, 0, AtkType);

                            }
                        }
                        else CompTarget = null;
                    }
                    else CompTarget = null;
                }
                else CompTarget = null;

                #endregion
            }
            catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }

        }
    }
}