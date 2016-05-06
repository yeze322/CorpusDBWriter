using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CorpusSpliter
{
    public interface IParser
    {
        MatchCollection executeMatch(string text);
    }
    public class RootParser : IParser
    {
        #region const member
        private static string _prefixRegex = @"";
        private static string _bodyRegex = @"\s*: (.*?)\r\n";
        private static string _suffixRegex = @"\r\n--------------------------------------------------------------------------------------";
        #endregion
        private string _reString = null;
        private Regex _regex = null;
        // Constructor:
        //  @pattern - re expression which is bounded to a specific class
        public RootParser(string itemListFile)
        {
            var itemListArray = System.IO.File.ReadAllLines(itemListFile);
            //@RegexOptions.Singleline:
            //  matches every character (instead of every character except \n)
            this._reString = _prefixRegex + string.Join(_bodyRegex, itemListArray) + _bodyRegex + _suffixRegex;
            this._regex = new Regex(this._reString, RegexOptions.Singleline);
        }
        public MatchCollection executeMatch(string text) { return this._regex.Matches(text); }
    }
    public class CaseNoteParser : IParser
    {
        private static readonly string re_name_catch = @"([a-zA-Z]+):";
        private static readonly string re_num2 = @"[0-9]{2}";
        private static readonly string re_time_catch = $"(\\[{re_num2}:{re_num2}:{re_num2}\\])";
        private static readonly string re_sentence_catch = "(.*)\r\n";
        private static readonly string reString = $"{re_time_catch} {re_name_catch} {re_sentence_catch}";

        private Regex _regex = null;
        public CaseNoteParser()
        {
            this._regex = new Regex(reString);
        }
        public MatchCollection executeMatch(string text) { return this._regex.Matches(text); }
    }
}