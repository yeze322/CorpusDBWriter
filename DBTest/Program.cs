using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest
{
    class Program
    {
        private static string ZIM_TOKEN = @"Server=zim-workstation;Database=CSSData;User ID=cssdata;Password=Password1234;";
        static void Main(string[] args)
        {
            var dbc = new DBController.DBController(ZIM_TOKEN);
            var queryResult = dbc.ExecuteQuery(@"SELECT id, IncidentId FROM dbo.incidents");
            Dictionary<string, int> IncidentString_Id_Map = queryResult.ToDictionary(x => x[1], x => int.Parse(x[0]));
            Console.WriteLine(queryResult.Count);
        }
    }
}
