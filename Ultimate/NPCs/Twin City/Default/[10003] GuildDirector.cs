using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_10003 : NPCBase
    {
        public NPC_10003(Main.GameClient _client)
            : base(_client)
        {
            ID = 10003;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                #region Control 0 - Base Control
                case 0:
                    {
                        AddText("Greetings " + GC.MyChar.Name + "! I am the guild director, in charge of administrating and managing guilds. What business do you have with me?");
                        if (GC.MyChar.MyGuild == null)
                            AddOption("Create a Guild.", 1);
                        else if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            AddOption("Guild Management.", 3);
                            AddOption("Disband '" + GC.MyChar.MyGuild.GuildName + "'", 18);
                        }
                        AddOption("Inquire about a guild.", 20);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                #endregion
                #region Create Guild
                #region Control 1 - Proceeding Option -> Control 0: Create a Guild.
                case 1:
                    {
                        AddText("Leading a guild is no small task. It requires not only strength, but also dedication and patience. For this very reason you are required to be at least Level 90, and pay an administrative fee of 3,000,000 silvers and 3 DragonBalls.");
                        AddInput("I would like to name it;", 2);
                        AddOption("I've changed my mind.", 255);
                        break;
                    }
                #endregion
                #region Control 2 - Proceeding Option -> Control 1: I would like to name it.
                case 2:
                    {
                        if (GC.MyChar.MyGuild == null)
                        {
                            if (GC.MyChar.Level >= 90)
                            {
                                if (GC.MyChar.Silvers >= 3000000)
                                {
                                    if (GC.MyChar.InventoryContains(1088000, 3))
                                    {
                                        string GuildName = ReadString(_data);
                                        if (Features.Guilds.ValidName(GuildName))
                                        {
                                            if (GuildName.Length < 16)
                                            {
                                                for (int a = 0; a < 3; a++)
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                                GC.MyChar.Silvers -= 3000000;
                                                ushort NewGuildID = (ushort)Program.Rnd.Next(ushort.MaxValue);
                                                while (Features.Guilds.AllTheGuilds.ContainsKey(NewGuildID))
                                                    NewGuildID = (ushort)Program.Rnd.Next(ushort.MaxValue);
                                                Features.Guilds.CreateNewGuild(GuildName, NewGuildID, GC.MyChar);

                                                Game.World.Action(GC.MyChar, Packets.StringPacket(GC.MyChar.MyGuild.GuildID, Game.StringType.GuildName, GC.MyChar.MyGuild.GuildName).Get);

                                                Game.World.Spawn(GC.MyChar, false);
                                                GC.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                                GC.AddSend(Packets.StringPacket(GC.MyChar.MyGuild.GuildID, Game.StringType.GuildName, GC.MyChar.MyGuild.GuildName));
                                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has successfully created the guild " + GuildName + "!", 2000, 0);
                                            }
                                            else
                                            {
                                                AddText("Unfortunately that name is too long (max 15 characters). Please choose another one.");
                                                AddInput("I would like to name it;", 2);
                                                AddOption("I've changed my mind.", 255);
                                            }
                                        }
                                        else
                                        {
                                            AddText("Unfortunately that name is either taken, or contains invalid characters. Please choose another one.");
                                            AddInput("I would like to name it;", 2);
                                            AddOption("I've changed my mind.", 255);
                                        }
                                    }
                                    else
                                    {
                                        AddText("I'm sorry but you don't have 3 DragonBalls.");
                                        AddOption("I see.", 255);
                                    }
                                }
                                else
                                {
                                    AddText("In order to create a guild, I require 3,000,000 silvers. Unfortunately it seems that you do not currently have this. Please come back when you have the money.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("In order to create a guild, you need to be at least Level 90. Unfortunately you are currently only Level " + GC.MyChar.Level + ". Come back when you are strong enough to lead an army.");
                                AddOption("I will!", 255);
                            }
                        }
                        else
                        {
                            AddText("You cannot create a guild while being a member of an existing one. Please leave your current guild before you try again.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Guild Management
                #region Control 3 - Proceeding Option -> Control 0: Guild Management.
                case 3:
                    {
                        AddText("Greetings " + GC.MyChar.Name + "! I can help you manage every aspect of '" + GC.MyChar.MyGuild.GuildName + "' from this very window. What would you like to do?");
                        if ((GC.MyChar.MyGuild.Members[(byte)90]).Count < 10)
                            AddOption("Add a Deputy", 4);
                        AddOption("Remove a Deputy", 6);
                        AddOption("Add an Ally", 8);
                        AddOption("Remove an Ally", 10);
                        AddOption("Add an Enemy", 12);
                        AddOption("Remove an Enemy", 14);
                        AddOption("Pass Leadership", 16);
                        AddOption("I've changed my mind.", 255);
                        break;
                    }
                #endregion
                #region Add Deputy
                #region Control 4 - Proceeding Option -> Control 3: Add a Deputy
                case 4:
                    {
                        AddText("It will cost you one DragonBall! Which of your noble guild members would you like to promote to a Deputy? Please keep in mind you can only have a maximum of five Deputies at any one time.");
                        AddInput("I choose;", 5);
                        break;
                    }
                #endregion
                #region Control 5 - Proceeding Option -> Control 4: I choose;
                case 5:
                    {
                        string PlayerName = ReadString(_data);
                        Features.MemberInfo M = GC.MyChar.MyGuild.MembOfName(PlayerName);
                        if (M != null && M.MembName == PlayerName)
                        {
                            if ((GC.MyChar.MyGuild.Members[(byte)90]).Count < 10)
                            {
                                if (PlayerName == GC.MyChar.Name)
                                {
                                    AddText("You can't appoint yourself as a Deputy Leader!");
                                    AddOption("I see", 255);
                                }
                                else if (M.Rank == Features.GuildRank.DeputyManager)
                                {
                                    AddText(M.MembName + " is already a Deputy Leader of your guild!");
                                    AddOption("I see", 255);
                                }
                                else
                                {
                                    if (GC.MyChar.InventoryContains(1088000, 1))
                                    {
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                        M.Rank = Features.GuildRank.DeputyManager;
                                        (GC.MyChar.MyGuild.Members[(byte)50]).Remove(M.MembID);
                                        (GC.MyChar.MyGuild.Members[(byte)90]).Add(M.MembID, M);
                                        Game.Character C = M.Info;
                                        AddText("Congratulations! " + M.MembName + " is now one of your trusted Deputies!");
                                        AddOption("Thank you.", 255);
                                        if (C != null)
                                        {
                                            C.GuildRank = Ultimate.Features.GuildRank.DeputyManager;
                                            Game.World.Spawn(C, false);
                                            C.MyClient.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                        }
                                        GC.MyChar.MyGuild.GuildMsg("SYSTEM", GC.MyChar.MyGuild.Members.Values.ToString(), GC.MyChar.Name + " has added " + C.Name + " has one of his/her trusted Deputy Leaders!", 0);
                                    }
                                    else
                                    {
                                        AddText("You don't have a DragonBall!");
                                        AddOption("I see", 255);
                                    }

                                }
                            }
                            else
                            {
                                AddText("I am afraid you already have 10 Deputies. If you wish to add a new one, you must first remove an existing Deputy.");
                                AddOption("Remove a Deputy", 6);
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText(PlayerName + " is not currently a member of '" + GC.MyChar.MyGuild.GuildName + "'. Would you like to try again?");
                            AddOption("Yes please.", 4);
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Remove Deputy
                #region Control 6 - Proceeding Option -> Control 3: Remove a Deputy
                case 6:
                    {
                        AddText("Which of your current Deputies would you like to demote?");
                        AddInput("I choose;", 7);
                        break;
                    }
                #endregion
                #region Control 7 - Proceeding Option -> Control 6: I choose;
                case 7:
                    {
                        string PlayerName = ReadString(_data);
                        Features.MemberInfo M = GC.MyChar.MyGuild.MembOfName(PlayerName);
                        if (M != null && M.MembName == PlayerName)
                        {
                            if (M.Rank == Features.GuildRank.DeputyManager)
                            {
                                M.Rank = Ultimate.Features.GuildRank.Member;
                                (GC.MyChar.MyGuild.Members[(byte)90]).Remove(M.MembID);
                                (GC.MyChar.MyGuild.Members[(byte)50]).Add(M.MembID, M);
                                Game.Character C = M.Info;
                                AddText("You have successfully demoted " + M.MembName + " to the rank of 'Member'.");
                                AddOption("Thank you.", 255);
                                if (C != null)
                                {
                                    C.GuildRank = Ultimate.Features.GuildRank.Member;
                                    Game.World.Spawn(C, false);
                                    C.MyClient.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                }
                            }
                            else
                            {
                                AddText("Unfortunately " + M.MembName + " is not a Deputy Leader, so you cannot demote him.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText(PlayerName + " is not currently a member of '" + GC.MyChar.MyGuild.GuildName + "'. Would you like to try again?");
                            AddOption("Yes please.", 6);
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Add an Ally
                #region Control 8 - Proceeding Option -> Control 3: Add an Ally
                case 8:
                    {
                        AddText("It will cost you a DragonBall. Which guild would you like to make an alliance with?");
                        AddInput("I want to Ally;", 9);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                #endregion
                #region Control 9 - Proceeding Option -> Control 8: I want to Ally;
                case 9:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                string Ally = ReadString(_data);
                                if (Ally != GC.MyChar.MyGuild.GuildName)
                                {
                                    if (GC.MyChar.MyGuild.Allies.Count < 5)
                                    {
                                        bool Found = false;
                                        foreach (Features.Guild g in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (g.GuildName == Ally)
                                         {
                                                Found = true;
                                                if (g.Creator.Info != null && g.Allies.Count < 5)
                                                {
                                                    if (g.Creator.Info.MyTeam != null)
                                                    {
                                                        if (g.Creator.Info.MyTeam.Members.Contains(GC.MyChar))
                                                        {
                                                            if (!GC.MyChar.MyGuild.Enemies.ContainsKey(g.GuildID))
                                                            {
                                                                if (GC.MyChar.InventoryContains(1088000, 1))
                                                                {
                                                                    if (GC.MyChar.MyGuild.AddAlly(g))
                                                                    {
                                                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                                                        AddText("Congratulations! You are now in an alliance with " + g.GuildName + "!");
                                                                        AddOption("Thank you.", 255);
                                                                    }
                                                                    else
                                                                    {
                                                                        AddText("It seems that you are already in an alliance with " + g.GuildName + ".");
                                                                        AddOption("I see", 255);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    AddText("I'm sorry but you don't have a DragonBall.");
                                                                    AddOption("I see", 255);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                AddText("It seems that you are currently at war with " + g.GuildName + ". If you wish to make an alliance, you must first delcare a truce.");
                                                                AddOption("I see.", 255);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            AddText("If you wish to form an alliance you must have the Guild Leader of " + g.GuildName + " in your team.");
                                                            AddOption("I see.", 255);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AddText("If you wish to form an alliance you must have the Guild Leader of " + g.GuildName + " in your team.");
                                                        AddOption("I see.", 255);
                                                    }
                                                }
                                                else
                                                {
                                                    AddText(g.GuildName + " cannot add more allies as they already have 5 allies!");
                                                    AddOption("I see.", 255);
                                                }
                                            }
                                        }
                                        if (!Found)
                                        {
                                            AddText("It appears that the guild you have specified does not exist.");
                                            AddOption("I see.", 255);
                                        }
                                    }
                                    else
                                    {
                                        AddText("You can't be allied to more than 5 guilds!");
                                        AddOption("I see", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You cannot form an alliance with your own guild.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("I am afraid only the Guild Leader can form an alliance.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Remove an Ally
                #region Control 10 - Proceeding Option -> Control 3: Remove an Ally
                case 10:
                    {
                        AddText("Which guild would you like to end your alliance with?");
                        AddInput("I want to remove;", 11);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                #endregion
                #region Control 11 - Proceeding Option -> Control 10: I want to remove;
                case 11:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                string Ally = ReadString(_data);
                                if (Ally != GC.MyChar.MyGuild.GuildName)
                                {
                                    bool Found = false;
                                    foreach (Features.Guild g in Features.Guilds.AllTheGuilds.Values)
                                    {
                                        if (g.GuildName == Ally)
                                        {
                                            Found = true;
                                            if (GC.MyChar.MyGuild.RemoveAlly(g))
                                            {
                                                AddText("You have ended your alliance with " + g.GuildName + "!");
                                                AddOption("Thank you", 255);
                                            }
                                            else
                                            {
                                                AddText("It seems that you were never in an alliance with " + g.GuildName + ".");
                                                AddOption("I see", 255);
                                            }
                                            //if (GC.MyChar.MyGuild.Allies.ContainsKey(g.GuildID))
                                            //{
                                            //    foreach (Dictionary<uint, Features.MemberInfo> H in GC.MyChar.MyGuild.Members.Values)
                                            //    {
                                            //        foreach (Features.MemberInfo M in H.Values)
                                            //            if (M.Info != null)
                                            //                M.Info.MyClient.AddSend(Packets.StringPacket(g.GuildID, Game.StringType.GuildName, g.GuildName));
                                            //    }
                                            //    GC.MyChar.MyGuild.Allies.Remove(g.GuildID);
                                            //    g.SaveAssociates = true;
                                            //    GC.MyChar.MyGuild.SaveAssociates = true;
                                            //    AddText("You have ended your alliance with " + g.GuildName + "!");
                                            //    AddOption("Thank you.", 255);
                                            //}
                                        }
                                    }
                                    if (!Found)
                                    {
                                        AddText("It appears that the guild you have specified does not exist.");
                                        AddOption("I see.", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You cannot end an alliance with your own guild.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("I am afraid only the Guild Leader can end an alliance.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Add an Enemy
                #region Control 12 - Proceeding Option -> Control 3: Add an Enemy
                case 12:
                    {
                        AddText("Which guild would you like to start a war with?");
                        AddInput("I want to Enemy;", 13);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                #endregion
                #region Control 13 - Proceeding Option -> Control 12: I want to Enemy;
                case 13:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                string Enemy = ReadString(_data);
                                if (Enemy != GC.MyChar.MyGuild.GuildName && Enemy != "UltimateConquer")
                                {
                                    if (GC.MyChar.MyGuild.Enemies.Count < 5)
                                    {
                                        bool Found = false;
                                        foreach (Features.Guild g in Features.Guilds.AllTheGuilds.Values)
                                        {
                                            if (g.GuildName == Enemy)
                                            {
                                                Found = true;
                                                if (!GC.MyChar.MyGuild.Allies.ContainsKey(g.GuildID))
                                                {
                                                    if (GC.MyChar.MyGuild.AddEnemy(g))
                                                    {
                                                        AddText("Congratulations! You are now at war with " + g.GuildName + "!");
                                                        AddOption("Thank you.", 255);
                                                    }
                                                    else
                                                    {
                                                        AddText("It seems that you are already at war with " + g.GuildName + ".");
                                                        AddOption("I see.", 255);
                                                    }
                                                }
                                                else
                                                {
                                                    AddText("You are currently in an alliance with " + g.GuildName + ". You must end your alliance before you can start a war.");
                                                    AddOption("I see.", 255);
                                                }
                                            }
                                        }
                                        if (!Found)
                                        {
                                            AddText("It appears that the guild you have specified does not exist.");
                                            AddOption("I see.", 255);
                                        }
                                    }
                                    else
                                    {
                                        AddText("You can't have more than 5 enemy guilds!");
                                        AddOption("I see", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You cannot start a war with your own guild or with the STAFF guild.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("I am afraid only the Guild Leader can start a war.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Remove an Enemy
                #region Control 14 - Proceeding Option -> Control 3: Remove an Enemy
                case 14:
                    {
                        AddText("Which guild would you like to declare a truce with?");
                        AddInput("I want to remove;", 15);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                #endregion
                #region Control 15 - Proceeding Option -> Control 14: I want to remove;
                case 15:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                string Enemy = ReadString(_data);
                                if (Enemy != GC.MyChar.MyGuild.GuildName)
                                {
                                    bool Found = false;
                                    foreach (Features.Guild g in Features.Guilds.AllTheGuilds.Values)
                                    {
                                        if (g.GuildName == Enemy)
                                        {
                                            Found = true;
                                            if (GC.MyChar.MyGuild.RemoveEnemy(g))
                                            {
                                                AddText("Congratulations! You have declared a truce in your war against " + g.GuildName + "!");
                                                AddOption("Thank you", 255);
                                            }
                                            else
                                            {
                                                AddText("It seems that you were never at war with " + g.GuildName + "!");
                                                AddOption("I see", 255);
                                            }
                                            //if (GC.MyChar.MyGuild.Enemies.ContainsKey(g.GuildID))
                                            //{
                                            //    foreach (Dictionary<uint, Features.MemberInfo> H in GC.MyChar.MyGuild.Members.Values)
                                            //    {
                                            //        foreach (Features.MemberInfo M in H.Values)
                                            //            if (M.Info != null)
                                            //                M.Info.MyClient.AddSend(Packets.StringPacket(g.GuildID, Game.StringType.GuildName, g.GuildName));
                                            //    }
                                            //    GC.MyChar.MyGuild.Enemies.Remove(g.GuildID);
                                            //    GC.MyChar.MyGuild.SaveAssociates = true;
                                            //    AddText("Congratulations! You have declared a truce in your war against " + g.GuildName + "!");
                                            //    AddOption("Thank you.", 255);
                                            //}
                                        }
                                    }
                                    if (!Found)
                                    {
                                        AddText("It appears that the guild you have specified does not exist.");
                                        AddOption("I see.", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You cannot declare a truce with your own guild!");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("I am afraid only the Guild Leader can declare a truce.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Pass Leadership
                #region Control 16 - Proceeding Option -> Control 3: Pass Leadership.
                case 16:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                AddText("It will cost you 5 DragonBalls! Which of your noble members would you like to pass your leadership to?");
                                AddInput("I choose;", 17);
                            }
                            else
                            {
                                AddText("Only the Guild Leader can pass the leadership of a guild.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #region Control 17 - Proceeding Option -> Control 16: I choose;
                case 17:
                    {
                        string playerName = ReadString(_data);
                        Features.MemberInfo M = GC.MyChar.MyGuild.MembOfName(playerName);
                        Features.MemberInfo GL = GC.MyChar.MyGuild.MembOfName(GC.MyChar.Name);
                        if (M != null && M.MembName == playerName)
                        {
                            if (M.MembName != GC.MyChar.Name)
                            {
                                if (M.Info.MyClient.Soc.Connected)
                                {
                                    if (GC.MyChar.InventoryContains(1088000, 3))
                                    {
                                        for (int e = 0; e < 3; e++)
                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                        GL.Rank = Features.GuildRank.Member;
                                        (GC.MyChar.MyGuild.Members[(byte)100]).Remove(GL.MembID);
                                        (GC.MyChar.MyGuild.Members[(byte)50]).Add(GL.MembID, GL);
                                        if (M.Rank == Features.GuildRank.Member)
                                        {
                                            GC.MyChar.MyGuild.Creator = M;
                                            M.Rank = Features.GuildRank.GuildLeader;
                                            (GC.MyChar.MyGuild.Members[(byte)50]).Remove(M.MembID);
                                            (GC.MyChar.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                        }
                                        else if (M.Rank == Features.GuildRank.DeputyManager)
                                        {
                                            GC.MyChar.MyGuild.Creator = M;
                                            M.Rank = Features.GuildRank.GuildLeader;
                                            (GC.MyChar.MyGuild.Members[(byte)90]).Remove(M.MembID);
                                            (GC.MyChar.MyGuild.Members[(byte)100]).Add(M.MembID, M);
                                        }
                                        Game.Character C = M.Info;
                                        Game.Character C2 = GL.Info;
                                        if (C != null)
                                        {
                                            C.GuildRank = Features.GuildRank.GuildLeader;
                                            Game.World.Spawn(C, false);
                                            C.MyClient.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                        }
                                        if (C2 != null)
                                        {
                                            C2.GuildRank = Features.GuildRank.Member;
                                            Game.World.Spawn(C2, false);
                                            C2.MyClient.AddSend(Packets.GuildInfo(GC.MyChar.MyGuild, GC.MyChar));
                                        }
                                        AddText("Congratulations! You have successfully passed the leadership of '" + GC.MyChar.MyGuild.GuildName + "' to " + playerName + "!");
                                        AddOption("Thank you.", 255);
                                        // Features.Guilds.SaveGuilds();
                                    }
                                    else
                                    {
                                        AddText("I'm sorry but it seems like you don't have 3 DragonBalls!");
                                        AddOption("I see.", 255);
                                    }
                                }
                                else
                                {
                                    AddText("Unfortunately " + playerName + " is currently offline. The guild member that you wish to pass your leadership to must be online for it to be successful.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("You cannot pass the leadership to yourself!");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText(playerName + " is not currently a member of " + GC.MyChar.MyGuild.GuildName + ". Would you like to try again?");
                            AddOption("Yes please.", 16);
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #endregion
                #region Disband Guild
                #region Control 18 - Proceeding Option -> Control 0: Disband Guild
                case 18:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                AddText("Disbanding your guild is a decision not to be taken lightly. When armies fall apart, a period of chaos ensues. Are you sure you want to disband your guild?");
                                AddOption("Yes, I've made up my mind.", 19);
                                AddOption("I've changed my mind.", 255);
                            }
                            else
                            {
                                AddText("Only the Guild Leader may choose to disband the guild.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #region Control 19 - Proceeding Option -> Control 18: Yes, I've made up my mind.
                case 19:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                            {
                                GC.MyChar.MyGuild.Disband();
                            }
                            else
                            {
                                AddText("Only the Guild Leader may choose to disband the guild.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You are not currently part of a guild.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Inquire about a Guild
                #region Control 20 - Proceeding Option -> Control 0: Inquire about a guild
                case 20:
                    {
                        AddText("Which guild would you like to inquire about?");
                        AddInput("I want to inquire about;", 21);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                #endregion
                #region Control 21 - Proceeding Option -> Control 20: I want to inquire about;
                case 21:
                    {
                        string Name = ReadString(_data);
                        bool Found = false;
                        foreach (Features.Guild G in Features.Guilds.AllTheGuilds.Values)
                        {
                            if (G.GuildName == Name)
                            {
                                AddText("Guild Name: " + G.GuildName + "\nGuild Leader: " + G.Creator.MembName + "\nGuild Members: " + G.MembersCount + "\nGuild Fund: " + G.Fund);
                                AddOption("Close", 255);
                                Found = true;
                            }
                        }
                        if (!Found)
                        {
                            AddText("It looks like that guild doesn't exist!");
                            AddOption("Close", 255);
                        }
                        break;
                    }
                    #endregion
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
}