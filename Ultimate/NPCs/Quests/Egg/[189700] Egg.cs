using System;
using System.Collections.Generic;
using Ultimate.Main;
using Ultimate.Game;
using System.Linq;

namespace Ultimate.NPCs
{
    public class NPC_189700 : NPCBase
    {
        public NPC_189700(Main.GameClient _client)
            : base(_client)
        {
            ID = 189700;
            Face = 14;
        }

        public override void Run(GameClient GC, byte[] Data, ushort _linkback)
        {
            Responses = new List<COPacket>();
            AddAvatar();

            switch (_linkback)
            {
                case 0:
                    AddText("I am collecting fruit packets. If you have any, I can exchange them for a special reward. You can recieve a +3 Item, +4 Item or +5 Item!\n");
                    AddText("You can also exchange 5 +1Stone Packs for a fruit packet!");
                    AddOption("Exchange Fruit Packet", 1);
                    AddOption("Exchange 5 +1 Stone Packs", 2); // New option added here
                    AddOption("Nevermind", 255);
                    break;

                case 1:
                    if (GC.MyChar.InventoryContains(720142, 1))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            GC.MyChar.RemoveItem(GC.MyChar.NextItem(720142));

                            AddText("Thank you for giving me the fruit packet. Here is your reward.");
                            AddOption("Thanks", 255);

                            Item newItem = GenerateRewardItem();
                            GC.MyChar.AddItem(newItem); // Add the random +3, +4, or +5 item to the player's inventory
                            GC.LocalMessage(2000, $"You received a +{newItem.Plus} item.");
                        }
                        else
                        {
                            AddText("Please make sure you have enough free slots in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have any Fruit Packets!");
                        AddOption("Alright", 255);
                    }
                    break;

                case 2: // New case for exchanging 5 +1 Stone Packs for a Fruit Pack
                    if (GC.MyChar.InventoryContains(723712, 5))
                    {
                        if (GC.MyChar.Inventory.Count <= 35)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                GC.MyChar.RemoveItem(GC.MyChar.NextItem(723712));
                            }

                            GC.MyChar.AddItem(new Item { ID = 720142 });

                            AddText("Thanks! Here is your fruit packet. Good luck!");
                            AddOption("Thanks", 255);
                        }
                        else
                        {
                            AddText("Please make sure you have enough free slots in your inventory.");
                            AddOption("Alright", 255);
                        }
                    }
                    else
                    {
                        AddText("You don't have enough +1 Stone Packs!");
                        AddOption("Alright", 255);
                    }
                    break;

                default:
                    break;
            }

            AddFinish();
            Send();
        }

        private Item GenerateRewardItem()
        {
            Random rnd = new Random();
            int plusValue = 3; // Default to +3
            int rareChance = rnd.Next(1, 101); // 1 to 100 for rare chance

            if (rareChance <= 5) // 5% chance for +4
            {
                plusValue = 4;
            }
            else if (rareChance == 1) // 1% chance for +5
            {
                plusValue = 5;
            }

            uint itemID = DetermineItemID(DeterminePart());
            DatabaseItem dbItem = Database.DatabaseItems[itemID];

            Item newItem = new Item
            {
                ID = itemID,
                Plus = (byte)plusValue,
                MaxDur = dbItem.Durability,
                CurDur = dbItem.Durability,
                Color = Item.ArmorColor.Orange // Example color
            };

            // Ensure the new item is of Normal quality
            ItemIDManipulation e = new ItemIDManipulation(newItem.ID);
            e.QualityChange(Item.ItemQuality.Normal);
            newItem.ID = e.ToID();

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
                .Where(d => d.LevReq >= 5 && d.LevReq <= 110 && ItemIDManipulation.Part(d.ID, 0, 3) == part)
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
