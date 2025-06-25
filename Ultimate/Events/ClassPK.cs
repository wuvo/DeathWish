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
    public class ClassPK : Events
    {
        DateTime StartsAt;
        public ClassPK()
        {
            EventTitle = "Class PK Tournament";
            Duration = 10;
            MapEvent = 1508;
            NoDamage = false;
            DialogID = 19;
        }

        public string PreFix
        {
            get
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    foreach (Character c in World.H_Chars.Values)
                        c.Top = 0;
                    return "Trojan";
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                {
                    foreach (Character c in World.H_Chars.Values)
                        c.Top = 0;
                    return "Warrior";
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                {
                    foreach (Character c in World.H_Chars.Values)
                        c.Top = 0;
                    return "Archer";
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    foreach (Character c in World.H_Chars.Values)
                        c.Top = 0;
                    return "Fire Taoist";
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    foreach (Character c in World.H_Chars.Values)
                        c.Top = 0;
                    return "Water Taoist";
                }
                return "None";
            }
        }

        public byte Class
        {
            get
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                    return 10;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                    return 20;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                    return 40;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                    return 140;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    return 130;
                return 0;
            }
        }

        /// <summary>
        /// Adds the player to the event
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool AddPlayer(Game.Character c)
        {
            if (c.Job >= Class && c.Job <= (Class + 5))
            {
                if (Stage == EventStage.Inviting || Stage == EventStage.Fighting && CountDown > 0)
                {
                    if (!DMaps.EventMaps.ContainsKey(c.Loc.Map))
                    {
                        if (c.ArenaQualifier == null || c.ArenaQualifier.Status == MatchStatus.None)
                        {
                            if (c.Loc.Map != 1038 && c.Loc.Map != 6001 && c.Loc.Map != 1844 && c.Loc.Map != 1505)
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
            }
            else
                c.MyClient.LocalMessage(2000, "Today isn't the day your class is fighting! Please check the day you are supposed to join and come back!");
            return false;
        }

        public override void BeginTournament()
        {
            World.SendMsgToAll("[EVENT]", PreFix + " " + EventTitle + " Event has started! Type @joinpvp if you want to join!", 2500, 0);
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Invitations && C.EventBase == null && C.Loc.Map != 1038 && !World.EventsMaps.Contains(C.Loc.Map) && !DMaps.EventMaps.ContainsKey(C.Loc.Map) && (C.ArenaQualifier == null || C.ArenaQualifier.Status == MatchStatus.None) && C.Job >= Class && C.Job <= (Class + 5))
                {
                    C.MyClient.AddSend(Packets.ShowDialog(DialogID, 1));
                }
            }
        }

        /// <summary>
        /// Begins the tournament
        /// </summary>
        public override void Inviting()
        {
            World.SendMsgToAll("SYSTEM", PreFix + " " + EventTitle + " has started! Find ClassPKEnvoy in TwinCity before " + (DateTime.Now.Hour + 1).ToString() + ":00 to join!", 2011, 0);
            World.SendMsgToAll("SYSTEM", PreFix + " " + EventTitle + " has started! Find ClassPKEnvoy in TwinCity before " + (DateTime.Now.Hour + 1).ToString() + ":00 to join!", 2500, 0);
            World.SendMsgToAll("SYSTEM", PreFix + " " + EventTitle + " has started! Find ClassPKEnvoy in TwinCity before " + (DateTime.Now.Hour + 1).ToString() + ":00 to join!", 2005, 0);
            World.SendMsgToAll("SYSTEM", PreFix + " " + EventTitle + " has started! Find ClassPKEnvoy in TwinCity before " + (DateTime.Now.Hour + 1).ToString() + ":00 to join!", 2000, 0);
            StartsAt = DateTime.Now.AddSeconds(CountDown);
            Stage = EventStage.Fighting;
        }

        public override bool CanStart()
        {
            return PlayerList.Count >= 1;
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
                Broadcast(Duration + " minutes have passed and no one won the " + PreFix + " " + EventTitle + " Event! Better luck next time!", BroadCastLoc.World);
            else
                Broadcast($"No one has joined the {PreFix} {EventTitle} and so no winner has been found this week!", BroadCastLoc.World);

            foreach (var c in PlayerList.Values.ToList())
                RemovePlayer(c);

            PlayerList.Clear();
            PlayerScores.Clear();
            return;
        }

        public override void Reward(Character c)
        {
            Broadcast(c.Name + " has won the " + PreFix + " " + EventTitle + "!", BroadCastLoc.World);
            c.Silvers += 1500000;

            for (int i = 0; i < 5; i++)
                c.AddItem(1088000);
            if (!World.GoldSource.ContainsKey("ClassPK"))
                World.GoldSource.Add("ClassPK", 0);
            World.GoldSource["ClassPK"] += 1500000;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                c.Top = 3;
                c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopTrojan);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                c.Top = 5;
                c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopWarrior);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                c.Top = 4;
                c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopArcher);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                c.Top = 6;
                c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopFireTaoist);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                c.Top = 7;
                c.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopWaterTaoist);
            }
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