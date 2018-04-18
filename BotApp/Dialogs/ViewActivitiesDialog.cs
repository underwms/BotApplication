using BotApp.Caches;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotApp.Dialogs
{
    [Serializable]
    public class ViewActivitiesDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var dbResults = ActivityCahce.GetAllActivities();
            var display = dbResults.Select(activity => activity.Description);

            var branches = $"Here are all the activities you can book:  {Environment.NewLine}" +
                $"{string.Join(",  " + Environment.NewLine, display)}";

            await context.PostAsync(branches);

            context.Done("");
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Done<object>(null);
        }
    }
}