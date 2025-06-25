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
    public class NPC_35500 : NPCBase
    {
        public NPC_35500(Main.GameClient _client)
            : base(_client)
        {
            ID = 35500;
            Face = 56;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I am the mighty tao who teaches skills to those worthy reborn characters. So do you want to learn anything?");
                        AddOption("Teach me Summon Guard.", 1);
                        AddOption("Nah. I'll just leave.", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Reborn)
                            GC.MyChar.NewSkill(new Game.Skill() { ID = 4000, Lvl = 0 });
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}