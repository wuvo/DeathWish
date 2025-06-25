using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_721246 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Loc.Map == 1844)
            {
                if (Features.CounterClock.War)
                {
                    C.RemoveItem(C.NextItem(721246));
                    C.TakeAttack(null, C.MaxHP);
                    int Square = Program.Rnd.Next(1, 11);
                    if (MyMath.ChanceSuccess(15))
                    {
                        World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside Pole Square!", 2011, 0, 1844);
                        foreach (Character C2 in World.H_Chars.Values)
                        {
                            if (C2.Loc.Map == 1844)
                            {
                                if (C2.Loc.X >= 95 && C2.Loc.X <= 133 && C2.Loc.Y >= 146 && C2.Loc.Y <= 182)
                                {
                                    C2.TakeAttack(null, C2.MaxHP);
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (Square)
                        {
                            case 1:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the bottom right square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 211 && C2.Loc.X <= 244 && C2.Loc.Y >= 110 && C2.Loc.Y <= 144)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the bottom middle square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 211 && C2.Loc.X <= 244 && C2.Loc.Y >= 145 && C2.Loc.Y <= 185)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 3:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the bottom left square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 211 && C2.Loc.X <= 244 && C2.Loc.Y >= 186 && C2.Loc.Y <= 226)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 4:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the middle left square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 172 && C2.Loc.X <= 210 && C2.Loc.Y >= 186 && C2.Loc.Y <= 226)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 5:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the middle square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 172 && C2.Loc.X <= 210 && C2.Loc.Y >= 145 && C2.Loc.Y <= 185)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 6:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the middle right square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 172 && C2.Loc.X <= 210 && C2.Loc.Y >= 110 && C2.Loc.Y <= 144)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 7:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the top right square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 137 && C2.Loc.X <= 171 && C2.Loc.Y >= 110 && C2.Loc.Y <= 144)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 8:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the top middle square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 137 && C2.Loc.X <= 171 && C2.Loc.Y >= 145 && C2.Loc.Y <= 185)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 9:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside the top left square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 137 && C2.Loc.X <= 171 && C2.Loc.Y >= 186 && C2.Loc.Y <= 226)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                            case 10:
                                World.SendMsgToAll("BOMB", C.Name + " has used the CCGWBomb and killed all the players inside Pole Square!", 2011, 0, 1844);
                                foreach (Character C2 in World.H_Chars.Values)
                                {
                                    if (C2.Loc.Map == 1844)
                                    {
                                        if (C2.Loc.X >= 95 && C2.Loc.X <= 133 && C2.Loc.Y >= 146 && C2.Loc.Y <= 182)
                                        {
                                            C2.TakeAttack(null, C2.MaxHP);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                else
                    C.MyClient.LocalMessage(2005, "The CCGWBomb can only be used while Counter Clock Guild War is active!");
            }
            else
                C.MyClient.LocalMessage(2005, "The CCGWBomb can only be used inside the CCGW Map!");
        }
    }
}