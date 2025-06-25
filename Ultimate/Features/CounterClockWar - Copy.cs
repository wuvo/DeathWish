using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewestCOServer.Game;
using System.Threading;

namespace NewestCOServer.Features
{
    public class ConquerTheCastle
    {
        public static ArrayList Players = new ArrayList();
        public static bool signup = false;
        
        public struct Gates
        {
            public Location Loc;
            public uint MaxHP;
            public uint CurHP;
            public uint EntityID;
            public uint Mesh;

            public bool Opened
            {
                set
                {
                    if (EntityID >= 6703 && EntityID <= 6708)//Left Gate
                    {
                        if (value) Mesh = 250;
                        else Mesh = 240;
                    }
                    else if (EntityID >= 6709 && EntityID <= 6725)//Right Gate
                    {
                        if (value) Mesh = 280;
                        else Mesh = 270;
                    }
                }
                get
                {
                    if (EntityID >= 6703 && EntityID <= 6708)//Left Gate
                    {
                        if (Mesh == 250) return true;
                        else return false;
                    }
                    else if (EntityID >= 6709 && EntityID <= 6725)//Right Gate
                    {
                        if (Mesh == 280) return true;
                        else return false;
                    }
                    return false;
                }
            }
            public void Spawn(Character C, bool Check)
            {
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 28) || !Check))
                    C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void ReSpawn()
            {
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28))
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 26, Loc, true, "Gate", CurHP, MaxHP));
            }
            public void TakeAttack(Character C, uint Damage, byte AtkType)
            {
                if (AtkType != 21)
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
                if (Damage >= CurHP)
                {
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, (byte)AttackType.Kill).Get);
                    CurHP = 0;
                    if (!Opened)
                    {
                        Opened = true;
                        ReSpawn();
                    }
                }
                else
                    CurHP -= Damage;
            }
            public void TakeAttack(Companion C, uint Damage, byte AtkType)
            {
                if (AtkType != 21)
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, Damage, AtkType).Get);
                if (Damage >= CurHP)
                {
                    CurHP = 0;
                    C.Owner.AtkMem.Attacking = false;
                    C.Owner.AtkMem.Target = 0;
                    World.Action(C, Packets.AttackPacket(C.EntityID, EntityID, Loc.X, Loc.Y, 0, 14).Get);
                    if (!Opened)
                    {
                        Opened = true;
                        ReSpawn();
                    }
                }
                else
                    CurHP -= Damage;
            }
        }
        public static Gates RG2;
        public static Gates RG1;
        public static Gates LG3;
        public static Gates LG2;
        public static Gates LG1;
        

        public static bool War;

        public static void Tower(ushort X, ushort Y)
        {
            Mob DI = new Mob();
            DI.Loc = new Location();
            DI.StartLoc.XTo = DI.StartLoc.XFrom = DI.Loc.X = X;
            DI.StartLoc.Yto = DI.StartLoc.YFrom = DI.Loc.Y = Y;
            DI.StartLoc.Map = DI.Loc.Map = 3868;
            DI.MobID = (int)Program.Rnd.Next(1000, 3000);
            DI.Name = "";
            DI.Type = MobBehaveour.HuntPlayers;
            DI.Mesh = 922;
            DI.Level = 138;
            DI.MaxHP = 1000000;
            DI.Defense = 3215;
            DI.MDef = 80;
            DI.MaxAttack = DI.MinAttack = DI.MAttack = 1215;
            DI.DmgReduceTimes = 2;
            DI.Dodge = 70;
            DI.AtkType = AttackType.Magic;
            DI.MagicSkill = 1180;
            DI.MagicLvl = 7;
            DI.Gives = false;
            DI.AttackDist = 8;
            DI.MaxSilvers = DI.MinSilvers = 0;
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

        public static void Init()
        {
            War = false;
            
            #region RGs
            
            RG2 = new Gates();
            RG2.EntityID = 6710;
            RG2.Opened = false;
            RG2.MaxHP = 10000000;
            RG2.CurHP = 10000000;
            RG2.Loc = new Location();
            RG2.Loc.Map = 3868;
            RG2.Loc.X = 175;
            RG2.Loc.Y = 138;
            RG2.ReSpawn();

            RG1 = new Gates(); //left side right gate
            RG1.EntityID = 6709;
            RG1.Opened = false;
            RG1.MaxHP = 5000000;
            RG1.CurHP = 5000000;
            RG1.Loc = new Location();
            RG1.Loc.Map = 1844;
            RG1.Loc.X = 280;
            RG1.Loc.Y = 143;
            RG1.ReSpawn();
            #endregion
            #region LGs

            LG3 = new Gates();
            LG3.EntityID = 6705;
            LG3.Opened = false;
            LG3.MaxHP = 5000000;
            LG3.CurHP = 5000000;
            LG3.Loc = new Location();
            LG3.Loc.Map = 1844;
            LG3.Loc.X = 289;
            LG3.Loc.Y = 241;
            LG3.ReSpawn();

            LG2 = new Gates();
            LG2.EntityID = 6704;
            LG2.Opened = false;
            LG2.MaxHP = 5000000;
            LG2.CurHP = 5000000;
            LG2.Loc = new Location();
            LG2.Loc.Map = 3868;
            LG2.Loc.X = 222;
            LG2.Loc.Y = 220;
            LG2.ReSpawn();

            LG1 = new Gates();
            LG1.EntityID = 8000;
            LG1.Opened = false;
            LG1.MaxHP = 5000000;
            LG1.CurHP = 5000000;
            LG1.Loc = new Location();
            LG1.Loc.Map = 3868;
            LG1.Loc.X = 168;
            LG1.Loc.Y = 242;
            LG1.ReSpawn();
            #endregion
            #region Towers
            Tower(164, 247);
            Tower(176, 247);
            Tower(234,229);
            Tower(219,229);
            Tower(286,246);
            Tower(300,246);
            Tower(286,138);
            Tower(286,152);
            Tower(162,142);
            Tower(166,131);
            #endregion
        }
        public static void StartWar()
        {
            signup = false;
            Init();
            LG1.ReSpawn();
            LG2.ReSpawn();
            LG3.ReSpawn();
            RG1.ReSpawn();
            RG2.ReSpawn();
            World.SendMsgToAll("SYSTEM", "Castle Invasion Event has begun!", 2011, 0);
            War = true;
            Teleport();
        }
        public static void EndWarForGood()
        {
            War = false;
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Loc.Map == 3686 || C.Loc.Map == 1090)
                {
                    C.Teleport(1002, 430, 378);
                }
            }
        }

        public static void Teleport()
        {
            try
            {
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == 1090)
                        World.Action(C, (Packets.String(C.EntityID, 10, "downnumber5")).Get);
                Thread.Sleep(1000);
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == 1090)
                        World.Action(C, (Packets.String(C.EntityID, 10, "downnumber4")).Get);
                Thread.Sleep(1000);
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == 1090)
                        World.Action(C, (Packets.String(C.EntityID, 10, "downnumber3")).Get);
                Thread.Sleep(1000);
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == 1090)
                        World.Action(C, (Packets.String(C.EntityID, 10, "downnumber2")).Get);
                Thread.Sleep(1000);
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == 1090)
                        World.Action(C, (Packets.String(C.EntityID, 10, "downnumber1")).Get);
                Thread.Sleep(1000);

                foreach (Character C in World.H_Chars.Values)
                {
                    if (C.Loc.Map == 1090)
                    {
                        C.Teleport(3868, (ushort)(233 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20)), (ushort)(390 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20)));
                        C.Protection = false;
                    }
                }
            }
            catch { }
        }

        public static void AddPlayer(Character C)
        {
            if (signup == true)
            {
                C.Teleport(1090, (ushort)(90 + Program.Rnd.Next(1, 16) - Program.Rnd.Next(1, 16)), (ushort)(65 + Program.Rnd.Next(1, 16) - Program.Rnd.Next(1, 16)));
                C.MyClient.LocalMessage(2000, "You'll be sent to the Counter Clock Guild War map in 3 minutes!");
            }
        }
    }
}
