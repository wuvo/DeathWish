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
    public class NPC_3013 : NPCBase
    {
        public NPC_3013(Main.GameClient _client)
            : base(_client)
        {
            ID = 3013;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("it will be a really tough competition. You have to be superior in all maps. ");
                        AddText("no matter which map you dominate, your guild will have %25 DropRate on that map. ");
                        AddText("you have to be fast. You should protect other maps.. be ready to fight !!");
                        AddOption("Can i get more information?", 1);
                        AddOption("Join City War DesertCity", 2);
                        AddOption("Claim Prize", 3);
                        //AddOption("Nevermind", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("You must protect the castle in this tough battle every day. Support your guild");
                        AddText("and protect the tower. if you manage to protect the tower, you will get");
                        AddText("the great prize.! %25 DropRate..");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 2:
                    {
                        if (Features.CityWarDc.War)
                        {
                            int x = Program.Rnd.Next(1, 6);
                            if (x == 1)
                                GC.MyChar.Teleport(8508, 102, 129);
                            else if (x == 2)
                                GC.MyChar.Teleport(8508, 117, 117);
                            else if (x == 3)
                                GC.MyChar.Teleport(8508, 150, 143);
                            else if (x == 4)
                                GC.MyChar.Teleport(8508, 151, 118);
                            else
                                GC.MyChar.Teleport(8508, 124, 89);
                        }
                        else
                        {
                            AddText("i don't think it's the right time. You can try rejoining when the war starts.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (Features.CityWarDc.LastWinner == GC.MyChar.MyGuild && Features.CityWarDc.DCPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got CityWarDc prize");
                                GC.MyChar.Silvers += 50000000;
                                //GC.MyChar.AddItem(700013);
                                //GC.MyChar.AddItem(700003);
                                //GC.MyChar.AddItem(700073);
                                AddText("Congratulations! You have received 50,000,000 silvers for winning the CityWarDc GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed 50,000,000 silvers for winning the CityWarDc GuildWar!", 2011, 0);
                                Features.CityWarDc.DCPrize = false;
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
                        else if (Features.CityWarDc.LastWinner == GC.MyChar.MyGuild && Features.CityWarDc.DCPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the CityWarDc or the prize has been already given.");
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