using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusSpliter
{
    public class PatternGenerator
    {
        #region const member
        private static string _prefixRegex = @"";
        private static string _bodyRegex = @"\s*: (.*?)\r\n";
        private static string _suffixRegex = @"\r\n--------------------------------------------------------------------------------------";
        #endregion
        public readonly string[] itemNameList = null;
        public PatternGenerator(string configFile)
        {
            this.itemNameList = System.IO.File.ReadAllLines(configFile);
        }
        private string regex = null;

        public string Regex
        {
            get
            {
                if (regex == null)
                {
                    this.regex = _prefixRegex
                        + string.Join(_bodyRegex, this.itemNameList) + _bodyRegex 
                        + _suffixRegex;
                    return this.regex;
                }
                else
                {
                    return this.regex;
                }
            }
        }
    }
}
