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
    public class NPC_2021 : NPCBase
    {
        public NPC_2021(Main.GameClient _client)
            : base(_client)
        {
            ID = 2021;
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
                        AddText("Great to see you! I was wondering if you could help me out!");
                        AddOption("What's wrong?", 1);
                        AddOption("I've found the Rat Fang", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("I have been poisoned in my latest adventure. Now I need someone brave enough to kill those rats for one of their fangs! Can you go get one and bring it to me?");
                        AddOption("I'll try", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(721120, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721120));
                            if (MyMath.ChanceSuccess(5))
                            {
                                GC.MyChar.AddItem(722384);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " has delivered a RatFang to OldManTang in PhoenixCastle and won a ProficiencyToken!", 2005, 0);
                                AddText("Congratulations! You have won a ProficiencyToken! Thanks for the help!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(721080);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " has delivered a RatFang to OldManTang in PhoenixCastle and won a MoonBox!", 2011, 0);
                                AddText("Thank you! Here's a MeteorScroll!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " has delivered a RatFang to OldManTang in PhoenixCastle and won a Dragonball!", 2005, 0);
                                AddText("Thank you! Here is a DragonBall!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(20))
                            {
                                GC.MyChar.AddItem(721258);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " has delivered a RatFang to OldManTang in PhoenixCastle and won a CleanWater!", 2005, 0);
                                AddText("Thank you! Here is a CleanWater!");
                                AddOption("Thanks!", 255);
                            }
                            else if (MyMath.ChanceSuccess(60))
                            {
                                for (int a = 0; a < 2; a++)
                                    GC.MyChar.AddItem(720027);
                                GC.MyChar.Silvers += 30000;
                                AddText("Thank you! Here's your reward!");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                GC.MyChar.AddItem(1088000);
                                Game.World.SendMsgToAll("SYSTEM", " " + GC.MyChar.Name + " has delivered a RatFang to OldManTang in PhoenixCastle and won a Dragonball!", 2005, 0);
                                AddOption("Thanks!", 255);
                            }
                        }
                        else
                        {
                            AddText("Are you trying to fool me? You don't have the Rat Fang!");
                            AddOption("I'm sorry", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}