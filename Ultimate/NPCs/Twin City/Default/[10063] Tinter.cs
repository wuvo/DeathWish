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
    public class NPC_10063 : NPCBase
    {
        public NPC_10063(Main.GameClient _client)
            : base(_client)
        {
            ID = 10063;
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
                        AddText("Are you not satisfied with your current armor, headgear or shield color? If not, then come in to our shop, that man inside will give you the service. Just give me one meteor.");
                        AddOption("Yeah, I want to dye my equipment.", 1);
                        AddOption("Nah, I like my colors, leave me alone!", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(1088001, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                            GC.MyChar.Teleport(1008, 22, 26);
                        }
                        else
                        {
                            AddText("No meteor, no entry.");
                            AddOption("Weird, I was sure I had brought one with me.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_10064 : NPCBase
    {
        public NPC_10064(Main.GameClient _client)
            : base(_client)
        {
            ID = 10064;
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
                        AddText("Great! A customer, so do you want to dye your equipment?");
                        AddOption("Dye my armor please.", 1);
                        AddOption("Dye my headgear please.", 2);
                        if (GC.MyChar.Equips.LeftHand.ID != 0 && Game.ItemIDManipulation.Part(GC.MyChar.Equips.LeftHand.ID, 0, 3) == 900)
                            AddOption("Dye my shield please.", 3);
                        break;
                    }
                case 1:
                case 2:
                case 3:
                    {
                        AddText("Choose the color.");
                        AddOption("Orange", (byte)(_linkback * 10 + 3));
                        AddOption("Light Blue", (byte)(_linkback * 10 + 4));
                        AddOption("Red", (byte)(_linkback * 10 + 5));
                        AddOption("Blue", (byte)(_linkback * 10 + 6));
                        AddOption("Yellow", (byte)(_linkback * 10 + 7));
                        AddOption("Purple", (byte)(_linkback * 10 + 8));
                        AddOption("White", (byte)(_linkback * 10 + 9));
                        AddOption("I've changed my mind.", 255);
                        break;
                    }
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                    {
                        if (GC.MyChar.Equips.Armor.ID == 0)
                        {
                            AddText("You don't have an armor equipped. What am i gonna dye, your body?");
                            AddOption("Noo, don't do that!", 255);
                        }
                        else
                        {
                            GC.MyChar.Equips.Armor.Color = (Game.Item.ArmorColor)(_linkback - 10);
                            GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.Armor, 3));
                        }
                        break;
                    }
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    {
                        if (GC.MyChar.Equips.HeadGear.ID == 0)
                        {
                            AddText("You don't have any headgear equipped. I'm no hair dyer, so put something on your head.");
                            AddOption("Noo, don't do that!", 255);
                        }
                        else
                        {
                            GC.MyChar.Equips.HeadGear.Color = (Game.Item.ArmorColor)(_linkback - 20);
                            GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.HeadGear, 1));
                        }
                        break;
                    }
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                    {
                        if ((GC.MyChar.Equips.LeftHand.ID == 0 || Game.ItemIDManipulation.Part(GC.MyChar.Equips.LeftHand.ID, 0, 3) != 900))
                        {
                            AddText("Where did you put your shield to? You just had one equiped.");
                            AddOption("Oops. Sorry.", 255);
                        }
                        else
                        {
                            GC.MyChar.Equips.LeftHand.Color = (Game.Item.ArmorColor)(_linkback - 30);
                            GC.AddSend(Packets.UpdateItem(GC.MyChar.Equips.LeftHand, 5));
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}