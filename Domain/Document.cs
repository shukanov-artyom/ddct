using System;
using System.Collections.Generic;

namespace Bot.Domain
{
    public class Document : BotDomainObject
    {
        public Document()
        {
            Keywords = new List<string>();
        }

        public string Url { get; set; }

        public BotUser User { get; set; }

        public DocumentRating Rating { get; set; }

        public List<string> Keywords { get; }
    }
}