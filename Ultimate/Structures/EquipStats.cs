using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Structures
{
    public struct EquipStats
    {
        public uint minatk;
        public uint maxatk;
        public uint matk;
        public uint MDamage;

        public double WeaponExtraAttack;
        public double GemExtraExp;
        public double GemExtraProf;
        public double GemExtraMExp;
        public double GemExtraAttack;
        public double GemExtraMAttack;
        public double GemExtraDex;
        public double GemBless;
        public uint MaxHP;
        public uint MaxMP;
        public ushort MDef2;
        public ushort ExtraDex;
        public ushort eq_pot;
        public uint MagicDamageDecrease;
        public uint MeleeDamageDecrease;
        public uint MagicDamageIncrease;
        public uint MeleeDamageIncrease;
        public byte TotalBless;
        public byte AddRideSpeed;
        //  public ushort AddVigor;
        public ushort defense;

        public byte Dodge;
        public ushort MDef1;

        public static EquipStats operator +(EquipStats Eqp, EquipStats eqp)
        {
            Eqp.minatk += eqp.minatk;
            Eqp.maxatk += eqp.maxatk;
            Eqp.matk += eqp.matk;
            Eqp.MDamage += eqp.MDamage;
            Eqp.defense += eqp.defense;
            Eqp.GemExtraExp += eqp.GemExtraExp;
            Eqp.GemExtraMExp += eqp.GemExtraMExp;
            Eqp.GemExtraProf += eqp.GemExtraProf;
            Eqp.GemExtraAttack += eqp.GemExtraAttack;
            Eqp.GemExtraMAttack += eqp.GemExtraMAttack;
            Eqp.WeaponExtraAttack += eqp.WeaponExtraAttack;
            Eqp.MaxHP += eqp.MaxHP;
            Eqp.MaxMP += eqp.MaxMP;
            Eqp.Dodge += eqp.Dodge;
            Eqp.MDef1 += eqp.MDef1;
            Eqp.MDef2 += eqp.MDef2;
            Eqp.ExtraDex += eqp.ExtraDex;
            Eqp.GemExtraDex += eqp.GemExtraDex;
            Eqp.eq_pot += eqp.eq_pot;
            Eqp.MagicDamageDecrease += eqp.MagicDamageDecrease;
            Eqp.MeleeDamageDecrease += eqp.MeleeDamageDecrease;
            Eqp.MagicDamageIncrease += eqp.MagicDamageIncrease;
            Eqp.MeleeDamageIncrease += eqp.MeleeDamageIncrease;
            Eqp.GemBless += eqp.GemBless;
            Eqp.TotalBless += eqp.TotalBless;
            Eqp.AddRideSpeed += eqp.AddRideSpeed;
            //   Eqp.AddVigor += eqp.AddVigor;
            return Eqp;
        }
        public static EquipStats operator -(EquipStats Eqp, EquipStats eqp)
        {
            Eqp.minatk -= eqp.minatk;
            Eqp.maxatk -= eqp.maxatk;
            Eqp.matk -= eqp.matk;
            Eqp.MDamage -= eqp.MDamage;
            Eqp.defense -= eqp.defense;
            Eqp.GemExtraExp -= eqp.GemExtraExp;
            Eqp.GemExtraProf -= eqp.GemExtraProf;
            Eqp.GemExtraMExp -= eqp.GemExtraMExp;
            Eqp.GemExtraAttack -= eqp.GemExtraAttack;
            Eqp.GemExtraMAttack -= eqp.GemExtraMAttack;
            Eqp.WeaponExtraAttack -= eqp.WeaponExtraAttack;
            Eqp.MaxHP -= eqp.MaxHP;
            Eqp.MaxMP -= eqp.MaxMP;
            Eqp.Dodge -= eqp.Dodge;
            Eqp.MDef1 -= eqp.MDef1;
            Eqp.MDef2 -= eqp.MDef2;
            Eqp.ExtraDex -= eqp.ExtraDex;
            Eqp.GemExtraDex -= eqp.GemExtraDex;
            Eqp.eq_pot -= eqp.eq_pot;
            Eqp.MagicDamageDecrease -= eqp.MagicDamageDecrease;
            Eqp.MeleeDamageDecrease -= eqp.MeleeDamageDecrease;
            Eqp.MagicDamageIncrease -= eqp.MagicDamageIncrease;
            Eqp.MeleeDamageIncrease -= eqp.MeleeDamageIncrease;
            Eqp.GemBless -= eqp.GemBless;
            Eqp.TotalBless -= eqp.TotalBless;
            Eqp.AddRideSpeed -= eqp.AddRideSpeed;
            //  Eqp.AddVigor -= eqp.AddVigor;
            return Eqp;
        }
    }
}
