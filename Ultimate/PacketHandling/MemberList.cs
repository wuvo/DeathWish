using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.PacketHandling
{
    public class MemberList
    {
        public static void Handle(Main.GameClient GC)
        {
            if (GC.MyChar.MyGuild != null)
            {
                var GL = "";
                var GLOn = "";
                Dictionary<byte, string> Messages = new Dictionary<byte, string>();
                var OnMembers = new List<MemberInfo>();
                var OffMembers = new List<MemberInfo>();
                var OnDeputies = new List<MemberInfo>();
                var OffDeputies = new List<MemberInfo>();
                var ToBeSent = new List<string>();

                var _memberList = new List<MemberInfo>();
                foreach (KeyValuePair<byte, Dictionary<uint, MemberInfo>> D in GC.MyChar.MyGuild.Members)
                {
                    foreach (MemberInfo M in D.Value.Values)
                    {
                        _memberList.Add(M);
                    }
                }

                foreach (MemberInfo entry in _memberList)
                {
                    if (entry.Rank == GuildRank.GuildLeader)
                    {
                        if (World.H_Chars.ContainsKey(entry.MembID))
                            GLOn = entry.MemberString;
                        else
                            GL += entry.MemberString;
                    }
                    else if (entry.Rank == GuildRank.DeputyManager)
                    {
                        if (World.H_Chars.ContainsKey(entry.MembID))
                            OnDeputies.Add(entry);
                        else
                            OffDeputies.Add(entry);
                    }
                    else
                    {
                        if (World.H_Chars.ContainsKey(entry.MembID))
                            OnMembers.Add(entry);
                        else
                            OffMembers.Add(entry);
                    }
                }

                if (GLOn.Length > 0)
                    ToBeSent.Add(GLOn);

                foreach (var dep in OnDeputies.OrderByDescending(s => s.Level))
                    ToBeSent.Add(dep.MemberString);

                foreach (var mem in OnMembers.OrderByDescending(s => s.Level))
                    ToBeSent.Add(mem.MemberString);

                if (GL.Length > 0)
                    ToBeSent.Add(GL);

                foreach (var dep in OffDeputies.OrderByDescending(s => s.Level))
                    ToBeSent.Add(dep.MemberString);

                foreach (var mem in OffMembers.OrderByDescending(s => s.Level))
                    ToBeSent.Add(mem.MemberString);

                List<KeyValuePair<int, string>> myStrings = new List<KeyValuePair<int, string>>();
                KeyValuePair<int, string> oldString = new KeyValuePair<int, string>();
                string newString = "";
                var myCount = 0;
                int page = 0;
                foreach (string S in ToBeSent)
                {
                    myCount++;
                    if (myCount % 50 == 0)
                    {
                        newString += S;
                        oldString = new KeyValuePair<int, string>(page, newString);
                        myStrings.Add(oldString);
                        page++;
                        newString = "";
                    }
                    else if (myCount == ToBeSent.Count)
                    {
                        newString += S;
                        oldString = new KeyValuePair<int, string>(page, newString);
                        myStrings.Add(oldString);
                    }
                    else
                        newString += S + " ";
                }

                if (GC.MyChar.List > Convert.ToByte(myStrings.Count - 1))
                    GC.MyChar.List = Convert.ToByte(myStrings.Count - 1);

                if (myStrings.Count >= GC.MyChar.List)
                {
                    GC.AddSend(Packets.StringPacket((uint)myCount, StringType.MemberList, myStrings[GC.MyChar.List].Value, true).Get);
                    GC.LocalMessage(2000, "There are currently " + myStrings.Count + " lists in your guild, please type '/list x' where x corresponds to the list number to see other members list from your guild!");
                }
            }
        }
    }
}
