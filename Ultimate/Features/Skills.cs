using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using Ultimate.Game;
using Ultimate.Structures;

namespace Ultimate.Features
{
#warning public unsafe class - Derek changes
    public class SkillsClass
    {
        public enum ExtraEffect : byte
        {
            None,
            Stigma,
            MagicShield,
            Accuracy,
            Superman,
            Cyclone,
            Invisibility,
            Revive,
            Poison,
            Fly,
            Transform,
            Summon,
            FatalStrike,
            ShurikenVortex,
            RemoveFly,
            FlashStep,
            Ride,
            UnMount,
            NoPots,
            Scapegoat,
            BlessPray,
            Dodge,
            Roar,
            Intensify,
            IceBlock
        }
        public enum TargetType : byte
        {
            Single,
            FromSingle,
            FromPoint,
            Range,
            Sector,
            Linear
        }
        public enum DamageType : byte
        {
            Magic,
            Ranged,
            Melee,
            HealHP,
            HealMP,
            Percent
        }
        public struct SkillInfo
        {
            public ushort ID;
            public byte Level;
            public ushort ManaCost;
            public byte StaminaCost;
            public byte ArrowsCost;
            public bool EndsXPWait;
            public byte UpgReqLvl;
            public uint UpgReqExp;
            public uint Damage;
            public TargetType Targetting;
            public DamageType Damageing;
            public ExtraEffect ExtraEff;
            public ushort EffectLasts;
            public float EffectValue;
            public byte ActivationChance;
            public byte MaxDist;
            public ushort SectorSize;

            public void LoadThis(BinaryReader BR)
            {
                ID = BR.ReadUInt16();
                Level = BR.ReadByte();
                ManaCost = BR.ReadUInt16();
                StaminaCost = BR.ReadByte();
                ArrowsCost = BR.ReadByte();
                EndsXPWait = BR.ReadBoolean();
                UpgReqLvl = BR.ReadByte();
                UpgReqExp = BR.ReadUInt32();
                Damage = BR.ReadUInt32();
                Targetting = (TargetType)BR.ReadByte();
                Damageing = (DamageType)BR.ReadByte();
                ExtraEff = (ExtraEffect)BR.ReadByte();
                EffectLasts = BR.ReadUInt16();
                EffectValue = BR.ReadSingle();
                ActivationChance = BR.ReadByte();
                MaxDist = BR.ReadByte();
                SectorSize = BR.ReadByte();
            }
            public void SaveThis(BinaryWriter BW)
            {
                BW.Write(ID);
                BW.Write(Level);
                BW.Write(ManaCost);
                BW.Write(StaminaCost);
                BW.Write(ArrowsCost);
                BW.Write(EndsXPWait);
                BW.Write(UpgReqLvl);
                BW.Write(UpgReqExp);
                BW.Write(Damage);
                BW.Write((byte)Targetting);
                BW.Write((byte)Damageing);
                BW.Write((byte)ExtraEff);
                BW.Write(EffectLasts);
                BW.Write(EffectValue);
                BW.Write(ActivationChance);
                BW.Write(MaxDist);
                BW.Write(SectorSize);
            }
        }

        public struct SkillUse
        {
            public SkillInfo Info;
            public Dictionary<Mob, uint> MobTargets;
            public Dictionary<Character, uint> PlayerTargets;
            public Dictionary<NPC, uint> NPCTargets;
            public Dictionary<Companion, uint> CompTargets;
            public Dictionary<uint, uint> MiscTargets;
            //public Dictionary<AI, uint> AITargets;
            public Game.Character User;
            public ushort AimX;
            public ushort AimY;

            public void Init(Game.Character C, ushort SkillID, byte SkillLvl, ushort AimX, ushort AimY)
            {
                try
                {
                    User = C;
                    Info = (SkillInfo)SkillsClass.SkillInfos[SkillID + " " + SkillLvl];
                    this.AimX = AimX;
                    this.AimY = AimY;
                    MobTargets = new Dictionary<Mob, uint>();
                    PlayerTargets = new Dictionary<Character, uint>();
                    NPCTargets = new Dictionary<NPC, uint>();
                    MiscTargets = new Dictionary<uint, uint>();
                    CompTargets = new Dictionary<Companion, uint>();
                    //if (User.Opponent != null)
                    //    AITargets = new Dictionary<AI, uint>();

                }
                catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            }
            public void GetTargets(uint Single = 0)
            {
                try
                {
                    if (User.ProtectTime.AddMilliseconds(0) > DateTime.Now && !User.CancelProtectTime)
                        return;
                    GetMobTargets(Single);
                    GetPlayerTargets(Single);
                    GetNPCTargets(Single);
                    GetCompTargets(Single);
                    GetMiscTargets(Single);
                    //if (User.Opponent != null)
                    //    GetAI(Single);

                }
                catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            }
            //void GetAI(uint Single)
            //{
            //    if (Info.Targetting == TargetType.Single)
            //    {
            //        if (Info.ID == 1051)
            //        {
            //            if (User.Loc.Map == 1039)
            //                return;
            //            Location e = User.Loc;
            //            e.Walk((byte)Single);
            //            Location e2 = e;
            //            e2.Walk((byte)Single);
            //            bool PlaceFree = true;
            //            if (DMaps.Loaded)
            //            {
            //                if (((DMap)DMaps.H_DMaps[User.Loc.Map]).GetCell(e2.X, e2.Y).NoAccess) PlaceFree = false;
            //            }
            //            if (PlaceFree && User.Loc.Map == 1038)
            //                PlaceFree = User.Loc.AbleToWalkGW(e2.X, e2.Y);
            //            if (PlaceFree)
            //                PacketHandling.WalkRun.Handle(User.MyClient, new byte[0], (byte)Single);
            //            if (Game.ItemIDManipulation.Part(User.Equips.LeftHand.ID, 0, 3) != 900 || User.Equips.LeftHand.CurDur <= 2)
            //                return;
            //            if (User.Opponent.Alive)
            //                if (User.Opponent.Loc.X == e.X && User.Opponent.Loc.Y == e.Y)
            //                    if (User.Opponent.EntityID != User.EntityID)
            //                        if (User.Opponent.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, User.Opponent.Loc.X, User.Opponent.Loc.Y, 30))
            //                        { AITargets.Add(User.Opponent, Single); User.Equips.LeftHand.CurDur -= 2; }
            //        }
            //        else
            //        {
            //            AI C = null;
            //            if (User.Opponent != null)
            //                C = User.Opponent;


            //            if (C != null)
            //                if ((C.Alive || Info.ExtraEff == ExtraEffect.Revive) && MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= 18 && User.Loc.Map == C.Loc.Map)
            //                    if (Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee)
            //                        if (Info.ExtraEff != ExtraEffect.None || Info.Damageing == DamageType.HealHP || Info.Damageing == DamageType.HealMP)
            //                            if (((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30))) || C.EntityID == User.EntityID || (C.Level <= 6))
            //                                AITargets.Add(User.Opponent, GetDamage(User));
            //                            else
            //                                User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");

            //        }
            //    }
            //    else
            //    {
            //        bool RangeFromChar = true;
            //        AI C = User.Opponent;
            //        if (Info.Targetting == TargetType.FromSingle)
            //        {
            //            if (C != null)
            //            {
            //                if (C.Alive)
            //                {
            //                    if (User.Loc.Map == C.Loc.Map)
            //                        if (Info.Damageing == DamageType.HealHP)
            //                            if (Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee)
            //                            {
            //                                    if ((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30)) || (C.Level <= 6))
            //                                    {
            //                                        AITargets.Add(C, GetDamage(User));
            //                                        AimX = C.Loc.X;
            //                                        AimY = C.Loc.Y;
            //                                        RangeFromChar = false;
            //                                    }
            //                                    else
            //                                        User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
            //                            }
            //                }
            //            }
            //        }
            //        else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
            //        {
            //            AimX = User.Loc.X;
            //            AimY = User.Loc.Y;
            //            RangeFromChar = true;
            //        }
            //        else
            //            RangeFromChar = false;
            //        List<coords> Line = new List<coords>(5);
            //        if (Info.Targetting == TargetType.Linear)
            //            Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);

            //        List<StoreLoc> Coords = new List<StoreLoc>();
            //        if (C != null)
            //            if (User.Loc.Map == C.Loc.Map) //  REVERSE TO ALL PLAYERS
            //                if (C.Alive)
            //                {
            //                    if ((!RangeFromChar && MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist) || MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist)
            //                        if (Info.Targetting == TargetType.Sector && InSector(C.Loc.X, C.Loc.Y) || Info.Targetting != TargetType.Sector)
            //                            if ((Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee) || (Info.Targetting == TargetType.Linear && Line.Contains(new coords(C.Loc.X, C.Loc.Y)) || Info.Targetting != TargetType.Linear))
            //                                if (!AITargets.ContainsKey(C) && (!World.NoPKMaps.Contains(User.Loc.Map) || GetDamage(User) == 0))
            //                                        if (!MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30))
            //                                    {
            //                                        StoreLoc SLoc = new StoreLoc() { Map = C.Loc.Map, X = C.Loc.X, Y = C.Loc.Y };
            //                                            DMap DM = ((DMap)DMaps.H_DMaps[C.Loc.Map]);
            //                                            if (DM != null)
            //                                            {
            //                                                DMapCell New = DM.GetCell(C.Loc.X, C.Loc.Y);
            //                                                DMapCell Old = DM.GetCell(User.Loc.X, User.Loc.Y);

            //                                                if (New.High && Old.High && !Coords.Contains(SLoc))
            //                                                {
            //                                                    Coords.Add(SLoc);
            //                                                    AITargets.Add(C, GetDamage(User));
            //                                                }
            //                                                else if (!New.High && !Old.High && !Coords.Contains(SLoc))
            //                                                {
            //                                                Coords.Add(SLoc);
            //                                                AITargets.Add(C, GetDamage(User));
            //                                            }
            //                                            }
            //                                            else if (!Coords.Contains(SLoc))
            //                                            {
            //                                                Coords.Add(SLoc);
            //                                                AITargets.Add(C, GetDamage(User));
            //                                            }

            //                                        }
            //                                        else
            //                                            User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
            //                }

