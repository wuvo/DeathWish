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
    public class NPC_10007 : NPCBase
    {
        public NPC_10007(Main.GameClient _client)
            : base(_client)
        {
            ID = 10007;
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
                        AddText("Glad~to~meet~you.~I~am~selling~different~armors~in~different~city.~To~level~up~quickly,~you~had~better~equip~the~best~armors.");
                        AddOption("How~to~buy~and~sell?", 1);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Right~click~on~an~armor~to~buy~it.~Drag~it~to~the~shop~window~to~sell~it.~Different~armors~give~different~stats.");
                        AddText("That~is~all.~If~you~have~not~talked~to~other~NPCs,~you~had~better~have~a~chat~with~them~so~that~you~can~learn~more.");
                        AddOption("I~see.~Thanks.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}