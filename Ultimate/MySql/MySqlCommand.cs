using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Ultimate.MySQL
{
    public class MySqlCommand
    {
        private MySqlCommandType _type;
        public MySqlCommandType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        protected string _command;
        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }
        private bool firstPart = true;
        public Dictionary<byte, string> Fields = new Dictionary<byte, string>();
        public Dictionary<byte, long> longValues = new Dictionary<byte, long>();
        public Dictionary<byte, ulong> ulongValues = new Dictionary<byte, ulong>();
        public Dictionary<byte, bool> boolValues = new Dictionary<byte, bool>();
        public Dictionary<byte, string> stringValues = new Dictionary<byte, string>();
        public Dictionary<byte, long> greaterValues = new Dictionary<byte, long>();
        public Dictionary<byte, long> smallerValues = new Dictionary<byte, long>();
        public Dictionary<byte, DateTime> dateTimeValues = new Dictionary<byte, DateTime>();
        public byte lastpair = 0;
        public byte toUpdate = 0;
        public MySqlCommand(MySqlCommandType Type)
        {
            this.Type = Type;
            switch (Type)
            {
                case MySqlCommandType.SELECT:
                    _command = "SELECT * FROM <R>";
                    break;
                case MySqlCommandType.UPDATE:
                    _command = "UPDATE <R> SET <F>";
                    break;
                case MySqlCommandType.INSERT:
                    _command = "INSERT INTO <R> (<F>) VALUES (<V>)";
                    break;
                case MySqlCommandType.DELETE:
                    _command = "DELETE FROM <R> WHERE <C> = <V>";
                    break;
                case MySqlCommandType.COUNT:
                    _command = "SELECT count(<V>) FROM <R>";
                    break;
                case MySqlCommandType.ONDUPLICATEKEY:
                    _command = "INSERT INTO <R> (<F>) VALUES (<V>) ON DUPLICATE KEY UPDATE <U>";
                    break;
                case MySqlCommandType.COPY:
                    _command = "INSERT INTO <R> SELECT * FROM <I>";
                    break;
                case MySqlCommandType.TRUNCATE:
                    _command = "TRUNCATE TABLE <R>";
                    break;
            }
        }

        #region Select
        public MySqlCommand Select(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        #endregion

        #region Count
        public MySqlCommand Count(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        #endregion

        public MySqlCommand Truncate(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }

        #region Delete
        public MySqlCommand Delete(string table, string column, string value)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            _command = _command.Replace("<C>", "`" + column + "`");
            _command = _command.Replace("<V>", "'" + value + "'");
            return this;
        }
        public MySqlCommand Delete(string table, string column, long value)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            _command = _command.Replace("<C>", "`" + column + "`");
            _command = _command.Replace("<V>", value.ToString());
            return this;
        }
        public MySqlCommand Delete(string table, string column, ulong value)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            _command = _command.Replace("<C>", "`" + column + "`");
            _command = _command.Replace("<V>", value.ToString());
            return this;
        }
        public MySqlCommand Delete(string table, string column, bool value)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            _command = _command.Replace("<C>", "`" + column + "`");
            _command = _command.Replace("<V>", (value ? "1" : "0"));
            return this;
        }
        public MySqlCommand COUNT(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        #endregion

        private bool Comma()
        {
            if (firstPart)
            {
                firstPart = false;
                return false;
            }
            if (_command[_command.Length - 1] == ',' || _command[_command.Length - 2] == ',' || _command[_command.Length - 3] == ',')
                return false;
            return true;
        }

        public MySqlCommand Copy(string oldTable, string newTable)
        {
            _command = _command.Replace("<R>", "`" + newTable + "`");
            _command = _command.Replace("<I>", "`" + oldTable + "`");
            return this;
        }

        #region Update
        public MySqlCommand Update(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        public MySqlCommand Set(string column, long value)
        {
            //if (Type == MySqlCommandType.UPDATE)
            //{
            //    if (Comma())
            //        _command = _command + ",`" + column + "` = " + value.ToString() + " ";
            //    else
            //        _command = _command + "`" + column + "` = " + value.ToString() + " ";
            //}
            Fields.Add(lastpair, column);
            longValues.Add(lastpair, value);
            lastpair++;
            toUpdate++;
            return this;
        }
        public MySqlCommand Set(string column, ulong value)
        {
            //if (Type == MySqlCommandType.UPDATE)
            //{
            //    bool comma = false;
            //    comma = (_command[_command.Length - 1] == ',' || _command[_command.Length - 2] == ',' || _command[_command.Length - 3] == ',') ? false : true;
            //    if (comma)
            //        _command = _command + ",`" + column + "` = " + value.ToString() + " ";
            //    else
            //        _command = _command + "`" + column + "` = " + value.ToString() + " ";
            //}
            Fields.Add(lastpair, column);
            ulongValues.Add(lastpair, value);
            lastpair++;
            toUpdate++;
            return this;
        }
        public MySqlCommand Set(string column, string value)
        {
            //if (Type == MySqlCommandType.UPDATE)
            //{
            //    if (Comma())
            //        _command = _command + ",`" + column + "` = '" + value + "' ";
            //    else
            //        _command = _command + "`" + column + "` = '" + value + "' ";
            //}
            //return this;
            Fields.Add(lastpair, column);
            stringValues.Add(lastpair, value);
            lastpair++;
            toUpdate++;
            return this;
        }
        public MySqlCommand Set(string column, bool value)
        {
            //if (Type == MySqlCommandType.UPDATE)
            //{
            //    if (Comma())
            //        _command = _command + ",`" + column + "` = " + (value ? "1" : "0") + " ";
            //    else
            //        _command = _command + "`" + column + "` = " + (value ? "1" : "0") + " ";
            //}
            //return this;
            Fields.Add(lastpair, column);
            boolValues.Add(lastpair, value);
            lastpair++;
            toUpdate++;
            return this;
        }
        public MySqlCommand Set(string field, DateTime value)
        {
            Fields.Add(lastpair, field);
            dateTimeValues.Add(lastpair, value);
            lastpair++;
            toUpdate++;
            return this;
        }
        #endregion

        #region Insert
        public MySqlCommand Insert(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        public MySqlCommand Insert(string field, long value)
        {
            Fields.Add(lastpair, field);
            longValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public MySqlCommand Insert(string field, ulong value)
        {
            Fields.Add(lastpair, field);
            ulongValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public MySqlCommand Insert(string field, bool value)
        {
            Fields.Add(lastpair, field);
            boolValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public MySqlCommand Insert(string field, string value)
        {
            Fields.Add(lastpair, field);
            stringValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public MySqlCommand Insert(string field, DateTime value)
        {
            Fields.Add(lastpair, field);
            dateTimeValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        #endregion

        #region Where
        public MySqlCommand Where(string column, long value)
        {
            Fields.Add(lastpair, column);
            longValues.Add(lastpair, value);
            lastpair++;
            //_command = _command + "WHERE `" + column + "` = " + value;
            return this;
        }
        public MySqlCommand Where(string column, long value, bool greater)
        {
            if (greater)
            {
                Fields.Add(lastpair, column);
                greaterValues.Add(lastpair, value);
                lastpair++;
            }
            //_command = _command + "WHERE `" + column + "` > " + value;
            else
            {
                Fields.Add(lastpair, column);
                smallerValues.Add(lastpair, value);
                lastpair++;
            }
            //_command = _command + "WHERE `" + column + "` < " + value;
            return this;
        }
        public MySqlCommand Where(string column, ulong value)
        {
            Fields.Add(lastpair, column);
            ulongValues.Add(lastpair, value);
            lastpair++;
            //_command = _command + "WHERE `" + column + "` = " + value;
            return this;
        }
        public MySqlCommand Where(string column, string value)
        {
            Fields.Add(lastpair, column);
            stringValues.Add(lastpair, value);
            lastpair++;
            //_command = _command + "WHERE `" + column + "` = '" + value + "'";
            return this;
        }
        public MySqlCommand Where(string column, bool value)
        {
            Fields.Add(lastpair, column);
            boolValues.Add(lastpair, value);
            lastpair++;
            //_command = _command + "WHERE `" + column + "` = " + (value ? "1" : "0");
            return this;
        }
        #endregion

        #region And
        public MySqlCommand And(string column, long value)
        {
            _command = _command + " AND `" + column + "` = " + value;
            return this;
        }
        public MySqlCommand And(string column, ulong value)
        {
            _command = _command + " AND `" + column + "` = " + value;
            return this;
        }
        public MySqlCommand And(string column, string value)
        {
            _command = _command + " AND `" + column + "` = '" + value + "'";
            return this;
        }
        public MySqlCommand And(string column, bool value)
        {
            _command = _command + " AND `" + column + "` = " + (value ? "1" : "0");
            return this;
        }
        #endregion

        #region Order
        public MySqlCommand Order(string column)
        {
            _command = _command + "ORDER BY " + column + "";
            return this;
        }
        #endregion

        public void Execute()
        {
            if (Type == MySqlCommandType.INSERT || Type == MySqlCommandType.ONDUPLICATEKEY)
            {
                string fields = "";
                string values = "";
                string duplicate = "";
                byte x = 0;
                for (x = 0; x < lastpair; x++)
                {
                    bool comma = (x + 1) == lastpair ? false : true;
                    #region Fields
                    if (comma)
                    {
                        fields += Fields[x] + ",";
                        values += $"?{Fields[x]},";
                        if (Type == MySqlCommandType.ONDUPLICATEKEY)
                            duplicate += Fields[x] + "=" + $"?{Fields[x]},";
                    }
                    else
                    {
                        fields += Fields[x];
                        values += $"?{Fields[x]}";
                        if (Type == MySqlCommandType.ONDUPLICATEKEY)
                            duplicate += Fields[x] + "=" + $"?{Fields[x]}";
                    }
                    #endregion
                }
                _command = _command.Replace("<F>", fields);
                _command = _command.Replace("<V>", values);
                if (Type == MySqlCommandType.ONDUPLICATEKEY)
                    _command = _command.Replace("<U>", duplicate);
            }
            else if (Type == MySqlCommandType.UPDATE || Type == MySqlCommandType.COPY)
            {
                string fields = "";
                byte x = 0;
                for (x = 0; x < toUpdate; x++)
                {
                    bool comma = (x + 1) == toUpdate ? false : true;

                    if (comma)
                        fields += $"{Fields[x]}=?{Fields[x]},";
                    else
                        fields += $"{Fields[x]}=?{Fields[x]}";
                }
                if (lastpair > toUpdate)
                    _command += " WHERE ";
                for (; x < lastpair; x++)
                {
                    bool first = (x + 1) == (toUpdate + 1) ? true : false;

                    if (first)
                        _command += $"{Fields[x]}=?{Fields[x]}";
                    else
                        _command += $" AND {Fields[x]}=?{Fields[x]}";
                }
                _command = _command.Replace("<F>", fields);
                //_command += $"WHERE {fields}={values}";
            }
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString))
            {
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(Command, conn);

                if (Type == MySqlCommandType.INSERT || Type == MySqlCommandType.ONDUPLICATEKEY || Type == MySqlCommandType.UPDATE || Type == MySqlCommandType.COPY)
                {
                    for (byte x = 0; x < lastpair; x++)
                    {
                        #region Values
                        if (longValues.ContainsKey(x))
                            cmd.Parameters.AddWithValue($"?{Fields[x]}", longValues[x].ToString());
                        else if (ulongValues.ContainsKey(x))
                            cmd.Parameters.AddWithValue($"?{Fields[x]}", ulongValues[x].ToString());
                        else if (boolValues.ContainsKey(x))
                            cmd.Parameters.AddWithValue($"?{Fields[x]}", (boolValues[x] ? "1" : "0"));
                        else if (stringValues.ContainsKey(x))
                            cmd.Parameters.AddWithValue($"?{Fields[x]}", stringValues[x]);
                        else if (dateTimeValues.ContainsKey(x))
                            cmd.Parameters.AddWithValue($"?{Fields[x]}", dateTimeValues[x]);
                        #endregion
                    }
                }
                cmd.ExecuteNonQuery();
            }
        }
        public uint ExecuteScalar()
        {
            if (lastpair > 0)
            {
                if (greaterValues.Count > 0)
                    Command = Command + " WHERE " + Fields[0] + ">" + $"?{Fields[0]}";
                else if (smallerValues.Count > 0)
                    Command = Command + " WHERE " + Fields[0] + ">" + $"?{Fields[0]}";
                else
                    Command = Command + " WHERE " + Fields[0] + "=" + $"?{Fields[0]}";
            }
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Ultimate"].ConnectionString))
            {
                conn.Open();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(Command, conn);
                if (lastpair > 0)
                {
                    if (longValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", longValues[0].ToString());
                    else if (ulongValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", ulongValues[0].ToString());
                    else if (boolValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", (boolValues[0] ? "1" : "0"));
                    else if (stringValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", stringValues[0]);
                    else if (greaterValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", greaterValues[0]);
                    else if (smallerValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", smallerValues[0]);
                    else if (dateTimeValues.ContainsKey(0))
                        cmd.Parameters.AddWithValue($"?{Fields[0]}", dateTimeValues[0]);
                }

                return Convert.ToUInt32(cmd.ExecuteScalar());
            }
        }
        public void UpdateInsert()
        {
            if (Type == MySqlCommandType.INSERT)
            {
                string fields = "";
                string values = "";
                byte x;
                for (x = 0; x < lastpair; x++)
                {
                    bool comma = (x + 1) == lastpair ? false : true;
                    #region Fields
                    if (comma)
                        fields += Fields[x] + ",";
                    else
                        fields += Fields[x];
                    #endregion
                    #region Values
                    if (longValues.ContainsKey(x))
                    {
                        if (comma)
                            values += longValues[x].ToString() + ",";
                        else
                            values += longValues[x].ToString();
                    }
                    else if (ulongValues.ContainsKey(x))
                    {
                        if (comma)
                            values += ulongValues[x].ToString()[x] + ",";
                        else
                            values += ulongValues[x].ToString();
                    }
                    else if (boolValues.ContainsKey(x))
                    {
                        if (comma)
                            values += (boolValues[x] ? "1" : "0") + ",";
                        else
                            values += (boolValues[x] ? "1" : "0");
                    }
                    else if (stringValues.ContainsKey(x))
                    {
                        if (comma)
                            values += "'" + stringValues[x] + "'" + ",";
                        else
                            values += "'" + stringValues[x] + "'";
                    }
                    else if (dateTimeValues.ContainsKey(x))
                    {
                        if (comma)
                            values += "'" + dateTimeValues[x] + "'" + ",";
                        else
                            values += "'" + dateTimeValues[x] + "'";
                    }
                    #endregion
                }
                _command = _command.Replace("<F>", fields);
                _command = _command.Replace("<V>", values);
            }
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
    }
    public enum MySqlCommandType
    {
        DELETE, INSERT, SELECT, UPDATE, COUNT, ONDUPLICATEKEY, COPY, TRUNCATE
    }
}
