using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultimate.Events
{
    public class MeteorShower : Events
    {
        private readonly Dictionary<uint, MapEffect> EventEffects = new Dictionary<uint, MapEffect>();
        private int _meteor = 0;
        private int _db = 0;
        private DateTime LastDrop;
        private DateTime LastMove;
        DateTime PKAllowed = DateTime.Now;
        public MeteorShower()
        {
            EventTitle = "Meteor Shower";
            Duration = 5;
            BaseMap = 700;
            NoDamage = true;
            MeleeAllowed = false;
            MagicAllowed = false;
            AllowedSkills = new List<ushort>();
            DialogID = 6;
        }

        public override bool CanStart()
        {
            return PlayerList.Count >= 1;
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.PK);
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            _meteor = 0;
            _db = 0;

            DisplayScore();
            LastDrop = DateTime.Now;
            LastMove = DateTime.Now;
        }

        public override void WaitForWinner()
        {
            if (DateTime.Now >= EndTime && DateTime.Now >= LastDrop || PlayerList.Count == 0)
                Finish();

            if (DateTime.Now >= LastDrop.AddMilliseconds(20000))
            {
                DropEffects();
                LastDrop = DateTime.Now;
            }
            else if (DateTime.Now >= LastDrop.AddMilliseconds(10000))
            {
                foreach (MapEffect I in EventEffects.Values.ToList())
                {
                    I.Dissappear();
                    DropItem(I);
                    EventEffects.Remove(I.UID);
                    DisplayScore();
                }
            }
            else if (DateTime.Now >= LastMove.AddMilliseconds(1500))
            {
                foreach (MapEffect I in EventEffects.Values.ToList())
                {
                    I.Dissappear();
                    MoveEffects(I);
                }
                LastMove = DateTime.Now;
            }
            if (MeleeAllowed && MagicAllowed)
            {
                if (DateTime.Now >= PKAllowed.AddMilliseconds(5000))
                {
                    MeleeAllowed = false;
                    MagicAllowed = false;
                }
            }
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now >= C.DeathHit.AddSeconds(5))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
            foreach (MapEffect I in EventEffects.Values.ToList())
            {
                if (C.Loc.X == I.Loc.X && C.Loc.Y == I.Loc.Y)
                {
                    if (C.Alive)
                    {
                        SkillsClass.SkillUse S = new SkillsClass.SkillUse();
                        S.Info.Damage = 999999;
                        S.Info.ID = 1180;
                        S.Info.Level = 7;

                        C.TakeAttack(S.Info.Damage, S);
                    }
                }
            }
        }

        private void DropEffects()
        {
            for (int x = 0; x < 5; x++)
            {
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                var DI = new Game.MapEffect();
                DI.DropTime = DateTime.Now;
                DI.LastDrop = DateTime.Now;
                DI.Loc = new Game.Location();
                DI.Loc.Map = MapEvent;
                DI.Info = new Game.MEffect();
                DI.Info.ID = 17;

                DI.UID = (uint)Program.Rnd.Next(900000, 999999);
                DI.Info.UID = DI.UID;
                DI.Loc.X = X;
                DI.Loc.Y = Y;

                foreach (MapEffect I in EventEffects.Values)
                {
                    while (I.Loc.X == DI.Loc.X && I.Loc.Y == DI.Loc.Y)
                    {
                        DI.Loc.X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                        DI.Loc.Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                    }
                }
                DI.Drop();
                EventEffects.Add(DI.UID, DI);
            }
        }

        private void MoveEffects(MapEffect DI)
        {
            X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
            Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
            DI.LastDrop = DateTime.Now;
            DI.Loc.Map = MapEvent;
            DI.Info.ID = 17;

            DI.Info.UID = DI.UID;
            DI.Loc.X = X;
            DI.Loc.Y = Y;

            foreach (MapEffect I in EventEffects.Values)
            {
                while (I.Loc.X == DI.Loc.X && I.Loc.Y == DI.Loc.Y)
                {
                    DI.Loc.X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                    DI.Loc.Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                }
            }
            DI.Drop();
            EventEffects[DI.UID] = DI;
        }

        private void DropItem(MapEffect I)
        {
            uint ID = 720027;
            if (MyMath.ChanceSuccess(5))
            {
                ID = 1088000;
                _db++;
                MeleeAllowed = true;
                MagicAllowed = true;
                Broadcast("A DragonBall have dropped ! PK is now allowed for 5 seconds !", BroadCastLoc.Map);
                PKAllowed = DateTime.Now;
            }
            else
                _meteor++;
            DroppedItem droppedItem = new DroppedItem();
            droppedItem.DropTime = DateTime.Now;
            droppedItem.Loc = new Location();
            droppedItem.Loc.Map = I.Loc.Map;
            droppedItem.Info = new Item();
            droppedItem.Info.ID = ID;
            droppedItem.UID = (uint)Program.Rnd.Next(10000000);
            droppedItem.Info.UID = droppedItem.UID;
            droppedItem.Loc.X = I.Loc.X;
            droppedItem.Loc.Y = I.Loc.Y;
            droppedItem.Drop();
        }

        public override void End()
        {
            foreach (MapEffect I in EventEffects.Values)
                I.Dissappear();
            foreach (Character C in PlayerList.Values.ToList())
                C.EventBase?.RemovePlayer(C);

            Broadcast("Meteor Shower Event has come to an end!", BroadCastLoc.World);
            Removeprotection();
            PlayerList.Clear();
            PlayerScores.Clear();
            EventEffects.Clear();
            return;
        }

        private void TeleAfterRev(Character C)
        {
            X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
            Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
            C.Teleport(MapEvent, (ushort)X, (ushort)Y);
        }

        public override void DisplayScore()
        {
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            Broadcast($"Meteors: {_meteor}", BroadCastLoc.Score, 2);
            Broadcast($"DragonBalls: {_db}", BroadCastLoc.Score, 3);
        }

        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            if (Info.ID == 8001)
                return Convert.ToUInt32(C.MaxHP * 0.35);
            else if (Info.ID == 1046 || Info.ID == 1045 || Info.ID == 1047
           || Info.ID == 2001 || Info.ID == 2002 || Info.ID == 2003 || Info.ID == 2004 || Info.ID == 2005 || Info.ID == 2006 || Info.ID == 2007 || Info.ID == 2008 || Info.ID == 2009 || Info.ID == 2010
           || Info.ID == 2011 || Info.ID == 2012 || Info.ID == 2013 || Info.ID == 2014 || Info.ID == 2015 || Info.ID == 2016 || Info.ID == 2017 || Info.ID == 2018 || Info.ID == 2019 || Info.ID == 2020
           || Info.ID == 2101 || Info.ID == 2102 || Info.ID == 2103 || Info.ID == 2104 || Info.ID == 2105 || Info.ID == 2106 || Info.ID == 2107 || Info.ID == 2108 || Info.ID == 2109 || Info.ID == 2110
           || Info.ID == 2111 || Info.ID == 2112 || Info.ID == 2113 || Info.ID == 2114 || Info.ID == 2115 || Info.ID == 2116 || Info.ID == 2117 || Info.ID == 2118 || Info.ID == 2119 || Info.ID == 2120)
                return Convert.ToUInt32(C.MaxHP);
            else if (Info.ID == 1000 || Info.ID == 1165)
                return Convert.ToUInt32(C.MaxHP * 0.35);
            else if (Info.ID == 1001 || Info.ID == 1115)
                return Convert.ToUInt32(C.MaxHP * 0.55);
            else if (Info.ID == 1150 || Info.ID == 1160 || Info.ID == 1180 || Info.ID == 1002)
                return Convert.ToUInt32(C.MaxHP * 0.55);
            else if (Info.ID == 1120)
                return Convert.ToUInt32(C.MaxHP * 0.55);
            else if (Info.ID == 1320)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID == 5001 || Info.ID == 1125 || Info.ID == 1010)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID != 1175 && Info.ID != 1170 && Info.ID != 1005 && Info.ID != 1055 && Info.ID != 1190 && Info.ID != 1195)
                return Convert.ToUInt32(C.MaxHP * 0.55);
            else if (Info.ID == 1190)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID == 1005 || Info.ID == 1055 || Info.ID == 1170 || Info.ID == 1175)
                return Convert.ToUInt32(C.MaxHP * 0.55);
            return Convert.ToUInt32(C.MaxHP * 0.35);
        }

        public override uint GetDamage(Character User, Character C, AttackType AttackType)
        {
            if (AttackType == AttackType.Melee)
                return Convert.ToUInt32(C.MaxHP);
            else if (AttackType == AttackType.Ranged)
                return Convert.ToUInt32(C.MaxHP * 0.35);
            return Convert.ToUInt32(C.MaxHP * 0.35);
        }
    }
}