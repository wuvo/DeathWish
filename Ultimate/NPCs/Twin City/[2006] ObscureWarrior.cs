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
    public class NPC_2006 : NPCBase
    {
        public NPC_2006(Main.GameClient _client)
            : base(_client)
        {
            ID = 2006;
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
                        AddText("Not so long ago, during my expedictions around the world, I have found ");
                        AddText("the ultimate castle full with labyrinths and traps. It was impossible to take it alone, ");
                        AddText("only the fiercest guild have the power to do it.");
                        AddOption("Tell me more about it", 1);
                        AddOption("Join the Counter Clock GW", 2);
                        AddOption("Claim Prize", 3);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("I'll be holding a special expediction to that place every Saturday where guilds ");
                        AddText("will be able to fight for the mighty castle. The guild that manages to keep the castle by ");
                        AddText("the end of the war will win countless treasures!");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 2:
                    {
                        if (Features.CounterClock.War)
                        {
                            ushort Y = 162;
                            int x = Program.Rnd.Next(0, 3);
                            if (x == 0)
                                Y = (ushort)(124 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));
                            else if (x == 1)
                                Y = (ushort)(162 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));
                            else if (x == 2)
                                Y = (ushort)(203 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8));
                            GC.MyChar.Teleport(1844, (ushort)(225 + Program.Rnd.Next(1, 8) - Program.Rnd.Next(1, 8)), Y);
                        }
                        else
                        {
                            AddText("Please come back in the right time. Counter Clock Guild War is not active.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (Features.CounterClock.LastWinner == GC.MyChar.MyGuild && Features.CounterClock.CCPrize == true && GC.MyChar.GuildRank == Features.GuildRank.GuildLeader)
                        {
                            if (GC.MyChar.Inventory.Count <= 39)
                            {
                                Program.WriteCmds(GC.MyChar.Name + " has got CCGW prize");
                                GC.MyChar.Silvers += 25000000;
                                GC.MyChar.AddItem(700013);
                                GC.MyChar.AddItem(700003);
                                GC.MyChar.AddItem(700073);
                                AddText("Congratulations! You have received 25,000,000 silvers, Super Tortoise, Dragon and Phoenix Gems for winning the Counter Clock GuildWar!");
                                Game.World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " claimed 25,000,000 silvers, a Super Tortoise, Dragon and Phoenix Gems for winning the Counter Clock GuildWar!", 2011, 0);
                                Features.CounterClock.CCPrize = false;
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
                        else if (Features.CounterClock.LastWinner == GC.MyChar.MyGuild && Features.CounterClock.CCPrize == true)
                        {
                            AddText("You are not the GuildLeader.");
                            AddOption("I see.", 255);
                            break;
                        }
                        else
                        {
                            AddText("You have not won the Counter Clock GuildWar or the prize has been already given.");
                            AddOption("I see.", 255);
                            break;
                        }
                    }
            }

            AddFinish();
            Send();
        }
    }
    public class NPC_2008 : NPCBase
    {
        public NPC_2008(Main.GameClient _client)
            : base(_client)
        {
            ID = 2008;
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
                        AddText("I have told you that this castle is not meant for everyone. Only the bravest should come in. Do you want me to send you out?");
                        AddOption("Yeah", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    GC.MyChar.Teleport(1002, 356, 319);
                    break;
            }

            AddFinish();
            Send();
        }
    }
}