using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewestCOServer.PacketHandling
{
    /*
     
*/
    public class GetGuildMembers
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            if (GC.MyChar.MyGuild != null)
            {//2sec
             //(char)Length Level ON
                if (Data[8] == 11)//nu mai iei dc de la whisper acu :P nvm...:D dar cum sti ca ar trebui sa fie 11 si care parte din data?
                {
                    //i= startindex din data[x] <amount din guild si vad dc count < 12 i++
                    
                    string[] MembersStringArr = new string[256];
                    for (int i = 0; i < 256; i++)
                        MembersStringArr[i] = "";
                    byte Count = 0;
                    byte Count2 = 0;
                    byte Count3 = 0;
                    string MembersString = "";
                    string MembersString2 = "";
                    string MembersString3 = "";
                    Dictionary<uint, Features.MemberInfo> CurHt = GC.MyChar.MyGuild.Members[(byte)100];
                    foreach (Features.MemberInfo M in CurHt.Values)
                    {
                        MembersStringArr[M.Level] += M.MemberString;
                        if (MembersString.Length < 985)
                        {
                            Count++;
                            MembersString += M.MemberString;
                        }
                        else if (MembersString2.Length < 985)
                        {
                            Count2++;
                            MembersString2 += M.MemberString;
                        }
                        else if (MembersString3.Length < 985)
                        {
                            Count3++;
                            MembersString3 += M.MemberString;
                        }

                    }
                    CurHt = GC.MyChar.MyGuild.Members[(byte)90];
                    foreach (Features.MemberInfo M in CurHt.Values)
                    {
                        MembersStringArr[M.Level] += M.MemberString;
                        if (MembersString.Length < 985)
                        {
                            Count++;
                            MembersString += M.MemberString;
                        }
                        else if (MembersString2.Length < 985)
                        {
                            Count2++;
                            MembersString2 += M.MemberString;
                        }
                        else if (MembersString3.Length < 985)
                        {
                            Count3++;
                            MembersString3 += M.MemberString;
                        }
                    }
                    CurHt = GC.MyChar.MyGuild.Members[(byte)50];
                    foreach (Features.MemberInfo M in CurHt.Values)
                    {
                        MembersStringArr[M.Level] += M.MemberString;
                        if (MembersString.Length < 985)
                        {
                            Count++;
                            MembersString += M.MemberString;
                        }
                        else if (MembersString2.Length < 985)
                        {
                            Count2++;
                            MembersString2 += M.MemberString;
                        }
                        else if (MembersString3.Length < 985)
                        {
                            Count3++;
                            MembersString3 += M.MemberString;
                        }
                    }

                    /*  for (int i = 255; i > 0; i--)
                          if (MembersStringArr[i].Length > 0)
                          {
                              if (MembersString.Length < 985)
                              {
                                  Count++;
                                  MembersString += MembersStringArr[i];
                              }
                              else if (MembersString2.Length < 985)
                              {
                                  Count2++;
                                  MembersString2 += MembersStringArr[i];
                              }
                              else if (MembersString3.Length < 985)
                              {
                                  Count3++;
                                  MembersString3 += MembersStringArr[i];
                              }
                          }*/
                    if (GC.MyChar.List == 1 || Count2 == 0)
                        GC.AddSend(Packets.StringGuild(GC.MyChar.MyGuild.GuildID, 11, MembersString, Count));
                    else if ((Count2 > 0 && GC.MyChar.List == 2) || Count3 == 0)
                    {
                        GC.AddSend(Packets.StringGuild(GC.MyChar.MyGuild.GuildID, 11, MembersString2, Count2));
                    }
                    else if (Count3 > 0 && GC.MyChar.List == 3)
                        GC.AddSend(Packets.StringGuild(GC.MyChar.MyGuild.GuildID, 11, MembersString3, Count3));
                    GC.LocalMessage(2000, "Type '/list 1' or '/list 2' or '/list 3' to see the second or third members list from your guild!");
                }
            }
        }
    }
}
