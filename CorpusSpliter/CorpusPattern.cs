using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusSpliter
{
    public class CorpusPattern
    {
        #region const member
        private static string _prefixRegex = @"";
        private static string _bodyRegex = @"\s*: (.*?)\r\n";
        private static string _suffixRegex = @"\r\n--------------------------------------------------------------------------------------";
        #endregion
        private readonly string[] itemNameList = null;
        public CorpusPattern(string configFile)
        {
            this.itemNameList = System.IO.File.ReadAllLines(configFile);
        }
        private string regex = null;

        public string Regex
        {
            get
            {
                if (this.regex == null)
                {
                    this.regex = _prefixRegex
                        + string.Join(_bodyRegex, this.itemNameList) + _bodyRegex
                        + _suffixRegex;
                }
                return this.regex;
            }
        }
        private List<string> tableHeaderList = null;
        public List<string> TableHeaderList
        {
            get
            {
                if (this.tableHeaderList == null)
                {
                    this.tableHeaderList = this.itemNameList.Select(x => x.Replace(" ", "")).ToList();
                }
                return this.tableHeaderList;
            }
        }
        private List<string> tableDataTypeList = null;
        public List<string> TableDataTypeList
        {
            get
            {
                if (this.tableDataTypeList == null)
                {
                    this.tableDataTypeList = new List<string> {
"STRING",
"STRING",
"STRING",
"INT",
"DATETIME",
"STRING",
"DATETIME",
"STRING",
"STRING",
"STRING",
"STRING",
"STRING",
"STRING",
"STRING",
"DATETIME",
"STRING",
"STRING",
"STRING",
"INT",
"INT",
"INT",
"INT",
"INT",
"STRING"};
                }
                return this.tableDataTypeList;
            }
        }
    }
}
