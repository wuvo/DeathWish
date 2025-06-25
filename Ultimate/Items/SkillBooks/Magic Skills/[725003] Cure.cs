using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725003 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Spi >= 30)
            {
                C.NewSkill(new Skill() { ID = 1005 });
                C.RemoveItem(I);
            }
        }
    }
}