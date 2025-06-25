using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_721751 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 30)
            {
                C.RemoveItem(I);
                for (int x = 0; x < 10; x++)
                {
                    C.AddItem(700051);
                }
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You cannot unpack gems! Your inventory is full!");
            }
        }
    }
}
