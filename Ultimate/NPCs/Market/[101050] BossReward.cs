using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;
using Ultimate.PacketHandling;
using Ultimate.Features;

namespace Ultimate.NPCs
{
    public class NPC_202050 : NPCBase
    {
        public NPC_202050(Main.GameClient _client)
            : base(_client)
        {
            ID = 202050;
            Face = 1;
        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I have hid in the shadows saving the most amazing costumes. Now, I have returned with the goal of dressing up everyone");
                        AddText(" the best I can. How can I help you with?");
                        AddOption("FineSkyrocket", 1);
                        AddOption("FineSkyrocket, GreatSkyrocket", 2);
                        AddOption("FineSkyrocket, GreatSkyrocket, SuperSkyrocket", 3);
                        AddOption("I see", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(720036, 1))
                        //GC.MyChar.InventoryContains(722350, 1) && GC.MyChar.InventoryContains(722351, 1) && GC.MyChar.InventoryContains(722352, 1)
                        {
                            if (GC.MyChar.Inventory.Count >= 38)
                            {
                                AddText("Please make some room in your inventory!");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720036));
                                for (int a = 0; a < 1; a++)
                                    GC.MyChar.AddItem(729912);
                                AddText("Here you go! Enjoy your MetScrollBag!");
                                AddOption("Thanks", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have any FineSkyrocket!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(720036, 1) && GC.MyChar.InventoryContains(720037, 1))
                        //GC.MyChar.InventoryContains(722350, 1) && GC.MyChar.InventoryContains(722351, 1) && GC.MyChar.InventoryContains(722352, 1)
                        {
                            if (GC.MyChar.Inventory.Count >= 38)
                            {
                                AddText("Please make some room in your inventory!");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720036));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720037));
                                for (int a = 0; a < 1; a++)
                                    GC.MyChar.AddItem(729910);
                                AddText("Here you go! Enjoy your DBScrollBag!");
                                AddOption("Thanks", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have any FineSkyrocket, GreatSkyrocket!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(720036, 1) && GC.MyChar.InventoryContains(720037, 1) && GC.MyChar.InventoryContains(720038, 1))
                        //GC.MyChar.InventoryContains(722350, 1) && GC.MyChar.InventoryContains(722351, 1) && GC.MyChar.InventoryContains(722352, 1)
                        {
                            if (GC.MyChar.Inventory.Count >= 38)
                            {
                                AddText("Please make some room in your inventory!");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720036));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720037));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720038));
                                for (int a = 0; a < 1; a++)
                                    GC.MyChar.AddItem(1088901);
                                AddText("Here you go! Enjoy your PlatinumGem!");
                                AddOption("Thanks", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have any FineSkyrocket, GreatSkyrocket, SuperSkyrocket!");
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