using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class WeeklyPKTournament : Events
    {
        DateTime StartsAt;
        public WeeklyPKTournament()
        {
            EventTitle = "Weekly PK Tournament";
            Duration = 10;
            MapEvent = 1508;
            NoDamage = false;
            DialogID = 18;
        }

        /// <summary>
        /// Adds the player to the event
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool AddPlayer(Character c)
        {
            if (Stage == EventStage.Inviting || Stage == EventStage.Fighting && CountDown > 0)
            {
                if (!DMaps.EventMaps.ContainsKey(c.Loc.Map))
                {
                    if (c.ArenaQualifier == null || c.ArenaQualifier.Status == MatchStatus.None)
                    {
                        if (c.Loc.Map != 1038 && c.Loc.Map != 6001 && c.Loc.Map != 1844)
                        {
                            if (!PlayerList.ContainsKey(c.EntityID))
                            {
                                if (!c.BOTJailed && c.Alive)
                                {
                                    c.Loc.OldMap = 1002;
                                    c.Loc.OldX = 430;
                                    c.Loc.OldY = 378;
                                    ushort X, Y;
                                    X = (ushort)Program.Rnd.Next(108, 141);
                                    Y = (ushort)Program.Rnd.Next(126, 159);
                                    PlayerList.Add(c.EntityID, c);
                                    c.Teleport(MapEvent, X, Y);
                                    ChangePKMode(c, PKMode.PK);

                                    PlayerScores.Add(c.EntityID, 0);
                                    c.MyClient.LocalMessage(2000, "You have sucessfully joined the " + EventTitle + " Event!");
                                    return true;
                                }
                                else
                                    c.MyClient.LocalMessage(2000, "You can't join the event if you're botjailed or dead!");
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

        /// <summary>
        /// Begins the tournament
        /// </summary>
        public override void Inviting()
        {
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at 22:00! Prepare to fight!", 2011, 0);
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at 22:00! Prepare to fight!", 2005, 0);
            World.SendMsgToAll("SYSTEM", EventTitle + " starts at 22:00! Prepare to fight!", 2000, 0);
            StartsAt = DateTime.Now.AddSeconds(CountDown);
            Stage = EventStage.Fighting;
        }

        /// <summary>
        /// Checks if the event has ended
        /// </summary>
        public override void WaitForWinner()
        {
            if (CountDown > 0)
            {
                Broadcast($"---------{EventTitle}---------", BroadCastLoc.Title, 0);
                TimeSpan T = TimeSpan.FromSeconds(CountDown);
                Broadcast($"Starts in: {T.ToString(@"mm\:ss")}", BroadCastLoc.Score, 2);
                --CountDown;
                if (CountDown == 1)
                {
                    EndTime = DateTime.Now.AddMinutes(Duration);
                    DisplayScores = DateTime.Now;
                    Broadcast($"{EventTitle} has started! Sign ups are now closed! May the strongest player win!", BroadCastLoc.World);
                    DisplayScore();
                }
            }
            else if (DateTime.Now >= EndTime || PlayerList.Count <= 1)
                Finish();
            else if (DateTime.Now >= DisplayScores.AddMilliseconds(1000))
                DisplayScore();
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            RemovePlayer(Victim, false);
        }

        public override void Kill(Companion Attacker, Character Victim)
        {
            RemovePlayer(Victim, false);
        }

        /// <summary>
        /// Teleports everyone out of the map, awards the winner and ends the event
        /// </summary>
        public override void End()
        {
            if (PlayerList.Count == 1)
                foreach (var c in PlayerList.Values.ToList())
                    Reward(c);
            else if (PlayerList.Count > 1)
                Broadcast(Duration + " minutes have passed and no one won the " + EventTitle + " Event! Better luck next time!", BroadCastLoc.World);
            else
                Broadcast($"No one has joined the {EventTitle} and so no winner has been found this week!", BroadCastLoc.World);
            
            foreach (var c in PlayerList.Values.ToList())
                RemovePlayer(c);

            PlayerList.Clear();
            PlayerScores.Clear();
            return;
        }

        public override void Reward(Character c)
        {
            Broadcast(c.Name + " has won the " + EventTitle + "!", BroadCastLoc.World);
            c.Silvers += 5000000;
            if (!World.GoldSource.ContainsKey("WeeklyPK"))
                World.GoldSource.Add("WeeklyPK", 0);

            World.GoldSource["WeeklyPK"] += 5000000;
            int j = Program.Rnd.Next(0, 8);
            uint gem = (uint)(700003 + (j * 10));
            c.AddItem(gem);
            c.Top = 9;
            c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.WeeklyPKChampion);
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            Broadcast($"Players left: {PlayerList.Count}", BroadCastLoc.Score, 2);
        }
    }
}