using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    class Revive
    {
        public static void Handle(Main.GameClient GC)
        {

            if (!GC.MyChar.Alive)
            {
                if (DateTime.Now > GC.MyChar.DeathHit.AddSeconds(20))
                {
                    GC.MyChar.Action = 100;
                    GC.MyChar.Stamina = 100;
                    GC.MyChar.Ghost = false;
                    GC.MyChar.BlueName = false;
                    GC.MyChar.CurHP = GC.MyChar.MaxHP;
                    if (GC.MyChar.MaxMP > 1)
                        GC.MyChar.CurMP = GC.MyChar.MaxMP;
                    GC.MyChar.Alive = true;
                    GC.MyChar.StatEff.Remove(StatusEffectEn.Dead);
                    GC.MyChar.StatEff.Remove(StatusEffectEn.BlueName);
                    GC.MyChar.Body = GC.MyChar.Body;
                    GC.MyChar.Hair = GC.MyChar.Hair;
                    GC.MyChar.XPKO = 0;
                    GC.MyChar.Equips.Send(GC, false);

                    if (GC.MyChar.Loc.Map == 1038 && Features.GuildWars.War)
                        GC.MyChar.Teleport(6001, 32, 72);

                    else if (GC.MyChar.Loc.Map == 1844)
                    {
                        if (Features.CounterClock.War)
                        {
                            ushort Y = 162;
                            int x = Program.Rnd.Next(0, 3);
                            if (x == 0)
                                Y = (ushort)(124 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));
                            else if (x == 1)
                                Y = (ushort)(162 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));
                            else if (x == 2)
                                Y = (ushort)(203 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));

                            GC.MyChar.Teleport(1844, (ushort)(225 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8)), Y);
                        }

                    }

                    else
                    {
                        if (GC.MyChar.PKPoints >= 100 && !Game.World.FreePKMaps.Contains(GC.MyChar.Loc.Map) && !Game.World.EventsMaps.Contains(GC.MyChar.Loc.Map) && GC.MyChar.Loc.Map < 8000)
                            GC.MyChar.Teleport(6000, 32, 72);
                        else
                        {
                            foreach (uint[] Point in Database.RevPoints)
                                if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                                {
                                    if (GC.MyChar.Loc.Map == 8000)
                                    {
                                        if (Point[0] == (ushort)(DMaps.EventMaps[GC.MyChar.Loc.Map]))
                                        {
                                            GC.MyChar.Teleport(8000, (ushort)Point[2], (ushort)Point[3]);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        GC.MyChar.Teleport(1002, 430, 378);
                                    }
                                }
                                else
                                {
                                    if (Point[0] == GC.MyChar.Loc.Map)
                                    {
                                        GC.MyChar.Teleport(Point[1], (ushort)Point[2], (ushort)Point[3]);
                                        break;
                                    }
                                }
                        }
                    }
                    GC.MyChar.CancelProtectTime = false;
                    GC.MyChar.ProtectTime = DateTime.Now.AddSeconds(0);
                }
            }
        }
    }
}
