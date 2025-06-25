using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_720010 : IItem
    {
        public override void Run(Character C, Item I)
        {
            uint DrugID = 1000030;
            if (C.Inventory.Count <= 37)
            {
                C.RemoveItem(I);
                C.AddItem(DrugID);
                C.AddItem(DrugID);
                C.AddItem(DrugID);
            }
        }
    }
}
