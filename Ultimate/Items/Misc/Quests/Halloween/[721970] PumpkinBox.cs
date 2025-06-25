using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721970 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count < 36)
            {
                C.RemoveItem(I.UID);
                bool _exp = C.Level < 130;
                if (MyMath.ChanceSuccess(25))
                {
                    C.AddItem(721954);
                    for (int a = 0; a < 2; a++)
                        C.AddItem(722176);
                    C.MyClient.LocalMessage(2005, "You have received a TransformCandy and 2 Pumpkins!");
                }
                else if (MyMath.ChanceSuccess(10))
                {
                    C.VotePoints++;
                    C.MyClient.LocalMessage(2005, "You have received a Vote Point!");
                }
                else if (MyMath.ChanceSuccess(20))
                {
                    C.AddItem(721541);
                    C.MyClient.LocalMessage(2005, "You have received a SunBox!");
                }
                else if (MyMath.ChanceSuccess(15))
                {
                    C.AddItem(721542);
                    C.MyClient.LocalMessage(2005, "You have received a WaningMoonBox!");
                }
                else if (MyMath.ChanceSuccess(10))
                {
                    C.AddItem(721543);
                    C.MyClient.LocalMessage(2005, "You have received a StarBox!");
                }
                else if (MyMath.ChanceSuccess(5))
                {
                    C.AddItem(721544);
                    C.MyClient.LocalMessage(2005, "You have received a CloudBox!");
                }
                else if (MyMath.ChanceSuccess(10))
                {
                    C.AddItem(721258);
                    C.MyClient.LocalMessage(2005, "You have received a Clean Water!");
                }
                //else if (MyMath.ChanceSuccess(10))
                //{
                //    C.AddItem(721080);
                //    C.MyClient.LocalMessage(2005, "You have received a MoonBox!");
                //}
                else
                {
                    for (int a = 0; a < 5; a++)
                        C.AddItem(722176);
                    C.MyClient.LocalMessage(2005, "You have received 5 Pumpkins!");
                }
            }
            else
                C.MyClient.LocalMessage(2005, "Please make some room in your inventory first!");
        }
    }
}