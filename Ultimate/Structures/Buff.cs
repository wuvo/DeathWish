using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Features;
using Ultimate.Game;

namespace Ultimate.Structures
{
    public struct Buff
    {
        public StatusEffectEn StEff;
        public SkillsClass.ExtraEffect Eff;
        public float Value;
        public ushort Lasts;
        public uint Transform;
        public DateTime Started;
        public ushort skillID;
    }
}
