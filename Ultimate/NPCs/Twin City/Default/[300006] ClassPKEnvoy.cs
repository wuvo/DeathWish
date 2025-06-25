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
    public class NPC_300006 : NPCBase
    {
        public NPC_300006(Main.GameClient _client)
            : base(_client)
        {
            ID = 300006;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I have been sent to this world to look for the strongest players of each Class! I'll be helding PK Tournaments from Monday to Friday in order to find them!");
                        AddOption("I want to know more", 1);
                        AddOption("Join ClassPK Tournament", 2);
                        break;
                    }
                case 1:
                    {
                        AddText("There will be two rounds for each class starting at 10:00 and 22:00. Trojans will be fighting on Mondays, Warriors on Tuesdays, Archers on Wednesdays, Fires on Thurdays and Waters on Fridays!");
                        AddOption("What are the rewards?", 3);
                        AddOption("How can I join?", 4);
                        AddOption("I see", 255);
                        break;
                    }
                case 2:
                    {
                        foreach (Events.Events E in World.Events)
                            if (E.DialogID == 19)
                            {
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
                        break;
                    }
                case 3:
                    {
                        AddText("The winner of each ClassPK Tournament will win 1,500,000 Silvers and 5 DragonBalls! Make sure you kill everyone else so that you can win the reward!");
                        AddOption("How can I join?", 4);
                        AddOption("I see", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("You can join the ClassPK Tournament 5 minutes before it starts. Once it starts you won't be able to get inside again. The last player alive will win the tournament!");
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