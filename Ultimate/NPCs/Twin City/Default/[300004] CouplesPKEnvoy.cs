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
    public class NPC_300004 : NPCBase
    {
        public NPC_300004(Main.GameClient _client)
            : base(_client)
        {
            ID = 300004;
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
                        AddText("Howdy dear adventurer! I've been sent here to look for the perfect couple. I'll be holding a tournament every week to see who are the strongest ones.");
                        AddOption("Tell me more about it", 1);
                        AddOption("Join Couples PK Tournament", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("As I said, I'll be holding a Tournament once a week. It will be held every Saturday at 23:00! You will have to team up with your lover and you'll be able to join 5 minutes before it starts. The last standing couple will win 1 DBScroll!");
                        AddOption("I see", 255);
                        break;
                    }
                case 2:
                    {
                        if (Features.TopSpouse.EventByPM || Features.TopSpouse._signup)
                        {
                            if (!Features.TopSpouse.CouplesWar)
                            {
                                if (GC.MyChar.MyTeam != null)
                                {
                                    if (GC.MyChar.MyTeam.Leader.EntityID == GC.MyChar.EntityID)
                                    {
                                        if (!Features.TopSpouse.SignupTeams.Contains(GC.MyChar.MyTeam))
                                        {
                                            Character Love = World.CharacterFromName2(GC.MyChar.Spouse);
                                            if (GC.MyChar.MyTeam.Members.Count == 2 && GC.MyChar.Spouse != null && GC.MyChar.MyTeam.Members.Contains(Love) || GC.PM)
                                            {
                                                Features.TopSpouse.SignupTeams.Add(GC.MyChar.MyTeam);
                                                GC.LocalMessage(2000, "Your team was succesfuly registered to the Couple's PK Tournament!");
                                            }
                                            else
                                                GC.LocalMessage(2000, "You must only team up with your lover.");
                                        }
                                        else
                                            GC.LocalMessage(2000, "Your team has already been registered!");
                                    }
                                    else
                                    {
                                        AddText("Only the team leader is able to register the team!");
                                        AddOption("I see", 255);
                                    }
                                }
                                else
                                    GC.LocalMessage(2000, "You must team up with your lover before joining.");
                            }
                            else
                                GC.LocalMessage(2000, "Couple's PK Tournament is already in progress!");
                        }
                        else
                        {
                            AddText("It's not the right time. Please team up with your lover and come later.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}