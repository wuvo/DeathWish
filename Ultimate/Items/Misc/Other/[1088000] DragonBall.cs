using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1088000 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(1088000, 10))
            {
                for (int i = 0; i < 10; i++)
                    C.RemoveItem(C.NextItem(1088000));
                C.AddItem(720028);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "Unable to pack DBScroll. Either you don't have 10 DragonBalls.");
            }
        }
    }
}