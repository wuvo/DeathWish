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
    public class NPC_30001 : NPCBase
    {
        public NPC_30001(Main.GameClient _client)
            : base(_client)
        {
            ID = 30001;
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
                        AddText("Lighting up your passion for sports! chasing after your glory!  If you have got Medals, you can come to me to exchange for nice prizes.");
                        AddOption("I want exchange for prizes", 1);
                        AddOption("No thanks", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Delicate medals are not only the evidence of your superior strenth, but also the  the evidence of your extraordinary courage. If you have got Medals in the competition, you can come to me and exchange for nice prizes. ");
                        AddText(", MiraculousGourd For [3] LoardToken");
                        AddText(", MagicalBottle For [1] LoardToken");
                        AddOption("I want to claim MiraculousGourd.", 2);
                        AddOption("I want to claim MagicalBottle.", 3);
                        AddOption("I'm just asking", 255);
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(723467, 3))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723467));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723467));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723467));
                            GC.MyChar.AddItem(2100025);

                        }
                        else
                        {
                            AddText("It seems that there is no medal of this class in your inverntory.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(723467, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723467));
                            GC.MyChar.AddItem(2100045);

                        }
                        else
                        {
                            AddText("It seems that there is no medal of this class in your inverntory.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}