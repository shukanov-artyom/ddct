using System;

namespace Bot.Domain
{
    public class BotUser : BotDomainObject
    {
        public string ChannelId { get; set; }

        public string UserId { get; set; }
    }
}