using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigInitializer
{
    public class BaseRegex
    {
        protected string reString = null;
        public override string ToString()
        {
            return this.reString;
        }
    }
    public sealed class RootRegex : BaseRegex
    {
        private static string PREFFIX = @"";
        private static string BODY = @"\s*: (.*?)\r\n";
        private static string SUFFIX = @"\r\n--------------------------------------------------------------------------------------";
        public RootRegex(string itemNameConfig)
        {
            var itemNameArray = System.IO.File.ReadAllLines(itemNameConfig);
            base.reString = PREFFIX + string.Join(BODY, itemNameArray) + BODY + SUFFIX;
        }
    }
    public sealed class CaseRegex : BaseRegex
    {
        private static readonly string NAME_CATCH = @"([a-zA-Z]+):";
        private static readonly string NUM2 = @"[0-9]{2}";
        private static readonly string TIME_CATCH = $"(\\[{NUM2}:{NUM2}:{NUM2}\\])";
        private static readonly string SENTENCE_CATCH = "(.*)\r\n";
        private static readonly string SENTENCE = $"{TIME_CATCH} {NAME_CATCH} {SENTENCE_CATCH}";

        public CaseRegex()
        {
            base.reString = SENTENCE;
        }
    }
}
