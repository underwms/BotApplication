using BotApp.Caches;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotApp.Dialogs
{
    [Serializable]
    public class ViewBranchesDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var dbResults = LocationCache.GetAllBranches();
            var display = dbResults.Select(branch => $"{branch.State}, {branch.City}");

            var branches = $"Here are all the branches I am aware of:  {Environment.NewLine}" +
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