
// Decompiled with JetBrains decompiler
// Type: NewestCOServer.Features.CaptureTheBag
// Assembly: NewestCOServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 00024EDF-C51F-412A-AB8C-AFC3DF039B78
// Assembly location: C:\Users\Proprietário\Desktop\NewestCOServer.exe

using NewestCOServer.Game;
using NewestCOServer.Main;
using NewestCOServer.PacketHandling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace NewestCOServer.Features
{
    public enum CaptureTheBagStage
    {
        None,
        Inviting,
        Countdown,
        Fighting,
        Over
    }
    
    public static class CaptureTheBag
    {
        private static int BTScore = 0, RTScore = 0;
        public static CaptureTheBagStage Stage = CaptureTheBagStage.None;
        public static bool CTB = false, Blue = false, Red = false;
        private static ushort Map, X, Y;
        private static Dictionary<uint, GameClient> CaptureTheBagHash, PlayersToRemove, RemovedPlayers;
        public static Dictionary<uint, GameClient> BlueTeam, RedTeam;
        public static int CountDown;
        private static Thread PkThread;
        private static DateTime LastScores, End;

        private static void SendScores()
        {
            foreach (GameClient GC in CaptureTheBagHash.Values)
            {
                GC.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", "Capture the Bag Scores", 0x83c, 0));
                if (BTScore > RTScore)
                {
                    GC.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", "Blue Team: " + BTScore + " ", 0x83d, 0));
                    GC.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", "Red Team: " + RTScore + " ", 0x83d, 0));
                }
                else
                {
                    GC.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", "Red Team: " + RTScore + " ", 0x83d, 0));
                    GC.AddSend(Packets.ChatMessage(3, "SYSTEM", "ALLUSERS", "Blue Team: " + BTScore + " ", 0x83d, 0));
                }
                LastScores = DateTime.UtcNow;
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
                foreach (GameClient gameClient in CaptureTheBagHash.Values)
                    gameClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
        }

        public static void AddPlayer(GameClient GC)
        {
            CaptureTheBagHash.Add(GC.MyChar.EntityID, GC);
            if (RedTeam.Count <= BlueTeam.Count)
                RedTeam.Add(GC.MyChar.EntityID, GC);
            else
                BlueTeam.Add(GC.MyChar.EntityID, GC);
            GC.MyChar.Teleport((uint)Map, X, Y);
        }

        public static void StartTournament()
        {
            CaptureTheBagHash = new Dictionary<uint, GameClient>();
            RedTeam = new Dictionary<uint, GameClient>();
            BlueTeam = new Dictionary<uint, GameClient>();
            PlayersToRemove = new Dictionary<uint, GameClient>();
            RemovedPlayers = new Dictionary<uint, GameClient>();
            CountDown = 180;
            Stage = CaptureTheBagStage.Inviting;
            Map = (ushort)1616;
            X = (ushort)54;
            Y = (ushort)64;
            PkThread = new Thread((ThreadStart)(() =>
            {
                BeginTournament();
                WaitForWinner();
                EndCTB();
            }));
            PkThread.IsBackground = true;
            PkThread.Start();
        }

        private static void BeginTournament()
        {
            foreach (Character character in World.H_Chars.Values)
            {
                character.MyClient.DialogNPC = 2037;
                NPCs.NPCHandler.Handle(character.MyClient, null, 2037, 0);
            }
            while (CountDown > 0)
            {
                if (CountDown == 120)
                    Broadcast("Capture the Bag Event will start in 2 minutes!", BroadCastLoc.World);
                else if (CountDown == 60)
                    Broadcast("Capture the Bag Event will start in 1 minute!", BroadCastLoc.World);
                else if (CountDown == 10)
                {
                    if (CaptureTheBagHash.Count < 2)
                    {
                        CTB = false;
                        Broadcast("The Capture the Bag Event requires atleast 2 players to start! CTB was cancelled!", BroadCastLoc.World);
                        foreach (GameClient gameClient in CaptureTheBagHash.Values)
                            gameClient.MyChar.Teleport(1002U, (ushort)430, (ushort)378);
                        CaptureTheBagHash.Clear();
                        RedTeam.Clear();
                        BlueTeam.Clear();
                        Stage = CaptureTheBagStage.None;
                        PkThread.Abort();
                        return;
                    }
                    Broadcast("10 seconds until start", BroadCastLoc.Map);
                }
                else if (CountDown < 6)
                    Broadcast(CountDown.ToString() + " seconds until start", BroadCastLoc.Map);
                --CountDown;
                Thread.Sleep(1000);
            }
            CTB = true;
            TeleportPlayersToMap();
            DropRed();
            DropBlue();
            Stage = CaptureTheBagStage.Fighting;
            Broadcast("Fight!", BroadCastLoc.World);
        }

        private static void TeleportPlayersToMap()
        {
            foreach (GameClient gameClient in RedTeam.Values)
            {
                if (gameClient.MyChar.Flying)
                    gameClient.MyChar.Flying = false;
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.Fly);
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.Cyclone);
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.SuperMan);
                gameClient.MyChar.RedTeam = true;
                gameClient.MyChar.BlueTeam = false;
                gameClient.MyChar.Teleport(1080U, (ushort)175, (ushort)205);
                gameClient.MyChar.CurHP = (ushort)20;
                gameClient.AddSend(Packets.OverwriteGarment(191305));
                gameClient.LocalMessage((ushort)2021, "Congratulations! You have joined the Red Team! Please use Team mode in order to avoid hitting your teammates!");
            }
            foreach (GameClient gameClient in BlueTeam.Values)
            {
                if (gameClient.MyChar.Flying)
                    gameClient.MyChar.Flying = false;
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.Fly);
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.Cyclone);
                gameClient.MyChar.StatEff.Remove(StatusEffectEn.SuperMan);
                gameClient.MyChar.RedTeam = false;
                gameClient.MyChar.BlueTeam = true;
                gameClient.MyChar.Teleport(1080U, (ushort)95, (ushort)35);
                gameClient.MyChar.CurHP = (ushort)20;
                gameClient.AddSend(Packets.OverwriteGarment(183425));
                gameClient.LocalMessage((ushort)2021, "Congratulations! You have joined the Blue Team! Please use Team mode in order to avoid hitting your teammates!");
            }
            foreach (GameClient gameClient in CaptureTheBagHash.Values)
            {
                foreach (GameClient CC in CaptureTheBagHash.Values)
                {
                    Database.SaveCharacter(gameClient.MyChar, gameClient.AuthInfo.Account);
                    if (CC.MyChar.Loc.Map == gameClient.MyChar.Loc.Map)
                    {
                        try
                        {
                            CC.AddSend(Packets.SpawnEntity(gameClient.MyChar));
                            gameClient.AddSend(Packets.SpawnEntity(CC.MyChar));
                        }
                        catch { }
                    }
                }

            }
            End = DateTime.UtcNow.AddMinutes(10.0);
        }

        private static void DropRed()
        {
            if (Red)
                return;
            DroppedItem droppedItem = new DroppedItem();
            droppedItem.DropTime = DateTime.UtcNow;
            droppedItem.Loc = new Location();
            droppedItem.Loc.Map = 1080U;
            droppedItem.Info = new Item();
            droppedItem.Info.ID = 710100U;
            droppedItem.UID = (uint)Program.Rnd.Next(10000000);
            droppedItem.Info.UID = droppedItem.UID;
            droppedItem.Loc.X = (ushort)180;
            droppedItem.Loc.Y = (ushort)215;
            //if (!droppedItem.FindPlace((ConcurrentDictionary<uint, DroppedItem>)World.H_Items[(object)1080]))
            //    return;
            droppedItem.Drop();
            Red = true;
        }

        private static void DropBlue()
        {
            if (Blue)
                return;
            DroppedItem droppedItem = new DroppedItem();
            droppedItem.DropTime = DateTime.UtcNow;
            droppedItem.Loc = new Location();
            droppedItem.Loc.Map = 1080U;
            droppedItem.Info = new Item();
            droppedItem.Info.ID = 722741U;
            droppedItem.UID = (uint)Program.Rnd.Next(10000000);
            droppedItem.Info.UID = droppedItem.UID;
            droppedItem.Loc.X = (ushort)93;
            droppedItem.Loc.Y = (ushort)19;
            //if (!droppedItem.FindPlace((ConcurrentDictionary<uint, DroppedItem>)World.H_Items[(object)1080]))
            //    return;
            droppedItem.Drop();
            Blue = true;
        }

        private static bool InBase(Character C)
        {
            if (BlueTeam.ContainsKey(C.EntityID))
            {
                if (C.Loc.X >= 91 && C.Loc.X <= 96 && C.Loc.Y >= 17 && C.Loc.Y <= 22)
                    return true;
            }
            else if (RedTeam.ContainsKey(C.EntityID))
                if (C.Loc.X >= 178 && C.Loc.X <= 183 && C.Loc.Y >= 213 && C.Loc.Y <= 218)
                    return true;

            return false;
        }

        private static void WaitForWinner()
        {
            PKTournament.Stage = PKTournamentStage.Fighting;
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (GameClient gameClient in CaptureTheBagHash.Values)
                {
                    if (!gameClient.Soc.Connected || gameClient.MyChar.LogOff || (int)gameClient.MyChar.Loc.Map != 1080)
                    {
                        PlayersToRemove.Add(gameClient.MyChar.EntityID, gameClient);
                        if (gameClient.MyChar.HasBag)
                        {
                            if (RedTeam.ContainsKey(gameClient.MyChar.EntityID))
                                DropBlue();
                            else if (BlueTeam.ContainsKey(gameClient.MyChar.EntityID))
                                DropRed();

                            gameClient.MyChar.HasBag = false;
                        }
                    }


                    if (!gameClient.MyChar.Alive)
                    {
                        if (gameClient.MyChar.HasBag)
                        {
                            gameClient.MyChar.StatEff.Remove(StatusEffectEn.Flashy);
                            gameClient.MyChar.HasBag = false;
                            if (BlueTeam.ContainsKey(gameClient.MyChar.EntityID))
                            {
                                DropRed();
                                RTScore += 6;
                                Broadcast(gameClient.MyChar.Name + " from the BlueTeam was killed while holding the RedBag!", BroadCastLoc.Map);
                            }
                            else if (RedTeam.ContainsKey(gameClient.MyChar.EntityID))
                            {
                                DropBlue();
                                BTScore += 6;
                                Broadcast(gameClient.MyChar.Name + " from the RedTeam was killed while holding the RedBag!", BroadCastLoc.Map);
                            }
                        }
                        if (DateTime.UtcNow > gameClient.MyChar.DeathHit.AddSeconds(10.0))
                        {
                            #region Revive
                            gameClient.MyChar.Action = (byte)100;
                            gameClient.MyChar.Stamina = (byte)100;
                            gameClient.MyChar.Ghost = false;
                            gameClient.MyChar.BlueName = false;
                            gameClient.MyChar.CurHP = (ushort)20;
                            if ((int)gameClient.MyChar.MaxMP > 1)
                                gameClient.MyChar.CurMP = gameClient.MyChar.MaxMP;
                            gameClient.MyChar.Alive = true;
                            gameClient.MyChar.StatEff.Remove(StatusEffectEn.Dead);
                            gameClient.MyChar.StatEff.Remove(StatusEffectEn.BlueName);
                            gameClient.MyChar.Body = gameClient.MyChar.Body;
                            gameClient.MyChar.Hair = gameClient.MyChar.Hair;
                            gameClient.MyChar.XPKO = (byte)0;
                            #endregion
                            if (BlueTeam.ContainsKey(gameClient.MyChar.EntityID))
                            {
                                gameClient.MyChar.Teleport(1080U, (ushort)95, (ushort)35);
                                RTScore += 2;
                            }
                            else if (RedTeam.ContainsKey(gameClient.MyChar.EntityID))
                            {
                                BTScore += 2;
                                gameClient.MyChar.Teleport(1080U, (ushort)175, (ushort)205);
                            }
                        }
                    }
                    else
                    {
                        if (gameClient.MyChar.CurHP > 20)
                            gameClient.MyChar.CurHP = 20;

                        if (InBase(gameClient.MyChar))
                        {
                            if (gameClient.MyChar.HasBag)
                            {
                                gameClient.MyChar.StatEff.Remove(StatusEffectEn.Flashy);
                                gameClient.MyChar.HasBag = false;
                                if (BlueTeam.ContainsKey(gameClient.MyChar.EntityID))
                                {
                                    DropRed();
                                    BTScore += 40;
                                    Broadcast(gameClient.MyChar.Name + " from the BlueTeam has sucessfully retrieved the RedBag!", BroadCastLoc.Map);
                                }
                                else if (RedTeam.ContainsKey(gameClient.MyChar.EntityID))
                                {
                                    DropBlue();
                                    RTScore += 40;
                                    Broadcast(gameClient.MyChar.Name + " from the RedTeam has sucessfully retrieved the BlueBag!", BroadCastLoc.Map);
                                }
                            }
                            else
                            {
                                if (BlueTeam.ContainsKey(gameClient.MyChar.EntityID))
                                    gameClient.MyChar.Teleport(1080U, (ushort)95, (ushort)35);
                                else if (RedTeam.ContainsKey(gameClient.MyChar.EntityID))
                                    gameClient.MyChar.Teleport(1080U, (ushort)175, (ushort)205);
                                gameClient.LocalMessage((ushort)2021, "You can't be inside your team's base!");
                            }
                        }
                    }
                }
                foreach (GameClient GC in PlayersToRemove.Values)
                {
                    if (BlueTeam.ContainsKey(GC.MyChar.EntityID))
                        BlueTeam.Remove(GC.MyChar.EntityID);
                    else if (RedTeam.ContainsKey(GC.MyChar.EntityID))
                        RedTeam.Remove(GC.MyChar.EntityID);
                    CaptureTheBagHash.Remove(GC.MyChar.EntityID);
                    GC.MyChar.Teleport(1002U, (ushort)430, (ushort)378);
                    GC.MyChar.Equips.Send(GC, false);
                    Database.SaveCharacter(GC.MyChar, GC.AuthInfo.Account);
                    RemovedPlayers.Add(GC.MyChar.EntityID, GC);
                }
                foreach (GameClient GC in RemovedPlayers.Values)
                    PlayersToRemove.Remove(GC.MyChar.EntityID);

                if (DateTime.UtcNow >= LastScores.AddMilliseconds(5000))
                    SendScores();

                if (!(DateTime.UtcNow >= End))
                {
                    DateTime dateTime = DateTime.UtcNow;
                    dateTime = dateTime.AddMinutes(5.0);
                    int num2;
                    if (dateTime.Minute == End.Minute)
                    {
                        dateTime = DateTime.UtcNow;
                        num2 = dateTime.Second % 15 == 0 ? 1 : 0;
                    }
                    else
                        num2 = 0;
                    if (num2 != 0)
                    {
                        Broadcast("The Capture the Bag Event will end in 5 minutes!", BroadCastLoc.Map);
                    }
                    else
                    {
                        dateTime = DateTime.UtcNow;
                        dateTime = dateTime.AddMinutes(3.0);
                        int num3;
                        if (dateTime.Minute == End.Minute)
                        {
                            dateTime = DateTime.UtcNow;
                            num3 = dateTime.Second % 15 == 0 ? 1 : 0;
                        }
                        else
                            num3 = 0;
                        if (num3 != 0)
                        {
                            Broadcast("The Capture the Bag Event will end in 3 minutes!", BroadCastLoc.Map);
                        }
                        else
                        {
                            dateTime = DateTime.UtcNow;
                            dateTime = dateTime.AddMinutes(1.0);
                            int num4;
                            if (dateTime.Minute == End.Minute)
                            {
                                dateTime = DateTime.UtcNow;
                                num4 = dateTime.Second % 15 == 0 ? 1 : 0;
                            }
                            else
                                num4 = 0;
                            if (num4 != 0)
                                Broadcast("The Capture the Bag Event will end in 1 minute!", BroadCastLoc.Map);
                        }
                    }
                }
                else
                    break;
                if (!CTB)
                    break;
            }
        }

        private static void EndCTB()
        {
            foreach (GameClient GC in CaptureTheBagHash.Values)
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
                    GC.MyChar.Equips.Send(GC, false);
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
            if (RTScore > BTScore)
            {
                Broadcast("Red Team won the Capture the Bag Event!", BroadCastLoc.World);
                foreach (GameClient gameClient in RedTeam.Values)
                {
                    if ((int)gameClient.MyChar.CTBPoints < 60000)
                    {
                        gameClient.MyChar.CTBPoints += (ushort)250;
                        gameClient.LocalMessage((ushort)2021, "Congratulations! Your team has won the Capture the Bag Event and you've received 250 CTBPoints!");
                    }
                    else
                        gameClient.LocalMessage((ushort)2021, "You have reached the limit of CTBPoints and so you were not awarded. Please use your CTBPoints!");
                }
            }
            else if (BTScore > RTScore)
            {
                Broadcast("Blue Team won the Capture the Bag Event!", BroadCastLoc.World);
                foreach (GameClient gameClient in BlueTeam.Values)
                {
                    if ((int)gameClient.MyChar.CTBPoints < 60000)
                    {
                        gameClient.MyChar.CTBPoints += (ushort)250;
                        gameClient.LocalMessage((ushort)2021, "Congratulations! Your team has won the Capture the Bag Event and you've received 250 CTBPoints!");
                    }
                    else
                        gameClient.LocalMessage((ushort)2021, "You have reached the limit of CTBPoints and so you were not awarded. Please use your CTBPoints!");
                }
            }
            else
            {
                Broadcast("It's a tie! Both teams have scored the same amount of points in the Capture the Bag Event!", BroadCastLoc.World);
                foreach (GameClient gameClient in CaptureTheBagHash.Values)
                {
                    if ((int)gameClient.MyChar.CTBPoints < 60000)
                    {
                        gameClient.MyChar.CTBPoints += (ushort)100;
                        gameClient.LocalMessage((ushort)2021, "It seems like it's a tie! Both teams have received 100 CTBPoints!");
                    }
                    else
                        gameClient.LocalMessage((ushort)2021, "You have reached the limit of CTBPoints and so you were not awarded. Please use your CTBPoints!");
                }
            }
            CTB = false;
            RTScore = 0;
            BTScore = 0;
            CaptureTheBagHash.Clear();
            RedTeam.Clear();
            BlueTeam.Clear();
            RemovedPlayers.Clear();
            PlayersToRemove.Clear();
            Stage = CaptureTheBagStage.None;
            PkThread.Abort();
            return;
        }
    }
}
