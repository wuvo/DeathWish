using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using Ultimate.Game;

namespace Ultimate.NPCs
{
    public class NPC_189701 : NPCBase
    {
        public NPC_189701(Main.GameClient _client)
            : base(_client)
        {
            ID = 189701;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("Hello This is the Newest System of UltimateConquer\n");
                    AddText("Random Items Lottery. Don't ask where the goods come from. No refunds.");
                    AddText("You will get DBs, Mets, BlackTulip, Money, 1SocItem, MoonBox, CleanWater, Vip, ");
                    AddText("Garment, ProfToken, +1/+2/+3 Items, EggPacket, +1ItemPacket, LotteryTicket, HousePerm, UpgradeCert..");
                    AddOption("I will try. (3kk Money)", 1);
                    AddOption("I don't feel lucky today.", 2);
                    break;
                case 1:
                    if (GC.MyChar.Silvers >= 3000000 && GC.MyChar.Inventory.Count < 36)
                    {
                        GC.MyChar.Silvers -= 3000000;


                        AddText($"Thanks You sir ! My children will be very happy now ! i'm giving your reward!");
                        AddOption("Thanks", 255);


                        Random Rnd = new Random();
                        switch (Rnd.Next(0, 22))
                        {
                            case 0:
                                for (int a = 0; a < 1; a++)
                                    GC.MyChar.AddItem(720027);
                                GC.LocalMessage(2000, "You got 1 MeteorScroll.");
                                break;
                            case 1:
                                GC.MyChar.AddItem(720652);
                                GC.LocalMessage(2000, "You got a DemonBox.");
                                break;
                            case 2:
                                for (int a = 0; a < 5; a++)
                                    GC.MyChar.AddItem(720027);
                                GC.LocalMessage(2000, "You got 5 MeteorScroll.");
                                break;
                            case 3:
                                GC.MyChar.AddItem(1088000);
                                GC.LocalMessage(2000, "You got 1 DragonBall.");
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1 DragonBall!", 2005, 0);
                                break;
                            case 4:
                                for (int a = 0; a < 2; a++)
                                    GC.MyChar.AddItem(1088000);
                                GC.LocalMessage(2000, "You got 2 DragonBall.");
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 2 DragonBall!", 2005, 0);
                                break;
                            case 5:
                                for (int a = 0; a < 3; a++)
                                    GC.MyChar.AddItem(1088000);
                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 3 DragonBall!", 2005, 0);

                                break;
                            case 6:
                                #region +1 Item
                                for (int a = 0; a < 1; a++)
                                {
                                top:
                                    Item I2 = new Item();
                                    I2.UID = (uint)Program.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Normal;

                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = Program.Rnd.Next(0, 255);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    else if (Type < 175) Part = 410;
                                    else if (Type < 185) Part = 420;
                                    else if (Type < 195) Part = 480;
                                    else if (Type < 205) Part = 481;
                                    else if (Type < 215) Part = 500;
                                    else if (Type < 225) Part = 530;
                                    else if (Type < 235) Part = 560;
                                    else if (Type < 245) Part = 561;
                                    else if (Type < 255) Part = 900;

                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }

                                        I2.Color = Item.ArmorColor.Orange;

                                        I2.Plus = 1;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                        GC.MyChar.AddItem(I2);
                                        GC.MyChar.MyClient.LocalMessage(2005, "You have received a +1Item!");

                                    }
                                    else goto top;
                                }
                                #endregion
                                break;
                            case 7:
                                GC.MyChar.AddItem(720656);
                                GC.LocalMessage(2000, "You got DreamGoldPack.");
                                break;
                            case 8:
                                GC.MyChar.AddItem(721246); //ccgw
                                break;
                            case 9:
                                #region +1 Item
                                for (int a = 0; a < 1; a++)
                                {
                                top:
                                    Item I2 = new Item();
                                    I2.UID = (uint)Program.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Normal;

                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = Program.Rnd.Next(0, 255);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    else if (Type < 175) Part = 410;
                                    else if (Type < 185) Part = 420;
                                    else if (Type < 195) Part = 480;
                                    else if (Type < 205) Part = 481;
                                    else if (Type < 215) Part = 500;
                                    else if (Type < 225) Part = 530;
                                    else if (Type < 235) Part = 560;
                                    else if (Type < 245) Part = 561;
                                    else if (Type < 255) Part = 900;

                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }

                                        I2.Color = Item.ArmorColor.Orange;

                                        I2.Plus = 1;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                        GC.MyChar.AddItem(I2);
                                        GC.MyChar.MyClient.LocalMessage(2005, "You have received a +1Item!");

                                    }
                                    else goto top;
                                }
                                #endregion
                                break;


