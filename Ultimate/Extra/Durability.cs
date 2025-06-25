using System;
using System.Collections.Generic;
using System.Text;

namespace Ultimate.Extra
{
    public class Durability
    {
        public static void DefenceDurability(Main.GameClient GC)
        {
            if (GC.MyChar.Loc.Map != 1039 && GC.MyChar.Loc.Map != 1080 && GC.MyChar.Loc.Map != 1017 && GC.MyChar.Loc.Map != 1844 && GC.MyChar.Loc.Map != 1038 && GC.MyChar.Loc.Map != 6000 && GC.MyChar.Loc.Map != 1004 && GC.MyChar.Loc.Map != 6001)
            {
                for (byte i = 1; i < 9; i++)
                {
                    if (i == 1 || i == 2 || i == 3 || i == 8)
                    {
                        Game.Item I = GC.MyChar.Equips.Get(i);
                        if (I.ID != 0 && I.CurDur > 1)
                        {
                            if (MyMath.ChanceSuccess(20))//change the chances!
                            {
                                I.CurDur -= 1;
                                if (I.CurDur <= I.MaxDur / 10)
                                {
                                    if (I.CurDur > 0)
                                    {
                                        if (I.DBInfo.ID != 1050000 && I.DBInfo.ID != 10500001 && I.DBInfo.ID != 1050002 && I.DBInfo.ID != 1051000)
                                        {

                                            GC.LocalMessage(2011, "Be carefull, your item named " + I.DBInfo.Name + " current dura is too low. Go repair it now!");
                                        }
                                    }
                                    else
                                    {
                                        I.CurDur = 0;
                                        GC.MyChar.EquipStats(i, false, true);
                                    }
                                }

                                GC.AddSend(Packets.UpdateItem(I, i));
                                //Console.Game.World.DebugAdd +="Durability removed from item position " + i);
                                //Console.Game.World.DebugAdd +="Durability removed from item named " + I.DBInfo.Name);
                            }
                        }
                        else if (I.ID != 0 && I.CurDur <= 1)
                        {
                            if (I.CurDur == 1)
                            {
                                I.CurDur = 0;
                                GC.MyChar.EquipStats(i, false, true);
                            }
                            if (I.DBInfo.ID != 1050000 && I.DBInfo.ID != 10500001 && I.DBInfo.ID != 1050002 && I.DBInfo.ID != 1051000)
                            { 
                            GC.LocalMessage(2011, "Be carefull, your item named " + I.DBInfo.Name + " current dura is broken, repair it now or the item will desappear!");
                            }
                        }
                    }
                }
            }
        }

        public static void AttackDurability(Main.GameClient GC)
        {
            if (GC.MyChar.Loc.Map != 1039 && GC.MyChar.Loc.Map != 1080 && GC.MyChar.Loc.Map != 1017 && GC.MyChar.Loc.Map != 1844 &&   GC.MyChar.Loc.Map != 1004 && GC.MyChar.Loc.Map != 1038 && GC.MyChar.Loc.Map != 6000 && GC.MyChar.Loc.Map != 6001)
            {
                for (byte i = 4; i < 7; i++)
                {
                        Game.Item I = GC.MyChar.Equips.Get(i);
                        if (I.ID == 0 || Game.Item.IsArrow(I.DBInfo.Name))
                            continue;
                        if (I.CurDur > 1)
                        {
                           // if (i != 4)
                              //  Console.WriteLine("Name: " + I.DBInfo.Name + " ID: " + I.DBInfo.ID + " IDD: " + I.ID);   
                                if (MyMath.ChanceSuccess(20))//change the chances... 
                                {
                                   

                                    I.CurDur -= 1;
                                    if (I.CurDur <= I.MaxDur / 10)
                                    {
                                if (I.CurDur > 0)
                                {
                                    if (I.DBInfo.ID != 1050000 && I.DBInfo.ID != 10500001 && I.DBInfo.ID != 1050002 && I.DBInfo.ID != 1051000)
                                    {

                                        GC.LocalMessage(2011, "Be carefull, your item named " + I.DBInfo.Name + " current dura is too low. Go repair it now!");
                                    }
                                }
                                else
                                    {
                                        I.CurDur = 0;
                                        GC.MyChar.EquipStats(i, false, true);
                                    }
                                    }

                                    GC.AddSend(Packets.UpdateItem(I, i));
                                }
                                //Console.Game.World.DebugAdd +="Durability removed from item position " + i);
                                //Console.Game.World.DebugAdd +="Durability removed from item named " + I.DBInfo.Name);
                            
                        }
                        else// if (I.ID != 0 && I.CurDur <= 1)
                        {
                           
                                if (I.CurDur == 1)
                                {
                                    I.CurDur = 0;
                                    GC.MyChar.EquipStats(i, false, true);
                                }
                        if (I.DBInfo.ID != 1050000 && I.DBInfo.ID != 1050001 && I.DBInfo.ID != 1050002 && I.DBInfo.ID != 1051000)
                        {
                            GC.LocalMessage(2011, "Be carefull, your item named " + I.DBInfo.Name + " current dura is broken, repair it now or the item will desappear!");
                        }
                            
                        }
                }
            }
        }
    }
}