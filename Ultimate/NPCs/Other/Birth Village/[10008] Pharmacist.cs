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
    public class NPC_10008 : NPCBase
    {
        public NPC_10008(Main.GameClient _client)
            : base(_client)
        {
            ID = 10008;
            Face = 1;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hi!~I~am~selling~all~kinds~of~potions~and~City~Gate~Scrolls~in~the~cities.~I~also~sell~fireworks~and~skill~books~in~the~market.");
                        AddOption("What~potions?", 1);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Healing~and~mana~potions.~Healing~potions~can~make~you~healthy,~and~mana~potions~will~enable~you~to~cast~spells.");
                        AddText("That~is~all.~If~you~have~not~talked~to~other~NPCs,~you~had~better~have~a~chat~with~them~so~that~you~can~learn~more.");
                        AddOption("I~see.~Thanks.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}