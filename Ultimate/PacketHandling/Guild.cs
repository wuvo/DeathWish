using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class Guild
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            byte Type = Data[4];
            switch (Type)
            {
                case 1://Join request
                    {
                        if (GC.MyChar.MyGuild == null)
                        {
                            uint UID = BitConverter.ToUInt32(Data, 8);
                            
                            if (World.H_Chars.ContainsKey(UID))
                            {
                                Character Chr = World.H_Chars[UID];

                                if (Chr.MyGuild != null && (Chr.GuildRank == Features.GuildRank.GuildLeader || Chr.GuildRank == Features.GuildRank.DeputyManager))
                                    Chr.MyClient.AddSend(Packets.SendGuild(GC.MyChar.EntityID, 1));
                            }
                        }
                        break;
                    }
                case 2://Receive join request
                    {
                        if (GC.MyChar.MyGuild != null && (GC.MyChar.GuildRank == Features.GuildRank.DeputyManager || GC.MyChar.GuildRank == Features.GuildRank.GuildLeader))
                        {
                            uint UID = BitConverter.ToUInt32(Data, 8);
                            if (World.H_Chars.ContainsKey(UID))
                            {
                                Character Chr = World.H_Chars[UID];
                                if (Chr.MyGuild == null)
                                {
                                    Features.MemberInfo M = new Features.MemberInfo();
                                    M.Donation = 0;
                                    M.Level = Chr.Level;
                                    M.MembID = Chr.EntityID;
                                    M.MembName = Chr.Name;
                                    M.MyGuildID = GC.MyChar.MyGuild.GuildID;
                                    M.Rank = Features.GuildRank.Member;
                                    Chr.MembInfo = M;
                                    Chr.GuildRank = Features.GuildRank.Member;
                                    Chr.GuildDonation = 0;
                                    Chr.MyGuild = GC.MyChar.MyGuild;
                                    GC.MyChar.MyGuild.AddMember(M, true);
                                    Chr.MyClient.AddSend(Packets.GuildInfo(Chr.MyGuild, Chr));
                                    Chr.MyClient.AddSend(Packets.StringPacket(Chr.MyGuild.GuildID, StringType.GuildName, Chr.MyGuild.GuildName));
                                    World.Spawn(Chr, false);
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.MyGuild != null && GC.MyChar.GuildRank != Features.GuildRank.GuildLeader)
                        {
                            GC.AddSend(Packets.SendGuild(GC.MyChar.MyGuild.GuildID, 19));
                            GC.MyChar.MyGuild.MemberLeaves(GC.MyChar.EntityID, false);
                            GC.MyChar.GuildRank = 0;
                            GC.MyChar.GuildDonation = 0;
                            GC.MyChar.MyGuild = null;
                            World.Spawn(GC.MyChar, false);
                        }
                        break;
                    }
                case 6:
                    {
                        uint UID = BitConverter.ToUInt32(Data, 8);
                        if (World.H_Chars.ContainsKey(UID))
                        {
                            Character C = World.H_Chars[UID];
                            if (!C.Invisible)
                                if (C.MyGuild != null)
                                    GC.AddSend(Packets.StringPacket(C.MyGuild.GuildID, Game.StringType.GuildName, C.MyGuild.GuildName));
                            if (GC.PM)
                            {
                                GC.LocalMessage(2000, "Guild case 6 (request name) on char: " + C.Name);
                            }
                        }

                        break;
                    }
                case 11://Donate
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            uint Amount = BitConverter.ToUInt32(Data, 8);
                            if (GC.MyChar.Silvers >= Amount)
                            {
                                GC.MyChar.Silvers -= Amount;
                                GC.MyChar.MembInfo.Donation += Amount;
                                GC.MyChar.GuildDonation += Amount;
                                GC.MyChar.MyGuild.Fund += Amount;
                                GC.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                GC.MyChar.MyGuild.GuildMsg("SYSTEM", "ALL", GC.MyChar.Name + " donated " + Amount.ToString() + " to " + GC.MyChar.MyGuild.GuildName + ".", 0);
                            }
                        }
                        break;
                    }
                case 12://Guild status
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            GC.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                            GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, GC.MyChar.MyGuild.Bulletin, 2111, 0));
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 97));
                            GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain " + (GC.MyChar.MyGuild.Wins) + "% more experience.", 2005, 0));

                            if (Features.CityWarTc.LastWinner == GC.MyChar.MyGuild)
                            {
                                GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain %25 more drop for win CityWarTC.", 2000, 0));
                            }
                            if (Features.CityWarPc.LastWinner == GC.MyChar.MyGuild)
                            {
                                GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain %25 more drop for win CityWarPC.", 2000, 0));
                            }
                            if (Features.CityWarAc.LastWinner == GC.MyChar.MyGuild)
                            {
                                GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain %25 more drop for win CityWarAC.", 2000, 0));
                            }
                            if (Features.CityWarDc.LastWinner == GC.MyChar.MyGuild)
                            {
                                GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain %25 more drop for win CityWarDC.", 2000, 0));
                            }
                            if (Features.CityWarBi.LastWinner == GC.MyChar.MyGuild)
                            {
                                GC.AddSend(Packets.ChatMessage(GC.MessageID, "SYSTEM", GC.MyChar.Name, "While being in this guild you gain %25 more drop for win CityWarBI.", 2000, 0));
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (GC.GM)
                        {
                            GC.LocalMessage(2001, "Guild Case: " + Type);
                        }
                        break;
                    }
            }
        }
    }
}
