using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    public class NPC_720155 : NPCBase
    {
        public NPC_720155(Main.GameClient _client)
                : base(_client)
        {
            ID = 720155;
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
                    AddText("We wish you a merry Christmas (3x)\n");
                    AddText("And a happy New Year\n");
                    AddText("Glad tidings we bring, To you and your kin,\n");
                    AddText("Glad tidings for Christmas, And a happy New Year!\n\n");
                    AddText("We want some milk and cookies(3x)\n");
                    AddText("Please bring it right here, Glad tidings we bring,\n");
                    AddText("To you and your kin, Glad tidings for Christmas, And a happy New Year\n");
                    AddOption("Listen to the song !", 1);
                    AddOption("Merry Christmas !", 255);
                    break;
                case 1:
                    GC.LocalMessage(2105, "https://www.youtube.com/watch?v=g-OF7KGyDis");
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
