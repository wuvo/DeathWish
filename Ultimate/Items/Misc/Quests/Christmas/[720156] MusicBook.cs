using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720156 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.MyClient.LocalMessage(2005, "You have learnt the whole carol. Please hand it to ConductorDarwen in the Twin City(428,261).");
        }
    }
}
