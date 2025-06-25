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
    public class NPC_380 : NPCBase
    {
        public NPC_380(Main.GameClient _client)
            : base(_client)
        {
            ID = 380;
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
                        AddText("Do you want enter the Guild Area?");
                        AddOption("Yes", 1);
                        AddOption("Claim my prize", 2);
                        //AddOption("Buy a StatueScroll", 3);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        if (!Features.GuildWars.War)
                        {
                            int x = Program.Rnd.Next(1, 4);
                            if (x == 1)
                                GC.MyChar.Teleport(1038, 348, 339);
                            else if (x == 2)
                                GC.MyChar.Teleport(1038, 348, 339);
                            else
                                GC.MyChar.Teleport(1038, 348, 339);
                        }
                        else
                        {
                            int x = Program.Rnd.Next(1, 6);
                            if (x == 1)
                                GC.MyChar.Teleport(1038, 308, 382);
                            else if (x == 2)
                                GC.MyChar.Teleport(1038, 303, 346);
                            else if (x == 3)
                                GC.MyChar.Teleport(1038, 335, 295);
                            else if (x == 4)
                                GC.MyChar.Teleport(1038, 379, 306);
                            else
                                GC.MyChar.Teleport(1038, 324, 315);
                        }
                    }
                    break;
                case 2:
                    {
                        if (GC.MyChar.GuildRank == Features.GuildRank.DeputyManager && Features.GuildWars.LastWinner == GC.MyChar.MyGuild && !Features.GuildWars.War)
                        {
                            GC.MyChar.Top = 2;
                            GC.MyChar.StatEff.Add(StatusEffectEn.TopDeputyLeader);
                            GC.Message(2005, GC.MyChar.Name + " has got the TopDL for being DL in the winner Guild of last GuildWar.");
                        }
                        if (GC.MyChar.GuildRank == Features.GuildRank.GuildLeader && Features.GuildWars.LastWinner == GC.MyChar.MyGuild && !Features.GuildWars.War)
                        {
                            GC.MyChar.Top = 1;
                            GC.MyChar.StatEff.Add(StatusEffectEn.TopGuildLeader);
                            GC.Message(2005, GC.MyChar.Name + " has got the TopGL for being the leader in the winner Guild of last GuildWar.");
                        }
                        if (Features.GuildWars.LastWinner == GC.MyChar.MyGuild && Features.GuildWars.GWPRIZE == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            foreach (Character Char in World.H_Chars.Values)
                            {
                                if (Char.GuildRank == Features.GuildRank.DeputyManager && Features.GuildWars.LastWinner == Char.MyGuild)
                                {
                                    Char.Top = 2;
                                    Char.StatEff.Add(StatusEffectEn.TopDeputyLeader);
                                    Char.MyClient.Message(2005, Char.Name + " has got the TopDL for being DL in the winner Guild of last GuildWar.");
                                }
                                if (Char.GuildRank == Features.GuildRank.GuildLeader && Features.GuildWars.LastWinner == Char.MyGuild)
                                {
                                    Char.Top = 1;
                                    Char.StatEff.Add(StatusEffectEn.TopGuildLeader);
                                    Char.MyClient.Message(2005, Char.Name + " has got the TopGL for being the leader in the winner Guild of last GuildWar.");
                                }
                            }
                            if (GC.MyChar.Inventory.Count <= 25)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got GW prize");
                                GC.MyChar.Silvers += 500000000;
                                GC.MyChar.AddItem(723584);
                                AddText("Congratulations! You have received 500,000,000 silvers, 2 Super Tortoise Gems, 3 StatueScrolls and a BlackTulip for winning the GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed a reward for Guild War: 500,000,000 silvers, 21 Day Vip, 2 Super Gems and a BlackTulip!", 2011, 0);
                                GC.MyChar.AddItem(700073);
                                GC.MyChar.AddItem(700073);
                                for (int a = 0; a < 3; a++)
                                    GC.MyChar.AddItem(720020);
                                for (int a = 0; a < 7; a++)
                                {
                                    Item I = new Item();
                                    I.ID = 780001;
                                    I.Plus = 3;
                                    I.Bless = 6;
                                    I.MaxDur = I.DBInfo.Durability;
                                    I.CurDur = I.MaxDur;
                                    GC.MyChar.AddItem(I);
                                }
                                Features.GuildWars.GWPRIZE = false;
                                AddOption("Thanks.", 255);
                                break;
                            }
                            else
                            {
                                AddText("You need to have at least 15 free slot in your inventory.");
                                AddOption("I see.", 255);
                                break;
                            }
                        }
                        else if (Features.GuildWars.LastWinner == GC.MyChar.MyGuild && Features.GuildWars.GWPRIZE == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the GuildWar or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                case 3:
                    AddText("If you're a GuildLeader/DeputyLeader and your Guild owns the Pole you can summon an amazing Statue of yourself! It costs 2,500,000 Silvers, are you interested?");
                    AddOption("Buy Statue", 4);
                    AddOption("Nevermind", 255);
                    break;
                case 4:
                    if (GC.MyChar.Silvers >= 2500000)
                    {
                        GC.MyChar.Silvers -= 2500000;
                        GC.MyChar.AddItem(720020);
                        AddText("Here you go ! Make sure you use it inside the Guild Map!");
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("You don't have 2,500,000 silvers!");
                        AddOption("I see", 255);
                    }
                    break;
            }

            AddFinish();
            Send();
        }
    }
}