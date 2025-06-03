using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DeathWish.Game;
using DeathWish.Game.MsgServer;

namespace DeathWish.WebServer
{
    public class LoaderServer
    {

        public class Client
        {
            public enum Flags : ulong
            {
                None = 0,
                Aim = 1UL << 1,
                Speed = 1UL << 2,
                Clicker = 1UL << 3,
                SuspectProgram = 1UL << 4
            }
            public string name = "";
            public uint UID = 0;
            public Flags MyFlags = Flags.None;
            public ServerSockets.SecuritySocket SecuritySocket;
            public object SyncRoot;
            public Client(ServerSockets.SecuritySocket _socket)
            {
                SyncRoot = new object();
                _socket.Client = this;
                SecuritySocket = _socket;
            }

            public unsafe void Send(ServerSockets.Packet msg)
            {
                SecuritySocket.Send(msg);
            }

            public void Disconnect()
            {
                if (Clients.ContainsKey(UID))
                {
                    Client user;
                    Clients.TryRemove(UID, out user);
                    SecuritySocket.Disconnect();
                }
            }
        }
        public static ServerSockets.ServerSocket Server;
        public static ConnectionPoll PollConnections;
        public static bool CanCheck = false;
        public static DateTime TimerStamp = DateTime.Now.AddMinutes(60);
        public static Extensions.Counter Counter = new Extensions.Counter(10);
        public static System.Collections.Concurrent.ConcurrentDictionary<uint, Client> Clients = new System.Collections.Concurrent.ConcurrentDictionary<uint, Client>();
        public static string CheckIP = "";
        public static char[] Chars = new char[] { 'A', 'B', 'C', 'D', 'R', 'T', 'Y', 'U', 'I', 'U', 'K', 'I', 'p', 'T', 'I', 'N', 'M', 'T', 'R', 'E', 'w', 'Q' };
        public static string LogginKey = "RayzoTrialLoader";
        public static void CreateLogginKey()
        {//C238xs65pjy7HU9Q";
            LogginKey = "RayzoTrialLoader";
            //  return;
            for (int i = 0; i < 16; i++)
            {
                LogginKey += Chars[Program.GetRandom.Next(0, Chars.Length)];
            }

        }
        public static void Init()
        {
            if (Program.ServerConfig.IsInterServer == false)
            {
                CreateLogginKey();
                PollConnections = new ConnectionPoll();

                Server = new ServerSockets.ServerSocket(
                     new Action<ServerSockets.SecuritySocket>(p => new Client(p))
                     , new Action<ServerSockets.SecuritySocket, ServerSockets.Packet>((p, data) =>
                     {
                         ProcesReceive(p, data);
                     })
                     , new Action<ServerSockets.SecuritySocket>(p => (p.Client as Client).Disconnect()));
                Server.Initilize(Program.ServerConfig.Port_SendSize, Program.ServerConfig.Port_ReceiveSize, 1, 3);
                Server.Open(Program.ServerConfig.LoaderIP, Program.ServerConfig.LoaderPort, Program.ServerConfig.Port_BackLog);

            }
        }
        public unsafe static void CheckPrograms(string name, string addres)
        {
            foreach (var obj in Clients.Values)
            {

                if (obj.SecuritySocket.RemoteIp == addres)
                {

                    obj.name = name;

                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();

                        stream.InitWriter();
                        stream.Write(0);
                        stream.Finalize(2000);
                        obj.Send(stream);

                    }
                    break;
                }

            }

        }
        public static int procount = 0;
        public static unsafe void ProcesReceive(ServerSockets.SecuritySocket obj, ServerSockets.Packet data)
        {
            var Game = (obj.Client as Client);
            uint packetid = data.ReadUInt16();

            switch (packetid)
            {
                case 1099:
                    {
                        try
                        {
                            ulong Flag = data.ReadUInt64();
                            Game.MyFlags = (Client.Flags)Flag;
                            byte[] datastr = data.ReadBytes(100);
                            string program = System.Text.ASCIIEncoding.ASCII.GetString(datastr).Replace("\0", "");
                            program = program.Split('.')[0];
                            program += ".exe";
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Socket.RemoteIp == Game.SecuritySocket.RemoteIp)
                                {

                                    if (Flag == 1ul << 4)
                                    {
                                        uint BanHours = 0;
                                        if (user.BanCount == 0)
                                            BanHours = 1 * 1;
                                        else if (user.BanCount == 1)
                                            BanHours = 1 * 1;
                                        else if (user.BanCount == 2)
                                            BanHours = 1 * 1;
                                        else
                                            BanHours = 1 * 1;
                                        user.BanCount += 1;
                                        Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, BanHours, program);
                                        string Messaje = "[ " + user.Player.Name + " ] Got Banned For [ " + BanHours / 1 + " ] Hours, because was Found Using Programs That Are Illegal In Game ( " + program + " ) Enjoy.";
                                        Game.MsgServer.MsgMessage msg = new MsgMessage(Messaje, MsgMessage.MsgColor.red, MsgMessage.ChatMode.System);
                                        Program.SendGlobalPackets.Enqueue(msg.GetArray(data));
                                    }
                                    user.Socket.Disconnect();
                                    Console.WriteLine(user.Player.Name + " <---- " + Flag.ToString() + " " + program);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;
                    }
                case 1300:
                    {

                        byte leng = data.ReadUInt8();
                        byte[] datastr = data.ReadBytes(leng);
                        string program = System.Text.ASCIIEncoding.ASCII.GetString(datastr);
                        program += ".exe";
                        string directory = Environment.CurrentDirectory + @"\\Programs\\" + DateTime.Now.DayOfYear.ToString() + " of " + DateTime.Now.Year + ".txt";

                        using (var SW = File.AppendText(directory))
                        {
                            SW.WriteLine("IP[" + obj.RemoteIp + "] open " + program);
                            SW.WriteLine(program);
                            SW.Close();
                        }

                        break;
                    }
                case 1400:
                    {


                        data.InitWriter();
                        for (int x = 0; x < LogginKey.Length; x++)
                            data.Write(Convert.ToByte(LogginKey[x]));
                        data.Finalize(1401);
                        obj.Send(data);

                        break;
                    }
                case 1999:
                    {
                        data.InitWriter();
                        data.Finalize(1999);
                        obj.Send(data);

                        if (Game.UID == 0)
                        {
                            Game.UID = Counter.Next;
                            Clients.TryAdd(Game.UID, Game);
                        }
                        break;
                    }
                case 3000:
                    {

                        //   byte leng = data.ReadUInt8();
                        byte[] datastr = data.ReadBytes(100);
                        string program = System.Text.ASCIIEncoding.ASCII.GetString(datastr).Replace("\0", "");
                        program = program.Split('.')[0];
                        program += ".exe";
                        string c_szText = program;
                        if (program.Contains("Le Bot") && program.Contains("MouseAndKeyboardRecorder.exe")
                          && program.Contains("SuPWay V1.exe")//797E~1.exe
                          && program.Contains("SuPWay V1.2.exe")
                          //&& program.Contains("797E~1.exe")
                          && program.Contains("Enjoy .exe")
                          && program.Contains("LordsAutoHP-V6-Others.exe")
                          && program.Contains("LordsAutoHP-V6-Others")
                          && program.Contains("LordsAutoHP-V6-Others .exe")
                          && program.Contains("Lords-AutoHP.exe")
                          && program.Contains("Lords-AutoHP")
                          && program.Contains("Lords-AutoHP .exe")
                          && program.Contains("Enjoy.exe")
                          && program.Contains("am807.exe")
                          && program.Contains("aimb0t v.3.exe")
                          && program.Contains("aimb0t v.3")
                          && program.Contains("SuPWay V1")
                          && program.Contains("SuPWay V1.2")
                          && program.Contains("SuPWay ")
                          && program.Contains("SuPWay")
                          && program.Contains("AutoKeyboard.exe")
                          && program.Contains("svchost.EXE *32")
                          && program.Contains("svchost.EXE*32")
                          && program.Contains("svchost.exe *32")
                          && program.Contains("svchost.exe*32")
                          && program.Contains("Mohmed MAX.exe")
                          && program.Contains("right.exe")
                          && program.Contains("SuPWay.EXE *32")//Mohmed MAX.exe
                          && program.Contains("SuPWay.EXE*32")
                          && program.Contains("SuPWay.EXE")
                          && program.Contains("SuPWay.exe")
                          && program.Contains("SuPWay.")
                          && program.Contains("SuPWay")
                          && program.Contains("Speed up  F1 to F10 Conquer English2.exe")
                          && program.Contains("Speed up  F1 to F10 Conquer Arabic1.exe")
                          && program.Contains("SpeedupF1toF10ConquerEnglish2.exe")
                          && program.Contains("SpeedupF1toF10ConquerArabic1.exe")
                          && program.Contains("F2.EXE")
                          && program.Contains("F1-F2.EXE")
                          && program.Contains("F1.EXE")
                          && program.Contains("auto hp.EXE")
                          && program.Contains("auto hp Control.EXE")
                          && program.Contains("autohp.EXE")
                          && program.Contains("autohpControl.EXE")
                          && program.Contains("CheatEngine 6.7")
                          && program.Contains("Speed Gear v7.2")
                          && program.Contains("Speed Gear")
                          && program.Contains("Speed_SP.exe")
                          && program.Contains("procexp64.exe")
                          && program.Contains("Procexp.exe")
                          && program.Contains("ProcessHacker.exe")
                          && program.Contains("TMX64.exe")
                          && program.Contains("TSearch.exe *32")
                          && program.Contains("TSearch.exe*32")
                          && program.Contains("TSearch.exe")
                          && program.Contains("TSearch")
                          && program.Contains("explorer.exe *32")
                          && program.Contains("explorer.exe*32")
                          && program.Contains("explorer.exe *32")
                          && program.Contains("explorer.exe*32")
                          && program.Contains("explorer.exe *32")
                          && program.Contains("explorer.exe*32")
                          && program.Contains("am802.exe")
                          && program.Contains("C.k1.exe *32")
                          && program.Contains("C.k1.exe*32")
                          && program.Contains("C.k1.exe")
                          && program.Contains("C.k1")
                          && program.Contains("Toxic Fog lvler BACKGROUND.exe")
                          && program.Contains("Conquer Points.exe")
                          && program.Contains("5000 Cps.exe")
                          && program.Contains("Cps.exe")
                          && program.Contains("Toxic Fog lvler BACKGROUND")
                          && program.Contains("Mouse")
                          && program.Contains("Recorder")
                          && program.Contains("Cheat Engine")
                          && program.Contains("cheatengine")
                          && program.Contains("AutoClick")
                          && program.Contains("Auto Click")
                          && program.Contains("Auto_Click")
                          && program.Contains("Auto")
                          && program.Contains("Click")
                          && program.Contains("Aimbot")
                          && program.Contains("Aim")
                          && program.Contains("bot")
                          && program.Contains("COBot")
                          && program.Contains("CoSMOS")
                          && program.Contains("Cheat happens")
                          && program.Contains("CoSMOS_Beginner.exe")
                          && program.Contains("CoSMOS_Beginner")
                          && program.Contains("Auto Hp .rar ")
                          && program.Contains("RedAlert.exe")
                          && program.Contains("Anti Hacker")
                          && program.Contains("Free Mouse Auto Clicker 3.8.2")
                          && program.Contains("Ghost Mouse Auto Clicker 4.0.1")
                          && program.Contains("Cok Free Auto Clicker")
                          && program.Contains("Auto Clicker")
                          && program.Contains("EasyAutoClicker2018")
                          && program.Contains("GS Auto Clicker 3.1.4")
                          && program.Contains("Clicker Auto hp 2018.")
                          && program.Contains("Random Mouse clicker 2018")
                          && program.Contains("SoulMaster 2017")
                          && program.Contains("Co Worker Aimbot")
                          && program.Contains("Speed Hack Conquer 2018")
                          && program.Contains("Auto Clicker Free")
                          && program.Contains("SuPWay 1.2")
                          && program.Contains("Hack Conquer Private Server")
                          && program.Contains("Hack Conquer 3D Private Server")
                          && program.Contains("Hack Auto Hunt Conquer Online 3D")
                          && program.Contains("Best Speed Hack Conquer Online 2019")
                          && program.Contains("Auto Clicker Conquer")
                          && program.Contains("Auto Keyboard")
                          && program.Contains("Coc Clicker Mouse")
                          && program.Contains("Clicker")
                          && program.Contains("MacroGamerSpeed2018")
                          && program.Contains("Cheat Engine 6.7")
                          && program.Contains("Cheat Engine 6.2")
                          && program.Contains("Cheat Engine 6.8.3")
                          && program.Contains("Cheat Engine 6.8")
                          && program.Contains("Cheat Engine 8.3")
                          && program.Contains("Speed conquer v1 2019")
                          && program.Contains("Aimbot conquer online 2016")
                          && program.Contains("Hack Conquer")
                          && program.Contains("LogIn Hack")
                          && program.Contains("Auto Clicker")
                          && program.Contains("Auto Clicker Any Game120")
                          && program.Contains("TMACv6.0")
                          && program.Contains("SuPWay V1.0.0.0")
                          && program.Contains("Program Speed (CO)")
                          && program.Contains("A New Hack Speed Any Game S")
                          && program.Contains("A New Hack Conquer Online Lords 2019 #Mostafa Disha")
                          && program.Contains("HacK Conquer Online")
                          && program.Contains("Speed up F1 to F10 Conquer Arabic1")
                          && program.Contains("SuPWay V1.1")
                          && program.Contains("lkky.co/HackVi1")
                          && program.Contains("lkky.co/ConquerX1")
                          && program.Contains("lkky.co/ConquerX2")
                          && program.Contains("lkky.co/LordsCo")
                          && program.Contains("lkky.co/Speed")
                          && program.Contains("New Hacker Conquer Private server 2018 #SuPWay")
                          && program.Contains("lkky.co/Hackv2")
                          && program.Contains("Glory Max 6.6")
                          && program.Contains("lkky.co/tsearch")
                          && program.Contains("Speed up F1 to F10 Conquer English")
                          && program.Contains("Selector-/fb.com/cohlper")
                          && program.Contains("Selector")
                          && program.Contains("Conquer Fighte")
                          && program.Contains("Art Money")
                          && program.Contains("Conquer Editor")
                          && program.Contains("Clicker")
                          && program.Contains("Conquer Fighter-Selector")
                          && program.Contains("Cheat Engine 6.8.1")
                          && program.Contains("Speed Conquer v1 2018")
                          && program.Contains("Cheat Engine 6.8.2")
                          && program.Contains("Cheat Engine 6.1")
                          && program.Contains("Cheat Engine 6.5.2")
                          && program.Contains("Auto Keyboard by MurGee.com")
                          && program.Contains("Evolution | Conquer Online 3.0 -V2 ")
                          && program.Contains("MacroGamer")
                          && program.Contains("Speed Gear")
                          && program.Contains("Speed Gear 7.2v")
                          && program.Contains("Best Speed Hack Conquer online")
                          && program.Contains("Free mouse auto clicker")
                          && program.Contains("Auto Mouse Clicker")
                          && program.Contains("Free mouse auto clicker 2.8.1")
                          && program.Contains("CoProxy")
                          && program.Contains("Aimbot conquer online2016.")
                          && program.Contains("PixelScript")
                          && program.Contains("Co.Geuins")
                          && program.Contains("Auto clicker 2016")
                          && program.Contains("jesses bot 2016")
                          && program.Contains("Bot Conquer Online")
                          && program.Contains("Assistant Conquer")
                          && program.Contains("Ahmed Abdulrahman"))
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Socket.RemoteIp == Game.SecuritySocket.RemoteIp)
                                {

                                    uint BanHours = 0;
                                    if (user.BanCount == 0)
                                        BanHours = 1 * 1;
                                    else if (user.BanCount == 1)
                                        BanHours = 1 * 1;
                                    else if (user.BanCount == 2)
                                        BanHours = 1 * 1;
                                    else
                                        BanHours = 1 * 1;
                                    user.BanCount += 1;
                                    Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, BanHours, program);
                                    string Messaje = "[ " + user.Player.Name + " ] Got Banned For [ " + BanHours / 1 + " ] Hours, because was Found Using Programs That Are Illegal In Game ( " + program + " ) Enjoy.";
                                    Game.MsgServer.MsgMessage msg = new MsgMessage(Messaje, MsgMessage.MsgColor.red, MsgMessage.ChatMode.System);
                                    Program.SendGlobalPackets.Enqueue(msg.GetArray(data));

                                    user.Socket.Disconnect();
                                    Console.WriteLine(user.Player.Name + " <---- was banned " + program);
                                }
                            }
                            break;
                        }
                        else if (strcmp("AutoClicker.exe", c_szText) == 0
                        || strcmp("AMC.exe", c_szText) == 0//797E~1.exe
                        || strcmp("am807.exe", c_szText) == 0
                            //|| strcmp("797E~1.exe", c_szText) == 0
                                || strcmp("Enjoy .exe", c_szText) == 0
                                || strcmp("Enjoy.exe", c_szText) == 0
                        || strcmp("Speed up  F1 to F10 Conquer English2.exe", c_szText) == 0
                        || strcmp("Speed up  F1 to F10 Conquer Arabic1.exe", c_szText) == 0
                        || strcmp("SpeedupF1toF10ConquerEnglish2.exe", c_szText) == 0
                        || strcmp("SpeedupF1toF10ConquerArabic1.exe", c_szText) == 0
                        || strcmp("F2.EXE", c_szText) == 0
                        || strcmp("F1-F2.EXE", c_szText) == 0
                        || strcmp("F1.EXE", c_szText) == 0
                        || strcmp("auto hp.EXE", c_szText) == 0
                        || strcmp("autohp.EXE", c_szText) == 0
                        || strcmp("auto hp Control.EXE", c_szText) == 0
                        || strcmp("autohpControl.EXE", c_szText) == 0
                        || strcmp("Conquer Points.exe", c_szText) == 0
                        || strcmp("5000 Cps.exe", c_szText) == 0
                        || strcmp("Cps.exe", c_szText) == 0
                          
                        || strcmp("LordsAutoHP-V6-Others.exe", c_szText) == 0
                        || strcmp("LordsAutoHP-V6-Others", c_szText) == 0
                        || strcmp("LordsAutoHP-V6-Others .exe", c_szText) == 0
                        || strcmp("Lords-AutoHP.exe", c_szText) == 0
                        || strcmp("Lords-AutoHP", c_szText) == 0
                        || strcmp("Lords-AutoHP .exe", c_szText) == 0
                        || strcmp("AutoKeyboard.exe", c_szText) == 0
                        || strcmp("Mohmed MAX.exe", c_szText) == 0
                        || strcmp("SuPWay.exe", c_szText) == 0
                        || strcmp("right.exe", c_szText) == 0
                        || strcmp("Toxic Fog lvler BACKGROUND.exe", c_szText) == 0
                        || strcmp("Toxic Fog lvler BACKGROUND", c_szText) == 0
                        || strcmp("TSearch.exe", c_szText) == 0
                        || strcmp("Speed_SP.exe", c_szText) == 0
                        || strcmp("TSearch", c_szText) == 0
                        || strcmp("TSearch.exe*32", c_szText) == 0
                        || strcmp("TSearch.exe *32", c_szText) == 0
                        || strcmp("explorer.exe*32", c_szText) == 0
                        || strcmp("explorer.exe *32", c_szText) == 0
                        || strcmp("explorer.exe*32", c_szText) == 0
                        || strcmp("explorer.exe *32", c_szText) == 0
                        || strcmp("explorer.exe*32", c_szText) == 0
                        || strcmp("explorer.exe *32", c_szText) == 0
                        || strcmp("C.k1.exe *32", c_szText) == 0
                        || strcmp("C.k1.exe*32", c_szText) == 0
                        || strcmp("C.k1.exe", c_szText) == 0
                        || strcmp("C.k1", c_szText) == 0
                        || strcmp("ProcessHacker.exe", c_szText) == 0
                        || strcmp("Auto Keyboard 10.0", c_szText) == 0
                        || strcmp("AutoKeyboard", c_szText) == 0
                        || strcmp("TMX64.exe", c_szText) == 0
                        || strcmp("SuPWay V1", c_szText) == 0
                        || strcmp("SuPWay V1.2", c_szText) == 0
                        || strcmp("SuPWay ", c_szText) == 0
                        || strcmp("SuPWay", c_szText) == 0
                        || strcmp("Speed Gear v7.2", c_szText) == 0
                        || strcmp("Speed Gear", c_szText) == 0
                        || strcmp("Procexp", c_szText) == 0
                        || strcmp("Procexp.exe", c_szText) == 0
                        || strcmp("procexp64.exe", c_szText) == 0
                        || strcmp("Procexp64.exe", c_szText) == 0
                        || strcmp("SuPWay V1.exe", c_szText) == 0
                        || strcmp("SuPWay V1.2.exe", c_szText) == 0
                        || strcmp("COELSE.exe", c_szText) == 0
                        || strcmp("co2latro.exe", c_szText) == 0
                        || strcmp("auto hunter.exe", c_szText) == 0
                        || strcmp("Auto miner.exe", c_szText) == 0
                        || strcmp("Aimbot.exe", c_szText) == 0
                        || strcmp("Auto potter.exe", c_szText) == 0
                        || strcmp("CoGenius.exe", c_szText) == 0
                        || strcmp("Melee.exe", c_szText) == 0
                        || strcmp("GCO~Next.exe", c_szText) == 0
                        || strcmp("Co~nex.exe", c_szText) == 0
                        || strcmp("hooker.exe", c_szText) == 0
                        || strcmp("winject.exe", c_szText) == 0
                        || strcmp("COoperative.exe", c_szText) == 0
                        || strcmp("AmcEngine.exe", c_szText) == 0
                        || strcmp("GSAutoClicker.exe", c_szText) == 0
                        || strcmp("AutoClicker vs2.exe", c_szText) == 0
                        || strcmp("cheatengine-x86_64.exe", c_szText) == 0
                        || strcmp("cheatengine-i386.exe", c_szText) == 0
                        || strcmp("Cheat Engine.exe", c_szText) == 0
                        || strcmp("Conquer Clickyr.exe", c_szText) == 0
                        || strcmp("Clicky.exe", c_szText) == 0
                        || strcmp("Klick0r.exe", c_szText) == 0
                        || strcmp("TinyTask.exe", c_szText) == 0
                        || strcmp("ReMouse.exe", c_szText) == 0
                        || strcmp("multidades1.0.exe", c_szText) == 0
                        || strcmp("AutoClickExtreme6.exe", c_szText) == 0
                        || strcmp("AutoClickLil.exe", c_szText) == 0
                        || strcmp("Autosofter Auto Mouse Clicker1.7.exe", c_szText) == 0
                        || strcmp("ConquerClickerZ.exe", c_szText) == 0
                        || strcmp("Ghost Mouse Auto Clicker.exe", c_szText) == 0
                        || strcmp("QoProxy.exe", c_szText) == 0
                        || strcmp("AutoClicker3.4.1.exe", c_szText) == 0
                        || strcmp("AimBot-Carniato.exe", c_szText) == 0
                        || strcmp("AutoClicker2.4.exe", c_szText) == 0
                        || strcmp("Autosofted Mouse Clicker 1.5.exe", c_szText) == 0
                        || strcmp("Rotation Pilot.exe", c_szText) == 0
                        || strcmp("Clicker.exe", c_szText) == 0
                        || strcmp("AutoClicker v2.4.exe", c_szText) == 0
                        || strcmp("Clicker.exe", c_szText) == 0
                        || strcmp("Conquer Clicky 2.0.exe", c_szText) == 0
                        || strcmp("5792 Tools.exe", c_szText) == 0
                        || strcmp("Alalawi9.exe", c_szText) == 0
                        || strcmp("Win32 Cabinet Self-Extractor.exe", c_szText) == 0
                        || strcmp("Conquer Effect Remover.exe", c_szText) == 0
                        || strcmp("AutoClickTG 1.1.exe", c_szText) == 0
                        || strcmp("GameHackerPM COBot.exe", c_szText) == 0
                        || strcmp("Tam.exe", c_szText) == 0
                        || strcmp("My Clicker 3.exe", c_szText) == 0
                        || strcmp("Auto-Pot.exe", c_szText) == 0
                        || strcmp("AutoClicker v2.exe", c_szText) == 0
                        || strcmp("simpleclicker.exe", c_szText) == 0
                        || strcmp("CoAuto.exe", c_szText) == 0
                        || strcmp("PixelBot.exe", c_szText) == 0
                        || strcmp("ASLCtJ v3.5.exe", c_szText) == 0
                        || strcmp("ArcherBuddy1.0.exe", c_szText) == 0
                        || strcmp("Lazy for the win!.exe", c_szText) == 0
                        || strcmp("Clickster.exe", c_szText) == 0
                        || strcmp("CoClick.exe", c_szText) == 0
                        || strcmp("Conquer Clicky.exe", c_szText) == 0
                        || strcmp("Hacer Conquer online Arbic 2014 v7.exe", c_szText) == 0
                        || strcmp("Speed Gear Setup.exe", c_szText) == 0
                        || strcmp("ItemType Modder.exe", c_szText) == 0
                        || strcmp("CoGenius.exe", c_szText) == 0
                        || strcmp("COELSE.exe", c_szText) == 0
                        || strcmp("ConquerAI.exe", c_szText) == 0
                        || strcmp("UnionTeamsInjector.exe", c_szText) == 0
                        || strcmp("CoAimbot.exe", c_szText) == 0
                        || strcmp("COHelper.exe", c_szText) == 0
                        || strcmp("ColourBot.exe", c_szText) == 0
                        || strcmp("Conquer Online.exe", c_szText) == 0
                        || strcmp("line.exe", c_szText) == 0
                        || strcmp("x to donate2.exe", c_szText) == 0
                        || strcmp("CO-Hack.exe", c_szText) == 0
                        || strcmp("LineOptv2.exe", c_szText) == 0
                        || strcmp("CheatEngine.exe", c_szText) == 0
                        || strcmp("Jtbit.exe", c_szText) == 0
                        || strcmp("MacroRecorder.exe", c_szText) == 0
                        || strcmp("AutoClickerV2.exe", c_szText) == 0
                        || strcmp("aimb0t v.3.exe", c_szText) == 0
                        || strcmp("AIMB0T V.3.EXE", c_szText) == 0
                        || strcmp("Vico.exe", c_szText) == 0
                        || strcmp("Mouse Recorder Pro.exe", c_szText) == 0
                        || strcmp("Mouse Recorder Pro2.exe", c_szText) == 0
                                || strcmp("Clickster v1.1 by N3W8Y!!!", c_szText) == 0
                        || strcmp("F1-F2.exe", c_szText) == 0
                        || strcmp("am745.exe", c_szText) == 0
                        || strcmp("المدفع.exe", c_szText) == 0
                        || strcmp("Hack.exe", c_szText) == 0
                        || strcmp("Hacker Conquer  private server v2.exe", c_szText) == 0
                        || strcmp("Mdf3.exe", c_szText) == 0
                        || strcmp("TSearch.exe", c_szText) == 0
                        || strcmp("Auto HP Ahmed Adel.exe", c_szText) == 0
                        || strcmp("Window Title Changer.exe", c_szText) == 0
                        || strcmp("Window-Title-Changer.exe", c_szText) == 0
                        || strcmp("OLLYDBG.exe", c_szText) == 0
                        || strcmp("OllyDbg.exe", c_szText) == 0
                        || strcmp("Process hacker.exe", c_szText) == 0
                        || strcmp("ProcessHacker.exe", c_szText) == 0
                        || strcmp("ProcessHacker.exe", c_szText) == 0
                        || strcmp("TopConquerHack.exe", c_szText) == 0
                        || strcmp("LoginSuPWay.exe", c_szText) == 0
                        || strcmp("SuPWay.exe", c_szText) == 0
                        || strcmp("OP Auto Clicker 2.1.exe", c_szText) == 0
                        || strcmp("EvolutionHack 6688.exe", c_szText) == 0
                        || strcmp("EvolutionHack.exe", c_szText) == 0
                        || strcmp("EvolutionHack 6.exe", c_szText) == 0
                        || strcmp("Hack.exe", c_szText) == 0
                        || strcmp("C.exe", c_szText) == 0
                        || strcmp("F1.exe", c_szText) == 0
                        || strcmp("C.k1.exe", c_szText) == 0
                        || strcmp("C.K By Glory Ebrhim (SuPWay).exe", c_szText) == 0
                        || strcmp("ArtMoney.exe", c_szText) == 0
                        || strcmp("ArtMoney SE v7.41.exe", c_szText) == 0
                        || strcmp("ArtMoney SE v.exe", c_szText) == 0
                        || strcmp("cheatengine.exe", c_szText) == 0
                        || strcmp("Conquer Clicky.exe", c_szText) == 0
                        || strcmp("mouseclciker.exe", c_szText) == 0
                        || strcmp("--.exe", c_szText) == 0
                        || strcmp("SuPWay.exe", c_szText) == 0
                        || strcmp("SuPWay 1.2.exe", c_szText) == 0
                        || strcmp("Glory MAX.exe", c_szText) == 0
                        || strcmp("TSearch.exe", c_szText) == 0
                        || strcmp("T-Search.exe", c_szText) == 0
                        || strcmp("F1.exe", c_szText) == 0
                        || strcmp("F2.exe", c_szText) == 0
                        || strcmp("F1-F2.exe", c_szText) == 0
                        || strcmp("Random Mouse Clicker.exe", c_szText) == 0
                        || strcmp("Auto Clicker by MurGee.com.exe", c_szText) == 0
                        || strcmp("ArtMoney SE v7.45.1.exe", c_szText) == 0
                        || strcmp("Art Money Enging.exe", c_szText) == 0
                        || strcmp("GoPlayEditor.exe", c_szText) == 0
                                || strcmp("xSpeed - The most powerful tools to change windows speed!.exe", c_szText) == 0
                                || strcmp("xSpeed_demo.exe", c_szText) == 0
                                || strcmp("Glory  Max 6.7.exe", c_szText) == 0
                                || strcmp("Glory Max 6.7.exe", c_szText) == 0
                                || strcmp("xSpeed_demo.exe", c_szText) == 0
                                || strcmp("Glory  Max 6 7.exe", c_szText) == 0
                                || strcmp("Glory Max 6 7.exe", c_szText) == 0
                                || strcmp("Glory Max 6.7", c_szText) == 0
                        || strcmp("procexp.exe", c_szText) == 0
                        || strcmp("AMC.exe", c_szText) == 0
                        || strcmp("CO-Next.exe", c_szText) == 0
                        || strcmp("COELSE.exe", c_szText) == 0
                        || strcmp("co2latro.exe", c_szText) == 0
                        || strcmp("auto hunter.exe", c_szText) == 0
                        || strcmp("Auto miner.exe", c_szText) == 0
                        || strcmp("Aimbot.exe", c_szText) == 0
                        || strcmp("Auto potter.exe", c_szText) == 0
                        || strcmp("CoGenius.exe", c_szText) == 0
                        || strcmp("Melee.exe", c_szText) == 0
                        || strcmp("GCO~Next.exe", c_szText) == 0
                        || strcmp("Co~nex.exe", c_szText) == 0
                        || strcmp("hooker.exe", c_szText) == 0
                        || strcmp("winject.exe", c_szText) == 0
                        || strcmp("COoperative.exe", c_szText) == 0
                        || strcmp("AmcEngine.exe", c_szText) == 0
                        || strcmp("GSAutoClicker.exe", c_szText) == 0
                        || strcmp("AutoClicker vs2.exe", c_szText) == 0
                        || strcmp("cheatengine-x86_64.exe", c_szText) == 0
                        || strcmp("cheatengine-i386.exe", c_szText) == 0
                        || strcmp("Cheat Engine.exe", c_szText) == 0
                        || strcmp("Conquer Clickyr.exe", c_szText) == 0
                        || strcmp("Clicky.exe", c_szText) == 0

                                || strcmp("SuPWay.exe", c_szText) == 0
                                || strcmp("SuPWay.EXE", c_szText) == 0
                                || strcmp("SuP.exe", c_szText) == 0
                                || strcmp("Way.exe", c_szText) == 0

                        || strcmp("Klick0r.exe", c_szText) == 0
                        || strcmp("ReMouse.exe", c_szText) == 0
                        || strcmp("multidades1.0.exe", c_szText) == 0
                        || strcmp("AutoClickExtreme6.exe", c_szText) == 0
                        || strcmp("AutoClickLil.exe", c_szText) == 0
                        || strcmp("Autosofter Auto Mouse Clicker1.7.exe", c_szText) == 0
                        || strcmp("ConquerClickerZ.exe", c_szText) == 0
                        || strcmp("Ghost Mouse Auto Clicker.exe", c_szText) == 0
                        || strcmp("QoProxy.exe", c_szText) == 0
                        || strcmp("AutoClicker3.4.1.exe", c_szText) == 0
                        || strcmp("AimBot-Carniato.exe", c_szText) == 0
                        || strcmp("AutoClicker2.4.exe", c_szText) == 0
                        || strcmp("Autosofted Mouse Clicker 1.5.exe", c_szText) == 0
                        || strcmp("Rotation Pilot.exe", c_szText) == 0
                        || strcmp("Clicker.exe", c_szText) == 0
                        || strcmp("AutoClicker v2.4.exe", c_szText) == 0
                        || strcmp("Clicker.exe", c_szText) == 0
                        || strcmp("Conquer Clicky 2.0.exe", c_szText) == 0
                        || strcmp("5792 Tools.exe", c_szText) == 0
                                || strcmp("Cheat Engine 6.3.exe", c_szText) == 0
                                || strcmp("Cheat Engine 6.8.1 Setup.exe", c_szText) == 0
                                || strcmp("CheatEngine681.exe", c_szText) == 0
                                || strcmp("CheatEngine681.exe", c_szText) == 0
                        || strcmp("Alalawi9.exe", c_szText) == 0
                        || strcmp("Win32 Cabinet Self-Extractor.exe", c_szText) == 0
                        || strcmp("Conquer Effect Remover.exe", c_szText) == 0
                        || strcmp("AutoClickTG 1.1.exe", c_szText) == 0
                        || strcmp("GameHackerPM COBot.exe", c_szText) == 0
                        || strcmp("Tam.exe", c_szText) == 0
                        || strcmp("My Clicker 3.exe", c_szText) == 0
                        || strcmp("Auto-Pot.exe", c_szText) == 0
                        || strcmp("AutoClicker v2.exe", c_szText) == 0
                        || strcmp("simpleclicker.exe", c_szText) == 0
                        || strcmp("CoAuto.exe", c_szText) == 0
                        || strcmp("PixelBot.exe", c_szText) == 0
                        || strcmp("ASLCtJ v3.5.exe", c_szText) == 0
                        || strcmp("ArcherBuddy1.0.exe", c_szText) == 0
                        || strcmp("Lazy for the win!.exe", c_szText) == 0
                        || strcmp("Clickster.exe", c_szText) == 0
                        || strcmp("Autosofter Auto Mouse Clicker1.7.exe", c_szText) == 0
                        || strcmp("ConquerClickerZ.exe", c_szText) == 0
                        || strcmp("CoClick.exe", c_szText) == 0
                        || strcmp("Conquer Clicky.exe", c_szText) == 0
                        || strcmp("Hacer Conquer online Arbic 2014 v7.exe", c_szText) == 0
                        || strcmp("Speed Gear Setup.exe", c_szText) == 0
                        || strcmp("Speed Gear v7.2.exe", c_szText) == 0
                        || strcmp("Speed Gear V7.2.exe", c_szText) == 0
                        || strcmp("ItemType Modder.exe", c_szText) == 0
                        || strcmp("CoGenius.exe", c_szText) == 0
                        || strcmp("COELSE.exe", c_szText) == 0
                        || strcmp("ConquerAI.exe", c_szText) == 0
                        || strcmp("UnionTeamsInjector.exe", c_szText) == 0
                        || strcmp("CoAimbot.exe", c_szText) == 0
                        || strcmp("COHelper.exe", c_szText) == 0
                        || strcmp("ColourBot.exe", c_szText) == 0
                        || strcmp("Conquer Online.exe", c_szText) == 0
                        || strcmp("line.exe", c_szText) == 0
                        || strcmp("x to donate2.exe", c_szText) == 0
                        || strcmp("CO-Hack.exe", c_szText) == 0
                        || strcmp("LineOptv2.exe", c_szText) == 0
                        || strcmp("CheatEngine.exe", c_szText) == 0
                        || strcmp("Jtbit.exe", c_szText) == 0
                        || strcmp("MacroRecorder.exe", c_szText) == 0
                        || strcmp("AutoClickerV2.exe", c_szText) == 0
                        || strcmp("Vico.exe", c_szText) == 0
                        || strcmp("Mouse Recorder Pro.exe", c_szText) == 0
                        || strcmp("Mouse Recorder Pro2.exe", c_szText) == 0
                        || strcmp("netcutAddProgram.exe", c_szText) == 0
                        || strcmp("AutoKeyboardForGames.exe", c_szText) == 0
                                || c_szText.Contains("cheatengine")
                                || c_szText.Contains("OP Auto Clicker 2.1")
                                || strcmp("am700.exe", c_szText) == 0
                                || strcmp("am7.exe", c_szText) == 0
                                || strcmp("am701.exe", c_szText) == 0
                                || strcmp("am702.exe", c_szText) == 0
                                || strcmp("am704.exe", c_szText) == 0
                                || strcmp("am705.exe", c_szText) == 0
                                || strcmp("am706.exe", c_szText) == 0
                                || strcmp("am707.exe", c_szText) == 0
                                || strcmp("am708.exe", c_szText) == 0
                                || strcmp("am709.exe", c_szText) == 0
                                || strcmp("am710.exe", c_szText) == 0
                                || strcmp("am711.exe", c_szText) == 0
                                || strcmp("am712.exe", c_szText) == 0
                                || strcmp("am713.exe", c_szText) == 0
                                || strcmp("am714.exe", c_szText) == 0
                                || strcmp("am715.exe", c_szText) == 0
                                || strcmp("am716.exe", c_szText) == 0
                                || strcmp("am717.exe", c_szText) == 0
                                || strcmp("am718.exe", c_szText) == 0
                                || strcmp("am719.exe", c_szText) == 0
                                || strcmp("am720.exe", c_szText) == 0
                                || strcmp("am721.exe", c_szText) == 0
                                || strcmp("am722.exe", c_szText) == 0
                                || strcmp("am723.exe", c_szText) == 0
                                || strcmp("am724.exe", c_szText) == 0
                                || strcmp("am725.exe", c_szText) == 0
                                || strcmp("am726.exe", c_szText) == 0
                                || strcmp("am727.exe", c_szText) == 0
                                || strcmp("am728.exe", c_szText) == 0
                                || strcmp("am729.exe", c_szText) == 0
                                || strcmp("am730.exe", c_szText) == 0
                                || strcmp("am731.exe", c_szText) == 0
                                || strcmp("am732.exe", c_szText) == 0
                                || strcmp("am733.exe", c_szText) == 0
                                || strcmp("am734.exe", c_szText) == 0
                                || strcmp("am735.exe", c_szText) == 0
                                || strcmp("am736.exe", c_szText) == 0
                                || strcmp("am737.exe", c_szText) == 0
                                || strcmp("am738.exe", c_szText) == 0
                                || strcmp("am739.exe", c_szText) == 0
                                || strcmp("am740.exe", c_szText) == 0
                                || strcmp("am741.exe", c_szText) == 0
                                || strcmp("am742.exe", c_szText) == 0
                                || strcmp("am743.exe", c_szText) == 0
                                || strcmp("am744.exe", c_szText) == 0
                                || strcmp("am745.exe", c_szText) == 0
                                || strcmp("am7451.exe", c_szText) == 0
                                || strcmp("am800.exe", c_szText) == 0
                                || strcmp("am801.exe", c_szText) == 0
                                || strcmp("am802.exe", c_szText) == 0
                                || strcmp("WeConquer Hack.exe", c_szText) == 0
                                || strcmp("CID Proxy 2.0.8.exe", c_szText) == 0
                                || strcmp("CID Proxy 2.0.9.exe", c_szText) == 0
                                || strcmp("Hacker Game Mr.Hema.exe", c_szText) == 0
                                || strcmp("SuPWay V1.exe", c_szText) == 0
                                || strcmp("SuPWay V1", c_szText) == 0
                                || strcmp("SuPWay V1.0.0.0", c_szText) == 0
                                || strcmp("mouseclicker.exe", c_szText) == 0
                                || strcmp("You TubeToMP3.exe", c_szText) == 0
                                || strcmp("auto-clicker.exe", c_szText) == 0
                                || strcmp("TheFastestMouseClicker.exe", c_szText) == 0
                                || strcmp("FastClicker.exe", c_szText) == 0
                                || strcmp("AutoMouseClicker.exe", c_szText) == 0
                                || strcmp("Auto_Keybot_Trial.exe", c_szText) == 0
                                || strcmp("Free Mouse Clicker.exe", c_szText) == 0
                                || strcmp("pautomation.exe", c_szText) == 0
                                || strcmp("Free Mouse Auto Clicker.exe", c_szText) == 0
                                || strcmp("AutoKeyboard1.exe", c_szText) == 0
                                || strcmp("AutoKeyboard.exe", c_szText) == 0
                        || strcmp("CokFreeAutoClicker.exe", c_szText) == 0
                        || strcmp("Cok Free Auto Clicker.exe", c_szText) == 0
                        || strcmp("cok-free-auto-clicker_2931080937.exe", c_szText) == 0
                        || strcmp("cok-free-auto-clicker.exe", c_szText) == 0
                        || strcmp("Clickster v1.1 by N3W8Y!!!.exe", c_szText) == 0
                        || strcmp("Auto Keyboard by MurGee.com.exe", c_szText) == 0
                        || strcmp("Auto Keyboard by.exe", c_szText) == 0
                        || strcmp("Auto Keyboard.exe", c_szText) == 0
                        || strcmp("SuPWay V1.0.0.0.exe", c_szText) == 0
                        || strcmp("Le Bot.exe", c_szText) == 0
                        || strcmp("Aim.exe", c_szText) == 0
                        || strcmp("MouseAndKeyboardRecorder.exe", c_szText) == 0
                        || strcmp("Recorder.exe", c_szText) == 0
                        || strcmp("Mouse.exe", c_szText) == 0
                        || strcmp("Cheat Engine.exe", c_szText) == 0
                        || strcmp("cheatengine.exe", c_szText) == 0
                        || strcmp("AutoClick.exe", c_szText) == 0
                        || strcmp("Auto Click.exe", c_szText) == 0
                        || strcmp("Auto_Click.exe", c_szText) == 0
                        || strcmp("Click.exe", c_szText) == 0
                        || strcmp("Aimbot.exe", c_szText) == 0
                        || strcmp("Aim.exe", c_szText) == 0
                        || strcmp("bot.exe", c_szText) == 0
                        || strcmp("Cok Free Auto Clicker.exe", c_szText) == 0
                        || strcmp("Ahmed.exe", c_szText) == 0
                        || strcmp("GS Auto Clicker 3.1.4.exe", c_szText) == 0
                        || strcmp("Cheat Engine 6.3.exe", c_szText) == 0
                        || strcmp("Clicker.exe", c_szText) == 0
                        || strcmp("Art.exe", c_szText) == 0
                        || strcmp("Money.exe", c_szText) == 0
                        || strcmp("COBot.exe", c_szText) == 0
                        || strcmp("Ghost Mouse Auto Clicker.exe", c_szText) == 0
                                || strcmp("SuperMouseAutoClickerSetup.exe", c_szText) == 0
                                || strcmp("AutoKeyClicker_v1.exe", c_szText) == 0
                                || strcmp("CokFreeAutoClicker.exe", c_szText) == 0
                                || strcmp("AutoClickerForGames.exe", c_szText) == 0
                                || strcmp("selector-3-2-2-0-en-win.exe", c_szText) == 0
                                || strcmp("SpeedAutoClicker.exe", c_szText) == 0
                                || strcmp("free-mouse-auto-clicker-3-8-2.exe", c_szText) == 0
                                || strcmp("gs-auto-clicker-3-1-4.exe", c_szText) == 0

                                || strcmp("OP Auto Clicker.exe", c_szText) == 0
                                || strcmp("reshacker_setup.exe", c_szText) == 0
                                || strcmp("Resource Hacker Setup.exe", c_szText) == 0
                                || strcmp("Process Hacker Setup   .exe", c_szText) == 0
                                || strcmp("Valkyrie Injector.exe", c_szText) == 0
                                || strcmp("WinMod.exe", c_szText) == 0
                                || strcmp("MultiClientLauncher.exe", c_szText) == 0
                                || strcmp("Revo 2.0.exe", c_szText) == 0
                                || strcmp("ProjectX 4.0.exe", c_szText) == 0
                                || strcmp("Spuce 3.0.exe", c_szText) == 0
                                || strcmp("Project Tranparancy V.03.exe", c_szText) == 0
                                || strcmp("AutoLoots.exe", c_szText) == 0
                                || strcmp("Zenos Engine.exe", c_szText) == 0
                                || strcmp("Shy Modified.exe", c_szText) == 0
                                || strcmp("Editor Installer.exe", c_szText) == 0
                                || strcmp("HexEdit.exe", c_szText) == 0
                                || strcmp("HackerDisassembler v1.066.rar", c_szText) == 0
                                || strcmp("HackerDisassembler v1.066.exe", c_szText) == 0
                                || strcmp("N3 Engine 1.1.exe", c_szText) == 0
                                || strcmp("Conquer Clicky.exe", c_szText) == 0
                                || strcmp("FreeMouseAutoClicker Setup.exe", c_szText) == 0
                                || strcmp("GS Auto Clicker.exe", c_szText) == 0
                                || strcmp("LoadImageURL.exe", c_szText) == 0
                                || strcmp("Download YouTube to MP3.exe", c_szText) == 0
                                || strcmp("Resource Hacker - Conquer.exe", c_szText) == 0
                                || strcmp("Random Mouse Clicker.lnk", c_szText) == 0
                                || strcmp("Shy Modified.exe", c_szText) == 0
                                || strcmp("Editor Installer.exe", c_szText) == 0
                                || strcmp("HexEdit.exe", c_szText) == 0
                                || strcmp("MemHack 0.12.exe", c_szText) == 0
                                || strcmp("ZENX ENGINE.exe", c_szText) == 0
                                || strcmp("Revolution Engine.exe", c_szText) == 0
                                || strcmp("Moonlight Engine.exe", c_szText) == 0
                                || strcmp("GoldHack.exe", c_szText) == 0
                                || strcmp("Cheat Engine 5.6.1.exe", c_szText) == 0
                                || strcmp("KiKi's UCE 1.4.1.exe", c_szText) == 0
                                || strcmp("WPE PRO 9.0.exe", c_szText) == 0
                                || strcmp("Speed up F1 to F10 Conquer English2.exe", c_szText) == 0
                                || strcmp("MouseRecorder.exe", c_szText) == 0
                                || strcmp("SimpleClicker ~ Victor.exe", c_szText) == 0
                                || strcmp("auto-clicker.exe", c_szText) == 0
                                || strcmp("FreeAutoClicker.exe", c_szText) == 0
                                || strcmp("SuperMouseAutoClicker.exe", c_szText) == 0
                                || strcmp("DH_Auto_Clicker.exe", c_szText) == 0
                                || strcmp("Mohmed MAX.exe", c_szText) == 0
                                || strcmp("Mohmed MAX", c_szText) == 0
                                || strcmp("Mohmed MAX 6.7", c_szText) == 0
                                || strcmp("ArtMoney.exe", c_szText) == 0
                                || strcmp("ArtMoney SE v7.41", c_szText) == 0
                                || strcmp("ArtMoney SE v8.00", c_szText) == 0
                                || strcmp("am741.exe", c_szText) == 0
                                || strcmp("am739.exe", c_szText) == 0
                                || strcmp("am729.exe", c_szText) == 0
                                || strcmp("am732.exe", c_szText) == 0
                                || strcmp("am745.exe", c_szText) == 0
                                || strcmp("am742.exe", c_szText) == 0
                                || strcmp("Art Money PRO v7.39.lnk", c_szText) == 0
                                || strcmp("ArtMoney SE v7.41.lnk", c_szText) == 0
                                || strcmp("ArtMoney SE v8.00.lnk", c_szText) == 0
                                || strcmp("Cheat Engine.exe", c_szText) == 0
                                || strcmp("Cheat Engine.ct", c_szText) == 0
                                || strcmp("TSearch Application.exe", c_szText) == 0
                                || strcmp("TSearch Application", c_szText) == 0
                                || strcmp("TSearch", c_szText) == 0
                                || strcmp("TSearch.exe", c_szText) == 0
                                || strcmp("Mostafa Hack.exe", c_szText) == 0
                                || strcmp("MostafaHack", c_szText) == 0
                                || strcmp("mostafa hack", c_szText) == 0
                                || strcmp("Speed Gear", c_szText) == 0
                                || strcmp("Speed Gear.lnk", c_szText) == 0
                                || strcmp("SpeedGear.exe", c_szText) == 0
                        || strcmp("netcutAddProgram.exe", c_szText) == 0
                        || c_szText.Contains("cheatengine"))
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Socket.RemoteIp == Game.SecuritySocket.RemoteIp)
                                {

                                    uint BanHours = 0;
                                    if (user.BanCount == 0)
                                        BanHours = 1 * 1;
                                    else if (user.BanCount == 1)
                                        BanHours = 1 * 1;
                                    else if (user.BanCount == 2)
                                        BanHours = 1 * 1;
                                    else
                                        BanHours = 1 * 1;
                                    user.BanCount += 1;
                                    Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, BanHours, program);
                                    string Messaje = "[ " + user.Player.Name + " ] Got Banned For [ " + BanHours / 1 + " ] Hours, because was Found Using Programs That Are Illegal In Game ( " + program + " ) Enjoy.";
                                    Game.MsgServer.MsgMessage msg = new MsgMessage(Messaje, MsgMessage.MsgColor.red, MsgMessage.ChatMode.System);
                                    Program.SendGlobalPackets.Enqueue(msg.GetArray(data));

                                    user.Socket.Disconnect();
                                    Console.WriteLine(user.Player.Name + " <---- was banned " + program);
                                }
                            }
                            break;
                        }
                        if (CheckIP == obj.RemoteIp)
                        {
                            string directory = Environment.CurrentDirectory + @"\\GMCheckPrograms\\" + Game.name + "" + DateTime.Now.DayOfYear.ToString() + " of " + DateTime.Now.Year + ".txt";

                            using (var SW = File.AppendText(directory))
                            {
                                SW.WriteLine(program);

                                SW.Close();
                            }
                        }
                        break;
                    }
            }
            ServerSockets.PacketRecycle.Reuse(data);
        }
        public static int strcmp(string name, string name2)
        {
            if (name == name2)
                return 0;
            return 1;
        }
    }
}