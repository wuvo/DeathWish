using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_1060100 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Job >= 140 && C.Job <= 145 && C.Level >= 80)
            {
                C.NewSkill(new Skill() { ID = 1160 });
                C.RemoveItem(I);
            }
        }
    }
}