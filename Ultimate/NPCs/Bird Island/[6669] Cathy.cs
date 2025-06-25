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
    public class NPC_6669 : NPCBase
    {
        public NPC_6669(Main.GameClient _client)
            : base(_client)
        {
            ID = 6669;
            Face = 28;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            bool questcomplete = false;
            if ((GC.MyChar.BI_Quest_Kills >= 15000) && GC.MyChar.BI_Quest != 0)
            {
                questcomplete = true;
            }
            switch (_linkback)
            {
                case 0:
                    {
                        if (questcomplete)
                        {
                            AddText("You're back! Are you finished already?");
                            AddOption("I'm finished!", 10);
                        }
                        else
                        {
                            AddText("We are being overrun with monsters! Please help us purify our beautiful islands. I'll give you a nice reward if you do a job for me. Interested?");
                            AddOption("Yes, tell me.", 2);
                        }
                        AddOption("No thanks.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Kill 15,000 monsters for me, Birdmen Hawkings or BanditL97s. You can mix them up if you want to!");
                        AddOption("Yes I'll do it.", 3);
                        AddOption("No thanks.", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.BI_Quest != 1)
                        {
                            GC.MyChar.BI_Quest = 1;
                            GC.MyChar.BI_Quest_Kills = 0;
                            AddText("Thank you! Type @quests to check your status and come see me when you're done. Hurry! If you disconnect, you have to start again!");
                            AddOption("Okay.", 255);
                        }
                        else
                        {
                            AddText("I already gave you a job to do! type @quests to check your status and come see me when you're done. Hurry!");
                            AddOption("Okay.", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        if ((GC.MyChar.BI_Quest_Kills >= 15000) && GC.MyChar.BI_Quest != 0 && GC.MyChar.Inventory.Count <= 39)
                        {
                            GC.MyChar.BI_Quest = 0;
                            GC.MyChar.BI_Quest_Kills = 0;
                            if (MyMath.ChanceSuccess(1))
                            {
                                GC.MyChar.AddItem(722384);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won a ProficiencyToken!", 2005, 0);
                                Game.World.DebugAdd += GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won a ProficiencyToken! \r\n";
                                AddText("Congratulations! You have won a ProficiencyToken! Thanks for the help!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                GC.MyChar.AddItem(721080);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won a MoonBox!", 2011, 0);
                                AddText("Thank you! Here's a MoonBox!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                for (int a = 0; a < 2; a++)
                                    GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won 2x DragonBall!", 2005, 0);
                                AddText("Thank you! Here is a DragonBall!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(15))
                            {
                                GC.MyChar.AddItem(721258);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won a CleanWater!", 2005, 0);
                                AddText("Thank you! Here is a CleanWater!");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                    GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Cathy in Bird Island and won a DragonBall!", 2005, 0);
                                AddText("Thank you! Here is a DragonBall!");
                                AddOption("Thanks!", 255);
                            }
                        }
                        else
                        {
                            AddText("Please clear some room in your inventory first!");
                            AddOption("Okay.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}