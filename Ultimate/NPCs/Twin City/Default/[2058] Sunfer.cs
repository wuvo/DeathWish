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
    public class NPC_2058 : NPCBase
    {
        public NPC_2058(Main.GameClient _client)
            : base(_client)
        {
            ID = 2058;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            Character OnBounty = World.CharacterFromName2(GC.MyChar.OnBounty);
            if (_linkback == 0)
            {
                AddText("Everyone has secrets to hide and people they hate. If you want someone dead, but dare not kill them yourself, or you don't think it worth while, I will settle it for you.");
                AddText(" I charge a minimum of 1,000,000 silvers. Tell me his her name.\nIf you think your skill is fine and you need some money, I would like to hire you as");
                AddText(" a bounty hunter. You will gain rewards for your work. If you are the target of a bounty, I can settle it if you pay me double bounty.");

                if (GC.MyChar.KilledBounty)
                    AddOption("Claim bounty reward", 9);
                else
                {
                    AddOption("Offer to kill an enemy", 1);
                    AddOption("Accept an offer", 2);
                    AddOption("Cancel an offer", 3);
                    AddOption("Add reward", 4);
                }
                AddOption("Just passing by", 255);
                GC.MyChar.Page = 0;
            }
            else if (_linkback == 1)
            {
                AddText("Who would you like to put a bounty on?");
                AddInput("Put a bounty on:", 5);
                AddOption("I changed my mind", 255);
            }
            else if (_linkback == 5)
            {
                string BountyName = ReadString(_data);
                if (BountyName == GC.MyChar.Name)
                {
                    AddText("I'm sorry but you can't place a bounty on yourself!");
                    AddOption("I see", 255);
                }
                else
                {
                    Character BountyChar = World.CharacterFromName2(BountyName);
                    if (BountyChar != null)
                    {
                        if (World.Bounty.ContainsKey(BountyName))
                        {
                            AddText("This player already has a bounty on his/her head. Do you want to increase it?");
                            AddOption("Yes", 4);
                            AddOption("I changed my mind", 255);
                        }
                        else
                        {
                            GC.MyChar.OnBounty = BountyName;
                            AddText("How much gold would you like to place as a bounty for your target?");
                            AddInput("Bounty:", 6);
                            AddOption("I changed my mind", 255);
                        }
                    }
                    else
                    {
                        AddText("The character you're trying to put a bounty on is not online or doesn't exist");
                        AddOption("I see", 255);
                    }
                }
            }
            else if (_linkback == 6)
            {
                uint Bounty = Convert.ToUInt32(ReadString(_data));
                if (Bounty < 1000000)
                {
                    AddText("You can't place a bounty lower than 5,000,000 silvers.");
                    AddOption("I see", 255);
                }
                else if (GC.MyChar.Silvers < Bounty)
                {
                    AddText("You don't have enough silvers with you! Please get some before placing a bounty.");
                    AddOption("I see", 255);
                }
                else
                {
                    if (World.Bounty.ContainsKey(GC.MyChar.OnBounty))
                    {
                        AddText("This player already has a bounty on his/her head. Do you want to increase it?");
                        AddOption("Yes", 4);
                        AddOption("I changed my mind", 255);
                    }
                    else
                    {
                        GC.MyChar.Silvers -= Bounty;
                        World.Bounty.Add(GC.MyChar.OnBounty, Bounty);
                        World.SendMsgToAll("SYSTEM", "Someone placed a bounty on " + GC.MyChar.OnBounty + "'s head! Sunfer is looking for brave people to take the job!", 2000, 0);
                        AddText(GC.MyChar.OnBounty + " now has a bounty of " + Bounty + " silvers on his/her head! I'll make sure the job is done!");
                        AddOption("Thanks", 255);
                        GC.MyChar.OnBounty = "";
                    }
                }
            }
            else if (_linkback == 2)
            {
                if (GC.MyChar.KilledBounty)
                {
                    AddText("You have killed a wanted player and didn't receive your reward yet! Please claim it before taking another offer!");
                    AddOption("Claim bounty reward", 9);
                }
                else
                {
                    byte Pages = Convert.ToByte(World.Bounty.Count / 6);
                    byte CurControl = 30;
                    byte _static = 30;
                    AddText("I'll keep 10% of the bounty for myself! Which bounty would you like to take?");
                    foreach (var Name in World.Bounty.OrderByDescending(s => s.Value))
                    {
                        if (CurControl >= (_static + (6 * GC.MyChar.Page)) && CurControl <= (_static + (6 * GC.MyChar.Page) + 5))
                            AddOption(Name.Key + " - " + Name.Value, CurControl);
                        //Page 0: CurControl >= 30 && CurControl <= 34
                        //Page 1: CurControl >= 35 && CurControl <= 39
                        //Page 2: CurControl >= 40 && CurControl <= 44

                        CurControl++;
                    }
                    if (Pages > GC.MyChar.Page)
                        AddOption("Next", 2);

                    AddOption("Nevermind", 255);
                    GC.MyChar.Page++;
                }
            }
            else if (_linkback >= 30)
            {
                byte Player = 30;
                foreach (string Name in World.Bounty.Keys)
                {
                    if (Player == _linkback)
                    {
                        if (GC.Agreed)
                        {
                            GC.MyChar.OnBounty = Name;
                            AddText("How much would you like to contribute with to " + GC.MyChar.OnBounty + "'s bounty?");
                            AddInput("Add:", 15);
                            GC.MyChar.KilledBounty = false;
                            GC.Agreed = false;
                        }
                        else
                        {
                            if (Name == GC.MyChar.Name)
                            {
                                AddText("You can't accept a bounty that's on you. Do you want to remove it?");
                                AddOption("Yeah", 3);
                                AddOption("Nevermind", 255);
                                GC.MyChar.KilledBounty = false;
                            }
                            else
                            {
                                GC.MyChar.OnBounty = Name;
                                AddText("You have accepted this bounty and you're now hunting " + GC.MyChar.OnBounty + "! You may come back to me after you kill him/her!");
                                AddOption("I will", 255);
                                GC.MyChar.KilledBounty = false;
                            }
                        }
                    }
                    Player++;
                }
            }
            else if (_linkback == 9)
            {
                if (GC.MyChar.KilledBounty)
                {
                    if (World.Bounty.ContainsKey(GC.MyChar.OnBounty))
                    {
                        if (GC.MyChar.Silvers <= (999999999 - Convert.ToUInt32(World.Bounty[GC.MyChar.OnBounty] * 0.9)))
                        {
                            AddText("You have received " + Convert.ToUInt32(World.Bounty[GC.MyChar.OnBounty] * 0.9) + " silvers for completing a job for me and killing " + GC.MyChar.OnBounty + "!");
                            AddOption("Thanks", 255);
                            GC.MyChar.Silvers += Convert.ToUInt32(World.Bounty[GC.MyChar.OnBounty] * 0.9);
                            World.Bounty.Remove(GC.MyChar.OnBounty);
                            GC.MyChar.OnBounty = "";
                            GC.MyChar.KilledBounty = false;
                        }
                    }
                }
                else
                {
                    AddText("It seems like you didn't kill " + GC.MyChar.OnBounty + " yet! You may come back to me after you kill him/her!");
                    AddOption("I will", 255);
                }
            }
            else if (_linkback == 4)
            {
                byte Pages = Convert.ToByte(World.Bounty.Count / 7);
                byte CurControl = 30;
                byte _static = 30;
                AddText("Which bounty would you like to contribute to?");
                foreach (var Name in World.Bounty.OrderByDescending(s => s.Value))
                {
                    if (CurControl >= (_static + (6 * GC.MyChar.Page)) && CurControl <= (_static + (6 * GC.MyChar.Page) + 5))
                        AddOption(Name.Key + " - " + Name.Value, CurControl);

                    CurControl++;
                }
                if (Pages > GC.MyChar.Page)
                    AddOption("Next", 2);

                AddOption("Nevermind", 255);
                GC.MyChar.Page++;
                GC.Agreed = true;
            }
            else if (_linkback == 15)
            {
                uint Money = Convert.ToUInt32(ReadString(_data));
                if (GC.MyChar.Silvers >= Money)
                {
                    GC.MyChar.Silvers -= Money;
                    World.Bounty[GC.MyChar.OnBounty] += Money;
                    AddText("You have added " + Money + " silvers to " + GC.MyChar.OnBounty + "'s bounty!");
                    AddOption("Thanks", 255);
                    GC.MyChar.OnBounty = "";
                    GC.MyChar.KilledBounty = false;
                }
                else
                {
                    AddText("You don't have enough silvers!");
                    AddOption("I see", 255);
                    GC.MyChar.OnBounty = "";
                    GC.MyChar.KilledBounty = false;
                }
            }
            else if (_linkback == 3)
            {
                if (World.Bounty.ContainsKey(GC.MyChar.Name))
                {
                    AddText("You can remove the bounty that's on your head by paying me " + (World.Bounty[GC.MyChar.Name] * 2) + " silvers! Do you want to do it?");
                    AddOption("Yeah", 16);
                    AddOption("Nevermind", 255);
                }
                else
                {
                    AddText("It seems like you don't have a bounty on your head...");
                    AddOption("I see", 255);
                }
            }
            else if (_linkback == 16)
            {
                if (World.Bounty.ContainsKey(GC.MyChar.Name))
                {
                    if (GC.MyChar.Silvers >= (World.Bounty[GC.MyChar.Name] * 2))
                    {
                        GC.MyChar.Silvers -= Convert.ToUInt32(World.Bounty[GC.MyChar.Name] * 2);
                        World.Bounty.Remove(GC.MyChar.Name);
                        AddText("You have successfully removed the bounty you had on your head!");
                        AddOption("Great!", 255);
                    }
                    else
                    {
                        AddText("It seems like you don't have enough money to cover the bounty on your head...");
                        AddOption("I see", 255);
                    }
                }
            }

            AddFinish();
            Send();
        }
    }
}