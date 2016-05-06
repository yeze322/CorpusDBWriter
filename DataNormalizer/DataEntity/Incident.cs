using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DataNormalizer.DataEntity
{
    using SQLAddParaFunc = Func<SqlCommand, int, string, SqlParameter>;
    public class Incident
    {
        private static Dictionary<string, SQLAddParaFunc> dbAddParaLambdaDic = new Dictionary<string, SQLAddParaFunc>
        {
            { "INT", (cmd, i, val) => {
                return cmd.Parameters.AddWithValue($"@{i}", val == "" ? -1 : int.Parse(val));
            } },
            { "STRING", (cmd, i, val) => { return cmd.Parameters.AddWithValue($"@{i}", val); }},
            { "DATETIME", (cmd, i, val) => { return cmd.Parameters.AddWithValue($"@{i}", DateTime.Parse(val)); }},
        };

        private ConfigInitializer.IncidentTableEntity tableInfo;
        public Incident(ConfigInitializer.IncidentTableEntity tableEntity) { this.tableInfo = tableEntity; }
        public void registerSqlCommand(Match match, ref SqlCommand cmd)
        {
            for (int i = 0; i < this.tableInfo.dataTypeList.Count; i++)
            {
                var dataType = this.tableInfo.dataTypeList[i];
                dbAddParaLambdaDic[dataType](cmd, i + 1, match.Groups[i + 1].Value);
            }
        }
    }
}
