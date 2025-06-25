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
    public class NPC_30162 : NPCBase
    {
        public NPC_30162(Main.GameClient _client)
            : base(_client)
        {
            ID = 30162;
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
                        AddText("I am here to issue all kinds of vouchers. Are you here for the Vouchers?");
                        AddOption("Yeah.", 1);
                        AddOption("Just passing by.", 255);
                        GC.Agreed = false;
                        break;
                    }
                case 1:
                    {
                        AddText("I can issue 1 Rosewood Voucher for 10 pieces of Timber, and 1 Timber Voucher for 10 Rosewood Vouchers. What do you need?");
                        AddOption("Rosewood Voucher.", 2);
                        AddOption("Timber Voucher.", 3);
                        AddOption("I changed my mind.", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(721171, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721171));
                            GC.MyChar.AddItem(721172);
                            AddText("Congratulations! You have exchanged 1 Rosewood Voucher for 10 pieces of Timber.");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Sorry you dont have 10 pieces of Timber. I heard that Craftsman is supplying Timber.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(721172, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721172));
                            GC.MyChar.AddItem(721173);
                            AddText("Congratulations! You have exchanged 1 Timber Voucher for 10 Rosewood Vouchers.");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Sorry you dont have 10 Rosewood Vouchers.");
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