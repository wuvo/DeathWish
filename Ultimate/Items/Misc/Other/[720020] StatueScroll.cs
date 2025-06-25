using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;
using Ultimate.Features;

namespace Ultimate.Items
{
    public class Item_720020 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.MyGuild != null && GuildWars.LastWinner == C.MyGuild && (C.GuildRank == GuildRank.DeputyManager || C.GuildRank == GuildRank.GuildLeader))
            {
                foreach (SOB S in World.H_SOBs.Values)
                {
                    if (S.Name == C.Name)
                        SOB.GuildStatue.RemoveStatue(S);
                    else if (S.GuildRank == (byte)C.GuildRank && S.GuildID != C.MyGuild.GuildID)
                        SOB.GuildStatue.RemoveStatue(S);
                }
                C.MyClient.AddSend(Packets.StatueWindow(C, 3, 1130, 5, 9));
            }
            else
                C.MyClient.LocalMessage(2005, "Only the GuildLeader and DeputyLeaders of the guild that owns the pole can summon their statues.");
        }
    }
}