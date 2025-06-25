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
    public class NPC_2039 : NPCBase
    {
        public NPC_2039(Main.GameClient _client)
            : base(_client)
        {
            ID = 2039;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Hello ! I know you worked hard by digging here.. I can make some things easier for you.. i need some ores and normal gems do you have ?");
                    AddOption("Yes, i have too much", 9);
                    AddOption("I will try to find.", 255);
                    break;
                case 9:
                    AddText("You won't regret it. You can win nice prizes here. RafinedGems, ExpPotions, Meteors, Metscrolls, Expballs, Money, ProffTokens, Someitems and you can even find DBs!");
                    AddOption("I found what you were looking for (" + World.ERPts + " Pts left)", 27);
                    //AddOption("25% Drop Rates (" + World.DRPts + " Pts left)", 4);
                    AddOption("I will continue to mining.", 255);
                    break;
                case 27:
                    {
                        AddOption("5 Ores (25 Points)", 15);
                        //AddOption("DragonBall (10 Points)", 20);
                        //AddOption("MetscrollBag (100 Points)", 100);
                        //AddOption("DBScroll (100 Points)", 200);
                        AddOption("Normal Gem (25 Points)", 14);
                        //AddOption("Just passing by.", 255);
                        break;
                    }
                case 14:
                    {
                        AddText("Each Normal Gem equals 25 Points! Which Super Gem would you like to contribute?");
                        AddOption("Dragon Gem", 2);
                        AddOption("Phoenix Gem", 1);
                        AddOption("Violet Gem", 6);
                        AddOption("Rainbow Gem", 4);
                        AddOption("Moon Gem", 7);
                        AddOption("Kylin Gem", 5);
                        AddOption("Fury Gem", 3);
                        AddOption("I changed my mind", 255);
                        break;

                    }

                case 15:
                    {
                        var Amount = 0;
                        for (int a = 0; a < 60; a++)
                            Amount += GC.MyChar.InventoryItemIDCount((uint)(1072010 + a));

                        if (Amount >= 5)
                        {
                            Amount = 5;
                            for (int a = 0; a < 60; a++)
                            {
                                var Count = GC.MyChar.InventoryItemIDCount((uint)(1072010 + a));
                                for (int b = 0; b < Count; b++)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem((uint)(1072010 + a)));
                                    Amount--;
                                    if (Amount == 0)
                                        break;
                                }
                                if (Amount == 0)
                                    break;
                            }


                            AddText("Congratulations ! You've contributed to the global events with a Ores and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got a 250K.");
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }

                        }
                        else
                        {
                            AddText("Sorry you dont have 5 Ores!");
                            AddOption("Thanks", 255);
                        }
                        break;
                    }



                case 1:
                    if (GC.MyChar.InventoryContains(700001, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700001));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the PhoenixGem!");
                        AddOption("Alright", 255);
                    }
                    break;




                case 2:
                    if (GC.MyChar.InventoryContains(700011, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700011));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the DragonGem!");
                        AddOption("Alright", 255);
                    }
                    break;

                case 3:
                    if (GC.MyChar.InventoryContains(700021, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700021));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 50 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the FuryGem!");
                        AddOption("Alright", 255);
                    }
                    break;

                case 4:
                    if (GC.MyChar.InventoryContains(700031, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700031));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 50 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the RainbowGem!");
                        AddOption("Alright", 255);
                    }
                    break;

                case 5:
                    if (GC.MyChar.InventoryContains(700041, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700041));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the KylinGem!");
                        AddOption("Alright", 255);
                    }
                    break;

                case 6:
                    if (GC.MyChar.InventoryContains(700051, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700051));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the Violet Gem!");
                        AddOption("Alright", 255);
                    }
                    break;


                case 7:
                    if (GC.MyChar.InventoryContains(700061, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(700061));
                            AddText("Congratulations ! You've contributed to the global events with a Gems and donated 25 Points!");
                            AddOption("Thanks", 255);
                            if (World.ERPts > 25)
                                World.ERPts -= 25;
                            else
                            {
                                World.ERPts = 1000;


                                Random Rnd = new Random();
                                switch (Rnd.Next(0, 21))
                                {
                                    case 0:
                                        GC.MyChar.AddItem(720027); //Metscroll
                                        GC.LocalMessage(2000, "You got a DemonBox.");
                                        break;
                                    case 1:
                                        GC.MyChar.AddItem(1088000); //Dragonball
                                        GC.LocalMessage(2000, "You got a MiniExpPot.");
                                        break;
                                    case 2:
                                        GC.MyChar.AddItem(720665); //250k
                                        GC.LocalMessage(2000, "You got an AncientBox .");
                                        break;
                                    case 3:
                                        GC.MyChar.AddItem(720666); //500k
                                        GC.LocalMessage(2000, "You got 500k Silvers.");
                                        break;
                                    case 4:
                                        if (GC.MyChar.Level < 130)
                                            GC.MyChar.AddExp(2);
                                        else
                                            GC.MyChar.AddItem(720670); //exp
                                        break;
                                    case 5:
                                        GC.MyChar.AddItem(700002); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined PhoenixGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref PhoenixGem!", 2011, 0);
                                        break;
                                    case 6:
                                        GC.MyChar.AddItem(700012); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined DragonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref DragonGem!", 2011, 0);
                                        break;
                                    case 7:
                                        GC.MyChar.AddItem(700022); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined FuryGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref FuryGem!", 2011, 0);
                                        break;
                                    case 8:
                                        GC.MyChar.AddItem(700032); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined RainbowGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref RainbowGem!", 2011, 0);
                                        break;
                                    case 9:
                                        GC.MyChar.AddItem(722384); //proftoken
                                        GC.LocalMessage(2000, "You got a ProficiencyToken.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref ProficiencyToken!", 2011, 0);
                                        break;
                                    case 10:
                                        GC.MyChar.AddItem(700042); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined KylinGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref KylinGem!", 2011, 0);
                                        break;
                                    case 11:
                                        GC.MyChar.AddItem(700052); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined VioletGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref VioletGem!", 2011, 0);
                                        break;
                                    case 12:
                                        GC.MyChar.AddItem(700062); //refgem
                                        GC.LocalMessage(2000, "You got a Rafined MoonGem.");
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Ref MoonGem!", 2011, 0);
                                        break;
                                    case 13:
                                        GC.MyChar.AddItem(1088000);
                                        World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has finished 1000 Points for mine cave and won Dragonball!", 2011, 0);
                                        break;
                                    case 14:
                                        GC.MyChar.AddItem(723017); //exppotion
                                        GC.LocalMessage(2000, "You got a ExpPotion.");
                                        break;
                                    case 15:
                                        GC.MyChar.AddItem(720027); //metscroll
                                        GC.LocalMessage(2000, "You got a MetScroll.");
                                        break;
                                    case 16:
                                        for (int a = 0; a < 5; a++)
                                            GC.MyChar.AddItem(1088001); //met
                                        GC.LocalMessage(2000, "You got meteors.");
                                        break;
                                    case 17:
                                        GC.MyChar.AddItem(721541); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 18:
                                        GC.MyChar.AddItem(721542); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 19:
                                        GC.MyChar.AddItem(721543); //uniquemet
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                    case 20:
                                        GC.MyChar.AddItem(721544); //ref met
                                        GC.LocalMessage(2000, "You got an ItemBox.");
                                        break;
                                        //case 21:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(1);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 22:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(3);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 23:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(4);
                                        //    else
                                        //        GC.MyChar.AddItem(720658); //1/6 exp
                                        //    GC.LocalMessage(2000, "You got 1/6 Expball.");
                                        //    break;
                                        //case 24:
                                        //    if (GC.MyChar.Level < 130)
                                        //        GC.MyChar.AddExp(5);
                                        //    else
                                        //        GC.MyChar.AddItem(720664); //5/6 exp
                                        //    GC.LocalMessage(2000, "You got 5/6 Expball.");
                                        //    break;
                                        //case 28:
                                        //    GC.MyChar.VotePoints++;
                                        //    GC.MyChar.MyClient.LocalMessage(2005, "You have received a Vote Point!"); //votepoint
                                        //    break;
                                        //case 29:
                                        //    GC.MyChar.AddItem(721954); //ratling purple
                                        //    break;
                                        //case 30:
                                        //    GC.MyChar.AddItem(721246); //ccgw
                                        //    break;
                                }



                            }
                        }
                        else
                        {
                            AddText("Please make sure you have 5 free slot in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have the MoonGem!");
                        AddOption("Alright", 255);
                    }
                    break;





            }
            AddFinish();
            Send();
        }
    }
}
