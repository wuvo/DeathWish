using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ultimate.Structures;

namespace Ultimate.Events
{
    public enum EventStage
    {
        None,
        Inviting,
        Countdown,
        Starting,
        Fighting,
        Over
    }
    public enum BroadCastLoc
    {
        World,
        Map,
        Score,
        Title,
        SpecificMap
    }
    public class Events
    {
        #region Properties
        public string EventTitle = "Base Event";
        public EventStage Stage = EventStage.None;
        public bool NoDamage = false;
        public ushort BaseMap = 700;
        public uint MapEvent = 11000;
        public bool FFADamage = false;
        public bool Reflect = false;
        public ushort X;
        public ushort Y;
        public bool MagicAllowed = true;
        public bool MeleeAllowed = true;
        public bool FriendlyFire = false;
        public Dictionary<uint, Character> PlayerList = new Dictionary<uint, Character>();
        public readonly Dictionary<uint, int> PlayerScores = new Dictionary<uint, int>();
        public Dictionary<uint, Dictionary<uint, Character>> Teams = new Dictionary<uint, Dictionary<uint, Character>>();
        public List<ushort> AllowedSkills = new List<ushort>();
        public byte minplayers = 2;
        public int CountDown;
        public int DialogID = 0;
        public double Duration = 20;
        public DateTime EndTime;
        public DateTime DisplayScores;
        #endregion

