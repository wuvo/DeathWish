using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_725016 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Level >= 70)
            {
                if (!C.Skills.ContainsKey(1360))
                {
                    C.RemoveItem(I);
                    C.NewSkill(new Skill() { ID = 1360 });
                }
            }
            else
            {
                C.MyClient.LocalMessage(2005, "Cannot use NightDevil at your Level!");
            }
        }
    }
}