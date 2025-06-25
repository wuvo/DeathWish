using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;
using Ultimate.Main;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [2097] ACCaptain
    /// </summary>
    public class NPC_2097 : NPC_2095
    {
        public NPC_2097(GameClient GC)
            : base(GC)
        {
            ID = 2097;
            Face = 14;
        }
    }
}