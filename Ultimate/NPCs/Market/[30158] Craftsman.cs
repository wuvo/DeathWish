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
    public class NPC_30158 : NPCBase
    {
        public NPC_30158(Main.GameClient _client)
            : base(_client)
        {
            ID = 30158;
            Face = 92;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("What can I do for you!");
                        AddOption("Do you have Timber?", 1);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("We are rebuilding the market and have stored a lot of Timber.");
                        AddOption("Can you give me some?", 2);
                        AddOption("You have wasted too much", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Well, we are in an urgent need of Iron Ores and nothing comes for free. You may exchange 5 Iron Ores for a piece of Timber.");
                        AddOption("Here are 5 Iron Ores", 3);
                        AddOption("I shall come later", 255);
                        break;
                    }
                case 3:
                    {
                        var Amount = 0;
                        for (int a = 0; a < 10; a++)
                            Amount += GC.MyChar.InventoryItemIDCount((uint)(1072010 + a));

                        if (Amount >= 5)
                        {
                            Amount = 5;
                            for (int a = 0; a < 10; a++)
                            {
                                var Count = GC.MyChar.InventoryItemIDCount((uint)(1072010 + a));
                                for (int b = 0; b < Count; b++)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem((uint)(1072010 + a)));
                                    Amount--;
                                    if (Amount == 0)
                                        break;
                                }
                                if (Amount == 0)
                                    break;
                            }
                            GC.MyChar.AddItem(721171);
                            AddText("Congratulations ! You have exchanged 5 Iron Ores for a piece of Timber !");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it looks like you don't have 5 Iron Ores!");
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