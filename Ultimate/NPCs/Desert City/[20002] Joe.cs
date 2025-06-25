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
    public class NPC_20002 : NPCBase
    {
        public NPC_20002(Main.GameClient _client)
            : base(_client)
        {
            ID = 20002;
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
                        if (!GC.MyChar.InventoryContains(721001, 1))
                        {
                            AddText("This desert is killing me it's so hot!");
                            AddOption("Yes it is.", 255);
                            break;
                        }
                        else
                        {
                            AddText("I can tell you already have the Guardian Star! However I will ask you to get me a Meteor and an Amrita...the sun is killing me!");
                            AddOption("Take them", 1);
                            AddOption("I see", 255);
                            break;
                        }
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(1000030, 1) && GC.MyChar.InventoryContains(1088001, 1) && GC.MyChar.InventoryContains(721001, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1000030));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721001));
                            GC.MyChar.AddItem(721002);
                            AddText("Wow! I feel refreshed now! Here take this Sad Meteor and give it to Minner.");
                            AddOption("Ok!", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}