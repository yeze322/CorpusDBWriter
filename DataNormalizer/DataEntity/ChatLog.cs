using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DataNormalizer.DataEntity
{
    public class ChatLog
    {
        private ConfigInitializer.ChatlogTableEntity tableInfo;
        public ChatLog(ConfigInitializer.ChatlogTableEntity tableEntity) { this.tableInfo = tableEntity; }
        public void registerSqlCommand(Match match, ref SqlCommand cmd, string IncidentId)
        {
            cmd.Parameters.AddWithValue("@1", match.Groups[2].Value);
            cmd.Parameters.AddWithValue("@2", match.Groups[3].Value);
            cmd.Parameters.AddWithValue("@3", IncidentId);
            cmd.Parameters.AddWithValue("@4", match.Groups[1].Value);
            cmd.Parameters.AddWithValue("@5", 1);
            cmd.Parameters.AddWithValue("@6", 1);
        }

        // record and split dialog
        private int lastStateTail = 0;
        public void clearState()
        {
            this.lastStateTail = 0;
        }
    }
}