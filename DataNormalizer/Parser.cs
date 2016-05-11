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
        private Regex _regex = null;
        // Constructor:
        //  @pattern - re expression which is bounded to a specific class
        public RootParser(string reString)
        {
            //@RegexOptions.Singleline:
            //  matches every character (instead of every character except \n)
            this._regex = new Regex(reString, RegexOptions.Singleline);
        }
        public MatchCollection executeMatch(string text) { return this._regex.Matches(text); }
    }
    public class CaseNoteParser : IParser
    {
        private Regex _regex = null;
        public CaseNoteParser(string reString)
        {
            this._regex = new Regex(reString);
        }
        public MatchCollection executeMatch(string text) { return this._regex.Matches(text); }
    }
    public class LevelParser : CaseNoteParser
    {
        public LevelParser(string reString) : base(reString) { }
    }
}