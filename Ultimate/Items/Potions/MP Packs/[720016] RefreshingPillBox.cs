using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720016 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 37)
            {
                C.RemoveItem(I);
                for (int a = 0; a < 3; a++)
                    C.AddItem(1002030);
            }
        }
    }
}
