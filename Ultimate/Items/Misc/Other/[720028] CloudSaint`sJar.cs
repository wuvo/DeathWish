using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_750000 : IItem
    {
        public override void Run(Character C, Item I)
        {
            C.MyClient.LocalMessage(2005, $"You currently have {C.CurrentKills} {(Cloudsaint.MonsterType)I.MaxDur} souls out of {I.CurDur} required. Please deliver the jar to the Captain once it's full!");
        }
    }
}