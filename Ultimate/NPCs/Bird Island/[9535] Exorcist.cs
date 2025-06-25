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
    public class NPC_9535 : NPCBase
    {
        public NPC_9535(Main.GameClient _client)
            : base(_client)
        {
            ID = 9535;
            Face = 30;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("A demon is sealed by the strength of the seals gradually disappearing, he will return to himself soon, can you help me fixing its seal?");
                        AddOption("How can I help you?", 1);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("The devil is in deep sleep. We must do so on this occasion!");
                        AddText(" The amulet is what we want! There are 5 amulet that must be harvested by killing the monster that corresponds to your class.");
                        AddText(" After you gather the 5 Amulets you have to summon the AncientDevil by right clicking them! Be careful with his guards tho, ");
                        AddText("you have to enable PK mode to kill them. Will you help us?");
                        AddOption("Count me in", 2);
                        AddOption("Let me think it over", 255);
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.Teleport(1082, 189, 233);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}