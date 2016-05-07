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
                Console.WriteLine("[IncidentDB] Importing file : " + fname + "...");
                string lineCache = System.IO.File.ReadAllText(fname);
                var incidentCollections = rootParser.executeMatch(lineCache);

                HashSet<string> dupIncidentList;
                // insert stem items
                incidentCount += dbc.BatchInsertIncidentCollections(incidentCollections, incidentTableEntity, out dupIncidentList);
                // insert dialogs

                int CASENOTES_INDEX = incidentCollections[0].Groups.Count - 1;
                Console.WriteLine("[IncidentDB] File imported...");

                // execute query to obtain Map(IncidentId_String, ID_Int)
                Console.WriteLine("[ChatlogDB] Start Feching ID...");
                var queryResult = dbc.ExecuteQuery(@"SELECT id, IncidentId FROM dbo.incidents");
                Dictionary<string, int> IncidentString_Id_Map = queryResult.ToDictionary(x => x[1], x => int.Parse(x[0]));
                Console.WriteLine("[ChatlogDB] Finish Feching ID!");

                foreach (Match match in incidentCollections)
                {
                    var incidentString = match.Groups[1].Value;
                    if (dupIncidentList.Contains(incidentString))
                    {
                        Console.WriteLine($"skip duplicated incident: {incidentString} ...");
                        continue;
                    }
                    else
                    {
                        var dialogCollections = caseParser.executeMatch(match.Groups[CASENOTES_INDEX].Value);
                        Console.WriteLine("     [Sub-Chatlog] Extracting Chatlogs: Done");
                        try
                        {
                            dbc.BatchInsertDialogCollections(
                                chatlogTableEntity,
                                dialogCollections,
                                IncidentString_Id_Map[match.Groups[1].Value]
                                );
                        }
                        catch
                        {
                            Console.WriteLine("===Find Key Failed: " + incidentString);
                        }
                    }
                }
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
