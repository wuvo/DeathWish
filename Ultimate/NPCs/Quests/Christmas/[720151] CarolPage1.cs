using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    public class NPC_720151 : NPCBase
    {
        public NPC_720151(Main.GameClient _client)
                : base(_client)
        {
            ID = 720151;
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
                    AddText("Dashing through the snow, In a one horse open sleigh,\nO'er the fields we go, Laughing all the way,\n");
                    AddText("Bells on bob tails ring, Making spirits bright,\nWhat fun it is to laugh and sing, A sleighing song tonight,\n\n");
                    AddText("Oh, jingle bells, jingle bells, Jingle all the way,\nOh, what fun it is to ride, In a one horse open sleigh, HEY!\n");
                    AddText("Oh, jingle bells, jingle bells, Jingle all the way,\nOh, what fun it is to ride, In a one horse open sleigh, HEY!\n");
                    AddOption("Listen to the song !", 1);
                    AddOption("Merry Christmas !", 255);
                    break;
                case 1:
                    GC.LocalMessage(2105, "https://www.youtube.com/watch?v=3PgNPc-iFW8");
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
