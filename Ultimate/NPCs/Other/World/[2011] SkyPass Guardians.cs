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
    public class NPC_2011 : NPCBase
    {
        public NPC_2011(Main.GameClient _client)
            : base(_client)
        {
            ID = 2011;
            Face = 30;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            int skynpc = ((int)(ID - 2011) + 1);
            if (ID == 2015)
                skynpc += 4;
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.InventoryContains(Convert.ToUInt32(721099 + skynpc), 1))
                        {
                            {
                                AddText("I can take you out of here if you give me your PassToken" + skynpc);
                                AddOption("Yay!", Convert.ToByte(skynpc));
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but it seems like you don't have a PassToken. Hunt one first and I'll help you getting out.");
                            AddOption("Damn", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721100));
                        GC.MyChar.Teleport(1040, 595, 383);
                        break;
                    }
                case 2:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721101));
                        GC.MyChar.Teleport(1040, 543, 330);
                        break;
                    }
                case 3:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721102));
                        GC.MyChar.Teleport(1040, 492, 280);
                        break;
                    }
                case 4:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721103));
                        GC.MyChar.Teleport(1040, 436, 224);
                        break;
                    }
                case 9:
                    {
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(721108));
                        GC.MyChar.Teleport(1040, 393, 181);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_2012 : NPC_2011
    {
        public NPC_2012(Main.GameClient _client)
            : base(_client)
        {
            ID = 2012;
            Face = 30;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2013 : NPC_2011
    {
        public NPC_2013(Main.GameClient _client)
            : base(_client)
        {
            ID = 2013;
            Face = 30;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2014 : NPC_2011
    {
        public NPC_2014(Main.GameClient _client)
            : base(_client)
        {
            ID = 2014;
            Face = 30;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2015 : NPC_2011
    {
        public NPC_2015(Main.GameClient _client)
            : base(_client)
        {
            ID = 2015;
            Face = 30;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}