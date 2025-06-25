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
    public class NPC_50000 : NPCBase
    {
        public NPC_50000(Main.GameClient _client)
            : base(_client)
        {
            ID = 50000;
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
                        AddText("People are pursuing greather achievement during their lives, but none can make it due to the limit of human constitution.");
                        AddOption("What does it mean?", 1);
                        AddOption("I don't belive it.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Mortals are mundane. Only getting rid of it can help them accomplish their aims. If you are high level enough, you can get reborn to learn more and stronger skills.");
                        AddOption("How to get reborn?", 2);
                        AddOption("I am satisfied.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("It is difficult.First, you should reach an certain level. Second, you need a CelestialStone.");
                        AddOption("How to get CelestialStone?", 3);
                        AddOption("Forget it.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("CelestialStone is made of 7 gems: VioletGem, KylinGem, RainbowGem, MoonGem, PhoenixGem, FuryGem, DragonGem, and CleanWater.");
                        AddOption("What is a CleanWater?", 4);
                        AddOption("It is difficult.", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("It is used to get rid of your mundaneness, and then you won't be affected by the environment. By the way, Clean Water comes from celestial rinsing.");
                        AddOption("What are gems used for?", 5);
                        AddOption("Awww", 255);
                        break;
                    }
                case 5:
                    {
                        AddText("Only seven gems can protect you during the rebirth. Otherwise you wouldn't be able to survive it.");
                        AddOption("I will collect them now.", 6);
                        AddOption("I changed my mind.", 255);
                        break;
                    }
                case 6:
                    {
                        AddText("It is easy to get the gems. But CleanWater...");
                        AddOption("What about it?", 7);
                        break;
                    }
                case 7:
                    {
                        AddText("The Adventure island is the headstream of CleanWater. But it is occupied by WaterLord and he uses spell to hide the stream.");
                        AddOption("What can I do?", 8);
                        AddOption("I give up", 255);
                        break;
                    }
                case 8:
                    {
                        AddText("WaterLord will go to get the water every certain time. If you defeat him, you may get the water. But he is very hard to deal with.");
                        AddOption("Anything else?", 9);
                        AddOption("I see", 255);
                        break;
                    }
                case 9:
                    {
                        uint x = 700001;//gem id
                        if (GC.MyChar.InventoryContains(721258, 1) && GC.MyChar.InventoryContains(x, 1) && GC.MyChar.InventoryContains(x + 10, 1) && GC.MyChar.InventoryContains(x + 20, 1) && GC.MyChar.InventoryContains(x + 30, 1) && GC.MyChar.InventoryContains(x + 40, 1) && GC.MyChar.InventoryContains(x + 50, 1) && GC.MyChar.InventoryContains(x + 60, 1))
                        {
                            AddText("Are you sure you want to refine CelestialStone?");
                            AddOption("Sure!", 10);
                            AddOption("Nevermind", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        uint x = 700001;//gem id
                        if (GC.MyChar.InventoryContains(721258, 1) && GC.MyChar.InventoryContains(x, 1) && GC.MyChar.InventoryContains(x + 10, 1) && GC.MyChar.InventoryContains(x + 20, 1) && GC.MyChar.InventoryContains(x + 30, 1) && GC.MyChar.InventoryContains(x + 40, 1) && GC.MyChar.InventoryContains(x + 50, 1) && GC.MyChar.InventoryContains(x + 60, 1))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                RemoveItem(x);
                                x += 10;
                            }
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721258));
                            GC.MyChar.AddItem(721259);
                            AddText("Here you go! I wish you luck!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have the required items!");
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