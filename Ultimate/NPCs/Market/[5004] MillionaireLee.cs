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
    public class NPC_5004 : NPCBase
    {
        public NPC_5004(Main.GameClient _client)
            : base(_client)
        {
            ID = 5004;
            Face = 29;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("If you have DragonBalls or Meteors I can combine them into scrolls and store them for you. I can also bag up MeteorScrolls into a MetScrollBag!");
                        AddOption("Compose my Meteors.", 1);
                        AddOption("Compose my Dragonballs", 2);
                        AddOption("Store my MeteorScrolls", 3);
                        AddOption("Retrieve MeteorScrolls", 4);
                        AddOption("Store my Dragonballs", 15);
                        AddOption("Retrieve Dragonballs", 16);
                        AddOption("Compose MetscrollBag", 7);
                        AddOption("Exchange DBs / MoneyBag", 10);
                        break;
                    }

                case 10:
                    {
                        AddText("You can exchange 1 DB for 2 MetScrolls or 1 DBScroll for 20 MetScrolls.");
                        AddOption("Exchange 1 DB for 2 MetScrolls", 11);
                        AddOption("Exchange 1 DBScroll for 20 MetScrolls", 12);
                        AddOption("Exchange 135kk for 135kk Money PACKET", 13);

                        break;
                    }

                case 11:
                    {
                        if (GC.MyChar.InventoryContains(1088000, 1))
                        {

                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                            GC.MyChar.AddItem(720027);
                            GC.MyChar.AddItem(720027);
                            //GC.MyChar.AddItem(720027);
                            GC.LocalMessage(2005, "You successfully exchange 1 DragonBall for 2 MetScrolls!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 1 Dragonball.");
                        }
                        break;
                    }
                case 13:
                    {
                        if (GC.MyChar.Silvers > 135000000)
                        {

                            GC.MyChar.Silvers -= 135000000;
                            GC.MyChar.AddItem(720684);


                            GC.LocalMessage(2005, "You successfully exchange money to packet!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 135kk Money.");
                        }
                        break;
                    }

                case 12:
                    {
                        if (GC.MyChar.InventoryContains(720028, 1))
                        {

                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(720028));
                            GC.MyChar.AddItem(729912);
                            GC.MyChar.AddItem(729912);
                            //GC.MyChar.AddItem(729912);
                            GC.LocalMessage(2005, "You successfully exchange 1 DBScroll for 2 MetScrollBags!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 1 DBScroll.");
                        }
                        break;
                    }

                case 1:
                    {//720027
                        if (GC.MyChar.InventoryContains(1088001, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088001));
                            GC.MyChar.AddItem(720027);
                            GC.LocalMessage(2005, "You successfully composed 10 Meteors into a MeteorScroll.");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 10 Meteors.");
                        }
                        break;
                    }
                case 2:
                    {
                        if (GC.MyChar.InventoryContains(1088000, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                            GC.MyChar.AddItem(720028);
                            GC.LocalMessage(2005, "You successfully composed 10 DragonBalls into a DBScroll");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 10 DragonBalls.");
                        }
                        break;
                    }
                case 3:
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 720027)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.MetScrolls + i <= 255)
                                {
                                    GC.MyChar.MetScrolls += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(720027));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " MeteorScrolls! You have " + GC.MyChar.MetScrolls + " MeteorScrolls stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 MeteorScrolls stored!");
                            }
                            GC.LocalMessage(2005, "You don't have any MeteorScrolls!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use MeteorBank you need to be VIP5 or More!");
                        }

                        break;

                    }
                case 4:
                    {
                        if (GC.MyChar.MetScrolls > 0)
                        {
                            AddText("How many MeteorScrolls do you want to retrieve? You have " + GC.MyChar.MetScrolls + " MeteorScrolls stored!");
                            AddInput("MeteorScrolls:", 55);
                        }
                        else
                        {
                            AddText("You don't have any MeteorScrolls stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }
                case 55:
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.MetScrolls >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.MetScrolls -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(720027);
                                        AddText("You retrieved " + o + " MeteorScrolls!");
                                        AddOption("Thanks", 255);
                                    }
                                    else
                                    {
                                        AddText("You don't have enough space in inventory!");
                                        AddOption("I see", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You don't have enough MeteorScrolls stored!");
                                    AddOption("I see", 255);
                                }
                            }
                            else
                            {
                                AddText("Enter a valid amount!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("Enter a valid amount!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 7:
                    {
                        if (GC.MyChar.InventoryContains(720027, 10))
                        {
                            for (int i = 0; i < 10; i++)
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(720027));
                            GC.MyChar.AddItem(729912);
                            GC.LocalMessage(2005, "You successfully packed 10 MeteorScrolls into 1 MetScrollBag!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "You don't have 10 MeteorScrolls.");
                        }
                        break;
                    }
                case 15://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 1088000)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.Dragonballs + i <= 255)
                                {
                                    GC.MyChar.Dragonballs += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(1088000));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " Dragonballs! You have " + GC.MyChar.Dragonballs + " Dragonballs stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 Dragonballs stored!");
                            }
                            GC.LocalMessage(2005, "You don't have any Dragonballs!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use MeteorBank you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 16://retrieve
                    {
                        if (GC.MyChar.Dragonballs > 0)
                        {
                            AddText("How many Dragonballs do you want to retrieve? You have " + GC.MyChar.Dragonballs + " Dragonballs stored!");
                            AddInput("Dragonballs:", 90);
                        }
                        else
                        {
                            AddText("You don't have any Dragonballs stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }
                case 90://tortoise
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.Dragonballs >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.Dragonballs -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(1088000);
                                        AddText("You retrieved " + o + " Dragonballs!");
                                        AddOption("Thanks", 255);
                                    }
                                    else
                                    {
                                        AddText("You don't have enough space in inventory!");
                                        AddOption("I see", 255);
                                    }
                                }
                                else
                                {
                                    AddText("You don't have enough Dragonballs stored!");
                                    AddOption("I see", 255);
                                }
                            }
                            else
                            {
                                AddText("Enter a valid amount!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("Enter a valid amount!");
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
