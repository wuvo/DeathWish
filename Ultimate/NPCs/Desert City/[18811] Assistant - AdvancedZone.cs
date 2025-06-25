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
    public class NPC_18811 : NPCBase
    {
        public NPC_18811(Main.GameClient _client)
            : base(_client)
        {
            ID = 18811;
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
                        AddText("Do you want to enter the Advanced Zone? Be careful there are many dangers ahead!");
                        AddOption("Yeah", 1);
                        AddOption("Let me think it over", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1205, 1350, 1198);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}