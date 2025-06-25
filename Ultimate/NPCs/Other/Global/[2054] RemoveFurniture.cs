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
    public class NPC_2054 : NPCBase
    {
        public NPC_2054(Main.GameClient _client)
            : base(_client)
        {
            ID = 2054;
            Face = 13;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    GC.AddSend(Packets.PopUp("Do you want to remove the furniture?", 1));
                    break;
                case 1:
                    Features.HouseTable.RemoveFurniture(GC.MyChar, GC.MyChar.RemoveFurniture);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}