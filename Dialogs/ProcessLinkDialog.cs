using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Services;
using Bot.Utils;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Test;

namespace Bot.Dialogs
{
    [Serializable]
    public class ProcessLinkDialog : IDialog<object>
    {
        private const int MaxDocumentLength = 5000; // MS Text Analytics Api limitation

        private string link;
        private int rating;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(
            IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            await context.SayAsync("ok, processing that doc");
            IMessageActivity messageActivity = await argument;
            string message = messageActivity.Text;
            var extractor = new LinksExtractor(message);
            List<string> links = extractor.ExtractLinks();
            await context.SayAsync($"ok, got {links.Count} links");
            link = links.First();
            if (links.Count > 1)
            {
                throw new InvalidOperationException(
                    "Cannot process several links at the moment.");
            }
            PromptDialog.Number(context,
                ResumeAfterRateIsSet,
                "How do you rate this article? (1-10)",
                "Please retry",
                3,
                null,
                1,
                10);
        }

        private async Task ResumeAfterRateIsSet(
            IDialogContext context,
            IAwaitable<long> argument)
        {
            rating = (int)await argument;
            string message = GetMessageAfterRatingSet(rating);
            await context.SayAsync(message);
            var scraper = new ScraperService();
            string scrappedText = await scraper.ScrapAsync(link);
            TextSplitter splitter = new TextSplitter(scrappedText);
            string[] split = splitter.Split(MaxDocumentLength);
            var cleanser = new SplitCleanser();
            split = cleanser.Cleanse(split);
            var textAnalytics = new TextAnalyticsApiService();
            var keywords = await textAnalytics.GetKeywordsAsync(split);
            context.Done<object>(null);
        }

        private static string GetMessageAfterRatingSet(int rating)
        {
            if (rating >= 8)
            {
                return 
                    "Oh, looks like it's something really interesting.";
            }
            else
            {
                return "Got it. Analysing.";
            }
        }
    }
}