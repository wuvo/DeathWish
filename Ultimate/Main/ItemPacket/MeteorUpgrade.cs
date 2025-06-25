using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class MeteorUpgrade
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            uint EquipUID = BitConverter.ToUInt32(Data, 4);
            uint MeteorUID = BitConverter.ToUInt32(Data, 8);

            Game.Item Equip = GC.MyChar.FindInvItem(EquipUID);
            Game.Item Meteor = GC.MyChar.FindInvItem(MeteorUID);

            if (GC.MyChar.LastUpgrade.AddMilliseconds(1000) > DateTime.Now)
                return;
            GC.MyChar.LastUpgrade = DateTime.Now;

            double socketChance1 = DropRates.MeteorSock1;
            double socketChance2 = DropRates.MeteorSock2;
            //if (GC.MyChar.Level <= 6)
            //{
            //    socketChance1 = DropRates.MeteorSock1 * 0.7;
            //    socketChance2 = DropRates.MeteorSock2 * 0.7;
            //}

            if (Meteor.ID == 1088001)
            {
                Game.ItemIDManipulation E = new Game.ItemIDManipulation(Equip.ID);

                sbyte Chance = (sbyte)(100 - (Equip.DBInfo.LevReq / 1.3));
                byte Quality = (byte)E.Quality;

                if (Quality < 6)
                    Chance += 10;
                else if (Quality == 7)
                    Chance -= 2;
                else if (Quality == 8)
                    Chance -= 3;
                else if (Quality == 9)
                    Chance -= 5;

                //if (Quality < 7)
                //    Chance += 10;
                //else if (Quality == 7)
                //    Chance += 8;
                //else if (Quality == 8)
                //    Chance += 5;

                E.IncreaseLevel();
                DatabaseItem Di = (DatabaseItem)Database.DatabaseItems[E.ToID()];

                byte NewLevel = Di.LevReq;
                if (NewLevel > Equip.DBInfo.LevReq && Equip.DBInfo.LevReq < 120)
                {
                    if (MyMath.ChanceSuccess(Chance))
                    {
                        // if (GC.MyChar.RemoveItem(EquipUID))
                        {
                            Equip.ID = E.ToID();
                            if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                            {
                                if (MyMath.ChanceSuccess(socketChance1))//change this to the rate for 1st sock 0.14
                                {
                                    //Equip.OpenSocket(GC.MyChar);
                                    Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got first socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket in " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) from Meteor upp. \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with meteor  " + DateTime.Now;
                                }
                            }
                            else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                            {
                                if (MyMath.ChanceSuccess(socketChance2))//change this for the 2nd sock rate 0.1
                                {
                                    //Equip.OpenSocket(GC.MyChar);
                                    Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket in " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) from Meteor upp. \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with meteor " + DateTime.Now;
                                }
                            }
                            Equip.MaxDur = Equip.DBInfo.Durability;
                            Equip.CurDur = Equip.MaxDur;
                            GC.AddSend(Packets.UpdateItem(Equip, 0));
                            // GC.MyChar.AddItem(Equip);
                            GC.LocalMessage(2005, "Item level improved.");
                        }
                    }
                    else
                    {
                        // if (GC.MyChar.RemoveItem(EquipUID))
                        {
                            Equip.CurDur = (ushort)(Equip.CurDur * 0.7);

                            if (MyMath.ChanceSuccess(50))
                            {
                                if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                                {
                                    if (MyMath.ChanceSuccess(socketChance1))//change this to the rate for 1st sock
                                    {
                                        //Equip.OpenSocket(GC.MyChar);
                                        Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got first socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from Meteor upp on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with meteor " + DateTime.Now;
                                    }
                                }
                                else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                                {
                                    if (MyMath.ChanceSuccess(socketChance2))//change this for the 2nd sock rate
                                    {
                                        //Equip.OpenSocket(GC.MyChar);
                                        Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from Meteor upp on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with meteor " + DateTime.Now;
                                    }
                                }
                            }
                            // GC.MyChar.AddItem(Equip);
                            GC.AddSend(Packets.UpdateItem(Equip, 0));
                            GC.LocalMessage(2005, "Item upgrade failed.");
                        }
                    }
                    /* if (!Chance.ToString().Contains('-'))
                     {
                         GC.MyChar.RemoveItem(MeteorUID);
                     }*/
                    if (Chance > 0)
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));// GC.MyChar.RemoveItem(MeteorUID);
                }
                else
                {
                    GC.LocalMessage(2005, "The item cannot be upgraded anymore here!");
                }
            }
            if (Meteor.ID == 1088002)
            {
                Game.ItemIDManipulation E = new Game.ItemIDManipulation(Equip.ID);

                sbyte Chance = (sbyte)(100 - (Equip.DBInfo.LevReq / 1.3));
                byte Quality = (byte)E.Quality;

                //if (Quality < 6)
                //    Chance += 10;
                /*  if (Quality == 7)
                      Chance -= 5;
                  if (Quality == 8)
                      Chance -= 20;
                  if (Quality == 9)
                      Chance -= 30;*/
                if (Quality < 7)
                    Chance += 15;
                else if (Quality == 7)
                    Chance += 10;
                else if (Quality == 8)
                    Chance += 5;
                else if (Quality == 9)
                    Chance += 3;

                E.IncreaseLevel();
                DatabaseItem Di = (DatabaseItem)Database.DatabaseItems[E.ToID()];
                byte NewLevel = Di.LevReq;
                if (Chance > 0)
                {
                    if (NewLevel > Equip.DBInfo.LevReq && Equip.DBInfo.LevReq < 120)
                    {
                        if (MyMath.ChanceSuccess(Chance))
                        {
                            if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                            {
                                if (MyMath.ChanceSuccess(socketChance1))//change this for 1st sock rate
                                {
                                    //Equip.OpenSocket(GC.MyChar);
                                    Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got 1 socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from MeteorTear upp on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with MeteorTear " + DateTime.Now;
                                }
                            }
                            else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                                if (MyMath.ChanceSuccess(socketChance2))//2nd sock rate
                                {
                                    //Equip.OpenSocket(GC.MyChar);
                                    Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from MeteorTear upp on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with MeteorTear " + DateTime.Now;
                                }
                            // GC.MyChar.RemoveItem(Equip.UID);
                            Equip.ID = E.ToID();
                            Equip.MaxDur = Equip.DBInfo.Durability;
                            Equip.CurDur = Equip.MaxDur;
                            // GC.MyChar.AddItem(ref Equip);
                            GC.AddSend(Packets.UpdateItem(Equip, 0));
                            GC.LocalMessage(2005, "Item level improved.");
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(50))
                            {
                                if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                                {
                                    if (MyMath.ChanceSuccess(socketChance1))//change this for 1st sock rate
                                    {
                                        //Equip.OpenSocket(GC.MyChar);
                                        Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got 1 socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from MeteorTear upp. \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with MeteorTear " + DateTime.Now;
                                    }
                                }
                                else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                                    if (MyMath.ChanceSuccess(socketChance2))//2nd sock rate
                                    {
                                        //Equip.OpenSocket(GC.MyChar);
                                        Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her " + Equip.DBInfo.Name, 2011, 0);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from MeteorTear upp. \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with MeteorTear " + DateTime.Now;
                                    }
                            }
                            Equip.CurDur = (ushort)(Equip.CurDur * 0.7);
                            GC.AddSend(Packets.UpdateItem(Equip, 0));
                            GC.LocalMessage(2005, "Item upgrade failed.");
                        }

                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088002)); // GC.MyChar.RemoveItem(MeteorUID);
                    }
                }
            }
        }
    }
}
