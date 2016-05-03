using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CorpusSpliter
{
    // need to add profile
    public class Parser
    {
        private Regex _regex = null;
        // Constructor:
        //  @pattern - re expression which is bounded to a specific class
        public Parser(string pattern)
        {
            //@RegexOptions.Singleline:
            //  matches every character (instead of every character except \n)
            this._regex = new Regex(pattern, RegexOptions.Singleline);
        }
        public MatchCollection Matches(string text) { return this._regex.Matches(text); }
    }
    public class PatternGenerator
    {
        private string[] _itemNames = null;
        private static string _prefixRegex = @"";
        private static string _bodyRegex = @"\s*: (.*)\r\n";
        private static string _suffixRegex = @"\r\n--------------------------------------------------------------------------------------";
        [Obsolete("this APi is consturcted from filename. abandoned")]
        public PatternGenerator(string fname)
        {
            this._itemNames = System.IO.File.ReadAllLines(fname);
        }
        public string Generate()
        {
            string body = string.Join(_bodyRegex, this._itemNames);
            string suffix = _bodyRegex;
            return body + suffix;
        }
    }
}