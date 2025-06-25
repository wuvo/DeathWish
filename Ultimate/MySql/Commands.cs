using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewestCOServer.MySql
{
    public class Commands
    {
        public enum MySqlCommandType
        {
            DELETE, INSERT, SELECT, UPDATE, COUNT
        }
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
        private Dictionary<byte, string> Fields;
        private Dictionary<byte, long> longValues;
        private Dictionary<byte, ulong> ulongValues;
        private Dictionary<byte, bool> boolValues;
        private Dictionary<byte, string> stringValues;
        private byte lastpair;

        public Commands(MySqlCommandType Type)
        {
            this.Type = Type;
            switch (Type)
            {
                case MySqlCommandType.SELECT:
                    {
                        _command = "SELECT * FROM <R>";
                        break;
                    }
                case MySqlCommandType.UPDATE:
                    {
                        _command = "UPDATE <R> SET ";
                        break;
                    }
                case MySqlCommandType.INSERT:
                    {
                        Fields = new Dictionary<byte, string>();
                        longValues = new Dictionary<byte, long>();
                        ulongValues = new Dictionary<byte, ulong>();
                        boolValues = new Dictionary<byte, bool>();
                        stringValues = new Dictionary<byte, string>();
                        lastpair = 0;
                        _command = "INSERT INTO <R> (<F>) VALUES (<V>)";
                        break;
                    }
                case MySqlCommandType.DELETE:
                    {
                        _command = "DELETE FROM <R> WHERE <C> = <V>";
                        break;
                    }
                case MySqlCommandType.COUNT:
                    {
                        _command = "SELECT count(<V>) FROM <R>";
                        break;
                    }
            }
        }

        #region Insert
        public Commands Insert(string table)
        {
            _command = _command.Replace("<R>", "`" + table + "`");
            return this;
        }
        public Commands Insert(string field, long value)
        {
            Fields.Add(lastpair, field);
            longValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public Commands Insert(string field, ulong value)
        {
            Fields.Add(lastpair, field);
            ulongValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public Commands Insert(string field, bool value)
        {
            Fields.Add(lastpair, field);
            boolValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        public Commands Insert(string field, string value)
        {
            Fields.Add(lastpair, field);
            stringValues.Add(lastpair, value);
            lastpair++;
            return this;
        }
        #endregion

        public void Execute()
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
                    values += $"@{Fields[x]}";
                    #endregion
                }
                _command = _command.Replace("<F>", fields);
                _command = _command.Replace("<V>", values);
            }
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Shannara"].ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(Command, conn);

                if (Type == MySqlCommandType.INSERT)
                {
                    for (byte x = 0; x < lastpair; x++)
                    {
                        #region Values
                        if (longValues.ContainsKey(x))
                            cmd.Parameters.Add($"@{Fields[x]}", longValues[x].ToString());
                        else if (ulongValues.ContainsKey(x))
                            cmd.Parameters.Add($"@{Fields[x]}", ulongValues[x].ToString());
                        else if (boolValues.ContainsKey(x))
                            cmd.Parameters.Add($"@{Fields[x]}", (boolValues[x] ? "1" : "0"));
                        else if (stringValues.ContainsKey(x))
                            cmd.Parameters.Add($"@{Fields[x]}", "'" + stringValues[x] + "'");
                        #endregion
                    }
                }
                cmd.ExecuteNonQuery();
            }
        }
    }
}
