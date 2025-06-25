using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class ElitePK : Events
    {
        bool Fighting = false;
        uint _mapEvent = 10000;
        bool createMap = false;

        private enum LadderStage
        {
            None,
            Pairing,
            Break,
            Countdown,
            Fighting
        }
        private LadderStage CurStage = LadderStage.None;

        public ElitePK()
        {
            EventTitle = "Elite PK";
            Duration = 20;
            BaseMap = 700;
            //NoDamage = true;
            MagicAllowed = true;
            MeleeAllowed = true;
            //AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047 };

            DialogID = 15;
            Features.ElitePKStats.Running = true;
            Features.ElitePKStats.Brackets.Clear();
            Features.ElitePKStats.mapsPairs.Clear();
            Features.ElitePKStats.WaitingList.Clear();
            Features.ElitePKStats.Finish = DateTime.Now.AddHours(3);
            Features.ElitePKStats.First = new Features.ElitePKStats.Rank();
            Features.ElitePKStats.Second = new Features.ElitePKStats.Rank();
            Features.ElitePKStats.Third = new Features.ElitePKStats.Rank();
            Features.ElitePKStats.Fourth = new Features.ElitePKStats.Rank();
        }
        //private readonly List<string> WinnerList = new List<string>();
        private readonly List<uint> Eliminated = new List<uint>();

        public override bool AddPlayer(Character c)
        {
            if (Stage == EventStage.Inviting)
            {
                if (!DMaps.EventMaps.ContainsKey(c.Loc.Map))
                {
                    if (c.ArenaQualifier == null || c.ArenaQualifier.Status == MatchStatus.None /*&& c.Loc.Map != c.ArenaQualifier.MapID*/)
                    {
                        if (c.Loc.Map != 1038 && c.Loc.Map != 6001 && c.Loc.Map != 1844)
                        {
                            if (!PlayerList.ContainsKey(c.EntityID))
                            {
                                if (!c.BOTJailed)
                                {
                                    c.Loc.OldMap = c.Loc.Map;
                                    c.Loc.OldX = c.Loc.X;
                                    c.Loc.OldY = c.Loc.Y;
                                    Random R = new Random();
                                    c.Teleport(2068, (ushort)R.Next(30, 63), (ushort)R.Next(29, 62));
                                    PlayerList.Add(c.EntityID, c);
                                    PlayerScores.Add(c.EntityID, 0);
                                    c.MyClient.LocalMessage(2000, "You have sucessfully joined the " + EventTitle + " Event!");
                                    if (!c.Alive)
                                        RevivePlayer(c, c.MaxHP);
                                    return true;
                                }
                                else
                                    c.MyClient.LocalMessage(2000, "You can't join the event if you're botjailed!");
                            }
                            else
                                c.MyClient.LocalMessage(2000, "You have already joined the " + EventTitle + " Event!");
                        }
                        else
                            c.MyClient.LocalMessage(2000, "You can't join the " + EventTitle + " Event in the current map!");
                    }
                    else
                        c.MyClient.LocalMessage(2000, "You can't join a PVP Event while you're fighting at the Arena Qualifier!");
                }
                else
                    c.MyClient.LocalMessage(2000, "You can't join a PVP Event while you're fighting at the Skill Arena!");
            }
            else
                c.MyClient.LocalMessage(2000, "There are no events running");
            return false;
        }

        public override void Inviting()
        {
            if (CountDown > 0)
            {
                if (CountDown == 120)
                    Broadcast(EventTitle + " Event will start in 2 minutes!", BroadCastLoc.World);
                else if (CountDown == 60)
                    Broadcast(EventTitle + " Event will start in 1 minute!", BroadCastLoc.World);
                else if (CountDown == 10)
                {
                    foreach (Character character in World.H_Chars.Values.ToList())
                    {
                        if (character.Invitations && character.Loc.Map != 1038 && !World.EventsMaps.Contains(character.Loc.Map))
                            character.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                    }
                    if (!CanStart())
                    {
                        Broadcast("The " + EventTitle + " Event requires atleast 2 players to start! Event was cancelled!", BroadCastLoc.World);
                        Broadcast($"---------{EventTitle}---------", BroadCastLoc.Title, 0);
                        Broadcast("Event cancelled", BroadCastLoc.Score, 2);
                        foreach (Character c in PlayerList.Values.ToList())
                        {
                            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                                c.Teleport(1002, 430, 378);
                            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 2068 || c.Loc.OldMap == 1616 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap) || DMaps.EventMaps.ContainsKey(c.Loc.Map))
                                c.Teleport(1002, 430, 378);
                            else
                                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                            c.EventBase = null;
                        }
                        PlayerList.Clear();
                        Stage = EventStage.None;
                        World.Events.Remove(this);
                        return;
                    }
                    Broadcast("10 seconds until start", BroadCastLoc.Map);
                }
                else if (CountDown < 6)
                    Broadcast(CountDown.ToString() + " seconds until start", BroadCastLoc.Map);

                Broadcast($"---------{EventTitle}---------", BroadCastLoc.Title, 0);
                TimeSpan T = TimeSpan.FromSeconds(CountDown);
                Broadcast($"Start in: {T.ToString(@"mm\:ss")}", BroadCastLoc.Score, 2);
                --CountDown;
            }
            else
            {
                foreach (Character C in PlayerList.Values.ToList())
                    if (!C.Alive)
                        RevivePlayer(C, C.MaxHP, false);

                Removeprotection();
                Stage = EventStage.Fighting;
                CurStage = LadderStage.Pairing;
                EndTime = DateTime.Now.AddMinutes(Duration);
                Broadcast(EventTitle + " Tournament has started! May the best player win!", BroadCastLoc.World);
            }
        }

        public override void TeleportPlayersToMap()
        {
            Console.WriteLine("Teleporting players to Map!");
            foreach (Character C in PlayerList.Values.ToList())
            {
                foreach (KeyValuePair<uint, Features.ElitePKStats.Match> M in Features.ElitePKStats.mapsPairs.ToList())
                    if (M.Value.Players.Contains(C.EntityID))
                        SendToMatch(C, M.Key);
            }
        }

        private void SendToMatch(Character C, uint MapID)
        {
            ChangePKMode(C, PKMode.PK);
            C.StatEff.Remove(StatusEffectEn.Fly);
            C.StatEff.Remove(StatusEffectEn.Cyclone);
            C.StatEff.Remove(StatusEffectEn.SuperMan);
            C.CurHP = C.MaxHP;
            C.Protection = true;
            X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
            Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
            //C.EventBase.MapEvent = _mapEvent;
            C.Teleport(MapID, X, Y);
            Console.WriteLine($"Teleported {C.Name} to MapID: {MapID}");
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();

            if (CurStage != LadderStage.Fighting)
                _nextMatch();
            else if (CurStage == LadderStage.Fighting && Features.ElitePKStats.mapsPairs.Count == 0)
            {
                CurStage = LadderStage.Pairing;
            }
        }

        public override void CharacterChecks(Character C)
        {
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                RemovePlayer(C);

            if (Fighting)
                foreach (KeyValuePair<uint, Features.ElitePKStats.Match> M in Features.ElitePKStats.mapsPairs.ToList())
                    if (M.Value.Players.Contains(C.EntityID))
                        if (C.Loc.Map != M.Key && M.Value.Players.Count > 1)
                            RemovePlayer(C);
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            RemovePlayer(Victim);
        }

        //public override void Hit(Character Attacker, Character Victim)
        //{
        //    if (PlayerScores.ContainsKey(Victim.EntityID))
        //    {
        //        if (PlayerScores[Victim.EntityID] < 3)
        //        {
        //            PlayerScores[Victim.EntityID]++;
        //            Victim.MyClient.LocalMessage(2011, "You can only be hitted " + (5 - PlayerScores[Victim.EntityID]) + " more times!");
        //        }
        //        else if (PlayerScores[Victim.EntityID] == 4)
        //        {
        //            PlayerScores[Victim.EntityID]++;
        //            Victim.MyClient.LocalMessage(2011, "You'll be kicked if anyone hits you again! Watch out!");
        //        }
        //        else
        //        {
        //            RemovePlayer(Victim);
        //        }
        //    }
        //}

        private void _nextMatch()
        {
            if (CurStage == LadderStage.Pairing)
            {
                CurStage = LadderStage.Break;
                PairUp();
                CountDown = 30;
            }
            else if (CurStage == LadderStage.Break)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));

                Broadcast("Next round in: " + CountDown + " Seconds!", BroadCastLoc.Score);
                Broadcast("Time left: " + CountDown + " Seconds!", BroadCastLoc.Map);
                CountDown--;
                if (CountDown == 0)
                {
                    CurStage = LadderStage.Countdown;
                    CountDown = 5;
                    TeleportPlayersToMap();
                    Broadcast("Next round of the " + EventTitle + " is about to start!", BroadCastLoc.Map);
                }
            }
            else if (CurStage == LadderStage.Countdown)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, $"downnumber{CountDown}")).Get);
                CountDown--;
                if (CountDown == 0)
                {
                    CurStage = LadderStage.Fighting;
                    foreach (Character C in PlayerList.Values.ToList())
                        C.StatEff.Remove(StatusEffectEn.Dead);
                    Removeprotection();
                }
            }
        }

        private void PairUp()
        {
            //Features.ElitePKStats.DuelHistory.Clear();

            Random r = new Random();
            var Players = PlayerList.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value)/*.Values.ToList()*/;
            foreach (var key in PlayerScores.Keys.ToList())
            {
                PlayerScores[key] = 0;
            }
            if (Eliminated.Count == 2)//Pairs up players for 3rd and 4th Place match
            {
                foreach (uint UID in Eliminated)
                    foreach (Character C in Players.Values)
                        if (C.EntityID == UID)
                            AssignMap(C);
            }
            else if (Features.ElitePKStats.Brackets.Count > 0)//Checks for the left/right bracket of the tournament when there are <= 8 players and pairs up accordingly (Brackets dictionary key corresponds to the position on the bracket so 0-7 for first elimination round, 10-17 2nd elimination round
            {
                for (int a = 0; a < 8; a++)
                {
                    if (Features.ElitePKStats.Brackets.ContainsKey((byte)a))
                    {
                        if (Players.ContainsKey(Features.ElitePKStats.Brackets[(byte)a].UID))
                        {
                            if (Players.Count > 4)
                            {

                                if ((a == 0 || a == 2 || a == 4 || a == 6) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 1)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 10)))
                                        Features.ElitePKStats.Brackets.Add((byte)(a + 10), Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 1 || a == 3 || a == 5 || a == 7) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a - 1)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 10)))
                                        Features.ElitePKStats.Brackets.Add((byte)(a + 10), Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else
                                    AssignMap(Players[Features.ElitePKStats.Brackets[(byte)a].UID]);
                            }
                            else if (Players.Count > 2)
                            {
                                if ((a == 0 || a == 1) && !Features.ElitePKStats.Brackets.ContainsKey(2) && !Features.ElitePKStats.Brackets.ContainsKey(3))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(18))
                                        Features.ElitePKStats.Brackets.Add(18, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 2 || a == 3) && (!Features.ElitePKStats.Brackets.ContainsKey(0) && !Features.ElitePKStats.Brackets.ContainsKey(2)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(19))
                                        Features.ElitePKStats.Brackets.Add(19, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 4 || a == 5) && (!Features.ElitePKStats.Brackets.ContainsKey(6) && !Features.ElitePKStats.Brackets.ContainsKey(7)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(20))
                                        Features.ElitePKStats.Brackets.Add(20, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 6 || a == 7) && (!Features.ElitePKStats.Brackets.ContainsKey(4) && !Features.ElitePKStats.Brackets.ContainsKey(5)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(21))
                                        Features.ElitePKStats.Brackets.Add(21, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else
                                    AssignMap(Players[Features.ElitePKStats.Brackets[(byte)a].UID]);
                            }
                            else
                                AssignMap(Players[Features.ElitePKStats.Brackets[(byte)a].UID]);
                        }
                        else
                        {
                            if ((a == 0 || a == 2 || a == 4 || a == 6) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 11)))
                                Features.ElitePKStats.Brackets.Add((byte)(a + 11), Features.ElitePKStats.Brackets[(byte)a]);
                            else if ((a == 1 || a == 3 || a == 5 || a == 7) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 9)))
                                Features.ElitePKStats.Brackets.Add((byte)(a + 9), Features.ElitePKStats.Brackets[(byte)a]);
                        }
                    }
                }
            }
            else if (Players.Count >= 16)//Pairs up players when there are >= 16 players (so that we get >= 8 players after the fight)
            {
                int TotalMatches = 0;

                bool map = false;
                foreach (Character C in Players.Values)
                {
                    if (!map)
                    {
                        TotalMatches++;
                        map = !map;
                    }
                    else
                        map = !map;

                    if (TotalMatches <= Players.Count / 2)
                        AssignMap(C);
                    else
                        Features.ElitePKStats.WaitingList.Add(C.EntityID);
                }
            }
            else if (Players.Count > 8)//Pairs up when we have > 8 and < 16 players so that we can get the exact number of 8 players after the round
            {
                int Waiting = Players.Count - 8;
                for (int a = 0; a < Players.Count; a++)
                {
                    if (a < Waiting * 2)
                        AssignMap(Players.Values.ToList()[a]);
                }
            }
            else if (Features.ElitePKStats.WaitingList.Count > 0)//Checks the waiting list for players and pairs up accordingly
            {
                for (uint a = 0; a < Features.ElitePKStats.WaitingList.Count; a++)
                {
                    AssignMap(Players[Features.ElitePKStats.WaitingList[(int)a]]);
                    bool Found = false;
                    while (!Found)
                    {
                        foreach (uint UID in Players.Keys)
                        {
                            if (!Features.ElitePKStats.WaitingList.Contains(UID))
                            {
                                bool Fight = false;
                                foreach (Features.ElitePKStats.Match M in Features.ElitePKStats.mapsPairs.Values)
                                    if (M.Players.Contains(UID))
                                    {
                                        Fight = true;
                                        break;
                                    }

                                if (!Fight)
                                {
                                    AssignMap(Players[UID]);
                                    Found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                Features.ElitePKStats.WaitingList.Clear();
            }
            else
            {
                if (Players.Count == 8)
                {
                    for (int a = 0; a < Players.Count; a++)
                    {
                        Character C = Players.Values.ToList()[a];
                        Features.ElitePKStats.Brackets.Add((byte)a, new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name });
                    }
                }
                else
                {
                    byte[] Order = { 0, 4, 2, 6, 1, 5, 3, 7 };
                    byte a = 0;
                    foreach (byte Position in Order)
                    {
                        if (Players.Count > a)
                        {
                            Character C = Players.Values.ToList()[a];
                            Features.ElitePKStats.Brackets.Add(Position, new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name });
                            a++;
                        }
                    }
                }

                for (int a = 0; a < 8; a++)
                {
                    if (Features.ElitePKStats.Brackets.ContainsKey((byte)a))
                    {
                        if (Players.ContainsKey(Features.ElitePKStats.Brackets[(byte)a].UID))
                        {
                            bool Semi = false;

                            if ((a == 0 || a == 2 || a == 4 || a == 6) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a + 1)))
                            {
                                Features.ElitePKStats.Brackets.Add((byte)(a + 10), Features.ElitePKStats.Brackets[(byte)a]);
                                Semi = true;
                            }
                            else if ((a == 1 || a == 3 || a == 5 || a == 7) && !Features.ElitePKStats.Brackets.ContainsKey((byte)(a - 1)))
                            {
                                Features.ElitePKStats.Brackets.Add((byte)(a + 10), Features.ElitePKStats.Brackets[(byte)a]);
                                Semi = true;
                            }
                            else
                                AssignMap(Players[Features.ElitePKStats.Brackets[(byte)a].UID]);

                            if (Semi)
                            {
                                if ((a == 0 || a == 1) && !Features.ElitePKStats.Brackets.ContainsKey(2) && !Features.ElitePKStats.Brackets.ContainsKey(3))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(18))
                                        Features.ElitePKStats.Brackets.Add(18, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 2 || a == 3) && (!Features.ElitePKStats.Brackets.ContainsKey(0) && !Features.ElitePKStats.Brackets.ContainsKey(2)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(19))
                                        Features.ElitePKStats.Brackets.Add(19, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 4 || a == 5) && (!Features.ElitePKStats.Brackets.ContainsKey(6) && !Features.ElitePKStats.Brackets.ContainsKey(7)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(20))
                                        Features.ElitePKStats.Brackets.Add(20, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else if ((a == 6 || a == 7) && (!Features.ElitePKStats.Brackets.ContainsKey(4) && !Features.ElitePKStats.Brackets.ContainsKey(5)))
                                {
                                    if (!Features.ElitePKStats.Brackets.ContainsKey(21))
                                        Features.ElitePKStats.Brackets.Add(21, Features.ElitePKStats.Brackets[(byte)a]);
                                }
                                else
                                    AssignMap(Players[Features.ElitePKStats.Brackets[(byte)a].UID]);
                            }
                        }
                    }
                }
            }
        }

        private void AssignMap(Character C, bool bracket = false)
        {
            createMap = !createMap;
            if (createMap)
            {
                while (DMaps.EventMaps.ContainsKey(_mapEvent) || Features.ElitePKStats.mapsPairs.ContainsKey(_mapEvent))
                    _mapEvent++;

                Console.Write($"Created MapID: {_mapEvent} and joined {C.Name}");

                Features.ElitePKStats.mapsPairs.Add(_mapEvent, new Features.ElitePKStats.Match() { MapID = MapEvent, Players = new List<uint>() { C.EntityID } });
                DMaps.CreateDynamicMap(700, _mapEvent, true);
            }
            else
            {
                Console.WriteLine($" and {C.Name}!");
                Features.ElitePKStats.mapsPairs[_mapEvent].Players.Add(C.EntityID);
            }
        }

        public override void RemovePlayer(Character C, bool exp = true)
        {
            RevivePlayer(C, C.MaxHP);
            uint UID = 0;
            foreach (KeyValuePair<uint, Features.ElitePKStats.Match> M in Features.ElitePKStats.mapsPairs.ToList())
                if (M.Value.Players.Contains(C.EntityID))
                {
                    M.Value.Players.Remove(C.EntityID);
                    UID = M.Value.Players[0];

                    Random R2 = new Random();
                    PlayerList[UID].Teleport(2068, (ushort)R2.Next(30, 63), (ushort)R2.Next(29, 62));

                    Features.ElitePKStats.mapsPairs.Remove(M.Key);
                    DMaps.DeleteDynamicMap(M.Key, true);

                    if (Eliminated.Count != 2)
                    {
                        if (PlayerList.Count > 4 && PlayerList.Count <= 8)
                        {
                            foreach (KeyValuePair<byte, Features.ElitePKStats.Rank> R in Features.ElitePKStats.Brackets.ToList())
                                if (R.Value.UID == UID)
                                    Features.ElitePKStats.Brackets.Add((byte)(R.Key + 10), R.Value);
                        }
                        else if (PlayerList.Count > 2 && PlayerList.Count <= 4)
                        {
                            foreach (KeyValuePair<byte, Features.ElitePKStats.Rank> R in Features.ElitePKStats.Brackets.ToList())
                                if (R.Value.UID == UID)
                                {
                                    if (R.Key == 0 || R.Key == 1)
                                        Features.ElitePKStats.Brackets.Add(18, R.Value);
                                    else if (R.Key == 2 || R.Key == 3)
                                        Features.ElitePKStats.Brackets.Add(19, R.Value);
                                    else if (R.Key == 4 || R.Key == 5)
                                        Features.ElitePKStats.Brackets.Add(20, R.Value);
                                    else if (R.Key == 6 || R.Key == 7)
                                        Features.ElitePKStats.Brackets.Add(21, R.Value);
                                }
                        }
                    }

                    break;
                }

            if (PlayerList.Count == 4 && Eliminated.Count < 2)
            {
                Eliminated.Add(C.EntityID);
                Random R = new Random();
                C.Teleport(2068, (ushort)R.Next(30, 63), (ushort)R.Next(29, 62));
            }
            else if (Eliminated.Count == 2)
            {
                Eliminated.Clear();
                base.RemovePlayer(C, exp);
            }
            else
                base.RemovePlayer(C, exp);

            if (PlayerList.Count == 3)
            {
                Features.ElitePKStats.Fourth = new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name };
                Broadcast(PlayerList[UID].Name + " has defeated " + C.Name + " in the Ladder Tournament and finished in 3rd Place!", BroadCastLoc.World);
                if (PlayerList.ContainsKey(UID))
                    RemovePlayer(PlayerList[UID]);
            }
            else if (PlayerList.Count == 2)
            {
                Features.ElitePKStats.Third = new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name };
            }
            else if (PlayerList.Count == 1)
            {
                Features.ElitePKStats.Second = new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name };
                Broadcast(C.Name + " finished in 2nd Place!", BroadCastLoc.World);
            }
            else if (PlayerList.Count == 0)
            {
                Features.ElitePKStats.First = new Features.ElitePKStats.Rank() { UID = C.EntityID, Face = C.Avatar, Name = C.Name };
                Features.ElitePKStats.Finish = DateTime.Now;
            }
            else
                Broadcast(PlayerList[UID].Name + " has defeated " + C.Name + " in the Ladder Tournament and moved on to the next stage!", BroadCastLoc.World);

            Console.WriteLine($"Removed {C.Name} from the tournament!");
        }

        public override void End()
        {
            //Features.ElitePKStats.Brackets.Clear();
            Features.ElitePKStats.Finish = DateTime.Now.AddMinutes(3);
            Features.ElitePKStats.mapsPairs.Clear();
            Features.ElitePKStats.WaitingList.Clear();
            base.End();
        }

        //public bool IsPowerOfTwo(int x)
        //{
        //    return (x > 0) && ((x & (x - 1)) == 0);
        //}
    }
}
