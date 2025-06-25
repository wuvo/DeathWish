using NewestCOServer.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.Features
{
    public class Dungeons
    {
        public static void Dungeon(Main.GameClient GC)
        {
            ushort mapid = (ushort)(Program.Rnd.Next(8008, 8999));
            DMaps.CreateDynamicMap(1767, mapid, true);

            #region SpawnMobs
            for (int x = 0; x < 10; x++)
            {
                Mob DI = new Mob();
                DI.Loc = new Location();
                var a = Program.Rnd.Next(0, 2);
                DI.Loc.X = (ushort)(53 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                DI.Loc.Y = (ushort)(57 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                DI.Loc.Map = mapid;

                DI.StartLoc.XFrom = (ushort)(53 - Program.Rnd.Next(10));
                DI.StartLoc.XTo = (ushort)(53 + Program.Rnd.Next(10));
                DI.StartLoc.YFrom = (ushort)(57 - Program.Rnd.Next(10));
                DI.StartLoc.Yto = (ushort)(57 + Program.Rnd.Next(10));
                DI.StartLoc.Map = mapid;
                DI.MobID = (int)Program.Rnd.Next(1000, 3000);
                DI.Name = "TestDungeon";
                DI.Type = MobBehaveour.HuntPlayers;
                DI.Mesh = 104;
                DI.Level = 20;
                DI.MaxHP = (ushort)(DI.Level * 10000);
                DI.Defense = (ushort)(DI.Level * 10);
                DI.MDef = (ushort)(DI.Level);
                DI.MAttack = (ushort)(DI.Level * 10);
                DI.MinAttack = (ushort)(DI.Level * 60);
                DI.MaxAttack = (ushort)(DI.Level * 80);
                DI.DmgReduceTimes = (byte)(DI.Level / 10);
                DI.Dodge = 1;
                DI.AtkType = AttackType.Magic;
                if (DI.AtkType == AttackType.Magic)
                {
                    DI.MagicSkill = 1002;
                    DI.MagicLvl = 3;
                    DI.Gives = true;
                    if (DI.Level < 30)
                        DI.AttackDist = 8;
                    else
                        DI.AttackDist = (byte)(DI.Level / 10);
                    DI.MinSilvers = DI.Level * 10;
                    DI.MaxSilvers = DI.Level * 100;
                }
                else
                {
                    DI.Gives = true;
                    if (DI.Level < 30)
                        DI.AttackDist = 2;
                    else
                        DI.AttackDist = (byte)(DI.Level / 10);
                    DI.MinSilvers = DI.Level * 10;
                    DI.MaxSilvers = DI.Level * 100;
                }

                DI.CurrentHP = DI.MaxHP;
                if (!World.H_Mobs.Contains(DI.Loc.Map))
                {
                    World.H_Mobs.Add(DI.Loc.Map, new Hashtable());
                    World.PlayersInMap.Add(DI.Loc.Map, new ThreadSafeDictionary<uint, Character>(400));
                }

                Hashtable MapMobs = (Hashtable)World.H_Mobs[DI.Loc.Map];
                DI.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                if (MapMobs != null)
                    while (MapMobs.Contains(DI.EntityID))
                        DI.EntityID = (uint)Program.Rnd.Next(400000, 500000);
                MapMobs.Add(DI.EntityID, DI);
                DI.Alive = true;
                DI.Respawn();
            }
            #endregion

            if (GC.MyChar.MyTeam.Members.Count > 0)
                if (GC.MyChar.MyTeam.Members.Count > 0)
                    foreach (Character C in GC.MyChar.MyTeam.Members)
                        if (C.EntityID != GC.MyChar.EntityID)
                            if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 14))
                                C.Teleport(mapid, 53, 57);

            GC.MyChar.Teleport(mapid, 53, 57);
        }
    }
}
