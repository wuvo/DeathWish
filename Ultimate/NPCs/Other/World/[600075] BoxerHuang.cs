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
    public class NPC_600075 : NPCBase
    {
        public NPC_600075(Main.GameClient _client)
            : base(_client)
        {
            ID = 600075;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to leave the Training Ground?");
                        AddOption("Yeah", 1);
                        AddOption("No, I'll stay here.", 255);
                        break;
                    }
                case 1:
                    {
                        try
                        {
                            Game.Vector2 V = (Game.Vector2)Database.DefaultCoords[GC.MyChar.Loc.PreviousMap];
                            GC.MyChar.Teleport(GC.MyChar.Loc.PreviousMap, V.X, V.Y);
                            GC.MyChar.Protection = false;
                        }
                        catch
                        {
                            GC.MyChar.Teleport(1002, 427, 379);
                            GC.MyChar.Protection = false;
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}