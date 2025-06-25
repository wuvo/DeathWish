using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1000030 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.CurHP += 500;
            C.RemoveItem(I);
        }
    }
}