using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    public class NPC_720154 : NPCBase
    {
        public NPC_720154(Main.GameClient _client)
                : base(_client)
        {
            ID = 720154;
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
                    AddText("You know Dasher, and Dancer, and Prancer, and Vixen,\n");
                    AddText("Comet, and Cupid, and Donder and Blitzen\n");
                    AddText("But do you recall the most famous reindeer of all\n\n");
                    AddText("Rudolph, the red-nosed reindeer had a very shiny nose\n");
                    AddText("and if you ever saw it you would even say it glows.\n");
                    AddText("All of the other reindeer used to laugh and call him names\n");
                    AddText("They never let poor Rudolph play in any reindeer games.\n");
                    AddOption("Listen to the song !", 1);
                    AddOption("Merry Christmas !", 255);
                    break;
                case 1:
                    GC.LocalMessage(2105, "https://www.youtube.com/watch?v=0byH9h1ClBY");
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
