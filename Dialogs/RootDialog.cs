using System;
using System.Threading.Tasks;
using Bot.Utils;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(
            IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var linksExtractor = new LinksExtractor(message.Text);
            int linksCount = linksExtractor.GetDetectedLinksCount();
            if (linksCount > 1)
            {
                await context.PostAsync(
                    "Wow, let's do one link at a time, i'm not so intelligent yet.");
            }
            else if (linksCount == 1)
            {
                await context.Forward(
                    new ProcessLinkDialog(),
                    ResumeAfterLinkProcessed,
                    message);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterLinkProcessed(
            IDialogContext context,
            IAwaitable<object> argument)
        {
            context.Wait(MessageReceivedAsync);
        }
    }
}