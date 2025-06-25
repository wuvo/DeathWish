using NewestCOServer.Features.Events;
using NewestCOServer.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NewestCOServer.Features
{
    public enum PKTournamentStage
    {
        None,
        Inviting,
        Countdown,
        Fighting,
        Over
    }
    public enum BroadCastLoc
    {
        World,
        Map,
        Score
    }

    public class PVPEvents
    {
        public string EventTitle = "Base Event";
        public EventStage Stage = EventStage.None;
        public static bool War = false;//false
        public static bool Signup = false;
        public bool NoDamage = false;
        public uint MapEvent = 700;
        public bool FFADamage = false;
        public ushort X;
        public ushort Y;
        public bool MagicAllowed = true;
        public bool MeleeAllowed = true;
        public bool FriendlyFire = false;
        public readonly System.Collections.Concurrent.ConcurrentDictionary<uint, Character> Invited = new System.Collections.Concurrent.ConcurrentDictionary<uint, Character>();
        public static Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public static Dictionary<uint, Character> PlayersToRemove = new Dictionary<uint, Character>();
        public static Dictionary<uint, Character> RemovedPlayers = new Dictionary<uint, Character>();
        public readonly Dictionary<uint, int> PlayerScores = new Dictionary<uint, int>();
        public Dictionary<uint, Character> TeamOne = new Dictionary<uint, Character>();
        public Dictionary<uint, Character> TeamTwo = new Dictionary<uint, Character>();
        public Dictionary<uint, Character> TeamThree = new Dictionary<uint, Character>();
        public Dictionary<uint, Character> TeamFour = new Dictionary<uint, Character>();
        public List<ushort> AllowedSkills = new List<ushort>();
        public byte minplayers = 2;
        public int CountDown;
        public double Duration = 20;
        public DateTime EndTime;
        private Thread PkThread;

        public void Broadcast(string msg, BroadCastLoc loc)
        {
            if (loc == BroadCastLoc.World)
            {
                foreach (Character character in World.H_Chars.Values)
                    character.MyClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
            else if (loc == BroadCastLoc.Map)
            {
                foreach (Character C in PlayerList.Values)
                    C.MyClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
        }
        public void Broadcast(string msg, BroadCastLoc loc, uint index)
        {
            if (loc == BroadCastLoc.World)
            {
                foreach (Character character in World.H_Chars.Values)
                    character.MyClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
            else if (loc == BroadCastLoc.Map)
            {
                foreach (Character C in PlayerList.Values)
                    C.MyClient.AddSend(Packets.ChatMessage(0U, "[GM]", "All", msg, (ushort)2011, 0U));
            }
            else if (loc == BroadCastLoc.Score)
            {
                foreach (Character C in PlayerList.Values)
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83d, 0));
            }
        }
        public virtual void AddPlayer(Character c)
        {
            if (Signup == true)
            {
                if (!PlayerList.ContainsKey(c.EntityID))
                {
                    if (!c.BOTJailed)
                    {
                        c.Loc.OldMap = c.Loc.Map;
                        c.Loc.OldX = c.Loc.X;
                        c.Loc.OldY = c.Loc.Y;
                        c.Teleport(1616, 54, 64);
                        PlayerList.Add(c.EntityID, c);
                        Invited.Remove(c.EntityID);
                        c.MyClient.LocalMessage(2000, "You have sucessfully joined the " + EventTitle + " Event!");
                    }
                    else
                        c.MyClient.LocalMessage(2000, "You can't join the event if you're botjailed!");
                }
                else
                    c.MyClient.LocalMessage(2000, "You have already joined the " + EventTitle + " Event!");
            }
        }
        public void RemovePlayer(Character c)
        {
            PlayerList.Remove(c.EntityID);
            c.EventBase = null;
            if (!c.Alive)
            {
                #region Revive
                c.Protection = false;
                c.Action = (byte)100;
                c.Stamina = (byte)100;
                c.Ghost = false;
                c.BlueName = false;
                c.CurHP = c.MaxHP;
                if ((int)c.MaxMP > 1)
                    c.CurMP = c.MaxMP;
                c.Alive = true;
                c.StatEff.Remove(StatusEffectEn.Dead);
                c.StatEff.Remove(StatusEffectEn.BlueName);
                c.Body = c.Body;
                c.Hair = c.Hair;
                c.XPKO = (byte)0;
                #endregion
            }
            else
            {
                c.CurHP = c.MaxHP;
                if (c.MaxMP > 1)
                    c.CurMP = c.MaxMP;
            }

            c.Equips.Send(c.MyClient, false);
            if (c.Equips.Garment.ID == 0)
                c.MyClient.AddSend(Packets.ItemPacket(0, 9, 6));

            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                c.Teleport(1002, 430, 378);
            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 1616 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap))
                c.Teleport(1002, 430, 378);
            else
                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
            Database.SaveCharacter(c, c.MyClient.AuthInfo.Account);
        }

        public void StartTournament()
        {
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            Invited.Clear();
            CountDown = 120;
            Signup = true;

            Stage = EventStage.Inviting;
            PkThread = new Thread((ThreadStart)(() =>
            {
                BeginTournament();
                WaitForWinner();
                End();
            }));
            PkThread.IsBackground = true;
            PkThread.Start();
        }

        public virtual void BeginTournament()
        {
            World.SendMsgToAll("[EVENT]", EventTitle + " Event has started! Type /joinpvp if you want to join!", 2500, 0);
            foreach (Character character in World.H_Chars.Values)
            {
                if (character != null)
                {
                    Invited.TryAdd(character.EntityID, character);
                    character.EventBase = this;
                    if (character.Invitations && character.Loc.Map != 1038 && !World.EventsMaps.Contains(character.Loc.Map))
                    {
                        character.MyClient.DialogNPC = 13654;
                        NPCs.NPCHandler.Handle(character.MyClient, null, 13654, 0);
                    }
                }
            }
            while (CountDown > 0)
            {
                foreach (Character C in Invited.Values)
                    if (C.EventBase == null)
                        C.EventBase = this;
                if (CountDown == 120)
                    Broadcast(EventTitle + " Event will start in 2 minutes!", BroadCastLoc.World);
                else if (CountDown == 60)
                    Broadcast(EventTitle + " Event will start in 1 minute!", BroadCastLoc.World);
                else if (CountDown == 10)
                {
                    Stage = EventStage.Countdown;
                    if (!CanStart())
                    {
                        War = false;
                        Signup = false;
                        Broadcast("The " + EventTitle + " Event requires atleast " + minplayers + " players to start! Event was cancelled!", BroadCastLoc.World);
                        foreach (Character c in PlayerList.Values)
                        {
                            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                                c.Teleport(1002, 430, 378);
                            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 1616 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap))
                                c.Teleport(1002, 430, 378);
                            else
                                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                        }
                        foreach (var C in Invited.Values)
                            C.EventBase = null;
                        PlayerList.Clear();
                        Invited.Clear();
                        Stage = EventStage.None;
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
            Signup = false;
            War = true;
            TeleportPlayersToMap();
            foreach (var v in Invited.Values)
                v.EventBase = null;
            foreach (var v in PlayersToRemove.Values)
                RemovePlayer(v);

            PlayersToRemove.Clear();
            Invited.Clear();
            Stage = EventStage.Fighting;
            foreach (Character C in PlayerList.Values)
                    World.Action(C, (Packets.String(C.EntityID, 10, "downnumber5")).Get);
            Thread.Sleep(1000);
            foreach (Character C in PlayerList.Values)
                World.Action(C, (Packets.String(C.EntityID, 10, "downnumber4")).Get);
            Thread.Sleep(1000);
            foreach (Character C in PlayerList.Values)
                World.Action(C, (Packets.String(C.EntityID, 10, "downnumber3")).Get);
            Thread.Sleep(1000);
            foreach (Character C in PlayerList.Values)
                World.Action(C, (Packets.String(C.EntityID, 10, "downnumber2")).Get);
            Thread.Sleep(1000);
            foreach (Character C in PlayerList.Values)
                World.Action(C, (Packets.String(C.EntityID, 10, "downnumber1")).Get);
            Thread.Sleep(1000);
            Removeprotection();
            EndTime = DateTime.UtcNow.AddMinutes(Duration);
            Broadcast(EventTitle + " Event has started! May the best player win!", BroadCastLoc.World);
        }

        /// <summary>
        /// Do all the requirement checks to start the event in here
        /// </summary>
        /// <returns></returns>
        public virtual bool CanStart()
        {
            return PlayerList.Count >= 2;
        }
        
        /// <summary>
        /// Here we choose who we want to reward and such, may depend on teams or w/e... Should add support for teams
        /// </summary>
        public virtual void End()
        {
            if (PlayerList.Count == 1)
            {
                foreach (var c in PlayerList.Values)
                {
                    Reward(c);
                    TeleportOut(c);

                    c.MyClient.LocalMessage(2108, "");
                    c.CurHP = c.MaxHP;
                }
            }
            else
                Broadcast(Duration + " minutes have passed and no one won the " + EventTitle + " Event! Better luck next time!", BroadCastLoc.World);
            foreach (var c in PlayerList.Values)
            {
                TeleportOut(c);
                c.MyClient.LocalMessage(2108, "");
                c.CurHP = c.MaxHP;
            }

            Removeprotection();
            War = false;
            PlayerList.Clear();
            PlayersToRemove.Clear();
            RemovedPlayers.Clear();
            PlayerScores.Clear();
            Abort();
            return;
        }

        public void Abort()
        {
            PkThread.Abort();
        }

        /// <summary>
        /// Used to choose which rewards we want to give
        /// </summary>
        public virtual void Reward(Character c)
        {
            bool DB = false;
            c.Silvers += 100000;
            World.Action(c, (Packets.String(c.EntityID, 10, "angelwing")).Get);
            if (c.Level < 130)
                c.IncreaseExp(c.ExpBallExp / 2, false, false);
            if (c.Inventory.Count > 38)
            {
                Broadcast(c.Name + " has won the hourly " + EventTitle + " Tournament!", BroadCastLoc.World);
                c.MyClient.LocalMessage(2000, "You didn't have enough space in your inventory and so you didn't receive a reward!");
                return;
            }
            
            int b = Program.Rnd.Next(0, 5);
            if (b == 0)
            {
                #region +1 Item
                for (int a = 0; a < 2; a++)
                {
                top:
                    Item I2 = new Item();
                    I2.UID = (uint)Program.Rnd.Next(10000000);
                    Item.ItemQuality Q = Item.ItemQuality.Normal;

                    uint ItemID = 0;
                    List<uint> From = new List<uint>();
                    int Type = Program.Rnd.Next(0, 255);
                    uint Part = 0;
                    if (Type < 10) Part = 111;
                    else if (Type < 20) Part = 113;
                    else if (Type < 30) Part = 114;
                    else if (Type < 40) Part = 117;
                    else if (Type < 50) Part = 118;
                    else if (Type < 60) Part = 120;
                    else if (Type < 70) Part = 121;
                    else if (Type < 80) Part = 130;
                    else if (Type < 90) Part = 131;
                    else if (Type < 100) Part = 133;
                    else if (Type < 110) Part = 134;
                    else if (Type < 120) Part = 141;
                    else if (Type < 130) Part = 142;
                    else if (Type < 140) Part = 150;
                    else if (Type < 150) Part = 151;
                    else if (Type < 160) Part = 152;
                    else if (Type < 165) Part = 160;
                    else if (Type < 175) Part = 410;
                    else if (Type < 185) Part = 420;
                    else if (Type < 195) Part = 480;
                    else if (Type < 205) Part = 481;
                    else if (Type < 215) Part = 500;
                    else if (Type < 225) Part = 530;
                    else if (Type < 235) Part = 560;
                    else if (Type < 245) Part = 561;
                    else if (Type < 255) Part = 900;

                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                    {
                        if (D.LevReq >= 5 && D.LevReq <= 110)
                        {
                            if (D.LevReq != 0)
                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                    From.Add(D.ID);
                        }
                    }
                    if (From != null)
                    {
                        if (From.Count > 0)
                        {
                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                            ItemID = (uint)From[Tries];
                        }
                    }
                    if (ItemID != 0)
                    {
                        I2.ID = ItemID;
                        if (I2.DBInfo.LevReq != 1)
                        {
                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                            E.QualityChange(Q);
                            I2.ID = E.ToID();
                        }

                        I2.Color = Item.ArmorColor.Orange;

                        I2.Plus = 1;
                        I2.MaxDur = I2.DBInfo.Durability;
                        I2.CurDur = I2.MaxDur;

                        c.AddItem(I2);
                    }
                    else goto top;
                }
                #endregion
            }
            else if (b == 1)
            {
                c.AddItem(1088000);
                DB = true;
            }
            else if (b == 2)
            {
                c.AddItem(720027);
            }
            else if (b == 3)
            {
                for (int D = 0; D < 2; D++)
                    c.AddItem(723017);
            }
            else if (b == 4)
                c.AddItem(722384);
            
            if (DB == true)
                Broadcast(c.Name + " has won the hourly " + EventTitle + " Tournament and received a DragonBall!", BroadCastLoc.World);
            else
                Broadcast(c.Name + " has won the hourly " + EventTitle + " Tournament!", BroadCastLoc.World);
        }
        public void TeleportOut(Character c)
        {
            c.EventBase = null;
            c.Equips.Send(c.MyClient, false);
            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                c.Teleport(1002, 430, 378);
            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 1616 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap))
                c.Teleport(1002, 430, 378);
            else
                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
        }

        public virtual void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                c.Teleport(MapEvent, X, Y);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                PlayerScores.Add(c.EntityID, 0);
                //if (c.Loc.Map == 1616)
                //{
                   
                //}
                //else
                //{
                //    c.MyClient.LocalMessage(2000, "You've been removed from the " + EventTitle + " Event!");
                //    PlayersToRemove.Add(c.EntityID, c);
                //    //PlayerList.Remove(c.EntityID);
                //    //break;
                //}
            }
        }

        public virtual void WaitForWinner()
        {
            uint num1 = (uint)Environment.TickCount;
            while (true)
            {
                foreach (Character C in PlayerList.Values)
                {
                    if (!C.MyClient.Soc.Connected || C.LogOff || C.Loc.Map != MapEvent)
                        PlayersToRemove.Add(C.EntityID, C);

                    else if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddSeconds(2))
                        PlayersToRemove.Add(C.EntityID, C);
                }
                foreach (Character C in PlayersToRemove.Values)
                {
                    C.EventBase?.RemovePlayer(C);
                    Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                    RemovedPlayers.Add(C.EntityID, C);
                }
                foreach(Character C in RemovedPlayers.Values)
                {
                    if (PlayersToRemove.ContainsKey(C.EntityID))
                        PlayersToRemove.Remove(C.EntityID);
                }
                if (DateTime.UtcNow >= EndTime)
                    break;

                else if (PlayerList.Count == 1)
                    break;

                if (!War)
                    break;
            }
        }

        public void Removeprotection()
        {
            foreach (Character c in PlayerList.Values)
            {
                c.Protection = false;
            }
        }

        public virtual void DisplayScore()
        {
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            foreach (var kvp in PlayerScores.OrderByDescending(s => s.Value))
            {
                PlayerList[kvp.Key].MyClient.AddSend(Packets.ChatMessage(2, "SYSTEM", "ALLUSERS", $"{PlayerList[kvp.Key].Name} - {kvp.Value}", 0x83d, 0));
            }
        }

        public virtual void Kill(Character player, Character entity)
        {
            if (PlayerScores.ContainsKey(player.EntityID))
                PlayerScores[player.EntityID]++;
        }
        public virtual void Hit(Character Attacker, Character Victim)
        {

        }
    }
}
