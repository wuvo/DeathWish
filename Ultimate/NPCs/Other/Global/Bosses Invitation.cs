using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_2094 : NPCBase
    {
        public NPC_2094(Main.GameClient _client)
            : base(_client)
        {
            ID = 2094;
            Face = 13;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
              
            switch (_linkback)
            {
                case 0:
                    if (GC.MyChar.Loc.Map == 1000 || GC.MyChar.Loc.Map == 1011 || GC.MyChar.Loc.Map == 1020 || GC.MyChar.Loc.Map == 1015 || GC.MyChar.Loc.Map == 1002)
                    {
                        GC.AddSend(Packets.PopUp("Do you want to fight the " + World.CurrentBoss + "?", 1));
                    }
                    break;
                case 1:
                    if (World.CurrentBoss == "ThrillingSpook")
                        GC.MyChar.Teleport(1015, 710, 925);
                    else if (World.CurrentBoss == "Capricorn")
                        GC.MyChar.Teleport(1011, 799, 465);
                    else if (World.CurrentBoss == "Tash")
                        GC.MyChar.Teleport(1000, 496, 301);
                    else if (World.CurrentBoss == "Raikou")
                        GC.MyChar.Teleport(1002, 375, 415);
                    //else if (World.Titan == true)
                    //    GC.MyChar.Teleport(1020, (ushort)(Program.Rnd.Next(1 + 385, 7 + 385)), (ushort)(Program.Rnd.Next(1 + 585, 7 + 585)));
                    //else if (World.Gano == true)
                    //    GC.MyChar.Teleport(1011, (ushort)(Program.Rnd.Next(1 + 659, 7 + 659)), (ushort)(Program.Rnd.Next(1 + 772, 7 + 772)));
                    break;
            }

            AddFinish();
            Send();
        }

    }
}