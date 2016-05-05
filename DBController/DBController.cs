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
        private readonly string testToken =
            @"Server=YEZE-PC;Database=AdventureWorks2014;
            User ID=yezetest;Password=950322;";
        private SqlConnection _connection = null;
        public DBController(string token)
        {
            this._connection = new SqlConnection(token);
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

        private static Dictionary<string, SQLAddParaFunc> delegateDic = new Dictionary<string, SQLAddParaFunc>
        {
            { "INT", (cmd, col, val) => {
                return cmd.Parameters.AddWithValue("@"+col, val == "" ? -1 : int.Parse(val));
            } },
            { "STRING", (cmd, col, val) => { return cmd.Parameters.AddWithValue("@"+col, val); }},
            { "DATETIME", (cmd, col, val) => { return cmd.Parameters.AddWithValue("@"+col, DateTime.Parse(val)); }},
        };

        // issues: @tableHeader do not contain columns' data type. will use json
        public bool InsertRegexMatch(string tableName, List<string> tableHeaderList, List<string> dataTypeList, Match match)
        {
            var queryItemPattern = $"({string.Join(",", tableHeaderList)})";
            var queryValuePattern = $"(@{string.Join(",@", tableHeaderList)})";

            var query = $"INSERT into {tableName} {queryItemPattern} VALUES {queryValuePattern}";
            var cmd = this.getSQLCommand(query);

            //usage:
            //delegateDic["INT"](tableHeaderList[0], match.Groups[0].Value);

            for (int i = 0; i < tableHeaderList.Count; i++)
            {
                delegateDic[dataTypeList[i]](cmd, tableHeaderList[i], match.Groups[i + 1].Value);
            }

            try
            {
                cmd.ExecuteNonQuery();
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