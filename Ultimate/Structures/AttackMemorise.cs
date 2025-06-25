using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Structures
{
    public struct AttackMemorise
    {
        public bool Attacking;
        public DateTime LastAttack;
        public DateTime LastExpInTG;
        public bool FireCircle;
        public uint Target;
        public byte AtkType;
        public ushort Skill;
        public ushort SX;
        public ushort SY;
    }
}
