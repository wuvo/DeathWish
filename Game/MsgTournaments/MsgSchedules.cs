using DeathWish.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeathWish.Game.MsgEvents;

namespace DeathWish.Game.MsgTournaments
{
    public class MsgSchedules
    {
        public static Dictionary<TournamentType, ITournament> Tournaments = new Dictionary<TournamentType, ITournament>();
        public static ITournament CurrentTournament;
        internal static MsgMissConquer MissConquer;
        internal static MsgMrConquer MrConquer;
        internal static MsgGuildWar GuildWar;
        internal static MsgPoleDomination PoleDomination;
        internal static MsgSuperGuildWar SuperGuildWar;
        internal static MsgArena Arena;
        internal static MsgPkWar PkWar;
        internal static MsgCouples CouplesPKWar;
        internal static DragonWar DragonWar;
        internal static LastManStand LastManStand;
        internal static KungfuSchool KungfuSchool;
        internal static LuckyDraw LuckyDraw;
        internal static MsgTeamArena TeamArena;
        internal static MsgClassPKWar ClassPkWar;
        internal static MsgEliteTournament ElitePkTournament;
        internal static MsgTeamPkTournament TeamPkTournament;
        internal static MsgSkillTeamPkTournament SkillTeamPkTournament;
        internal static MsgCaptureTheFlag CaptureTheFlag;
        internal static MsgTowerOfMystery TowerOfMystery;
        internal static MsgDisCity DisCity;
        internal static MsgPowerArena PowerArena;
        internal static MsgTOPS TOPS;
        internal static MsgChristmasAnimation ChristmasAnimation;
        internal static MsgEliteGuildWar EliteGuildWar;
        internal static MsgEliteGuildWar1st EliteGuildWar1st;
        internal static MsgEliteGuildWar2nd EliteGuildWar2nd;
        internal static MsgEliteGuildWar3rd EliteGuildWar3rd;
        internal static MsgUnionWar UnionWar;
        internal static MsgSmallGuildWar SmallGuildWar;
        internal static MsgFightersPole1 FightersPole1;
        internal static MsgFightersPole2 FightersPole2;
        internal static MsgFightersPole3 FightersPole3;
        internal static MsgFightersPole4 FightersPole4;
        internal static MsgSquidwardOctopus SquidwardOctopus;
        internal static MsgDragonIsland DragonIsland;
        internal static MsgMonthlyPK MonthlyPK;
        internal static MsgWeeklyPK WeeklyPK;
        internal static MsgSteedRace SteedRace;

        public static object ClanWar { get; internal set; }

