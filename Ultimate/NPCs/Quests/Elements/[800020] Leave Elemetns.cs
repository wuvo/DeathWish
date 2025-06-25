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
    public class NPC_800020 : NPCBase
    {
        public NPC_800020(Main.GameClient _client)
            : base(_client)
        {
            ID = 800020;
            Face = 7;
        }
        public class NPC_800021 : NPC_800020
        {
            public NPC_800021(Main.GameClient _client)
                : base(_client)
            {
                ID = 10028;
                Face = 5;
            }
        }

        public class NPC_800022 : NPC_800020
        {
            public NPC_800022(Main.GameClient _client)
                : base(_client)
            {
                ID = 10028;
                Face = 5;
            }
        }

        public class NPC_800023 : NPC_800020
        {
            public NPC_800023(Main.GameClient _client)
                : base(_client)
            {
                ID = 10028;
                Face = 5;
            }
        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I can't believe you've made it this far! You deserve my help. Do you want me to take you to Twin City?");
                        AddOption("Yes, please!", 1);
                        AddOption("Wait a moment", 255);
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.Teleport(1002, 427, 379);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}