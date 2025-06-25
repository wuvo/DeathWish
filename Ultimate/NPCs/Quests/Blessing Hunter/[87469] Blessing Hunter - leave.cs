using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [3002] Shirley
    /// </summary>
    public class NPC_87469 : NPCBase
    {
        public NPC_87469(Main.GameClient _client)
            : base(_client)
        {
            ID = 87469;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Would you like to leave here?");
                    AddOption("Yes please", 1);
                    AddOption("Just passing by", 255);
                    break;
                case 1:
                    {
                        GC.MyChar.Teleport(1002, 453, 380); // Change map location if you need to
                        AddText("You are now back in Twin City");
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
    
}
