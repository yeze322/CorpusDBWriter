using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNormalizer.DataEntity
{
    public interface IDataEntity
    {
        string toJson();
        string generateJsonTemplate();
    }
}
