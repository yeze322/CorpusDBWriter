using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CorpusSpliter
{
    public class CorpusInfo
    {
        #region const member
        private static string _prefixRegex = @"";
        private static string _bodyRegex = @"\s*: (.*?)\r\n";
        private static string _suffixRegex = @"\r\n--------------------------------------------------------------------------------------";
        #endregion
        public readonly DBController.DBInfo dbinfo = null;
        public readonly string regex = null;
        public CorpusInfo(string itemListFile, string dataTypeDictFile)
        {
            string[] itemListArray = System.IO.File.ReadAllLines(itemListFile);
            string dictFileContent = System.IO.File.ReadAllText(dataTypeDictFile);
            var itemToDataTypeMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictFileContent);
            this.regex = _prefixRegex + string.Join(_bodyRegex, itemListArray) + _bodyRegex + _suffixRegex;

            this.dbinfo = new DBController.DBInfo
            {
                columnNameList = itemListArray.Select(x => x.Replace(" ", "")).ToList(),
                dataTypeList = itemListArray.Where(k => itemToDataTypeMap.ContainsKey(k)).Select(v=>itemToDataTypeMap[v]).ToList()
            };
        }
    }
}
