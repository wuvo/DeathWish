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
    public class NPC_1621 : NPCBase
    {
        public NPC_1621(Main.GameClient _client)
            : base(_client)
        {
            ID = 1621;
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
                        AddText("I was sent here to protect the castle from the terrible beast that's hidden inside.");
                        AddText(" I shall only let the strongest players fight it. Therefore, only the guild leader of the guild that ");
                        AddText("won the the GuildWar can summon the GuildBeast! However, 1,000,000 silvers from the GuildFund have to be given to me!");
                        AddOption("Summon the GuildBeast", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if ((GC.MyChar.GuildRank == Features.GuildRank.GuildLeader && Features.GuildWars.LastWinner == GC.MyChar.MyGuild) || GC.PM)
                        {
                            if (!Features.GuildWars.War)
                            {
                                if (!World.GuildBeast)
                                {
                                    if (GC.MyChar.MyGuild.Fund >= 1000000)
                                    {
                                        GC.MyChar.MyGuild.Fund -= 1000000;
                                        World.GuildBeast = true;
                                        AddText("Thanks you for your contribution! The GuildBeast will spawn at 20:00! Make sure you get ready to fight it!");
                                        AddOption("Thanks", 255);
                                        break;
                                    }
                                    else
                                    {
                                        AddText("You don't have enough guild fund!");
                                        AddOption("I see", 255);
                                        break;
                                    }
                                }
                                else
                                {
                                    AddText("You have already paid to summon the GuildBeast! It will appear at 20:00!");
                                    AddOption("I see", 255);
                                    break;
                                }
                            }
                            else
                            {
                                AddText("The GuildBeast cannot be spawned during the Guild War!");
                                AddOption("I see", 255);
                                break;
                            }
                        }
                        else if (Features.GuildWars.LastWinner == GC.MyChar.MyGuild && GC.MyChar.GuildRank != Features.GuildRank.GuildLeader)
                        {
                            AddText("Only the leader of the guild that won the Guild War can summon the Guild Beast!");
                            AddOption("I see", 255);
                            break;
                        }
                        else
                        {
                            AddText("Only the leader of the guild that won the Guild War can summon the Guild Beast!");
                            AddOption("I see", 255);
                            break;
                        }
                    }
            }

            AddFinish();
            Send();
        }
    }
}