using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_722399 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(722399, 1))
            {
                for (int i = 0; i < 1; i++)
                    C.RemoveItem(C.NextItem(722399));
                    C.ClassicPoints += 1;
                    C.MyClient.LocalMessage(2005, "You got 1 Classic Point, you can go trade them in the TC!");
            }
        }
    }
    public class Item_721699 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(721699, 1))
            {
                for (int i = 0; i < 1; i++)
                    C.RemoveItem(C.NextItem(721699));
                C.ClassicPoints += 20;
                C.MyClient.LocalMessage(2005, "You got 20 Classic Points, you can go trade them in the TC!");
            }
        }
    }
}