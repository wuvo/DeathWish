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
    public class NPC_1003 : NPCBase
    {
        public NPC_1003(Main.GameClient _client)
            : base(_client)
        {
            ID = 1003;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("I can give you free double experience if you are below Level70 non reborn!~Vip4~Vip5~or~Vip6~Players~can~~get~unlitimed~free~exppotion.");
                        AddOption("Gimme~me~it!", 1);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 1:
                    {
                        if (GC.MyChar.VipLevel >= 4)
                        {
                            GC.MyChar.ExpPotionUsed = DateTime.Now;
                            GC.MyChar.DoubleExp = true;
                            GC.MyChar.DoubleExpLeft = 3600;
                            GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));
                            GC.MyChar.MyClient.LocalMessage(2005, "Congratulations you received one hour of Double Experience!");
                        }
                        else if ((GC.MyChar.Level < 70 && !GC.MyChar.Reborn))
                        {
                                GC.MyChar.ExpPotionUsed = DateTime.Now;
                                GC.MyChar.DoubleExp = true;
                                GC.MyChar.DoubleExpLeft = 3600;
                                GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));
                                GC.MyChar.MyClient.LocalMessage(2005, "Congratulations you received one hour of Double Experience!");   
                        }
                        else
                        {
                            AddText("I'm sorry but you're either level 90 or above. I can't help you.");
                            AddOption("Oh dear!", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}