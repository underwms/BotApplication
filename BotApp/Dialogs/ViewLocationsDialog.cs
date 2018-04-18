using BotApp.Caches;
using BotAssets.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotApp.Dialogs
{
    [Serializable]
    public class ViewLocationsDialog : IDialog<object>
    {
        private List<Branch> Branches = new List<Branch>();
        
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Which branch's locations are you interested in?");
            Branches = LocationCache.GetAllBranches().ToList();

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var all = message.Text.Equals("all", StringComparison.InvariantCultureIgnoreCase);
            var branch = Branches.FirstOrDefault(b => b.City.Equals(message.Text, StringComparison.InvariantCultureIgnoreCase));

            if(all || !(branch is null))
            {
                var dbResults = all ? LocationCache.GetAllLocations() : LocationCache.GetLocationsByBranch(branch.BranchId);

                var display = all
                    ? dbResults.Select(location => $"{location.Branch.State}, {location.Branch.City} - {location.Description}")
                    : dbResults.Select(location => location.Description);

                var header = all
                    ? "Here are all the locations I am aware of:  "
                    : $"Here are the location's for branch {branch.State}, {branch.City}:  ";

                var branches = $"{header}{Environment.NewLine}" +
                    $"{string.Join(",  " + Environment.NewLine, display)}";

                await context.PostAsync(branches);

                context.Done("");
            }
            else
            {
                await context.PostAsync($"I'm sorry, I don't understand your reply.  " +
                    $"Which branch's locations are you interested in?  ");

                context.Wait(MessageReceivedAsync);
            }
        }
    }
}