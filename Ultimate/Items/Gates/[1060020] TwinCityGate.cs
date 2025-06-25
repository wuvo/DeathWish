using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_1060020 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068 && C.Loc.Map != 1039)
            {
                C.Teleport(1002, 429, 378);
                C.RemoveItem(I);
                C.Invisible = false;
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }
    }
}
