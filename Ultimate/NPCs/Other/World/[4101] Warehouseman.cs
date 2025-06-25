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
    public class NPC_4101 : NPC_8
    {
        public NPC_4101(Main.GameClient _client)
            : base(_client)
        {
            ID = 4101;
            Face = 5;
        }
    }
    public class NPC_2100 : NPC_8
    {
        public NPC_2100(Main.GameClient _client)
            : base(_client)
        {
            ID = 2100;
            Face = 5;
        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            if (GC.MyChar.VipLevel == 5 || GC.MyChar.VipLevel == 6)
            {
                Responses = new List<COPacket>();
                AddAvatar();

                switch (_linkback)
                {
                    case 0:
                        if (!GC.MyChar.WHOpen && GC.MyChar.WHPassword != "0")
                        {
                            if (GC.MyChar.WHErrors < 4 && !GC.MyChar.WHOpen)
                            {
                                if (GC.MyChar.WHErrors == 0)
                                {
                                    GC.MyChar.WHErrors = 1;
                                    GC.AddSend(Packets.NPCSay("It seems like your warehouse is protected! Please provide me with the password!"));
                                }
                                else
                                    GC.AddSend(Packets.NPCSay("Wrong! You have " + (4 - GC.MyChar.WHErrors) + " more times to try."));
                                GC.AddSend(Packets.NPCLink2("Password:", 1));
                                GC.AddSend(Packets.NPCLink("Let me think.", 255));
                                GC.AddSend(Packets.NPCSetFace(1));
                                GC.AddSend(Packets.NPCFinish());
                                return;
                            }
                            else if (GC.MyChar.WHErrors == 4)
                            {
                                GC.AddSend(Packets.NPCSay("You have used too many tries already. Please log-off and try again."));
                                GC.AddSend(Packets.NPCLink("I see", 255));
                                GC.AddSend(Packets.NPCSetFace(1));
                                GC.AddSend(Packets.NPCFinish());
                                return;
                            }
                        }
                        else if (GC.MyChar.WHPassword.Length == 1)
                        {
                            GC.LocalMessage(2005, "To protect your items, please put a password in the WHSGuardian in TwinCity.");
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        }
                        else
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        break;
                    case 1:
                        string Pass = ReadString(_data);
                        if (GC.MyChar.WHPassword == Pass)
                        {
                            GC.MyChar.WHOpen = true;
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        }
                        else if (Pass.Length > 0)
                        {
                            if (GC.MyChar.WHErrors >= 1 && GC.MyChar.WHErrors < 4)
                                GC.MyChar.WHErrors++;
                        }
                        break;
                }
            }
            else
            {
                GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use VipBank you need to be VIP5!");
            }
        }
    }
    public class NPC_2101 : NPC_8
    {
        public NPC_2101(Main.GameClient _client)
            : base(_client)
        {
            ID = 2101;
            Face = 5;
        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            if (GC.MyChar.VipLevel == 5 || GC.MyChar.VipLevel == 6)
            {
                Responses = new List<COPacket>();
                AddAvatar();

                switch (_linkback)
                {
                    case 0:
                        if (!GC.MyChar.WHOpen && GC.MyChar.WHPassword != "0")
                        {
                            if (GC.MyChar.WHErrors < 4 && !GC.MyChar.WHOpen)
                            {
                                if (GC.MyChar.WHErrors == 0)
                                {
                                    GC.MyChar.WHErrors = 1;
                                    GC.AddSend(Packets.NPCSay("It seems like your warehouse is protected! Please provide me with the password!"));
                                }
                                else
                                    GC.AddSend(Packets.NPCSay("Wrong! You have " + (4 - GC.MyChar.WHErrors) + " more times to try."));
                                GC.AddSend(Packets.NPCLink2("Password:", 1));
                                GC.AddSend(Packets.NPCLink("Let me think.", 255));
                                GC.AddSend(Packets.NPCSetFace(1));
                                GC.AddSend(Packets.NPCFinish());
                                return;
                            }
                            else if (GC.MyChar.WHErrors == 4)
                            {
                                GC.AddSend(Packets.NPCSay("You have used too many tries already. Please log-off and try again."));
                                GC.AddSend(Packets.NPCLink("I see", 255));
                                GC.AddSend(Packets.NPCSetFace(1));
                                GC.AddSend(Packets.NPCFinish());
                                return;
                            }
                        }
                        else if (GC.MyChar.WHPassword.Length == 1)
                        {
                            GC.LocalMessage(2005, "To protect your items, please put a password in the WHSGuardian in TwinCity.");
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        }
                        else
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        break;
                    case 1:
                        string Pass = ReadString(_data);
                        if (GC.MyChar.WHPassword == Pass)
                        {
                            GC.MyChar.WHOpen = true;
                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 4, 0, 0, 0x7e));
                        }
                        else if (Pass.Length > 0)
                        {
                            if (GC.MyChar.WHErrors >= 1 && GC.MyChar.WHErrors < 4)
                                GC.MyChar.WHErrors++;
                        }
                        break;
                }
            }
            else
            {
                GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use VipBank you need to be VIP5!");
            }
        }
    }
}
