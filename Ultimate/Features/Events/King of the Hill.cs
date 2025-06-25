using NewestCOServer.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NewestCOServer.Features
{
    public class KOTH : PVPEvents
    {
        public DateTime LastScore;
        private DateTime DisplayScores;
        public KOTH()
        {
            EventTitle = "King Of The Hill";
            Duration = 10;
            MapEvent = 700;
            NoDamage = false;
            FFADamage = true;
        }
        
        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {

                PlayerScores.Add(c.EntityID, 0);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                TeleAfterRev(c);
                c.CurHP = 20;
                c.Protection = true;
            }
            DisplayScore();
        }

        public override void WaitForWinner()
        {
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        PlayersToRemove.Add(C.EntityID, C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
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
                }
                foreach (Character C in PlayersToRemove.Values)
                {
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

                if (DateTime.UtcNow >= LastScore.AddSeconds(1))
                    KingOfTheHill();

                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count == 1)
                    break;

                if (!War)
                    break;

                if (PlayerScores.ContainsValue(500))
                    break;

                if (DateTime.UtcNow >= DisplayScores.AddMilliseconds(5000))
                    DisplayScore();
            }
        }

        public void KingOfTheHill()
        {
            foreach (Character C in PlayerList.Values)
            {
                if (C.Loc.X >= 47 && C.Loc.X <= 54 && C.Loc.Y >= 47 && C.Loc.Y <= 54)
                {
                    LastScore = DateTime.UtcNow;
                    if (PlayerScores.ContainsKey(C.EntityID))
                        if (PlayerScores[C.EntityID] + 5 > 500)
                            PlayerScores[C.EntityID] = 500;
                        else
                        PlayerScores[C.EntityID] += 5;
                }
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
            switch(RndY)
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

        public override void End()
        {
            DisplayScore();
            int NO = 1;
            while (PlayerList.Count > 1)
            {
                foreach (var player in PlayerScores.OrderByDescending(s => s.Value))
                {
                    if (NO == 1)
                    {
                        Reward(PlayerList[player.Key]);
                        RemovePlayer(PlayerList[player.Key]);
                        if (PlayerScores.ContainsKey(player.Key))
                            PlayerScores.Remove(player.Key);
                        NO++;
                        break;
                    }
                    else if (NO >= 2 && NO <= 5)
                    {
                        World.Action(PlayerList[player.Key], (Packets.String(PlayerList[player.Key].EntityID, 10, "angelwing")).Get);
                        if (PlayerList[player.Key].Level < 130)
                            PlayerList[player.Key].IncreaseExp((PlayerList[player.Key].ExpBallExp * 2) / Convert.ToByte(NO), false, false);
                        RemovePlayer(PlayerList[player.Key]);
                        if (PlayerScores.ContainsKey(player.Key))
                            PlayerScores.Remove(player.Key);
                        NO++;
                        break;
                    }
                    else
                    {
                        if (PlayerList.ContainsKey(player.Key))
                        {
                            RemovePlayer(PlayerList[player.Key]);
                            if (PlayerScores.ContainsKey(player.Key))
                                PlayerScores.Remove(player.Key);
                            NO++;
                            break;
                        }
                        
                    }
                }
            }
            foreach (var c in PlayerList.Values)
            {
                TeleportOut(c);
                c.MyClient.LocalMessage(2108, "");
                c.CurHP = c.MaxHP;
            }
            Removeprotection();
            War = false;
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            Abort();
            return;
        }

        public override void Kill(Character player, Character entity)
        {
            if (PlayerScores.ContainsKey(player.EntityID))
            {
                if (PlayerScores[player.EntityID] + 2 > 500)
                    PlayerScores[player.EntityID] = 500;
                else
                    PlayerScores[player.EntityID] += 2;
            }
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.UtcNow;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            byte Score = 2;
            foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)))
            {
                if (Score == 7)
                    break;
                if (Score == PlayerScores.Count + 2)
                    break;
                Broadcast($"Nº {Score - 1}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                Score++;
            }
        }
    }
}