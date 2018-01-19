using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bot.Utils
{
    public class LinksExtractor
    {
        private const string RegexPattern =
            @"(?<Header>https://www|http://www|www.|https://|http://)\S+";

        private readonly string text;

        private readonly Match match;

        private readonly Lazy<List<string>> detectedLinks;

        public LinksExtractor(string text)
        {
            this.text = text;
            var regex = new Regex(
                RegexPattern,
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            match = regex.Match(text);
            detectedLinks =
                new Lazy<List<string>>(() => GetLinks(match).ToList());
        }

        public int GetDetectedLinksCount()
        {
            return detectedLinks.Value.Count;
        }

        public List<string> ExtractLinks()
        {
            return detectedLinks.Value;
        }

        private IEnumerable<string> GetLinks(Match match)
        {
            while (match.Success)
            {
                yield return match.Value;
                match = match.NextMatch();
            }
        }
    }
}