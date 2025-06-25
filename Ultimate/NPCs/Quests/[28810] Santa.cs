using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Bosses
{
    public class MobID_28810 : MobBase
    {
        public MobID_28810(Mob _mob)
            : base(_mob)
        {
            ID = 28810;
        }


        public override void Run(Character C, Mob _mob, uint _damage, List<Character> PlayerTargets)
        {
            base.Run(C, _mob, _damage, PlayerTargets);
            {
                NPC N = null;
                Dictionary<uint, NPC> MapNPC = World.H_NPCs[C.Loc.Map];
                if (MapNPC != null && MapNPC.ContainsKey(ID)/* || NPC == 12*/)
                    N = (NPC)MapNPC[ID];
                MapNPC.Remove(ID);
                Game.World.Action(N, Packets.GeneralData(ID, 0, 0, 0, 135).Get);
                Game.World.Found = true;
                C.StatEff.Add(StatusEffectEn.Flashy);
            }
        }
    }
}
