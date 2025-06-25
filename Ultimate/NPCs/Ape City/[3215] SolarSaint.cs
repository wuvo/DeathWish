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
    public class NPC_3215 : NPCBase
    {
        public NPC_3215(Main.GameClient _client)
            : base(_client)
        {
            ID = 3215;
            Face = 67;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Our ancestor exerted their utmost efforts and defeated the demons. Since");
                        AddText("then the world has been kept in peace forhundreds of years. Now the");
                        AddText("demons have come back and the world is getting into turbulence again.");
                        AddOption("Could you tell me more?", 3);
                        AddOption("I am here to drive away the evils", 1);
                        AddOption("I'd better leave", 255);
                        if (GC.MyChar.InventoryContains(723088, 1))
                        {
                            AddOption("I have a StarSword", 11);
                        }
                        break;
                    }
                case 1:
                    {
                        if (!Game.World.DisCityON)
                        {
                            AddText("I'm glad to see that you are such a knightly person. Please come next time.");
                            AddOption("Ok", 2);
                        }
                        else if (Game.World.DisCityON)
                        {
                            if (GC.MyChar.Level >= 110)
                            {
                                GC.MyChar.Teleport(2021, 291, 475);
                                List<Item> Items = new List<Item>();
                                foreach (Game.Item I in GC.MyChar.Inventory)
                                    if (I.ID == 723085)
                                        Items.Add(I);
                                foreach (Game.Item I in Items)
                                    GC.MyChar.RemoveItem(I);
                            }
                            else
                            {
                                AddText("Sorry, you are too weak to fight thoose monsters, go get level 110 and talk to me later");
                                AddOption("Okay, I will!", 255);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        GC.LocalMessage(2011, "Find SolarSaint to take the quest from 18:00 to 18:05 every Monday and Wednesday or from 06:00 to 06:05 every Tuesday and Thursday");
                        break;
                    }
                case 3:
                    {
                        AddText("The decisive battle between human and demon broke out here. The");
                        AddText("ferocious battle lasted for seven days andnights. Countless heroes lost their");
                        AddText("lives. And the justise won. Afterwards we can live in peace for hundreds of");
                        AddText("years.");
                        AddOption("And then?", 4);
                        AddOption("Sounds great", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("UltimatePluto, leader of the demons sworn to take revenge in one thousand ");
                        AddText("years before he managed to run away. Toprevent the demons coming back, I");
                        AddText(" have been scouting their land cautiously. Yesterday I found something ");
                        AddText("unwanted. It seems that Ultimate Pluto has come round to endanger humans ");
                        AddText("again.");
                        AddOption("What should we do?", 5);
                        AddOption("Cool", 255);
                        break;
                    }
                case 5:
                    {
                        AddText("Fortunately UltimatePluto is still unfledged. He must resort to a Battle ");
                        AddText("Formation for the moment. So I am about to organize an army too infiltrate his ");
                        AddText("land and destroy his formation before he becomes stronger.");
                        AddOption("I'll join in you", 1);
                        AddOption("I'd like to know more", 6);
                        AddOption("Sigh, I'm helpless.", 255);
                        break;
                    }
                case 6:
                    {
                        AddText("You must kill UltimatePluto rapidly to get DarkHorn so that I can use it to ");
                        AddText("disable the formation. Before you can do that, you must break into HellGate, ");
                        AddText("enter the HellHall, and fight through the HellCloster. Countless ferocious ");
                        AddText("demonsare watching those strongholds. I'll give you some strategies on ");
                        AddText("breaking through those fortresses if you like.");
                        AddOption("I want to know HellGate.", 7);
                        AddOption("I want to know HellHall", 8);
                        AddOption("I want to know HellCloister", 9);
                        AddOption("I want to know BattleFormation", 10);
                        AddOption("Thanks. I know how to do", 255);
                        break;
                    }
                case 7:
                    {
                        AddText("HellGate is shielded from poisonous fog, so you can't approach it. But ");
                        AddText("demons do not fear the gas. They may turninto SoulStones after they die. If ");
                        AddText("you get five stones for me, I'll help you break through the gate. To protect ");
                        AddText("the unrelated persons, I'll send the others back as soon as the first 60 ");
                        AddText("persons pass through the gate");
                        AddOption("I'd like to know more!", 6);
                        AddOption("Thank you", 255);
                        break;
                    }
                case 8:
                    {
                        AddText("HellHall is the very spot where the demons swear their oaths of allegiance to ");
                        AddText("UltimatePluto. Everybody must do his best to make a way out. Due to limited ");
                        AddText("time, I can lead only 30 persons to HellCloister");
                        AddOption("I'd like to know more!", 6);
                        AddOption("Thank You", 255);
                        break;
                    }
                case 9:
                    {
                        AddText("You will be divided into two groups to attack from the left and the right flank of ");
                        AddText("HellCloister. Kill the Wraithas many as you can, because you can't reach ");
                        AddText("BattleFormation until the amount of Wraith is decreased to a certain level.");
                        AddOption("I'd like to know more!", 6);
                        AddOption("Thank You", 255);
                        break;
                    }
                case 10:
                    {
                        AddText("BattleFormation is protected by Syrens. After they are killed out ");
                        AddText("UltimatePluto will appear. Make the besteffort to kill him, get his DarkHorn ");
                        AddText("and give it to me. I can disable the BattleFormation with it. Then I'll sendyou ");
                        AddText("back. But if you fails to do it, we have to retreat and wait for another opportunity");
                        AddOption("I'd like to know more!", 6);
                        AddOption("Thank You", 255);
                        break;
                    }
                case 11:
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(723088));
                            if (GC.MyChar.Level < 130)
                                GC.MyChar.AddExp(5);
                            #region Garment
                            uint Item;
                           List<uint> From = new List<uint>();
                            foreach (DatabaseItem D in Database.DatabaseItems.Values)
                            {
                                if (Game.ItemIDManipulation.Part(D.ID, 0, 3) == 181 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 182 || Game.ItemIDManipulation.Part(D.ID, 0, 3) == 191)
                                    From.Add(D.ID);
                            }
                            Item = (uint)From[Program.Rnd.Next(0, From.Count)];
                            GC.MyChar.AddItem(Item);
                            #endregion
                            #region +3 Item
                            top:
                            Item I2 = new Item();
                            I2.UID = (uint)Program.Rnd.Next(10000000);
                            Item.ItemQuality V = Game.Item.ItemQuality.Normal;
                            uint ItemID = 0;
                            List<uint> Form = new List<uint>();
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
                                            Form.Add(D.ID);
                                }
                            }
                            if (Form != null)
                            {
                                if (Form.Count > 0)
                                {
                                    byte Tries = (byte)Program.Rnd.Next(0, Form.Count);
                                    ItemID = (uint)Form[Tries];
                                }
                            }
                            if (ItemID != 0)
                            {
                                I2.ID = ItemID;
                                if (I2.DBInfo.LevReq != 1)
                                {
                                    ItemIDManipulation E = new ItemIDManipulation(ItemID);
                                    E.QualityChange(V);
                                    I2.ID = E.ToID();
                                }
                                I2.Color = Game.Item.ArmorColor.Orange;
                                I2.Plus = 3;
                                I2.MaxDur = I2.DBInfo.Durability;
                                I2.CurDur = I2.MaxDur;
                                GC.MyChar.AddItem(I2);
                            }
                            else goto top;
                            #endregion
                            if (MyMath.ChanceSuccess(85))
                            {
                                GC.MyChar.AddItem(1088000);
                            }
                            if (MyMath.ChanceSuccess(60))
                            {
                                GC.MyChar.AddItem(1088000);
                            }
                            if (MyMath.ChanceSuccess(35))
                            {
                                GC.MyChar.AddItem(1088000);
                            }
                            //if (MyMath.ChanceSuccess(75))
                            //{
                            //    int j = Program.Rnd.Next(0, 8);
                            //    uint gem = (uint)(700003 + (j * 10));
                            //    GC.MyChar.AddItem(gem);
                            //}
                            AddText("Here you are! Check your inventory!");
                            AddOption("Thank you!", 255);
                        }
                        else
                        {
                            AddText("Please make 5 or more empty spaces in your inventory in order to collect your reward!");
                            AddOption("Ok.", 255);
                        }
                        break;
                    }
            }

            AddFinish();
            Send();
        }
    }
}