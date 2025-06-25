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
    public class NPC_7381 : NPCBase
    {
        public NPC_7381(Main.GameClient _client)
            : base(_client)
        {
            ID = 7381;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("What is the matter? If there is nothing special, do not disturb me.");
                        AddOption("Can you let me out?", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (!Features.GuildWars.War)
                            GC.MyChar.Teleport(1002, 430, 380);
                        else
                        {
                            if ((DateTime.Now.Minute >= 0 && DateTime.Now.Minute <= 5) || (DateTime.Now.Minute >= 30 && DateTime.Now.Minute <= 35))
                            {
                                GC.MyChar.Teleport(1002, 430, 380);
                            }
                            else
                            {
                                AddText("Calm down and stay here. Learn what peace and love is. I will let you out later. Time : 00-05 - 30:35");
                                AddOption("I see", 255);
                                
                            }
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}