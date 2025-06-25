using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721177 : IItem
    {
        public override void Run(Character C, Item I)
        {
            NPC NPCInfo = new NPC();
            NPCInfo.EntityID = Convert.ToUInt16(Program.Rnd.Next(30000, 50000));
            if (I.ID == 721177)
                NPCInfo.Type = 10;
            else if (I.ID == 721178)
                NPCInfo.Type = 21;
            else if (I.ID == 721179)
                NPCInfo.Type = 37;
            else if (I.ID == 721181)
                NPCInfo.Type = 51;
            else if (I.ID == 721182)
                NPCInfo.Type = 61;
            else if (I.ID == 721183)
                NPCInfo.Type = 71;
            else if (I.ID == 721184)
                NPCInfo.Type = 81;
            else if (I.ID == 721185)
                NPCInfo.Type = 91;
            else if (I.ID == 721186)
                NPCInfo.Type = 101;
            else if (I.ID == 721187)
                NPCInfo.Type = 111;
            else if (I.ID == 721180)
                NPCInfo.Type = 41;
            else if (I.ID == 721188)
                NPCInfo.Type = 121;
            else if (I.ID == 720391)
                NPCInfo.Type = 570;
            else if (I.ID == 720392)
                NPCInfo.Type = 540;
            else if (I.ID == 721229)
                NPCInfo.Type = 130;
            else if (I.ID == 721230)
                NPCInfo.Type = 140;
            else if (I.ID == 721231)
                NPCInfo.Type = 150;
            else if (I.ID == 721232)
                NPCInfo.Type = 160;
            else if (I.ID == 721233)
                NPCInfo.Type = 170;
            else if (I.ID == 721234)
                NPCInfo.Type = 180;
            else if (I.ID == 721235)
                NPCInfo.Type = 190;
            else if (I.ID == 721225)
                NPCInfo.Type = 200;
            else if (I.ID == 721226)
                NPCInfo.Type = 210;
            else if (I.ID == 721227)
                NPCInfo.Type = 220;
            else if (I.ID == 721228)
                NPCInfo.Type = 230;
            else if (I.ID == 720164)
                NPCInfo.Type = 410;
            else if (I.ID == 720165)
                NPCInfo.Type = 380;
            else if (I.ID == 720166)
                NPCInfo.Type = 390;
            else if (I.ID == 720167)
                NPCInfo.Type = 400;

            //else if (I.ID == 721189)
            //{
            //    NPCInfo.Type = 8200;
            //}
            NPCInfo.Flags = 26;
            NPCInfo.Avatar = 188;
            NPCInfo.Loc = new Location();
            C.MyClient.AddSend(Packets.PlaceNPC(NPCInfo));
        }
    }
}