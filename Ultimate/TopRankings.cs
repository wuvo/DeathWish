using Ultimate.Game;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Main;
using System.Collections.Concurrent;

namespace Ultimate
{
    public class TopRankings
    {
        public enum TopsType : byte
        {
            Potency = 1,
            Money = 2,
            Virtue = 3,
            PK = 4,
            KO = 5,
            MyElites = 6,
            PVPPoints = 7,
            Killers = 8
            /* WHSilvers = 9*/

        }
        public static ConcurrentDictionary<uint, GameClient> Players = new ConcurrentDictionary<uint, GameClient>();
        public static Dictionary<string, uint> Results = new Dictionary<string, uint>();
        public static void LoadTops()
        {
            try
            {
                string charDir = @"C:\OldCODB\Users\Characters\";
                var files = Directory.GetFiles(charDir);
                List<Character> Players = new List<Character>();
                foreach (var file in files.Where(e => e.EndsWith(".chr")))
                {
                    string name = file.Replace(".chr", "").Replace(charDir, "");
                    string Acc = "";
                    var c = Database.LoadCharacter(name, ref Acc);
                    if (c != null)
                        if (c.Name.Contains("[PM]"))
                            continue;
                        else
                            Players.Add(c);
                }
                using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString))
                {
                    conn.Open();
                    #region ClearCommand
                    try
                    {
                        using (var cmdd = new MySql.Data.MySqlClient.MySqlCommand("DELETE FROM tops where toptype <> 5", conn))// skip ko board
                            cmdd.ExecuteNonQuery();

                        //Console.WriteLine("[TOPS] Deleted old tops..");

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    #endregion

                    #region Potency
                    foreach (var p in Players.OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", (byte)TopsType.Potency);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");

                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region PKPoints
                    foreach (var p in Players.OrderByDescending(e => e.PKPoints).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", (byte)TopsType.PK);
                                cmd.Parameters.AddWithValue("@param", p.PKPoints);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region Silvers
                    foreach (var p in Players.OrderByDescending(e => e.WHSilvers + e.Silvers).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", (byte)TopsType.Money);
                                cmd.Parameters.AddWithValue("@param", p.Silvers + p.WHSilvers);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion

                    #region WHSilvers
                    /*foreach (var p in Players.OrderByDescending(e => e.WHSilvers + e.Silvers).Take(10))
                    {
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", (byte)TopsType.WHSilvers);
                                cmd.Parameters.AddWithValue("@param", p.WHSilvers);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }*/
                    #endregion

                    #region VirtuePoints
                    foreach (var p in Players.OrderByDescending(e => e.VP).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", (byte)TopsType.Virtue);
                                cmd.Parameters.AddWithValue("@param", p.VP);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region TopTrojans
                    foreach (var p in Players.Where(e => e.Job >= 11 && e.Job <= 15).OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", 15);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region TopWarrior
                    foreach (var p in Players.Where(e => e.Job >= 21 && e.Job <= 25).OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", 25);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region TopArcher
                    foreach (var p in Players.Where(e => e.Job >= 41 && e.Job <= 45).OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", 45);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region TopFire
                    foreach (var p in Players.Where(e => e.Job >= 142 && e.Job <= 145).OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {
                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", 145);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion
                    #region TopWater
                    foreach (var p in Players.Where(e => e.Job >= 132 && e.Job <= 135).OrderByDescending(e => e.Potency).Take(10))
                    {
                        string Nobility1;

                        if (p.Nobility.Rank == Ranks.Duke)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Duke";
                            else
                                Nobility1 = "Duchess";
                        else if (p.Nobility.Rank == Ranks.Prince)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Prince";
                            else
                                Nobility1 = "Princess";
                        else if (p.Nobility.Rank == Ranks.King)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "King";
                            else
                                Nobility1 = "Queen";
                        else if (p.Nobility.Rank == Ranks.Knight)
                            Nobility1 = "Knight";
                        else if (p.Nobility.Rank == Ranks.Baron)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Baron";
                            else
                                Nobility1 = "Baroness";
                        else if (p.Nobility.Rank == Ranks.Earl)
                            if (p.Body == 1003 || p.Body == 1004)
                                Nobility1 = "Earl";
                            else
                                Nobility1 = "Countess";
                        else
                            Nobility1 = "Serf";
                        try
                        {

                            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("INSERT INTO tops VALUES(@name,@top,@param,@level,@vip,@guildname,@nobility,@spouse,@avatar,@job)", conn))
                            {
                                cmd.Parameters.AddWithValue("@name", p.Name);
                                cmd.Parameters.AddWithValue("@top", 135);
                                cmd.Parameters.AddWithValue("@param", p.Potency);
                                cmd.Parameters.AddWithValue("@level", p.Level);
                                cmd.Parameters.AddWithValue("@vip", p.VipLevel);
                                cmd.Parameters.AddWithValue("@nobility", Nobility1);
                                cmd.Parameters.AddWithValue("@spouse", p.Spouse);
                                cmd.Parameters.AddWithValue("@avatar", p.Avatar);
                                cmd.Parameters.AddWithValue("@job", p.Job);
                                if (p.MyGuild != null)
                                    cmd.Parameters.AddWithValue("@guildname", p.MyGuild.GuildName);
                                else
                                    cmd.Parameters.AddWithValue("@guildname", "None");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                    #endregion


                    //  Console.WriteLine("[TOPS] Rankings are reloaded.");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

