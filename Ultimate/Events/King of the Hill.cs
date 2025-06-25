using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ultimate.Events
{
    public class KOTH : Events
    {
        private List<uint> Kings = new List<uint>();
        DateTime LastScore;
        public KOTH()
        {
            EventTitle = "Death Square";
            Duration = 10;
            BaseMap = 700;
            //MagicAllowed = false;
            NoDamage = true;
            //AllowedSkills = new List<ushort>() { 1000, 1001, 1002, 1005, 1055, 1075, 1085, 1090,
            //    1095, 1100, 1105, 1120, 1150, 1160, 1165, 1170, 1175, 1180, 1015, 1010, 1020, 1040,
            //    1050, 1125, 1270, 1280, 1320, 1360, 5001, 1045, 1046, 1047, 1190, 1195, 1115, 3050,
            //    3090, 1250, 1260, 1290, 1300, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040 };
            DialogID = 4;

            MagicAllowed = false;
            MeleeAllowed = false;
            AllowedSkills = new System.Collections.Generic.List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.PK);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                TeleAfterRev(c);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            DisplayScore();
            DisplayScores = DateTime.Now;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (PlayerScores.ContainsValue(300))
                Finish();

            if (DateTime.Now >= DisplayScores.AddMilliseconds(2500))
                DisplayScore();

            if (DateTime.Now >= LastScore.AddMilliseconds(1000))
            {
                Kings.Clear();
                LastScore = DateTime.Now;
            }
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            //if (!C.Alive)
            //{
            //    if (DateTime.Now > C.DeathHit.AddMilliseconds(2000))
            //    {
            //        RevivePlayer(C, C.MaxHP);
            //        TeleAfterRev(C);
            //    }
            //}
            /*else*/
            if (!Kings.Contains(C.EntityID) && C.Loc.X >= 47 && C.Loc.X <= 54 && C.Loc.Y >= 47 && C.Loc.Y <= 54)
            {
                Kings.Add(C.EntityID);
                if (PlayerScores.ContainsKey(C.EntityID))
                    if (PlayerScores[C.EntityID] + 5 > 300)
                        PlayerScores[C.EntityID] = 300;
                    else
                        PlayerScores[C.EntityID] += 5;
            }
        }

        public void TeleAfterRev(Character C)
        {
            int RndX = Program.Rnd.Next(0, 2);
            int RndY = Program.Rnd.Next(0, 2);
            int X = 50;
            int Y = 50;
            switch (RndX)
            {
                case 0:
                    X = 50 + Program.Rnd.Next(5, 19);
                    break;
                case 1:
                    X = 50 - Program.Rnd.Next(4, 18);
                    break;
            }
            switch (RndY)
            {
                case 0:
                    Y = 50 - Program.Rnd.Next(4, 18);
                    break;
                case 1:
                    Y = 50 + Program.Rnd.Next(5, 19);
                    break;
            }


            C.Teleport(MapEvent, (ushort)X, (ushort)Y);
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Victim.Loc.X >= 47 && Victim.Loc.X <= 54 && Victim.Loc.Y >= 47 && Victim.Loc.Y <= 54)
            {
                byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(Attacker.Loc.X, Attacker.Loc.Y, Victim.Loc.X, Victim.Loc.Y) / 45 % 8)) - 1 % 8);
                byte Direction = (byte)((int)ToDir % 8);
                if (Direction == 0)//sw
                    Victim.Loc.Y += 6;
                else if (Direction == 2)//nw
                    Victim.Loc.X -= 6;
                else if (Direction == 4)//ne
                    Victim.Loc.Y -= 6;
                else if (Direction == 6)//se
                    Victim.Loc.X += 6;
                else if (Direction == 1)//w
                {
                    Victim.Loc.X -= 6;
                    Victim.Loc.Y += 6;
                }
                else if (Direction == 3)//n
                {
                    Victim.Loc.X -= 6;
                    Victim.Loc.Y -= 6;
                }
                else if (Direction == 5)//e
                {
                    Victim.Loc.X += 6;
                    Victim.Loc.Y -= 6;
                }
                else if (Direction == 7)//s
                {
                    Victim.Loc.X += 6;
                    Victim.Loc.Y += 6;
                }
                World.Action(Victim, Packets.GeneralData(Victim.EntityID, 0, Victim.Loc.X, Victim.Loc.Y, 0x9c).Get);
            }
        }

        public override void End()
        {
            DisplayScore();
            Removeprotection();
            int NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                if (NO == 1)
                {
                    Reward(PlayerList[player.Key]);
                    RemovePlayer(PlayerList[player.Key]);
                    NO++;
                }
                else
                {
                    if (PlayerList.ContainsKey(player.Key))
                    {
                        RemovePlayer(PlayerList[player.Key]);
                        NO++;
                    }
                }
            }
            PlayerList.Clear();
            PlayerScores.Clear();
            return;
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        //public override void Kill(Character player, Character entity)
        //{
        //    if (PlayerScores.ContainsKey(player.EntityID))
        //    {
        //        if (PlayerScores[player.EntityID] + 2 > 500)
        //            PlayerScores[player.EntityID] = 500;
        //        else
        //            PlayerScores[player.EntityID] += 2;
        //    }
        //}

        //public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        //{
        //    if (Info.ID == 8001)
        //        return Convert.ToUInt32(C.MaxHP / 20);
        //    else if (Info.ID == 1046 || Info.ID == 1045 || Info.ID == 1047)
        //        return Convert.ToUInt32(C.MaxHP * 0.4);
        //    else if (Info.ID == 1000 || Info.ID == 1165)
        //        return Convert.ToUInt32(C.MaxHP * 0.1);
        //    else if (Info.ID == 1001 || Info.ID == 1115)
        //        return Convert.ToUInt32(C.MaxHP * 0.15);
        //    else if (Info.ID == 1150 || Info.ID == 1160 || Info.ID == 1180 || Info.ID == 1002)
        //        return Convert.ToUInt32(C.MaxHP * 0.2);
        //    else if (Info.ID == 1120)
        //        return Convert.ToUInt32(C.MaxHP * 0.25);
        //    else if (Info.ID == 1320)
        //        return Convert.ToUInt32(C.MaxHP * 0.6);
        //    else if (Info.ID == 5001 || Info.ID == 1125 || Info.ID == 1010)
        //        return Convert.ToUInt32(C.MaxHP * 0.6);
        //    else if (Info.ID != 1175 && Info.ID != 1170 && Info.ID != 1005 && Info.ID != 1055 && Info.ID != 1190 && Info.ID != 1195)
        //        return Convert.ToUInt32(C.MaxHP * 0.15);
        //    else if (Info.ID == 1190)
        //        return Convert.ToUInt32(C.MaxHP * 0.6);
        //    else if (Info.ID == 1005 || Info.ID == 1055 || Info.ID == 1170 || Info.ID == 1175)
        //        return Convert.ToUInt32(C.MaxHP * 0.15);
        //    return Convert.ToUInt32(C.MaxHP * 0.15);
        //}

        //public override uint GetDamage(Character User, Character C, AttackType AttackType)
        //{
        //    if (AttackType == AttackType.Melee)
        //        return Convert.ToUInt32(C.MaxHP * 0.4);
        //    else if (AttackType == AttackType.Ranged)
        //        return Convert.ToUInt32(C.MaxHP * 0.1);
        //    return Convert.ToUInt32(C.MaxHP * 0.1);
        //}
    }
}