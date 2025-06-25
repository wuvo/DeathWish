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
    /// Handles NPC usage for [2096] PCCaptain
    /// </summary>
    public class NPC_2096 : NPC_2095
    {
        public NPC_2096(Main.GameClient GC)
            : base(GC)
        {
            ID = 2096;
            Face = 14;
        }
    }
}