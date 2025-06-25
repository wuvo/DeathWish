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
    public class NPC_10052 : NPCBase
    {
        public NPC_10052(Main.GameClient _client)
            : base(_client)
        {
            ID = 10052;
            Face = 156;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hey want me to teleport you to somewhere? It will only cost you 1,000 silvers.");
                        AddOption("Twin City", 1);
                        AddOption("Market", 2);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 1000)
                        {
                            GC.MyChar.Silvers -= 1000;
                            GC.MyChar.Teleport(1011, 10, 377);
                        }
                        else
                        {
                            AddText("I said 1,000 silvers! If you don't have that, don't bother me.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.Silvers >= 1000)
                        {
                            GC.MyChar.Silvers -= 1000;
                            GC.MyChar.Teleport(1036, 211, 196);
                        }
                        else
                        {
                            AddText("I said 1,000 silvers! If you don't have that, don't bother me.");
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