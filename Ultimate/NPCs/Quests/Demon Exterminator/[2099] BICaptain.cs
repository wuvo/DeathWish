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
    /// Handles NPC usage for [2099] BICaptain
    /// </summary>
    public class NPC_2099 : NPC_2095
    {
        public NPC_2099(Main.GameClient _client)
            : base(_client)
        {
            ID = 2099;
            Face = 14;
        }
    }
}