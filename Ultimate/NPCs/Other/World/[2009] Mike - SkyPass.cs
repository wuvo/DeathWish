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
    public class NPC_2009 : NPCBase
    {
        public NPC_2009(Main.GameClient _client)
            : base(_client)
        {
            ID = 2009;
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
                        AddText("This is the way to Sky Pass. Are you here to challenge the Sky Pass?");
                        AddOption("Please show me the way", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Don't rush into dangers! The Sky Pass was designed by my master, the ");
                        AddText("God Cloud. He trapped horrific monsters in the Pass to test out the real hero.");
                        AddOption("Can I have a look?", 2);
                        break;
                    }
                case 2:
                    {
                        AddText("Sure! Everyone has a chance to challenge it. If you pass 5 floors at a time, you'll see my great master.");
                        AddOption("I'm up for it", 3);
                        AddOption("I better leave", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("The Pass consists of 5 floors, and each floor is guarded by the monsters of different level.");
                        AddOption("Take me in", 4);
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.InventoryContains(721109) ||
                            GC.MyChar.InventoryContains(721100) ||
                            GC.MyChar.InventoryContains(721101) ||
                            GC.MyChar.InventoryContains(721102) ||
                            GC.MyChar.InventoryContains(721103) ||
                            GC.MyChar.InventoryContains(721108))
                        {
                            AddText("No cheating! A true hero must hunt for his own tokens inside the Sky Pass! Please get rid of those!");
                            AddOption("I see", 255);
                        }
                        else
                            GC.MyChar.Teleport(1040, 596, 384);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}