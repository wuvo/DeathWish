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
    public class NPC_10027 : NPC_8
    {
        public NPC_10027(Main.GameClient _client)
            : base(_client)
        {
            ID = 10027;
            Face = 5;
        }
    }
}