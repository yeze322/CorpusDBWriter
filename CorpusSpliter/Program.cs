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
        private static string fname = @"Phone and email_Targeting.txt";
        private static string testToken =@"Server=YEZE-PC;Database=AdventureWorks2014;User ID=yezetest;Password=950322;";
        //public static string testFileName = "single.txt";
        static void Main(string[] args)
        {
            var db = new DBController.DBController(testToken);
            //var ret = db.ExecuteQuery("SELECT * FROM [HumanResources].[Department]");

            db.InsertRegexMatch(null, null, null);

            string text = System.IO.File.ReadAllText(fname);
            var pattern = new CorpusPattern(
                @"ItemName.txt");
            var itemList = pattern.itemNameList;

            var matchCollections = new Parser(pattern.Regex).Matches(text);
            var x = matchCollections[0];
            //usage, unfinished
            string middle = matchCollections[0].Groups[1].Value;
        }
    }
}
