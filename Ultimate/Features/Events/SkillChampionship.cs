using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Features
{
    public class SkillChampionship : PVPEvents
    {
        public SkillChampionship()
        {
            EventTitle = "Skill Championship";
            Duration = 10;
            MapEvent = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = true;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };
        }

        private DateTime LastScores;

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
                c.CurHP = 1;
                c.Protection = true;
            }
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Attacker.EntityID))
                PlayerScores[Attacker.EntityID]++;
        }

        public override void WaitForWinner()
        {
            LastScores = DateTime.UtcNow;
            while (true)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        C.EventBase?.RemovePlayer(C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(10))
                    {
                        #region Revive
                        C.Action = (byte)100;
                        C.Stamina = (byte)100;
                        C.Ghost = false;
                        C.BlueName = false;
                        C.CurHP = 1;
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

                    else if (C.CurHP > 1)
                        C.CurHP = 1;
                }
                
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count <= 1)
                    break;

                if (!War)
                    break;

                if (DateTime.UtcNow >= LastScores.AddMilliseconds(3000))
                    DisplayScore();
            }
        }

        public override void End()
        {
            DisplayScore();
            byte NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                if (NO == 1)
                {
                    Reward(PlayerList[player.Key]);
                    PlayerList[player.Key].TopFB = 1;
                    PlayerList[player.Key].StatEff.Add(StatusEffectEn.TopFBSS);
                    NO++;
                }
                else if (NO >= 2 && NO <= 3)
                {
                    World.Action(PlayerList[player.Key], (Packets.String(PlayerList[player.Key].EntityID, 10, "angelwing")).Get);
                    PlayerList[player.Key].TopFB = 2;
                    PlayerList[player.Key].StatEff.Add(StatusEffectEn.Top3FBSS);
                    if (PlayerList[player.Key].Level < 130)
                        PlayerList[player.Key].IncreaseExp((PlayerList[player.Key].ExpBallExp * 2), false, false);
                    NO++;
                }
                else
                {
                    if (PlayerList.ContainsKey(player.Key))
                        NO++;
                }
                if (PlayerList.ContainsKey(player.Key))
                    RemovePlayer(PlayerList[player.Key]);

                if (PlayerScores.ContainsKey(player.Key))
                    PlayerScores.Remove(player.Key);
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

        public override void Reward(Character c)
        {
            Broadcast(c.Name + " has won the " + EventTitle + " receiving the Top FB/SS Halo and a DBScroll!", BroadCastLoc.World);
            if (c.Inventory.Count < 40)
                c.AddItem(720028);
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
            LastScores = DateTime.UtcNow;
        }
    }
}