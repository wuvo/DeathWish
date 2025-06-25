using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.NPCs
{
    /// <summary>
    ///     Handles NPC usage for [2080] MysticOldMan
    ///     Written by Nyorai 05/09/2016
    /// </summary>
    public class NPC_2080 : NPCBase
    {
        public NPC_2080(Main.GameClient _client)
            : base(_client)
        {
            ID = 2080;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("After many years of practise, I've managed to find out some of the secrets for socketing items. ");
                    AddText("I've been crafting my own items since I can remember and I've now decided to share my secrets with the world. ");
                    AddText("Would you like to try your luck?");
                    AddOption("Let's go for it", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (GC.MyChar.Silvers >= 5000)
                    {
                        GC.MyChar.Teleport(1009, 22, 26);
                        GC.MyChar.Silvers -= 5000;
                    }
                    else
                    {
                        AddText("Sorry, you do not have 5,000 silvers!");
                        AddOption("I see.", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
    /// <summary>
     ///     Handles NPC usage for [2081] MysticOldMan
     ///     Written by Nyorai 05/09/2016
     /// </summary>
    public class NPC_2081 : NPCBase
    {
        public NPC_2081(Main.GameClient _client)
            : base(_client)
        {
            ID = 2081;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You can manually craft your item and obtain higher rates at socketing like you'd never imagine. ");
                    AddText("However, it comes with a great cost. You can only use Meteors, MeteorScrolls, MetScrollBags, DragonBalls or DBScrolls.");
                    AddText("Would you like to try your luck?");
                    AddOption("Craft my item", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    AddText("It will cost you 10,000 silvers for each Meteor and 25,000 silvers for each DragonBall you use. The more you use, the higher the socket rate. ");
                    AddText("Would you like to try your luck?");
                    AddOption("Craft my item", 2);
                    AddOption("Just passing by.", 255);
                    break;
                case 2:
                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 2, (ushort)(GC.MyChar.Loc.X), (ushort)(GC.MyChar.Loc.Y), 126));
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
