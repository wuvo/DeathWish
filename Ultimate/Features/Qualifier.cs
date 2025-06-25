using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Structures;
using Ultimate.PacketHandling;
using System.Collections.Specialized;

namespace Ultimate.Features
{
    public enum MatchStatus
    {
        None,
        Fighting,
        WaitingForOpponent,
        Countdown,
        Finish
    }
    public enum ButtonType
    {
        TeamPK = 10112,
        ElitePK = 10113,
        Qualifier = 10114,
        SkillPK = 10116,
        Ranking = 10119,
        Store = 10121
    }
    public class QualifierMatch
    {
        //public uint MatchID = 0;
        public uint MapID = 10500;
        //public bool Accepted = false;
        public bool Over = false;
        public Character Opponent = null;
        public Character Winner = null;
        public uint EntityID = 0;
        public uint OpponentID = 0;
        public DateTime NextMatch;
        public DateTime Countdown;
        public MatchStatus Status = MatchStatus.None;

        /// <summary>
        /// Handles match acceptance after pla
        /// </summary>
        public void AcceptMatch(Character C)
        {
            if (Status == MatchStatus.WaitingForOpponent)
                StartMatch(C);
            else if (Status == MatchStatus.Countdown)
                C.ArenaQualifier.Opponent.ArenaQualifier.Status = MatchStatus.WaitingForOpponent;
        }

        /// <summary>
        /// Creates the map event and teleports players inside
        /// </summary>
        public void StartMatch(Character C)
        {
            try
            {
                while (DMaps.EventMaps.ContainsKey(MapID))
                    MapID++;

                DMaps.CreateDynamicMap(700, MapID, true);

                ushort X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21)), Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                C.Loc.OldMap = C.Loc.Map; C.Loc.OldX = C.Loc.X; C.Loc.OldY = C.Loc.Y;
                foreach (Buff B in C.Buffs.Keys)
                    C.RemoveBuff(B);
                if (!C.Alive)
                    RevivePlayer(C);

                C.Teleport(MapID, X, Y);

