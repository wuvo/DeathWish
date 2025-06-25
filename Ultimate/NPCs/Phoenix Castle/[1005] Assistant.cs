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
    public class NPC_1005 : NPCBase
    {
        public NPC_1005(Main.GameClient _client)
            : base(_client)
        {
            ID = 1005;
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
                        AddText("Are you heading for copper mine? The rock monsters are very fierce. You cannot enter this cave before you are level 40.");
                        AddOption("Please teleport me there", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Level >= 40)
                        {
                            GC.MyChar.Teleport(1025, 28, 70);
                        }
                        else
                        {
                            AddText("Sorry, you are not allowed to enter this cave before you are level 40.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}