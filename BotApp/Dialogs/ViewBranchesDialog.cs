
using Bot_Application1.Caches;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class ViewBranchesDialog : IDialog<object>
    {
        public ViewBranchesDialog()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            var dbResults = LocationCache.GetAllBranches();
            var display = dbResults.Select(branch => $"{branch.State}, {branch.City}");

            var branches = $"Here are all the branches I am aware of:  {Environment.NewLine}" +
                $"{string.Join(",  " + Environment.NewLine, display)}";

            await context.PostAsync(branches);

            context.Done("");
        }

        //even tho this dialog's job is just to displaying information this method is needed for the Scorable to intrupt the dialog stack
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Done<object>(null);
        }
    }
}