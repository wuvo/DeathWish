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
    public class NPC_10006 : NPCBase
    {
        public NPC_10006(Main.GameClient _client)
            : base(_client)
        {
            ID = 10006;
            Face = 10;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Welcome!~I~run~a~warehouse~in~every~city.~You~can~store~your~money~and~items~in~my~warehouses,~and~retrieve~them~for~free.");
                        AddOption("How~to~use~the~warehouse.", 1);
                        AddOption("Consult~others.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("To~deposit~money,~just~click~on~me,~enter~the~amount,~and~then~click~on~Deposit.~Withdrawing~is~in~the~same~way.");
                        AddText("To~store~an~item,~just~drag~it~to~my~slots~and~then~release~it.~To~take~it~out,~just~click~on~it.~There~is~one");
                        AddText("warehouse~available~in~each~city~and~the~market.~You~had~better~store~your~valuable~items~and~do~not~carry~too~much~money.");
                        AddOption("I~see.~Thanks.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}