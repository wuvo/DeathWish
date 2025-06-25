using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate
{
    public class MsgCheatPacket
    {
        public static unsafe void GetCheatPacket(byte[] data, out int subtype, out string Conquer,
           out string MagicType, out string MagicEffect, out string C3_WDB, out string DLL_Hash, out string reason, out string Hashes)
        {
            Conquer = MagicEffect = MagicType = C3_WDB = DLL_Hash = reason = Hashes = "";
            MemoryStream memstream = new MemoryStream(data);
            BinaryReader stream = new BinaryReader(memstream);
            stream.ReadInt32();
            subtype = stream.ReadInt32();
            if (subtype == 1)
            {
                Conquer = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
                MagicType = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
                C3_WDB = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
                DLL_Hash = Encoding.ASCII.GetString(stream.ReadBytes(33)).Replace("\0", "");
                Hashes = Encoding.ASCII.GetString(stream.ReadBytes(200)).Replace("\0", "");
            }
            else if (subtype == 2) // Hack detected.
            {
                reason = Encoding.ASCII.GetString(stream.ReadBytes(100));
            }
        }
        //"17063025030779170997 9438413111858442096 12915502383925585523 10320171395943267754 6797930606716609703 1826207612468735926 2073468101519580334 9813604703961070782"
        // "17063025030779170997 9438413111858442096 12915502383925585523 10320171395943267754 6797930606716609703 1826207612468735926 2073468101519580334 9813604703961070782"

        const string Hash1 = "3420047276086224166",
            Hash2 = "1302156966448411776",
            Hash3 = "9740925105848962489",
            Hash4 = "11106025133425261390",
            Hash5 = "5056719650055602888",
            Hash6 = "2400184231126635273",
            Hash7 = "3312528886157380234";
        public async static void CheatPacketHandler(byte[] data, GameClient client)
        {
            int subtype;
            string Conquer;
            string MagicType;
            string MagicEffect;
            string C3_WDB;
            string DLL_Hash;
            string CheatReason;
            string Hashes;
            GetCheatPacket(data, out subtype, out Conquer, out MagicType, out MagicEffect, out C3_WDB, out DLL_Hash, out CheatReason, out Hashes);
            switch (subtype)
            {
                case 1:// files packet
                    {
                        //if (client.AuthInfo.Status != "[PM]")
                        {
                            if (!MsgCheatPacket.Validated(Conquer, MagicType, MagicEffect, C3_WDB, DLL_Hash))
                            {
                                MsgCheatPacket.Report(client.MyChar.Name, "Files Changed");

                                Console.WriteLine($"Detected files changed on client [{client.MyChar.Name}].");
                                Task delay = Task.Delay(5000);
                                client.LocalMessage(2005, "Modified files in client or not updated to latest patch! Please patch! You'll be disconnected.");
                                await delay;
                                client.Disconnect();
                                Game.World.SendMsgToAll("[AntiCHEAT]", client.MyChar.Name + " was found cheating and was disconnected.", 2005, 0);


                                return;
                            }
                            if (Hashes == "")
                                break;
                            string[] HashesSplit = Hashes.Split(' ');
                            if (HashesSplit.Length == 7)
                            {
                                if (!((HashesSplit[0] == Hash1 || HashesSplit[0] == "7202432830908948127")
                                   && HashesSplit[1] == Hash2
                                   && HashesSplit[2] == Hash3
                                   && HashesSplit[3] == Hash4
                                   && HashesSplit[4] == Hash5
                                   && HashesSplit[5] == Hash6
                                   && HashesSplit[6] == Hash7))
                                {
                                    MsgCheatPacket.Report(client.MyChar.Name, "Memory Edits.");

                                    //   Console.WriteLine($"Detected changes on client [{client.Player.Name}].");
                                    Task delay = Task.Delay(5000);
                                    client.LocalMessage(2005, "Modified files /cheat in client or not updated to latest patch! Please patch! You'll be disconnected.");
                                    //await delay;
                                    client.Disconnect();
                                    Game.World.SendMsgToAll("[AntiCHEAT]", client.MyChar.Name + " was found cheating and was disconnected.", 2005, 0);

                                    return;
                                }
                            }
                            else
                            {
                                MsgCheatPacket.Report(client.MyChar.Name, "Memory Edits.");

                                //   Console.WriteLine($"Detected changes on client [{client.Player.Name}].");
                                Task delay = Task.Delay(5000);
                                client.LocalMessage(2005, "Modified files/cheat in client or not updated to latest patch! Please patch! You'll be disconnected.");
                                //await delay;
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
                        MsgCheatPacket.Report(client.MyChar.Name, CheatReason);

                        Console.WriteLine($"Detected {CheatReason} on player [{client.MyChar.Name}]");
                        Task delay = Task.Delay(5000);
                        client.LocalMessage(2005, "Cheat has been detected in your client and was reported! You'll be disconnected.");
                        await delay;
                        client.Disconnect();
                        Game.World.SendMsgToAll("[AntiCHEAT]", client.MyChar.Name + " was found cheating and was disconnected.", 2005, 0);

                        break;
                    }
                case 3:
                    {
                        Console.WriteLine($"Suspicious injection: {CheatReason} on player [{client.MyChar.Name}]");
                        MsgCheatPacket.Report(client.MyChar.Name, "Injection");

                        //client.Socket.Disconnect();

                        break;
                    }
            }

        }
        public static string ConquerHash, MagicHash, MagicHash2, C3Hash, MagicEffectHash, DLLHash;

        public static void Report(string Name, string reason)
        {
            string logs = $"[CHEAT] {Name} -- REASON: {reason}";
            Game.World.AntiCheatAdd += logs + "\r\n";
            // Database.ServerDatabase.LoginQueue.Enqueue(logs);
            Console.WriteLine(logs);
        }
        public static void LoadFiles()
        {
            ConquerHash = CalculateMD5(@"Files\Conquer.exe");
            MagicHash = CalculateMD5(@"Files\magictype.dat");
            C3Hash = CalculateMD5(@"Files\c3.wdb");
            DLLHash = CalculateMD5(@"Files\GameMonitor.dll");
            Console.WriteLine("Loaded all protection files.", ConsoleColor.Magenta);
        }
        static string CalculateMD5(string filename)
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
        internal static bool Validated(string conquer, string magicType, string magicEffect, string c3_WDB, string dLL_Hash)
        {
            if (conquer == ConquerHash && magicType == MagicHash && c3_WDB == C3Hash && DLLHash == dLL_Hash) // Disbale for now this for testing.
                return true;
            return false;
        }
    }
}
