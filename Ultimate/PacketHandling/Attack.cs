using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Bcpg.Sig;
using static Ultimate.Features.TCGuildWars;
using Ultimate.Events;
using Ultimate.Game;
using Ultimate.Structures;
using Ultimate;

namespace Ultimate.PacketHandling
{
    public class Attack
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            try
            {
                uint AttackType = BitConverter.ToUInt32(Data, 20);
                if (AttackType != 24)
                    GC.MyChar.AtkMem.AtkType = (byte)AttackType;
                GC.MyChar.Action = 100;

                if (!GC.MyChar.Alive) return;
                if (GC.MyChar.ProtectTime.AddMilliseconds(0) > DateTime.Now && !GC.MyChar.CancelProtectTime)
                    return;

                if (GC.MyChar.StatEff.Contains(Game.StatusEffectEn.IceBlock))
                    return;
                if (GC.WaitingKillCaptcha)
                {
                    //GC.KillCountCaptchaStamp = DateTime.Now;
                    GC.WaitingKillCaptcha = true;
                    GC.DialogNPC = 9999997;
                    if (GC.KillCountCaptcha == "")
                        GC.KillCountCaptcha = Program.Rnd.Next(10000, 50000).ToString();
                    GC.AddSend(Packets.NPCSay("Input the current text: " + GC.KillCountCaptcha + " to verify your humanity."));
                    GC.AddSend(Packets.NPCLink2("Captcha message:", (byte)GC.KillCountCaptcha.Length));
                    GC.AddSend(Packets.NPCLink("Just passing by", 255));
                    GC.AddSend(Packets.NPCSetFace(30));
                    GC.AddSend(Packets.NPCFinish());
                    return;
                }
                GC.LastAttack = DateTime.Now;
                Extra.Durability.AttackDurability(GC);
                if (AttackType == 2 || AttackType == 28)
                {
                    //     if (Game.World.PKTourny)
                    if (GC.MyChar.EventBase != null)
                        if (!GC.MyChar.EventBase.MeleeAllowed && GC.MyChar.EventBase?.Stage == Events.EventStage.Fighting)
                            return;
                    if (GC.MyChar.Arena != null && GC.MyChar.Arena.MapID == GC.MyChar.Loc.Map)
                        return;
                    if (GC.MyChar.Loc.Map == 8004 || GC.MyChar.Loc.Map == 8005 || GC.MyChar.Loc.Map == 8006)
                        if (AttackType != 2)
                            return;
                    if (AttackType == 2)
                        if (GC.MyChar.Flying)
                            return;
                    //if (!GC.MyChar.StatEff.Contains(Game.StatusEffectEn.Fly))

                    uint TargetUID = BitConverter.ToUInt32(Data, 12);
                    Game.Mob PossMob = null;
                    Game.Character PossChar = null;
                    Game.Companion PossComp = null;



                    if (Game.World.H_Mobs.ContainsKey(GC.MyChar.Loc.Map))
                    {
                        if (Game.World.H_Mobs[GC.MyChar.Loc.Map].ContainsKey(TargetUID))
                            PossMob = (Game.Mob)Game.World.H_Mobs[GC.MyChar.Loc.Map][TargetUID];
                        else if (Game.World.H_Chars.ContainsKey(TargetUID))
                            PossChar = Game.World.H_Chars[TargetUID];
                        else if (Game.World.H_Companions.ContainsKey(TargetUID))
                            PossComp = (Game.Companion)Game.World.H_Companions[TargetUID];
                    }
                    else if (Game.World.H_Chars.ContainsKey(TargetUID))
                        PossChar = Game.World.H_Chars[TargetUID];
                    else if (Game.World.H_Companions.ContainsKey(TargetUID))
                        PossComp = (Game.Companion)Game.World.H_Companions[TargetUID];
                    if (PossChar != null)
                    {
                        if (!PossChar.PKAble(GC.MyChar.PKMode, GC.MyChar) || PossChar.Loc.Map != GC.MyChar.Loc.Map)
                        {
                            PossChar = null;
                        }
                        if (PossChar != null)
                        {
                            if (PossChar.ProtectTime.AddMilliseconds(0) > DateTime.Now && !PossChar.CancelProtectTime)
                            {
                                PossChar = null;
                            }
                        }
                        if (PossChar != null)
                        {
                            if ((PossChar.Level <= 6 || GC.MyChar.Level <= 6) && (PossChar.Loc.Map == 1002 || PossChar.Loc.Map == 1011 || PossChar.Loc.Map == 1020 || PossChar.Loc.Map == 1000 || PossChar.Loc.Map == 1015 || PossChar.Loc.Map == 1009))
                            {
                                PossChar = null;
                                GC.LocalMessage(2005, "Newbies PK protection in this map! You cannot pk level 6 or below characters!");
                            }
                        }
                    }
                    byte Dist = Math.Max(GC.MyChar.Equips.RightHand.DBInfo.Dist, GC.MyChar.Transformation.Dist);
                    Dist = Math.Max((byte)2, Dist);
                    if (PossMob != null || PossChar != null || PossComp != null)
                    {
                        GC.MyChar.AtkMem.Target = TargetUID;
                        GC.MyChar.AtkMem.Attacking = true;


                        if (DateTime.Now >= GC.MyChar.AtkMem.LastAttack.AddMilliseconds(GC.MyChar.AtkFrequence))
                        {
                            uint Damage = GC.MyChar.PrepareAttack((byte)AttackType, true);
                            if (GC.MyChar.EventBase != null)
                            {
                                if (GC.MyChar.EventBase.NoDamage && GC.MyChar.EventBase?.Stage == Events.EventStage.Fighting)
                                    //if (GC.MyChar.EventBase?.MapEvent == GC.MyChar.Loc.Map)
                                    Damage = GC.MyChar.EventBase.GetDamage(GC.MyChar, PossChar, (AttackType)GC.MyChar.AtkMem.AtkType);
                            }
                            //if (GC.MyChar.Loc.Map == 1080 || (GC.MyChar.EventBase?.Stage == Events.EventStage.Fighting && GC.MyChar.EventBase?.MapEvent == GC.MyChar.Loc.Map && GC.MyChar.EventBase.FFADamage))
                            //{
                            //    if (AttackType == 2)
                            //        Damage = 8;
                            //    else if (AttackType == 28)
                            //        Damage = 2;
                            //}
                            // if (PossMob != null && PossMob.Alive && (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= Dist || AttackType == 28 && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= 15))
                            if (PossMob != null && PossMob.Alive && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossMob.Loc.X, PossMob.Loc.Y) <= Dist)
                            {
                                if ((GC.MyChar.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison && GC.MyChar.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(10))
                                {
                                    PossMob.TakeAttack(GC.MyChar, ref Damage, Game.AttackType.Melee, false, true);
                                    //Console.WriteLine("Poisoned");
                                }
                                else if ((GC.MyChar.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison || GC.MyChar.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(5))
                                {
                                    PossMob.TakeAttack(GC.MyChar, ref Damage, Game.AttackType.Melee, false, true);
                                }
                                if (!GC.MyChar.WeaponSkill(PossMob.Loc.X, PossMob.Loc.Y, PossMob.EntityID))
                                {
                                    PossMob.TakeAttack(GC.MyChar, ref Damage, (Game.AttackType)AttackType, false);
                                }
                            }
                            // else if (PossChar != null && (PossChar.CanBeMeleed || GC.MyChar.AtkMem.AtkType != 2) && PossChar.Alive && (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= 2 || AttackType == 28 && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= 15))
                            else if (PossChar != null && (PossChar.CanBeMeleed || GC.MyChar.AtkMem.AtkType != 2) && PossChar.Alive && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossChar.Loc.X, PossChar.Loc.Y) <= Dist)
                            {
                                if (!GC.MyChar.WeaponSkill(PossChar.Loc.X, PossChar.Loc.Y, PossChar.EntityID))
                                    PossChar.TakeAttack(GC.MyChar, ref Damage, (Ultimate.Game.AttackType)AttackType, false);
                                if (!Game.World.NoPKMaps.Contains(PossChar.Loc.Map) && GC.MyChar.Loc.Map != 1080)
                                    if ((GC.MyChar.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison && GC.MyChar.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(15))
                                    {
                                        Features.Poison.PoisonCharacter(PossChar.EntityID, GC.MyChar.EntityID);
                                        //Console.WriteLine("Poisoned");
                                    }
                                    else if ((GC.MyChar.Equips.LeftHand.Effect == Game.Item.RebornEffect.Poison || GC.MyChar.Equips.RightHand.Effect == Game.Item.RebornEffect.Poison) && MyMath.ChanceSuccess(10))
                                    {
                                        Features.Poison.PoisonCharacter(PossChar.EntityID, GC.MyChar.EntityID);
                                    }
                            }
                            // else if (PossComp != null && (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= Dist || (AttackType == 28 && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= 15)))
                            else if (PossComp != null && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossComp.Loc.X, PossComp.Loc.Y) <= Dist)
                            {
                                if (PossComp.Owner.EntityID != GC.MyChar.EntityID && PossComp.Owner.PKAble(GC.MyChar.PKMode, GC.MyChar))
                                {
                                    PossComp.TakeAttack(GC.MyChar, ref Damage, (Ultimate.Game.AttackType)AttackType, false);
                                }
                                else
                                {
                                    GC.MyChar.AtkMem.Target = 0;
                                    GC.MyChar.AtkMem.Attacking = false;
                                }
                            }
                            else
                            {
                                GC.MyChar.AtkMem.Target = 0;
                                GC.MyChar.AtkMem.Attacking = false;
                            }
                        }
                    }
                    else if (World.H_SOBs.ContainsKey(TargetUID))
                    {
                        GC.MyChar.AtkMem.Target = TargetUID;
                        GC.MyChar.AtkMem.Attacking = true;

                        if (DateTime.Now > GC.MyChar.AtkMem.LastAttack.AddMilliseconds(GC.MyChar.AtkFrequence))
                        {
                            uint Damage = GC.MyChar.PrepareAttack((byte)AttackType, true);
                            if (World.H_SOBs[TargetUID].IsPole())
                            {
                                if (World.H_SOBs[TargetUID].War && GC.MyChar.MyGuild != null && (World.H_SOBs[TargetUID].LastWinner == null || GC.MyChar.MyGuild.GuildID != World.H_SOBs[TargetUID].LastWinner.GuildID))
                                {
                                    if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, World.H_SOBs[TargetUID].Loc.X, World.H_SOBs[TargetUID].Loc.Y) > Dist)
                                    {
                                        GC.MyChar.AtkMem.Target = 0;
                                        GC.MyChar.AtkMem.Attacking = false;
                                    }
                                    if (!GC.MyChar.WeaponSkill(World.H_SOBs[TargetUID].Loc.X, World.H_SOBs[TargetUID].Loc.Y, World.H_SOBs[TargetUID].EntityID))
                                        World.H_SOBs[TargetUID].TakeAttack(GC.MyChar, Damage, (byte)AttackType);
                                }
                                else
                                {
                                    GC.MyChar.AtkMem.Target = 0;
                                    GC.MyChar.AtkMem.Attacking = false;
                                }
                            }
                            else
                            {
                                if (!GC.MyChar.WeaponSkill(World.H_SOBs[TargetUID].Loc.X, World.H_SOBs[TargetUID].Loc.Y, World.H_SOBs[TargetUID].EntityID))
                                    World.H_SOBs[TargetUID].TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                            }
                        }
                        return;
                    }
                    #region unused
                    //else if (TargetUID >= 6700 && TargetUID <= 6702)
                    //{
                    //    GC.MyChar.AtkMem.Target = TargetUID;
                    //    GC.MyChar.AtkMem.Attacking = true;

