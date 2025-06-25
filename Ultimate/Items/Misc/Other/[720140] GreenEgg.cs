using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720140 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.VipLevel >= 5)
            {
                if (C.InventoryContains(720140, 5))
                {
                    for (int i = 0; i < 5; i++)
                        C.RemoveItem(C.NextItem(720140));
                    C.AddItem(720147);
                }
                else
                {
                    C.MyClient.LocalMessage(2005, "Unable to pack EggPacket. Either you don't have 5 GreenEgg.");
                }
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You must be Vip 5/6 to use this feature.");
            }
        }
    }
}