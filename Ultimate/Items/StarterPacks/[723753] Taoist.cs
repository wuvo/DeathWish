using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_723753 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 30)
            {
                C.RemoveItem(I);
                for (int i = 0; i < 1; i++)

                C.AddItem(114009); //cap
                C.AddItem(121009); //bag
                C.AddItem(152019); //bracelet
                C.AddItem(421009); //backsword
                C.AddItem(134009); //robe
                C.AddItem(160019); //boots
                C.AddItem(725003); //cure 
                C.AddItem(421301); //LuckyBacksword the level 1 wep
                C.Silvers += 1000;
            }
            else
            {
                C.MyClient.LocalMessage(2005, "There is not enough space in your inventory. Please make space and try again.");
            }
        }
    }
}