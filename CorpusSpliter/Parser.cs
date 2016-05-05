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
}