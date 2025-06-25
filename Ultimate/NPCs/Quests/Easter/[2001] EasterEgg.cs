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
    public class NPC_2001 : NPCBase
    {
        public NPC_2001(Main.GameClient _client)
            : base(_client)
        {
            ID = 2001;
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
                        var _increment = ID - 2001;
                        if (GC.MyChar.InventoryContains(729921, 1))//EggBag
                        {
                            if (GC.MyChar.Inventory.Count > 38)
                            {
                                AddText("You need at least 2 free spaces in your inventory!");
                                AddOption("I see", 255);
                            }
                            else if (GC.MyChar.InventoryContains((729922 + _increment), 1))
                            {
                                AddText("It seems like you already have an Egg Fragment from this city! Go find the other ones!");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                if (MyMath.ChanceSuccess(40))//EggFragment
                                {
                                    GC.MyChar.AddItem(729922 + _increment);
                                    string _city = "Twin  City";
                                    if (ID == 2002)
                                        _city = "Phoenix Castle";
                                    else if (ID == 2003)
                                        _city = "Ape City";
                                    else if (ID == 2004)
                                        _city = "Desert City";
                                    else if (ID == 2005)
                                        _city = "Bird Island";
                                    AddText("Congratulations you received the " + _city +" Egg Fragment!");
                                    AddOption("Thanks.", 255);
                                }
                                else
                                {
                                    AddText("Ouch! The egg fragment was broken! Find me again for a new one!");
                                    AddOption("Damn", 255);
                                }
                            }
                        }
                        else
                        {
                            AddText("Get a Feed from Commander in Twin City (431, 435) before talking to me!");
                            AddOption("I see", 255);
                        }
                        RemoveEgg(GC);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
        public void RemoveEgg(Main.GameClient GC)
        {
            NPC N = null;
            Dictionary<uint, NPC> MapNPC = World.H_NPCs[GC.MyChar.Loc.Map];
            if (MapNPC != null && MapNPC.ContainsKey(ID))
                N = (NPC)MapNPC[ID];
            #region RemoveEgg
            MapNPC.Remove(ID);
            Game.World.Action(N, Packets.GeneralData(ID, 0, 0, 0, 135).Get);
            #endregion
            #region SpawnEgg
            int Location;
            int Dir;
            Random Rndom = new Random();
            Dir = Program.Rnd.Next(0, 8);
            Location = Program.Rnd.Next(1, 9);
            NPC NPCInfo = new Game.NPC();
            NPCInfo.EntityID = ID;
            NPCInfo.Type = 4370;
            NPCInfo.Flags = 2;
            NPCInfo.Loc = new Game.Location();
            NPCInfo.Loc.Map = N.Loc.Map;
            if (ID == 2001)
                #region Locations
                switch (Location)
                {
                    case 1:
                        {
                            NPCInfo.Loc.X = 377;
                            NPCInfo.Loc.Y = 393;
                            break;
                        }
                    case 2:
                        {
                            NPCInfo.Loc.X = 369;
                            NPCInfo.Loc.Y = 312;
                            break;
                        }
                    case 3:
                        {
                            NPCInfo.Loc.X = 399;
                            NPCInfo.Loc.Y = 314;
                            break;
                        }
                    case 4:
                        {
                            NPCInfo.Loc.X = 395;
                            NPCInfo.Loc.Y = 294;
                            break;
                        }
                    case 5:
                        {
                            NPCInfo.Loc.X = 465;
                            NPCInfo.Loc.Y = 240;
                            break;
                        }
                    case 6:
                        {
                            NPCInfo.Loc.X = 413;
                            NPCInfo.Loc.Y = 219;
                            break;
                        }
                    case 7:
                        {
                            NPCInfo.Loc.X = 385;
                            NPCInfo.Loc.Y = 240;
                            break;
                        }
                    case 8:
                        {
                            NPCInfo.Loc.X = 428;
                            NPCInfo.Loc.Y = 389;
                            break;
                        }
                }
            #endregion
            else if (ID == 2002)
                #region Locations
                switch (Location)
                {
                    case 1:
                        {
                            NPCInfo.Loc.X = 210;
                            NPCInfo.Loc.Y = 260;
                            break;
                        }
                    case 2:
                        {
                            NPCInfo.Loc.X = 180;
                            NPCInfo.Loc.Y = 225;
                            break;
                        }
                    case 3:
                        {
                            NPCInfo.Loc.X = 220;
                            NPCInfo.Loc.Y = 230;
                            break;
                        }
                    case 4:
                        {
                            NPCInfo.Loc.X = 212;
                            NPCInfo.Loc.Y = 197;
                            break;
                        }
                    case 5:
                        {
                            NPCInfo.Loc.X = 247;
                            NPCInfo.Loc.Y = 230;
                            break;
                        }
                    case 6:
                        {
                            NPCInfo.Loc.X = 190;
                            NPCInfo.Loc.Y = 271;
                            break;
                        }
                    case 7:
                        {
                            NPCInfo.Loc.X = 236;
                            NPCInfo.Loc.Y = 283;
                            break;
                        }
                    case 8:
                        {
                            NPCInfo.Loc.X = 246;
                            NPCInfo.Loc.Y = 296;
                            break;
                        }
                }
            #endregion
            else if (ID == 2003)
                #region Locations
                switch (Location)
                {
                    case 1:
                        {
                            NPCInfo.Loc.X = 542;
                            NPCInfo.Loc.Y = 545;
                            break;
                        }
                    case 2:
                        {
                            NPCInfo.Loc.X = 525;
                            NPCInfo.Loc.Y = 501;
                            break;
                        }
                    case 3:
                        {
                            NPCInfo.Loc.X = 559;
                            NPCInfo.Loc.Y = 495;
                            break;
                        }
                    case 4:
                        {
                            NPCInfo.Loc.X = 570;
                            NPCInfo.Loc.Y = 531;
                            break;
                        }
                    case 5:
                        {
                            NPCInfo.Loc.X = 583;
                            NPCInfo.Loc.Y = 576;
                            break;
                        }
                    case 6:
                        {
                            NPCInfo.Loc.X = 578;
                            NPCInfo.Loc.Y = 601;
                            break;
                        }
                    case 7:
                        {
                            NPCInfo.Loc.X = 541;
                            NPCInfo.Loc.Y = 607;
                            break;
                        }
                    case 8:
                        {
                            NPCInfo.Loc.X = 577;
                            NPCInfo.Loc.Y = 558;
                            break;
                        }
                }
            #endregion
            else if (ID == 2004)
                #region Locations
                switch (Location)
                {
                    case 1:
                        {
                            NPCInfo.Loc.X = 462;
                            NPCInfo.Loc.Y = 668;
                            break;
                        }
                    case 2:
                        {
                            NPCInfo.Loc.X = 533;
                            NPCInfo.Loc.Y = 684;
                            break;
                        }
                    case 3:
                        {
                            NPCInfo.Loc.X = 507;
                            NPCInfo.Loc.Y = 604;
                            break;
                        }
                    case 4:
                        {
                            NPCInfo.Loc.X = 523;
                            NPCInfo.Loc.Y = 593;
                            break;
                        }
                    case 5:
                        {
                            NPCInfo.Loc.X = 496;
                            NPCInfo.Loc.Y = 579;
                            break;
                        }
                    case 6:
                        {
                            NPCInfo.Loc.X = 463;
                            NPCInfo.Loc.Y = 531;
                            break;
                        }
                    case 7:
                        {
                            NPCInfo.Loc.X = 463;
                            NPCInfo.Loc.Y = 618;
                            break;
                        }
                    case 8:
                        {
                            NPCInfo.Loc.X = 492;
                            NPCInfo.Loc.Y = 649;
                            break;
                        }
                }
            #endregion
            else if (ID == 2005)
                #region Locations
                switch (Location)
                {
                    case 1:
                        {
                            NPCInfo.Loc.X = 687;
                            NPCInfo.Loc.Y = 548;
                            break;
                        }
                    case 2:
                        {
                            NPCInfo.Loc.X = 706;
                            NPCInfo.Loc.Y = 594;
                            break;
                        }
                    case 3:
                        {
                            NPCInfo.Loc.X = 766;
                            NPCInfo.Loc.Y = 601;
                            break;
                        }
                    case 4:
                        {
                            NPCInfo.Loc.X = 762;
                            NPCInfo.Loc.Y = 588;
                            break;
                        }
                    case 5:
                        {
                            NPCInfo.Loc.X = 729;
                            NPCInfo.Loc.Y = 499;
                            break;
                        }
                    case 6:
                        {
                            NPCInfo.Loc.X = 694;
                            NPCInfo.Loc.Y = 520;
                            break;
                        }
                    case 7:
                        {
                            NPCInfo.Loc.X = 731;
                            NPCInfo.Loc.Y = 533;
                            break;
                        }
                    case 8:
                        {
                            NPCInfo.Loc.X = 709;
                            NPCInfo.Loc.Y = 572;
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
        }
    }
    public class NPC_2002 : NPC_2001
    {
        public NPC_2002(Main.GameClient _client)
            : base(_client)
        {
            ID = 2002;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2003 : NPC_2001
    {
        public NPC_2003(Main.GameClient _client)
            : base(_client)
        {
            ID = 2003;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2004 : NPC_2001
    {
        public NPC_2004(Main.GameClient _client)
            : base(_client)
        {
            ID = 2004;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_2005 : NPC_2001
    {
        public NPC_2005(Main.GameClient _client)
            : base(_client)
        {
            ID = 2005;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}