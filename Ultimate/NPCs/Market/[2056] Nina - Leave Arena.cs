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
    public class NPC_2056 : NPCBase
    {
        public NPC_2056(Main.GameClient _client)
            : base(_client)
        {
            ID = 2056;
            Face = 13;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Would you like me to take you back to market?");
                        AddOption("Yes please", 1);
                        AddOption("Clear my stats", 2);
                        AddOption("I'll stay", 255);
                        break;
                    }
                case 1:
                    {
                        //if (World.UnlimitedArena.Contains(GC.MyChar))
                        //    World.UnlimitedArena.Remove(GC.MyChar);

                        GC.MyChar.Teleport(1036, 216, 206);
                        GC.LocalMessage(0x83c, "");
                        GC.LocalMessage(0x83d, "");
                        GC.LocalMessage(0x83d, "");
                        GC.LocalMessage(0x83d, "");
                        break;
                    }
                case 2:
                    AddText("I can clear your current stats for 20,000 silvers. Would you like to do it?");
                    AddOption("Yes please", 3);
                    AddOption("Nevermind", 255);
                    break;
                case 3:
                    if (GC.MyChar.Silvers >= 20000)
                    {
                        GC.MyChar.Silvers -= 20000;
                        GC.MyChar.Shots = 0;
                        GC.MyChar.Hits = 0;
                    }
                    else
                    {
                        AddText("It seems like you don't have 20,000 silvers.");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}