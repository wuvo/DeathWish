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
    public class NPC_6726 : NPCBase
    {
        public NPC_6726(Main.GameClient _client)
            : base(_client)
        {
            ID = 6726;
            Face = 30;
            IsGlobal = true;
        }
    }

    public class NPC_6720 : NPCBase
    {
        public NPC_6720(Main.GameClient _client)
            : base(_client)
        {
            ID = 6720;
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
                        AddText("What do you want to do?");
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.CounterClock.LastWinner != null)
                            {
                                if (Features.CounterClock.LastWinner.GuildID == GC.MyChar.MyGuild.GuildID)
                                {
                                    AddOption("Get inside", 1);
                                    AddOption("Get outside", 2);
                                }
                            }
                        }
                        AddOption("Nothing", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1844, 150, 162);
                    break;
                case 2:
                    GC.MyChar.Teleport(1844, 188, 162);
                    break;
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_6721 : NPC_6720
    {
        public NPC_6721(Main.GameClient _client)
            : base(_client)
        {
            ID = 6721;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6722 : NPC_6720
    {
        public NPC_6722(Main.GameClient _client)
            : base(_client)
        {
            ID = 6722;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }

    public class NPC_6723 : NPCBase
    {
        public NPC_6723(Main.GameClient _client)
            : base(_client)
        {
            ID = 6723;
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
                        AddText("What do you want to do?");
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.CounterClock.LastWinner != null)
                            {
                                if (Features.CounterClock.LastWinner.GuildID == GC.MyChar.MyGuild.GuildID)
                                {
                                    AddOption("Get inside", 1);
                                    AddOption("Get outside", 2);
                                }
                            }
                        }
                        AddOption("Nothing", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1844, 104, 164);
                    break;
                case 2:
                    GC.MyChar.Teleport(1844, 150, 162);
                    break;
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_6724 : NPC_6723
    {
        public NPC_6724(Main.GameClient _client)
            : base(_client)
        {
            ID = 6724;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6725 : NPC_6723
    {
        public NPC_6725(Main.GameClient _client)
            : base(_client)
        {
            ID = 6725;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }

    public class NPC_6717 : NPCBase
    {
        public NPC_6717(Main.GameClient _client)
            : base(_client)
        {
            ID = 6717;
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
                        AddText("What do you want to do?");
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.CounterClock.LastWinner != null)
                            {
                                if (Features.CounterClock.LastWinner.GuildID == GC.MyChar.MyGuild.GuildID)
                                {
                                    AddOption("Get inside", 1);
                                    AddOption("Get outside", 2);
                                }
                            }
                        }
                        AddOption("Nothing", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1844, 188, 162);
                    break;
                case 2:
                    GC.MyChar.Teleport(1844, 225, 162);
                    break;
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_6718 : NPC_6717
    {
        public NPC_6718(Main.GameClient _client)
            : base(_client)
        {
            ID = 6718;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6719 : NPC_6717
    {
        public NPC_6719(Main.GameClient _client)
            : base(_client)
        {
            ID = 6719;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }

    public class NPC_6703 : NPCBase
    {
        public NPC_6703(Main.GameClient _client)
            : base(_client)
        {
            ID = 6703;
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
                        AddText("The zone ahead is under my protection! Not even members belonging to the winning guild can get through ");
                        AddText("without having to take me down first!");
                        AddOption("I see", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_6704 : NPC_6703
    {
        public NPC_6704(Main.GameClient _client)
            : base(_client)
        {
            ID = 6704;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6705 : NPC_6703
    {
        public NPC_6705(Main.GameClient _client)
            : base(_client)
        {
            ID = 6705;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6706 : NPC_6703
    {
        public NPC_6706(Main.GameClient _client)
            : base(_client)
        {
            ID = 6706;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6707 : NPC_6703
    {
        public NPC_6707(Main.GameClient _client)
            : base(_client)
        {
            ID = 6707;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6708 : NPC_6703
    {
        public NPC_6708(Main.GameClient _client)
            : base(_client)
        {
            ID = 6708;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6709 : NPC_6703
    {
        public NPC_6709(Main.GameClient _client)
            : base(_client)
        {
            ID = 6709;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6710 : NPC_6703
    {
        public NPC_6710(Main.GameClient _client)
            : base(_client)
        {
            ID = 6710;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6711 : NPC_6703
    {
        public NPC_6711(Main.GameClient _client)
            : base(_client)
        {
            ID = 6711;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6712 : NPC_6703
    {
        public NPC_6712(Main.GameClient _client)
            : base(_client)
        {
            ID = 6712;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6713 : NPC_6703
    {
        public NPC_6713(Main.GameClient _client)
            : base(_client)
        {
            ID = 6713;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6714 : NPC_6703
    {
        public NPC_6714(Main.GameClient _client)
            : base(_client)
        {
            ID = 6714;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6715 : NPC_6703
    {
        public NPC_6715(Main.GameClient _client)
            : base(_client)
        {
            ID = 6715;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_6716 : NPC_6703
    {
        public NPC_6716(Main.GameClient _client)
            : base(_client)
        {
            ID = 6716;
            Face = 30;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}