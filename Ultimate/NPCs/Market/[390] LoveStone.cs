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
    public class NPC_390 : NPCBase
    {
        public NPC_390(Main.GameClient _client)
            : base(_client)
        {
            ID = 390;
            Face = 6;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Sometimes, you may feel lonely. Yeah, you do need someone to be with you. I can understand that. Now answer me, have you ever ");
                    AddText("fallen in love with someone?");
                    AddOption("Maybe, what should I do?", 1);
                    AddOption("Nevermind", 255);
                    break;
                case 1:
                    if (GC.MyChar.Spouse != "None")
                    {
                        AddText("It seems you've already found your true love. Be sure to cherish them daily");
                        AddOption("I will", 255);
                    }
                    else
                    {
                        AddText("Marriage is not a decision to be taken lightly and means spending your life with your lover. Are you positive you've found your mate?");
                        AddOption("Positive!", 2);
                        AddOption("Maybe not...", 255);
                    }
                    break;
                case 2:
                    {
                        AddText("By sending this flower to your lover you are giving yourself to them. They may chose to accept or reject your offer.");
                        AddOption("Thank you", 255);
                        GC.AddSend(Packets.GeneralData(GC.MyChar.EntityID, 1067, 0, 0, 116));
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}