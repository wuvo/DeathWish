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
    public class NPC_10055 : NPCBase
    {
        public NPC_10055(Main.GameClient _client)
            : base(_client)
        {
            ID = 10055;
            Face = 6;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if (GC.MyChar.Job == 100)
                        {
                            AddText("I~can~teach~you~Thunder~and~Cure.~After~you~learn,~you~may~cast~Thunder~to~kill~enemies,~use~Cure~to~heal~yourself~and~others.");
                            AddOption("I~want~to~learn.", 1);
                            AddOption("Just~passing~by.", 255);
                        }
                        else
                        {
                            AddText("Sorry,~you~are~not~Taoist.~I~am~here~to~teach~Taoist~some~elementary~spells.");
                            AddOption("I~see.~Thanks.", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Job == 100 && !GC.MyChar.Skills.ContainsKey((ushort)1000))
                        {
                            GC.MyChar.NewSkill(new Game.Skill() { ID = 1000 });
                            GC.MyChar.NewSkill(new Game.Skill() { ID = 1005 });
                        }
                        AddText("You~have~learned~Thunder~and~Cure.~Please~remember~that~spells~are~only~used~to~punish~the~devils~and~help~the~kind~people.");
                        AddOption("I~see.~Thanks.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}