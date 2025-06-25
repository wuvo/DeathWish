using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_722176 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.MyClient.LocalMessage(2005, "Take 2 or more pumpkins to King Jack at middle TC and exchange every 2 pumpkins for 1 pumpkin point.");
        }
    }
}