using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Features
{
    public class Bounty
    {
        public static void Delevel(Character C)
        {
            if (C.Level == 130)
            {
                C.Level--;
                ulong CurExp = Database.LevelExp[C.Level] - ((Database.LevelExp[129] * 10) / 100);
                World.Action(C, Packets.GeneralData(C.EntityID, 0, 0, 0, 92).Get);
                if (C.Reborn || C.Level >= 120)
                {
                    if (C.StatPoints >= 3)
                        C.StatPoints -= 3;
                    else if (C.Vit >= 3)
                        C.Vit -= 3;
                    else if (C.Spi >= 3)
                        C.Spi -= 3;
                    else if (C.Agi >= 3)
                        C.Agi -= 3;
                    else if (C.Str >= 3)
                        C.Str -= 3;
                }
                LowerGears(C);
                C.Experience = CurExp;
                Database.SaveCharacter(C, C.MyClient.AuthInfo.Account);
                World.SendMsgToAll("[SYSTEM]", "Ouch! " + C.Name + " was killed while having a bounty on his/her head and lost a level!", 2005, 0);
            }
            else if (!(C.Experience >= ((Database.LevelExp[C.Level] * 10) / 100)))
            {
                C.Level--;
                ulong CurExp = Database.LevelExp[C.Level] - ((Database.LevelExp[C.Level] * 10) / 100);
                World.Action(C, Packets.GeneralData(C.EntityID, 0, 0, 0, 92).Get);
                if (C.Reborn || C.Level >= 120)
                {
                    if (C.StatPoints >= 3)
                        C.StatPoints -= 3;
                    else if (C.Vit >= 3)
                        C.Vit -= 3;
                    else if (C.Spi >= 3)
                        C.Spi -= 3;
                    else if (C.Agi >= 3)
                        C.Agi -= 3;
                    else if (C.Str >= 3)
                        C.Str -= 3;
                }
                //LowerGears(C);
                C.Experience = CurExp;
                World.SendMsgToAll("[SYSTEM]", "Ouch! " + C.Name + " was killed while having a bounty on his/her head and lost a level!", 2005, 0);
            }
            else
                LooseExp(C);
        }
        public static void LooseExp(Character C)
        {
            ulong CurExp = C.Experience;
            byte CurLevel = C.Level;

            if (CurExp >= ((Database.LevelExp[CurLevel] * 10) / 100))
            {
                CurExp -= ((Database.LevelExp[CurLevel] * 10) / 100);
            }
            else
                Delevel(C);
            
            C.Experience = CurExp;
        }
        public static void LowerGears(Character C)
        {
            try
            {
                for (byte i = 1; i < 9; i++)
                {
                    Game.Item I = C.Equips.Get(i);
                    if (I.ID != 0)
                    {
                        byte PrevLevel = I.DBInfo.LevReq;
                        if (C.Level < PrevLevel)
                        {
                            ItemIDManipulation IMan = new ItemIDManipulation(I.ID);
                            IMan.LowerLevel();
                            DatabaseItem Di = (DatabaseItem)Database.DatabaseItems[IMan.ToID()];
                            byte NewLevel = Di.LevReq;
                            if (NewLevel < PrevLevel && NewLevel >= 120)
                            {
                                if (C.Level >= NewLevel)
                                {
                                    C.EquipStats(i, false, false);
                                    I.ID = IMan.ToID();
                                    I.MaxDur = I.DBInfo.Durability;
                                    I.CurDur = I.MaxDur;
                                    C.Equips.Replace(i, I, C);
                                    C.EquipStats(i, true, false);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

        }
    }
}
