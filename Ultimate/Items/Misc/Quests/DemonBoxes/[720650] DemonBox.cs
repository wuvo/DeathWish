using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720650 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (!World.NoPKMaps.Contains(C.Loc.Map))
                NPCs.NPCHandler.Handle(C.MyClient, null, I.ID, 0);
            else
                C.MyClient.LocalMessage(2005, "You can't spawn monsters in this map!");
        }
    }
}