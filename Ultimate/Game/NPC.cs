using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.Game
{
    public enum NPCActionType
    {
        Shop = 1,
        Dialog = 2,//3, 4, 10, 15, 27
        FaceChange = 5,
        TCUpgrade = 6,
        GemSocket = 7,
        ShopCarpet = 14,
        AskVending = 16,
        Gamble = 19,
        Stake = 21,
        Scarecrow = 22
    }
    public class NPC
    {
        public Location Loc;
        public ushort Type;
        public uint EntityID;
        public byte Direction;
        public byte Avatar;
        public byte Flags;
        public bool PlayerEvent = false;
        public string Name = "";
        public bool Database = false;

        public byte Level;

        public uint CurHP = 0;
        public uint MaxHP = 0;
        
        public NPC()
        {
        }
        public NPC(string Line)
        {
            string[] Info = Line.Split(' ');
            EntityID = uint.Parse(Info[0]);
            Type = ushort.Parse(Info[1]);
            Flags = byte.Parse(Info[2]);
            Avatar = byte.Parse(Info[3]);
            Loc = new Location();
            Loc.Map = ushort.Parse(Info[4]);
            Loc.X = ushort.Parse(Info[5]);
            Loc.Y = ushort.Parse(Info[6]);

            if (Flags == 26)
            {

            }

            if (Flags == 21)
                Level = (byte)((Type - 427) / 6 + 20);
            if (Flags == 22)
                Level = (byte)((Type - 437) / 6 + 20);
            if (Type == 1507 || Type == 1527)
                Level = 125;
            else if (Type == 1517 || Type == 1587)
                Level = 130;

            if (Flags == 21 || Flags == 22)
            {
                CurHP = 10000;
                MaxHP = 10000;
            }
        }
        public uint TakeAttack(Character Attacker, uint Damage, AttackType AT, bool IsSkill)
        {
            try
            {
                if (Level > Attacker.Level)
                {
                    Attacker.MyClient.LocalMessage(2005, "This dummy is too high level for you.");
                    return 0;
                }
                double e = 0.1;
               /* if (Level + 4 > Attacker.Level)
                    e = 0.1;
                if (Level + 4 <= Attacker.Level)
                    e = 0.01;*/

                if (AT == AttackType.Melee || AT == AttackType.Ranged || AT == AttackType.FatalStrike)
                    Damage += Attacker.EqStats.MeleeDamageIncrease;
                else if (AT == AttackType.Magic)
                    Damage += Attacker.EqStats.MagicDamageIncrease;
                if (Attacker.Intensify.Active)
                {
                    if (Attacker.Intensify.Activated.AddMilliseconds(5300) < DateTime.Now)
                    {
                        if (Attacker.Intensify.X == Attacker.Loc.X && Attacker.Intensify.Y == Attacker.Loc.Y)
                        {
                            Attacker.Intensify.Active = false;
                            if (Attacker.Intensify.Level == 0)
                                Damage *= 2;
                            else if (Attacker.Intensify.Level == 1)
                                Damage = (uint)(Damage * 2.5);
                            else if (Attacker.Intensify.Level == 2)
                                Damage *= 3;
                            else Damage = (uint)(Damage * 3.5);
                        }
                        else
                            Attacker.Intensify.Active = false;
                    }
                }
                uint Exp = 0;
                if (Damage < CurHP)
                {
                    CurHP -= Damage;
                    if (AT != AttackType.Magic && !IsSkill)
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);

                    if (Attacker?.EventBase != null)
                    {
                        if (Attacker.EventBase.Stage == Events.EventStage.Fighting)
                            Attacker?.EventBase?.Hit(Attacker, this);
                    }

                    Exp = (uint)(Damage * e);
                    if (Exp < 2)
                        Exp = 2;
                    if (!IsSkill)
                    {
                        
                        if (AT == AttackType.Ranged || AT == AttackType.Melee || AT == AttackType.FatalStrike)
                        {
                            uint ProfExp = (uint)(Exp * 0.8);
                            if (Attacker.Equips.RightHand.ID == 0 && Attacker.Equips.LeftHand.ID == 0)
                                Attacker.AddProfExp((ushort)000, ProfExp);//damage /8
                            if (Attacker.Equips.RightHand.ID != 0)
                                Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), ProfExp);//damage /8
                            if (Attacker.Equips.LeftHand.ID != 0)
                                Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), ProfExp/2);//damage/15
                        }
                    }
                    Attacker.IncreaseExp(Exp, false, false);
                }
                else
                 {
                    uint Benefit = CurHP;
                    CurHP = MaxHP;
                    //World.Spawn(this);
                    if (AT != AttackType.Magic && !IsSkill)
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                    //else
                    //    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)21).Get);

                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);

                    if (Attacker?.EventBase != null)
                    {
                        if (Attacker.EventBase.Stage == Events.EventStage.Fighting)
                            Attacker?.EventBase?.Kill(Attacker, this);
                    }

                    //World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, 0, (byte)AttackType.Kill).Get);
                    Exp = (uint)(Benefit * e);
                    if (Exp < 2)
                        Exp = 2;
                    if (!IsSkill)
                    {
                        if (AT == AttackType.Ranged || AT == AttackType.Melee || AT == AttackType.FatalStrike)
                        {
                            uint ProfExp = Exp;
                            if (Attacker.Equips.RightHand.ID != 0)
                                Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.RightHand.ID, 0, 3), ProfExp);// Benefit/8
                            if (Attacker.Equips.LeftHand.ID != 0)
                                Attacker.AddProfExp((ushort)Game.ItemIDManipulation.Part(Attacker.Equips.LeftHand.ID, 0, 3), ProfExp / 2);//benefit/15
                        }
                    }
                    Attacker.IncreaseExp(Exp, false, true);
                    World.Action(this, Packets.Status(EntityID, Status.HP, CurHP).Get);
                }
                return Exp;
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; return 0; }
        }
    }
}
