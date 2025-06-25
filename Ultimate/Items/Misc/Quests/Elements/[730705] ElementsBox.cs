using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_730705 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 35)
            {
                Random Rnd = new Random();
                switch(Rnd.Next(0,8))
                {
                    case 0:
                       for (int i = 0; i < 5;i++)
                            C.AddItem(1088001);
                        break;
                    case 1:
                        C.Silvers += 100000;
                        break;
                    case 2:
                        C.Silvers += 150000;
                        break;
                    case 3:
                        C.Silvers += 200000;
                        break;
                    case 4:
                        C.Silvers += 500000;
                        break;
                    case 5:
                        C.Silvers += 250000;
                        C.MyClient.LocalMessage(2011, "Congratulations! " + C.Name + " has got 250,000 Gold by opening ElementsBox!");
                        break;
                    case 6:
                        for (int i = 0; i < 2; i++)
                            C.AddItem(720027); //MetScroll
                        break;
                    case 7:
                        C.AddItem(720027);
                        break;
                    case 8:
                        C.AddItem(1088000); //Dragonball
                        break;
                    //case 9:
                    //    for (int i = 0; i < 2; i++)
                    //        C.AddItem(1088000);
                    //    C.MyClient.LocalMessage(2011, "Congratulations! " + C.Name + " has got (2) Dragonballs by opening ElementsBox!");
                    //    break;
                    //case 10:
                    //    break;
                }
                C.RemoveItem(I);
                C.MyClient.LocalMessage(2005, "You have successfuly opened the ElementsBox! Check your inventory!");
            }
            else
                C.MyClient.LocalMessage(2005, "Please clear room in your inventory! You need at least 5 spaces!");
        }
    }
}