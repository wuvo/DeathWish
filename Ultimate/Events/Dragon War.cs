using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Linq;

namespace Ultimate.Events
{
    public class DragonWar : Events
    {
        DateTime LastScore;
        bool DW = false;
        public DragonWar()
        {
            EventTitle = "Dragon War";
            Duration = 10;
            BaseMap = 2090;
            Reflect = false;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            AllowedSkills = new System.Collections.Generic.List<ushort>{ (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 7;
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
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            LastScore = DateTime.Now;
            DisplayScores = DateTime.Now;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (PlayerScores.ContainsValue(300))
                Finish();
            else if (DateTime.Now >= DisplayScores.AddMilliseconds(3000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now >= C.DeathHit.AddSeconds(2))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
            else if (C.StatEff.Contains(StatusEffectEn.DragonWar))
            {
                if (DateTime.Now >= LastScore.AddMilliseconds(1000))
                {
                    LastScore = DateTime.Now;
                    if (PlayerScores[C.EntityID] + 3 > 300)
                        PlayerScores[C.EntityID] = 300;
                    else
                        PlayerScores[C.EntityID] += 3;
                }
            }
        }

        public override void RemovePlayer(Character C, bool exp = true)
        {
            base.RemovePlayer(C, exp);
            if (C.StatEff.Contains(StatusEffectEn.DragonWar))
                DW = false;
            C.StatEff.Remove(StatusEffectEn.DragonWar);
        }

        public override void End()
        {
            foreach (Character C in PlayerList.Values.ToList())
                C.StatEff.Remove(StatusEffectEn.DragonWar);

            DW = false;
            Removeprotection();
            DisplayScore();
            int NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                if (NO == 1)
                {
                    Reward(PlayerList[player.Key]);
                    RemovePlayer(PlayerList[player.Key]);
                    NO++;
                }
                else
                    RemovePlayer(PlayerList[player.Key]);
            }
            PlayerList.Clear();
            PlayerScores.Clear();
            return;
        }

        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            if (User.StatEff.Contains(StatusEffectEn.DragonWar))
                return Convert.ToUInt32(C.MaxHP);
            else if (!DW)
                return Convert.ToUInt32(C.MaxHP * 0.4);
            else if (C.StatEff.Contains(StatusEffectEn.DragonWar))
                return Convert.ToUInt32(C.MaxHP * 0.4);
            return 1;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Victim.StatEff.Contains(StatusEffectEn.DragonWar))
            {
                if (PlayerScores[Attacker.EntityID] + 5 > 300)
                    PlayerScores[Attacker.EntityID] = 300;
                else
                    PlayerScores[Attacker.EntityID] += 5;
            }
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            if (Victim.StatEff.Contains(StatusEffectEn.DragonWar))
            {
                Victim.StatEff.Remove(StatusEffectEn.DragonWar);
                Attacker.StatEff.Add(StatusEffectEn.DragonWar);
                Attacker.Stamina = 100;
                if (PlayerScores[Attacker.EntityID] + 5 > 300)
                    PlayerScores[Attacker.EntityID] = 300;
                else
                    PlayerScores[Attacker.EntityID] += 5;
            }
            else if (!DW)
            {
                Attacker.StatEff.Add(StatusEffectEn.DragonWar);
                PlayerScores[Attacker.EntityID]++;
                DW = true;
            }
            else if (Attacker.StatEff.Contains(StatusEffectEn.DragonWar))
            {
                if (PlayerScores[Attacker.EntityID] + 5 > 300)
                    PlayerScores[Attacker.EntityID] = 300;
                else
                    PlayerScores[Attacker.EntityID] += 5;
            }
            else
            {
                if (PlayerScores[Attacker.EntityID]++ > 300)
                    PlayerScores[Attacker.EntityID] = 300;
                else
                    PlayerScores[Attacker.EntityID]++;
            }
        }

        private void TeleAfterRev(Character C)
        {
            X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
            Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
            C.Teleport(MapEvent, (ushort)X, (ushort)Y);
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
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