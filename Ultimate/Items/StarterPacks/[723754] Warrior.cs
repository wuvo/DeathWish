using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_723754 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 30)
            {
                C.RemoveItem(I);
                for (int i = 0; i < 1; i++)

                C.AddItem(410301); // Level 1 NoobBlade Ultimate Conquer Special Blade kek
                C.AddItem(160019); //Oxhide boots level 10 super all classes
                C.AddItem(120029); // LightNecklace Super lvl 7
                C.AddItem(150009); // Iron Ring Super lvl 1
                C.AddItem(561019); //SlantWand Super lvl 10
                C.AddItem(131009); //Oxhide Armor lvl 15
                C.AddItem(111009);  //IronHelmet super lvl 15
                C.Silvers += 1000;
            }
            else
            {
                C.MyClient.LocalMessage(2005, "There is not enough space in your inventory. Please make space and try again.");
            }
        }
    }
}
