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
    public class NPC_300007 : NPCBase
    {
        public NPC_300007(Main.GameClient _client)
            : base(_client)
        {
            ID = 300007;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("If you wish to quit you still have time! Would you like me to send you to TwinCity?");
                        AddOption("Yeah", 1);
                        AddOption("I fear nothing", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.EventBase != null && GC.MyChar.EventBase?.Stage == Events.EventStage.Inviting)
                        {
                            //GC.MyChar.EventBase?.Invited.TryAdd(GC.MyChar.EntityID, GC.MyChar);
                            GC.MyChar.EventBase?.RemovePlayer(GC.MyChar, false);
                        }
                        else
                        {
                            if (GC.MyChar.Loc.OldMap != 0 && GC.MyChar.Loc.OldX != 0 && GC.MyChar.Loc.OldY != 0)
                                GC.MyChar.Teleport(GC.MyChar.Loc.OldMap, GC.MyChar.Loc.OldX, GC.MyChar.Loc.OldY);
                            else if (GC.MyChar.PKPoints >= 100)
                                GC.MyChar.Teleport(6000, 29, 73);
                            else
                                GC.MyChar.Teleport(1002, 430, 378);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}