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
    public class NPC_20006 : NPCBase
    {
        public NPC_20006(Main.GameClient _client)
            : base(_client)
        {
            ID = 20006;
            Face = 15;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.Reborns > 0)
                        {
                            AddText("The DragonBall is a really precious item. You can use it to redistribute your attribute points after you have been reborned.");
                            AddText("I can help you redistribute them if you are reborned and L70(+) and you'll just have to pay me a DragonBall.");
                            AddOption("Redistribute my points", 1);
                            AddOption("Let me think it voer", 255);
                        }
                        else
                        {
                            AddText("You're either not reborned or level 70 yet.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Alright, as I said earlier, I will need a Dragonball in order to help you!");
                        AddOption("Here it is", 2);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.Reborns > 0)
                        {
                            if (GC.MyChar.InventoryContains(1088000, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                int AllAtributes = GC.MyChar.Str +
                                    GC.MyChar.Agi +
                                    GC.MyChar.Spi +
                                    GC.MyChar.Vit + GC.MyChar.StatPoints;
                                GC.MyChar.Str = 0;
                                GC.MyChar.Agi = 0;
                                GC.MyChar.Spi = 0;
                                GC.MyChar.Vit = 1;
                                GC.MyChar.StatPoints = (ushort)(AllAtributes - 1);
                                GC.MyChar.CurHP = 1;
                                AddText("Here you go, it is done!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("You don't have a DragonBall.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You're either not reborned or level 70 yet.");
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