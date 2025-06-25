using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1060090 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.Hair = ushort.Parse("4" + Convert.ToString(C.Hair)[1] + Convert.ToString(C.Hair)[2]);
            C.RemoveItem(I);
        }
    }
}