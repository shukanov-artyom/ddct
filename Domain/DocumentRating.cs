using System;
using System.Collections.Generic;

namespace Bot.Domain
{
    public class DocumentRating : BotDomainObject
    {
        public DocumentRating()
        {
            UserRatings = new List<UserDocumentRating>();
        }

        public List<UserDocumentRating> UserRatings { get; }
    }
}