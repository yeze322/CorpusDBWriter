using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void TableConfig()
        {
            var rtb = new ConfigInitializer.IncidentTableEntity(@"./Config/ItemList.txt", "asd");
            Console.WriteLine(rtb.ToString());
        }
        [TestMethod]
        public void RegexTest()
        {
            var rg = new ConfigInitializer.RootRegex(@"./Config/ItemList.txt");
            var cg = new ConfigInitializer.CaseRegex();
            Console.WriteLine(rg);
            Console.WriteLine(cg);
        }
    }
}
