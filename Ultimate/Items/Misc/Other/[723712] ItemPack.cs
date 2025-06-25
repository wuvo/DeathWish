using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Game;

namespace Ultimate.Items
{
    public class Item_723712 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (C.Inventory.Count <= 35)
            {
                C.RemoveItem(C.NextItem(723712));
                #region +1 Item
                for (int a = 0; a < 10; a++)
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

                        I2.Plus = 1;
                        I2.MaxDur = I2.DBInfo.Durability;
                        I2.CurDur = I2.MaxDur;

                        C.AddItem(I2);
                    }
                    else goto top;
                }
                #endregion
            }

        }
    }
}