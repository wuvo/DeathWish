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
    public class NPC_2028 : NPCBase
    {
        public NPC_2028(Main.GameClient _client)
            : base(_client)
        {
            ID = 2028;
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
                        AddText("Howdy! As you probably know, talent wins games but teamwork wins championships. Capture the Bag is where you can prove your true value. Would you like to team-up and rule the world?");
                        AddOption("Check my CTBPoints and prizes", 1);
                        AddOption("Join the Capture the Bag", 2);
                        AddOption("Just passing by", 255);
                        break;
                    }
                case 2:
                    {
                        foreach (Events.Events E in World.Events)
                            if (E.EventTitle == "Capture The Bag")
                                if (E.AddPlayer(GC.MyChar))
                                    GC.MyChar.EventBase = E;
                                else
                                {
                                    AddText("You're too late, " + GC.MyChar.Name + ", the CTB has already started!");
                                    AddOption("I see", 255);
                                }
                        if (GC.MyChar.EventBase?.EventTitle != "Capture The Bag")
                        {
                            AddText("The Capture the Bag Event is only held on Fridays at 16:00 Server Time and on Saturdays at 22:00 Server Time. Please come back in the right time. ");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 1:
                    {
                        AddText("Currently you've got " + GC.MyChar.CTBPoints + " CTBPoints! What would you like to choose?");
                        AddOption("DB (100 Points)", 3);
                        AddOption("+3 Random Item (500 Points)", 10);
                        //AddOption("DBScroll (200 Points)", 9);
                        AddOption("CleanWater (250 Points)", 11);
                        AddOption("Random Garment (250 Points)", 12);
                        //AddOption("Double Exp (100 Points)", 5);
                        //AddOption("CCGWBomb (150 Points)", 6);
                        AddOption("Emerald (100 Points)", 6);
                        //AddOption("Next Page", 7);
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 7:
                    {
                        AddText("Currently you've got " + GC.MyChar.CTBPoints + " CTBPoints! What would you like to choose?");
                        //AddOption("Bomb (150 Points)", 8);

                        //AddOption("+2 Random Item (200 Points)", 10);
                        
                        //AddOption("Random Garment (750 Points)", 12);
                        AddOption("Nevermind", 255);
                        break;
                    }
                #region Prizes
                case 3:
                    {
                        if (GC.MyChar.CTBPoints >= 100)
                        {
                            GC.MyChar.AddItem(1088000);
                            GC.MyChar.CTBPoints -= 100;
                            AddText("Here you go! Enjoy!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 4:
                    {
                        if (GC.MyChar.CTBPoints >= 100)
                        {
                            GC.MyChar.VP += 10000;
                            GC.MyChar.CTBPoints -= 100;
                            AddText("Here you go! Enjoy!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 5:
                    {
                        if (GC.MyChar.CTBPoints >= 100)
                        {
                            if (GC.MyChar.DoubleExpLeft == 0)
                            {
                                GC.MyChar.DoubleExpLeft = 3600;
                                GC.MyChar.CTBPoints -= 100;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("You still have some double experience time left!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 6:
                    {
                        if (GC.MyChar.CTBPoints >= 100)
                        {
                            if (GC.MyChar.Inventory.Count < 39)
                            {
                                GC.MyChar.AddItem(1080001);
                                GC.MyChar.CTBPoints -= 100;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 8:
                    {
                        if (GC.MyChar.CTBPoints >= 150)
                        {
                            if (GC.MyChar.Inventory.Count < 39)
                            {
                                GC.MyChar.AddItem(721261);
                                GC.MyChar.CTBPoints -= 150;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 9:
                    {
                        if (GC.MyChar.CTBPoints >= 200)
                        {
                            if (GC.MyChar.Inventory.Count < 39)
                            {
                                GC.MyChar.AddItem(720028);
                                GC.MyChar.CTBPoints -= 200;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first!");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 10:
                    {
                        if (GC.MyChar.CTBPoints >= 500)
                        {
                            if (GC.MyChar.Inventory.Count < 37)
                            {
                                #region +1 Item
                                for (int a = 0; a < 1; a++)
                                {
                                top:
                                    Item I2 = new Item();
                                    I2.UID = (uint)Program.Rnd.Next(10000000);
                                    Item.ItemQuality Q = Item.ItemQuality.Normal;

                                    uint ItemID = 0;
                                    List<uint> From = new List<uint>();
                                    int Type = Program.Rnd.Next(0, 255);
                                    uint Part = 0;
                                    if (Type < 10) Part = 111;
                                    else if (Type < 20) Part = 113;
                                    else if (Type < 30) Part = 114;
                                    else if (Type < 40) Part = 117;
                                    else if (Type < 50) Part = 118;
                                    else if (Type < 60) Part = 120;
                                    else if (Type < 70) Part = 121;
                                    else if (Type < 80) Part = 130;
                                    else if (Type < 90) Part = 131;
                                    else if (Type < 100) Part = 133;
                                    else if (Type < 110) Part = 134;
                                    else if (Type < 120) Part = 141;
                                    else if (Type < 130) Part = 142;
                                    else if (Type < 140) Part = 150;
                                    else if (Type < 150) Part = 151;
                                    else if (Type < 160) Part = 152;
                                    else if (Type < 165) Part = 160;
                                    else if (Type < 175) Part = 410;
                                    else if (Type < 185) Part = 420;
                                    else if (Type < 195) Part = 480;
                                    else if (Type < 205) Part = 481;
                                    else if (Type < 215) Part = 500;
                                    else if (Type < 225) Part = 530;
                                    else if (Type < 235) Part = 560;
                                    else if (Type < 245) Part = 561;
                                    else if (Type < 255) Part = 900;

                                    foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                    {
                                        if (D.LevReq >= 5 && D.LevReq <= 110)
                                        {
                                            if (D.LevReq != 0)
                                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == Part)
                                                    From.Add(D.ID);
                                        }
                                    }
                                    if (From != null)
                                    {
                                        if (From.Count > 0)
                                        {
                                            byte Tries = (byte)Program.Rnd.Next(0, From.Count);
                                            ItemID = (uint)From[Tries];
                                        }
                                    }
                                    if (ItemID != 0)
                                    {
                                        I2.ID = ItemID;
                                        if (I2.DBInfo.LevReq != 1)
                                        {
                                            ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                            E.QualityChange(Q);
                                            I2.ID = E.ToID();
                                        }

                                        I2.Color = Item.ArmorColor.Orange;

                                        I2.Plus = 3;
                                        I2.MaxDur = I2.DBInfo.Durability;
                                        I2.CurDur = I2.MaxDur;

                                        GC.MyChar.AddItem(I2);
                                    }
                                    else goto top;
                                }
                                #endregion
                                GC.MyChar.CTBPoints -= 500;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 11:
                    {
                        if (GC.MyChar.CTBPoints >= 250)
                        {
                            if (GC.MyChar.Inventory.Count < 39)
                            {
                                GC.MyChar.AddItem(721258);
                                GC.MyChar.CTBPoints -= 250;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                case 12:
                    {
                        if (GC.MyChar.CTBPoints >= 250)
                        {
                            if (GC.MyChar.Inventory.Count < 39)
                            {
                                #region Garment
                                uint Item;
                                List<uint> From = new List<uint>();
                                foreach (DatabaseItem D in Database.DatabaseItems.Values)
                                {
                                    if (ItemIDManipulation.Part(D.ID, 0, 3) == 181 || ItemIDManipulation.Part(D.ID, 0, 3) == 182 || ItemIDManipulation.Part(D.ID, 0, 3) == 183 || ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                        From.Add(D.ID);
                                }
                                Item = (uint)From[Program.Rnd.Next(0, From.Count)];
                                #endregion
                                GC.MyChar.AddItem(Item);
                                GC.MyChar.CTBPoints -= 250;
                                AddText("Here you go! Enjoy!");
                                AddOption("Thanks", 255);
                            }
                            else
                            {
                                AddText("Please make some room in your inventory first");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("It seems like you don't have enough CTB Points!");
                            AddOption("I see", 255);
                        }
                        break;
                    }
                    #endregion
            }

            AddFinish();
            Send();
        }
    }
}