using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Features
{
    public class Vampire_War : PVPEvents
    {
        public static bool VWar = false;
        public DateTime _vampire;
        public Vampire_War()
        {
            EventTitle = "Vampire War";
            Duration = 10;
            MapEvent = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = true;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };
        }
        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                PlayerScores.Add(c.EntityID, 0);
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = 500;
                c.Protection = true;
            }
            VWar = true;
            _vampire = DateTime.UtcNow;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == Events.EventStage.Fighting)
            {
                Game.World.Action(Victim, (Packets.String(Victim.EntityID, 10, "levin")).Get);
                Game.World.Action(Attacker, (Packets.String(Attacker.EntityID, 10, "heal2")).Get);
                if ((Attacker.CurHP + 50) > 500)
                    Attacker.CurHP = 500;
                else
                    Attacker.CurHP += 50;
            }
        }

        public override void WaitForWinner()
        {
            while (true)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                    {
                        C.EventBase?.RemovePlayer(C);
                        Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    }

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                    {
                        C.EventBase?.RemovePlayer(C);
                        Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    }

                    else if (C.CurHP > 500)
                        C.CurHP = 500;
                }

                if (DateTime.UtcNow >= _vampire)
                {
                    _vampire = DateTime.UtcNow.AddMilliseconds(5000);
                    foreach (Character C in PlayerList.Values)
                    {
                        if (C.CurHP > 25)
                            C.CurHP -= 25;
                        else
                        {
                            #region KillPlayer
                            C.AtkMem.Attacking = false;
                            C.AtkMem.Target = 0;

                            C.Alive = false;
                            C.CurHP = 0;
                            C.DeathHit = DateTime.UtcNow;
                            World.Action(C, Packets.AttackPacket(C.EntityID, C.EntityID, C.Loc.X, C.Loc.Y, C.CurHP, (byte)AttackType.Kill).Get);
                            foreach (Buff B in C.Buffs.Keys)
                                C.BDelete.TryAdd(B, B.Lasts);
                            C.BlueName = false;
                            C.PoisonedInfo.Times = 0;
                            C.StatEff.Add(StatusEffectEn.Dead);
                            if (C.MyCompanion != null)
                                C.MyCompanion.Dissappear();
                            #endregion
                        }
                        Game.World.Action(C, (Packets.String(C.EntityID, 10, "poison")).Get);
                    }
                }
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count <= 1)
                    break;

                if (!War)
                    break;
            }
        }

        public override void End()
        {
            VWar = false;
            base.End();
        }
    }
}
