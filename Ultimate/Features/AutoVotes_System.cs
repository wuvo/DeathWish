using System;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using Ultimate.Game;
using Ultimate;
using System.Configuration;

namespace Ultimate
{
    class Votes
    {
       // #region Database Connection Variables
       // static string server = "localhost";
       // static string database = "characterinfo";
       // static string uid = "root";
       // static string password = "";
       // static readonly string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
        // database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
       // MySqlCommand Cmd_MySQL;
       // TableDATA Table_CharData;
       // IPSTable IPTable;
       // MySqlDataReader DataRead_MySQL;
        //string nr = "Select COUNT(*) FROM votes";
        //string nr1 = "Select COUNT(*) FROM voteips";
       // string CharactersVotes = "Select idvotes from votes";
        //string IPSVotes = "Select id from voteips";
        //readonly MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);
       // struct TableDATA
        //{
        //    public string CharacterName, IPAddress;
        //    public DateTime LastVote;
        //    public int ID;
       // }
       // #endregion
        //struct IPSTable
       //{
        //    public string IPAddress;
        //    public DateTime TimeVote;
        //    public int ID;
        //}
        public Votes()
        {
            VotesProcess();
            CheckIPs();
        }
        public void VotesProcess()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("votes");
                MySQL.MySqlReader Votes = new MySQL.MySqlReader(Cmd);

                //2.Check for votes
                while (Votes.Read())
                    AddVote(Votes.ReadInt32("idvotes"), Votes.ReadString("EntityID"), Votes.ReadString("IPAddress"), Convert.ToDateTime(Votes.ReadString("LastVote")));
            }

            //Any errors go to this file
            catch (Exception exc)
            {
                string pathErr = "C:\\Debug\\Errors_Mysql.txt";
                if (Directory.Exists("C:\\Debug"))
                {
                    StreamWriter svc = File.AppendText(pathErr);

                    svc.WriteLine("Error MYSQL: " + exc.ToString() + "|||Date : " + DateTime.Now);
                    svc.WriteLine("  ");
                    svc.Flush();
                    svc.Close();
                }
            }
        }
        public void CheckIPs()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("voteips");
                MySQL.MySqlReader Votes = new MySQL.MySqlReader(Cmd);
                
                while (Votes.Read())
                {
                    if (!World.VotedIps.Contains(Votes.ReadString("ips")))
                        World.VotedIps.Add(Votes.ReadString("ips"));
                    RemoveIPAdress(Votes.ReadString("ips"), Convert.ToDateTime(Votes.ReadString("votetime")));
                }
            }
            //Any errors go to this file
            catch (Exception exc)
            {
                string pathErr = "C:\\Debug\\Errors_Mysql.txt";
                //Connect_MySQL.Close();
                if (Directory.Exists("C:\\Debug"))
                {
                    StreamWriter svc = File.AppendText(pathErr);

                    svc.WriteLine("Error MYSQL: " + exc.ToString() + "|||Date : " + DateTime.Now);
                    svc.WriteLine("  ");
                    svc.Flush();
                    svc.Close();
                }
            }
        }


        void AddVote(int id, string CharacterName, string IPAddress, DateTime LastVote)
        {
            Character C = World.CharacterFromName2(CharacterName);
            try
            {
                bool Sucess = false;
                if (C != null)
                {
                    if (!World.VotedIps.Contains(IPAddress))
                        World.VotedIps.Add(IPAddress);
                    C.VotePoints++;
                    C.MyClient.LocalMessage(2000, "Thank you for voting for the server! You have received one Vote Point in return! Talk to VoteManagement and exchange it for treasures!");
                    Sucess = true;
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(CharacterName, ref Account);
                    if (C != null)
                    {
                        if (!World.VotedIps.Contains(IPAddress))
                            World.VotedIps.Add(IPAddress);
                        C.VotePoints++;
                        Database.SaveCharacter(C, Account);
                        Sucess = true;
                    }
                }

                if (Sucess)
                {
                    MySQL.MySqlCommand Payment = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                    Payment.Delete("votes", "idvotes", id).Execute();

                    MySQL.MySqlCommand VoteIP = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
                    VoteIP.Insert("voteips").Insert("ips", IPAddress).Insert("votetime", LastVote.ToString("yyyy-MM-dd HH:mm:ss")).Execute();

                    MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("vote_history").Where("CharacterName", CharacterName);
                    MySQL.MySqlReader Votes = new MySQL.MySqlReader(Cmd);

                    //2.Check for votes
                    int Amount = 0;
                    while (Votes.Read())
                        Amount = Votes.ReadInt32("Total");
                    Amount = Amount + 1;

                    MySQL.MySqlCommand AddVote = new MySQL.MySqlCommand(MySQL.MySqlCommandType.ONDUPLICATEKEY);
                    AddVote.Insert("vote_history").Insert("CharacterName", CharacterName).Insert("Total", Amount).Execute();
                }
            }
            catch (Exception exc)
            {
                World.ExcAdd += exc + "\r\n";
            }
        }
        void RemoveIPAdress(string IPAddress, DateTime TimeVote)
        {
            try
            {
                if (DateTime.Now >= TimeVote.AddHours(12))
                {
                    MySQL.MySqlCommand Payment = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                    Payment.Delete("voteips", "ips", IPAddress).Execute();
                    if (World.VotedIps.Contains(IPAddress))
                        World.VotedIps.Remove(IPAddress);
                }
            }
            catch (Exception exc)
            {
                World.ExcAdd += exc + "\r\n";
            }

        }
        public void AvailabilityToVote(Character C)
        {
            try
            {
                if (C.MyClient != null)
                {
                    if (C.MyClient.Soc.Connected)
                    {
                        if (C != null)
                        {
                            string IP = C.MyClient.Soc.RemoteEndPoint.ToString().Split(':')[0].ToString();

                            if (World.VotedIps.Contains(IP))
                                return;
                            C.MyClient.LocalMessage(2000, "You're now able to vote again! Make sure you make the best out of your voting points!");
                            C.MyClient.DialogNPC = 13653;
                            Ultimate.NPCs.NPCHandler.Handle(C.MyClient, null, 13653, 0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                World.ExcAdd += e + "\r\n";
            }
        }
    }
}