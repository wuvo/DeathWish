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
    public class NPC_19002 : NPCBase
    {
        public NPC_19002(Main.GameClient _client)
            : base(_client)
        {
            ID = 19002;
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
                        AddText("Hello there! I can take you to the Meteor Zone mine cave. Do you want to enter?");
                        AddOption("Yes!", 1);
                        AddOption("Just passing by...", 255);
                        break;
                    }
                case 1:
                    if (GC.MyChar.Equips.RightHand.ID == 410005)
                        GC.MyChar.Silvers += 100000;
                    GC.MyChar.Teleport(1029, 30, 70);
                        break;
            }

            AddFinish();
            Send();
        }
    }
}