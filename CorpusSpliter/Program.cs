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
        static void Main(string[] args)
        {
            bool SKIP = true;
            // init resources and configs
            string[] flist = System.IO.Directory.GetFiles("./Corpus");

            var ZIM_TOKEN = System.IO.File.ReadAllText("./Config/dbconfig");

            var incidentTableEntity = new ConfigInitializer.IncidentTableEntity(@"./Config/IncidentItemList.txt", @"./Config/IncidentDataTypeList.txt");
            var chatlogTableEntity = new ConfigInitializer.ChatlogTableEntity(@"./Config/ChatlogItemList.txt", @"./Config/ChatlogDataTypeList.txt");
            var levelTableEntity = new ConfigInitializer.LevelTableEntity(@"./Config/LevelItemList.txt", @"./Config/LevelDataTypeList.txt");

            var rootRegex = new ConfigInitializer.RootRegex(@"./Config/IncidentItemList.txt");
            var caseRegex = new ConfigInitializer.CaseRegex();
            var levelRegex = new ConfigInitializer.LevelRegex();

            var rootParser = new RootParser(rootRegex.ToString());
            var caseParser = new CaseNoteParser(caseRegex.ToString());
            var levelParser = new LevelParser(levelRegex.ToString());

            var dbc = new DBController.DBController(ZIM_TOKEN);

            // start insert by file
            int incidentSuccessCount = 0;
            foreach (var fname in flist)
            {
                Console.WriteLine("[IncidentDB] Importing file : " + fname + "...");
                string lineCache = System.IO.File.ReadAllText(fname);
                var incidentCollections = rootParser.executeMatch(lineCache);

                // out parameter, saving dup incident id
                HashSet<string> dupIncidentList;
                // insert stem items
                incidentSuccessCount += dbc.BatchInsertIncidentCollections(incidentCollections, incidentTableEntity, out dupIncidentList, SKIP);
                Console.WriteLine("[IncidentDB] File imported...");
                // execute query to obtain Map(IncidentId_String, ID_Int)
                Console.WriteLine("[ChatlogDB] Start Feching ID...");
                var queryResult = dbc.ExecuteQuery(@"SELECT id, IncidentId FROM dbo.incidents");
                Dictionary<string, int> IncidentString_Id_Map = queryResult.ToDictionary(x => x[1], x => int.Parse(x[0]));
                Console.WriteLine("[ChatlogDB] Finish Feching ID!");

                // insert dialogs & case level
                int CASENOTES_INDEX = incidentCollections[0].Groups.Count - 1;
                int INCIDENT_INDEX = 1;
                foreach (Match incidentMatch in incidentCollections)
                {
                    var incidentString = incidentMatch.Groups[INCIDENT_INDEX].Value;
                    if (dupIncidentList.Contains(incidentString) || ! IncidentString_Id_Map.ContainsKey(incidentString))
                    {
                        Console.WriteLine($"skip incident: {incidentString} ...");
                        continue;
                    }

                    int incidentId = IncidentString_Id_Map[incidentString];
                    string caseNotesText = incidentMatch.Groups[CASENOTES_INDEX].Value;

                    var dialogCollections = caseParser.executeMatch(caseNotesText);
                    // insert chalogs' main function
                    dbc.BatchInsertDialogCollections(chatlogTableEntity, dialogCollections, incidentId, SKIP);
                    // insert level info's main funciton
                    var levelCollections = levelParser.executeMatch(caseNotesText);
                    if (levelCollections.Count == 5)
                    {
                        //assert.equal(levelCollections.groups.count, 5);
                        dbc.InsertIncidentLevel(levelTableEntity, levelCollections, incidentId);
                    }
                }
            }
            executeExitAction(incidentSuccessCount);
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
