using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.Linq; // Ensure this is included for LINQ methods like Contains
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Items
{
    public class Item_720374 : IItem
    {
        public override void Run(Character C, Item I)
        {
            // Existing logic for +1 stone pack, gem packs, etc.
            if (InventoryContains(C, 730001, 10)) // +1stonepack
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(730001));
                C.AddItem(723712);
            }
            else if (InventoryContains(C, 720027, 10)) //me bag
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(720027));
                C.AddItem(729912);
            }
            else if (InventoryContains(C, 700011, 10)) //Dragon gem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700011));
                C.AddItem(720126);
            }
            else if (InventoryContains(C, 700001, 10)) //Phoenix gem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700001));
                C.AddItem(720125);
            }
            else if (InventoryContains(C, 700021, 10)) //Fury gem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700021));
                C.AddItem(722358);
            }
            else if (InventoryContains(C, 700061, 10)) //moon gem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700061));
                C.AddItem(721285);
            }
            else if (InventoryContains(C, 700031, 10)) //Rainbowgem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700031));
                C.AddItem(722450);
            }
            else if (InventoryContains(C, 700041, 10)) //Kylin
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700041));
                C.AddItem(720129);
            }
            else if (InventoryContains(C, 700051, 10)) //Violetgem
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(700051));
                C.AddItem(721751);
            }
            else if (InventoryContains(C, 720028, 10)) // dbbag
            {
                for (int x = 0; x < 10; x++)
                    C.RemoveItem(C.NextItem(720028));
                C.AddItem(729910);
            }
            // New logic for fruit pack
            else if (InventoryContainsAnyFruit(C, 10)) // Any combination of 10 fruits
            {
                RemoveAnyFruits(C, 10);
                C.AddItem(720142); // Fruit pack item ID
            }
            else
            {
                C.MyClient.LocalMessage(2005, "Unable to pack +1StonePack, GemsPack, DBScrollBag, MetScrollBag, or FruitPack; not enough materials!");
            }
        }

        private bool InventoryContains(Character C, int itemId, int count)
        {
            return C.Inventory.Count(i => i.ID == itemId) >= count;
        }

        private bool InventoryContainsAnyFruit(Character C, int count)
        {
            List<int> fruitIds = new List<int> { 711001, 711002, 711003, 711004, 711005 };
            int fruitCount = 0;

            foreach (var item in C.Inventory)
            {
                if (fruitIds.Contains((int)item.ID)) // Cast item.ID to int
                {
                    fruitCount++;
                    if (fruitCount >= count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void RemoveAnyFruits(Character C, int count)
        {
            List<int> fruitIds = new List<int> { 711001, 711002, 711003, 711004, 711005 };
            int removedCount = 0;

            for (int i = 0; i < C.Inventory.Count && removedCount < count; i++)
            {
                Item item = C.Inventory[i];
                if (fruitIds.Contains((int)item.ID)) // Cast item.ID to int
                {
                    C.RemoveItem(item);
                    removedCount++;
                    i--; // Adjust index due to removal
                }
            }
        }
    }
}
