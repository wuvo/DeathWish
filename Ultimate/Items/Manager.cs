using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Manager
    {
        public static void ProcessItem(Character C, Item I)
        {
            try
            {
                var type = Type.GetType("Ultimate.Items.Item_" + I.ID);
                var item = Activator.CreateInstance(type) as IItem;
                if (C.Loc.Map == 700 || C.Loc.Map == 1080 || C.Loc.Map == 1017 || (World.EventsMaps.Contains(C.Loc.Map) && C.Loc.Map != 1844) || (C.Loc.Map >= 10000) && DMaps.EventMaps.ContainsKey(C.Loc.Map))
                {
                    if (C.Loc.Map == 10200)
                    {
                        item.Run(C, I);
                    }
                    else
                    { 
                    C.MyClient.LocalMessage(2005, "Can't use items in this map!");
                    }
                    return;
                }
                else
                    item.Run(C, I);
            }
            catch
            {
                if (C.MyClient.PM)
                {
                    C.MyClient.LocalMessage(2000, "Could not load script for item ID: " + I.ID);
                }
            }
        }
    }
}
