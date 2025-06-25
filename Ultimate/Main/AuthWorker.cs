using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Ultimate;
using Ultimate.Main.Sockets;
using System.Collections.Concurrent;
using NHibernate.Util;

namespace Ultimate.Main
{
    public class AuthWorker
    {
        static byte[] Keys = { 18, 18, 17, 19, 20, 75, 27, 12, 10, 71, 18, 18, 04, 17, 21, 78 };// key length = 16
        public static void Decrypt(byte[] data, int len)
        {
            for (int i = 0; i < len; i++)
            {
                data[i] = (byte)(Keys[i % 16] ^ data[i]);
            }
        }
        public struct AuthInfo
        {
            public uint UID;
            public string Account;
            public string Status;
            public string Character;
            public byte LogonType;
            public ushort Width;
            public ushort Height;
            public ulong CryptoKey;
            public bool RightVersion;
            public DateTime Used;
            internal bool InvalidFiles;
            internal string SignatureKey;
            internal string MacAddress;
        }
        public class AuthClient
        {
            public Socket Soc;
            public LegacyCipher Crypto;
            public bool Player = true;

            public void Send(COPacket P)
            {
                try
                {
                    if (Soc.Connected)
                    {
                        byte[] data = P.Get;
                        byte[] Data = new byte[data.Length];//
                        //fixed (byte* src = data, dest = Data)//when i open it by .exe it works but idk where to put it when i start it from here where is the dll now?
                        //    Native.memcpy(dest, src, (uint)Data.Length);//libeay32.dll should be put somewhere.. / take me back to? packet ahndler area


                        Buffer.BlockCopy(data, 0, Data, 0, Data.Length);

                        Crypto.Encode(Data);
                        Soc.Send(Data);
                    }
                }
                catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
            }
            public void Disconnect()
            {
                try
                {
                    if (Soc.Connected)
                    {
                        Soc.Shutdown(SocketShutdown.Both);
                        Soc.Close();
                    }
                }
                catch (Exception e) { Game.World.ExcAdd += e.ToString() + "\r\n"; }
            }
        }
        public class LoaderEncryption
        {
            private static readonly byte[] Key1 = new byte[32]
            {
     173, 52, 189, 7, 220, 96, 138, 41, 110, 201, 14, 83, 169, 72, 243, 30, 5, 121, 254, 18, 77, 149, 206, 91, 132, 58, 100, 237, 26, 185, 64, 197
            };

            private static readonly byte[] Key2 = new byte[32]
            {
       39, 143, 88, 215, 126, 71, 233, 179, 60, 167, 94, 12, 231, 108, 22, 154, 190, 248, 3, 47, 82, 112, 204, 161, 55, 136, 209, 74, 102, 19, 157, 226
            };

            public static byte[] Decrypt(byte[] data, byte size)
            {
                var BufferOut = new byte[Math.Min((int)size, 32)];
                for (var x = 0; x < Math.Min((int)size, 32); x++)
                {
                    BufferOut[x] = (byte)(Key1[x * 44 % 32] ^ data[x]);
                    BufferOut[x] = (byte)(Key2[x * 99 % 32] ^ BufferOut[x]);
                }

                return BufferOut;
            }
        }
        public static string GameIP = "121.99.242.180";// new IniFile(@"C:\OldCODB\Config.ini").ReadString("Database", "GameServerIP");//80.240.25.201 94.136.50.223  172.22.144.132 "109.75.167.241"
        public static DateTime LastGameIP = DateTime.Now;
        public static void GetGameIP()
        {
            GameIP = "121.99.242.180";
            LastGameIP = DateTime.Now;
        }

        private static readonly MyRandom Rnd = new MyRandom();
        public static ConcurrentDictionary<ulong, AuthInfo> KeyedClients = new ConcurrentDictionary<ulong, AuthWorker.AuthInfo>();