                //C.MyClient.LocalMessage(2000, "Arena Qualifier match has started! Good luck to both of you!");
                ChangePKMode(C, PKMode.PK);
                Status = MatchStatus.Fighting;

                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21)); Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                C.ArenaQualifier.Opponent.Loc.OldMap = C.ArenaQualifier.Opponent.Loc.Map;
                C.ArenaQualifier.Opponent.Loc.OldX = C.ArenaQualifier.Opponent.Loc.X;
                C.ArenaQualifier.Opponent.Loc.OldY = C.ArenaQualifier.Opponent.Loc.Y;
                foreach (Buff B in C.ArenaQualifier.Opponent.Buffs.Keys)
                    C.ArenaQualifier.Opponent.RemoveBuff(B);
                if (!C.ArenaQualifier.Opponent.Alive)
                    RevivePlayer(C.ArenaQualifier.Opponent);
                C.ArenaQualifier.Opponent.Teleport(MapID, X, Y);
                //C.ArenaQualifier.Opponent.MyClient.LocalMessage(2000, "Arena Qualifier match has started! Good luck to both of you!");
                ChangePKMode(C.ArenaQualifier.Opponent, PKMode.PK);
                C.ArenaQualifier.Opponent.ArenaQualifier.Status = MatchStatus.Fighting;
                C.ArenaQualifier.Opponent.ArenaQualifier.MapID = MapID;
                //C.MyClient.LocalMessage(2000, $"{C.Name} Arena MapID: {MapID} == {C.ArenaQualifier.MapID}");
                //C.MyClient.LocalMessage(2000, $"{C.ArenaQualifier.Opponent.Name} Arena MapID: {C.ArenaQualifier.Opponent.ArenaQualifier.MapID}");
            }
            catch (Exception e)
            {
                if (DMaps.EventMaps.ContainsKey(MapID))
                    foreach (Character C2 in World.H_Chars.Values)
                        if (C.Loc.Map == DMaps.EventMaps[MapID])
                            C.Teleport(1002, 430, 380);
                if (ArenaQualifier.Matches.ContainsKey(EntityID))
                    ArenaQualifier.Matches.Remove(EntityID);
                else if (ArenaQualifier.Matches.ContainsKey(OpponentID))
                    ArenaQualifier.Matches.Remove(OpponentID);

                World.ExcAdd += e + "\r\n";
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

        /// <summary>
        /// Ends the match and puts players on countdown to be teleported
        /// </summary>
        /// <param name="C"></param>
        public void RemovePlayer(Character C, bool Quit = false)
        {
            try
            {
                if (!Features.ArenaQualifier.PlayersInWaiting.ContainsKey(C.EntityID) && C.ArenaQualifier != null)
                {
                    if (!Over && Status != MatchStatus.None)
                    {
                        Over = true;
                        if (C.ArenaQualifier.Opponent.ArenaQualifier != null)
                        {
                            C.ArenaQualifier.Opponent.ArenaQualifier.Over = true;//null error
                            Winner = C.ArenaQualifier.Opponent;
                            if (C.ArenaQualifier.Status == MatchStatus.Fighting)
                            {
                                Winner.ArenaQualifier.Status = MatchStatus.Finish;
                                Winner.ArenaQualifier.ChangePKMode(Winner, PKMode.Capture);
                                if (!Quit)
                                    Winner.ArenaQualifier.NextMatch = DateTime.Now.AddMilliseconds(3000);
                                else
                                    Winner.ArenaQualifier.NextMatch = DateTime.Now.AddMilliseconds(60000);

                                Status = MatchStatus.Finish;
                                ChangePKMode(C, PKMode.Capture);
                                if (!Quit)
                                    NextMatch = DateTime.Now.AddMilliseconds(3000);
                                else
                                    NextMatch = DateTime.Now.AddMilliseconds(60000);

                                if (Features.ArenaQualifier.Matches.ContainsKey(C.EntityID))
                                    Features.ArenaQualifier.Matches[C.EntityID] = this;
                                else if (Features.ArenaQualifier.Matches.ContainsKey(Winner.EntityID))
                                    Features.ArenaQualifier.Matches[Winner.EntityID] = this;
                                if (Quit)
                                    DestroyMatch();
                            }
                            else if (Status != MatchStatus.Finish)
                            {
                                if (ArenaQualifier.Matches.ContainsKey(OpponentID))
                                    ArenaQualifier.Matches.Remove(OpponentID);
                                else if (ArenaQualifier.Matches.ContainsKey(EntityID))
                                    ArenaQualifier.Matches.Remove(EntityID);

                                if (C.ArenaQualifier.Status == MatchStatus.Countdown && Winner.ArenaQualifier.Status == MatchStatus.Countdown && DateTime.Now >= Countdown)
                                {
                                    Winner.MyClient.AddSend(Packets.ShowDialog(24, 0));
                                    Winner.MyClient.AddSend(Packets.ShowDialog(23, 1));
                                    C.MyClient.AddSend(Packets.ShowDialog(24, 0));
                                    C.MyClient.AddSend(Packets.ShowDialog(23, 1));
                                }
                                else if (C.ArenaQualifier.Status == MatchStatus.Countdown && Winner.ArenaQualifier.Status == MatchStatus.Countdown)
                                {
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(24, 0));//Found
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(22, 1));//Won
                                    //C.MyClient.AddSend(Packets.ShowDialog(24, 0));
                                    World.SendMsgToAll("[SYSTEM]", $"{Winner.Name} has defeated {C.Name} in the Arena Qualifier and is currently ranked Nº {MatchStatistics(Winner, C, true)}!", 2005, 0);
                                }
                                else if (C.ArenaQualifier.Status == MatchStatus.WaitingForOpponent && Winner.ArenaQualifier.Status == MatchStatus.Countdown)
                                {
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(24, 0));//Found
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(22, 1));//Won
                                    //C.MyClient.AddSend(Packets.ShowDialog(24, 0));
                                    //C.MyClient.AddSend(Packets.ShowDialog(23, 1));
                                    World.SendMsgToAll("[SYSTEM]", $"{Winner.Name} has defeated {C.Name} in the Arena Qualifier and is currently ranked Nº {MatchStatistics(Winner, C, true)}!", 2005, 0);
                                }
                                else if (C.ArenaQualifier.Status == MatchStatus.Countdown && Winner.ArenaQualifier.Status == MatchStatus.WaitingForOpponent)
                                {
                                    //C.MyClient.AddSend(Packets.ShowDialog(24, 0));//Found
                                    //C.MyClient.AddSend(Packets.ShowDialog(22, 1));//Won
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(24, 0));
                                    //Winner.MyClient.AddSend(Packets.ShowDialog(23, 1));//Lose
                                    World.SendMsgToAll("[SYSTEM]", $"{Winner.Name} has defeated {C.Name} in the Arena Qualifier and is currently ranked Nº {MatchStatistics(Winner, C, true)}!", 2005, 0);
                                }
                                if (C.ArenaQualifier.Opponent != null)
                                    C.ArenaQualifier.Opponent.ArenaQualifier = null;
                                C.ArenaQualifier = null;

                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                    Features.ArenaQualifier.PlayersInWaiting.Remove(C.EntityID);
            }
            catch (Exception e)
            {
                if (DMaps.EventMaps.ContainsKey(MapID))
                    foreach (Character C2 in World.H_Chars.Values)
                        if (C.Loc.Map == DMaps.EventMaps[MapID])
                            C.Teleport(1002, 430, 380);


                if (ArenaQualifier.Matches.ContainsKey(EntityID))
                    ArenaQualifier.Matches.Remove(EntityID);
                else if (ArenaQualifier.Matches.ContainsKey(OpponentID))
                    ArenaQualifier.Matches.Remove(OpponentID);

                World.ExcAdd += e + "\r\n";
            }
        }

        /// <summary>
        /// Destroys the current match, announces winner and register player again
        /// </summary>
        /// <param name="C"></param>
        public void DestroyMatch()
        {
            try
            {
                World.Action(Packets.RemoveButton(20, (int)OpponentID).Get);
                World.Action(Packets.RemoveButton(20, (int)EntityID).Get);
                World.Action(Packets.RemoveButton(20, (int)MapID).Get);

                if (ArenaQualifier.Matches.ContainsKey(EntityID))
                    ArenaQualifier.Matches.Remove(EntityID);
                else if (ArenaQualifier.Matches.ContainsKey(OpponentID))
                    ArenaQualifier.Matches.Remove(OpponentID);
                else
                {
                    if (DMaps.EventMaps.ContainsKey(MapID))
                        foreach (Character C in World.H_Chars.Values)
                            if (C.Loc.Map == DMaps.EventMaps[MapID])
                                C.Teleport(1002, 430, 380);
                }
                Random Rnd = new Random();
                //ArenaQualifier.Matches.Clear();
                if (Winner != null)
                {
                    if (Winner.ArenaQualifier != null && Winner.ArenaQualifier.Opponent != null)
                    {
                        RevivePlayer(Winner.ArenaQualifier.Opponent);//null error
                        if (!DMaps.EventMaps.ContainsKey(Winner.ArenaQualifier.Opponent.Loc.OldMap))
                            Winner.ArenaQualifier.Opponent.Teleport(Winner.ArenaQualifier.Opponent.Loc.OldMap, Winner.ArenaQualifier.Opponent.Loc.OldX, Winner.ArenaQualifier.Opponent.Loc.OldY);
                        else
                            Winner.ArenaQualifier.Opponent.Teleport(1002, 430, 380);
                        Winner.ArenaQualifier.Opponent.MyClient.AddSend(Packets.ShowDialog(23, 1));
                        Winner.ArenaQualifier.Opponent.ArenaQualifier = null;
                        //MatchStatistics(Winner, Winner.ArenaQualifier.Opponent);
                        World.SendMsgToAll("[SYSTEM]", $"{Winner.Name} has defeated {Winner.ArenaQualifier.Opponent.Name} in the Arena Qualifier and is currently ranked Nº {MatchStatistics(Winner, Winner.ArenaQualifier.Opponent)}!", 2005, 0);
                        //World.SendMsgToAll("[SYSTEM]", $"{Winner.Name} has beat {Winner.ArenaQualifier.Opponent.Name} in the Arena Qualifier!", 2005, 0);
                    }
                    RevivePlayer(Winner);
                    if (!DMaps.EventMaps.ContainsKey(Winner.Loc.OldMap))
                        Winner.Teleport(Winner.Loc.OldMap, Winner.Loc.OldX, Winner.Loc.OldY);
                    else
                        Winner.Teleport(1002, 430, 380);

                    Winner.MyClient.AddSend(Packets.ShowDialog(22, 1));
                    Winner.ArenaQualifier = null;
                }
                DMaps.DeleteDynamicMap(MapID, true);
            }
            catch (Exception e)
            {
                if (DMaps.EventMaps.ContainsKey(MapID))
                    foreach (Character C2 in World.H_Chars.Values)
                        if (C2.Loc.Map == DMaps.EventMaps[MapID])
                            C2.Teleport(1002, 430, 380);

                if (ArenaQualifier.Matches.ContainsKey(EntityID))
                    ArenaQualifier.Matches.Remove(EntityID);
                else if (ArenaQualifier.Matches.ContainsKey(OpponentID))
                    ArenaQualifier.Matches.Remove(OpponentID);

                World.ExcAdd += e + "\r\n";
            }
        }

        /// <summary>
        /// Adds the match result to players statistics to be displayed in arena window
        /// </summary>
        /// <param name="Winner"></param>
        /// <param name="Loser"></param>
        /// <returns></returns>
        public byte MatchStatistics(Character Winner, Character Loser, bool Quit = false)
        {
            byte Rank = 0;
            Winner.WinsToday++;
            Winner.WinsTotal++;

            Loser.LossesToday++;
            Loser.LossesTotal++;

            if (!ArenaQualifier.Ranking.ContainsKey(Winner.EntityID))
                ArenaQualifier.Ranking.Add(Winner.EntityID, new ArenaQualifier.YesterdayRank() { Face = Winner.Avatar, Job = Winner.Class, Name = Winner.Name, Level = Winner.Level, Points = 1500});
            if (!ArenaQualifier.Ranking.ContainsKey(Loser.EntityID))
                ArenaQualifier.Ranking.Add(Loser.EntityID, new ArenaQualifier.YesterdayRank() { Face = Loser.Avatar, Job = Loser.Class, Name = Loser.Name, Level = Loser.Level, Points = 1500 });

            if (ArenaQualifier.Ranking[Loser.EntityID].Points > 0)
            {
                if (ArenaQualifier.Ranking[Loser.EntityID].Points >= ArenaQualifier.Ranking[Winner.EntityID].Points)
                {
                    int Points = (int)(ArenaQualifier.Ranking[Loser.EntityID].Points * 0.1);

                    ArenaQualifier.Ranking[Loser.EntityID].Points -= Points;
                    ArenaQualifier.Ranking[Winner.EntityID].Points += Points;
                }
                else
                {
                    int Points = (int)(ArenaQualifier.Ranking[Loser.EntityID].Points * 0.05);

                    ArenaQualifier.Ranking[Loser.EntityID].Points -= Points;
                    ArenaQualifier.Ranking[Winner.EntityID].Points += Points;
                }
            }

            if (Quit)
            {
                Loser.MyClient.AddSend(Packets.ShowDialog(20, 0));
                Loser.MyClient.AddSend(Packets.ShowDialog(24, 0));
                Loser.MyClient.AddSend(Packets.ShowDialog(25, 0));
                Loser.MyClient.AddSend(Packets.ShowDialog(23, 1));

                Winner.MyClient.AddSend(Packets.ShowDialog(20, 0));
                Winner.MyClient.AddSend(Packets.ShowDialog(24, 0));
                Winner.MyClient.AddSend(Packets.ShowDialog(25, 0));
                Winner.MyClient.AddSend(Packets.ShowDialog(22, 1));
            }

            var myList = ArenaQualifier.Ranking.ToList();
            myList.Sort((pair1, pair2) => pair2.Value.Points.CompareTo(pair1.Value.Points));

            var pair = myList.Select((Value, Index) => new { Value.Key, Index })
             .Single(p => p.Key == Winner.EntityID);
            Rank = (byte)(pair.Index + 1);

            return Rank;
        }

        /// <summary>
        /// Used to revive players in different situations
        /// </summary>
        /// <param name="C"></param>
        /// <param name="HP"></param>
        public void RevivePlayer(Character C)
        {
            C.Action = (byte)100;
            C.Stamina = (byte)100;
            C.Ghost = false;
            C.BlueName = false;
            C.CurHP = C.MaxHP;
            if ((int)C.MaxMP > 1)
                C.CurMP = C.MaxMP;
            C.Alive = true;
            C.StatEff.Remove(StatusEffectEn.Dead);
            C.StatEff.Remove(StatusEffectEn.BlueName);
            C.Body = C.Body;
            C.Hair = C.Hair;
            C.XPKO = (byte)0;
            C.ProtectTime.AddSeconds(0);
        }
    }

    public class ArenaQualifier
    {
        public class YesterdayRank
        {
            public string Name = "None";
            public string Job = "None";
            public byte Level;
            public ushort Face;
            public int Points = 1500;
        }
        public static Dictionary<uint, YesterdayRank> Ranking = new Dictionary<uint, YesterdayRank>();
        public static ConcurrentDictionary<uint, Character> PlayersInWaiting = new ConcurrentDictionary<uint, Character>();//Lists players waiting to be paired
        public static Dictionary<uint, QualifierMatch> Matches = new Dictionary<uint, QualifierMatch>();//Lists current matches
        public static YesterdayRank Champion = new YesterdayRank();
        public static YesterdayRank Second = new YesterdayRank();
        public static YesterdayRank Third = new YesterdayRank();

        /// <summary>
        /// Adds a player to the waiting list
        /// </summary>
        /// <param name="C"></param>
        public static void AddPlayer(Character C)
        {
            if (!Ranking.ContainsKey(C.EntityID))
                Ranking.Add(C.EntityID, new ArenaQualifier.YesterdayRank() { Face = C.Avatar, Job = C.Class, Name = C.Name, Level = C.Level, Points = 1500 });

            //PlayersInWaiting.Remove(C.EntityID);
            if (C.EventBase == null)
            {
                if (!Features.ArenaQualifier.PlayersInWaiting.ContainsKey(C.EntityID) && C.ArenaQualifier == null)
                {
                    Features.ArenaQualifier.PlayersInWaiting.TryAdd(C.EntityID, C);
                    Random Rnd = new Random();
                    C.ArenaQualifier = new QualifierMatch() { NextMatch = DateTime.Now.AddMilliseconds((double)(Rnd.Next(1000, 15000))) };
                    //Console.WriteLine($"Added {C.Name} to PlayersInWaiting");
                    //if (C.ArenaQualifier == null)
                    //{
                    //    Random Rnd = new Random();
                    //    C.ArenaQualifier = new QualifierMatch() { NextMatch = DateTime.Now.AddMilliseconds((double)(Rnd.Next(1000, 60000))) };
                    //    C.MyClient.LocalMessage(2000, "You have joined the Arena Qualifier! Please wait until a match comes up!");
                    //}
                }
            }
            else
                C.MyClient.LocalMessage(2000, "You can't join the Arena Qualifier while you're in a PVP Event");
        }

        /// <summary>
        /// Pairs up two players and send them an invitation
        /// </summary>
        /// <param name="user"></param>
        public static void PairUp(Character user)
        {
            //PlayersInWaiting.Clear();
            //var Match =
            //                  from I in PlayersInWaiting.Values
            //                  where (Math.Abs(user.Level - I.Level) < 30 && user.EntityID != I.EntityID && DateTime.Now >= I.ArenaQualifier.NextMatch)
            //                  //orderby Math.Abs(user.Level - I.Level) < 30 descending
            //                  select I;
            if (!PlayersInWaiting.ContainsKey(user.EntityID))
                return;
            List<Character> List = new List<Character>();
            foreach (Character C in PlayersInWaiting.Values)
                if (C.Loc.Map != 1038 && user.EntityID != C.EntityID && DateTime.Now >= C.ArenaQualifier.NextMatch && C.EventBase == null && (C.Arena == null || C.Loc.Map != C.Arena.MapID))
                    List.Add(C);
            if (List.Count > 0)
            {
                Random Rnd = new Random();
                Character opponent = List[Rnd.Next(0, List.Count)];

                //user.ArenaQualifier.MatchID = user.EntityID;
                user.ArenaQualifier.Opponent = opponent;
                user.ArenaQualifier.Status = MatchStatus.Countdown;
                user.ArenaQualifier.Countdown = DateTime.Now.AddMilliseconds(60000);
                user.ArenaQualifier.EntityID = user.EntityID;
                user.ArenaQualifier.OpponentID = opponent.EntityID;
                PlayersInWaiting.Remove(user.EntityID);
                //user.MyClient.LocalMessage(2000, $"Paired with: {opponent.Name}");

                //opponent.ArenaQualifier.MatchID = opponent.EntityID;
                opponent.ArenaQualifier.Opponent = user;
                opponent.ArenaQualifier.Status = MatchStatus.Countdown;
                opponent.ArenaQualifier.Countdown = DateTime.Now.AddMilliseconds(60000);
                opponent.ArenaQualifier.EntityID = opponent.EntityID;
                opponent.ArenaQualifier.OpponentID = user.EntityID;
                PlayersInWaiting.Remove(opponent.EntityID);
                //opponent.MyClient.LocalMessage(2000, $"Paired with: {user.Name}");
                Matches.Add(user.EntityID, user.ArenaQualifier);
                user.MyClient.AddSend(Packets.ShowDialog(24, 1));
                opponent.MyClient.AddSend(Packets.ShowDialog(24, 1));
                //Console.WriteLine($"Removed {user.Name} and {opponent.Name} from PlayersInWaiting and sent them invitations");
            }
        }

        /// <summary>
        /// Sends the arena qualifier information to player window
        /// </summary>
        /// <param name="C"></param>
        public static void WindowInformation(Character C, uint DialogID, byte page = 0)
        {
            if (DialogID == 29)
            {
                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    DlgId = DialogID,
                    TextCount = (byte)(51),
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };

                var myList = ArenaQualifier.Ranking.Values.ToList();
                myList.Sort((pair1, pair2) => pair2.Points.CompareTo(pair1.Points));

                if (myList.Count / 10 < page)
                {
                    C.ArenaPage--;
                    return;
                }

                for (int a = (page * 10); a < (page * 10 + 10); a++)
                {
                    if (myList.Count > a)
                    {
                        MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(1 + a * 10), ypos = (ushort)(70 + ((a - page * 10) * 20)), Color = 0xFFFFFF, Fontsize = 12 };
                        Name.Text = ((int)(a + 1)).ToString(); Name.TextLength = (byte)Name.Text.Length; Name.xpos = 78; Txt.Text.Add(Name);

                        Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a * 10), ypos = (ushort)(70 + ((a - page * 10) * 20)), Color = 0xFFFFFF, Fontsize = 12 };
                        Name.Text = myList[a].Name; Name.TextLength = (byte)Name.Text.Length; Name.xpos = (ushort)(98); Txt.Text.Add(Name);

                        Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a * 10), ypos = (ushort)(70 + ((a - page * 10) * 20)), Color = 0xFFFFFF, Fontsize = 12 };
                        Name.Text = myList[a].Job; Name.TextLength = (byte)Name.Text.Length; Name.xpos = (ushort)(220 - MeasureStringMin(Name.Text, Name.Fontsize)); Txt.Text.Add(Name);

                        Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a * 10), ypos = (ushort)(70 + ((a - page * 10) * 20)), Color = 0xFFFFFF, Fontsize = 12 };
                        Name.Text = myList[a].Level.ToString(); Name.TextLength = (byte)Name.Text.Length; Name.xpos = (ushort)(258 - (Name.TextLength / 2) * 5); Txt.Text.Add(Name);

                        Name = new MSG_DLG_Text.DlgTxtData() { Id = (uint)(2 + a * 10), ypos = (ushort)(70 + ((a - page * 10) * 20)), Color = 0xFFFFFF, Fontsize = 12 };
                        Name.Text = myList[a].Points.ToString(); Name.TextLength = (byte)Name.Text.Length; Name.xpos = (ushort)(310 - (Name.TextLength / 2) * 5); Txt.Text.Add(Name);
                    }
                    else
                        break;
                }

                MSG_DLG_Text.DlgTxtData Page = new MSG_DLG_Text.DlgTxtData() { Id = 50, ypos = 275, Color = 0xFFFFFF, Fontsize = 16 };
                Page.Text = (page + 1).ToString(); Page.TextLength = (byte)Page.Text.Length; Page.xpos = 200; Txt.Text.Add(Page);

                C.MyClient.AddSend(Packets.MsgDlgText(Txt));
                if (page == 0)
                {
                    CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 165, ypos = 270, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                    B.AniId = 10122;
                    B.ButtonUID = B.AniId;
                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                    B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 220, ypos = 270, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                    B.AniId = 10123;
                    B.ButtonUID = B.AniId;
                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                }
            }
            else
            {
                if (Matches.Count < page * 4 && page != 0)
                {
                    page = 0;
                    C.ArenaPage = 0;
                }
                if (Matches.Count <= (page * 4) && (page * 4) > 0)
                    return;

                int amount = 0;

                for (int a = (page * 4); a < (page * 4 + 4); a++)
                {
                    if (Matches.Count > a)
                        amount++;
                    else
                        break;
                }
                //amount = Matches.Count / 4;
                //amount = Matches.Count - (amount * 4);/* - (Matches.Count % 4)*/
                //amount = Matches.Count - amount;
                //if (amount > 0)
                //    amount = Matches.Count - amount;
                //if (amount == 0 && Matches.Count > 1)
                //    amount = 1;
                //else
                //    arrows = true;
                MSG_DLG_IMAGE Img = new MSG_DLG_IMAGE()
                {
                    DlgId = DialogID,
                    ImgCount = (byte)(11 +/* page **/ (amount * 4)),
                    Images = new List<MSG_DLG_IMAGE.DlgImgData>()
                };
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 504, xpos = 76, ypos = 72, Width = 318, Height = 339 });//Information Window
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 505, xpos = 244, ypos = 11, Width = 168, Height = 44 });//Window Heading
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 506, xpos = 400, ypos = 72, Width = 184, Height = 270 });//Matches Window
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = C.Avatar, xpos = 100, ypos = 129, Width = 64, Height = 64 });//My avatar
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Champion.Face, xpos = 207, ypos = 288, Width = 64, Height = 64 });//Champion avatar
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Second.Face, xpos = 100, ypos = 320, Width = 64, Height = 64 });//2ndPlace avatar
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Third.Face, xpos = 309, ypos = 320, Width = 64, Height = 64 });//3rdPlace avatar
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 500, xpos = 85, ypos = 121, Width = 99, Height = 80 });//My face
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 501, xpos = 195, ypos = 276, Width = 84, Height = 77 });//Champion
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 502, xpos = 89, ypos = 316, Width = 84, Height = 77 });//2nd Place
                Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 503, xpos = 299, ypos = 316, Width = 84, Height = 78 });//3rd Place

                for (int a = (page * 4); a < (page * 4 + 4); a++)
                {
                    if (Matches.Count > a)
                    {
                        Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 510, xpos = 403, ypos = (ushort)(111 + (a - (page * 4)) * 50), Width = 178, Height = 56 });//MatchFrame
                        Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = 511, xpos = 477, ypos = (ushort)(121 + (a - (page * 4)) * 50), Width = 24, Height = 20 });//VS

                        Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = World.H_Chars[Matches.ElementAt(a).Value.EntityID].Avatar, xpos = 436, ypos = (ushort)(116 + (a - (page * 4)) * 50), Width = 30, Height = 30 });//Player1
                        Img.Images.Add(new MSG_DLG_IMAGE.DlgImgData() { AniId = Matches.ElementAt(a).Value.Opponent.Avatar, xpos = 514, ypos = (ushort)(116 + (a - (page * 4)) * 50), Width = 30, Height = 30 });//Player2
                    }
                }

                C.MyClient.AddSend(Packets.MsgDlgImage(Img));


                MSG_DLG_Text Txt = new MSG_DLG_Text()
                {
                    DlgId = DialogID,
                    TextCount = (byte)(17 + (amount * 2)),
                    Text = new List<MSG_DLG_Text.DlgTxtData>()
                };

                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 1, xpos = 185, ypos = 115, Color = 0xFFFFFF00, Fontsize = 12, Text = "Ranking:", TextLength = 8 });
                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 2, xpos = 185, ypos = 135, Color = 0xFFFFFF00, Fontsize = 12, Text = "Arena Point:", TextLength = 12 });
                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 3, xpos = 185, ypos = 155, Color = 0xFFFFFF00, Fontsize = 12, Text = "Triumph Today:", TextLength = 14 });
                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 4, xpos = 185, ypos = 175, Color = 0xFFFFFF00, Fontsize = 12, Text = "History Triumph:", TextLength = 16 });
                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 5, xpos = 185, ypos = 195, Color = 0xFFFFFF00, Fontsize = 12, Text = "History Honor:", TextLength = 14 });
                Txt.Text.Add(new MSG_DLG_Text.DlgTxtData() { Id = 6, xpos = 185, ypos = 215, Color = 0xFFFFFF00, Fontsize = 12, Text = "Current Honor:", TextLength = 14 });

                MSG_DLG_Text.DlgTxtData Name = new MSG_DLG_Text.DlgTxtData() { Id = 7, ypos = 204, Color = 0xFFFFFF, Fontsize = 12 };
                Name.Text = C.Name; Name.TextLength = (byte)Name.Text.Length; Name.xpos = (ushort)(131 - (Name.TextLength / 2) * 5); Txt.Text.Add(Name);

                MSG_DLG_Text.DlgTxtData Ranking = new MSG_DLG_Text.DlgTxtData() { Id = 8, xpos = 285, ypos = 115, Color = 0xFFFFFF, Fontsize = 12 };
                if (ArenaQualifier.Ranking.ContainsKey(C.EntityID))
                {
                    var myList = ArenaQualifier.Ranking.ToList();
                    myList.Sort((pair1, pair2) => pair2.Value.Points.CompareTo(pair1.Value.Points));

                    var pair = myList.Select((Value, Index) => new { Value.Key, Index })
                     .Single(p => p.Key == C.EntityID);

                    Ranking.Text = (pair.Index + 1).ToString();
                }
                else
                    Ranking.Text = "None";

                Ranking.TextLength = (byte)Ranking.Text.Length; Txt.Text.Add(Ranking);//Ranking

                MSG_DLG_Text.DlgTxtData ArenaPoints = new MSG_DLG_Text.DlgTxtData() { Id = 9, xpos = 285, ypos = 135, Color = 0xFFFFFF, Fontsize = 12 };
                if (ArenaQualifier.Ranking.ContainsKey(C.EntityID))
                    ArenaPoints.Text = ArenaQualifier.Ranking[C.EntityID].Points.ToString();
                else
                    ArenaPoints.Text = "1500";

                ArenaPoints.TextLength = (byte)ArenaPoints.Text.Length; Txt.Text.Add(ArenaPoints);//ArenaPoints

                MSG_DLG_Text.DlgTxtData TriumphToday = new MSG_DLG_Text.DlgTxtData() { Id = 10, xpos = 285, ypos = 155, Color = 0xFFFFFF, Fontsize = 12 };
                TriumphToday.Text = $"{C.WinsToday}/{C.LossesToday}"; TriumphToday.TextLength = (byte)TriumphToday.Text.Length; Txt.Text.Add(TriumphToday);

                MSG_DLG_Text.DlgTxtData HistoryTriumph = new MSG_DLG_Text.DlgTxtData() { Id = 11, xpos = 285, ypos = 175, Color = 0xFFFFFF, Fontsize = 12 };
                HistoryTriumph.Text = $"{C.WinsTotal}/{C.LossesTotal}"; HistoryTriumph.TextLength = (byte)HistoryTriumph.Text.Length; Txt.Text.Add(HistoryTriumph);

                MSG_DLG_Text.DlgTxtData HistoryHonor = new MSG_DLG_Text.DlgTxtData() { Id = 12, xpos = 285, ypos = 195, Color = 0xFFFFFF, Fontsize = 12 };
                HistoryHonor.Text = C.TotalHonor.ToString(); HistoryHonor.TextLength = (byte)HistoryHonor.Text.Length; Txt.Text.Add(HistoryHonor);

                MSG_DLG_Text.DlgTxtData CurrentHonor = new MSG_DLG_Text.DlgTxtData() { Id = 13, xpos = 285, ypos = 215, Color = 0xFFFFFF, Fontsize = 12 };
                CurrentHonor.Text = C.CurrentHonor.ToString(); CurrentHonor.TextLength = (byte)CurrentHonor.Text.Length; Txt.Text.Add(CurrentHonor);

                MSG_DLG_Text.DlgTxtData ChampionName = new MSG_DLG_Text.DlgTxtData() { Id = 14, ypos = 353, Color = 0xFFFFFF, Fontsize = 15 };
                ChampionName.Text = Champion.Name; ChampionName.TextLength = (byte)ChampionName.Text.Length; ChampionName.xpos = (ushort)(234 - (ChampionName.TextLength / 2) * 6.5); Txt.Text.Add(ChampionName);

                MSG_DLG_Text.DlgTxtData PlaceTwoName = new MSG_DLG_Text.DlgTxtData() { Id = 15, ypos = 390, Color = 0xFFFFFF, Fontsize = 12 };
                PlaceTwoName.Text = Second.Name; PlaceTwoName.TextLength = (byte)PlaceTwoName.Text.Length; PlaceTwoName.xpos = (ushort)(127 - (PlaceTwoName.TextLength / 2) * 5); Txt.Text.Add(PlaceTwoName);

                MSG_DLG_Text.DlgTxtData PlaceThreeName = new MSG_DLG_Text.DlgTxtData() { Id = 16, ypos = 390, Color = 0xFFFFFF, Fontsize = 12 };
                PlaceThreeName.Text = Third.Name; PlaceThreeName.TextLength = (byte)PlaceThreeName.Text.Length; PlaceThreeName.xpos = (ushort)(337 - (PlaceThreeName.TextLength / 2) * 5); Txt.Text.Add(PlaceThreeName);

                MSG_DLG_Text.DlgTxtData Registered = new MSG_DLG_Text.DlgTxtData() { Id = 17, ypos = 353, Color = 0xFFFFFF, Fontsize = 15, xpos = 400, Text = "Total Participants:" };
                Registered.Text += $" {PlayersInWaiting.Count + (Matches.Count * 2)}"; Registered.TextLength = (byte)Registered.Text.Length; Txt.Text.Add(Registered);

                for (int a = (page * 4); a < (page * 4 + 4); a++)
                {
                    if (Matches.Count > a)
                    {
                        MSG_DLG_Text.DlgTxtData Match = new MSG_DLG_Text.DlgTxtData() { Id = (byte)(20 + a), ypos = (ushort)(145 + (a - (page * 4)) * 50), Color = 0xFFFFFF, Fontsize = 12, xpos = 410 };
                        Match.Text = $"{World.H_Chars[Matches.ElementAt(a).Value.EntityID].Name}"; Match.TextLength = (byte)Match.Text.Length; Txt.Text.Add(Match);

                        Match = new MSG_DLG_Text.DlgTxtData() { Id = (byte)(21 + a), ypos = (ushort)(145 + (a - (page * 4)) * 50), Color = 0xFFFFFF, Fontsize = 12, xpos = 515 };
                        Match.Text = $"{Matches.ElementAt(a).Value.Opponent.Name}"; Match.TextLength = (byte)Match.Text.Length; Txt.Text.Add(Match);
                    }
                }

                C.MyClient.AddSend(Packets.MsgDlgText(Txt));

                CustomDialog.DlgBtnData B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 136, xpos = 444, ypos = 378, Height = 37, Width = 136, TipColor = 0, TipStr = "" };

                if (C.ArenaQualifier == null)
                    B.AniId = 10106;
                else
                    B.AniId = 10111;
                B.ButtonUID = B.AniId;

                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 81, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
                B.AniId = (int)ButtonType.ElitePK;
                B.ButtonUID = B.AniId;
                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 262, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
                B.AniId = (int)ButtonType.SkillPK;
                B.ButtonUID = B.AniId;

                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 37, AniWidth = 140, xpos = 444, ypos = 423, Height = 37, Width = 140, TipColor = 0, TipStr = "" };
                B.AniId = (int)ButtonType.TeamPK;
                B.ButtonUID = B.AniId;

                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                B = new CustomDialog.DlgBtnData() { AniHeight = 19, AniWidth = 62, xpos = 325, ypos = 114, Height = 19, Width = 62, TipColor = 0, TipStr = "" };
                B.AniId = (int)ButtonType.Ranking;
                B.ButtonUID = B.AniId;

                C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                if (Champion.Name == C.Name || Second.Name == C.Name || Third.Name == C.Name)
                {
                    B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 48, xpos = 213, ypos = 375, Height = 41, Width = 48, TipColor = 0, TipStr = "" };
                    B.AniId = 10124;
                    B.ButtonUID = B.AniId;

                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                }
                //B = new CustomDialog.DlgBtnData() { AniHeight = 41, AniWidth = 48, xpos = 213, ypos = 375, Height = 41, Width = 48, TipColor = 0, TipStr = "" };
                //B.AniId = 10124;
                //B.ButtonUID = B.AniId;

                //C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                //B = new CustomDialog.DlgBtnData() { AniHeight = 19, AniWidth = 62, xpos = 325, ypos = 194, Height = 19, Width = 62, TipColor = 0, TipStr = "" };
                //B.AniId = (int)ButtonType.Ranking;
                //B.ButtonUID = 10120;

                //C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                //B = new CustomDialog.DlgBtnData() { AniHeight = 19, AniWidth = 62, xpos = 325, ypos = 214, Height = 19, Width = 62, TipColor = 0, TipStr = "" };
                //B.AniId = (int)ButtonType.Store;
                //B.ButtonUID = B.AniId;

                //C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                if (Matches.Count > 0)
                {
                    
                    for (int a = (page * 4); a < (page * 4 + 4); a++)
                    {
                        if (Matches.Count > a)
                        {
                            B = new CustomDialog.DlgBtnData() { AniId = 10117, AniHeight = 17, AniWidth = 44, xpos = 469, ypos = (ushort)(144 + (a - (page * 4)) * 50), Height = 17, Width = 44, TipColor = 0, TipStr = "" };
                            B.ButtonUID = (int)Matches.ElementAt(a).Value.MapID;
                            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                            B = new CustomDialog.DlgBtnData() { AniId = 10118, AniHeight = 25, AniWidth = 24, xpos = 410, ypos = (ushort)(119 + (a - (page * 4)) * 50), Height = 25, Width = 24, TipColor = 0, TipStr = "" };
                            B.ButtonUID = (int)Matches.ElementAt(a).Value.EntityID;
                            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                            B = new CustomDialog.DlgBtnData() { AniId = 10118, AniHeight = 25, AniWidth = 24, xpos = 547, ypos = (ushort)(119 + (a - (page * 4)) * 50), Height = 25, Width = 24, TipColor = 0, TipStr = "" };
                            B.ButtonUID = (int)Matches.ElementAt(a).Value.OpponentID;
                            C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                        }
                    }
                }

                if (/*arrows && */Matches.Count > 4)
                {
                    B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 447, ypos = 310, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                    B.AniId = 10122;
                    B.ButtonUID = B.AniId;
                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));

                    B = new CustomDialog.DlgBtnData() { AniHeight = 33, AniWidth = 26, xpos = 507, ypos = 310, Height = 33, Width = 26, TipColor = 0, TipStr = "" };
                    B.AniId = 10123;
                    B.ButtonUID = B.AniId;
                    C.MyClient.AddSend(Packets.DynamicButton((int)DialogID, B));
                }
            }
        }

        private static float MeasureStringMin(string Text, float Size)
        {
            //set font, size & style
            System.Drawing.Font f = new System.Drawing.Font("Arial", Size);

            //create a bmp / graphic to use MeasureString on
            System.Drawing.Bitmap b = new  System.Drawing.Bitmap(2200, 2200);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);

            //measure the string
            System.Drawing.SizeF sizeOfString = new System.Drawing.SizeF();
            sizeOfString = g.MeasureString(Text, f);

            return sizeOfString.Width / 2;
        }

        public static void ResetRankings()
        {
            Character C = World.CharacterFromName2(Champion.Name);
            if (C != null)
                if (C.Garment != 0)
                    Garment(C, 0, false);
            C = World.CharacterFromName2(Second.Name);
            if (C != null)
                if (C.Garment != 0)
                    Garment(C, 0, false);
            C = World.CharacterFromName2(Third.Name);
            if (C != null)
                if (C.Garment != 0)
                    Garment(C, 0, false);

            Champion = new YesterdayRank();
            Second = new YesterdayRank();
            Third = new YesterdayRank();

            var myList = ArenaQualifier.Ranking.ToList();
            myList.Sort((pair1, pair2) => pair2.Value.Points.CompareTo(pair1.Value.Points));

            int count = myList.Count;
            for (int a = 0; a < count; a++)
            {
                //MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT);
                //Cmd.Select("characters").Where("UID", myList[a].Key);
                //MySQL.MySqlReader Name = new MySQL.MySqlReader(Cmd);
                //while (Name.Read())
                //{
                if (a == 0)
                    Champion = myList[0].Value;
                //{
                //Champion.Name = Name.ReadString("Name");
                //Champion.Face = Name.ReadUInt16("Face");
                //}
                else if (a == 1)
                    Second = myList[1].Value;
                //{
                //Second.Name = Name.ReadString("Name");
                //Second.Face = Name.ReadUInt16("Face");
                //}
                else
                    Third = myList[2].Value;
                //{
                //Third.Name = Name.ReadString("Name");
                //Third.Face = Name.ReadUInt16("Face");
                //}
                //}
                if (a == 2)
                    break;
            }
            Ranking.Clear();

            foreach (KeyValuePair<uint, Character> Char in PlayersInWaiting.ToList())
                Ranking.Add(Char.Key, new ArenaQualifier.YesterdayRank() { Face = Char.Value.Avatar, Job = Char.Value.Class, Name = Char.Value.Name, Level = Char.Value.Level, Points = 1500 });
            

            World.SendMsgToAll("[SYSTEM]", "Arena Qualifier rankings have been reset! A whole new season has begun! Who will be the unbeatable one this time?", 2011, 0);
        }

        public static void Garment(Character C, uint GarmentID, bool Equip)
        {
            if (!Equip)
            {
                C.Garment = 0;
                C.Equips.Send(C.MyClient, false);

                if (C.Equips.Garment.ID == 0)
                    C.MyClient.AddSend(Packets.ItemPacket(0, 9, 6));

                Game.World.Spawn(C, false);
            }
            else
            {
                C.Garment = GarmentID;
                C.MyClient.AddSend(Packets.OverwriteGarment(C.Garment));
                Game.World.Spawn(C, false);
            }
        }

        public static void SaveRankings()
        {
            MySQL.MySqlCommand Del = new MySQL.MySqlCommand(MySQL.MySqlCommandType.TRUNCATE).Truncate("arena");
            Del.Execute();

            foreach (KeyValuePair<uint, YesterdayRank> P in Ranking)
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Cmd.Insert("arena").Insert("UID", P.Key).Insert("Name", P.Value.Name).Insert("Job", P.Value.Job).Insert("Level", P.Value.Level).Insert("Face", P.Value.Face).Insert("Points", P.Value.Points);
                
                if (Champion != null && Champion.Name == P.Value.Name)
                {
                    Cmd.Insert("Rank", 1);
                    Champion = null;
                }
                else if (Second != null && Second.Name == P.Value.Name)
                {
                    Cmd.Insert("Rank", 2);
                    Second = null;
                }
                else if (Third != null && Third.Name == P.Value.Name)
                {
                    Cmd.Insert("Rank", 3);
                    Third = null;
                }
                
                Cmd.Execute();
            }
            if (Champion != null)
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Cmd.Insert("arena").Insert("UID", 1).Insert("Name", Champion.Name).Insert("Job", Champion.Job).Insert("Level", Champion.Level).Insert("Face", Champion.Face).Insert("Points", 0).Insert("Rank", 1).Execute();
            }
            if (Second != null)
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Cmd.Insert("arena").Insert("UID", 2).Insert("Name", Second.Name).Insert("Job", Second.Job).Insert("Level", Second.Level).Insert("Face", Second.Face).Insert("Points", 0).Insert("Rank", 2).Execute();
            }
            if (Third != null)
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                Cmd.Insert("arena").Insert("UID", 3).Insert("Name", Third.Name).Insert("Job", Third.Job).Insert("Level", Third.Level).Insert("Face", Third.Face).Insert("Points", 0).Insert("Rank", 3).Execute();
            }
        }

        public static void LoadRankings()
        {
            MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("arena");
            MySQL.MySqlReader R = new MySQL.MySqlReader(Cmd);

            while (R.Read())
            {
                uint UID = R.ReadUInt32("UID");
                if (!Ranking.ContainsKey(UID) && UID > 3)
                {
                    YesterdayRank M = new YesterdayRank()
                    {
                        Name = R.ReadString("Name"),
                        Job = R.ReadString("Job"),
                        Level = R.ReadByte("Level"),
                        Face = R.ReadUInt16("Face"),
                        Points = R.ReadInt32("Points")
                    };

                    byte Rank = R.ReadByte("Rank");
                    if (Rank == 1)
                        Champion = M;
                    else if (Rank == 2)
                        Second = M;
                    else if (Rank == 3)
                        Third = M;

                    Ranking.Add(UID, M);
                }
                else if (UID <= 3)
                {
                    YesterdayRank M = new YesterdayRank()
                    {
                        Name = R.ReadString("Name"),
                        Job = R.ReadString("Job"),
                        Level = R.ReadByte("Level"),
                        Face = R.ReadUInt16("Face"),
                        Points = R.ReadInt32("Points")
                    };

                    byte Rank = R.ReadByte("Rank");
                    if (Rank == 1)
                        Champion = M;
                    else if (Rank == 2)
                        Second = M;
                    else if (Rank == 3)
                        Third = M;
                }
            }
        }
    }
}
