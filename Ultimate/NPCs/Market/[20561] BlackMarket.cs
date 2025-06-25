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
    public class NPC_202010 : NPCBase
    {
        public NPC_202010(Main.GameClient _client)
            : base(_client)
        {
            ID = 202010;
            Face = 13;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Main.CryptoRandom Rnd = new Main.CryptoRandom();
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello This is the Newest System of UltimateConquer Online,");
                        AddText("Weapons Black Market. Don't ask where the goods come from. No refunds.");
                        AddText("(NOTE) you will get +2 or +3 weapon if you are lucky enough for it and You get a random item based on how much you spend.");
                        AddOption("Spend 1 LotteryTicket", 1);
                        AddOption("Spend 5 LotteryTicket", 2);

                        AddOption("Spend 2,000,000 Money", 3);
                        AddOption("Spend 10,000,000 Money", 4);

                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.InventoryContains(710212, 1) && GC.MyChar.Inventory.Count <= 39)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710212));

                        top:
                            Item I2 = new Item();
                            I2.UID = (uint)Rnd.Next(10000000);
                            Item.ItemQuality Q = Item.ItemQuality.Simple;


                            uint ItemID = 0;
                            ArrayList From = new ArrayList();
                            int Type = Rnd.Next(0, 180);
                            uint Part = 0;
                            if (Type < 11) Part = 410;
                            else if (Type < 22) Part = 420;
                            else if (Type < 33) Part = 421;
                            else if (Type < 44) Part = 430;
                            else if (Type < 55) Part = 440;
                            else if (Type < 66) Part = 450;
                            else if (Type < 77) Part = 460;
                            else if (Type < 88) Part = 480;
                            else if (Type < 99) Part = 481;
                            else if (Type < 101) Part = 490;
                            else if (Type < 111) Part = 500;
                            else if (Type < 121) Part = 510;
                            else if (Type < 131) Part = 530;
                            else if (Type < 141) Part = 540;
                            else if (Type < 151) Part = 560;
                            else if (Type < 161) Part = 561;
                            else Part = 580;

                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {
                                if (D.LevReq >= 5 && D.LevReq <= 100)
                                {
                                    if (D.LevReq != 0)
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                            From.Add(D.ID);
                                }
                            }
                            if (From != null)
                            {
                                if (From.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    ItemID = (uint)From[Tries];
                                }
                            }
                            if (ItemID != 0)
                            {
                                I2.ID = ItemID;
                                if (I2.DBInfo.LevReq != 1)
                                {
                                    if (MyMath.ChanceSuccess(0.1))
                                    {
                                        Q = Item.ItemQuality.Super;
                                    }
                                    else if (MyMath.ChanceSuccess(5))
                                    {
                                        Q = Item.ItemQuality.Elite;
                                    }
                                    else if (MyMath.ChanceSuccess(10))
                                    {
                                        Q = Item.ItemQuality.Unique;
                                    }
                                    else if (MyMath.ChanceSuccess(7))
                                    {
                                        Q = Item.ItemQuality.Refined;
                                    }
                                    ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                    E.QualityChange(Q);
                                    I2.ID = E.ToID();
                                }

                                I2.Color = Item.ArmorColor.Orange;

                                I2.Plus = 0;

                                if (MyMath.ChanceSuccess(05))
                                {
                                    if (MyMath.ChanceSuccess(0.1))
                                        I2.Plus = 5;

                                    else if (MyMath.ChanceSuccess(0.2))
                                        I2.Plus = 4;
                                    else if (MyMath.ChanceSuccess(3))
                                        I2.Plus = 3;
                                    else if (MyMath.ChanceSuccess(2))
                                        I2.Plus = 2;
                                    else if (MyMath.ChanceSuccess(20))
                                        I2.Plus = 1;
                                }
                                else if (MyMath.ChanceSuccess(40))
                                    I2.Plus = 1;

                                if (MyMath.ChanceSuccess(1))
                                    I2.Bless = 5;
                                else if (MyMath.ChanceSuccess(3.5))
                                    I2.Bless = 3;
                                else if (MyMath.ChanceSuccess(7))
                                    I2.Bless = 1;

                                if (MyMath.ChanceSuccess(20))
                                {
                                    I2.Soc1 = Item.Gem.EmptySocket;
                                    I2.Soc2 = Item.Gem.EmptySocket;
                                }
                                else if (MyMath.ChanceSuccess(45))
                                    I2.Soc1 = Item.Gem.EmptySocket;

                                if (I2.Soc1 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }

                                }

                                if (I2.Soc2 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (MyMath.ChanceSuccess(1))
                                    I2.Enchant = 255;
                                else if (MyMath.ChanceSuccess(5))
                                    I2.Enchant = 200;
                                else if (MyMath.ChanceSuccess(10))
                                    I2.Enchant = 145;
                                else if (MyMath.ChanceSuccess(25))
                                    I2.Enchant = 86;
                                else if (MyMath.ChanceSuccess(45))
                                    I2.Enchant = 56;
                                else
                                    I2.Enchant = 25;

                                I2.MaxDur = I2.DBInfo.Durability;
                                I2.CurDur = I2.MaxDur;

                                GC.MyChar.AddItem(I2);
                                if (I2.Plus >= 5)
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market 1 LotteryTicket!", 2005, 0);
                                Game.World.DebugAdd += GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 200,000 Silvers And 1 LotteryTicket \r\n";
                                AddText("You received a +" + I2.Plus + " Weapon for 1 LotteryTicket!");
                                AddOption("Ok.", 255);

                            }
                            else goto top;
                        }
                        else
                        {
                            AddText("You don't have 1 LotteryTicket, or your inventory is full. You need 1 LotteryTicket and 1 empty spot in your bag.");
                            AddOption("I see...", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(710212, 5) && GC.MyChar.Inventory.Count <= 39)
                        {
                            for (int a = 0; a < 5; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710212));

                            top:
                            Item I2 = new Item();
                            I2.UID = (uint)Rnd.Next(10000000);
                            Item.ItemQuality Q = Item.ItemQuality.Simple;


                            uint ItemID = 0;
                            ArrayList From = new ArrayList();
                            int Type = Rnd.Next(0, 180);
                            uint Part = 0;
                            if (Type < 11) Part = 410;
                            else if (Type < 22) Part = 420;
                            else if (Type < 33) Part = 421;
                            else if (Type < 44) Part = 430;
                            else if (Type < 55) Part = 440;
                            else if (Type < 66) Part = 450;
                            else if (Type < 77) Part = 460;
                            else if (Type < 88) Part = 480;
                            else if (Type < 99) Part = 481;
                            else if (Type < 101) Part = 490;
                            else if (Type < 111) Part = 500;
                            else if (Type < 121) Part = 510;
                            else if (Type < 131) Part = 530;
                            else if (Type < 141) Part = 540;
                            else if (Type < 151) Part = 560;
                            else if (Type < 161) Part = 561;
                            else Part = 580;

                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {
                                if (D.LevReq >= 5 && D.LevReq <= 100)
                                {
                                    if (D.LevReq != 0)
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                            From.Add(D.ID);
                                }
                            }
                            if (From != null)
                            {
                                if (From.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    ItemID = (uint)From[Tries];
                                }
                            }
                            if (ItemID != 0)
                            {
                                I2.ID = ItemID;
                                if (I2.DBInfo.LevReq != 1)
                                {
                                    if (MyMath.ChanceSuccess(0.2))
                                    {
                                        Q = Item.ItemQuality.Super;
                                    }
                                    else if (MyMath.ChanceSuccess(20))
                                    {
                                        Q = Item.ItemQuality.Elite;
                                    }
                                    else if (MyMath.ChanceSuccess(40))
                                    {
                                        Q = Item.ItemQuality.Unique;
                                    }
                                    else
                                    {
                                        Q = Item.ItemQuality.Refined;
                                    }
                                    ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                    E.QualityChange(Q);
                                    I2.ID = E.ToID();
                                }

                                I2.Color = Item.ArmorColor.Orange;

                                I2.Plus = 0;

                                if (MyMath.ChanceSuccess(0.1))
                                {
                                    if (MyMath.ChanceSuccess(0.05))

                                        I2.Plus = 5;
                                    else if (MyMath.ChanceSuccess(0.1))
                                        I2.Plus = 4;
                                    else if (MyMath.ChanceSuccess(0.5))
                                        I2.Plus = 3;
                                    else if (MyMath.ChanceSuccess(0.2))
                                        I2.Plus = 3;
                                    else
                                        I2.Plus = 2;
                                }
                                else
                                    I2.Plus = 2;


                                if (MyMath.ChanceSuccess(5))
                                    I2.Bless = 5;
                                else if (MyMath.ChanceSuccess(10))
                                    I2.Bless = 3;
                                else if (MyMath.ChanceSuccess(17.5))
                                    I2.Bless = 1;

                                if (MyMath.ChanceSuccess(65))
                                {
                                    I2.Soc1 = Item.Gem.EmptySocket;
                                    I2.Soc2 = Item.Gem.EmptySocket;
                                }
                                else
                                    I2.Soc1 = Item.Gem.EmptySocket;

                                if (I2.Soc1 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (I2.Soc2 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (MyMath.ChanceSuccess(20))
                                    I2.Enchant = 255;
                                else if (MyMath.ChanceSuccess(25))
                                    I2.Enchant = 200;
                                else if (MyMath.ChanceSuccess(35))
                                    I2.Enchant = 145;
                                else if (MyMath.ChanceSuccess(51))
                                    I2.Enchant = 86;
                                else if (MyMath.ChanceSuccess(75))
                                    I2.Enchant = 56;
                                else
                                    I2.Enchant = 25;

                                I2.MaxDur = I2.DBInfo.Durability;
                                I2.CurDur = I2.MaxDur;

                                GC.MyChar.AddItem(I2);
                                if (I2.Plus >= 5)
                                {
                                    if (I2.Plus >= 7)
                                    {
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 5 LotteryTicket,!", 2011, 0);
                                    }
                                    else
                                    {
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 5 LotteryTicket!", 2005, 0);
                                    }
                                }
                                Game.World.DebugAdd += GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 5 LotteryTicket \r\n";
                                AddText("You received a +" + I2.Plus + " Weapon for 5 LotteryTicket!");
                                AddOption("Ok.", 255);

                            }
                            else goto top;
                        }
                        else
                        {
                            AddText("You don't have 5 LotteryTicket, or your inventory is full. You need 5 LotteryTicket and 1 empty spot in your bag.");
                            AddOption("I see...", 255);
                        }
                        break;
                    }


                case 3:
                    {
                        if (GC.MyChar.Silvers >= 2000000 && GC.MyChar.Inventory.Count <= 39)
                        {
                            GC.MyChar.Silvers -= 2000000;

                        top:
                            Item I2 = new Item();
                            I2.UID = (uint)Rnd.Next(10000000);
                            Item.ItemQuality Q = Item.ItemQuality.Simple;


                            uint ItemID = 0;
                            ArrayList From = new ArrayList();
                            int Type = Rnd.Next(0, 180);
                            uint Part = 0;
                            if (Type < 11) Part = 410;
                            else if (Type < 22) Part = 420;
                            else if (Type < 33) Part = 421;
                            else if (Type < 44) Part = 430;
                            else if (Type < 55) Part = 440;
                            else if (Type < 66) Part = 450;
                            else if (Type < 77) Part = 460;
                            else if (Type < 88) Part = 480;
                            else if (Type < 99) Part = 481;
                            else if (Type < 101) Part = 490;
                            else if (Type < 111) Part = 500;
                            else if (Type < 121) Part = 510;
                            else if (Type < 131) Part = 530;
                            else if (Type < 141) Part = 540;
                            else if (Type < 151) Part = 560;
                            else if (Type < 161) Part = 561;
                            else Part = 580;

                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {
                                if (D.LevReq >= 5 && D.LevReq <= 100)
                                {
                                    if (D.LevReq != 0)
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                            From.Add(D.ID);
                                }
                            }
                            if (From != null)
                            {
                                if (From.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    ItemID = (uint)From[Tries];
                                }
                            }
                            if (ItemID != 0)
                            {
                                I2.ID = ItemID;
                                if (I2.DBInfo.LevReq != 1)
                                {
                                    if (MyMath.ChanceSuccess(0.1))
                                    {
                                        Q = Item.ItemQuality.Super;
                                    }
                                    else if (MyMath.ChanceSuccess(5))
                                    {
                                        Q = Item.ItemQuality.Elite;
                                    }
                                    else if (MyMath.ChanceSuccess(10))
                                    {
                                        Q = Item.ItemQuality.Unique;
                                    }
                                    else if (MyMath.ChanceSuccess(7))
                                    {
                                        Q = Item.ItemQuality.Refined;
                                    }
                                    ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                    E.QualityChange(Q);
                                    I2.ID = E.ToID();
                                }

                                I2.Color = Item.ArmorColor.Orange;

                                I2.Plus = 0;

                                if (MyMath.ChanceSuccess(05))
                                {
                                    if (MyMath.ChanceSuccess(0.1))
                                        I2.Plus = 5;

                                    else if (MyMath.ChanceSuccess(0.2))
                                        I2.Plus = 4;
                                    else if (MyMath.ChanceSuccess(3))
                                        I2.Plus = 3;
                                    else if (MyMath.ChanceSuccess(2))
                                        I2.Plus = 2;
                                    else if (MyMath.ChanceSuccess(20))
                                        I2.Plus = 1;
                                }
                                else if (MyMath.ChanceSuccess(40))
                                    I2.Plus = 1;

                                if (MyMath.ChanceSuccess(1))
                                    I2.Bless = 5;
                                else if (MyMath.ChanceSuccess(3.5))
                                    I2.Bless = 3;
                                else if (MyMath.ChanceSuccess(7))
                                    I2.Bless = 1;

                                if (MyMath.ChanceSuccess(20))
                                {
                                    I2.Soc1 = Item.Gem.EmptySocket;
                                    I2.Soc2 = Item.Gem.EmptySocket;
                                }
                                else if (MyMath.ChanceSuccess(45))
                                    I2.Soc1 = Item.Gem.EmptySocket;

                                if (I2.Soc1 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }

                                }

                                if (I2.Soc2 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(14))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(45))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (MyMath.ChanceSuccess(1))
                                    I2.Enchant = 255;
                                else if (MyMath.ChanceSuccess(5))
                                    I2.Enchant = 200;
                                else if (MyMath.ChanceSuccess(10))
                                    I2.Enchant = 145;
                                else if (MyMath.ChanceSuccess(25))
                                    I2.Enchant = 86;
                                else if (MyMath.ChanceSuccess(45))
                                    I2.Enchant = 56;
                                else
                                    I2.Enchant = 25;

                                I2.MaxDur = I2.DBInfo.Durability;
                                I2.CurDur = I2.MaxDur;

                                GC.MyChar.AddItem(I2);
                                if (I2.Plus >= 5)
                                    Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market 2,000,000 Money!", 2005, 0);
                                Game.World.DebugAdd += GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market 2,000,000 Money \r\n";
                                AddText("You received a +" + I2.Plus + " Weapon for 2,000,000 Money!");
                                AddOption("Ok.", 255);

                            }
                            else goto top;
                        }
                        else
                        {
                            AddText("You don't have 2,000,000 Money, or your inventory is full. You need 2,000,000 Money and 1 empty spot in your bag.");
                            AddOption("I see...", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.Silvers >= 10000000 && GC.MyChar.Inventory.Count <= 39)
                        {
                            GC.MyChar.Silvers -= 10000000;

                        top:
                            Item I2 = new Item();
                            I2.UID = (uint)Rnd.Next(10000000);
                            Item.ItemQuality Q = Item.ItemQuality.Simple;


                            uint ItemID = 0;
                            ArrayList From = new ArrayList();
                            int Type = Rnd.Next(0, 180);
                            uint Part = 0;
                            if (Type < 11) Part = 410;
                            else if (Type < 22) Part = 420;
                            else if (Type < 33) Part = 421;
                            else if (Type < 44) Part = 430;
                            else if (Type < 55) Part = 440;
                            else if (Type < 66) Part = 450;
                            else if (Type < 77) Part = 460;
                            else if (Type < 88) Part = 480;
                            else if (Type < 99) Part = 481;
                            else if (Type < 101) Part = 490;
                            else if (Type < 111) Part = 500;
                            else if (Type < 121) Part = 510;
                            else if (Type < 131) Part = 530;
                            else if (Type < 141) Part = 540;
                            else if (Type < 151) Part = 560;
                            else if (Type < 161) Part = 561;
                            else Part = 580;

                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {
                                if (D.LevReq >= 5 && D.LevReq <= 100)
                                {
                                    if (D.LevReq != 0)
                                        if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                            From.Add(D.ID);
                                }
                            }
                            if (From != null)
                            {
                                if (From.Count > 0)
                                {
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    ItemID = (uint)From[Tries];
                                }
                            }
                            if (ItemID != 0)
                            {
                                I2.ID = ItemID;
                                if (I2.DBInfo.LevReq != 1)
                                {
                                    if (MyMath.ChanceSuccess(0.2))
                                    {
                                        Q = Item.ItemQuality.Super;
                                    }
                                    else if (MyMath.ChanceSuccess(20))
                                    {
                                        Q = Item.ItemQuality.Elite;
                                    }
                                    else if (MyMath.ChanceSuccess(40))
                                    {
                                        Q = Item.ItemQuality.Unique;
                                    }
                                    else
                                    {
                                        Q = Item.ItemQuality.Refined;
                                    }
                                    ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                    E.QualityChange(Q);
                                    I2.ID = E.ToID();
                                }

                                I2.Color = Item.ArmorColor.Orange;

                                I2.Plus = 0;

                                if (MyMath.ChanceSuccess(0.1))
                                {
                                    if (MyMath.ChanceSuccess(0.05))

                                        I2.Plus = 5;
                                    else if (MyMath.ChanceSuccess(0.1))
                                        I2.Plus = 4;
                                    else if (MyMath.ChanceSuccess(0.5))
                                        I2.Plus = 3;
                                    else if (MyMath.ChanceSuccess(0.2))
                                        I2.Plus = 3;
                                    else
                                        I2.Plus = 2;
                                }
                                else
                                    I2.Plus = 2;


                                if (MyMath.ChanceSuccess(5))
                                    I2.Bless = 5;
                                else if (MyMath.ChanceSuccess(10))
                                    I2.Bless = 3;
                                else if (MyMath.ChanceSuccess(17.5))
                                    I2.Bless = 1;

                                if (MyMath.ChanceSuccess(65))
                                {
                                    I2.Soc1 = Item.Gem.EmptySocket;
                                    I2.Soc2 = Item.Gem.EmptySocket;
                                }
                                else
                                    I2.Soc1 = Item.Gem.EmptySocket;

                                if (I2.Soc1 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (I2.Soc2 == Item.Gem.EmptySocket)
                                {
                                    if (Part == 421)
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedPhoenixGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalPhoenixGem;
                                    }
                                    else
                                    {
                                        if (MyMath.ChanceSuccess(33))
                                            I2.Soc1 = Item.Gem.RefinedDragonGem;
                                        else if (MyMath.ChanceSuccess(70))
                                            I2.Soc1 = Item.Gem.NormalDragonGem;
                                    }
                                }

                                if (MyMath.ChanceSuccess(20))
                                    I2.Enchant = 255;
                                else if (MyMath.ChanceSuccess(25))
                                    I2.Enchant = 200;
                                else if (MyMath.ChanceSuccess(35))
                                    I2.Enchant = 145;
                                else if (MyMath.ChanceSuccess(51))
                                    I2.Enchant = 86;
                                else if (MyMath.ChanceSuccess(75))
                                    I2.Enchant = 56;
                                else
                                    I2.Enchant = 25;

                                I2.MaxDur = I2.DBInfo.Durability;
                                I2.CurDur = I2.MaxDur;

                                GC.MyChar.AddItem(I2);
                                if (I2.Plus >= 5)
                                {
                                    if (I2.Plus >= 7)
                                    {
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 10,000,000 Money!", 2011, 0);
                                    }
                                    else
                                    {
                                        Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 10,000,000 Money!", 2005, 0);
                                    }
                                }
                                Game.World.DebugAdd += GC.MyChar.Name + " bought a +" + I2.Plus + " Weapon from the Black Market for 10,000,000 Money \r\n";
                                AddText("You received a +" + I2.Plus + " Weapon for 10,000,000 Money!");
                                AddOption("Ok.", 255);

                            }
                            else goto top;
                        }
                        else
                        {
                            AddText("You don't have 10,000,000 Money, or your inventory is full. You need 10,000,000 Money and 1 empty spot in your bag.");
                            AddOption("I see...", 255);
                        }
                        break;
                    }
            }


            AddFinish();
            Send();
        }
    }
}