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
    public class NPC_300005 : NPCBase
    {
        public NPC_300005(Main.GameClient _client)
            : base(_client)
        {
            ID = 300005;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I'm protecting the WineZone! The strongest monsters in the world were kept inside to prevent disasters. I can take you in if you bring me a HealthWine!");
                        AddOption("Here, take it.", 1);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(723030, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723030));
                            GC.MyChar.Teleport(1300, 315, 645);
                        }
                        else
                        {
                            AddText("You don't have an HealthWine!");
                            AddOption("Sorry.", 255);
                           
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}