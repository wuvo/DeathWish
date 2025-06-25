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
    public class NPC_20005 : NPCBase
    {
        public NPC_20005(Main.GameClient _client)
            : base(_client)
        {
            ID = 20005;
            Face = 15;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();
            if (_linkback == 0)
            {
                if (GC.MyChar.Reborns > 0)
                {
                    AddText("You can't reborn anymore! You are already reborned!");
                    AddOption("I see", 255);
                }
                else
                {
                    AddText("I devote all my life to the research of eternity, finally I know the arcanum of rebirth of the life.");
                    AddOption("I would like to know about it", 1);
                    AddOption("Just passing by", 255);
                }

            }
            else if (_linkback == 1)
            {
                if (GC.MyChar.Reborns == 0)
                {
                    AddText("If you want to rebirth, you should reach a certain level, get the highest occupation on title and get a CelestialStone.");
                    AddText(" After the rebirth, you can distribute your attribute more freely and you'll be able to learn powerful skills.");
                    AddOption("What's a CelestialStone?", 150);
                    AddOption("Distribute the attributes?", 151);
                    AddOption("Powerful skills?", 152);
                    AddOption("I would like to reborn", 2);
                    AddOption("Let me think it over", 255);
                }
                else
                {
                    AddText("You are already reborned. You can't do it again.");
                    AddOption("Ah.", 255);
                }
            }
            #region Reborn
            else if (_linkback == 2)
            {
                if (GC.MyChar.Reborns == 0)
                {
                    if (GC.MyChar.Level >= 120 || GC.MyChar.Job == 135 && GC.MyChar.Level >= 110)
                    {
                        if (GC.MyChar.Job % 10 == 5)
                        {
                            if (GC.MyChar.InventoryContains(721259, 1))
                            {

                                AddText("There are two types of rebirth from which you can choose:");
                                AddOption("Normal rebirth", 30);
                                AddOption("Blessed rebirth", 155);
                                AddOption("I changed my mind", 255);
                            }
                            else
                            {
                                AddText("I can't help you with the rebirth if you don't bring me a CelestialStone.");
                                AddOption("I see", 255);
                            }
                        }
                        else
                        {
                            AddText("You must promote yourself till the highest occupation before having rebirth.");
                            AddOption("I see", 255);
                        }
                    }
                    else
                    {
                        AddText("I'm sorry but you're not yet ready to reborn. You must train harder and reach an higher level.");
                        AddOption("I see", 255);
                    }
                }
            }
            else if (_linkback == 30 || _linkback == 31 || _linkback == 155)
            {
                if (_linkback == 30)
                {
                    GC.MyChar.addBless = 0;
                    GC.MyChar.SuperGem = 1;
                    AddText("Which gem would you like to choose?");
                    AddOption("Dragon Gem", 43);
                    AddOption("Phoenix Gem", 33);
                    AddOption("Fury Gem", 53);
                    AddOption("Moon Gem", 93);
                    AddOption("Rainbow Gem", 63);
                    AddOption("Next", 64);
                }
                else if (_linkback == 31)
                {
                    GC.MyChar.addBless = 1;
                    GC.MyChar.SuperGem = 0;
                    AddText("Which class do you want to reborn into?");
                    AddOption("Trojan", 3);
                    AddOption("Warrior", 4);
                    AddOption("Archer", 5);
                    AddOption("WaterTaoist", 6);
                    AddOption("FireTaoist", 7);
                    AddOption("I changed my mind", 255);
                }
                else if (_linkback == 155)
                {
                    AddText("Since Tortoise Gem appearance with the essences of the universe, its holy aura has blessed all living beings.");
                    AddText(" I have mastered the holy force of Tortoise Gem and now I can use the CelestialStone to add the spirit of ");
                    AddText("Tortoise Gem to your equipment, then you'll have the god blesses and bear the holy aura.");
                    AddOption("I want to reborn", 31);
                    AddOption("I changed my mind", 255);
                }
            }
            else if (_linkback == 64)
            {
                AddOption("Kylin Gem", 73);
                AddOption("Violet Gem", 83);
                AddOption("Back", 30);
                AddOption("Let me think it over", 255);
            }
            else if (_linkback >= 33 && _linkback <= 103)
            {
                byte Gem = (byte)(_linkback - 30);
                GC.MyChar.SuperGem = Gem;
                AddText("Which class do you want to reborn into?");
                AddOption("Trojan", 3);
                AddOption("Warrior", 4);
                AddOption("Archer", 5);
                AddOption("WaterTaoist", 6);
                AddOption("FireTaoist", 7);
            }
            else if (_linkback >= 3 && _linkback <= 7)
            {
                if (GC.MyChar.Level >= 120 || GC.MyChar.Job == 135 && GC.MyChar.Level >= 110)
                {
                    if (GC.MyChar.InventoryContains(721259, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 37)
                        {
                            bool RBWeapon = false;
                            RemoveItem(721259);
                            Item I = new Item();
                            I.UID = (uint)Program.Rnd.Next(10000000);

                            if (GC.MyChar.Level >= 120)
                                RBWeapon = true;

                            if (GC.MyChar.SuperGem != 0)
                            {
                                GC.MyChar.AddItem((uint)(700000 + GC.MyChar.SuperGem));
                                GC.MyChar.SuperGem = 0;
                            }

                            else if (GC.MyChar.addBless != 0)
                            {
                                List<byte> ToBless = new List<byte>();
                                #region HG 1
                                if (GC.MyChar.Equips.HeadGear.ID != 0)
                                    if (GC.MyChar.Equips.HeadGear.Bless == 0)
                                        ToBless.Add((byte)1);
                                #endregion
                                #region Necklace 2
                                if (GC.MyChar.Equips.Necklace.ID != 0)
                                    if (GC.MyChar.Equips.Necklace.Bless == 0)
                                        ToBless.Add((byte)2);
                                #endregion
                                #region Ring 6
                                if (GC.MyChar.Equips.Ring.ID != 0)
                                    if (GC.MyChar.Equips.Ring.Bless == 0)
                                        ToBless.Add((byte)6);
                                #endregion
                                #region LeftHand 5
                                if (GC.MyChar.Equips.LeftHand.ID != 0)
                                    if (GC.MyChar.Equips.LeftHand.Bless == 0 && !Item.IsArrow(GC.MyChar.Equips.LeftHand.ID))
                                        ToBless.Add((byte)5);
                                #endregion
                                #region RightHand 4
                                if (GC.MyChar.Equips.RightHand.ID != 0)
                                    if (GC.MyChar.Equips.RightHand.Bless == 0)
                                        ToBless.Add((byte)4);
                                #endregion
                                #region Armor 3
                                if (GC.MyChar.Equips.Armor.ID != 0)
                                    if (GC.MyChar.Equips.Armor.Bless == 0)
                                        ToBless.Add((byte)3);
                                #endregion
                                #region Boots 8
                                if (GC.MyChar.Equips.Boots.ID != 0)
                                    if (GC.MyChar.Equips.Boots.Bless == 0)
                                        ToBless.Add((byte)8);
                                #endregion
                                #region Gourd 7
                                if (GC.MyChar.Equips.Gourd.ID != 0)
                                    if (GC.MyChar.Equips.Gourd.Bless == 0)
                                        ToBless.Add((byte)7);
                                #endregion
                                #region Garment 9
                                if (GC.MyChar.Equips.Garment.ID != 0)
                                    if (GC.MyChar.Equips.Garment.Bless == 0)
                                        ToBless.Add((byte)9);
                                #endregion
                                if (ToBless.Count > 0)
                                {
                                    byte i = (byte)ToBless[Program.Rnd.Next(0, ToBless.Count)];
                                    Item Eq = GC.MyChar.Equips.Get(i);
                                    Eq.Bless = 1;
                                    GC.MyChar.addBless = 0;
                                    GC.LocalMessage(2000, "Congratulations! Your " + Eq.DBInfo.Name + " got blessed!");
                                }
                            }
                            if (_linkback == 3) GC.MyChar.RebornCharacter(11);
                            if (_linkback == 4) GC.MyChar.RebornCharacter(21);
                            if (_linkback == 5) GC.MyChar.RebornCharacter(41);
                            if (_linkback == 6) GC.MyChar.RebornCharacter(132);
                            if (_linkback == 7) GC.MyChar.RebornCharacter(142);
                            if (GC.MyChar.Job == 11 || GC.MyChar.Job == 21)
                            {
                                I.ID = 410087;
                                I.Effect = Item.RebornEffect.Poison;
                            }
                            else if (GC.MyChar.Job == 132)
                            {
                                I.ID = 421087;
                                I.Effect = Item.RebornEffect.MP;

                            }
                            else if (GC.MyChar.Job == 142)
                            {
                                I.ID = 421087;
                                I.Effect = Item.RebornEffect.MP;
                            }
                            else
                            {
                                I.ID = 500077;
                                I.Effect = Item.RebornEffect.Shield;
                            }
                            I.MaxDur = I.DBInfo.Durability;
                            I.CurDur = I.MaxDur;
                            if (RBWeapon)
                                GC.MyChar.AddItem(I);

                            World.SendMsgToAll("SYSTEM", GC.MyChar.Name + " has reborned!", 2011, 0);

                        }
                        else
                        {
                            AddText("Please make some room in your inventory first!");
                            AddOption("I see.", 255);
                        }
                    }
                    else
                    {
                        AddText("I can't help you with the rebirth if you don't bring me a CelestialStone.");
                        AddOption("I see", 255);
                    }

                }
            }
            #endregion
            #region Infos
            else if (_linkback == 150)
            {
                AddText("The CelestialStone syncretizes seven gems in the world. If you're able to find a CleanWater and each one of the gems you'll be able to make one.");
                AddText(" Find Celestine in TwinCity(365,92) if you wish to know more about it!");
                AddOption("I see", 255);
            }
            else if (_linkback == 151)
            {
                AddText("After the rebirth you'll be starting at level 15 and get some bonus attribute points for reborning.");
                AddText(" You'll also win 3 attribute points for each level you get which you'll be able to redistribute freely.");
                AddOption("I see", 255);
            }
            else if (_linkback == 152)
            {
                AddText("The skills you have before the rebirth will disappear. But some only disappear for a while and they'll come back when their level reaches half of what it was in your past life.");
                AddText(" You can also learn new skills if you reborn into the same class you were before reborning.");
                AddOption("I see", 255);
            }
            #endregion
            AddFinish();
            Send();
        }
    }
}