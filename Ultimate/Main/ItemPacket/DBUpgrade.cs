using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class DBUpgrade
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            if (GC.MyChar.LastUpgrade.AddMilliseconds(1000) > DateTime.Now)
                return;
            uint EquipUID = BitConverter.ToUInt32(Data, 4);
            uint MeteorUID = BitConverter.ToUInt32(Data, 8);

            Game.Item Equip = GC.MyChar.FindInvItem(EquipUID);
            Game.Item DragonBall = GC.MyChar.FindInvItem(MeteorUID);

            GC.MyChar.LastUpgrade = DateTime.Now;

            if (Equip.DBInfo.LevReq > 1)
            {
                if (DragonBall.ID == 1088000)
                {
                    Game.ItemIDManipulation E = new Game.ItemIDManipulation(Equip.ID);

                    sbyte Chance = (sbyte)(100 - (Equip.DBInfo.LevReq / 3));
                    byte Quality = (byte)E.Quality;

                    if (Quality == 6)
                        Chance -= 25;
                    else if (Quality == 7)
                        Chance -= 40;
                    else if (Quality == 8)
                        Chance -= 70;

                    if (Quality < 9 && Equip.DBInfo.LevReq > 1 && Equip.ID != 562000 && Equip.ID != 562001)
                    {
                        if (Quality < 5) Quality = 5;
                        E.QualityChange((Game.Item.ItemQuality)(Quality + 1));
                        Chance += 16;
                        //GC.MyChar.RemoveItem(MeteorUID);
                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                        double socketChance1 = DropRates.DBSock1;
                        double socketChance2 = DropRates.DBSock2;
                        //if (GC.MyChar.Level <= 6)
                        //{
                        //    socketChance1 = DropRates.DBSock1 * 0.7;
                        //    socketChance2 = DropRates.DBSock2 * 0.7;
                        //}
                        if (MyMath.ChanceSuccess(Chance))
                        {
                            if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                            {
                                if (MyMath.ChanceSuccess(socketChance1))//change this for 1st sock rate  1.2
                                {
                                    Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", "As a very lucky player, " + GC.MyChar.Name + " has added the first socket to his/her " + Equip.DBInfo.Name + "!", 2011, 0);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from DB upgrade on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                }
                            }
                            else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                            {
                                if (MyMath.ChanceSuccess(socketChance2))//2nd sock rate   0.3
                                {
                                    Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                    GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                    Game.World.SendMsgToAll("SYSTEM", "As a very lucky player, " + GC.MyChar.Name + "  has added the second socket into his/her " + Equip.DBInfo.Name + "!", 2011, 0);
                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from DB upgrade on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                    Discord DCord = new Discord();
                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                }
                            }
                            //if (GC.MyChar.RemoveItem(EquipUID))
                            {
                                Equip.ID = E.ToID();
                                //GC.MyChar.AddItem(Equip);
                                GC.AddSend(Packets.UpdateItem(Equip, 0));
                                GC.LocalMessage(2005, "Item quality improved.");
                            }
                        }
                        else
                        {
                            if (MyMath.ChanceSuccess(60))
                            {
                                if (Equip.Soc1 == Game.Item.Gem.NoSocket)
                                {
                                    if (MyMath.ChanceSuccess(socketChance1))//change this for 1st sock rate  1.2
                                    {
                                        Equip.Soc1 = Game.Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.SendMsgToAll("SYSTEM", "As a very lucky player, " + GC.MyChar.Name + " has added the first socket to his/her " + Equip.DBInfo.Name + "!", 2011, 0);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from DB upgrade failure on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + Equip.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                    }
                                }
                                else if (Equip.Soc2 == Game.Item.Gem.NoSocket)
                                    if (MyMath.ChanceSuccess(socketChance2))//2nd sock rate   0.3
                                    {
                                        Equip.Soc2 = Game.Item.Gem.EmptySocket;
                                        GC.AddSend(Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "congratulate"));
                                        Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, Game.StringType.Effect, "LuckyGuy")).Get);
                                        Game.World.SendMsgToAll("SYSTEM", "As a very lucky player, " + GC.MyChar.Name + "  has added the second socket into his/her " + Equip.DBInfo.Name + "!", 2011, 0);
                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from DB upgrade failure on " + Equip.DBInfo.Name + " ( " + Equip.ID + "~" + Equip.Plus + "~" + Equip.Bless + "~" + Equip.Soc1 + "~" + Equip.Soc2 + "~" + Equip.Progress + " ) \r\n";
                                        Discord DCord = new Discord();
                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + Equip.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                    }
                            }
                            Equip.CurDur = (ushort)(Equip.CurDur * 0.7);
                            //if (GC.MyChar.RemoveItem(EquipUID))
                            {
                                // GC.MyChar.AddItem(ref Equip);
                                GC.AddSend(Packets.UpdateItem(Equip, 0));
                                GC.LocalMessage(2005, "Item upgrade failed.");
                            }
                        }

                    }
                }
            }
        }
    }
}
