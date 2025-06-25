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
    public class NPC_1000 : NPCBase
    {
        public NPC_1000(Main.GameClient _client)
            : base(_client)
        {
            ID = 1000;
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
                        AddText("I don't see many adventurers out this far! If you'd like I can teleport you to the next map. Be warned though that none have ever returned from its depths since you can't use scrolls inside.");
                        AddOption("Send me in", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.Teleport(1210, 1039, 717);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}