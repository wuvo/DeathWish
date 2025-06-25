using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using Ultimate.Game;
using Ultimate;
using System.Configuration;

namespace Ultimate
{
    class AutoPayments_System
    {
        //MySqlCommand Cmd_MySQL;
        //TableDATA Table_CharData;
        //MySqlDataReader DataRead_MySQL;
        //string nr = "Select COUNT(*) FROM payments";
        //string CharactersPayments = "Select ID from payments";
        //readonly MySqlConnection Connect_MySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);
        //struct TableDATA
        //{
        //    public string CharacterName, Amount, Email, PayDate;
        //    public int IDChar;
        //    public byte VipDays;
        //    public ushort NrDBS;
        //}
        public AutoPayments_System()
        {
            PaymentsProcess();
        }
        public void PaymentsProcess()
        {
            try
            {
                MySQL.MySqlCommand Cmd = new MySQL.MySqlCommand(MySQL.MySqlCommandType.SELECT).Select("payments");
                MySQL.MySqlReader Payments = new MySQL.MySqlReader(Cmd);

                //2.Check for DBs or VIP
                while (Payments.Read())
                {
                    if (Payments.ReadUInt16("DBScrolls") == 1111)
                    {
                        MySQL.MySqlCommand Payment1 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                        Payment1.Delete("payments", "DBScrolls", 1111).Execute();
                    }
                    else if (Payments.ReadUInt16("DBScrolls") == 7777)
                    {
                        MySQL.MySqlCommand Payment1 = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
                        Payment1.Delete("payments", "DBScrolls", 7777).Execute();
                    }


                    else if (Payments.ReadUInt16("DBScrolls") == 2001)
                        AddGarment1(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadUInt16("DBScrolls") == 2002)
                        AddGarment2(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadUInt16("DBScrolls") == 2003)
                        AddGarment3(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadUInt16("DBScrolls") > 3 && Payments.ReadUInt16("DBScrolls") <= 50)
                        AddDBS(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadByte("VIPDays") > 3)
                        AddVIP(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("VIPDays"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadUInt16("DBScrolls") == 2222)
                        AddMine(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("VIPDays"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                    else if (Payments.ReadUInt16("DBScrolls") == 3333)
                        AddMinePacket(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("VIPDays"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));

                    #region Accessory
                    if (Payments.ReadUInt16("DBScrolls") >= 2010 && Payments.ReadUInt16("DBScrolls") <= 2029)
                    {
                        if (Payments.ReadUInt16("DBScrolls") == 2010)
                            Accessory0(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2011)
                            Accessory1(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2012)
                            Accessory2(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2013)
                            Accessory3(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2014)
                            Accessory4(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2015)
                            Accessory5(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2016)
                            Accessory6(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2017)
                            Accessory7(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2018)
                            Accessory8(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2019)
                            Accessory9(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2020)
                            Accessory10(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2021)
                            Accessory11(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2022)
                            Accessory12(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2023)
                            Accessory13(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2024)
                            Accessory14(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2025)
                            Accessory15(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2026)
                            Accessory16(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2027)
                            Accessory17(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2028)
                            Accessory18(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        else if (Payments.ReadUInt16("DBScrolls") == 2029)
                            Accessory19(Payments.ReadInt32("ID"), Payments.ReadString("CharacterName"), Payments.ReadUInt16("DBScrolls"), Payments.ReadString("PayDate"), Payments.ReadString("Email"), Payments.ReadString("Amount"));
                        #endregion
                    }
                }
            }
            //string[] Payer_Info = new string[9];
            //int NrLineChar = 0;
            //try
            //{
            //    //1.Database connection opening
            //    if (Connect_MySQL.State == System.Data.ConnectionState.Closed)
            //    {
            //        Connect_MySQL.Open();
            //    }

            //    //2.Nr lines of chars that made a payment
            //    Cmd_MySQL = new MySqlCommand(nr, Connect_MySQL);
            //    Int64 nrLines = Convert.ToInt32(Cmd_MySQL.ExecuteScalar().ToString());
            //    var nrChars = new int[nrLines];


            //    //3.Check if no payments
            //    if (nrLines == 0)
            //    {
            //        Cmd_MySQL.Dispose();
            //        return;
            //    }
            //    else //4.else payments...
            //    {
            //        //1. Add char's ids from mysql to and int array
            //        Cmd_MySQL.Dispose();
            //        Cmd_MySQL = new MySqlCommand(CharactersPayments, Connect_MySQL);
            //        DataRead_MySQL = Cmd_MySQL.ExecuteReader();
            //        while (DataRead_MySQL.Read())
            //        {
            //            nrChars[NrLineChar] = Convert.ToInt32(DataRead_MySQL.GetString(0));
            //            NrLineChar++;
            //        }
            //        DataRead_MySQL.Close();

            //        //2.Loop through lines to add vip or dbscrolls
            //        for (int i = 0; i < nrLines; i++)
            //        {
            //            //1.We take all char's info from table to info array

            //            Table_CharData.IDChar = nrChars[i]; //IDChar = char's id from array
            //            string InfoCharTable = "SELECT CharacterName,Currency,Amount,Email,VIPDays,DBScrolls,PayDate FROM payments Where ID=" + Table_CharData.IDChar;
            //            Cmd_MySQL = new MySqlCommand(InfoCharTable, Connect_MySQL);
            //            DataRead_MySQL = Cmd_MySQL.ExecuteReader();

            //            while (DataRead_MySQL.Read())
            //            {
            //                Payer_Info[0] = DataRead_MySQL.GetString(0);
            //                Payer_Info[1] = DataRead_MySQL.GetString(1);
            //                Payer_Info[2] = DataRead_MySQL.GetString(2);
            //                Payer_Info[3] = DataRead_MySQL.GetString(3);
            //                Payer_Info[4] = DataRead_MySQL.GetString(4);
            //                Payer_Info[5] = DataRead_MySQL.GetString(5);
            //                Payer_Info[6] = DataRead_MySQL.GetString(6);
            //            }

            //            DataRead_MySQL.Close();

            //            //Add info to structure
            //            Table_CharData.CharacterName = Payer_Info[0];
            //            Table_CharData.Amount = Payer_Info[2];
            //            Table_CharData.Email = Payer_Info[3];
            //            Table_CharData.VipDays = Convert.ToByte(Payer_Info[4]);
            //            Table_CharData.NrDBS = Convert.ToUInt16(Payer_Info[5]);
            //            Table_CharData.PayDate = Payer_Info[6];


            //            //2.Check for DBs or VIP

            //            if (Table_CharData.NrDBS > 0)
            //            {
            //                AddDBS(Table_CharData.IDChar, Table_CharData.CharacterName, Table_CharData.NrDBS, Table_CharData.PayDate, Table_CharData.Email, Table_CharData.Amount);
            //            }
            //            else if (Table_CharData.VipDays > 0)
            //            {
            //                AddVIP(Table_CharData.IDChar, Table_CharData.CharacterName, Table_CharData.VipDays, Table_CharData.PayDate, Table_CharData.Email, Table_CharData.Amount);
            //            }
            //        }
            //        Cmd_MySQL.Dispose();

            //    }

            //}
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
            //finally
            //{
            //    if (Connect_MySQL.State != System.Data.ConnectionState.Closed)
            //    {
            //        Connect_MySQL.Close();
            //    }
            //}
        }
        #region Server Add DBs/VIP
        void AddGarment1(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2001;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received GarmentToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive GarmentToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2001;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive GarmentToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }

        void AddGarment2(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2002;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received GarmentToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive GarmentToken email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2002;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive GarmentToken  email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }

        void AddGarment3(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2003;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received GarmentToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive GarmentToken email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2003;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive GarmentToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }

        void AddDBS(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.DBScrolls += DBS;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received " + DBS + " DBScrolls. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive DBScrolls " + DBS + " email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.DBScrolls += DBS;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive DBScrolls " + DBS + " email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        void AddVIP(int ID, string CharacterName, int Days, string PayDate, string Email, string Amount)
        {
            byte VIPD = (byte)Days;
            byte VIPL = 5;
            Character C = World.CharacterFromName(CharacterName);

            if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            {
                try
                {
                    if (C == null)
                    {

                        string Account = "";
                        C = Database.LoadCharacter(CharacterName, ref Account);
                        if (C != null)
                        {
                            if (VIP_History(ID, CharacterName, Email, VIPD, PayDate, Amount))
                            {
                                C.VIPLevelToReceive = VIPL;
                                C.VIPDaysToReceive += VIPD;
                                Database.SaveCharacter(C, Account);
                            }
                            else World.DonationAdd += CharacterName + " did not receive VIP " + VIPL + " " + VIPD + " email " + Email + "(auto donation error)\r\n";
                        }
                        else Error_VIP(ID, CharacterName, Email, VIPD, PayDate, Amount);
                    }


                    else
                    {
                        if (VIP_History(ID, CharacterName, Email, VIPD, PayDate, Amount))
                        {
                            C.VIPLevelToReceive = VIPL;
                            C.VIPDaysToReceive += VIPD;
                            C.MyClient.AddSend(Packets.ChatMessage(0, "SYSTEM", C.Name, "Congratulations! Check PRIZE NPC in market to receive your VIP " + C.VIPLevelToReceive + " . Thank you for donating.", 2001, 0));
                        }
                        else World.DonationAdd += CharacterName + " did not receive VIP " + VIPL + " " + VIPD + " email " + Email + "(auto donation error)\r\n";
                    }
                }
                catch (Exception exc)
                {
                    OtherErrors_VIP(exc.ToString(), CharacterName, VIPD, Email, PayDate, Amount);
                }
            }
            else
            {
                Error_VIP(ID, CharacterName, Email, VIPD, PayDate, Amount);
            }
        }
        #endregion
        #region MinePacket
        void AddMine(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 5;
                        C.VIPLevelToReceive = 3;
                        C.VIPDaysToReceive += 30;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received MineVip" + DBS + "  You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive MineVip " + DBS + " email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 5;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive MineVip " + DBS + " email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion



        #region MinePacket
        void AddMinePacket(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 4;
                        C.VIPLevelToReceive = 3;
                        C.VIPDaysToReceive += 30;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received MineVipPacket" + DBS + "  You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive MineVipPacket " + DBS + " email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (DBS_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 4;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive MineVipPacket " + DBS + " email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion


        #region Accessory0
        void Accessory0(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2010;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2010;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion

        #region Accessory1
        void Accessory1(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2011;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2011;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory0
        void Accessory2(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2012;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2012;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory0
        void Accessory3(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2013;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2013;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory4
        void Accessory4(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2014;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2014;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory5
        void Accessory5(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2015;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2015;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory6
        void Accessory6(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2016;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2016;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory7
        void Accessory7(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2017;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2017;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory8
        void Accessory8(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2018;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2018;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory9
        void Accessory9(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2019;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2019;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory10
        void Accessory10(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2020;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2020;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory11
        void Accessory11(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2021;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2021;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory12
        void Accessory12(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2022;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2022;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory13
        void Accessory13(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2023;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2023;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory14
        void Accessory14(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2024;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2024;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory15
        void Accessory15(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2025;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2025;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory16
        void Accessory16(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2026;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2026;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory17
        void Accessory17(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2027;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2027;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory18
        void Accessory18(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2028;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2028;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region Accessory19
        void Accessory19(int ID, string CharacterName, ushort DBSc, string PayDate, string Email, string Amount)
        {
            ushort DBS = DBSc;
            string Name = CharacterName;

            Character C = World.CharacterFromName(Name);

            //if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
            //{
            try
            {
                if (C != null)
                {
                    if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                    {
                        C.GarmentToken += 2029;
                        C.MyClient.LocalMessage(2000, "Congratulations! You received AccessoryToken. You can claim them at Prize NPC in market at any time.");
                    }
                    else World.DonationAdd += CharacterName + " did not receive AccessoryToken  email " + Email + "(auto donation error)\r\n";
                }
                else if (File.Exists(World.GlobalCharactersPath2Slashes + CharacterName + ".chr"))
                {
                    string Account = "";
                    C = Database.LoadCharacter(Name, ref Account);
                    if (C != null)
                    {
                        if (Accessory_History(ID, CharacterName, Email, DBS, PayDate, Amount))
                        {
                            C.GarmentToken += 2029;
                            Database.SaveCharacter(C, Account);
                        }
                        else World.DonationAdd += CharacterName + " did not receive AccessoryToken email " + Email + "(auto donation error)\r\n";
                    }
                }
                else
                {
                    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
                }

            }
            catch (Exception exc)
            {
                OtherErrors_DBS(exc.ToString(), CharacterName, DBS, Email, PayDate, Amount);
            }
            //}
            //else
            //{
            //    Error_DBS(ID, CharacterName, Email, DBS, PayDate, Amount);
            //}
        }
        #endregion
        #region MySQL Commands
        bool VIP_History(int id, string CharacterName, string Email, int Days, string PayDate, string Amount)
        {
            MySQL.MySqlCommand VIP = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            VIP.Insert("vip_history").Insert("CharacterName", CharacterName).Insert("Email", Email).Insert("Amount", Amount).Insert("Days", Days).Insert("PayDate", PayDate).Execute();
            //string History = "Insert into vip_history VALUES('" + id + "','" + CharacterName + "','" + Email + "','" + Amount + "','" + Days + "','" + PayDate + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);

            //cmd.ExecuteNonQuery();
            DeletePayment(id);

            return true;
        }
        bool DBS_History(int id, string CharacterName, string Email, int DBSCrolls, string PayDate, string Amount)
        {
            MySQL.MySqlCommand DBs = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            DBs.Insert("dbs_history").Insert("CharacterName", CharacterName).Insert("Email", Email).Insert("Amount", Amount).Insert("DBSCrolls", DBSCrolls).Insert("PayDate", PayDate).Execute();
            //string History = "Insert into dbs_history VALUES('" + id + "','" + CharacterName + "','" + Email + "','" + Amount + "','" + DBSCrolls + "','" + PayDate + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
            DeletePayment(id);
            return true;
        }
        bool Accessory_History(int id, string CharacterName, string Email, int DBSCrolls, string PayDate, string Amount)
        {
            MySQL.MySqlCommand DBs = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            DBs.Insert("accessory_history").Insert("CharacterName", CharacterName).Insert("Email", Email).Insert("Amount", Amount).Insert("Itemid", DBSCrolls).Insert("PayDate", PayDate).Execute();
            //string History = "Insert into dbs_history VALUES('" + id + "','" + CharacterName + "','" + Email + "','" + Amount + "','" + DBSCrolls + "','" + PayDate + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
            DeletePayment(id);
            return true;
        }
        //bool VIP_History(int id, string CharacterName, string Email, int Days, string PayDate, string Amount)
        //{
        //    string History = "Insert into vip_history VALUES('" + id + "','" + CharacterName + "','" + Email + "','" + Amount + "','" + Days + "','" + PayDate + "')";
        //    MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);

        //    cmd.ExecuteNonQuery();
        //    DeletePayment(id);
        //    return true;
        //}
        //bool DBS_History(int id, string CharacterName, string Email, int DBSCrolls, string PayDate, string Amount)
        //{
        //    string History = "Insert into dbs_history VALUES('" + id + "','" + CharacterName + "','" + Email + "','" + Amount + "','" + DBSCrolls + "','" + PayDate + "')";
        //    MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
        //    cmd.ExecuteNonQuery();
        //    DeletePayment(id);
        //    return true;
        //}
        void Error_VIP(int id, string CharacterName, string Email, int Days, string PayDate, string Amount)
        {
            MySQL.MySqlCommand VIP = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            VIP.Insert("vip_errors").Insert("CharacterName", CharacterName).Insert("Error", "Character doesn't exist").Insert("Days", Days).Insert("Email", Email).Insert("Email", Email).Insert("PayDate", PayDate).Execute();

            //string History = "Insert into vip_errors VALUES('" + id + "','" + CharacterName + "','Character doesn't exist','" + Days + "','" + Email + "','" + PayDate + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
            DeletePayment(id);
        }
        void Error_DBS(int id, string CharacterName, string Email, ushort DBScrolls, string PayDate, string Amount)
        {
            MySQL.MySqlCommand DBs = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            DBs.Insert("dbs_errors").Insert("CharacterName", CharacterName).Insert("Error", "Character doesn't exist").Insert("PayDate", PayDate).Insert("Email", Email).Insert("DBSCrolls", DBScrolls).Execute();
            //string History = "Insert into dbs_errors VALUES('" + id + "','" + CharacterName + "','Character doesn't exist','" + PayDate + "','" + DateTime.Now + "','" + Email + "','" + DBScrolls + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
            DeletePayment(id);
        }
        void OtherErrors_VIP(string error, string CharacterName, int Days, string Email, string PayDate, string Amount)
        {
            MySQL.MySqlCommand DBsError = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            DBsError.Insert("payments_errors").Insert("Error", error).Insert("CharacterName", CharacterName).Insert("Email", Email).Insert("VIPDays", Days.ToString()).Insert("PayDate", PayDate).Insert("Amount", Amount).Execute();
            //string History = "Insert into payments_errors VALUES('" + "ID" + "','" + error + "','" + CharacterName + "','" + Days.ToString() + "','" + "NO" + "','" + PayDate + "','" + Amount + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
        }
        void OtherErrors_DBS(string error, string CharacterName, int DBScrolls, string Email, string PayDate, string Amount)
        {
            MySQL.MySqlCommand DBsError = new MySQL.MySqlCommand(MySQL.MySqlCommandType.INSERT);
            DBsError.Insert("payments_errors").Insert("Error", error).Insert("CharacterName", CharacterName).Insert("Email", Email).Insert("DBSCrolls", DBScrolls.ToString()).Insert("PayDate", PayDate).Insert("Amount", Amount).Execute();
            //string History = "Insert into payments_errors VALUES('" + "ID" + "','" + error + "','" + CharacterName + "','" + "NO" + "','" + DBScrolls.ToString() + "','" + PayDate + "','" + Amount + "')";
            //MySqlCommand cmd = new MySqlCommand(History, Connect_MySQL);
            //cmd.ExecuteNonQuery();
        }
        void DeletePayment(int id)
        {
            MySQL.MySqlCommand Payment = new MySQL.MySqlCommand(MySQL.MySqlCommandType.DELETE);
            Payment.Delete("payments", "ID", id).Execute();
            //string del = "Delete from payments where ID=" + id + "";
            //MySqlCommand cmd = new MySqlCommand(del, Connect_MySQL);
            //cmd.ExecuteNonQuery();
        }
        #endregion
    }
}
