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
    public class NPC_3018 : NPC_1001
    {
        public NPC_3018(Main.GameClient _client)
            : base(_client)
        {
            ID = 3018;
            Face = 5;
        }
    }
}