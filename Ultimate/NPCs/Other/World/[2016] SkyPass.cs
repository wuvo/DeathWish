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
    public class NPC_2016 : NPCBase
    {
        public NPC_2016(Main.GameClient _client)
            : base(_client)
        {
            ID = 2016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            byte skynpc = Convert.ToByte((ID - 2016) + 1);

            uint skyrnd = (uint)Program.Rnd.Next(2);
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, sugar! We brothers watch the battle stages of different levels on this floor. Which stage you need ");
                        AddText("to conquer depends on your ability. When the test starts, you must defeat the monsters on the stage and collect a Pass Token to continue your challenge. So?");
                        AddOption("I'm ready", (byte)skynpc);
                        AddOption("Return to Twin City", 6);
                        AddOption("I'll think it over", 255);
                        break;
                    }
                case 1:
                    {
                        if (skyrnd == 1)
                            GC.MyChar.Teleport(1040, 543, 330);
                        else
                            GC.MyChar.Teleport(1040, 368, 588);
                        break;
                    }
                case 2:
                    {
                        if (skyrnd == 1)
                            GC.MyChar.Teleport(1040, 492, 280);
                        else
                            GC.MyChar.Teleport(1040, 320, 540);
                        break;
                    }
                case 3:
                    {
                        if (skyrnd == 1)
                            GC.MyChar.Teleport(1040, 436, 224);
                        else
                            GC.MyChar.Teleport(1040, 272, 492);
                        break;
                    }
                case 4:
                    {
                        if (skyrnd == 1)
                            GC.MyChar.Teleport(1040, 393, 181);
                        else
                            GC.MyChar.Teleport(1040, 224, 444);
                        break;
                    }
                case 5:
                    {
                        if (skyrnd == 1)
                        {
                            if (GC.MyChar.Inventory.Count < 40)
                            {
                                GC.MyChar.AddItem(721109);
                                AddText("Here you go! Deliver this SkyPrizeToken to God Cloud and claim your reward!");
                                AddOption("Thanks", 255);
                                GC.MyChar.Teleport(1040, 141, 240);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                            GC.MyChar.Teleport(1040, 176, 396);
                        break;
                    }
                case 6:
                    {
                        GC.MyChar.Teleport(1002, 429, 378);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_2017 : NPC_2016
    {
        public NPC_2017(Main.GameClient _client)
            : base(_client)
        {
            ID = 2017;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2018 : NPC_2016
    {
        public NPC_2018(Main.GameClient _client)
            : base(_client)
        {
            ID = 2018;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2019 : NPC_2016
    {
        public NPC_2019(Main.GameClient _client)
            : base(_client)
        {
            ID = 2019;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2020 : NPC_2016
    {
        public NPC_2020(Main.GameClient _client)
            : base(_client)
        {
            ID = 2020;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2059 : NPC_2016
    {
        public NPC_2059(Main.GameClient _client)
            : base(_client)
        {
            ID = 2016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2060 : NPC_2016
    {
        public NPC_2060(Main.GameClient _client)
            : base(_client)
        {
            ID = 2016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2061 : NPC_2016
    {
        public NPC_2061(Main.GameClient _client)
            : base(_client)
        {
            ID = 2016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2062 : NPC_2016
    {
        public NPC_2062(Main.GameClient _client)
            : base(_client)
        {
            ID = 2016;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2063 : NPC_2016
    {
        public NPC_2063(Main.GameClient _client)
            : base(_client)
        {
            ID = 2017;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2064 : NPC_2016
    {
        public NPC_2064(Main.GameClient _client)
            : base(_client)
        {
            ID = 2017;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2066 : NPC_2016
    {
        public NPC_2066(Main.GameClient _client)
            : base(_client)
        {
            ID = 2017;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2067 : NPC_2016
    {
        public NPC_2067(Main.GameClient _client)
            : base(_client)
        {
            ID = 2017;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2068 : NPC_2016
    {
        public NPC_2068(Main.GameClient _client)
            : base(_client)
        {
            ID = 2018;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2069 : NPC_2016
    {
        public NPC_2069(Main.GameClient _client)
            : base(_client)
        {
            ID = 2018;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2070 : NPC_2016
    {
        public NPC_2070(Main.GameClient _client)
            : base(_client)
        {
            ID = 2018;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2071 : NPC_2016
    {
        public NPC_2071(Main.GameClient _client)
            : base(_client)
        {
            ID = 2018;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2072 : NPC_2016
    {
        public NPC_2072(Main.GameClient _client)
            : base(_client)
        {
            ID = 2019;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2073 : NPC_2016
    {
        public NPC_2073(Main.GameClient _client)
            : base(_client)
        {
            ID = 2019;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2074 : NPC_2016
    {
        public NPC_2074(Main.GameClient _client)
            : base(_client)
        {
            ID = 2019;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2075 : NPC_2016
    {
        public NPC_2075(Main.GameClient _client)
            : base(_client)
        {
            ID = 2019;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2076 : NPC_2016
    {
        public NPC_2076(Main.GameClient _client)
            : base(_client)
        {
            ID = 2020;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2077 : NPC_2016
    {
        public NPC_2077(Main.GameClient _client)
            : base(_client)
        {
            ID = 2020;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2078 : NPC_2016
    {
        public NPC_2078(Main.GameClient _client)
            : base(_client)
        {
            ID = 2020;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2079 : NPC_2016
    {
        public NPC_2079(Main.GameClient _client)
            : base(_client)
        {
            ID = 2020;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}