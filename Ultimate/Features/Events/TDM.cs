using NewestCOServer.Features.Events;
using NewestCOServer.Game;
using System;
using System.Collections.Generic;

namespace NewestCOServer.Features
{
    public class TDM : PVPEvents
    {
        public DateTime DisplayScores;
        public int ScoreOne, ScoreTwo = 0;
        public TDM()
        {
            EventTitle = "Team Deathmatch";
            Duration = 10;
            NoDamage = false;
            FFADamage = false;
            MapEvent = 1505;
            FriendlyFire = false;
        }
        public override bool CanStart()
        {
            return PlayerList.Count >= 2;
        }
        
        public override void TeleportPlayersToMap()
        {
            var counter = 0;
            foreach (Character c in PlayerList.Values)
            {
                PlayerScores.Add(c.EntityID, 0);
                c.StatEff.Remove(Game.StatusEffectEn.Fly);
                c.StatEff.Remove(Game.StatusEffectEn.Cyclone);
                c.StatEff.Remove(Game.StatusEffectEn.SuperMan);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                if (counter % 2 == 0)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.Teleport(MapEvent, 136, 211);
                    c.MyClient.AddSend(Packets.OverwriteGarment(183425));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of team one!");
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.Teleport(MapEvent, 187, 207);
                    c.MyClient.AddSend(Packets.OverwriteGarment(191305));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of team two!");
                }
                counter++;
            }
        }

        public void TeleAfterRev(Character C)
        {
            if (TeamOne.ContainsKey(C.EntityID))
                    C.Teleport(MapEvent, 136, 211);
                else
                    C.Teleport(MapEvent, 187, 207);
        }

        public override void WaitForWinner()
        {
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                    {
                        PlayersToRemove.Add(C.EntityID, C);
                    }
                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                    {
                        if (TeamOne.ContainsKey(C.EntityID))
                            ScoreTwo++;
                        else
                            ScoreOne++;

                        #region Revive
                        C.Action = (byte)100;
                        C.Stamina = (byte)100;
                        C.Ghost = false;
                        C.BlueName = false;
                        C.CurHP = C.MaxHP;
                        if ((int)C.MaxMP > 1)
                            C.CurMP = C.MaxMP;
                        C.Alive = true;
                        C.StatEff.Remove(StatusEffectEn.Dead);
                        C.StatEff.Remove(StatusEffectEn.BlueName);
                        C.Body = C.Body;
                        C.Hair = C.Hair;
                        C.XPKO = (byte)0;
                        #endregion
                        
                        TeleAfterRev(C);
                    }
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    C.EventBase?.RemovePlayer(C);
                    if (TeamOne.ContainsKey(C.EntityID))
                        TeamOne.Remove(C.EntityID);
                    else if (TeamTwo.ContainsKey(C.EntityID))
                        TeamTwo.Remove(C.EntityID);
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
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count == 1)
                    break;

                if (!War)
                    break;

                if (DateTime.UtcNow >= DisplayScores.AddMilliseconds(5000))
                    DisplayScore();
            }
        }
        public override void DisplayScore()
        {
            DisplayScores = DateTime.UtcNow;
            foreach (var player in PlayerList.Values)
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));

            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", $"My Score - {PlayerScores[player.EntityID]}", 0x83d, 0));
                if (ScoreOne > ScoreTwo)
                {
                    player.MyClient.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", $"Team 1 - {ScoreOne}", 0x83d, 0));
                    player.MyClient.AddSend(Packets.ChatMessage(4, "SYSTEM", "ALLUSERS", $"Team 2 - {ScoreTwo}", 0x83d, 0));
                }
                else
                {
                    player.MyClient.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", $"Team 1 - {ScoreTwo}", 0x83d, 0));
                    player.MyClient.AddSend(Packets.ChatMessage(4, "SYSTEM", "ALLUSERS", $"Team 2 - {ScoreOne}", 0x83d, 0));
                }
                
            }
        }

        public override void Reward(Character c)
        {
            c.AddItem(1088000);
            World.Action(c, (Packets.String(c.EntityID, 10, "angelwing")).Get);
            if (c.Level < 130)
                c.IncreaseExp(c.ExpBallExp, false, false);
            c.EventBase = null;
        }

        public override void End()
        {
            DisplayScore();
            foreach (var c in PlayerList.Values)
            {
                c.MyClient.LocalMessage(0x83c, "");
                c.MyClient.LocalMessage(0x83d, "");
                c.MyClient.LocalMessage(0x83d, "");
                c.CurHP = c.MaxHP;
            }
            if (ScoreOne == ScoreTwo)
            {
                World.SendMsgToAll("[EVENT]", "It's a tie! 10 Minutes have passed and the teams scored the same points! Better luck next time!", 2011, 0);
                while (PlayerList.Count > 0)
                {
                    foreach (Character C in PlayerList.Values)
                    {
                        PlayersToRemove.Add(C.EntityID, C);
                    }
                    foreach (Character C in PlayersToRemove.Values)
                    {
                        C.EventBase?.RemovePlayer(C);
                        if (TeamOne.ContainsKey(C.EntityID))
                            TeamOne.Remove(C.EntityID);
                        else if (TeamTwo.ContainsKey(C.EntityID))
                            TeamTwo.Remove(C.EntityID);
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
                }
            }
            else
            {
                if (ScoreOne > ScoreTwo)
                {
                    World.SendMsgToAll("[EVENT]", "The Red Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);
                    while (TeamTwo.Count > 0)
                    {
                        foreach (Character C in TeamTwo.Values)
                        {
                            PlayersToRemove.Add(C.EntityID, C);
                        }
                        foreach (Character C in PlayersToRemove.Values)
                        {
                            C.EventBase?.RemovePlayer(C);
                            if (TeamOne.ContainsKey(C.EntityID))
                                TeamOne.Remove(C.EntityID);
                            else if (TeamTwo.ContainsKey(C.EntityID))
                                TeamTwo.Remove(C.EntityID);
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
                    }
                }
                else if (ScoreTwo > ScoreOne)
                {
                    World.SendMsgToAll("[EVENT]", "The Blue Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);
                    while (TeamOne.Count > 0)
                    {
                        foreach (Character C in TeamOne.Values)
                        {
                            PlayersToRemove.Add(C.EntityID, C);
                        }
                        foreach (Character C in PlayersToRemove.Values)
                        {
                            C.EventBase?.RemovePlayer(C);
                            if (TeamOne.ContainsKey(C.EntityID))
                                TeamOne.Remove(C.EntityID);
                            else if (TeamTwo.ContainsKey(C.EntityID))
                                TeamTwo.Remove(C.EntityID);
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
                    }
                }
                foreach (var c in PlayerList.Values)
                {
                    Reward(c);
                    TeleportOut(c);
                }
            }
            Removeprotection();
            War = false;
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            TeamOne.Clear();
            TeamTwo.Clear();
            Abort();
            return;
        }
    }
}