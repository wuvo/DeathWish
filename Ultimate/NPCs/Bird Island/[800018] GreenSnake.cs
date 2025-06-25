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
    public class NPC_800018 : NPCBase
    {
        public NPC_800018(Main.GameClient _client)
            : base(_client)
        {
            ID = 800018;
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
                        AddText("Do not kill me, please! We have precious jewels in the nest, if you defeat our 17 Snake Formation, you can have them!");
                        AddOption("The formation of 17 snakes, what is it?", 1);
                        break;
                    }
                case 1:
                    {
                        AddText("The formation is organized by 17 islands joined by lotus. My brother preserve them. The king keeps the jewelry in the last island!");
                        AddOption("I want to go.", 2);
                        AddOption("I want to stay.", 255);
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.Teleport(1051, 448, 356);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}