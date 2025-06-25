using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class SkillChampionship : Events
    {
        DateTime LastScores;

        public SkillChampionship()
        {
            EventTitle = "Skill Championship";
            Duration = 10;
            BaseMap = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = true;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 3;
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
                c.Protection = true;
                c.Teleport(MapEvent, X, Y);
            }
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Attacker.EntityID))
                PlayerScores[Attacker.EntityID]++;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (DateTime.Now >= LastScores.AddMilliseconds(3000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now >= C.DeathHit.AddMilliseconds(5000))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
            else if (DateTime.Now >= C.LastMove.AddSeconds(60))
                C.EventBase?.RemovePlayer(C);
        }

        public override void End()
        {
            DisplayScore();
            Removeprotection();
            byte NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                if (NO == 1)
                {
                    Reward(PlayerList[player.Key]);
                    PlayerList[player.Key].TopFB = 1;
                    PlayerList[player.Key].StatEff.Add(StatusEffectEn.TopFBSS);
                    RemovePlayer(PlayerList[player.Key]);
                    NO++;
                }
                else if (NO >= 2 && NO <= 3)
                {
                    PlayerList[player.Key].TopFB = 2;
                    PlayerList[player.Key].StatEff.Add(StatusEffectEn.Top3FBSS);
                    RemovePlayer(PlayerList[player.Key]);
                    NO++;
                }
                else
                {
                    if (PlayerList.ContainsKey(player.Key))
                    {
                        RemovePlayer(PlayerList[player.Key]);
                        NO++;
                    }
                }
            }

            PlayerList.Clear();
            PlayerScores.Clear();
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
            LastScores = DateTime.Now;
        }

        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            return C.MaxHP;
        }
    }
}