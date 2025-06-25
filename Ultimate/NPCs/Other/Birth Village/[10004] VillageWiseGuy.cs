using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.NPCs
{
    public class NPC_10004 : NPCBase
    {
        public NPC_10004(Main.GameClient _client)
            : base(_client)
        {
            ID = 10004;
            Face = 9;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Great adventures await you in this world! Make sure you explore everything to the limit and experience ");
                        AddText("everything you can!");
                        AddOption("I see", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}
