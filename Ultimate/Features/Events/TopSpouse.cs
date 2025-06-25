using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using NewestCOServer.Game;

namespace NewestCOServer.Features
{
    public class TopSpouse
    {
        public static List<Team> SignupTeams = new List<Team>();
        private static List<Team> SpouseTeams = new List<Team>();
        private static List<Game.Character> TeamMembers = new List<Game.Character>();
        private static List<Team> RemoveTeams = new List<Team>();
        public static bool EventByPM = false;
        public static DateTime Start;
        public static bool CouplesWar;
        public static bool _signup = false;
        private Thread CouplesPK;
        private int _countdown;

        public void BeginTournament()
        {
            SignupTeams.Clear();
            SpouseTeams.Clear();
            TeamMembers.Clear();
            RemoveTeams.Clear();
            _countdown = 300;
            CouplesPK = new Thread((ThreadStart)(() =>
            {
                StartTournament();
                WaitForWinner();
                End();
            }));
            CouplesPK.IsBackground = true;
            CouplesPK.Start();
        }

        public void StartTournament()
        {
            _signup = true;
            World.SendMsgToAll("SYSTEM", "Couples PK Tournament starts at 23:00! Team up with your beloved one and prepare to fight!", 2011, 0);
            World.SendMsgToAll("SYSTEM", "Couples PK Tournament starts at 23:00! Team up with your beloved one and prepare to fight!", 2005, 0);
            World.SendMsgToAll("SYSTEM", "Couples PK Tournament starts at 23:00! Team up with your beloved one and prepare to fight!", 2000, 0);
            while (_countdown > 0)
            {
                if (_countdown % 60 == 0)
                    World.SendMsgToAll("SYSTEM", "Couples PK Tournament starts at 23:00! Team up with your beloved one and prepare to fight!", 2011, 0);
                else if (_countdown == 10)
                {
                    if (!CanStart())
                    {
                        World.SendMsgToAll("SYSTEM", "Couples PK Tournament requires at least 2 Couples to join the fight! The event was cancelled!", 2011, 0);
                        SignupTeams.Clear();
                        _signup = false;
                        CouplesPK.Abort();
                        return;
                    }
                }
                --_countdown;
                Thread.Sleep(1000);
            }
            _signup = false;
            CouplesWar = true;
            StartTourny();
            TeleportPlayersToMap();
            foreach (Team T in SpouseTeams)
                foreach (Character C in T.Members.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber5")).Get);
            Thread.Sleep(1000);
            foreach (Team T in SpouseTeams)
                foreach (Character C in T.Members.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber4")).Get);
            Thread.Sleep(1000);
            foreach (Team T in SpouseTeams)
                foreach (Character C in T.Members.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber3")).Get);
            Thread.Sleep(1000);
            foreach (Team T in SpouseTeams)
                foreach (Character C in T.Members.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber2")).Get);
            Thread.Sleep(1000);
            foreach (Team T in SpouseTeams)
                foreach (Character C in T.Members.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber1")).Get);
            Removeprotection();
            World.SendMsgToAll("TopSpouse", "Top Spouse Tournament has started! Good Luck for everyone!", 2005, 0);
        }

        public bool CanStart()
        {
            return SignupTeams.Count >= 2;
        }

        public void Removeprotection()
        {
            foreach(Team T in SpouseTeams)
                foreach (Character c in T.Members.Values)
                    c.Protection = false;
        }

        public void TeleportPlayersToMap()
        {
            for (int i = 0; i < SignupTeams.Count + SpouseTeams.Count; i++)
            {
                int TakeTeam = Program.Rnd.Next(0, SignupTeams.Count);
                ushort X, Y;

                X = (ushort)Program.Rnd.Next(108, 141);
                Y = (ushort)Program.Rnd.Next(126, 159);
                foreach (Game.Character C in ((Team)(SignupTeams[TakeTeam])).Members.Values)
                {
                    if (!C.Alive)
                    {
                        C.CancelProtectTime = false;
                        C.ProtectTime = DateTime.UtcNow;
                        C.Ghost = false;
                        C.BlueName = false;
                        C.CurHP = C.MaxHP;
                        C.Alive = true;
                        C.StatEff.Remove(NewestCOServer.Game.StatusEffectEn.Dead);
                        C.StatEff.Remove(NewestCOServer.Game.StatusEffectEn.BlueName);
                        C.XPKO = 0;
                        C.Body = C.Body;
                        C.Hair = C.Hair;
                        C.Equips.Send(C.MyClient, false);
                    }
                    else
                        C.CurHP = C.MaxHP;
                    C.Protection = true;
                    TeamMembers.Add(C);
                    C.Teleport(1508, X, Y);
                    X++;
                    Y++;
                }
                SpouseTeams.Add(SignupTeams[TakeTeam]);
                SignupTeams.Remove(SignupTeams[TakeTeam]);
            }
            Start = DateTime.UtcNow;
        }

        public void StartTourny()
        {
            #region TopSpouse
            try
            {
                foreach (Team T in SignupTeams)
                {
                    if (T != null)
                    {
                        if (T.Members != null)
                        {
                            if (T.Members.Count == 2 || T.Leader.MyClient.PM)
                            {
                                foreach (Game.Character C in T.Members.Values)
                                {
                                    if (C.MyClient.Soc.Connected)
                                    {
                                        if (T.Leader != null && C.MyTeam.Leader.EntityID == T.Leader.EntityID)
                                        {
                                            Game.Character Love = Game.World.CharacterFromName2(C.MyClient.MyChar.Spouse);
                                            if (C.MyClient.MyChar.Spouse != null && C.MyClient.MyChar.MyTeam.Members.Values.Contains(Love) || T.Leader.MyClient.PM)
                                                continue;

                                            else
                                            {
                                                RemoveTeams.Add(T);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            RemoveTeams.Add(T);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        RemoveTeams.Add(T);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                RemoveTeams.Add(T);
                            }
                        }
                        else
                        {
                            RemoveTeams.Add(T);
                        }
                    }
                    else
                        RemoveTeams.Add(T);
                }
                foreach (Team T in RemoveTeams)
                {
                    if (T != null)
                        if (T.Members != null)
                            T.Leader.MyClient.LocalMessage(2000, "Your team was removed from the Couple PK Tournament because it didn't meet the requirements!");
                    SignupTeams.Remove(T);
                }
            }
            catch (Exception E) { Console.WriteLine(E.ToString()); World.ExcAdd += E + "\r\n"; }
            #endregion
        }

        public void WaitForWinner()
        {
            while (true)
            {
                foreach (Team T in SpouseTeams)
                {
                    if (T != null)
                        if (T.Members != null)
                        {
                            bool Alive = false;
                            foreach (Game.Character C in T.Members.Values)
                            {
                                if (C.MyClient.Soc.Connected && C.Alive && C.Loc.Map == 1508 && C.MyTeam != null)
                                    Alive = true;
                            }
                            if (T.Members.Count != 2 && !T.Leader.MyClient.PM)
                                Alive = false;

                            if (!Alive)
                                RemoveTeams.Add(T);
                        }
                        else
                            RemoveTeams.Add(T);

                    else
                        RemoveTeams.Add(T);
                }
                foreach (Team T in RemoveTeams)
                {
                    SpouseTeams.Remove(T);
                    if (T.Members != null)
                        foreach (Character C in T.Members.Values)
                        {
                            TeamMembers.Remove(C);
                            C.Teleport(1002, 430, 378);
                        }
                }

                if (SpouseTeams.Count <= 1)
                    break;

                if (DateTime.UtcNow >= Start.AddMinutes(10))
                    break;
            }
        }

        public void End()
        {
            if (SpouseTeams.Count > 1)
            {
                foreach (Game.Character C in TeamMembers)
                {
                    C.Teleport(1002, 430, 378);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                }
                CouplesWar = false;
                _signup = false;
                Game.World.SendMsgToAll("TopSpouse", "10 minutes have passed, the Couples PK Tournament has come to an end! There are no winners this time!", 2005, 0);
                TeamMembers.Clear();
                SpouseTeams.Clear();
                SignupTeams.Clear();
                RemoveTeams.Clear();
                CouplesPK.Abort();
                return;
            }
            foreach (Team T in SpouseTeams)
            {
                if (SpouseTeams.Count == 1)
                {
                    if (MyMath.ChanceSuccess(100))
                    {
                        T.Leader.DBScrolls += 1;
                        T.Leader.MyClient.LocalMessage(2000, "Please check Prize NPC in market in order to pick up your DBScroll!");
                        Game.World.SendMsgToAll("TeamPK", T.Leader.Name + "'s team has won the Top Spouse Tournament and received a DBScroll!", 2011, 0);
                    }
                }
            }
            foreach (Game.Character C in TeamMembers)
            {
                C.Teleport(1002, 427, 379);
                Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                if (!C.Alive)
                {
                    #region Revive
                    C.Protection = false;
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
                    #endregion
                }
            }
            CouplesWar = false;
            _signup = false;
            TeamMembers.Clear();
            SpouseTeams.Clear();
            SignupTeams.Clear();
            RemoveTeams.Clear();
            CouplesPK.Abort();
            return;
        }
    }
}
