using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataNormalizer
{
    public class JsonFactory
    {
        public string ToJson(DataEntity.Incident obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}
