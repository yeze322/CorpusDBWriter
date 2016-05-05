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
            string [] flist = System.IO.Directory.GetFiles("./Corpus");
            var dbinfo = new DBController.DBInfo(@"./Config/ItemList.txt", @"./Config/DataTypeDiction.json");
            var rootParser = new RootParser(@"./Config/ItemList.txt");
            
            int newCount = 0;
            foreach (var fname in flist)
            {
                Console.WriteLine("Importing file : " + fname + "...");
                string lineCache = System.IO.File.ReadAllText(fname);
                var matchCollections = rootParser.executeMatch(lineCache);
                var db = new DBController.DBController(ZIM_TOKEN, dbinfo);
                newCount += db.BatchInsertRegexCollections(TABLE_NAME, matchCollections);

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
