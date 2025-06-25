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
    public class NPC_101602 : NPCBase
    {
        public NPC_101602(Main.GameClient _client)
            : base(_client)
        {
            ID = 101602;
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
                        AddText("Want to go to training grounds? You need to pay me 1000 silvers to do that.");
                        AddOption("Yeah.", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 1000)
                        {
                            GC.MyChar.Silvers -= 1000;
                            GC.MyChar.Teleport(1039, 219, 223);
                            GC.MyChar.Protection = true;
                            AddText("Ok. Here you are. You can only hit dummies that are of your level or lower.");
                            AddOption("Ok then.", 255);
                        }
                        else
                        {
                            AddText("As i said... you need 1000 silvers. Why do i need to repeat myself?");
                            AddOption("Alright alright.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_101616 : NPC_101602
    {
        public NPC_101616(Main.GameClient _client)
            : base(_client)
        {
            ID = 101616;
            Face = 67;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_101618 : NPC_101602
    {
        public NPC_101618(Main.GameClient _client)
            : base(_client)
        {
            ID = 101618;
            Face = 67;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_101614 : NPC_101602
    {
        public NPC_101614(Main.GameClient _client)
            : base(_client)
        {
            ID = 101614;
            Face = 67;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_101620 : NPC_101602
    {
        public NPC_101620(Main.GameClient _client)
            : base(_client)
        {
            ID = 101620;
            Face = 67;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}