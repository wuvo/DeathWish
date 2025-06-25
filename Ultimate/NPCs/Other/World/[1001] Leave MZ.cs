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
    public class NPC_1001 : NPCBase
    {
        public NPC_1001(Main.GameClient _client)
            : base(_client)
        {
            ID = 1001;
            Face = 67;
        }
        public void ChangePKMode(Character C, PKMode Mode)
        {
            C.PKMode = Mode;
            if (C.MyClient != null)
                C.MyClient.AddSend(Packets.GeneralData(C.EntityID, (uint)Mode, 0, 0, 96));
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
                        ChangePKMode(GC.MyChar, PKMode.Capture);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}