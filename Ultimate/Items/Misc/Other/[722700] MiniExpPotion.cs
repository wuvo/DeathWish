using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_722700 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (!C.ExpPotUnder70)
            {
                C.RemoveItem(I);
                C.ExpPotionUsed = DateTime.Now;
                C.DoubleExp = true;
                C.DoubleExpLeft = 600;
                C.MyClient.AddSend(Packets.Status(C.EntityID, Status.DoubleExpTime, (ulong)C.DoubleExpLeft));
            }
            else
                C.MyClient.LocalMessage(2000, "Free Double Exp is already in effect!");
        }
    }
}
