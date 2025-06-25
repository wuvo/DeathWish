using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NewestCOServer.Game;
namespace NewestCOServer.Main.Sockets
{
    public class InterserverClient
    {
        public Socket Socket { get; private set; }
        public byte[] Buffer { get; private set; }
        public int LastReceiveBufferSize { get; private set; }
        private int _sizeCurrent;
        private byte[] _data;
        public bool Connected { get { return Socket != null && Socket.Connected; } }

        public InterserverClient(string ip, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            Buffer = new byte[1024];
            try
            {
                Socket.Connect(ip, port);
                BeginReceive();
            }
            catch { }
        }

        public void InternalReceive(int amount, Action callback, int offset = 0)
        {
            if (!Connected)
                return;
            try
            {
                Socket.BeginReceive(Buffer, offset, amount, SocketFlags.None,
                    (cb) =>
                    {
                        try
                        {
                            int rcvd = Socket.EndReceive(cb);
                            LastReceiveBufferSize = rcvd;

                            if (rcvd <= 0)
                                Disconnect();
                            else
                            {
                                if (amount - rcvd == 0 || amount == Buffer.Length)
                                    callback();
                                else
                                    InternalReceive(amount - rcvd, callback, offset + rcvd);
                            }
                        }
                        catch (Exception ex)
                        {
                            World.ExcAdd += ex + "\r\n";
                            //BeginReceive();
                            Disconnect();
                        }

                    }, null);
            }
            catch
            {
                Disconnect();
            }
        }

        public void BeginReceive()
        {
            InternalReceive(2, InternalReceiveHeader);
        }

        private void InternalReceiveHeader()
        {
            byte[] block = new byte[2];
            Array.Copy(Buffer, block, 2);
            _sizeCurrent = BitConverter.ToUInt16(block, 0);

            if (_sizeCurrent < 0 || _sizeCurrent > Buffer.Length - 2)
                Disconnect();
            else
                InternalReceive(_sizeCurrent - 2, InternalReceiveBody);
        }

        private void InternalReceiveBody()
        {
            byte[] body = new byte[_sizeCurrent - 2];
            Array.Copy(Buffer, body, body.Length);

            byte[] msg = new byte[_sizeCurrent];
            msg[0] = (byte)(_sizeCurrent & 0xFF);
            msg[1] = (byte)((_sizeCurrent >> 8) & 0xFF);
            Array.Copy(body, 0, msg, 2, body.Length);
            _data = msg;
            InvokeOnReceive(_data);
        }

        public void InternalSend(byte[] block)
        {
            if (!Connected)
                return;
            try
            {
                Socket.BeginSend(block, 0, block.Length, SocketFlags.None,
                            (cb) =>
                            {
                                try
                                {
                                    int sent = Socket.EndSend(cb);
                                }
                                catch
                                {
                                    Disconnect();
                                }

                            }, null);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Send(byte[] data)
        {
            InternalSend(data);
        }

        public void Disconnect()
        {
            try
            {
                Socket.Disconnect(false);
            }
            catch (SocketException)
            {
            }
            InvokeOnDisconnect();
        }

        public ProxyClientReceive OnReceive;
        public ProxyClientConnection OnDisconnect;
        public override string ToString()
        {
            return Socket.RemoteEndPoint.ToString().Split(':')[0];
        }
        public void InvokeOnReceive(byte[] packet)
        {
            if (OnReceive != null)
                OnReceive(packet);
            BeginReceive();
        }

        public void InvokeOnDisconnect()
        {
            if (!Connected) return;
            if (OnDisconnect != null) OnDisconnect();
        }
    }
}