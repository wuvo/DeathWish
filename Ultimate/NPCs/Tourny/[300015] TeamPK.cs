using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    public class NPC_300015 : NPCBase
    {
        public NPC_300015(Main.GameClient _client)
            : base(_client)
        {
            ID = 10021;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Only 2 teams can go in (picked at random) from teams that join. Your team needs to be made of 3 players. Levels accepted (70-99, 100-115, 116-130).\n");
                    AddText("If your team does not meet the requirements it will not be picked to join the tournament!");
                    AddOption("Yes", 1);
                    AddOption("Just passing by", 255);
                    break;
                case 1:
                    AddText("Did you come to join the Team PK Tournament? Its disabled. The Emperor has said I'll have something new soon.?\n");
                    AddText("Every Saturday 18:10 you can enter this event.");
                    AddOption("Join my team (level 70-99)!", 2);
                    AddOption("Join my team (level 100-115)!", 3);
                    AddOption("Join my team (level 116-130)!", 4);
                    break;
                case 2:
                    {
                        if (!Features.TeamPKTourny.Started)
                        {
                            if (GC.MyChar.MyTeam != null)
                            {
                                if (GC.MyChar.MyTeam.Leader.EntityID == GC.MyChar.EntityID)
                                {
                                    if (!Features.TeamPKTourny.Queue70To99.Contains(GC.MyChar.MyTeam))
                                    {
                                        /* if (Features.TeamPKTourny.Queue70To99.Count == 1)
                                             Features.TeamPKTourny.JoinTime = DateTime.Now;*/
                                        if (GC.MyChar.MyTeam.Members.Count == 3 || GC.PM)
                                        {
                                            Features.TeamPKTourny.Queue70To99.Add(GC.MyChar.MyTeam);
                                            AddText("Your team was succesfuly added to the queue of the tournament (level 70 - 99).");
                                            AddOption("Thanks!", 255);
                                        }
                                        else
                                        {
                                            AddText("Your team has more or less than 3 members.");
                                            AddOption("Oh...", 255);

                                        }
                                    }
                                    else
                                    {
                                        AddText("You already placed your team in the queue.");
                                        AddOption("Thanks!", 255);


                                    }
                                }
                                else
                                {
                                    AddText("You are not the team leader! You can't join your team.");
                                    AddOption("Ok.", 255);

                                }
                            }
                            else
                            {
                                AddText("You don't have a team!");
                                AddOption("Ok.", 255);

                            }
                        }
                        else
                        {
                            AddText("Team PK Tourny is already in progress!");
                            AddOption("Ok.", 255);

                        }
                    }
                    break;

                case 3:

                    {
                        if (!Features.TeamPKTourny.Started)
                        {
                            if (GC.MyChar.MyTeam != null)
                            {
                                if (GC.MyChar.MyTeam.Leader.EntityID == GC.MyChar.EntityID)
                                {
                                    if (!Features.TeamPKTourny.Queue100To115.Contains(GC.MyChar.MyTeam))
                                    {
                                        /* if (Features.TeamPKTourny.Queue70To99.Count == 1)
                                             Features.TeamPKTourny.JoinTime = DateTime.Now;*/
                                        if (GC.MyChar.MyTeam.Members.Count == 3)
                                        {
                                            Features.TeamPKTourny.Queue100To115.Add(GC.MyChar.MyTeam);
                                            AddText("Your team was succesfuly added to the queue of the tournament (level 100 - 115).");
                                            AddOption("Thanks!", 255);

                                        }
                                        else
                                        {
                                            AddText("Your team has more or less than 3 members.");
                                            AddOption("Oh...", 255);

                                        }
                                    }
                                    else
                                    {
                                        AddText("You already placed your team in the queue.");
                                        AddOption("Thanks!", 255);


                                    }
                                }
                                else
                                {
                                    AddText("You are not the team leader! You can't join your team.");
                                    AddOption("Ok.", 255);

                                }
                            }
                            else
                            {
                                AddText("You don't have a team!");
                                AddOption("Ok.", 255);

                            }
                        }
                        else
                        {
                            AddText("Team PK Tourny is already in progress!");
                            AddOption("Ok.", 255);

                        }
                    }

                    break;

                case 4:
                    {
                        if (!Features.TeamPKTourny.Started)
                        {
                            if (GC.MyChar.MyTeam != null)
                            {
                                if (GC.MyChar.MyTeam.Leader.EntityID == GC.MyChar.EntityID)
                                {
                                    if (!Features.TeamPKTourny.Queue116To130.Contains(GC.MyChar.MyTeam))
                                    {
                                        /* if (Features.TeamPKTourny.Queue70To99.Count == 1)
                                             Features.TeamPKTourny.JoinTime = DateTime.Now;*/
                                        if (GC.MyChar.MyTeam.Members.Count == 1)
                                        {
                                            Features.TeamPKTourny.Queue116To130.Add(GC.MyChar.MyTeam);
                                            AddText("Your team was succesfuly added to the queue of the tournament (level 116 - 130).");
                                            AddOption("Thanks!", 255);

                                        }
                                        else
                                        {
                                            AddText("Your team has more or less than 3 members.");
                                            AddOption("Oh...", 255);

                                        }
                                    }
                                    else
                                    {
                                        AddText("You already placed your team in the queue.");
                                        AddOption("Thanks!", 255);


                                    }
                                }
                                else
                                {
                                    AddText("You are not the team leader! You can't join your team.");
                                    AddOption("Ok.", 255);
                                }
                            }
                            else
                            {
                                AddText("You don't have a team!");
                                AddOption("Ok.", 255);

                            }
                        }
                        else
                        {
                            AddText("Team PK Tourny is already in progress!");
                            AddOption("Ok.", 255);

                        }
                    }
                    break;

            }

            AddFinish();
            Send();
        }
    }

}
