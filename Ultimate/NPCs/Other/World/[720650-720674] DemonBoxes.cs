using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using System.Collections.Concurrent;

namespace Ultimate.NPCs
{
    public class NPC_720650 : NPCBase
    {
        public NPC_720650(Main.GameClient _client)
            : base(_client)
        {
            ID = 720650;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You should open your Demon Box in a secret place far away from the guards and other players! ");
                    AddText("Do not open it at the border of the map! By killing the Demon, you will have a chance to get a pack that");
                    AddText(" can give up to 54x what you invested in the box. Are you sure you want to release it now?");
                    AddOption("Yes!", 1);
                    AddOption("Wait...", 255);
                    break;
                case 1:
                    {
                        if (!World.NoPKMaps.Contains(GC.MyChar.Loc.Map))
                        {
                            if (GC.MyChar.InventoryContains(ID, 1))
                            {
                                Mob M = new Mob()
                                {
                                    LastMove = DateTime.Now,
                                    Level = 1,
                                    MaxHP = 33,
                                    CurrentHP = 33,
                                    Defense = 0,
                                    MDef = 0,
                                    MAttack = 0,
                                    MinAttack = 5,
                                    MaxAttack = 6,
                                    Type = MobBehaveour.HuntPlayers,
                                    DmgReduceTimes = 1,
                                    Dodge = 36,
                                    AtkType = AttackType.Melee,
                                    Gives = true,
                                    AttackDist = 2,
                                    MinSilvers = 1,
                                    MaxSilvers = 50,
                                    MoveSpeed = 1000,
                                    SpawnSpeed = 0,
                                };
                                Character C = new Character()
                                {
                                    LastMove = DateTime.Now,
                                    Level = 130,
                                    CurHP = 65000,
                                    EntityID = 1050000,
                                    
                                };
                                if (ID == 720650)
                                {
                                    M.Name = "Demon";
                                    M.Mesh = 369;
                                    M.MobID = 800;
                                    Game.World.demonBoxesCur += 50000;

                                }
                                else if (ID == 720651)
                                {
                                    M.Name = "AncientDemon";
                                    M.Mesh = 756;
                                    M.MobID = 801;
                                    Game.World.demonBoxesCur += 100000;
                                }
                                else if (ID == 720652)
                                {
                                    M.Name = "FloodDemon";
                                    M.Mesh = 377;
                                    M.MobID = 802;
                                    Game.World.demonBoxesCur += 500000;
                                }
                                else if (ID == 720671)
                                {
                                    M.Name = "HeavenDemon";
                                    M.Mesh = 266;
                                    M.MobID = 803;
                                    Game.World.demonBoxesCur += 1000000;
                                }
                                else if (ID == 720672)
                                {
                                    M.Name = "ChaosDemon";
                                    M.Mesh = 327;
                                    M.MobID = 804;
                                    Game.World.demonBoxesCur += 5000000;
                                }
                                else if (ID == 720673)
                                {
                                    M.Name = "SacredDemon";
                                    M.Mesh = 168;
                                    M.MobID = 805;

                                    Game.World.demonBoxesCur += 10000000;
                                }
                                else if (ID == 720674)
                                {
                                    M.Name = "AuroraDemon";
                                    M.Mesh = 223;
                                    M.MobID = 806;
                                    Game.World.demonBoxesCur += 20000000;
                                }
                                C.Name = "Demon";
                                C.Loc.Map = GC.MyChar.Loc.Map;
                                C.Loc.X = (ushort)(GC.MyChar.Loc.X - 5);
                                C.Loc.Y = (ushort)(GC.MyChar.Loc.Y - 5);

                                M.Loc.Map = GC.MyChar.Loc.Map;
                                M.StartLoc.XFrom = (ushort)(GC.MyChar.Loc.X - 5);
                                M.StartLoc.YFrom = (ushort)(GC.MyChar.Loc.Y - 5);
                                M.StartLoc.XTo = (ushort)(GC.MyChar.Loc.X + 5);
                                M.StartLoc.Yto = (ushort)(GC.MyChar.Loc.Y + 5);
                                if (M.AddMob())
                                {
                                    RemoveItem(ID);
                                    GC.MyChar.TotalDemonBoxes++;
                                }
                                else if (World.H_Mobs.ContainsKey(M.EntityID))
                                    World.H_Mobs[M.Loc.Map].Remove(M.EntityID);
                            }
                            else
                            {
                                AddText("I'm sorry but it seems like you don't have a DemonBox in your inventory!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("I'm sorry but you can't open DemonBoxes in this map!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }
            AddFinish();
            Send();
        }
    }
    public class NPC_720651 : NPC_720650
    {
        public NPC_720651(Main.GameClient _client)
            : base(_client)
        {
            ID = 720651;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_720652 : NPC_720650
    {
        public NPC_720652(Main.GameClient _client)
            : base(_client)
        {
            ID = 720652;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_720671 : NPC_720650
    {
        public NPC_720671(Main.GameClient _client)
            : base(_client)
        {
            ID = 720671;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_720672 : NPC_720650
    {
        public NPC_720672(Main.GameClient _client)
            : base(_client)
        {
            ID = 720672;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_720673 : NPC_720650
    {
        public NPC_720673(Main.GameClient _client)
            : base(_client)
        {
            ID = 720673;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
    public class NPC_720674 : NPC_720650
    {
        public NPC_720674(Main.GameClient _client)
            : base(_client)
        {
            ID = 720674;
            Face = 5;
            IsGlobal = true;
        }
        public override void Run(GameClient GC, byte[] _data, ushort _linkback)
        {
            base.Run(GC, _data, _linkback);
        }
    }
}
