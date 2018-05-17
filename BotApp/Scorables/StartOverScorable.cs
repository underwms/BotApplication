using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Scorables.Internals;

namespace BotApp.Scorables
{
    public class StartOverScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public StartOverScorable(IDialogTask task) =>
            SetField.NotNull(out task, nameof(task), task);

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals(BotAssets.Commands.StartOver, StringComparison.InvariantCultureIgnoreCase))
                { return message.Text; }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state) =>
            state != null;
        
        protected override double GetScore(IActivity item, string state) =>
            1.0;

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token) =>
            task.Reset();
        
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token) =>
            Task.CompletedTask;
    }
}