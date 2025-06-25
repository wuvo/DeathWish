using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725028 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Job >= 130 && C.Job <= 135 || C.Job >= 140 && C.Job <= 145)
            {
                C.NewSkill(new Skill() { ID = 5001 });
                C.RemoveItem(I);
            }
        }
    }
}