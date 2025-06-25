using Ultimate.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_1611 : NPCBase
    {
        public NPC_1611(Main.GameClient _client)
            : base(_client)
        {
            ID = 1611;
            Face = 95;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Thieves stole precious artefacts from the Mansion! The Aster Necklace, Pinetum Picture, and the Royal Sword! together with the Secret Command of Twin City's ruler. We have sent soldier to trace back the lost items.");
                        AddOption("Did you get any clues?", 1);
                        AddOption("I want to claim my prize!", 6);
                        AddOption("Poor guy.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Yeah. The thieves were captured. But we did not find out the lost items.I heard a rumor about blue mice, but I can't leave my post to investigate. Could you search for our items in the Phoenix Castle Mine");
                        AddOption("Yes sure.", 2);
                        AddOption("Oh, I see.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("The King pormises big rewards for those who can retrieve the lost items. Do you want to go to Castle mine cave? I knew an old miner outside the mine had seen it.");
                        AddOption("Yes sure.", 3);
                        AddOption("I am quite busy now.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("Good! OldMiner has a kind of special needle to catch the mouse. Hope you can find the lost items soon.");
                        AddOption("Ok. I will visit OldMiner now.", 4);
                        AddOption("Sorry that I can't help you.", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("Thank you very much for your kindness. For any treasure you retrieve, I will repay you something. Good Luck.");
                        AddOption("Thanks, Bye.", 255);
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.InventoryContains(722515, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(722515));
                            if (MyMath.ChanceSuccess(50))
                            {
                                GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", "Lucky player " + GC.MyChar.Name + " has retrieved treasures stolen by Blue Mouses and won a Dragonball!", 2011, 0);
                            }
                            else
                                GC.MyChar.AddItem(720027);
                            AddText("Here you are!");
                            AddOption("Thanks.", 255);
                            break;
                        }
                        else if (GC.MyChar.InventoryContains(722514, 1) && GC.MyChar.InventoryContains(722513, 1) && GC.MyChar.InventoryContains(722512, 1))
                        {
                            if (GC.MyChar.Inventory.Count < 37)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722514));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722513));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722512));
                                if (MyMath.ChanceSuccess(3))
                                {
                                    GC.MyChar.AddItem(1088000);
                                    World.SendMsgToAll("SYSTEM", "Lucky player " + GC.MyChar.Name + " has retrieved treasures stolen by Blue Mouses and won a Dragonball!", 2011, 0);
                                }
                                else
                                    GC.MyChar.AddItem(720027);
                                AddText("Here you are!");
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("I'm sorry but your inventory is full.");
                                AddOption("Ah, I see.", 255);
                                break;
                            }
                        }
                        else if (GC.MyChar.InventoryContains(722514, 1) && (GC.MyChar.InventoryContains(722513, 1) || GC.MyChar.InventoryContains(722512, 1)))
                        {
                            if (GC.MyChar.Inventory.Count < 35)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722514));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722513));
                                for (int i = 0; i < 5; i++)
                                    GC.MyChar.AddItem(1088001);
                                AddText("Here you are!");
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("Please clear some room in your inventory first.");
                                AddOption("Ah, I see.", 255);
                                break;
                            }
                        }
                        else if (GC.MyChar.InventoryContains(722513, 1) && GC.MyChar.InventoryContains(722512, 1))
                        {
                            if (GC.MyChar.Inventory.Count < 38)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722513));
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722512));
                                for (int i = 0; i < 3; i++)
                                    GC.MyChar.AddItem(1088001);
                                AddText("Here you are!");
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("Please clear some room in your inventory first.");
                                AddOption("Ah, I see.", 255);
                                break;
                            }
                        }
                        else if (GC.MyChar.InventoryContains(722514, 1))
                        {
                            if (GC.MyChar.Inventory.Count < 36)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722514));
                                for (int i = 0; i < 3; i++)
                                    GC.MyChar.AddItem(1088001);
                                AddText("Here you are!");
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("Please clear some room in your inventory first.");
                                AddOption("Ah, I see.", 255);
                                break;
                            }
                        }
                        else if (GC.MyChar.InventoryContains(722513, 1) || GC.MyChar.InventoryContains(722512, 1))
                        {
                            if (GC.MyChar.InventoryContains(722513, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722513));
                            else
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722512));
                            GC.MyChar.AddItem(1088001);
                            AddText("Here you are!");
                            AddOption("Thanks.", 255);
                            break;
                        }
                        else
                        {
                            AddText("I'm sorry but you don't have any treasures.");
                            AddOption("Ah, I see.", 255);
                            break;
                        }
                    }
            }

            AddFinish();
            Send();
        }
    }
}