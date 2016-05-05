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
        public List<List<string>> ExecuteQuery(string request)
        {
            var cmd = new SqlCommand(request, this._connection);
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

        private void setInsertCommandValue(SqlCommand cmd, Match match)
        {

            for (int i = 0; i < this.dbinfo.columnNameList.Count; i++)
            {
                var dataType = this.dbinfo.dataTypeList[i];
                var columnName = this.dbinfo.columnNameList[i];

                dbAddParaLambdaDic[dataType](cmd, columnName, match.Groups[i + 1].Value);
            }
        }
        // issues: @tableHeader do not contain columns' data type. will use json
        public int BatchInsertRegexCollections(string tableName, MatchCollection collections, int batchSize = 1000)
        {
            int successCount = 0;
            var queryText = $"INSERT into {tableName} {this.dbinfo.queryItemPattern} VALUES {this.dbinfo.queryValuePattern}";
            using (var transaction = this._connection.BeginTransaction())
            {
                foreach (Match match in collections)
                {
                    var cmd = new SqlCommand(queryText, this._connection, transaction);
                    this.setInsertCommandValue(cmd, match);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"Insert: {match.Groups[1].Value}");
                        successCount += 1;
                    }
                    catch
                    {
                        Console.WriteLine($"Duplicate Incident: {match.Groups[1].Value}");
                    }
                }
                transaction.Commit();
            }
            return successCount;
        }
        public void ClearTable(string tableName, string pkname)
        {
            var queryText = $"DELETE FROM {tableName} WHERE {pkname} IN (SELECT {pkname} FROM {tableName})";
            var cmd = new SqlCommand(queryText, this._connection);
            cmd.ExecuteNonQuery();
        }
    }
}