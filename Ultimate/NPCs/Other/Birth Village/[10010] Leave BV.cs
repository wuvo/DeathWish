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
    public class NPC_10010 : NPCBase
    {
        public NPC_10010(Main.GameClient _client)
            : base(_client)
        {
            ID = 10010;
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
                        if (GC.MyChar.Job == 100 && !GC.MyChar.Skills.ContainsKey((ushort)1000))
                        {
                            AddText("The~circle~is~very~dangerous.~You~had~better~ask~Taoist~Star~to~teach~you~elementary~spells~before~you~leave~Birth~Village.");
                            AddOption("I~see.~Thanks.", 255);
                        }
                        else
                        {
                            AddText("This~is~the~way~to~Twin~City.~Many~people~are~gathering~there.~Shall~I~give~you~some~advice~before~I~teleport~you~there.");
                            AddOption("Yes,~please.", 2);
                            AddOption("Teleport~me~to~Twin~City.", 1);
                            AddOption("Consult~others.", 255);
                        }
                        break;
                    }
                case 1:
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
                            GC.MyChar.VipLevel = 5;
                            GC.MyChar.VIPDays += 5;

                            AddText("I~give~you~a~Coat,~weapon~,~some~healing~potions~30~minute~of~EXPPotion~and~3Day~Vip4.~Wish~you~a~pleasant~journey\n");
                            AddText("if you want to get Free Elite/Super Items you need to go TC Captain NPC and kill some monsters.");
                            AddOption("I~see.~Thanks.", 255);
                        }
                        break;
                    }
                case 2:
                    {
                        AddText("Make~good~use~of~Hot~Key~(F1-F10)~will~save~you~a~lot~of~troubles.~You~can~drag~potions~and~spells~to~F1-F10.~Then");
                        AddText("you~just~press~on~the~corresponding~key~on~your~keyboard~to~use~it.~You~like~PK?~Be~careful.~If~you~get~blue~even");
                        AddText("black~name~after~you~kill~other~players,~you~will~lose~the~equipments~you~are~wearing.~Remember~to~switch~PK~to~Peace.");
                        AddText("That~is~all.~Have~a~good~journey.");
                        AddOption("Thanks.", 255);
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}