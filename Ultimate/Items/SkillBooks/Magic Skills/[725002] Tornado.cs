using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725002 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Job >= 140 && C.Job <= 145 && C.Spi >= 160)
            {
                C.NewSkill(new Skill() { ID = 1002 });
                C.RemoveItem(I);
            }
        }
    }
}