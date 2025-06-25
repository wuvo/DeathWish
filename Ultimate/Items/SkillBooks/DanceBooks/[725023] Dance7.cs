using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725023 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (!C.Skills.ContainsKey(1405))
            {
                C.NewSkill(new Skill() { ID = 1405 });
                C.RemoveItem(I);
            }
        }
    }
}