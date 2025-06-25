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
    public class NPC_4293 : NPCBase
    {
        public NPC_4293(Main.GameClient _client)
            : base(_client)
        {
            ID = 4293;
            Face = 9;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to visit the mine?");
                        AddOption("Yeah.", 1);
                        AddOption("No.", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1028, 155, 95);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}