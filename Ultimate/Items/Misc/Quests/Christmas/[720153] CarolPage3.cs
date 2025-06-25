using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720153 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(720151, 1) && C.InventoryContains(720152, 1) && C.InventoryContains(720153, 1) && C.InventoryContains(720154, 1) && C.InventoryContains(720155, 1))
            {
                for (uint a = 720151; a < 720156; a++)
                    C.RemoveItem(C.NextItem(a));
                C.AddItem(720156);
            }
            else
                C.MyClient.LocalMessage(2005, "Gather all the Christmas Carols pages and make a MusicBook! ConductorDarwen is counting on you!");
            NPCs.NPCHandler.Handle(C.MyClient, null, I.ID, 0);
        }
    }
}
