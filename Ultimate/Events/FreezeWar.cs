using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class FreezeWar : Events
    {
        byte Freeze = 0;
        public FreezeWar()
        {
            EventTitle = "Team Freeze Pk";
            Duration = 10;
            BaseMap = 1506;
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
            DialogID = 8;
            minplayers = 1;
        }

        public override void TeleportPlayersToMap()
        {
            var counter = 0;
            Teams = new Dictionary<uint, Dictionary<uint, Character>>();
            Dictionary<uint, Character> TeamOne = new Dictionary<uint, Character>();
            Dictionary<uint, Character> TeamTwo = new Dictionary<uint, Character>();
            foreach (Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.Team);
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
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Blue Team!");
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(83 + (ushort)Program.Rnd.Next(6)), (ushort)(26 + (ushort)Program.Rnd.Next(6)));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Red Team!");
                }
                counter++;
            }
            Teams.Add(183425, TeamOne);
            Teams.Add(191605, TeamTwo);
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
            DisplayScores = DateTime.Now;
            Freeze = 5;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if ((Teams[183425].ContainsKey(Attacker.EntityID) && Teams[183425].ContainsKey(Victim.EntityID)) || (Teams[191605].ContainsKey(Attacker.EntityID) && Teams[191605].ContainsKey(Victim.EntityID)))
                {
                    if (Victim.StatEff.Contains(StatusEffectEn.IceBlock))
                    {
                        Victim.StatEff.Remove(StatusEffectEn.IceBlock);
                        PlayerScores[Attacker.EntityID] += 2;
                    }
                }
                else
                {
                    if (!Victim.StatEff.Contains(StatusEffectEn.IceBlock))
                    {
                        Victim.StatEff.Add(StatusEffectEn.IceBlock);
                        PlayerScores[Attacker.EntityID] += 2;
                    }
                }
            }
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        public override void RemovePlayer(Character C, bool exp = true)
        {
            base.RemovePlayer(C, exp);
            C.StatEff.Remove(StatusEffectEn.IceBlock);
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();

            if ((OneAllFrozen() || TwoAllFrozen()) && Freeze != 5)
            {
                if (Freeze < 1)
                    Freeze++;
                else if (Freeze == 1)
                    Finish();
            }
            else if (Freeze == 5)
            {
                foreach (Character C in PlayerList.Values)
                    C.StatEff.Remove(StatusEffectEn.IceBlock);
                Freeze = 0;
            }
            else if (Teams[183425].Count == 0 || Teams[191605].Count == 0)
                Finish();

            if (DateTime.Now >= DisplayScores.AddMilliseconds(3000))
                DisplayScore();
        }

        public bool OneAllFrozen()
        {
            try
            {
                if (Teams[183425].Count == 0)
                    return true;
                foreach (Character p in Teams[183425].Values)
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
                if (Teams[191605].Count == 0)
                    return true;
                foreach (Character p in Teams[191605].Values)
                    if (!p.StatEff.Contains(StatusEffectEn.IceBlock))
                        return false;
                return true;
            }
            catch { return false; }
        }

        public override void End()
        {
            DisplayScore();
            Removeprotection();
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
            Teams.Clear();
            return;
        }
    }
}