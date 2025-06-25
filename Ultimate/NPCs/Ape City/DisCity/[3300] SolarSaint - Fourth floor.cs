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
    public class NPC_3300 : NPCBase
    {
        public NPC_3300(Main.GameClient _client)
            : base(_client)
        {
            ID = 3300;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.InventoryContains(790001, 1))
                        {
                            AddText("Hooray! Thanks to you " + GC.MyChar.Name + " Ultimate Pluto has been defeated! Here take this Sword and see me in Ape Mountain for the reward!");
                            AddOption("Thanks!", 1);
                            break;
                        }
                        else
                        {
                            AddText("Hurry up and kill Ultimate Pluto! Don't forget to bring the Dark Horn!");
                            AddOption("Ok.!", 255);
                            break;
                        }
                    }
                case 1:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(790001));
                        GC.MyChar.AddItem(723088);
                        AddText("Here you are!");
                        AddOption("Thanks", 255);
                        foreach (Character C in World.H_Chars.Values)
                        {
                            if (C.Loc.Map == 2024 || C.Loc.Map == 2023 || C.Loc.Map == 2022 || C.Loc.Map == 2021)
                                C.Teleport(1020, 566, 564);
                        }
                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has delivered the DarkHorn to SolarSaint and DisCity has ended!", 2011, 0);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}