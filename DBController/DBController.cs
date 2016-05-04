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
            this._connection.Open();
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
        public bool InsertRegexMatch(string tablename, Match match, List<string> valueList)
        {
            //int LEN = match.Groups.Count;
            var q = "INSERT into HumanResources.Department (Name, GroupName, ModifiedDate) VALUES (@b, @c, @d)";
            var cmd = this.getSQLCommand(q);
            cmd.Parameters.AddWithValue("@b", "yeze");
            cmd.Parameters.AddWithValue("@c", "yezepc");
            cmd.Parameters.AddWithValue("@d", DateTime.Now);
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