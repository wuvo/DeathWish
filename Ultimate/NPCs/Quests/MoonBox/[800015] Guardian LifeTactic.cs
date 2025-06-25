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
    public class NPC_800015 : NPCBase
    {
        public NPC_800015(Main.GameClient _client)
            : base(_client)
        {
            ID = 800015;
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
                        AddText("I can take you to the Life Tactic if you have all the 6 tokens, otherwise you'll have to die.");
                        AddOption("Ok. Take me there!", 1);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(721010, 1) && GC.MyChar.InventoryContains(721011, 1) && GC.MyChar.InventoryContains(721012, 1) && GC.MyChar.InventoryContains(721013, 1) && GC.MyChar.InventoryContains(721014, 1) && GC.MyChar.InventoryContains(721015, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721010));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721011));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721012));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721013));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721014));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721015));
                            GC.MyChar.Teleport(1050, 207, 159);
                            GC.MyChar.AddItem(721072);
                        }
                        else
                        {
                            AddText("You don't have all the tokens and you can't go to life tactic so you will be forced to die.");
                            AddOption("Damn!", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}