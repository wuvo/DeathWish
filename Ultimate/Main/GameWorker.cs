using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NewestCOServer.Main
{
    class GameWorker
    {
        public static Connection Listener;
        static Random Rnd = new Random();

        public static void StartServer()
        {
            try
            {
                Listener = new Connection();
                Listener.SetConnHandler(new ConnectionArrived(ConnectionHandler));
                Listener.SetDataHandler(new DataArrived(DataHandler));
                Listener.SetDCHandler(new Disconnection(DCHandler));
                Listener.Listen(5816);
            }
            catch (Exception Exc) { Program.WriteLine(Exc); }
        }
        static void ConnectionHandler(StateObj S)
        {
            try
            {
                GameClient C = new GameClient();
                C.Soc = S.Sock;
                S.Wrapper = C;
                C.AddSend(Packets.DHKeyPacket(C.KeyExchance.PublicKey.ToHexString(), C.NewServerIV, C.NewClientIV));
                C.EndSend();
            }
            catch (Exception Exc) { Program.WriteLine(Exc); }
        }
        static void DataHandler(StateObj StO, byte[] Data)
        {
            try
            {
                GameClient GC = (GameClient)StO.Wrapper;
                GC.Crypto.Decrypt(Data);

                if (GC != null)
                {
                    if (GC.SetBF)
                    {
                        GC.SetBF = false;
                        MemoryStream MS = new MemoryStream(Data);
                        BinaryReader BR = new BinaryReader(MS);

                        BR.ReadBytes(7);
                        uint PacketLen = BR.ReadUInt32();
                        int JunkLen = BR.ReadInt32();
                        BR.ReadBytes(JunkLen);
                        int Len = BR.ReadInt32();
                        string PubKey = ASCIIEncoding.ASCII.GetString(BR.ReadBytes(Len));

                        GC.Crypto = new GameCrypto(GC.KeyExchance.ComputeKey(OpenSSL.BigNumber.FromHexString(PubKey)));
                        GC.Crypto.Blowfish.DecryptIV = GC.NewClientIV;
                        GC.Crypto.Blowfish.EncryptIV = GC.NewServerIV;

                        BR.Dispose();
                        BR.Close();
                        MS.Dispose();
                        MS.Close();
                    }
                    else
                    {
                        ushort PacketLength = BitConverter.ToUInt16(Data, 0);
                        ushort PacketID = BitConverter.ToUInt16(Data, 2);

                        if (PacketID == 1052)
                        {
                            try
                            {
                                ulong CryptoKey = BitConverter.ToUInt64(Data, 4);

                                AuthWorker.AuthInfo Info = (AuthWorker.AuthInfo)AuthWorker.KeyedClients[CryptoKey];
                                GC.AuthInfo = Info;
                                GC.MessageID = (uint)Rnd.Next(50000);
                                GC.Soc = StO.Sock;

                                if (GC.AuthInfo.LogonType == 2)
                                    GC.AddSend(Packets.SystemMessage(GC.MessageID, "NEW_ROLE"));
                                else if (GC.AuthInfo.LogonType == 1)
                                {
                                    string Acc = "";

                                    try
                                    {
                                        foreach (Game.Character character in Game.World.H_Chars.Values)
                                        {
                                            if (character.Name == GC.AuthInfo.Character)
                                            {
                                                GC.Soc.Disconnect(false);
                                                return;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        GC.Soc.Disconnect(false);
                                        return;
                                    }
                                    GC.MyChar = Database.LoadCharacter(GC.AuthInfo.Character, ref Acc);
                                    try
                                    {
                                        GC.MyChar.MyClient = GC;
                                    }
                                    catch { GC.Soc.Disconnect(false); return; }

                                    GC.AddSend(Packets.SystemMessage(GC.MessageID, "ANSWER_OK"));
                                    GC.AddSend(Packets.CharacterInfo(GC.MyChar));
                                    GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.VIPLevel, GC.MyChar.VipLevel));
                                    GC.AddSend(Packets.Time());
                                    GC.AddSend(Packets.Donators(GC.MyChar));
                                    GC.AddSend(Packets.Packet1012(GC.MyChar.EntityID));
                                    GC.AddSend(Packets.Status(GC.MyChar.EntityID, Game.Status.Effect, 0));
                                    Game.World.H_Clients.Add(GC.MyChar.EntityID, GC);

                                    if (GC.MyChar.VIPDays > 0)
                                        Program.WriteLine(GC.MyChar.Name + " has logged on with IP: " + GC.Soc.RemoteEndPoint + " VIP: " + GC.MyChar.VipLevel + " Days Left: " + GC.MyChar.VIPDays);
                                    else
                                    {
                                        Program.WriteLine(GC.MyChar.Name + " has logged on with IP: " + GC.Soc.RemoteEndPoint);
                                    }
                                   
                                    if (GC.MyChar.MyGuild != null)
                                        GC.MyChar.MyGuild.GuildMsg("SYSTEM", GC.MyChar.MyGuild.Members.Values.ToString(), GC.MyChar.Name + " has logged in.", 0);

                                }
                                else { GC.Soc.Disconnect(false); return; };
                                GC.EndSend();
                            }
                            catch { GC.Soc.Disconnect(false); }
                        }
                        else PacketHandler.Handle(GC, Data);
                    }
                }
                else
                {
                    GC.Crypto.Decrypt(Data);
                    PacketHandler.Handle(GC, Data);
                }
            }
            catch (Exception Exc) { Program.WriteLine(Exc); }
        }
        static void DCHandler(StateObj StO)
        {
            try
            {
                GameClient GC = (GameClient)StO.Wrapper;
                if (GC != null && GC.MyChar != null)
                {
                    GC.LogOff();
                }
            }
            catch (Exception Exc) { Program.WriteLine(Exc); }
        }
    }
}
