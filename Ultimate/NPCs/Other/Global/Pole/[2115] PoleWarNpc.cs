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
    public class NPC_2115 : NPCBase
    {
        public NPC_2115(Main.GameClient _client)
            : base(_client)
        {
            ID = 2115;
            Face = 7;
        }
        Random Rnd = new Random();
        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("it will be a really tough competition. You have to be superior in all maps.");
                        AddText("You will win awards on the maps you dominate. Which map did you dominate ?");
                        AddText("Only Guild Leader Cant get Rewards..?");
                        AddOption("TwinCity", 1);
                        AddOption("PhoenixCastle", 2);
                        AddOption("ApeCity", 3);
                        AddOption("DesertCity", 4);
                        AddOption("BirdIsland", 5);
                        AddOption("Im not leader.", 255);
                        break;
                    }
                case 1:
                    {
                        if (Features.PoleWarTC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarTC.PoleTcPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got PoleWarTC prize");
                                GC.MyChar.Silvers += 25000000;

                                List<uint> From = new List<uint>() { 700003, 700013, 700033, 700043, 700053, 700063, 700073/*, 700072*/ };
                                byte Tries = (byte)Rnd.Next(0, From.Count);
                                GC.MyChar.AddItem((uint)From[Tries]);
                                AddText("Congratulations! You have received some money, super gem for winning the PoleWarTC GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed some money, super gem for winning the CityWarPc GuildWar!", 2011, 0);
                                Features.PoleWarTC.PoleTcPrize = false;
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
                        else if (Features.PoleWarTC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarTC.PoleTcPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the PoleWarTC or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                case 2:
                    {
                        if (Features.PoleWarPC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarPC.PolePcPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got PoleWarPC prize");
                                GC.MyChar.Silvers += 25000000;

                                List<uint> From = new List<uint>() { 700003, 700013, 700033, 700043, 700053, 700063, 700073/*, 700072*/ };
                                byte Tries = (byte)Rnd.Next(0, From.Count);
                                GC.MyChar.AddItem((uint)From[Tries]);
                                AddText("Congratulations! You have received some money, super gem for winning the PoleWarPC GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed some money, super gem for winning the CityWarPc GuildWar!", 2011, 0);
                                Features.PoleWarPC.PolePcPrize = false;
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
                        else if (Features.PoleWarPC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarPC.PolePcPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the PoleWarPC or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                case 3:
                    {
                        if (Features.PoleWarAC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarAC.PoleAcPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got PoleWarAC prize");
                                GC.MyChar.Silvers += 25000000;

                                List<uint> From = new List<uint>() { 700003, 700013, 700033, 700043, 700053, 700063, 700073/*, 700072*/ };
                                byte Tries = (byte)Rnd.Next(0, From.Count);
                                GC.MyChar.AddItem((uint)From[Tries]);
                                AddText("Congratulations! You have received some money, super gem for winning the PoleWarAC GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed some money, super gem for winning the CityWarPc GuildWar!", 2011, 0);
                                Features.PoleWarAC.PoleAcPrize = false;
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
                        else if (Features.PoleWarAC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarAC.PoleAcPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the PoleWarAC or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                case 4:
                    {
                        if (Features.PoleWarDC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarDC.PoleDcPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got PoleWarDC prize");
                                GC.MyChar.Silvers += 25000000;

                                List<uint> From = new List<uint>() { 700003, 700013, 700033, 700043, 700053, 700063, 700073/*, 700072*/ };
                                byte Tries = (byte)Rnd.Next(0, From.Count);
                                GC.MyChar.AddItem((uint)From[Tries]);
                                AddText("Congratulations! You have received some money, super gem for winning the PoleWarDC GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed some money, super gem for winning the CityWarPc GuildWar!", 2011, 0);
                                Features.PoleWarDC.PoleDcPrize = false;
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
                        else if (Features.PoleWarDC.LastWinner == GC.MyChar.MyGuild && Features.PoleWarDC.PoleDcPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the PoleWarDC or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
                case 5:
                    {
                        if (Features.PoleWarBI.LastWinner == GC.MyChar.MyGuild && Features.PoleWarBI.PoleBiPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got PoleWarBI prize");
                                GC.MyChar.Silvers += 25000000;

                                List<uint> From = new List<uint>() { 700003, 700013, 700033, 700043, 700053, 700063, 700073/*, 700072*/ };
                                byte Tries = (byte)Rnd.Next(0, From.Count);
                                GC.MyChar.AddItem((uint)From[Tries]);

                                AddText("Congratulations! You have received some money, super gem for winning the PoleWarBI GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed some money, super gem for winning the CityWarPc GuildWar!", 2011, 0);
                                Features.PoleWarBI.PoleBiPrize = false;
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
                        else if (Features.PoleWarBI.LastWinner == GC.MyChar.MyGuild && Features.PoleWarBI.PoleBiPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the PoleWarBI or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
            }

            AddFinish();
            Send();
        }
    }
}