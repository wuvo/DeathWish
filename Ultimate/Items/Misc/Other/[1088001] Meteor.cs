using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1088001 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(1088001, 10))
            {
                for (int i = 0; i < 10; i++)
                    C.RemoveItem(C.NextItem(1088001));
                C.AddItem(720027);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "Unable to pack MeteorScroll. Either you don't have 10 Meteors.");
            }
        }
    }
}