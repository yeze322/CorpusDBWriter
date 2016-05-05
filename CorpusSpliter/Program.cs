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
        #region obselete test config
        private static string CORPUS_FILE_NAME = @"../Phone and email_Targeting.txt";
        private static string TEST_TOKEN = @"Server=YEZE-PC;Database=AdventureWorks2014;User ID=yezetest;Password=950322;";
        private static string TEST_TABLENAME = @"HumanResources.Department";
        private static string TEST_TABLEHEADER = @"(Name, GroupName, ModifiedDate)";
        #endregion
        private static string ZIM_TOKEN = @"Server=zim-workstation;Database=CSSData;User ID=cssdata;Password=Password1234;";
        private static string TABLE_NAME = @"dbo.Incidents";
        static void Main(string[] args)
        {
            var pattern = new CorpusInfo(@"./Config/ItemList.txt", @"./Config/DataTypeDiction.json");
            string [] flist = System.IO.Directory.GetFiles("./Corpus");
            int newCount = 0;
            foreach (var fname in flist)
            {
                Console.WriteLine("Importing file : " + fname + "...");
                string text = System.IO.File.ReadAllText(fname);
                var matchCollections = new Parser(pattern.regex).Matches(text);
                var db = new DBController.DBController(ZIM_TOKEN, pattern.dbinfo);
                newCount += db.BatchInsertRegexCollections(TABLE_NAME, matchCollections);
                //var db = new DBController.DBController(TEST_TOKEN);
                //db.InsertRegexMatch(TEST_TABLENAME, TEST_TABLEHEADER, matchCollections[0]);

                //db.ClearTable(TABLE_NAME, "Id");
            }
            Console.WriteLine("===================================");
            Console.WriteLine($"      Add Items :  {newCount}");
            Console.WriteLine("===================================");
            for(int i = 3; i >0; i--)
            {
                Console.WriteLine($"exit in {i} seconds...");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
