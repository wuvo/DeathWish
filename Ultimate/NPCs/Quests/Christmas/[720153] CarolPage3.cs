using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    public class NPC_720153 : NPCBase
    {
        public NPC_720153(Main.GameClient _client)
                : base(_client)
        {
            ID = 720153;
            Face = 67;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Oh the weather outside is frightful, But the fire is so delightful\n");
                    AddText("And since we've no place to go, Let It Snow! Let It Snow! Let It Snow!\n");
                    AddText("It doesn't show signs of stopping, And I've bought some corn for popping,\n");
                    AddText("The lights are turned way down low, Let It Snow! Let It Snow! Let It Snow!\n\n");
                    AddText("When we finally kiss good night, How I'll hate going out in the storm!\n");
                    AddText("But if you'll really hold me tight, All the way home I'll be warm,");
                    AddText("The fire is slowly dying, And, my dear, we're still goodbying,\n");
                    AddText("But as long as you love me so, Let It Snow! Let It Snow! Let It Snow!\n");
                    AddOption("Listen to the song !", 1);
                    AddOption("Merry Christmas !", 255);
                    break;
                case 1:
                    GC.LocalMessage(2105, "https://www.youtube.com/watch?v=mN7LW0Y00kE");
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
