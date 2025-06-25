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
    public class NPC_800000 : NPCBase
    {
        public NPC_800000(Main.GameClient _client)
            : base(_client)
        {
            ID = 800000;
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
                        AddText("The eight diagram quest is extremely dangerous ! Are you sure you want to get inside?");
                        AddOption("I am ready to face all the dangers", 1);
                        AddOption("Wait...", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Level >= 70)
                        {
                            GC.MyChar.Teleport(1042, 26, 36);
                        }
                        else
                        {
                            AddText("You have to be level 70 or higher to get in.!");
                            AddOption("Sorry...", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}