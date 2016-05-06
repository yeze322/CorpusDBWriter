using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CorpusSpliter
{
    class Program
    {
        private static string ZIM_TOKEN = @"Server=zim-workstation;Database=CSSData;User ID=cssdata;Password=Password1234;";
        static void Main(string[] args)
        {
            string[] flist = System.IO.Directory.GetFiles("./Corpus");

            var incidentTableEntity = new ConfigInitializer.IncidentTableEntity(@"./Config/IncidentItemList.txt", @"./Config/IncidentDataTypeList.txt");
            var chatlogTableEntity = new ConfigInitializer.ChatlogTableEntity(@"./Config/ChatlogItemList.txt", @"./Config/ChatlogDataTypeList.txt");

            var rootRegex = new ConfigInitializer.RootRegex(@"./Config/IncidentItemList.txt");
            var caseRegex = new ConfigInitializer.CaseRegex();

            var rootParser = new RootParser(rootRegex.ToString());
            var caseParser = new CaseNoteParser(caseRegex.ToString());

            var dbc = new DBController.DBController(ZIM_TOKEN);

            int incidentCount = 0;
            foreach (var fname in flist)
            {
                Console.WriteLine("Importing file : " + fname + "...");
                string lineCache = System.IO.File.ReadAllText(fname);
                var incidentCollections = rootParser.executeMatch(lineCache);
                // insert stem items
                incidentCount += dbc.BatchInsert(incidentCollections, incidentTableEntity, chatlogTableEntity);
                // insert dialog messages
            }
            executeExitAction(incidentCount);
        }
        public static void executeExitAction(int newCount)
        {
            Console.WriteLine("===================================");
            Console.WriteLine($"      Add Items :  {newCount}");
            Console.WriteLine("===================================");
            for (int i = 3; i > 0; i--)
            {
                Console.WriteLine($"exit in {i} seconds...");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
