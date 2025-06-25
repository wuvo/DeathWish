using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_729912 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 30)
            {
                C.RemoveItem(C.NextItem(729912));
                for (int i = 0; i < 10; i++)
                    C.AddItem(720027);
            }
        }
    }
}