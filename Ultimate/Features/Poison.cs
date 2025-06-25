using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.Features
{
    public class PoisonType
    {
        public Game.Character Attacker;
        public int Times = 0;
        public DateTime LastAttack;
        public bool Poisoned = false;
    }
    public class Poison
    {
        public static void PoisonCharacter(uint CC, uint Attacker)
        {
            Game.Character C = Game.World.H_Chars[CC];
            if (C.Alive)
            {
                C.PoisonedInfo.LastAttack = DateTime.Now;
                C.PoisonedInfo.Times = 23;
                C.PoisonedInfo.Attacker = Game.World.H_Chars[Attacker];
                C.PoisonedInfo.Poisoned = true;
                C.StatEff.Add(Game.StatusEffectEn.Poisoned);
            }
        }
        public static void AttackPoisonedCharacter(uint CC)
        {
            Game.Character C = Game.World.H_Chars[CC];
            if (C.PoisonedInfo.Times > 0 && C.Alive)
            {
                if (C.CurHP > 130)
                {
                    C.CurHP -= (ushort)130;
                    Game.World.Action(C, Packets.AttackPacket(0, CC, C.Loc.X, C.Loc.Y, 130, (byte)Game.AttackType.Melee).Get);
                }
                else if (C.CurHP > 1)
                {
                    C.CurHP = 1;
                    Game.World.Action(C, Packets.AttackPacket(0, CC, C.Loc.X, C.Loc.Y, (uint)(C.CurHP - 1), (byte)Game.AttackType.Melee).Get);
                }
                C.Action = 100;
                C.PoisonedInfo.Times--;
                C.PoisonedInfo.LastAttack = DateTime.Now;
            }
            else { C.StatEff.Remove(Game.StatusEffectEn.Poisoned); C.PoisonedInfo.Poisoned = false; }
        }
    }
}
