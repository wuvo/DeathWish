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
    public class NPC_5015 : NPCBase
    {
        public NPC_5015(Main.GameClient _client)
            : base(_client)
        {
            ID = 5015;
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
                        AddText("If you have Gems or I can store them for you.!");
                        AddOption("DragonGems.", 21);
                        AddOption("PhoenixGems", 22);
                        AddOption("RainbowGems", 23);
                        AddOption("KylinGems", 24);
                        AddOption("FuryGems", 25);
                        AddOption("VioletGems", 26);
                        AddOption("MoonGems", 27);
                        AddOption("TortoiseGems", 28);
                        break;
                    }


                case 21:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My DragonGems", 1);
                        AddOption("Retrieve My DragonGems", 2);

                    }
                    break;
                case 22:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My PhoenixGems", 3);
                        AddOption("Retrieve My PhoenixGems", 4);

                    }
                    break;
                case 23:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My RainbowGems", 5);
                        AddOption("Retrieve My RainbowGems", 6);

                    }
                    break;
                case 24:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My KylinGems", 7);
                        AddOption("Retrieve My KylinGems", 8);

                    }
                    break;
                case 25:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My FuryGems", 9);
                        AddOption("Retrieve My FuryGems", 10);



                    }
                    break;
                case 26:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My VioletGems", 11);
                        AddOption("Retrieve My VioletGems", 12);



                    }
                    break;
                case 27:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My MoonGems", 13);
                        AddOption("Retrieve My MoonGems", 14);



                    }
                    break;
                case 28:
                    {
                        AddText("I can keep it for you, you can come and get it whenever you want ! What you wanna do ?");
                        AddOption("Store My TortoiseGems", 15);
                        AddOption("Retrieve My TortoiseGems", 16);



                    }

                    break;

                case 1://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700011)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.DragonGems + i <= 255)
                                {
                                    GC.MyChar.DragonGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700011));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " DragonGems! You have " + GC.MyChar.DragonGems + " DragonGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 DragonGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any DragonGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }




                case 2://retrieve
                    {
                        if (GC.MyChar.DragonGems > 0)
                        {
                            AddText("How many DragonGems do you want to retrieve? You have " + GC.MyChar.DragonGems + " DragonGems stored!");
                            AddInput("DragonGems:", 20);
                        }
                        else
                        {
                            AddText("You don't have any DragonGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }
                case 3://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700001)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.PhoenixGems + i <= 255)
                                {
                                    GC.MyChar.PhoenixGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700001));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " PhoenixGems! You have " + GC.MyChar.PhoenixGems + " PhoenixGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 PhoenixGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any PhoenixGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 4://retrieve
                    {
                        if (GC.MyChar.PhoenixGems > 0)
                        {
                            AddText("How many PhoenixGems do you want to retrieve? You have " + GC.MyChar.PhoenixGems + " PhoenixGems stored!");
                            AddInput("PhoenixGems:", 30);
                        }
                        else
                        {
                            AddText("You don't have any PhoenixGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }

                case 5://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700031)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.RainbowGems + i <= 255)
                                {
                                    GC.MyChar.RainbowGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700031));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " RainbowGems! You have " + GC.MyChar.RainbowGems + " RainbowGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 RainbowGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any RainbowGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 6://retrieve
                    {
                        if (GC.MyChar.RainbowGems > 0)
                        {
                            AddText("How many RainbowGems do you want to retrieve? You have " + GC.MyChar.RainbowGems + " RainbowGems stored!");
                            AddInput("RainbowGems:", 40);
                        }
                        else
                        {
                            AddText("You don't have any RainbowGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }
                case 7://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700041)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.KylinGems + i <= 255)
                                {
                                    GC.MyChar.KylinGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700041));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " KylinGems! You have " + GC.MyChar.KylinGems + " KylinGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 KylinGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any KylinGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 8://retrieve
                    {
                        if (GC.MyChar.KylinGems > 0)
                        {
                            AddText("How many KylinGems do you want to retrieve? You have " + GC.MyChar.KylinGems + " KylinGems stored!");
                            AddInput("KylinGems:", 50);
                        }
                        else
                        {
                            AddText("You don't have any KylinGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }

                case 9://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700021)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.FuryGems + i <= 255)
                                {
                                    GC.MyChar.FuryGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700021));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " FuryGems! You have " + GC.MyChar.FuryGems + " FuryGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 FuryGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any FuryGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 10://retrieve
                    {
                        if (GC.MyChar.FuryGems > 0)
                        {
                            AddText("How many FuryGems do you want to retrieve? You have " + GC.MyChar.FuryGems + " FuryGems stored!");
                            AddInput("FuryGems:", 60);
                        }
                        else
                        {
                            AddText("You don't have any FuryGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }

                case 11://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700051)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.VioletGems + i <= 255)
                                {
                                    GC.MyChar.VioletGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700051));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " VioletGems! You have " + GC.MyChar.VioletGems + " VioletGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 VioletGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any VioletGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 12://retrieve
                    {
                        if (GC.MyChar.VioletGems > 0)
                        {
                            AddText("How many VioletGems do you want to retrieve? You have " + GC.MyChar.VioletGems + " VioletGems stored!");
                            AddInput("VioletGems:", 70);
                        }
                        else
                        {
                            AddText("You don't have any VioletGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }


                case 13://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700061)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.MoonGems + i <= 255)
                                {
                                    GC.MyChar.MoonGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700061));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " MoonGems! You have " + GC.MyChar.MoonGems + " MoonGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 MoonGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any MoonGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 14://retrieve
                    {
                        if (GC.MyChar.MoonGems > 0)
                        {
                            AddText("How many MoonGems do you want to retrieve? You have " + GC.MyChar.MoonGems + " MoonGems stored!");
                            AddInput("MoonGems:", 80);
                        }
                        else
                        {
                            AddText("You don't have any MoonGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }


                case 15://store
                    {
                        byte i = 0;
                        if (GC.MyChar.VipLevel > 4 || GC.MyChar.VipLevel == 3)
                        {
                            foreach (Game.Item I in GC.MyChar.Inventory)
                            {
                                if (I.ID == 700071)
                                    i++;
                            }
                            if (i > 0)
                            {
                                if (GC.MyChar.TortoiseGems + i <= 255)
                                {
                                    GC.MyChar.TortoiseGems += i;
                                    for (int j = 0; j < i; j++)
                                        GC.MyChar.RemoveItem(GC.MyChar.NextItem(700071));
                                    GC.LocalMessage(2005, "You successfully stored " + i + " TortoiseGems! You have " + GC.MyChar.TortoiseGems + " TortoiseGems stored!");
                                }
                                else GC.LocalMessage(2005, "You can't have more than 255 TortoiseGems stored!");
                            }
                            else
                                GC.LocalMessage(2005, "You don't have any TortoiseGems!");
                        }
                        else
                        {
                            GC.LocalMessage(2005, "Sorry you dont have Vip. if you want to use GemBanks you need to be VIP5 or More!");
                        }

                        break;

                    }


                case 16://retrieve
                    {
                        if (GC.MyChar.TortoiseGems > 0)
                        {
                            AddText("How many TortoiseGems do you want to retrieve? You have " + GC.MyChar.TortoiseGems + " TortoiseGems stored!");
                            AddInput("TortoiseGems:", 90);
                        }
                        else
                        {
                            AddText("You don't have any TortoiseGems stored!");
                        }
                        AddOption("I see", 255);
                        break;
                    }





























                case 20://dragon
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.DragonGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.DragonGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700011);
                                        AddText("You retrieved " + o + " DragonGems!");
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
                                    AddText("You don't have enough DragonGems stored!");
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

                case 30://phoenix
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.PhoenixGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.PhoenixGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700001);
                                        AddText("You retrieved " + o + " PhoenixGems!");
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
                                    AddText("You don't have enough PhoenixGems stored!");
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

                case 40://rainbow
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.RainbowGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.RainbowGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700031);
                                        AddText("You retrieved " + o + " RainbowGems!");
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
                                    AddText("You don't have enough RainbowGems stored!");
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

                case 50://kylin
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.KylinGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.KylinGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700041);
                                        AddText("You retrieved " + o + " KylinGems!");
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
                                    AddText("You don't have enough KylinGems stored!");
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

                case 60://fury
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.FuryGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.FuryGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700021);
                                        AddText("You retrieved " + o + " FuryGems!");
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
                                    AddText("You don't have enough FuryGems stored!");
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

                case 70://violet
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.VioletGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.VioletGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700051);
                                        AddText("You retrieved " + o + " VioletGems!");
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
                                    AddText("You don't have enough VioletGems stored!");
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

                case 80://moon
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.MoonGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.MoonGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700061);
                                        AddText("You retrieved " + o + " MoonGems!");
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
                                    AddText("You don't have enough MoonGems stored!");
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


                case 90://tortoise
                    {
                        byte o;
                        if (byte.TryParse(ReadString(_data), out o))
                        {
                            if (o > 0)
                            {
                                if (GC.MyChar.TortoiseGems >= o)
                                {
                                    if (GC.MyChar.Inventory.Count + o <= 40)
                                    {
                                        GC.MyChar.TortoiseGems -= o;
                                        for (byte i = 0; i < o; i++)
                                            GC.MyChar.AddItem(700071);
                                        AddText("You retrieved " + o + " TortoiseGems!");
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
                                    AddText("You don't have enough TortoiseGems stored!");
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