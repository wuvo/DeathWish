using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Ultimate.Events
{
    public class LadderTournament : Events
    {
        bool start = false;
        bool nextRound = false;
        public LadderTournament()
        {
            EventTitle = "Skill Ladder";
            Duration = 20;
            BaseMap = 1507;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 15;
        }
        private readonly Dictionary<uint, List<uint>> mapsPairs = new Dictionary<uint, List<uint>>();
        private readonly List<string> WinnerList = new List<string>();

        public override void RemovePlayer(Character C, bool exp = true)
        {
            foreach (KeyValuePair<uint, List<uint>> Map in mapsPairs.ToList())
                if (Map.Value.Contains(C.EntityID))
                {
                    Map.Value.Remove(C.EntityID);
                    Broadcast(PlayerList[Map.Value[0]].Name + " has defeated " + C.Name + " in the Ladder Tournament and moved on to the next stage!", BroadCastLoc.World);

                    PlayerList[Map.Value[0]].Teleport(1616, 53, 65);
                    WinnerList.Add(PlayerList[Map.Value[0]].Name);
                    mapsPairs.Remove(Map.Key);
                    DMaps.DeleteDynamicMap(Map.Key, true);
                    break;
                }
            base.RemovePlayer(C, exp);
        }

        public override void TeleportPlayersToMap()
        {
            WinnerList.Clear();
            uint _mapEvent = 10000;
            byte _count = 0;
            int number = PlayerList.Count;
            Random r = new Random();
            foreach (Character C in PlayerList.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value).Values.ToList())
            {
                ChangePKMode(C, PKMode.PK);
                C.StatEff.Remove(StatusEffectEn.Fly);
                C.StatEff.Remove(StatusEffectEn.Cyclone);
                C.StatEff.Remove(StatusEffectEn.SuperMan);
                C.CurHP = C.MaxHP;
                C.Protection = true;
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                C.EventBase.MapEvent = _mapEvent;
                if (_count == 0)
                {
                    if (number == 1)
                        WinnerList.Add(C.Name);
                    else
                    {
                        DMaps.CreateDynamicMap(700, _mapEvent, true);
                        mapsPairs.Add(_mapEvent, new List<uint>());
                        mapsPairs[_mapEvent].Add(C.EntityID);
                        C.Teleport(_mapEvent, X, Y);
                        _count++;
                    }
                }
                else
                {
                    mapsPairs[_mapEvent].Add(C.EntityID);
                    C.Teleport(_mapEvent, X, Y);
                    _count = 0;
                    _mapEvent++;
                    number = number - 2;
                }
            }
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();

            if (DateTime.Now >= DisplayScores.AddMilliseconds(2000))
                DisplayScore();
            if (mapsPairs.Count == 0 || start)
            {
                if (!nextRound && !start)
                {
                    CountDown = 30;
                    nextRound = true;
                }
                else if (CountDown == 0 && mapsPairs.Count == 0)
                    nextRound = false;
                _nextMatch();
            }
        }

        public override void CharacterChecks(Character C)
        {
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                RemovePlayer(C);
            foreach (KeyValuePair<uint, List<uint>> M in mapsPairs.ToList())
                if (M.Value.Contains(C.EntityID))
                    if (C.Loc.Map != M.Key)
                        RemovePlayer(C);
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Victim.EntityID))
            {
                if (PlayerScores[Victim.EntityID] < 8)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You can only be hitted " + (10 - PlayerScores[Victim.EntityID]) + " more times!");
                }
                else if (PlayerScores[Victim.EntityID] == 8)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You'll be kicked if anyone hits you again! Watch out!");
                }
                else
                {
                    RemovePlayer(Victim);
                    //foreach (KeyValuePair<uint, List<uint>> Map in mapsPairs.ToList())
                    //    if (Map.Value.Contains(Victim.EntityID))
                    //        Map.Value.Remove(Victim.EntityID);
                    //Broadcast(Attacker.Name + " has defeated " + Victim.Name + " in the Ladder Tournament and moved on to the next stage!", BroadCastLoc.World);
                }
            }
        }

        private void _nextMatch()
        {
            if (CountDown > 0 && !start)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));

                Broadcast("Time left: " + CountDown + " Seconds!", BroadCastLoc.Map);
                CountDown--;
            }
            else if (!start)
            {
                TeleportPlayersToMap();
                Broadcast("Next round of the " + EventTitle + " is about to start!", BroadCastLoc.Map);
                start = true;
                CountDown = 5;
            }
            else if (CountDown > 0)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, $"downnumber{CountDown}")).Get);
                CountDown--;
            }
            else
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.StatEff.Remove(StatusEffectEn.Dead);
                Removeprotection();
                start = false;
            }
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            byte Score = 2;
            byte Rank = 1;
            foreach (Character C in PlayerList.Values.ToList())
                C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            foreach (KeyValuePair<uint, List<uint>> M in mapsPairs.ToList())
            {
                if (M.Value.Count == 2)
                {
                    Broadcast($"{PlayerList[M.Value[0]].Name} - {(10 - PlayerScores[M.Value[0]])} VS {PlayerList[M.Value[1]].Name} - {(10 - PlayerScores[M.Value[1]])}", BroadCastLoc.Score, Score);
                    Score++;
                }
            }
            foreach (string Name in WinnerList.ToList())
            {
                Broadcast($"Nº {Rank}: {Name}", BroadCastLoc.Score, Score);
                Rank++;
            }
        }
    }
}