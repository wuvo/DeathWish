using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1200007 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(1200007, 1) && C.InventoryContains(1200008, 1) && C.InventoryContains(1200009, 1) &&
                                            C.InventoryContains(1200010, 1) && C.InventoryContains(1200011, 1))
            {
                if (C.Loc.Map == 1082 && C.Loc.X >= 213 - 5 && C.Loc.X <= 218 + 5 && C.Loc.Y >= 208 - 5 && C.Loc.Y <= 208 + 5)
                {
                    uint ID = 1200006;
                    for (int i = 0; i < 5; i++)
                    {
                        ID = (ID + 1);
                        C.RemoveItem(C.NextItem(ID));
                    }
                    World.AncientDevil = true;
                    World.SendMsgToAll("SYSTEM", "The AncientDevil is being awaken! Prepare yourself to fight, it will appear in a matter of seconds!", 2000, 0, C.Loc.Map);
                }
                else
                {
                    C.MyClient.LocalMessage(2000, "You can only summon the AncientDevil inside its map near 218,208!");
                }
            }
            else
                C.MyClient.LocalMessage(2000, "You have to gather all the amulets before summoning the AncientDevil!");
        }
    }
}