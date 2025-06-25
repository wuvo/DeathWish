using NewestCOServer.Features.Events;
using NewestCOServer.Game;
using System;

namespace NewestCOServer.Features
{
    public class PTB : PVPEvents
    {
        public static bool PTBomb = false;
        byte PTBC = 0;
        public PTB()
        {
            EventTitle = "Pass the Bomb";
            Duration = 10;
            MapEvent = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FFADamage = false;
            AllowedSkills = new System.Collections.Generic.List<ushort>{ (ushort)1045, (ushort)1046, (ushort)1047 };
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
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            PTBomb = true;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (Attacker.StatEff.Contains(StatusEffectEn.Confused))
                {
                    Broadcast(Attacker.Name + " has passed the bomb to " + Victim.Name + "! Be careful!", BroadCastLoc.Map);
                    PTBC = 0;
                    Attacker.StatEff.Remove(StatusEffectEn.Confused);
                    Victim.StatEff.Add(StatusEffectEn.Confused);
                }
            }
        }

        public override void WaitForWinner()
        {
            Randomize();
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        PlayersToRemove.Add(C.EntityID, C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                        PlayersToRemove.Add(C.EntityID, C);
                }
                foreach (Character C in PlayerList.Values)
                {
                    if (C.StatEff.Contains(StatusEffectEn.Confused))
                    {
                        if (PTBC == 0)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber9")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 1)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber8")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 2)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber7")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 3)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber6")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 4)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber5")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 5)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber4")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 6)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber3")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 7)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber2")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 8)
                        {
                            World.Action(C, (Packets.String(C.EntityID, 10, "downnumber1")).Get);
                            PTBC++;
                        }
                        else if (PTBC == 9)
                        {
                            PTBC = 0;
                            World.Action(C, (Packets.String(C.EntityID, 10, "attach_accept05")).Get);
                            System.Threading.Thread.Sleep(750);
                            C.StatEff.Remove(StatusEffectEn.Confused);
                            PlayersToRemove.Add(C.EntityID, C);
                            Randomize();
                            //break;
                        }
                    }
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    C.EventBase?.RemovePlayer(C);
                    if (C.StatEff.Contains(StatusEffectEn.Confused))
                        Randomize();
                    C.StatEff.Remove(StatusEffectEn.Confused);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    RemovedPlayers.Add(C.EntityID, C);
                }
                foreach (Character C in RemovedPlayers.Values)
                {
                    if (PlayersToRemove.ContainsKey(C.EntityID))
                        PlayersToRemove.Remove(C.EntityID);
                }
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count == 1)
                    break;

                if (!War)
                    break;
                System.Threading.Thread.Sleep(1000);
            }
        }

        public override void End()
        {
            PTBomb = false;
            foreach (Character C in PlayerList.Values)
                C.StatEff.Remove(StatusEffectEn.Confused);
            base.End();
        }

        public void Randomize() 
        {
            if (PlayerList.Count == 1)
                return;
            int Number = Program.Rnd.Next(1, (PlayerList.Count + 1));
            int MyPlace = 1;
            foreach (Character C in PlayerList.Values)
            {
                if (MyPlace == Number)
                {
                    C.StatEff.Add(StatusEffectEn.Confused);
                    Broadcast(C.Name + " has the bomb! Be careful!", BroadCastLoc.Map);
                    break;
                }
                else
                    MyPlace++;
            }
        }
    }
}