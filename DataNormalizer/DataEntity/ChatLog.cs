using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DataNormalizer.DataEntity
{
    public class ChatLog
    {
        private static readonly string TableName = "dbo.ChatLogs";
        private static readonly string QueryString = "INSERT INTO ";
        public int Id { get; set; }
        public int Dialogue { get; set; }
        public int Order { get; set; }
        public string TimeLabel { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public int IncidentId { get; set; }

        public void registerSqlCommand(Match match, ref SqlCommand cmd)
        {

        }
    }
}