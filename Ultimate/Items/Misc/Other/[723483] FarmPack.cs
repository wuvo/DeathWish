using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_723483 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 31)
            {
                C.RemoveItem(C.NextItem(723483));//Promote Pack

                for (int rnd = 0; rnd < 8; rnd++)
                {
                    uint id = 0;
                    switch (rnd)
                    {
                        case 0: id = 1080001; break;//emerald
                        case 1: id = 721080; break;//moonbox
                        case 2: id = 721259; break;//CelestialStone
                        case 3: id = 1072031; break;//euxenite
                        case 4: id = 1072031; break;//euxenite
                        case 5: id = 1072031; break;//euxenite
                        case 6: id = 1072031; break;//euxenite
                        case 7: id = 1072031; break;//euxenite
                        case 8: id = 1088001; break;//Meteor
                    }
                    if (id != 0)
                        C.MyClient.MyChar.AddItem(id, 0);
                }
            }
            else C.MyClient.LocalMessage(2005, "You need 8 free spots!");
        }
    }
}