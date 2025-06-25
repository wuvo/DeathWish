using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720670 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Level < 130)
            {
                C.AddExp(1 + (2 / 3.0));
                C.RemoveItem(I.UID);
            }
            else
                C.MyClient.LocalMessage(2005, "You are already at the maximum level!");
        }
    }
}