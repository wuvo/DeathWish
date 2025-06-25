using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class PimpOutSanta : Events
    {
        private readonly Dictionary<uint, SOB> Statues = new Dictionary<uint, SOB>();
        uint TeamOneSantaID = 0;
        uint TeamTwoSantaID = 0;
        byte TeamOneScore = 0;
        byte TeamTwoScore = 0;
        bool TeamOneDone = false;
        bool TeamTwoDone = false;

        /// <summary>
        /// Details of the PVP Event
        /// </summary>
        public PimpOutSanta()
        {
            EventTitle = "Pimp Out Hero";
            Duration = 10;
            MapEvent = 700;
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
            DialogID = 11;
        }

        /// <summary>
        /// Teleports players in the Playerlist to the map where the event will be held
        /// </summary>
        public override void TeleportPlayersToMap()
        {
            Teams = new Dictionary<uint, Dictionary<uint, Character>>();
            Dictionary<uint, Character> TeamOne = new Dictionary<uint, Character>();
            Dictionary<uint, Character> TeamTwo = new Dictionary<uint, Character>();
            foreach (Character c in PlayerList.Values)
            {
                if (TeamOne.Count <= TeamTwo.Count)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.MyClient.LocalMessage(2000, "Congratulations! You have joined the Black Team!");
                    X = (ushort)(65 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(35 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.MyClient.LocalMessage(2000, "Congratulations! You have joined the White Team!");
                    X = (ushort)(35 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(65 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                }
                ChangePKMode(c, PKMode.Team);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            Teams.Add(194350, TeamOne);
            Teams.Add(194360, TeamTwo);
            DisplayScores = DateTime.Now;
            SpawnStatueOne();
            SpawnStatueTwo();

            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        /// <summary>
        /// Determines what we're supposed to do when a player gets hit
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (Teams[194350].ContainsKey(Attacker.EntityID))
                {
                    if (!TeamOneDone)
                    {
                        TeamOneScore++;
                        if (TeamOneScore >= 5)
                        {
                            TeamOneScore = 0;
                            DressUpSanta(Statues[TeamOneSantaID]);
                        }
                    }
                    else if (Teams[194360].ContainsKey(Victim.EntityID))
                    {
                        PlayerScores[Attacker.EntityID]++;
                        Teams[194360].Remove(Victim.EntityID);
                        RemovePlayer(Victim);
                    }
                }
                else
                {
                    if (!TeamTwoDone)
                    {
                        TeamTwoScore++;
                        if (TeamTwoScore >= 5)
                        {
                            TeamTwoScore = 0;
                            DressUpSanta(Statues[TeamTwoSantaID]);
                        }
                    }
                    else if (Teams[194350].ContainsKey(Victim.EntityID))
                    {
                        PlayerScores[Attacker.EntityID]++;
                        Teams[194350].Remove(Victim.EntityID);
                        RemovePlayer(Victim);
                    }
                }
            }
        }

        /// <summary>
        /// Handles everything that's happening while the event is running
        /// </summary>
        public override void WaitForWinner()
        {
            base.WaitForWinner();
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                if (T.Value.Count == 0)
                    Finish();
            if (DateTime.Now >= DisplayScores.AddMilliseconds(1000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (DateTime.Now >= C.LastMove.AddSeconds(60))
            {
                C.EventBase?.RemovePlayer(C);
                if (Teams[194350].ContainsKey(C.EntityID))
                    Teams[194350].Remove(C.EntityID);
                else if (Teams[194360].ContainsKey(C.EntityID))
                    Teams[194360].Remove(C.EntityID);
            }
        }
        /// <summary>
        /// Spawns the statue for the BlueTeam
        /// </summary>
        private void SpawnStatueOne()
        {
            uint EntityID = 100001;
            while (World.H_SOBs.ContainsKey(EntityID))
                EntityID++;
            SOB TeamOneSanta = new SOB()
            {
                EntityID = EntityID,
                Name = "BlackCat",
                Mesh = 11004,
                Type = Looks.Statue,
                GuildID = 43678,
                GuildRank = 90,
                Headgear = 0,
                Necklace = 0,
                Ring = 0,
                RightHand = 0,
                Armor = 0,
                LeftHand = 0,
                Garment = 0,
                Hair = 417,
                ArmorColor = 0,
                LeftHandColor = 0,
                HeadgearColor = 0,
                Direction = 7,
                Frame = 7,
                Action = 190,
                Loc = new Location() { X = 59, Y = 41, Map = MapEvent },
                MaxHP = 50000,
                CurHP = 50000
            };
            SOB.GuildStatue.AddStatue(TeamOneSanta);
            Statues.Add(TeamOneSanta.EntityID, TeamOneSanta);
            TeamOneSantaID = EntityID;
        }

        /// <summary>
        /// Spawns the statue for the RedTeam
        /// </summary>
        private void SpawnStatueTwo()
        {
            uint EntityID = 100001;
            while (World.H_SOBs.ContainsKey(EntityID))
                EntityID++;
            SOB TeamTwoSanta = new SOB()
            {
                EntityID = EntityID,
                Name = "WhiteCat",
                Mesh = 11004,
                Type = Looks.Statue,
                GuildID = 43678,
                GuildRank = 90,
                Headgear = 0,
                Necklace = 0,
                Ring = 0,
                RightHand = 0,
                Armor = 0,
                LeftHand = 0,
                Garment = 0,
                Hair = 441,
                ArmorColor = 0,
                LeftHandColor = 0,
                HeadgearColor = 0,
                Direction = 7,
                Frame = 9,
                Action = 190,
                Loc = new Location() { X = 41, Y = 59, Map = MapEvent },
                MaxHP = 50000,
                CurHP = 50000
            };
            SOB.GuildStatue.AddStatue(TeamTwoSanta);
            Statues.Add(TeamTwoSanta.EntityID, TeamTwoSanta);
            TeamTwoSantaID = EntityID;
        }

        /// <summary>
        /// Dresses up Santa
        /// </summary>
        /// <param name="S"></param>
        private void DressUpSanta(SOB S)
        {
            if (S.RightHand == 0)
                S.RightHand = 410309;
            else if (S.Headgear == 0)
                S.Headgear = 115000;
            else if (S.LeftHand == 0)
                S.LeftHand = 420309;
            else if (S.Garment == 0)
            {
                if (S.EntityID == TeamOneSantaID)
                {
                    TeamOneDone = true;
                    S.Garment = 194350;
                    S.Action = 130;
                    Broadcast("BlackCat`s Hero is all dressed up ! Be careful with his teammates or you'll be kicked if they hit you !", BroadCastLoc.Map);
                }
                else
                {
                    TeamTwoDone = true;
                    S.Garment = 194360;
                    S.Action = 130;
                    Broadcast("WhiteCat`s Hero is all dressed up ! Be careful with his teammates or you'll be kicked if they hit you !", BroadCastLoc.Map);
                }
            }
            S.ReSpawn();
        }

        /// <summary>
        /// Finish event and reward winner
        /// </summary>
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
            foreach (SOB S in Statues.Values)
                SOB.GuildStatue.RemoveStatue(S);

            Statues.Clear();
            TeamOneScore = 0;
            TeamTwoScore = 0;
            TeamOneDone = false;
            TeamTwoDone = false;
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }

        /// <summary>
        /// Displays the score inside the event
        /// </summary>
        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values.ToList())
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            if (!TeamOneDone && !TeamTwoDone)
            {
                Broadcast($"BlackCat Hits to next item - {5 - TeamTwoScore}", BroadCastLoc.Score, 2);
                Broadcast($"WhiteCat Hits to next item - {5 - TeamOneScore}", BroadCastLoc.Score, 3);
                byte Score = 4;
                foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)).ToList())
                {
                    if (Score == 7)
                        break;
                    if (Score == PlayerScores.Count + 4)
                        break;
                    if (PlayerList.ContainsKey(kvp.Key))
                        Broadcast($"Nº {Score - 3}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                    Score++;
                }
            }
            else if (TeamOneDone && !TeamTwoDone)
            {
                Broadcast($"BlackCat Hits to next item - {5 - TeamTwoScore}", BroadCastLoc.Score, 2);
                Broadcast($"WhiteCat Team is scoring! Points: {TeamScores(1)}", BroadCastLoc.Score, 3);
                byte Score = 4;
                foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)).ToList())
                {
                    if (Score == 7)
                        break;
                    if (Score == PlayerScores.Count + 4)
                        break;
                    if (PlayerList.ContainsKey(kvp.Key))
                        Broadcast($"Nº {Score - 3}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                    Score++;
                }
            }
            else if (!TeamOneDone && TeamTwoDone)
            {
                Broadcast($"BlackCat Team is scoring! Points: {TeamScores(2)}", BroadCastLoc.Score, 2);
                Broadcast($"WhiteCat Hits to next item - {5 - TeamOneScore}", BroadCastLoc.Score, 3);

                byte Score = 4;
                foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)).ToList())
                {
                    if (Score == 7)
                        break;
                    if (Score == PlayerScores.Count + 4)
                        break;
                    if (PlayerList.ContainsKey(kvp.Key))
                        Broadcast($"Nº {Score - 3}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                    Score++;
                }
            }
            else
            {
                Broadcast($"BlackCat Team is scoring! Points: {TeamScores(2)}", BroadCastLoc.Score, 2);
                Broadcast($"WhiteCat Team is scoring! Points: {TeamScores(1)}", BroadCastLoc.Score, 3);

                byte Score = 4;
                foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)).ToList())
                {
                    if (Score == 7)
                        break;
                    if (Score == PlayerScores.Count + 4)
                        break;
                    if (PlayerList.ContainsKey(kvp.Key))
                        Broadcast($"Nº {Score - 3}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                    Score++;
                }
            }
        }

        /// <summary>
        /// Gets the team score
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private int TeamScores(byte team)
        {
            int Score = 0;
            if (team == 1)
                foreach (Character C in Teams[194350].Values.ToList())
                    Score += PlayerScores[C.EntityID];
            else
                foreach (Character C in Teams[194360].Values.ToList())
                    Score += PlayerScores[C.EntityID];
            return Score;
        }
    }
}