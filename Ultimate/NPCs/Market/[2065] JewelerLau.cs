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
    public class NPC_2065 : NPCBase
    {
        public NPC_2065(Main.GameClient _client)
            : base(_client)
        {
            ID = 2065;
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
                        AddText("Wanna compose some gems together? Give me 15 Normal gems and 10,000 silvers and I'll mix them into Refined one. And for Refined to Super, it costs 15 Refined gems, 1 DragonBall and 1,000,000 silvers.");
                        AddOption("Compose refined gems", 1);
                        AddOption("Compose super gems", 2);
                        AddOption("No.", 255);
                        break;
                    }
                case 1:
                case 2:
                    {
                        AddText("Choose the gem you want to compose.");
                        AddOption("Phoenix Gem", (byte)(_linkback * 10 + 0));
                        AddOption("Dragon Gem", (byte)(_linkback * 10 + 1));
                        AddOption("Fury Gem", (byte)(_linkback * 10 + 2));
                        AddOption("Rainbow Gem", (byte)(_linkback * 10 + 3));
                        AddOption("Kylin Gem", (byte)(_linkback * 10 + 4));
                        AddOption("Violet Gem", (byte)(_linkback * 10 + 5));
                        AddOption("Moon Gem", (byte)(_linkback * 10 + 6));
                        AddOption("Next.", (byte)(100 * _linkback));
                        break;
                    }
                case 100:
                case 200:
                    {
                        _linkback = (byte)(_linkback / 100);
                        AddText("Choose the gem you want to compose.");
                        AddOption("Tortoise Gem", (byte)(_linkback * 10 + 7));
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    {
                        uint ItemID = (uint)((_linkback - 10) * 10 + 700001);
                        if (GC.MyChar.Silvers >= 10000)
                        {
                            if (GC.MyChar.InventoryContains(ItemID, 15))
                            {
                                for (byte i = 0; i < 15; i++)
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(ItemID));
                                GC.MyChar.AddItem((uint)(ItemID + 1));
                                GC.MyChar.Silvers -= 10000;
                            }
                            else
                            {
                                AddText("You don't have enough gems.");
                                AddOption("I see.", 255);
                                break;
                            }
                        }
                        else
                        {
                            AddText("You don't have 10.000 gold.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                    {
                        uint ItemID = (uint)((_linkback - 20) * 10 + 700002);
                        if (GC.MyChar.InventoryContains(1088000, 1))
                        {
                            if (GC.MyChar.Silvers >= 1000000)
                            {
                                if (GC.MyChar.InventoryContains(ItemID, 15))
                                {
                                    for (byte i = 0; i < 15; i++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(ItemID));
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                    GC.MyChar.AddItem((uint)(ItemID + 1));
                                    if (MyMath.ChanceSuccess(0.3))
                                    {
                                        GC.MyChar.AddItem(700073);
                                        GC.LocalMessage(2000, "Congratulations you received a bonus Super Tortoise Gem from composing refined gems!");
                                        World.SendMsgToAll("SYSTEM", "Lucky player " + GC.MyChar.Name + " has received a bonus Super Tortoise Gem from composing refined gems!", 2011, 0);
                                    }
                                    GC.MyChar.Silvers -= 1000000;
                                }
                                else
                                {
                                    AddText("You don't have enough gems.");
                                    AddOption("I see.", 255);
                                }
                            }
                            else
                            {
                                AddText("You don't have 1,000,000 gold.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have a DragonBall.");
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