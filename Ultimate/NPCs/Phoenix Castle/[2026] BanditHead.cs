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
    public class NPC_2026 : NPCBase
    {
        public NPC_2026(Main.GameClient _client)
            : base(_client)
        {
            ID = 2026;
            Face = 30;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("What do you want?");
                        AddOption("I am here to get the Army Token,", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("That got stolen from me by the Caterans.  If you want it you will have to get it from them.");
                        AddOption("I see", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}