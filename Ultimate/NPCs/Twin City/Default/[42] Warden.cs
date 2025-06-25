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
    public class NPC_43 : NPCBase
    {
        public NPC_43(Main.GameClient _client)
            : base(_client)
        {
            ID = 43;
            Face = 54;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to visit jail?");
                        AddOption("Yes.", 1);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(6000, 32, 72);
                    break;
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_42 : NPCBase
    {
        public NPC_42(Main.GameClient _client)
            : base(_client)
        {
            ID = 42;
            Face = 54;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Only if you aren't an evildoer you can leave here.");
                        AddOption("Let me out of here!", 1);
                        AddOption("I see.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.PKPoints < 100)
                            GC.MyChar.Teleport(1002, 512, 355);
                        else
                        {
                            AddText("No, you will stay here until you have regretted your sins. Or you could do some work and mine me some gold ores.");
                            AddText("Mine me 5 Gold Ores with rate of 2 or higher and you can get out of here. You'll also have to pay a 500,000 silvers fee.");
                            AddOption("Lend me a hoe then.", 2);
                            AddOption("I already have the ores.", 3);
                            AddOption("Thats bad...", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (!GC.MyChar.InventoryContains(562001, 1))
                            GC.MyChar.AddItem(562001);
                        break;
                    }
                case 3:
                    {
                        byte Count = 0;
                        List<Item> Items = new List<Item>();
                        foreach (Item I in GC.MyChar.Inventory)
                            if (I.ID >= 1072051 && I.ID <= 1072059)
                            {
                                Count++;
                                Items.Add(I);
                            }
                        if (Count >= 5 && GC.MyChar.Silvers >= 500000)
                        {
                            GC.MyChar.Silvers -= 500000;
                            Count = 5;
                            foreach (Item I in Items)
                                if (Count > 0)
                                {
                                    Count--;
                                    GC.MyChar.RemoveItem(I);
                                }
                            GC.MyChar.Teleport(1002, 512, 355);
                        }
                        else
                        {
                            AddText("You must be blind because you don't have 5 gold ores with rate of 3 or higher or 500,000 silvers.");
                            AddOption("I'm not blind!", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}