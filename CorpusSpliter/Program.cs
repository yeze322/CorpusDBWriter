﻿using System;
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
        static void Main(string[] args)
        {
            string [] flist = System.IO.Directory.GetFiles("./Corpus");

            var incidentTableEntity = new ConfigInitializer.IncidentTableEntity(@"./Config/ItemList.txt", @"./Config/DataTypeList.txt");
            var rootRegex = new ConfigInitializer.RootRegex(@"./Config/ItemList.txt");
            var caseRegex = new ConfigInitializer.CaseRegex();

            var rootParser = new RootParser(rootRegex.ToString());
            var caseParser = new CaseNoteParser(caseRegex.ToString());

            var dbc = new DBController.DBController(ZIM_TOKEN);

            int newCount = 0;
            foreach (var fname in flist)
            {
                Console.WriteLine("Importing file : " + fname + "...");
                string lineCache = System.IO.File.ReadAllText(fname);
                var matchCollections = rootParser.executeMatch(lineCache);
                // insert stem items
                newCount += dbc.BatchInsertRegexCollections(incidentTableEntity, matchCollections);
                // insert dialog messages
            }
            executeExitAction(newCount);
        }
        public static void executeExitAction(int newCount)
        {
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
