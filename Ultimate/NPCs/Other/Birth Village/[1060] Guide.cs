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
    public class NPC_1060 : NPCBase
    {
        public NPC_1060(Main.GameClient _client)
            : base(_client)
        {
            ID = 1060;
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
                        AddText("Hi,~are~you~new~to~Conquer~Online?~What~can~I~do~for~you?");
                        AddOption("Where~am~I?", 1);
                        break;
                    }
                case 1:
                    {
                        AddText("This~is~the~way~to~Birth~Village,~where~people~can~teach~you~some~basic~knowledge~of~the~game~and~your~profession.");
                        AddOption("How~to~go~to~the~village?", 2);
                        AddOption("I~want~to~skip~the~Birth~Village~and~start~playing.", 3);
                        break;
                    }
                case 2:
                    {
                        AddText("Follow~the~stone~path~all~the~way~to~the~right.~You~can~switch~to~run~by~click~on~the~Walk/Run~button~on~the~left");
                        AddOption("Cool.~Please~send~me~to~the~village.", 4);
                        AddOption("Thanks.~I`d~rather~go~by~myself.", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.Job == 100 && !GC.MyChar.Skills.ContainsKey((ushort)1000))
                        {
                            AddText("You`re~a~Taoist!~I`ll~recommend~you~to~TaoistStar.~He~can~teach~you~some~basic~spells.");
                            AddOption("Thanks~a~lot.", 4);
                        }
                        else
                        {
                            GC.MyChar.Teleport(1002, 430, 380);
                            for (int a = 0; a < 3; a++)
                                GC.MyChar.AddItem(1060020);

                            GC.MyChar.ExpPotionUsed = DateTime.Now;
                            GC.MyChar.DoubleExp = true;
                            if ((GC.MyChar.Level < 70 && !GC.MyChar.Reborn))
                                GC.MyChar.ExpPotUnder70 = true;
                            GC.MyChar.DoubleExpLeft = 3600;
                            GC.MyChar.MyClient.AddSend(Packets.Status(GC.MyChar.EntityID, Status.DoubleExpTime, (ulong)GC.MyChar.DoubleExpLeft));

                            if (DateTime.Now > GC.MyChar.VIPStarted.AddDays(3) || GC.MyChar.VIPDays == 0)
                            GC.MyChar.VIPStarted = DateTime.Now;
                            GC.MyChar.VipLevel = 4;
                            GC.MyChar.VIPDays += 3;

                            AddText("I~give~you~a~Coat,~weapon~,~some~healing~potions~30~minute~of~EXPPotion~and~3Day~Vip4.~Wish~you~a~pleasant~journey\n");
                            AddText("if you want to get Free Elite/Super Items you need to go TC Captain NPC and kill some monsters.");
                            AddOption("I~see.~Thanks.", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        GC.MyChar.Teleport(1010, 83, 59);
                        AddText("Here~it~is.~You~see~the~flickering~PK~in~you~lower~screen?~It~means~you~are~in~PK~mode.~Switch~it~to~Peace.~By~the");
                        AddOption("Thanks~a~lot.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}