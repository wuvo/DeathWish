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
    public class NPC_2025 : NPCBase
    {
        public NPC_2025(Main.GameClient _client)
            : base(_client)
        {
            ID = 2025;
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
                        AddText("I don't have time to talk, go to Bandit Head in Phoenix Castle and you will find your answers.");
                        AddOption("Alright", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}