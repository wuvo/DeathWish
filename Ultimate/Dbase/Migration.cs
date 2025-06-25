using Ultimate.Features;
using Ultimate.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultimate.Dbase
{
    public class Migration
    {
        public static void MigrateAccounts()
        {
            try
            {
                string[] Paths = Directory.GetFiles(World.GlobalAccountsPath);
                int Count = 0;
                Count = Paths.Length;
                foreach (string Path in Paths)
                {
                    if (Path.Remove(0, Path.Length - 4) == ".usr")
                    {
                        try
                        {
                            byte[] buffer = File.ReadAllBytes(Path);
                            MemoryStream ms = new MemoryStream(buffer);
                            BinaryReader BR = new BinaryReader(ms);
                            string Password = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                            string status = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadByte()));
                            int Status = 0;
                            if (status == "[GM]")
                                Status = 1;
                            else if (status == "[PM]")
                                Status = 3;

                            string Character = "";
                            if (BR.BaseStream.Position != BR.BaseStream.Length)
                            {
                                byte len = BR.ReadByte();
                                Character = Encoding.ASCII.GetString(BR.ReadBytes(len));
                            }
                            BR.Close();
                            ms.Close();

                            string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                            uint UID = 0;

                            if (File.Exists(World.GlobalCharactersPath + Character + ".chr"))
                            {
                                Character C;
                                C = Database.LoadCharacter(Character, ref Name);
                                UID = C.EntityID;
                                try
                                {
                                    MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                                    Cmd2.Insert("characters").Insert("UID", UID).Insert("Name", C.Name).Execute();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    World.ExcAdd += e.ToString() + "\r\n";
                                }
                            }
                            else if (File.Exists(World.BannedChars + Character + ".chr"))
                            {
                                string Acc = "";
                                Character C = new Character();
                                byte[] buffer2 = File.ReadAllBytes(World.BannedChars + Character + ".chr");
                                MemoryStream ms2 = new MemoryStream(buffer2);
                                BinaryReader BR2 = new BinaryReader(ms2);

                                C.Name = Character;
                                Acc = BR2.ReadString();
                                C.EntityID = BR2.ReadUInt32();

                                UID = C.EntityID;
                                Status = 5;
                                try
                                {
                                    MySQL.MySqlCommand Cmd2 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                                    Cmd2.Insert("characters").Insert("UID", UID).Insert("Name", C.Name).Execute();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    World.ExcAdd += e.ToString() + "\r\n";
                                }
                            }
                            else
                            {
                                ms = new MemoryStream();
                                BinaryWriter BW = new BinaryWriter(ms);
                                BW.Write(Password);
                                BW.Write(status);
                                buffer = ms.ToArray();
                                BW.Close();
                                ms.Close();
                                File.WriteAllBytes(World.GlobalAccountsPath + Name + ".usr", buffer);

                                UID = (uint)Program.Rnd.Next(1000001, 19999999);
                                while (World.EIDS.Contains(UID))
                                    UID = (uint)Program.Rnd.Next(1000001, 19999999);

                                World.EIDS.Add(UID);
                            }
                            string Email = "";
                            if (File.Exists(World.GlobalAccountsPath + Name + ".txt"))
                                Email = File.ReadAllText(World.GlobalAccountsPath + Name + ".txt");
                            else
                                Email = "Missing";

                            
                            if (Password.Length != 60)
                                Console.WriteLine("Password Lenght == " + Password.Length);
                            try
                            {
                                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                                Cmd.Insert("accounts").Insert("UID", UID).Insert("Username", Name).Insert("Password", Password).Insert("Email", Email).Insert("Status", Status).Execute();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                World.ExcAdd += e.ToString() + "\r\n";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            World.ExcAdd += e.ToString() + "\r\n";
                        }
                    }
                    Count--;
                    //else
                    //    Console.WriteLine("Some error");
                }
                Console.WriteLine("Finished migrating accounts");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void MigrateGuilds()
        {
            //if (File.Exists(@"C:\OldCODB\Guilds.dat"))
            //{
            //    byte[] buffer = File.ReadAllBytes(@"C:\OldCODB\Guilds.dat");
            //    MemoryStream ms = new MemoryStream(buffer);
            //    BinaryReader BR = new BinaryReader(ms);
            //    int GuildsCount = BR.ReadInt32();
            //    for (int i = 0; i < GuildsCount; i++)
            //    {

            //        Guild G = new Guild(BR);
            //        if (G.GuildID != 0)
            //            Guilds.AllTheGuilds.Add(G.GuildID, G);
            //        else
            //        {
            //            World.ExcAdd += G.GuildName + " was not added! " + G.Creator.MembName + " was the GL! \r\n";
            //        }
            //    }
            //    ushort LGWIN = BR.ReadUInt16();
            //    if (Guilds.AllTheGuilds.ContainsKey(LGWIN))
            //        GuildWars.LastWinner = (Guild)Guilds.AllTheGuilds[LGWIN];
            //    else GuildWars.LastWinner = null;
            //    BR.Close();
            //    ms.Close();
            //}

            Guilds.SaveGuilds();
        }

        public static void MoveFirstValues()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            foreach (string Path in Directory.GetFiles(World.GlobalCharactersPath))
            {
                if (Path.Remove(0, Path.Length - 4) == ".chr")
                {
                    try
                    {
                        string Name = Path.Substring(Path.LastIndexOf("\\") + 1, Path.LastIndexOf('.') - Path.LastIndexOf("\\") - 1);
                        Character C;
                        C = World.CharacterFromName2(Name);
                        if (C == null)
                        {
                            string Account = "";
                            C = Database.LoadCharacter(Name, ref Account);
                            if (C != null)
                            {
                                MySQL.MySqlCommand Update = new MySQL.MySqlCommand(MySQL.MySqlCommandType.UPDATE);
                                Update.Update("characters").Set("Level", C.Level).Set("Experience", C.Experience).Set("Spouse", C.Spouse).Set("Body", C.Body).Set("Face", C.Avatar).Set("Hair", C.Hair).Set("Silvers", C.Silvers).Set("WHSilvers", C.WHSilvers).Set("GuildID", C.GuildID).Set("Map", C.Loc.Map).Set("X", C.Loc.X).Set("Y", C.Loc.Y).Set("Job", C.Job).Set("PreviousJob1", C.PreviousJob1).Set("Strength", C.Str).Set("Agility", C.Agi).Set("Spirit", C.Spi).Set("Vitality", C.Vit).Set("ExtraStats", C.StatPoints).Set("Life", C.CurHP).Set("Mana", C.CurMP).Set("VirtuePoints", C.VP).Set("DBScrolls", C.DBScrolls).Set("VIPLevelToReceive", C.VIPLevelToReceive).Set("VIPDaysToReceive", C.VIPDaysToReceive).Set("DoubleExp", C.DoubleExpLeft).Set("WHPassword", C.WHPassword)/*.Set("VIP", C.VIP)*/.Set("PumpkinPoints", C.PumpkinPoints).Set("TreasurePoints", C.TreasurePoints).Set("CTBPoints", C.CTBPoints).Set("MetScrolls", C.MetScrolls).Set("DragonGems", C.DragonGems).Set("PhoenixGems", C.PhoenixGems).Set("RainbowGems", C.RainbowGems).Set("KylinGems", C.KylinGems).Set("FuryGems", C.FuryGems).Set("VioletGems", C.VioletGems).Set("MoonGems", C.MoonGems).Set("TortoiseGems", C.TortoiseGems).Set("Dragonballs", C.Dragonballs).Set("Ultimates", C.Ultimates).Set("GarmentToken", C.GarmentToken).Set("OnlineTime", C.OnlineTime).Set("CurrentKills", C.CurrentKills).Set("Nobility", C.Nobility.Donation).Set("PKPoints", C.PKPoints)/*.Set("LastLogin", C.LastLogin)*/.Set("BotJailedDays", C.BOTJailedDays).Set("MutedDays", C.MutedDays).Set("VotePoints", C.VotePoints).Set("ClassicPoints", C.ClassicPoints)/*.Set("LastVote", C.LastVote)*/.Where("UID", C.EntityID).Execute();

                                MySQL.MySqlCommand Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");

                                Item I = new Item();
                                if (C.Equips.HeadGear.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.HeadGear;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.Necklace.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.Necklace;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.Ring.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.Ring;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.Armor.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.Armor;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.Boots.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.Boots;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.Garment.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.Garment;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.LeftHand.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.LeftHand;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                if (C.Equips.RightHand.ID != 0)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    I = C.Equips.RightHand;
                                    Insert.Insert("UID", I.UID).Insert("StaticID", I.ID).Insert("Owner", C.EntityID).Insert("Location", C.Equips.GetSlot(I.UID) + 10).Insert("Plus", I.Plus).Insert("Bless", I.Bless).Insert("Enchant", I.Enchant).Insert("Gem1", (byte)I.Soc1).Insert("Gem2", (byte)I.Soc2).Insert("MaxDura", I.MaxDur).Insert("CurDura", I.CurDur).Insert("Color", (byte)I.Color).Insert("Effect", (ushort)I.Effect).Insert("Progress", I.Progress).Insert("TalismanProgress", I.TalismanProgress).Insert("FreeItem", I.FreeItem).Insert("RestrainType", I.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Inventory)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 0).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.ACWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 10028).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.BIWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 10027).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.TCWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 8).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.PCWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 10012).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.DCWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 10011).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.MAWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 44).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.MAWarehouse2)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 46).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.SCWarehouse)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 4101).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.HouseWH1)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 2100).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }
                                foreach (Item I2 in C.Warehouses.HouseWH2)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("items");
                                    Insert.Insert("UID", I2.UID).Insert("StaticID", I2.ID).Insert("Owner", C.EntityID).Insert("Location", 2101).Insert("Plus", I2.Plus).Insert("Bless", I2.Bless).Insert("Enchant", I2.Enchant).Insert("Gem1", (byte)I2.Soc1).Insert("Gem2", (byte)I2.Soc2).Insert("MaxDura", I2.MaxDur).Insert("CurDura", I2.CurDur).Insert("Color", (byte)I2.Color).Insert("Effect", (ushort)I2.Effect).Insert("Progress", I2.Progress).Insert("TalismanProgress", I2.TalismanProgress).Insert("FreeItem", I2.FreeItem).Insert("RestrainType", I2.RestrainType).Execute();
                                }

                                foreach (Skill S in C.Skills.Values)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("skills");
                                    byte PreviousLevel = 0;
                                    if (C.SkillsBeforeReborn.ContainsKey(S.ID))
                                        PreviousLevel = C.SkillsBeforeReborn[S.ID].Lvl;
                                    Insert.Insert("Owner", C.EntityID).Insert("ID", S.ID).Insert("Level", S.Lvl).Insert("Experience", S.Exp).Insert("PreviousLevel", PreviousLevel).Execute();
                                }
                                foreach (Prof P in C.Profs.Values)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("proficiencies");
                                    byte PreviousLevel = 0;
                                    if (C.ProfsBeforeReborn.ContainsKey(P.ID))
                                        PreviousLevel = C.ProfsBeforeReborn[P.ID].Lvl;
                                    Insert.Insert("Owner", C.EntityID).Insert("ID", P.ID).Insert("Level", P.Lvl).Insert("Experience", P.Exp).Insert("PreviousLevel", PreviousLevel).Execute();
                                }
                                foreach (uint F in C.Friends.Keys)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("associates");
                                    Insert.Insert("UID", C.EntityID).Insert("AssociateID", F).Insert("Type", 1).Execute();
                                }
                                foreach (uint F in C.Enemies.Keys)
                                {
                                    Insert = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT).Insert("associates");
                                    Insert.Insert("UID", C.EntityID).Insert("AssociateID", F).Insert("Type", 0).Execute();
                                }
                            }
                            else
                                Console.WriteLine("Something went wrong");
                        }
                        else
                            Console.WriteLine("Something went wrong");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            stopwatch.Stop();
            TimeSpan T = stopwatch.Elapsed;
            Console.WriteLine($"Done in: {T}");
        }

        public static void MigrateNPCs()
        {
            try
            {
                foreach (Dictionary<uint, NPC> Map in World.H_NPCs.Values)
                {
                    foreach (NPC N in Map.Values)
                    {
                        MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                        Cmd.Insert("npcs").Insert("UID", N.EntityID).Insert("Type", N.Type).Insert("Flags", N.Flags).Insert("Face", N.Avatar).Insert("Map", N.Loc.Map).Insert("X", N.Loc.X).Insert("Y", N.Loc.Y).Execute();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Finished Migrating NPCS !");
        }

        public static void RemoveBans()
        {
            try
            {
                foreach (string Name in World.BanChars)
                {
                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.COPY);
                    Cmd.Copy("characters","bannedchars").Where("Name", Name).Execute();

                    Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("characters", "Name", Name);
                    Cmd.Execute();

                    Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE).Delete("guildmembers", "Name", Name);
                    Cmd.Execute();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
