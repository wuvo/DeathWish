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
    public class NPC_3299 : NPCBase
    {
        public NPC_3299(Main.GameClient _client)
            : base(_client)
        {
            ID = 3299;
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
                        if (GC.MyChar.DisCityMobs < GC.MyChar.DisToKill)
                        {
                            AddText("You'll pass if you kill " + GC.MyChar.DisToKill + " monsters in HellHall. Hurry up you got " + (GC.MyChar.DisToKill - GC.MyChar.DisCityMobs) + " monsters left to kill! Only 30 persons can geet to HellCloister with me.");
                            AddOption("I see", 255);
                        }
                        else
                        {
                            AddText("You've killed the " + GC.MyChar.DisToKill + " monsters, and you can pass to the HellCloister. Hurry up! What flank do you want to go in?");
                            AddOption("Left flank.", 1);
                            AddOption("Right flank.", 2);
                        }
                        break;
                    }
                case 1:
                    {
                        if (World.Dis3 < 30)
                        {
                            if (World.H_LeftFlank.Count < 15)
                            {
                                GC.MyChar.Teleport(2023, 297, 649);
                                if (!World.H_LeftFlank.ContainsKey(GC.MyChar.EntityID))
                                    World.H_LeftFlank.Add(GC.MyChar.EntityID, GC.MyChar);
                                if (GC.MyChar.Level < 130)
                                    GC.MyChar.AddExp(2);

                                World.Dis3 += 1;
                                World.LeftFlank += 1;
                                GC.MyChar.DisToKill = 0;
                                if (World.Dis3 < 4)
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has passed through the HellHall and has entered into the HellCloister, ranked " + World.Dis3 + "!", 2011, 0);
                                else
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has entered into the left flank of the HellCloister, ranked " + World.LeftFlank + "!", 2011, 0);
                            }
                            else
                            {
                                AddText("I'm sorry but this flank is full.");
                                AddOption("Oh damn...", 255);
                            }
                        }
                        else
                        {
                            foreach (Character C in World.H_Chars.Values)
                            {
                                if (C.Loc.Map == 2022)
                                {
                                    C.Teleport(1020, 566, 564);
                                    AddText("Sorry but all the 30 places were taken. Come back next time.");
                                    AddOption("Oh damn...", 255);
                                }
                            }
                            World.Dis3 = 0;
                            World.LeftFlank = 0;
                            World.RightFlank = 0;
                        }
                        break;
                    }
                case 2:
                    {
                        if (World.Dis3 < 30)
                        {
                            if (World.H_RightFlank.Count < 15)
                            {
                                GC.MyChar.Teleport(2023, 297, 649);
                                GC.MyChar.DisToKill = 0;
                                if (!World.H_RightFlank.ContainsKey(GC.MyChar.EntityID))
                                    World.H_RightFlank.Add(GC.MyChar.EntityID, GC.MyChar);
                                if (GC.MyChar.Level < 130)
                                    GC.MyChar.AddExp(2);
                                World.Dis3 += 1;
                                World.RightFlank += 1;
                                if (World.Dis3 < 4)
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has passed through the HellHall and has entered into the HellCloister, ranked " + World.Dis3 + "!", 2011, 0);
                                else
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has entered into the right flank of the HellCloister, ranked " + World.RightFlank + "!", 2011, 0);
                            }
                            else
                            {
                                AddText("I'm sorry but this flank is full.");
                                AddOption("Oh damn...", 255);
                            }
                        }
                        else
                        {
                            foreach (Character C in World.H_Chars.Values)
                            {
                                if (C.Loc.Map == 2022)
                                {
                                    C.Teleport(1020, 566, 564);
                                    AddText("Sorry but all the 30 places were taken. Come back next time.");
                                    AddOption("Oh damn...", 255);
                                }
                            }
                            World.Dis3 = 0;
                            World.LeftFlank = 0;
                            World.RightFlank = 0;
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}