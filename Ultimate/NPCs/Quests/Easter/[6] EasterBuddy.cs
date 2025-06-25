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
    public class NPC_6 : NPCBase
    {
        public NPC_6(Main.GameClient _client)
            : base(_client)
        {
            ID = 6;
            Face = 7;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        if(DateTime.Now.AddDays(5) >= NPC_7.EasterSunday(DateTime.Now.Year) && DateTime.Now <= NPC_7.EasterSunday(DateTime.Now.Year).AddDays(5))
                        {
                            AddText("Howdy dear adventurer! Easter is almost here and it has come the time to start gathering eggs for everyone!");
                            AddText(" Would you be interested in helping me?");
                            AddOption("Tell me more about it", 1);
                            AddOption("Exchange Eggs", 2);
                            AddOption("Just passing by", 255);
                        }
                        else if (DateTime.Now.AddDays(5) < NPC_7.EasterSunday(DateTime.Now.Year))
                        {
                            AddText($"You'll be only able to exchange eggs between {NPC_7.EasterSunday(DateTime.Now.Year).Subtract(new TimeSpan(5, 0, 0, 0, 0)).ToString("dd-MMMMM")} and {NPC_7.EasterSunday(DateTime.Now.Year).AddDays(5).ToString("dd-MMMMM")}!");
                            AddOption("I see", 255);
                        }
                        else
                        {
                            AddText("It seems like the Easter Quest already ended! Please come back next year!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Until April 3rd, monsters across the world will have a chance of dropping Eggs of different kinds! Players must ");
                        AddText("catch them and come back to me to exchange the eggs for rewards! Look out for Stripped Eggs, those give the best rewards!");
                        AddOption("Exchange Eggs", 2);
                        AddOption("I see", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("It's always great to have some help in this world. Fight is all we see everywhere.");
                        AddText(" What kind of eggs would you like to exchange?");
                        AddOption("Easter Eggs", 3);
                        AddOption("Stripped Eggs", 4);
                        AddOption("I see", 255);
                        break;
                    }
                case 3:
                    {
                        if (GC.MyChar.InventoryContains(710060, 1) && GC.MyChar.InventoryContains(710061, 1) && GC.MyChar.InventoryContains(710062, 1) && GC.MyChar.InventoryContains(710063, 1))
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710060));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710061));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710062));
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(710063));
                            GC.MyChar.AddItem(710064);
                            AddText("Take this WhiteEgg! You may use it to exchange it for rewards!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have enough Eggs. Please get one EasterEgg of each colour first!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.InventoryContains(710065, 1) ||
                            GC.MyChar.InventoryContains(710066, 1) ||
                            GC.MyChar.InventoryContains(710067, 1) ||
                            GC.MyChar.InventoryContains(710068, 1) ||
                            GC.MyChar.InventoryContains(710069, 1) ||
                            GC.MyChar.InventoryContains(710070, 1) ||
                            GC.MyChar.InventoryContains(710071, 1))
                        {
                            if (GC.MyChar.InventoryContains(710065, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710065));
                            else if (GC.MyChar.InventoryContains(710066, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710066));
                            else if (GC.MyChar.InventoryContains(710067, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710067));
                            else if (GC.MyChar.InventoryContains(710068, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710068));
                            else if (GC.MyChar.InventoryContains(710069, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710069));
                            else if (GC.MyChar.InventoryContains(710070, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710070));
                            else if (GC.MyChar.InventoryContains(710071, 1))
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710071));
                            GC.MyChar.AddItem(710072);
                            AddText("Take this ColoredStone! Gather 5 of them and exchange them for an EasterFlamePack at Tracy in Market!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have a Stripped Egg! Please go hunt for them!");
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