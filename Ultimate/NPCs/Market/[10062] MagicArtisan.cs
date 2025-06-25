using Ultimate.Main;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using Ultimate.Game;
using System.Threading;

namespace Ultimate.NPCs
{
    public class NPC_10062 : NPCBase
    {
        public NPC_10062(Main.GameClient _client)
            : base(_client)
        {
            ID = 10062;
            Face = 6;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hi. What i do is upgrade your equipment for meteors. How about it?");
                        AddOption("Upgrade quality", 1);
                        AddOption("Upgrade level", 2);
                        GC.Agreed = false;
                        break;
                    }
                case 1:
                case 2:
                    {
                        AddText("Choose the equipment you want to upgrade.");
                        AddOption("Headgear", (byte)(_linkback * 100 + 1));
                        AddOption("Necklace/Bag", (byte)(_linkback * 100 + 2));
                        AddOption("Armor", (byte)(_linkback * 100 + 3));
                        AddOption("Weapon", (byte)(_linkback * 100 + 4));
                        AddOption("Shield", (byte)(_linkback * 100 + 5));
                        AddOption("Ring", (byte)(_linkback * 100 + 6));
                        AddOption("Boots", (byte)(_linkback * 100 + 8));
                        break;
                    }
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                    {
                        Game.Item I = GC.MyChar.Equips.Get((byte)(_linkback - 100));
                        if (_linkback == 105)
                            if (Game.ItemIDManipulation.Part(I.ID, 0, 3) == 105)
                            {
                                AddText("You don't have a shield/weapon.");
                                AddOption("I see.", 255);
                                break;
                            }
                        if (I.ID != 0)
                        {
                            byte ItemLevel = I.DBInfo.LevReq;
                            Game.ItemIDManipulation IMan = new Ultimate.Game.ItemIDManipulation(I.ID);
                            if (IMan.Quality != Ultimate.Game.Item.ItemQuality.Super && IMan.Quality != Ultimate.Game.Item.ItemQuality.NoUpgrade)
                            {
                                byte DBReq = 2;
                                Game.Item.ItemQuality Q = IMan.Quality;
                                if ((byte)Q < 5) Q = Game.Item.ItemQuality.Normal;
                                if (Q == Ultimate.Game.Item.ItemQuality.Refined) DBReq++;
                                else if (Q == Ultimate.Game.Item.ItemQuality.Unique) DBReq += 2;
                                else if (Q == Ultimate.Game.Item.ItemQuality.Elite) DBReq += 5;
                                DBReq = (byte)(DBReq + (ItemLevel) / 30);
                                if (!GC.Agreed)
                                {
                                    AddText("You need " + DBReq + " Dragonballs to upgrade.");
                                    AddOption("Upgrade it.", Convert.ToByte(_linkback));
                                    AddOption("Forget it.", 255);
                                    GC.Agreed = true;
                                }
                                else
                                {
                                    GC.Agreed = false;
                                    if (GC.MyChar.InventoryContains(1088000, DBReq))
                                    {
                                        for (byte i = 0; i < DBReq; i++)
                                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                        GC.MyChar.EquipStats((byte)(_linkback - 100), false, false);
                                        IMan.QualityChange((Ultimate.Game.Item.ItemQuality)(Q + 1));
                                        I.ID = IMan.ToID();
                                        if (I.Soc1 == Game.Item.Gem.NoSocket)
                                        {
                                            if (MyMath.ChanceSuccess(DropRates.DBSock1))//change this for 1st sock rate
                                            {
                                                I.Soc1 = Game.Item.Gem.EmptySocket;
                                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got 1 socket into his/her item", 2011, 0);
                                                Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from DB upp. on " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                Discord DCord = new Discord();
                                                DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + I.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                            }
                                        }
                                        else if (I.Soc2 == Game.Item.Gem.NoSocket)
                                            if (MyMath.ChanceSuccess(DropRates.DBSock2))//2nd sock rate
                                            {
                                                I.Soc2 = Game.Item.Gem.EmptySocket;
                                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her item", 2011, 0);
                                                Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from DB upp on " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                Discord DCord = new Discord();
                                                DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + I.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                            }
                                        GC.MyChar.Equips.Replace((byte)(_linkback - 100), I, GC.MyChar);
                                        GC.MyChar.EquipStats((byte)(_linkback - 100), true, false);
                                        AddText("Here you are. It's done.");
                                        AddOption("Thanks.", 255);
                                    }
                                    else
                                    {
                                        AddText("You don't have enough Dragonballs.");
                                        AddOption("I see.", 255);
                                    }
                                }
                            }
                            else
                            {
                                AddText("You cannot upgrade an item's quality which is already at maximum.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have any equipment in that slot.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 201:
                case 202:
                case 203:
                case 204:
                case 205:
                case 206:
                case 207:
                case 208:
                    {
                        Game.Item I = GC.MyChar.Equips.Get((byte)(_linkback - 200));
                        if (_linkback == 205)
                            if (Game.ItemIDManipulation.Part(I.ID, 0, 3) == 105)
                            {
                                AddText("You don't have a shield/weapon.");
                                AddOption("I see.", 255);
                                break;
                            }
                        if (I.ID != 0)
                        {
                            byte PrevLevel = I.DBInfo.LevReq;
                            Game.ItemIDManipulation IMan = new Ultimate.Game.ItemIDManipulation(I.ID);
                            IMan.IncreaseLevel();
                            if (!Database.DatabaseItems.ContainsKey(IMan.ToID()))
                                return;
                            DatabaseItem Ii = (DatabaseItem)Database.DatabaseItems[IMan.ToID()];
                            byte NewLevel = Ii.LevReq;
                            if (GC.MyChar.Level >= NewLevel)
                            {
                                if (NewLevel != 0 && NewLevel <= 123 && PrevLevel < 120 && NewLevel >= PrevLevel)
                                {
                                    Game.Item.ItemQuality Q = IMan.Quality;
                                    double DMetsReq = (double)((double)PrevLevel / 130 * 3);
                                    if (Q == Game.Item.ItemQuality.Unique) DMetsReq *= 2;
                                    else if (Q == Game.Item.ItemQuality.Elite) DMetsReq *= 4;
                                    else if (Q == Game.Item.ItemQuality.Super) DMetsReq *= 10;
                                    DMetsReq++;
                                    byte MetsReq = (byte)DMetsReq;
                                    if (!GC.Agreed)
                                    {
                                        AddText("You need " + MetsReq + " Meteors to upgrade.");
                                        AddOption("Upgrade it.", Convert.ToByte(_linkback));
                                        AddOption("Forget it.", 255);
                                        GC.Agreed = true;
                                    }
                                    else
                                    {
                                        GC.Agreed = false;
                                        byte Mets = 0;
                                        byte MetScrolls = 0;
                                        foreach (Item I2 in GC.MyChar.Inventory)
                                        {
                                            if (I2.ID == 1088001)
                                                Mets++;
                                            else if (I2.ID == 720027)
                                                MetScrolls++;
                                        }
                                        int InvC = GC.MyChar.Inventory.Count;
                                        byte MetsRemove = 0;
                                        byte MSRemove = 0;
                                        byte MetsAdd = 0;
                                        if (MetScrolls * 10 + Mets >= MetsReq)
                                        {
                                            while (MetsReq > 0)
                                            {
                                                if (MetsReq >= 10 && MetScrolls > 0)
                                                {
                                                    InvC--;
                                                    MetsReq -= 10;
                                                    MetScrolls--;
                                                    MSRemove++;
                                                }
                                                else if (Mets > 0)
                                                {
                                                    InvC--;
                                                    MetsReq--;
                                                    Mets--;
                                                    MetsRemove++;
                                                }
                                                else
                                                {
                                                    MSRemove++;
                                                    InvC--;
                                                    MetsAdd = (byte)(10 - MetsReq);
                                                    InvC += MetsAdd;
                                                    MetsReq = 0;
                                                }
                                            }
                                            if (InvC <= 40)
                                            {
                                                for (int i = 0; i < MSRemove; i++)
                                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(720027));
                                                int M = MetsRemove - MetsAdd;
                                                if (M > 0)
                                                {
                                                    for (int i = 0; i < M; i++)
                                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                                                }
                                                else if (M < 0)
                                                {
                                                    M = -M;
                                                    for (int i = 0; i < M; i++)
                                                        GC.MyChar.AddItem(1088001);
                                                }
                                                GC.MyChar.EquipStats((byte)(_linkback - 200), false, false);
                                                I.ID = IMan.ToID();
                                                I.MaxDur = I.DBInfo.Durability;
                                                I.CurDur = I.MaxDur;
                                                if (I.Soc1 == Game.Item.Gem.NoSocket)
                                                {
                                                    if (MyMath.ChanceSuccess(DropRates.MeteorSock1))//change this for 1st sock rate
                                                    {
                                                        I.Soc1 = Game.Item.Gem.EmptySocket;
                                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got 1 socket into his/her item", 2011, 0);
                                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from DB upp on: " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                        Discord DCord = new Discord();
                                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + I.DBInfo.Name + "**__ with Meteor " + DateTime.Now;
                                                    }
                                                }
                                                else if (I.Soc2 == Game.Item.Gem.NoSocket)
                                                    if (MyMath.ChanceSuccess(DropRates.MeteorSock2))//2nd sock rate
                                                    {
                                                        I.Soc2 = Game.Item.Gem.EmptySocket;
                                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her item", 2011, 0);
                                                        Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from DB upp on " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                        Discord DCord = new Discord();
                                                        DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + I.DBInfo.Name + "**__ with Meteor " + DateTime.Now;
                                                    }
                                                GC.MyChar.Equips.Replace((byte)(_linkback - 200), I, GC.MyChar);
                                                GC.MyChar.EquipStats((byte)(_linkback - 200), true, false);
                                                AddText("Here you are. It's done.");
                                                AddOption("Thanks.", 255);
                                            }
                                            else
                                            {
                                                AddText("You won't have enough space in inventory after upgrade! Make some space!");
                                                AddOption("No way! Are you really sure?", 255);
                                            }
                                        }
                                        else
                                        {
                                            AddText("You don't have enough Meteors.");
                                            AddOption("No way! Are you really sure?", 255);
                                        }
                                    }
                                }
                                else
                                {
                                    AddText("I'm afraid i can't help you with that. I am not experienced enough with equipment that high level.");
                                    AddOption("You old geezer!", 255);
                                }
                            }
                            else
                            {
                                AddText("You aren't high level enough to wear the item after upgrading.");
                                AddOption("Alright.", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have any equipment in that slot.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}