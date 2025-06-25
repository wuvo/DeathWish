using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725019 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (!C.Skills.ContainsKey(1385))
            {
                C.NewSkill(new Skill() { ID = 1385 });
                C.RemoveItem(I);
            }
        }
    }
}