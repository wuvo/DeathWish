using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Features;
using System.Threading.Tasks;

namespace Ultimate.Events
{
    public class HalloweenInfection : Events
    {
        private byte _safe = 0;
        public HalloweenInfection()
        {
            EventTitle = "Halloween Infection";
            Duration = 10;
            BaseMap = 1508;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = false;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 16;
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
                c.CurHP = c.MaxHP;
                c.Protection = true;
                if (TeamOne.Count <= TeamTwo.Count)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(125 + (ushort)Program.Rnd.Next(3)), (ushort)(165 + (ushort)Program.Rnd.Next(3)));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Humans!");
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.Teleport(MapEvent, (ushort)(113 + (ushort)Program.Rnd.Next(3)), (ushort)(79 + (ushort)Program.Rnd.Next(3)));
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Zombies!");
                }
                counter++;
                c.MyClient.AddSend(Packets.GeneralData(c.EntityID, 5855577, 0, 0, 104));
            }
            Teams.Add(184345, TeamOne);
            Teams.Add(189010, TeamTwo);
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (Infected())
            {
                if (_safe == 2)
                    Finish();
                else
                    _safe++;
            }

            if (DateTime.Now >= DisplayScores.AddMilliseconds(2500))
                DisplayScore();
        }

        private bool Infected()
        {
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                if (T.Value.Count == 0)
                    return true;
            return false;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                {
                    if (T.Value.ContainsKey(Attacker.EntityID) && !T.Value.ContainsKey(Victim.EntityID))
                    {
                        foreach (Dictionary<uint, Character> T2 in Teams.Values)
                            if (T2.ContainsKey(Victim.EntityID))
                            {
                                T2.Remove(Victim.EntityID);
                                break;
                            }
                        T.Value.Add(Victim.EntityID, Victim);
                        Victim.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
                    }
                }
                //if (Teams[184345].ContainsKey(Attacker.EntityID))
                //{
                //    if (Teams[189010].ContainsKey(Victim.EntityID))
                //        Teams[189010].Remove(Victim.EntityID);

                //    Teams[184345].Add(Victim.EntityID, Victim);
                //    Victim.MyClient.LocalMessage(2000, "You've joined the Humans!");
                //    Victim.MyClient.AddSend(Packets.OverwriteGarment(TeamOneGarment));
                //}
                //else if (Teams[189010].ContainsKey(Attacker.EntityID))
                //{
                //    if (Teams[184345].ContainsKey(Victim.EntityID))
                //        Teams[184345].Remove(Victim.EntityID);

                //    Teams[189010].Add(Victim.EntityID, Victim);
                //    Victim.MyClient.LocalMessage(2000, "You've joined the Zombies!");
                //    Victim.MyClient.AddSend(Packets.OverwriteGarment(TeamTwoGarment));
                //}

                if (PlayerScores.ContainsKey(Attacker.EntityID))
                    PlayerScores[Attacker.EntityID]++;

                foreach (Character C in Victim.ScreenChars.Values)
                    C.MyClient.AddSend(Packets.SpawnEntity(Victim));
            }
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

        public override void End()
        {
            DisplayScore();
            int NO = 1;
            foreach (var player in PlayerScores.OrderByDescending(s => s.Value).ToList())
            {
                PlayerList[player.Key].MyClient.AddSend(Packets.GeneralData(PlayerList[player.Key].EntityID, 0, 0, 0, 104));
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
            foreach (var c in PlayerList.Values.ToList())
            {
                c.MyClient.AddSend(Packets.GeneralData(c.EntityID, 0, 0, 0, 104));
                if (c.Equips.Garment.ID == 0)
                {
                    c.MyClient.AddSend(Packets.OverwriteGarment(0));
                    c.Equips.Replace(9, new Item(), c);
                }

                c.Equips.Send(c.MyClient, false);
                RemovePlayer(c);
            }
            Removeprotection();
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }
    }
}