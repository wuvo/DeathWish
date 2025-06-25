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
    public class NPC_12 : NPCBase
    {
        public NPC_12(Main.GameClient _client)
            : base(_client)
        {
            ID = 12;
            Face = 5;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            switch (_linkback)
            {
                case 0:
                case 1:
                    {
                        if (GC.MyChar.VipLevel >= 1)
                        {
                            if (!GC.MyChar.WHOpen && GC.MyChar.WHPassword != "0")

                            {
                                string Pass = ReadString(_data);
                                if (GC.MyChar.WHPassword == Pass)
                                { GC.MyChar.WHOpen = true; }
                                else if (Pass.Length > 0)
                                {
                                    if (GC.MyChar.WHErrors >= 1 && GC.MyChar.WHErrors < 4)
                                        GC.MyChar.WHErrors++;
                                }

                                if (GC.MyChar.WHErrors < 4 && !GC.MyChar.WHOpen)
                                {
                                    if (GC.MyChar.WHErrors == 0)
                                    {
                                        GC.MyChar.WHErrors = 1;
                                        GC.AddSend(Packets.NPCSay("Please put your warehouse password to open it."));
                                    }
                                    else
                                    {
                                        GC.AddSend(Packets.NPCSay("Wrong! You have " + (4 - GC.MyChar.WHErrors) + " more times to try."));
                                    }
                                    GC.AddSend(Packets.NPCLink2("Password.", 1));
                                    GC.AddSend(Packets.NPCLink("Let me think.", 255));
                                    GC.AddSend(Packets.NPCSetFace(1));
                                    GC.AddSend(Packets.NPCFinish());
                                    return;
                                }
                                else if (GC.MyChar.WHErrors == 4)
                                {
                                    GC.AddSend(Packets.NPCSay("You have to logoff and login to try it again."));
                                    GC.AddSend(Packets.NPCLink("I See.", 255));
                                    GC.AddSend(Packets.NPCSetFace(1));
                                    GC.AddSend(Packets.NPCFinish());
                                    return;
                                }
                            }
                            else if (GC.MyChar.WHPassword.Length == 1)
                            {
                                if (GC.MyChar.Loc.Map == 1038 || World.EventsMaps.Contains(GC.MyChar.Loc.Map) || GC.MyChar.Loc.Map == 8001)
                                    GC.LocalMessage(2000, "VIP Warehouse cannot be used in this map!");
                                else
                                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 341, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x7e));
                            }
                            else
                            {
                                if (GC.MyChar.Loc.Map == 1038 || World.EventsMaps.Contains(GC.MyChar.Loc.Map) || GC.MyChar.Loc.Map == 8001)
                                    GC.LocalMessage(2000, "VIP Warehouse cannot be used in this map!");
                                else
                                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 341, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x7e));
                            }

                        }
                        else
                        {
                            GC.LocalMessage(2000, "You require VIP 1 or more to use this function.");
                        }
                        break;
                    }
            }
        }
    }
}