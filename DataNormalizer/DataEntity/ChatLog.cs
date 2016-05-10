using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DataNormalizer.DataEntity
{
    public class ChatLog
    {
        private ConfigInitializer.ChatlogTableEntity tableInfo;
        public ChatLog(ConfigInitializer.ChatlogTableEntity tableEntity) { this.tableInfo = tableEntity; }


        private int lastStateTail = 0;
        private int dialogIndex = 0;
        private int messageIndex = 0;
        private static readonly int CHAR_GAP = 6; //the accurate gap between two continious messages is exactly 6
        private void calculateState(Match match)
        {
            if (this.lastStateTail == 0)
            {
                // init state
                this.dialogIndex = 1;
                this.messageIndex = 1;
            }
            else
            {
                int gap = match.Index - this.lastStateTail;
                if (gap == CHAR_GAP)
                {
                    this.messageIndex += 1;
                }
                else if (gap >= CHAR_GAP)
                {
                    // Start a new dialog
                    this.dialogIndex += 1;
                    this.messageIndex = 1;
                }
            }
            this.lastStateTail = match.Index + match.Length;
        }
        public void registerSqlCommand(Match match, ref SqlCommand cmd, int IncidentId)
        {
            this.calculateState(match);

            cmd.Parameters.AddWithValue("@1", match.Groups[2].Value); // userName : string
            cmd.Parameters.AddWithValue("@2", match.Groups[3].Value); // content : string
            cmd.Parameters.AddWithValue("@3", IncidentId); // Incidents.ID : int
            cmd.Parameters.AddWithValue("@4", match.Groups[1].Value); // TimeLabel : string
            cmd.Parameters.AddWithValue("@5", this.dialogIndex);// Sub Dialog Index
            cmd.Parameters.AddWithValue("@6", this.messageIndex);// Per Dialog Message Index
        }
        public string getParameterString(Match match, int IncidentId)
        {
            this.calculateState(match);

            string lp = "'";
            string rp = "'";
            string ret = "(";
            ret += lp + match.Groups[2].Value + rp + ","; // 'userName',
            ret += lp + match.Groups[3].Value + rp + ","; // 'content',
            ret += IncidentId.ToString() + ",";     // incidentId,
            ret += lp + match.Groups[1].Value + rp + ",";   // 'TimeLabel',
            ret += this.dialogIndex.ToString() + ","; // Dialog,
            ret += this.messageIndex.ToString();    // Order
            ret += ")";
            return ret;
        }

        // record and split dialog
        public void clearState()
        {
            this.lastStateTail = 0;
        }
    }
}