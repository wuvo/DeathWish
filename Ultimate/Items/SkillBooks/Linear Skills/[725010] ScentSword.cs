using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725010 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Level >= 40)
            {
                C.NewSkill(new Skill() { ID = 1046 });
                C.RemoveItem(I);
            }
        }
    }
}