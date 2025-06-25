using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetFwTypeLib;

namespace Ultimate.Main.Sockets
{
    public class Wrapper
    {
        public byte[] buffer;//1024 bytes max  ... TQSERV end
        public Socket _socket;
        public object connector;
        public bool allow;
    }
    public class MasterSocket
    {
       // private Dictionary<string, int> Connections;

        public event Action<Wrapper> AnnounceNewConnection;
        public event Action<Wrapper> AnnounceDisconnection;
        public event Action<byte[], Wrapper, byte[]> AnnounceReceive;
        private readonly Socket _socket;
        //const string guidFWPolicy2 = "{E2B3C97F-6AE1-41AC-817A-F6F92166D7DD}";
        //const string guidRWRule = "{2C5BC43E-3369-4C33-AB0C-BE9469677AF4}";

        public MasterSocket(ushort port)
        {
            try
            {
                //Connections = new Dictionary<string, int>();
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(IPAddress.Any, port));
                _socket.Listen(500);
                _socket.BeginAccept(AcceptConnections, new Wrapper());
            }
            catch (Exception e)
            {
                Game.World.ExcAdd += e.ToString() + "\r\n";
            }
        }
        //~MasterSocket()
        //{
        //    _socket.Dispose();
        //}
        private void AcceptConnections(IAsyncResult result)//prima?
        {
            //Console.WriteLine("Accessed AcceptConnections MasterSocket.cs");
            try
            {
               
                Socket client = _socket.EndAccept(result);            
                if (Game.World.Firewall)
                {
                    string IP = client.RemoteEndPoint.ToString().Split(':')[0].ToString();
                    if (Game.World.SpammIps.ContainsKey(IP))
                    {
                        //DateTime Now = DateTime.Now;
                        Game.IPLog S = (Game.IPLog)Game.World.SpammIps[IP];
                        //if (S.Logs > 3 && (Now - S.LogDate).TotalMilliseconds / S.Logs < 1000)
                        if (S.Logs > 3)
                        {
                            if (IP != AuthWorker.GameIP /*&& IP != "80.240.25.201"*/)
                            {
                                /* if (Game.World.Firewall)
                                 {
                                     Type typeFWPolicy2 = Type.GetTypeFromCLSID(new Guid(guidFWPolicy2));
                                     Type typeFWRule = Type.GetTypeFromCLSID(new Guid(guidRWRule));
                                     INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(typeFWPolicy2);
                                     INetFwRule newRule = (INetFwRule)Activator.CreateInstance(typeFWRule);
                                     newRule.Name = "Block_IP";
                                     newRule.Description = "Block inbound traffic from " + IP + " over all TCP/UDP ports";
                                     newRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY;
                                     //newRule.LocalPorts = "400";
                                     newRule.RemoteAddresses = IP;
                                     newRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                                     newRule.Enabled = true;
                                     newRule.Grouping = "@firewallapi.dll,-23255";
                                     newRule.Profiles = fwPolicy2.CurrentProfileTypes;
                                     newRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                                     fwPolicy2.Rules.Add(newRule);
                                     Console.WriteLine(IP + " BANNED!");
                                     Game.World.SpammIps.Remove(IP);
                                 }*/
                               /* if (!Game.World.ToBanIPList.Contains(IP))
                                {
                                    Game.World.ToBanIPList.Add(IP, 1);
                                }
                                else
                                {
                                    uint key = (uint)Game.World.ToBanIPList[IP];
                                    key++;
                                    Game.World.ToBanIPList.Remove(IP);
                                    Game.World.ToBanIPList.Add(IP, key);
                                }*/
                                if (!Game.World.ToBanIPList.ContainsKey(IP))
                                {
                                    Game.World.ToBanIPList.Add(IP, (uint)1);
                                }
                                else if (Game.World.ToBanIPList.ContainsKey(IP))
                                {
                                    uint key = 0;
                                    key = (uint)Game.World.ToBanIPList[IP];
                                    key++;
                                    Game.World.ToBanIPList.Remove(IP);
                                    Game.World.ToBanIPList.Add(IP, key);
                                }
                                S.Logs = (ushort)(S.Logs - 1);
                                if (S.Logs > 50000)
                                    S.Logs = 0;
                                Game.World.SpammIps.Remove(IP);
                                Game.World.SpammIps.Add(IP, S);
                                client.Disconnect(false);//false
                                _socket.BeginAccept(AcceptConnections, new Wrapper());


                                return;

                            }

                        }
                        else
                        {
                            // int Tries = (int)Game.World.SpammIps[IP];
                            //Tries = Tries + 1;
                           // Game.IPLog S = (Game.IPLog)Game.World.SpammIps[IP];
                            S.Logs += 1;
                            Game.World.SpammIps[IP] = S;
                        }
                    }
                    else //if (Game.World.Firewall)
                    {


                        Game.IPLog S = new Game.IPLog { Logs = 1, LogDate = DateTime.Now };
                        Game.World.SpammIps.Add(IP, S);

                    }
                }
                
                Wrapper wr = result.AsyncState as Wrapper;

                if (wr != null)
                {
                    wr._socket = client;

                    #region Invisible

/* string IP = wr._socket.RemoteEndPoint.ToString().Split(':')[0].ToString();
                                         if (!Connections.ContainsKey(IP))
                                             try
                                             {
                                                 Connections.Add(IP, 1);
                                             }
                                             catch (Exception E)
                                             {
                                                 Game.World.DebugAdd +=E.ToString());
                                                 Game.World.DebugAdd +="error ip: " + IP);
                                                 wr._socket.Disconnect(false);
                                                 _socket.BeginAccept(AcceptConnections, new Wrapper());
                                                 return;
                                             }
                                         else
                                             if (Connections[IP] < 12)// <= 12
                                             {   
                                                 int connections = Connections[IP];
                                                 Connections.Remove(IP);
                                                 Connections.Add(IP, (byte)(connections + 1));
                                             }
                                             else
                                             {
                                                 wr._socket.Disconnect(false);
                                                 _socket.BeginAccept(AcceptConnections, new Wrapper());
                                                 return;
                                             }*/

                    #endregion

                    wr.buffer = new byte[65535];
                    wr._socket.BeginReceive(wr.buffer, 0, 65535, SocketFlags.None, ReceiveData, wr);
                    wr.allow = true;
                    AnnounceNewConnection.Invoke(wr);
                    _socket.BeginAccept(AcceptConnections, new Wrapper());
                }
                else
                    _socket.BeginAccept(AcceptConnections, new Wrapper());
            }
            catch (Exception e)
            {
                _socket.BeginAccept(AcceptConnections, new Wrapper());
                Game.World.ExcAdd += e.ToString() + "\r\n";
            }
        }
        private void ReceiveData(IAsyncResult result)
        {
           // Console.WriteLine("Accessed ReceiveData MasterSocket.cs");
            try
            {
                Wrapper wr = result.AsyncState as Wrapper;

                
                if (Game.World.Firewall)
                {
                    string IP = wr._socket.RemoteEndPoint.ToString().Split(':')[0].ToString();
                    if (Game.World.SpammIps.ContainsKey(IP))
                    {
                        DateTime Now = DateTime.Now;
                        Game.IPLog S = (Game.IPLog)Game.World.SpammIps[IP];
                        if (S.Logs > 3 && (Now - S.LogDate).TotalSeconds > 4)
                        {
                           /* if (!Game.World.ToBanIPList.Contains(IP))
                            {
                                Game.World.ToBanIPList.Add(IP, 1);
                            }
                            else
                            {
                                uint key = (uint)Game.World.ToBanIPList[IP];
                                key++;0
                                Game.World.ToBanIPList.Remove(IP);
                                Game.World.ToBanIPList.Add(IP, key);
                            }*/
                            if (!Game.World.ToBanIPList.ContainsKey(IP))
                            {
                                Game.World.ToBanIPList.Add(IP, (uint)1);
                            }
                            else if (Game.World.ToBanIPList.ContainsKey(IP))
                            {
                                uint key = 0;
                                key = (uint)Game.World.ToBanIPList[IP];
                                key++;
                                Game.World.ToBanIPList.Remove(IP);
                                Game.World.ToBanIPList.Add(IP, key);
                            }
                            S.Logs = (ushort)(S.Logs - 1);
                            if (S.Logs > 50000)
                                S.Logs = 0;
                            Game.World.SpammIps.Remove(IP);
                            Game.World.SpammIps.Add(IP, S);


                            wr._socket.Disconnect(false);
                            return;
                        }
                    }
                }
                
                SocketError error = SocketError.Disconnecting;
                int size = wr._socket.EndReceive(result, out error);
                //Console.WriteLine("Error : " + error.ToString() + " size : " + size); 
                if (error == SocketError.Success && size != 0)
                {
                    byte[] buffer = new byte[size];
                    Buffer.BlockCopy(wr.buffer, 0, buffer, 0, size);
                    byte[] question = new byte[] { 1 };
                    if (wr.connector == null || !wr._socket.Connected)
                    {
                        if (wr._socket.Connected)
                        {
                            wr._socket.Disconnect(false);
                        }
                        return;
                    }
                    /* if (wr.connector != null)
                     {
                         if (wr._socket.Ttl == 1)
                         {
                             if (wr._socket.Connected)
                                 wr._socket.Disconnect(false);
                             return;
                         }
                     }
                     else
                     {
                         if (wr._socket.Connected)
                             wr._socket.Disconnect(false);
                       
                     }*/
                    AnnounceReceive.Invoke(buffer, wr, question);
                    wr._socket.BeginReceive(wr.buffer, 0, 65535, SocketFlags.None, ReceiveData, wr);
                }
                else
                {


                    if (wr._socket.Connected)
                    {
                        wr._socket.Disconnect(false);//true
                    }
                    /*   int connections = Connections[IP];
                       Connections.Remove(IP);
                       if (connections - 1 > 0)
                           Connections.Add(IP, (byte)(connections - 1));*/
                    try
                    {
                        if (AnnounceDisconnection != null)
                            AnnounceDisconnection.Invoke(wr);
                    }
                    catch { }



                }
                // }
            }
            catch { } /*(Exception e)
            {
                Game.World.ExcAdd += e.ToString() + "\r\n";
            }*/
        }
    }
}