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
    public class NPC_44 : NPC_8
    {
        public NPC_44(Main.GameClient _client)
            : base(_client)
        {
            ID = 44;
            Face = 5;
            IsGlobal = true;
        }
    }
    public class NPC_46 : NPC_8
    {
        public NPC_46(Main.GameClient _client)
            : base(_client)
        {
            ID = 46;
            Face = 5;
            IsGlobal = true;
        }
    }
}