using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.Structures;

namespace Ultimate.Game
{    
    public class Companion
    {
        public uint MinAttack;
        public uint MaxAttack;
        public byte Level;
        public uint SkillUses;//0 if just melee
        public Location Loc;
        public Character Owner;
        public uint CurHP;
        public ushort MaxHP;
        public byte Dodge;
        public ushort Def;
        public uint Mesh;
        public uint EntityID;
        public string Name;
        public byte Direction;
        DateTime LastMovement = DateTime.Now;
        DateTime LastAttack = DateTime.Now;
        public Features.PoisonType PoisonedInfo = null;
        public Companion(Character Owner, uint Type)
        {
            this.Owner = Owner;
            if (Database.CompanionInfos.ContainsKey(Type))
            {
                CompanionInfo Cmp = (CompanionInfo)Database.CompanionInfos[Type];
                MinAttack = Cmp.MinAttack;
                MaxAttack = Cmp.MaxAttack;
                Level = Cmp.Level;
                SkillUses = Cmp.SkillUses;
                CurHP = Cmp.HP;
                MaxHP = Cmp.HP;
                Mesh = Cmp.Mesh;
                Name = Cmp.Name;
                Dodge = Cmp.Dodge;
                Def = Cmp.Def;
                EntityID = (uint)Program.Rnd.Next(400000, 500000);
                while (World.H_Companions.ContainsKey(EntityID))
                    EntityID = (uint)Program.Rnd.Next(400000, 500000);
                Direction = 0;

                Loc = Owner.Loc;

                World.H_Companions.TryAdd(EntityID, this);
                World.Spawn(this, false);
            }

        }
        public void Dissappear()
        {
            if (World.H_Companions.ContainsKey(EntityID))
            {
                World.H_Companions.Remove(EntityID);
                World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 135).Get);
                Owner.MyCompanion = null;
            }
        }
        public void GetReflect(uint Damage, AttackType AT)
        {
            /* if (Damage > 4000)
                 Damage = 4000;*/
            if (Damage < CurHP)
            {
                CurHP -= (ushort)Damage;
                if (AT != AttackType.Magic)
                    World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                
                CurHP = 0;
                PoisonedInfo = null;
                Dissappear();
                /*  if (Loc.Map == 700)
                  {
                      Features.Turnny.RemovePlayer(this);
                  }*/

                if (AT != AttackType.Magic)
                    World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                World.Action(this, Packets.AttackPacket(EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);
            }
        }
        public void Step()
        {
            DateTime TimeNow = DateTime.Now;
            if (TimeNow > LastMovement.AddMilliseconds(250))
            {
                LastMovement = DateTime.Now;
                if (MyMath.PointDistance(Loc.X, Loc.Y, Owner.Loc.X, Owner.Loc.Y) <= 28)
                {
                    if (Owner.AtkMem.Target != 0 && Owner.AtkMem.Target != Owner.EntityID && MyMath.PointDistance(Loc.X, Loc.Y, Owner.Loc.X, Owner.Loc.Y) <= 10 && Loc.Map != 1039 && Loc.Map != 1004)
                    {
                        if (TimeNow > LastAttack.AddMilliseconds(1000))
                        {
                            if (SkillUses != 0)
                            {
                                LastAttack = DateTime.Now;
                                uint Damage = (uint)Program.Rnd.Next((int)MinAttack, (int)MaxAttack);
                                if (World.H_Mobs.ContainsKey(Loc.Map) && World.H_Mobs[Loc.Map].ContainsKey(Owner.AtkMem.Target))
                                {
                                    Mob M = World.H_Mobs[Loc.Map][Owner.AtkMem.Target];
                                    if (M.Alive && M.Loc.Map == Loc.Map && MyMath.PointDistance(Owner.Loc.X, Owner.Loc.Y, M.Loc.X, M.Loc.Y) <= 15)
                                        M.TakeAttack(this, Damage, AttackType.Magic);
                                }
                                else if (World.H_Chars.ContainsKey(Owner.AtkMem.Target))
                                {
                                    Character C = World.H_Chars[Owner.AtkMem.Target];
                                    if (C.Alive && C.Loc.Map == Loc.Map && MyMath.PointDistance(Owner.Loc.X, Owner.Loc.Y, C.Loc.X, C.Loc.Y) <= 15 && C.PKAble(Owner.PKMode, Owner))
                                        C.TakeAttack(this, Damage, AttackType.Magic);
                                }
                                else if (World.H_Companions.ContainsKey(Owner.AtkMem.Target))
                                {
                                    Companion C = World.H_Companions[Owner.AtkMem.Target];
                                    if (C.CurHP > 0 && C.Loc.Map == Loc.Map && MyMath.PointDistance(Owner.Loc.X, Owner.Loc.Y, C.Loc.X, C.Loc.Y) <= 15)
                                        C.TakeAttack(this, Damage, AttackType.Magic);
                                }
                                else if (World.H_SOBs.ContainsKey(Owner.AtkMem.Target))
                                {
                                    SOB SOB = World.H_SOBs[Owner.AtkMem.Target];
                                    if (SOB.Type == Looks.Statue)
                                        Damage = 10;
                                    if (SOB.CurHP > 0 && SOB.Loc.Map == Loc.Map && MyMath.PointDistance(Owner.Loc.X, Owner.Loc.Y, SOB.Loc.X, SOB.Loc.Y) <= 15)
                                        SOB.TakeAttack(this, Damage, (byte)AttackType.Magic);
                                }
                            }
                            else
                            {
                                LastAttack = DateTime.Now;
                                uint Damage = (uint)Program.Rnd.Next((int)MinAttack, (int)MaxAttack);
                                if (World.H_Mobs.ContainsKey(Loc.Map) && World.H_Mobs[Loc.Map].ContainsKey(Owner.AtkMem.Target))
                                {
                                    Mob M = World.H_Mobs[Loc.Map][Owner.AtkMem.Target];
                                    if (M.Alive && M.Loc.Map == Loc.Map && MyMath.PointDistance(Loc.X, Loc.Y, M.Loc.X, M.Loc.Y) <= 3)
                                        M.TakeAttack(this, Damage, AttackType.Melee);
                                }
                                else if (World.H_Chars.ContainsKey(Owner.AtkMem.Target))
                                {
                                    Character C = World.H_Chars[Owner.AtkMem.Target];
                                    if (C.Alive && C.Loc.Map == Loc.Map && MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= 3 && C.PKAble(Owner.PKMode, Owner))
                                        C.TakeAttack(this, Damage, AttackType.Melee);
                                }
                                else if (World.H_Companions.ContainsKey(Owner.AtkMem.Target))
                                {
                                    Companion C = World.H_Companions[Owner.AtkMem.Target];
                                    if (C.CurHP > 0 && C.Loc.Map == Loc.Map && MyMath.PointDistance(Loc.X, Loc.Y, C.Loc.X, C.Loc.Y) <= 3)
                                        C.TakeAttack(this, Damage, AttackType.Melee);
                                }
                                else if (World.H_SOBs.ContainsKey(Owner.AtkMem.Target))
                                {
                                    SOB SOB = World.H_SOBs[Owner.AtkMem.Target];
                                    if (SOB.Type == Looks.Statue)
                                        Damage = 10;
                                    if (SOB.CurHP > 0 && SOB.Loc.Map == Loc.Map && MyMath.PointDistance(Owner.Loc.X, Owner.Loc.Y, SOB.Loc.X, SOB.Loc.Y) <= 3)
                                        SOB.TakeAttack(this, Damage, (byte)AttackType.Melee);
                                }
                            }
                        }
                    }
                    else if (MyMath.PointDistance(Loc.X, Loc.Y, Owner.Loc.X, Owner.Loc.Y) >= 3 && MyMath.PointDistance(Loc.X, Loc.Y, Owner.Loc.X, Owner.Loc.Y) < 10)
                    {
                        GetDirection(Owner.Loc);
                        byte eDir = Direction;
                        bool Success = true;

                        var a = 0;
                        while (!FreeToGo())
                        {
                            Direction = (byte)((Direction + 1) % 8);
                            if (Direction == eDir)
                            {
                                Success = false;
                                break;
                            }
                            Success = true;
                            if (a > 5000)
                            {
                                Dissappear();
                                break;
                            }
                            a++;
                        }
                        if (!Success)
                            JumpToOwner();
                        else
                        {
                            Loc.Walk(Direction);
                            World.Action(this, Packets.Movement(EntityID, Direction).Get);
                            World.Spawn(this, true);
                        }
                    }
                    else if (!(MyMath.InBox(Loc.X, Loc.Y, Owner.Loc.X, Owner.Loc.Y, 10)))
                        JumpToOwner();
                }
                else
                    Teleport();
            }
        }
        void JumpToOwner()
        {
            /*   short x = (short)(Program.Rnd.Next(6) - Program.Rnd.Next(6));
               short y = (short)(Owner.Loc.Y + Program.Rnd.Next(6) - Program.Rnd.Next(6));
               ushort NewX = (ushort)(Owner.Loc.X + x);
               ushort NewY = (ushort)(Owner.Loc.Y + y);*/
            ushort NewLocX = (ushort)(Owner.Loc.X + Program.Rnd.Next(3) - Program.Rnd.Next(3));//offset 12
            ushort NewLocY = (ushort)(Owner.Loc.Y + Program.Rnd.Next(3) - Program.Rnd.Next(3));//offset 14
            World.Action(this, Packets.GeneralData(EntityID, NewLocX, NewLocY, Loc.X, Loc.Y, Direction, 137).Get);
            Loc.X = NewLocX;
            Loc.Y = NewLocY;
            //World.Action(this, Packets.GeneralData(EntityID, NewX, NewY, 0, 0, 137).Get);
            World.Spawn(this, true);
        }
        void Teleport()
        {
            /*   short x = (short)(Program.Rnd.Next(6) - Program.Rnd.Next(6));
               short y = (short)(Owner.Loc.Y + Program.Rnd.Next(6) - Program.Rnd.Next(6));
               ushort NewX = (ushort)(Owner.Loc.X + x);
               ushort NewY = (ushort)(Owner.Loc.Y + y);*/
            //ushort OldLocX = Loc.X;//offset 16
            //ushort OldLocY = Loc.Y;//offset 18
            World.Action(this, Packets.GeneralData(EntityID, 0, 0, 0, 0, 135).Get);
            Loc.X = (ushort)(Owner.Loc.X + Program.Rnd.Next(3) - Program.Rnd.Next(3));//offset 12
            Loc.Y = (ushort)(Owner.Loc.Y + Program.Rnd.Next(3) - Program.Rnd.Next(3));//offset 14
            //World.Action(this, Packets.GeneralData(EntityID, Loc.X, Loc.Y, OldLocX, OldLocY, Direction, 137).Get);
            //World.Action(this, Packets.GeneralData(EntityID, NewX, NewY, 0, 0, 137).Get);
            World.Spawn(this, true);
        }
        public uint TakeAttack(Character Attacker, ref uint Damage, AttackType AT, bool IsSkill)
        {
            if (AT != AttackType.Magic && Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Superman).Eff == Features.SkillsClass.ExtraEffect.Superman)
                Damage = (uint)(Damage * 10);

            if (EntityID != Attacker.EntityID)
            {
                if (!Owner.BlueName && Owner.PKPoints < 100 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                {
                    Attacker.BlueName = true;
                    if (Attacker.BlueNameLasts < 15)
                        Attacker.BlueNameLasts = 15;
                }
            }
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
            if (AT != AttackType.Magic && !IsSkill)
            {
                short _Agi = (short)((Attacker.Agi + Attacker.EqStats.ExtraDex) * Attacker.EqStats.GemExtraDex);

                Buff Accuracy = Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Accuracy);
                if (Accuracy.Eff == Features.SkillsClass.ExtraEffect.Accuracy)
                    _Agi = (short)(_Agi * Accuracy.Value);
                Buff SM = Attacker.BuffOf(Features.SkillsClass.ExtraEffect.Superman);
                if (SM.StEff == StatusEffectEn.SuperMan)
                    _Agi *= 2;
                double MissValue = Program.Rnd.Next(15 + _Agi, _Agi + 70 + 15);//70 == dodge
                if (MissValue <= Dodge )
                    Damage = 0;
                
            }
            if (!IsSkill && Damage != 0)
            {
                if (AT == AttackType.Melee)
                {
                    if (Def >= Damage)//defense == 1600
                        Damage = 1;
                    else
                        Damage -= Def;

                    Damage += Attacker.EqStats.MeleeDamageIncrease;
                }
                else if (AT == AttackType.Ranged)
                {
                    Damage = (uint)((double)Damage * ((double)(180 - Dodge) / 100));//dodge= 70
                    Damage += Attacker.EqStats.MeleeDamageIncrease;
                }
                else
                {
                    if (Def/2 >= Damage)//1000
                        Damage = 1;
                    else
                        Damage -= (uint)Def/2;

                    Damage += Attacker.EqStats.MagicDamageIncrease;
                }
            }
            if (Damage < CurHP)
            {
                
                CurHP -= Damage;
                
                if (!IsSkill)
                {
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                }
            }
            else
            {
                Attacker.XPKO++;
                if (Attacker.Superman || Attacker.Cyclone)
                    Attacker.TotalKO++;
               
                CurHP = 0;
                PoisonedInfo = null;
                Dissappear();
                if (!IsSkill)
                {
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                }
                if (Attacker.Superman || Attacker.Cyclone)
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, (uint)(65536 * Attacker.TotalKO), (byte)AttackType.Kill).Get);
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, (uint)(1), (byte)AttackType.Kill).Get);

                World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                Attacker.AtkMem.Attacking = false;
                Attacker.AtkMem.Target = 0;

                
            }
            return 0;
        }
        public void TakeAttack(Mob Attacker, uint Damage, AttackType AT)
        {
            if (Damage != 0)
            {
                /* if (BuffOf(SkillsClass.ExtraEffect.Scapegoat).Eff == SkillsClass.ExtraEffect.Scapegoat && MyMath.ChanceSuccess(30))
                 {
                     Buff B = BuffOf(SkillsClass.ExtraEffect.Scapegoat);
                     BDelete.Add(B);
                     uint Dmg = (uint)(PrepareAttack(2, false) * B.Value);
                     Attacker.TakeAttack(this, ref Dmg, AttackType.Scapegoat, false);
                     return;//Will not be damaged
                 }*/
                if (AT == AttackType.Melee)
                {
                    if (Def * 0.8 >= Damage)
                        Damage = 1;
                    else
                        Damage -= (uint)(Def*0.8);
                }
                else if (AT == AttackType.Ranged)
                {
                        Damage = (uint)((double)Damage * (((double)(106 - Dodge) / 100)));
                    Damage *= 2 / 3;
                }
                else
                {
                    if (Def * 0.1 >= Damage)
                        Damage = 1;
                    else
                        Damage -= (uint)(Def*0.1);
                }
            }
            if (Damage < CurHP)
            {
                CurHP = (ushort)(CurHP - Damage);
                if (AT == AttackType.Magic)
                    World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
            }
            else
            {
                /*  Attacking = false;
                  AtkMem.Target = 0;
                  DeathHit = DateTime.Now;
                  if (!World.FreePKMaps.Contains(Loc.Map) && Loc.Map < 8000)
                      InitAngry(false);
                  Alive = false;
                  CurHP = 0;

                  if (AT == AttackType.Magic)
                      World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                  else
                      World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                  World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                  /* List<Buff> BDelete = new List<Buff>();
                   foreach (Buff B in Buffs)
                       BDelete.Add(B);
                   foreach (Buff B in BDelete)
                       RemoveBuff(B); */
                /*    foreach (Buff B in Buffs)
                        BDelete.Add(B);
                    BlueName = false;
                    PoisonedInfo.Times = 0;
                    StatEff.Add(StatusEffectEn.Dead);
                    if (MyCompanion != null)
                        MyCompanion.Dissappear();*/
                //

                CurHP = 0;
                PoisonedInfo = null;
                Dissappear();
                if (AT == AttackType.Magic)
                    World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, Attacker.MagicSkill, Attacker.MagicLvl, Loc.X, Loc.Y).Get);
                else
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);
                World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

            }
        }
        public void TakeAttack(Companion Attacker, uint Damage, AttackType AT)
        {
            try
            {
                
                if (AT == AttackType.Melee)
                {
                    if (Def >= Damage)
                        Damage = 1;
                    else
                        Damage -= Def;
                }
                else if (AT == AttackType.Ranged)
                    Damage = (uint)((double)Damage * ((double)Dodge / 100));
                else if (AT == AttackType.Magic)
                {
                    if (Def/2 >= Damage)
                        Damage = 1;
                    else
                        Damage -= (uint)Def/2;
                }
                if (EntityID != Attacker.EntityID)
                {
                    if (!Owner.BlueName && Owner.PKPoints < 100 && !World.FreePKMaps.Contains(Loc.Map) && !World.EventsMaps.Contains(Loc.Map) && Loc.Map < 8000)
                    {
                        Attacker.Owner.BlueName = true;
                        if (Attacker.Owner.BlueNameLasts < 15)
                            Attacker.Owner.BlueNameLasts = 15;
                    }
                }
                else return;
                if (Damage < CurHP)
                {
                    CurHP -= Damage;
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AT).Get);

               
                }
                else
                {
                    PoisonedInfo = null;
                    CurHP = 0;
                    Dissappear();
                    if (AT == AttackType.Magic)
                        World.Action(this, Packets.SkillUse(Attacker.EntityID, EntityID, Damage, (ushort)Attacker.SkillUses, 0, Loc.X, Loc.Y).Get);
                    else
                        World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                    World.Action(this, Packets.AttackPacket(Attacker.EntityID, EntityID, Loc.X, Loc.Y, Damage, (byte)AttackType.Kill).Get);
                    World.Action(this, Packets.Status(EntityID, Status.Effect, 2080).Get);

                }
            }
            catch (Exception Exc) { Game.World.ExcAdd += Exc.ToString() + "\r\n"; }
        }
        void GetDirection(Location Target)
        {
            byte ToDir = (byte)(7 - (Math.Floor(MyMath.PointDirecton(Loc.X, Loc.Y, Target.X, Target.Y) / 45 % 8)) - 1 % 8);
            Direction = (byte)((int)ToDir % 8);
        }
        bool FreeToGo()
        {
            Location eLoc = Loc;
            eLoc.Walk(Direction);
            if (!DMaps.H_DMaps.ContainsKey(Loc.Map))
                return true;
            if (((DMap)DMaps.H_DMaps[Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess)
                return false;
            return true;
        }
    }
}
