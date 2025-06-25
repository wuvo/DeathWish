using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.Items
{
    public class Item_1060025 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068 && C.Loc.Map != 1039)
            {
                C.Teleport(1002, 411, 704 + 2);
                C.RemoveItem(I);
                C.Invisible = false;
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060026 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1002, 96, 323 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060027 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1002, 795, 465 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060028 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1011, 538, 772 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060029 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1011, 734, 452 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060031 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1020, 824, 601 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060032 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1020, 491, 731 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060033 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1020, 106, 394 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060034 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1000, 225, 205 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060035 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1000, 793, 549 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060037 : IItem
    {

        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1001, 470, 366 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }

    }
    public class Item_1060038 : IItem
    {
        public override void Run(Character C, Item I)

        {
            if (C.Loc.Map != 6000 && !World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 6001 && C.Loc.Map != 1210 && C.Loc.Map != 1211 && C.Loc.Map != 1212 && C.Loc.Map != 1049 && C.Loc.Map != 6003 && C.Loc.Map != 1029 && C.Loc.Map != 2024 && C.Loc.Map != 2068 && C.Loc.Map != 2068)
            {
                C.Teleport(1011, 67, 423 + 2);
                C.RemoveItem(I);
            }
            else
                C.MyClient.LocalMessage(2005, "Cannot use teleport scrolls in here.");
        }
    }
}