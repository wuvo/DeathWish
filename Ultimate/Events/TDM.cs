using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultimate.Events
{
    public class TDM : Events
    {
        int ScoreOne, ScoreTwo = 0;
        public TDM()
        {
            EventTitle = "Team Deathmatch";
            Duration = 10;
            NoDamage = false;
            BaseMap = 1505;
            FriendlyFire = false;
        }
        
        public override void TeleportPlayersToMap()
        {
            var counter = 0;
            Teams = new Dictionary<uint, Dictionary<uint, Character>>();
            Dictionary<uint, Character> TeamOne = new Dictionary<uint, Character>();
            Dictionary<uint, Character> TeamTwo = new Dictionary<uint, Character>();
            foreach (Character c in PlayerList.Values)
            {
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
            Teams.Add(183425, TeamOne);
            Teams.Add(191305, TeamTwo);
        }

        public void TeleAfterRev(Character C)
        {
            if (Teams[183425].ContainsKey(C.EntityID))
                    C.Teleport(MapEvent, 136, 211);
                else
                    C.Teleport(MapEvent, 187, 207);
        }

        public override void WaitForWinner()
        {
            while (true)
            {

                if (DateTime.Now >= DisplayScores.AddMilliseconds(5000))
                    DisplayScore();
                System.Threading.Thread.Sleep(1);
            }
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            base.Kill(Attacker, Victim);
            if (Teams[183425].ContainsKey(Attacker.EntityID))
                ScoreOne++;
            else
                ScoreTwo++;
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
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
        }

        public override void End()
        {
            DisplayScore();
            if (ScoreOne == ScoreTwo)
            {
                World.SendMsgToAll("[EVENT]", "It's a tie! 10 Minutes have passed and the teams scored the same points! Better luck next time!", 2011, 0);
                foreach (Character C in PlayerList.Values.ToList())
                    C.EventBase?.RemovePlayer(C);
            }
            else
            {
                if (ScoreOne > ScoreTwo)
                {
                    World.SendMsgToAll("[EVENT]", "The Red Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);
                    foreach (Character C in Teams[191305].Values.ToList())
                        C.EventBase?.RemovePlayer(C);
                }
                else if (ScoreTwo > ScoreOne)
                {
                    World.SendMsgToAll("[EVENT]", "The Blue Team has won the " + EventTitle + "! Congratulations to all their members!", 2011, 0);
                    foreach (Character C in Teams[183425].Values.ToList())
                        C.EventBase?.RemovePlayer(C);
                }
                foreach (var c in PlayerList.Values.ToList())
                {
                    Reward(c);
                    RemovePlayer(c);
                }
            }
            Removeprotection();
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }
    }
}