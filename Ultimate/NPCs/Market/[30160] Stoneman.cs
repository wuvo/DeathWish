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
    public class NPC_30160 : NPCBase
    {
        public NPC_30160(Main.GameClient _client)
            : base(_client)
        {
            ID = 30160;
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
                        AddText("I am issuing all kinds of vouchers. What vouchers do you want?");
                        AddOption("Copper Ore Voucher.", 1);
                        AddOption("Ore Voucher", 2);
                        GC.Agreed = false;
                        break;
                    }
                case 1:
                    {
                        AddText("To obtain a Copper Ore Voucher, 12 Copper Ores will be charged. Are you ready for that?");
                        AddOption("Yeah. Here you are.", 3);
                        AddOption("No. I shall come later.", 255);
                        break;
                    }
                case 3:
                    {
                        var Amount = 0;
                        for (int a = 0; a < 10; a++)
                            Amount += GC.MyChar.InventoryItemIDCount((uint)(1072020 + a));

                        if (Amount >= 12)
                        {
                            Amount = 12;
                            for (int a = 0; a < 10; a++)
                            {
                                var Count = GC.MyChar.InventoryItemIDCount((uint)(1072020 + a));
                                for (int b = 0; b < Count; b++)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem((uint)(1072020 + a)));
                                    Amount--;
                                    if (Amount == 0)
                                        break;
                                }
                                if (Amount == 0)
                                    break;
                            }
                            GC.MyChar.AddItem(721175);
                            AddText("Congratulations ! You have exchanged a 12 Copper Ores for one Copper Ore Voucher !");
                            AddOption("Thanks", 255);
                            break;
                        }
                        else
                        {
                            AddText("I'm sorry but it seems like you don't have 12 Copper Ores.");
                            AddOption("I see", 255);
                            break;
                        }
                    }
                case 2:
                    {
                        AddText("To obtain an Ore Voucher, 10 Copper Ore Vouchers will be charged. Are you ready for that?");
                        AddOption("Yeah. Here you are.", 4);
                        AddOption("No. I shall come later.", 255);
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.InventoryContains(721175, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721175));
                            GC.MyChar.AddItem(721176);
                            AddText("Congratulations! You have exchanged an Ore Voucher for 10 Copper Ore Vouchers.");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Sorry you dont have 10 Copper Ore Vouchers.");
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