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
    public class NPC_1613 : NPCBase
    {
        public NPC_1613(Main.GameClient _client)
            : base(_client)
        {
            ID = 1613;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            #region RemoveMouse
            NPC N = null;
            Dictionary<uint, NPC> MapNPC = World.H_NPCs[GC.MyChar.Loc.Map];
            if (MapNPC != null && MapNPC.ContainsKey(ID))
                N = (NPC)MapNPC[ID];
            MapNPC.Remove(ID);
            Game.World.Action(N, Packets.GeneralData(ID, 0, 0, 0, 135).Get);
            #endregion

            #region SpawnMouses
            ushort[] Maps = new ushort[2] { 1502, 1503 };// Right and Left
            int i;
            int Location;
            int Dir;
            Random Rndom = new Random();
            i = Rndom.Next(0, 2);
            Dir = Program.Rnd.Next(0, 8);
            Location = Program.Rnd.Next(1, 9);
            DMap D = (DMap)DMaps.H_DMaps[Maps[i]];
            NPC NPCInfo = new Game.NPC();
            NPCInfo.EntityID = ID;
            NPCInfo.Type = 6890;
            NPCInfo.Flags = 2;
            NPCInfo.Loc = new Location();
            NPCInfo.Loc.Map = Maps[i];
            #region Locations
            switch (Location)
            {
                case 1:
                    {
                        NPCInfo.Loc.X = 157;
                        NPCInfo.Loc.Y = 109;
                        break;
                    }
                case 2:
                    {
                        NPCInfo.Loc.X = 130;
                        NPCInfo.Loc.Y = 119;
                        break;
                    }
                case 3:
                    {
                        NPCInfo.Loc.X = 114;
                        NPCInfo.Loc.Y = 152;
                        break;
                    }
                case 4:
                    {
                        NPCInfo.Loc.X = 79;
                        NPCInfo.Loc.Y = 132;
                        break;
                    }
                case 5:
                    {
                        NPCInfo.Loc.X = 87;
                        NPCInfo.Loc.Y = 111;
                        break;
                    }
                case 6:
                    {
                        NPCInfo.Loc.X = 40;
                        NPCInfo.Loc.Y = 60;
                        break;
                    }
                case 7:
                    {
                        NPCInfo.Loc.X = 100;
                        NPCInfo.Loc.Y = 100;
                        break;
                    }
                case 8:
                    {
                        NPCInfo.Loc.X = 105;
                        NPCInfo.Loc.Y = 50;
                        break;
                    }
            }
            #endregion
            NPCInfo.Direction = (byte)(Dir);
            NPCInfo.Avatar = N.Avatar;
            if (!Game.World.H_NPCs.ContainsKey(NPCInfo.Loc.Map))
            {
                Game.World.H_NPCs.Add(NPCInfo.Loc.Map, new Dictionary<uint, NPC>());
            }
            Dictionary<uint, NPC> NPCMap = Game.World.H_NPCs[NPCInfo.Loc.Map];
            if (!NPCMap.ContainsKey(NPCInfo.EntityID))
            {
                NPCMap.Add(NPCInfo.EntityID, NPCInfo);
                Game.World.Spawn(NPCInfo);
            }
            #endregion
            switch (_linkback)
            {
                #region Dialog
                case 0:
                    {
                        if (GC.MyChar.InventoryContains(722510, 1))//silver neddle
                        {
                            if (GC.MyChar.Inventory.Count > 38)
                            {
                                AddText("You need at least 2 free spaces in your inventory before trying to catch me!");
                                AddOption("Damn.", 255);
                            }
                            else if (MyMath.ChanceSuccess(70))
                            {
                                AddText("Oh, my god! Narrowly escaped! Bye, bye!");
                                AddOption("Damn.", 255);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722510));
                                if (MyMath.ChanceSuccess(0.9))//secret command
                                {
                                    GC.MyChar.AddItem(722515);
                                    AddText("Congratulations you received the Secret Command!");
                                    AddOption("Thanks.", 255);
                                }
                                else
                                {
                                    if (MyMath.ChanceSuccess(17))
                                    {
                                        GC.MyChar.AddItem(722512);
                                        GC.MyChar.AddItem(722513);
                                        AddText("Congratulations you received the Aster Necklace, Pinetum Picture!");
                                        AddOption("Thanks.", 255);
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(50))
                                        {
                                            GC.MyChar.AddItem(722512);
                                            AddText("Congratulations you received the Aster Necklace!");
                                            AddOption("Thanks.", 255);
                                        }
                                        else
                                        {
                                            GC.MyChar.AddItem(722513);
                                            AddText("Congratulations you received the Pinetum Picture!");
                                            AddOption("Thanks.", 255);
                                        }
                                    }
                                }
                            }
                        }
                        else if (GC.MyChar.InventoryContains(722511, 1))//Gold Neddle
                        {
                            if (GC.MyChar.Inventory.Count > 37)
                            {
                                AddText("You need at least 3 free spaces in your inventory before trying to catch me!");
                                AddOption("Damn.", 255);
                            }
                            else if (MyMath.ChanceSuccess(60))
                            {
                                AddText("Oh, my god! Narrowly escaped! Bye, bye!");
                                AddOption("Damn.", 255);
                            }
                            else
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(722511));
                                if (MyMath.ChanceSuccess(1.4))//secret command
                                {
                                    GC.MyChar.AddItem(722515);
                                    AddText("Congratulations you received the Secret Command!");
                                    AddOption("Thanks.", 255);
                                }
                                else
                                {
                                    GC.MyChar.AddItem(722514);
                                    if (MyMath.ChanceSuccess(15))
                                    {
                                        GC.MyChar.AddItem(722512);
                                        GC.MyChar.AddItem(722513);
                                        AddText("Congratulations you received the Royal Sword, Aster Necklace, Pinetum Picture!");
                                        AddOption("Thanks.", 255);
                                    }
                                    else if (MyMath.ChanceSuccess(30))
                                    {
                                        GC.MyChar.AddItem(722512);
                                        AddText("Congratulations you received the Royal Sword and Aster Necklace!");
                                        AddOption("Thanks.", 255);
                                    }
                                    else if (MyMath.ChanceSuccess(30))
                                    {
                                        GC.MyChar.AddItem(722513);
                                        AddText("Congratulations you received the Royal Sword and Pinetum Picture!");
                                        AddOption("Thanks.", 255);
                                    }
                                    else
                                    {
                                        AddText("Congratulations you received the Royal Sword!");
                                        AddOption("Thanks.", 255);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddText("You don't have any needle! Good luck catching me without it ahah!");
                            AddOption("Crap!", 255);
                        }
                        break;
                    }
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_1614 : NPC_1613
    {
        public NPC_1614(Main.GameClient _client)
            : base(_client)
        {
            ID = 1614;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1615 : NPC_1613
    {
        public NPC_1615(Main.GameClient _client)
            : base(_client)
        {
            ID = 1615;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1616 : NPC_1613
    {
        public NPC_1616(Main.GameClient _client)
            : base(_client)
        {
            ID = 1616;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1617 : NPC_1613
    {
        public NPC_1617(Main.GameClient _client)
            : base(_client)
        {
            ID = 1617;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1618 : NPC_1613
    {
        public NPC_1618(Main.GameClient _client)
            : base(_client)
        {
            ID = 1618;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1619 : NPC_1613
    {
        public NPC_1619(Main.GameClient _client)
            : base(_client)
        {
            ID = 1619;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_1620 : NPC_1613
    {
        public NPC_1620(Main.GameClient _client)
            : base(_client)
        {
            ID = 1620;
            Face = 7;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}