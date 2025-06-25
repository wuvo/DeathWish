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
    public class NPC_6700 : NPCBase
    {
        public NPC_6700(Main.GameClient _client)
            : base(_client)
        {
            ID = 6700;
            Face = 30;
            IsGlobal = true;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("What do you want to do?");
                        AddOption("Heal the pole", 1);
                        AddOption("Nothing", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (Features.GuildWars.LastWinner != null)
                            {
                                if (GC.MyChar.MyGuild.GuildID == Features.GuildWars.LastWinner.GuildID && (GC.MyChar.GuildRank == Ultimate.Features.GuildRank.GuildLeader || GC.MyChar.GuildRank == Ultimate.Features.GuildRank.DeputyManager))
                                {
                                    AddText("How much of your guild fund are you going to waste? (1 silver = 2 HP)");
                                    AddInput("Heal", 2);
                                    AddOption("I changed my mind", 255);
                                }
                                else
                                {
                                    AddText("You are not authorized to do that.");
                                    AddOption("I see", 255);
                                }
                            }
                        }
                        else
                        {
                            AddText("You are not authorized to do that.");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.MyGuild != null)
                        {
                            if (GC.MyChar.MyGuild.GuildID == Features.GuildWars.LastWinner.GuildID && (GC.MyChar.GuildRank == Ultimate.Features.GuildRank.GuildLeader || GC.MyChar.GuildRank == Ultimate.Features.GuildRank.DeputyManager))
                            {
                                uint Amount = 0;
                                if (uint.TryParse(ReadString(_data), out Amount))
                                {
                                    if (Amount > 0 && Amount <= GC.MyChar.MyGuild.Fund && World.H_SOBs[ID].CurHP < World.H_SOBs[ID].MaxHP)
                                    {
                                        uint ToHeal = World.H_SOBs[ID].MaxHP - World.H_SOBs[ID].CurHP;
                                        if (Amount > ToHeal / 2) Amount = ToHeal / 2;
                                        GC.MyChar.MyGuild.Fund -= Amount;
                                        Amount *= 2;
                                        World.H_SOBs[ID].CurHP += Amount;
                                        World.H_SOBs[ID].ReSpawn();
                                    }
                                }
                                else
                                {
                                    AddText("Please enter a valid amount!");
                                    AddOption("Sorry.", 255);
                                }
                            }
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}