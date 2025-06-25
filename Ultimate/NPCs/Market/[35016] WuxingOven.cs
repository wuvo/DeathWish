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
    public class NPC_35016 : NPCBase
    {
        public NPC_35016(Main.GameClient _client)
            : base(_client)
        {
            ID = 35016;
            Face = 0;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Compose or Enchant?");
                        AddOption("Compose", 1);
                        AddOption("Enchant", 2);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    NPC N = null;
                    Dictionary<uint, NPC> MapNPC = World.H_NPCs[GC.MyChar.Loc.Map];
                    if (MapNPC != null && MapNPC.ContainsKey(ID))
                        N = (NPC)MapNPC[ID];
                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 1, N.Loc.X, N.Loc.Y, 0x7e));
                    break;
                case 2:
                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0x443, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 116));
                    break;
            }

            AddFinish();
            Send();
        }
    }
}