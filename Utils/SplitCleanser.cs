using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test
{
    public class SplitCleanser
    {
        public string[] Cleanse(string[] source)
        {
            List<Regex> unacceptablePatterns =
                new List<Regex>()
                {
                    new Regex("\\W+", RegexOptions.Compiled),
                    new Regex("\\d+", RegexOptions.Compiled)
                };
            List<string> result = new List<string>();
            foreach (string item in source)
            {
                bool bad = unacceptablePatterns.Any(p => p.Match(item).Success);
                if (!bad)
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }
    }
}
