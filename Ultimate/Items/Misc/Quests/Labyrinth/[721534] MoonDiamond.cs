using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721534 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(721534, 13) && C.VipLevel >= 5)
            {
                for (int i = 0; i < 13; i++)
                    C.RemoveItem(C.NextItem(721534));
                C.AddItem(721542);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You must delivery 13 MoonDiamonds to Simon. VIPs can exchange diamonds by right clicking them.");
            }
        }
    }
}