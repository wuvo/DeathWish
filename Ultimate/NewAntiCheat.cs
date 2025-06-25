using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Main;

namespace Ultimate
{
    public enum DETECTION_REASONS : int
    {
        NONE = 0,
        SUSPENDED_THREADS = 0xF1,
        DEBUGGER,
        LOAD_LIBRARY_HOOK_1,
        LOAD_LIBRARY_HOOK_2,
        CLICKER_1,
        CLICKER_2,
        MEMORY_TAMPERING,
        SPEED_HACK,
        SEND_MSG_NOT_FOUND,
        RECV_MSG_NOT_FOUND,
        ATTACK_PLAYER_NOT_FOUND,
        CHEAT_ENGINE,
        PROCESS_HASH,
        DLL_HASH,
        AIMBOT,
        ATTACK_EXTERNAL_THREAD,
        INVALID_THREADS
    };
    public class NewAntiCheat
    {
        public static Random GetRandom = new Random();
        public static bool IsValidTail(string key, byte[] packet, bool isGameServer = false)
        {
            return true;
            var gameServerTail = isGameServer ? 8 : 0;
            var tail = Encoding.Default.GetString(packet, packet.Length - 65 - gameServerTail, 65).Replace("\0", "");
            var timestamp = BitConverter.ToUInt32(packet, packet.Length - 69 - gameServerTail);
            var firstHash = ComputeSHA256Hash(Encoding.Default.GetString(packet, 2, packet.Length - 71 - gameServerTail));
            var hash = ComputeSHA256Hash(key + firstHash + timestamp.ToString());
            return hash == tail;
        }
        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.Default.GetBytes(text))).Replace("-", "").ToLower();
            }
        }
        internal static string GenerateGlobalHash(string fileHashesConcated)
        {
            return ComputeSHA256Hash(fileHashesConcated);
        }
    }
    public class AntiCheatPacket
    {
        const string GlobalMemoryHash = "4037e250682bcccfe01351c4af47fb9ffd9a95e484f5c291f80558e6efff778c";
        static string ConquerHash, MagicTypeHash, C3Hash, DllFileHash, ItemTypeHash, GlobalFilesHash;
        static Random GetRandom = new Random();
        private static List<string> KnownDllsHashes = new List<string>();
        private static List<string> KnownProcessHashes = new List<string>();
        private static void LoadCheats()
        {
            try
            {
                using (var stream = new StreamReader("DllCheats.txt"))
                {
                    var Found = stream.ReadToEnd();
                    KnownDllsHashes = Found.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Console.WriteLine($"Loaded {KnownDllsHashes.Count} dll hashes.");
                }
                using (var stream = new StreamReader("ProcessCheats.txt"))
                {
                    var Found = stream.ReadToEnd();
                    KnownProcessHashes = Found.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Console.WriteLine($"Loaded {KnownProcessHashes.Count} process hashes.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static void SendCheck(Character client)
        {
            client.LastDLLCheck = DateTime.Now;
            client.MyClient.AddSend(Packets.StringPacket(client.EntityID, (StringType)102, ""));
            client.MyClient.AddSend(Packets.StringPacket(client.EntityID, (StringType)104, ""));
        }
        public static void SendKnownCheats(Character Client)
        {
            Client.MyClient.AddSend(Packets.StringPacket(Client.EntityID, (StringType)105, ""));// Clear the stored cheats there
            foreach (var c in KnownDllsHashes)
                Client.MyClient.AddSend(Packets.StringPacket(Client.EntityID, (StringType)101, c.ToLower()));
            foreach (var c in KnownProcessHashes)
                Client.MyClient.AddSend(Packets.StringPacket(Client.EntityID, (StringType)103, c.ToLower()));
            SendCheck(Client);
        }
        public static void GetCheatPacket(byte[] data, out int subtype, out string FileHashes, out DETECTION_REASONS reason, out string MemoryHashes,
            out string FileHash, out string FileName)
        {
            FileHash = FileName = FileHashes = MemoryHashes = "";
            reason = DETECTION_REASONS.NONE;
            MemoryStream memstream = new MemoryStream(data);
            BinaryReader stream = new BinaryReader(memstream);
            stream.ReadInt32();
            subtype = stream.ReadInt32();
            if (subtype == 1)
            {
                FileHashes = Encoding.ASCII.GetString(stream.ReadBytes(65)).Replace("\0", "");
                MemoryHashes = Encoding.ASCII.GetString(stream.ReadBytes(65)).Replace("\0", "");
            }
            else if (subtype == 2) // Hack detected.
            {
                reason = (DETECTION_REASONS)BitConverter.ToInt16(data, 8);
                FileHash = Encoding.Default.GetString(data, 10, 400).Replace("\0", "");
                FileName = Encoding.Default.GetString(data, 410, 400).Replace("\0", "");
            }
        }
        public static void GetCheatPacket2(byte[] data, out int subtype, out string Str1,
          out string Str2, out string Str3, out string Str4)
        {
            Str1 = Str2 = Str3 = Str4 = "";
            MemoryStream memstream = new MemoryStream(data);
            BinaryReader stream = new BinaryReader(memstream);
            stream.ReadInt32();
            subtype = stream.ReadInt32();
            if (subtype == 4)
            {
                Str1 = Encoding.ASCII.GetString(stream.ReadBytes(100)).Replace("\0", "");
                Str2 = Encoding.ASCII.GetString(stream.ReadBytes(100)).Replace("\0", "");
                Str3 = Encoding.ASCII.GetString(stream.ReadBytes(200)).Replace("\0", "");
                Str4 = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
            }
            else if (subtype == 5 || subtype == 6) // Hack detected.
            {
                Str1 = Encoding.ASCII.GetString(stream.ReadBytes(200)).Replace("\0", "");
                Str2 = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
            }
        }
        public static List<string> ExcludedPlayers = new List<string>() { "Burn", "GwapZ123", "TaniquE" };

        public async static void CheatPacketHandler(byte[] data, GameClient client)
        {
            try
            {
                int subtype;
                string FileHashes;
                string MemoryHashes;
                string FileName;
                string FileHash;
                DETECTION_REASONS CheatType;
                GetCheatPacket(data, out subtype, out FileHashes, out CheatType, out MemoryHashes, out FileHash, out FileName);
                if (ExcludedPlayers.Contains(client.MyChar.Name))
                    return;
                switch (subtype)
                {
                    case 1:// files packet
                        {
                            if (client.AuthInfo.Status != "[PM]")
                            {
                                if (!AntiCheatPacket.Validated(FileHashes, MemoryHashes, out string errorCode))
                                    if (client.MyChar.Name != "Burn" && client.MyChar.Name != "GwapZ123")
                                    {
                                        AntiCheatPacket.Report(client.MyChar.Name, errorCode, client.MyChar.EntityID);
                                        Task delay = Task.Delay(5000);
                                        //client.LocalMessage(2005, "Modified files in client or not updated to latest patch! Please patch! You'll be disconnected.");
                                        await delay;
                                        client.Disconnect();
                                        Game.World.SendMsgToAll("[AntiCHEAT]", client.MyChar.Name + " was found cheating and was disconnected.", 2005, 0);
                                        return;
                                    }
                            }
                            client.LastCheatPacket = DateTime.Now;
                            break;
                        }
                    case 2:
                        {
                            if (CheatType == DETECTION_REASONS.PROCESS_HASH || CheatType == DETECTION_REASONS.DLL_HASH)// 100 pct cheating.
                            {
                                AntiCheatPacket.Report(client.MyChar.Name, CheatType.ToString() + $"-> Hash:{FileHash}, FileName:{FileName}", client.MyChar.EntityID);

                                if (client.MyChar.Loc.Map != 6003)
                                {
                                    client.MyChar.BOTJailedDays += 3;
                                    client.MyChar.Teleport(6003, 30, 72);
                                    client.MyChar.MyClient.LocalMessage(2011, "You are now botjailed for " + client.MyChar.BOTJailedDays + " days!");
                                }
                            }
                            else
                                AntiCheatPacket.Report(client.MyChar.Name, CheatType.ToString(), client.MyChar.EntityID);

                            Console.WriteLine($"Detected {CheatType.ToString()} on player [{client.MyChar.Name}]");
                            Task delay = Task.Delay(5000);
                            client.LocalMessage(2005, "Cheat has been detected in your client and was reported! You'll be disconnected.");
                            await delay;
                            Game.World.SendMsgToAll("[AntiCHEAT]", client.MyChar.Name + " was found cheating and was disconnected.", 2005, 0);
                            client.Disconnect();
                            break;
                        }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void CheatPacketHandler2(byte[] data, GameClient client)
        {
            try
            {
                int subtype;
                string Str1;
                string Str2;
                string Str3;
                string Str4;
                GetCheatPacket2(data, out subtype, out Str1, out Str2, out Str3, out Str4);
                switch (subtype)
                {
                    case 4:// files packet
                        {
                            using (var streamWriter = new StreamWriter($"CheatLogs\\Processes\\{client.MyChar.Name}.txt", true))
                            {
                                streamWriter.WriteLine($"ProcessName: {Str1} -- > Title: {Str2} --> ExePath: {Str3} --> Hash: {Str4}");
                                streamWriter.Close();
                            }
                            break;
                        }
                    case 5:
                        {
                            using (var streamWriter = new StreamWriter($"CheatLogs\\Modules1\\{client.MyChar.Name}.txt", true))
                            {
                                streamWriter.WriteLine($"Module: {Str1} --> Hash: {Str2}");
                                streamWriter.Close();
                            }
                            break;
                        }
                    case 6:
                        {
                            using (var streamWriter = new StreamWriter($"CheatLogs\\Modules2\\{client.MyChar.Name}.txt", true))
                            {
                                streamWriter.WriteLine($"Module: {Str1} --> Hash: {Str2}");
                                streamWriter.Close();
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void Report(string Name, string reason, uint PlayerUid)
        {
            string logs = $"[CHEAT] {Name} -- REASON: {reason}";
            if (!reason.Contains("Time-packet"))
            {
                var cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                cmd.Insert("cheats_reports")
                  .Insert("Date", DateTime.Now)
                  .Insert("Name", Name)
                  .Insert("Report", reason)
                  .Insert("PlayerUid", PlayerUid);
                cmd.Execute();
            }
            Console.WriteLine(logs);
        }
        public static void LoadFiles()
        {
            ConquerHash = CalculateMD5(@"Files\Conquer.exe");
            ItemTypeHash = CalculateMD5(@"Files\itemtype.dat");
            MagicTypeHash = CalculateMD5(@"Files\magictype.dat");
            C3Hash = CalculateMD5(@"Files\c3.wdb");
            DllFileHash = CalculateMD5(@"Files\AnticheatLibrary.dll");
            GlobalFilesHash = NewAntiCheat.GenerateGlobalHash(ItemTypeHash + MagicTypeHash + C3Hash + ConquerHash + DllFileHash + ItemTypeHash + MagicTypeHash + C3Hash + ConquerHash + DllFileHash);
            LoadCheats();
            Console.WriteLine("Loaded all protection files.", ConsoleColor.Magenta);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[GetRandom.Next(s.Length)]).ToArray());
        }
        public static bool Validated(string FileHash, string MemoryHashes, out string errorCode)
        {
            errorCode = "";
            if (FileHash != GlobalFilesHash)
            {
                errorCode = $"FileHash not valid, Received=${FileHash}";
                return false;
            }
            if (MemoryHashes != GlobalMemoryHash)
            {
                errorCode = $"MemoryHash not valid, Received=${MemoryHashes}";
                return false;
            }
            return true;
        }
    }
}
