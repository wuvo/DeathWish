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
    public class NPC_10012 : NPC_8
    {
        public NPC_10012(Main.GameClient _client)
            : base(_client)
        {
            ID = 10012;
            Face = 5;
        }
    }
}