        /// <summary>
        /// Used to send messages related to the current PVP Event
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loc"></param>
        /// <param name="index"></param>
        public void Broadcast(string msg, BroadCastLoc loc, uint index = 0, uint Map = 0)
        {
            if (loc == BroadCastLoc.World)
            {
                foreach (Character character in World.H_Chars.Values)
                    character.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 2011, 0U));
            }
            else if (loc == BroadCastLoc.Map)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 2011, 0U));
            }
            else if (loc == BroadCastLoc.Score)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83d, 0));
            }
            else if (loc == BroadCastLoc.Title)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83c, 0));
            }
            else if (loc == BroadCastLoc.SpecificMap)
            {
                foreach (Character C in PlayerList.Values.ToList())
                    if (C.Loc.Map == Map)
                        C.MyClient.AddSend(Packets.ChatMessage(index, "[GM]", "All", msg, 0x83c, 0));
            }
        }

        /// <summary>
        /// Adds a player to the current PVP Event
        /// </summary>
        /// <param name="c"></param>
        public virtual bool AddPlayer(Character c)
        {
            if (Stage == EventStage.Inviting)
            {
                if (/*c.Arena == null || c.Loc.Map != c.Arena.MapID*/ !DMaps.EventMaps.ContainsKey(c.Loc.Map))
                {
                    if (c.ArenaQualifier == null || c.ArenaQualifier.Status == MatchStatus.None /*&& c.Loc.Map != c.ArenaQualifier.MapID*/)
                    {
                        if (c.Loc.Map != 1038 && c.Loc.Map != 6001 && c.Loc.Map != 1844 && c.Loc.Map != 1505)
                        {
                            if (!PlayerList.ContainsKey(c.EntityID))
                            {
                                if (!c.BOTJailed)
                                {
                                    c.Loc.OldMap = c.Loc.Map;
                                    c.Loc.OldX = c.Loc.X;
                                    c.Loc.OldY = c.Loc.Y;
                                    c.Teleport(2068, 47, 35);
                                    PlayerList.Add(c.EntityID, c);
                                    PlayerScores.Add(c.EntityID, 0);
                                    c.MyClient.LocalMessage(2000, "You have sucessfully joined the " + EventTitle + " Event, if you want to giveup use '/giveup' !");
                                    if (!c.Alive)
                                        RevivePlayer(c, c.MaxHP);
                                    c.AddItem(410005);
                                   
                                    return true;

                                }
                                else
                                    c.MyClient.LocalMessage(2000, "You can't join the event if you're botjailed!");
                            }
                            else
                                c.MyClient.LocalMessage(2000, "You have already joined the " + EventTitle + " Event!");
                        }
                        else
                            c.MyClient.LocalMessage(2000, "You can't join the " + EventTitle + " Event in the current map!");
                    }
                    else
                        c.MyClient.LocalMessage(2000, "You can't join a PVP Event while you're fighting at the Arena Qualifier!");
                }
                else
                    c.MyClient.LocalMessage(2000, "You can't join a PVP Event while you're fighting at the Skill Arena!");
            }
            else
                c.MyClient.LocalMessage(2000, "There are no events running");
            return false;
        }

        /// <summary>
        /// Removes a player from the current PVP Event
        /// </summary>
        /// <param name="c"></param>
        public virtual void RemovePlayer(Character c, bool exp = true)
        {
            PlayerList.Remove(c.EntityID);
            PlayerScores.Remove(c.EntityID);
            //if (PlayerScores.ContainsKey(c.EntityID))

            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                if (T.Value.ContainsKey(c.EntityID))
                    T.Value.Remove(c.EntityID);

            if (c.Level < 130 && (c.Job < 100 || c.Job > 135) && exp)
                c.AddExp(1);

            c.EventBase = null;
            RevivePlayer(c, c.MaxHP);
            c.Protection = false;

            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                c.Teleport(1002, 430, 378);
            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 2068 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap))
                c.Teleport(1002, 430, 378);
            else
                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);

            if (c.MyClient.Soc.Connected && !c.LogOff)
            {
                c.Equips.Send(c.MyClient, false);
                ChangePKMode(c, PKMode.Capture);

                if (c.Equips.Garment.ID == 0)
                    c.MyClient.AddSend(Packets.ItemPacket(0, 9, 6));
            }
            Database.SaveCharacter(c, c.MyClient.AuthInfo.Account);
            c.RemoveItem(410005);
        }

        /// <summary>
        /// Tells the server which part of the PVP Event we want to execute next
        /// </summary>
        public void ActionHandler()
        {
            if (Stage == EventStage.Inviting)
                Inviting();
            else if (Stage == EventStage.Countdown || Stage == EventStage.Starting)
                Countdown();
            else if (Stage == EventStage.Fighting)
                WaitForWinner();
            else if (Stage == EventStage.Over)
            {
                Stage = EventStage.None;
                End();
                DMaps.DeleteDynamicMap(MapEvent, true);
                World.Events.Remove(this);
            }
        }

        /// <summary>
        /// Sets events' configuration and starts it, possible to determine how long we want signup to last
        /// </summary>
        /// <param name="_signuptime"></param>
        public void StartTournament(int _signuptime = 60)//120
        {
            PlayerList.Clear();
            PlayerScores.Clear();
            CountDown = _signuptime;
            Stage = EventStage.Inviting;
            World.Events.Add(this);

            BeginTournament();
        }

        /// <summary>
        /// Initializes tournament
        /// </summary>
        public virtual void BeginTournament()
        {
            World.SendMsgToAll("[EVENT]", EventTitle + " Event has started! Type @joinpvp if you want to join!", 2500, 0);
            foreach (Character C in World.H_Chars.Values)
            {
                if (C.Invitations && C.EventBase == null && C.Loc.Map != 1038 && !World.EventsMaps.Contains(C.Loc.Map) && !DMaps.EventMaps.ContainsKey(C.Loc.Map) && (C.ArenaQualifier == null || C.ArenaQualifier.Status == MatchStatus.None))
                {

                    //if (DialogID == 0)
                    //{
                    //    C.MyClient.DialogNPC = 13654;
                    //    NPCs.NPCHandler.Handle(C.MyClient, null, 13654, 0);
                    //}
                    //else
                    // C.MyClient.AddSend(Packets.ShowDialog(DialogID, 1));
                    World.HourlyEvent = EventTitle;
                    NPCs.NPCHandler.Handle(C.MyClient, null, 13654, 0);
                }
            }
        }

        /// <summary>
        /// Handles the logic while the event is on sign-up
        /// </summary>
        public virtual void Inviting()
        {
            if (CountDown > 0)
            {
                if (CountDown == 120)
                    Broadcast(EventTitle + " Event will start in 2 minutes!", BroadCastLoc.World);
                else if (CountDown == 60)
                    Broadcast(EventTitle + " Event will start in 1 minute!", BroadCastLoc.World);
                else if (CountDown == 10)
                {
                    foreach (Character character in World.H_Chars.Values)
                    {
                        if (character.Invitations && character.Loc.Map != 1038 && !World.EventsMaps.Contains(character.Loc.Map))
                            character.MyClient.AddSend(Packets.ShowDialog(DialogID, 0));
                    }
                    if (!CanStart())
                    {
                        Broadcast("The " + EventTitle + " Event requires atleast " + minplayers + " players to start! Event was cancelled!", BroadCastLoc.World);
                        Broadcast($"---------{EventTitle}---------", BroadCastLoc.Title, 0);
                        Broadcast("Event cancelled", BroadCastLoc.Score, 2);
                        foreach (Character c in PlayerList.Values)
                        {
                            if (c.Loc.OldX <= 0 || c.Loc.OldX >= 1400 || c.Loc.OldY <= 0 || c.Loc.OldY >= 3000)
                                c.Teleport(1002, 430, 378);
                            else if (c.Loc.OldMap == 1038 || c.Loc.OldMap == 2068 || c.Loc.OldMap >= 8001 && c.Loc.OldMap <= 8003 || World.EventsMaps.Contains(c.Loc.OldMap) || DMaps.EventMaps.ContainsKey(c.Loc.Map))
                                c.Teleport(1002, 430, 378);
                            else
                                c.Teleport(c.Loc.OldMap, c.Loc.OldX, c.Loc.OldY);
                            c.EventBase = null;
                        }
                        PlayerList.Clear();
                        Stage = EventStage.None;
                        World.Events.Remove(this);
                        return;
                    }
                    Broadcast("10 seconds until start", BroadCastLoc.Map);
                }
                else if (CountDown < 6)
                    Broadcast(CountDown.ToString() + " seconds until start", BroadCastLoc.Map);
                //var mins = CountDown / 60;
                Broadcast($"---------{EventTitle}---------", BroadCastLoc.Title, 0);
                TimeSpan T = TimeSpan.FromSeconds(CountDown);
                Broadcast($"Start in: {T.ToString(@"mm\:ss")}", BroadCastLoc.Score, 2);
                --CountDown;
            }
            else
            {
                Stage = EventStage.Countdown;

                if (MapEvent == 11000)
                {
                    while (DMaps.EventMaps.ContainsKey(MapEvent))
                        MapEvent = MapEvent + 1;
                    DMaps.CreateDynamicMap(BaseMap, MapEvent, true);
                }
                TeleportPlayersToMap();
            }
        }

        /// <summary>
        /// Do all the requirement checks to start the event in here
        /// </summary>
        /// <returns></returns>
        public virtual bool CanStart()
        {
            return PlayerList.Count >= minplayers;
        }

        /// <summary>
        /// Used to teleport players to event map
        /// </summary>
        public virtual void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.Team);
                c.Teleport(MapEvent, X, Y);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
        }

        /// <summary>
        /// Handles the logic for event protection countdown
        /// </summary>
        private void Countdown()
        {
            if (CountDown == 0 && Stage == EventStage.Countdown)
            {
                CountDown = 5;
                Stage = EventStage.Starting;
                Broadcast(EventTitle + " Event is about to start!", BroadCastLoc.Map);
            }
            else if (CountDown > 0)
            {
                foreach (Character C in PlayerList.Values)
                    World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, $"downnumber{CountDown}")).Get);
                CountDown--;
            }
            else
            {
                foreach (Character C in PlayerList.Values)
                    if (!C.Alive)
                        RevivePlayer(C, C.MaxHP, false);

                Removeprotection();
                Stage = EventStage.Fighting;
                EndTime = DateTime.Now.AddMinutes(Duration);
                Broadcast(EventTitle + " Event has started! May the best player win!", BroadCastLoc.World);
            }
        }

        /// <summary>
        /// Removes players' protection so they can get damage
        /// </summary>
        public void Removeprotection()
        {
            foreach (Character c in PlayerList.Values)
            {
                c.Protection = false;
                foreach (Buff B in c.Buffs.Keys)
                    c.RemoveBuff(B);
                c.LastMove = DateTime.Now;
            }
        }

        /// <summary>
        /// Handles all the logic during the events and determines conditions to find a winner
        /// </summary>
        public virtual void WaitForWinner()
        {
            if (DateTime.Now >= EndTime || PlayerList.Count == 1)
                Finish();
        }

        /// <summary>
        /// Handles all the character related checks during the event
        /// </summary>
        /// <param name="C"></param>
        public virtual void CharacterChecks(Character C)
        {
            if (C.Loc.Map != MapEvent)
                RemovePlayer(C);
        }

        /// <summary>
        /// Announces the event end. Gives each player a small protection and changes stage to over
        /// </summary>
        public void Finish()
        {
            foreach (Character C in PlayerList.Values)
                C.Protection = true;
            Stage = EventStage.Over;
        }

        /// <summary>
        /// Here we choose who we want to reward and such, may depend on teams or w/e... Should add support for teams
        /// </summary>
        public virtual void End()
        {
            if (PlayerList.Count == 1)
                foreach (var c in PlayerList.Values.ToList())
                    Reward(c);
            else
                Broadcast(Duration + " minutes have passed and no one won the " + EventTitle + " Event! Better luck next time!", BroadCastLoc.World);

            foreach (var c in PlayerList.Values.ToList())
                RemovePlayer(c);

            PlayerList.Clear();
            PlayerScores.Clear();
            return;
        }

        /// <summary>
        /// Used to choose which rewards we want to give
        /// </summary>
        public virtual void Reward(Character c)
        {
            c.Silvers += 1000000;
            if (!World.GoldSource.ContainsKey("HourlyEvents"))
                World.GoldSource.Add("HourlyEvents", 0);
            World.GoldSource["HourlyEvents"] += 1000000;
            if (c.Level < 130)
                c.AddExp(1);
            if (c.Inventory.Count > 38)
            {
                Broadcast(c.Name + " has won the hourly " + EventTitle + " Tournament!", BroadCastLoc.World);
                c.MyClient.LocalMessage(2000, "You didn't have enough space in your inventory and so you didn't receive a reward!");
                return;
            }
            switch (Program.Rnd.Next(0, 1))
            {
                case 0:
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
                            c.AddItem(720142);//EggPacket
                            c.AddItem(723017);//ExpPotion
                            c.AddItem(723712);//+1ItemPacket
                            c.AddItem(1088000);//+Dragonball
                        }   
                    }
                    #endregion
                    break;
                //case 1:
             
                //    break;
                //case 2:
            
                //    break;
                //case 3:
                 
                //    break;
            }

            Broadcast(c.Name + " has won the hourly " + EventTitle + " Tournament!", BroadCastLoc.World);
        }

        /// <summary>
        /// Display the score on the top right corner
        /// </summary>
        public virtual void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }

            byte Score = 2;
            foreach (var kvp in PlayerScores.OrderByDescending((s => s.Value)))
            {
                if (Score == 7)
                    break;
                if (Score == PlayerScores.Count + 2)
                    break;
                Broadcast($"Nº {Score - 1}: {PlayerList[kvp.Key].Name} - {kvp.Value}", BroadCastLoc.Score, Score);
                Score++;
            }
        }

        /// <summary>
        /// Determines if we're supposed to do something when a player gets killed
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public virtual void Kill(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Attacker.EntityID))
                PlayerScores[Attacker.EntityID]++;
        }

        /// <summary>
        /// Determines if we're supposed to do something when a player gets killed
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public virtual void Kill(Companion Attacker, Character Victim)
        {

        }

        /// <summary>
        /// Determines if we're supposed to do something when a NPC gets killed
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public virtual void Kill(Character Attacker, NPC Victim)
        {
            if (PlayerScores.ContainsKey(Attacker.EntityID))
                PlayerScores[Attacker.EntityID]++;
        }

        /// <summary>
        /// Determines wether if an event has unlimited stamina or not and allow us to track number of sent FB/SS
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="SU"></param>
        public virtual void Shot(Character Attacker, SkillsClass.SkillInfo SU)
        {
            Attacker.Stamina -= SU.StaminaCost;
        }

        /// <summary>
        /// Determines what we're supposed to do when a player gets hit
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public virtual void Hit(Character Attacker, Character Victim)
        {

        }

        /// <summary>
        /// Determines wether we're supposed to do something when a NPC gets hitted
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Victim"></param>
        public virtual void Hit(Character Attacker, NPC Victim)
        {

        }

        /// <summary>
        /// Overrides the damage dealt by a certain skill
        /// </summary>
        /// <param name="User"></param>
        /// <param name="C"></param>
        /// <param name="Info"></param>
        /// <returns></returns>
        public virtual uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            return 1;
        }

        /// <summary>
        /// Overrides the melee damage dealt
        /// </summary>
        /// <param name="User"></param>
        /// <param name="C"></param>
        /// <param name="AttackType"></param>
        /// <returns></returns>
        public virtual uint GetDamage(Character User, Character C, AttackType AttackType)
        {
            return 1;
        }

        /// <summary>
        /// Overrides the damage dealt by a certain skill to a NPC
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Victim"></param>
        /// <param name="Info"></param>
        /// <returns></returns>
        public virtual uint GetDamage(Character User, NPC Victim, SkillsClass.SkillInfo Info)
        {
            return 1;
        }

        /// <summary>
        /// Used to revive players in different situations
        /// </summary>
        /// <param name="C"></param>
        /// <param name="HP"></param>
        public void RevivePlayer(Character C, ushort HP, bool Protect = true)
        {
            if (!C.Alive)
            {
                C.Action = (byte)100;
                C.Ghost = false;
                C.Alive = true;
                C.StatEff.Remove(StatusEffectEn.Dead);
                C.StatEff.Remove(StatusEffectEn.BlueName);
                C.Body = C.Body;
                C.Hair = C.Hair;
                C.XPKO = (byte)0;
                if (Protect)
                    C.ProtectTime.AddSeconds(0);
            }
            C.Stamina = (byte)100;
            C.BlueName = false;
            C.CurHP = HP;
            if ((int)C.MaxMP > 1)
                C.CurMP = C.MaxMP;
        }

        /// <summary>
        /// Sends a packet to the client that updates the PK Button 
        /// </summary>
        public void ChangePKMode(Character C, PKMode Mode)
        {
            C.PKMode = Mode;
            if (C.MyClient != null)
                C.MyClient.AddSend(Packets.GeneralData(C.EntityID, (uint)Mode, 0, 0, 96));
        }
    }
}