            //    }
            //}
            void GetMiscTargets(uint Single)
            {
                if (Info.Targetting == TargetType.Single)
                {
                    if (World.H_SOBs.ContainsKey(Single))
                    {
                        if (World.H_SOBs[Single].IsPole())
                        {
                            if ((User.MyGuild != null && GuildWars.LastWinner.GuildID != User.MyGuild.GuildID) && World.H_SOBs[Single].War)
                                MiscTargets.Add(Single, GetDamage(World.H_SOBs[Single]));
                        }
                        else
                            MiscTargets.Add(Single, GetDamage(World.H_SOBs[Single]));
                    }

                    #region unused
                    //if (Single == 6700 && GuildWars.War && User.MyGuild != null && (GuildWars.LastWinner == null || User.MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                    //    MiscTargets.Add(Single, GetDamage(GuildWars.ThePole.CurHP));
                    //else if (Single == 6701 && !GuildWars.TheLeftGate.Opened)
                    //    MiscTargets.Add(Single, GetDamage(GuildWars.TheLeftGate.CurHP));
                    //else if (Single == 6702 && !GuildWars.TheRightGate.Opened)
                    //    MiscTargets.Add(Single, GetDamage(GuildWars.TheRightGate.CurHP));

                    #region Counter Clock GW
                    //if (Single == 6726 && CounterClock.War && User.MyGuild != null && (CounterClock.LastWinner == null || User.MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.ThePole.CurHP));
                    //else if (Single == 6703 && !CounterClock.LG1.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG1.CurHP));
                    //else if (Single == 6704 && !CounterClock.LG2.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG2.CurHP));
                    //else if (Single == 6705 && !CounterClock.LG3.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG3.CurHP));
                    //else if (Single == 6706 && !CounterClock.LG4.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG4.CurHP));
                    //else if (Single == 6707 && !CounterClock.LG5.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG5.CurHP));
                    //else if (Single == 6708 && !CounterClock.LG6.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.LG6.CurHP));
                    //else if (Single == 6709 && !CounterClock.RG1.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG1.CurHP));
                    //else if (Single == 6710 && !CounterClock.RG2.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG2.CurHP));
                    //else if (Single == 6711 && !CounterClock.RG3.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG3.CurHP));
                    //else if (Single == 6712 && !CounterClock.RG4.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG4.CurHP));
                    //else if (Single == 6713 && !CounterClock.RG5.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG5.CurHP));
                    //else if (Single == 6714 && !CounterClock.RG6.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG6.CurHP));
                    //else if (Single == 6715 && !CounterClock.RG7.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG7.CurHP));
                    //else if (Single == 6716 && !CounterClock.RG8.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG8.CurHP));
                    //else if (Single == 6717 && !CounterClock.RG9.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG9.CurHP));
                    //else if (Single == 6718 && !CounterClock.RG10.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG10.CurHP));
                    //else if (Single == 6719 && !CounterClock.RG11.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG11.CurHP));
                    //else if (Single == 6720 && !CounterClock.RG12.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG12.CurHP));
                    //else if (Single == 6721 && !CounterClock.RG13.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG13.CurHP));
                    //else if (Single == 6722 && !CounterClock.RG14.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG14.CurHP));
                    //else if (Single == 6723 && !CounterClock.RG15.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG15.CurHP));
                    //else if (Single == 6724 && !CounterClock.RG16.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG16.CurHP));
                    //else if (Single == 6725 && !CounterClock.RG17.Opened)
                    //    MiscTargets.Add(Single, GetDamage(CounterClock.RG17.CurHP));
                    #endregion
                    #endregion

                }
                else
                {
                    bool RangeFromChar = true;
                    if (Info.Targetting == TargetType.FromSingle)
                    {
                        if (World.H_SOBs.ContainsKey(Single))
                        {
                            if (World.H_SOBs[Single].IsPole())
                            {
                                if ((User.MyGuild != null && (World.H_SOBs[Single].LastWinner.GuildID != User.MyGuild.GuildID || World.H_SOBs[Single].LastWinner == null)) && World.H_SOBs[Single].War)
                                {
                                    MiscTargets.Add(Single, GetDamage(World.H_SOBs[Single]));
                                    AimX = World.H_SOBs[Single].Loc.X;
                                    AimY = World.H_SOBs[Single].Loc.Y;
                                }
                            }
                            else
                            {
                                MiscTargets.Add(Single, GetDamage(World.H_SOBs[Single]));
                                AimX = World.H_SOBs[Single].Loc.X;
                                AimY = World.H_SOBs[Single].Loc.Y;
                            }
                        }
                        #region unused
                        //if (Single == 6700 && GuildWars.War && User.MyGuild != null && (GuildWars.LastWinner == null || User.MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                        //{
                        //    MiscTargets.Add(Single, GetDamage(GuildWars.ThePole.CurHP));
                        //    AimX = GuildWars.ThePole.Loc.X;
                        //    AimY = GuildWars.ThePole.Loc.Y;
                        //}
                        //else if (Single == 6701 && !GuildWars.TheLeftGate.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(GuildWars.TheLeftGate.CurHP));
                        //    AimX = GuildWars.TheLeftGate.Loc.X;
                        //    AimY = GuildWars.TheLeftGate.Loc.Y;
                        //}
                        //else if (Single == 6702 && !GuildWars.TheRightGate.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(GuildWars.TheRightGate.CurHP));
                        //    AimX = GuildWars.TheRightGate.Loc.X;
                        //    AimY = GuildWars.TheRightGate.Loc.Y;
                        //}

                        #region Counter Clock GW
                        //if (Single == 6726 && CounterClock.War && User.MyGuild != null && (CounterClock.LastWinner == null || User.MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.ThePole.CurHP));
                        //    AimX = CounterClock.ThePole.Loc.X;
                        //    AimY = CounterClock.ThePole.Loc.Y;
                        //}
                        //else if (Single == 6703 && !CounterClock.LG1.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG1.CurHP));
                        //    AimX = CounterClock.LG1.Loc.X;
                        //    AimY = CounterClock.LG1.Loc.Y;
                        //}
                        //else if (Single == 6704 && !CounterClock.LG2.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG2.CurHP));
                        //    AimX = CounterClock.LG2.Loc.X;
                        //    AimY = CounterClock.LG2.Loc.Y;
                        //}
                        //else if (Single == 6705 && !CounterClock.LG3.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG3.CurHP));
                        //    AimX = CounterClock.LG3.Loc.X;
                        //    AimY = CounterClock.LG3.Loc.Y;
                        //}
                        //else if (Single == 6706 && !CounterClock.LG4.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG4.CurHP));
                        //    AimX = CounterClock.LG4.Loc.X;
                        //    AimY = CounterClock.LG4.Loc.Y;
                        //}
                        //else if (Single == 6707 && !CounterClock.LG5.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG5.CurHP));
                        //    AimX = CounterClock.LG5.Loc.X;
                        //    AimY = CounterClock.LG5.Loc.Y;
                        //}
                        //else if (Single == 6708 && !CounterClock.LG6.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.LG6.CurHP));
                        //    AimX = CounterClock.LG6.Loc.X;
                        //    AimY = CounterClock.LG6.Loc.Y;
                        //}
                        //else if (Single == 6709 && !CounterClock.RG1.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG1.CurHP));
                        //    AimX = CounterClock.RG1.Loc.X;
                        //    AimY = CounterClock.RG1.Loc.Y;
                        //}
                        //else if (Single == 6710 && !CounterClock.RG2.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG2.CurHP));
                        //    AimX = CounterClock.RG2.Loc.X;
                        //    AimY = CounterClock.RG2.Loc.Y;
                        //}
                        //else if (Single == 6711 && !CounterClock.RG3.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG3.CurHP));
                        //    AimX = CounterClock.RG3.Loc.X;
                        //    AimY = CounterClock.RG3.Loc.Y;
                        //}
                        //else if (Single == 6712 && !CounterClock.RG4.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG4.CurHP));
                        //    AimX = CounterClock.RG4.Loc.X;
                        //    AimY = CounterClock.RG4.Loc.Y;
                        //}
                        //else if (Single == 6713 && !CounterClock.RG5.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG5.CurHP));
                        //    AimX = CounterClock.RG5.Loc.X;
                        //    AimY = CounterClock.RG5.Loc.Y;
                        //}
                        //else if (Single == 6714 && !CounterClock.RG6.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG6.CurHP));
                        //    AimX = CounterClock.RG6.Loc.X;
                        //    AimY = CounterClock.RG6.Loc.Y;
                        //}
                        //else if (Single == 6715 && !CounterClock.RG7.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG7.CurHP));
                        //    AimX = CounterClock.RG7.Loc.X;
                        //    AimY = CounterClock.RG7.Loc.Y;
                        //}
                        //else if (Single == 6716 && !CounterClock.RG8.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG8.CurHP));
                        //    AimX = CounterClock.RG8.Loc.X;
                        //    AimY = CounterClock.RG8.Loc.Y;
                        //}
                        //else if (Single == 6717 && !CounterClock.RG9.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG9.CurHP));
                        //    AimX = CounterClock.RG9.Loc.X;
                        //    AimY = CounterClock.RG9.Loc.Y;
                        //}
                        //else if (Single == 6718 && !CounterClock.RG10.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG10.CurHP));
                        //    AimX = CounterClock.RG10.Loc.X;
                        //    AimY = CounterClock.RG10.Loc.Y;
                        //}
                        //else if (Single == 6719 && !CounterClock.RG11.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG11.CurHP));
                        //    AimX = CounterClock.RG11.Loc.X;
                        //    AimY = CounterClock.RG11.Loc.Y;
                        //}
                        //else if (Single == 6720 && !CounterClock.RG12.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG12.CurHP));
                        //    AimX = CounterClock.RG12.Loc.X;
                        //    AimY = CounterClock.RG12.Loc.Y;
                        //}
                        //else if (Single == 6721 && !CounterClock.RG13.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG13.CurHP));
                        //    AimX = CounterClock.RG13.Loc.X;
                        //    AimY = CounterClock.RG13.Loc.Y;
                        //}
                        //else if (Single == 6722 && !CounterClock.RG14.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG14.CurHP));
                        //    AimX = CounterClock.RG14.Loc.X;
                        //    AimY = CounterClock.RG14.Loc.Y;
                        //}
                        //else if (Single == 6723 && !CounterClock.RG15.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG15.CurHP));
                        //    AimX = CounterClock.RG15.Loc.X;
                        //    AimY = CounterClock.RG15.Loc.Y;
                        //}
                        //else if (Single == 6724 && !CounterClock.RG16.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG16.CurHP));
                        //    AimX = CounterClock.RG16.Loc.X;
                        //    AimY = CounterClock.RG16.Loc.Y;
                        //}
                        //else if (Single == 6725 && !CounterClock.RG17.Opened)
                        //{
                        //    MiscTargets.Add(Single, GetDamage(CounterClock.RG17.CurHP));
                        //    AimX = CounterClock.RG17.Loc.X;
                        //    AimY = CounterClock.RG17.Loc.Y;
                        //}
                        #endregion
                        #endregion
                    }
                    else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
                    {
                        AimX = User.Loc.X;
                        AimY = User.Loc.Y;
                        RangeFromChar = true;
                    }
                    else
                        RangeFromChar = false;
                    List<coords> Line = new List<coords>(5);
                    if (Info.Targetting == TargetType.Linear)
                        Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                    //Line = MyMath.LineCoords(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);

                    if (User.Loc.Map == 1038 || User.Loc.Map == 1844)
                    {
                        foreach (SOB S in World.H_SOBs.Values)
                        {
                            if (User.Loc.Map == S.Loc.Map)
                            {
                                if (S.IsPole())
                                {
                                    if (User.MyGuild != null)
                                        if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, S.Loc.X, S.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, S.Loc.X, S.Loc.Y) <= Info.MaxDist) && S.War && (S.LastWinner == null || User.MyGuild.GuildID != S.LastWinner.GuildID))
                                            if (Info.Targetting == TargetType.Sector && InSector(S.Loc.X, S.Loc.Y) || Info.Targetting != TargetType.Sector)
                                                if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(S.Loc.X, S.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                                    if (!MiscTargets.ContainsKey(S.EntityID))
                                                        MiscTargets.Add(S.EntityID, GetDamage(S));
                                }
                                else
                                {
                                    if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, S.Loc.X, S.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, S.Loc.X, S.Loc.Y) <= Info.MaxDist))
                                        if (Info.Targetting == TargetType.Sector && InSector(GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) || Info.Targetting != TargetType.Sector)
                                            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(S.Loc.X, S.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                                if (!MiscTargets.ContainsKey(S.EntityID))
                                                    MiscTargets.Add(S.EntityID, GetDamage(S));
                                }
                            }
                        }
                    }
                    #region unused
                    //if (User.Loc.Map == 1038)
                    //{
                    //    if (User.MyGuild != null)
                    //        if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) <= Info.MaxDist) && GuildWars.War && (GuildWars.LastWinner == null || User.MyGuild.GuildID != GuildWars.LastWinner.GuildID))
                    //            if (Info.Targetting == TargetType.Sector && InSector(GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //                if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(GuildWars.ThePole.Loc.X, GuildWars.ThePole.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                    if (!MiscTargets.ContainsKey(GuildWars.ThePole.EntityID))
                    //                        MiscTargets.Add(GuildWars.ThePole.EntityID, GetDamage(GuildWars.ThePole.CurHP));

                    //    if (!GuildWars.TheRightGate.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(GuildWars.TheRightGate.Loc.X, GuildWars.TheRightGate.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(GuildWars.TheRightGate.EntityID))
                    //                    MiscTargets.Add(GuildWars.TheRightGate.EntityID, GetDamage(GuildWars.TheRightGate.CurHP));

                    //    if (!GuildWars.TheLeftGate.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(GuildWars.TheLeftGate.Loc.X, GuildWars.TheLeftGate.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(GuildWars.TheLeftGate.EntityID))
                    //                    MiscTargets.Add(GuildWars.TheLeftGate.EntityID, GetDamage(GuildWars.TheLeftGate.CurHP));
                    //}
                    #region Counter Clock GW
                    //if (User.Loc.Map == 1844)
                    //{
                    //    if (User.MyGuild != null)
                    //        if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) <= Info.MaxDist) && CounterClock.War && (CounterClock.LastWinner == null || User.MyGuild.GuildID != CounterClock.LastWinner.GuildID))
                    //            if (Info.Targetting == TargetType.Sector && InSector(CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //                if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.ThePole.Loc.X, CounterClock.ThePole.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                    if (!MiscTargets.ContainsKey(CounterClock.ThePole.EntityID))
                    //                        MiscTargets.Add(CounterClock.ThePole.EntityID, GetDamage(CounterClock.ThePole.CurHP));

                    //    if (!CounterClock.LG5.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG5.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG5.EntityID, GetDamage(CounterClock.LG5.CurHP));

                    //    if (!CounterClock.LG2.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG2.Loc.X, CounterClock.LG2.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG2.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG2.EntityID, GetDamage(CounterClock.LG2.CurHP));

                    //    if (!CounterClock.LG3.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG3.Loc.X, CounterClock.LG3.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG3.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG3.EntityID, GetDamage(CounterClock.LG3.CurHP));

                    //    if (!CounterClock.LG4.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG4.Loc.X, CounterClock.LG4.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG4.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG4.EntityID, GetDamage(CounterClock.LG4.CurHP));

                    //    if (!CounterClock.LG5.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG5.Loc.X, CounterClock.LG5.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG5.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG5.EntityID, GetDamage(CounterClock.LG5.CurHP));

                    //    if (!CounterClock.LG6.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.LG6.Loc.X, CounterClock.LG6.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.LG6.EntityID))
                    //                    MiscTargets.Add(CounterClock.LG6.EntityID, GetDamage(CounterClock.LG6.CurHP));

                    //    if (!CounterClock.RG1.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG1.Loc.X, CounterClock.RG1.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG1.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG1.EntityID, GetDamage(CounterClock.RG1.CurHP));

                    //    if (!CounterClock.RG2.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG2.Loc.X, CounterClock.RG2.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG2.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG2.EntityID, GetDamage(CounterClock.RG2.CurHP));

                    //    if (!CounterClock.RG3.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG3.Loc.X, CounterClock.RG3.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG3.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG3.EntityID, GetDamage(CounterClock.RG3.CurHP));

                    //    if (!CounterClock.RG4.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG4.Loc.X, CounterClock.RG4.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG4.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG4.EntityID, GetDamage(CounterClock.RG4.CurHP));

                    //    if (!CounterClock.RG5.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG5.Loc.X, CounterClock.RG5.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG5.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG5.EntityID, GetDamage(CounterClock.RG5.CurHP));

                    //    if (!CounterClock.RG6.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG6.Loc.X, CounterClock.RG6.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG6.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG6.EntityID, GetDamage(CounterClock.RG6.CurHP));

                    //    if (!CounterClock.RG7.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG7.Loc.X, CounterClock.RG7.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG7.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG7.EntityID, GetDamage(CounterClock.RG7.CurHP));

                    //    if (!CounterClock.RG8.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG8.Loc.X, CounterClock.RG8.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG8.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG8.EntityID, GetDamage(CounterClock.RG8.CurHP));

                    //    if (!CounterClock.RG9.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG9.Loc.X, CounterClock.RG9.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG9.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG9.EntityID, GetDamage(CounterClock.RG9.CurHP));

                    //    if (!CounterClock.RG10.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG10.Loc.X, CounterClock.RG10.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG10.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG10.EntityID, GetDamage(CounterClock.RG10.CurHP));

