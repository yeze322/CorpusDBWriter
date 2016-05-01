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
        public static string fname = @"D:\BingAdsCSSAMBot\Data\Phone and email_Targeting.txt";
        public static string testFileName = "single.txt";
        public static readonly string[] STOP_WORDS = { "\r\n--------------------------------------------------------------------------------------" };
        static void Main(string[] args)
        {
            Spliter spliter = new Spliter(STOP_WORDS);
            var ret = spliter.SplitFile(fname);
            System.IO.File.WriteAllText("single.txt", ret[0]);
            string pattern = new PatternGenerator(@"D:\BingAdsPrj\CorpusSpliter\CorpusSpliter\bin\Debug\ItemName.txt").Generate();
            //string pattern = @"Item Number : (.*)\r\nIncident ID";
            Parser ps = new Parser(pattern);
            var matched = ps.Matches(ret[0]);
            string middle = matched[0].Groups[1].Value;
        }
    }
}
