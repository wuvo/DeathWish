using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1001000 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.CurMP += 70;
            C.RemoveItem(I);
        }
    }
}