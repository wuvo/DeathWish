using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721851 : IItem
    {
        public override void Run(Character C, Item I)
        {
            //C.VIPUsage = I;
            C.MyClient.DialogNPC = 780101;
            NPCs.NPCHandler.Handle(C.MyClient, null, 780101, 0);
        }
    }
    }