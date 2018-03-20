using System;

namespace Bot.Domain
{
    public class UserDocumentRating : BotDomainObject
    {
        long UserId { get; set; }

        long DocumentId { get; set; }

        int RatingValue { get; set; }
    }
}