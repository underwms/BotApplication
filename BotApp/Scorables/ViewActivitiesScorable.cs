using BotApp.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotApp.Scorables
{
    public class ViewActivitiesScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogFactory _dialogFactory;
        private readonly IDialogTask _task;

        public ViewActivitiesScorable(IDialogTask task, IDialogFactory dialogFactory)
        {
            SetField.NotNull(out _task, nameof(task), task);
            SetField.NotNull(out _dialogFactory, nameof(dialogFactory), dialogFactory);
        }

        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals(BotAssets.Commands.ViewActivities, StringComparison.InvariantCultureIgnoreCase))
                { return message.Text; }
            }

            return null;
        }

        protected override bool HasScore(IActivity item, string state) =>
            state != null;
        
        protected override double GetScore(IActivity item, string state) =>
            1.0;

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null)
            {
                var dialog = _dialogFactory.Create<ViewActivitiesDialog>();

                var interruption = dialog.Void<object, IMessageActivity>();

                _task.Call(interruption, null);

                await _task.PollAsync(token);
            }
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token) =>
            Task.CompletedTask;
    }
}