                            case 10:
                                if (MyMath.ChanceSuccess(0.6))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 780001;
                                        I.Plus = 3;
                                        I.Bless = 5;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 3Day Vip5!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 3Day Vip5!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(0.8))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 780001;
                                        I.Plus = 1;
                                        I.Bless = 6;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1Day Vip6!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1Day Vip6!", 2000, 0);

                                }
                                else
                                {
                                    GC.MyChar.AddItem(722384);
                                    GC.LocalMessage(2000, "You got Proficiency Token.");
                                }

                                break;

                            case 11:
                                if (MyMath.ChanceSuccess(1))
                                {
                                    int _id = Program.Rnd.Next(0, 14);
                                    uint toadd = 181925;
                                    switch (_id)
                                    {
                                        case 0:
                                            toadd = 181315;// WhiteElegance
                                            break;
                                        case 1:
                                            toadd = 181325;// WhiteCelestial
                                            break;
                                        case 2:
                                            toadd = 181415;// BrownElegance
                                            break;
                                        case 3:
                                            toadd = 181425;// BrownCelestial
                                            break;
                                        case 4:
                                            toadd = 181515;// BlackElegance
                                            break;
                                        case 5:
                                            toadd = 181525;// BlackCelestial
                                            break;
                                        case 6:
                                            toadd = 181615;// RedElegance
                                            break;
                                        case 7:
                                            toadd = 181625;// RedCelestial
                                            break;
                                        case 8:
                                            toadd = 181715;// GreenElegance
                                            break;
                                        case 9:
                                            toadd = 181725;// GreenCelestial
                                            break;
                                        case 10:
                                            toadd = 181815;// BlueElegance
                                            break;
                                        case 11:
                                            toadd = 181825;// BlueCelestial
                                            break;
                                        case 12:
                                            toadd = 181915;// PurpleElegance
                                            break;
                                        case 13:
                                            toadd = 181925;// PurpleCelestial 
                                            break;
                                    }
                                    GC.MyChar.AddItem(toadd);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a GARMENT!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a GARMENT!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(1))
                                {
                                    int _id = Program.Rnd.Next(0, 30);
                                    uint toadd = 181345;
                                    switch (_id)
                                    {
                                        case 0:
                                            toadd = 181345; // ColorfulDress
                                            break;
                                        case 1:
                                            toadd = 181365; // PrairieWind
                                            break;
                                        case 2:
                                            toadd = 181355; // DarkWizard
                                            break;
                                        case 3:
                                            toadd = 181385; // RoyalDignity
                                            break;
                                        case 4:
                                            toadd = 181395; // UglyDuck
                                            break;
                                        case 5:
                                            toadd = 181505; // BlackPhoenix
                                            break;
                                        case 6:
                                            toadd = 181555; // DarkWizard
                                            break;
                                        case 7:
                                            toadd = 191405; // Alternate Goodluck *rare*
                                            break;
                                        case 8:
                                            toadd = 181395; // Ugly Duck
                                            break;
                                        case 9:
                                            toadd = 181555; // Dark Wizard
                                            break;
                                        case 10:
                                            toadd = 191905; // Normal Goodluck 
                                            break;
                                        case 11:
                                            toadd = 191405; // Alternate Goodluck *rare*
                                            break;
                                        case 12:
                                            toadd = 182345; // MoonOrchid
                                            break;
                                        case 13:
                                            toadd = 181355; // DarkWizard
                                            break;
                                        case 14:
                                            toadd = 181385; // RoyalDignity
                                            break;
                                        case 15:
                                            toadd = 181705; // GreenPhoenix
                                            break;
                                        case 16:
                                            toadd = 181505; // BlackPhoenix
                                            break;
                                        case 17:
                                            toadd = 181395; // UglyDuck
                                            break;
                                        case 18:
                                            toadd = 182505; // SouthOfCloud
                                            break;
                                        case 19:
                                            toadd = 182535; // Blue Dream
                                            break;
                                        case 20:
                                            toadd = 182415; // Bonfire Night
                                            break;
                                        case 21:
                                            toadd = 181675; // SongOfTianshan
                                            break;
                                        case 22:
                                            toadd = 191605; // Normal Goodluck
                                            break;
                                        case 23:
                                            toadd = 191405; // Alternate Goodluck *rare*
                                            break;
                                        case 24:
                                            toadd = 182505; // SouthOfCloud
                                            break;
                                        case 25:
                                            toadd = 191605; // Normal Goodluck
                                            break;
                                        case 26:
                                            toadd = 182415; // Bonfire Night
                                            break;
                                        case 27:
                                            toadd = 181605; // RedPhoenix
                                            break;
                                        case 28:
                                            toadd = 191605; // Normal Goodluck
                                            break;
                                        case 29:
                                            toadd = 191405; // Alternate Goodluck *rare*
                                            break;
                                    }
                                    GC.MyChar.AddItem(toadd);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a GARMENT!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a GARMENT!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(40))
                                {
                                    GC.MyChar.AddItem(722384);//lotteryticket
                                    GC.LocalMessage(2000, "You got  ProfToken.");
                                }
                                else
                                {
                                    GC.MyChar.AddItem(720652);//250k
                                    GC.LocalMessage(2000, "You got FloodDemonBox.");
                                }
                                break;

                            case 12:
                                if (MyMath.ChanceSuccess(20))
                                {
                                    #region +1 Item
                                    for (int a = 0; a < 3; a++)
                                    {
                                    top:
                                        Item I2 = new Item();
                                        I2.UID = (uint)Program.Rnd.Next(10000000);
                                        Item.ItemQuality Q = Item.ItemQuality.Normal;

                                        uint ItemID = 0;
                                        List<uint> From = new List<uint>();
                                        int Type = Program.Rnd.Next(0, 255);
                                        uint Part = 0;
                                        if (Type < 10) Part = 111;
                                        else if (Type < 20) Part = 113;
                                        else if (Type < 30) Part = 114;
                                        else if (Type < 40) Part = 117;
                                        else if (Type < 50) Part = 118;
                                        else if (Type < 60) Part = 120;
                                        else if (Type < 70) Part = 121;
                                        else if (Type < 80) Part = 130;
                                        else if (Type < 90) Part = 131;
                                        else if (Type < 100) Part = 133;
                                        else if (Type < 110) Part = 134;
                                        else if (Type < 120) Part = 141;
                                        else if (Type < 130) Part = 142;
                                        else if (Type < 140) Part = 150;
                                        else if (Type < 150) Part = 151;
                                        else if (Type < 160) Part = 152;
                                        else if (Type < 165) Part = 160;
                                        else if (Type < 175) Part = 410;
                                        else if (Type < 185) Part = 420;
                                        else if (Type < 195) Part = 480;
                                        else if (Type < 205) Part = 481;
                                        else if (Type < 215) Part = 500;
                                        else if (Type < 225) Part = 530;
                                        else if (Type < 235) Part = 560;
                                        else if (Type < 245) Part = 561;
                                        else if (Type < 255) Part = 900;

                                        foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                        {
                                            if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                                byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                ItemID = (uint)From[Tries];
                                            }
                                        }
                                        if (ItemID != 0)
                                        {
                                            I2.ID = ItemID;
                                            if (I2.DBInfo.LevReq != 1)
                                            {
                                                ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                                E.QualityChange(Q);
                                                I2.ID = E.ToID();
                                            }

                                            I2.Color = Item.ArmorColor.Orange;

                                            I2.Plus = 1;
                                            I2.MaxDur = I2.DBInfo.Durability;
                                            I2.CurDur = I2.MaxDur;

                                            GC.MyChar.AddItem(I2);
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received 3x +1 Item!", 2000, 0);
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received 3x +1 Item!", 2011, 0);
                                        }
                                        else goto top;
                                    }

                                }
                                else
                                {
                                    GC.MyChar.AddItem(722384);
                                    GC.LocalMessage(2000, "You got  ProfToken.");
                                }
                                #endregion
                                break;

                            case 13:

                                if (MyMath.ChanceSuccess(20))
                                {
                                    GC.MyChar.AddItem(723712);//+1ItemPack
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a +1 ItemPack!", 2000, 0);
                                }
                                else
                                {
                                    List<uint> From = new List<uint>() { 700002, 700012, 700032, 700042, 700052, 700062, 700072 };
                                    byte Tries = (byte)Rnd.Next(0, From.Count);
                                    GC.MyChar.AddItem((uint)From[Tries]);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a Rafined Gem!", 2005, 0);
                                    break;
                                }
                                break;


                            case 14:
                                if (MyMath.ChanceSuccess(1))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 780001;
                                        I.Plus = 1;
                                        I.Bless = 5;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1 Day Vip5!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1 Day Vip5!", 2000, 0);
                                }
                                else
                                {
                                    GC.MyChar.AddItem(710212);//lotteryticket
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a LotteryTicket!", 2005, 0);
                                }
                                break;




                            case 15:
                                if (MyMath.ChanceSuccess(15))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 720142;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a Egg Packet!", 2005, 0);

                                }
                                else
                                {
                                    GC.MyChar.AddItem(710212);//lotteryticket
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a LotteryTicket!", 2005, 0);
                                }
                                break;



                            case 16:
                                if (MyMath.ChanceSuccess(15))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 721080;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a MoonBox!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a MoonBox!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(15))
                                {
                                    #region +3 Item
                                    for (int a = 0; a < 1; a++)
                                    {
                                    top:
                                        Item I2 = new Item();
                                        I2.UID = (uint)Program.Rnd.Next(10000000);
                                        Item.ItemQuality Q = Item.ItemQuality.Normal;

                                        uint ItemID = 0;
                                        List<uint> From = new List<uint>();
                                        int Type = Program.Rnd.Next(0, 255);
                                        uint Part = 0;
                                        if (Type < 10) Part = 111;
                                        else if (Type < 20) Part = 113;
                                        else if (Type < 30) Part = 114;
                                        else if (Type < 40) Part = 117;
                                        else if (Type < 50) Part = 118;
                                        else if (Type < 60) Part = 120;
                                        else if (Type < 70) Part = 121;
                                        else if (Type < 80) Part = 130;
                                        else if (Type < 90) Part = 131;
                                        else if (Type < 100) Part = 133;
                                        else if (Type < 110) Part = 134;
                                        else if (Type < 120) Part = 141;
                                        else if (Type < 130) Part = 142;
                                        else if (Type < 140) Part = 150;
                                        else if (Type < 150) Part = 151;
                                        else if (Type < 160) Part = 152;
                                        else if (Type < 165) Part = 160;
                                        else if (Type < 175) Part = 410;
                                        else if (Type < 185) Part = 420;
                                        else if (Type < 195) Part = 480;
                                        else if (Type < 205) Part = 481;
                                        else if (Type < 215) Part = 500;
                                        else if (Type < 225) Part = 530;
                                        else if (Type < 235) Part = 560;
                                        else if (Type < 245) Part = 561;
                                        else if (Type < 255) Part = 900;

                                        foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                        {
                                            if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                                byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                                ItemID = (uint)From[Tries];
                                            }
                                        }
                                        if (ItemID != 0)
                                        {
                                            I2.ID = ItemID;
                                            if (I2.DBInfo.LevReq != 1)
                                            {
                                                ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                                E.QualityChange(Q);
                                                I2.ID = E.ToID();
                                            }

                                            I2.Color = Item.ArmorColor.Orange;

                                            I2.Plus = 3;
                                            I2.MaxDur = I2.DBInfo.Durability;
                                            I2.CurDur = I2.MaxDur;

                                            GC.MyChar.AddItem(I2);
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a +3 Item!", 2011, 0);
                                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a +3 Item!", 2000, 0);

                                        }
                                        else goto top;
                                    }
                                    #endregion
                                }

                                else
                                {
                                    GC.MyChar.AddItem(710212);//lotteryticket
                                    GC.LocalMessage(2000, "You got  LotteryTicket.");
                                }
                                break;


                            case 17:
                                if (MyMath.ChanceSuccess(25))
                                {
                                    for (int a = 0; a < 1; a++)
                                    {
                                        Item I = new Item();
                                        I.ID = 721258;
                                        I.MaxDur = I.DBInfo.Durability;
                                        I.CurDur = I.MaxDur;
                                        GC.MyChar.AddItem(I);
                                    }
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a CleanWater!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a CleanWater!", 2000, 0);
                                }
                                else
                                {
                                    GC.MyChar.AddItem(710212);
                                    GC.LocalMessage(2000, "You got LotteryTicket.");
                                }
                                break;
                            case 18:
                                #region +2 Item
                                for (int a = 0; a < 1; a++)
                                {
                                top:
                                    Item I2 = new Item();
                                    I2.UID = (uint)Program.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Normal;

                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = Program.Rnd.Next(0, 255);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    else if (Type < 175) Part = 410;
                                    else if (Type < 185) Part = 420;
                                    else if (Type < 195) Part = 480;
                                    else if (Type < 205) Part = 481;
                                    else if (Type < 215) Part = 500;
                                    else if (Type < 225) Part = 530;
                                    else if (Type < 235) Part = 560;
                                    else if (Type < 245) Part = 561;
                                    else if (Type < 255) Part = 900;

                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }

                                        I2.Color = Item.ArmorColor.Orange;

                                        I2.Plus = 2;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                        GC.MyChar.AddItem(I2);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a +2 Item!", 2005, 0);

                                    }
                                    else goto top;
                                }
                                #endregion
                                break;
                            case 19:
                                if (MyMath.ChanceSuccess(15))
                                {
                                    GC.MyChar.AddItem(723584);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a BlackTulip!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a BlackTulip!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(30))
                                {
                                    GC.MyChar.AddItem(723712);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a +1 ItemPacket!", 2005, 0);

                                }
                                else
                                {
                                    GC.MyChar.AddItem(720027);
                                    GC.LocalMessage(2000, "You got MeteorScroll.");
                                }
                                break;
                            case 20:
                                if (MyMath.ChanceSuccess(8))
                                {
                                    Item I2 = new Item();
                                    I2.UID = (uint)GC.MyChar.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Refined;
                                    if (MyMath.ChanceSuccess(4.5))
                                        Q = Item.ItemQuality.Super;
                                    else if (MyMath.ChanceSuccess(9))
                                        Q = Item.ItemQuality.Elite;
                                    else if (MyMath.ChanceSuccess(25))
                                        Q = Item.ItemQuality.Unique;

                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = GC.MyChar.Rnd.Next(0, 330);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    else if (Type < 175) Part = 410;
                                    else if (Type < 185) Part = 420;
                                    else if (Type < 195) Part = 421;
                                    else if (Type < 203) Part = 430;
                                    else if (Type < 211) Part = 440;
                                    else if (Type < 219) Part = 450;
                                    else if (Type < 229) Part = 460;
                                    else if (Type < 239) Part = 480;
                                    else if (Type < 247) Part = 481;
                                    else if (Type < 255) Part = 490;
                                    else if (Type < 265) Part = 500;
                                    else if (Type < 275) Part = 510;
                                    else if (Type < 285) Part = 530;
                                    else if (Type < 295) Part = 540;
                                    else if (Type < 305) Part = 560;
                                    else if (Type < 315) Part = 561;
                                    else if (Type < 325) Part = 580;
                                    else if (Type < 330) Part = 900;

                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
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
                                            byte Tries = (byte)GC.MyChar.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }

                                        I2.Color = Item.ArmorColor.Orange;

                                        if (ItemIDManipulation.Digit(I2.ID, 1) == 4 || ItemIDManipulation.Digit(I2.ID, 1) == 5)
                                        {
                                            if (MyMath.ChanceSuccess(DropRates.OneSoc + 3))
                                                I2.Soc1 = Item.Gem.EmptySocket;
                                            if (MyMath.ChanceSuccess(DropRates.TwoSoc + 1))
                                            {
                                                I2.Soc1 = Item.Gem.EmptySocket;
                                                I2.Soc2 = Item.Gem.EmptySocket;
                                            }
                                        }
                                        else if (ItemIDManipulation.Digit(I2.ID, 1) == 1 || ItemIDManipulation.Digit(I2.ID, 1) == 2 || ItemIDManipulation.Digit(I2.ID, 1) == 3 || ItemIDManipulation.Digit(I2.ID, 1) == 6 || ItemIDManipulation.Digit(I2.ID, 1) == 8)
                                        {
                                            if (MyMath.ChanceSuccess(100))
                                            {
                                                I2.Soc1 = Item.Gem.EmptySocket;
                                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1 Socket Item!", 2011, 0);
                                                World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a 1 Socket Item!", 2000, 0);
                                            }
                                        }
                                        if (MyMath.ChanceSuccess(DropRates.PlusOne + 3.5))
                                        {
                                            I2.Plus = 1;
                                            if (MyMath.ChanceSuccess(1.5))
                                                I2.Plus = 2;
                                        }
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                        GC.MyChar.AddItem(I2);
                                    }

                                }
                                else
                                {
                                    GC.MyChar.AddItem(720027);
                                    GC.LocalMessage(2000, "You got MeteorScroll.");
                                }
                                break;

                            case 21:
                                if (MyMath.ChanceSuccess(8))
                                {
                                    GC.MyChar.AddItem(721170);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a HousePermit!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a HousePermit!", 2000, 0);
                                }
                                else if (MyMath.ChanceSuccess(8))
                                {
                                    GC.MyChar.AddItem(721174);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a House UpgradeCert!", 2011, 0);
                                    World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has spend 3,000,000 Money for BlackMarket and received a House UpgradeCert!", 2000, 0);

                                }
                                else
                                {
                                    GC.MyChar.AddItem(710212);
                                    GC.LocalMessage(2000, "You got LotteryTicket.");
                                }

                                break;
                        }



                    }

                    else
                    {
                        AddText("Please make sure you have 5 free slot in your inventory and you have to be 3kk Money.");
                        AddOption("Alright", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}
