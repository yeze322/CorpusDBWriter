using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusSpliter
{
    public class Spliter
    {
        // @_splitWords: word list for spliting text source
        private string[] _splitWords = null;
        // Constructors
        public Spliter(string[] splitWordList) { this._splitWords = splitWordList; }
        public Spliter(string splitWord) { this._splitWords = new string[] { splitWord }; }
        //Split method:
        //  @fname: file name for spliting
        public string[] SplitFile(string fname)
        {
            string text = System.IO.File.ReadAllText(fname);
            //string[] lines = System.IO.File.ReadAllLines(fname);
            return text.Split(this._splitWords, StringSplitOptions.None);
        }
    }
}
