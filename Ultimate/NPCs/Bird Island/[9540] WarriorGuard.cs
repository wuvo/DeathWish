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
    public class NPC_9540 : NPCBase
    {
        public NPC_9540(Main.GameClient _client)
            : base(_client)
        {
            ID = 9540;
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
                        AddText("Hello, I am the Warrior Guard, if you are a Warrior I can take you to the next island so you can try to");
                        AddText(" defeat the monster that steal the amulet. Once you're done you'll have to give me the Cert. and I'll give you the Amulet in exchange.");
                        AddOption("Take me in", 1);
                        AddOption("I've got the WarriorCert.", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.Job >= 20 && GC.MyChar.Job <= 25)
                        {
                            GC.MyChar.Teleport(1082, 152, 086);
                        }
                        else
                        {
                            AddText("You're not an Warrior!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(710016, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710016));
                            GC.MyChar.AddItem(1200007);
                        }
                        else
                        {
                            AddText("I'm sorry but it seems like you don't have a WarriorCert.! Get inside and kill the Devil for it!");
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