using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Linq;

namespace Ultimate.Events
{
    public class PTB : Events
    {
        byte PTBC = 0;
        bool Bomb = false;
        DateTime Timer;

        public PTB()
        {
            EventTitle = "Pass the Bomb";
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
            DialogID = 5;
        }

        public override void TeleportPlayersToMap()
        {
            foreach (Character c in PlayerList.Values)
            {
                ChangePKMode(c, PKMode.PK);
                X = (ushort)(51 + Program.Rnd.Next(1, 21) - Program.Rnd.Next(1, 21));
                Y = (ushort)(50 + Program.Rnd.Next(1, 20) - Program.Rnd.Next(1, 20));
                c.StatEff.Remove(StatusEffectEn.Fly);
                c.StatEff.Remove(StatusEffectEn.Cyclone);
                c.StatEff.Remove(StatusEffectEn.SuperMan);
                c.Teleport(MapEvent, X, Y);
                c.CurHP = c.MaxHP;
                c.Protection = true;
            }
            PTBC = 0;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                if (Attacker.StatEff.Contains(StatusEffectEn.Confused) && PTBC < 9)
                {
                    Broadcast(Attacker.Name + " has passed the bomb to " + Victim.Name + "! Be careful!", BroadCastLoc.Map);
                    PTBC = 0;
                    Attacker.StatEff.Remove(StatusEffectEn.Confused);
                    Victim.StatEff.Add(StatusEffectEn.Confused);
                }
                else if (Victim.StatEff.Contains(StatusEffectEn.Confused) && PTBC < 9)
                    ReduceTimer(Victim);
            }
        }

        public override void RemovePlayer(Character C, bool exp = true)
        {
            base.RemovePlayer(C, exp);
            if (C.StatEff.Contains(StatusEffectEn.Confused))
                Randomize();
            C.StatEff.Remove(StatusEffectEn.Confused);
        }

        public override void WaitForWinner()
        {
            if (!Bomb)
            {
                Timer = DateTime.Now;
                Randomize();
                Bomb = true;
            }
            base.WaitForWinner();
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);
            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                C.EventBase?.RemovePlayer(C);
            else if (C.StatEff.Contains(StatusEffectEn.Confused))
            {
                if (DateTime.Now >= Timer.AddMilliseconds(1000))
                {
                    ReduceTimer(C);
                    Timer = DateTime.Now;
                }
            }
        }

        public override void End()
        {
            foreach (Character C in PlayerList.Values)
                C.StatEff.Remove(StatusEffectEn.Confused);
            base.End();
        }

        public void Randomize()
        {
            if (PlayerList.Count == 1)
                return;
            int Number = Program.Rnd.Next(1, (PlayerList.Count + 1));
            int MyPlace = 1;
            foreach (Character C in PlayerList.Values)
            {
                if (MyPlace == Number)
                {
                    C.StatEff.Add(StatusEffectEn.Confused);
                    Broadcast(C.Name + " has the bomb! Be careful!", BroadCastLoc.Map);
                    break;
                }
                else
                    MyPlace++;
            }
        }
        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 1;
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
                C.StatEff.Remove(StatusEffectEn.Confused);
                RemovePlayer(C);
                Randomize();
            }
        }
    }
}