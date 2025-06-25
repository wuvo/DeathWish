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
    public class NPC_300000 : NPCBase
    {
        public NPC_300000(Main.GameClient _client)
            : base(_client)
        {
            ID = 300000;
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
                        AddText("I hope everyone can help eachother. If you powerlevel the newbies, I may reward you with Experience, Meteors or even DragonBalls.");
                        AddOption("Tell me more details", 8);
                        AddOption("Check my Virtue Points", 1);
                        AddOption("Claim prize", 2);
                        AddOption("Just passing by.", 255);
                        break;
                    }
                case 1:
                    {
                        AddText("Your current virtue points are " + GC.MyChar.VP + ". Please try to earn more and exchange them for rewards.");
                        AddOption("Thanks!", 255);
                        break;
                    }
                case 8:
                    {
                        AddText("If you are above level 70 and try to powerlevel the newbies (at least 20 levels lower than you), you will gain virtue points which can be exchanged for rewards.");
                        AddOption("How can I gain virtue points?", 9);
                        AddOption("What rewards can I expect?", 10);
                        break;
                    }
                case 9:
                    {
                        AddText("All you have to do is create a team and go powerlevel the newbies in your team. You must be above level 70 and the newbies must be at least 20 levels below you and below level 70. Once they level up you will gain virtue points which you can use to either enter the labyrinth or exchange for rewards with me.");
                        AddOption("What rewards can I expect?", 10);
                        AddOption("Thanks", 255);
                        break;
                    }
                case 10:
                    {
                        AddText("I will reward you the experience equivalent to an ExpBall for 30,000 virtue points, a Meteor for 2,500 virtue points, a DragonBall for 50,000 virtue points and a +2 non-weapon item for 200,000 virtue points!");
                        AddOption("Thanks", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("You can trade your " + GC.MyChar.VP + " virtue points for any of these prizes.");
                        //AddOption("ExpPotion(50,000)", 7);
                        AddOption("MeteorScroll(20,000)", 4);
                        AddOption("DragonBall(150,000)", 3);
                        AddOption("Random non-weapon +2(150,000)", 5);
                        AddOption("Let me think", 255);
                        break;
                    }
                case 7:
                    {
                        if (GC.MyChar.Level < 130)
                        {
                            ulong Price = 50000;
                            if (GC.MyChar.VP >= Price)
                            {
                                GC.MyChar.VP -= Price;
                                GC.MyChar.AddItem(723017);
                                AddText("Congratulations! You have received ExpPotion in exchange of your virtue points!");
                                AddOption("Thanks.", 255);
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have enough virtue points.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You already reached the highest level. I can't help you any further.");
                            AddOption("Thanks.", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.Inventory.Count <= 39)
                        {
                            ulong Price = 150000;
                            if (GC.MyChar.VP >= Price)
                            {
                                GC.MyChar.VP -= Price;
                                GC.MyChar.AddItem(1088000);
                                AddText("Congratulations! You have received your prize.");
                                AddOption("Thanks.", 255);
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have enough virtue points.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You do not have one free slot in your inventory.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.Inventory.Count <= 39)
                        {
                            ulong Price = 20000;
                            if (GC.MyChar.VP >= Price)
                            {
                                GC.MyChar.VP -= Price;
                                GC.MyChar.AddItem(720027);
                                AddText("Congratulations! You have received your prize.");
                                AddOption("Thanks.", 255);
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have enough virtue points.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You do not have one free slot in your inventory.");
                            AddOption("I see.", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.Inventory.Count <= 39)
                        {
                            ulong Price = 150000;
                            if (GC.MyChar.VP >= Price)
                            {
                                GC.MyChar.VP -= Price;
                                PlusItemReward(2, 1);
                                AddText("Congratulations! You have received your prize.");
                                AddOption("Thanks.", 255);
                            }
                            else
                            {
                                AddText("I'm sorry but it seems you don't have enough virtue points.");
                                AddOption("I see.", 255);
                            }
                        }
                        else
                        {
                            AddText("You do not have one free slot in your inventory.");
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