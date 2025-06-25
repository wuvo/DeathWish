using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Text;
using Ultimate.Game;
using Ultimate.Main.Sockets;
using System.IO;
using Ultimate.Main;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Ultimate.Events;
using Ultimate.MysqlDB;
using System.Threading.Tasks;
using DSharpPlus;
using System.Management;

namespace Ultimate
{

    class Program
    {
    
        //static string Mac()
        //{
        //    ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //    foreach (ManagementObject obj in manager.GetInstances())
        //    {
        //        if ((bool)obj["IPEnabled"])
        //        {
        //            return obj["MacAddress"].ToString();
        //        }
        //    }

        //    return String.Empty;
        //}
        public static void WriteBans(string Line)
        {
            //  Console.WriteLine(Line);

            try
            {
                if (!System.IO.File.Exists("bans." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("bans." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("bans." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("bans." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteUnhandledException(string Line)
        {
            //Console.WriteLine(Line);

            try
            {
                if (!System.IO.File.Exists("uexc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("uexc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("uexc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("uexc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteException(string Line)
        {
            //Console.WriteLine(Line);

            try
            {
                if (!System.IO.File.Exists("exc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("exc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("exc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("exc." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteCrash(string Line)
        {
            try
            {
                if (!System.IO.File.Exists("crash." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("crash." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("crash." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("crash." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteChatLine(string Line)
        {
            try
            {
                if (!System.IO.File.Exists("chat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("chat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("chat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("chat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteGMChatLine(string Line)
        {
            try
            {
                if (!System.IO.File.Exists("gmchat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("gmchat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("gmchat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("gmchat." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteLine(string Line)
        {
            try
            {
                //Console.WriteLine(Line);
                if (!System.IO.File.Exists("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                File.WriteAllText("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WritePacketLog(string Line)
        {
            try
            {
                Console.WriteLine(Line);
                if (!System.IO.File.Exists("packets." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("packets." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("packets." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("packets." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteDonation(string Line)
        {
            try
            {
                if (!System.IO.File.Exists("donations.txt"))
                    System.IO.File.Create("donations.txt").Close();
                string Text = System.IO.File.ReadAllText("donations.txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("donations.txt", Text);
            }
            catch { }
        }
        public static void WriteAntiCheat(string Line)
        {
            try
            {
                if (!System.IO.File.Exists("anticheat.txt"))
                    System.IO.File.Create("anticheat.txt").Close();
                string Text = System.IO.File.ReadAllText("anticheat.txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("anticheat.txt", Text);
            }
            catch { }
        }
        public static void WriteCmds(string Line)
        {
            try
            {
                //Console.WriteLine(Line);
                if (!System.IO.File.Exists("cmds." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("cmds." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("cmds." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("cmds." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteActions(string Line)
        {
            try
            {
                //Console.WriteLine(Line);
                if (!System.IO.File.Exists("actions." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("actions." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("actions." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("actions." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WritePickDrop(string Line)
        {
            try
            {
                //Console.WriteLine(Line);
                if (!System.IO.File.Exists("PicksDrops." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("PicksDrops." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("PicksDrops." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("PicksDrops." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteTrade(string Line)
        {
            try
            {
                // Console.WriteLine(Line);
                if (!System.IO.File.Exists("trade." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("trade." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("trade." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("trade." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static void WriteInfo(string Line)
        {
            try
            {
                //Console.WriteLine(Line);
                if (!System.IO.File.Exists("items." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("items." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("items." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += Line + "\r\n";
                System.IO.File.WriteAllText("items." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }

        public static void WriteLine(Exception e)
        {
            try
            {
                Console.WriteLine();
                if (!System.IO.File.Exists("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt"))
                    System.IO.File.Create("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt").Close();
                string Text = System.IO.File.ReadAllText("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt");
                Text += "\r\n";
                System.IO.File.WriteAllText("debug." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + ".txt", Text);
            }
            catch { }
        }
        public static bool EndSession = false;
        public static double SaveOldRate = 0;
        public static bool Reseting = false;
        public static string 
            Type = "";
        public static MyRandom Rnd = new MyRandom();
        public static AutoPayments_System Donations;
        //public static Votes Voting;
        public static string ConquerPath;
        //public static Features.QuizShow.MainInfo MainQuizShowInfo;
        //public static Events.LastManStanding LMS = new Events.LastManStanding();
        //public static Events.PTB PTB = new Events.PTB();
        //public static Events.SkillPK SkillPK = new Events.SkillPK();
        //public static Events.KOTH KOTH = new Events.KOTH();
        //public static Events.TDM TDM = new Events.TDM();
        //public static Events.FreezeWar FreezeWar = new Events.FreezeWar();
        //public static Events.Vampire_War VampireWar = new Events.Vampire_War();
        //public static Events.Infection Infection = new Events.Infection();
        //public static Events.MeteorShower MeteorShower = new Events.MeteorShower();
        //public static Events.DragonWar DragonWar = new Events.DragonWar();
        //public static Events.CaptureTheBag CaptureTheBag = new Events.CaptureTheBag();
        public static Events.LadderTournament LadderTournament = new Events.LadderTournament();
        //public static Events.HalloweenInfection HalloweenInfection = new Events.HalloweenInfection();
        //public static Events.WackaMoleHalloween WackaMoleHalloween = new Events.WackaMoleHalloween();
        //public static Events.PimpOutSanta PimpOutSanta = new Events.PimpOutSanta();
        //static int CharsC = 0;
        // public static Character[] Chars;
        //public static DataThread[] ThreadInfo = new DataThread[7];
        // public static DataThread ThreadInfo = new DataThread();
#warning ThreadInfo
        //public static Character[] ThreadInfo;
        static MyThread CompanionThread, ServerStuff, Events, /*TakeChrOFF,*/ MobThread, Timer, /*TimerS2, TimerS3, TimerAT2, TimerAT3,*/ Timer2, /*TakeChars, /*MobThread2,*/ MobAttack /*, MobAttack2 */, Log;
        private static ConcurrentDictionary<uint, DroppedItem> DeletedItems = new ConcurrentDictionary<uint, DroppedItem>();
        //private static List<MapEffect> DeletedEffects = new List<MapEffect>();
        private static readonly NPC _npcInfo = new NPC();
        private static Location _location;
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
        // private static bool isclosing = false;
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:

                case CtrlTypes.CTRL_BREAK_EVENT:

                case CtrlTypes.CTRL_CLOSE_EVENT:

                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    Console.WriteLine("Program closing!");
                    ServerClose();
                    break;

            }
            return true;
        }
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            WriteUnhandledException(e.ExceptionObject.ToString());
            WriteUnhandledException("\nTerminating: " + e.IsTerminating.ToString());
            if (e.IsTerminating)
            {
                ServerClose(true);
            }
        }
        static void Main(string[] args)
        {
            MainAsync(args);
            //System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.SustainedLowLatency;
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            string sp = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Split(Path.DirectorySeparatorChar).Last();
            
            World.LowRatedServer = false;
            Console.Title = "Ultimate";
            World.AccPathCount = 17;
            World.CharPathCount = 28;
            World.GlobalAccountsPath = @"C:\OldCODB\Users\";
            World.GlobalCharactersPath = @"C:\OldCODB\Users\Characters\";
            World.BannedChars = @"C:\OldCODB\Users\Characters\Banned\";
            World.GlobalAccountsPath2Slashes = "C:\\OldCODB\\Users\\";
            World.GlobalCharactersPath2Slashes = "C:\\OldCODB\\Users\\Characters\\";
            if (System.IO.Directory.Exists(@"C:\OldCODB"))
            {
                try
                {
                    var _stopwatch = new System.Diagnostics.Stopwatch();
                    _stopwatch.Start();


                    Console.WriteLine("Server starting please wait....");
                    if (World.LowRatedServer)
                        Thread.Sleep(3000);
                    else Thread.Sleep(6000);
                    IniFile I = new IniFile(@"C:\OldCODB\Config.ini");
                    ConquerPath = I.ReadString("Database", "ConquerPath");
                    World.Blowfish = I.ReadString("Blowfish", "Key");
                    AntiCheatPacket.LoadFiles();
                    Database.DefaultCoords.TryAdd((uint)1002, new Game.Vector2() { X = 430, Y = 378 });
                    Database.DefaultCoords.TryAdd((uint)1015, new Game.Vector2() { X = 717, Y = 571 });
                    Database.DefaultCoords.TryAdd((uint)1000, new Game.Vector2() { X = 500, Y = 650 });
                    Database.DefaultCoords.TryAdd((uint)1011, new Game.Vector2() { X = 188, Y = 264 });
                    Database.DefaultCoords.TryAdd((uint)1020, new Game.Vector2() { X = 565, Y = 562 });
                    Database.AddSkills();
                    Database.LoadItems();
                    Database.LoadPlusInfo();
                    Database.LoadRevPoints();
                    Database.LoadProfExp();
                    Database.LoadPortals();
                    Database.LoadLevelExp();
                    Database.LoadNPCs();
                    Database.LoadShops();
                    DropRates.Load();
                    Database.ReadAllCharacterStats();
                    Database.LoadKOs();
                    Features.SkillsClass.Load();
                    Features.Guilds.LoadGuilds();
                    DMaps.Load();
                    DMaps.LoadHouses();
                    Features.HouseTable.LoadFurnitures();
                    PacketHandling.CustomDialog.LoadDialogs();
                    Database.LoadMobs();
                    //Features.Lottery.LoadLotto();
                    //Database.LoadReviverGuards();
                    Database.LoadCompanions();
                    Database.LoadSquamas(new MapEffect());
                    World._serverVersion = Convert.ToUInt16(File.ReadAllLines(@"C:\OldCODB\version.txt").First());
                    //MainQuizShowInfo = new Features.QuizShow.MainInfo();
                    if (File.Exists("entityids.txt"))
                    {
                        string[] Eids = File.ReadAllText("entityids.txt").Split(' ');
                        foreach (string Line in Eids)
                        {
                            World.EIDS.Add(uint.Parse(Line));
                        }
                    }
                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("accounts").Where("Status", 5);
                    MySQL.MySqlReader Banned = new MySQL.MySqlReader(Cmd);

                    while (Banned.Read())
                    {
                        MySQL.MySqlCommand Add = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("characters").Where("UID", Banned.ReadUInt32("UID"));
                        MySQL.MySqlReader Ban = new MySQL.MySqlReader(Add);
                        while (Ban.Read())
                            if (!Game.World.BanChars.Contains(Ban.ReadString("Name")))
                                Game.World.BanChars.Add(Ban.ReadString("Name"));

                        Add = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("bannedchars").Where("UID", Banned.ReadUInt32("UID"));
                        Ban = new MySQL.MySqlReader(Add);
                        while (Ban.Read())
                            if (!Game.World.BanChars.Contains(Ban.ReadString("Name")))
                                Game.World.BanChars.Add(Ban.ReadString("Name"));
                    }
                    //if (File.Exists("BanList.txt"))
                    //{
                    //    string[] BanList = File.ReadAllLines("BanList.txt");
                    //    foreach (string Line in BanList)
                    //    {
                    //        Game.World.BanChars.Add(Line);
                    //    }
                    //}

                    Features.GuildWars.Init();
                    Features.TCGuildWars.Init();
                    Features.CounterClock.Init();
                    Features.CityWarTc.Init();
                    Features.CityWarPc.Init();
                    Features.CityWarAc.Init();
                    Features.CityWarDc.Init();
                    Features.CityWarBi.Init();
                    Features.PoleWarTC.Init();
                    Features.PoleWarPC.Init();
                    Features.PoleWarAC.Init();
                    Features.PoleWarDC.Init();
                    Features.PoleWarBI.Init();

                    World.DebugAdd += "Loading Sockets... \r\n";
                    ushort LoginPort = 0;
                    ushort GamePort = 0;
                    if (!World.LowRatedServer)
                    {
                        LoginPort = ushort.Parse(I.ReadString("Database", "LoginPort"));
                        GamePort = ushort.Parse(I.ReadString("Database", "GamePort"));
                    }
                    else
                    {
                        LoginPort = ushort.Parse(I.ReadString("DatabaseNewServer", "LoginPort"));
                        GamePort = ushort.Parse(I.ReadString("DatabaseNewServer", "GamePort"));
                    }
                    //LoginPort = 9959;
                    //GamePort = 5817;// for test
                    MasterSocket AuthServer = new MasterSocket(LoginPort);//9958
                    AuthServer.AnnounceNewConnection += new Action<Wrapper>(AuthServer_AnnounceNewConnection);
                    AuthServer.AnnounceReceive += new Action<byte[], Wrapper, byte[]>(AuthServer_AnnounceReceive);

                    MasterSocket GameServer = new MasterSocket(GamePort);//5816
                    GameServer.AnnounceNewConnection += new Action<Wrapper>(GameServer_AnnounceNewConnection);
                    GameServer.AnnounceReceive += new Action<byte[], Wrapper, byte[]>(GameServer_AnnounceReceive);
                    GameServer.AnnounceDisconnection += new Action<Wrapper>(GameServer_AnnounceDisconnection);
                    Native.DeleteMenu(Native.GetSystemMenu(Native.GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                    try
                    {
                        AuthWorker.GetGameIP();
                    }
                    catch { Console.WriteLine("IP retrieved from config file!"); AuthWorker.GameIP = I.ReadString("Database", "GameServerIP"); }
                    World.DebugAdd += "\r\n";
                    World.DebugAdd += "The server is ready for connections. \r\n";
                    World.DebugAdd += "Server IP : " + AuthWorker.GameIP + " Auth(P) : " + LoginPort + " Game(P) : " + GamePort + "\r\n";
                    World.DebugAdd += "Always close the server by pressing Enter, or the important data wont't save. \r\n";
                    Console.WriteLine("The server is ready for connections.");
                    Console.WriteLine("Server IP : " + AuthWorker.GameIP + " Auth(P) : " + LoginPort + " Game(P) : " + GamePort);
                    Console.WriteLine("Always close the server by pressing Enter, or the important data wont't save.");
                    //Database.LoadQuestions();
                    Database.CreateEquipsDrops();
                    Database.LoadEmpire();
                    SOB.GuildStatue.LoadStatues();
                    Features.ArenaQualifier.LoadRankings();
                    //ControlPanel E = new ControlPanel();
                    //System.Windows.Forms.Application.Run(E);
                    Donations = new AutoPayments_System();
                    //Voting = new Votes();

                    if (DateTime.Now.AddDays(10) >= NPCs.NPC_7.EasterSunday(DateTime.Now.Year) && DateTime.Now <= NPCs.NPC_7.EasterSunday(DateTime.Now.Year).AddDays(10))
                    {
                        Database.LoadNPCs(13);
                    }

                    //  for (int i = 0; i < 7; i++)
#warning ThreadInfo
                    //ThreadInfo = new Character[0];
                    CompanionThread = new MyThread();
                    CompanionThread.Execute += new Execute(CompanionThread_Execute);
                    CompanionThread.Start(500);

                    ServerStuff = new MyThread();
                    ServerStuff.Execute += new Execute(ServerStuff_Execute);
                    ServerStuff.Start(1000);


                    Events = new MyThread();
                    Events.Execute += new Execute(Events_Execute);
                    Events.Start(60000);

#warning TakeOFFChar
                    //TakeChrOFF = new MyThread();
                    //TakeChrOFF.Execute += new Execute(TakeChrOFF_Execute);
                    //TakeChrOFF.Start(4000);

                    MobThread = new MyThread();
                    MobThread.Execute += new Execute(MobThread_Execute);
                    MobThread.Start(750);

                    Log = new MyThread();
                    Log.Execute += new Execute(WriteLogs);
                    Log.Start(3600000);


                    /*MobThread2 = new MyThread();
                    MobThread2.Execute += new Execute(MobThread2_Execute);
                    MobThread2.Start(1000);*/

                    MobAttack = new MyThread();
                    MobAttack.Execute += new Execute(MobAttack_Execute);
                    MobAttack.Start(250);//1000
                    /*  MobAttack2 = new MyThread();
                      MobAttack2.Execute += new Execute(MobAttack2_Execute);
                      MobAttack2.Start(1000); */

                    Timer = new MyThread();
                    Timer.Execute += new Execute(Step);
                    Timer.Start(250);
                    /* TimerS2 = new MyThread();
                     TimerS2.Execute += new Execute(Step2);
                     TimerS2.Start(200);

                     TimerS3 = new MyThread();
                     TimerS3.Execute += new Execute(Step3);
                     TimerS3.Start(200);*/
                    #region Not used
                    /*
                StepTimer4 = new MyThread();
                StepTimer4.Execute += new Execute(Step4);
                StepTimer4.Start(200);

                StepTimer5 = new MyThread();
                StepTimer5.Execute += new Execute(Step5);
                StepTimer5.Start(200);

                StepTimer6 = new MyThread();
                StepTimer6.Execute += new Execute(Step6);
                StepTimer6.Start(200);
                 */

                    /*
    TimerAT4 = new MyThread();
    TimerAT4.Execute += new Execute(Attacks4);
    TimerAT4.Start(200);

    TimerAT5 = new MyThread();
    TimerAT5.Execute += new Execute(Attacks5);
    TimerAT5.Start(200);

    TimerAT6 = new MyThread();
    TimerAT6.Execute += new Execute(Attacks6);
    TimerAT6.Start(200);*/
                    #endregion

                    Timer2 = new MyThread();
                    Timer2.Execute += new Execute(Attacks);
                    Timer2.Start(250);

                    /* TimerAT2 = new MyThread();
                     TimerAT2.Execute += new Execute(Attacks2);
                     TimerAT2.Start(200);

                     TimerAT3 = new MyThread();
                     TimerAT3.Execute += new Execute(Attacks3);
                     TimerAT3.Start(200);*/

                    /*
                    Timer3 = new MyThread();
                    Timer3.Execute += new Execute(EndSend);
                    Timer3.Start(200);

                    TimerE2 = new MyThread();
                    TimerE2.Execute += new Execute(EndSend2);
                    TimerE2.Start(200); */



                    /*  TakeChars = new MyThread();
                      TakeChars.Execute += new Execute(TakeChr);
                      TakeChars.Start(4000);*/
                    World.MobsStart = true;
                    //Features.Weather.NextChange = DateTime.Now.AddMinutes(Rnd.Next(4, 30));

                    _stopwatch.Stop();
                    TimeSpan elapsedTime = _stopwatch.Elapsed;
                    Console.WriteLine("Started in: " + elapsedTime);
                }
                catch (Exception E)
                {
                    WriteCrash(E.ToString() + "\r\n");
                    Console.WriteLine(E);
                    WriteLogs();
                    Database.Dispose();
                    Console.WriteLine("Database disposed.");
                    Console.WriteLine("Restarting...");
                    System.Diagnostics.Process.Start("Ultimate.exe");
                    Environment.Exit(0);
                }
                while (true)
                {
                    #region Cmds
                    string Command = Console.ReadLine();
                    if (Command == null)
                        continue;
                    string[] Cmd = Command.Split(' ');

                    if (Command.Length == 0)
                    {
                        Console.ReadLine();
                    }
                    #region /vipbonus
                    if (Cmd[0] == "/vipbonus")
                    {
                        System.Threading.Thread Add7VIp = new System.Threading.Thread(Database.Add7VIPDays);
                        Add7VIp.Start();
                    }
                    #endregion
                    #region /restart
                    else if (Cmd[0] == "/restart")
                    {
                        Game.Character[] BaseCharacters = World.H_Chars.Values.ToArray();
                        //Game.Character[] BaseCharacters = new Character[World.H_Chars.Count];
                        //World.H_Chars.Values.CopyTo(BaseCharacters, 0);

                        KillThreads();
                        EndSession = true;
                        try
                        {
                            foreach (Game.Character C in BaseCharacters)
                            {
                                try
                                {
                                    C.MyClient.Disconnect();
                                    if (C.MyClient.Soc.Connected)
                                        C.MyClient.Soc.Disconnect(false);
                                    Console.WriteLine(C.Name + " has logged off successfuly.");
                                }
                                catch { }
                            }
                        }
                        catch { }
                        WriteLogs();
                        Database.SaveKOs();
                        Console.WriteLine("KOs saved.");
                        Database.SaveEmpire();
                        Console.WriteLine("Empire saved.");
                        Features.Guilds.SaveGuilds();
                        Console.WriteLine("Guilds saved.");
                        Features.SkillsClass.Save();
                        DMaps.Save();
                        Console.WriteLine("Skills saved.");
                        Features.HouseTable.SaveFurnitures();
                        Console.WriteLine("Furnitures saved.");
                        SOB.GuildStatue.SaveStatues();
                        Console.WriteLine("Guild Statues saved.");
                        Features.ArenaQualifier.SaveRankings();
                        Console.WriteLine("Arena Rankings saved.");

                        Database.Dispose();
                        Console.WriteLine("Database disposed.");
                        Console.WriteLine("Write /close to close the window. >>>Do NOT press the X button!<<<");
                        System.Diagnostics.Process.Start("Ultimate.exe");
                        Environment.Exit(0);
                    }
                    #endregion
                    #region /writeban
                    else if (Cmd[0] == "/writeban")
                    {
                        Console.WriteLine("Writing bans...");
                        string ban = "";
                        foreach (KeyValuePair<string, uint> DE in Game.World.ToBanIPList)
                        {
                            ban += "IP: " + DE.Key + " logs: " + DE.Value + "\r\n";
                        }
                        Program.WriteBans(ban);
                        Game.World.ToBanIPList.Clear();
                        Console.WriteLine("Writing bans finished!");
                    }
                    #endregion
                    else if (Cmd[0] == "/writelogs")
                    {
                        Console.WriteLine("Saving logs...");
                        WriteLogs();
                        Console.WriteLine("Logs saved!");
                    }
                    else if (Cmd[0] == "/players")
                    {
                        World.DebugAdd += "Time : " + DateTime.Now + "\r\n";
                        World.DebugAdd += "Players Online: " + Game.World.H_Chars.Count + "\r\n";
                        string eMsg = "";
                        foreach (Game.Character C in Game.World.H_Chars.Values)
                            eMsg += C.Name + C.MyClient.AuthInfo.Status + ", ";
                        if (eMsg.Length > 1)
                            eMsg = eMsg.Remove(eMsg.Length - 2, 2);
                        World.DebugAdd += eMsg + "\r\n";
                    }
                    else if (Cmd[0] == "/getmets")
                    {
                        Thread T = new Thread(Database.GetMets);
                        T.Start();
                    }
                    else if (Cmd[0] == "/rnobility")
                    {
                        System.Threading.Thread RemoveNobility = new System.Threading.Thread(Database.RemoveAllNobility);
                        RemoveNobility.Start();
                    }
                    else if (Cmd[0] == "/protect")
                    {
                        if (World.Firewall)
                        {
                            World.Firewall = false;
                            // World.Connections = 3;
                            World.SpammIps.Clear();
                            Console.WriteLine("Authentication & Game Protection off!");

                        }
                        else
                        {
                            World.Firewall = true;
                            //World.Connections = 8;
                            Console.WriteLine("Authentication & Game Protection on!");
                        }
                    }
                    else if (Cmd[0] == "/checkgold")
                    {

                        System.Threading.Thread ResetGold = new System.Threading.Thread(() => Database.CheckGoldOnPlayers(uint.Parse(Cmd[1])));
                        ResetGold.Start();
                    }
                    else if (Cmd[0] == "/cleartopfb")
                    {
                        Thread FBReset = new Thread(Database.TopFBReset);
                        FBReset.Start();
                    }
                    else if (Cmd[0] == "/migrateaccs")
                        Dbase.Migration.MigrateAccounts();
                    else if (Cmd[0] == "/migrateguilds")
                        Dbase.Migration.MigrateGuilds();
                    else if (Cmd[0] == "/migratenpcs")
                        Dbase.Migration.MigrateNPCs();
                    else if (Cmd[0] == "/migratechars")
                        Dbase.Migration.MoveFirstValues();
                    else if (Cmd[0] == "/removebanned")
                        Dbase.Migration.RemoveBans();
                    //else if (Cmd[0] == "/checktreasure")
                    //{

                    //    System.Threading.Thread ResetGold = new System.Threading.Thread(() => Database.CheckTreasureOnPlayers(ushort.Parse(Cmd[1])));
                    //    ResetGold.Start();
                    //}
                    //else if (Cmd[0] == "/checkflowers")
                    //{

                    //    System.Threading.Thread ResetGold = new System.Threading.Thread(() => Database.CheckFlowersOnPlayers(uint.Parse(Cmd[1])));
                    //    ResetGold.Start();
                    //}
                    //else if (Cmd[0] == "/delchars")
                    //{
                    //    Thread T = new Thread(Database.DeleteUnusedAccounts);
                    //    T.Start();
                    //}
                    //else if (Cmd[0] == "/fixids")
                    //{
                    //    System.Threading.Thread FixIds = new System.Threading.Thread(Database.Fill_EntityID_And_Fix);
                    //    FixIds.Start();
                    //}
                    else if (Cmd[0] == "/exit")
                    {
                        ServerClose();
                    }
                    else if (Cmd[0] == "/close")
                    {
                        Console.WriteLine("Server is closing...");
                        ServerClose();
                        Environment.Exit(0);
                    }
                    else if (Cmd[0] == "/clear")
                        Console.Clear();


                    else if (Cmd[0] == "/newacc" && Cmd.Length > 2)
                    {
                        if (Cmd.Length == 3)
                            Database.CreateAccount(Cmd[1], Cmd[2], "");
                        else
                            Database.CreateAccount(Cmd[1], Cmd[2], Cmd[3]);
                    }
                    else if (Cmd[0] == "/rename_lowrate_accounts")
                    {
                        Database.ChangeNames();
                    }
                    #region /ban
                    else if (Cmd[0] == "/ban")
                    {

                        Game.Character C = Game.World.CharacterFromName(Cmd[1]);
                        if (C != null)
                        {
                            if (!Game.World.BanChars.Contains(C.Name))
                            {
                                Game.World.BanChars.Add(C.Name);
                                if (C.MyClient != null)
                                    if (C.MyClient.Soc.Connected)
                                        C.MyClient.Soc.Disconnect(false);
                                Console.WriteLine(C.Name + " got banned!");
                            }
                            else
                            {
                                Console.WriteLine(C.Name + " is already banned!");
                            }
                            if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                    File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");
                        }
                        else
                        {
                            string Account = "";
                            C = Database.LoadCharacter(Cmd[1], ref Account);
                            if (C != null)
                            {
                                if (!Game.World.BanChars.Contains(C.Name))
                                {
                                    Game.World.BanChars.Add(C.Name);
                                    Console.WriteLine(C.Name + " got banned!");
                                }
                                else
                                    Console.WriteLine(C.Name + " is already banned!");

                                if (File.Exists(Game.World.GlobalCharactersPath + C.Name + ".chr"))
                                    if (Directory.Exists(Game.World.GlobalCharactersPath + "Banned"))
                                        File.Move(Game.World.GlobalCharactersPath + C.Name + ".chr", Game.World.GlobalCharactersPath + @"Banned\" + C.Name + ".chr");
                            }
                            else Console.WriteLine(Cmd[1] + " does not exist!");
                        }
                    }
                    #endregion
                    #region /mapevent
                    else if (Cmd[0] == "/mapevent")
                    {
                        Console.WriteLine("Created map : " + DMaps.CreateDynamicMap(ushort.Parse(Cmd[1]), uint.Parse(Cmd[2]), true) + " id: " + uint.Parse(Cmd[2]) + " map used: " + ushort.Parse(Cmd[1]));
                    }
                    else if (Cmd[0] == "/dmapevent")
                    {
                        Console.WriteLine("Deleted map : " + DMaps.DeleteDynamicMap(uint.Parse(Cmd[1]), true));
                    }
                    #endregion
                    else if (Cmd[0] == "/load")
                    {
                        Game.Character C = Game.World.CharacterFromName(Cmd[1]);
                        if (C == null)
                        {
                            string Account = "";
                            Database.LoadCharacterWithLogs(Cmd[1], ref Account);
                            Console.WriteLine("Finished loading character with logs");
                        }
                        else Console.WriteLine("Character is not null (online)");
                    }
                    else if (Cmd[0] == "/updatechars")
                    {
                        Thread UpdateChars = new System.Threading.Thread(Database.UpdateChars);
                        UpdateChars.Start();
                    }
                    else if (Cmd[0] == "/updatebannedchars")
                    {
                        Thread UpdateBannedChars = new System.Threading.Thread(Database.UpdateBannedChars);
                        UpdateBannedChars.Start();
                    }
                    System.Threading.Thread.Sleep(1);
                    #endregion
                }
            }
            else
            {
                Console.WriteLine("No DatabaseFolder!");
                World.DebugAdd += "The database folder doesn't exist, cannot start the server. \r\n";
                Console.ReadLine();
            }

        }
        static async Task MainAsync(string[] args)
        {

            Discord DCord = new Discord();
            DCord.Discord_Basladi();

        }
        public static void ExitProgram()
        {
            Environment.Exit(0);
        }
        public static void RestartPC()
        {
            System.Diagnostics.Process.Start("shutdown", "/r /t 0");
        }
        public static void ServerClose(bool Restart = false)
        {
            if (!System.IO.Directory.Exists(@"C:\OldCODB") || !World.MobsStart)
                return;
            Game.World.Exit = true;
            Game.Character[] TempChars = World.H_Chars.Values.ToArray();
            // Game.Character[] TempChars = new Character[World.H_Chars.Count];
            //World.H_Chars.Values.CopyTo(TempChars, 0);

            KillThreads();
            EndSession = true;
            try
            {
                foreach (Game.Character C in TempChars)
                {
                    try
                    {
                        C.MyClient.Disconnect();
                        C.MyClient.LogOff();
                        if (C.MyClient.Soc.Connected)
                            C.MyClient.Soc.Disconnect(false);
                        Console.WriteLine(C.Name + " has logged off successfuly.");
                    }
                    catch { }
                }
            }
            catch { }
            WriteLogs();
            Database.SaveKOs();
            Console.WriteLine("KOs saved.");
            Database.SaveEmpire();
            Console.WriteLine("Empire saved.");
            Features.Guilds.SaveGuilds();
            Console.WriteLine("Guilds saved.");
            Features.SkillsClass.Save();
            DMaps.Save();
            Console.WriteLine("Skills saved.");
            Features.HouseTable.SaveFurnitures();
            Console.WriteLine("Furnitures saved.");
            SOB.GuildStatue.SaveStatues();
            Console.WriteLine("Guild Statues saved.");
            Features.ArenaQualifier.SaveRankings();
            Console.WriteLine("Arena Rankings saved.");

            Database.Dispose();
            //Native.DeleteMenu(Native.GetSystemMenu(Native.GetConsoleWindow(), true), SC_CLOSE, MF_BYCOMMAND);
            Console.WriteLine("Database disposed.");
            Console.WriteLine("Type /close to exit the application! >>>Do NOT press the X button!<<<");
            Console.WriteLine("Type /close to exit the application! >>>Do NOT press the X button!<<<");
            Console.WriteLine("Type /close to exit the application! >>>Do NOT press the X button!<<<");
            Thread.Sleep(1000);
            if (Restart)
            {
                Thread.Sleep(1000);
                System.Diagnostics.Process.Start("Ultimate.exe");
                Environment.Exit(0);
            }
        }
        static void GameServer_AnnounceDisconnection(Wrapper obj)
        {
            //Console.WriteLine("Accessed GameServer_AnnounceDisconnection - Program.cs");
            GameClient GC = (GameClient)obj.connector;
            if (GC != null)
            {
                GC.Disconnect();
                GC.LogOff();
                if (GC.Soc.Connected)
                    GC.Soc.Disconnect(true);
            }

        }
        static void GameServer_AnnounceReceive(byte[] arg1, Wrapper arg2, byte[] arg3)
        {
            // Console.WriteLine("Accessed GameServer_AnnounceReceive - Program.cs");
            arg3[0] = 0;
            GameClient GC = (GameClient)arg2.connector;
            //AuthWorker.Decrypt(arg1, arg1.Length);
            GC.Crypto.Decrypt(arg1);
            if (GC != null)
            {
                if (!EndSession)
                {
                    string IP = GC.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();
                    //string mac = Mac();

                    try
                    {
                        if (GC.SetBF)
                        {
                            MemoryStream MS = new MemoryStream(arg1);
                            BinaryReader BR = new BinaryReader(MS);


                            BR.ReadBytes(7);
                            uint PacketLen = BR.ReadUInt32();
                            int JunkLen = BR.ReadInt32();

                            if (JunkLen + 148 != PacketLen)
                            {
                                if (Game.World.SpammIps.ContainsKey(IP))
                                {
                                    //  int Tries = (int)Game.World.SpammIps[IP];
                                    //  Tries = Tries + 1;
                                    Game.IPLog S = (Game.IPLog)Game.World.SpammIps[IP];
                                    S.Logs += 1;
                                    Game.World.SpammIps[IP] = S;
                                }
                                else
                                {
                                    Game.IPLog S = new Game.IPLog { Logs = 1, LogDate = DateTime.Now };
                                    Game.World.SpammIps.Add(IP, S);
                                }
                                World.ExcAdd += IP + " spamming the server! - GS_AN (GOOD)\r\n";
                                Console.WriteLine(IP + " spamming the server - GS_AN (GOOD)!");

                                GC.Soc.Disconnect(false);
                                BR.Close();
                                MS.Close();
                                return;
                            }
                            GC.SetBF = false;
                            BR.ReadBytes(JunkLen);
                            int Len = BR.ReadInt32();
                            string PubKey = ASCIIEncoding.ASCII.GetString(BR.ReadBytes(Len));

                            GC.Crypto = new GameCrypto(GC.KeyExchance.ComputeKey(OpenSSL.BigNumber.FromHexString(PubKey)));
                            GC.Crypto.Blowfish.DecryptIV = GC.NewClientIV;
                            GC.Crypto.Blowfish.EncryptIV = GC.NewServerIV;




                            BR.Close();
                            MS.Close();
                        }
                        else
                        {
                            ushort PacketLength = BitConverter.ToUInt16(arg1, 0);
                            if (arg1.Length <= 2)
                            {
                                GC.Soc.Disconnect(false);
                                return;
                            }
                            ushort PacketID = BitConverter.ToUInt16(arg1, 2);

                            if (PacketID == 10852)
                            {
                                try
                                {
                                    ulong CryptoKey = BitConverter.ToUInt64(arg1, 4);

                                    AuthWorker.AuthInfo Info = (AuthWorker.AuthInfo)AuthWorker.KeyedClients[CryptoKey];
                                    GC.AuthInfo = Info;
                                    GC.MessageID = (uint)Rnd.Next(50000);
                                    GC.Soc = arg2._socket;
                                    if (Game.World.SpammIps.ContainsKey(IP))
                                        Game.World.SpammIps.Remove(IP);
                                    if (Game.World.Firewall)
                                        if (Game.World.ToBanIPList.ContainsKey(IP))
                                            Game.World.ToBanIPList.Remove(IP);

                                    GC.SignatureKey = Info.SignatureKey;
                                    GC.MacAddress = Info.MacAddress;
                                    //if (!Info.RightVersion)
                                    //{
                                    //    GC.AddSend(Packets.SystemMessage(GC.MessageID, "Your client version doesn't match the server version!\r\nPlease restart the client or re-download the client from our website!"));
                                    //    GC.Soc.Disconnect(false);

                                    //    return;
                                    //}
                                    if (IPBan.BannedIPs.Contains(IP))
                                    {
                                        GC.AddSend(Packets.SystemMessage(GC.MessageID, "You're IP banned !"));
                                        GC.Soc.Disconnect(false);
                                        return;
                                    }


                                    if (Info.InvalidFiles)
                                    {
                                        Console.WriteLine($"Invalid file hashes.! Rejected login ==> {GC.AuthInfo.Character}");
                                        //GC.AddSend(Packets.SystemMessage(GC.MessageID, "Modified files! Please patch your client to latest patch."));
                                        World.DebugAdd += GC.AuthInfo.Character + " got kicked! (Invalid files) \r\n";
                                        GC.Soc.Disconnect(false);
                                        //  Database.SaveCharacter(C.MyChar, C.AuthInfo.Account);
                                        if (GC.Soc.Connected)
                                            GC.Soc.Disconnect(false);
                                        else
                                            GC.LogOff();
                                        return;
                                    }
                                    //if (!NewAntiCheat.IsValidTail(GC.SignatureKey, arg1, true))
                                    //{
                                    //    Console.WriteLine($"{GC.AuthInfo.Character} has sent broken tail. (1052)");
                                    //    GC.AddSend(Packets.SystemMessage(GC.MessageID,
                                    //        "You cannot login."));
                                    //    GC.Disconnect();
                                    //    return;
                                    //}
                                    if (GC.AuthInfo.LogonType == 2)
                                    {
                                        GC.AddSend(Packets.SystemMessage(GC.MessageID, "NEW_ROLE"));
                                    }
                                    else if (GC.AuthInfo.LogonType == 1)
                                    {
                                        string Acc = "";
                                        try
                                        {
                                            if (Game.World.BanChars.Contains(GC.AuthInfo.Character) || GC.AuthInfo.Status == "5")
                                            {
                                                GC.AddSend(Packets.SystemMessage(GC.MessageID, "This account has been banned!\nFeel free to contact us at our Facebook Page!"));
                                                GC.LocalMessage(2105, "http://www.Ultimateconquer.com");
                                                World.DebugAdd += GC.AuthInfo.Character + " got kicked! (banned user) \r\n";
                                                GC.Soc.Disconnect(false);

                                                return;
                                            }

                                            foreach (GameClient C in Game.World.H_Clients.Values)
                                            {
                                                if (C.AuthInfo.Character == GC.AuthInfo.Character)
                                                {
                                                    GC.AddSend(Packets.SystemMessage(GC.MessageID, "Character is already logged! Please try again!"));
                                                    World.DebugAdd += GC.AuthInfo.Character + " got kicked! (already logged) \r\n";
                                                    GC.Soc.Disconnect(false);
                                                    //  Database.SaveCharacter(C.MyChar, C.AuthInfo.Account);
                                                    if (C.Soc.Connected)
                                                        C.Soc.Disconnect(false);
                                                    else
                                                        C.LogOff();
                                                    return;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            World.DebugAdd += GC.AuthInfo.Character + " got kicked! (error processing) \r\n";
                                            GC.Soc.Disconnect(false);
                                            return;
                                        }
                                        //var _stopwatch = new System.Diagnostics.Stopwatch();
                                        //_stopwatch.Start();
                                        GC.MyChar = Database.LoadCharacter(GC.AuthInfo.Character, ref Acc, true, true);
                                        //GC.MyChar = Database.LoadCharacter(GC.AuthInfo.Character, true);
                                        //_stopwatch.Stop();

                                        //TimeSpan elapsedTime = _stopwatch.Elapsed;
                                        //Console.WriteLine("Loaded account in: " + elapsedTime);

                                        //MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                        //Cmd.Update("characters").Set("Face", GC.MyChar.Avatar).Where("UID", GC.MyChar.EntityID).Execute();

                                        try
                                        {
                                            GC.MyChar.MyClient = GC;
                                        }
                                        catch
                                        {
                                            World.DebugAdd += GC.AuthInfo.Character + " got kicked! (error adding GC) \r\n";
                                            GC.Soc.Disconnect(false); return;
                                        }

                                        GC.AddSend(Packets.SystemMessage(GC.MessageID, "ANSWER_OK"));
                                        GC.AddSend(Packets.CharacterInfo(GC.MyChar));//When I send this from interserver, it sends that anti cheat packet and then the client itself freezes...

                                        GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.VIPLevel, GC.MyChar.VipLevel));
                                        GC.AddSend(Packets.Time());
                                        GC.AddSend(Packets.Donators(GC.MyChar));
                                        GC.AddSend(Packets.Packet1012(GC.MyChar.EntityID));
                                        GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Effect, 0));
                                        GC.MyChar.LastLogin = DateTime.Now;
                                        Game.World.H_Clients.Add(GC.MyChar.EntityID, GC);
                                        /* if (Game.World.SpammIps.Contains(IP))
                                             Game.World.SpammIps.Remove(IP);*/
                                        //string LocalIP = GC.Soc.LocalEndPoint.ToString().Split(':')[0].ToString();

                                        // Console.WriteLine(GC.Soc.sendToString());

                                        /* System.Net.IPHostEntry H = null;
                                         try
                                         {
                                             H = System.Net.Dns.GetHostEntry(IP);


                                         }
                                         catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; } */
                                        // if (H != null)
                                        // {
                                        if (GC.MyChar.VIPDays > 0)

                                            World.DebugAdd += GC.MyChar.Name + GC.AuthInfo.Status + " has logged on with IP: " + IP + " VIP: " + GC.MyChar.VipLevel + " Days Left: " + GC.MyChar.VIPDays + "\r\n";
                                        else
                                        {
                                            World.DebugAdd += GC.MyChar.Name + GC.AuthInfo.Status + " has logged on with IP: " + IP + "\r\n";
                                        }
                                        //  }
                                        /*   else
                                           {
                                               if (GC.MyChar.VIPDays > 0)
                                                   World.DebugAdd += GC.MyChar.Name + GC.AuthInfo.Status + " has logged on with IP: " + IP + " HostName: null VIP: " + GC.MyChar.VipLevel + " Days Left: " + GC.MyChar.VIPDays + "\r\n";
                                               else
                                               {
                                                   World.DebugAdd += GC.MyChar.Name + GC.AuthInfo.Status + " has logged on with IP: " + IP + " HostName: null \r\n";
                                               }
                                           }*/

                                    }
                                    // GC.EndSend();
                                }
                                catch
                                {
                                    World.DebugAdd += GC.AuthInfo.Character + " got kicked! (error processing whole login) \r\n";
                                    GC.Soc.Disconnect(false); return;
                                }
                            }
                            else PacketHandler.Handle(GC, arg1);
                        }
                    }
                    catch (Exception E)
                    {
                        if (Game.World.SpammIps.ContainsKey(IP))
                        {
                            //   int Tries = (int)Game.World.SpammIps[IP];
                            //    Tries = Tries + 1;
                            Game.IPLog S = (Game.IPLog)Game.World.SpammIps[IP];
                            S.Logs += 1;
                            Game.World.SpammIps[IP] = S;
                        }
                        else
                        {
                            IPLog S = new IPLog { Logs = 1, LogDate = DateTime.Now };
                            World.SpammIps.Add(IP, S);
                        }
                        World.ExcAdd += IP + " spamming the server! - GS_AN (CATCH_EXC)\r\n";
                        World.ExcAdd += E.ToString() + "\r\n";
                        Console.WriteLine(IP + " spamming the server! - GS_AN (CATCH_EXC)");

                        GC.Soc.Disconnect(false);
                    }
                }
                else
                    GC.Soc.Disconnect(false);
            }
            /* else
             {
                 GC.Crypto.Decrypt(arg1);
                 PacketHandler.Handle(GC, arg1);
             }*/
        }
        static void GameServer_AnnounceNewConnection(Wrapper obj)
        {
            // Console.WriteLine("Accessed GameServer_AnnounceNewConnection - Program.cs");
            if (Game.World.Firewall)
                if (!obj.allow)
                {
                    obj._socket.Disconnect(false);
                    return;
                }
            // Console.WriteLine("GS_ANC");
            GameClient C = new GameClient();
            C.Soc = obj._socket;
            obj.connector = C;
            C.AddSend(Packets.DHKeyPacket(C.KeyExchance.PublicKey.ToHexString(), C.NewServerIV, C.NewClientIV));
            // Console.WriteLine(C.Soc.Connected);
            //C.EndSend();
        }

        static void AuthServer_AnnounceReceive(byte[] arg1, Wrapper arg2, byte[] arg3)
        {
            //Console.WriteLine("Accessed AuthServer_AnnounceReceive Program.cs");
            try
            {
                //   Console.WriteLine("arg1.length : " + arg1.Length);
                if (arg1.Length == 276)
                {
                    Ultimate.Main.AuthWorker.DataHandler(arg2, arg1);
                }
            }
            catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }

        }
        static void AuthServer_AnnounceNewConnection(Wrapper obj)
        {
            //Console.WriteLine("Accessed AuthServer_AnnounceNewConnection Program.cs");
            try
            {
                /*  string IP = obj._socket.RemoteEndPoint.ToString().Split(':')[0].ToString();
                  if (Game.World.SpammIps.Contains(IP))
                  {
                      if ((int)Game.World.SpammIps[IP] > 3)
                      {
                          obj._socket.Disconnect(true);
                          return;
                      }
                  }*/
                if (EndSession || !World.MobsStart)
                    return;
                Ultimate.Main.AuthWorker.AuthClient AC = new Ultimate.Main.AuthWorker.AuthClient();
                AC.Crypto = new LegacyCipher();
                AC.Soc = obj._socket;
                obj.connector = AC;
            }
            catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }

        }
        /*  static void KillThreads()
        {
            if (ServerStuff.T.IsAlive)
            {
                //TakeChars.T.Abort();
                CompanionThread.T.Abort();
                ServerStuff.T.Abort();
                TakeChrOFF.T.Abort();
                // Events.T.Abort();
                MobThread.T.Abort();
                Timer.T.Abort();
               // TimerS2.T.Abort();
               // TimerS3.T.Abort();
               // TimerAT2.T.Abort();
               // TimerAT3.T.Abort();
                Timer2.T.Abort();
                //  MobThread2.T.Abort();
                MobAttack.T.Abort();
            }
          //  MobAttack2.Close();

        } */
        static void KillThreads()
        {


            //TakeChars.T.Abort();
            CompanionThread.Close();
            ServerStuff.Close();
#warning TakeCharOFF
            //TakeChrOFF.Close();
            // Events.T.Abort();
            MobThread.Close();
            Timer.Close();
            // TimerS2.T.Abort();
            // TimerS3.T.Abort();
            // TimerAT2.T.Abort();
            // TimerAT3.T.Abort();
            Timer2.Close();
            //  MobThread2.T.Abort();
            MobAttack.Close();

            //  MobAttack2.Close();
        }
        /* static void TakeChr()
       {
           
               if (World.H_Chars.Count > 0)
               {
                   lock (World.H_Chars)
                   {
                       Chars = new Character[World.H_Chars.Count];
                       World.H_Chars.Values.CopyTo(Chars, 0);
                       
                   }

               }
            
       }*/
        static void Step()
        {
            // StepWatch.Start();
            if (World.H_Chars.Count > 0)
            {//0
                /*  if (ThreadInfo.Modified)
                  {
                      lock (ThreadInfo.Array)
                      {
                          ThreadInfo[0].Array = World.H_Chars.Values.ToArray();
                         // ThreadInfo[0].Array = new Character[World.H_Chars.Count];
                         // World.H_Chars.Values.CopyTo(ThreadInfo[0].Array, 0);
                          ThreadInfo[0].Modified = false;
                      }
                  }*/
#warning ThreadInfo
                //if (ThreadInfo != null)
                //{
                try
                {
#warning ThreadInfo
                    foreach (Character C in World.H_Chars.Values/*ThreadInfo*/) // 0 to length/3   for (int x = 0; x < ThreadInfo.Length / 3; x++) // 0 to length/3
                    {
                        //  Character C = ThreadInfo[x];//altfel... poti sa faci altfel 
                        if (!EndSession)
                        {
                            if (C != null)
                            {
                                if (C.MyClient.Soc.Connected)
                                    C.Step();
                                else if (C.LastLogin.AddSeconds(8) < DateTime.Now)
                                {
                                    World.DebugAdd += C.Name + " got automatically kicked! " + DateTime.Now + "\r\n";
                                    C.MyClient.LogOff();
                                }
                            }
                        }

                    }
                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }

                //}
            }


        }
        /* static void EndSend3()
        {
            if (Chars.Length > 0 && CharReady)
            {
                for (int x = (int)(Chars.Length / 1.5); x < Chars.Length; x++)
                {
                    Character C = Chars[x];
                    try
                    {
                        C.MyClient.EndSend();
                    }
                    catch { }
                }
            }
        }*/
        static void Attacks()
        {
            if (World.H_Chars.Count > 0)
            {
                /*if (ThreadInfo[3].Modified)
                {
                    lock (ThreadInfo[3].Array)
                    {
                        ThreadInfo[3].Array = World.H_Chars.Values.ToArray();
                       // ThreadInfo[3].Array = new Character[World.H_Chars.Count];
                      //  World.H_Chars.Values.CopyTo(ThreadInfo[3].Array, 0);
                        ThreadInfo[3].Modified = false;
                    }
                }*/

                //if (ThreadInfo != null)
                //{


                try
                {
                    foreach (Character C in /*ThreadInfo*/World.H_Chars.Values) // 0 to length/3   for (int x = 0; x < ThreadInfo.Length / 3; x++) 
                    {
                        //Character C = ThreadInfo[x];//altfel... poti sa faci altfel 
                        if (!EndSession)
                            if (C != null)
                                if (C.MyClient.Soc.Connected)
                                    if (C.AtkMem.Attacking)
                                        C.Attack();

                    }

                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
                //}

            }

        }
        static void MobThread_Execute()
        {
            if (World.H_Chars.Count > 0)
            {
                try
                {
                    /*foreach (Hashtable H in World.H_Mobs.Values)
                    {
                        foreach (Mob M in H.Values)
                            if (!M.Alive || M.MobID == 701 || M.MobID == 244 || M.MobID == 247 || M.MobID == 300)
                                M.Step();
                    }*/
                    foreach (uint Map in World.H_Mobs.Keys)//  foreach (uint Map in World.PlayersInMap.Keys)
                    {
                        // if (World.H_Mobs.ContainsKey(Map))
                        //{
                        if (World.PlayersInMap[Map].Count > 0 || Map == 1038 || Map == 1004 || Map == 1002 || Map == 1051 || ((Map == 1020 || Map == 1011) && (DateTime.Now.Minute >= 14 && DateTime.Now.Minute <= 17)))
                        {
                            //Mob[] Mobs = null;
                            //if (H.Count > 0)
                            //{
                            //    Mobs = new Mob[H.Count];
                            //    H.Values.CopyTo(Mobs, 0);
                            //}

                            foreach (Mob M in World.H_Mobs[Map].Values)
                                if (!M.Alive || M.MobID == 701 || M.MobID == 244 || M.MobID == 247 || M.MobID == 300 || M.MobID == 8423 || M.MobID == 8424 || M.MobID == 501)
                                    M.Step();

                            //lock (H)
                            //{
                            //    foreach (Mob M in H.Values)
                            //        if (!M.Alive || M.MobID == 701 || M.MobID == 244 || M.MobID == 247 || M.MobID == 300 || M.MobID == 8423 || M.MobID == 8424 || M.MobID == 501)
                            //            M.Step();
                            //}

                        }
                        //}
                    }
                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
            }
        }
        static void MobAttack_Execute()
        {
            if (World.H_Chars.Count > 0)
            {
                foreach (uint Map in World.H_Mobs.Keys) // foreach (uint Map in World.PlayersInMap.Keys)
                {
                    // if (World.H_Mobs.ContainsKey(Map))
                    //{
                    if (World.PlayersInMap[Map].Count > 0)
                    {
                        foreach (Mob M in World.H_Mobs[Map].Values)
                            if (M.Alive && DateTime.Now >= M.LastMove)
                                //if (M.Alive && DateTime.Now >= M.LastMove.AddMilliseconds(1000))
                                if (M.MobID != 150 && M.MobID != 151 && M.MobID != 8423 && M.MobID != 8424 && M.MobID != 702 && M.MobID != 703 && M.MobID != 704 && M.MobID != 500)
                                    M.Attack();
                    }
                    //  }
                }
                /* if (ThreadInfo != null)
                 {
                     List<ushort> Maps = new List<ushort>();
                     foreach (Character C in ThreadInfo)
                     {
                         if (C != null)
                         {
                             if (!Maps.Contains(C.Loc.Map))
                             {
                                 if (World.H_Mobs.ContainsKey(C.Loc.Map))
                                 {
                                     Hashtable H = (Hashtable)World.H_Mobs[C.Loc.Map];
                                     foreach (Mob M in H.Values)
                                     {
                                         if (M.Alive && DateTime.Now > M.LastMove.AddMilliseconds(1000))
                                             if (M.MobID != 150 && M.MobID != 702 && M.MobID != 703)
                                                 M.Attack();
                                     }
                                 }
                                 Maps.Add(C.Loc.Map);
                             }
                         }
                     }
                 }/*
             }


             /* foreach (Hashtable H in World.H_Mobs.Values)
              {

                  foreach (Mob M in H.Values)
                  {
                      if (M.Alive && DateTime.Now > M.LastMove.AddMilliseconds(1000))
                          M.Attack();
                  }


              }*/
            }
        }
        static void TakeChrOFF_Execute()
        {
            try
            {
#warning ThreadInfo
                //if (World.H_Chars.Count > 0)
                //    lock (ThreadInfo)
                //    {
                //        ThreadInfo = World.H_Chars.Values.ToArray();
                //        // ThreadInfo[0].Array = new Character[World.H_Chars.Count];
                //        // World.H_Chars.Values.CopyTo(ThreadInfo[0].Array, 0);
                //        //ThreadInfo = false;
                //    }
                //else ThreadInfo = new Character[0];


                /*Hashtable TempClients = World.H_Clients;
                foreach (Main.GameClient C in TempClients.Values)
                    if ((!C.Soc.Connected || C.Soc == null || C == null) && !World.Exit)
                    {
                        if (C.MyChar.LoggedOn.AddMilliseconds(5000) < DateTime.Now)
                        {
                            Program.WriteLine(C.MyChar.Name + " got automatically kicked!");
                            C.LogOff();
                        }
                    }*/

                /* Hashtable TempChars = World.H_Clients;
                 foreach (GameClient C in TempChars.Values)
                 {
                     try
                     {
                         if (C != null)
                         {
                             if ((!C.Soc.Connected || C.Soc == null) && !World.Exit)
                             {

                                 if (C.MyChar.LoggedOn.AddMilliseconds(5000) < DateTime.Now)
                                 {
                                     World.DebugAdd += C.MyChar.Name + " got automatically kicked! \r\n";
                                     C.LogOff();
                                 }
                             }
                         }
                     }
                     catch { }
                    // System.Threading.Thread.Sleep(1);
                 }*/
#warning ThreadInfo
                foreach (Character C in /*ThreadInfo*/World.H_Chars.Values)
                {
                    /* if (C == null || C.MyClient == null)
                     {
                         C.MyClient.Soc.Disconnect(false);
                         World.DebugAdd += C.Name + " got automatically kicked! (soc disconnected) " + DateTime.Now + "\r\n";
                         C.MyClient.LogOff();
                     }*/
                    if (C != null)
                    {
                        if (!C.MyClient.Soc.Connected)
                        {
                            if (C.LastLogin.AddSeconds(8) < DateTime.Now)
                            {
                                World.DebugAdd += C.Name + " got automatically kicked! " + DateTime.Now + "\r\n";
                                // Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                                C.MyClient.LogOff();
                            }
                        }
                    }
                }


            }
            catch (Exception E) { Game.World.ExcAdd += "TakeOFF error: " + E.ToString() + "\r\n"; }
        }
        static void Events_Execute()
        {
            try
            {
                if (World.InfoAdd.Length > 700000 || World.ExcAdd.Length > 700000 || World.ChatAdd.Length > 700000 || World.DebugAdd.Length > 700000 || World.DropAdd.Length > 700000 || World.TradeAdd.Length > 700000)
                {
                    WriteLogs();
                }
            }
            catch { }
            try
            {
                if (World.SaveGuilds)
                {
                    Features.Guilds.SaveGuilds();
                    World.SaveGuilds = false;
                }
            }
            catch { }
            #region Hourly Bonus
            try
            {
                if (DateTime.Now.Minute == 00)
                {
                    string Drop;
                    if ((DateTime.Now.Hour == 0 || DateTime.Now.Hour == 8 || DateTime.Now.Hour == 16) && !World.EventDB)
                    {
                        World.EventDB = true;
                        World.EventElite = false;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Dragonball drop rate";
                    }
                    else if ((DateTime.Now.Hour == 1 || DateTime.Now.Hour == 9 || DateTime.Now.Hour == 17) && !World.EventElite)
                    {
                        World.EventDB = false;
                        World.EventElite = true;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Egg items drop rate";
                    }
                    else if ((DateTime.Now.Hour == 2 || DateTime.Now.Hour == 10 || DateTime.Now.Hour == 18) && !World.EventGem)
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = true;
                        World.EventMet = false;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Gem mining rate";
                    }
                    else if ((DateTime.Now.Hour == 3 || DateTime.Now.Hour == 11 || DateTime.Now.Hour == 19) && !World.EventMet)
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = false;
                        World.EventMet = true;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Meteor drop rate";
                    }
                    else if ((DateTime.Now.Hour == 4 || DateTime.Now.Hour == 12 || DateTime.Now.Hour == 20) && !World.EventPlus)
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventPlus = true;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        Drop = "+1 Items drop rate";
                    }
                    else if ((DateTime.Now.Hour == 5 || DateTime.Now.Hour == 13 || DateTime.Now.Hour == 21) && !World.EventProfExp)
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventProfExp = true;
                        World.EventSkillExp = false;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Weapon Proficiency exp rate";
                    }
                    else if ((DateTime.Now.Hour == 6 || DateTime.Now.Hour == 14 || DateTime.Now.Hour == 22) && !World.EventSkillExp)
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventProfExp = false;
                        World.EventSkillExp = true;
                        World.EventSuper = false;
                        World.EventPlus = false;
                        Drop = "Skill exp rate";
                    }
                    else /*if ((DateTime.Now.Hour == 7 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 23) && !World.EventSuper)*/
                    {
                        World.EventElite = false;
                        World.EventDB = false;
                        World.EventGem = false;
                        World.EventMet = false;
                        World.EventProfExp = false;
                        World.EventSkillExp = false;
                        World.EventSuper = true;
                        World.EventPlus = false;
                        Drop = "Super Items drop rate";
                    }
                    var Message = Drop + " has been increased for the next hour!";
                    World.SendMsgToAll("[EVENT]", Message, 2500, 0);
                }
            }
            catch { }
            #endregion
            #region TeamPKTournament
            /*try
            {
                if (!Features.TeamPKTourny.Started70To99 && !Features.TeamPKTourny.Started100To115 && !Features.TeamPKTourny.Started116To130)
                {
                    if (ServerTime.DayName == "Monday")
                    {
                        if (ServerTime.Minute == 0 && ServerTime.Hour == 6 && ServerTime.AMPM == "PM")
                        {
                            Features.TeamPKTourny.StartTourny();
                        }
                        else if (ServerTime.Hour == 5 && ServerTime.AMPM == "PM")
                        {
                            if (ServerTime.Minute == 30)
                                World.SendMsgToAll("Team PK", "Team PK tournament will start in 30 minutes!", 2011, 0);
                            else if (ServerTime.Minute == 40) World.SendMsgToAll("Team PK", "Team PK tournament will start in 20 minutes!", 2011, 0);
                            else if (ServerTime.Minute == 45) World.SendMsgToAll("Team PK", "Team PK tournament will start in 15 minutes!", 2011, 0);
                            else if (ServerTime.Minute == 50) World.SendMsgToAll("Team PK", "Team PK tournament will start in 10 minutes!", 2011, 0);
                            else if (ServerTime.Minute == 55) World.SendMsgToAll("Team PK", "Team PK tournament will start in 5 minutes! Hurry up and join your team in the queue!", 2011, 0);
                            else if (ServerTime.Minute == 58) World.SendMsgToAll("Team PK", "Team PK tournament will start in 2 minutes! Hurry up and join your team in the queue!", 2011, 0);
                        }
                    }
                }
                else { Features.TeamPKTourny.CheckEndTourny(); }
            } 
            catch { } */
            #endregion
            #region Check Broadcasts
            try
            {
                if (World.BroadCastCount > 0 && DateTime.Now > World.LastBroadCast.AddMinutes(1))
                {
                    BroadCastMessage B = World.BroadCasts[0];

                    for (int i = 0; i < World.BroadCastCount; i++)
                        World.BroadCasts[i] = World.BroadCasts[i + 1];

                    World.BroadCastCount--;

                    World.SendMsgToAll(B.Name, B.Message, 2500, 0);
                    World.LastBroadCast = DateTime.Now;
                    World.CurrentBC = B;
                }
            }
            catch { }
            #endregion
            #region Interserver - Disabled
            //try
            //{
            //    if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            //    {
            //        if (DateTime.Now.Hour == 17 && (DateTime.Now.Minute == 5 || DateTime.Now.Minute == 25 || DateTime.Now.Minute == 45))
            //        {
            //            foreach (Game.Character C in Game.World.H_Chars.Values)
            //            {
            //                C.MyClient.DialogNPC = 2027;
            //                NPCs.NPCHandler.Handle(C.MyClient, null, 2027, 0);
            //            }
            //        }
            //        else if (DateTime.Now.Hour == 18 && (DateTime.Now.Minute == 5 || DateTime.Now.Minute == 25 || DateTime.Now.Minute == 45))
            //        {
            //            foreach (Game.Character C in Game.World.H_Chars.Values)
            //            {
            //                C.MyClient.DialogNPC = 2027;
            //                NPCs.NPCHandler.Handle(C.MyClient, null, 2027, 0);
            //            }
            //        }
            //    }
            //}
            //catch { }
            #endregion
            #region AntiBot
            /* try
             {
                 if (ServerTime.Minute == 45 && World.BOTSEND == false)
                 {
                     World.BOTSEND = true;
                     foreach (Game.Character Char in Game.World.H_Chars.Values)
                     {
                         Char.MyClient.DialogNPC = 13652;
                         PacketHandling.NPCDialog.Handle(Char.MyClient, null, 13652, 0);
                     }
                 }
             }
             catch { }*/
            /*  if (ServerTime.Minute == 40)
                  World.BOTSEND = false;*/
            //try
            //{
            //    if (ServerTime.Minute == 36)
            //    {
            //        World.Titan = false;
            //        World.Gano = false;
            //    }
            //}

            //catch { }
            #endregion

            #region ArtisanChanger
            try
            {

                if (DateTime.Now.Minute == 4 || DateTime.Now.Minute == 19 || DateTime.Now.Minute == 34 || DateTime.Now.Minute == 49)
                {
                    ushort X;
                    X = (ushort)Program.Rnd.Next(10, 399);
                    foreach (Character C in World.H_Chars.Values)
                        if (World.H_NPCs.ContainsKey(1002))

                            if (World.H_NPCs[1002].ContainsKey(10019))
                            {
                                var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                {
                                    cmd.Update("npcs")
                                        .Set("Type", X)
                                        .Where("UID", 10019);
                                    cmd.Execute();
                                }
                                var npc = World.H_NPCs[1002][10019];
                                npc.Type = X;
                                World.Spawn(C, true);
                                
                            }
                }


            }
            catch { }
            #endregion

            #region Hourly PVP Events
            try
            {
                if (DateTime.Now.Minute == 38)
                {
                    byte _totalEvents = 9;
                    int _nextEvent = Program.Rnd.Next(0, _totalEvents);

                    while (World.WorldEvent == _nextEvent)
                        _nextEvent = Program.Rnd.Next(0, _totalEvents);

                    World.WorldEvent = _nextEvent;
                    Events.Events NextEvent = new Ultimate.Events.Events();
                    switch (_nextEvent)
                    {
                        case 8:
                           NextEvent = new Vampire_War();
                           break;
                       // case 5:
                         //   NextEvent = new MeteorShower();
                           // break;
                        //case 2:
                        //    NextEvent = new SkillPK();
                        //    break;
                        case 0:
                            NextEvent = new LastManStanding();
                            break;
                        case 1:
                            NextEvent = new KOTH();
                            break;
                        case 5:
                            NextEvent = new Infection();
                           break;
                        case 2:
                            NextEvent = new PTB();
                            break;
                        case 3:
                            NextEvent = new FreezeWar();
                            break;
                        case 4:
                            NextEvent = new DragonWar();
                            break;
                        //case 9:
                        //    NextEvent = new CycloneWar();
                        //    break;
                        // case 5:
                        //   NextEvent = new ElitePK();
                        // break;
                        //case 9:
                        //    NextEvent = new PimpOutSanta();
                        //    break;

                        //case 6:
                        //    NextEvent = new Football();
                        //    break;

                        case 7:
                            NextEvent = new MeteorShower();
                            break;

                            //case 8:
                            //    NextEvent = new TDM();
                            //    break;

                            //case 11:
                            //    NextEvent = new WackaMoleHalloween();
                            //    break;
                            //    World.HourlyEvent = new PimpOutSanta();
                    }
                    NextEvent.StartTournament();

                    //if (World.HourlyEvent == null || World.HourlyEvent.Stage == EventStage.None || World.IgnoreNull)
                    //{

                    //    //World.HourlyEvent.StartTournament();
                    //}
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The " + NextEvent + " was started. If you want to join please use @joinpvp command!```";
                }
            }
            catch { }
            #endregion
            #region Skill Championship
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 00)
                    {
                        Events.SkillChampionship S = new Events.SkillChampionship();
                        S.StartTournament();
                    }
                    else if (DateTime.Now.Hour == 14)
                    {
                        if (DateTime.Now.Minute == 45)
                        {
                            Thread FBReset = new Thread(Database.TopFBReset);
                            FBReset.Start();
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 14:00! Be ready!", 2011, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```Skill Championship Event will start at 14:00! Be ready!```";
                        }
                        else if (DateTime.Now.Minute == 50)
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 14:00! Be ready!", 2011, 0);
                        else if (DateTime.Now.Minute == 57)
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 14:00! Be ready!", 2011, 0);

                    }
                }
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 00)
                    {
                        Events.SkillChampionship S = new Events.SkillChampionship();
                        S.StartTournament();
                    }
                    else if (DateTime.Now.Hour == 22)
                    {
                        if (DateTime.Now.Minute == 45)
                        {
                            Thread FBReset = new Thread(Database.TopFBReset);
                            FBReset.Start();
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 23:00! Be ready!", 2011, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```Skill Championship Event will start at 23:00! Be ready!```";
                        }
                        else if (DateTime.Now.Minute == 50)
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 23:00! Be ready!", 2011, 0);
                        else if (DateTime.Now.Minute == 57)
                            World.SendMsgToAll("[EVENT]", "Skill Championship Event will start at 23:00! Be ready!", 2011, 0);

                    }
                }
            }
            catch { }
            #endregion
            #region //Team Deathmatch
            //try
            //{
            //    if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            //    {
            //        if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 00)
            //        {
            //            if (!Features.TDM.Signup && !Features.TDM.War)
            //            {
            //                Features.TDM TeamDeathMatch = new Features.TDM();
            //                TeamDeathMatch.StartTournament();
            //            }
            //        }
            //        else if (DateTime.Now.Hour == 15)
            //        {
            //            if (DateTime.Now.Minute == 45)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 16:00! Be ready!", 2011, 0);
            //            else if (DateTime.Now.Minute == 50)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 16:00! Be ready!", 2011, 0);
            //            else if (DateTime.Now.Minute == 57)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 16:00! Be ready!", 2011, 0);

            //        }
            //    }
            //    else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 00)
            //        {
            //            if (!Features.TDM.Signup && !Features.TDM.War)
            //            {
            //                TDM.StartTournament();
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event has started! Type /joinpvp if you want to join!", 2011, 0);
            //            }
            //        }
            //        else if (DateTime.Now.Hour == 22)
            //        {
            //            if (DateTime.Now.Minute == 45)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 23:00! Be ready!", 2011, 0);
            //            else if (DateTime.Now.Minute == 50)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 23:00! Be ready!", 2011, 0);
            //            else if (DateTime.Now.Minute == 57)
            //                World.SendMsgToAll("[EVENT]", "Team Deathmatch Event will start at 23:00! Be ready!", 2011, 0);

            //        }
            //    }
            //}
            //catch { }
            #endregion
            #region CTB Event
            try
            {
                if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 22)
                {
                    if (DateTime.Now.Minute == 57)
                    {
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```The Capture the Bag Event has been started! You can get ready to fight!```";
                        Events.Events CaptureTheBag = new CaptureTheBag();
                        CaptureTheBag.StartTournament(180);
                    }
                    else if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "Capture the Bag Event will start in 30 minutes! Find CTBFlag in TwinCity to join!", 2011, 0);
                    else if (DateTime.Now.Minute == 50)
                        World.SendMsgToAll("SYSTEM", "Capture the Bag Event will start in 10 minutes! Find CTBFlag in TwinCity to join!", 2011, 0);
                    else if (DateTime.Now.Minute == 55)
                        World.SendMsgToAll("SYSTEM", "Capture the Bag Event will start in 5 minutes! Find CTBFlag in TwinCity to join!", 2011, 0);
                }
            }
            catch { }

            #endregion
            #region GameOfThrones War
            //try
            //{
            //    if (World.GOTWar)
            //    {
            //        World.GOTWar = false;
            //        GameOfThones.Start();
            //    }
            //}
            //catch
            //{

            //}
            #endregion
            #region GW StartEnd
            try
            {
                if (Features.GuildWars.War)
                {
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (DateTime.Now.Minute == 0)
                        {
                            if (World.LowRatedServer)
                            {
                                if (DateTime.Now.Hour == 7) // old server
                                {
                                    Features.GuildWars.EndWarForGood();  // old server
                                }
                                else if (DateTime.Now.Hour == 5) // old server
                                    World.SendMsgToAll("SYSTEM", "Guild war ends in 2 hours! /guildwar", 2011, 0);  // old server
                            }
                            else
                            {
                                if (DateTime.Now.Hour == 19)
                                    Features.GuildWars.EndWarForGood();
                                else if (DateTime.Now.Hour == 18)
                                    World.SendMsgToAll("SYSTEM", "Guild war ends in 1 hours! /guildwar", 2011, 0);
                                else if (DateTime.Now.Hour == 17)
                                    World.SendMsgToAll("SYSTEM", "Guild war ends in 2 hours! /guildwar", 2011, 0);
                                else if (DateTime.Now.Hour == 16)
                                    World.SendMsgToAll("SYSTEM", "Guild war ends in 3 hours! /guildwar", 2011, 0);
                            }
                        }
                    }
                }

            }
            catch { }
            try
            {
                if (!Features.GuildWars.War)
                {
                    if (World.LowRatedServer) // low server
                    {
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || (DateTime.Now.DayOfWeek == DayOfWeek.Friday && DateTime.Now.Hour > 20) || (DateTime.Now.Hour < 21 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
                        {
                            Features.GuildWars.StartWar(); // low server
                            Features.GuildWars.War = true; // low server
                        }
                    }
                    else
                    {
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || (DateTime.Now.DayOfWeek == DayOfWeek.Friday && DateTime.Now.Hour > 20) || (DateTime.Now.Hour < 19 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
                        {
                            Features.GuildWars.StartWar();
                            Features.GuildWars.War = true;
                        }
                    }
                }
            }
            catch { }
            #endregion
            #region Counter Clock GW StartEnd
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (Features.CounterClock.War)
                    {
                        if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 0)
                            Features.CounterClock.EndWarForGood();
                        else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 0)
                            World.SendMsgToAll("SYSTEM", "Counter Clock Guild War ends in 1 hour!", 2011, 0);
                    }
                    else
                    {
                        if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 5)
                            World.SendMsgToAll("WAR", "The Counter Clock Guild War will start at 17:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 30)
                            World.SendMsgToAll("WAR", "The Counter Clock Guild War will start at 17:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 40)
                            World.SendMsgToAll("WAR", "6The Counter Clock Guild War will start at 17:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 50)
                            World.SendMsgToAll("WAR", "The Counter Clock Guild War will start at 17:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 57)
                            World.SendMsgToAll("WAR", "The Counter Clock Guild War will start at 17:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 0)
                        {
                            Features.CounterClock.StartWar();
                            Features.CounterClock.War = true;
                            Game.World.SendMsgToAll("WAR", "You can get ready to fight! Counter Clock GW is about to start! Speak to ObscureWarrior in TwinCity to join!", 2011, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```Counter Clock GW is about to start! Speak to ObscureWarrior in TwinCity to join!```";
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region TwinCityGuildWar
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    if (Features.TCGuildWars.War)
                    {
                        if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00)
                        {
                            Features.TCGuildWars.EndWarForGood();
                        }
                        else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 30)
                            World.SendMsgToAll("SYSTEM", "TwinCity War ends in 30 Minute!", 2011, 0);

                    }
                    else
                    {
                        if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 5)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 30)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 40)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 50)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 57)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 1)
                        {
                            Features.TCGuildWars.StartWar();
                            Features.TCGuildWars.War = true;

                            World.SendMsgToAll("WAR", "The TwinCity Guild War has been started! You can get ready to fight!", 2011, 0);
                            Game.World.SendMsgToAll("WAR", "You can get ready to fight! TwinCity GW is about to start! Speak to TwinCity NPC in EveryMap to join!", 2000, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```The TwinCity Guild War has been started! You can get ready to fight!```";
                        }
                    }
                }
            }
            catch { }
            #endregion
            #region PoleWarTC
            try
            {
                if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 56)
                    World.SendMsgToAll("WAR", "The TwinCity Pole Domination will start at 16:00! Get yourself ready!", 2011, 0);
                else if (DateTime.Now.Hour == 16 && DateTime.Now.Minute == 0)
                {
                    Features.PoleWarTC.StartWar();
                    Features.PoleWarTC.War = true;

                    World.SendMsgToAll("WAR", "The TwinCity Pole Domination has been started! You can get ready to fight!", 2011, 0);
                    Game.World.SendMsgToAll("WAR", "You can get ready to fight! TwinCity Pole Domination is about to start!", 2000, 0);
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The TwinCity Pole Domination has been started!```";
                    foreach (Character C in World.H_Chars.Values)
                    {
                        C.MyClient.DialogNPC = 2110;
                        if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                        Bosses.BossHandler.WindowInformation(C);
                        NPCs.NPCHandler.Handle(C.MyClient, null, 2110, 0);
                    }

                }
            }
            catch { }
            #endregion

            #region PoleWarPC
            try
            {
                if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 56)
                    World.SendMsgToAll("WAR", "The PhoenixCastle Pole Domination will start at 16:00! Get yourself ready!", 2011, 0);
                else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 0)
                {
                    Features.PoleWarPC.StartWar();
                    Features.PoleWarPC.War = true;

                    World.SendMsgToAll("WAR", "The PhoenixCastle Pole Domination has been started! You can get ready to fight!", 2011, 0);
                    Game.World.SendMsgToAll("WAR", "You can get ready to fight! PhoenixCastle Pole Domination is about to start!", 2000, 0);
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The PhoenixCastle Pole Domination has been started!```";
                    foreach (Character C in World.H_Chars.Values)
                    {
                        C.MyClient.DialogNPC = 2111;
                        if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                        Bosses.BossHandler.WindowInformation(C);
                        NPCs.NPCHandler.Handle(C.MyClient, null, 2111, 0);
                    }

                }
            }
            catch { }
            #endregion

            #region PoleWarAC
            try
            {
                if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 56)
                    World.SendMsgToAll("WAR", "The ApeCity Pole Domination will start at 16:00! Get yourself ready!", 2011, 0);
                else if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 0)
                {
                    Features.PoleWarAC.StartWar();
                    Features.PoleWarAC.War = true;

                    World.SendMsgToAll("WAR", "The ApeCity Pole Domination has been started! You can get ready to fight!", 2011, 0);
                    Game.World.SendMsgToAll("WAR", "You can get ready to fight! ApeCity Pole Domination is about to start!", 2000, 0);
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The ApeCity Pole Domination has been started!```";
                    foreach (Character C in World.H_Chars.Values)
                    {
                        C.MyClient.DialogNPC = 2112;
                        if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                        Bosses.BossHandler.WindowInformation(C);
                        NPCs.NPCHandler.Handle(C.MyClient, null, 2112, 0);
                    }

                }
            }
            catch { }
            #endregion

            #region PoleWarDC
            try
            {
                if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 56)
                    World.SendMsgToAll("WAR", "The DesertCity Pole Domination will start at 16:00! Get yourself ready!", 2011, 0);
                else if (DateTime.Now.Hour == 22 && DateTime.Now.Minute == 0)
                {
                    Features.PoleWarDC.StartWar();
                    Features.PoleWarDC.War = true;

                    World.SendMsgToAll("WAR", "The DesertCity Pole Domination has been started! You can get ready to fight!", 2011, 0);
                    Game.World.SendMsgToAll("WAR", "You can get ready to fight! DesertCity Pole Domination is about to start!", 2000, 0);
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The DesertCity Pole Domination has been started!```";
                    foreach (Character C in World.H_Chars.Values)
                    {
                        C.MyClient.DialogNPC = 2113;
                        if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                        Bosses.BossHandler.WindowInformation(C);
                        NPCs.NPCHandler.Handle(C.MyClient, null, 2113, 0);
                    }

                }
            }
            catch { }
            #endregion

            #region PoleWarBI
            try
            {
                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 56)
                    World.SendMsgToAll("WAR", "The BirdIsland Pole Domination will start at 16:00! Get yourself ready!", 2011, 0);
                else if (DateTime.Now.Hour == 00 && DateTime.Now.Minute == 0)
                {
                    Features.PoleWarBI.StartWar();
                    Features.PoleWarBI.War = true;

                    World.SendMsgToAll("WAR", "The BirdIsland Pole Domination has been started! You can get ready to fight!", 2011, 0);
                    Game.World.SendMsgToAll("WAR", "You can get ready to fight! BirdIsland Pole Domination is about to start!", 2000, 0);
                    Discord DCord = new Discord();
                    DCord.MesajVer7 = "```The BirdIsland Pole Domination has been started!```";
                    foreach (Character C in World.H_Chars.Values)
                    {
                        C.MyClient.DialogNPC = 2114;
                        if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                        Bosses.BossHandler.WindowInformation(C);
                        NPCs.NPCHandler.Handle(C.MyClient, null, 2114, 0);
                    }

                }
            }
            catch { }
            #endregion
            #region CityWarStartEnd
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    if (Features.CityWarTc.War || Features.CityWarPc.War || Features.CityWarAc.War || Features.CityWarDc.War || Features.CityWarBi.War)
                    {
                        if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00)
                        {
                            Features.CityWarTc.EndWarForGood();
                            Features.CityWarPc.EndWarForGood();
                            Features.CityWarAc.EndWarForGood();
                            Features.CityWarDc.EndWarForGood();
                            Features.CityWarBi.EndWarForGood();
                        }
                        else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 30)
                            World.SendMsgToAll("SYSTEM", "City War ends in 30 Minute!", 2011, 0);

                    }
                    else
                    {
                        if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 5)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 30)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 40)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 50)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 57)
                            World.SendMsgToAll("WAR", "The City Guild War will start at 18:00! Get yourself ready!", 2011, 0);
                        else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 1)
                        {
                            Features.CityWarTc.StartWar();
                            Features.CityWarPc.StartWar();
                            Features.CityWarAc.StartWar();
                            Features.CityWarDc.StartWar();
                            Features.CityWarBi.StartWar();
                            Features.CityWarTc.War = true;
                            Features.CityWarPc.War = true;
                            Features.CityWarAc.War = true;
                            Features.CityWarDc.War = true;
                            Features.CityWarBi.War = true;
                            World.SendMsgToAll("WAR", "The City Guild War has been started! You can get ready to fight!", 2011, 0);
                            Game.World.SendMsgToAll("WAR", "You can get ready to fight! CityWar GW is about to start! Speak to CityWar NPC in EveryMap to join!", 2000, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```The City Guild War has been started! You can get ready to fight!```";
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region WeeklyPK
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    //if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 55)
                    //{
                    //    Features.WeeklyPKTournament WeeklyPKTournament = new Features.WeeklyPKTournament();
                    //    WeeklyPKTournament.BeginTournament();
                    //    World.SendMsgToAll("WeeklyPK", Features.WeeklyPKTournament.EventTitle + " has started! Find GeneralBravery in TwinCity before 22:00 to join!", 2500, 0);
                    //}
                    //else if (DateTime.Now.Hour == 21 && DateTime.Now.Minute == 30)
                    //{
                    //    if (!Features.WeeklyPKTournament.SignUp)
                    //    {
                    //        World.SendMsgToAll("WeeklyPK", Features.WeeklyPKTournament.EventTitle + " will start at 22:00 ! Find GeneralBravery in TwinCity and get ready to fight!", 2500, 0);
                    //    }
                    //}
                }
            }
            catch { }
            #endregion
            #region Couples PK Tournament
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    if (DateTime.Now.Hour == 22 && DateTime.Now.Minute == 55)
                    {
                        Features.TopSpouse CouplesPK = new Features.TopSpouse();
                        CouplesPK.BeginTournament();
                    }
                    else if (DateTime.Now.Hour == 22 && DateTime.Now.Minute == 30)
                    {
                        World.SendMsgToAll("SYSTEM", "Couple's PK Tournament will start in 30 minutes! Find CouplesPKGuide near the PKArena and team up with your lover to join!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Couple's PK Tournament will start in 30 minutes! Find CouplesPKGuide near the PKArena and team up with your lover to join!", 2005, 0);
                        World.SendMsgToAll("SYSTEM", "Couple's PK Tournament will start in 30 minutes! Find CouplesPKGuide near the PKArena and team up with your lover to join!", 2000, 0);
                    }
                }
            }
            catch { }
            #endregion
            #region ClassPK Tourn
            try
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    if ((DateTime.Now.Hour == 9 || DateTime.Now.Hour == 21) && DateTime.Now.Minute == 55)
                    {
                        Events.Events ClassPK = new Ultimate.Events.ClassPK();
                        ClassPK.StartTournament(300);
                    }
                }
            }
            catch { }
            #endregion
            #region UpdateTops, VIPAura, Squamas and Online Points
            try
            {
                if (DateTime.Now.Minute == 0 || DateTime.Now.Minute == 30)
                {

                    #region save Tops
                    foreach (Character C in World.H_Chars.Values)
                    {
                        string Nobility;
                        if (C.Nobility.Rank == Ranks.Duke)
                            if (C.Body == 1003 || C.Body == 1004)
                                Nobility = "Duke";
                            else
                                Nobility = "Duchess";
                        else if (C.Nobility.Rank == Ranks.Prince)
                            if (C.Body == 1003 || C.Body == 1004)
                                Nobility = "Prince";
                            else
                                Nobility = "Princess";
                        else if (C.Nobility.Rank == Ranks.King)
                            if (C.Body == 1003 || C.Body == 1004)
                                Nobility = "King";
                            else
                                Nobility = "Queen";
                        else if (C.Nobility.Rank == Ranks.Knight)
                            Nobility = "Knight";
                        else if (C.Nobility.Rank == Ranks.Baron)
                            if (C.Body == 1003 || C.Body == 1004)
                                Nobility = "Baron";
                            else
                                Nobility = "Baroness";
                        else if (C.Nobility.Rank == Ranks.Earl)
                            if (C.Body == 1003 || C.Body == 1004)
                                Nobility = "Earl";
                            else
                                Nobility = "Countess";
                        else
                            Nobility = "Serf";
                        try
                        {
                            MySQL.MySqlCommand Top = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                            Top.Update("charstats").Set("Level", C.Level).Set("Potency", C.Potency).Set("Nobility", Nobility).Set("PKPoints", C.PKPoints).Set("VirtuePoints", C.VP).Set("Gold", ((C.Silvers + C.WHSilvers))).Set("OnlineTime", C.OnlineTime).Set("Job", C.Job).Set("VipDays", C.VIPDays).Set("VipLevel", C.VipLevel).Set("Spouse", C.Spouse).Set("Face", C.Avatar).Set("GuildName", C.MyGuild.GuildName).Where("Name", C.Name).Execute();
                            using (var session = NHibernateHelper.OpenSession())
                            {
                                var t = session.CreateSQLQuery("UPDATE charstats SET Level= " + C.Level + ", Potency=" + C.Potency + ", Nobility=" + Nobility + ", PKPoints=" + C.PKPoints + ", VirtuePoints=" + C.VP + ", Gold=" + ((C.Silvers + C.WHSilvers)) + ", OnlineTime=" + C.OnlineTime + ", Job=" + C.Job + ", VipDays= " + C.VIPDays + ", VipLevel=" + C.VipLevel + ", Spouse=" + C.Spouse + ", Face=" + C.Avatar + ", GuildName=" + C.MyGuild.GuildName);
                                t.ExecuteUpdate();
                            }
                        }
                        catch { }
                    }
                    #endregion

                   

                    if (DateTime.Now.Hour == 11)
                    {

                        Thread SaveNobility = new Thread(Database.SaveEmpire);
                        SaveNobility.Start();
                    }
                    else if (DateTime.Now.Hour == 23)
                    {
                        Thread LowerChars2 = new Thread(Database.LowerVote);
                        LowerChars2.Start();

                        Thread SaveNobility = new Thread(Database.SaveEmpire);
                        SaveNobility.Start();
                    }
                    else if (DateTime.Now.Hour == 20)
                    {
                        World.SendMsgToAll("SYSTEM", "Squamas have spawned around the world ! Make sure you find them for great rewards!", 2011, 0);
                        World.SendMsgToAll("SYSTEM", "Squamas have spawned around the world ! Make sure you find them for great rewards!", 2000, 0);
                        World.SendMsgToAll("SYSTEM", "Squamas have spawned around the world ! Make sure you find them for great rewards!", 2005, 0);
                        Database.LoadSquamas(new Game.MapEffect(), true);
                    }
                    World.UnlimitedStaminaMap = 0;
                }
                else if (DateTime.Now.Minute == 15)
                {
                    World.UnlimitedStaminaMap = 1005;
                    World.SendMsgToAll("SYSTEM", "Stamina costs have been removed for 15 minutes at Arena! Enjoy!", 2000, 0);
                }
                else if (DateTime.Now.Minute == 30)
                    World.UnlimitedStaminaMap = 0;
                else if (DateTime.Now.Minute == 45)
                {
                    World.UnlimitedStaminaMap = 6000;
                    World.SendMsgToAll("SYSTEM", "Stamina costs have been removed for 15 minutes at Jail! Enjoy!", 2000, 0);
                }
                int servermin = DateTime.Now.Minute;
                if (servermin % 1 == 0 || servermin % 1 == 1)
                {
                    foreach (Character C in Game.World.H_Chars.Values)
                    {
                        if (C.VipLevel > 0)
                        {
                            if (C.VIPAura)
                            {
                                C.MyClient.MyChar.StatEff.Add(Game.StatusEffectEn.TopNinja);
                            }
                        }
                        else
                        {
                            C.MyClient.MyChar.StatEff.Remove(Game.StatusEffectEn.TopNinja);
                        }

                        if (C.Level < 50) continue; // cancel attempt if under level 50.
                        if (DateTime.Now >= C.LoginTime.AddHours(1))
                        {
                            C.LoginTime = DateTime.Now;
                            C.ClassicPoints++;
                            C.OnlineTime += 1;
                            C.MyClient.LocalMessage(2011, "You have earned 1 Online Point for being online! You can use your online points to exchange them for rewards.");
                        }
                    }
                }
            }
            catch { }
            #endregion
            #region Global Messages
            try
            {
                if (DateTime.Now.Minute == 5 || DateTime.Now.Minute == 25 || DateTime.Now.Minute == 45)
                {
                    int tipsamt = 0;
                    if ((DateTime.Now.Minute == 5 || DateTime.Now.Minute == 15 || DateTime.Now.Minute == 25 || DateTime.Now.Minute == 35 || DateTime.Now.Minute == 45 || DateTime.Now.Minute == 55) && MyMath.ChanceSuccess(50))
                    {
                        World.SendMsgToAll("SYSTEM", "Selling/Buying gears for real money or for items in other servers or just the attempt of doing it is forbidden and will result in permanent ban.", 2015, 0);
                    }
                    if (MyMath.ChanceSuccess(30))
                    {
                        World.SendMsgToAll("SYSTEM", "Our Discord is the best way to get in contact with other players and Ultimate-CO staff. Use @Discord to join!", 2015, 0);
                        tipsamt++;
                    }
                    if (MyMath.ChanceSuccess(10))
                    {
                        World.SendMsgToAll("SYSTEM", "Players below level 70 can get free double experience from 'FreeXPPotion' NPC in TwinCity (441,382)!", 2015, 0);
                        World.SendMsgToAll("SYSTEM", "Level 1-6 characters have newbies protection and therefore can't be attacked!", 2015, 0); // If you fail to upgrade the quality or level of an item At ArtisanWind, it can still socket!
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(20))
                    {
                        World.SendMsgToAll("SYSTEM", "If you fail to upgrade the quality or level of an item, there's still a chance it will open a socket.", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(10) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "VIPs can maximize their mining experience and disable ores mining by using the @vipmineores command.", 2015, 0);
                        tipsamt++;
                    }
                    if (MyMath.ChanceSuccess(10) && tipsamt < 2)
                    {
                        World.SendMsgToAll("SYSTEM", "You will win online points for being online. Find 'OnlinePoints' NPC in TwinCity (425,338) to exchange them for rewards!", 2015, 0);
                        World.SendMsgToAll("SYSTEM", "Remmember to set a warehouse password in order to keep your account safe!", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(15) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "Both Titan and Ganoderma have a chance of dropping ProficiencyTokens. These can be used to level your weapon proficiency.", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(25) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "Type @quests to see daily quest status. Available quests: Bird Island, Ape City", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(25) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "Both WaterLord and DBDevils can be found in the Adventure Maps. WaterLord takes 20 minutes to respawn and DBDevils take 6 hours to respawn!", 2015, 0);
                        tipsamt++;
                    }
                    if (MyMath.ChanceSuccess(20) && tipsamt < 2)
                    {
                        World.SendMsgToAll("SYSTEM", "Exchange Lab Diamonds for rewards! VIP players can exchange diamonds by right clicking them!", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(25) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "Monsters in the Adventure Maps have higher chances of dropping DragonBalls!", 2015, 0);
                        tipsamt++;
                    }
                    if (MyMath.ChanceSuccess(10) && tipsamt < 2)
                    {
                        World.SendMsgToAll("SYSTEM", "Terato Dragon spawns every Saturday at 19:10. Make sure you'll be ready to fight by then so you can win awesome rewards!", 2015, 0);
                        World.SendMsgToAll("SYSTEM", "Make sure you keep yourself updated about the server and all the changes introduced! These can be seen at our forum!", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(20) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "Make sure not to miss our scheduled hourly events!", 2015, 0);
                        tipsamt++;
                    }
                    else if (MyMath.ChanceSuccess(25) && tipsamt < 3)
                    {
                        World.SendMsgToAll("SYSTEM", "If you need help and want to talk with a [GM] you can use this command @help", 2015, 0);
                        tipsamt++;
                    }
                }
                if (DateTime.Now.Minute == 15)
                {
                    if (MyMath.ChanceSuccess(15))
                    {
                        World.SendMsgToAll("SYSTEM", "Remember to vote! By voting you're helping to increase the community and you can earn some cool rewards for it!", 2011, 0);
                    }
                }
            }
            catch { }
            #endregion
            #region Donations
            try
            {
                Donations.PaymentsProcess();
            }
            catch { }
            #endregion
            #region Votes
            try
            {
                //Voting.VotesProcess();
                //Voting.CheckIPs();
                //if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                //    Features.ArenaQualifier.ResetRankings();
            }
            catch
            {

            }
            #endregion
            #region Bosses
            try
            {
                if (DateTime.Now.DayOfWeek >= (DayOfWeek)1 && DateTime.Now.DayOfWeek <= (DayOfWeek)4 || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (DateTime.Now.Hour == 1 || DateTime.Now.Hour == 5 || DateTime.Now.Hour == 9 ||
                        DateTime.Now.Hour == 13 || DateTime.Now.Hour == 17 || DateTime.Now.Hour == 21)
                    {
                        if (DateTime.Now.Minute == 25)
                        {
                            Bosses.BossHandler.ChooseBoss(DateTime.Now.DayOfWeek);
                            if (World.CurrentBoss != "")
                            {
                                World.SendMsgToAll("SYSTEM", World.CurrentBoss + " will appear at " + DateTime.Now.Hour + ":30! Get ready to fight!", 2011, 0);
                                Discord DCord = new Discord();
                                DCord.MesajVer7 = "```" + World.CurrentBoss + " will appear at " + DateTime.Now.Hour + ":30! Get ready to fight!```";
                                foreach (Character C in World.H_Chars.Values)
                                {
                                    C.MyClient.DialogNPC = 2094;
                                    if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                        C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                                    Bosses.BossHandler.WindowInformation(C);
                                    NPCs.NPCHandler.Handle(C.MyClient, null, 2094, 0);
                                }
                            }
                            

                        }
                        else if (DateTime.Now.Minute == 30)
                            Bosses.BossHandler.SpawnBoss();
                    }
                }
            }
            catch { }

            #endregion
            #region TeratoDragon
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (DateTime.Now.Hour == 19)
                    {
                        if (DateTime.Now.Minute == 10)
                        {
                            World.Dragon = true;
                            World.DebugAdd += "Dragon = true! at: " + DateTime.Now.ToString() + "\r\n";
                        }
                        else if (DateTime.Now.Minute == 0) World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight!", 2011, 0);
                        else if (DateTime.Now.Minute == 5)
                        {
                            World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight!", 2011, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```Terato Dragon will appear at 19:10! Get ready to fight!```";
                            foreach (Character C in World.H_Chars.Values)
                            {


                                C.MyClient.DialogNPC = 2092;
                                if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                    C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                                Bosses.BossHandler.WindowInformation(C);
                                NPCs.NPCHandler.Handle(C.MyClient, null, 2092, 0);
                            }
                        }
                    }
                    else if (DateTime.Now.Hour == 18)
                    {
                        if (DateTime.Now.Minute == 30)
                            World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight it!", 2011, 0);
                        else if (DateTime.Now.Minute == 40) World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight!", 2011, 0);
                        else if (DateTime.Now.Minute == 50) World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight!", 2011, 0);
                        else if (DateTime.Now.Minute == 55) World.SendMsgToAll("SYSTEM", "Terato Dragon will appear at 19:10! Get ready to fight!", 2011, 0);
                    }

                }
            }
            catch { }
            #endregion
            #region TreasureHunt
            try
            {
                if (DateTime.Now.Hour == 16)
                {
                    if (DateTime.Now.Minute == 0)
                    {
                        World.TreasureHunt = true;
                        World.TreasureMap = (ushort)Rnd.Next(8004, 8007);
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has started! Hurry up and join it while you can! It will last for 20 minutes!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2091, 0);

                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2091, 0);
                        }
                    }
                    else if (DateTime.Now.Minute == 20)
                    {
                        World.TreasureHunt = false;
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has ended! Good luck next time!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                            if (C.Loc.Map == World.TreasureMap)
                                C.Teleport(1002, 438, 327);
                    }
                }
                else if (DateTime.Now.Hour == 15)
                {
                    if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 30 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 40)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 20 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 50)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 10 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 5 minutes!", 2005, 0);
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```Treasure Hunt Event will start in 5 minutes!```";
                        
                    }
                }

                else if (DateTime.Now.Hour == 8)
                {
                    if (DateTime.Now.Minute == 0)
                    {
                        World.TreasureHunt = true;
                        World.TreasureMap = (ushort)Rnd.Next(8004, 8007);
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has started! Hurry up and join it while you can! It will last for 20 minutes!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.DialogNPC = 2091;
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2091, 0);
                        }
                    }
                    else if (DateTime.Now.Minute == 20)
                    {
                        World.TreasureHunt = false;
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has ended! Good luck next time!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                            if (C.Loc.Map == World.TreasureMap)
                                C.Teleport(1002, 438, 327);
                    }
                }
                else if (DateTime.Now.Hour == 7)
                {
                    if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 30 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 40)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 20 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 50)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 10 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 5 minutes!", 2005, 0);
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```Treasure Hunt Event will start in 5 minutes!```";
                      
                    }
                }

                else if (DateTime.Now.Hour == 0)
                {
                    if (DateTime.Now.Minute == 0)
                    {
                        World.TreasureHunt = true;
                        World.TreasureMap = (ushort)Rnd.Next(8004, 8007);
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has started! Hurry up and join it while you can! It will last for 20 minutes!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.DialogNPC = 2091;
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2091, 0);
                        }
                    }
                    else if (DateTime.Now.Minute == 20)
                    {
                        World.TreasureHunt = false;
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event has ended! Good luck next time!", 2005, 0);
                        foreach (Character C in World.H_Chars.Values)
                            if (C.Loc.Map == World.TreasureMap)
                                C.Teleport(1002, 438, 327);
                    }
                }
                else if (DateTime.Now.Hour == 23)
                {
                    if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 30 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 40)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 20 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 50)
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 10 minutes!", 2005, 0);
                    else if (DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "Treasure Hunt Event will start in 5 minutes!", 2005, 0);
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```Treasure Hunt Event will start in 5 minutes!```";
                      
                    }
                }

            }
            catch { }
            #endregion
            //#region TeamPK
            //try
            //{
            //    if (!Features.TeamPKTourny.Started70To99 && !Features.TeamPKTourny.Started100To115 && !Features.TeamPKTourny.Started116To130)
            //    {
            //        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            //        {
            //            if (DateTime.Now.Hour == 18)
            //            {
            //                if (DateTime.Now.Minute == 10)
            //                {
            //                    Features.TeamPKTourny.EventByPM = true;
            //                    Features.TeamPKTourny.StartTourny();
            //                }
            //                else if (DateTime.Now.Minute == 0) World.SendMsgToAll("SYSTEM", "Team PK tournament will start in 10 minutes!", 2011, 0);
            //                else if (DateTime.Now.Minute == 5)
            //                {
            //                    World.SendMsgToAll("SYSTEM", "Team PK tournament will start in 5 minutes!", 2011, 0);
            //                    Discord DCord = new Discord();
            //                    dcord.MesajVer7 = "```Team PK tournament will start in 5 minutes!```";

            //                }

            //            }
            //            else if (DateTime.Now.Hour == 17)
            //            {
            //                if (DateTime.Now.Minute == 30)
            //                    World.SendMsgToAll("SYSTEM", "Team PK tournament will start 18:10!", 2011, 0);
            //                else if (DateTime.Now.Minute == 40) World.SendMsgToAll("SYSTEM", "Team PK tournament will start in 30 minutes!!", 2011, 0);
            //                else if (DateTime.Now.Minute == 50) World.SendMsgToAll("SYSTEM", "Team PK tournament will start in 20 minutes!!", 2011, 0);
            //                else if (DateTime.Now.Minute == 55) World.SendMsgToAll("SYSTEM", "Team PK tournament will start in 15 minutes!", 2011, 0);
            //            }

            //        }
            //    }
            //    else { Features.TeamPKTourny.CheckEndTourny(); }



            //}
            //catch { }
            //#endregion

            #region TeamPK1
            try
            {
                if (!World.PKTourny)
                {
                    if (DateTime.Now.Minute == 15)
                    {
                        if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 20 || DateTime.Now.Hour == 01)
                        {
                            World.PKTourny = true;
                            World.PKTList = new ArrayList();
                            World.PKTIPs = new ArrayList();
                            World.SendMsgToAll("PKT", "PK Tournament is ON for 15 minutes! Stay inside the arena to Join! (SS/FB)", 2000, 0);
                            Discord DCord = new Discord();
                            DCord.MesajVer7 = "```PK Tournament is ON for 15 minutes! Stay inside the arena to Join! (SS/FB)```";
                            Dictionary<uint, Main.GameClient> Clients = Game.World.H_Clients;
                            List<string> IPs = new List<string>();
                            foreach (Main.GameClient C in Clients.Values)
                            {
                                if (C.MyChar.Loc.Map == 1005)
                                {
                                    string IP = C.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();

                                    if (!World.PKTIPs.Contains(IP))
                                    {
                                        World.PKTIPs.Add(IP);
                                        C.MyChar.Teleport(8000, C.MyChar.Loc.X, C.MyChar.Loc.Y);

                                        if (C.MyChar.Job >= 40 && C.MyChar.Job <= 45)
                                        {
                                            C.MyChar.Teleport(1002, 455, 294);
                                        }
                                    }
                                    else
                                    {
                                        C.MyChar.Teleport(1002, 455, 294);
                                        C.LocalMessage(2000, "You already have 1 character inside PKArena during PKTourny!");
                                    }
                                }

                            }

                        }

                    }
                    else if (DateTime.Now.Minute == 10)
                    {
                        if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 20 || DateTime.Now.Hour == 01)
                        {
                            World.SendMsgToAll("PKT", "PKTourny starts in 5 minutes! Enter PKArena before it's too late!", 2000, 0);
                        }
                    }
                    else if (DateTime.Now.Minute == 5)
                    {
                        if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 20 || DateTime.Now.Hour == 01)
                        {
                            World.SendMsgToAll("PKT", "PKTourny starts in 10 minutes! Enter PKArena before it's too late!", 2011, 0);
                        }
                    }
                    else if (DateTime.Now.Minute == 2)
                    {
                        if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 20 || DateTime.Now.Hour == 01)
                        {
                            World.SendMsgToAll("PKT", "PKTourny starts in 15 minutes! Enter PKArena before it's too late!", 2011, 0);
                        }
                    }

                }

            }
            catch { }
            #endregion
            #region TeamPKFinish
            try
            {
                if (World.PKTourny)
                {
                    if (DateTime.Now.Minute == 30)
                    {
                        if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 15 || DateTime.Now.Hour == 20 || DateTime.Now.Hour == 01)
                        {
                            World.PKTourny = false;
                            ArrayList PKwinners = new ArrayList();
                            foreach (uint ID in World.PKTList)
                            {
                                if (World.H_Chars.ContainsKey(ID))
                                    PKwinners.Add(World.H_Chars[ID]);
                            }
                            for (int i = 0; i < PKwinners.Count - 1; i++)
                            {
                                for (int j = i + 1; j < PKwinners.Count; j++)
                                {
                                    try
                                    {
                                        Character C = (Character)PKwinners[i];
                                        Character C2 = (Character)PKwinners[j];

                                        if (C.PKTHits < C2.PKTHits)
                                        {
                                            PKwinners[i] = PKwinners[j];
                                            PKwinners[j] = C;
                                        }
                                    }

                                    catch { }
                                }
                            }

                            byte Winners = 0;
                            foreach (Character C in PKwinners)
                            {
                                if (Winners == 0)
                                {
                                    //C.Top = 9;
                                    Winners++;
                                    World.SendMsgToAll("PKT", "PK Tourny has ended: TOP 1: " + C.Name + " hits: " + C.PKTHits, 2011, 0);
                                    World.SendMsgToAll("PKT", "PK Tourny has ended: TOP 1: " + C.Name + "won 5,000,000 Money DbScroll and EggPacket", 2000, 0);
                                    C.Silvers += 5000000;
                                    C.AddItem(720028);
                                    C.AddItem(720142);
                                    C.PKTHits = 0;
                                }
                                else if (Winners == 1)
                                {
                                    //C.Top = 9;
                                    Winners++;
                                    World.SendMsgToAll("PKT", "PK Tourny has ended: TOP 2: " + C.Name + " hits: " + C.PKTHits, 2005, 0);
                                    World.SendMsgToAll("PKT", "PK Tourny has ended: TOP 2: " + C.Name + " won 2,500,000 Money and 5DBs", 2000, 0);
                                    C.Silvers += 2500000;
                                    for (int a = 0; a < 5; a++)
                                        C.AddItem(1088000);
                                    C.PKTHits = 0;
                                }
                                else C.PKTHits = 0;
                            }
                            World.PKTList = new ArrayList();
                        }
                    }

                }
            }

            catch { }
            #endregion
            #region EXPMob
            try
            {
                if (DateTime.Now.Minute == 0 && DateTime.Now.Hour == 19)
                {
                    World.ExpMob = true;
                    World.DebugAdd += "ExpMob = true! at: " + DateTime.Now.ToString() + "\r\n";
                }
                else if (DateTime.Now.Hour == 18)
                {
                    if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 30 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 40) World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 20 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 50) World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 10 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 5 minutes!", 2011, 0);
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```The EXP Mob will appear in 5 minutes!```";
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.DialogNPC = 2093;
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2093, 0);
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region EXPMob
            try
            {
                if (DateTime.Now.Minute == 0 && DateTime.Now.Hour == 7)
                {
                    World.ExpMob = true;
                    World.DebugAdd += "ExpMob = true! at: " + DateTime.Now.ToString() + "\r\n";
                }
                else if (DateTime.Now.Hour == 6)
                {
                    if (DateTime.Now.Minute == 30)
                        World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 30 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 40) World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 20 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 50) World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 10 minutes!", 2011, 0);
                    else if (DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "The EXP Mob will appear in 5 minutes!", 2011, 0);
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```The EXP Mob will appear in 5 minutes!```";
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.DialogNPC = 2093;
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2093, 0);
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region DisCity
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                {
                    if (DateTime.Now.Minute == 0 && DateTime.Now.Hour == 18)
                    {
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```The Dis City quest has started! Run to ApeMountain and find SolarSaint(530, 482)!```";
                        World.SendMsgToAll("SYSTEM", "The Dis City quest has started! Run to ApeMountain and find SolarSaint(530, 482)!", 2011, 0);
                        World.Syrens = 8;
                        World.Pluto = false;
                        World.PlutoKilled = false;
                        World.LeftKills = 0;
                        World.RightKills = 0;
                        World.Dis2 = 0;
                        World.Dis3 = 0;
                        World.LeftFlank = 0;
                        World.RightFlank = 0;
                        Game.World.DisCityON = true;
                        World.H_LeftFlank.Clear();
                        World.H_RightFlank.Clear();
                    }
                    else if (DateTime.Now.Hour == 18 && DateTime.Now.Minute == 6)
                    {
                        World.SendMsgToAll("SYSTEM", "You can no longer enter the Dis City! If you didn't manage to get in on time you can always come next time!", 2011, 0);
                        Game.World.DisCityON = false;
                    }
                    else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 30)
                    {
                        World.SendMsgToAll("SYSTEM", "Dis City will start in 30 minutes!", 2011, 0);
                    }
                    else if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 56)
                    {
                        World.SendMsgToAll("SYSTEM", "Dis City will start in 5 minutes hurry up!", 2011, 0);

                        foreach (Character C in World.H_Chars.Values)
                        {
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2090, 0);
                            C.MyClient.DialogNPC = 2090;
                        }
                    }
                    else if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 00)
                    {
                        foreach (Character C in World.PlayersInMap[2021].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 30)
                    {
                        foreach (Character C in World.PlayersInMap[2022].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 19 && DateTime.Now.Minute == 45)
                    {
                        foreach (Character C in World.PlayersInMap[2023].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 20 && DateTime.Now.Minute == 00)
                    {
                        foreach (Character C in World.PlayersInMap[2024].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to kill UltimatePluto has passed! Better luck next time!");
                        }
                    }
                }
            }
            catch { }

            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)  // tues/thurs dis
                {
                    if (DateTime.Now.Hour == 6 && DateTime.Now.Minute == 0)
                    {
                        Discord DCord = new Discord();
                        DCord.MesajVer7 = "```The Dis City quest has started! Run to ApeMountain and find SolarSaint(530,482)!```";
                        World.SendMsgToAll("SYSTEM", "The Dis City quest has started! Run to ApeMountain and find SolarSaint(530,482)!", 2011, 0);
                        World.Syrens = 8;
                        World.Pluto = false;
                        World.PlutoKilled = false;
                        World.LeftKills = 0;
                        World.RightKills = 0;
                        World.Dis2 = 0;
                        World.Dis3 = 0;
                        World.LeftFlank = 0;
                        World.RightFlank = 0;
                        Game.World.DisCityON = true;
                        World.H_LeftFlank.Clear();
                        World.H_RightFlank.Clear();
                    }
                    else if (DateTime.Now.Hour == 6 && DateTime.Now.Minute == 6)
                    {
                        World.SendMsgToAll("SYSTEM", "You can no longer enter the Dis City! If you didn't manage to get in on time you can always come next time!", 2011, 0);
                        Game.World.DisCityON = false;
                    }
                    else if (DateTime.Now.Hour == 5 && DateTime.Now.Minute == 30)
                    {
                        World.SendMsgToAll("SYSTEM", "Dis City will start in 30 minutes!", 2011, 0);
                    }
                    else if (DateTime.Now.Hour == 5 && DateTime.Now.Minute == 55)
                    {
                        World.SendMsgToAll("SYSTEM", "Dis City will start in 5 minutes hurry up!", 2011, 0);
                        foreach (Character C in World.H_Chars.Values)
                        {
                            C.MyClient.DialogNPC = 2090;
                            if (!C.BOTJailed && C.PKPoints < 100 && !(C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                                C.MyClient.AddSend(Packets.ShowDialog(34, 1));
                            Bosses.BossHandler.WindowInformation(C);
                            NPCs.NPCHandler.Handle(C.MyClient, null, 2090, 0);
                        }
                    }
                    else if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 00)
                    {
                        foreach (Character C in World.PlayersInMap[2021].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 30)
                    {
                        foreach (Character C in World.PlayersInMap[2022].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 45)
                    {
                        foreach (Character C in World.PlayersInMap[2023].Values)
                        {
                            C.Teleport(1020, 566, 564);
                            C.MyClient.LocalMessage(2011, "The time to get to the next stage has passed! Better luck next time!");
                        }
                    }
                    else if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 00)
                    {
                        foreach (Character C in World.PlayersInMap[2024].Values)
                        {
                            if (!World.PlutoKilled)
                            {
                                C.Teleport(1020, 566, 564);
                                C.MyClient.LocalMessage(2011, "The time to kill UltimatePluto has passed! Better luck next time!");
                            }
                        }
                    }
                }
            }
            catch { }
            #endregion
            #region CloseShop
            try
            {
                foreach (Features.PersonalShops.Shop P in World.H_PShops.Values)
                    if (!Game.World.H_Chars.ContainsKey(P.Owner.EntityID) || P.NPCInfo.Loc.X != (P.Owner.Loc.X + 1) || P.NPCInfo.Loc.Y != P.Owner.Loc.Y)
                    {
                        World.DebugAdd += "Shop closed! \r\n";
                        P.Close();
                    }
            }
            catch { }
            #endregion
            #region Hide&Seek
            try
            {
                if (DateTime.Now.Minute == 6/* && DateTime.Now.Month == 12 && DateTime.Now.Day <= 26*/)
                {
                    if (World.Found)
                    {
                        #region Spawn Santa
                        ushort[] Maps = new ushort[3] { 1002, 1020, 1011/*, 1000, 1015*/ };// twin city , ape city, bird island, phoenix castle , desert city
                        var i = Rnd.Next(0, 3);
                        var a = Rnd.Next(0, 15);
                        //DMap D = (DMap)DMaps.H_DMaps[Maps[i]];
                        NPC NPCInfo = _npcInfo;
                        NPCInfo.EntityID = 18810;
                        NPCInfo.Type = 2560;
                        NPCInfo.Flags = 2;
                        _location = NPCInfo.Loc = new Location();
                        NPCInfo.Loc.Map = Maps[i];

                        #region TwinCity
                        if (i == 0)
                        {
                            if (a == 0)
                            {
                                NPCInfo.Loc.X = 428;
                                NPCInfo.Loc.Y = 388;
                                World.Hint1 = " near the center!";
                            }
                            else if (a == 1)
                            {
                                NPCInfo.Loc.X = 418;
                                NPCInfo.Loc.Y = 472;
                                World.Hint1 = " near the Pheasants!";
                            }
                            else if (a == 2)
                            {
                                NPCInfo.Loc.X = 490;
                                NPCInfo.Loc.Y = 480;
                                World.Hint1 = " near the Pheasants!";
                            }
                            else if (a == 3)
                            {
                                NPCInfo.Loc.X = 730;
                                NPCInfo.Loc.Y = 670;
                                World.Hint1 = " near the TurtleDoves!";
                            }
                            else if (a == 4)
                            {
                                NPCInfo.Loc.X = 620;
                                NPCInfo.Loc.Y = 680;
                                World.Hint1 = " in the middle of a bridge!";
                            }
                            else if (a == 5)
                            {
                                NPCInfo.Loc.X = 565;
                                NPCInfo.Loc.Y = 720;
                                World.Hint1 = " near the Robins!";
                            }
                            else if (a == 6)
                            {
                                NPCInfo.Loc.X = 565;
                                NPCInfo.Loc.Y = 793;
                                World.Hint1 = " near the Altar!";
                            }
                            else if (a == 7)
                            {
                                NPCInfo.Loc.X = 492;
                                NPCInfo.Loc.Y = 781;
                                World.Hint1 = " near the Robins!";
                            }
                            else if (a == 8)
                            {
                                NPCInfo.Loc.X = 350;
                                NPCInfo.Loc.Y = 665;
                                World.Hint1 = " near the Apparitions!";
                            }
                            else if (a == 9)
                            {
                                NPCInfo.Loc.X = 325;
                                NPCInfo.Loc.Y = 585;
                                World.Hint1 = " near the Apparitions!";
                            }
                            else if (a == 10)
                            {
                                NPCInfo.Loc.X = 245;
                                NPCInfo.Loc.Y = 610;
                                World.Hint1 = " near the Apparitions!";
                            }
                            else if (a == 11)
                            {
                                NPCInfo.Loc.X = 165;
                                NPCInfo.Loc.Y = 545;
                                World.Hint1 = " in the middle of a bridge!";
                            }
                            else if (a == 12)
                            {
                                NPCInfo.Loc.X = 105;
                                NPCInfo.Loc.Y = 485;
                                World.Hint1 = " near the Poltergeists!";
                            }
                            else if (a == 13)
                            {
                                NPCInfo.Loc.X = 120;
                                NPCInfo.Loc.Y = 385;
                                World.Hint1 = " near the Poltergeists!";
                            }
                            else if (a == 14)
                            {
                                NPCInfo.Loc.X = 41;
                                NPCInfo.Loc.Y = 417;
                                World.Hint1 = " near a Buddha!";
                            }
                            World.M = "Twin City";
                        }
                        #endregion
                        #region ApeCity
                        else if (i == 1)
                        {
                            if (a == 0)
                            {
                                NPCInfo.Loc.X = 577;
                                NPCInfo.Loc.Y = 557;
                                World.Hint1 = " near the center!";
                            }
                            else if (a == 1)
                            {
                                NPCInfo.Loc.X = 682;
                                NPCInfo.Loc.Y = 627;
                                World.Hint1 = " near the Macaques!";
                            }
                            else if (a == 2)
                            {
                                NPCInfo.Loc.X = 597;
                                NPCInfo.Loc.Y = 535;
                                World.Hint1 = " near the Waterfall!";
                            }
                            else if (a == 3)
                            {
                                NPCInfo.Loc.X = 617;
                                NPCInfo.Loc.Y = 400;
                                World.Hint1 = " near the GiantApes!";
                            }
                            else if (a == 4)
                            {
                                NPCInfo.Loc.X = 470;
                                NPCInfo.Loc.Y = 226;
                                World.Hint1 = " near the GiantApes!";
                            }
                            else if (a == 5)
                            {
                                NPCInfo.Loc.X = 409;
                                NPCInfo.Loc.Y = 317;
                                World.Hint1 = " in the middle of a bridge!";
                            }
                            else if (a == 6)
                            {
                                NPCInfo.Loc.X = 285;
                                NPCInfo.Loc.Y = 230;
                                World.Hint1 = " near the ThunderApes!";
                            }
                            else if (a == 7)
                            {
                                NPCInfo.Loc.X = 235;
                                NPCInfo.Loc.Y = 326;
                                World.Hint1 = " near the ThunderApes!";
                            }
                            else if (a == 8)
                            {
                                NPCInfo.Loc.X = 97;
                                NPCInfo.Loc.Y = 370;
                                World.Hint1 = " in the middle of a bridge!";
                            }
                            else if (a == 9)
                            {
                                NPCInfo.Loc.X = 182;
                                NPCInfo.Loc.Y = 477;
                                World.Hint1 = " near the Snakemen!";
                            }
                            else if (a == 10)
                            {
                                NPCInfo.Loc.X = 292;
                                NPCInfo.Loc.Y = 555;
                                World.Hint1 = " near the Snakemen!";
                            }
                            else if (a == 11)
                            {
                                NPCInfo.Loc.X = 402;
                                NPCInfo.Loc.Y = 633;
                                World.Hint1 = " near the Snakemen!";
                            }
                            else if (a == 12)
                            {
                                NPCInfo.Loc.X = 690;
                                NPCInfo.Loc.Y = 670;
                                World.Hint1 = " near the Macaques!";
                            }
                            else if (a == 13)
                            {
                                NPCInfo.Loc.X = 753;
                                NPCInfo.Loc.Y = 628;
                                World.Hint1 = " near the Macaques!";
                            }
                            else if (a == 14)
                            {
                                NPCInfo.Loc.X = 725;
                                NPCInfo.Loc.Y = 702;
                                World.Hint1 = " near the Macaques!";
                            }
                            World.M = "Ape City";
                        }
                        #endregion
                        #region PhoenixCastle
                        else if (i == 2)
                        {
                            if (a == 0)
                            {
                                NPCInfo.Loc.X = 286;
                                NPCInfo.Loc.Y = 459;
                                World.Hint1 = " near the Bandits!";
                            }
                            else if (a == 1)
                            {
                                NPCInfo.Loc.X = 430;
                                NPCInfo.Loc.Y = 473;
                                World.Hint1 = " near the Bandits!";
                            }
                            else if (a == 2)
                            {
                                NPCInfo.Loc.X = 375;
                                NPCInfo.Loc.Y = 355;
                                World.Hint1 = " near the Bandits!";
                            }
                            else if (a == 3)
                            {
                                NPCInfo.Loc.X = 430;
                                NPCInfo.Loc.Y = 340;
                                World.Hint1 = " near the Bandits!";
                            }
                            else if (a == 4)
                            {
                                NPCInfo.Loc.X = 600;
                                NPCInfo.Loc.Y = 380;
                                World.Hint1 = " near the Ratlings!";
                            }
                            else if (a == 5)
                            {
                                NPCInfo.Loc.X = 626;
                                NPCInfo.Loc.Y = 324;
                                World.Hint1 = " near the Ratlings!";
                            }
                            else if (a == 6)
                            {
                                NPCInfo.Loc.X = 650;
                                NPCInfo.Loc.Y = 450;
                                World.Hint1 = " near the Ratlings!";
                            }
                            else if (a == 7)
                            {
                                NPCInfo.Loc.X = 798;
                                NPCInfo.Loc.Y = 471;
                                World.Hint1 = " near the Village!";
                            }
                            else if (a == 8)
                            {
                                NPCInfo.Loc.X = 578;
                                NPCInfo.Loc.Y = 788;
                                World.Hint1 = " near the FireSpirits!";
                            }
                            else if (a == 9)
                            {
                                NPCInfo.Loc.X = 455;
                                NPCInfo.Loc.Y = 815;
                                World.Hint1 = " near the FireSpirits!";
                            }
                            else if (a == 10)
                            {
                                NPCInfo.Loc.X = 800;
                                NPCInfo.Loc.Y = 630;
                                World.Hint1 = " near the FireSpirits!";
                            }
                            else if (a == 11)
                            {
                                NPCInfo.Loc.X = 220;
                                NPCInfo.Loc.Y = 230;
                                World.Hint1 = " inside the city!";
                            }
                            else if (a == 12)
                            {
                                NPCInfo.Loc.X = 405;
                                NPCInfo.Loc.Y = 100;
                                World.Hint1 = " near the WingedSnakes!";
                            }
                            else if (a == 13)
                            {
                                NPCInfo.Loc.X = 470;
                                NPCInfo.Loc.Y = 115;
                                World.Hint1 = " near the WingedSnakes!";
                            }
                            else if (a == 14)
                            {
                                NPCInfo.Loc.X = 335;
                                NPCInfo.Loc.Y = 150;
                                World.Hint1 = " near the WingedSnakes!";
                            }
                            World.M = "Phoenix Castle";
                        }
                        #endregion
                        //else if (i == 3) World.M = "Desert City";
                        //else if (i == 4) World.M = "Bird Island";
                        NPCInfo.Direction = 0;
                        NPCInfo.Avatar = 67;

                        World.MapN = NPCInfo.Loc.Map;
                        World.Xn = NPCInfo.Loc.X;
                        World.Yn = NPCInfo.Loc.Y;

                        if (!World.H_NPCs.ContainsKey(NPCInfo.Loc.Map))
                        {
                            World.H_NPCs.Add(NPCInfo.Loc.Map, new Dictionary<uint, NPC>());
                        }
                        Dictionary<uint, NPC> NPCMap = World.H_NPCs[NPCInfo.Loc.Map];
                        if (!NPCMap.ContainsKey(NPCInfo.EntityID))
                        {
                            NPCMap.Add(NPCInfo.EntityID, NPCInfo);
                            World.Spawn(NPCInfo);
                        }
                        World.SendMsgToAll("SYSTEM", "Santa has spawned in " + World.M + "! Find him and get the prize!", 2011, 0);
                        Console.WriteLine(NPCInfo.Loc.Map + " " + NPCInfo.Loc.X + " " + NPCInfo.Loc.Y);
                        World.Found = false;
                        #endregion
                    }
                    else
                    {
                        World.SendMsgToAll("SYSTEM", "Santa has spawned in " + World.M + "! Find him and get the prize!", 2011, 0);
                        World.DebugAdd += "Santa was not found in an hour";
                    }
                }
                else if (DateTime.Now.Minute == 20 && !World.Found)
                    World.SendMsgToAll("SYSTEM", "Santa has not been found yet. He's" + World.Hint1 + "! Find him and get the prize!", 2011, 0);
            }
            catch { }
            #endregion
            #region AntiBot
            /*  try
              {
                  if (World.BOTSEND)
                  {
                      Hashtable Chars = World.H_Chars;
                      foreach (Character C in Chars.Values)
                      {
                          if (C.BOTStarted)
                              if (DateTime.Now > C.BOTStartedTime.AddSeconds(60))
                              {
                                  C.MyClient.Soc.Disconnect(false);
                              }
                      }
                  }
              }
              catch { }*/
            #endregion
            #region Restart
            try
            {
                if (!World.TestCmds)
                {
                    if (DateTime.Now.Hour == 4 && (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) && World.LastWeek)// || ServerTime.DayName == "Saturday"))
                    {
                        if (DateTime.Now.Minute == 30)
                            World.SendMsgToAll("SYSTEM", "Server Maintenance in 30 minutes! Please be ready !", 2011, 0);
                        else if (DateTime.Now.Minute == 40) World.SendMsgToAll("SYSTEM", "Server Maintenance in 20 minutes! Please be ready !", 2011, 0);
                        else if (DateTime.Now.Minute == 50) World.SendMsgToAll("SYSTEM", "Server Maintenance in 10 minutes! Please be ready !", 2011, 0);
                        else if (DateTime.Now.Minute == 55) World.SendMsgToAll("SYSTEM", "Server Maintenance in 5 minutes! Please be ready !", 2011, 0);
                        else if (DateTime.Now.Minute == 57) World.SendMsgToAll("SYSTEM", "Server Maintenance in 3 minutes! Please log-off to avoid data loss !", 2011, 0);
                        else if (DateTime.Now.Minute == 59) World.SendMsgToAll("SYSTEM", "Server Maintenance in 1 minute! Please log-off to avoid data loss !", 2011, 0);
                    }
                    else if (DateTime.Now.Minute == 0 && DateTime.Now.Hour == 5 && (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) && World.LastWeek)// || ServerTime.DayName == "Saturday"))
                    {
                        World.SendMsgToAll("SYSTEM", "Server Maintenance ! Please log-off to avoid data loss !", 2011, 0);
                        Thread.Sleep(5000);
                        World.Exit = true;
                        Character[] BaseCharacters = World.H_Chars.Values.ToArray();
                        //Game.Character[] BaseCharacters = new Character[World.H_Chars.Count];
                        //World.H_Chars.Values.CopyTo(BaseCharacters, 0);

                        KillThreads();
                        EndSession = true;
                        try
                        {
                            foreach (Character C in BaseCharacters)
                            {
                                try
                                {
                                    C.MyClient.Disconnect();
                                    C.MyClient.LogOff();
                                    Console.WriteLine(C.Name + " has logged off successfuly.");
                                }
                                catch { }
                            }
                        }
                        catch { }
                        WriteLogs();
                        Database.SaveKOs();
                        Console.WriteLine("KOs saved.");
                        Database.SaveEmpire();
                        Console.WriteLine("Empire saved.");
                        Features.Guilds.SaveGuilds();
                        Console.WriteLine("Guilds saved.");
                        Features.SkillsClass.Save();
                        DMaps.Save();
                        Console.WriteLine("Skills saved.");
                        Features.HouseTable.SaveFurnitures();
                        Console.WriteLine("Furnitures saved.");
                        SOB.GuildStatue.SaveStatues();
                        Console.WriteLine("Guild Statues saved.");
                        Features.ArenaQualifier.SaveRankings();
                        Console.WriteLine("Arena Rankings saved.");

                        Database.Dispose();
                        Console.WriteLine("Database disposed.");
                        Console.WriteLine("Wait for Server to restart!");
                        System.Threading.Thread.Sleep(60000);
                        System.Diagnostics.Process.Start("Ultimate.exe");
                        Environment.Exit(0);
                    }
                    else if (DateTime.Now.Minute == 0 && DateTime.Now.Hour == 5 && (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) && !World.LastWeek)
                        World.LastWeek = true;
                }
            }
            catch { }
            #endregion
            #region GuildChests - Disabled // Must Uncomment
            //try
            //{
            //    if (Features.GuildWars.GuildChests > 0)
            //        for (int i =0;i<Features.GuildWars.GuildChests;i++)
            //            if (DateTime.Now.AddMinutes(5) > Features.GuildWars.ChestTime[i])
            //            {
            //                World.SendMsgToAll("SYSTEM", "GuildChest will spawn in Guild Area in " + Math.Ceiling((Features.GuildWars.ChestTime[i] - DateTime.Now).TotalMinutes) + " minutes!", 2011, 0);
            //            }
            //}
            //catch { }
            #endregion
            #region Weather
            try
            {
                if (DateTime.Now.Month == 12 && DateTime.Now >= Features.Weather.NextChange)
                {
                    Features.Weather.Intensity = (uint)(1010 - World.Snowballs);
                    Features.Weather.Direction = (uint)Rnd.Next(100, 200);

                    Features.Weather.Appearence = 0;
                    Features.Weather.NextChange = DateTime.Now.AddMinutes(Rnd.Next(2, 4));
                    Features.Weather.CurrentWeather = Features.WeatherType.Snow;
                }
            }
            catch
            {

            }
            #endregion
            #region Elite PK
            try
            {
                if (Features.ElitePKStats.Running && DateTime.Now >= Features.ElitePKStats.Finish)
                {
                    Features.ElitePKStats.Running = false;
                    Features.ElitePKStats.Brackets.Clear();
                }
            }
            catch
            {

            }
            #endregion
            #region Remove Extra KeyedClients
            try
            {
                foreach (AuthWorker.AuthInfo Info in AuthWorker.KeyedClients.Values.ToList())
                    if (DateTime.Now > Info.Used.AddSeconds(60))
                        AuthWorker.KeyedClients.Remove(Info.CryptoKey);
            }
            catch
            {

            }
            #endregion
            try
            {
                if (World.DemonBoxes && DateTime.Now >= World.DemonBoxStarted)
                {
                    if (World.H_NPCs[1002].ContainsKey(2084))
                    {
                        Game.World.Action(World.H_NPCs[1002][2084], Packets.GeneralData(2084, 0, 0, 0, 135).Get);
                        World.H_NPCs[1002].Remove(2084);
                        World.DemonBoxes = false;
                    }
                }
            }
            catch
            {

            }
            //try
            //{
            //    WrapperSet.CheckWrappers();
            //}
            //catch { }
            try
            {
                GC.Collect();
            }
            catch { }
            //try
            //{
            //    if (DateTime.Now > AuthWorker.LastGameIP.AddMinutes(5))
            //        AuthWorker.GetGameIP();
            //}
            //catch { Console.WriteLine("Couldn't retrieve GAMEIP!"); }
        }
        public static void WriteLogs()
        {
            try
            {
                if (World.InfoAdd.Length > 0)
                {
                    World.InfoAdd += DateTime.Now + "\r\n";
                    WriteInfo(World.InfoAdd);
                    World.InfoAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.ChatAdd.Length > 0)
                {
                    World.ChatAdd += DateTime.Now + "\r\n";
                    WriteChatLine(World.ChatAdd);
                    World.ChatAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.GMChatAdd.Length > 0)
                {
                    World.GMChatAdd += DateTime.Now + "\r\n";
                    WriteGMChatLine(World.GMChatAdd);
                    World.GMChatAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.TradeAdd.Length > 0)
                {
                    World.TradeAdd += DateTime.Now + "\r\n";
                    Program.WriteTrade(World.TradeAdd);
                    World.TradeAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.DropAdd.Length > 0)
                {
                    World.DropAdd += DateTime.Now + "\r\n";
                    Program.WritePickDrop(World.DropAdd);
                    World.DropAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.ExcAdd.Length > 0)
                {
                    World.ExcAdd += DateTime.Now + "\r\n";
                    Program.WriteException(World.ExcAdd);
                    World.ExcAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.DebugAdd.Length > 0)
                {
                    World.DebugAdd += DateTime.Now + "\r\n";
                    Program.WriteLine(World.DebugAdd);
                    World.DebugAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.PacketAdd.Length > 0)
                {
                    Program.WritePacketLog(World.PacketAdd);
                    World.PacketAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.AntiCheatAdd.Length > 0)
                {
                    Program.WriteAntiCheat(World.AntiCheatAdd);
                    World.AntiCheatAdd = "";
                }
            }
            catch { }
            try
            {
                if (World.DonationAdd.Length > 0)
                {
                    WriteDonation(World.DonationAdd);
                    World.DonationAdd = "";
                }
            }
            catch { }
            try
            {
                string EIds = "";
                foreach (uint I in World.EIDS)
                    EIds += I + " ";
                EIds = EIds.Remove(EIds.Length - 1);
                if (!System.IO.File.Exists("entityids.txt"))
                    System.IO.File.Create("entityids.txt").Close();
                System.IO.File.WriteAllText("entityids.txt", EIds);
            }
            catch { }
            try
            {
                string EIds = "";
                foreach (string I in Game.World.BanChars)
                    EIds += I + "\r\n";
                if (!File.Exists("BanList.txt"))
                    File.Create("BanList.txt").Close();
                if (EIds.Length > 0)
                    File.WriteAllText("BanList.txt", EIds);
            }
            catch { }
        }


        static int lastOnline = -1;
        static int max = 0;
        static int _on = 0;
        static int _online;
        static int _maxon;
        static int OnlineC
        {
            get { return _on; }
            set
            {
                if (value > max)
                    max = value;
                _on = value;
                lastOnline = value;
                File.WriteAllText(@"C:\OldCODB\online.txt", lastOnline.ToString() + " / " + max);

                var cmd1 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                if (lastOnline > 50)
                {
                    cmd1.Update("online").Set("online", lastOnline + 50).Execute();
                    max = lastOnline + 50;
                }
                else
                {
                    cmd1.Update("online").Set("online", lastOnline + 25).Execute();
                    max = lastOnline + 25;
                }


                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("online");
                MySQL.MySqlReader online = new MySQL.MySqlReader(Cmd);

                while (online.Read())
                {
                    _online = online.ReadInt16("online");
                    _maxon = online.ReadInt32("max");
                }
                if (max > _maxon)
                {
                    _maxon = max;

                    var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    if (_maxon > 50)
                        cmd.Update("online").Set("max", _maxon + 50).Execute();
                    else
                        cmd.Update("online").Set("max", _maxon + 25).Execute();
                }

            }
        }
        static DateTime RankingsStamp;
        static DateTime lastOnlineUpdate;

        static void ServerStuff_Execute()
        {
            DateTime TimeNow = DateTime.Now;
            if (lastOnline != Game.World.H_Chars.Count)
                OnlineC = Game.World.H_Chars.Count;
            try
            {
                Console.Title = string.Format("Ultimate Classic 1.0 - Online: {0} - Max: {1}", OnlineC, max);
            }
            catch
            {
            }
            if (DateTime.Now > lastOnlineUpdate.AddSeconds(30))
            {
                try
                {
                    lastOnlineUpdate = DateTime.Now;
                    var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                    cmd.Update("cfg").Set("Online", World.H_Chars.Count).Execute();
                }
                catch (Exception e) { World.ExcAdd += e.ToString() + "\r\n"; }
                {

                }

            }

            try
            {
                if (Features.GuildWars.War && TimeNow > Features.GuildWars.LastScores.AddMilliseconds(5000) && Features.GuildWars.Scores.Values != null)
                    Features.GuildWars.SendScores();

                if (Features.TCGuildWars.War && TimeNow > Features.TCGuildWars.LastScores.AddMilliseconds(5000) && Features.TCGuildWars.Scores.Values != null)
                    Features.TCGuildWars.SendScores();

                if (Features.CounterClock.War && TimeNow > Features.CounterClock.LastScores.AddMilliseconds(5000) && Features.CounterClock.Scores.Values != null)
                    Features.CounterClock.SendScores();

                if (Features.CityWarTc.War && TimeNow > Features.CityWarTc.LastScores.AddMilliseconds(5000) && Features.CityWarTc.Scores.Values != null)
                    Features.CityWarTc.SendScores();

                if (Features.CityWarPc.War && TimeNow > Features.CityWarPc.LastScores.AddMilliseconds(5000) && Features.CityWarPc.Scores.Values != null)
                    Features.CityWarPc.SendScores();

                if (Features.CityWarAc.War && TimeNow > Features.CityWarAc.LastScores.AddMilliseconds(5000) && Features.CityWarAc.Scores.Values != null)
                    Features.CityWarAc.SendScores();

                if (Features.CityWarDc.War && TimeNow > Features.CityWarDc.LastScores.AddMilliseconds(5000) && Features.CityWarDc.Scores.Values != null)
                    Features.CityWarDc.SendScores();

                if (Features.CityWarBi.War && TimeNow > Features.CityWarBi.LastScores.AddMilliseconds(5000) && Features.CityWarBi.Scores.Values != null)
                    Features.CityWarBi.SendScores();

                if (Features.PoleWarTC.War && TimeNow > Features.PoleWarTC.LastScores.AddMilliseconds(5000) && Features.PoleWarTC.Scores.Values != null)
                    Features.PoleWarTC.SendScores();

                if (Features.PoleWarPC.War && TimeNow > Features.PoleWarPC.LastScores.AddMilliseconds(5000) && Features.PoleWarPC.Scores.Values != null)
                    Features.PoleWarPC.SendScores();

                if (Features.PoleWarAC.War && TimeNow > Features.PoleWarAC.LastScores.AddMilliseconds(5000) && Features.PoleWarAC.Scores.Values != null)
                    Features.PoleWarAC.SendScores();

                if (Features.PoleWarDC.War && TimeNow > Features.PoleWarDC.LastScores.AddMilliseconds(5000) && Features.PoleWarDC.Scores.Values != null)
                    Features.PoleWarDC.SendScores();

                if (Features.PoleWarBI.War && TimeNow > Features.PoleWarBI.LastScores.AddMilliseconds(5000) && Features.PoleWarBI.Scores.Values != null)
                    Features.PoleWarBI.SendScores();

            }
            catch { }
            try
            {

                if (DateTime.Now > RankingsStamp.AddMinutes(15))
                {
                    RankingsStamp = DateTime.Now;
                    TopRankings.LoadTops();
                }
            }
            catch
            {

            }
            try
            {
                if (GameOfThones.Stage != GameOfThones.WarStage.None && DateTime.Now >= GameOfThones.WaitingPeriod)
                    GameOfThones.Shuffle();
                else if (GameOfThones.Stage == GameOfThones.WarStage.RoundOne)
                    GameOfThones.RoundOne();
            }
            catch { }

            try
            {
                foreach (Events.Events E in World.Events.ToList())
                    E.ActionHandler();
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
                Console.WriteLine(e);
            }

            try
            {
                if (Features.ArenaQualifier.PlayersInWaiting.Count > 1)
                    foreach (Character C in Features.ArenaQualifier.PlayersInWaiting.Values)
                    {
                        if (C.ArenaQualifier != null)
                        {
                            if (C.Loc.Map != 1038 && DateTime.Now >= C.ArenaQualifier.NextMatch && C.ArenaQualifier.Status == Features.MatchStatus.None && C.EventBase == null && (C.Arena == null || C.Loc.Map != C.Arena.MapID))
                                Features.ArenaQualifier.PairUp(C);
                        }
                        else
                        {
                            C.ArenaQualifier = new Features.QualifierMatch() { NextMatch = DateTime.Now.AddMilliseconds((double)(Rnd.Next(1000, 15000))) };
                        }
                    }
                if (Features.ArenaQualifier.Matches.Count > 0)
                    foreach (Features.QualifierMatch Match in Features.ArenaQualifier.Matches.Values.ToList())
                    {
                        if (Match.Status == Features.MatchStatus.Finish && DateTime.Now >= Match.NextMatch)
                            Match.DestroyMatch();
                        else if (Match.Status != Features.MatchStatus.Fighting && DateTime.Now >= Match.Countdown)
                            Match.RemovePlayer(Match.Opponent);
                        //{
                        //    if (Match.Accepted && !Match.Opponent.ArenaQualifier.Accepted)
                        //    {
                        //        Match.Opponent.MyClient.AddSend(Packets.ShowDialog(24, 0));
                        //        Match.Opponent.MyClient.AddSend(Packets.ShowDialog(23, 1));
                        //        Match.RemovePlayer(Match.Opponent);
                        //    }
                        //    else
                        //    {
                        //        Match.Opponent.MyClient.AddSend(Packets.ShowDialog(24, 0));
                        //        Match.Opponent.ArenaQualifier.Opponent.MyClient.AddSend(Packets.ShowDialog(23, 1));
                        //        Match.RemovePlayer(Match.Opponent.ArenaQualifier.Opponent);
                        //        Match.Opponent.ArenaQualifier = null;
                        //    }
                        //}
                    }
            }
            catch (Exception e)
            {
                World.ExcAdd += e.ToString() + "\r\n";
            }

            try
            {
                if (Features.DiceKing.Players.Count > 0)
                {
                    if (World.DiceKing.Seconds <= 1)
                    {
                        //World.DiceKing.StartTime = DateTime.Now;
                        //World.DiceKing.Status = Features.MsgDice.Stage.End;
                        Features.DiceKing.EndDice();
                    }
                    else if (World.DiceKing.Status == Features.MsgDice.Stage.Betting && World.DiceKing.Seconds > 1)
                        World.DiceKing.Seconds--;
                }
            }
            catch (Exception e)
            {
                World.ExcAdd += e.ToString() + "\r\n";
            }

            foreach (ConcurrentDictionary<uint, DroppedItem> H in World.H_Items.Values)
            {
                try
                {
                    DeletedItems.Clear();
                    foreach (DroppedItem I in H.Values)
                        if ((TimeNow >= I.DropTime.AddSeconds(120)) && I.Info.ID != 710100 && I.Info.ID != 722741 && I.Info.ID > 1000)
                            DeletedItems.TryAdd(I.UID, I);
                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
                try
                {
                    if (DeletedItems.Count > 0)
                    {
                        foreach (DroppedItem I in DeletedItems.Values)
                            I.Dissappear();
                    }
                }
                catch (Exception E) { World.ExcAdd += E.ToString() + "\r\n"; }
            }

        }
        static void CompanionThread_Execute()
        {
            try
            {
                if (World.H_Chars.Count > 0)
                {
                    foreach (Companion C in World.H_Companions.Values)
                        if (C.Owner.MyClient.Soc.Connected)
                            C.Step();
                        else
                            C.Dissappear();
                }
            }
            catch (Exception e)
            {
                World.ExcAdd += e.ToString() + "\r\n";
            }
        }
    }
}
