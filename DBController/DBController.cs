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
        private SqlConnection _connection = null;
        public DBController(string connectToken)
        {
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

        public int BatchInsertRegexCollections(ConfigInitializer.IncidentTableEntity targetTable, MatchCollection collections, int batchSize = 1000)
        {
            var incident = new DataNormalizer.DataEntity.Incident(targetTable);
            int successCount = 0;
            var queryText = targetTable.ToString();
            //var queryText = $"INSERT into {this.tableInfo.tableName} {this.tableInfo.queryItemPattern} VALUES {this.tableInfo.queryValuePattern}";
            using (var transaction = this._connection.BeginTransaction())
            {
                foreach (Match match in collections)
                {
                    var cmd = new SqlCommand(queryText, this._connection, transaction);
                    incident.registerSqlCommand(match, ref cmd);
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