using DeathWish.Game.MsgServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeathWish.WebServer
{
    public static class LoaderServerProcess
    {
        public class Client
        {
            public ServerSockets.SecuritySocket SecuritySocket;
            public Client(ServerSockets.SecuritySocket _socket)
            {
                if (_socket.RemoteIp == Program.ServerConfig.LoaderIP)
                {
                    Object = this;
                    _socket.Client = this;
                    SecuritySocket = _socket;
                }
                else
                    _socket.Disconnect();
            }

            public unsafe void Send(ServerSockets.Packet msg)
            {
                int size = msg.Size;
                SecuritySocket.Send(msg);
            }
            public void Disconnect()
            {
                SecuritySocket.Disconnect();
            }
        }
        public static uint SecurityPadding = 626151132;
        public static Client Object;
        public static ServerSockets.ServerSocket LoaderServer;
        public static void Init()
        {
            if (Program.ServerConfig.IsInterServer == false)
            {
                LoaderServer = new ServerSockets.ServerSocket(
                     new Action<ServerSockets.SecuritySocket>(p => new Client(p))
                     , new Action<ServerSockets.SecuritySocket, ServerSockets.Packet>((p, data) =>
                     {
                         ProcesReceive(p, data);
                     })
                     , new Action<ServerSockets.SecuritySocket>(p => (p.Client as Client).Disconnect()));
                LoaderServer.Initilize(Program.ServerConfig.Port_SendSize, Program.ServerConfig.Port_ReceiveSize, 1, 3);
                LoaderServer.Open(Program.ServerConfig.LoaderIP, Program.ServerConfig.LoaderPort, Program.ServerConfig.Port_BackLog);
            }
        }
        public static unsafe void ProcesReceive(ServerSockets.SecuritySocket obj, ServerSockets.Packet stream)
        {
            if (obj.RemoteIp != Program.ServerConfig.LoaderIP)
                return;
            var Game = (obj.Client as Client);
            ushort PacketID = stream.ReadUInt16();
            uint Padding = stream.ReadUInt32();
            if (Padding != SecurityPadding)
                return;
            switch (PacketID)
            {
                case 1319:
                    {
                        string Key = stream.ReadCString(16);
                        WebServer.LoaderServer.LogginKey = stream.ReadCString(16);
                        break;
                    }
                case 1320:
                    {
                        break;
                    }
                case 1321:
                    {
                        string IP = stream.ReadCString(16);
                        string Programname = stream.ReadCString(32);
                        foreach (var user in Database.Server.GamePoll.Values)
                        {
                            if (user.Socket.RemoteIp == IP)
                            {
                                uint BanHours = 0;
                                if (user.BanCount == 0)
                                    BanHours = 24 * 1;
                                else if (user.BanCount == 1)
                                    BanHours = 24 * 7;
                                else if (user.BanCount == 2)
                                    BanHours = 24 * 14;
                                else
                                    BanHours = 24 * 364;
                                user.BanCount += 1;
                                Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, BanHours, Programname);
                                string Messaje = "" + user.Player.Name + " got banned for " + BanHours / 23 + " hours, because was found using programs that are illegal in game (" + Programname + ").";
                                Game.MsgServer.MsgMessage msg = new MsgMessage(Messaje, MsgMessage.MsgColor.red, MsgMessage.ChatMode.System);
                                Program.SendGlobalPackets.Enqueue(msg.GetArray(stream));
                                user.Socket.Disconnect();
                                Console.WriteLine(user.Player.Name + " <---- " + Programname);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            ServerSockets.PacketRecycle.Reuse(stream);
        }
        public unsafe static void work(Extensions.Time32 clock)
        {
        }
    }
}
