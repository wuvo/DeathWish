using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Ultimate.Game;

namespace Ultimate.PacketHandling
{
    public class PickItemUp
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint IUID = BitConverter.ToUInt32(Data, 4);

            if (((ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[GC.MyChar.Loc.Map]).ContainsKey(IUID) && GC.MyChar.MyShop == null)
            {
                Game.DroppedItem DI = (Game.DroppedItem)((ConcurrentDictionary<uint, Game.DroppedItem>)Game.World.H_Items[GC.MyChar.Loc.Map])[IUID];

                if (DI.Info.ID != 0)
                {
                    if (DI.Owner != 0 && DI.Silvers > 0)
                    {
                        if (!World.GoldSource.ContainsKey("MonsterPickUp"))
                            World.GoldSource.Add("MonsterPickUp", 0);
                        World.GoldSource["MonsterPickUp"] += DI.Silvers;
                    }
                    if (GC.MyChar.MyTeam != null)
                    {
                        if (Game.World.H_Chars.ContainsKey(DI.Owner))
                        {
                            Game.Character Owner = Game.World.H_Chars[DI.Owner];
                            if (DI.Silvers > 0)
                            {
                                if ((GC.MyChar.MyTeam.Money && GC.MyChar.MyTeam.Members.Contains(Owner) && DI.Owner == Owner.EntityID) || DI.Owner == GC.MyChar.EntityID || DateTime.Now > DI.DropTime.AddSeconds(20) || DI.Owner == 0)
                                {
                                    if (GC.MyChar.Silvers + DI.Silvers <= 2000000000)
                                    {
                                        if (DI.Owner == GC.MyChar.EntityID || DI.Owner == 0 || DateTime.Now > DI.DropTime.AddSeconds(20))
                                        {

                                            GC.MyChar.Silvers += DI.Silvers;
                                            DI.Dissappear();
                                            GC.LocalMessage(2005, "You have picked up " + DI.Silvers + " Silvers.");
                                            Game.World.DropAdd += GC.MyChar.Name + " has picked up silvers: " + DI.Silvers + "\r\n";
                                            GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, DI.Silvers, 0, 0, 121));

                                        }
                                        else
                                        {
                                            if (DI.Silvers >= 10)
                                            {
                                                GC.MyChar.Silvers += (uint)(DI.Silvers * 0.9);
                                                GC.MyChar.MyTeam.Leader.Silvers += (uint)(DI.Silvers * 0.1);
                                                GC.LocalMessage(2005, "You have picked up " + (uint)(DI.Silvers * 0.9) + " Silvers.");
                                                if (DI.Silvers >= 10000)
                                                    Game.World.DropAdd += GC.MyChar.Name + " has picked up silvers: " + DI.Silvers + "\r\n";
                                                GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, DI.Silvers, 0, 0, 121));
                                                GC.MyChar.MyTeam.Leader.MyClient.LocalMessage(2005, "You received " + (uint)(DI.Silvers * 0.1) + " silvers from teammate gold pick-up.");
                                            }
                                            else
                                            {
                                                GC.MyChar.Silvers += DI.Silvers;
                                                if (DI.Silvers >= 10000)
                                                    Game.World.DropAdd += GC.MyChar.Name + " has picked up silvers: " + DI.Silvers + "\r\n";
                                                GC.LocalMessage(2005, "You have picked up " + DI.Silvers + " Silvers.");
                                            }
                                            DI.Dissappear();

                                        }
                                    }
                                    else
                                    {
                                        GC.LocalMessage(2005, "You can't have more than 2kkk in your inventory!");
                                    }
                                }
                                else if (DI.Silvers > 0)
                                    GC.LocalMessage(2005, "You have to wait a while before picking up items dropped by monsters killed by other players.");
                            }





                            if (DI.Info != null && DI.Silvers == 0 && (((GC.MyChar.MyTeam.Items && GC.MyChar.MyTeam.Members.Contains(Owner) && DI.Owner == Owner.EntityID && DI.Info.ID != 1088000 && DI.Info.ID != 1088001) || DI.Owner == GC.MyChar.EntityID) || DateTime.Now > DI.DropTime.AddSeconds(20) || DI.Owner == 0))
                            {
                                if (DI.Info.ID != 722741 && DI.Info.ID != 710100 && DI.Info.ID != 710103)
                                {
                                    if (GC.MyChar.Inventory.Count < 40)
                                    {
                                        GC.MyChar.AddItem(DI.Info);
                                        DI.Dissappear();

                                        try
                                        {
                                            foreach (MapEffect I in Mob.DropsEffects.Values.ToList())
                                            {
                                                if (I.Loc.X == DI.Loc.X && I.Loc.Y == DI.Loc.Y)
                                                {
                                                    I.Dissappear();
                                                    Mob.DropsEffects.Remove(I.UID);
                                                }
                                            }
                                            GC.LocalMessage(2005, "You have picked up a(n) " + DI.Info.DBInfo.Name + ".");
                                            if (DI.Info.IsWorth())
                                                Game.World.DropAdd += GC.MyChar.Name + " has picked up " + DI.UID + "~" + DI.Info.ID + "~" + DI.Info.Plus + "~" + DI.Info.Bless + "~" + DI.Info.Enchant + "~" + (byte)DI.Info.Soc1 + "~" + (byte)DI.Info.Soc2 + "~" + DI.Info.Progress + " Map " + GC.MyChar.Loc.Map + " X " +GC.MyChar.Loc.X + " Y " + GC.MyChar.Loc.Y + " : " + DateTime.Now + "\r\n";
                                        }
                                        catch
                                        {
                                            Game.World.ExcAdd += "Item ID fail: " + DI.Info.ID + "\r\n";
                                        }
                                    }
                                    else
                                        GC.LocalMessage(2005, "Your inventory is full.");
                                }
                                else if (DI.Info.ID == 710100 || DI.Info.ID == 722741)
                                {
                                    if (DI.Info.ID == 710100)
                                    {
                                        if (GC.MyChar.RedTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the RedBag! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }

                                    if (DI.Info.ID == 710103)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }

                                    else if (DI.Info.ID == 722741)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the BlueBag! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                    }
                                }


                            }
                            else if (DI.Info != null && DI.Silvers == 0)
                                GC.LocalMessage(2005, "You have to wait a while before picking up items dropped by monsters killed by other players.");
                        }
                        else
                        {
                            if (DI.Silvers > 0)
                            {
                                if (GC.MyChar.Silvers + DI.Silvers <= 2000000000)
                                {
                                    GC.MyChar.Silvers += DI.Silvers;
                                    DI.Dissappear();
                                    GC.LocalMessage(2005, "You have picked up " + DI.Silvers + " Silvers.");
                                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, DI.Silvers, 0, 0, 121));
                                    if (DI.Silvers >= 10000)
                                        Game.World.DropAdd += GC.MyChar.Name + " has picked up silvers: " + DI.Silvers + "\r\n";
                                }
                                else
                                {
                                    GC.LocalMessage(2005, "You can't have more than 2kkk in your inventory!");
                                }
                            }
                            else if (DI.Info != null)
                            {
                                if (DI.Info.ID != 722741 && DI.Info.ID != 710100 && DI.Info.ID != 710103)
                                {
                                    if (GC.MyChar.Inventory.Count < 40)
                                    {
                                        GC.MyChar.AddItem(DI.Info);
                                        DI.Dissappear();

                                        try
                                        {
                                            foreach (MapEffect I in Mob.DropsEffects.Values.ToList())
                                            {
                                                if (I.Loc.X == DI.Loc.X && I.Loc.Y == DI.Loc.Y)
                                                {
                                                    I.Dissappear();
                                                    Mob.DropsEffects.Remove(I.UID);
                                                }
                                            }
                                            GC.LocalMessage(2005, "You have picked up a(n) " + DI.Info.DBInfo.Name + ".");
                                            if (DI.Info.IsWorth())
                                                Game.World.DropAdd += GC.MyChar.Name + " has picked up " + DI.UID + "~" + DI.Info.ID + "~" + DI.Info.Plus + "~" + DI.Info.Bless + "~" + DI.Info.Enchant + "~" + (byte)DI.Info.Soc1 + "~" + (byte)DI.Info.Soc2 + "~" + DI.Info.Progress + " Map " + GC.MyChar.Loc.Map + " X " + GC.MyChar.Loc.X + " Y " + GC.MyChar.Loc.Y + " : " + DateTime.Now + "\r\n";
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                Game.World.ExcAdd += "Item pickup fail: " + DI.Info.ID + "\r\n";
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                    else
                                        GC.LocalMessage(2005, "Your inventory is full.");
                                }
                                else if (DI.Info.ID == 710100 || DI.Info.ID == 722741 || DI.Info.ID == 710103)
                                {
                                    if (DI.Info.ID == 710100)
                                    {
                                        if (GC.MyChar.RedTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the RedBag! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }

                                    if (DI.Info.ID == 710103)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }

                                    else if (DI.Info.ID == 722741)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the BlueBag! Be careful!", Events.BroadCastLoc.Map);
                                            DI.Dissappear();
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (DI.Owner == GC.MyChar.EntityID || DI.Owner == 0 || DateTime.Now > DI.DropTime.AddSeconds(20))
                        {
                            if (DI.Silvers > 0)
                            {
                                if (GC.MyChar.Silvers + DI.Silvers <= 2000000000)
                                {
                                    GC.MyChar.Silvers += DI.Silvers;
                                    DI.Dissappear();
                                    GC.LocalMessage(2005, "You have picked up " + DI.Silvers + " Silvers.");
                                    GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, DI.Silvers, 0, 0, 121));
                                    if (DI.Silvers >= 10000)
                                        Game.World.DropAdd += GC.MyChar.Name + " has picked up silvers: " + DI.Silvers + "\r\n";
                                }
                                else
                                {
                                    GC.LocalMessage(2005, "You can't have more than 2kkk in your inventory!");
                                }
                            }
                            else if (DI.Info != null)
                            {
                                if (DI.Info.ID != 722741 && DI.Info.ID != 710100 && DI.Info.ID != 710103)
                                {
                                    if (GC.MyChar.Inventory.Count < 40)
                                    {
                                        GC.MyChar.AddItem(DI.Info);
                                        DI.Dissappear();

                                        try
                                        {
                                            foreach (MapEffect I in Mob.DropsEffects.Values.ToList())
                                            {
                                                if (I.Loc.X == DI.Loc.X && I.Loc.Y == DI.Loc.Y)
                                                {
                                                    I.Dissappear();
                                                    Mob.DropsEffects.Remove(I.UID);
                                                }
                                            }
                                            GC.LocalMessage(2005, "You have picked up a(n) " + DI.Info.DBInfo.Name + ".");
                                            if (DI.Info.IsWorth())
                                                Game.World.DropAdd += GC.MyChar.Name + " has picked up " + DI.UID + "~" + DI.Info.ID + "~" + DI.Info.Plus + "~" + DI.Info.Bless + "~" + DI.Info.Enchant + "~" + (byte)DI.Info.Soc1 + "~" + (byte)DI.Info.Soc2 + "~" + DI.Info.Progress + " Map " + GC.MyChar.Loc.Map + " X " + GC.MyChar.Loc.X + " Y " + GC.MyChar.Loc.Y + " : " + DateTime.Now + "\r\n";
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                Game.World.ExcAdd += "Item pickup fail: " + DI.Info.ID + "\r\n";
                                            }
                                            catch
                                            { }
                                        }
                                    }
                                    else
                                        GC.LocalMessage(2005, "Your inventory is full.");
                                }
                                else if (DI.Info.ID == 710100 || DI.Info.ID == 722741 || DI.Info.ID == 710103)
                                {
                                    if (DI.Info.ID == 710100)
                                    {
                                        if (GC.MyChar.RedTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the RedBag! Be careful!", Events.BroadCastLoc.Map);
                                            try
                                            {
                                                DI.Dissappear();
                                            }
                                            catch
                                            {
                                                DI.Dissappear();
                                            }
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }
                                    if (DI.Info.ID == 710103)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            try
                                            {
                                                DI.Dissappear();
                                            }
                                            catch
                                            {
                                                DI.Dissappear();
                                            }
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                            //GC.MyChar.StatEff.Add(Game.StatusEffectEn.Cursed);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the BlueTeam has picked up the Ball! Be careful!", Events.BroadCastLoc.Map);
                                            try
                                            {
                                                DI.Dissappear();
                                            }
                                            catch
                                            {
                                                DI.Dissappear();
                                            }
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Red = false;
                                            Events.Football.Red = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                        }
                                    }
                                    else if (DI.Info.ID == 722741)
                                    {
                                        if (GC.MyChar.RedTeam)
                                        {
                                            GC.MyChar.EventBase.Broadcast(GC.MyChar.Name + " from the RedTeam has picked up the BlueBag! Be careful!", Events.BroadCastLoc.Map);
                                            try
                                            {
                                                DI.Dissappear();
                                            }
                                            catch
                                            {
                                                DI.Dissappear();
                                            }
                                            GC.MyChar.HasBag = true;
                                            Events.CaptureTheBag.Blue = false;
                                            Events.Football.Blue = false;
                                            GC.MyChar.StatEff.Add(Game.StatusEffectEn.Flashy);
                                            //GC.MyChar.StatEff.Add(Game.StatusEffectEn.Cursed);
                                        }
                                        else if (GC.MyChar.BlueTeam)
                                            GC.LocalMessage(2021, "You can't pick up the bag of your own team!");
                                    }
                                }
                            }
                        }
                        else
                            GC.LocalMessage(2005, "You have to wait a while before picking up items dropped by monsters killed by other players.");
                    }
                }
                else
                    GC.LocalMessage(2005, "The item you are trying to pick up doesn't exist!");
            }
        }
    }
}
