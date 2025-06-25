using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721955 : IItem
    {
        public override void Run(Character C, Item I)
        {
            //C.VIPUsage = I;
            C.MyClient.DialogNPC = 780103;
            NPCs.NPCHandler.Handle(C.MyClient, null, 780103, 0);
        }
    }
    }