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
    public class NPC_3298 : NPCBase
    {
        public NPC_3298(Main.GameClient _client)
            : base(_client)
        {
            ID = 3298;
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
                        AddText("With these SoulStones I can help you enter the HellGate shielded from poisonous fogs. Are you ready?");
                        AddOption("I'm ready", 1);
                        AddOption("Wait a moment", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(723085, 5))
                        {
                            if (World.Dis2 < 50)
                            {
                                GC.MyChar.Teleport(2022, 220, 340);
                                if (GC.MyChar.Level < 130)
                                    GC.MyChar.AddExp(1);
                                for (int a = 0; a < 5; a++)
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(723085));

                                World.Dis2++;
                                if (GC.MyChar.Job >= 10 && GC.MyChar.Job <= 15)
                                    GC.MyChar.DisToKill = 800;
                                else if (GC.MyChar.Job >= 20 && GC.MyChar.Job <= 25)
                                    GC.MyChar.DisToKill = 900;
                                else if (GC.MyChar.Job >= 40 && GC.MyChar.Job <= 45)
                                    GC.MyChar.DisToKill = 1300;
                                else if (GC.MyChar.Job >= 132 && GC.MyChar.Job <= 135)
                                    GC.MyChar.DisToKill = 600;
                                else if (GC.MyChar.Job >= 142 && GC.MyChar.Job <= 145)
                                    GC.MyChar.DisToKill = 1000;
                                else
                                    GC.MyChar.DisToKill = 800;
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has passed through the HellGate and have entered in the HellHall, ranked " + World.Dis2 + "!", 2011, 0);
                            }
                            else
                            {
                                foreach (Character C in World.H_Chars.Values)
                                {
                                    if (C.Loc.Map == 2021)
                                    {
                                        C.Teleport(1020, 566, 564);
                                        AddText("Sorry but all the 50 places were taken. Come back next time.");
                                        AddOption("Oh damn...", 255);
                                    }
                                }
                                World.Dis2 = 0;
                            }
                        }
                        else
                        {
                            AddText("Sorry you don't have 5 SoulStones");
                            AddOption("Oh sorry, will got them!", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}