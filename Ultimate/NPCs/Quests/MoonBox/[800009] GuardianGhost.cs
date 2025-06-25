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
    public class NPC_800009 : NPCBase
    {
        public NPC_800009(Main.GameClient _client)
            : base(_client)
        {
            ID = 800009;
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
                        AddText("I am here for several years. Sometimes I see people who can get out of this tactic, however only a few managed to do it.");
                        AddOption("Take me out", 1);
                        AddOption("I see", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Loc.Map == 1043)
                        {
                            if (GC.MyChar.InventoryContains(721010, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1044)
                        {
                            if (GC.MyChar.InventoryContains(721011, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1045)
                        {
                            if (GC.MyChar.InventoryContains(721012, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1046)
                        {
                            if (GC.MyChar.InventoryContains(721013, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1047)
                        {
                            if (GC.MyChar.InventoryContains(721014, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        else if (GC.MyChar.Loc.Map == 1048)
                        {
                            if (GC.MyChar.InventoryContains(721015, 1))
                            {
                                GC.MyChar.Teleport(1042, 26, 36);
                            }
                            else
                            {
                                AddText("You didn't find the token of this tactic I can't let you out!");
                                AddOption("Damn!", 255);
                            }
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_800010 : NPC_800009
    {
        public NPC_800010(Main.GameClient _client)
            : base(_client)
        {
            ID = 800010;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800011 : NPC_800009
    {
        public NPC_800011(Main.GameClient _client)
            : base(_client)
        {
            ID = 800011;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800012 : NPC_800009
    {
        public NPC_800012(Main.GameClient _client)
            : base(_client)
        {
            ID = 800012;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800013 : NPC_800009
    {
        public NPC_800013(Main.GameClient _client)
            : base(_client)
        {
            ID = 800013;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_800014 : NPC_800009
    {
        public NPC_800014(Main.GameClient _client)
            : base(_client)
        {
            ID = 800014;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}