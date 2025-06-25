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
    public class NPC_45 : NPCBase
    {
        public NPC_45(Main.GameClient _client)
            : base(_client)
        {
            ID = 45;
            Face = 156;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Do you want to leave the market?");
                        AddOption("Take me out", 1);
                        AddOption("I'll stay", 255);
                        break;
                    }
                case 1:
                    {
                        try
                        {
                            Game.Vector2 V = (Game.Vector2)Database.DefaultCoords[GC.MyChar.Loc.PreviousMap];
                            GC.MyChar.Teleport(GC.MyChar.Loc.PreviousMap, V.X, V.Y);
                        }
                        catch
                        {
                            GC.MyChar.Teleport(1002, 432, 372);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}