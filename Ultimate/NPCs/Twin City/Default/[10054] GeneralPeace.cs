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
    public class NPC_10054 : NPCBase
    {
        public NPC_10054(Main.GameClient _client)
            : base(_client)
        {
            ID = 10054;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("This is the way to Desert City. Do you want to continue?");
                        AddOption("Take me there", 1);
                        AddOption("I'll stay", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1000, 973, 668);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}