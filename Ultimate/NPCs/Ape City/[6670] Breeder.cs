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
    public class NPC_6670 : NPCBase
    {
        public NPC_6670(Main.GameClient _client)
            : base(_client)
        {
            ID = 6670;
            Face = 28;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.AC_Quest_Hops && GC.MyChar.AC_Quest_Hops_Completed)
                        {
                            AddText("Oh, you are finished finally! Quickly give me the hops so I can make my alcohol.");
                            AddOption("Okay...", 33);
                        }
                        else
                        {
                            AddText("Hen Breeding is such a stressful job! That's why in my spare time I use hops to make alcohol.");
                            AddOption("Uhh... Okay...", 1);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Hops are hard to come by though, and I drink a lot. I mean, a LOT. Do you think you could find some for me?");
                        AddOption("Yes.", 10);
                        AddOption("No thanks.", 255);
                        break;
                    }
                case 10:
                    {
                        if (GC.MyChar.AC_Quest_Hops)
                        {
                            AddText("I already gave you a job to do! Come back when your finished. Type @quests to check your status. Try killing something!");
                            AddOption("Okay.", 255);
                        }
                        else
                        {
                            GC.MyChar.AC_Quest_Hops = true;
                            GC.MyChar.AC_Quest_Hops_Completed = false;
                            AddText("Collect 5 Hops from Macaque, MacaqueL48, GiantApe, GiantApeL53, ThunderApe or ThunderApeL58 then come see me. They all have hops. I need my alcohol.");
                            AddOption("Ok I'll do it.", 255);
                        }
                        break;
                    }
                case 33:
                    {
                        if (GC.MyChar.InventoryContains(729933, 5))
                        {
                            for (int i = 0; i < 5; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(729933));
                            if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(722384);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Breeder in Ape Mountain and won a ProficiencyToken!", 2005, 0);
                                AddText("Congratulations! You have won a ProficiencyToken! Thanks for the help!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(721080);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Breeder in Ape Mountain and won a MoonBox!", 2011, 0);
                                AddText("Thank you! Here's a MoonBox!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(30))
                            {
                                GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Breeder in Ape Mountain and won a Dragonball!", 2005, 0);
                                AddText("Thank you! Here is a DragonBall!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(721258);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " finished a quest for Breeder in Ape Mountain and won a CleanWater!", 2005, 0);
                                AddText("Thank you! Here is a CleanWater!");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                GC.MyChar.AddItem(720027);
                                AddText("Thank you! Here is a MeteorScroll!");
                                AddOption("Thanks!", 255);
                            }
                            GC.MyChar.AC_Quest_Hops_Completed = false;
                            GC.MyChar.AC_Quest_Hops = false;
                        }
                        else
                        {
                            AddText("Are you trying to trick me? BRING ME MY HOPS!");
                            AddOption("Opps hold on!", 255);
                        }
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}