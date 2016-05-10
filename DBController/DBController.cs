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
            //Avoid Exception:
            //  There is already an open DataReader associated with this Command
            reader.Close();
            return ret;
        }

        private void _executeCMD(string queryText, SqlConnection connection, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(queryText, connection, transaction);
            cmd.Parameters.AddRange(;
            try
            {
                var ret = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
                Console.WriteLine($"Error happens while inserting dialog but was ignored");
            }
        }
        public void BatchInsertDialogCollections(ConfigInitializer.ChatlogTableEntity chatlogTable, MatchCollection chatlogCollections, int IncidentId)
        {
            Console.WriteLine("     [Sub-Chatlog] Inserting...");
            var queryText = chatlogTable.ToHeader();
            var chatlog = new DataNormalizer.DataEntity.ChatLog(chatlogTable);
            using (var transaction = this._connection.BeginTransaction())
            {
                int TOTAL_DIALOG_NUM = chatlogCollections.Count;
                int finished = 0;

                int BATCH_COUNT = 0;

                int LENGTH = chatlogCollections.Count;
                if (LENGTH == 0) return;
                else
                {
                    queryText += chatlog.getParameterString(chatlogCollections[0], IncidentId);
                }
                for (int i = 1; i < LENGTH; i++)
                {
                    var match = chatlogCollections[i];
                    queryText += "," + chatlog.getParameterString(match, IncidentId);
                    if (BATCH_COUNT >= 100 || i == LENGTH - 1)
                    {
                        _executeCMD(queryText, this._connection, transaction);
                        BATCH_COUNT = 0;
                        queryText = chatlogTable.ToHeader();
                        finished += 100;
                    }
                }

                //chatlog.clearState();
                transaction.Commit();
                Console.WriteLine($"     [Sub-Chatlog] Insert : Done! IncidentID = {IncidentId}");
            }
        }
        public int BatchInsertIncidentCollections(MatchCollection collections, ConfigInitializer.IncidentTableEntity incidentTable, out HashSet<string> duplicateHash)
        {
            duplicateHash = new HashSet<string>();

            var queryText = incidentTable.ToString();
            var incident = new DataNormalizer.DataEntity.Incident(incidentTable);
            int successCount = 0;
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
                        var incidentString = match.Groups[1].Value;
                        duplicateHash.Add(incidentString);
                        Console.WriteLine($"Duplicate Incident: {incidentString}");
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