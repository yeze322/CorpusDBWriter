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
        //public static string testFileName = "single.txt";
        static void Main(string[] args)
        {
            string text = System.IO.File.ReadAllText(fname);
            var pattern = new CorpusPattern(
                @"ItemName.txt");
            var itemList = pattern.itemNameList;

            var matchCollections = new Parser(pattern.Regex).Matches(text);
            //usage, unfinished
            string middle = matchCollections[0].Groups[1].Value;
        }
    }
}
