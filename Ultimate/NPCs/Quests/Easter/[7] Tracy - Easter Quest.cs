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
    public class NPC_7 : NPCBase
    {
        public NPC_7(Main.GameClient _client)
            : base(_client)
        {
            ID = 7;
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
                        if (DateTime.Now.AddDays(5) >= EasterSunday(DateTime.Now.Year) && DateTime.Now <= EasterSunday(DateTime.Now.Year).AddDays(5))
                        {
                            AddText("I'm here to favour those who are helping us retrieve all the eggs that were stolen by monsters!");
                            AddText(" If you have already helped you may get the reward you deserve!");
                            AddOption("Tell me more about it", 1);
                            AddOption("Claim Reward", 2);
                            AddOption("Just passing by", 255);
                        }
                        else if (DateTime.Now.AddDays(5) < EasterSunday(DateTime.Now.Year))
                        {
                            AddText($"You'll be only able to exchange eggs between {EasterSunday(DateTime.Now.Year).Subtract(new TimeSpan(5, 0, 0, 0,0)).ToString("dd-MMMMM")} and {EasterSunday(DateTime.Now.Year).AddDays(5).ToString("dd-MMMMM")}!");
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
                        AddText("If you managed to help EasterBuddy by hunting some Eggs for him I'm sure he has given you either a ColoredStone");
                        AddText(" Or a WhiteEgg depending on which type of Eggs you have found during your hunt! I'm giving an EasterFlamePack for 5 ColoredStones and ");
                        AddText("Bronze/Silver/Gold EasterPacks for WhiteEggs depending on how many you're willing to trade!");
                        AddOption("Claim Reward", 2);
                        AddOption("I see", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("It's always great to have some help in this world. Fight is all we see everywhere.");
                        AddText(" What would you like to exchange?");
                        AddOption("5 ColoredStones", 15);
                        AddOption("3 WhiteEggs", 3);
                        AddOption("5 WhiteEggs", 5);
                        AddOption("10 WhiteEggs", 10);
                        AddOption("I see", 255);
                        break;
                    }
                case 15:
                    {
                        if (GC.MyChar.InventoryContains(710072, 5))
                        {
                            for (int a = 0; a < 5; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710072));
                            GC.MyChar.AddItem(720124);
                            AddText("Here you go! You may open the EasterFlamePack for rewards!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("You don't have five ColoredStones!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 3:
                case 5:
                case 10:
                    {
                        if (GC.MyChar.InventoryContains(710064, Convert.ToByte(_linkback)))
                        {
                            for (int a = 0; a < _linkback; a++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(710064));

                            if (_linkback == 3)
                            {
                                GC.MyChar.AddItem(720123);
                                AddText("Take this EasterBronzePack! You may open it to obtain rewards!");
                                AddOption("Thanks", 255);
                            }
                            else if (_linkback == 5)
                            {
                                GC.MyChar.AddItem(720122);
                                AddText("Take this EasterSilverPack! You may open it to obtain rewards!");
                                AddOption("Thanks", 255);
                            }
                            else if (_linkback == 10)
                            {
                                GC.MyChar.AddItem(720121);
                                AddText("Take this EasterGoldPack! You may open it to obtain rewards!");
                                AddOption("Thanks", 255);
                            }
                        }
                        else
                        {
                            AddText("You don't have enough WhiteEggs! Please come back later!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }

        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}