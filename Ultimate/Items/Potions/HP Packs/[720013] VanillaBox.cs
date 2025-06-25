using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720013 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 37)
            {
                C.RemoveItem(I);
                C.AddItem(1002020);
                C.AddItem(1002020);
                C.AddItem(1002020);
            }
        }
    }
}
