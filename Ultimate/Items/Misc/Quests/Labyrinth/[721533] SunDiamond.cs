using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721533 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(721533, 15))
            {
                for (int i = 0; i < 15; i++)
                    C.RemoveItem(C.NextItem(721533));
                C.AddItem(721541);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You don't have 15 SunDiamonds.");
            }
        }
    }
}