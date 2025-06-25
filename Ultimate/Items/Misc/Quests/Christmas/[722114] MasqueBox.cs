using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Structures;

namespace Ultimate.Items
{
    public class Item_722114 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count < 36)
            {
                C.RemoveItem(I.UID);
                List<uint> Plates = new List<uint>() { 722107, 722108, 722109, 722110, 722111, 722113 };
                Random Rnd = new Random();
                for (int a = 0; a < 5; a++)
                {
                    if (a < 3)
                        C.AddItem(Plates[Rnd.Next(0, Plates.Count)]);
                    else if (MyMath.ChanceSuccess(20))
                        C.AddItem(Plates[Rnd.Next(0, Plates.Count)]);
                }
            }
            else
                C.MyClient.LocalMessage(2005, "Please make some room in your inventory!");
        }
    }
}
