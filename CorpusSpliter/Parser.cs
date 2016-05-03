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
    }
}