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
    public class NPC_70001 : NPCBase
    {
        public NPC_70001(Main.GameClient _client)
            : base(_client)
        {
            ID = 70001;
            Face = 7;
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
                        AddText("Do you want to leave the Guild Area?");
                        AddOption("Yes", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1002, 355, 337);
                    ChangePKMode(GC.MyChar, PKMode.Capture);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}