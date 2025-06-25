using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling
{
    public class Portal
    {
        public static void Handle(Main.GameClient GC, byte[] Data, bool RangeSmall = false)
        {
            uint NewMap = GC.MyChar.Loc.Map;
            ushort NewX = (ushort)(GC.MyChar.Loc.X + 3);
            ushort NewY = (ushort)(GC.MyChar.Loc.Y - 2);
            byte Range = 4;
            if (RangeSmall) Range = 2;
            foreach (Dbase.Portal P in Game.World.Portals)
            {
                if (GC.MyChar.Loc.Map >= 10000)
                {
                    if (P.PortalMapID == 1098 && DMaps.HouseLevel(GC.MyChar.Loc.Map) == 1)
                    {
                        if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, P.PortalX, P.PortalY) <= Range)
                        {
                            NewMap = P.DestinationMapID;
                            NewX = (ushort)P.DestinationX;
                            NewY = (ushort)P.DestinationY;
                            GC.MyChar.Teleport(NewMap, NewX, NewY);
                            break;
                        }
                    }
                    else if (P.PortalMapID == 1099 && DMaps.HouseLevel(GC.MyChar.Loc.Map) == 2)
                    {
                        if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, P.PortalX, P.PortalY) <= Range)
                        {
                            NewMap = P.DestinationMapID;
                            NewX = (ushort)P.DestinationX;
                            NewY = (ushort)P.DestinationY;
                            GC.MyChar.Teleport(NewMap, NewX, NewY);
                            break;
                        }
                    }
                }
                if (GC.MyChar.Loc.Map == 10200)
                {
                    NewMap = 1002;
                    NewX = 359;
                    NewY = 336;
                    GC.MyChar.Teleport(NewMap, NewX, NewY);
                    break;
                }
                else if (DMaps.EventMaps.ContainsKey(GC.MyChar.Loc.Map))
                {
                    if (GC.MyChar.Loc.Map == 8000)
                    {
                        if (P.PortalMapID == (ushort)(DMaps.EventMaps[GC.MyChar.Loc.Map]))
                        {
                            if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, P.PortalX, P.PortalY) <= Range)
                            {
                                NewMap = P.DestinationMapID;
                                NewX = (ushort)P.DestinationX;
                                NewY = (ushort)P.DestinationY;
                                GC.MyChar.Teleport(NewMap, NewX, NewY);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (P.PortalMapID == GC.MyChar.Loc.Map)
                        {
                            if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, P.PortalX, P.PortalY) <= Range)
                            {
                                NewMap = P.DestinationMapID;
                                NewX = (ushort)P.DestinationX;
                                NewY = (ushort)P.DestinationY;
                                GC.MyChar.Teleport(NewMap, NewX, NewY);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (P.PortalMapID == GC.MyChar.Loc.Map)
                    {
                        if (MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, P.PortalX, P.PortalY) <= Range)
                        {
                            NewMap = P.DestinationMapID;
                            NewX = (ushort)P.DestinationX;
                            NewY = (ushort)P.DestinationY;
                            GC.MyChar.Teleport(NewMap, NewX, NewY);
                            break;
                        }
                    }
                }
            }
        }
    }
}