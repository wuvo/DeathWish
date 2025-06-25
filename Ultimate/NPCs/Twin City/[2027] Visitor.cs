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
    public class NPC_2027 : NPCBase
    {
        public NPC_2027(Main.GameClient _client)
            : base(_client)
        {
            ID = 2027;
            Face = 7;
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
                        AddText("The interserver allows you to battle players from other servers! Would you like to join the battle?");
                        AddOption("Join Interserver", 1);
                        AddOption("What is the Interserver?", 3);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("The interserver is currently inactive.");
                        AddOption("I see", 255);
                        //if (World.Interserver == 0)
                        //    GC.MyChar.ConnectToInterserver(!Features.InterserverManager.SEND_NA);
                        //else
                        //    GC.MyChar.ConnectToInterserver(Features.InterserverManager.SEND_NA);
                        break;
                    }
                case 3:
                    {
                        AddText("The interserver allows you to with and against members of other servers without having to relog or transfer characters. ");
                        AddText("You can join either a NA or EU  hosted interserver network to join pvp events or to fight for glory. When finished simply talk to ");
                        AddText("the NPC and you will return to me!");
                        AddOption("Which servers are in the Interserver?", 4);
                        break;
                    }
                case 4:
                    {
                        AddText("Currently, the servers that have joined the Interserver are: Throne Conquer, Apex Conquer, Triumph Conquer, Original Conquer and Raw Conquer! Make sure you ");
                        AddText("join the fight and avenge your server!");
                        AddOption("Thanks", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}