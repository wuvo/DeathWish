using System;
using Ultimate.Game;
using Ultimate.Features;
using Ultimate.NPCs;

namespace Ultimate.PacketHandling
{
    public class TeamHandle
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            try
            {
                byte Type = Data[4];
                switch (Type)
                {
                    case 0:
                        {
                            GC.MyChar.MyTeam = new Team(GC.MyChar);
       
                            break;
                        }
                    case 1: // Request to join
                        {
                            uint WhoUID = BitConverter.ToUInt32(Data, 8);
                            Character Who = (Character)World.H_Chars[WhoUID];
                            if (Who == null) return;
                            if (Who.MyTeam != null && Who.TeamLeader && GC.MyChar != null)
                            {
                                if (Who.MyTeam.Members.Contains(GC.MyChar)) return;
                                if (!Who.MyTeam.Forbid)
                                {
                                    if (Who.MyTeam.Members.Count < 5)
                                    {
                                        Who.MyClient.AddSend(Packets.TeamPacket(GC.MyChar.EntityID, 1));
                                        GC.LocalMessage(2005, "[Team]Request to join team has been sent out.");
                                    }
                                    else GC.LocalMessage(2005, "[Team]The team is full. To join this team, the level of the team leader must be Vip6");
                                }
                                else GC.LocalMessage(2005, "[Team]The team doesn't accept new members.");
                            }
                            else GC.LocalMessage(2005, "[Team]The target has not created a team.");

                            break;
                        }
                    case 2:
                        {
                            if (GC.MyChar.MyTeam == null) return;
                            if (GC.MyChar.MyTeam.Members.Contains(GC.MyChar)) GC.MyChar.MyTeam.Leaves(GC.MyChar);
                            break;
                        }
                    case 3:
                        {
                            uint WhoUID = BitConverter.ToUInt32(Data, 8);
                            Character Who = (Character)World.H_Chars[WhoUID];
                            if (Who == null) return;
                            if (!Who.TeamLeader) return;
                            if (GC.MyChar.TeamLeader) return;
                            if (Who.MyTeam == null) return;
                            Who.MyTeam.Joins(GC.MyChar);

                            break;
                        }
                    case 4:
                        {
                            if (GC.MyChar.MyTeam.Members.Count < 5)
                            {
                                uint WhoUID = BitConverter.ToUInt32(Data, 8);
                                Character Who = (Character)World.H_Chars[WhoUID];
                                if (GC.MyChar.MyTeam == null) return;
                                if (!GC.MyChar.TeamLeader) return;
                                if (!Who.TeamLeader && !GC.MyChar.MyTeam.Members.Contains(Who)) Who.MyClient.AddSend(Packets.TeamPacket(GC.MyChar.EntityID, 4));
                                GC.LocalMessage(2005, "[Team]Request to join team has been sent out.");
                            }
                            else if (GC.MyChar.VipLevel == 6)
                            {
                                if (GC.MyChar.MyTeam.Members.Count < 6)
                                {
                                    uint WhoUID = BitConverter.ToUInt32(Data, 8);
                                    Character Who = (Character)World.H_Chars[WhoUID];
                                    if (GC.MyChar.MyTeam == null) return;
                                    if (!GC.MyChar.TeamLeader) return;
                                    if (!Who.TeamLeader && !GC.MyChar.MyTeam.Members.Contains(Who)) Who.MyClient.AddSend(Packets.TeamPacket(GC.MyChar.EntityID, 4));
                                    GC.LocalMessage(2005, "[Team]Request to join team has been sent out.");
                                }
                            }
                            else
                            {
                                GC.LocalMessage(2005, "[Team]The team is full. if you want invite more players you need to be VIP Level 6");
                            }
                            break;
                        }
                    case 5:
                        {
                            uint WhoUID = BitConverter.ToUInt32(Data, 8);
                            Character Who = (Character)World.H_Chars[WhoUID];
                            if (Who == null) return;
                            if (Who.TeamLeader) return;
                            if (GC.MyChar.MyTeam == null) return;
                            if (!GC.MyChar.TeamLeader) return;
                            GC.MyChar.MyTeam.Joins(Who);
                            break;
                        }
                    case 6:// dismiss
                        {
                            if (!GC.MyChar.TeamLeader) return;
                            if (GC.MyChar.MyTeam == null) return;
                            GC.MyChar.MyTeam.Dismiss(GC.MyChar);
                            break;
                        }
                    case 7:
                        {
                            uint WhoUID = BitConverter.ToUInt32(Data, 8);
                            Character Who = (Character)World.H_Chars[WhoUID];
                            if (!GC.MyChar.MyTeam.Members.Contains(Who)) return;
                            if (Who == GC.MyChar) return;
                            if (GC.MyChar.MyTeam == null) return;
                            GC.MyChar.MyTeam.Leaves(Who);
                            break;
                        }
                    case 8:
                        {
                            GC.MyChar.MyTeam.Forbid = true;
                            break;
                        }
                    case 9:
                        {
                            GC.MyChar.MyTeam.Forbid = false;
                            break;
                        }
                    case 10://Money X
                        {
                            GC.MyChar.MyTeam.Money = false;
                            break;
                        }
                    case 11://Money Check
                        {
                            GC.MyChar.MyTeam.Money = true;
                            break;
                        }
                    case 12://Items X
                        {
                            GC.MyChar.MyTeam.Items = false;
                            break;
                        }
                    case 13://Items Check
                        {
                            GC.MyChar.MyTeam.Items = true;
                            break;
                        }
                }
            }
            catch (Exception e) { Program.WriteLine(e); }
        }
    }
}
