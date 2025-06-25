using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721536 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.InventoryContains(721536, 10) && C.VipLevel >= 5)
            {
                for (int i = 0; i < 10; i++)
                    C.RemoveItem(C.NextItem(721536));
                C.AddItem(721544);
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You must delivery 10 CloudDiamonds to Simon. VIPs can exchange diamonds by right clicking them.");
            }
        }
    }
}