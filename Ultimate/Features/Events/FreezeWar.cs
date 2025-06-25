using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Game;
using NewestCOServer.Features.Events;

namespace NewestCOServer.Features
{
    public class FreezeWar : PVPEvents
    {
        public static bool FW = false;

        public FreezeWar()
        {
            EventTitle = "Freeze War";
            Duration = 10;
            MapEvent = 1506;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = true;
            FFADamage = false;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };
        }

        public override void TeleportPlayersToMap()
        {
            var counter = 0;
            FW = true;
            foreach (Character c in PlayerList.Values)
            {
                PlayerScores.Add(c.EntityID, 0);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.StatEff.Add(StatusEffectEn.IceBlock);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                if (counter % 2 == 0)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(108 + (ushort)Program.Rnd.Next(6)), (ushort)(134 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(183425));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Blue Team!");
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(83 + (ushort)Program.Rnd.Next(6)), (ushort)(26 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(191305));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Red Team!");
                }
                counter++;
            }
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if ((TeamOne.ContainsKey(Attacker.EntityID) && TeamOne.ContainsKey(Victim.EntityID)) || (TeamTwo.ContainsKey(Attacker.EntityID) && TeamTwo.ContainsKey(Victim.EntityID)))
                    Victim.StatEff.Remove(NewestCOServer.Game.StatusEffectEn.IceBlock);
                else
                    Victim.StatEff.Add(StatusEffectEn.IceBlock);
            }
        }

        public override void WaitForWinner()
        {
            uint num1 = (uint)Environment.TickCount;
            foreach (Character C in PlayerList.Values)
                C.StatEff.Remove(StatusEffectEn.IceBlock);
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                    {
                        PlayersToRemove.Add(C.EntityID, C);
                    }
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    C.StatEff.Remove(StatusEffectEn.IceBlock);
                    if (TeamOne.ContainsKey(C.EntityID))
                        TeamOne.Remove(C.EntityID);
                    else if (TeamTwo.ContainsKey(C.EntityID))
                        TeamTwo.Remove(C.EntityID);
                    C.EventBase?.RemovePlayer(C);
                    if (PlayerScores.ContainsKey(C.EntityID))
                        PlayerScores.Remove(C.EntityID);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    RemovedPlayers.Add(C.EntityID, C);
                }
                foreach (Character C in RemovedPlayers.Values)
                {
                    if (PlayersToRemove.ContainsKey(C.EntityID))
                        PlayersToRemove.Remove(C.EntityID);
                }
                if (OneAllFrozen())
                {
                    System.Threading.Thread.Sleep(3000);
                    while (TeamOne.Count > 0)
                    {
                        foreach (Character C in TeamOne.Values)
                        {
                            TeamOne.Remove(C.EntityID);
                            C.StatEff.Remove(StatusEffectEn.IceBlock);
                            C.EventBase?.RemovePlayer(C);
                            break;
                        }
                    }
                }
                else if (TwoAllFrozen())
                {
                    System.Threading.Thread.Sleep(3000);
                    while (TeamTwo.Count > 0)
                    {
                        foreach (Character C in TeamTwo.Values)
                        {
                            TeamTwo.Remove(C.EntityID);
                            C.StatEff.Remove(StatusEffectEn.IceBlock);
                            C.EventBase?.RemovePlayer(C);
                            break;
                        }
                    }
                }

                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (TeamOne.Count == 0 || TeamTwo.Count == 0)
                    break;

                if (!War)
                    break;
            }
        }

        public bool OneAllFrozen()
        {
            try
            {
                if (TeamOne.Count == 0)
                    return true;
                foreach (Character p in TeamOne.Values)
                    if (!p.StatEff.Contains(StatusEffectEn.IceBlock))
                        return false;
                return true;
            }
            catch { return false; }
        }
        public bool TwoAllFrozen()
        {
            try
            {
                if (TeamTwo.Count == 0)
                    return true;
                foreach (Character p in TeamTwo.Values)
                    if (!p.StatEff.Contains(StatusEffectEn.IceBlock))
                        return false;
                return true;
            }
            catch { return false; }
        }

        public override void End()
        {
            if (TeamOne.Count > 0 && TeamTwo.Count > 0)
                World.SendMsgToAll("[EVENT]", "It's a tie! 10 Minutes have passed and neither of the teams won the Freeze War! Better luck next time!", 2011, 0);
            else
            {
                if (TeamOne.Count > 0)
                    World.SendMsgToAll("[EVENT]", "The Blue Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);
                else if (TeamTwo.Count > 0)
                    World.SendMsgToAll("[EVENT]", "The Red Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);

                foreach (var c in PlayerList.Values)
                {
                    c.StatEff.Remove(StatusEffectEn.IceBlock);
                    Reward(c);
                    TeleportOut(c);
                    c.CurHP = c.MaxHP;
                }
            }
            
            Removeprotection();
            FW = false;
            War = false;
            TeamOne.Clear();
            TeamTwo.Clear();
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            Abort();
            return;
        }
        public override void Reward(Character c)
        {
            c.AddItem(1088000);
            World.Action(c, (Packets.String(c.EntityID, 10, "angelwing")).Get);
            if (c.Level < 130)
                c.IncreaseExp(c.ExpBallExp, false, false);
            c.EventBase = null;
        }
    }
}
