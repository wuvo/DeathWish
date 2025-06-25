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
    public class NPC_20001 : NPCBase
    {
        public NPC_20001(Main.GameClient _client)
            : base(_client)
        {
            ID = 20001;
            Face = 3;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (!GC.MyChar.InventoryContains(721000, 1) && !GC.MyChar.InventoryContains(721001, 1) && !GC.MyChar.InventoryContains(721002, 1))
                        {
                            AddText("I'm so sad. I haven't seen my sister for a while and didn't get any notices from her. She should be in Phoenix Castle. Tell me if you see her.");
                            AddOption("Sure I will.", 255);
                        }
                        else if (GC.MyChar.InventoryContains(721000, 1) && !GC.MyChar.InventoryContains(721001, 1) && !GC.MyChar.InventoryContains(721002, 1))
                        {
                            AddText("I'm so sad. I haven't seen my sister for a while and didn't get any notices from her.");
                            AddOption("I got a letter from Milly.", 1);
                            AddOption("Sorry, I'm busy.", 255);
                        }
                        else if (!GC.MyChar.InventoryContains(721000, 1) && GC.MyChar.InventoryContains(721001, 1) && !GC.MyChar.InventoryContains(721002, 1))
                        {
                            AddText("Go meet Joe in Desert city near Mystic Castle entrace.");
                            AddOption("Yeah right.", 255);
                        }
                        else if (GC.MyChar.InventoryContains(721002, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721002));
                            GC.MyChar.AddItem(1088002);
                            AddText("Oh here you are! Thanks for delivering that to Joe. Take this Meteor Tear as reward for your courage!");
                            AddOption("Thanks.", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Oh! Thank you but I miss my darling Joe. I'm sure he's in the Desert. I would appreciate if you gave him this Guardian Star. It means a lot for me.");
                        AddOption("Ok bet on me.", 2);
                        AddOption("Sorry, I'm busy.", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(721000, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(721000));
                            GC.MyChar.AddItem(721001);
                            AddText("Thank you so much! Take this Guardian Star and meet Joe in the Desert near the Mystic Castle entrance.");
                            AddOption("I'm on my way.", 255);
                        }
                        else
                        {
                            AddText("Don't trick me you have no letter!");
                            AddOption("I'm on my way.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}