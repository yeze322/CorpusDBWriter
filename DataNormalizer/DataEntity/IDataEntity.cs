using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DataNormalizer.DataEntity
{
    public interface IDataEntity
    {
        string getParameterString(Match match, int IncidentId);
        void registerSqlCommand(Match match, ref SqlCommand cmd, int IncidentId);
    }
}