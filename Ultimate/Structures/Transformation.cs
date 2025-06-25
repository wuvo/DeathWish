using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Structures
{
    public struct Transformation
    {
        public bool Transformed;
        public ushort HP;
        public ushort MinDmg;
        public ushort MaxDmg;
        public ushort Def;
        public ushort Dex;
        public byte Dist;
        public byte Dodge;//percent
        public byte MagicDef;//percent

    }
}
