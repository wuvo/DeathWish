using NewestCOServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Events
{
    public static class EventHandler
    {
        public static void Handle(PVPEvents Event)
        {
            try
            {
                var npc = Activator.CreateInstance(Event.GetType()) as PVPEvents;
                npc.StartTournament();
            }
            catch(Exception e)
            {
                World.ExcAdd += e + "\r\n";
            }
        }
    }
}
