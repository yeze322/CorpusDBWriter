using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigInitializer
{
    public class TableEntity
    {
        public readonly List<string> dataTypeList;
        protected string TABLE_NAME = null;
        protected string TABLE_ITEMS = null;
        protected string TABLE_VALUE_FORMAT = null;
        public TableEntity(string itemNameConfig, string dataTypeConfig)
        {
            var itemNameArray = System.IO.File.ReadAllLines(itemNameConfig);
            this.TABLE_ITEMS = string.Join(",", itemNameArray.Select(x => x.Replace(" ", "")).ToList());
            // [1,2,3....] => "@1,@2,@3..."
            this.TABLE_VALUE_FORMAT = string.Join(",", Enumerable.Range(1, itemNameArray.Length).Select(i => "@" + i.ToString()));
            this.dataTypeList = System.IO.File.ReadAllLines(dataTypeConfig).ToList();
        }
        private string _tostring = null;
        public override string ToString()
        {
            if (this._tostring == null)
            {
                this._tostring = $"INSERT INTO {TABLE_NAME} ({TABLE_ITEMS}) VALUES ({TABLE_VALUE_FORMAT})";
            }
            return this._tostring;
        }

        private string _toheader = null;
        public string ToHeader()
        {
            if (this._toheader == null)
            {
                this._toheader = $"INSERT INTO {TABLE_NAME} ({TABLE_ITEMS}) VALUES ";
            }
            return this._toheader;
        }
    }

    public sealed class IncidentTableEntity : TableEntity
    {
        public IncidentTableEntity(string itemNameConfig, string dataTypeConfig) : base(itemNameConfig, dataTypeConfig)
        {
            base.TABLE_NAME = @"dbo.Incidents";
        }
    }

    public sealed class ChatlogTableEntity : TableEntity
    {
        public ChatlogTableEntity(string itemNameConfig, string dataTypeConfig) : base(itemNameConfig, dataTypeConfig)
        {
            base.TABLE_NAME = @"dbo.ChatLogs";
        }
    }
}
