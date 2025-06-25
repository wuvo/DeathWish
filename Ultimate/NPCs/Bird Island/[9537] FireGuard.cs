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
    public class NPC_9537 : NPCBase
    {
        public NPC_9537(Main.GameClient _client)
            : base(_client)
        {
            ID = 9537;
            Face = 30;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello, I am the Fire Guard, if you are a Fire I can take you to the next island so you can try to");
                        AddText(" defeat the monster that steal the amulet. Once you're done you'll have to give me the Cert. and I'll give you the Amulet in exchange.");
                        AddOption("Take me in", 1);
                        AddOption("I've got the FireCert.", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Job >= 140 && GC.MyChar.Job <= 145)
                        {
                            GC.MyChar.Teleport(1082, 295, 168);
                        }
                        else
                        {
                            AddText("You're not a Fire!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(710018, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710018));
                            GC.MyChar.AddItem(1200009);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems like you don't have a FireCert.! Get inside and kill the Devil for it!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}