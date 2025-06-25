using NewestCOServer.Features;
using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewestCOServer.Features
{
    public class MeteorShower : PVPEvents
    {
        private Dictionary<uint, MapEffect> EventEffects = new Dictionary<uint, MapEffect>();
        private int _meteor = 0;
        private int _db = 0;
        private DateTime LastDrop;
        DateTime PKAllowed = DateTime.UtcNow;
        public MeteorShower()
        {
            EventTitle = "Meteor Shower";
            Duration = 5;
            MapEvent = 700;
            NoDamage = false;
            FFADamage = true;
            MeleeAllowed = false;
            MagicAllowed = false;
            //AllowedSkills = new System.Collections.Generic.List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };
        }

        public override bool CanStart()
        {
            return PlayerList.Count >= 1;
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = 20;
                c.Protection = true;
            }
            _meteor = 0;
            _db = 0;
        }
        
        public override void WaitForWinner()
        {
            DisplayScore();
            LastDrop = DateTime.UtcNow;
            while (true)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        C.EventBase?.RemovePlayer(C);

                    else if (!C.Alive && DateTime.UtcNow >= C.DeathHit.AddSeconds(5))
                    {
                        #region Revive
                        C.Action = (byte)100;
                        C.Stamina = (byte)100;
                        C.Ghost = false;
                        C.BlueName = false;
                        C.CurHP = 20;
                        if ((int)C.MaxMP > 1)
                            C.CurMP = C.MaxMP;
                        C.Alive = true;
                        C.StatEff.Remove(StatusEffectEn.Dead);
                        C.StatEff.Remove(StatusEffectEn.BlueName);
                        C.Body = C.Body;
                        C.Hair = C.Hair;
                        C.XPKO = (byte)0;
                        C.ProtectTime.AddSeconds(3);
                        #endregion
                        TeleAfterRev(C);
                    }
                    else if (C.CurHP > 20)
                        C.CurHP = 20;

                    foreach (MapEffect I in EventEffects.Values)
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

                if (!War) break;

                else if (DateTime.UtcNow >= EndTime)
                {
                    if (DateTime.UtcNow >= LastDrop)
                        break;
                }
                else
                {
                    foreach (MapEffect I in EventEffects.Values.ToList())
                    {
                        if (DateTime.UtcNow >= I.DropTime.AddMilliseconds(10000))
                        {
                            I.Dissappear();
                            DropItem(I);
                            EventEffects.Remove(I.UID);
                            DisplayScore();
                        }
                        else if (DateTime.UtcNow >= I.LastDrop.AddMilliseconds(2000))
                        {
                            I.Dissappear();
                            MoveEffects(I);
                        }
                    }
                    if (DateTime.UtcNow >= LastDrop)
                    {
                        DropEffects();
                        LastDrop = DateTime.UtcNow.AddMilliseconds(20000);
                    }
                    if (MeleeAllowed && MagicAllowed)
                    {
                        if (DateTime.UtcNow >= PKAllowed.AddMilliseconds(5000))
                        {
                            MeleeAllowed = false;
                            MagicAllowed = false;
                        }
                    }
                }
            }
        }
        
        private void DropEffects()
        {
            Game.MapEffect DI;
            for (int x = 0; x < 5; x++)
            {
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                DI = new Game.MapEffect();
                DI.DropTime = DateTime.UtcNow;
                DI.LastDrop = DateTime.UtcNow;
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
            DI.LastDrop = DateTime.UtcNow;
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
            uint ID = 1088001;
            if (MyMath.ChanceSuccess(5))
            {
                ID = 1088000;
                _db++;
                MeleeAllowed = true;
                MagicAllowed = true;
                Broadcast("A DragonBall have dropped ! PK is now allowed for 5 seconds !", BroadCastLoc.Map);
                PKAllowed = DateTime.UtcNow;
            }
            else
                _meteor++;
            DroppedItem droppedItem = new DroppedItem();
            droppedItem.DropTime = DateTime.UtcNow;
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
            War = false;
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            EventEffects.Clear();
            Abort();
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
    }
}