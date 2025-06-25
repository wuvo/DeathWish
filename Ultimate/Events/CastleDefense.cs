using NewestCOServer.Game;
using NewestCOServer.Main;
using NewestCOServer.PacketHandling;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace NewestCOServer.Features
{
    public static class CastleDefense
    {
        public enum CastleDefenseStage
        {
            None,
            Inviting,
            Countdown,
            Fighting,
            Over
        }
        public struct Poles
        {
            public Location Loc;
            public uint MaxHP;
            public uint CurHP;
            public uint Mesh;
            public uint EntityID;

            public void Spawn(Character C, bool Check)
            {
                if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28) && (!MyMath.InBox(C.Loc.PreviousX, C.Loc.PreviousY, Loc.X, Loc.Y, 28) || !Check))
                {
                    C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Shannara Conquer", CurHP, MaxHP));
                }
            }
            public void ReSpawn()
            {
                foreach (Character C in World.H_Chars.Values)
                    if (C.Loc.Map == Loc.Map && MyMath.InBox(C.Loc.X, C.Loc.Y, Loc.X, Loc.Y, 28))
                        C.MyClient.AddSend(Packets.SpawnNPCWithHP(EntityID, (ushort)Mesh, 10, Loc, true, "Shannara Conquer", CurHP, MaxHP));
            }
            public void TakeAttack(Mob M, uint Wave)
            {
                if (War)
                {
                    M.NPCTarget = null;
                    M.Alive = false;
                    uint Benefit = M.CurrentHP;
                    M.CurrentHP = 0;
                    M.PoisonedInfo = null;
                    M.Died = DateTime.UtcNow;
                    World.Action(M, Packets.Status(M.EntityID, Status.Effect, 2080).Get);

                    if (Wave >= CurHP)
                        War = false;
                    else
                    {
                        uint CurHP2 = CurHP;
                        if (CurHP > Wave)
                            CurHP -= Wave;
                        else CurHP = 0;
                        if (CurHP > 15000000)
                        {
                            World.ExcAdd += "Pole HP: " + CurHP + "\r\n";
                            Console.WriteLine("GW PROBLEM! Pole HP: " + CurHP);
                            if (CurHP2 < 15000000)
                                CurHP = CurHP2;
                            else CurHP = 7500000;
                        }
                    }
                }
            }
        }
        public static Poles ThePole;
        public static byte Wave = 0;
        public static bool War = false;
        public static CastleDefenseStage Stage = CastleDefenseStage.None;
        public static bool CTB = false;
        public static bool SignUp = false;
        public static ushort Map;
        public static ushort X;
        public static ushort Y;
        public static Dictionary<uint, GameClient> CastleDefenseHash;
        public static int CountDown;
        private static Thread PkThread;
        public static DateTime LastScores;
        public static DateTime End;

        public static void SendScores()
        {
            foreach (GameClient GC in CastleDefense.CastleDefenseHash.Values)
            {
                if (GC.MyChar.Loc.Map == 3974)
                {
                    GC.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", "Castle Defense Board", 0x83c, 0));
                    GC.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", "Wave: " + Wave, 0x83d, 0));
                    LastScores = DateTime.UtcNow;
                }
            }
        }

        public static void Broadcast(string msg, BroadCastLoc loc)
        {
            if (loc == BroadCastLoc.World)
            {
                foreach (Character character in World.H_Chars.Values)
                    character.MyClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
            else
            {
                if (loc != BroadCastLoc.Map)
                    return;
                foreach (GameClient gameClient in CastleDefense.CastleDefenseHash.Values)
                    gameClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
        }

        public static void AddPlayer(GameClient GC)
        {
            CastleDefense.CastleDefenseHash.Add(GC.MyChar.EntityID, GC);
            GC.MyChar.Teleport((uint)CastleDefense.Map, CastleDefense.X, CastleDefense.Y);
        }

        public static void StartTournament()
        {
            CastleDefense.CastleDefenseHash = new Dictionary<uint, GameClient>();
            CastleDefense.CountDown = 20;
            CastleDefense.Stage = CastleDefenseStage.Inviting;
            CastleDefense.Map = (ushort)1616;
            CastleDefense.X = (ushort)54;
            CastleDefense.Y = (ushort)64;
            CastleDefense.SignUp = true;
            CastleDefense.PkThread = new Thread((ThreadStart)(() =>
            {
                CastleDefense.BeginTournament();
                CastleDefense.WaitForWinner();
                CastleDefense.EndCTB();
            }));
            CastleDefense.PkThread.IsBackground = true;
            CastleDefense.PkThread.Start();
        }

        public static void SpawnWave()
        {
            #region SpawnMobs
            for (int x = 0; x < 10; x++)
            {
                Mob DI = new Mob();
                DI.Loc = new Location();
                var a = Program.Rnd.Next(0, 2);
                DI.Loc.X = 103;
                DI.Loc.Y = 233;
                DI.Loc.Map = 3976;

                DI.StartLoc.XFrom = 103;
                DI.StartLoc.XTo = 103;
                DI.StartLoc.YFrom = 233;
                DI.StartLoc.Yto = 233;
                DI.StartLoc.Map = 3976;
                DI.MobID = (int)Program.Rnd.Next(1000, 3000);
                DI.Name = "Wave" + Wave;
                DI.Type = MobBehaveour.HuntPlayers;
                DI.Mesh = 104;
                DI.Level = Wave;
                DI.MaxHP = (ushort)(DI.Level * 10000);
                DI.Defense = 1;
                DI.MDef = 1;
                DI.MAttack = 1;
                DI.MinAttack = 1;
                DI.MaxAttack = 1;
                DI.DmgReduceTimes = 1;
                DI.Dodge = 1;
                DI.AtkType = AttackType.Melee;
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
                    DI.AttackDist = 1;
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
        }

        public static void BeginTournament()
        {
            foreach (Character character in World.H_Chars.Values)
            {
                character.MyClient.DialogNPC = 2058;
                NPCDialog.Handle(character.MyClient, null, 2058, 0);
            }
            CastleDefense.Stage = CastleDefenseStage.Inviting;
            while (CastleDefense.CountDown > 0)
            {
                if (CastleDefense.CountDown == 120)
                    CastleDefense.Broadcast("Capture the Bag Event will start in 2 minutes!", BroadCastLoc.World);
                else if (CastleDefense.CountDown == 60)
                    CastleDefense.Broadcast("Capture the Bag Event will start in 1 minute!", BroadCastLoc.World);
                else if (CastleDefense.CountDown == 10)
                {
                    CastleDefense.Stage = CastleDefenseStage.Countdown;
                    if (CastleDefense.CastleDefenseHash.Count < 1)
                    {
                        CastleDefense.CTB = false;
                        CastleDefense.SignUp = false;
                        CastleDefense.Broadcast("The Capture the Bag Event requires atleast 2 players to start! CTB was cancelled!", BroadCastLoc.World);
                        foreach (GameClient gameClient in CastleDefense.CastleDefenseHash.Values)
                            gameClient.MyChar.Teleport(1002U, (ushort)430, (ushort)378);
                        CastleDefense.CastleDefenseHash.Clear();
                        CastleDefense.Stage = CastleDefenseStage.None;
                        return;
                    }
                    CastleDefense.Broadcast("10 seconds until start", BroadCastLoc.Map);
                }
                else if (CastleDefense.CountDown < 4)
                    CastleDefense.Broadcast(CastleDefense.CountDown.ToString() + " seconds until start", BroadCastLoc.Map);
                --CastleDefense.CountDown;
                Thread.Sleep(1000);
            }
            CastleDefense.SignUp = false;
            CastleDefense.CTB = true;
            SpawnWave();
            CastleDefense.TeleportPlayersToMap();
            CastleDefense.Stage = CastleDefenseStage.Fighting;
            CastleDefense.Broadcast("Fight!", BroadCastLoc.World);
        }

        public static void TeleportPlayersToMap()
        {
            foreach (GameClient gameClient in CastleDefense.CastleDefenseHash.Values)
                gameClient.MyChar.Teleport(3976, 156, 165);

            CastleDefense.End = DateTime.UtcNow.AddMinutes(10.0);
        }

        public static void WaitForWinner()
        {
            PKTournament.Stage = PKTournamentStage.Fighting;
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                //SpawnWave();
                foreach (GameClient gameClient in CastleDefenseHash.Values)
                {
                    if (!gameClient.Soc.Connected || gameClient.MyChar.LogOff)
                    {
                        CastleDefense.CastleDefenseHash.Remove(gameClient.MyChar.EntityID);
                        gameClient.MyChar.Teleport(1002U, (ushort)430, (ushort)378);
                        Database.SaveCharacter(gameClient.MyChar, gameClient.AuthInfo.Account);
                        break;
                    }
                }
                foreach (Hashtable G in World.H_Mobs.Values)
                {
                    foreach (Mob M in G.Values)
                    {
                        if (M.Loc.Map == 3976 && DateTime.UtcNow > M.LastMove.AddSeconds(1))
                        {
                            byte Direction = 1;
                            M.LastMove = DateTime.UtcNow;
                            byte ToDir;
                            if (Direction == 1)
                            {
                                if (M.Loc.Y > 171 && M.Loc.X == 103)
                                    M.Direction = 4;
                                else if (M.Loc.Y == 170 && M.Loc.X < 135)
                                    M.Direction = 6;
                                else if (M.Loc.X == 134 && M.Loc.Y < 176)
                                    M.Direction = 0;
                                else
                                {
                                    ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(M.Loc.X, M.Loc.Y, 166, 175) / 45 % 8)) - 1 % 8);
                                    Direction = (byte)((int)ToDir % 8);
                                }
                            }
                            

                            Location eLoc = M.Loc;
                            eLoc.Walk(M.Direction);
                            Hashtable H = (Hashtable)World.H_Mobs[M.Loc.Map];
                            bool PlaceFree = true;

                            if (((DMap)DMaps.H_DMaps[M.Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                            if (PlaceFree)
                            {
                                World.Action(M, Packets.Movement(M.EntityID, M.Direction).Get);
                                World.Spawn(M, true);
                                M.Loc.Walk(M.Direction);
                            }
                            else
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    PlaceFree = true;
                                    eLoc = M.Loc;
                                    M.Direction = (byte)((M.Direction + 1) % 8);
                                    eLoc.Walk(M.Direction);

                                    if (((DMap)DMaps.H_DMaps[M.Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess) PlaceFree = false;

                                    if (PlaceFree)
                                    {
                                        World.Action(M, Packets.Movement(M.EntityID, M.Direction).Get);
                                        World.Spawn(M, true);
                                        M.Loc.Walk(M.Direction);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!(DateTime.UtcNow >= CastleDefense.End))
                {
                    DateTime dateTime = DateTime.UtcNow;
                    dateTime = dateTime.AddMinutes(5.0);
                    int num2;
                    if (dateTime.Minute == CastleDefense.End.Minute)
                    {
                        dateTime = DateTime.UtcNow;
                        num2 = dateTime.Second % 15 == 0 ? 1 : 0;
                    }
                    else
                        num2 = 0;
                    if (num2 != 0)
                    {
                        CastleDefense.Broadcast("The Capture the Bag Event will end in 5 minutes!", BroadCastLoc.Map);
                    }
                    else
                    {
                        dateTime = DateTime.UtcNow;
                        dateTime = dateTime.AddMinutes(3.0);
                        int num3;
                        if (dateTime.Minute == CastleDefense.End.Minute)
                        {
                            dateTime = DateTime.UtcNow;
                            num3 = dateTime.Second % 15 == 0 ? 1 : 0;
                        }
                        else
                            num3 = 0;
                        if (num3 != 0)
                        {
                            CastleDefense.Broadcast("The Capture the Bag Event will end in 3 minutes!", BroadCastLoc.Map);
                        }
                        else
                        {
                            dateTime = DateTime.UtcNow;
                            dateTime = dateTime.AddMinutes(1.0);
                            int num4;
                            if (dateTime.Minute == CastleDefense.End.Minute)
                            {
                                dateTime = DateTime.UtcNow;
                                num4 = dateTime.Second % 15 == 0 ? 1 : 0;
                            }
                            else
                                num4 = 0;
                            if (num4 != 0)
                                CastleDefense.Broadcast("The Capture the Bag Event will end in 1 minute!", BroadCastLoc.Map);
                        }
                    }
                }
                else
                    break;
                if (!CTB)
                    break;
            }
        }

        public static void EndCTB()
        {
            foreach (GameClient GC in CastleDefense.CastleDefenseHash.Values)
            {
                if (!GC.MyChar.Alive)
                {
                    GC.MyChar.Action = (byte)100;
                    GC.MyChar.Stamina = (byte)100;
                    GC.MyChar.Ghost = false;
                    GC.MyChar.BlueName = false;
                    GC.MyChar.CurHP = GC.MyChar.MaxHP;
                    if ((int)GC.MyChar.MaxMP > 1)
                        GC.MyChar.CurMP = GC.MyChar.MaxMP;
                    GC.MyChar.Alive = true;
                    GC.MyChar.StatEff.Remove(StatusEffectEn.Dead);
                    GC.MyChar.StatEff.Remove(StatusEffectEn.BlueName);
                    GC.MyChar.Body = GC.MyChar.Body;
                    GC.MyChar.Hair = GC.MyChar.Hair;
                    GC.MyChar.XPKO = (byte)0;
                    GC.MyChar.Equips.Send(GC, false);
                }
                else
                {
                    GC.MyChar.CurHP = GC.MyChar.MaxHP;
                    GC.MyChar.Stamina = (byte)100;
                    if ((int)GC.MyChar.MaxMP > 1)
                        GC.MyChar.CurMP = GC.MyChar.MaxMP;
                }
                GC.MyChar.PKAble(PKMode.Capture, GC.MyChar);
                GC.MyChar.Teleport(1002U, (ushort)430, (ushort)378);
                GC.AddSend(Packets.ChatMessage(0U, "SYSTEM", "ALLUSERS", "", (ushort)2108, 0U));
                GC.AddSend(Packets.ChatMessage(2U, "SYSTEM", "ALLUSERS", "", (ushort)2108, 0U));
                GC.AddSend(Packets.ChatMessage(3U, "SYSTEM", "ALLUSERS", "", (ushort)2108, 0U));
                GC.MyChar.StatEff.Remove(StatusEffectEn.Flashy);
                GC.MyChar.HasBag = false;
                GC.MyChar.RedTeam = false;
                GC.MyChar.BlueTeam = false;
            }
            
            CastleDefense.CTB = false;
            CastleDefense.CastleDefenseHash.Clear();
            CastleDefense.Stage = CastleDefenseStage.None;
            CastleDefense.PkThread.Abort();
        }
    }
}
