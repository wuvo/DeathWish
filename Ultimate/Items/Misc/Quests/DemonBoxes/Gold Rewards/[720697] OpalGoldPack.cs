using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720697 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count < 39)
            {
                for (int a = 0; a < 2; a++)
                    C.AddItem(720696);
                World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, "eidolon")).Get);
            }
            else
                C.MyClient.LocalMessage(2005, "Please make some room in your inventory!");
        }
    }
}