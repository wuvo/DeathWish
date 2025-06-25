using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725040 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.NewSkill(new Skill() { ID = 7000 });
            C.RemoveItem(I);
        }
    }
}