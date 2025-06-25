using System.Collections.Generic;
using System.Linq;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class Arena
    {
        public enum DuelType
        {
            Standard,
            Leech,
            UnlimitedStamina
        }
        public enum Opponent
        {
            Single,
            Team
        }
        public enum Hits
        {
            Ten,
            Hundred
        }
        public enum BroadCastLoc
        {
            World,
            Map,
            Score,
            Title
        }

        public uint MapID = 10000;
        public bool Wager = false;
        public uint WagerAmount = 0;
        public DuelType Type;
        public Opponent Against;
        public Hits Count;
        public uint Inviter = 0;
        public Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public Dictionary<uint, Character> TeamOne;
        public Dictionary<uint, Character> TeamTwo;

        /// <summary>
        /// Used to send messages related to the current Duel
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loc"></param>
        /// <param name="index"></param>
        public void Broadcast(string msg, BroadCastLoc loc, uint index = 0, ushort _chatType = 2011)
        {
            if (loc == BroadCastLoc.World)
                World.SendMsgToAll("[System]", msg, 2005, 0);

            else if (loc == BroadCastLoc.Map)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, _chatType, 0U));
            }
            else if (loc == BroadCastLoc.Score)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83d, 0));
            }
            else if (loc == BroadCastLoc.Title)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83c, 0));
            }
        }

        /// <summary>
        /// Used to Accept duels received by players
        /// </summary>
        /// <param name="C"></param>
        /// <param name="C2"></param>
        public void AcceptDuel(Character C, Character C2)
        {
            if (C.EventBase == null && (C.ArenaQualifier == null || C.ArenaQualifier.Status == MatchStatus.None) && C2.EventBase == null && (C2.ArenaQualifier == null || C2.ArenaQualifier.Status == MatchStatus.None))
            {
                if (C2.Loc.Map == C.Loc.Map)
                {
                    PlayerList.Add(C.EntityID, C);
                    PlayerList.Add(C2.EntityID, C2);
                    C.Dueler = C2.EntityID;
                    C2.Dueler = C.EntityID;

                    if (Wager)
                    {
                        if (C.Silvers >= WagerAmount && C2.Silvers >= WagerAmount)
                        {
                            C.Silvers -= WagerAmount;
                            C2.Silvers -= WagerAmount;
                        }
                        else
                        {
                            Broadcast("One of the parties doesn't have enough gold to start the duel!", BroadCastLoc.Map, 0, 2000);
                            C.Arena = null;
                            C2.Arena = null;
                            return;
                        }
                    }
                    if (Against == Opponent.Team)
                    {
                        if (C.MyTeam != null && C.TeamLeader && C.MyTeam.Members.Count <= 3 && C2.MyTeam != null && C2.TeamLeader && C2.MyTeam.Members.Count <= 3)
                        {
                            TeamOne = new Dictionary<uint, Character>();
                            TeamTwo = new Dictionary<uint, Character>();
                            foreach (Character C3 in C.MyTeam.Members)
                            {
                                if (!PlayerList.ContainsKey(C3.EntityID))
                                {
                                    C3.Arena = this;
                                    C3.Dueler = C.Dueler;
                                    PlayerList.Add(C3.EntityID, C3);
                                }
                                TeamOne.Add(C3.EntityID, C3);
                            }
                            foreach (Character C3 in C2.MyTeam.Members)
                            {
                                if (!PlayerList.ContainsKey(C3.EntityID))
                                {
                                    C3.Arena = this;
                                    C3.Dueler = C2.Dueler;
                                    PlayerList.Add(C3.EntityID, C3);
                                }
                                TeamTwo.Add(C3.EntityID, C3);
                            }
                        }
                        else
                        {
                            Broadcast("One of the parties didn't have 3 or less members in their teams or the player invited wasn't the team leader!", BroadCastLoc.Map, 0, 2000);
                            C.Arena = null;
                            C2.Arena = null;
                            return;
                        }
                    }

                    Initialize();
                }
                else
                {
                    Broadcast("You and your opponent were not in the same map!", BroadCastLoc.Map, 0, 2000);
                    C.Arena = null;
                    C2.Arena = null;
                }
            }
            else
            {
                Broadcast("Either you or your opponent are in a PVP Event or dueling at the Arena Qualifier!", BroadCastLoc.Map, 0, 2000);
                C.Arena = null;
                C2.Arena = null;
            }
        }

        /// <summary>
        /// Once the duel is accepted by both players, map is created and duel starts
        /// </summary>
        public void Initialize()
        {
            while (DMaps.EventMaps.ContainsKey(MapID))
                MapID++;
            DMaps.CreateDynamicMap(700, MapID, true);
            foreach (Character C in PlayerList.Values.ToList())
            {
                ushort X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                ushort Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                C.Loc.OldMap = C.Loc.Map;
                C.Loc.OldX = C.Loc.X;
                C.Loc.OldY = C.Loc.Y;
                C.Teleport(MapID, X, Y);
                C.MyClient.LocalMessage(2000, "Duel has started! Please type /quitduel if you want to give up!");
                World.SendMsgToAll("SYSTEM", $"{C.Name} Enter the Duel Arena. if you want to watch Door Number is : {C.Loc.Map}", 2000, 0);
                ChangePKMode(C, PKMode.PK);
                C.AddItem(410005);
               



            }
        }

        /// <summary>
        /// Determines what we're supposed to do when someone is shot
        /// </summary>
        /// <param name="User"></param>
        public void Shot(Character User, SkillsClass.SkillInfo Info)
        {
            if (Against == Opponent.Team)
                User.MyTeam.Leader.Shots++;
            else
                User.Shots++;
            if (User.Hit)
            {
                User.Hit = false;
                User.Chains++;
                if (User.Chains > User.MaxChains)
                    User.MaxChains = User.Chains;


                if (User.CountEffect == true)
                {

                    if (User.Chains == 1)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189701").Get);
                    }
                    if (User.Chains == 2)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189702").Get);
                    }
                    if (User.Chains == 3)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189703").Get);
                    }
                    if (User.Chains == 4)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189704").Get);
                    }
                    if (User.Chains == 5)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189705").Get);
                    }
                    if (User.Chains == 6)
                    {
                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189706").Get);
                    }
                    if (User.Chains == 7)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189707").Get);
                    }
                    if (User.Chains == 8)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189708").Get);
                    }
                    if (User.Chains == 9)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189709").Get);
                    }
                    if (User.Chains == 10)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189710").Get);
                    }
                    if (User.Chains == 11)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189711").Get);
                    }
                    if (User.Chains == 12)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189712").Get);
                    }
                    if (User.Chains == 13)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189713").Get);
                    }
                    if (User.Chains == 14)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189714").Get);
                    }
                    if (User.Chains == 15)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189715").Get);
                    }
                    if (User.Chains == 16)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189716").Get);
                    }
                    if (User.Chains == 17)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189717").Get);
                    }
                    if (User.Chains == 18)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189718").Get);
                    }
                    if (User.Chains == 19)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189719").Get);
                    }
                    if (User.Chains == 20)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189720").Get);
                    }
                    if (User.Chains == 21)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189721").Get);
                    }
                    if (User.Chains == 22)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189722").Get);
                    }
                    if (User.Chains == 23)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189723").Get);
                    }
                    if (User.Chains == 24)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189724").Get);
                    }
                    if (User.Chains == 25)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189725").Get);
                    }
                    if (User.Chains == 26)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189726").Get);
                    }
                    if (User.Chains == 27)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189727").Get);
                    }
                    if (User.Chains == 28)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189728").Get);
                    }
                    if (User.Chains == 29)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189729").Get);
                    }
                    if (User.Chains == 30)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189730").Get);
                    }
                    if (User.Chains == 31)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189731").Get);
                    }
                    if (User.Chains == 32)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189732").Get);
                    }
                    if (User.Chains == 33)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189733").Get);
                    }
                    if (User.Chains == 34)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189734").Get);
                    }
                    if (User.Chains == 35)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189735").Get);
                    }
                    if (User.Chains == 36)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189736").Get);
                    }
                    if (User.Chains == 37)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189737").Get);
                    }
                    if (User.Chains == 38)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189738").Get);
                    }
                    if (User.Chains == 39)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189739").Get);
                    }
                    if (User.Chains == 40)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189740").Get);
                    }
                    if (User.Chains == 41)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189741").Get);
                    }
                    if (User.Chains == 42)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189742").Get);
                    }
                    if (User.Chains == 43)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189743").Get);
                    }
                    if (User.Chains == 44)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189744").Get);
                    }
                    if (User.Chains == 45)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189745").Get);
                    }
                    if (User.Chains == 46)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189746").Get);
                    }
                    if (User.Chains == 47)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189747").Get);
                    }
                    if (User.Chains == 48)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189748").Get);
                    }
                    if (User.Chains == 49)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189749").Get);
                    }
                    if (User.Chains == 50)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189750").Get);
                    }
                    if (User.Chains == 51)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189751").Get);
                    }
                    if (User.Chains == 52)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189752").Get);
                    }
                    if (User.Chains == 53)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189753").Get);
                    }
                    if (User.Chains == 54)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189754").Get);
                    }
                    if (User.Chains == 55)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189755").Get);
                    }
                    if (User.Chains == 56)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189756").Get);
                    }
                    if (User.Chains == 57)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189757").Get);
                    }
                    if (User.Chains == 58)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189758").Get);
                    }
                    if (User.Chains == 59)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189759").Get);
                    }
                    if (User.Chains == 60)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189760").Get);
                    }
                    if (User.Chains == 61)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189761").Get);
                    }
                    if (User.Chains == 62)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189762").Get);
                    }
                    if (User.Chains == 63)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189763").Get);
                    }
                    if (User.Chains == 64)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189764").Get);
                    }
                    if (User.Chains == 65)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189765").Get);
                    }
                    if (User.Chains == 66)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189766").Get);
                    }
                    if (User.Chains == 67)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189767").Get);
                    }
                    if (User.Chains == 68)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189768").Get);
                    }
                    if (User.Chains == 69)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189769").Get);
                    }
                    if (User.Chains == 70)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189770").Get);
                    }
                    if (User.Chains == 71)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189771").Get);
                    }
                    if (User.Chains == 72)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189772").Get);
                    }
                    if (User.Chains == 73)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189773").Get);
                    }
                    if (User.Chains == 74)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189774").Get);
                    }
                    if (User.Chains == 75)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189775").Get);
                    }
                    if (User.Chains == 76)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189776").Get);
                    }
                    if (User.Chains == 77)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189777").Get);
                    }
                    if (User.Chains == 78)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189778").Get);
                    }
                    if (User.Chains == 79)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189779").Get);
                    }
                    if (User.Chains == 80)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189780").Get);
                    }
                    if (User.Chains == 81)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189781").Get);
                    }
                    if (User.Chains == 82)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189782").Get);
                    }
                    if (User.Chains == 83)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189783").Get);
                    }
                    if (User.Chains == 84)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189784").Get);
                    }
                    if (User.Chains == 85)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189785").Get);
                    }
                    if (User.Chains == 86)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189786").Get);
                    }
                    if (User.Chains == 87)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189787").Get);
                    }
                    if (User.Chains == 88)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189788").Get);
                    }
                    if (User.Chains == 89)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189789").Get);
                    }
                    if (User.Chains == 90)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189790").Get);
                    }
                    if (User.Chains == 91)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189791").Get);
                    }
                    if (User.Chains == 92)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189792").Get);
                    }
                    if (User.Chains == 93)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189793").Get);
                    }
                    if (User.Chains == 94)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189794").Get);
                    }
                    if (User.Chains == 95)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189795").Get);
                    }
                    if (User.Chains == 96)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189796").Get);
                    }
                    if (User.Chains == 97)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189797").Get);
                    }
                    if (User.Chains == 98)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189798").Get);
                    }
                    if (User.Chains == 99)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "189799").Get);
                    }
                    if (User.Chains == 100)
                    {

                        World.Action(Packets.StringPacket(User.EntityID, StringType.Effect, "1897100").Get);
                    }
                }
            }

            else
                User.Chains = 0;

            if (Type != DuelType.UnlimitedStamina)
                User.Stamina -= Info.StaminaCost;

            DisplayScore();

            if (Count == Hits.Ten && User.Hits >= 10)
            {
                if (World.H_Chars.ContainsKey((User.Dueler)))
                    Reward(User, World.H_Chars[User.Dueler]);
                foreach (Game.Character c in Game.World.H_Chars.Values)
                    if (MapID == c.Loc.Map)
                    {
                        Game.World.Action(User, Packets.GeneralData(User.EntityID, 0, 0, 0, 135).Get);
                        User.Invisible = false;
                        User.Teleport(1002, 430, 380);
                    }

                Finish();
                return;
            }
            else if (Count == Hits.Hundred && User.Hits >= 100)
            {
                if (World.H_Chars.ContainsKey((User.Dueler)))
                    Reward(User, World.H_Chars[User.Dueler]);
                foreach (Game.Character c in Game.World.H_Chars.Values)
                    if (MapID == c.Loc.Map)
                    {
                        Game.World.Action(User, Packets.GeneralData(User.EntityID, 0, 0, 0, 135).Get);
                        User.Invisible = false;
                        User.Teleport(1002, 430, 380);
                        Finish();
                        return;
                    }
            }
        }

        /// <summary>
        /// Determines wether we're supposed to do something when a player gets hitted
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public void Hit(Character Attacker, Character Victim)
        {
            if (Against == Opponent.Team)
            {
                if ((TeamOne.ContainsKey(Attacker.EntityID) && TeamTwo.ContainsKey(Victim.EntityID)) || (TeamTwo.ContainsKey(Attacker.EntityID) && TeamOne.ContainsKey(Victim.EntityID)))
                    Attacker.MyTeam.Leader.Hits++;
            }
            else
                Attacker.Hits++;
            Attacker.Hit = true;
        }

        /// <summary>
        /// Overrides the damage dealt by a certain skill
        /// </summary>
        /// <param name="User"></param>
        /// <param name="C"></param>
        /// <param name="Info"></param>
        /// <returns></returns>
        public uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            if (Type == DuelType.Leech)
            {
                if (C.Stamina >= Info.StaminaCost)
                {
                    if (User.Stamina + Info.StaminaCost > 100)
                        User.Stamina = 100;
                    else
                        User.Stamina += Info.StaminaCost;
                }
            }
            return 1;
        }

        /// <summary>
        /// Announces the winner and rewards in case players are dueling for a wagger
        /// </summary>
        /// <param name="C"></param>
        /// <param name="C2"></param>
        public void Reward(Character C, Character C2)
        {
            if (C != null && C2 != null)
            {
                if (C.Shots > 0 && C2.Shots > 0)
                    Broadcast($"{C.Name} has beat {C2.Name} in a {Type.ToString()} Duel with the following Score: {C.Hits}-{C2.Hits}, Ratio: {((C.Hits * 100) / C.Shots)}%-{((C2.Hits * 100) / C2.Shots)}% and Max Chains: {C.MaxChains}-{C2.MaxChains}", BroadCastLoc.World);
                else if (C.Shots > 0)
                    Broadcast($"{C.Name} has beat {C2.Name} in a {Type.ToString()} Duel with the following Score: {C.Hits}-{C2.Hits}, Ratio: {((C.Hits * 100) / C.Shots)}%-{C2.Shots}% and Max Chains: {C.MaxChains}-{C2.MaxChains}", BroadCastLoc.World);
                else if (C2.Shots > 0)
                    Broadcast($"{C.Name} has beat {C2.Name} in a {Type.ToString()} Duel with the following Score: {C.Hits}-{C2.Hits}, Ratio: {C.Shots}%-{((C2.Hits * 100) / C2.Shots)}% and Max Chains: {C.MaxChains}-{C2.MaxChains}", BroadCastLoc.World);
                else
                    Broadcast($"{C.Name} has beat {C2.Name} in a {Type.ToString()} Duel with the following Score: {C.Hits}-{C2.Hits}, Ratio: {C.Shots}%-{C2.Shots}% and Max Chains: {C.MaxChains}-{C2.MaxChains}", BroadCastLoc.World);

                if (Wager)
                {
                    if (C.Silvers + (WagerAmount * 2) < 2000000000)
                        C.Silvers += WagerAmount * 2;
                    else
                    {
                        C.MyClient.LocalMessage(2000, "WARNING: You can't have more than 2,000,000,000 in your inventory! Please take a screenshot and contact UltimateConquerGM!");
                        World.GMChatAdd += C.Name + " won a total of " + WagerAmount + " but couldn't receive it!";
                    }
                }
            }
        }

        /// <summary>
        /// Called when duel is over, teleports players to old location
        /// </summary>
        public void Finish()
        {
            DisplayScore();
            foreach (Character C in PlayerList.Values.ToList())
            {
                ChangePKMode(C, PKMode.Capture);
                C.Teleport(C.Loc.OldMap, C.Loc.OldX, C.Loc.OldY);
                C.Dueler = 0;
                C.Hits = 0;
                C.Shots = 0;
                C.Chains = 0;
                C.MaxChains = 0;
                C.Hit = false;
                C.Arena = null;

                foreach (Game.Character c in Game.World.H_Chars.Values)
                    if (MapID == c.Loc.Map)
                    {
                        Game.World.Action(c, Packets.GeneralData(c.EntityID, 0, 0, 0, 135).Get);
                        c.Invisible = false;
                        c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                    }

            }

            DMaps.DeleteDynamicMap(MapID, true);
            return;
        }

        /// <summary>
        /// Displays the score inside the map
        /// </summary>
        public void DisplayScore()
        {
            Broadcast("---------Score---------", BroadCastLoc.Title);
            byte count = 1;
            if (Against == Opponent.Single)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {

                    if (C.Hits > 0 && C.Shots > 0)

                        Broadcast($"{C.Name} - Hits: {C.Hits} Ratio: {((C.Hits * 100) / C.Shots)}% Max Chain: {C.MaxChains}", BroadCastLoc.Score, count);
                    else
                        Broadcast($"{C.Name} - Hits: {C.Hits} Ratio: 0% Max Chain: {C.MaxChains}", BroadCastLoc.Score, count);
                    count++;

                }
            }

            else
            {
                foreach (Character C in PlayerList.Values.ToList())
                {

                    if (C.TeamLeader)
                    {
                        if (C.Hits > 0 && C.Shots > 0)
                            Broadcast($"{C.Name} - Hits: {C.Hits} Ratio: {((C.Hits * 100) / C.Shots)}% Max Chain: {C.MaxChains}", BroadCastLoc.Score, count);
                        else
                            Broadcast($"{C.Name} - Hits: {C.Hits} Ratio: 0% Max Chain: {C.MaxChains}", BroadCastLoc.Score, count);
                    }
                    count++;
                }
            }
        }

        /// <summary>
        /// Removes a player from the current match
        /// </summary>
        /// <param name="C"></param>
        public void RemovePlayer(Character C)
        {
            if (Against == Opponent.Single)
            {
                if (World.H_Chars.ContainsKey((C.Dueler)))
                    Reward(World.H_Chars[C.Dueler], C);
                ChangePKMode(C, PKMode.Capture);
                C.Teleport(C.Loc.OldMap, C.Loc.OldX, C.Loc.OldY);
                Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);

                foreach (Game.Character c in Game.World.H_Chars.Values)
                    if (MapID == c.Loc.Map)
                    {

                        Game.World.Action(c, Packets.GeneralData(c.EntityID, 0, 0, 0, 135).Get);
                        c.Invisible = false;
                        c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                    }
                Finish();
            }
            else
            {
                if (C.TeamLeader)
                {
                    if (World.H_Chars.ContainsKey((C.Dueler)))
                        Reward(World.H_Chars[C.Dueler], C);

                    foreach (Game.Character c in Game.World.H_Chars.Values)
                        if (MapID == c.Loc.Map)
                        {
                            Game.World.Action(c, Packets.GeneralData(c.EntityID, 0, 0, 0, 135).Get);
                            c.Invisible = false;
                            c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                        }
                    Finish();
                }
                else
                {
                    if (TeamOne.ContainsKey(C.EntityID))
                        TeamOne.Remove(C.EntityID);
                    else if (TeamTwo.ContainsKey(C.EntityID))
                        TeamTwo.Remove(C.EntityID);
                    if (TeamOne.Count == 0 || TeamTwo.Count == 0)
                    {
                        foreach (Character C2 in TeamOne.Values)
                            if (C2.TeamLeader)
                                Reward(C2, C);
                        foreach (Character C2 in TeamTwo.Values)
                            if (C2.TeamLeader)
                                Reward(C2, C);

                        foreach (Game.Character c in Game.World.H_Chars.Values)
                            if (MapID == c.Loc.Map)
                            {
                                Game.World.Action(c, Packets.GeneralData(c.EntityID, 0, 0, 0, 135).Get);
                                c.Invisible = false;
                                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                            }
                        Finish();
                    }
                    C.Teleport(C.Loc.OldMap, C.Loc.OldX, C.Loc.OldY);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                }
            }
        }

        /// <summary>
        /// Sends a packet to the client that updates the PK Button 
        /// </summary>
        public void ChangePKMode(Character C, PKMode Mode)
        {
            C.PKMode = Mode;
            if (C.MyClient != null)
                C.MyClient.AddSend(Packets.GeneralData(C.EntityID, (uint)Mode, 0, 0, 96));
        }
    }
}