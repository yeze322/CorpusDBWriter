using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace DataNormalizer.DataEntity
{
    public class LevelInfo : IDataEntity
    {
        public string getParameterString(Match match, int IncidentId) { return ""; }
        public void registerSqlCommand(Match match, ref SqlCommand cmd, int IncidentId) { }
    }
}
