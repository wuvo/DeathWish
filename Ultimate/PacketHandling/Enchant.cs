using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.PacketHandling
{
    public class Enchant
    {
        public static void Handle(Main.GameClient GC, byte[] Data)
        {
            Random Rnd = new Random();
            Game.Item I = GC.MyChar.FindInvItem(BitConverter.ToUInt32(Data, 4));
            Game.Item Gem = GC.MyChar.FindInvItem(BitConverter.ToUInt32(Data, 8));

            if (I.ID != 0 && Gem.ID != 0)
            {
                GC.MyChar.RemoveItem(Gem);
                byte Enchant = 0;
                if (Game.ItemIDManipulation.Digit(Gem.ID, 6) == 1)
                    Enchant = (byte)Rnd.Next(1, 59);
                else if (Game.ItemIDManipulation.Digit(Gem.ID, 6) == 2)
                {
                    if (Gem.ID == 700012)
                        Enchant = (byte)Rnd.Next(100, 159);
                    else if (Gem.ID == 700002 || Gem.ID == 700052 || Gem.ID == 700062)
                        Enchant = (byte)Rnd.Next(60, 109);
                    else if (Gem.ID == 700032)
                        Enchant = (byte)Rnd.Next(80, 129);
                    else
                        Enchant = (byte)Rnd.Next(40, 89);
                }
                else if (Game.ItemIDManipulation.Digit(Gem.ID, 6) == 3)
                {
                    if (Gem.ID == 700013)
                        Enchant = (byte)Rnd.Next(200, 255);
                    else if (Gem.ID == 700003 || Gem.ID == 700073 || Gem.ID == 700033)
                        Enchant = (byte)Rnd.Next(170, 229);
                    else if (Gem.ID == 700063 || Gem.ID == 700053)
                        Enchant = (byte)Rnd.Next(140, 199);
                    else if (Gem.ID == 700023)
                        Enchant = (byte)Rnd.Next(90, 149);
                    else
                        Enchant = (byte)Rnd.Next(70, 119);
                }
                if (Gem.ID == 700013)
                {
                    if (Enchant > I.Enchant)
                    {
                        I.Enchant = Enchant;
                        GC.AddSend(Packets.UpdateItem(I, 0));
                        GC.LocalMessage(2005, "The equip now gives " + I.Enchant + " extra HP.");
                    }
                    else
                    {
                        if (MyMath.ChanceSuccess(22))
                        {
                            if (I.Enchant > 252)
                            {
                                I.Enchant = 255;
                                GC.LocalMessage(2005, "New enchant value is lower, SDG Maxed the Enchant to 255. (Rolled +3 HP)");
                            }
                            else
                            {
                                I.Enchant += 3;
                                GC.LocalMessage(2005, "New enchant value is lower. SDG Added +3 HP to current enchant.");
                            }
                        }
                        else if (MyMath.ChanceSuccess(45))
                        {
                            if (I.Enchant > 253)
                            {
                                I.Enchant = 255;
                                GC.LocalMessage(2005, "New enchant value is lower, SDG Maxed the Enchant to 255. (Rolled +2 HP)");
                            }
                            else
                            {
                                I.Enchant += 2;
                                GC.LocalMessage(2005, "New enchant value is lower. SDG Added +2 HP to current enchant.");
                            }
                        }
                        else
                        {
                            I.Enchant += 1;
                            GC.LocalMessage(2005, "New enchant value is lower. SDG Added +1 HP to current enchant.");
                        }
                        GC.AddSend(Packets.UpdateItem(I, 0));
                        GC.LocalMessage(2005, "The equip now gives " + I.Enchant + " extra HP.");
                    }
                }
                else if (Enchant > I.Enchant)
                {
                    //GC.MyChar.RemoveItem(I.UID);//aici to ITEM?
                    I.Enchant = Enchant;
                    //GC.MyChar.AddItem(I);
                    GC.AddSend(Packets.UpdateItem(I, 0));
                    GC.LocalMessage(2005, "The equip now gives " + I.Enchant + " extra HP.");
                }
                else
                    GC.LocalMessage(2005, Enchant + " extra HP, the current one will stay.");
            }
        }
    }
}
