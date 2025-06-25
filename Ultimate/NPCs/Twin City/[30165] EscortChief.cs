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
    public class NPC_30165 : NPCBase
    {
        public NPC_30165(Main.GameClient _client)
            : base(_client)
        {
            ID = 30165;
            Face = 34;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("You can delete all the items in your inventory by typing  ^/clearinv^ without ^^. Keep in mind that we are not responsible if you clear your inventory by mistake!");
                        AddOption("Thanks", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}