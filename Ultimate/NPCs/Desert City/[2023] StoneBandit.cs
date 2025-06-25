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
    public class NPC_2023 : NPCBase
    {
        public NPC_2023(Main.GameClient _client)
            : base(_client)
        {
            ID = 2023;
            Face = 30;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Will you help me?");
                        AddOption("With what?", 1);
                        AddOption("I have the Broken Sword", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("I need you to kill Stone Bandits until they drop my broken sword, then bring me it and I will reward you.");
                        AddOption("Count me in", 255);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(721128, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721128));
                            GC.MyChar.Silvers += 150000;
                            AddText("Great you have it.  Here is your reward!");
                            AddOption("Thanks!", 255);
                        }
                        else
                        {
                            AddText("You do not have the required item");
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