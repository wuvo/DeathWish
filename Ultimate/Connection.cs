using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Ultimate
{
    public delegate void ConnectionArrived(StateObj StO);
    public delegate void DataArrived(StateObj StO, byte[] Buffer);
    public delegate void Disconnection(StateObj StO);


    public class StateObj
    {
        public byte[] Data;
        public Socket Sock;
        public object Wrapper;
    }

    public class Connection
    {
        Socket Listener;
        ConnectionArrived ConnHandler;
        DataArrived DataHandler;
        Disconnection DCHandler;

        public void SetConnHandler(ConnectionArrived Method)
        {
            ConnHandler = Method;
        }
        public void SetDataHandler(DataArrived Method)
        {
            DataHandler = Method;
        }
        public void SetDCHandler(Disconnection Method)
        {
            DCHandler = Method;
        }
        public void Close()
        {
            Listener.Close();
        }
        public void Listen(int Port)
        {
            try
            {
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IEP = new IPEndPoint(IPAddress.Any, Port);
                Listener.Bind(IEP);
                Listener.Listen(100);
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }

            Listener.BeginAccept(new AsyncCallback(WaitConnections), new StateObj());
        }
        void WaitConnections(IAsyncResult Res)
        {
            try
            {
                StateObj S = null;
                try
                {
                    S = (StateObj)Res.AsyncState;
                    S.Sock = Listener.EndAccept(Res);
                    S.Data = new byte[1024];                    
                }
                catch
                {
                    Listener.BeginAccept(new AsyncCallback(WaitConnections), new StateObj());
                    return;
                }
                if (ConnHandler != null)
                    ConnHandler.Invoke(S);

                Listener.BeginAccept(new AsyncCallback(WaitConnections), new StateObj());
                S.Sock.BeginReceive(S.Data, 0, 1024, SocketFlags.None, new AsyncCallback(WaitData), S);
            }
            catch { }
        }
        /*unsafe*/ void WaitData(IAsyncResult Res)
        {
            try
            {
                StateObj S = (StateObj)Res.AsyncState;
                try
                {
                    if (S.Sock.Connected)
                    {
                        SocketError SE;
                        uint DataLen = (uint)S.Sock.EndReceive(Res, out SE);

                        if (SE == SocketError.Success && DataLen != 0)
                        {
                            byte[] RData = new byte[DataLen];

                            //fixed (byte* p = RData, p2 = S.Data)
                            //    Native.memcpy(p, p2, DataLen);

                            Buffer.BlockCopy(S.Data, 0, RData, 0, RData.Length);//wait, lemme back up first

                            if (DataHandler != null)
                                DataHandler.Invoke(S, RData);

                            S.Sock.BeginReceive(S.Data, 0, 1024, SocketFlags.None, new AsyncCallback(WaitData), S);
                        }
                        else if (DCHandler != null)
                            DCHandler.Invoke(S);
                    }
                    else if (DCHandler != null)
                        DCHandler.Invoke(S);
                }
                catch
                {
                    if (DCHandler != null)
                        DCHandler.Invoke(S);
                }
            }
            catch (Exception Exc) { Game.World.ExcAdd +=Exc.ToString() + "\r\n"; }
        }
    }
}
