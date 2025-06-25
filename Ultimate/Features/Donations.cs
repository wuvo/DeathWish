using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ultimate
{
    public class Donations
    {
        public const string connectionString = "server=localhost;username=root;password=Dreamysky77@;database=ultimate;";

        internal bool processOrder(string productId, int quantity, Game.Character C)
        {
            int requiredSlots = quantity;

            if (C.GetAvailableInventorySlots() < requiredSlots)
            {
                updateStatus("Not enough inventory space.", C);
                return false;
            }

            Console.WriteLine($"Processing order: Product ID = {productId}, Quantity = {quantity}");

            switch (productId)
            {
                case "1": // VIP 3 Days
                    for (var i = 0; i < quantity; i++)
                        ApplyVIPCard(C, 780001, 5, 3);
                    return true;
                case "2": // VIP 7 Days
                    for (var i = 0; i < quantity; i++)
                        ApplyVIPCard(C, 780001, 5, 7);
                    return true;
                case "3": // VIP 15 Days
                    for (var i = 0; i < quantity; i++)
                        ApplyVIPCard(C, 780001, 5, 15);
                    return true;
                case "4": // VIP 30 Days
                    for (var i = 0; i < quantity; i++)
                        ApplyVIPCard(C, 780001, 5, 30);
                    return true;
                case "5": // Akatsuki Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(193255);
                    return true;
                case "6": // Anniversary Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192195);
                    return true;
                case "7": // Assassin Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(188140);
                    return true;
                case "8": // Cupid Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(193245);
                    return true;
                case "9": // Dragon Gem
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(700013);
                    return true;
                case "10": // Phoenix Gem
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(700003);
                    return true;
                case "11": // Rainbow Gem
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(700033);
                    return true;
                case "12": // Silver Prize
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(2100065);
                    return true;
                case "14": // Frozen Fantasy
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(191020);
                    return true;
                case "15": // Assassin G
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(188140);
                    return true;
                case "16": // Assassin Y
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(188190);
                    return true;
                case "17": // Assassin B
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(188180);
                    return true;
                case "18": // Jungle Deer
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(189685);
                    return true;
                case "19": // Heavenly Warrior
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(193725);
                    return true;
                case "20": // Oblique
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(194795);
                    return true;
                case "21": // Kingdom King
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192465);
                    return true;
                case "22": // Yellow Wizard
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(181100);
                    return true;
                case "23": // Blue Duffel
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192505);
                    return true;
                case "24": // Red Duffel
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192525);
                    return true;
                case "25": // Turquoise Duffel
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192535);
                    return true;
                case "26": // Orange Duffel
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(192545);
                    return true;
                case "27": // Halloween Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(199415);
                    return true;
                case "28": // Power Garm
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(194995);
                    return true;
                case "29": // River Spirit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(188995);
                    return true;
                case "30": // Ragnarok Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(189025);
                    return true;
                case "31": // Dragon Robe
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(189035);
                    return true;
                case "32": // Hades Suit
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(189045);
                    return true;
                case "33": // Freeze Blade
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720170);
                    return true;
                case "34": // Dragon Blade
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720171);
                    return true;
                case "35": // Flat Blade
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720172);
                    return true;
                case "36": // Demon Sword
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720173);
                    return true;
                case "37": // Buried Sword
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720174);
                    return true;
                case "38": // Meteor Sword
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720175);
                    return true;
                case "39": // Golden Dagger
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720176);
                    return true;
                case "40": // Freeze Club
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720177);
                    return true;
                case "41": // God of Clubs
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720178);
                    return true;
                case "42": // Longest Club
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720179);
                    return true;
                case "43": // Bright Earth
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720180);
                    return true;
                case "44": // Magic Hammer
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720181);
                    return true;
                case "45": // Magic Sword
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720182);
                    return true;
                case "46": // Swipe Bow
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720183);
                    return true;
                case "47": // Fire Bow
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720184);
                    return true;
                case "48": // Gods Bow
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720185);
                    return true;
                case "49": // Gods Blade
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720186);
                    return true;
                case "50": // Enchanted Bow
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720187);
                    return true;
                case "51": // Poseidon Spear
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720188);
                    return true;
                case "52": // Xerxes Spear
                    for (var i = 0; i < quantity; i++)
                        C.AddItem(720189);
                    return true;
                default:
                    updateStatus("Product not found", C);
                    return false;
            }
        }

        private void ApplyVIPCard(Game.Character C, int itemId, int vipLevel, int vipDays)
        {
            Game.Item I = new Game.Item
            {
                ID = (uint)itemId,
                UID = (uint)new Random().Next(10000000),
                Bless = (byte)vipLevel,
                Plus = (byte)vipDays
            };

            C.AddItem(I);
        }

        public void check(Game.Character C)
        {
            bool throttled = false;

            if (throttled)
            {
                long timeNow = 0; // Implement time check logic
                long lastDonationCheck = 0; // Implement last check time logic
                long e = timeNow - lastDonationCheck;
                if (e < 30000)
                {
                    long rem = 30 - (e / 1000);
                    updateStatus("Rate limit reached! Please try again in " + rem + " seconds.", C);
                    return;
                }
            }
            Thread checkDonationThread = new Thread(() => doDbCheck(C));
            checkDonationThread.Start();
        }

        internal void updateStatus(string message, Game.Character C)
        {
            C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", C.Name, message, 2001, 0));
        }

        internal bool markAsCollected(string orderId, MySqlConnection connection)
        {
            try
            {
                using (var updateCommand = new MySqlCommand("UPDATE payments SET collected=1 WHERE orderId=@orderId", connection))
                {
                    updateCommand.Parameters.AddWithValue("@orderId", orderId);
                    updateCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.ToString());
            }
            return false;
        }

        internal bool processDbResult(MySqlDataReader reader, Game.Character C)
        {
            bool atleastOne = false;
            if (!reader.Read())
            {
                Console.WriteLine("[STORE] Nothing found for executed query");
                return false;
            }
            else
            {
                string orderId = reader["orderId"].ToString();
                string user = reader["charname"].ToString();
                string paymentMethod = reader["payment_method"].ToString();
                decimal amount = decimal.Parse(reader["payment_amount"].ToString());
                string cartItemList = reader["cart"].ToString();

                // Deserialize JSON string to dictionary
                Dictionary<string, int> items = JsonConvert.DeserializeObject<Dictionary<string, int>>(cartItemList);

                foreach (var item in items)
                {
                    string productId = item.Key;
                    int quantity = item.Value;

                    bool processed = processOrder(productId, quantity, C);
                    if (processed)
                    {
                        atleastOne = true;
                    }
                }
            }
            return atleastOne;
        }

        internal bool doDbCheck(Game.Character C)
        {
            bool ok = false;
            string charName = C.Name;
            Console.WriteLine("Scheduling collecting for " + charName);
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            updateStatus("Checking for donations", C);
            try
            {
                using (var cmd = new MySqlCommand("SELECT * FROM payments WHERE charname=@charName AND collected=0", connection))
                {
                    cmd.Parameters.AddWithValue("@charName", charName);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool processed = processDbResult(reader, C);
                        if (!processed)
                        {
                            updateStatus("No donations found or Inventory Is full, Ensure you have enough space then relog.", C);
                            ok = false;
                        }
                        else
                        {
                            string orderId = reader["orderId"].ToString();
                            reader.Close();
                            ok = markAsCollected(orderId, connection);
                            if (ok)
                            {
                                updateStatus("Thanks for donating!", C);
                            }
                            else
                            {
                                updateStatus("ERROR EZ6444 - Contact admin for a reward!", C);
                            }
                        }
                    }
                }
            }
            catch (Exception Exc)
            {
                Console.WriteLine(Exc.ToString());
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return ok;
        }
    }
}
