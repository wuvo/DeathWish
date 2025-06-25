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
    public class NPC_159752 : NPCBase
    {
        public NPC_159752(Main.GameClient _client)
            : base(_client)
        {
            ID = 159752;
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
                        AddText("Would you like to go back to the Guild Area?");
                        AddOption("Yes, please", 1);
                        AddOption("No thanks", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 500)
                        {
                            GC.MyChar.Silvers -= 500;
                            GC.MyChar.Teleport(1038, 348, 339);
                        }
                        else
                        {
                            AddText("You don't have 500 silvers.");
                            AddOption("Oh, I see.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_159753 : NPC_159752
    {
        public NPC_159753(Main.GameClient _client)
            : base(_client)
        {
            ID = 159753;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_159754 : NPC_159752
    {
        public NPC_159754(Main.GameClient _client)
            : base(_client)
        {
            ID = 159754;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_159755 : NPC_159752
    {
        public NPC_159755(Main.GameClient _client)
            : base(_client)
        {
            ID = 159755;
            Face = 1;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}