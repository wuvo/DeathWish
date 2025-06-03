namespace DeathWish.Game.MsgLoader
{
    using System;
    using TqShield;
    using System.IO;
    using System.Text;
    using ServerSockets;
    using System.Threading.Tasks;
    using DeathWish.Game.MsgTournaments;
    using System.Collections.Generic;

    public static class CheatPacket
    {
        const ushort RequiredDllVersion = 1000;
        public static uint[] keys = {  3184399020, 1587232413, 1121456739, 3650391128,
                           2003402937, 1456738923, 2187654321, 3412891345 };
        public static uint DecryptIdentifier(uint encryptedIdentifier, uint[] keys)
        {
            foreach (var key in keys)
            {
                encryptedIdentifier ^= key;
            }
            return encryptedIdentifier;
        }
        [Packet((UInt16)Types.PacketTypes.CMsgRequestLogin)]
        public static void RequestLogin(Client.GameClient pClient, Packet packet)
        {
            uint encryptedIdentifier = packet.ReadUInt32();
            pClient.OnLogin.Key = DecryptIdentifier(encryptedIdentifier, keys);

            pClient.TqSerial = packet.ReadCString(12).Replace("\0", "");

            pClient.Version = packet.ReadUInt16();

            pClient.DllVersion = packet.ReadUInt16();

            pClient.LoaderLanguage = packet.ReadCString(2);

            pClient.OnLogin.AccountHash = packet.ReadUInt64();

            if (!IsValidLoginData(pClient))
            {
                pClient.Socket.Disconnect();
                return;
            }

            if (pClient.DllVersion != RequiredDllVersion)
            {
                LogUnsupportedDllVersion(pClient);
                pClient.Socket.Disconnect();
                return;
            }

            pClient.ClientFlag |= Client.ServerFlag.OnLoggion;
            Database.ServerDatabase.LoginQueue.TryEnqueue(pClient);
        }

        private static bool IsValidLoginData(Client.GameClient client)
        {
            if (client.OnLogin.Key == 0 || string.IsNullOrEmpty(client.TqSerial) || string.IsNullOrEmpty(client.Player.Language) ||
                client.Version == 0 || client.DllVersion == 0 || client.OnLogin.AccountHash == 0)
            {
                string reason = "Invalid or missing data: ";
                reason += client.OnLogin.Key == 0 ? "Key is missing or invalid. " : string.Empty;
                reason += string.IsNullOrEmpty(client.TqSerial) ? "TqSerial is missing or invalid. " : string.Empty;
                reason += string.IsNullOrEmpty(client.Player.Language) ? "Language is missing or invalid. " : string.Empty;
                reason += client.Version == 0 ? "Version is missing or invalid. " : string.Empty;
                reason += client.DllVersion == 0 ? "DllVersion is missing or invalid. " : string.Empty;
                reason += client.OnLogin.AccountHash == 0 ? "AccountHash is missing or invalid. " : string.Empty;
                Console.WriteLine($"{reason} UID {client.OnLogin.Key}");
                return false;
            }
            return true;
        }
        private static void LogUnsupportedDllVersion(Client.GameClient client)
        {
            Console.WriteLine($"Player with UID {client.OnLogin.Key} attempted to login with an unsupported DLL version. Current version: {client.DllVersion}, Required version: {RequiredDllVersion}.");
        }
        [PacketAttribute((ushort)Types.PacketTypes.CMsgShield)]
        static async void LoaderHandler(Client.GameClient pClient, Packet stream)
        {
            await Task.Run(async() =>
            {
                pClient.ProCipher.Decrypt(stream);
                stream.Seek(0);
                var pBuffer = stream.ReadBytes(stream.Size);

                BinaryReader BR = new BinaryReader(new MemoryStream(pBuffer));
                BR.BaseStream.Seek(4, SeekOrigin.Current);
                Types.SubType Type = (Types.SubType)BR.ReadInt16();
                try
                {
                    #region HackDetected

                    if (Type == Types.SubType.HackDetected)
                    {
                        Types.CheatFlags Flags = (Types.CheatFlags)BR.ReadInt16();
                        Int16 lReaosn = BR.ReadInt16();
                        String ReaosnString = Encoding.Default.GetString(BR.ReadBytes(lReaosn)).Replace("\0", "");
                        switch (Flags)
                        {
                            //case Types.CheatFlags.Program: Detected(pClient, $"Program: {ReaosnString}"); break;
                            case Types.CheatFlags.Debugger: Detected(pClient, $"Debugger: {ReaosnString}"); break;
                            case Types.CheatFlags.SpeedCClient: Detected(pClient, $"Speed Client: {ReaosnString}"); break;
                            //case Types.CheatFlags.AutoClicker: Detected(pClient, $"Auto Clicker: {ReaosnString}"); break;
                            //case Types.CheatFlags.AutoKeyboard: Detected(pClient, $"Auto Keyboard: {ReaosnString}"); break;
                            case Types.CheatFlags.CloseingThread: Detected(pClient, $"Closing Thread: {ReaosnString}"); break;
                            case Types.CheatFlags.dbg: Detected(pClient, $"Dbg: {ReaosnString}"); break;
                            //case Types.CheatFlags.GamingMouse: Detected(pClient, $"GamingMouse: {ReaosnString}"); break;

                        }
                    }
                    #endregion

                    #region ConquerFileHash
                    if (Type == Types.SubType.ConquerFileHash)
                    {
                        String[] Files = new String[9];
                        for (UInt16 i = 0; i < Files.Length; i++)
                        {
                            try
                            {
                                Files[i] = Encoding.Default.GetString(BR.ReadBytes(33)).Replace("\0", "");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Warning: Error reading file hash: " + ex.Message);
                                Files[i] = String.Empty;
                            }
                        }

                    
                        //if (!_TqShield.Validated(pClient.LoaderLanguage, Files[0], Files[1], Files[2], Files[3], Files[4], Files[5], Files[6], out string StrParam))
                        //{

                        //    pClient.Player.MessageBox("Modified files [ " + StrParam + "] in client or not updated to latest patch! Please patch! You'll be disconnected.", null, null);
                        //    await Task.Delay(TimeSpan.FromSeconds(5));
                        //    Console.WriteLine(pClient.Player.Name + " Detected Files Changed On Client [ " + StrParam + "]", ConsoleColor.Red);
                        //    using (var Recycled = new RecycledPacket())
                        //    {
                        //        var cPacket = Recycled.GetStream();

                        //        string detectedMessage = $"Detected Files Changed On Client [{StrParam}]";
                        //        pClient.Send(CheatPacket.SendClosePacket(cPacket, 6, detectedMessage));
                        //    }
                        //    CheatPacket.Detectedfile(pClient, "Detected Files Changed On Client[" + StrParam + " ]");
                        //    using (var rec = new ServerSockets.RecycledPacket())
                        //    {
                        //        var cPacket = rec.GetStream();
                        //        Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage("" + pClient.Player.Name + " got preason for, because was found Change file (" + StrParam + ")."
                        //        , Game.MsgServer.MsgMessage.MsgColor.white, Game.MsgServer.MsgMessage.ChatMode.BroadcastMessage).GetArray(stream));
                        //    }
                        //    Console.WriteLine("Player ( " + pClient.Player.Name + " ) <= Was Banned [ " + StrParam + " ]", ConsoleColor.Red);
                            
                        //}

                    }
                    #endregion
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString());
                    if (pClient.Socket.Alive)
                    {
                        pClient.Socket.Disconnect();
                    }
                }
                BR.Close();
            });
        }

        [Packet((UInt16)Types.PacketTypes.CMsgMemoryCheck)]
        static void MemoryShield(Client.GameClient pClient, Packet stream)
        {
            pClient.ProCipher.Decrypt(stream);

            stream.Seek(4);
            String FirstMemoryDump, SecondMemoryDump;
            var pBuffer = stream.ReadBytes(stream.Size);
            using (BinaryReader BR = new BinaryReader(new MemoryStream(pBuffer)))
            {
                FirstMemoryDump = Encoding.Default.GetString(BR.ReadBytes(17)).Replace("\0", "");
                SecondMemoryDump = Encoding.Default.GetString(BR.ReadBytes(17)).Replace("\0", "");

                if (FirstMemoryDump != String.Empty && SecondMemoryDump != String.Empty)
                {
                    if (FirstMemoryDump != SecondMemoryDump)
                    {
                        Detected(pClient, "ModifdeMemory");
                    }
                }
                BR.Close();
            }
        }
        public static Packet SendProPacket(Packet Stream, Types.SubType sData)
        {
            Stream.InitWriter();
            Stream.Write(Convert.ToInt32(sData));
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgShield));
            return Stream;
        }
        public static Packet DiscordTitle1Create(this Packet Stream, String Title)
        {
            Stream.InitWriter();
            Stream.Write(Convert.ToInt32(Types.SubType.DiscordTitle1));
            Stream.Write((Int16)Title.Length);
            Stream.Write(Title, Title.Length);
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgShield));
            return Stream;
        }
        public static Packet DiscordTitle2Create(this Packet Stream, UInt16 FaceID, String Title)
        {
            Stream.InitWriter();
            Stream.Write(Convert.ToInt32(Types.SubType.DiscordTitle2));
            Stream.Write(Convert.ToInt32(FaceID).ToString(), 3);
            Stream.Write((Int16)Title.Length);
            Stream.Write(Title, Title.Length);
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgShield));
            return Stream;
        }
     
        public static Packet SendPlayerCommand(Packet Stream, UInt16 CommandType, UInt16 CommandValue)
        {
            Stream.InitWriter();
            Stream.Write(CommandType);
            Stream.Write(CommandValue);
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgPlayerCommands));
            return Stream;
        }
        public static Packet SendHotKey(Packet Stream, UInt16 State, UInt32 DelayMilliseconds)
        {
            Stream.InitWriter();
            Stream.Write(State);
            Stream.Write(DelayMilliseconds);
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgHotKey));
            return Stream;
        }
        public static Packet SendUpdateCustomWord(Packet Stream, string newWord)
        {

            Stream.InitWriter();
            Stream.Write((UInt16)Types.PacketTypes.UpdateCustomWord);
            Stream.Write((UInt16)newWord.Length);
            Stream.Write(newWord, newWord.Length);
            Stream.Finalize((UInt16)Types.PacketTypes.UpdateCustomWord);
            return Stream;
        }
        public static Packet SendClosePacket(Packet Stream, Int16 AfterTime, String Message)
        {
            Stream.InitWriter();
            Stream.Write(AfterTime);
            Stream.Write((Int16)Message.Length);
            Stream.Write(Message, Message.Length);
            Stream.Finalize(Convert.ToUInt16(Types.PacketTypes.CMsgCloseClient));
            return Stream;
        }
        public static void SendDiscordStatus(Client.GameClient pClient)
        {
            using (var rec = new RecycledPacket())
            {
                var cPacket = rec.GetStream();
                pClient.Send(DiscordTitle1Create(cPacket, string.Format("{0}({1})", pClient.Player.Name, pClient.Player.NobilityRank.ToString())));
                pClient.Send(DiscordTitle2Create(cPacket, pClient.Player.Face, string.Format("{0}", Helper.GetJobName(pClient.Player.Class))));
            }
        }
        public static void SendPacket(Client.GameClient pClient, Types.SubType In)
        {
            using (var Recycled = new RecycledPacket())
            {
                var cPacket = Recycled.GetStream();
                pClient.Send(SendProPacket(cPacket, In));
            }
        }
        public static void OnThread(Client.GameClient pClient)
        {
            if (DateTime.Now > pClient.MemoryCheckStamp.AddSeconds(20))
            {
                pClient.MemoryCheckStamp = DateTime.Now;
                SendPacket(pClient, Types.SubType.MemoryCheck);
            }

            if (DateTime.Now > pClient.ThreadCheckStamp.AddSeconds(75))
            {
                pClient.ThreadCheckStamp = DateTime.Now;
                SendPacket(pClient, Types.SubType.ThreadCheck);
            }

            if (DateTime.Now > pClient.ProcessesCheckStamp.AddSeconds(45))
            {
                pClient.ProcessesCheckStamp = DateTime.Now;
                SendPacket(pClient, Types.SubType.ProcessesCheck);
            }
            if (DateTime.Now > pClient.ConquerFileHashStamp.AddSeconds(300))
            {
                pClient.ConquerFileHashStamp = DateTime.Now;
                SendPacket(pClient, Types.SubType.ConquerFileHash);
            }
        }
        public static void OnLogin(Client.GameClient pClient)
        {
            SendDiscordStatus(pClient);
            SendPacket(pClient, Types.SubType.DoLogin);
            SendPacket(pClient, Types.SubType.ThreadCheck);
            SendPacket(pClient, Types.SubType.ConquerFileHash);

        }

        #region Detected Handle
        public static void Detected(Client.GameClient pClient, string used)
        {
            if (Database.SystemBannedAccount.BannedPoll.ContainsKey(pClient.Player.UID)) return;

            if (!string.IsNullOrEmpty(used))
            {
                uint banHours = GetBanHours(pClient.BanCount);
                pClient.BanCount += 0;

                LogBanEvent(pClient, used);
                NotifyPlayers(pClient, used);
                Console.WriteLine($"Player ({pClient.Player.Name}) <= Was Banned [{used}]", ConsoleColor.Red);

                SaveBanDetailsToFile(pClient, used, banHours);
                pClient.Socket.Disconnect();
            }
        }

        public static void Detectedfile(Client.GameClient pClient, String Used)
        {
            if (!Database.SystemBannedAccount.BannedPoll.ContainsKey(pClient.Player.UID))
            {
                if (Used != String.Empty)
                {

                    string logs = $"[FileChaned] {pClient.Player.Name } -- Changed file: {Used}";
                    Database.ServerDatabase.LoginQueue.Enqueue(logs);
                    SaveFileDetailsToFile(pClient, Used);

                }
            }
        }
        private static uint GetBanHours(int banCount)
        {
            uint[] banHoursOptions = new uint[] { 24 * 1, 24 * 7, 24 * 14, 24 * 364 };
            return banCount >= banHoursOptions.Length ? banHoursOptions[banHoursOptions.Length - 1] : banHoursOptions[banCount];
        }

        private static void LogBanEvent(Client.GameClient pClient, string used)
        {
            string logEntry = $"[CHEAT] {pClient.Player.Name} -- REASON: {used}";
            Database.ServerDatabase.LoginQueue.Enqueue(logEntry);
        }

        private static void NotifyPlayers(Client.GameClient pClient, string used)
        {
            MsgSchedules.SendSysMesage($"{pClient.Player.Name} was banned for using illegal programs ({used}).",
                Game.MsgServer.MsgMessage.ChatMode.Center,
                Game.MsgServer.MsgMessage.MsgColor.red);

            using (var rec = new ServerSockets.RecycledPacket())
            {
                var stream = rec.GetStream();
                Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage(
                    $"{pClient.Player.Name} was banned for using illegal programs ({used}).",
                    Game.MsgServer.MsgMessage.MsgColor.white,
                    Game.MsgServer.MsgMessage.ChatMode.BroadcastMessage).GetArray(stream));
            }
        }

        private static void SaveFileDetailsToFile(Client.GameClient pClient, string used)
        {
            string directory = Path.Combine(Environment.CurrentDirectory, "FileChaned");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, $"{pClient.Player.Name} - {DateTime.Now:dd-MMMM-yyyy}.txt");

            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("---------------------------------------------------------------------");
                sw.WriteLine($"Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sw.WriteLine($"Player Name: {pClient.Player.Name}");
                sw.WriteLine($"UID: {pClient.Player.UID}");
                sw.WriteLine($"TQ Serial: {pClient.TqSerial}");
                sw.WriteLine($"IP Address: {pClient.Socket.RemoteIp}");
                sw.WriteLine($"MAC Address: {pClient.Socket.GetMACAddress()}");
                sw.WriteLine($"Change File: {used}");
                sw.WriteLine("---------------------------------------------------------------------\n");
            }
        }

        private static void SaveBanDetailsToFile(Client.GameClient pClient, string used, uint banHours)
        {
            string directory = Path.Combine(Environment.CurrentDirectory, "BannedUsersCheat");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, $"{pClient.Player.Name} - {DateTime.Now:dd-MMMM-yyyy}.txt");

            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("---------------------------------------------------------------------");
                sw.WriteLine($"Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sw.WriteLine($"Player Name: {pClient.Player.Name}");
                sw.WriteLine($"UID: {pClient.Player.UID}");
                sw.WriteLine($"TQ Serial: {pClient.TqSerial}");
                sw.WriteLine($"IP Address: {pClient.Socket.RemoteIp}");
                sw.WriteLine($"Banned For: {banHours} hours");
                sw.WriteLine($"Ban Count: {pClient.BanCount}");
                sw.WriteLine($"Using Program: {used}");
                sw.WriteLine("---------------------------------------------------------------------\n");
            }
        }
        #endregion

    }

