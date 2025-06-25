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
    public class NPC_10005 : NPCBase
    {
        public NPC_10005(Main.GameClient _client)
            : base(_client)
        {
            ID = 10005;
            Face = 9;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I~am~selling~different~weapons~in~different~city.~In~order~to~slay~the~enemies,~you~had~better~equip~the~best~weapons.");
                        AddOption("How~to~buy~and~sell?", 1);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Before~you~buy~an~item,~you`d~better~check~its~stats.~Red~stats~means~you~cannot~wear~it~until~all~stats~are~white.");
                        AddText("Click~on~me~to~open~my~shop~window,~and~then~right~click~on~an~items~to~buy~it.~To~equip~it,~just~right~click~on~it.");
                        AddText("If~you~want~to~sell~an~item,~you~may~click~on~me,~then~drag~and~drop~your~item~into~my~slots.~I~have~multipage~items.");
                        AddOption("How~to~repair?", 2);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("Disarm~your~item,~click~on~a~shopkeeper,~then~click~on~repair~button~and~your~item.~The~better~the~quality,~the~higher~the~fee.");
                        AddOption("What~are~super~items?", 3);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("Items~are~graded~as~normal,~refined,~unique,~elite~and~super.~NPC~sells~only~normal~items.~Mobs~gives~better~ones.");
                        AddText("That~is~all.~If~you~have~not~talked~to~other~NPCs,~you~had~better~have~a~chat~with~them~so~that~you~can~learn~more.");
                        AddOption("Consult~others.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}