using Ultimate.Game;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ultimate.Events
{
    public class SkillPK : Events
    {
        public SkillPK()
        {
            EventTitle = "Five'n'Out";
            Duration = 10;
            BaseMap = 2090;
            NoDamage = true;
            MagicAllowed = false;
            MeleeAllowed = false;
            AllowedSkills = new System.Collections.Generic.List<ushort>{ (ushort)1045, (ushort)1046, (ushort)1047,
            (ushort)2001,(ushort)2002,(ushort)2003,(ushort)2004,(ushort)2005,(ushort)2006,(ushort)2007,(ushort)2008,(ushort)2009,(ushort)2010,
            (ushort)2011,(ushort)2012,(ushort)2013,(ushort)2014,(ushort)2015,(ushort)2016,(ushort)2017,(ushort)2018,(ushort)2019,(ushort)2020,

            (ushort)2101,(ushort)2102,(ushort)2103,(ushort)2104,(ushort)2105,(ushort)2106,(ushort)2107,(ushort)2108,(ushort)2109,(ushort)2110,
            (ushort)2111,(ushort)2112,(ushort)2113,(ushort)2114,(ushort)2115,(ushort)2116,(ushort)2117,(ushort)2118,(ushort)2119,(ushort)2120
            };
            DialogID = 2;
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

            if (DateTime.Now >= DisplayScores.AddMilliseconds(1000))
                DisplayScore();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                C.EventBase?.RemovePlayer(C);
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (PlayerScores.ContainsKey(Victim.EntityID))
            {
                if (PlayerScores[Victim.EntityID] < 3)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You can only be hitted " + (5 - PlayerScores[Victim.EntityID]) + " more times!");
                }
                else if (PlayerScores[Victim.EntityID] < 5)
                {
                    PlayerScores[Victim.EntityID]++;
                    Victim.MyClient.LocalMessage(2011, "You'll be kicked if anyone hits you again! Watch out!");
                }
                else if (PlayerScores[Victim.EntityID] >= 5)
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