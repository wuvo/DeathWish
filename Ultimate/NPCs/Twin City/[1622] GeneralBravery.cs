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
    public class NPC_1622 : NPCBase
    {
        public NPC_1622(Main.GameClient _client)
            : base(_client)
        {
            ID = 1622;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I'm here to find the strongest players in the world! I'll be helding weekly and monthly tournaments in order to find them.");
                        AddOption("I want to know more", 1);
                        AddOption("Join Weekly PK Tournament", 2);
                        AddOption("I see", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("There will be two types of tournaments, the Weekly PK Tournament and the Monthly PK Tournament. The first will be held every Sunday at 22:00 and the second will be held every first day of each month at 00:00!");
                        AddOption("What are the rewards?", 3);
                        AddOption("How can I join?", 4);
                        AddOption("I see", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.Level >= 15)
                        {
                            if (World.Events.Count > 0)
                            {
                                foreach (Events.Events E in World.Events)
                                {
                                    if (E.EventTitle == "Weekly PK Tournament")
                                        if (E.AddPlayer(GC.MyChar))
                                        {
                                            GC.MyChar.EventBase = E;
                                            break;
                                        }
                                }
                                if (GC.MyChar.EventBase == null)
                                {
                                    AddText("It's not the right time! Please check the schedule and come back later!");
                                    AddOption("When can I join?", 1);
                                    AddOption("I see", 255);
                                }
                            }
                            else
                            {
                                AddText("It's not the right time! Please check the schedule and come back later!");
                                AddOption("When can I join?", 1);
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You must be at least level 15 to join the event!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        AddText("The winner of each Weekly PK Tournament will win 3,000,000 Silvers and 1 Random Super Gem! The winner of Monthly PK Tournament will win 10,000,000 Silvers and a socket item! Make sure you kill everyone else so that you can win the reward!");
                        AddOption("How can I join?", 4);
                        AddOption("I see", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("You can join the Weekly PK Tournament 5 minutes before it starts. Once it starts you won't be able to get inside again. The last player alive will win the tournament!");
                        AddOption("What are the rewards?", 3);
                        AddOption("I see", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}