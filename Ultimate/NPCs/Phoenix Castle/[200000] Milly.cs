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
    public class NPC_20000 : NPCBase
    {
        public NPC_20000(Main.GameClient _client)
            : base(_client)
        {
            ID = 20000;
            Face = 3;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello stranger. I haven't seen my sister for a while and I got a letter for her would you like to deliver it for me please?");
                        AddOption("Yes, bet on me.", 1);
                        AddOption("Sorry, I'm busy.", 255);
                        break;
                    }
                case 1:
                    {
                        if (!GC.MyChar.InventoryContains(721000, 1) && !GC.MyChar.InventoryContains(721001, 1))
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.AddItem(721000);
                                AddText("Here! Take this letter and meet my sister Minner in Bird Island.");
                                AddOption("Ok.", 255);
                                break;
                            }
                            else
                            {
                                AddText("I'm sorry but your inventory is full..");
                                AddOption("Ah, I see.", 255);
                                break;
                            }
                        }
                        else
                        {
                            AddText("I already gave you the letter. Go see my sister in Bird Island.");
                            AddOption("Ok ok.", 255);
                            break;
                        }
                    }
            }

            AddFinish();
            Send();
        }
    }
}