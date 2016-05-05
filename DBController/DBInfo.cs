using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DBController
{
    public class DBInfo
    {
        public readonly List<string> columnNameList;
        public readonly List<string> dataTypeList;
        public readonly string queryItemPattern;
        public readonly string queryValuePattern;
        public DBInfo(string[] itemListArray, Dictionary<string,string> itemToDataTypeMap)
        {
            this.columnNameList = itemListArray.Select(x => x.Replace(" ", "")).ToList();
            this.dataTypeList = itemListArray.Where(k => itemToDataTypeMap.ContainsKey(k)).Select(v => itemToDataTypeMap[v]).ToList();

            this.queryItemPattern = $"({string.Join(",", this.columnNameList)})";
            this.queryValuePattern = $"(@{string.Join(",@", this.columnNameList)})";
        }
    }
}
