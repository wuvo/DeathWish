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
    public class NPC_2040 : NPCBase
    {
        public NPC_2040(Main.GameClient _client)
            : base(_client)
        {
            ID = 2040;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.Job >= 132 && GC.MyChar.Job <= 135)
                        {
                            AddText("If you give me 5,000 silvers, I will help you to create a SpaceMark. When you'll use it, you will be teleported here.");
                            AddOption("Help me, please", 1);
                            AddOption("Just passing by", 255);
                        }
                        else
                        {
                            AddText("Only the water taoists are allowed to use the space marks.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Silvers >= 5000 && GC.MyChar.Inventory.Count < 40)
                        {
                            GC.MyChar.Silvers -= 5000;
                            if (ID >= 2040 && ID <= 2044)
                                GC.MyChar.AddItem(ID + 1057985);
                            else if (ID >= 2045 && ID <= 2049)
                                GC.MyChar.AddItem(ID + 1057986);
                            else
                                GC.MyChar.AddItem(ID + 1057987);
                            AddText("Here you go! Right-click on the SpaceMark and you'll be teleported here!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems you don't have 5,000 silvers or your inventory is full!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_2041 : NPC_2040
    {
        public NPC_2041(Main.GameClient _client)
            : base(_client)
        {
            ID = 2041;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2042 : NPC_2040
    {
        public NPC_2042(Main.GameClient _client)
            : base(_client)
        {
            ID = 2042;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2043 : NPC_2040
    {
        public NPC_2043(Main.GameClient _client)
            : base(_client)
        {
            ID = 2043;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2044 : NPC_2040
    {
        public NPC_2044(Main.GameClient _client)
            : base(_client)
        {
            ID = 2044;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2045 : NPC_2040
    {
        public NPC_2045(Main.GameClient _client)
            : base(_client)
        {
            ID = 2045;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2046 : NPC_2040
    {
        public NPC_2046(Main.GameClient _client)
            : base(_client)
        {
            ID = 2046;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2047 : NPC_2040
    {
        public NPC_2047(Main.GameClient _client)
            : base(_client)
        {
            ID = 2047;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2048 : NPC_2040
    {
        public NPC_2048(Main.GameClient _client)
            : base(_client)
        {
            ID = 2048;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2049 : NPC_2040
    {
        public NPC_2049(Main.GameClient _client)
            : base(_client)
        {
            ID = 2049;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2050 : NPC_2040
    {
        public NPC_2050(Main.GameClient _client)
            : base(_client)
        {
            ID = 2050;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2051 : NPC_2040
    {
        public NPC_2051(Main.GameClient _client)
            : base(_client)
        {
            ID = 2051;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}