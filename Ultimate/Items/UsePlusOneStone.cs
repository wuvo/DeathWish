using Ultimate.Game;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ultimate.Items
{
    public class Item_730001 : IItem
    {
        public override void Run(Character C, Item I)
        {
            if (InventoryContains(C, 730001, 1)) // Check if inventory contains at least one +1 Stone
            {
                C.RemoveItem(C.NextItem(730001)); // Remove the used +1 Stone item

                Item newItem = GenerateNewItem(C); // Generate a new item based on the +1 Stone

                if (newItem != null)
                {
                    C.AddItem(newItem); // Add the new item to the character's inventory
                    C.MyClient.LocalMessage(2005, "You have successfully used a +1 Stone to generate a new item.");
                }
                else
                {
                    C.MyClient.LocalMessage(2005, "Failed to generate a new item from +1 Stone.");
                }
            }
            else
            {
                C.MyClient.LocalMessage(2005, "No +1 Stones available in inventory.");
            }
        }

        private bool InventoryContains(Character C, int itemId, int count)
        {
            return C.Inventory.Count(i => i.ID == itemId) >= count;
        }

        private Item GenerateNewItem(Character C)
        {
            Item newItem = new Item();
            newItem.UID = (uint)new Random().Next(10000000);

            uint part = DeterminePart();
            uint itemId = DetermineItemID(part);

            if (itemId != 0)
            {
                newItem.ID = itemId;
                newItem.Plus = 1;
                newItem.MaxDur = 100; // Example max durability
                newItem.CurDur = 100; // Example current durability
                newItem.Color = Item.ArmorColor.Orange; // Example color

                // Ensure the new item is of Normal quality
                Game.ItemIDManipulation e = new Game.ItemIDManipulation(newItem.ID);
                e.QualityChange(Game.Item.ItemQuality.Normal);
                newItem.ID = e.ToID();
            }

            return newItem;
        }

        private uint DeterminePart()
        {
            int type = new Random().Next(0, 340);
            uint part = 0;

            if (type < 10) part = 111;
            else if (type < 20) part = 113;
            else if (type < 30) part = 114;
            else if (type < 40) part = 117;
            else if (type < 50) part = 118;
            else if (type < 60) part = 120;
            else if (type < 70) part = 121;
            else if (type < 80) part = 130;
            else if (type < 90) part = 131;
            else if (type < 100) part = 133;
            else if (type < 110) part = 134;
            else if (type < 120) part = 141;
            else if (type < 130) part = 142;
            else if (type < 140) part = 150;
            else if (type < 150) part = 151;
            else if (type < 160) part = 152;
            else if (type < 165) part = 160;
            else if (type < 175) part = 410;
            else if (type < 185) part = 420;
            else if (type < 195) part = 421;
            else if (type < 205) part = 430;
            else if (type < 215) part = 440;
            else if (type < 225) part = 450;
            else if (type < 235) part = 460;
            else if (type < 245) part = 480;
            else if (type < 255) part = 481;
            else if (type < 265) part = 490;
            else if (type < 275) part = 500;
            else if (type < 285) part = 510;
            else if (type < 295) part = 530;
            else if (type < 305) part = 540;
            else if (type < 315) part = 560;
            else if (type < 325) part = 561;
            else if (type < 335) part = 580;
            else if (type < 340) part = 900;

            return part;
        }

        private uint DetermineItemID(uint part)
        {
            List<uint> possibleItems = Database.DatabaseItems.Values
                .Where(d => d.LevReq >= 5 && d.LevReq <= 110 && Game.ItemIDManipulation.Part(d.ID, 0, 3) == part)
                .Select(d => d.ID)
                .ToList();

            if (possibleItems.Count > 0)
            {
                int index = new Random().Next(possibleItems.Count);
                return possibleItems[index];
            }

            return 0; // Fallback ID if no items found
        }
    }
}
