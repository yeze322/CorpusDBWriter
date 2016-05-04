﻿using System;
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
        public readonly string[] itemNameList = null;
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
        private string tableHeader = null;
        public string TableHeader
        {
            get
            {
                if (this.tableHeader == null)
                {
                    string header = "(" + string.Join(",", this.itemNameList) + ")";
                    header.Replace(" ", "");
                }
                return this.tableHeader;
            }
        }
    }
}
