using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;

namespace Ultimate.Events
{
    public class Vampire_War : Events
    {
        public DateTime _vampire;
        public Vampire_War()
        {
            EventTitle = "Vampire War";
            Duration = 10;
            BaseMap = 700;
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
            DialogID = 12;
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
                c.CurHP = 500;
                c.Protection = true;
            }
            _vampire = DateTime.Now;
        }

        public override void Hit(Character Attacker, Character Victim)
        {
            if (Stage == EventStage.Fighting)
            {
                Game.World.Action(Victim, (Packets.StringPacket(Victim.EntityID, StringType.Effect, "levin")).Get);
                Game.World.Action(Attacker, (Packets.StringPacket(Attacker.EntityID, StringType.Effect, "heal2")).Get);
                if ((Attacker.CurHP + 50) > 500)
                    Attacker.CurHP = 500;
                else
                    Attacker.CurHP += 50;
            }
        }

        public override uint GetDamage(Character User, Character C, SkillsClass.SkillInfo Info)
        {
            User.Stamina += Info.StaminaCost;
            return 50;
        }

        public override void WaitForWinner()
        {
            base.WaitForWinner();

            if (DateTime.Now >= _vampire)
            {
                foreach (Character C in PlayerList.Values.ToList())
                {
                    if (C.CurHP > 25)
                        C.CurHP -= 25;
                    else
                        C.TakeAttack(null, 25);

                    Game.World.Action(C, (Packets.StringPacket(C.EntityID, StringType.Effect, "poison")).Get);
                }
                _vampire = DateTime.Now.AddMilliseconds(5000);
            }
        }

        public override void CharacterChecks(Character C)
        {
            base.CharacterChecks(C);

            if (!C.Alive && DateTime.Now > C.DeathHit.AddSeconds(2))
                C.EventBase?.RemovePlayer(C);
            else if (C.CurHP > 500)
                C.CurHP = 500;
        }

        public override void Kill(Character Attacker, Character Victim)
        {
            base.Kill(Attacker, Victim);
            RemovePlayer(Victim);
        }
    }
}
