using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_723711 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 35)
            {
                C.RemoveItem(C.NextItem(723711));
                for (int i = 0; i < 5; i++)
                    C.AddItem(1088002);
            }
        }
    }
}