        public static void DataHandler(Wrapper wr, byte[] buffer)
        {
            try
            {
                var ac = (AuthClient)wr.connector;

                if (ac == null) return;
                ////AuthWorker.Decrypt(buffer, buffer.Length);
                ac.Crypto.Decode(buffer);

                using (var ms = new MemoryStream(buffer))
                using (var br = new BinaryReader(ms))
                {
                    var packetLength = br.ReadUInt16();
                    var packetId = br.ReadUInt16();
                    // Console.WriteLine(packetId);
                    if (packetId == 10851)
                    {
                        //if (!NewAntiCheat.IsValidTail("S8E1WX56EF", buffer))
                        //{
                        //    wr._socket.Disconnect(false);
                        //    return;
                        //}

                        //var version = BitConverter.ToUInt16(buffer, 32);
                        //var width = BitConverter.ToUInt16(buffer, 34);
                        //var height = BitConverter.ToUInt16(buffer, 36);

                        string Account = Encoding.ASCII.GetString(br.ReadBytes(16));
                        Account = Account.Replace("\0", "");
                        br.ReadBytes(112);
                        string Password = Encoding.ASCII.GetString(br.ReadBytes(16));
                        br.ReadBytes(112);
                        string Server = Encoding.ASCII.GetString(br.ReadBytes(16));
                        Server = Server.Replace("\0", "");

                        var data = new byte[16];
                        Buffer.BlockCopy(buffer, 132, data, 0, 16);
                        const uint corc5PwKey = 0xB7E15163;
                        const uint corc5QwKey = 0x61C88647;
                        var corc5BufKey = new byte[]
                        {
                            0x3C, 0xDC, 0xFE, 0xE8, 0xC4, 0x54, 0xD6, 0x7E, 0x16, 0xA6, 0xF8, 0x1A, 0xE8, 0xD0,
                            0x38, 0xBE
                        };

                        var rc5 =
                            new CO2_CORE_DLL.Security.Cryptography.CORC5(corc5PwKey, corc5QwKey);
                        rc5.GenerateKey(corc5BufKey);
                        rc5.Decrypt(ref data);

                        Password = Encoding.ASCII.GetString(data);
                        Password = Password.Replace("\0", "");
                        //string Password2 = Encoding.Default.GetString(BR.ReadBytes(16));
                        //string test = new string(, 0, 16, Encoding.Default).TrimEnd('\0');

                        // var info = Database.Authenticate(account, password);
                        AuthInfo Info = Database.Authenticate(Account, Password);

                        //var FileHashes = Encoding.Default.GetString(buffer, 300, 65).Replace("\0", "");
                        //var MemoryHashes = Encoding.Default.GetString(buffer, 366, 65).Replace("\0", "");
                        //var NacAddress = Encoding.Default.GetString(buffer, 432, 20).Replace("\0", "");
                        //if (!AntiCheatPacket.Validated(FileHashes, MemoryHashes, out string error))
                        {
                            //Console.WriteLine($"Invalid file hashes.! Rejected login ==> {Account}");
                            //Info.InvalidFiles = true;
                        }

                        if (Info.LogonType != 255)
                        {
                            var iv = new byte[8];
                            for (var i = 0; i < 8; i++)
                                iv[i] = (byte)Rnd.Next(255);

                            //Info.Width = width;
                            //Info.Height = height;
                            ////Refix
                            //Info.Width = 1280;
                            //Info.Height = 1024;

                            //Info.MacAddress = NacAddress;
                            Info.SignatureKey = new Random().Next(100000, 999999).ToString("D6");
                            Info.CryptoKey = BitConverter.ToUInt64(iv, 0);
                            Info.Used = DateTime.Now;
                            KeyedClients.TryAdd(BitConverter.ToUInt64(iv, 0), Info);

                            ac.Send(Packets.SendAuthentication(GameIP, Info.CryptoKey)); //GameIP

                            //AC.Disconnect();
                        }
                        else
                        {
                            ac.Send(Packets.WrongAuth());
                            //AC.Disconnect();
                        }
                    }

                    br.Close();
                    ms.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    public /*unsafe*/ class PassCrypto
    {
        static UInt32 LeftRotate(UInt32 var, UInt32 offset)
        {
            offset &= 0x1f;
            var tmp1 = var >> (int)(32 - offset);
            var tmp2 = var << (int)offset;
            tmp2 |= tmp1;
            return tmp2;
        }
        /*
                static UInt32 RightRotate(UInt32 var, UInt32 offset)
                {
                    offset &= 0x1f;
                    var tmp1 = var << (int)(32 - offset);
                    var tmp2 = var >> (int)offset;
                    tmp2 |= tmp1;
                    return tmp2;
                }
        */

        private static readonly uint[] key = new uint[] {
                0xEBE854BC, 0xB04998F7, 0xFFFAA88C,
                0x96E854BB, 0xA9915556, 0x48E44110,
                0x9F32308F, 0x27F41D3E, 0xCF4F3523,
                0xEAC3C6B4, 0xE9EA5E03, 0xE5974BBA,
                0x334D7692, 0x2C6BCF2E, 0xDC53B74,
                0x995C92A6, 0x7E4F6D77, 0x1EB2B79F,
                0x1D348D89, 0xED641354, 0x15E04A9D,
                0x488DA159, 0x647817D3, 0x8CA0BC20,
                0x9264F7FE, 0x91E78C6C, 0x5C9A07FB,
                0xABD4DCCE, 0x6416F98D, 0x6642AB5B
        };
        public static string EncryptPassword(string password)
        {
            byte[] plain = new byte[16];
            Encoding.ASCII.GetBytes(password, 0, password.Length, plain, 0);

            MemoryStream mStream = new MemoryStream(plain);
            BinaryReader bReader = new BinaryReader(mStream);
            UInt32[] pSeeds = new UInt32[4];
            for (int i = 0; i < 4; i++) pSeeds[i] = bReader.ReadUInt32();
            bReader.Close();

            uint chiperOffset = 7;

            byte[] encrypted = new byte[plain.Length];
            MemoryStream eStream = new MemoryStream(encrypted);
            BinaryWriter bWriter = new BinaryWriter(eStream);

            for (int j = 0; j < 2; j++)
            {
                UInt32 tmp2;
                UInt32 tmp3;
                UInt32 tmp4;
                var tmp1 = tmp2 = tmp3 = tmp4 = 0;
                tmp1 = key[5];
                tmp2 = pSeeds[j * 2];
                tmp3 = key[4];
                tmp4 = pSeeds[j * 2 + 1];

                tmp2 += tmp3;
                tmp1 += tmp4;

                UInt32 B;
                var A = B = 0;

                for (int i = 0; i < 12; i++)
                {
                    UInt32 chiperContent = 0;
                    A = LeftRotate(tmp1 ^ tmp2, tmp1);
                    chiperContent = key[chiperOffset + i * 2 - 1];
                    tmp2 = A + chiperContent;

                    B = LeftRotate(tmp1 ^ tmp2, tmp2);
                    chiperContent = key[chiperOffset + i * 2];
                    tmp1 = B + chiperContent;
                }

                bWriter.Write(tmp2);
                bWriter.Write(tmp1);
            }
            bWriter.Close();

            return ASCIIEncoding.ASCII.GetString(encrypted);

        }

    }
}
