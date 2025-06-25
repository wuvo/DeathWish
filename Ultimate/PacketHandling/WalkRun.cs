using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class WalkRun
    {
        public static void Handle(Main.GameClient GC, byte[] Data, byte Dir = 8)
        {
            bool PlaceFree = true;
            GC.MyChar.CheckTank = true;
            Game.Location eLoc = GC.MyChar.Loc;
            if (Dir == 8)
                eLoc.Walk((byte)(Data[4] % 8));
            else eLoc.Walk(Dir);
            if (GC.MyChar.StatEff.Contains(Game.StatusEffectEn.IceBlock))
            {
                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 0x6c));
                return;
            }
            if (DMaps.Loaded)
            {
                if ((DMaps.H_DMaps.ContainsKey(GC.MyChar.Loc.Map)))
                    if (((DMap)DMaps.H_DMaps[GC.MyChar.Loc.Map]).GetCell(eLoc.X, eLoc.Y).NoAccess)
                        PlaceFree = false;
            }
            if (PlaceFree && GC.MyChar.Loc.Map == 1038)
                PlaceFree = GC.MyChar.Loc.AbleToWalkGW(eLoc.X, eLoc.Y);
            GC.MyChar.LastMove = DateTime.Now;
            /* if (GC.MyChar.BuffOf(Ultimate.Features.SkillsClass.ExtraEffect.BlessPray).Eff == Ultimate.Features.SkillsClass.ExtraEffect.BlessPray)
                 GC.MyChar.BDelete.Add(GC.MyChar.BuffOf(Ultimate.Features.SkillsClass.ExtraEffect.BlessPray));*/
            GC.MyChar.Mining = false;
            GC.MyChar.AtkMem.Attacking = false;
            if (!GC.MyChar.Transformation.Transformed)
                GC.MyChar.ExtraDex = 0;
            //GC.MyChar.AtkMem.FireCircle = false; // Gump note: disabled
            GC.MyChar.Action = 100;

            if (Dir == 8)
                GC.MyChar.Direction = (byte)(Data[4] % 8);
            else GC.MyChar.Direction = Dir;
            if (PlaceFree)
            {
                //  Game.World.Action(GC.MyChar, Data);
                if (GC.MyChar.MyShop != null)
                    GC.MyChar.MyShop.Close();
                if (Dir == 8)
                {
                    GC.MyChar.Loc.Walk((byte)(Data[4] % 8));
                    GC.AddSend(Data);
                }
                else
                {
                    GC.MyChar.Loc.Walk(Dir);
                    GC.AddSend(Packets.Movement(GC.MyChar.EntityID, Dir).Get);
                    // World.Action(this, Packets.Movement(EntityID, Direction).Get);
                }

                if (!GC.MyChar.CancelProtectTime)
                    GC.MyChar.CancelProtectTime = true;
                //Game.World.Spawns(GC.MyChar, true);
                #region Check For Characters
                // ConcurrentDictionary<uint, Game.Character> Map = (ConcurrentDictionary<uint, Game.Character>)Game.World.PlayersInMap[GC.MyChar.Loc.Map];
                /*foreach (Game.Character C in Map.Values)*/
                foreach (Game.Character C in Game.World.H_Chars.Values)
                {
                    if (C != null)
                        if (C.Loc.Map == GC.MyChar.Loc.Map)
                        {
                            if (MyMath.InBox(GC.MyChar.Loc.PreviousX, GC.MyChar.Loc.PreviousY, C.Loc.X, C.Loc.Y, 28)) //&& MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28))
                                                                                                                      //    if (MyMath.PointDistance(GC.MyChar.Loc.PreviousX, GC.MyChar.Loc.PreviousY, C.Loc.X, C.Loc.Y) <= 18)
                            {

                                //Console.WriteLine("Previous location in screen");
                                if (C.EntityID != GC.MyChar.EntityID)
                                {
                                    if (Dir == 8)
                                        C.MyClient.AddSend(Data);
                                    else C.MyClient.AddSend(Packets.Movement(GC.MyChar.EntityID, Dir).Get);
                                    // if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) > 18)//
                                    if (!MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28))
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
                            else if (MyMath.InBox(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y, 28))
                            //else if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) <= 18)
                            {
                                // if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y) >= 18)
                                //  Console.WriteLine("Distance: " + MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, C.Loc.X, C.Loc.Y));
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
                                //C.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, GC.MyChar.Loc.Map, GC.MyChar.Loc.X, GC.MyChar.Loc.Y, 135).Get);
                                C.MyClient.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 0, 0, 0, 135).Get);
                                if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                                {
                                    GC.MyChar.ScreenChars.Remove(C.EntityID);
                                    // GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                    GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                                }
                            }
                            else if (GC.MyChar.ScreenChars.ContainsKey(C.EntityID))
                            {
                                GC.MyChar.ScreenChars.Remove(C.EntityID);
                                //GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, C.Loc.Map, C.Loc.X, C.Loc.Y, 135).Get);
                                GC.MyChar.MyClient.AddSend(Packets.GeneralData(C.EntityID, 0, 0, 0, 135).Get);
                            }
                        }
                }
                foreach (Game.Character CC in GC.MyChar.ScreenChars.Values)
                    if (CC.Loc.Map != GC.MyChar.Loc.Map)
                        if (GC.MyChar.ScreenChars.ContainsKey(CC.EntityID))
                            /*                          {
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
                            GC.MyChar.Tank = true;
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
                if (GC.MyChar.Loc.Map == 1025 && GC.MyChar.Loc.X >= 164 && GC.MyChar.Loc.X <= 169 && GC.MyChar.Loc.Y >= 109 && GC.MyChar.Loc.Y <= 113)
                {
                    //System.Threading.Thread.Sleep(1000);
                    GC.MyChar.Teleport(1502, 16, 80);
                }
                if (GC.MyChar.Loc.Map == 1025 && GC.MyChar.Loc.X >= 112 && GC.MyChar.Loc.X <= 117 && GC.MyChar.Loc.Y >= 159 && GC.MyChar.Loc.Y <= 163)
                {
                    //System.Threading.Thread.Sleep(1000);
                    GC.MyChar.Teleport(1503, 74, 17);
                }
                if (GC.MyChar.Loc.Map == 1502 && GC.MyChar.Loc.X >= 10 && GC.MyChar.Loc.X <= 14 && GC.MyChar.Loc.Y >= 77 && GC.MyChar.Loc.Y <= 79)
                {
                    //System.Threading.Thread.Sleep(1000);
                    GC.MyChar.Teleport(1025, 162, 111);
                }
                if (GC.MyChar.Loc.Map == 1503 && GC.MyChar.Loc.X >= 72 && GC.MyChar.Loc.X <= 74 && GC.MyChar.Loc.Y >= 11 && GC.MyChar.Loc.Y <= 14)
                {
                    //System.Threading.Thread.Sleep(1000);
                    GC.MyChar.Teleport(1025, 114, 157);
                }
                /* if (!GC.MyChar.Transformation.Transformed)
                 {
                     GC.MyChar.Transformation.Dex = 0;
                     GC.MyChar.Transformation.Dist = 0;
                 }*/
            }
        }
    }
}