                    //    if (DateTime.Now > GC.MyChar.AtkMem.LastAttack.AddMilliseconds(GC.MyChar.AtkFrequence))
                    //    {
                    //        uint Damage = GC.MyChar.PrepareAttack((byte)AttackType, true);
                    //        if (TargetUID == 6700)
                    //        {
                    //            if (Features.GuildWars.War)
                    //            {
                    //                if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, Features.GuildWars.ThePole.Loc.X, Features.GuildWars.ThePole.Loc.Y) > Dist)
                    //                {
                    //                    GC.MyChar.AtkMem.Target = 0;
                    //                    GC.MyChar.AtkMem.Attacking = false;
                    //                }
                    //                if (!GC.MyChar.WeaponSkill(Features.GuildWars.ThePole.Loc.X, Features.GuildWars.ThePole.Loc.Y, Features.GuildWars.ThePole.EntityID))
                    //                    Features.GuildWars.ThePole.TakeAttack(GC.MyChar, Damage, (byte)AttackType);
                    //            }
                    //            else
                    //            {
                    //                GC.MyChar.AtkMem.Target = 0;
                    //                GC.MyChar.AtkMem.Attacking = false;
                    //            }
                    //        }
                    //        else if (TargetUID == 6701)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.GuildWars.TheLeftGate.Loc.X, Features.GuildWars.TheLeftGate.Loc.Y, Features.GuildWars.TheLeftGate.EntityID))
                    //                Features.GuildWars.TheLeftGate.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.GuildWars.TheRightGate.Loc.X, Features.GuildWars.TheRightGate.Loc.Y, Features.GuildWars.TheRightGate.EntityID))
                    //                Features.GuildWars.TheRightGate.TakeAttack(GC.MyChar, Damage, (byte)AttackType);
                    //        }
                    //    }
                    //    return;
                    //}
                    #region Counter Clock GW
                    //else if (TargetUID >= 6703 && TargetUID <= 6726)
                    //{
                    //    GC.MyChar.AtkMem.Target = TargetUID;
                    //    GC.MyChar.AtkMem.Attacking = true;

