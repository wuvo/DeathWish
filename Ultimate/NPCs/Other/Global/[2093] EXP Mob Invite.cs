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
    public class NPC_2093 : NPCBase
    {
        public NPC_2093(Main.GameClient _client)
            : base(_client)
        {
            ID = 2093;
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
                    if (GC.MyChar.Loc.Map == 1000 || GC.MyChar.Loc.Map == 1011 || GC.MyChar.Loc.Map == 1020 || GC.MyChar.Loc.Map == 1015 || GC.MyChar.Loc.Map == 1002)
                    {
                        //GC.AddSend(Packets.PopUp("Do you want to fight the EXP Mob?", 1));
                    }
                    break;
                case 1:
                    GC.MyChar.Teleport(1004, 51, 64);
                    break;
            }

            AddFinish();
            Send();
        }

    }
}