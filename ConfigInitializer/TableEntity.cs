using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigInitializer
{
    public class TableEntity
    {
        protected string TABLE_NAME = null;
        protected string TABLE_ITEMS = null;
        protected string TABLE_VALUE_FORMAT = null;
        public override string ToString()
        {
            return $"INSERT INTO {TABLE_NAME} ({TABLE_ITEMS}) VALUES ({TABLE_VALUE_FORMAT})";
        }
    }

    public sealed class IncidentTableEntity : TableEntity
    {
        public IncidentTableEntity(string itemNameConfig)
        {
            var itemNameArray = System.IO.File.ReadAllLines(itemNameConfig);
            var columnNameList = itemNameArray.Select(x => x.Replace(" ", "")).ToList();

            base.TABLE_NAME = @"dbo.Incidents";
            base.TABLE_ITEMS = string.Join(",", columnNameList);
            // [1,2,3....] => "@1,@2,@3..."
            base.TABLE_VALUE_FORMAT = string.Join(",", Enumerable.Range(1, itemNameArray.Length).Select( i => "@" + i.ToString()));
        }
    }
}
