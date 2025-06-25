using Ultimate.Game;
using Ultimate.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ultimate.Features.SkillsClass;

namespace Ultimate.PacketHandling.ItemPacket
{
    public class Equip
    {
        public static void HandleUnEquip(Main.GameClient GC, byte[] Data)
        {
            uint ItemUID = BitConverter.ToUInt32(Data, 4);

            if (GC.MyChar.Inventory.Count < 40)
            {
                byte Slot = GC.MyChar.Equips.GetSlot(ItemUID);
                if (Slot != 0)
                {
                    GC.MyChar.Inventory.Add(GC.MyChar.Equips.Get(Slot));
                    GC.MyChar.EquipStats(Slot, false, false);
                    GC.MyChar.Equips.UnEquip(Slot, GC.MyChar);
                    Game.World.Spawn(GC.MyChar, false);
                    Buff B = GC.MyChar.BuffOf(ExtraEffect.Fly);
                    if (Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) != 500)
                        if (B.Eff == ExtraEffect.Fly)
                            if (!GC.MyChar.BDelete.ContainsKey(B))
                                GC.MyChar.BDelete.TryAdd(B, B.Lasts);


                }
            }
        }
        public static bool FemaleGarment(uint ID)
        {
            if (ID == 182375 || ID == 182335 || ID == 182435 || ID == 182535 || ID == 182635 || ID == 182735 || ID == 182835 || ID == 182935 || ID == 182365 || ID == 182465 || ID == 182565 || ID == 182665 || ID == 182765 || ID == 182865 || ID == 182965 || ID == 182345 || ID == 182445 || ID == 182545 || ID == 182645 || ID == 182745 || ID == 182845 || ID == 182945 || ID == 182385 || ID == 182485 || ID == 182585 || ID == 182685 || ID == 182785 || ID == 182885 || ID == 182985 || ID == 182355 || ID == 182975 || ID == 182875 || ID == 182775 || ID == 182675 || ID == 182575 || ID == 182475 || ID == 182455 || ID == 182555 || ID == 182655 || ID == 182755 || ID == 182855 || ID == 182955)
                return true;

            return false;
        }
        public static bool MaleGarment(uint ID)
        {
            if (ID == 184385 || ID == 184395 || ID == 182435 || ID == 184405)
                return true;

            return false;
        }
        public static bool EquipPassSexReq(Game.Item Item, Game.Character C)
        {
            int ClientSex = C.Body % 10000 < 1005 ? 1 : 2;
            if (Item.DBInfo.GenderReq == 2 && ClientSex == 2)
                return true;
            else if (Item.DBInfo.GenderReq == 1 && ClientSex == 1)
                return true;
            else if (Item.DBInfo.GenderReq == 0)
                return true;
            return false;
        }
        public static bool EquipPassSexReq(uint ItemID, int ClientSex)
        {
            //int ClientSex = Body % 10000 < 1005 ? 1 : 2;
            if (Database.DatabaseItems.ContainsKey(ItemID))
            {
                if (Database.DatabaseItems[ItemID].GenderReq == 2 && ClientSex == 1)
                    return true;
                else if (Database.DatabaseItems[ItemID].GenderReq == 1 && ClientSex == 1)
                    return true;
                else if (Database.DatabaseItems[ItemID].GenderReq == 0)
                    return true;
                return false;
            }
            return false;
        }

