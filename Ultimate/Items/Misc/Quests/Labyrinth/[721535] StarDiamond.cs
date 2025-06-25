using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721535 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(721535, 12) && C.VipLevel >= 5)
            {
                for (int i = 0; i < 12; i++)
                    C.RemoveItem(C.NextItem(721535));
                C.AddItem(721543);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You must delivery 12 StarDiamonds to Simon. VIPs can exchange diamonds by right clicking them.");
            }
        }
    }
}