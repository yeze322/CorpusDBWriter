using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class JsonTest
    {
        [TestMethod]
        public void ToJson()
        {
            var jf = new DataNormalizer.JsonFactory();
            var inci = new DataNormalizer.DataEntity.Incident();
            Console.WriteLine(jf.ToJson(inci));
        }
    }
}