                    //    if (!CounterClock.RG11.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG11.Loc.X, CounterClock.RG11.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG11.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG11.EntityID, GetDamage(CounterClock.RG11.CurHP));

                    //    if (!CounterClock.RG12.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG12.Loc.X, CounterClock.RG12.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG12.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG12.EntityID, GetDamage(CounterClock.RG12.CurHP));

                    //    if (!CounterClock.RG13.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG13.Loc.X, CounterClock.RG13.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG13.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG13.EntityID, GetDamage(CounterClock.RG13.CurHP));

                    //    if (!CounterClock.RG14.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG14.Loc.X, CounterClock.RG14.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG14.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG14.EntityID, GetDamage(CounterClock.RG14.CurHP));

                    //    if (!CounterClock.RG15.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG15.Loc.X, CounterClock.RG15.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG15.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG15.EntityID, GetDamage(CounterClock.RG15.CurHP));

                    //    if (!CounterClock.RG16.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG16.Loc.X, CounterClock.RG16.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG16.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG16.EntityID, GetDamage(CounterClock.RG16.CurHP));

                    //    if (!CounterClock.RG17.Opened && (!RangeFromChar && MyMath.PointDistance(AimX, AimY, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Info.MaxDist || MyMath.PointDistance(User.Loc.X, User.Loc.Y, CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) <= Info.MaxDist))
                    //        if (Info.Targetting == TargetType.Sector && InSector(CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y) || Info.Targetting != TargetType.Sector)
                    //            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(CounterClock.RG17.Loc.X, CounterClock.RG17.Loc.Y)) || Info.Targetting != TargetType.Linear)
                    //                if (!MiscTargets.ContainsKey(CounterClock.RG17.EntityID))
                    //                    MiscTargets.Add(CounterClock.RG17.EntityID, GetDamage(CounterClock.RG17.CurHP));
                    //}
                    #endregion
                    #endregion
                }
            }
            void GetMobTargets(uint Single)
            {
                if (User.Loc.Map != 1039)
                {
                    if (Info.Targetting == TargetType.Single)
                    {
                        if (World.H_Mobs.ContainsKey(User.Loc.Map))
                        {
                            if (World.H_Mobs[User.Loc.Map].ContainsKey(Single))
                            {
                                Mob M = World.H_Mobs[User.Loc.Map][Single];
                                if (M != null)
                                    if (M.Alive)
                                        MobTargets.Add(M, GetDamage(M));
                            }
                        }
                    }
                    else
                    {
                        bool RangeFromChar = true;
                        if (Info.Targetting == TargetType.FromSingle)
                        {
                            if (World.H_Mobs.ContainsKey(User.Loc.Map))
                            {
                                if (World.H_Mobs[User.Loc.Map].ContainsKey(Single))
                                {
                                    Mob M = World.H_Mobs[User.Loc.Map][Single];
                                    if (M != null)
                                    {
                                        if (M.Alive)
                                        {
                                            MobTargets.Add(M, GetDamage(M));
                                            AimX = M.Loc.X;
                                            AimY = M.Loc.Y;
                                            RangeFromChar = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
                        {
                            AimX = User.Loc.X;
                            AimY = User.Loc.Y;
                            RangeFromChar = true;
                        }
                        else
                        {
                            RangeFromChar = false;
                        }
                        if (World.H_Mobs.ContainsKey(User.Loc.Map))
                        {
                            List<coords> Line = new List<coords>(5);
                            if (Info.Targetting == TargetType.Linear)
                                Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                            //Line = MyMath.LineCoords(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                            foreach (Mob M in World.H_Mobs[User.Loc.Map].Values)
                            {
                                if (M.Alive)
                                {
                                    if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, M.Loc.X, M.Loc.Y) <= Info.MaxDist) || MyMath.PointDistance(User.Loc.X, User.Loc.Y, M.Loc.X, M.Loc.Y) <= Info.MaxDist)
                                        if (Info.ID == 8001)
                                        {
                                            if (Info.Targetting == TargetType.Sector && FunctiaCuUnghiVariabil(AimX, AimY, User.Loc.X, User.Loc.Y, M.Loc.X, M.Loc.Y, Info.SectorSize) || Info.Targetting != TargetType.Sector)
                                                if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(M.Loc.X, M.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                                    if ((User.PKMode == PKMode.PK || !M.NeedsPKMode) && !MobTargets.ContainsKey(M))
                                                        MobTargets.Add(M, GetDamage(M));
                                        }
                                        else
                                            if (Info.Targetting == TargetType.Sector && InSector(M.Loc.X, M.Loc.Y) || Info.Targetting != TargetType.Sector)
                                            if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(M.Loc.X, M.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                                if ((User.PKMode == PKMode.PK || !M.NeedsPKMode) && !MobTargets.ContainsKey(M))
                                                    MobTargets.Add(M, GetDamage(M));

                                }
                            }
                        }

                    }
                }
            }
            void GetCompTargets(uint Single)
            {
                if (User.Loc.Map != 1039)
                {
                    if (Info.Targetting == TargetType.Single)
                    {
                        if (World.H_Companions.ContainsKey(Single))
                        {
                            Companion M = (Companion)World.H_Companions[Single];
                            if (M.Owner.EntityID != User.EntityID && M.Owner.PKAble(User.PKMode, User))
                                CompTargets.Add(M, GetDamage(M));
                        }

                    }
                    else
                    {
                        bool RangeFromChar = true;
                        if (Info.Targetting == TargetType.FromSingle)
                        {
                            if (World.H_Companions.ContainsKey(Single))
                            {
                                Companion M = (Companion)World.H_Companions[Single];

                                if (M.Owner.EntityID != User.EntityID && M.Owner.PKAble(User.PKMode, User))
                                {
                                    CompTargets.Add(M, GetDamage(M));
                                    AimX = M.Loc.X;
                                    AimY = M.Loc.Y;
                                    RangeFromChar = false;
                                }


                            }
                        }
                        else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
                        {
                            AimX = User.Loc.X;
                            AimY = User.Loc.Y;
                            RangeFromChar = true;
                        }
                        else
                            RangeFromChar = false;
                        List<coords> Line = new List<coords>(5);
                        if (Info.Targetting == TargetType.Linear)
                            Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                        //Line = MyMath.LineCoords(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                        foreach (Companion M in World.H_Companions.Values)
                        {
                            if ((!RangeFromChar && MyMath.PointDistance(AimX, AimY, M.Loc.X, M.Loc.Y) <= Info.MaxDist) || MyMath.PointDistance(User.Loc.X, User.Loc.Y, M.Loc.X, M.Loc.Y) <= Info.MaxDist)
                                if (Info.ID == 8001)
                                {
                                    if (Info.Targetting == TargetType.Sector && FunctiaCuUnghiVariabil(AimX, AimY, User.Loc.X, User.Loc.Y, M.Loc.X, M.Loc.Y, Info.SectorSize) || Info.Targetting != TargetType.Sector)
                                        if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(M.Loc.X, M.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                            if (M.Owner.PKAble(User.PKMode, User) && !CompTargets.ContainsKey(M))
                                                if (M.Owner.EntityID != User.EntityID)
                                                    CompTargets.Add(M, GetDamage(M));
                                }
                                else
                                    if (Info.Targetting == TargetType.Sector && InSector(M.Loc.X, M.Loc.Y) || Info.Targetting != TargetType.Sector)
                                    if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(M.Loc.X, M.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                        if (M.Owner.PKAble(User.PKMode, User) && !CompTargets.ContainsKey(M))
                                            if (M.Owner.EntityID != User.EntityID)
                                                CompTargets.Add(M, GetDamage(M));


                        }
                    }
                }
            }
            void GetPlayerTargets(uint Single)
            {
                /* if (Info.ExtraEff == ExtraEffect.FlashStep || Info.ExtraEff == ExtraEffect.Ride || Info.ExtraEff == ExtraEffect.Scapegoat)
                 {
                     PlayerTargets.Add(User, (uint)0);
                     return;
                 }*/
                if (Info.Targetting == TargetType.Single)
                {
                    if (Info.ID == 1051)
                    {
                        if (User.Loc.Map == 1039)
                            return;
                        Location e = User.Loc;
                        e.Walk((byte)Single);
                        Location e2 = e;
                        e2.Walk((byte)Single);
                        bool PlaceFree = true;
                        if (DMaps.Loaded)
                        {
                            if (((DMap)DMaps.H_DMaps[User.Loc.Map]).GetCell(e2.X, e2.Y).NoAccess) PlaceFree = false;
                        }
                        if (PlaceFree && User.Loc.Map == 1038)
                            PlaceFree = User.Loc.AbleToWalkGW(e2.X, e2.Y);
                        if (PlaceFree)
                            PacketHandling.WalkRun.Handle(User.MyClient, new byte[0], (byte)Single);
                        if (Game.ItemIDManipulation.Part(User.Equips.LeftHand.ID, 0, 3) != 900 || User.Equips.LeftHand.CurDur <= 2)
                            return;
                        foreach (Character C in User.ScreenChars.Values)
                        {
                            if (C.Alive)
                                if (C.Loc.X == e.X && C.Loc.Y == e.Y)
                                    if (C.EntityID != User.EntityID)
                                        if (C.CanBeMeleed && C.PKAble(User.PKMode, User))
                                            if (C.ProtectTime.AddMilliseconds(0) < DateTime.Now || C.CancelProtectTime)
                                                if (C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30))
                                                { PlayerTargets.Add(C, Single); User.Equips.LeftHand.CurDur -= 2; break; }
                        }
                    }
                    else
                    {
                        Character C = null;
                        if (World.H_Chars.ContainsKey(Single))
                            C = World.H_Chars[Single];


                        if (C != null)
                            if ((C.Alive || Info.ExtraEff == ExtraEffect.Revive) && MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= 18 && User.Loc.Map == C.Loc.Map)
                                if (C.CanBeMeleed || (Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee))
                                    if (C.PKAble(User.PKMode, User) && User.EntityID != C.EntityID || Info.ExtraEff != ExtraEffect.None || Info.Damageing == DamageType.HealHP || Info.Damageing == DamageType.HealMP)
                                        if (C.ProtectTime.AddMilliseconds(0) < DateTime.Now || C.CancelProtectTime)
                                            if (((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30))) || C.EntityID == User.EntityID || (C.Level <= 6 && (C.Loc.Map != 1002 && C.Loc.Map != 1011 && C.Loc.Map != 1004  && C.Loc.Map != 1020 && C.Loc.Map != 1000 && C.Loc.Map != 1015 && C.Loc.Map != 1009)))
                                            {
                                                if ((Info.Damageing == DamageType.HealHP || Info.ExtraEff == ExtraEffect.Roar) && User.MyTeam != null && (Info.ID == 1170 || Info.ID == 1055 || Info.ID == 1040))
                                                {
                                                    //if (User.Loc.Map == 1080)
                                                    //{
                                                    //    if (User.BlueTeam && C.BlueTeam)
                                                    //        foreach (Main.GameClient C2 in Events.CaptureTheBag.Tea.Values)
                                                    //        {
                                                    //            if (MyMath.InBox(User.Loc.X, User.Loc.Y, C2.MyChar.Loc.X, C2.MyChar.Loc.Y, 20) && User.Loc.Map == C2.MyChar.Loc.Map && C2.MyChar.Alive)
                                                    //                PlayerTargets.Add(C2.MyChar, GetDamage(C2.MyChar));
                                                    //        }
                                                    //    else if (User.RedTeam && C.RedTeam)
                                                    //    {
                                                    //        foreach (Main.GameClient C2 in Features.CaptureTheBag.RedTeam.Values)
                                                    //        {
                                                    //            if (MyMath.InBox(User.Loc.X, User.Loc.Y, C2.MyChar.Loc.X, C2.MyChar.Loc.Y, 20) && User.Loc.Map == C2.MyChar.Loc.Map && C2.MyChar.Alive)
                                                    //                PlayerTargets.Add(C2.MyChar, GetDamage(C2.MyChar));
                                                    //        }
                                                    //    }
                                                    //}
                                                    if (User.MyTeam.Members.Contains(C))
                                                        foreach (Character C2 in User.MyTeam.Members)
                                                        {
                                                            if (MyMath.InBox(User.Loc.X, User.Loc.Y, C2.Loc.X, C2.Loc.Y, 20) && User.Loc.Map == C2.Loc.Map && C2.Alive)
                                                                PlayerTargets.Add(C2, GetDamage(C2));
                                                        }
                                                }
                                                else
                                                {
                                                    //User.MyClient.LocalMessage(2000, "New target: " + C.Name);
                                                    if ((C.Loc.Map == 1039 || C.Loc.Map == 1004) && C != User)
                                                    {
                                                        if (Info.ID == 1095 || Info.ID == 1075 || Info.ID == 1085 || Info.ID == 1090 || Info.Damageing == DamageType.HealHP)
                                                            PlayerTargets.Add(C, GetDamage(C));
                                                        else
                                                        {
                                                            User.AtkMem.Attacking = false;
                                                            User.AtkMem.Target = 0;
                                                            return;
                                                        }
                                                    }
                                                    else if (World.NoPKMaps.Contains(C.Loc.Map) && Info.ExtraEff == ExtraEffect.None)
                                                    {
                                                        User.AtkMem.Attacking = false;
                                                        User.AtkMem.Target = 0;
                                                        return;
                                                    }
                                                    else
                                                        PlayerTargets.Add(C, GetDamage(C));

                                                }
                                            }
                                            else if (MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30) && Info.Damageing == DamageType.HealHP)
                                                if (World.DragonTank != null)
                                                    PlayerTargets.Add(World.DragonTank, GetDamage(World.DragonTank));
                                                else
                                                    PlayerTargets.Add(C, GetDamage(C));
                                            else
                                                User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");

                    }
                }
                else
                {
                    bool RangeFromChar = true;
                    if (Info.Targetting == TargetType.FromSingle)
                    {
                        Character C = null;
                        if (World.H_Chars.ContainsKey(Single))
                            C = World.H_Chars[Single];
                        if (C != null)
                        {
                            if (C.Alive)
                            {
                                if (User.Loc.Map == C.Loc.Map)
                                    if (C.PKAble(User.PKMode, User) && C.EntityID != User.EntityID || Info.Damageing == DamageType.HealHP)
                                        if (C.CanBeMeleed || (Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee))
                                        {
                                            if (C.ProtectTime.AddMilliseconds(0) < DateTime.Now || C.CancelProtectTime)
                                                if ((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30)) || (C.Level <= 6 && (C.Loc.Map != 1002 && C.Loc.Map != 1011 && C.Loc.Map != 1020 && C.Loc.Map != 1004 && C.Loc.Map != 1000 && C.Loc.Map != 1015 && C.Loc.Map != 1009)))
                                                {
                                                    PlayerTargets.Add(C, GetDamage(C));
                                                    AimX = C.Loc.X;
                                                    AimY = C.Loc.Y;
                                                    RangeFromChar = false;
                                                }
                                                else
                                                    User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
                                        }
                            }
                        }
                    }
                    else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
                    {
                        AimX = User.Loc.X;
                        AimY = User.Loc.Y;
                        RangeFromChar = true;
                    }
                    else
                        RangeFromChar = false;
                    //InLineAlgorithm ila = new InLineAlgorithm(User.Loc.X,
                    //                    AimX, User.Loc.Y, AimY, (byte)Info.MaxDist, InLineAlgorithm.Algorithm.DDA);
                    List<coords> Line = new List<coords>(5);
                    if (Info.Targetting == TargetType.Linear)
                        Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                    /*  List<coords> Line = new List<coords>(5);
                      if (Info.Targetting == TargetType.Linear)
                          Line = MyMath.LineCoords(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist); */

                    List<StoreLoc> Coords = new List<StoreLoc>();
                    // ConcurrentDictionary<uint, Character> Map = (ConcurrentDictionary<uint, Character>)World.PlayersInMap[User.Loc.Map];
                    // ThreadSafeList<uint> Map = (ThreadSafeList<uint>)World.PlayersInMap[User.Loc.Map];
                    // foreach(Character C in Map.Values)
                    // foreach (uint k in Map.Keys)
#warning Changed on 24/10/2016 - attempt to reduce nesting on linear skills
                    foreach (Character C in User.ScreenChars.Values)
                    {
                        // Character C = World.H_Chars[k];
                        if (C != null)
                            if (User.Loc.Map == C.Loc.Map) //  REVERSE TO ALL PLAYERS
                                if (C.Alive && (C.PKAble(User.PKMode, User) && User.EntityID != C.EntityID || Info.Damageing == DamageType.HealHP || Info.Damageing == DamageType.HealMP))
                                {
#warning Attempt to fix Magic Skills Range!
                                    if ((!RangeFromChar && MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist) || MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist)
                                        if (Info.Targetting == TargetType.Sector && InSector(C.Loc.X, C.Loc.Y) || Info.Targetting != TargetType.Sector)
                                            if ((C.CanBeMeleed || (Info.ID != 1115 && Info.ID != 1120 && Info.Damageing != DamageType.Melee)) && ((Info.Targetting == TargetType.Linear && Line.Contains(new coords(C.Loc.X, C.Loc.Y)) /*ila.InLine(C.Loc.X, C.Loc.Y)*/) /* Line.Contains(new coords(C.Loc.X, C.Loc.Y))*/ || Info.Targetting != TargetType.Linear))
                                                if (!PlayerTargets.ContainsKey(C) && (!World.NoPKMaps.Contains(User.Loc.Map) || GetDamage(C) == 0))  //if (!PlayerTargets.Contains(C) && !World.NoPKMaps.Contains(User.Loc.Map))
                                                                                                                                                     //if (Game.World.NoPKMaps.Contains(User.Loc.Map) && GetDamage(C) == 0 || !Game.World.NoPKMaps.Contains(User.Loc.Map))
                                                    if (C.ProtectTime.AddMilliseconds(0) < DateTime.Now || C.CancelProtectTime)
                                                        if ((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30)) || C.EntityID == User.EntityID || (C.Level <= 6 && (C.Loc.Map != 1002 && C.Loc.Map != 1011 && C.Loc.Map != 1004 && C.Loc.Map != 1020 && C.Loc.Map != 1000 && C.Loc.Map != 1015 && C.Loc.Map != 1009)))
                                                        {
                                                            C.SLoc.Map = C.Loc.Map;
                                                            C.SLoc.X = C.Loc.X;
                                                            C.SLoc.Y = C.Loc.Y;
                                                            DMap DM = ((DMap)DMaps.H_DMaps[C.Loc.Map]);
                                                            if (DM != null)
                                                            {
                                                                DMapCell New = DM.GetCell(C.Loc.X, C.Loc.Y);
                                                                DMapCell Old = DM.GetCell(User.Loc.X, User.Loc.Y);

                                                                if (New.High && Old.High && !Coords.Contains(C.SLoc))
                                                                {
                                                                    Coords.Add(C.SLoc);
                                                                    PlayerTargets.Add(C, GetDamage(C));
                                                                    if (World.PKTourny)
                                                                        if (User.Loc.Map == 8000)
                                                                        {
                                                                            if ((Game.ItemIDManipulation.Part(C.Equips.LeftHand.ID, 0, 3) == 500))
                                                                            {
                                                                                C.Teleport(1002, 430, 380);
                                                                            }
                                                                            User.PKTHits++;
                                                                            if (!World.PKTList.Contains(User.EntityID))
                                                                                World.PKTList.Add(User.EntityID);
                                                                        }
                                                                }
                                                                else if (!New.High && !Old.High && !Coords.Contains(C.SLoc))
                                                                {
                                                                    if ((C.Loc.Map == 10391 || C.Loc.Map == 1004) && C != User) //emre tg disable pk
                                                                        return;
                                                                    else
                                                                    {
                                                                        Coords.Add(C.SLoc);
                                                                        PlayerTargets.Add(C, GetDamage(C));
                                                                        if (World.PKTourny)
                                                                            if (User.Loc.Map == 8000)
                                                                            {
                                                                                if ((Game.ItemIDManipulation.Part(C.Equips.LeftHand.ID, 0, 3) == 500))
                                                                                {
                                                                                    C.Teleport(1002, 430, 380);
                                                                                }
                                                                                User.PKTHits++;
                                                                                World.SendMsgToAll("SYSTEM", "Name : " + User.Name + " Point :  " + User.PKTHits + "", 2005, 0, User.Loc.Map);

                                                                                if (!World.PKTList.Contains(User.EntityID))
                                                                                    World.PKTList.Add(User.EntityID);
                                                                            }
                                                                    }
                                                                }
                                                                else if (!Coords.Contains(C.SLoc) && C.Loc.Map == 1038)
                                                                {
                                                                    if ((!New.High && Old.High) || (!Old.High && New.High))
                                                                    {
                                                                        if ((C.Loc.X >= 160 && C.Loc.X <= 171) || (C.Loc.Y >= 174 && C.Loc.Y <= 185))
                                                                        {
                                                                            Coords.Add(C.SLoc);
                                                                            PlayerTargets.Add(C, GetDamage(C));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else if (!Coords.Contains(C.SLoc))
                                                            {
                                                                Coords.Add(C.SLoc);
                                                                PlayerTargets.Add(C, GetDamage(C));
                                                                if (World.PKTourny)
                                                                    if (User.Loc.Map == 8000)
                                                                    {
                                                                        if ((Game.ItemIDManipulation.Part(C.Equips.LeftHand.ID, 0, 3) == 500))
                                                                        {
                                                                            C.Teleport(1002, 430, 380);
                                                                        }
                                                                        User.PKTHits++;
                                                                        if (!World.PKTList.Contains(User.EntityID))
                                                                            World.PKTList.Add(User.EntityID);
                                                                    }
                                                            }

                                                        }
                                                        else
                                                            User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
                                }
                    }

                }
            }
            void GetNPCTargets(uint Single)
            {
                if (Info.ExtraEff == ExtraEffect.Ride || Info.ExtraEff == ExtraEffect.UnMount) return;
                if (Info.Targetting == TargetType.Single)
                {
                    if (World.H_NPCs.ContainsKey(User.Loc.Map))
                    {
                        Dictionary<uint, NPC> MapNPC = World.H_NPCs[User.Loc.Map];
                        if (MapNPC.ContainsKey(Single))
                        {
                            NPC C = (NPC)MapNPC[Single];
                            if (C.Flags == 21 || C.Flags == 22)
                                NPCTargets.Add(C, GetDamage(C));
                        }
                    }
                }
                else
                {
                    bool RangeFromChar = true;
                    if (Info.Targetting == TargetType.FromSingle)
                    {
                        if (World.H_NPCs.ContainsKey(User.Loc.Map))
                        {
                            Dictionary<uint, NPC> MapNPC = World.H_NPCs[User.Loc.Map];
                            if (MapNPC.ContainsKey(Single))
                            {
                                NPC C = (NPC)MapNPC[Single];
                                if (C.Flags == 21 || C.Flags == 22)
                                {
                                    NPCTargets.Add(C, GetDamage(C));
                                    AimX = C.Loc.X;
                                    AimY = C.Loc.Y;
                                    RangeFromChar = false;
                                }
                            }
                        }

                    }
                    else if (Info.Targetting != TargetType.Sector && Info.Targetting != TargetType.Linear && Info.Targetting != TargetType.Range && Info.Targetting != TargetType.FromPoint)
                    {
                        AimX = User.Loc.X;
                        AimY = User.Loc.Y;
                        RangeFromChar = true;
                    }
                    else
                        RangeFromChar = false;
                    List<coords> Line = new List<coords>(5);
                    if (Info.Targetting == TargetType.Linear)
                        Line = MyMath.GetLinePoints(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                    //Line = MyMath.LineCoords(User.Loc.X, User.Loc.Y, AimX, AimY, Info.MaxDist);
                    if (World.H_NPCs.ContainsKey(User.Loc.Map))
                    {
                        Dictionary<uint, NPC> MapNPC = World.H_NPCs[User.Loc.Map];
                        foreach (NPC C in MapNPC.Values)
                        {
                            if ((C.Flags == 21 || C.Flags == 22) && User.Level >= C.Level)
                                if ((!RangeFromChar && MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist) || MyMath.PointDistance(User.Loc.X, User.Loc.Y, C.Loc.X, C.Loc.Y) <= Info.MaxDist)
                                    if (Info.Targetting == TargetType.Sector && InSector(C.Loc.X, C.Loc.Y) || Info.Targetting != TargetType.Sector)
                                        if (Info.Targetting == TargetType.Linear && Line.Contains(new coords(C.Loc.X, C.Loc.Y)) || Info.Targetting != TargetType.Linear)
                                            if (!NPCTargets.ContainsKey(C))
                                                NPCTargets.Add(C, GetDamage(C));
                        }
                    }
                }
            }
            public bool FunctiaCuUnghiVariabil(double xA, double yA, double xB, double yB, double x, double y, double angle = 180)
            {
                if (angle <= 0)
                    return false;

                if (angle >= 360)
                    return true;

                if (yA == yB && xA == xB) // dai click dreapta fix unde esti tu, cam imposibil de calculat...
                    return false;

                // xA, yA - coord clikului
                // xB, yB - coord playerului
                // x, y - coord monstrului pe care vrei sa-l verifici
                // angle - unghiul(in grade)

                double ang1 = Math.Atan2(yA - yB, xA - xB) * 180 / Math.PI; // unghiul facut de dreapta determinata de coord. clickului si coord. pozitiei charului cu Ox
                double A1 = ang1 - angle / 2;
                double A2 = ang1 + angle / 2;
                double A1rad = A1 * Math.PI / 180;
                double A2rad = A2 * Math.PI / 180;
                double m1 = Math.Tan(A1rad);
                double m2 = Math.Tan(A2rad);

                var s1 = Math.Sign(y - yB - m1 * (x - xB));
                var s2 = Math.Sign(yA - yB - m1 * (xA - xB));
                var s3 = Math.Sign(y - yB - m2 * (x - xB));
                var s4 = Math.Sign(yA - yB - m2 * (xA - xB));

                return (s1 == s2) || (s3 == s4) || (s1 == 0) || (s3 == 0);

            }
            bool Functia(double xA, double yA, double xB, double yB, double x, double y)
            {
                if (yA == yB && xA == xB)	// dai click dreapta fix unde esti tu, cam imposibil de calculat...
                    return false;
                // xA, yA - coord clikului
                // xB, yB - coord playerului
                // x, y - coord monstrului pe care vrei sa-l verifici

                // varianta cea mai optimizata
                // stiu ca arata ca dreq de aia ti-am scris mai jos varianta explicita
                //return (yB != yA) ? (((yA - yB + (xB - xA) / (yB - yA) * (xA - xB)) >= 0) == ((y - yB + (xB - xA) / (yB - yA) * (x - xB)) >= 0)) : ((xB - xA >= 0) == (xB - x >= 0));


                if (yB == yA)
                    return (xB - xA >= 0) == (xB - x >= 0);

                double md = -(xB - xA) / (yB - yA); // panta dreptei
                bool pos = (yA - yB - md * (xA - xB)) >= 0;
                bool aux = (y - yB - md * (x - xB)) >= 0;

                return aux == pos;

            }

            /*   bool FunctiaCuUnghiVariabil(double xA, double yA, double xB, double yB, double x, double y, double angle)
               {
                   if (angle <= 0)
                       return false;

                   if (angle >= 360)
                       return true;

                   if (yA == yB && xA == xB)	// dai click dreapta fix unde esti tu, cam imposibil de calculat...
                       return false;

                   // xA, yA - coord clikului
                   // xB, yB - coord playerului
                   // x, y - coord monstrului pe care vrei sa-l verifici
                   // angle - unghiul(in grade)

                   // varianta cea mai optimizata
                   // stiu ca arata ca dreq de aia ti-am scris mai jos varianta explicita

                   if (yB == yA)
                       return (xB - xA >= 0) == (xB - x >= 0);

                   double md1, md2;

                   if (angle == 180)
                       md2 = md1 = -(xB - xA) / (yB - yA);
                   else
                   {
                       // foloseste o functie pt calcularea tangentei...

                       md1 = Math.Tan((angle/2 - 90) / 180 * Math.PI);
                       md2 = -md1;
                   }

                   double distY, distX;
                   if (y < yB)
                       distY = yB - y;
                   else
                       distY = y - yB;

                   if (x < xB)
                       distX = xB - x;
                   else
                       distX = x - xB;

                   bool pos = (yA - yB - md1 * (xA - xB)) >= 0;
                   bool aux = (distY - md1 * (distX)) >= 0;
                   bool pos1 = (yA - yB - md2 * (xA - xB)) >= 0;
                   bool aux1 = (distY - md2 * (distX)) >= 0;

                   if (angle >= 180)
                       return aux1 == pos1 || aux == pos;
                   else
                       return (aux1 == pos1) && (aux == pos);
               } */
            public bool InSector(ushort X, ushort Y)
            {
                double Aim = MyMath.PointDirecton2(User.Loc.X, User.Loc.Y, AimX, AimY);
                double MobAngle = MyMath.PointDirecton2(User.Loc.X, User.Loc.Y, X, Y);

                if (Aim - Info.SectorSize / 2 < MobAngle)
                    if (Aim + Info.SectorSize / 2 > MobAngle)
                        return true;
                return false;
            }
            //public static uint x = 2;
            public uint GetDamage(Character C)
            {
                if (Info.ExtraEff == ExtraEffect.Roar)
                    return 20;
                if (Info.ExtraEff != ExtraEffect.None && Info.ExtraEff != ExtraEffect.FlashStep)
                    return 0;
                uint Damage = 1;
                if (C.Protection && C.Loc.Map != 1038 && C.Loc.Map != 1005)
                    return 0;
                if (Info.ExtraEff == ExtraEffect.NoPots || Info.ExtraEff == ExtraEffect.RemoveFly)
                    return 1;
                if (C.Loc.Map != User.Loc.Map)
                    return 0;
                if (C.EventBase != null)
                    if (C.EventBase.NoDamage && C.EventBase?.Stage == Events.EventStage.Fighting)
                        return C.EventBase.GetDamage(User, C, Info);
                if (C.Arena != null && C.Arena.MapID == C.Loc.Map)
                    return C.Arena.GetDamage(User, C, Info);

                //if (C.Loc.Map == 1080 || (C.EventBase?.Stage == Events.EventStage.Fighting && C.EventBase?.MapEvent == C.Loc.Map && C.EventBase.FFADamage))
                //{
                //    if (Info.ID == 8001)
                //        return 1;
                //    else if (Info.ID == 1046 || Info.ID == 1045 || Info.ID == 1047)
                //        return 8;
                //    else if (Info.ID == 1000 || Info.ID == 1165)
                //        return 2;
                //    else if (Info.ID == 1001 || Info.ID == 1115/* || Info.ID == 1005*/)
                //        return 3;
                //    else if (Info.ID == 1150 || Info.ID == 1160 || Info.ID == 1180 || Info.ID == 1002)
                //        return 4;
                //    else if (Info.ID == 1120)
                //        return 6;
                //    else if (Info.ID == 1320)
                //        return 15;
                //    else if (Info.ID == 5001 || Info.ID == 1125 || Info.ID == 1010)
                //        return 10;
                //    else if (Info.ID != 1175 && Info.ID != 1170 && Info.ID != 1005 && Info.ID != 1055 && Info.ID != 1190 && Info.ID != 1195)
                //        return 3;
                //}


                switch (Info.Damageing)
                {
                    case DamageType.Percent:
                        {

                            if (World.NoPKMaps.Contains(User.Loc.Map))
                                if (Info.ExtraEff == ExtraEffect.None)
                                    return 0;

                            Damage = (uint)(C.CurHP * Info.EffectValue);
                            if (User.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                                Damage *= 3;
                            Damage = (uint)(Math.Floor((double)Damage * (100 - C.EqStats.TotalBless) / 100));
                            if (C.CanReflect && MyMath.ChanceSuccess(10))
                            {
                                if (Damage >= 2600)
                                    Damage = 2600;

                                User.GetReflect(Damage, AttackType.Magic);
                                World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "MagicReflect").Get);
                                return 0;
                            }

                            break;
                        }
                    case DamageType.Melee:
                        {

                            if (World.NoPKMaps.Contains(User.Loc.Map))
                                if (Info.ExtraEff == ExtraEffect.None)
                                    return 0;
                            //if (User.BuffOf(ExtraEffect.Fly).Eff == ExtraEffect.Fly)
                            if (User.Flying)
                                return 0;

                            ushort Def;
                            if (!C.Transformation.Transformed)
                                Def = C.EqStats.defense;
                            else Def = C.Transformation.Def;
                            Buff Shield = C.BuffOf(SkillsClass.ExtraEffect.MagicShield);
                            if (Shield.Eff == SkillsClass.ExtraEffect.MagicShield)
                                Def = (ushort)(Def * Shield.Value);
                            /*  if (Info.ID == 1115)
                              {
                                  Def += (ushort)(Def / 8);
                                  Def = (ushort)(Def * 2);
                              }*/
                            Damage = User.PrepareAttack(2, false);
                            if (User.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                                Damage *= 3;
                            Damage = (uint)(Damage * Info.EffectValue);
                            if (Info.ExtraEff == ExtraEffect.FlashStep)
                                Damage /= 2;
                            Damage += Info.Damage;


                            if (Info.ID == 1115)
                            {
                                Damage = (uint)(Damage * 0.45);//0.45
                            }
                            if (Info.ID == 3050)
                                return Convert.ToUInt32(C.CurHP * Info.EffectValue);
                            //if (C.Reborns == 1 && User.Reborns == 0)
                            //{
                            //    Damage = (uint)Math.Floor((double)Damage * .7);
                            //}
                            if (C.Reborns == 1 && C.Level >= 70)
                                Def = (ushort)(Def * 1.3);

                            Damage = (uint)(Math.Floor((double)Damage * (1 - ((C.EqStats.GemBless < .52) ? C.EqStats.GemBless : .52))));

                            Damage = (uint)(Math.Floor((double)Damage * (100 - C.EqStats.TotalBless) / 100));

                            Damage += User.EqStats.MeleeDamageIncrease;

                            if (Def >= Damage)
                                Damage = 1;
                            else
                                Damage -= Def;

                            if (C.EqStats.MeleeDamageDecrease >= Damage)
                                Damage = 1;
                            else
                                Damage -= C.EqStats.MeleeDamageDecrease;

                            if (C.CanReflect && MyMath.ChanceSuccess(10))
                            {
                                if (Damage >= 2600)
                                    Damage = 2600;

                                User.GetReflect(Damage, AttackType.Melee);
                                World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "MagicReflect").Get);
                                return 0;
                            }

                            break;
                        }
                    case DamageType.Ranged:
                        {
                            if (MyMath.ChanceSuccess(C.WarriorDodge))//checks shield dodge on warrior and return 0 if it's dodged
                                return 0;
                            if (World.NoPKMaps.Contains(User.Loc.Map))
                                if (Info.ExtraEff == ExtraEffect.None)
                                    return 0;
                            //if (User.BuffOf(ExtraEffect.Fly).Eff == ExtraEffect.Fly)
                            //if (User.Flying)
                            //    return 0;

                            byte dodgev;
                            if (!C.Transformation.Transformed)
                                dodgev = C.EqStats.Dodge;
                            else dodgev = C.Transformation.Dodge;
                            if (dodgev > 95)
                                dodgev = 95;
                            Buff Dodge = C.BuffOf(SkillsClass.ExtraEffect.Dodge);
                            if (Dodge.Eff == SkillsClass.ExtraEffect.Dodge)
                                dodgev = (byte)(dodgev * Dodge.Value);
                            if (dodgev < 20)
                                dodgev = 20;
                            if (dodgev > 105)
                                dodgev = 105;
                            Damage = User.PrepareAttack(28, false);
                            if (Info.EffectValue != 0)
                                Damage = (uint)(Damage * Info.EffectValue);
                            if (User.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                                Damage *= 3;
                            Damage += Info.Damage;
                            Damage = (uint)((Damage * (((double)(110 - dodgev)) / 110)) / 6);
                            // Damage = (uint)((Damage * (((double)(100 - dodgev)) / 100)) / 6);

                            //Damage = (uint)((Damage * (((double)(304 - C.EqStats.Dodge)) / 300)) / 12);
                            // Damage = (uint)((Damage * (((double)(200 - dodgev)) / 200)) / 13); before
                            Damage = (uint)(Math.Floor((double)Damage * (1 - ((C.EqStats.GemBless < .52) ? C.EqStats.GemBless : .52))));
                            Damage = (uint)(Math.Floor((double)Damage * (100 - C.EqStats.TotalBless) / 100));

                            Damage += User.EqStats.MeleeDamageIncrease;
                            if (C.EqStats.MeleeDamageDecrease >= Damage)
                                Damage = 1;
                            else
                                Damage -= C.EqStats.MeleeDamageDecrease;
                            if (Damage > 1500)
                                Damage = 1500;
                            if (C.CanReflect && MyMath.ChanceSuccess(10))
                            {
                                if (Damage >= 2600)
                                    Damage = 2600;

                                User.GetReflect(Damage, AttackType.Ranged);
                                World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "MagicReflect").Get);
                                return 0;
                            }

                            break;
                        }
                    case DamageType.Magic:
                        {

                            if (World.NoPKMaps.Contains(User.Loc.Map))
                                if (Info.ExtraEff == ExtraEffect.None)
                                    return 0;
                            // if (Info.ID == 3080)
                            //   return 0;       
                            //var dmg = (CombatStats.MagicDamage + _spell.Power) * (200 + CombatStats.PhoenixGemPct) / 100;
                            Damage = User.PrepareAttack(21, false, Info.Damage);
                            Damage = (uint)(Damage * 0.75);
                            if (Info.EffectValue != 0)
                                Damage = (uint)(Damage * Info.EffectValue);
                            //Damage += Info.Damage;
                            ushort MDPC = C.EqStats.MDef1;
                            if (MDPC < 10)
                                MDPC = 10;
                            // if (MDPC > 80)
                            //  MDPC = 80;
                            if (C.EqStats.MDef2 >= Damage)
                                Damage = 1;
                            else
                                Damage -= C.EqStats.MDef2;
                            if (C.EqStats.MagicDamageDecrease >= Damage)
                                Damage = 1;
                            else
                                Damage -= C.EqStats.MagicDamageDecrease;
                            if (!C.Transformation.Transformed)
                                if (MDPC >= 90)
                                    Damage = (uint)((double)Damage * (((double)10 / 100)));
                                else
                                    Damage = (uint)((double)Damage * (((double)(100 - MDPC) / 100)));
                            else Damage = (uint)((double)Damage * (((double)(100 - C.Transformation.MagicDef) / 100)));


                            Damage = (uint)(Math.Floor((double)Damage * (1 - ((C.EqStats.GemBless < .52) ? C.EqStats.GemBless : .52))));
                            Damage = (uint)(Math.Floor((double)Damage * (100 - C.EqStats.TotalBless) / 100));
                            Damage += User.EqStats.MagicDamageIncrease;
                            Damage += (uint)(User.EqStats.MDamage * User.EqStats.GemExtraMAttack);



                            if (C.CanReflect && MyMath.ChanceSuccess(10))
                            {
                                if (Damage >= 2600)
                                    Damage = 2600;

                                User.GetReflect(Damage, AttackType.Ranged);
                                World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "MagicReflect").Get);
                                return 0;
                            }

                            break;
                        }
                    case DamageType.HealHP:
                        {
                            Damage = Info.Damage;
                            if (Damage > (uint)(C.MaxHP - C.CurHP) && C.Loc.Map != 1080 && C.Loc.Map != 1017)
                                Damage = (uint)(C.MaxHP - C.CurHP);

                            C.CurHP += (ushort)Info.Damage;

                            if (World.DragonTank != null)
                            {
                                if (World.DragonTank.MyClient.Soc.Connected)
                                {
                                    if (World.DragonTank.EntityID == C.EntityID)
                                    {
                                        if (World.DragonHeal.ContainsKey(User.EntityID))
                                        {
                                            User.DragonHeal += Damage;
                                        }
                                        else
                                        {
                                            World.DragonHeal.Add(User.EntityID, User);
                                            User.DragonHeal += Damage;
                                        }
                                    }
                                }
                            }
                            if (C.CurHP > C.MaxHP)
                                C.CurHP = C.MaxHP;
                            break;
                        }
                    case DamageType.HealMP:
                        {
                            Damage = Info.Damage;
                            if (Damage > (uint)(C.MaxMP - C.CurMP))
                                Damage = (uint)(C.MaxMP - C.CurMP);
                            C.CurMP += (ushort)Info.Damage;
                            if (C.CurMP > C.MaxMP)
                                C.CurMP = C.MaxMP;
                            break;
                        }
                }
                return Damage;
            }
            public uint GetDamage(NPC C)
            {
                if (User.EventBase != null)
                    if (User.EventBase.NoDamage && User.EventBase?.Stage == Events.EventStage.Fighting)
                        return User.EventBase.GetDamage(User, C, Info);
                uint Damage = 1;
                switch (Info.Damageing)
                {
                    case DamageType.Percent:
                        {
                            Damage = (uint)(C.CurHP * Info.EffectValue);
                            break;
                        }
                    case DamageType.Melee:
                        {
                            Damage = User.PrepareAttack(2, false);
                            if (Info.EffectValue != 0)
                                Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;
                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Ranged:
                        {
                            Damage = User.PrepareAttack(28, false);
                            if (Info.EffectValue != 0)
                                Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;
                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Magic:
                        {
                            Damage = User.PrepareAttack(21, false);
                            if (Info.EffectValue != 0)
                                Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;
                            Damage += User.EqStats.MagicDamageIncrease;
                            break;
                        }
                    case DamageType.HealHP:
                        {
                            Damage = Info.Damage;
                            C.CurHP += (ushort)Info.Damage;
                            if (C.CurHP > C.MaxHP)
                                C.CurHP = C.MaxHP;
                            break;
                        }
                }
                if (C.Flags == 21) Damage = (uint)((double)Damage * 0.75);
                return Damage;
            }
            public uint GetDamage(Mob M)
            {
                uint Damage = 1;
                /*   if (M.Loc.Map != User.Loc.Map)
                      return 0;*/
                switch (Info.Damageing)
                {
                    case DamageType.Percent:
                        {
                            Damage = (uint)(M.CurrentHP * Info.EffectValue);
                            break;
                        }
                    case DamageType.Melee:
                        {
                            Damage = User.PrepareAttack(2, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;

                            if (M.Defense >= Damage)
                                Damage = 1;
                            else
                                Damage -= M.Defense;
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));

                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Ranged:
                        {
                            Damage = User.PrepareAttack(28, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;
                            Damage = (uint)((Damage * (((double)(155 - M.Dodge)) / 100)));
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));
                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Magic:
                        {
                            Damage = User.PrepareAttack(21, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;

                            if (M.MDef >= Damage)
                                Damage = 1;
                            else
                                Damage -= M.MDef;
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));
                            Damage += User.EqStats.MagicDamageIncrease;
                            break;
                        }
                    case DamageType.HealHP:
                        {
                            Damage = Info.Damage;
                            M.CurrentHP += Info.Damage;
                            if (M.CurrentHP > M.MaxHP)
                                M.CurrentHP = M.MaxHP;
                            break;
                        }
                }
                if (Damage != 0 && M.DmgReduceTimes != 0)
                    Damage = (uint)(Damage / M.DmgReduceTimes);

                return Damage;
            }
            public uint GetDamage(Companion M)
            {
                uint Damage = 1;
                /*   if (M.Loc.Map != User.Loc.Map)
                      return 0;*/
                switch (Info.Damageing)
                {
                    case DamageType.Percent:
                        {
                            Damage = (uint)(M.CurHP * Info.EffectValue);
                            break;
                        }
                    case DamageType.Melee:
                        {
                            Damage = User.PrepareAttack(2, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;

                            if (M.Def >= Damage)
                                Damage = 1;
                            else
                                Damage -= M.Def;
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));

                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Ranged:
                        {
                            Damage = User.PrepareAttack(28, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;
                            Damage = (uint)((Damage * (((double)(180 - M.Dodge)) / 100)));
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));
                            Damage += User.EqStats.MeleeDamageIncrease;
                            break;
                        }
                    case DamageType.Magic:
                        {
                            Damage = User.PrepareAttack(21, false);
                            Damage = (uint)(Damage * Info.EffectValue);
                            Damage += Info.Damage;

                            if (M.Def / 2 >= Damage)
                                Damage = 1;
                            else
                                Damage -= (uint)M.Def / 2;
                            Damage = (uint)(Damage * MyMath.LevelDifference(User.Level, M.Level));
                            Damage += User.EqStats.MagicDamageIncrease;
                            break;
                        }
                }
                return Damage;
            }
            public uint GetDamage(SOB S)
            {
                uint Damage = 1;
                if (S.Type == Looks.Statue)
                {
                    Damage = 10;
                }
                else
                {
                    switch (Info.Damageing)
                    {
                        case DamageType.Percent:
                            {
                                Damage = (uint)(S.CurHP * Info.EffectValue);
                                break;
                            }
                        case DamageType.Melee:
                            {
                                Damage = User.PrepareAttack(2, false);
                                Damage = (uint)(Damage * Info.EffectValue);
                                Damage += Info.Damage;
                                Damage += User.EqStats.MeleeDamageIncrease;
                                break;
                            }
                        case DamageType.Ranged:
                            {
                                Damage = User.PrepareAttack(28, false);
                                Damage = (uint)(Damage * Info.EffectValue);
                                Damage += Info.Damage;
                                Damage += User.EqStats.MeleeDamageIncrease;
                                break;
                            }
                        case DamageType.Magic:
                            {
                                Damage = User.PrepareAttack(21, false);
                                Damage = (uint)(Damage * Info.EffectValue);
                                Damage += Info.Damage;
                                Damage += User.EqStats.MagicDamageIncrease;
                                break;
                            }
                    }
                }
                return Damage;
            }
            public void Use()
            {
                try
                {
                    uint Exp = 0;
                    uint SkillExp = 0;
                    foreach (KeyValuePair<uint, uint> DE in MiscTargets)
                    {
                        uint EntityID = (uint)DE.Key;
                        uint Damage = (uint)DE.Value;

                        if (World.H_SOBs.ContainsKey(EntityID))
                            World.H_SOBs[EntityID].TakeAttack(User, Damage, 21);

                        #region unused
                        //if (EntityID == 6700)
                        //    GuildWars.ThePole.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6701)
                        //    GuildWars.TheLeftGate.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6702)
                        //    GuildWars.TheRightGate.TakeAttack(User, Damage, 21);

                        #region Counter Clock GW
                        //if (EntityID == 6726)
                        //    CounterClock.ThePole.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6703)
                        //    CounterClock.LG5.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6704)
                        //    CounterClock.LG2.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6705)
                        //    CounterClock.LG3.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6706)
                        //    CounterClock.LG4.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6707)
                        //    CounterClock.LG5.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6708)
                        //    CounterClock.LG6.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6709)
                        //    CounterClock.RG1.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6710)
                        //    CounterClock.RG2.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6711)
                        //    CounterClock.RG3.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6712)
                        //    CounterClock.RG4.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6713)
                        //    CounterClock.RG5.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6714)
                        //    CounterClock.RG6.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6715)
                        //    CounterClock.RG7.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6716)
                        //    CounterClock.RG8.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6717)
                        //    CounterClock.RG9.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6718)
                        //    CounterClock.RG10.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6719)
                        //    CounterClock.RG11.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6720)
                        //    CounterClock.RG12.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6721)
                        //    CounterClock.RG13.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6722)
                        //    CounterClock.RG14.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6723)
                        //    CounterClock.RG15.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6724)
                        //    CounterClock.RG16.TakeAttack(User, Damage, 21);
                        //if (EntityID == 6725)
                        //    CounterClock.RG17.TakeAttack(User, Damage, 21);
                        #endregion
                        #endregion

                        if (Info.ID == 5010 || Info.ID == 5050 || Info.ID == 1250 || Info.ID == 5020 || Info.ID == 1260 || Info.ID == 1290 || Info.ID == 1300 || Info.ID == 7020 || Info.ID == 7040)
                            SkillExp += 100;
                        else if (Info.ID == 1045 || Info.ID == 1046 || Info.ID == 1047 || Info.ID == 1115)
                            SkillExp += 150;
                        else if (Info.ID == 1120)
                            SkillExp += 1000;
                    }
                    Dictionary<Mob, uint> TempHash = new Dictionary<Mob, uint>();
                    foreach (KeyValuePair<Mob, uint> DE in MobTargets)
                    {
                        Mob M = DE.Key;
                        uint Damage = (uint)DE.Value;
                        if (Info.Damageing != DamageType.HealHP)
                        {
                            if (Info.Damageing == DamageType.Ranged)
                                Exp += M.TakeAttack(User, ref Damage, AttackType.Ranged, true);
                            else if (Info.Damageing == DamageType.Melee)
                                Exp += M.TakeAttack(User, ref Damage, AttackType.Melee, true);
                            else
                            {
                                Exp += M.TakeAttack(User, ref Damage, AttackType.Magic, true);
                                if (Info.ID == 1000 && User.Loc.Map != 1080 && User.Loc.Map != 1017)
                                    if ((User.Equips.LeftHand.Effect == Item.RebornEffect.Poison && User.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(5))
                                    {
                                        Exp += M.TakeAttack(User, ref Damage, AttackType.Melee, true, true);
                                        //Console.WriteLine("Poisoned");
                                    }
                                    else if ((User.Equips.LeftHand.Effect == Item.RebornEffect.Poison || User.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(15))
                                    {
                                        Exp += M.TakeAttack(User, ref Damage, AttackType.Melee, true, true);
                                    }
                            }
                            if (Info.ID == 5010 || Info.ID == 5050 || Info.ID == 1250 || Info.ID == 5020 || Info.ID == 1260 || Info.ID == 1290 || Info.ID == 1300 || Info.ID == 7020 || Info.ID == 7040)
                                SkillExp += 100;
                            else if (Info.ID == 1045 || Info.ID == 1046 || Info.ID == 1047 || Info.ID == 1115)
                                SkillExp += 150;
                            else if (Info.ID == 1120)
                                SkillExp += 1000;
                        }
                        else
                        {
                            Exp += Damage;
                            if (!World.LowRatedServer)
                            {
                                if (M.MobID == 150)
                                    Exp *= 4;
                            }
                            else
                            {
                                if (M.MobID == 150)
                                    Exp *= 2;
                            }
                            if (!World.LowRatedServer)
                            {
                                if (M.MobID == 151)
                                    Exp *= 4;
                            }
                            else
                            {
                                if (M.MobID == 151)
                                    Exp *= 2;
                            }
                            /*  M.CurrentHP += Damage;
                              if (M.CurrentHP > M.MaxHP) M.CurrentHP = M.MaxHP;*/
                        }
                        TempHash.Add(M, Damage);
                    }
                    MobTargets = TempHash;
                    foreach (KeyValuePair<Companion, uint> DE in CompTargets)
                    {
                        Companion M = (Companion)DE.Key;
                        uint Damage = (uint)DE.Value;
                        if (Info.Damageing != DamageType.HealHP)
                        {
                            if (Info.Damageing == DamageType.Ranged)
                                M.TakeAttack(User, ref Damage, AttackType.Ranged, true);
                            else if (Info.Damageing == DamageType.Melee)
                                M.TakeAttack(User, ref Damage, AttackType.Melee, true);
                            else
                                M.TakeAttack(User, ref Damage, AttackType.Magic, true);
                            if (Info.ID == 5010 || Info.ID == 5050 || Info.ID == 1250 || Info.ID == 5020 || Info.ID == 1260 || Info.ID == 1290 || Info.ID == 1300 || Info.ID == 7020 || Info.ID == 7040)
                                SkillExp += 100;
                            else if (Info.ID == 1045 || Info.ID == 1046 || Info.ID == 1047 || Info.ID == 1115)
                                SkillExp += 150;
                            else if (Info.ID == 1120)
                                SkillExp += 1000;
                        }
                    }
                    foreach (KeyValuePair<NPC, uint> DE in NPCTargets)
                    {
                        NPC N = (NPC)DE.Key;
                        uint Damage = (uint)DE.Value;
                        if (Info.Damageing != DamageType.HealHP)
                        {
                            if (N.Flags == 21 || N.Flags == 22)
                            {
                                if (Info.Damageing == DamageType.Ranged)
                                    Exp += N.TakeAttack(User, Damage, AttackType.Ranged, true);
                                else if (Info.Damageing == DamageType.Melee)
                                    Exp += N.TakeAttack(User, Damage, AttackType.Melee, true);
                                else
                                    Exp += N.TakeAttack(User, Damage, AttackType.Magic, true);
                                if (Info.ID == 5010 || Info.ID == 5050 || Info.ID == 1250 || Info.ID == 5020 || Info.ID == 1260 || Info.ID == 1290 || Info.ID == 1300 || Info.ID == 7020 || Info.ID == 7040)
                                    SkillExp += 100;
                                else if (Info.ID == 1045 || Info.ID == 1046 || Info.ID == 1047 || Info.ID == 1115)
                                    SkillExp += 150;
                                else if (Info.ID == 1120)
                                    SkillExp += 1000;
                            }
                        }
                        else
                        {
                            if (N.Flags == 21 || N.Flags == 22)
                            {
                                /*   N.CurHP += Damage;
                                   if (N.CurHP > N.MaxHP) N.CurHP = N.MaxHP; */
                                Exp += (uint)(Damage * 1.5);
                            }
                        }
                    }

                    Character CC = null;
                    foreach (KeyValuePair<Character, uint> DE in PlayerTargets)
                    {
                        Character C = (Character)DE.Key;
                        uint Damage = (uint)DE.Value;
                        if (Info.ID != 1051 && C.Loc.Map != 1039 &&C.Loc.Map != 1004)
                        {
                            if (Info.Damageing != DamageType.HealHP && Info.Damageing != DamageType.HealMP)
                            {
                                if (Info.ExtraEff == ExtraEffect.None || Info.ExtraEff == ExtraEffect.RemoveFly)
                                {
                                    if (C.ProtectTime.AddMilliseconds(0) < DateTime.Now || C.CancelProtectTime)
                                        if ((C.Level > 6 && User.Level > 6 && !MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30)) || (C.Level <= 6 && (C.Loc.Map != 1002 && C.Loc.Map != 1011 &&  C.Loc.Map != 1004 &&  C.Loc.Map != 1020 && C.Loc.Map != 1000 && C.Loc.Map != 1015 && C.Loc.Map != 1009)))
                                        {
                                            if (Damage == 0)
                                                Damage = 1;
                                            if (Info.Damageing == DamageType.Ranged)
                                                C.TakeAttack(User, ref Damage, AttackType.Ranged, true);
                                            else if (Info.Damageing == DamageType.Melee)
                                            {
                                                C.TakeAttack(User, ref Damage, AttackType.Melee, true);
                                            }
                                            else
                                            {

                                                if ((Info.ID == 1000 || Info.ID == 1320) && !World.NoPKMaps.Contains(User.Loc.Map) && User.Loc.Map != 700 && User.Loc.Map != 1080)
                                                {
                                                    if (((User.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison) && ((MyMath.ChanceSuccess(8.5) && Info.ID == 1000) || (MyMath.ChanceSuccess(30) && Info.ID == 1320))))
                                                    {
                                                        World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "nomove").Get);
                                                        uint Damage2 = User.PrepareAttack((byte)AttackType.Melee, false);
                                                        C.TakeAttack(User, ref Damage2, AttackType.Melee, false, true);
                                                        Poison.PoisonCharacter(C.EntityID, User.EntityID);
                                                    }
                                                    else if (((User.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && ((MyMath.ChanceSuccess(8.5) && Info.ID == 1000) || (MyMath.ChanceSuccess(30) && Info.ID == 1320))))
                                                    {
                                                        World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "nomove").Get);
                                                        uint Damage2 = User.PrepareAttack((byte)AttackType.Melee, false);
                                                        C.TakeAttack(User, ref Damage2, AttackType.Melee, false, true);
                                                        Poison.PoisonCharacter(C.EntityID, User.EntityID);
                                                    }
                                                    else
                                                        C.TakeAttack(User, ref Damage, AttackType.Magic, true);
                                                }
                                                else
                                                    C.TakeAttack(User, ref Damage, AttackType.Magic, true);

                                            }
                                        }
                                        else
                                        {
                                            User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 1 characters!");
                                        }

                                }
                            }
                            else
                            {
                                Exp += (uint)(Damage * 1.5);
                            }
                        }
                        switch (Info.ExtraEff)
                        {
                            case ExtraEffect.BlessPray:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Pray;
                                    C.AddBuff(B);
                                    C.Prayer = true;
                                    C.PrayDT = DateTime.Now;
                                    C.GettingLuckyTime = true;
                                    break;
                                }
                            case ExtraEffect.UnMount:
                                {
                                    if (C.Equips.Steed.Plus < User.Equips.Steed.Plus)
                                    {
                                        C.StatEff.Remove(StatusEffectEn.Ride);
                                    }
                                    break;
                                }
                            case ExtraEffect.Scapegoat:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Normal;
                                    C.AddBuff(B);
                                    break;
                                }
                            case ExtraEffect.NoPots:
                                {
                                    C.UnableToUseDrugsFor = Info.EffectLasts;
                                    C.UnableToUseDrugs = DateTime.Now;
                                    break;
                                }
                            /* case ExtraEffect.Ride:
                                 {
                                     if (!User.StatEff.Contains(StatusEffectEn.Ride))
                                         User.StatEff.Add(StatusEffectEn.Ride);
                                     else
                                         User.StatEff.Remove(StatusEffectEn.Ride);
                                   //  User.Vigor = User.MaxVigor;
                                     break;
                                 }*/
                            case ExtraEffect.Summon:
                                {
                                    if (User.MyCompanion != null) { User.MyCompanion.Dissappear(); }
                                    User.MyCompanion = new Game.Companion(User, Info.Damage);
                                    break;
                                }
                            case ExtraEffect.RemoveFly:
                                {
                                    Buff B = C.BuffOf(ExtraEffect.Fly);
                                    if (B.Eff == ExtraEffect.Fly)
                                        if (!C.BDelete.ContainsKey(B))
                                            C.BDelete.TryAdd(B, B.Lasts);
                                    break;
                                }
                            case ExtraEffect.Transform:
                                {
                                    //  if (C.Loc.Map != 1039)
                                    //  {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Transform = Info.Damage;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Normal;
                                    C.AddBuff(B);
                                    // }
                                    break;
                                }

                            case ExtraEffect.Fly:
                                {
                                    if (!C.StatEff.Contains(StatusEffectEn.Shield) && C.Loc.Map != 1051)
                                    {
                                        Buff B = new Buff();
                                        B.Eff = Info.ExtraEff;
                                        B.Lasts = Info.EffectLasts;
                                        B.Value = Info.EffectValue;
                                        B.Started = DateTime.Now;
                                        B.StEff = StatusEffectEn.Fly;
                                        C.AddBuff(B);
                                        C.Flying = true;
                                    }
                                    break;
                                }
                            case ExtraEffect.Revive:
                                {
                                    //  C.ProtectTime = DateTime.Now;
                                    // C.CancelProtectTime = false;
                                    C.Action = 100;
                                    C.Stamina = 100;
                                    C.Ghost = false;
                                    C.BlueName = false;
                                    C.CurHP = (ushort)C.MaxHP;
                                    if (C.MaxMP > 1)
                                        C.CurMP = (ushort)C.MaxMP;
                                    C.Alive = true;
                                    C.StatEff.Remove(StatusEffectEn.Dead);
                                    C.StatEff.Remove(StatusEffectEn.BlueName);
                                    // C.StatEff = new StatusEffect(C);
                                    C.PKPoints = C.PKPoints;
                                    C.XPKO = 0;
                                    C.Body = C.Body;
                                    C.Hair = C.Hair;
                                    C.Equips.Send(C.MyClient, false);
                                    World.Spawn(C, false);
                                    break;
                                }
                            case ExtraEffect.FatalStrike:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.FatalStrike;
                                    C.AddBuff(B);
                                    break;
                                }
                            case ExtraEffect.ShurikenVortex:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.ShurikenVortex;
                                    C.AddBuff(B);
                                    C.VortexOn = true;
                                    C.LastVortexAttk = DateTime.Now;
                                    break;
                                }
                            case ExtraEffect.Stigma:
                                {


                                    Buff S = C.BuffOf(ExtraEffect.Invisibility);
                                    if (S.Eff == ExtraEffect.Invisibility)
                                        if (!C.BDelete.ContainsKey(S))
                                            C.BDelete.TryAdd(S, S.Lasts);
                                    /* 
                                      List<Buff> Bufffs = C.Buffs;
                                      foreach (Buff S in Bufffs)
                                     {
                                         if (S.Eff == SkillsClass.ExtraEffect.Invisibility)
                                             if (!C.BDelete.Contains(S))
                                             C.BDelete.Add(S);
                                     }*/
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Stigma;
                                    C.AddBuff(B);


                                    break;
                                }
                            case ExtraEffect.IceBlock:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.IceBlock;
                                    C.AddBuff(B);

                                    break;
                                }
                            case ExtraEffect.MagicShield:
                                {
                                    if (C.BuffOf(ExtraEffect.MagicShield).Value == 2)
                                        return;
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Shield;
                                    C.AddBuff(B);

                                    break;
                                }
                            case ExtraEffect.Invisibility:
                                {
                                    Buff S = C.BuffOf(ExtraEffect.Stigma);
                                    if (S.Eff == ExtraEffect.Stigma)
                                        if (!C.BDelete.ContainsKey(S))
                                            C.BDelete.TryAdd(S, S.Lasts);
                                    /*  List<Buff> Bufffs = C.Buffs;
                                      foreach (Buff S in Bufffs)
                                      {
                                          if (S.Eff == SkillsClass.ExtraEffect.Stigma)
                                              if (!C.BDelete.Contains(S))
                                              C.BDelete.Add(S);
                                      }*/
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Invisible;
                                    C.AddBuff(B);

                                    break;
                                }
                            case ExtraEffect.Accuracy:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Accuracy;
                                    C.AddBuff(B);

                                    break;
                                }
                            case ExtraEffect.Cyclone:
                                {

                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Cyclone;
                                    C.TimeBuff = B.Lasts;
                                    C.AddBuff(B);
                                    Buff S = C.BuffOf(ExtraEffect.Superman);
                                    if (S.Eff == ExtraEffect.Superman)
                                        if (!C.BDelete.ContainsKey(S))
                                            C.BDelete.TryAdd(S, S.Lasts);
                                    /* foreach (Buff S in C.Buffs)
                                     {
                                         if (S.Eff == SkillsClass.ExtraEffect.Superman)
                                             if (!C.BDelete.Contains(S))
                                                 C.BDelete.Add(S);
                                     }*/
                                    break;
                                }
                            case ExtraEffect.Superman:
                                {

                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.SuperMan;
                                    C.TimeBuff = B.Lasts;

                                    C.AddBuff(B);
                                    Buff S = C.BuffOf(ExtraEffect.Cyclone);
                                    if (S.Eff == ExtraEffect.Cyclone)
                                        if (!C.BDelete.ContainsKey(S))
                                            C.BDelete.TryAdd(S, S.Lasts);
                                    /* foreach (Buff S in C.Buffs)
                                     {
                                         if (S.Eff == SkillsClass.ExtraEffect.Cyclone)
                                             if (!C.BDelete.Contains(S))
                                                 C.BDelete.Add(S);
                                     }*/
                                    break;
                                }
                            case ExtraEffect.Dodge:
                                {
                                    Buff B = new Buff();
                                    B.Eff = Info.ExtraEff;
                                    B.Lasts = Info.EffectLasts;
                                    B.Value = Info.EffectValue;
                                    B.Started = DateTime.Now;
                                    B.StEff = StatusEffectEn.Dodge;
                                    C.AddBuff(B);
                                    break;
                                }
                            case ExtraEffect.Roar:
                                {
                                    C.XPKO += 20;
                                    break;
                                }
                            case ExtraEffect.Intensify:
                                {
                                    C.Intensify.Activated = DateTime.Now;
                                    C.Intensify.Active = true;
                                    C.Intensify.Level = Info.Level;
                                    C.Intensify.X = C.Loc.X;
                                    C.Intensify.Y = C.Loc.Y;
                                    break;
                                }
                            case ExtraEffect.FlashStep:
                                {

                                    PacketHandling.WalkRun.Handle(C.MyClient, new byte[0], (byte)Damage);
                                    World.Action(C, Packets.GeneralData(C.EntityID, 0, C.Loc.X, C.Loc.Y, 0x9c).Get);
                                    uint dmg = GetDamage(C);
                                    C.TakeAttack(User, ref dmg, AttackType.Melee, true);
                                    CC = C;

                                    // Damage = GetDamage(C);

                                    // PlayerTargets[C] = Damage;
                                    //C.Shift(User.Direction);
                                    break;
                                }
                        }
                    }
                    if (CC != null)
                        PlayerTargets[CC] = GetDamage(CC);
                    //foreach (KeyValuePair<AI, uint> DE in AITargets)
                    //{
                    //    AI C = (AI)DE.Key;
                    //    uint Damage = (uint)DE.Value;
                    //    if (Info.ID != 1051 && C.Loc.Map != 1039)
                    //    {
                    //        if (Info.Damageing != DamageType.HealHP && Info.Damageing != DamageType.HealMP)
                    //        {
                    //            if (Info.ExtraEff == ExtraEffect.None || Info.ExtraEff == ExtraEffect.RemoveFly)
                    //            {
                    //                    if (!MyMath.InBox(565, 794, C.Loc.X, C.Loc.Y, 30))
                    //                    {
                    //                        if (Damage == 0)
                    //                            Damage = 1;
                    //                        if (Info.Damageing == DamageType.Ranged)
                    //                            C.TakeAttack(User, ref Damage, AttackType.Ranged, true);
                    //                        else if (Info.Damageing == DamageType.Melee)
                    //                        {
                    //                            C.TakeAttack(User, ref Damage, AttackType.Melee, true);
                    //                        }
                    //                        else
                    //                        {

                    //                            if ((Info.ID == 1000 || Info.ID == 1320) && !World.NoPKMaps.Contains(C.Loc.Map) && C.Loc.Map != 700)
                    //                            {
                    //                                if (((User.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison && User.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) &&
                    //                                    ((MyMath.ChanceSuccess(7.5) && Info.ID == 1000) || (MyMath.ChanceSuccess(20) && Info.ID == 1320))) && User.Loc.Map != 1080)
                    //                                {
                    //                                    World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "nomove").Get);
                    //                                    uint Damage2 = User.PrepareAttack((byte)AttackType.Melee, false);
                    //                                    C.TakeAttack(User, ref Damage2, AttackType.Melee, false, true);
                    //                                }
                    //                                else if (((User.Equips.LeftHand.Effect == Item.RebornEffect.Poison && ((MyMath.ChanceSuccess(5) && Info.ID == 1000) ||
                    //                                    (MyMath.ChanceSuccess(15) && Info.ID == 1320))) || (User.Equips.RightHand.Effect == Item.RebornEffect.Poison &&
                    //                                    ((MyMath.ChanceSuccess(10) && Info.ID == 1000) || (MyMath.ChanceSuccess(30) && Info.ID == 1320)))) && User.Loc.Map != 1080)
                    //                                {
                    //                                    World.Action(C, Packets.StringPacket(C.EntityID, StringType.Effect, "nomove").Get);
                    //                                    uint Damage2 = User.PrepareAttack((byte)AttackType.Melee, false);
                    //                                    C.TakeAttack(User, ref Damage2, AttackType.Melee, false, true);
                    //                                }
                    //                                else
                    //                                    C.TakeAttack(User, ref Damage, AttackType.Magic, true);
                    //                            }
                    //                            else
                    //                                C.TakeAttack(User, ref Damage, AttackType.Magic, true);

                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        User.MyClient.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 1 characters!");
                    //                    }

                    //            }
                    //        }
                    //        else
                    //        {
                    //            Exp += (uint)(Damage * 1.5);
                    //        }
                    //    }
                    //}
                    if (Info.ID == 5010 || Info.ID == 5050 || Info.ID == 1250 || Info.ID == 5020 || Info.ID == 1260 || Info.ID == 1290 || Info.ID == 1300 || Info.ID == 7020 || Info.ID == 7040 || Info.ID == 1120 || Info.ID == 1045 || Info.ID == 1046 || Info.ID == 1047 || Info.ID == 1115)
                    {
                        //SkillExp = SkillExp;
                    }
                    else if (Info.ID == 4000 || Info.ID == 1095 || Info.ID == 7010 || Info.ID == 1085 || Info.ID == 1090 || Info.ID == 1075 || Info.ID == 1320 || Info.ID == 1270 || Info.ID == 1280 || Info.ID == 1350 || Info.ID == 1360 || Info.ID == 3080)
                        SkillExp = 1;
                    else if (Info.ID == 5030 || Info.ID == 9000)
                        SkillExp = 10;
                    else SkillExp = Exp;


                    if (Game.World.TestXP)
                        SkillExp = Game.World.TestXPP;
                    if (User.Loc.Map != 1039)
                    {
                        if (Info.ID == 3090)
                            if (Exp > 40)
                                SkillExp /= 40;
                        uint ExpMob = Exp;
                        uint ExpProf = SkillExp;
                        if (User.Loc.Map == 1004)
                            if (MobTargets.Count > 0)
                            {
                                ExpMob *= 2;
                                ExpProf *= 2;
                            }
                        uint Ball = Exp;
                        if (User.Loc.Map == 1017)
                            if (MobTargets.Count > 0)
                            {
                                Ball *= 1;
                                ExpProf *= 1;
                            }
                        // ExpProf += 1000;
                        // 
                        if (Info.ExtraEff == ExtraEffect.None && Info.Damageing != DamageType.HealHP && (MobTargets.Count > 0 || MiscTargets.Count > 0))
                        {
                            if (MobTargets.Count > 0)
                                User.IncreaseExp(ExpMob, false, false, User);

                            User.AddSkillExp(Info.ID, ExpProf);
                        }
                        else if (Info.ExtraEff != ExtraEffect.None || Info.Damageing == DamageType.HealHP || Info.Damageing == DamageType.HealMP)
                            User.AddSkillExp(Info.ID, ExpProf);

                    }
                    else if (User.AtkMem.LastExpInTG.AddSeconds(10) < DateTime.Now || (Info.ExtraEff == ExtraEffect.None && Info.Damageing != DamageType.HealHP))
                    {
                        User.AtkMem.LastExpInTG = DateTime.Now;
                        if (Info.ExtraEff == ExtraEffect.None && Info.Damageing != DamageType.HealHP && NPCTargets.Count > 0)
                        {
                            if (SkillExp >= 10)
                                SkillExp = (uint)(SkillExp / 10);
                            if (Info.ID == 3090)
                                if (SkillExp >= 2)
                                    SkillExp /= 2;
                            if (Exp >= 10)
                                Exp = (uint)(Exp / 10);
                            // Exp += 200;
                            User.IncreaseExp(Exp, false, false);
                            User.AddSkillExp(Info.ID, SkillExp);
                        }
                        else if (Info.ExtraEff != ExtraEffect.None || Info.Damageing == DamageType.HealHP)
                        {
                            if (SkillExp >= 10)
                                SkillExp = (uint)(SkillExp / 10);
                            User.AddSkillExp(Info.ID, SkillExp);
                        }

                    }
                    if (MobTargets.Count + PlayerTargets.Count + CompTargets.Count + MiscTargets.Count < 82)
                        Game.World.Action(User, Packets.SkillUse(this).Get);
                    else
                    {
                        Dictionary<Mob, uint> Mobs1 = new Dictionary<Mob, uint>();
                        Dictionary<Mob, uint> Mobs2 = new Dictionary<Mob, uint>();
                        Dictionary<Character, uint> Players1 = new Dictionary<Character, uint>();
                        Dictionary<Character, uint> Players2 = new Dictionary<Character, uint>();
                        foreach (KeyValuePair<Mob, uint> DE in MobTargets)
                        {
                            if (Mobs1.Count + CompTargets.Count < 81)
                            {
                                Mob M = DE.Key;
                                uint Dmg = DE.Value;
                                Mobs1.Add(M, Dmg);
                            }
                            else if (Mobs2.Count + CompTargets.Count < 81)
                            {
                                Mob M = DE.Key;
                                uint Dmg = (uint)DE.Value;
                                Mobs2.Add(M, Dmg);
                            }
                        }
                        foreach (KeyValuePair<Character, uint> DE in PlayerTargets)
                        {
                            if (Players1.Count + Mobs1.Count + CompTargets.Count < 82)
                            {
                                Character C = (Character)DE.Key;
                                uint Dmg = (uint)DE.Value;

                                Players1.Add(C, Dmg);
                            }
                            else if (Players2.Count + Mobs2.Count + CompTargets.Count < 82)
                            {
                                Character C = (Character)DE.Key;
                                uint Dmg = (uint)DE.Value;
                                Players2.Add(C, Dmg);
                            }
                        }
                        PlayerTargets = Players1;
                        MobTargets = Mobs1;
                        Game.World.Action(User, Packets.SkillUse(this).Get);
                        PlayerTargets = Players2;
                        MobTargets = Mobs2;
                        Game.World.Action(User, Packets.SkillUse(this).Get);
                    }
                }
                catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            }
        }
        public static Dictionary<string, SkillInfo> SkillInfos = new Dictionary<string, SkillInfo>();
        public static Dictionary<ushort, ushort> WepSkillIDs = new Dictionary<ushort, ushort>();

        public static void Load()
        {
            if (File.Exists(@"C:\OldCODB\Skills.dat"))
            {
                byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\Skills.dat");
                MemoryStream ms = new MemoryStream(buffer);
                BinaryReader BR = new BinaryReader(ms);
                int SkillCount = BR.ReadInt32();
                for (int i = 0; i < SkillCount; i++)
                {
                    SkillInfo S = new SkillInfo();
                    S.LoadThis(BR);
                    if (S.ID == 8001)
                        continue;
                    try
                    {
                        SkillInfos.Add(S.ID + " " + S.Level, S);
                    }
                    catch
                    {

                    }
                    //Skills[i] = BR;
                }
                BR.Close();
                ms.Close();
            }
            WepSkillIDs.Add((ushort)480, (ushort)7020);
            WepSkillIDs.Add((ushort)420, (ushort)5030);
            WepSkillIDs.Add((ushort)421, (ushort)5030);
            WepSkillIDs.Add((ushort)510, (ushort)1250);
            WepSkillIDs.Add((ushort)530, (ushort)5050);
            WepSkillIDs.Add((ushort)561, (ushort)5010);
            WepSkillIDs.Add((ushort)560, (ushort)1260);
            WepSkillIDs.Add((ushort)721, (ushort)1290);
            WepSkillIDs.Add((ushort)460, (ushort)5040);
            WepSkillIDs.Add((ushort)540, (ushort)1300);
            WepSkillIDs.Add((ushort)430, (ushort)7000);
            WepSkillIDs.Add((ushort)450, (ushort)7010);
            WepSkillIDs.Add((ushort)481, (ushort)7030);
            WepSkillIDs.Add((ushort)440, (ushort)7040);
            WepSkillIDs.Add((ushort)580, (ushort)5020);

            if (File.Exists(@"C:\OldCODB\MagicType.txt"))
            {
                string[] Lines = File.ReadAllLines(@"C:\OldCODB\MagicType.txt");

                foreach (string Line in Lines)
                {
                    string[] Info = Line.Split(' ');

                    SkillsClass.SkillInfo S = new SkillInfo();
                    S.ID = ushort.Parse(Info[0]);
                    if (((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade1 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade2 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade3 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade4
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade5 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade6 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade7 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade8
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade9 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade10 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade11 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade12
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade13 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade14 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade15 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade16
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade17 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade18 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade19 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.FastBlade20
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword1 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword2 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword3 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword4
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword5 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword6 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword7 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword8
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword9 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword10 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword11 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword12
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword13 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword14 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword15 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword16
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword17 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword18 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword19 || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ScentSword20
                        || ((Extra.SkillIDs)S.ID) == Extra.SkillIDs.ViperFang || S.ID == 8001)
                    {
                        S.Level = byte.Parse(Info[7]);
                        S.ActivationChance = byte.Parse(Info[11]);
                        S.Damageing = DamageType.Magic;
                        S.Targetting = TargetType.Single;
                        S.ExtraEff = ExtraEffect.None;

                        if (Info[1] == "5")
                        {
                            if (S.ID == 1165 || S.ID == 1125 || S.ID == 5001 || S.ID == 1010 || S.ID == 8030)
                                S.Targetting = TargetType.FromSingle;
                            else
                                S.Targetting = TargetType.Range;
                        }
                        else if (Info[1] == "30")
                        {
                            S.ExtraEff = ExtraEffect.UnMount;
                            S.Targetting = TargetType.Single;
                        }
                        else if (Info[1] == "31")
                        {
                            S.ExtraEff = ExtraEffect.UnMount;
                            S.Targetting = TargetType.Range;
                        }
                        else if (Info[1] == "32")
                        {
                            S.ExtraEff = ExtraEffect.Ride;
                            S.Targetting = TargetType.Single;
                        }
                        else if (Info[1] == "19")
                            S.ExtraEff = ExtraEffect.Transform;
                        else if (Info[1] == "23")
                            S.ExtraEff = ExtraEffect.Summon;
                        else if (Info[1] == "4" || Info[1] == "14")
                            S.Targetting = TargetType.Sector;
                        else if (Info[1] == "7")
                            S.ExtraEff = ExtraEffect.Revive;
                        else if (Info[1] == "16")
                        {
                            S.Targetting = TargetType.Single;
                            S.Damageing = DamageType.Melee;
                        }
                        ushort dmg = ushort.Parse(Info[33]);
                        if (dmg >= 400 && dmg <= 499)
                            S.Damageing = DamageType.Melee;
                        else if (dmg >= 800 && dmg <= 899)
                            S.Damageing = DamageType.Ranged;
                        else if (dmg >= 900 && dmg <= 999)
                            S.Damageing = DamageType.Magic;

                        if (int.Parse(Info[9]) > 0)
                        {
                            if (int.Parse(Info[9]) >= 30000)
                                S.EffectValue = (float)(int.Parse(Info[9]) - 30000) / 100;
                            else
                            {
                                S.EffectValue = 1;
                                S.Damage = uint.Parse(Info[9]);
                            }
                        }
                        S.ManaCost = ushort.Parse(Info[8]);
                        S.UpgReqExp = uint.Parse(Info[17]);
                        S.UpgReqLvl = byte.Parse(Info[18]);
                        S.MaxDist = byte.Parse(Info[13]);
                        StatusEffectEn Eff = (StatusEffectEn)ulong.Parse(Info[15]);
                        if (Eff == StatusEffectEn.SuperMan)
                            S.ExtraEff = ExtraEffect.Superman;
                        else if (Eff == StatusEffectEn.Cyclone)
                            S.ExtraEff = ExtraEffect.Cyclone;
                        else if (Eff == StatusEffectEn.Invisible)
                            S.ExtraEff = ExtraEffect.Invisibility;
                        else if (Eff == StatusEffectEn.Accuracy)
                            S.ExtraEff = ExtraEffect.Accuracy;
                        else if (Eff == StatusEffectEn.Fly)
                            S.ExtraEff = ExtraEffect.Fly;
                        else if (Eff == StatusEffectEn.Stigma)
                            S.ExtraEff = ExtraEffect.Stigma;
                        else if (Eff == StatusEffectEn.Shield)
                            S.ExtraEff = ExtraEffect.MagicShield;
                        else if (Eff == StatusEffectEn.FatalStrike)
                            S.ExtraEff = ExtraEffect.FatalStrike;
                        else if (Eff == StatusEffectEn.ShurikenVortex)
                            S.ExtraEff = ExtraEffect.ShurikenVortex;

                        S.EffectLasts = ushort.MaxValue;//ushort.Parse(Info[12]);
                        if (S.ID == 8001)
                            S.SectorSize = (byte)(105 + S.Level * 15);
                        else
                            S.SectorSize = (byte)(90 + S.Level * 5);
                        if (S.ID == 8001 && S.Level == 5)
                            S.SectorSize = 360;
                        if (S.ID == 8001)
                            S.MaxDist += 3;
                        if (S.ID == 1045 || S.ID == 1046 || S.ID == 1047
                            || S.ID == 2001 || S.ID == 2002 || S.ID == 2003 || S.ID == 2004 || S.ID == 2005 || S.ID == 2006 || S.ID == 2007 || S.ID == 2008 || S.ID == 2009 || S.ID == 2010
                            || S.ID == 2011 || S.ID == 2012 || S.ID == 2013 || S.ID == 2014 || S.ID == 2015 || S.ID == 2016 || S.ID == 2017 || S.ID == 2018 || S.ID == 2019 || S.ID == 2020
                            || S.ID == 2101 || S.ID == 2102 || S.ID == 2103 || S.ID == 2104 || S.ID == 2105 || S.ID == 2106 || S.ID == 2107 || S.ID == 2108 || S.ID == 2109 || S.ID == 2110
                            || S.ID == 2111 || S.ID == 2112 || S.ID == 2113 || S.ID == 2114 || S.ID == 2115 || S.ID == 2116 || S.ID == 2117 || S.ID == 2118 || S.ID == 2119 || S.ID == 2120)
                            S.Targetting = TargetType.Linear;
                        S.StaminaCost = byte.Parse(Info[27]);
                        S.ExtraEff = ExtraEffect.None;
                        if (SkillInfos.ContainsKey(S.ID.ToString() + " " + S.Level.ToString()))
                           SkillInfos.Remove(S.ID.ToString() + " " + S.Level.ToString());
                        SkillInfos.Add(S.ID.ToString() + " " + S.Level.ToString(), S);
                    }
                }
            }
        }
        public static void Save()
        {
            return;
            MemoryStream ms = new MemoryStream();
            BinaryWriter BW = new BinaryWriter(ms);
            BW.Write(SkillInfos.Count);
            foreach (SkillInfo S in SkillInfos.Values)
                S.SaveThis(BW);
            byte[] buffer = ms.ToArray();
            BW.Close();
            ms.Close();
            File.WriteAllBytes(@"C:\OldCODB\Skills.dat", buffer);
        }
    }
}
