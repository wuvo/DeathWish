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
    public class NPC_1612 : NPCBase
    {
        public NPC_1612(Main.GameClient _client)
            : base(_client)
        {
            ID = 1612;
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
                        AddText("You must use Silver or Gold Needle to catch this kind of mouse. I do have the stuff. If you want, I can exchange Silver Needle for a DeagonGem, PhoenixGem, KylinGem, or RainbowGem; and exchange Gold Needle for FuryGem, MoonGem or VioletGem.");
                        AddOption("What is the difference?", 1);
                        AddOption("I want Silver Needle.", 2);
                        AddOption("I want Gold Needle.", 3);
                        break;
                    }
                case 1:
                    {
                        AddText("It is said that some have seen Blue Mice scurrying in the forest mine cave, carying precious treasures. I have never seen such a thing myself, but have found large mice tracks in the deeper mines! The mouse must be bigger than a dog!");
                        AddOption("Yeah, why not?", 2);
                        AddOption("I don't believe it.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("You must use Silver or Gold Needle to catch this kind of mouse. I do have the stuff. If you want, I can exchange Silver Needle for a DeagonGem, PhoenixGem, KylinGem, or RainbowGem; and exchange Gold Needle for FuryGem, MoonGem or VioletGem.");
                        AddOption("What is the difference?", 3);
                        AddOption("I want Silver Needle.", 4);
                        AddOption("I want Gold Needle.", 5);
                        break;
                    }
                case 3:
                    {
                        AddText("The chance of getting treasures is a little bit higher if you use GoldNeedle. But it's invariable that you must be patient.");
                        AddOption("So I want GoldNeedle.", 5);
                        AddOption("Even so, I want SilverNeedle.", 4);
                        AddOption("Oh, I see.", 255);
                        break;
                    }
                case 4:
                    {
                        if (!GC.MyChar.InventoryContains(722510, 20))
                        {
                            if (GC.MyChar.InventoryContains(700011, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700011));
                                GC.MyChar.AddItem(722510);
                                AddText("Here you are. Good luck with the hunting..");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else if (GC.MyChar.InventoryContains(700001, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700001));
                                GC.MyChar.AddItem(722510);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else if (GC.MyChar.InventoryContains(700031, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700031));
                                GC.MyChar.AddItem(722510);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else if (GC.MyChar.InventoryContains(700041, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700041));
                                GC.MyChar.AddItem(722510);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else
                            {
                                AddText("In order to receive a Silver Needle you must bring me a DragonGem, PhoenixGem, KylinGem, or RainbowGem!");
                                AddOption("Ok, I will get it ready soon", 255);
                                AddOption("Take a Gold Needle instead", 5);
                                break;
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you can carry only 20 needle of each kind at a time.");
                            AddOption("Ah, I see.", 255);
                            break;
                        }
                    }
                case 5:
                    {
                        if (!GC.MyChar.InventoryContains(722511, 20))
                        {
                            if (GC.MyChar.InventoryContains(700021, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700021));
                                GC.MyChar.AddItem(722511);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else if (GC.MyChar.InventoryContains(700061, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700061));
                                GC.MyChar.AddItem(722511);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else if (GC.MyChar.InventoryContains(700051, 1))
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(700051));
                                GC.MyChar.AddItem(722511);
                                AddText("Here you are. Good luck with the hunting.");
                                AddOption("Thanks, bye.", 255);
                                break;
                            }
                            else
                            {
                                AddText("In order to receive a GoldNeedle you must bring me a FuryGem, a VioletGem or a MoonGem!");
                                AddOption("Ok, I will get it ready soon", 255);
                                AddOption("Take the Silver needle instead", 4);
                                break;
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you can carry only 20 needle of each kind at a time.");
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