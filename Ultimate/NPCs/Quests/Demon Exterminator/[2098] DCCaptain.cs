using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;

namespace Ultimate.NPCs
{
    /// <summary>
    /// Handles NPC usage for [2098] DCCaptain
    /// </summary>
    public class NPC_2098 : NPC_2095
    {
        public NPC_2098(Main.GameClient _client)
            : base(_client)
        {
            ID = 2098;
            Face = 14;
        }
    }
}