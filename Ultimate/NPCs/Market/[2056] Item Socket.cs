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
    public class NPC_550 : NPCBase
    {
        public NPC_550(Main.GameClient _client)
            : base(_client)
        {
            ID = 550;
            Face = 5;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("It's a chaotic world... rich steal from poor... bla bla bla. I am the great item socketer.");
                        AddText("It will cost you 1 PlatinumGem and 2,000,000 silvers to create the first socket.");
                        AddOption("I want to create a first socket.", 1);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Prepare break and 2,000.000 silvers for the socket and choose the equipment you want the socket to be created in.");
                        AddOption("Headgear", (byte)(_linkback * 100 + 1));
                        AddOption("Necklace/Bag", (byte)(_linkback * 100 + 2));
                        AddOption("Armor/Coat/Vest/Robe", (byte)(_linkback * 100 + 3));
                        AddOption("Shield", (byte)(_linkback * 100 + 5));
                        AddOption("Ring/Bracellet", (byte)(_linkback * 100 + 6));
                        AddOption("Boots", (byte)(_linkback * 100 + 8));
                        break;
                    }
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                    {
                        byte Pos = (byte)(_linkback - 100);
                        Game.Item Eq = GC.MyChar.Equips.Get(Pos);
                        if (Eq.ID != 0)
                        {
                            if (Eq.Soc1 == Ultimate.Game.Item.Gem.NoSocket)
                            {
                                if (GC.MyChar.Silvers >= 2000000 && GC.MyChar.InventoryContains(1088901, 1))
                                {
                                    GC.MyChar.EquipStats((byte)(_linkback - 100), false, false);
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088901));
                                    GC.MyChar.Silvers -= 2000000;
                                    Eq.Soc1 = Ultimate.Game.Item.Gem.EmptySocket;
                                    GC.MyChar.Equips.Replace(Pos, Eq, GC.MyChar);
                                    GC.MyChar.EquipStats((byte)(_linkback - 100), true, false);
                                    Database.SaveCharacter(GC.MyChar, GC.MyChar.MyClient.AuthInfo.Account);
                                    AddText("Congratulations! You now have the first socket in your equipment.");
                                    AddOption("I see.", 255);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " Congratulations! he now have the first socket in your equipment", 2011, 0);

                                }
                                else
                                {
                                    AddText("You don't have the materials.");
                                    AddOption("I see.", 255);

                                }
                            }
                            else
                            {
                                AddText("The item already has the first socket.");
                                AddOption("Oh, right.", 255);

                            }
                        }
                        else
                        {
                            AddText("You don't have any equipment in that slot.");
                            AddOption("I see.", 255);
                        }
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}