        internal static void Create()
        {
            Tournaments.Add(TournamentType.None, new MsgNone(TournamentType.None));
            Tournaments.Add(TournamentType.BattleField, new MsgBattleField(TournamentType.BattleField));
            Tournaments.Add(TournamentType.DBShower, new MsgDBShower(TournamentType.DBShower));
            Tournaments.Add(TournamentType.TeamDeathMatch, new MsgTeamDeathMatch(TournamentType.TeamDeathMatch));
           // Tournaments.Add(TournamentType.LastManStand, new MsgLastMan(TournamentType.LastManStand));
          //  Tournaments.Add(TournamentType.DragonWar, new MsgDragonWar(TournamentType.DragonWar));
            Tournaments.Add(TournamentType.ExtremePk, new MsgExtremePk(TournamentType.ExtremePk));
            Tournaments.Add(TournamentType.TopFight, new MsgTopFight(TournamentType.TopFight));
            Tournaments.Add(TournamentType.BettingCPs, new MsgBettingCompetition(TournamentType.BettingCPs));
            Tournaments.Add(TournamentType.KillTheCaptain, new MsgKillTheCaptain(TournamentType.KillTheCaptain));
            Tournaments.Add(TournamentType.KillerOfElite, new MsgTheKillerOfElite(TournamentType.KillerOfElite));
            Tournaments.Add(TournamentType.TreasureThief, new MsgTreasureThief(TournamentType.TreasureThief));
            Tournaments.Add(TournamentType.FootBall, new MsgFootball(TournamentType.FootBall));
            Tournaments.Add(TournamentType.FreezeWar, new MsgFreezeWar(TournamentType.FreezeWar));
            Tournaments.Add(TournamentType.KingOfTheHill, new MsgKingOfTheHill(TournamentType.KingOfTheHill));
            Tournaments.Add(TournamentType.SkillTournament, new MsgSkillTournament(TournamentType.SkillTournament));
            Tournaments.Add(TournamentType.QuizShow, new MsgQuizShow(TournamentType.QuizShow));
            ChristmasAnimation = new MsgChristmasAnimation();
            CurrentTournament = Tournaments[TournamentType.None];
            MrConquer = new MsgMrConquer();
            MissConquer = new MsgMissConquer();
            PowerArena = new MsgPowerArena();
            SquidwardOctopus = new MsgSquidwardOctopus();
            SuperGuildWar = new MsgSuperGuildWar();
            GuildWar = new MsgGuildWar();
            EliteGuildWar = new MsgEliteGuildWar();
            DragonWar = new DragonWar();
            LastManStand = new LastManStand();
            KungfuSchool = new KungfuSchool();
            LuckyDraw = new LuckyDraw();
            EliteGuildWar1st = new MsgEliteGuildWar1st();
            EliteGuildWar2nd = new MsgEliteGuildWar2nd();
            EliteGuildWar3rd = new MsgEliteGuildWar3rd();

            UnionWar = new MsgUnionWar();
            SmallGuildWar = new MsgSmallGuildWar();
            Arena = new MsgArena();
            TeamArena = new MsgTeamArena();
            FightersPole1 = new MsgFightersPole1();
            FightersPole2 = new MsgFightersPole2();
            FightersPole3 = new MsgFightersPole3();
            FightersPole4 = new MsgFightersPole4();
            ClassPkWar = new MsgClassPKWar(ProcesType.Dead);
            PoleDomination = new MsgPoleDomination();
            ElitePkTournament = new MsgEliteTournament();
            CaptureTheFlag = new MsgCaptureTheFlag();
            WeeklyPK = new MsgWeeklyPK();
            PkWar = new MsgPkWar();
            CouplesPKWar = new MsgCouples();
            MonthlyPK = new MsgMonthlyPK();
            TOPS = new MsgTOPS();
            PowerArena = new MsgPowerArena();
            DisCity = new MsgDisCity();
            DragonIsland = new MsgDragonIsland(ProcesType.Dead);
            TowerOfMystery = new MsgTowerOfMystery();
            SteedRace = new MsgSteedRace();
            TeamPkTournament = new MsgTeamPkTournament();
            SkillTeamPkTournament = new MsgSkillTeamPkTournament();
            MsgBroadcast.Create();
        }
        internal static void SendInvitation(string Name, string Prize, ushort X, ushort Y, ushort map, ushort DinamicID, int Secounds, Game.MsgServer.MsgStaticMessage.Messages messaj = Game.MsgServer.MsgStaticMessage.Messages.None)
        {
            string Message = "[ " + Name + " ] Is About To Begin! Will You Join It? Prize [" + Prize + "] .";
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                var packet = new Game.MsgServer.MsgMessage(Message, MsgServer.MsgMessage.MsgColor.yellow, MsgServer.MsgMessage.ChatMode.Center).GetArray(stream);
                foreach (var client in Database.Server.GamePoll.Values)
                {
                    if (!client.Player.OnMyOwnServer || client.IsConnectedInterServer())
                        continue;
                    if (client.Player.Map == 1858 || client.Player.Map == 1038 || client.Player.Map == 2038)
                        continue;
                    if (client.Player.Map == 6004 || client.Player.Map == 7000)
                        continue;
                    #region Block SendInvitation
                    if (Program.BlockInvitation.Contains(client.Player.Map) || (Game.AISystem.UnlimitedArenaRooms.Maps.ContainsValue(client.Player.DynamicID) && client.Player.fbss == 1))
                        continue;
                    #endregion
                    if (client.Player.ContainFlag(MsgUpdate.Flags.Ghost))
                        continue;
                    client.Send(packet);
                    client.Player.MessageBox(Message, new Action<Client.GameClient>(user => user.Teleport(X, Y, map, DinamicID)), null, Secounds, messaj);
                }
            }
        }
        internal unsafe static void SendSysMesage(string Messaj, Game.MsgServer.MsgMessage.ChatMode ChatType = Game.MsgServer.MsgMessage.ChatMode.TopLeft, Game.MsgServer.MsgMessage.MsgColor color = Game.MsgServer.MsgMessage.MsgColor.red, bool SendScren = false)
        {
            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                var packet = new Game.MsgServer.MsgMessage(Messaj, color, ChatType).GetArray(stream);
                foreach (var client in Database.Server.GamePoll.Values)
                    client.Send(packet);
            }
        }
        internal static void CheckUp()
        {
            DateTime Now64 = DateTime.Now;
            #region HORUS Boss
            //if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 05 && DateTime.Now.Second < 1)
            //{
            //    var map = Database.Server.ServerMaps[3030];
            //    if (!map.ContainMobID(60915))
            //    {

            //        using (var rec = new ServerSockets.RecycledPacket())
            //        {
            //            var stream = rec.GetStream();
            //            Database.Server.AddMapMonster(stream, map, 60915, 350, 350, 0, 0, 1);
            //            string Messaj = "Trojan appeared! In HORUS-Event at X = 350, Y = 350 Defeat it To Get [ 2500 ] D-Points!";
            //            Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage(Messaj, Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));
            //            foreach (var user in Database.Server.GamePoll.Values)
            //                if (user.Player.Map == 3030)
            //                {
            //                    user.Player.MessageBox("Trojan appeared! In HORUS-Event Want Go ?", new Action<Client.GameClient>(p => { p.Teleport(350, 350, 3030); }), null, 120);
            //                }
            //        }
            //    }
            //}

            //if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 15 && DateTime.Now.Second < 1)
            //{
            //    var map = Database.Server.ServerMaps[3030];
            //    if (!map.ContainMobID(60915))
            //    {

            //        using (var rec = new ServerSockets.RecycledPacket())
            //        {
            //            var stream = rec.GetStream();
            //            Database.Server.AddMapMonster(stream, map, 60915, 350, 350, 0, 0, 1);
            //            string Messaj = "Trojan appeared! In HORUS-Event at X = 350, Y = 350 Defeat it To Get [ 2500 ] D-Points!";
            //            Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage(Messaj, Game.MsgServer.MsgMessage.MsgColor.red, Game.MsgServer.MsgMessage.ChatMode.Center).GetArray(stream));
            //            foreach (var user in Database.Server.GamePoll.Values)
            //                if (user.Player.Map == 3030)
            //                {
            //                    user.Player.MessageBox("Trojan appeared! In HORUS-Event Want Go ?", new Action<Client.GameClient>(p => { p.Teleport(350, 350, 3030); }), null, 120);
            //                }
            //        }
            //    }
            //}
            #endregion
            if (!Database.Server.FullLoading)
                return;
            if (Arena.Proces == ProcesType.Dead)
            {
                Arena.Proces = ProcesType.Alive;
            }
            if (DragonIsland.Process == ProcesType.Dead)
            {
                DragonIsland.Process = ProcesType.Idle;
            }
            try
            { 
                #region CheckEvents
                MsgArcherClass.CheckUP();
                MsgDragonClass.CheckUP();
                MsgFireClass.CheckUP();
                MsgMonkClass.CheckUP();
                MsgNinjaClass.CheckUP();
                MsgPirateClass.CheckUP();
                MsgTrojanClass.CheckUP();
                MsgWarriorClass.CheckUP();
                MsgWaterClass.CheckUP();
                MsgWindClass.CheckUP();
                MsgEmperorWar.CheckUP();
                SteedRace.work(0);
                MsgVeteransWar.CheckUP();
                MsgWarOfPlayers.CheckUP();
                SquidwardOctopus.CheckUp();
                MsgNobilityPole.CheckUP();
                MsgNobilityPole1.CheckUP();
                MsgNobilityPole2.CheckUP();
                MsgNobilityPole3.CheckUP();
                FightersPole1.CheckUP();
                FightersPole2.CheckUP();
                FightersPole3.CheckUP();
                FightersPole4.CheckUP();
                MsgGuildPole.CheckUP();
                MsgGuildPole1.CheckUP();
                MsgGuildPole2.CheckUP();
                PkWar.CheckUp();
                CouplesPKWar.CheckUp();
                MsgSchedules.EliteGuildWar.CheckUP();
                MsgSchedules.EliteGuildWar1st.CheckUP();
                MsgSchedules.EliteGuildWar2nd.CheckUP();
                MsgSchedules.EliteGuildWar3rd.CheckUP();

                CurrentTournament.CheckUp();
                #endregion
                #region Events list
                if (Program.Events != null)
                    foreach (Game.MsgEvents.Events E in Program.Events.ToList())
                        E.ActionHandler();
                #endregion



                #region Top1
                if(Now64.Minute == 12 && Now64.Second == 01)
                {
                    SendInvitation("Top1 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top1 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                
                #region Top2
                if (Now64.Minute == 16 && Now64.Second == 01)
                {
                    SendInvitation("Top2 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top2 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                #region Top3
                if (Now64.Minute == 20 && Now64.Second == 01)
                {
                    SendInvitation("Top3 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top3 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                
                #region Top4
                if (Now64.Minute == 24 && Now64.Second == 01)
                {
                    SendInvitation("Top4 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top4 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                
                #region Top5
                if (Now64.Minute == 28 && Now64.Second == 01)
                {
                    SendInvitation("Top5 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top5 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                
                #region Top6
                if (Now64.Minute == 32 && Now64.Second == 01)
                {
                    SendInvitation("Top6 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top6 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                
                #region Top7
                if (Now64.Minute == 36 && Now64.Second == 01)
                {
                    SendInvitation("Top7 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top7 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                #region Top8
                if (Now64.Minute == 40 && Now64.Second == 01)
                {
                    SendInvitation("Top8 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top8 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                #region Top8
                if (Now64.Minute == 44 && Now64.Second == 01)
                {
                    SendInvitation("Top8 Has begin want join ?", "Cps", 451, 377, 1002, 0, 60);
                    MsgSchedules.SendSysMesage("Top8 hast started Now !.", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                }
                #endregion
                #region 20:00
                if (Now64.Hour == 20 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    CurrentTournament = Tournaments[TournamentType.BattleField];
                    CurrentTournament.Open();
                }
                if (Now64.Hour == 20 && Now64.Minute == 30 && Now64.Second == 01)
                {
                    Game.MsgTournaments.MsgSchedules.SquidwardOctopus.Start();
                }
                if (Now64.Minute == 30 && Now64.Second == 01)
                {
                    CurrentTournament = Tournaments[TournamentType.TreasureThief];
                    CurrentTournament.Open();
                }
                #endregion
                #region 21:00
                if (Now64.DayOfWeek == DayOfWeek.Friday && Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    ElitePkTournament.Start();
                }
                if (Now64.DayOfWeek == DayOfWeek.Thursday && Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    SkillTeamPkTournament.Start();
                }
                if (Now64.DayOfWeek == DayOfWeek.Sunday  && Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    PkWar.Open();
                }
                if (Now64.DayOfWeek == DayOfWeek.Monday && Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    Game.MsgTournaments.MsgSchedules.ClassPkWar.Start();
                }
                if (Now64.DayOfWeek == DayOfWeek.Saturday &&  Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                {
                    CouplesPKWar.Open();
                }
                #endregion
                #region GuildWar// Saturday
                if (Now64.DayOfWeek == DayOfWeek.Friday)
                {
                    if (Now64.Hour < 22)
                    {
                        if (GuildWar.Proces == ProcesType.Dead)
                            GuildWar.Start();
                        if (GuildWar.Proces == ProcesType.Idle)
                        {
                            if (Now64 > GuildWar.StampRound)
                                GuildWar.Began();
                        }
                        if (GuildWar.Proces != ProcesType.Dead)
                        {
                            if (DateTime.Now > GuildWar.StampShuffleScore)
                            {
                                GuildWar.ShuffleGuildScores();
                            }
                        }
                        if (Now64.Hour == 21 && Now64.Minute == 45)
                        {
                            SendSysMesage("Guild War Will End After 15 Minutes All Be Ready .", MsgServer.MsgMessage.ChatMode.Center, MsgServer.MsgMessage.MsgColor.red);
                        }
                        if (GuildWar.SendInvitation == false && Now64.Hour == 21 && Now64.Minute == 45)
                        {
                            SendInvitation("GuildWar", "ConquerPoints", 358, 337, 1002, 0, 500, MsgServer.MsgStaticMessage.Messages.GuildWar);
                            GuildWar.SendInvitation = true;
                        }
                    }
                    else if (Now64.Hour == 22 && Now64.Minute == 00 && Now64.Second == 00)
                    {
                        if (GuildWar.Proces == ProcesType.Alive || GuildWar.Proces == ProcesType.Idle)
                            GuildWar.CompleteEndGuildWar();
                        GuildWar.Save();
                    }
                }
                #endregion
                #region SuperGuildWar //Wednesday
                if (Now64.DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (Now64.Hour < 22)
                    {
                        if (SuperGuildWar.Proces == ProcesType.Dead)
                            SuperGuildWar.Start();
                        if (SuperGuildWar.Proces == ProcesType.Idle)
                        {
                            if (Now64 > SuperGuildWar.StampRound)
                                SuperGuildWar.Began();
                        }
                        if (SuperGuildWar.Proces != ProcesType.Dead)
                        {
                            if (DateTime.Now > SuperGuildWar.StampShuffleScore)
                            {
                                SuperGuildWar.ShuffleGuildScores();
                            }
                        }
                    }
                    else if (Now64.Hour == 23 && Now64.Minute == 00 && Now64.Second == 01)
                    {
                        if (SuperGuildWar.Proces == ProcesType.Alive || GuildWar.Proces == ProcesType.Idle)
                            SuperGuildWar.CompleteEndGuildWar();
                        SuperGuildWar.Save();
                    }
                }
                #endregion
                #region CaptureTheFlag //Thursday
                if (Now64.DayOfWeek == DayOfWeek.Wednesday)
                {
                    if (Now64.Hour == 21 && Now64.Minute == 00 && Now64.Second == 01)
                    {
                        CaptureTheFlag.Start();
                    }
                    if (CaptureTheFlag.Proces == ProcesType.Alive)
                    {
                        CaptureTheFlag.UpdateMapScore();
                        CaptureTheFlag.CheckUpX2();
                        CaptureTheFlag.SpawnFlags();
                    }
                    if (Now64.Hour == 22 && Now64.Minute == 00 && Now64.Second == 00)
                    {
                        CaptureTheFlag.CheckFinish();
                    }
                }
                #endregion                                                                     
            }
            catch (Exception e)
            {
                MyConsole.SaveException(e);
            }
        }
    }
}