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
    public class NPC_35015 : NPCBase
    {
        public NPC_35015(Main.GameClient _client)
            : base(_client)
        {
            ID = 35015;
            Face = 59;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello there, you have probably heard of me already, I'm the Etheral, known by having great skills on upgrading equipment extra bonuses. If you want help with it, you're talking to the best. How can I help you?");
                        AddOption("Improve blessing attributes", 9);
                        AddOption("Just passing by.", 255);
                        GC.Agreed = false;
                        break;
                    }
                case 9:
                    {
                        AddText("Choose the equipment you want to set bless.");
                        AddOption("Headgear", 1);
                        AddOption("Necklace/Bag", 2);
                        AddOption("Armor", 3);
                        AddOption("Weapon", 4);
                        AddOption("Shield", 5);
                        AddOption("Ring", 6);
                        AddOption("Boots", 8);
                        break;
                    }
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    {
                        Game.Item I = GC.MyChar.Equips.Get((byte)(_linkback));
                        if (I.Bless != 5)
                        {
                            if (I.Bless != 0)
                            {
                                byte TortoiseNeed = 0;
                                if (I.Bless == 1)
                                    TortoiseNeed = 1;
                                else
                                    TortoiseNeed = 3;
                                if (!GC.Agreed)
                                {
                                    AddText("You need " + TortoiseNeed + " Super Tortoises to upgrade. Do you want it?");
                                    AddText("Your item current bless is " + I.Bless + ".");
                                    if (I.Bless != 0)
                                        AddText("It will be " + (I.Bless + 2) + ".");
                                    AddOption("Yes.", Convert.ToByte(_linkback));
                                    AddOption("Nevermind.", 255);
                                    GC.Agreed = true;
                                }
                                else
                                {
                                    GC.Agreed = false;
                                    if (GC.MyChar.InventoryContains(700073, TortoiseNeed))
                                    {
                                        for (byte i = 0; i < TortoiseNeed; i++)
                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700073));
                                        GC.MyChar.EquipStats((byte)(_linkback), false, false);
                                        I.Bless += 2;
                                        GC.MyChar.Equips.Replace((byte)(_linkback), I, GC.MyChar);
                                        GC.MyChar.EquipStats((byte)(_linkback), true, false);
                                        AddText("Here you are. It's done.");
                                        AddOption("Thanks.", 255);
                                    }
                                    else
                                    {
                                        AddText("You don't have enough Tortoise Gems.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("You cannot upgrade an non blessed item.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You cannot upgrade an item's bless which is already at maximum.");
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