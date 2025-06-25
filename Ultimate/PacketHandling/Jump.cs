using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class Jump
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            /*if (GC.MyChar.BuffOf(Ultimate.Features.SkillsClass.ExtraEffect.BlessPray).Eff == Ultimate.Features.SkillsClass.ExtraEffect.BlessPray)
                GC.MyChar.BDelete.Add(GC.MyChar.BuffOf(Ultimate.Features.SkillsClass.ExtraEffect.BlessPray));*/
            GC.MyChar.Mining = false;
            GC.MyChar.AtkMem.Attacking = false;
            GC.MyChar.AtkMem.FireCircle = false;
            GC.MyChar.Action = 100;
            if (!GC.MyChar.Transformation.Transformed)
                GC.MyChar.ExtraDex = 0;
            ushort NX = BitConverter.ToUInt16(Data, 8);
            ushort NY = BitConverter.ToUInt16(Data, 10);
            if (GC.MyChar.StatEff.Contains(Game.StatusEffectEn.IceBlock))
            {
                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x6c));
                return;
            }
            if (GC.MyChar.Loc.AbleToJump(NX, NY, GC.MyChar.StatEff.Contains(Ultimate.Game.StatusEffectEn.Cyclone), GC.MyChar.DH || GC.MyChar.StatEff.Contains(Game.StatusEffectEn.Fly)) && GC.MyChar.Alive && GC.MyChar.MyShop == null)
            {
                if (GC.MyChar.MyShop != null)
                    GC.MyChar.MyShop.Close();
                GC.MyChar.LastMove = DateTime.Now;
                GC.MyChar.Direction = (byte)MyMath.GetAngle(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, NX, NY);
                // Game.World.Action(GC.MyChar, Data);
                if (GC.MyChar.RecordAction)
                    Game.World.Actions += GC.MyChar.Name + " Jump at : " + DateTime.Now + " Distance : " + MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, NX, NY) + " Cyclone: " + GC.MyChar.StatEff.Contains(Ultimate.Game.StatusEffectEn.Cyclone) + " DH/Fly: " + (GC.MyChar.DH || GC.MyChar.StatEff.Contains(Game.StatusEffectEn.Fly)) + "\r\n";

                if ((GC.MyChar.JumpingStamp - GC.MyChar.PreviousJump).TotalMilliseconds <= 800)
                {
                    if (!GC.MyChar.StatEff.Contains(StatusEffectEn.Ride) && !GC.MyChar.StatEff.Contains(StatusEffectEn.Cyclone) && !GC.MyChar.StatEff.Contains(StatusEffectEn.Fly) && !GC.MyChar.Transformation.Transformed)
                    {
                        GC.MyChar.CountSpeedHack++;
                        if (GC.MyChar.CountSpeedHack >= 4)
                        {
                            AntiCheatPacket.Report(GC.MyChar.Name, "SpeedHACK", GC.MyChar.EntityID);
                            GC.Disconnect();
                        }
                    }
                }
                else
                {
                    if (GC.MyChar.CountSpeedHack > 0)
                        GC.MyChar.CountSpeedHack = Math.Max(0, GC.MyChar.CountSpeedHack - 1);
                }
                GC.MyChar.PreviousJump = GC.MyChar.JumpingStamp;
                GC.MyChar.JumpingStamp = DateTime.Now;

                GC.MyChar.Loc.Jump(NX, NY);
                GC.AddSend(Data);
                #region Check For Characters
                //ConcurrentDictionary<uint, Game.Character> Map = (ConcurrentDictionary<uint, Game.Character>)Game.World.PlayersInMap[GC.MyChar.Loc.Map];
                /*foreach (Game.Character C in Map.Values)*/
                foreach (Game.Character C in Game.World.H_Chars.Values)
                {
                    if (C != null)
                    {
                        if (C.Loc.Map == GC.MyChar.Loc.Map)
                        {
                            if (MyMath.InBox(GC.MyChar.Loc.PreviousX, GC.MyChar.Loc.PreviousY, C.Loc.X, C.Loc.Y, 28)) //&& MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28))
                                                                                                                      // if (MyMath.PointDistance(GC.MyChar.Loc.PreviousX, GC.MyChar.Loc.PreviousY, C.Loc.X, C.Loc.Y) <= 18)
                            {

                                // Console.WriteLine("Previous location in screen");
                                if (C.EntityID != GC.MyChar.EntityID)
                                {
                                    C.MyClient.AddSend(Data);
                                    if (!MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28))
                                    //  if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) > 18)
                                    {
                                        // Console.WriteLine("New location not in screen");
                                        if (C.ScreenChars.ContainsKey(GC.MyChar.EntityID))
                                        {
                                            C.ScreenChars.Remove(GC.MyChar.EntityID);
                                            if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                            {
                                                GC.MyChar.ScreenChars.Remove(C.EntityID);
                                            }
                                        }
                                        else if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                        {
                                            GC.MyChar.ScreenChars.Remove(C.EntityID);
                                        }
                                    }
                                }
                            }
                            else if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, C.Range()))
                            // else if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18)
                            {
                                // Console.WriteLine("Previous location not in screen , new location in screen");
                                if (!GC.MyChar.Invisible)
                                {
                                    if (GC.MyChar.MyGuild != null)
                                        C.MyClient.AddSend(Packets.StringPacket(GC.MyChar.MyGuild.GuildID, Game.StringType.GuildName, GC.MyChar.MyGuild.GuildName));
                                    C.MyClient.AddSend(Packets.SpawnEntity(GC.MyChar));
                                }
                                if (!C.Invisible)
                                {
                                    if (C.MyGuild != null)
                                        GC.AddSend(Packets.StringPacket(C.MyGuild.GuildID, Game.StringType.GuildName, C.MyGuild.GuildName));
                                    GC.AddSend(Packets.SpawnEntity(C));
                                }

                                if (!C.ScreenChars.ContainsKey(GC.MyChar.EntityID))
                                {
                                    C.ScreenChars.TryAdd(GC.MyChar.EntityID, GC.MyChar);

                                    if (!GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                    {
                                        GC.MyChar.ScreenChars.TryAdd(C.EntityID, C);

                                    }
                                }
                                else if (!GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                {
                                    GC.MyChar.ScreenChars.TryAdd(C.EntityID, C);
                                }
                            }
                        }
                        else
                        {
                            if (C.ScreenChars.ContainsKey(GC.MyChar.EntityID))
                            {
                                C.ScreenChars.Remove(GC.MyChar.EntityID);
                                //C.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, GC.MyChar.Loc.Map,GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 135).Get);
                                C.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                {
                                    GC.MyChar.ScreenChars.Remove(C.EntityID);
                                    //GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                    GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                }
                            }
                            else if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                            {
                                GC.MyChar.ScreenChars.Remove(C.EntityID);
                                // GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                            }
                        }
                    }
                }
                /* List<Character> Chrs = new List<Character>();
                     foreach (Game.Character CC in GC.MyChar.ScreenChars.Values)
                         if (CC.Loc.Map != GC.MyChar.Loc.Map)
                             if (GC.MyChar.ScreenChars.ContainsKey(CC.EntityID))
                             {
                                 // C.ScreenChars.Remove(CC.EntityID);
                                 Chrs.Add(CC.EntityID);
                                 // C.MyClient.AddSend(Packets.GeneralData(CC.EntityID, CC.Loc.Map, CC.Loc.X,CC.Loc.Y, 135).Get);
                                 GC.AddSend(Packets.GeneralData(CC.EntityID, 0, 0, 0, 135).Get);
                                 if (CC.ScreenChars.ContainsKey(GC.MyChar.EntityID))
                                 {
                                     CC.ScreenChars.Remove(GC.MyChar.EntityID);
                                     //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map,C.Loc.X,C.Loc.Y, 135).Get);
                                     CC.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                 }
                             }
                             else if (CC.ScreenChars.ContainsKey(GC.MyChar.EntityID))
                             {
                                 CC.ScreenChars.Remove(GC.MyChar.EntityID);
                                 //CC.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                 CC.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                             }
                     foreach (uint Key in Chrs)
                         GC.MyChar.ScreenChars.Remove(Key);*/
                #endregion
                Game.World.Spawns(GC.MyChar, true, false);

                #region Check for Traps
                if (Game.World.H_Effects.ContainsKey(GC.MyChar.Loc.Map))
                {
                    foreach (Game.MapEffect I in Game.World.H_Effects[GC.MyChar.Loc.Map].Values)
                    {
                        if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, I.Loc.X, I.Loc.Y, 28))
                            Traps.Handle(GC.MyChar, I);
                    }
                }
                #endregion

                if (!GC.MyChar.CancelProtectTime)
                    GC.MyChar.CancelProtectTime = true;
                GC.MyChar.Tank = true;
                GC.MyChar.CheckTank = true;
                if (!GC.MyChar.Transformation.Transformed)
                {
                    GC.MyChar.Transformation.Dex = 0;
                    GC.MyChar.Transformation.Dist = 0;
                }
            }
            else
            {
                GC.LocalMessage(2005, "Invalid Jump!");
                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x6c));
            }
        }
    }
}