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
    public class NPC_19000 : NPCBase
    {
        public NPC_19000(Main.GameClient _client)
            : base(_client)
        {
            ID = 19000;
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
                        AddText("Hello there. In order to make a bomb I need 5 pieces of Saltpeter, 1 piece of Sulphur and 50,000 silvers. Old Quarrier in Ape City can help you with the Saltpeter, as for the Sulphur, the Caterans over here drop them.");
                        AddOption("I have all materials", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721263, 1) && GC.MyChar.InventoryContains(721262, 5))
                        {
                            if (GC.MyChar.Silvers >= 50000)
                            {
                                GC.MyChar.Silvers -= 50000;
                                for (int i = 0; i < 5; i++)
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(721262));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721263));
                                GC.MyChar.AddItem(721261);
                                AddText("There you go! Take care of my bomb and use it carefully!");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                AddText("You don't have 50,000 silvers with you! I can't make the bomb for you!");
                                AddOption("Oh...", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have 5 pieces of Saltpeter and 1 piece of Sulphur. I can't make the bomb for you!");
                            AddOption("Oh...", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}