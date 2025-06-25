using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.NPCs
{
    /// <summary>
    ///     Handles NPC usage for [10021] ArenaGuard
    ///     Written by Nyorai 19/07/2016
    /// </summary>
    public class NPC_10021 : NPCBase
    {
        public NPC_10021(Main.GameClient _client)
            : base(_client)
        {
            ID = 10021;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("The arena is open. Welcome to challenge other people.  ");
                    AddText("If you PK in the arena, you will not gain or lose any experience or items equipped but you will get revived to TwinCity. ");
                    AddText("The Kungfu circle is very dangerous, I suggest you PK in area.");
                    AddOption("Enter the arena.", 1);
                    AddOption("Clear my stats", 2);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (GC.MyChar.Silvers >= 1000)
                    {
                        GC.MyChar.Teleport(1005, 51, 71);
                        GC.MyChar.Silvers -= 1000;
                    }
                    else
                    {
                        AddText("Sorry, you do not have enough gold.");
                        AddOption("I see.", 255);
                    }
                    break;

                case 2:
                    GC.LocalMessage(0x83c, "");
                    GC.MyChar.Kills = 0;
                    GC.MyChar.Deaths = 0;
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
