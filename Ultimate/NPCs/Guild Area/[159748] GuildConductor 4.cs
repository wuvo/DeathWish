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
    public class NPC_159748 : NPCBase
    {
        public NPC_159748(Main.GameClient _client)
            : base(_client)
        {
            ID = 159748;
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
                        AddText("Do you want to go to Adventure Zone (plains)?");
                        AddOption("Yes, Please", 1);
                        AddOption("No Thanks", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 1000)
                        {
                            GC.MyChar.Silvers -= 1000;
                            GC.MyChar.Teleport(1216, 624, 753);
                        }
                        else
                        {
                            AddText("You don't have 1000 silvers.");
                            AddOption("Oh, I see.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}