        public static void HandleEquip(Main.GameClient GC, byte[] Data)
        {
            //if (GC.MyChar.LastTrade.AddMilliseconds(5000) > DateTime.Now)
            //   return;
            uint ItemUID = BitConverter.ToUInt32(Data, 4);
            byte Pos = Data[8];
            Game.Item I = GC.MyChar.FindInvItem(ItemUID);
            if (I.ID == 0)
            {
                GC.AddSend(Packets.ItemPacket(ItemUID, 0, 3));
                return;
            }


            if (!EquipPassSexReq(I, GC.MyChar))
                return;
            //if (FemaleGarment(I.ID))
            //{
            //    if (GC.MyChar.Body == 1003 || GC.MyChar.Body == 1004)
            //    {
            //        return;
            //    }
            //}
            //if (MaleGarment(I.ID))
            //    if (GC.MyChar.Body == 1001 || GC.MyChar.Body == 1002)
            //        return;
            bool Arrow = false;
            if (I.ID == 1050000 || I.ID == 1050001 || I.ID == 1050002 || I.ID == 1051000)
            { Pos = 5; Arrow = true; }
            if (Arrow)
            {
                if (GC.MyChar.Equips.Get(4).ID == 0)
                    return;
                else if (GC.MyChar.Equips.Get(4).ID / 10000 != 50)
                    return;
            }
            if (Game.ItemIDManipulation.Part(I.ID, 0, 3) >= 410 && Game.ItemIDManipulation.Part(I.ID, 0, 3) <= 590 && Game.ItemIDManipulation.Part(I.ID, 0, 3) != 500)
                if (Game.ItemIDManipulation.Part(GC.MyChar.Equips.Get(4).ID, 0, 3) == 500)
                    if (Game.ItemIDManipulation.Part(GC.MyChar.Equips.Get(5).ID, 0, 3) == 105)
                    {
                        GC.MyChar.EquipStats(5, false, false);
                        GC.MyChar.Inventory.Add(GC.MyChar.Equips.Get(5));
                        GC.MyChar.Equips.UnEquip(5, GC.MyChar);
                    }

            if (Game.ItemIDManipulation.Part(I.ID, 0, 3) == 900)
            {
                if (GC.MyChar.Equips.Get(4).ID == 0)
                    return;
                else if (Game.ItemIDManipulation.Part(GC.MyChar.Equips.Get(4).ID, 0, 3) > 490)
                    return;
            }
            if (Pos == 0)
                Items.Manager.ProcessItem(GC.MyChar, I);
            //GC.MyChar.UseItem(I);
            else
            if (I.CanEquip(GC.MyChar))
            {
                Game.Item Current = GC.MyChar.Equips.Get(Pos);
                if (Current.UID == 0 && (Pos != 4 || GC.MyChar.Equips.Get(5).UID == 0 || Game.ItemIDManipulation.Digit(I.ID, 1) != 5))
                {
                    GC.MyChar.RemoveItem(I);
                    GC.MyChar.Equips.Replace(Pos, I, GC.MyChar);
                    GC.MyChar.EquipStats(Pos, true, false);
                }
                else if (Current.UID != 0 && (Pos != 4 || GC.MyChar.Equips.Get(5).UID == 0 || Game.ItemIDManipulation.Digit(I.ID, 1) != 5))
                {
                    GC.MyChar.EquipStats(Pos, false, false);
                    GC.MyChar.RemoveItem(I);
                    GC.MyChar.AddItem(Current);
                    GC.MyChar.Equips.Replace(Pos, I, GC.MyChar);
                    GC.MyChar.EquipStats(Pos, true, false);
                }
                else if (GC.MyChar.Inventory.Count < 40)
                {
                    if (GC.MyChar.Equips.Get(4).UID != 0)
                    {
                        GC.MyChar.EquipStats(4, false, false);
                        GC.MyChar.RemoveItem(I);
                        GC.MyChar.AddItem(GC.MyChar.Equips.Get(4));
                        GC.MyChar.Equips.Replace(4, I, GC.MyChar);
                        GC.MyChar.EquipStats(4, true, false);
                    }
                    GC.MyChar.EquipStats(5, false, false);
                    GC.MyChar.Inventory.Add(GC.MyChar.Equips.Get(5));
                    GC.MyChar.Equips.UnEquip(5, GC.MyChar);
                }
                if (GC.MyChar.Loc.Map == 1039 && GC.MyChar.AtkMem.Skill != 0)
                    GC.MyChar.AtkMem.Attacking = false;
                int Dist = MyMath.PointDistance(GC.MyChar.Loc.X, GC.MyChar.Loc.Y, GC.MyChar.Loc.PreviousX, GC.MyChar.Loc.PreviousY);
                if (Dist < 7)
                    Dist = 7;
                if (GC.MyChar.LastMove.AddMilliseconds(Dist * 150) >= DateTime.Now)
                    GC.MyChar.ToUpdate = true;
                else
                    Game.World.Spawn(GC.MyChar, false);
            }
            Buff B = GC.MyChar.BuffOf(ExtraEffect.Fly);
            if (Game.ItemIDManipulation.Part(GC.MyChar.Equips.RightHand.ID, 0, 3) != 500)
                if (B.Eff == ExtraEffect.Fly)
                    if (!GC.MyChar.BDelete.ContainsKey(B))
                        GC.MyChar.BDelete.TryAdd(B, B.Lasts);
        }
    }
}
