using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Utils;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    public class ProcessLinkDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync("ok, processing that doc");
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(
            IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            IMessageActivity messageActivity = await argument;
            string message = messageActivity.Text;
            var extractor = new LinksExtractor(message);
            List<string> links = extractor.ExtractLinks();
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
            throw new NotImplementedException();
        }
    }
}