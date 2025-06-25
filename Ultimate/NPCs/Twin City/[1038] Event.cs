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
    public class NPC_1038 : NPCBase
    {
        string Message = "";
        public NPC_1038(Main.GameClient _client)
            : base(_client)
        {
            ID = 1038;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            string[] Cmd = Message.Split(' ');
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("You can visit me when an event starts or you can use @joinpvp command sir!");
                        AddText("Do you want to join event ?");
                        AddOption("Yes i want to join event.", 1);
                        AddOption("No, im not  ready.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.EventBase == null)
                        {
                            if (World.Events.Count > 0)
                            {
                                if (World.Events.Count == 1)
                                {
                                    if (World.Events[0].AddPlayer(GC.MyChar))
                                        GC.MyChar.EventBase = World.Events[0];
                                }
                                else if (World.Events.Count > 1)
                                {
                                    if (Cmd.Length == 1)
                                    {
                                        string titles = "";
                                        for (int a = 0; a < World.Events.Count; a++)
                                            titles += $"{World.Events[a].EventTitle}: {a},";
                                        GC.LocalMessage(2000, titles);
                                        GC.LocalMessage(2000, "More than one event is running. Please type @joinpvp X where 'X' corresponds to the event ID.");
                                    }
                                    else
                                    {
                                        int a = 0;
                                        bool b = int.TryParse(Cmd[1], out a);
                                        if (b)
                                            if (World.Events.Count >= a)
                                                if (World.Events[a].AddPlayer(GC.MyChar))
                                                    GC.MyChar.EventBase = World.Events[a];
                                    }
                                }
                            }
                            else
                                GC.LocalMessage(2000, "There are no PVP Events running!");
                        }
                        break;
                    }

            }

            AddFinish();
            Send();
        }
    }
}