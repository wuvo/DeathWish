using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725015 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Job >= 130 && C.Job <= 135 && C.Level >= 54)
            {
                C.NewSkill(new Skill() { ID = 1350 });
                C.RemoveItem(I);
            }
        }
    }
}