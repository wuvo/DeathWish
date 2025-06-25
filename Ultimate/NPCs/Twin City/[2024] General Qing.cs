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
    public class NPC_2024 : NPCBase
    {
        public NPC_2024(Main.GameClient _client)
            : base(_client)
        {
            ID = 2024;
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
                        AddText("Hello.  I have lost my Army Token and it has brung shame to my family.  This is my only chance to redeem myself.  Can you retrieve it?");
                        AddOption("Count me in", 255);
                        AddOption("I got the Army Token", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721117, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721117));
                            GC.MyChar.AddItem(420088);
                            AddText("Wow, you actually did it.  You have brung great honor today young boy.  Thanks so much.  Here is your reward.");
                            AddOption("Thanks!", 255);
                        }
                        else
                        {
                            AddText("You do not have the required item");
                            AddOption("Whatever", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}