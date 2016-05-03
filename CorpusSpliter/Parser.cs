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
        public Parser(string regexString)
        {
            //@RegexOptions.Singleline:
            //  matches every character (instead of every character except \n)
            this._regex = new Regex(regexString, RegexOptions.Singleline);
        }
        public MatchCollection Matches(string text) { return this._regex.Matches(text); }
        [Obsolete("This function is unfinished")]
        public List<Tuple<string>> ExtractToList(string text)
        {
            var matchCollections = this._regex.Matches(text);
            var ret = new List<List<string>>();
            foreach (var x in matchCollections)
            {
//                ret.Add(new List<string>(x))
            }
            return null;
        }
    }
}