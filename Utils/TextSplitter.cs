using System;
using System.Collections.Generic;
using System.Linq;

namespace Bot.Utils
{
    public class TextSplitter
    {
        private readonly string text;

        public TextSplitter(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text is empty.");
            }
            this.text = text;
        }

        public string[] Split(int segmentLength)
        {
            if (text.Length <= segmentLength)
            {
                return new string[] { text };
            }
            return SplitStringPrivate(text, segmentLength).ToArray();
        }

        private static IEnumerable<string> SplitStringPrivate(
            string sourceText,
            int segLength)
        {
            string[] split = sourceText.Split(' ');
            string accum = String.Empty;
            for (int i = 0; i < split.Length; i++)
            {
                if (accum.Length + split[i].Length < segLength)
                {
                    accum = $"{accum} {split[i]}";
                }
                else
                {
                    yield return accum;
                    accum = String.Empty;
                }
            }
        }
    }
}