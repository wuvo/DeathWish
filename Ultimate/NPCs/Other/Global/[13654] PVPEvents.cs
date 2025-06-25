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
    public class NPC_13654 : NPCBase
    {
        public NPC_13654(Main.GameClient _client)
            : base(_client)
        {
            ID = 13654;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        //AddText(World.HourlyEvent.EventTitle + " is about to start! Do you want to join the fight?\nNote: If you want to enable/disable these invitations type /invitations");
                        //AddOption("Count me in", 1);
                        //AddOption("Nevermind", 255);
                        GC.AddSend(Packets.PopUp(World.HourlyEvent + " is about to start! Do you want to join the fight?", 1));
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
                                    string titles = "";
                                    for (int a = 0; a < World.Events.Count; a++)
                                        titles += $"{World.Events[a].EventTitle}: {a},";
                                    GC.LocalMessage(2000, titles);
                                    GC.LocalMessage(2000, "More than one event is running. Please type @joinpvp X where 'X' corresponds to the event ID.");
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