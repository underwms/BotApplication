using BotApp.Caches;
using BotApp.Forms;
using BotAssets;
using BotAssets.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotApp.Dialogs
{
    [Serializable]
    public class EventRootDialog : IDialog<object>
    {
        private readonly IDialogFactory _dialogFactory;
        private List<string> AvailableCommands = new List<string>()
        {
            Commands.ViewEvents,
            Commands.CreateEvent
        };

        public EventRootDialog(IDialogFactory dialogFactory) =>
            SetField.NotNull(out _dialogFactory, nameof(dialogFactory), dialogFactory);
        
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("What do you want to do now?");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var command = AvailableCommands.FirstOrDefault(c => c.Equals(message.Text, StringComparison.InvariantCultureIgnoreCase));

            switch (command)
            {
                case Commands.ViewEvents:
                    await ViewEvents(context);
                    break;
                case Commands.CreateEvent:
                    await CreateEvent(context);
                    break;
                default:
                    await DontUnderstand(context);
                    break;
            }
        }

        private async Task ViewEvents(IDialogContext context) =>
            context.Call(_dialogFactory.Create<ViewEventsDialog>(), this.AfterEventSelected);

        private async Task CreateEvent(IDialogContext context)
        {
            var branchId = context.ConversationData.GetValue<Int16>(StateKeys.BranchKey);
            var eventForm = new FormDialog<EventForm>(new EventForm(branchId), EventForm.BuildForm, FormOptions.PromptInStart);
            context.Call(eventForm, this.AfterFormComplegte);
        }

        private async Task AfterFormComplegte(IDialogContext context, IAwaitable<EventForm> result)
        {
            var @event = await result;
            
            EventCache.AddEvent(new Event()
            {
                EventId = EventCache.GetAllEvents().Max(x => x.EventId) + 1,
                Activity = @event.Activities.ElementAt(0),
                Location = @event.Locations.ElementAt(0),
                Organnizer = new Participant()
                {
                    FirstName = @event.FirstName,
                    LastName = @event.LastName,
                    Email = @event.Email
                },
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                ParticipantMin = @event.ParticipantMin,
                ParticipantMax = @event.ParticipantMax
            });

            await context.PostAsync("Ok we'll keep you informed about your event.  What do you want to do now?");
            context.Wait(MessageReceivedAsync);
        }

        private async Task AfterEventSelected(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("What do you want to do now?");

            context.Wait(MessageReceivedAsync);
        }

        private async Task DontUnderstand(IDialogContext context)
        {
            await context.PostAsync($"I'm sorry, I don't understand your reply.  " );

            context.Wait(MessageReceivedAsync);
        }
    }
}