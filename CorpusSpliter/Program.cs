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
            string text = System.IO.File.ReadAllText(fname);
            var pattern = new PatternGenerator(
                @"D:\BingAdsPrj\CorpusSpliter\CorpusSpliter\bin\Debug\ItemName.txt");
            Parser ps = new Parser(pattern.Regex);
            var matchCollections = ps.Matches(text);
            string middle = matchCollections[0].Groups[1].Value;
        }
    }
}
