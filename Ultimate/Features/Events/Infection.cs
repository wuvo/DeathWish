using NewestCOServer.Features.Events;
using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Features
{
    public class Infection : PVPEvents
    {
        private DateTime DisplayScores;
        public Infection()
        {
            EventTitle = "Infection";
            Duration = 10;
            MapEvent = 1507;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = false;
            FFADamage = false;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };
        }
        public override void TeleportPlayersToMap()
        {
            var counter = 0;
            foreach (Character c in PlayerList.Values)
            {
                PlayerScores.Add(c.EntityID, 0);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                if (counter % 4 == 0)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(57 + (ushort)Program.Rnd.Next(5)), (ushort)(85 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(183425));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Blue Team!");
                }
                else if (counter == 1)
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(106 + (ushort)Program.Rnd.Next(7)), (ushort)(85 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(191605));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Red Team!");
                }
                else if (counter == 2)
                {
                    TeamThree.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(57 + (ushort)Program.Rnd.Next(5)), (ushort)(136 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(181525));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Black Team!");
                }
                else if (counter == 3)
                {
                    TeamFour.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(106 + (ushort)Program.Rnd.Next(7)), (ushort)(136 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.AddSend(Packets.OverwriteGarment(181325));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the White Team!");
                }
                counter++;
            }
        }

        public override void WaitForWinner()
        {
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        PlayersToRemove.Add(C.EntityID, C);
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    if (TeamOne.ContainsKey(C.EntityID))
                        TeamOne.Remove(C.EntityID);
                    else if (TeamTwo.ContainsKey(C.EntityID))
                        TeamTwo.Remove(C.EntityID);
                    else if (TeamThree.ContainsKey(C.EntityID))
                        TeamThree.Remove(C.EntityID);
                    else if (TeamFour.ContainsKey(C.EntityID))
                        TeamFour.Remove(C.EntityID);
                    C.EventBase?.RemovePlayer(C);

                    if (PlayerScores.ContainsKey(C.EntityID))
                        PlayerScores.Remove(C.EntityID);

                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    RemovedPlayers.Add(C.EntityID, C);
                }
                foreach (Character C in RemovedPlayers.Values)
                    if (PlayersToRemove.ContainsKey(C.EntityID))
                        PlayersToRemove.Remove(C.EntityID);

                if (Infected())
                {
                    System.Threading.Thread.Sleep(3000);
                    break;
                }

                if (DateTime.UtcNow >= EndTime)
                    break;

                if (!War)
                    break;

                if (DateTime.UtcNow >= DisplayScores.AddMilliseconds(2500))
                    DisplayScore();
            }
        }

        private bool Infected()
        {
            if (TeamOne.Count > 0 && TeamTwo.Count == 0 && TeamThree.Count == 0 && TeamFour.Count == 0)
                return true;
            else if (TeamOne.Count == 0 && TeamTwo.Count > 0 && TeamThree.Count == 0 && TeamFour.Count == 0)
                return true;
            else if (TeamOne.Count == 0 && TeamTwo.Count == 0 && TeamThree.Count > 0 && TeamFour.Count == 0)
                return true;
            else if (TeamOne.Count == 0 && TeamTwo.Count == 0 && TeamThree.Count == 0 && TeamFour.Count > 0)
                return true;
            return false;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (TeamOne.ContainsKey(Attacker.EntityID))
                {
                    if (TeamTwo.ContainsKey(Victim.EntityID))
                        TeamTwo.Remove(Victim.EntityID);
                    else if (TeamThree.ContainsKey(Victim.EntityID))
                        TeamThree.Remove(Victim.EntityID);
                    else if (TeamFour.ContainsKey(Victim.EntityID))
                        TeamFour.Remove(Victim.EntityID);
                    TeamOne.Add(Victim.EntityID, Victim);
                    Victim.MyClient.LocalMessage(2000, "You've joined the Blue Team");
                    Victim.MyClient.AddSend(Packets.OverwriteGarment(183425));
                }
                else if (TeamTwo.ContainsKey(Attacker.EntityID))
                {
                    if (TeamOne.ContainsKey(Victim.EntityID))
                        TeamOne.Remove(Victim.EntityID);
                    else if (TeamThree.ContainsKey(Victim.EntityID))
                        TeamThree.Remove(Victim.EntityID);
                    else if (TeamFour.ContainsKey(Victim.EntityID))
                        TeamFour.Remove(Victim.EntityID);
                    TeamTwo.Add(Victim.EntityID, Victim);
                    Victim.MyClient.LocalMessage(2000, "You've joined the Red Team");
                    Victim.MyClient.AddSend(Packets.OverwriteGarment(191305));
                }
                else if (TeamThree.ContainsKey(Attacker.EntityID))
                {
                    if (TeamOne.ContainsKey(Victim.EntityID))
                        TeamOne.Remove(Victim.EntityID);
                    else if (TeamTwo.ContainsKey(Victim.EntityID))
                        TeamTwo.Remove(Victim.EntityID);
                    else if (TeamFour.ContainsKey(Victim.EntityID))
                        TeamFour.Remove(Victim.EntityID);
                    TeamThree.Add(Victim.EntityID, Victim);
                    Victim.MyClient.LocalMessage(2000, "You've joined the Black Team");
                    Victim.MyClient.AddSend(Packets.OverwriteGarment(181525));
                }
                else if (TeamFour.ContainsKey(Attacker.EntityID))
                {
                    if (TeamOne.ContainsKey(Victim.EntityID))
                        TeamOne.Remove(Victim.EntityID);
                    else if (TeamTwo.ContainsKey(Victim.EntityID))
                        TeamTwo.Remove(Victim.EntityID);
                    else if (TeamThree.ContainsKey(Victim.EntityID))
                        TeamThree.Remove(Victim.EntityID);
                    TeamFour.Add(Victim.EntityID, Victim);
                    Victim.MyClient.LocalMessage(2000, "You've joined the White Team");
                    Victim.MyClient.AddSend(Packets.OverwriteGarment(181325));
                }
                if (PlayerScores.ContainsKey(Attacker.EntityID))
                    PlayerScores[Attacker.EntityID]++;
                foreach (Character C in Victim.ScreenChars.Values)
                    C.MyClient.AddSend(Packets.SpawnEntity(Victim));
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

        public override void End()
        {
            DisplayScore();
            int NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                if (NO == 1)
                {
                    Reward(PlayerList[player.Key]);
                    RemovePlayer(PlayerList[player.Key]);
                    //if (PlayerScores.ContainsKey(player.Key))
                    //    PlayerScores.Remove(player.Key);
                    NO++;
                }
                else if (NO >= 2 && NO <= 5)
                {
                    World.Action(PlayerList[player.Key], (Packets.String(PlayerList[player.Key].EntityID, 10, "angelwing")).Get);
                    if (PlayerList[player.Key].Level < 130)
                        PlayerList[player.Key].IncreaseExp((PlayerList[player.Key].ExpBallExp * 2) / Convert.ToByte(NO), false, false);
                    RemovePlayer(PlayerList[player.Key]);
                    //if (PlayerScores.ContainsKey(player.Key))
                    //    PlayerScores.Remove(player.Key);
                    NO++;
                }
                else
                {
                    if (PlayerList.ContainsKey(player.Key))
                    {
                        RemovePlayer(PlayerList[player.Key]);
                        //if (PlayerScores.ContainsKey(player.Key))
                        //    PlayerScores.Remove(player.Key);
                        NO++;
                    }
                }
            }
            foreach (var c in PlayerList.Values)
            {
                if (c.Equips.Garment.ID == 0)
                {
                    c.MyClient.AddSend(Packets.OverwriteGarment(0));
                    c.Equips.Replace(9, new Item(), c);
                }

                c.Equips.Send(c.MyClient, false);
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
            TeamOne.Clear();
            TeamTwo.Clear();
            TeamThree.Clear();
            TeamFour.Clear();
            Abort();
            return;
        }
    }
}