public static class Helper
    {
        private static readonly Dictionary<byte, string> jobNames = new Dictionary<byte, string>
    {
        // Trojan
        { 10, "InternTrojan" }, { 11, "Trojan" }, { 12, "VeteranTrojan" },
        { 13, "TigerTrojan" }, { 14, "DragonTrojan" }, { 15, "TrojanMaster" },
        // Warrior
        { 20, "InternWarrior" }, { 21, "Warrior" }, { 22, "BrassWarrior" },
        { 23, "SilverWarrior" }, { 24, "GoldWarrior" }, { 25, "WarriorMaster" },
        // Archer
        { 40, "InternArcher" }, { 41, "Archer" }, { 42, "EagleArcher" },
        { 43, "TigerArcher" }, { 44, "DragonArcher" }, { 45, "ArcherMaster" },
        // Ninja
        { 50, "InternNinja" }, { 51, "Ninja" }, { 52, "MiddleNinja" },
        { 53, "DarkNinja" }, { 54, "MysticNinja" }, { 55, "NinjaMaster" },
        // Monk
        { 60, "InternMonk" }, { 61, "Monk" }, { 62, "DhyanaMonk" },
        { 63, "DharmaMonk" }, { 64, "PrajnaMonk" }, { 65, "NirvanaMonk" },
        // Pirate
        { 70, "InternMalePirate" }, { 71, "MalePirate" }, { 72, "PirateGunner" },
        { 73, "Quartermaster" }, { 74, "PirateCaptain" }, { 75, "PirateLord" },
        // Dragon-Warrior
        { 80, "NoviceLeeLong" }, { 81, "LeeLong" }, { 82, "Expert~LeeLong" },
        { 83, "Elite~LeeLong" }, { 84, "Master~LeeLong" }, { 85, "King~LeeLong" },
        // Taoist
        { 100, "InternTaoist" }, { 101, "Taoist" },
        { 112, "MetalTaoist" }, { 113, "MetalWizard" }, { 114, "MetalMaster" },
        { 115, "MetalSaint" }, { 122, "WoodTaoist" }, { 123, "WoodWizard" },
        { 124, "WoodMaster" }, { 125, "WoodSaint" },
        // Water
        { 132, "WaterTaoist" }, { 133, "WaterWizard" }, { 134, "WaterMaster" },
        { 135, "WaterSaint" },
        // Fire
        { 142, "FireTaoist" }, { 143, "FireWizard" }, { 144, "FireMaster" },
        { 145, "FireSaint" }, { 152, "EarthTaoist" }, { 153, "EarthWizard" },
        { 154, "EarthMaster" }, { 155, "EarthSaint" },
        // Windwalker
        { 160, "Windwalker" }, { 161, "WindGuard" }, { 162, "WindOfficer" },
        { 163, "WindSupervisor" }, { 164, "WindManager" }, { 165, "WindLord" }
    };

        public static string GetJobName(byte job)
        {
            return jobNames.TryGetValue(job, out var jobName) ? jobName : null; 
        }
    }
}