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
    public class NPC_41 : NPCBase
    {
        public NPC_41(Main.GameClient _client)
            : base(_client)
        {
            ID = 41;
            Face = 10;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("A weapon without socket is just a weapon. But if you add sockets to your weapon, you can put gems in it. Want to socket a weapon?");
                        AddOption("Alright. I'm in.", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("You need 1 DragonBall for first socket and 5 DragonBalls for the second socket.");
                        AddOption("Let's do this.", 2);
                        AddOption("What the hell will you do with all these dragonballs?", 10);
                        break;
                    }
                case 2:
                    {
                        Game.Item I = GC.MyChar.Equips.RightHand;
                        if (I.ID != 0)
                        {
                            if (I.Soc1 == Ultimate.Game.Item.Gem.NoSocket)
                            {
                                AddText("So you will need 1 DragonBall. You ready?");
                                AddOption("Ready as ever", 3);
                                AddOption("Let me think it over", 255);
                            }
                            else if (I.Soc2 == Ultimate.Game.Item.Gem.NoSocket)
                            {
                                AddText("So you will need 5 DragonBalls. You ready?");
                                AddOption("Ready as ever", 3);
                                AddOption("Let me think it over", 255);
                            }
                            else
                            {
                                AddText("I'm sorry but an item can only have a maximum of 2 sockets.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't even have a weapon equipped.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        Game.Item I = GC.MyChar.Equips.RightHand;
                        if (I.ID >= 410003 && I.ID <= 601339)
                        {
                            if (I.ID != 0)
                            {
                                if (I.Soc1 == Ultimate.Game.Item.Gem.NoSocket)
                                {
                                    if (GC.MyChar.InventoryContains(1088000, 1))
                                    {
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                        GC.MyChar.EquipStats(4, false, false);
                                        GC.MyChar.Equips.RightHand.Soc1 = Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                        GC.MyChar.EquipStats(4, true, false);
                                        AddText("It was a success! But it's not like I ever fail anyways.");
                                        AddOption("Cool. Thanks.", 255);
                                    }
                                }
                                else if (I.Soc2 == Ultimate.Game.Item.Gem.NoSocket)
                                {
                                    if (GC.MyChar.InventoryContains(1088000, 5))
                                    {
                                        for (int i = 0; i < 5; i++)
                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                        GC.MyChar.EquipStats(4, false, false);
                                        GC.MyChar.Equips.RightHand.Soc2 = Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.RightHand, 4));
                                        GC.MyChar.EquipStats(4, true, false);
                                        AddText("It was a success! But it's not like i ever fail anyways.");
                                        AddOption("Cool. Thanks.", 255);
                                    }
                                }
                                else
                                {
                                    AddText("Is your weapon somekind of magic weapon, because it just wasn't 2 socketed?");
                                    AddOption("Maybe...", 255);
                                }
                            }
                            else
                            {
                                AddText("What the ****, where did you put your weapon? Stop playing tricks on me!");
                                AddOption("Oops, sorry.", 255);
                            }
                        }
                        else
                        {
                            AddText("That item does not appear to be a valid weapon. If you think this is an error, please report it on the forums.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        AddText("Well..... I don't know, I just want them because they're shiny!");
                        AddOption("Yeah, didn't think a socket would need them.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}