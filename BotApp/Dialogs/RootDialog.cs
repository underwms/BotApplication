using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot_Application1.Caches;
using Bot_Application1.Properties;
using BotAssets;
using BotAssets.Models;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IDialogFactory _dialogFactory;

        public RootDialog(IDialogFactory dialogFactory)
        {
            SetField.NotNull(out _dialogFactory, nameof(dialogFactory), dialogFactory);
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var scrubedInput = message.Text //this kind of scrubbing can get out of hand, this is a great place to get some AI up in here
                .Trim()
                .Split(new char[] { ',', ' ' })
                .Select(part => part.Trim());

            var branches = LocationCache.FindBranches(scrubedInput);

            if (branches.Count() == 1)
            {
                context.ConversationData.Clear();
                context.ConversationData.SetValue(StateKeys.BranchKey, branches.ElementAt(0).BranchId);
                await MoveToEventRoot(context, branches.ElementAt(0));
            }
            else
            { await StartOverAsync(context, $"{BotDialog.RootDialog_Error}  {BotDialog.RootDialog_Intro}"); }
        }

        private async Task MoveToEventRoot(IDialogContext context, Branch selectedBranch)
        {
            await context.PostAsync($"Thank you, your default branch is {selectedBranch.City}, {selectedBranch.State}");
            var locationDialog = _dialogFactory.Create<EventRootDialog>();

            context.Call(locationDialog, this.ViewDialogResumeAfter);
        }

        private async Task ViewDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            { await StartOverAsync(context, BotDialog.RootDialog_Intro); }
            catch (TooManyAttemptsException)
            { await StartOverAsync(context, $"{BotDialog.RootDialog_Error}  {BotDialog.RootDialog_Intro}"); }
        }

        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            message.TextFormat = TextFormatTypes.Plain;
            message.SuggestedActions = new SuggestedActions()
            {
                Actions = LocationCache.GetAllBranches().Select(b =>
                new CardAction()
                {
                    Title = $"{b.City}, {b.State}  ",
                    Type = ActionTypes.ImBack,
                    Value = $"{b.City}, {b.State}  "
                }).ToList()
            };

            await context.PostAsync(message);
        }
    }
}