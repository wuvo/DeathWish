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
    public class NPC_3800 : NPCBase
    {
        public NPC_3800(Main.GameClient _client)
            : base(_client)
        {
            ID = 3800;
            Face = 123;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I'm the teleporter of the TwinCityGuildWar! How can I help you?\n");
                        AddOption("Yes", 1);
                        AddOption("Claim my prize", 2);
                        //AddOption("Buy a StatueScroll", 3);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:

                    {
                        if (!Features.TCGuildWars.War)
                        {
                            int x = Program.Rnd.Next(1, 4);
                            if (x == 1)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else if (x == 2)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else
                                GC.MyChar.Teleport(10200, 621, 590);
                        }
                        else
                        {
                            int x = Program.Rnd.Next(1, 6);
                            if (x == 1)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else if (x == 2)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else if (x == 3)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else if (x == 4)
                                GC.MyChar.Teleport(10200, 621, 590);
                            else
                                GC.MyChar.Teleport(10200, 621, 590);
                        }
                    }
                    break;

                case 2:
                    {
                        //if (GC.MyChar.GuildRank == Features.GuildRank.DeputyManager && Features.TCGuildWars.LastWinner == GC.MyChar.MyGuild && !Features.TCGuildWars.War)
                        //{
                        //    GC.MyChar.Top = 2;
                        //    GC.MyChar.StatEff.Add(StatusEffectEn.TopDeputyLeader);
                        //    GC.Message(2005, GC.MyChar.Name + " has got the TopDL for being DL in the winner Guild of last GuildWar.");
                        //}
                        //if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader && Features.TCGuildWars.LastWinner == GC.MyChar.MyGuild && !Features.TCGuildWars.War)
                        //{
                        //    GC.MyChar.Top = 1;
                        //    GC.MyChar.StatEff.Add(StatusEffectEn.TopGuildLeader);
                        //    GC.Message(2005, GC.MyChar.Name + " has got the TopGL for being the leader in the winner Guild of last GuildWar.");
                        //}
                        if (Features.TCGuildWars.LastWinner == GC.MyChar.MyGuild && Features.TCGuildWars.GWPRIZE == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            //foreach (Character Char in World.H_Chars.Values)
                            //{
                            //    if (Char.GuildRank == Features.GuildRank.DeputyManager && Features.TCGuildWars.LastWinner == Char.MyGuild)
                            //    {
                            //        Char.Top = 2;
                            //        Char.StatEff.Add(StatusEffectEn.TopDeputyLeader);
                            //        Char.MyClient.Message(2005, Char.Name + " has got the TopDL for being DL in the winner Guild of last GuildWar.");
                            //    }
                            //    if (Char.GuildRank == Features.GuildRank.GuildLeader && Features.TCGuildWars.LastWinner == Char.MyGuild)
                            //    {
                            //        Char.Top = 1;
                            //        Char.StatEff.Add(StatusEffectEn.TopGuildLeader);
                            //        Char.MyClient.Message(2005, Char.Name + " has got the TopGL for being the leader in the winner Guild of last GuildWar.");
                            //    }
                            //}
                            if (GC.MyChar.Inventory.Count <= 30)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got TCGuildWars prize");
                                GC.MyChar.Silvers += 100000000;
                                //GC.MyChar.AddItem(710213);
                                //GC.MyChar.AddItem(710213);
                                AddText("Congratulations! You have received 100,000,000 silvers, 1 Sup Tortoise Gems for winning the TCGuildWars!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed a reward for TCGuildWars: 100,000,000 silvers, 1 Super Tortoise Gems!", 2011, 0);
                                GC.MyChar.AddItem(700073);

                                if (MyMath.ChanceSuccess(90))
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
                                                World.SendMsgToAll("LUCKY", GC.MyChar.Name + " has claim a TC GuildWar Prize and received a socket " + I2.DBInfo.Name + "!", 2000, 0);
                                                World.DebugAdd += GC.MyChar.Name + " obtained socket " + I2.DBInfo.Name + " from a TC GuildWar! \r\n";
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
                                //GC.MyChar.AddItem(700072);
                                //GC.MyChar.AddItem(700072);
                                //GC.MyChar.AddItem(700072);
                                //GC.MyChar.AddItem(700072);
                                Features.TCGuildWars.GWPRIZE = false;
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("You need to have at least one free slot in your inventory.");
                                AddOption("I see.", 255);
                                break;
                            }
                        }
                        else if (Features.TCGuildWars.LastWinner == GC.MyChar.MyGuild && Features.TCGuildWars.GWPRIZE == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the TCGuildWars or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                //case 3:
                //    AddText("If you're a GuildLeader/DeputyLeader and your Guild owns the Pole you can summon an amazing Statue of yourself! It costs 2,500,000 Silvers, are you interested?");
                //    AddOption("Buy Statue", 4);
                //    AddOption("Nevermind", 255);
                //    break;
                //case 4:
                //    if (GC.MyChar.Silvers >= 2500000)
                //    {
                //        GC.MyChar.Silvers -= 2500000;
                //        GC.MyChar.AddItem(720020);
                //        AddText("Here you go ! Make sure you use it inside the Guild Map!");
                //        AddOption("Thanks", 255);
                //    }
                //    else
                //    {
                //        AddText("You don't have 2,500,000 silvers!");
                //        AddOption("I see", 255);
                //    }
                //    break;
            }

            AddFinish();
            Send();
        }
    }
}