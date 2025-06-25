using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_723584 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Equips.Armor.ID != 0)
            {
                C.RemoveItem(I);
                C.Equips.Armor.Color = Item.ArmorColor.Black;
                C.MyClient.AddSend(Packets.UpdateItem(C.Equips.Armor, 3));
                //Equips.Replace(3, Equips.Armor, this);
                World.Spawn(C, true);

            }
            else
                C.MyClient.LocalMessage(2005, "Please wear an armor before using the BlackTulip!");
        }
    }
}
