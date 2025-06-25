using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class WackaMoleHalloween : Events
    {
        private byte _safe;
        public WackaMoleHalloween()
        {
            EventTitle = "Halloween Wack-A-Mole";
            Duration = 10;
            BaseMap = 700;
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
            DialogID = 17;
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
                if (TeamTwo.Count < TeamOne.Count)
                {
                    TeamTwo.Add(c.EntityID, c);
                    X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                    Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                    c.Teleport(MapEvent, X, Y);
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Zombies!");
                }
                else
                {
                    TeamOne.Add(c.EntityID, c);
                    X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                    Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                    c.Teleport(MapEvent, X, Y);
                    c.MyClient.LocalMessage(2000, $"Welcome to {EventTitle} you're a member of the Humans!");
                }
                counter++;
            }
            Teams.Add(184345, TeamOne);
            Teams.Add(189010, TeamTwo);
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
            _safe = 5;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (Infected())
            {
                if (_safe < 20)
                    _safe = 20;
                else if (_safe == 22)
                    Finish();
                else
                    _safe++;
            }
            else if (DateTime.Now >= DisplayScores.AddMilliseconds(1000))
            {
                DisplayScore();
                if (_safe > 0)
                {
                    _safe--;
                    if (_safe == 5)
                        RemoveTargets();
                }
                else
                {
                    AddTargets();
                    _safe = 11;
                }
            }
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (Teams[184345].ContainsKey(Attacker.EntityID))
                    Attacker.MyClient.LocalMessage(2000, "This player already turned into a Zombie! Please kill his/her Scarecrow!");
                else if (Teams[189010].ContainsKey(Attacker.EntityID))
                {
                    if (Teams[184345].ContainsKey(Victim.EntityID))
                        Teams[184345].Remove(Victim.EntityID);

                    Teams[189010].Add(Victim.EntityID, Victim);
                    Victim.MyClient.LocalMessage(2000, "You've joined the Zombies!");
                    Victim.MyClient.AddSend(Packets.OverwriteGarment(189010));

                    if (PlayerScores.ContainsKey(Attacker.EntityID))
                        PlayerScores[Attacker.EntityID]++;

                    foreach (Character C in Victim.ScreenChars.Values)
                        C.MyClient.AddSend(Packets.SpawnEntity(Victim));
                }
            }
        }

        public override uint GetDamage(Character User, NPC Victim, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 10000;
        }

        public override void Kill(Character Attacker, NPC Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (Teams[184345].ContainsKey(Attacker.EntityID))
                {
                    foreach (Character C in Teams[189010].Values.ToList())
                    {
                        if (Victim.Name == C.Name)
                        {
                            Teams[189010].Remove(C.EntityID);
                            Teams[184345].Add(C.EntityID, C);
                            C.MyClient.LocalMessage(2000, "Your Scarescrow was killed! You've joined the Humans!");
                            C.MyClient.AddSend(Packets.OverwriteGarment(184345));

                            if (PlayerScores.ContainsKey(Attacker.EntityID))
                                PlayerScores[Attacker.EntityID]++;

                            foreach (Character C2 in C.ScreenChars.Values)
                                C2.MyClient.AddSend(Packets.SpawnEntity(C));

                            if (World.H_NPCs[MapEvent].ContainsKey(Victim.EntityID))
                            {
                                World.H_NPCs[MapEvent].Remove(Victim.EntityID);
                                Game.World.Action(Victim, Packets.GeneralData(Victim.EntityID, 0, 0, 0, 135).Get);
                            }
                        }
                    }
                }
                else if (Teams[189010].ContainsKey(Attacker.EntityID))
                    Attacker.MyClient.LocalMessage(2000, "Zombies cannot kill Scarecrows! Please fight the Humans!");
                else
                    Attacker.MyClient.LocalMessage(2000, "There has to be at least one Zombie in this tournament!");
            }
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            if (_safe > 5)
                Broadcast($"Scarecrows vanish in: 00:0{_safe - 6} secs", BroadCastLoc.Score, 2);
            else if (_safe <= 5)
                Broadcast($"Scarecrows spawn in: 00:0{_safe} secs", BroadCastLoc.Score, 2);
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
            if (World.H_NPCs.ContainsKey(MapEvent))
            {
                Dictionary<uint, NPC> MapNPC = World.H_NPCs[MapEvent];
                foreach (NPC N in MapNPC.Values.ToList())
                {
                    MapNPC.Remove(N.EntityID);
                    Game.World.Action(N, Packets.GeneralData(N.EntityID, 0, 0, 0, 135).Get);
                }
                World.H_NPCs.Remove(MapEvent);
            }
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
            Removeprotection();
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }

        private void AddTargets()
        {
            uint count = 7500;
            RemoveTargets();
            foreach (Character C in Teams[189010].Values)
            {
                NPC NPCInfo = new NPC()
                {
                    EntityID = count,
                    Type = 9330,
                    Flags = 22,
                    Loc = new Location() { Map = MapEvent, X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21)), Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20)) },
                    Direction = 0,
                    Avatar = 0,
                    CurHP = 10000,
                    MaxHP = 10000,
                    Level = 1,
                    PlayerEvent = true,
                    Name = C.Name,
                };

                if (!World.H_NPCs.ContainsKey(NPCInfo.Loc.Map))
                    World.H_NPCs.Add(NPCInfo.Loc.Map, new Dictionary<uint, NPC>());

                Dictionary<uint, NPC> NPCMap = World.H_NPCs[NPCInfo.Loc.Map];
                if (!NPCMap.ContainsKey(NPCInfo.EntityID))
                {
                    NPCMap.Add(NPCInfo.EntityID, NPCInfo);
                    World.Spawn(NPCInfo);
                }
                count++;
            }
        }

        private void RemoveTargets()
        {
            if (World.H_NPCs.ContainsKey(MapEvent))
            {
                Dictionary<uint, NPC> MapNPC = World.H_NPCs[MapEvent];
                foreach (NPC N in MapNPC.Values.ToList())
                {
                    MapNPC.Remove(N.EntityID);
                    Game.World.Action(N, Packets.GeneralData(N.EntityID, 0, 0, 0, 135).Get);
                }
            }
        }

        private bool Infected()
        {
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                if (T.Value.Count == 0)
                    return true;
            return false;
        }
    }
}