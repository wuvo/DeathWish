using System;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Configuration;

namespace Ultimate.MySQL
{
    public class MySqlReader
    {
        private DataSet _dataset;
        private DataRow _datarow;
        private int _row;
        const string Table = "table";

        public MySqlReader(MySqlCommand command)
        {
            if (command.Type == MySqlCommandType.SELECT)
            {
                TryFill(command);
            }
        }
        private MySqlConnection SelectConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString);
        }
        private string _lasterror = null;
        public string LastError
        {
            get
            {
                return _lasterror;
            }
            set
            {
                _lasterror = value;
            }
        }
        private void TryFill(MySqlCommand command)
        {
            //MySql.Data.MySqlClient.MySqlConnection connection = SelectConnection();
            //MySqlDataAdapter DataAdapter = null;
            //if (connection.State == ConnectionState.Open)
            //{
            //    while (_dataset == null && (_lasterror == null || _lasterror.Contains("connection")))
            //    {
            //        if (_lasterror != null && _lasterror.Contains("connection"))
            //            connection = SelectConnection();
            //        DataAdapter = new MySqlDataAdapter(command.Command, connection);
            //        _dataset = new DataSet();
            //        try
            //        {
            //            DataAdapter.Fill(_dataset, Table);
            //        }
            //        catch (MySql.Data.MySqlClient.MySqlException e)
            //        {
            //            _lasterror = e.ToString().ToLower();
            //            _dataset = null;
            //            continue;
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.ToString());
            //            break;
            //        }
            //        _row = 0;
            //    }
            //}
            if (command.lastpair > 0)
            {
                if (command.greaterValues.Count > 0)
                    command.Command = command.Command + " WHERE " + command.Fields[0] + ">" + $"?{command.Fields[0]}";
                else if (command.smallerValues.Count > 0)
                    command.Command = command.Command + " WHERE " + command.Fields[0] + ">" + $"?{command.Fields[0]}";
                else
                    command.Command = command.Command + " WHERE " + command.Fields[0] + "=" + $"?{command.Fields[0]}";
            }

            using (MySql.Data.MySqlClient.MySqlConnection conn = SelectConnection())
            {
                conn.Open();
                using (MySqlDataAdapter DataAdapter = new MySqlDataAdapter(command.Command, conn))
                {
                    if (command.lastpair > 0)
                    {
                        if (command.longValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.longValues[0].ToString());
                        else if (command.ulongValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.ulongValues[0].ToString());
                        else if (command.boolValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", (command.boolValues[0] ? "1" : "0"));
                        else if (command.stringValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.stringValues[0]);
                        else if (command.greaterValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.greaterValues[0]);
                        else if (command.smallerValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.smallerValues[0]);
                        else if (command.dateTimeValues.ContainsKey(0))
                            DataAdapter.SelectCommand.Parameters.AddWithValue($"?{command.Fields[0]}", command.dateTimeValues[0]);
                    }
                    _dataset = new DataSet();
                    try
                    {
                        DataAdapter.Fill(_dataset, Table);
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        _lasterror = e.ToString().ToLower();
                        _dataset = null;
                        Console.WriteLine(e.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    _row = 0;
                }
            }
        }
        public bool Read()
        {
            if (_dataset.Tables[Table].Rows.Count > _row)
            {
                _datarow = _dataset.Tables[Table].Rows[_row];
                _row++;
                return true;
            }
            _row++;
            return false;
        }

        public sbyte ReadSByte(string columnName)
        {
            sbyte result = 0;
            try
            {
                sbyte.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public byte ReadByte(string columnName)
        {
            byte result = 0;
            try
            {
                byte.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public short ReadInt16(string columnName)
        {
            short result = 0;
            try
            {
                short.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public ushort ReadUInt16(string columnName)
        {
            ushort result = 0;
            try
            {
                ushort.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public int ReadInt32(string columnName)
        {
            int result = 0;
            try
            {
                string a = _datarow[columnName].ToString();
                int.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public uint ReadUInt32(string columnName)
        {
            uint result = 0;
            try
            {
                uint.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public long ReadInt64(string columnName)
        {
            long result = 0;
            try
            {
                long.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public ulong ReadUInt64(string columnName)
        {
            ulong result = 0;
            try
            {
                ulong.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
        public string ReadString(string columnName)
        {
            string result = "";
            try
            {
                result = _datarow[columnName].ToString();
            }
            catch { }
            return result;
        }
        public bool ReadBoolean(string columnName)
        {
            bool result = false;
            try
            {
                //string something = _datarow[columnName].ToString();
                //bool.TryParse(something, out result);
                //Convert.ToBoolean()
                bool.TryParse(_datarow[columnName].ToString().Replace("0", "false").Replace("1", "true"), out result);
            }
            catch
            {
                byte value = 0;
                try
                {
                    byte.TryParse(_datarow[columnName].ToString(), out value);
                }
                catch { }
                result = value == 0 ? false : true;
            }
            return result;
        }
        public DateTime ReadDatetime(string columnName)
        {
            DateTime result = new DateTime();
            try
            {
                DateTime.TryParse(_datarow[columnName].ToString(), out result);
            }
            catch { }
            return result;
        }
    }
}
