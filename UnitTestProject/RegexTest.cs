using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorpusSpliter;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void RootRegexTest()
        {
            RootParser rp = new RootParser(@"./Config/ItemList.txt");
            string testCorpus = System.IO.File.ReadAllText(@"sampleCorpus.txt");
            var collection = rp.executeMatch(testCorpus);
            Assert.AreEqual(collection.Count, 1);
            var match = collection[0];
            Assert.AreEqual(match.Groups.Count, 25);

            Func<int, string, bool> checkMatch = (index, str) => { return match.Groups[index].Value == str; };
            
            Assert.IsTrue(checkMatch(1, @"10015678375"));
            Assert.IsTrue(checkMatch(5, @"7/10/2015 12:00:00 AM"));
            Assert.IsTrue(checkMatch(17, @"TASK \ 20150715 \ CLOSURE \ RM FOMENTO MERCANTIL \ B014G8UC \ SYNDICATED SEARCH \ BR"));

        }
        [TestMethod]
        public void CaseNotesRegexTest()
        {
            CaseNoteParser cp = new CaseNoteParser();
            string testDialog = System.IO.File.ReadAllText(@"sampleDialog.txt");
            var collections = cp.executeMatch(testDialog);

            Debug.WriteLine(collections[0].Groups.Count);
            Debug.WriteLine(collections[0].Groups[1]);
            Debug.WriteLine(collections[0].Groups[2]);

            Func<Match, int, string> mGet = (match, index) => { return match.Groups[index+1].Value; };
            //assert line equals.....
            foreach(Match x in collections) { Assert.AreEqual(4,x.Groups.Count); }
            var checkSet = new List<List<string>>
            {
                new List<string>{ @"[10:12:01]", @"wojtek", "hi " },
                new List<string>{ @"[10:12:19]", @"Alex", "Hello Wojtek." },
                new List<string>{ @"[10:12:30]", @"wojtek", "i want to know how in my campaign i set to not display ads to women or specified age " },
            };

            for(int i=0;i<checkSet.Count;i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(mGet(collections[i], j), checkSet[i][j]);
                }
            }
        }
    }
}
