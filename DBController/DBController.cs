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
        // issues: @tableHeader do not contain columns' data type. will use json
        public bool InsertRegexMatch(string tableName, List<string> tableHeaderList, Match match)
        {
            int itemCount = match.Groups.Count - 1;

            var queryItemPattern = $"({string.Join(",", tableHeaderList)})";
            var queryValuePattern = $"(@{string.Join(",@", tableHeaderList)})";

            var query = $"INSERT into {tableName} {queryItemPattern} VALUES {queryValuePattern}";
            var cmd = this.getSQLCommand(query);

            var delegateDic = new Dictionary<string, Func<string, string, SqlParameter>>
            {
                { "INT", (string a, string b) => { return cmd.Parameters.AddWithValue("@"+a, int.Parse(b)); } },
                { "STRING", (string a, string b) => { return cmd.Parameters.AddWithValue("@"+a, b); }},
                { "DATETIME", (string a, string b) => { return cmd.Parameters.AddWithValue("@"+a, DateTime.Parse(b)); }},
            };

            cmd.Parameters.AddWithValue("@1", "yeze");
            cmd.Parameters.AddWithValue("@2", "yezepc");
            cmd.Parameters.AddWithValue("@3", DateTime.Now);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                throw;
                //caused by dump keys
                return false;
            }
        }
    }
}