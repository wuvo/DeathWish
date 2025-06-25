using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class Friends
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint UID = BitConverter.ToUInt32(Data, 4);
            byte Type = Data[8];
            switch (Type)
            {
                case 10://Add
                    {
                        if (!GC.MyChar.Friends.ContainsKey(UID))
                        {
                            if (GC.MyChar.Friends.Count < 255)
                            {
                                Character C = null; if (World.H_Chars.ContainsKey(UID)) C = World.H_Chars[UID];
                                if (C != null)
                                {
                                    /*GC.LocalMessage(2005, "Friends are not allowed for now.");
                                    break;*/
                                    if (C.RequestFriends != GC.MyChar.EntityID)
                                    {
                                        GC.MyChar.RequestFriends = UID;
                                        GC.LocalMessage(2005, "Request to make friends has been sent out.");
                                        C.MyClient.LocalMessage(2005, GC.MyChar.Name + " wants to make friends with you.");
                                    }
                                    else
                                    {
                                        Friend F = new Friend();
                                        F.Name = C.Name;
                                        F.UID = C.EntityID;
                                        Friend F2 = new Friend();
                                        F2.Name = GC.MyChar.Name;
                                        F2.UID = GC.MyChar.EntityID;

                                        GC.MyChar.Friends.Add(F.UID, F);
                                        C.Friends.Add(F2.UID, F2);

                                        //15 = AddFriend, 19 = AddEnemy, 14 = Remove
                                        World.Chat(GC.MyChar, 2005, "SYSTEM", "ALL", GC.MyChar.Name + " and " + C.Name + " are friends from now on.");
                                        GC.AddSend(Packets.FriendEnemyPacket(F.UID, F.Name, 15, 1));
                                        C.MyClient.AddSend(Packets.FriendEnemyPacket(F2.UID, F2.Name, 15, 1));
                                    }
                                }
                            }
                            else
                                GC.LocalMessage(2005, "You have too many friends! Please delete some first!");
                        }
                        break;
                    }
                case 14://Remove
                    {
                        if (GC.MyChar.Friends.ContainsKey(UID))
                        {
                            Friend F = (Friend)GC.MyChar.Friends[UID];
                            if (F.Online)
                            {
                                Character C = F.Info;
                                if (C.Friends.ContainsKey(GC.MyChar.EntityID))
                                {
                                    C.Friends.Remove(GC.MyChar.EntityID);
                                    C.MyClient.AddSend(Packets.FriendEnemyPacket(GC.MyChar.EntityID, "", 14, 0));
                                }
                                GC.MyChar.Friends.Remove(C.EntityID);
                            }
                            else
                            {
                                string Acc = "";
                                Character C = Database.LoadCharacter(F.Name, ref Acc);
                                if (C != null)
                                {
                                    if (C.Friends.ContainsKey(GC.MyChar.EntityID))
                                        C.Friends.Remove(GC.MyChar.EntityID);
                                    Database.SaveCharacter(C, Acc);
                                }
                            }
                            GC.MyChar.Friends.Remove(F.UID);
                            GC.AddSend(Packets.FriendEnemyPacket(F.UID, "", 14, 0));

                            GC.LocalMessage(2005, GC.MyChar.Name + " has broken the friendship with " + F.Name + ".");
                            World.Chat(GC.MyChar, 2005, "SYSTEM", "ALL", GC.MyChar.Name + " has broken the friendship with " + F.Name + ".");
                        }                        
                        break;
                    }
                case 18:
                    {
                        if (GC.MyChar.Enemies.ContainsKey(UID))
                        {
                            GC.MyChar.Enemies.Remove(UID);
                            GC.AddSend(Packets.FriendEnemyPacket(UID, "", 14, 0));
                        }
                        break;
                    }
            }
        }
    }
}
