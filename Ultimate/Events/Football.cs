using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class Football : Events
    {
        public static bool Red = false, Blue = false;
        int ScoreBlue, ScoreRed, Freeze = 0;
        byte PTBC = 0;
        DateTime Timer;

        public Football()
        {
            EventTitle = "Football";
            Duration = 10;
            MapEvent = 1017;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            FriendlyFire = true;
            AllowedSkills = new List<ushort> { (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 13;
        }

        public override void TeleportPlayersToMap()
        {
            Teams = new Dictionary<uint, Dictionary<uint, Character>>();
            Dictionary<uint, Character> TeamOne = new Dictionary<uint, Character>();
            Dictionary<uint, Character> TeamTwo = new Dictionary<uint, Character>();
            foreach (Character c in PlayerList.Values)
            {
                if (TeamOne.Count <= TeamTwo.Count)
                {
                    TeamOne.Add(c.EntityID, c);
                    c.MyClient.LocalMessage(2000, "Congratulations! You have joined the Blue Team!");
                    X = (ushort)(88 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(110 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    c.RedTeam = false;
                    c.BlueTeam = true;
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.MyClient.LocalMessage(2000, "Congratulations! You have joined the Red Team!");
                    X = (ushort)(107 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(91 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    c.RedTeam = true;
                    c.BlueTeam = false;
                }
                ChangePKMode(c, PKMode.Team);
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
                World.Spawn(c, false);
            }
            DropBall1();
            Teams.Add(183425, TeamOne);
            Teams.Add(191605, TeamTwo);
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
        }

        private void DropBall()
        {
            DroppedItem droppedItem = new DroppedItem();
            droppedItem.DropTime = DateTime.Now;
            droppedItem.Loc = new Location();
            droppedItem.Loc.Map = 1017U;
            droppedItem.Info = new Item();
            droppedItem.Info.ID = 710103U;
            droppedItem.UID = (uint)Program.Rnd.Next(10000000);
            droppedItem.Info.UID = droppedItem.UID;
            droppedItem.Loc.X = (ushort)98;
            droppedItem.Loc.Y = (ushort)101;
            droppedItem.Drop();
        }
        private static readonly NPC _npcInfo = new NPC();
        public static MyRandom Rnd = new MyRandom();
        private static Location _location;
        private void DropBall1()
        {
            World.Ball = true;
            World.DebugAdd += "Ball = true! at: " + DateTime.Now.ToString() + "\r\n";
        }





        private bool InBase(Character C)
        {
            if (Teams[183425].ContainsKey(C.EntityID))
            {
                if (C.Loc.X >= 141 && C.Loc.X <= 149 && C.Loc.Y >= 56 && C.Loc.Y <= 64)
                    return true;
            }
            else if (Teams[191605].ContainsKey(C.EntityID))
                if (C.Loc.X >= 56 && C.Loc.X <= 64 && C.Loc.Y >= 141 && C.Loc.Y <= 149)
                    return true;

            return false;
        }
        public void ReduceTimer(Character C)
        {
            if (PTBC >= 0 && PTBC < 9)
            {
                World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, "downnumber" + (9 - PTBC))).Get);
                PTBC++;
            }

            else if (PTBC == 9)
            {
                World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, "attach_accept05")).Get);
                PTBC++;
            }
            else if (PTBC == 10)
            {
                PTBC = 0;
                C.StatEff.Remove(StatusEffectEn.IceBlock);
            }
        }
        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now > C.DeathHit.AddMilliseconds(10000))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
            if (C.StatEff.Contains(StatusEffectEn.IceBlock))
            {
                if (DateTime.Now >= Timer.AddMilliseconds(1000))
                {
                    ReduceTimer(C);
                    Timer = DateTime.Now;
                }
            }
            else if (InBase(C))
            {
                if (C.StatEff.Contains(StatusEffectEn.SparkleHalo))
                {
                    C.StatEff.Remove(StatusEffectEn.SparkleHalo);
                    PlayerScores[C.EntityID] += 1;
                    if (Teams[183425].ContainsKey(C.EntityID))
                    {
                        ScoreBlue += 1;
                        DropBall1();
                        Broadcast(C.Name + " from the BlueTeam has sucessfully retrieved the Ball!", BroadCastLoc.Map);
                        foreach (var kvp in PlayerScores.ToList())
                        {
                            PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.SparkleHalo);
                            PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.IceBlock);
                            ushort _reward = Convert.ToUInt16(kvp.Value);

                            foreach (Character c in PlayerList.Values)
                            {
                                TeleAfterRev(c);


                            }
                        }


                    }
                    else
                    {
                        ScoreRed += 1;
                        DropBall1();
                        Broadcast(C.Name + " from the RedTeam has sucessfully retrieved the Ball!", BroadCastLoc.Map);
                        foreach (var kvp in PlayerScores.ToList())
                        {
                            PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.SparkleHalo);
                            PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.IceBlock);
                            ushort _reward = Convert.ToUInt16(kvp.Value);
                            foreach (Character c in PlayerList.Values)

                            {
                                TeleAfterRev(c);



                            }
                        }

                    }
                }
                TeleAfterRev(C);
            }
        }

        void TeleAfterRev(Character C)
        {
            ushort X1 = 0;
            ushort Y1 = 0;
            if (Teams[183425].ContainsKey(C.EntityID))
            {
                X1 = (ushort)(90 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                Y1 = (ushort)(111 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
            }
            else
            {
                X1 = (ushort)(107 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                Y1 = (ushort)(91 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
            }
            C.Teleport(MapEvent, X1, Y1);
        }
        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                bool _hadbag = false;
                byte _score = 2;
                if (Victim.StatEff.Contains(StatusEffectEn.SparkleHalo))
                {
                    _hadbag = true;
                    _score = 6;
                    Victim.StatEff.Remove(StatusEffectEn.SparkleHalo);
                }


                if ((Teams[183425].ContainsKey(Attacker.EntityID) && Teams[183425].ContainsKey(Victim.EntityID)) || (Teams[191605].ContainsKey(Attacker.EntityID) && Teams[191605].ContainsKey(Victim.EntityID)))
                {
                    if (Victim.StatEff.Contains(StatusEffectEn.IceBlock))
                    {
                        Victim.StatEff.Remove(StatusEffectEn.IceBlock);

                    }
                }
                else
                {
                    if (!Victim.StatEff.Contains(StatusEffectEn.IceBlock))
                    {
                        Victim.StatEff.Add(StatusEffectEn.IceBlock);

                    }
                }

                if ((Teams[183425].ContainsKey(Attacker.EntityID) && Teams[183425].ContainsKey(Victim.EntityID)) || (Teams[191605].ContainsKey(Attacker.EntityID) && Teams[191605].ContainsKey(Victim.EntityID)))
                {
                    if (_hadbag)
                    {
                        Victim.StatEff.Remove(StatusEffectEn.SparkleHalo);
                        Attacker.StatEff.Add(StatusEffectEn.SparkleHalo);
                    }
                }
                else
                {
                    if (_hadbag)
                    {
                        Victim.StatEff.Remove(StatusEffectEn.SparkleHalo);
                        Attacker.StatEff.Add(StatusEffectEn.SparkleHalo);
                    }
                }
            }
        }


        public override void RemovePlayer(Character C, bool exp = true)
        {
            if (C.StatEff.Contains(StatusEffectEn.SparkleHalo))
            {
                C.StatEff.Remove(StatusEffectEn.SparkleHalo);
                if (Teams[183425].ContainsKey(C.EntityID))
                    DropBall1();
                else
                    DropBall1();
            }
            base.RemovePlayer(C, exp);
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (DateTime.Now >= DisplayScores.AddMilliseconds(3000))
                DisplayScore();
            if (ScoreBlue > 5 || ScoreRed > 5)
                Finish();
        }


        public bool OneAllFrozen()
        {
            try
            {
                if (Teams[183425].Count == 0)
                    return true;
                foreach (Character p in Teams[183425].Values)
                    if (!p.StatEff.Contains(StatusEffectEn.IceBlock))
                        return false;
                return true;
            }
            catch { return false; }
        }
        public bool TwoAllFrozen()
        {
            try
            {
                if (Teams[191605].Count == 0)
                    return true;
                foreach (Character p in Teams[191605].Values)
                    if (!p.StatEff.Contains(StatusEffectEn.IceBlock))
                        return false;
                return true;
            }
            catch { return false; }
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
        }
        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
                player.MyClient.AddSend(Packets.ChatMessage(1, "SYSTEM", "ALLUSERS", $"My score: {PlayerScores[player.EntityID]}", 0x83d, 0));
            }
            if (ScoreBlue > ScoreRed)
            {
                Broadcast($"Blue Team: {ScoreBlue}", BroadCastLoc.Score, 2);
                Broadcast($"Red Team:  {ScoreRed}", BroadCastLoc.Score, 3);
            }
            else
            {
                Broadcast($"Red Team:  {ScoreRed}", BroadCastLoc.Score, 2);
                Broadcast($"Blue Team: {ScoreBlue}", BroadCastLoc.Score, 3);
            }
        }



        public override void End()
        {
            if (ScoreRed > ScoreBlue)
            {
                Broadcast("Red Team has won the Football Event ! Congratulations to the winning team !", BroadCastLoc.World);
                World.SendMsgToAll("[EVENT]", EventTitle + "Red Team has won the Event ! Congratulations to the winning team !!", 2000, 0);
            }
            else if (ScoreBlue > ScoreRed)
            {
                Broadcast("Blue Team has won the Football Event ! Congratulations to the winning team !", BroadCastLoc.World);
                World.SendMsgToAll("[EVENT]", EventTitle + " Blue Team has won the Event ! Congratulations to the winning team !!", 2000, 0);
            }
            else
                Broadcast("Football Event has come to an end and both teams scored the same amount of points!", BroadCastLoc.World);
            foreach (var kvp in PlayerScores.ToList())
            {
                PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.SparkleHalo);
                PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.IceBlock);
                ushort _reward = Convert.ToUInt16(kvp.Value);

                if (ScoreRed > ScoreBlue && Teams[191605].ContainsKey(kvp.Key))
                    Reward(PlayerList[kvp.Key]);
                else if (ScoreBlue > ScoreRed && Teams[183425].ContainsKey(kvp.Key))
                    Reward(PlayerList[kvp.Key]);
                RemovePlayer(PlayerList[kvp.Key]);
            }
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }


    }
}