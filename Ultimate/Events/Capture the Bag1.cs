using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konquer.Features;
using Konquer.Game;

namespace Konquer.Events
{
    public class CaptureTheBag : Events
    {
        public static bool Red = false, Blue = false;
        int ScoreBlue, ScoreRed = 0;
    
        public CaptureTheBag()
        {
            EventTitle = "Capture The Bag";
           Duration = 10;
            MapEvent = 1080;
            NoDamage = true;
            FriendlyFire = false;
            MagicAllowed = false;
            MeleeAllowed = true;
           AllowedSkills = new List<ushort>() { 1000, 1001, 1002, 1005, 1055, 1075, 1085, 1090,
               1095, 1100, 1105, 1120, 1150, 1160, 1165, 1170, 1175, 1180, 1015, 1010, 1020, 1040,
                1050, 1125, 1270, 1280, 1320, 1360, 5001, 1045, 1046, 1047, 1190, 1195, 1115, 3050,
              3090, 1250, 1260, 1290, 1300, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040, 8001 };
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
                    X = (ushort)(95 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(35 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    c.RedTeam = false;
                    c.BlueTeam = true;
                }
                else
                {
                    TeamTwo.Add(c.EntityID, c);
                    c.MyClient.LocalMessage(2000, "Congratulations! You have joined the Red Team!");
                    X = (ushort)(175 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                    Y = (ushort)(205 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
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
            DropBlue();
            DropRed();
            Teams.Add(183425, TeamOne);
            Teams.Add(191605, TeamTwo);
            foreach (KeyValuePair<uint, Dictionary<uint, Character>> T in Teams)
                foreach (Character C in T.Value.Values)
                    C.MyClient.AddSend(Packets.OverwriteGarment(T.Key));
        }

        private void DropRed()
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
            droppedItem.Drop();
            Red = true;
        }

        private void DropBlue()
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
            droppedItem.Drop();
            Blue = true;
        }

        private bool InBase(Character C)
        {
            if (Teams[183425].ContainsKey(C.EntityID))
            {
                if (C.Loc.X >= 91 && C.Loc.X <= 96 && C.Loc.Y >= 17 && C.Loc.Y <= 22)
                    return true;
            }
            else if (Teams[191605].ContainsKey(C.EntityID))
                if (C.Loc.X >= 178 && C.Loc.X <= 183 && C.Loc.Y >= 213 && C.Loc.Y <= 218)
                    return true;

            return false;
        }

        void TeleAfterRev(Character C)
        {
            ushort X1 = 0;
            ushort Y1 = 0;
            if (Teams[183425].ContainsKey(C.EntityID))
            {
                X1 = (ushort)(95 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                Y1 = (ushort)(35 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
            }
            else
            {
                X1 = (ushort)(175 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
                Y1 = (ushort)(205 + Program.Rnd.Next(0, 3) - Program.Rnd.Next(0, 3));
            }
            C.Teleport(MapEvent, X1, Y1);
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (DateTime.UtcNow >= DisplayScores.AddMilliseconds(3000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);

            if (!C.Alive && DateTime.UtcNow > C.DeathHit.AddMilliseconds(10000))
            {
                RevivePlayer(C, C.MaxHP);
                TeleAfterRev(C);
            }
            else if (InBase(C))
            {
                if (C.StatEff.Contains(StatusEffectEn.Flashy))
                {
                    C.StatEff.Remove(StatusEffectEn.Flashy);
                    PlayerScores[C.EntityID] += 40;
                    if (Teams[183425].ContainsKey(C.EntityID))
                    {
                        ScoreBlue += 40;
                        DropRed();
                        Broadcast(C.Name + " from the BlueTeam has sucessfully retrieved the RedBag!", BroadCastLoc.Map);
                    }
                    else
                    {
                        ScoreRed += 40;
                        DropBlue();
                        Broadcast(C.Name + " from the RedTeam has sucessfully retrieved the BlueBag!", BroadCastLoc.Map);
                    }
                }
                TeleAfterRev(C);
            }
        }

        public override void RemovePlayer(Character C, bool exp = true)
        {
            if (C.StatEff.Contains(StatusEffectEn.Flashy))
            {
                C.StatEff.Remove(StatusEffectEn.Flashy);
                if (Teams[183425].ContainsKey(C.EntityID))
                    DropRed();
                else
                    DropBlue();
            }
            base.RemovePlayer(C, exp);
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            bool _hadbag = false;
            byte _score = 2;
            if (Victim.StatEff.Contains(StatusEffectEn.Flashy))
            {
                _hadbag = true;
                _score = 6;
                Victim.StatEff.Remove(StatusEffectEn.Flashy);
            }
            PlayerScores[Attacker.EntityID] += _score;

            if (Teams[183425].ContainsKey(Victim.EntityID))
            {
                if (_hadbag)
                {
                    Broadcast(Victim.Name + " from the BlueTeam was killed while holding the RedBag!", BroadCastLoc.Map);
                    DropRed();
                }
                ScoreRed += _score;
            }
            else
            {
                if (_hadbag)
                {
                    Broadcast(Victim.Name + " from the RedTeam was killed while holding the BlueBag!", BroadCastLoc.Map);
                    DropBlue();
                }
                ScoreBlue += _score;

            }
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.UtcNow;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
                player.MyClient.AddSend(Packets.ChatMessage(1, "SYSTEM", "ALLUSERS", $"My score: { PlayerScores[player.EntityID] }", 0x83d, 0));
            }
            if (ScoreBlue > ScoreRed)
            {
                Broadcast($"Blue Team: { ScoreBlue }", BroadCastLoc.Score, 2);
                Broadcast($"Red Team:  { ScoreRed }", BroadCastLoc.Score, 3);
            }
            else
            {
                Broadcast($"Red Team:  { ScoreRed }", BroadCastLoc.Score, 2);
                Broadcast($"Blue Team: { ScoreBlue }", BroadCastLoc.Score, 3);
            }
        }

        public override void End()
        {
            if (ScoreRed > ScoreBlue)
                Broadcast("Red Team has won the Capture the Bag Event ! Congratulations to the winning team !", BroadCastLoc.World);
            else if (ScoreBlue > ScoreRed)
                Broadcast("Blue Team has won the Capture the Bag Event ! Congratulations to the winning team !", BroadCastLoc.World);
            else
                Broadcast("Capture the Bag Event has come to an end and both teams scored the same amount of points!", BroadCastLoc.World);
            foreach (var kvp in PlayerScores.ToList())
            {
                PlayerList[kvp.Key].StatEff.Remove(StatusEffectEn.Flashy);
                ushort _reward = Convert.ToUInt16(kvp.Value);

                if (ScoreRed > ScoreBlue && Teams[191605].ContainsKey(kvp.Key))
                    _reward = Convert.ToUInt16(_reward * 2);
                else if (ScoreBlue > ScoreRed && Teams[183425].ContainsKey(kvp.Key))
                    _reward = Convert.ToUInt16(_reward * 2);

                if (_reward > 250)
                    _reward = 250;
                PlayerList[kvp.Key].CTBPoints += _reward;
                PlayerList[kvp.Key].MyClient.LocalMessage(2000, "Congratulations ! You have received " + _reward + " CTBPoints for your participation at Capture the Bag Event!");
                RemovePlayer(PlayerList[kvp.Key]);
            }
            PlayerList.Clear();
            PlayerScores.Clear();
            Teams.Clear();
            return;
        }
        
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            if (Info.ID == 8001)
                return Convert.ToUInt32(C.MaxHP / 20);
            else if (Info.ID == 1046 || Info.ID == 1045 || Info.ID == 1047)
                return Convert.ToUInt32(C.MaxHP * 0.4);
            else if (Info.ID == 1000 || Info.ID == 1165 || Info.ID == 1001)
                return Convert.ToUInt32(C.MaxHP / 12);
            else if (Info.ID == 1115)
                return Convert.ToUInt32(C.MaxHP * 0.15);
            else if (Info.ID == 1150 || Info.ID == 1160 || Info.ID == 1180 || Info.ID == 1002)
                return Convert.ToUInt32(C.MaxHP * 0.15);
            else if (Info.ID == 1120)
                return Convert.ToUInt32(C.MaxHP * 0.25);
            else if (Info.ID == 1320)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID == 5001 || Info.ID == 1125 || Info.ID == 1010)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID != 1175 && Info.ID != 1170 && Info.ID != 1005 && Info.ID != 1055 && Info.ID != 1190 && Info.ID != 1195)
                return Convert.ToUInt32(C.MaxHP * 0.15);
            else if (Info.ID == 1190)
                return Convert.ToUInt32(C.MaxHP * 0.6);
            else if (Info.ID == 1005 || Info.ID == 1055 || Info.ID == 1170 || Info.ID == 1175)
                return Convert.ToUInt32(C.MaxHP * 0.15);
            return Convert.ToUInt32(C.MaxHP * 0.15);
        }

        public override uint GetDamage(Character User, Character C, AttackType AttackType)
        {
            if (AttackType == AttackType.Melee)
                return Convert.ToUInt32(C.MaxHP * 0.4);
            else if (AttackType == AttackType.Ranged)
                return Convert.ToUInt32(C.MaxHP * 0.05);
            return 1;
        }
    }
}
