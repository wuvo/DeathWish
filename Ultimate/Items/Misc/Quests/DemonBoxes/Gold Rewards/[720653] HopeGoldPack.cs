using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_720653 : IItem
    {
        public override void Run(Character C, Item I)
        {
            uint amount = 25000;
            if (I.ID == 720654 || I.ID == 720659)
                amount = 50000;
            else if (I.ID == 720655 || I.ID == 720660)
                amount = 100000;
            else if (I.ID == 720656)
                amount = 1350000;
            else if (I.ID == 720657)
                amount = 2700000;
            else if (I.ID == 720661)
                amount = 200000;
            else if (I.ID == 720662)
                amount = 3000000;
            else if (I.ID == 720663)
                amount = 5400000;
            else if (I.ID == 720665)
                amount = 250000;
            else if (I.ID == 720666 || I.ID == 720675)
                amount = 500000;
            else if (I.ID == 720667 || I.ID == 720676)
                amount = 1000000;
            else if (I.ID == 720668)
                amount = 13500000;
            else if (I.ID == 720669 || I.ID == 720678)
                amount = 27000000;
            else if (I.ID == 720677)
                amount = 2000000;
            else if (I.ID == 720679)
                amount = 54000000;
            else if (I.ID == 720681)
                amount = 2500000;
            else if (I.ID == 720682 || I.ID == 720687)
                amount = 5000000;
            else if (I.ID == 720683 || I.ID == 720688 || I.ID == 720693)
                amount = 10000000;
            else if (I.ID == 720684)
                amount = 135000000;
            else if (I.ID == 720685 || I.ID == 720686)
                amount = 270000000;
            else if (I.ID == 720689 || I.ID == 720694)
                amount = 20000000;
            else if (I.ID == 720691 || I.ID == 720696)
                amount = 540000000;
            else if (I.ID == 720695)
                amount = 40000000;
            if (C.Silvers + amount <= 2000000000)
            {
                C.Silvers += amount; C.MyClient.LocalMessage(2005, "You have received " + amount + " silvers."); C.RemoveItem(I.UID); World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, "eidolon")).Get);
            }
            else
                C.MyClient.LocalMessage(2005, "You can't have more than 2,000,000,000 silvers in your inventory!");
        }
    }
}