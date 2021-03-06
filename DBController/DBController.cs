﻿using System;
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
            try
            {
                var ret = cmd.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine($"Error happens while inserting dialog but was ignored");
            }
        }
        public void BatchInsertDialogCollections(ConfigInitializer.ChatlogTableEntity chatlogTable, MatchCollection chatlogCollections, int IncidentId, bool SKIP)
        {
            if (SKIP) return;

            Console.WriteLine("     [Sub-Chatlog] Inserting...");
            var queryText = chatlogTable.ToHeader();
            var chatlog = new DataNormalizer.DataEntity.ChatLog(chatlogTable);
            using (var transaction = this._connection.BeginTransaction())
            {
                int TOTAL_DIALOG_NUM = chatlogCollections.Count;
                int finished = 0;

                int BATCH_COUNT = 0;

                int LENGTH = chatlogCollections.Count;

                for (int i = 0; i < LENGTH; i++)
                {
                    var match = chatlogCollections[i];
                    if (BATCH_COUNT != 0)
                    {
                        // add head value with ","
                        queryText += ",";
                    }
                    queryText += chatlog.getParameterString(match, IncidentId);
                    BATCH_COUNT += 1;
                    // reach BATCH SIZE || last match
                    if (BATCH_COUNT >= 100 || i == LENGTH - 1)
                    {
                        // execute command and reset cmd text
                        _executeCMD(queryText, this._connection, transaction);
                        queryText = chatlogTable.ToHeader();
                        BATCH_COUNT = 0;
                        finished += 100;
                    }
                }

                //chatlog.clearState();
                transaction.Commit();
                Console.WriteLine($"     [Sub-Chatlog] Insert : Done! IncidentID = {IncidentId}, LENGTH={LENGTH}");
            }
        }
        public int BatchInsertIncidentCollections(MatchCollection collections, ConfigInitializer.IncidentTableEntity incidentTable, out HashSet<string> duplicateHash, bool SKIP)
        {
            duplicateHash = new HashSet<string>();
            if (SKIP) return 0;


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
        public void InsertIncidentLevel(ConfigInitializer.LevelTableEntity levelTable, MatchCollection collections, int incidentId)
        {
            string qText = levelTable.ToString();
            var cmd = new SqlCommand(qText, this._connection);
            for (int i = 0; i < 5; i++)
            {
                string tag = collections[i].Groups[1].Value;
                cmd.Parameters.AddWithValue($"@{i + 1}", tag);
            }
            cmd.Parameters.AddWithValue($"@6", incidentId);
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($" @InsertIncidentLevel : insert {incidentId}");
            }
            catch
            {
                Console.WriteLine("Error happend at @InsertIncidentLevel");
            }
        }
        public void ClearTable(string tableName, string pkname)
        {
            var queryText = $"DELETE FROM {tableName} WHERE {pkname} IN (SELECT {pkname} FROM {tableName})";
            var cmd = new SqlCommand(queryText, this._connection);
            cmd.ExecuteNonQuery();
        }
    }
}