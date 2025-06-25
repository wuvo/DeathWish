using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720163 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.MyClient.LocalMessage(2005, "Snowman is melting and he needs your help. Bring this snowball to him as soon as possible in the Twin City(508,314).");
        }
    }
}
