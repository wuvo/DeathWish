namespace TqShield
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    public class Types
    {

        public enum PacketTypes : Int16
        {
            CMsgShield = 10050,
            CMsgMemoryCheck = 10053,
            CMsgCloseClient = 10054,
            CMsgRequestLogin = 10055,
            CMsgOpenedProcesses = 10056,
            CMsgPlayerCommands = 10057,
            CMsgHotKey = 10058,
            UpdateCustomWord = 10059,
        }
        public enum CheatFlags : Int16
        {
            None = 0,
            Program = 2,
            AutoClicker = 4,
            AutoKeyboard = 6,
            SpeedCClient = 8,
            Debugger = 10,
            InjectDll = 12,
            CloseingThread = 14,
            ChangeConquerFile = 16,
            dbg = 18,
            GamingMouse = 20
        }
        public enum SubType : Int16
        {
            DoLogin = 10,
            ThreadCheck = 20,
            MemoryCheck = 30,
            HackDetected = 40,
            DiscordTitle1 = 50,
            DiscordTitle2 = 60,
            ProcessesCheck = 70,
            ConquerFileHash = 80,
        }
        public enum ProcessesType : Int16
        {
            Start = 110,
            Insert = 220,
            Finish = 330,
        }
    }
}
