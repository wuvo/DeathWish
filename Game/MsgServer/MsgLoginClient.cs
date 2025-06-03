
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeathWish.Game.MsgServer
{
    public struct MsgLoginClient
    {
        public uint Key;
        public ulong AccountHash;
        public unsafe static void LoginHandler(Client.GameClient client, MsgLoginClient packet)
        {

            client.ClientFlag &= ~Client.ServerFlag.OnLoggion;
            if (client.Socket != null)
            {
                if (client.Socket.RemoteIp == "NONE")
                {
                    MyConsole.WriteLine("Breack login client.");
                    return;
                }
            }
            try
            {

                //NetShield
                if (client.Newrole)
                {
                    uint encryptedIdentifier = client.OnLogin.Key;
                    client.OnLogin.Key = Game.MsgLoader.CheatPacket.DecryptIdentifier(encryptedIdentifier, Game.MsgLoader.CheatPacket.keys);
                }
                if (client.OnLogin.Key > 100000000 || client.OnLogin.Key < 1000000)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        string message = "You can't do this";
                        client.Send(new MsgServer.MsgMessage(message, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    }
                    return;
                }

                //NetShield
                var pool = Database.Server.GamePoll.Values.ToArray();
                if (pool.Count(i => i.TqSerial == client.TqSerial) >= 5)
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        client.Send(new MsgServer.MsgMessage("You can not open more than 5 accounts.", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    }
                    return;
                }
                //NetShield
                string BanMessaje;
                if (Database.SystemBanned.IsBanned(client.Socket.RemoteIp, out BanMessaje))
                {
                    using (var rec = new ServerSockets.RecycledPacket())
                    {
                        var stream = rec.GetStream();
                        string Messaj = "You IP Address is Banned for: " + BanMessaje + " ";
                        client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    }
                    return;
                }

                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) == Client.ServerFlag.CreateCharacterSucces)
                {
                    if (Database.ServerDatabase.AllowCreate(client.ConnectionUID))
                    {
                        client.ClientFlag &= ~Client.ServerFlag.CreateCharacterSucces;
                        if (client.Player.MyChi == null)
                        {
                            client.Player.MyChi = new Role.Instance.Chi(client.Player.UID);
                        }
                        if (client.Player.SubClass == null)
                            client.Player.SubClass = new Role.Instance.SubClass();
                        if (client.Player.Flowers == null)
                        {
                            client.Player.Flowers = new Role.Instance.Flowers(client.Player.UID, client.Player.Name);
                            client.Player.Flowers.FreeFlowers = 1;
                        }
                        if (client.Player.Nobility == null)
                            client.Player.Nobility = new Role.Instance.Nobility(client);
                        if (client.Player.Associate == null)
                        {
                            client.Player.Associate = new Role.Instance.Associate.MyAsociats(client.Player.UID);
                            client.Player.Associate.MyClient = client;
                            client.Player.Associate.Online = true;
                        }
                        if (client.Player.InnerPower == null)
                            client.Player.InnerPower = new Role.Instance.InnerPower(client.Player.Name, client.Player.UID);
                        Database.Server.GamePoll.TryAdd(client.Player.UID, client);
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            client.Send(new MsgServer.MsgMessage("ANSWER_OK", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                            Database.ServerDatabase.CreateCharacte(client);
                            Database.ServerDatabase.SaveClient(client);
                            client.ClientFlag |= Client.ServerFlag.AcceptLogin;
                            //NetShield
                            MyConsole.WriteLine("-------------------------------------------------", ConsoleColor.Green);
                            MyConsole.WriteLine("Account: PlayerName > " + client.Player.Name + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("PlayerUID > " + client.Player.UID + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("Version > " + client.Version + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("ProSerial > " + client.TqSerial + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("Mac > " + client.Socket.GetMACAddress() + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("IP > " + client.Socket.RemoteIp + " < ", ConsoleColor.Yellow);
                            MyConsole.WriteLine("-------------------------------------------------", ConsoleColor.Green);
                            //NetShield
                            client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                            MsgLoginHandler.LoadMap(client, stream);
                        }
                        Program.CallBack.Register(client);
                        return;
                    }
                }

                if ((client.ClientFlag & Client.ServerFlag.AcceptLogin) != Client.ServerFlag.AcceptLogin)
                {
                    var login = client.OnLogin;

                    client.ConnectionUID = login.Key;
                    if (Database.SystemBannedAccount.IsBanned(client.ConnectionUID, out BanMessaje))
                    {
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();
                            string aMessaj = "Your account is Banned for: " + BanMessaje + " ";
                            client.Send(new MsgServer.MsgMessage(aMessaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                        return;
                    }
                    //try
                    //{
                    //    var pool = Database.Server.GamePoll.Values.ToArray();
                    //    if (pool.Count(i => i.OnLogin.HWID == client.OnLogin.HWID) >= 6)
                    //    {
                    //        using (var rec = new ServerSockets.RecycledPacket())
                    //        {
                    //            var stream = rec.GetStream();
                    //            client.Send(new MsgServer.MsgMessage("You can not open more than 6 accounts.", "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                    //        }
                    //        return;
                    //    }
                    //}
                    
                    string Messaj = "NEW_ROLE";
                    if (Database.ServerDatabase.AllowCreate(login.Key) == false)
                    {
                        Client.GameClient InGame = null;
                        if (Database.Server.GamePoll.TryGetValue((uint)login.Key, out InGame))
                        {
                            if (InGame.Player != null)
                            {
                                MyConsole.WriteLine("Account try join but is in use. " + InGame.Player.Name);
                                if (InGame.Player.UID == 0)
                                {
                                    Database.Server.GamePoll.TryRemove((uint)login.Key, out InGame);
                                    if (InGame != null && InGame.Player != null)
                                    {
                                        if (InGame.Map != null)
                                            InGame.Map.Denquer(InGame);
                                    }
                                }
                            }
                            InGame.Socket.Disconnect();
                            Messaj = "Sorry, you Account is online, Try Again";
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                            }
                            if (InGame.TRyDisconnect-- == 0)
                            {
                                if (InGame.Player != null && InGame.FullLoading)
                                {
                                    InGame.ClientFlag |= Client.ServerFlag.Disconnect;
                                    Database.ServerDatabase.SaveClient(InGame);
                                }
                                Database.Server.GamePoll.TryRemove((uint)login.Key, out InGame);
                                if (InGame != null && InGame.Player != null)
                                {
                                    if (InGame.Map != null)
                                        InGame.Map.Denquer(InGame);

                                }
                            }
                        }
                        else
                        {

                            Database.Server.GamePoll.TryAdd((uint)login.Key, client);
                            Messaj = "ANSWER_OK";
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                if ((client.ClientFlag & Client.ServerFlag.CreateCharacterSucces) != Client.ServerFlag.CreateCharacterSucces)
                                {
                                    Database.ServerDatabase.LoadCharacter(client, (uint)login.Key);
                                }

                                client.ClientFlag |= Client.ServerFlag.AcceptLogin;
                                //NetShield
                                MyConsole.WriteLine("-------------------------------------------------", ConsoleColor.Green);
                                MyConsole.WriteLine("Account: PlayerName > " + client.Player.Name + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("PlayerUID > " + client.Player.UID + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("Version > " + client.Version + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("ProSerial > " + client.TqSerial + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("Mac > " + client.Socket.GetMACAddress() + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("IP > " + client.Socket.RemoteIp + " < ", ConsoleColor.Yellow);
                                MyConsole.WriteLine("-------------------------------------------------", ConsoleColor.Green);
                                //NetShield
                                var dt = DateTime.Now;
                                //string logs = "[Login]" + dt.Hour + "H:" + dt.Minute + "M:" + dt.Second + "S pid: " + client.Player.UID + " " + client.Player.Name + " [ Login  ]   On Game IP  [" + client.Socket.RemoteIp + "] MachineName [" + client.OnLogin.MachineName + "]  MacAddress [ " + client.OnLogin.MacAddress + " ] HDSerial  [" + client.OnLogin.HDSerial + " ]  ";
                                //Database.ServerDatabase.LoginQueue.Enqueue(logs);
                                client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                                client.Send(stream.LoginHandlerCreate(1, client.Player.Map));
                                MsgLoginHandler.LoadMap(client, stream);
                            }
                            Program.CallBack.Register(client);
                        }
                    }
                    else
                    {
                        client.ClientFlag |= Client.ServerFlag.CreateCharacter;
                        using (var rec = new ServerSockets.RecycledPacket())
                        {
                            var stream = rec.GetStream();

                            client.Send(new MsgServer.MsgMessage(Messaj, "ALLUSERS", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Dialog).GetArray(stream));
                        }
                    }
                }
            }
            catch (Exception e) { MyConsole.WriteException(e); }
        }
    }
}