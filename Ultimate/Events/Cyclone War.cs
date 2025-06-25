using Ultimate.Game;
using Ultimate.Structures;
using System;
using System.Linq;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class CycloneWar : Events
    {
        bool cyclone = false;
        public CycloneWar()
        {
            EventTitle = "Cyclone Pk Event";
            Duration = 10;
            BaseMap = 700;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            AllowedSkills = new System.Collections.Generic.List<ushort>{ (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 14;
        }

        public override void RemovePlayer(Character c, bool exp = true)
        {
            base.RemovePlayer(c, exp);
            foreach (Buff B in c.Buffs.Keys)
                c.RemoveBuff(B);
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Game.Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.PK);
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(Game.StatusEffectEn.Fly);
                c.StatEff.Remove(Game.StatusEffectEn.Cyclone);
                c.StatEff.Remove(Game.StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;

                c.Protection = true;
            }
            DisplayScores = DateTime.Now;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();
            if (!cyclone)
            {
                cyclone = true;
                foreach (Character c in PlayerList.Values)
                {
                    Buff B2 = new Buff();
                    B2.Eff = Features.SkillsClass.ExtraEffect.Cyclone;
                    B2.Lasts = 600;
                    B2.Value = 600;
                    B2.Started = DateTime.Now;
                    B2.StEff = Game.StatusEffectEn.Cyclone;
                    c.TimeBuff = B2.Lasts;
                    c.AddBuff(B2);
                }
            }
            if (DateTime.Now >= DisplayScores.AddMilliseconds(1000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                C.EventBase?.RemovePlayer(C);
        }

        public override void Shot(Character Attacker, SkillsClass.SkillInfo SU)
        {

        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Victim.EntityID))
            {
                if (PlayerScores[Victim.EntityID] < 2)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You can only be hitted " + (3 - PlayerScores[Victim.EntityID]) + " more times!");
                }
                else if (PlayerScores[Victim.EntityID] < 3)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You'll be kicked if anyone hits you again! Watch out!");
                }
                else if (PlayerScores[Victim.EntityID] >= 3)
                    RemovePlayer(Victim);
            }
        }

        public override void DisplayScore()
        {
            DisplayScores = DateTime.Now;
            foreach (var player in PlayerList.Values)
            {
                player.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", "ALLUSERS", $"---------{EventTitle}---------", 0x83c, 0));
            }
            Broadcast($"Players left: {PlayerList.Count}", BroadCastLoc.Score, 2);
        }
    }
}