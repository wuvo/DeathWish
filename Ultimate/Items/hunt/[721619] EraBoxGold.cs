using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721619 : IItem
    {
        public override void Run(Character C, Item I)
        {
            //C.VIPUsage = I;
            C.MyClient.DialogNPC = 780102;
            NPCs.NPCHandler.Handle(C.MyClient, null, 780102, 0);
        }
    }
    }