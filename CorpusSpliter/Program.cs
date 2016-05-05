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
        private static string CORPUS_FILE_NAME = @"Phone and email_Targeting.txt";
        private static string TEST_TOKEN =@"Server=YEZE-PC;Database=AdventureWorks2014;User ID=yezetest;Password=950322;";
        private static string TEST_TABLENAME = @"HumanResources.Department";
        private static string TEST_TABLEHEADER = @"(Name, GroupName, ModifiedDate)";

        private static string ZIM_TOKEN = @"Server=zim-workstation;Database=CSSData;User ID=cssdata;Password=Password1234;";
        private static string TABLE_NAME = @"dbo.Incidents";
        static void Main(string[] args)
        {
            string text = System.IO.File.ReadAllText(CORPUS_FILE_NAME);
            var pattern = new CorpusPattern(
                @"ItemName.txt");

            var matchCollections = new Parser(pattern.Regex).Matches(text);
            var x = matchCollections[0];
            //usage, unfinished
            string middle = matchCollections[0].Groups[1].Value;


            var db = new DBController.DBController(ZIM_TOKEN);
            foreach(Match match in matchCollections)
            {
                db.InsertRegexMatch(TABLE_NAME, pattern.TableHeaderList, pattern.TableDataTypeList, match);
            }
            //var db = new DBController.DBController(TEST_TOKEN);
            //db.InsertRegexMatch(TEST_TABLENAME, TEST_TABLEHEADER, matchCollections[0]);

            var ret = db.ExecuteQuery($"SELECT * FROM {TABLE_NAME}");
        }
    }
}
