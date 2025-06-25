using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720141 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.VipLevel >= 5)
            {
                if (C.InventoryContains(720141, 5))
                {
                    for (int i = 0; i < 5; i++)
                        C.RemoveItem(C.NextItem(720141));
                    C.AddItem(720142);
                }
                else
                {
                    C.MyClient.LocalMessage(2005, "Unable to pack EggPacket. Either you don't have 5 RedEgg.");
                }
            }
            else
            {
                C.MyClient.LocalMessage(2005, "You must be Vip 5/6 to use this feature.");
            }
        }
    }
}