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
    public class NPC_800002 : NPCBase
    {
        public NPC_800002(Main.GameClient _client)
            : base(_client)
        {
            ID = 800002;
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
                        if (GC.MyChar.Loc.Map == 1043)
                        {
                            if (GC.MyChar.InventoryContains(721010, 1))
                            {
                                AddText("This is Peace tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1044)
                        {
                            if (GC.MyChar.InventoryContains(721011, 1))
                            {
                                AddText("This is Chaos tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1045)
                        {
                            if (GC.MyChar.InventoryContains(721012, 1))
                            {
                                AddText("This is Deserted tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1046)
                        {
                            if (GC.MyChar.InventoryContains(721013, 1))
                            {
                                AddText("This is Prosperous tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1047)
                        {
                            if (GC.MyChar.InventoryContains(721014, 1))
                            {
                                AddText("This is Disturbed tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1048)
                        {
                            if (GC.MyChar.InventoryContains(721015, 1))
                            {
                                AddText("This is Calmed tactic congratulations!");
                                AddOption("OK. Thanks!", 255);
                            }
                            else
                            {
                                AddText("I can't tell you the name of the tactic untill you get the token.");
                                AddOption("OK. Thanks!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1049)
                        {
                            AddText("This is Death tactic! If you don't have all the tokens you'll have to die.");
                            AddOption("Oh. I see...", 255);
                        }
                        break;
                    }
            }
            AddFinish();
            Send();
        }

            
    }
    public class NPC_800003 : NPC_800002
    {
        public NPC_800003(Main.GameClient _client)
            : base(_client)
        {
            ID = 800003;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800004 : NPC_800002
    {
        public NPC_800004(Main.GameClient _client)
            : base(_client)
        {
            ID = 800004;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800005 : NPC_800002
    {
        public NPC_800005(Main.GameClient _client)
            : base(_client)
        {
            ID = 800005;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800006 : NPC_800002
    {
        public NPC_800006(Main.GameClient _client)
            : base(_client)
        {
            ID = 800006;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800007 : NPC_800002
    {
        public NPC_800007(Main.GameClient _client)
            : base(_client)
        {
            ID = 800007;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800008 : NPC_800002
    {
        public NPC_800008(Main.GameClient _client)
            : base(_client)
        {
            ID = 800008;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}