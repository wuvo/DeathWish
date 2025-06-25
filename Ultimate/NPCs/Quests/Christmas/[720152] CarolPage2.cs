using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    public class NPC_720152 : NPCBase
    {
        public NPC_720152(Main.GameClient _client)
                : base(_client)
        {
            ID = 720152;
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
                    AddText("Jingle bell, jingle bell, jingle bell rock, jingle bells swing and jingle bells ring,\n");
                    AddText("Snowing and blowing up bushels of fun, Now the jingle hop has begun\n");
                    AddText("Jingle bell, Jingle bell, Jingle bell rock, Jingle bells chime in jingle bell time,\n");
                    AddText("Dancing and prancing in Jingle Bell Square, In the frosty air.\n\n");
                    AddText("What a bright time, it's the right time, To rock the night away\n\n");
                    AddText("Jingle bell time is a swell time, To go gliding in a one-horse sleigh,\n");
                    AddText("Giddy-up jingle horse, pick up your feet, Jingle around the clock\n");
                    AddText("Mix and a-mingle in the jingling feet, That's the jingle bell,\n");
                    AddText("That's the jingle bell, That's the jingle bell rock!\n");
                    AddOption("Listen to the song !", 1);
                    AddOption("Merry Christmas !", 255);
                    break;
                case 1:
                    GC.LocalMessage(2105, "https://www.youtube.com/watch?v=VfLf7A_-1Vw");
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
