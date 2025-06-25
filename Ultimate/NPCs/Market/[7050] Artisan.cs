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
    public class NPC_7050 : NPCBase
    {
        public NPC_7050(Main.GameClient _client)
            : base(_client)
        {
            ID = 7050;
            Face = 28;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("As you know Magic Artisan sucks at upgrading high level gear. So i'm the one in charge of upgrading the ones over 115 he cannot.");
                        AddOption("Great! Just what I needed.", 10);
                        AddOption("Just passing by.", 255);
                        GC.Agreed = false;
                        break;
                    }
                case 10:
                    {
                        AddText("Choose the equipment you want to upgrade.");
                        AddOption("Headgear", 1);
                        AddOption("Necklace/Bag", 2);
                        AddOption("Armor", 3);
                        AddOption("Weapon", 4);
                        AddOption("Shield", 5);
                        AddOption("Ring", 6);
                        AddOption("Boots", 8);
                        break;
                    }
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    {
                        Game.Item I = GC.MyChar.Equips.Get(Convert.ToByte(_linkback));
                        if (I.ID != 0)
                        {
                            byte PrevLevel = I.DBInfo.LevReq;
                            ItemIDManipulation IMan = new ItemIDManipulation(I.ID);
                            IMan.IncreaseLevel();
                            DatabaseItem Di = (DatabaseItem)Database.DatabaseItems[IMan.ToID()];
                            byte NewLevel = Di.LevReq;
                            if (NewLevel > PrevLevel && NewLevel >= 120)
                            {
                                if (GC.MyChar.Level >= NewLevel)
                                {
                                    if (!GC.Agreed)
                                    {
                                        AddText("You'll have to give me one Dragonballs if you want me to upgrade your item! Are you ready?");
                                        AddOption("Upgrade it.", Convert.ToByte(_linkback));
                                        AddOption("Forget it.", 255);
                                        GC.Agreed = true;
                                    }
                                    else
                                    {
                                        GC.Agreed = false;
                                        if (GC.MyChar.InventoryContains(1088000, 1))
                                        {
                                            for (int a = 0; a < 1; a++)
                                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                            GC.MyChar.EquipStats(Convert.ToByte(_linkback), false, false);
                                            I.ID = IMan.ToID();
                                            I.MaxDur = I.DBInfo.Durability;
                                            I.CurDur = I.MaxDur;
                                            if (I.Soc1 == Game.Item.Gem.NoSocket)
                                            {
                                                if (MyMath.ChanceSuccess(DropRates.DBSock1))//change this for 1st sock rate
                                                {
                                                    //I.OpenSocket(GC.MyChar);
                                                    I.Soc1 = Game.Item.Gem.EmptySocket;
                                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "LuckyGuy")).Get);
                                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got 1 socket into his/her item", 2011, 0);
                                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 1 socket from DB upp on " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                    Discord DCord = new Discord();
                                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got first socket into his/her __**" + I.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                                }
                                            }
                                            else if (I.Soc2 == Game.Item.Gem.NoSocket)
                                                if (MyMath.ChanceSuccess(DropRates.DBSock2))//2nd sock rate
                                                {
                                                    //I.OpenSocket(GC.MyChar);
                                                    I.Soc2 = Game.Item.Gem.EmptySocket;
                                                    Game.World.Action(GC.MyChar, (Packets.StringPacket(GC.MyChar.EntityID, StringType.Effect, "LuckyGuy")).Get);
                                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has got second socket into his/her item", 2011, 0);
                                                    Game.World.DebugAdd += GC.MyChar.Name + " has got 2nd socket from DB upp on " + I.DBInfo.Name + " ( " + I.ID + "~" + I.Plus + "~" + I.Bless + "~" + I.Soc1 + "~" + I.Soc2 + "~" + I.Progress + " ) \r\n";
                                                    Discord DCord = new Discord();
                                                    DCord.MesajVer3 = "  __**" + GC.MyChar.Name + "**__ has got second socket into his/her __**" + I.DBInfo.Name + "**__ with Dragonball " + DateTime.Now;
                                                }
                                            GC.MyChar.Equips.Replace(Convert.ToByte(_linkback), I, GC.MyChar);
                                            GC.MyChar.EquipStats(Convert.ToByte(_linkback), true, false);
                                            AddText("Here you are. It's done.");
                                            AddOption("Thanks", 255);
                                        }
                                        else
                                        {
                                            AddText("You don't have one DragonBalls.");
                                            AddOption("I see", 255);
                                        }
                                    }
                                }
                                else
                                {
                                    AddText("You aren't high level enough to wear the item after upgrading.");
                                    AddOption("Alright", 255);
                                }
                            }
                            else
                            {
                                AddText("The item is either not higher level than 115 or it's maxed out.");
                                AddOption("Alright", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have any equipment in that slot.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}