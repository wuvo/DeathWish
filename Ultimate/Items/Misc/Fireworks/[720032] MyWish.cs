using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720032 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.RemoveItem(I);
            C.SendScreen(Packets.StringPacket(C.MyClient.MyChar.EntityID, StringType.Effect, "firework-2love"));
        }
    }
}
