using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Ultimate.Main;

namespace Ultimate.PacketHandling
{
    class MessageBoard
    {
        public static Counter BoardCount = new Counter(1);
        public enum MessageType
        {
            Trade = 2201,
            Friends,
            Team,
            Guilds,
            Others
        }
        public static ConcurrentDictionary<ushort, Message> MessagePosts = new ConcurrentDictionary<ushort, Message>();
        public struct Message
        {
            public string Name;
            public string Msg;
            public DateTime Time;
            public uint Sender;
            public MessageType Type;
            public uint GuildSender;
        }
        public static void RemoveByUID(GameClient GC, ushort BoardID)
        {
            var list = MessagePosts.Where(e => e.Value.Name == GC.MyChar.Name + GC.AuthInfo.Status && e.Value.Type == (MessageType)BoardID);
            foreach (var item in list)
                MessagePosts.Remove(item.Key);
        }
        public static void Handle(Main.GameClient GC, byte[] Data)
        {

            ushort BoardID = BitConverter.ToUInt16(Data, 6);
            byte Action = Data[8];
            switch (Action)
            {
                case 2:// View Board
                    {
                        GC.AddSend(ShowPosts(GC, (MessageType)BoardID, 0));
                        break;
                    }
                case 1:
                    {
                        RemoveByUID(GC, BoardID);
                        break;
                    }
                case 4:// View message.
                    {
                        var message = MessagePosts.Values.Where(e => e.Type == (MessageType)BoardID && e.Sender == GC.MyChar.EntityID)
                            .Take(1)
                            .SingleOrDefault();
                        GC.AddSend(Packets.ChatMessage(0, GC.MyChar.Name, "ALLUSERS", message.Msg, BoardID, 0));
                        break;
                    }
            }


            /*case 153://Trade
              case 154://Friend
              case 155://Team
              case 156://Guild
              case 157://Others
            /*  if (Action == 2)
              {
                  if (BoardID == 153)
                  {
                      ArraySize = (ushort)(Game.World.TradeBoard.Count * 3);
                  }
                  GC.AddSend(Packets.BoardMessage(BoardID, Action, ArraySize, Game.World.TradeBoard));
              }
              else if (Action == 1)
              {
                  if (Game.World.TradeBoard.ContainsKey(GC.MyChar.EntityID))
                      Game.World.TradeBoard.Remove(GC.MyChar.EntityID);

              }
             // int Position = 11;
             // byte Len = Data[11];
             // Position++;
              string Message = "";
              for (int C = 0; C < 18; C++)
              {
                  Message += Convert.ToChar(Data[C]);
                  //Position++;
              }
              GC.LocalMessage(2000, "Board: " + BoardID + " Action: " + Action + " ArrSize: " + ArraySize);
              Console.WriteLine("Data 0: " + Data[0]);
              Console.WriteLine("Data 1: " + Data[1]);
              Console.WriteLine("Data 2: " + Data[2]);
              Console.WriteLine("Data 3: " + Data[3]);
              Console.WriteLine("Data 4: " + Data[4]);
              Console.WriteLine("Data 5: " + Data[5]);
              Console.WriteLine("Data 6: " + Data[6]);
              Console.WriteLine("Data 7: " + Data[7]);
              Console.WriteLine("Data 8: " + Data[8]);
                  Console.WriteLine(Message);
                  */
        }
        public static COPacket ShowPosts(GameClient GC, MessageType Board, int Page)
        {
            int TotalStringLength = 0;

            List<Message> Posts = new List<Message>();

            int PostsAdded = 0;
            int Skip = (Page * 10);
            foreach (Message Post in MessagePosts.Values.Where(e => e.Type == Board).OrderByDescending(e => e.Time).Take(25))
            {
                if (Board != MessageType.Trade)
                {
                    //  Validation for each message type.
                    if (Board == MessageType.Friends && !GC.MyChar.Friends.ContainsKey(Post.Sender))
                        continue;
                    if (Board == MessageType.Guilds && GC.MyChar.MyGuild != null && GC.MyChar.GuildID != Post.GuildSender)
                        continue;
                    if (Board == MessageType.Team && GC.MyChar.MyTeam != null && !GC.MyChar.MyTeam.Members.Contains(Post.Sender))
                        continue;

                }
                Posts.Add(Post);
                PostsAdded++;
                string TimeStamp = String.Format("{0:yyyyMMddHHmm}", Post.Time);
                TotalStringLength += (Post.Name.Length + (Post.Msg.Length > 50 ? 50 : Post.Msg.Length) + TimeStamp.Length);
                //if (PostsAdded == 10) break;
            }
            var byteArr = new byte[10 + TotalStringLength + (3 * Posts.Count) + 8];
            COPacket Packet = new COPacket(byteArr);
            Packet.WriteInt16((ushort)(byteArr.Length - 8));
            Packet.WriteInt16(1111);
            Packet.WriteInt16((ushort)Page); // Assuming this is the page, since it does nothing atm.
            Packet.WriteInt16((ushort)Board); // The board type, aka TradeBoard, FriendBoard, ...
            Packet.WriteByte(3); // Flag - Should always be 3? 3=Send List
            Packet.WriteByte((byte)(3 * Posts.Count)); // String Count
            foreach (Message Post in Posts.OrderByDescending(e => e.Time))// If we wanted to display oldest first, we'd replace Posts with Posts.Reverse<MessageBoardPost>()
            {
                string Msg = Post.Msg;
                string TimeStamp = String.Format("{0:yyyyMMddHHmm}", Post.Time);
                if (Msg.Length > 50)
                    Msg = Msg.Substring(0, 50);
                Packet.WriteByte((byte)Post.Name.Length);
                Packet.WriteString(Post.Name); // Poster's name
                Packet.WriteByte((byte)Post.Msg.Length);
                Packet.WriteString(Msg);
                Packet.WriteByte((byte)TimeStamp.Length);
                Packet.WriteString(TimeStamp); // And the timestamp, YYYY-MM-DD-HH-MM
            }
            return Packet;
        }
        internal static void Write(GameClient player, string Message, int ChatType)
        {
            if (Message.Length < 4 || Message.Length > 63)
            {
                player.LocalMessage(2005, "Message length must between 4 -> 63 character.");
                return;
            }
            if (player.MyChar.Level < 16)
            {
                player.LocalMessage(2005, "You're below level 16.");
                return;
            }
            var result = MessagePosts.Where(e => e.Value.Type == (MessageType)ChatType && e.Value.Sender == player.MyChar.EntityID);
            if (result.Count() != 0)
            {
                var date = result.Take(1).SingleOrDefault().Value.Time;
                if (DateTime.Now < date.AddDays(1))
                {
                    player.LocalMessage(2005, "You already posted for today.");
                    return;
                }
                else
                    RemoveByUID(player, (ushort)ChatType);
            }
            var type = (MessageType)ChatType;
            var UID = BoardCount.Next;
            MessagePosts.TryAdd((ushort)UID, new MessageBoard.Message()
            {
                Name = player.MyChar.Name + player.AuthInfo.Status,
                Msg = Message,
                Time = DateTime.Now,
                Type = type,
                GuildSender = player.MyChar.GuildID,
                Sender = player.MyChar.EntityID
            });


        }
    }
}
