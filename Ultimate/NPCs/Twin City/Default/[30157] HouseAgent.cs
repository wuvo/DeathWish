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
    public class NPC_30157 : NPCBase
    {
        public NPC_30157(Main.GameClient _client)
            : base(_client)
        {
            ID = 30157;
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
                        AddText("I am the HouseAgent in Twin City. If you want to buy a house, you must have my authorization.");
                        AddOption("Buy a house.", 1);
                        AddOption("Upgrade my house.", 4);
                        AddOption("Just passing by.", 255);
                        GC.Agreed = false;
                        break;
                    }
                case 1:
                    {
                        AddText("You should make an effort to buy a house.");
                        AddOption("What shall I do?", 2);
                        AddOption("I changed my mind.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("I can give you an HousePermit if you bring me 5 TimberVouchers!");
                        AddOption("Here are the vouchers.", 3);
                        AddOption("I shall come later.", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(721173, 5))
                        {
                            for (int i = 0; i < 5; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721173));
                            GC.MyChar.AddItem(721170);
                            AddText("Congratulations! You now have a HousePermit!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Sorry you dont have 5 Timber Vouchers.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        AddText("Which class would you like to upgrade your house to?");
                        AddOption("Second-class.", 5);
                        AddOption("Let me think it over.", 255);
                        break;
                    }
                case 5:
                    {
                        AddText("I can give you an UpgradeCertificate if you bring me 10 OreVouchers!");
                        AddOption("Here are the vouchers.", 6);
                        AddOption("I shall come later.", 255);
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.InventoryContains(721176, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(721176));
                            GC.MyChar.AddItem(721174);
                            AddText("Congratulations! Make sure you make a good use of that Upgrade Certificate!");
                            AddOption("Thanks", 255);
                            break;
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have 10 Ore Vouchers!");
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