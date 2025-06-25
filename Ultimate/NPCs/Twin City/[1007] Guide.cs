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
    public class NPC_1007 : NPCBase
    {
        public NPC_1007(Main.GameClient _client)
            : base(_client)
        {
            ID = 1007;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello! Welcome to Ultimate-Conquer! Thank you for joining our server. \nIf you wish to know more about the server, feel free to look through this guide! ");
                        AddText("For more complex questions, make sure you join our Discord and open a support-ticket for advanced help. ");
                        AddText("What can I help you with?");
                        //AddText("Howdy! I've been sent here to help players throughout their experience while playing the server!");
                        //AddText(" I'll give my best on answering the most common questions but I must advice you to either contact a PM or");
                        //AddText(" visit the website/forum for more information! So, what do you want to know about?");
                        AddOption("Ask for P-Level", 17);
                        AddOption("How to get started?", 1);
                        AddOption("Vote Points", 2);
                        AddOption("Reborn system", 4);
                        AddOption("Online Points", 14);
                        AddOption("Commands", 13);
                        AddOption("VIP", 6);
                        AddOption("Next Page", 7);
                        break;
                    }
                case 1:
                    {
                        AddText("The best way to get started on this server is by far to get power leveled by higher level players.");
                        AddText(" Your first character should be an archer so that you will be able to hunt and build your own set.");
                        AddText(" You will receive some equipments on promotions that you can use as starting gears.");
                        AddText(" Warriors are also great for leveling and Trojans can reborn into archers and maximize your hunting experience.");
                        AddOption("First Page", 16);
                        AddOption("Thanks!", 255);
                        break;
                    }
                case 7:
                    {
                        AddText("Hello! Welcome to Ultimate-Conquer! Thank you for joining our server. \nIf you wish to know more about the server, feel free to look through this guide! ");
                        AddText("For more complex questions, make sure you join our Discord and open a support-ticket for advanced help. ");
                        AddText("What can I help you with?");
                        //AddText("Howdy! I've been sent here to help players throughout their experience while playing the server!");
                        //AddText(" I'll give my best on answering the most common questions but I must advice you to either contact a PM or");
                        //AddText(" visit the website/forum for more information! So, what do you want to know about?");
                        AddOption("Sockets", 8);
                        AddOption("Mining", 9);
                        AddOption("Gears", 10);
                        AddOption("CPs", 11);
                        AddOption("+n items", 12);
                        AddOption("Quests", 5);
                        AddOption("First Page", 16);
                        AddOption("Thanks!", 255);
                        break;
                    }
                #region VotePoints
                case 2:
                    {
                        AddText("As you probably know, voting is one of the most important factors for a server to increase its community. Therefore, ");
                        AddText("we as a community, will need all the help that we can in order to keep the server on the high ranks. As an acknowledgment ");
                        AddText(" players will get a vote point for each sucessful vote which can be exchanged by rewards! Find VoteManagement NPC in TwinCity for more info!");
                        AddOption("Thanks", 255);
                        break;
                    }
                #endregion
                #region Reborn
                case 4:
                    {
                        AddText("Ultimate Conquer emulates a version similar to TQ's 5017 version, which is the patch core features we are targeting. Therefore, there is only one reborn in here.");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Quests
                case 5:
                    {
                        AddText("As you have probably understood by now, Ultimate Conquer is targeting a Classic version of the game. Therefore many of the old ");
                        AddText("quests loved by players back in the days were added and spiced up a bit in order to make them even more attractive. Quests such ");
                        AddText("as AncientDevil, BlueMouse, SnakeKing, BombQuest, DisCity and so on are here to give you the chills you're looking for!");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region VIP
                case 6:
                    {
                        AddText("We are offering a VIP Subscription through donations. In case you want to know more about it you should check our donation page. ");
                        AddText("VIPs have several benefits such as: Meteors/Dragonballs auto-pick and pack them by right clicking, increased exp/drop rates, ");
                        AddText("drop notifications on +1s and blessed items, among others.");
                        AddOption("Thanks", 255);
                        break;
                    }
                #endregion
                #region Sockets
                case 8:
                    {
                        AddText("While upgrading your gears level or quality in Twin City at Artisan Wind or Market at Magic Artisan you have a chance that your item will be socketed.");
                        AddText(" That is the only way you can socket your gears, apart of weapons that can be socketed in Bird Island at ArtisanOu.");
                        AddText("Alternatively, if you want to spam MetScrolls for faster socketing, make sure you check out SocketMeDaddy NPC in TC.");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Mining
                case 9:
                    {
                        AddText("Oh mining! There's no veteran that doesn't remmember how the old days were in those dark scary caves! ");
                        AddText("In order to provide you the best experience, our mining system was spiced up a bit and so you can get Dragonballs from ");
                        AddText("mining. Plus, there's also the chance of getting refined and super gems as well as gold ores that will surely come handy when you're bankrupted!");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Gears
                case 10:
                    {
                        AddText("The only possible way to get gears is by interacting with the server itself as we do NOT sell any gears for donations. This being ");
                        AddText("said, you will have to hunt, buy and sell from/to other players, PK other players and so on to improve your set.");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Cps
                case 11:
                    {
                        AddText("Considering the fact that we are targeting a Classic version of the game, we have decided to completely remove CPs from our server, ");
                        AddText("therefore there are no CPs on Ultimate Conquer.");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region + items
                case 12:
                    {
                        AddText("The only way you can get +n items is by hunting or participating in PVE/PVP Events. Some quests have also ");
                        AddText("a chance of giving you +1 items!");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Commands
                case 13:
                    {
                        AddText("There are a few commands that our players can use in order to improve their gameplay.");
                        AddText("A few examples are @clearinv (deletes everything inside your inventory), @dc (disconnects you), @onlinepoints (check your online points) ");
                        AddText("@joinpvp (join hourly pvp events if active), @vip (ceck the VIP remaining time) and so on.");
                        //AddOption("All Commands", 18);
                        AddOption("Thanks!", 255);
                        break;
                    }

                //case 18:
                //    {
                //        GC.LocalMessage(2105, "https://discord.gg/HD75P4sBsH");
                //        break;
                //    }
                #endregion
                #region Online Points
                case 14:
                    {
                        AddText("There is a chance your character will win Online Points for the time spent being online. Online Points can be exchanged for many rewards");
                        AddText(" such as double experience, better drops, virtue points or a random reward that can give you Meteors, Dragonballs, Garments and so on.");
                        AddText(" You can claim your rewards at OnlinePoints NPC just to your left :).");
                        AddOption("Thanks!", 255);
                        break;
                    }
                #endregion
                #region Return to page 1
                case 16:
                    {
                        GC.DialogNPC = 1007;
                        NPCs.NPCHandler.Handle(GC, null, 1007, 0);
                        break;
                    }
                #endregion
                #region Ask for PLVL
                case 17:
                    {
                        if (GC.MyChar.Level <= 90)
                        {
                            if (DateTime.Now >= GC.MyChar.LastRequest.AddMinutes(1))
                            {
                                GC.MyChar.LastRequest = DateTime.Now;
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " is asking for P-Level, if anyone is abled to help him/her out, please contact him/her!", 2005, 0);
                            }
                            else
                                GC.LocalMessage(2005, "Please wait for a minute before asking for P-Level again!");
                        }
                        else
                            GC.LocalMessage(2005, "P-Level is only for players under level 90.");
                    }
                    break;
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
}