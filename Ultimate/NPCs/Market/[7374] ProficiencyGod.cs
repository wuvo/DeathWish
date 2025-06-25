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
    public class NPC_7374 : NPCBase
    {
        public NPC_7374(Main.GameClient _client)
            : base(_client)
        {
            ID = 7374;
            Face = 65;
        }

        public override void Run(Main.GameClient GC, byte[] _data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            if (_linkback == 0)
            {
                AddText("The ProficiencyToken is an extremely miraculous item, it can make you");
                AddText("full of spirit to upgrade the proficiency of weapons very");
                AddText("fast. However it is really hard to find, only Ganoderma and Titan have a chance to drop them.");
                AddText("What would you like to improve?");
                AddOption("One handed weapons", 1);
                AddOption("Two handed weapons", 2);
                AddOption("Boxing", 5);
                AddOption("Shield", 6);
                AddOption("Just passing by.", 255);
            }

            else if (_linkback == 1)
            {
                AddText("Pick the one-handed weapon proficiency you would like to upgrade:");
                AddOption("Club", 80);
                AddOption("Sword", 20);
                AddOption("Blade", 10);
                AddOption("BackSword", 21);
                AddOption("Hook", 30);
                AddOption("Axe", 50);
                AddOption("Next Page", 3);
            }
            else if (_linkback == 3)
            {
                AddText("Pick the one-handed weapon proficiency you would like to upgrade:");
                AddOption("Whip", 40);
                AddOption("Hammer", 60);
                AddOption("Scepter", 81);
                AddOption("Dagger", 90);
                AddOption("Previous Page", 1);
            }
            else if (_linkback == 2)
            {
                AddText("Pick the two-handed weapon proficiency you would like to upgrade:");
                AddOption("Bow", 100);
                AddOption("Wand", 161);
                AddOption("Spear", 160);
                AddOption("Poleaxe", 130);
                AddOption("Next Page", 4);
            }
            else if (_linkback == 4)
            {
                AddText("Pick the two-handed weapon proficiency you would like to upgrade:");
                AddOption("Glaive", 110);
                AddOption("LongHammer", 140);
                AddOption("Halbert", 180);
                AddOption("Pickaxe", 162);
                AddOption("Previous Page", 2);
            }
            else if (_linkback > 4)
            {
                #region Upgrade
                ushort WeaponId = (ushort)(_linkback + 400);//Club

                switch (_linkback)
                {
                    case 5:
                        WeaponId = 000;
                        goto top;
                    case 6:
                        WeaponId = 900;
                        goto top;
                    case 40://whip
                    case 60://hammer
                    case 81://scepter
                    case 90://dagger
                    case 80://club
                    case 20://Sword ,BS=421
                    case 10://Blade
                    case 21://BackSword
                    case 30://Hook
                    case 50://Axe
                    case 100://Bow
                    case 161://Wand
                    case 160://Spear
                    case 130://Poleaxe
                    case 110://Glaive
                    case 140://LongHammer
                    case 180://Halbert
                    case 162://Pickaxe
                        top: if (!GC.MyChar.Profs.ContainsKey(WeaponId))
                        {
                            AddText("Your weapon proficiency must be at least level 1!");
                            AddOption("I see", 255);
                            break;
                        }
                        Prof P = (Prof)GC.MyChar.Profs[WeaponId];
                        #region ProficiencyTokens Price
                        byte Price;

                        if (P.Lvl < 3)
                            Price = 1;
                        else if (P.Lvl > 2 && P.Lvl < 6)
                            Price = 2;
                        else if (P.Lvl > 5 && P.Lvl < 8)
                            Price = 3;
                        else if (P.Lvl > 7 && P.Lvl < 10)
                            Price = Convert.ToByte((P.Lvl - 4));
                        else if (P.Lvl > 9 && P.Lvl < 12)
                            Price = Convert.ToByte((P.Lvl - 3));
                        else if (P.Lvl == 12)
                            Price = 10;
                        else if (P.Lvl > 12 && P.Lvl < 16)
                            Price = Convert.ToByte((P.Lvl));
                        else
                            Price = Convert.ToByte((P.Lvl + 1) * 2);
                        #endregion
                        #region Upgrade
                        if (GC.MyChar.Profs.ContainsKey(WeaponId))
                        {
                            if (P.Lvl >= 17)
                            {
                                AddText("I'm sorry for your weapon proficiency is too high for me to help you! You must do it by yourself from now on!");
                                AddOption("I see", 255);
                            }
                            else if (GC.Agreed && GC.MyChar.InventoryContains(722384, Price))
                            {
                                GC.Agreed = false;
                                for (int i = 0; i < Price; i++)
                                {
                                    GC.MyChar.RemoveItem(GC.MyChar.NextItem(722384));
                                }
                                P.Lvl++;
                                P.Exp = 0;
                                GC.AddSend(Packets.Prof(P));
                                AddText("Congratulations! Your proficiency has been upgraded!");
                                AddOption("Thanks", 255);
                            }
                            else if (GC.Agreed && !GC.MyChar.InventoryContains(722384, Price))
                            {
                                GC.Agreed = false;
                                AddText("I'm sorry but you don't have enough ProfiencyTokens!");
                                AddOption("I see", 255);
                            }
                            else
                            {
                                AddText("I will need " + Price + " ProficiencyTokens to updrage your weapon proficiency to " + (P.Lvl + 1) + "! Would you like to continue?");
                                AddOption("Yeah", Convert.ToByte(_linkback));
                                AddOption("Nevermind", 255);
                                GC.Agreed = true;
                            }

                        }
                        #endregion
                        break;
                }
                #endregion
            }

            AddFinish();
            Send();
        }
    }
}