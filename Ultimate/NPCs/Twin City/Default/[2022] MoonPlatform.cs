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
    public class NPC_2022 : NPCBase
    {
        public NPC_2022(Main.GameClient _client)
            : base(_client)
        {
            ID = 2022;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("You may enjoy the bright moon and twinkling stars on the Moon Platform. I can teleport you there for a fee of 500 silver.");
                        AddOption("Teleport me there", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 5000)
                        {
                            GC.MyChar.Silvers -= 5000;
                            GC.MyChar.Teleport(1105, 184, 164);
                            AddText("Enjoy your stay at the MoonPlatform!");
                            AddOption("Thank you", 255);
                        }
                        else
                        {
                            AddText("You don't have 5,000 Silvers.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}