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
    public class NPC_19001 : NPCBase
    {
        public NPC_19001(Main.GameClient _client)
            : base(_client)
        {
            ID = 19001;
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
                        AddText("Hello there. I am looking for euxenite ores. If you can give me 10 euxenite ores or 2,500,000 silvers I will give you 5 pieces of Saltpeter.");
                        AddOption("I have 10 euxenite ores.", 1);
                        AddOption("I have 5,000,000 silvers", 2);
                        AddOption("Just passing by...", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(1072031, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1072031));
                            for (int i = 0; i < 5; i++)
                                GC.MyChar.AddItem(721262);
                            AddText("There you go! Use the Saltpeter wisely!");
                            AddOption("Thanks!", 255);
                        }
                        else
                        {
                            AddText("You don't have 10 euxenite ores! I can't give you 5 pieces of Saltpeter!");
                            AddOption("Oh...", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.Silvers >= 2500000 && GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.Silvers -= 2500000;
                            for (int i = 0; i < 5; i++)
                                GC.MyChar.AddItem(721262);
                            AddText("There you go! Use the Saltpeter wisely!");
                            AddOption("Thanks!", 255);
                        }
                        else
                        {
                            AddText("You don't have 2,500,000 silvers or you don't have 5 spaces in your bag! I can't give you 5 pieces of Saltpeter!");
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