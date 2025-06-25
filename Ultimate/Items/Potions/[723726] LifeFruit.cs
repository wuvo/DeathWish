using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_723726 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.RemoveItem(I);
            C.CurHP = C.MaxHP;
            C.CurMP = C.MaxMP;
        }
    }
}
