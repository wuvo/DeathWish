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
    public class NPC_7382 : NPCBase
    {
        public NPC_7382(Main.GameClient _client)
            : base(_client)
        {
            ID = 7382;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("This is the botjail. Only botters get in here or visitors.");
                        AddOption("Can you let me out?", 1);
                        break;
                    }
                case 1:
                    {
                        if (!GC.MyChar.BOTJailed)
                        {
                            GC.MyChar.Teleport(1002, 430, 380);
                        }
                        else
                        {
                            AddText("You are botjailed for " + GC.MyChar.BOTJailedDays + " days. You can't get out of here.");
                            AddOption("Ahh ok.", 255);
                           
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}