using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DBController
{
    using SQLAddParaFunc = Func<SqlCommand, string, string, SqlParameter>;
    public class DBController
    {
        private SqlConnection _connection = null;
        private readonly DBInfo dbinfo = null;
        public DBController(string connectToken, DBInfo dbinfo)
        {
            this.dbinfo = dbinfo;
            this._connection = new SqlConnection(connectToken);
            try
            {
                this._connection.Open();
            }
            catch
            {
                throw;
            }
        }
        private SqlCommand getSQLCommand(string request)
        {
            return new SqlCommand
            {
                CommandText = request,
                CommandType = System.Data.CommandType.Text,
                Connection = this._connection
            };
        }

        public List<List<string>> ExecuteQuery(string request)
        {
            var cmd = this.getSQLCommand(request);
            var reader = cmd.ExecuteReader();
            var COL_NUM = reader.FieldCount;

            var ret = new List<List<string>>();
            while (reader.Read())
            {
                var row = new List<string>();

                for (int i = 0; i < COL_NUM; i++)
                {
                    row.Add(reader[i].ToString());
                }
                ret.Add(row);
            }
            return ret;
        }

        private static Dictionary<string, SQLAddParaFunc> dbAddParaLambdaDic = new Dictionary<string, SQLAddParaFunc>
        {
            { "INT", (cmd, col, val) => {
                return cmd.Parameters.AddWithValue("@"+col, val == "" ? -1 : int.Parse(val));
            } },
            { "STRING", (cmd, col, val) => { return cmd.Parameters.AddWithValue("@"+col, val); }},
            { "DATETIME", (cmd, col, val) => { return cmd.Parameters.AddWithValue("@"+col, DateTime.Parse(val)); }},
        };

        // issues: @tableHeader do not contain columns' data type. will use json
        public bool InsertRegexMatch(string tableName, Match match)
        {
            var queryItemPattern = $"({string.Join(",", this.dbinfo.columnNameList)})".Replace(" ", "");
            var queryValuePattern = $"(@{string.Join(",@", this.dbinfo.columnNameList)})".Replace(" ", "");

            var queryText = $"INSERT into {tableName} {queryItemPattern} VALUES {queryValuePattern}";
            var insertCommand = this.getSQLCommand(queryText);

            //usage:
            //delegateDic["INT"](tableHeaderList[0], match.Groups[0].Value);
            for (int i = 0; i < this.dbinfo.columnNameList.Count; i++)
            {
                var dataType = this.dbinfo.dataTypeList[i];
                var columnName = this.dbinfo.columnNameList[i];
                dbAddParaLambdaDic[this.dbinfo.dataTypeList[i]](
                    insertCommand,
                    columnName,
                    match.Groups[i+1].Value
                    );
            }

            try
            {
                insertCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                Console.WriteLine($"Ducplicate key at : {match.Groups[1].Value}");
                //caused by dump keys
                return false;
            }
        }
    }
}