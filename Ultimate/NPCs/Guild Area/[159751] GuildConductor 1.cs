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
    public class NPC_159751 : NPCBase
    {
        public NPC_159751(Main.GameClient _client)
            : base(_client)
        {
            ID = 159751;
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
                        AddText("Do you want to go to Advanced Zone (canyon map)?");
                        AddOption("Yes, Please", 1);
                        AddOption("No thanks", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 1000)
                        {
                            GC.MyChar.Silvers -= 1000;
                            GC.MyChar.Teleport(2020, 534, 558);
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