                    //    if (DateTime.Now > GC.MyChar.AtkMem.LastAttack.AddMilliseconds(GC.MyChar.AtkFrequence))
                    //    {
                    //        uint Damage = GC.MyChar.PrepareAttack((byte)AttackType, true);
                    //        if (TargetUID == 6726)
                    //        {
                    //            if (Features.CounterClock.War)
                    //            {
                    //                if (!GC.MyChar.WeaponSkill(Features.CounterClock.ThePole.Loc.X, Features.CounterClock.ThePole.Loc.Y, Features.CounterClock.ThePole.EntityID))
                    //                    Features.CounterClock.ThePole.TakeAttack(GC.MyChar, Damage, (byte)AttackType);
                    //            }
                    //            else
                    //            {
                    //                GC.MyChar.AtkMem.Target = 0;
                    //                GC.MyChar.AtkMem.Attacking = false;
                    //            }
                    //        }
                    //        else if (TargetUID == 6703)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG1.Loc.X, Features.CounterClock.LG1.Loc.Y, Features.CounterClock.LG1.EntityID))
                    //                Features.CounterClock.LG1.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6704)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG2.Loc.X, Features.CounterClock.LG2.Loc.Y, Features.CounterClock.LG2.EntityID))
                    //                Features.CounterClock.LG2.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6705)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG3.Loc.X, Features.CounterClock.LG3.Loc.Y, Features.CounterClock.LG3.EntityID))
                    //                Features.CounterClock.LG3.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6706)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG4.Loc.X, Features.CounterClock.LG4.Loc.Y, Features.CounterClock.LG4.EntityID))
                    //                Features.CounterClock.LG4.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6707)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG5.Loc.X, Features.CounterClock.LG5.Loc.Y, Features.CounterClock.LG5.EntityID))
                    //                Features.CounterClock.LG5.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6708)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.LG6.Loc.X, Features.CounterClock.LG6.Loc.Y, Features.CounterClock.LG6.EntityID))
                    //                Features.CounterClock.LG6.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6709)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG1.Loc.X, Features.CounterClock.RG1.Loc.Y, Features.CounterClock.RG1.EntityID))
                    //                Features.CounterClock.RG1.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6710)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG2.Loc.X, Features.CounterClock.RG2.Loc.Y, Features.CounterClock.RG2.EntityID))
                    //                Features.CounterClock.RG2.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6711)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG3.Loc.X, Features.CounterClock.RG3.Loc.Y, Features.CounterClock.RG3.EntityID))
                    //                Features.CounterClock.RG3.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6712)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG4.Loc.X, Features.CounterClock.RG4.Loc.Y, Features.CounterClock.RG4.EntityID))
                    //                Features.CounterClock.RG4.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6713)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG5.Loc.X, Features.CounterClock.RG5.Loc.Y, Features.CounterClock.RG5.EntityID))
                    //                Features.CounterClock.RG5.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6714)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG6.Loc.X, Features.CounterClock.RG6.Loc.Y, Features.CounterClock.RG6.EntityID))
                    //                Features.CounterClock.RG6.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6715)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG7.Loc.X, Features.CounterClock.RG7.Loc.Y, Features.CounterClock.RG7.EntityID))
                    //                Features.CounterClock.RG7.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6716)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG8.Loc.X, Features.CounterClock.RG8.Loc.Y, Features.CounterClock.RG8.EntityID))
                    //                Features.CounterClock.RG8.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6717)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG9.Loc.X, Features.CounterClock.RG9.Loc.Y, Features.CounterClock.RG9.EntityID))
                    //                Features.CounterClock.RG9.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6718)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG10.Loc.X, Features.CounterClock.RG10.Loc.Y, Features.CounterClock.RG10.EntityID))
                    //                Features.CounterClock.RG10.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6719)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG11.Loc.X, Features.CounterClock.RG11.Loc.Y, Features.CounterClock.RG11.EntityID))
                    //                Features.CounterClock.RG11.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6720)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG12.Loc.X, Features.CounterClock.RG12.Loc.Y, Features.CounterClock.RG12.EntityID))
                    //                Features.CounterClock.RG12.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6721)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG13.Loc.X, Features.CounterClock.RG13.Loc.Y, Features.CounterClock.RG13.EntityID))
                    //                Features.CounterClock.RG13.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6722)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG14.Loc.X, Features.CounterClock.RG14.Loc.Y, Features.CounterClock.RG14.EntityID))
                    //                Features.CounterClock.RG14.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6723)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG15.Loc.X, Features.CounterClock.RG15.Loc.Y, Features.CounterClock.RG15.EntityID))
                    //                Features.CounterClock.RG15.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else if (TargetUID == 6724)
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG16.Loc.X, Features.CounterClock.RG16.Loc.Y, Features.CounterClock.RG16.EntityID))
                    //                Features.CounterClock.RG16.TakeAttack(GC.MyChar, Damage, (byte)AttackType);

                    //        }
                    //        else
                    //        {
                    //            if (!GC.MyChar.WeaponSkill(Features.CounterClock.RG17.Loc.X, Features.CounterClock.RG17.Loc.Y, Features.CounterClock.RG17.EntityID))
                    //                Features.CounterClock.RG17.TakeAttack(GC.MyChar, Damage, (byte)AttackType);
                    //        }
                    //    }
                    //    return;
                    //}
                    #endregion
                    #endregion

                    else
                        GC.MyChar.AtkMem.Attacking = false;

                    if (PossChar == null && PossMob == null && PossComp == null)
                    {
                        if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                        {
                            Dictionary<uint, NPC> MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                            if (MapNPC.ContainsKey(TargetUID))
                            {
                                Game.NPC PossNPC = (Game.NPC)MapNPC[TargetUID];
                                //if (PossNPC != null && PossNPC.Flags == 21 && (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= Dist || AttackType == 28 && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= Dist))
                                if (PossNPC.Flags == 21 && MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, PossNPC.Loc.X, PossNPC.Loc.Y) <= Dist)
                                {
                                    GC.MyChar.AtkMem.Target = TargetUID;
                                    GC.MyChar.AtkMem.Attacking = true;

                                    if (DateTime.Now > GC.MyChar.AtkMem.LastAttack.AddMilliseconds(GC.MyChar.AtkFrequence))
                                    {
                                        uint Damage = GC.MyChar.PrepareAttack((byte)AttackType, true);
                                        if (!GC.MyChar.WeaponSkill(PossNPC.Loc.X, PossNPC.Loc.Y, PossNPC.EntityID))
                                            PossNPC.TakeAttack(GC.MyChar, Damage, (Ultimate.Game.AttackType)AttackType, false);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (AttackType == 24)
                {
                    ushort SkillId;
                    long x;
                    long y;
                    uint Target;
                    #region GetSkillID
                    SkillId = Convert.ToUInt16(((long)Data[24] & 0xFF) | (((long)Data[25] & 0xFF) << 8));
                    SkillId ^= (ushort)0x915d;
                    SkillId ^= (ushort)GC.MyChar.EntityID;
                    SkillId = (ushort)(SkillId << 0x3 | SkillId >> 0xd);
                    SkillId -= 0xeb42;
                    #endregion
                    #region GetCoords
                    x = (Data[16] & 0xFF) | ((Data[17] & 0xFF) << 8);
                    x = x ^ (uint)(GC.MyChar.EntityID & 0xffff) ^ 0x2ed6;
                    x = ((x << 1) | ((x & 0x8000) >> 15)) & 0xffff;
                    x |= 0xffff0000;
                    x -= 0xffff22ee;

                    y = (Data[18] & 0xFF) | ((Data[19] & 0xFF) << 8);
                    y = y ^ (uint)(GC.MyChar.EntityID & 0xffff) ^ 0xb99b;
                    y = ((y << 5) | ((y & 0xF800) >> 11)) & 0xffff;
                    y |= 0xffff0000;
                    y -= 0xffff8922;
                    #endregion
                    #region GetTarget
                    Target = ((uint)Data[12] & 0xFF) | (((uint)Data[13] & 0xFF) << 8) | (((uint)Data[14] & 0xFF) << 16) | (((uint)Data[15] & 0xFF) << 24);
                    //Console.WriteLine("Target: " + Target);
                    Target = ((((Target & 0xffffe000) >> 13) | ((Target & 0x1fff) << 19)) ^ 0x5F2D2463 ^ GC.MyChar.EntityID) - 0x746F4AE6;
                    //Console.WriteLine("FTarget: " + Target);
                    #endregion
                    // Console.WriteLine(SkillId);
                    // Console.WriteLine(x);
                    //Console.WriteLine(y);
                    if (SkillId == 7020 || SkillId == 5010 || SkillId == 1260 || SkillId == 5030 || SkillId == 5040 || SkillId == 1290 || SkillId == 7000 || SkillId == 7010 || SkillId == 7030 || SkillId == 1250 || SkillId == 5050 || SkillId == 5020 || SkillId == 1300 || SkillId == 7040)
                        return;
                    /* if (GC.MyChar.Flying)
                         if (SkillId != 8002 && SkillId != 8003)
                             return;*/
                    if (SkillId == 1051 && GC.MyChar.Loc.Map == 1004) // Only allow Dash skill in GuildWar map. Block it in all other maps.
                        return;
                    if (GC.MyChar.Loc.Map == 8004 || GC.MyChar.Loc.Map == 8005 || GC.MyChar.Loc.Map == 8006)
                        if (SkillId != 1110)
                            return;

                    if (SkillId == 1045
                       || SkillId == 2001 || SkillId == 2002 || SkillId == 2003 || SkillId == 2004 || SkillId == 2005 || SkillId == 2006 || SkillId == 2007 || SkillId == 2008 || SkillId == 2009 || SkillId == 2010
                       || SkillId == 2011 || SkillId == 2012 || SkillId == 2013 || SkillId == 2014 || SkillId == 2015 || SkillId == 2016 || SkillId == 2017 || SkillId == 2018 || SkillId == 2019 || SkillId == 2020)
                    {
                        if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.LeftHand.ID, 0, 3) != 410 && Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) != 410) || GC.MyChar.Transformation.Transformed)
                            return;
                    }
                    else if (SkillId == 1046
                        || SkillId == 2101 || SkillId == 2102 || SkillId == 2103 || SkillId == 2104 || SkillId == 2105 || SkillId == 2106 || SkillId == 2107 || SkillId == 2108 || SkillId == 2109 || SkillId == 2110
                        || SkillId == 2111 || SkillId == 2112 || SkillId == 2113 || SkillId == 2114 || SkillId == 2115 || SkillId == 2116 || SkillId == 2117 || SkillId == 2118 || SkillId == 2119 || SkillId == 2120)
                    {
                        if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.LeftHand.ID, 0, 3) != 420 && Game.ItemIDManipulation.Part(GC.MyChar.Equips.LeftHand.ID, 0, 3) != 421 && Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) != 420 && Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) != 421) || GC.MyChar.Transformation.Transformed)
                            return;
                    }
                    else if (SkillId == 1047)
                    {
                        if ((Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 1) != 5 || Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) == 500) || GC.MyChar.Transformation.Transformed)
                            return;
                    }
                    if (GC.MyChar.Loc.Map != 1038 && GC.MyChar.Loc.Map != 700 && !DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                    {
                        for (byte i = 1; i < 8; i++)
                        {
                            Game.Item I = GC.MyChar.Equips.Get(i);
                            if (I.ID != 0 && I.Soc1 != Game.Item.Gem.EmptySocket)
                                Features.GemEffect.GemEffects(I.Soc1, GC, GC.MyChar);
                        }
                    }
                    if (GC.MyChar.Equips.Garment.ID == 193255)
                        if (MyMath.ChanceSuccess(5))
                            GC.MyChar.SendScreen(Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "akatsuki4"));

                    if (SkillId != 0 && GC.MyChar.Skills.ContainsKey(SkillId))
                    {
                        Game.Skill S = (Game.Skill)GC.MyChar.Skills[SkillId];
                        if (Features.SkillsClass.SkillInfos.ContainsKey(S.ID + " " + S.Lvl))
                        {
                            Features.SkillsClass.SkillUse SU = new Ultimate.Features.SkillsClass.SkillUse();
                            SU.Init(GC.MyChar, S.ID, S.Lvl, (ushort)x, (ushort)y);
                            if (SU.Info.ID == 0)
                                return;
                            bool EnoughArrows = true;
                            #region ArrowCost
                            if (SU.Info.ArrowsCost > 0)
                            {
                                if (GC.MyChar.Loc.Map != 1039)
                                {
                                    if (DateTime.Now >= GC.MyChar.AtkMem.LastAttack.AddMilliseconds(250)) // Gump mod default = 800 ms
                                    {
                                        if (GC.MyChar.Equips.LeftHand.ID != 0 && Game.Item.IsArrow(GC.MyChar.Equips.LeftHand.ID))
                                        {
                                            if (GC.MyChar.Equips.LeftHand.CurDur > GC.MyChar.Equips.LeftHand.MaxDur)
                                                GC.MyChar.Equips.LeftHand.CurDur = GC.MyChar.Equips.LeftHand.MaxDur;
                                            if (GC.MyChar.Equips.LeftHand.CurDur >= SU.Info.ArrowsCost)
                                            {
                                                GC.MyChar.Equips.LeftHand.CurDur -= SU.Info.ArrowsCost;
                                            }
                                            else
                                            {
                                                GC.MyChar.Equips.LeftHand.CurDur = 0;
                                            }
                                            if (GC.MyChar.Equips.LeftHand.CurDur == 0)
                                            {
                                                if (GC.MyChar.InventoryContains(1050000, 1))
                                                {
                                                    GC.MyChar.Equips.LeftHand = GC.MyChar.NextItem(1050000);
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1050000));
                                                }
                                                else if (GC.MyChar.InventoryContains(1050001, 1) && GC.MyChar.Level >= 32)
                                                {
                                                    GC.MyChar.Equips.LeftHand = GC.MyChar.NextItem(1050001);
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1050001));
                                                }
                                                else if (GC.MyChar.InventoryContains(1050002, 1) && GC.MyChar.Level >= 73)
                                                {
                                                    GC.MyChar.Equips.LeftHand = GC.MyChar.NextItem(1050002);
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1050002));
                                                }
                                                else if (GC.MyChar.InventoryContains(1051000, 1) && GC.MyChar.Level >= 1)
                                                {
                                                    GC.MyChar.Equips.LeftHand = GC.MyChar.NextItem(1051000);
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(1051000));
                                                }
                                                else
                                                    GC.LocalMessage(2005, "You ran out of arrows!");
                                            }

                                            if (GC.MyChar.Equips.LeftHand.CurDur == 0)
                                            {
                                                GC.AddSend(Packets.ItemPacket(GC.MyChar.Equips.LeftHand.UID, 5, 6));
                                                GC.AddSend(Packets.ItemPacket(GC.MyChar.Equips.LeftHand.UID, 0, 3));
                                                GC.MyChar.Equips.LeftHand = new Game.Item();
                                            }
                                            else
                                                GC.AddSend(Packets.AddItem(GC.MyChar.Equips.LeftHand, 5));
                                        }
                                        else
                                        {
                                            GC.MyChar.AtkMem.Attacking = false;
                                            EnoughArrows = false;
                                        }
                                    }
                                }
                            }
                            #endregion
                            if (GC.MyChar.CurMP >= SU.Info.ManaCost && GC.MyChar.Stamina >= SU.Info.StaminaCost && (EnoughArrows || GC.MyChar.Loc.Map == 1039 && GC.MyChar.Loc.Map == 1004))
                            {
                                if (GC.MyChar.EventBase != null)
                                {
                                    if (!GC.MyChar.EventBase.MagicAllowed && GC.MyChar.EventBase?.Stage == Events.EventStage.Fighting)
                                    {
                                        if (GC.MyChar.EventBase?.AllowedSkills != null)
                                        {

                                            if (GC.MyChar.EventBase?.AllowedSkills.Count > 0)
                                            {
                                                if (!GC.MyChar.EventBase.AllowedSkills.Contains(SU.Info.ID))
                                                {
                                                    GC.LocalMessage(2005, "This skill cannot be used in this event!");
                                                    return;
                                                }
                                            }
                                            else
                                                return;
                                        }
                                        else
                                        {
                                            GC.LocalMessage(2005, "This skill cannot be used in this event!");
                                            return;
                                        }
                                    }
                                }
                                if (GC.MyChar.Arena != null && GC.MyChar.Arena.MapID == GC.MyChar.Loc.Map)
                                    if (SU.Info.ID != 1045 && SU.Info.ID != 1046 && SU.Info.ID != 1047
                                        && SU.Info.ID != 2001 && SU.Info.ID != 2002 && SU.Info.ID != 2003 && SU.Info.ID != 2004 && SU.Info.ID != 2005 && SU.Info.ID != 2006 && SU.Info.ID != 2007 && SU.Info.ID != 2008 && SU.Info.ID != 2009 && SU.Info.ID != 2010
                                        && SU.Info.ID != 2011 && SU.Info.ID != 2012 && SU.Info.ID != 2013 && SU.Info.ID != 2014 && SU.Info.ID != 2015 && SU.Info.ID != 2016 && SU.Info.ID != 2017 && SU.Info.ID != 2018 && SU.Info.ID != 2019 && SU.Info.ID != 2020
                                        && SU.Info.ID != 2101 && SU.Info.ID != 2102 && SU.Info.ID != 2103 && SU.Info.ID != 2104 && SU.Info.ID != 2105 && SU.Info.ID != 2106 && SU.Info.ID != 2107 && SU.Info.ID != 2108 && SU.Info.ID != 2109 && SU.Info.ID != 2110
                                        && SU.Info.ID != 2111 && SU.Info.ID != 2112 && SU.Info.ID != 2113 && SU.Info.ID != 2114 && SU.Info.ID != 2115 && SU.Info.ID != 2116 && SU.Info.ID != 2117 && SU.Info.ID != 2118 && SU.Info.ID != 2119 && SU.Info.ID != 2120)

                                    {
                                        GC.LocalMessage(2005, "Only FastBlade, ScentSword or ViperFang can be used in this map!");
                                        return;
                                    }

                                if (SU.Info.EndsXPWait)
                                {
                                    if (GC.MyChar.StatEff.Contains(StatusEffectEn.XPStart))
                                    {
                                        GC.MyChar.StatEff.Remove(StatusEffectEn.XPStart);
                                        Buff B = new Buff()
                                        {
                                            StEff = StatusEffectEn.XPStart,
                                            Lasts = 20,
                                            Started = DateTime.Now,
                                            Eff = Features.SkillsClass.ExtraEffect.None
                                        };
                                        GC.MyChar.BDelete.TryAdd(B, B.Lasts);
                                    }
                                    else
                                        return;
                                }
                                try  // 
                                {
                                    if (DateTime.Now >= GC.MyChar.AtkMem.LastAttack.AddMilliseconds(550) && (SkillId != 1120 || !GC.MyChar.AtkMem.FireCircle) && SkillId != 1115 && SkillId != 8001 || (SkillId == 1115 && DateTime.Now >= GC.MyChar.AtkMem.LastAttack.AddMilliseconds(1200)) || (SkillId == 8001 && DateTime.Now >= GC.MyChar.AtkMem.LastAttack.AddMilliseconds(800)) || SU.Info.ID == 1110 || SU.Info.ID == 1025 || SU.Info.EndsXPWait || SU.Info.ID == 1045 || SU.Info.ID == 1046 || SU.Info.ID == 1047
                                     || SU.Info.ID == 2001 || SU.Info.ID == 2002 || SU.Info.ID == 2003 || SU.Info.ID == 2004 || SU.Info.ID == 2005 || SU.Info.ID == 2006 || SU.Info.ID == 2007 || SU.Info.ID == 2008 || SU.Info.ID == 2009 || SU.Info.ID == 2010
                                     || SU.Info.ID == 2101 || SU.Info.ID == 2102 || SU.Info.ID == 2103 || SU.Info.ID == 2104 || SU.Info.ID == 2105 || SU.Info.ID == 2106 || SU.Info.ID == 2107 || SU.Info.ID == 2108 || SU.Info.ID == 2109 || SU.Info.ID == 2110) // Gump mod: delay 800 ms default
                                    {
                                        if (GC.MyChar.Loc.Map != 1039 && GC.MyChar.Loc.Map != 701)
                                        {
                                            GC.MyChar.AtkMem.LastAttack = DateTime.Now;

                                            if (SU.Info.ID == 1000 || SU.Info.ID == 1001 || SU.Info.ID == 1002 || SU.Info.ID == 3090)
                                            {
                                                if (SU.Info.ID == 3090)
                                                {
                                                    GC.MyChar.Stamina -= SU.Info.StaminaCost;
                                                    GC.MyChar.Pervade = 4;
                                                }

                                                GC.MyChar.AtkMem.AtkType = 21;
                                                GC.MyChar.AtkMem.Skill = SU.Info.ID;
                                                GC.MyChar.AtkMem.Attacking = true;
                                                GC.MyChar.AtkMem.Target = Target;

                                                GC.MyChar.AtkMem.SX = (ushort)x;
                                                GC.MyChar.AtkMem.SY = (ushort)y;


                                                if (Game.World.H_Chars.ContainsKey(Target))
                                                {
                                                    Game.Character C = Game.World.H_Chars[Target];

                                                    // GC.MyChar.AtkMem.Target = 0;
                                                    GC.MyChar.AtkMem.Attacking = false;
                                                    if (C.Loc.Map != GC.MyChar.Loc.Map)
                                                        return;
                                                    //else if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 15)
                                                    //    return;
                                                }
                                                else if (Game.World.H_Companions.ContainsKey(Target))
                                                {
                                                    Game.Companion Comp = (Game.Companion)Game.World.H_Companions[Target];
                                                    if (Comp.Owner.EntityID == GC.MyChar.EntityID)
                                                    {
                                                        //GC.MyChar.AtkMem.Target = 0;
                                                        GC.MyChar.AtkMem.Attacking = false;
                                                        return;
                                                    }
                                                }
                                                else if (Game.World.H_NPCs.ContainsKey(GC.MyChar.Loc.Map))
                                                {
                                                    Dictionary<uint, NPC> MapNPC = Game.World.H_NPCs[GC.MyChar.Loc.Map];
                                                    if (MapNPC.ContainsKey(Target))
                                                    {
                                                        Game.NPC N = (Game.NPC)MapNPC[Target];

                                                        // GC.MyChar.AtkMem.Target = 0;
                                                        GC.MyChar.AtkMem.Attacking = false;
                                                        return;

                                                    }
                                                }
                                                SU.GetTargets(Target);
                                                SU.Use();
                                            }
                                            else if (SU.Info.ID != 1100 && SU.Info.ID != 1050)
                                            {
                                                if (SU.Info.ID == 1120)
                                                {
                                                    GC.MyChar.AtkMem.FireCircle = true;
                                                }
                                                else
                                                {
                                                    SU.GetTargets(Target);
                                                    SU.Use();
                                                }
                                            }
                                            if (GC.MyChar.Equips.RightHand.Effect == Ultimate.Game.Item.RebornEffect.MP)//ManaBS
                                            {
                                                if (MyMath.ChanceSuccess(30))
                                                {
                                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "spilth1"));
                                                    GC.MyChar.CurMP += 310;
                                                }
                                            }
                                            else if (GC.MyChar.Equips.RightHand.Effect == Ultimate.Game.Item.RebornEffect.HP)//HpBS
                                            {
                                                if (MyMath.ChanceSuccess(30))
                                                {
                                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "spilth"));
                                                    if (GC.MyChar.EventBase != null && GC.MyChar.EventBase?.Stage == Events.EventStage.Fighting && GC.MyChar.EventBase.NoDamage)
                                                        GC.MyChar.CurHP += 3;
                                                    else
                                                        GC.MyChar.CurHP += 310;
                                                }
                                            }
                                            if (SU.Info.ID != 1100 && SU.Info.ID != 1050 && SU.Info.ID != 3090)
                                            {
                                                GC.MyChar.CurMP -= SU.Info.ManaCost;
                                                if (GC.MyChar.Arena != null && GC.MyChar.Loc.Map == GC.MyChar.Arena.MapID)
                                                    GC.MyChar.Arena.Shot(GC.MyChar, SU.Info);
                                                else if (GC.MyChar.EventBase != null && GC.MyChar.EventBase.Stage == Events.EventStage.Fighting)
                                                    GC.MyChar.EventBase.Shot(GC.MyChar, SU.Info);
                                                else if (GC.MyChar.Loc.Map != World.UnlimitedStaminaMap && GC.MyChar.Loc.Map != 8000)
                                                    GC.MyChar.Stamina -= SU.Info.StaminaCost;
                                            }
                                            else if (SU.Info.ID == 3090)
                                            {
                                                if (GC.MyChar.Pervade <= 0)
                                                    GC.MyChar.Stamina -= SU.Info.StaminaCost;

                                                GC.MyChar.CurMP -= SU.Info.ManaCost;
                                            }
                                        }
                                        else
                                        {
                                            if (GC.MyChar.Loc.Map == 1039)
                                            {
                                                GC.MyChar.AtkMem.AtkType = 21;
                                                GC.MyChar.AtkMem.Skill = SU.Info.ID;
                                                if (!SU.Info.EndsXPWait)
                                                    GC.MyChar.AtkMem.Attacking = true;
                                                else GC.MyChar.AtkMem.Attacking = false;
                                                GC.MyChar.AtkMem.Target = Target;
                                                GC.MyChar.AtkMem.LastAttack = DateTime.Now;
                                                GC.MyChar.AtkMem.SX = (ushort)x;
                                                GC.MyChar.AtkMem.SY = (ushort)y;
                                            }
                                            SU.GetTargets(Target);
                                            SU.Use();
                                        }
                                    }
                                }
                                catch (Exception c) { Game.World.ExcAdd += c.ToString() + "\r\n"; Console.WriteLine(c); }
                                if (SU.Info.ID == 1100 || SU.Info.ID == 1050)
                                {
                                    #region Pray
                                    if (SU.User.EntityID != Target)
                                    {
                                        if (Game.World.H_Chars.ContainsKey(Target))
                                        {
                                            Game.Character Char = Game.World.H_Chars[Target];
                                            if (!Char.Alive)
                                            {
                                                GC.MyChar.CurMP -= SU.Info.ManaCost;
                                                GC.MyChar.Stamina -= SU.Info.StaminaCost;
                                                SU.GetTargets(Target);
                                                Char.CancelProtectTime = false;
                                                Char.ProtectTime = DateTime.Now.AddSeconds(0);
                                                if (SU.PlayerTargets.ContainsKey(Char))
                                                {
                                                    SU.PlayerTargets[Char] = (uint)0;
                                                    SU.Use();
                                                }
                                            }

                                            /* Game.Character Char = Game.World.H_Chars[Target];
                                             if (!Char.Alive)
                                             {
                                                 GC.MyChar.CurMP -= SU.Info.ManaCost;
                                                 GC.MyChar.Stamina -= SU.Info.StaminaCost;
                                                 SU.GetTargets(Target);
                                                 SU.PlayerTargets[Char] = (uint)1;
                                                 Game.World.Action(Char, Packets.SkillUse(SU).Get);
                                                 Char.Ghost = false;
                                                 Char.BlueName = false;
                                                 Char.CurHP = (ushort)Char.MaxHP;
                                                 Char.Alive = true;
                                                 Char.StatEff.Remove(StatusEffectEn.Dead);
                                                 Char.StatEff.Remove(StatusEffectEn.BlueName);
                                                 Char.XPKO = 0;
                                                 Char.Body = Char.Body;
                                                 Char.Hair = Char.Hair;
                                                 Char.Equips.Send(Char.MyClient, false);
                                             }*/
                                        }
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                GC.MyChar.AtkMem.Target = 0;
                                GC.MyChar.AtkMem.Attacking = false;
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Game.World.ExcAdd += e.ToString() + "\r\n"; }
        }
    }
}
