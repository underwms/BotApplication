using BotAssets;
using BotAssets.Models;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Bot_Application1.Caches;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class ViewEventsDialog : IDialog<object>
    {
        private readonly Dictionary<Int64, string> _cardImageUrls = new Dictionary<Int64, string>()
        {
            [1] = "https://images-na.ssl-images-amazon.com/images/I/41hYHvqrUDL._SL500_AC_SS350_.jpg",
            [2] = "http://casino.harringtonraceway.com/sites/default/files/header-poker_0.jpg",
            [3] = "https://mariokart8.nintendo.com/assets/img/top/char_mario.png"
        };
        
        public async Task StartAsync(IDialogContext context)
        {
            var branchId = context.ConversationData.GetValue<Int16>(StateKeys.BranchKey);

            var events = EventCache.GetAllEvents().Where(e => _cardImageUrls.ContainsKey(e.Activity.ActivityId));

            var replyToConversation = context.MakeMessage();
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = GetCardsAttachments(events);
            
            await context.PostAsync(replyToConversation);
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            Event selectedEvent = null;
            if (JsonHelper.TryParse<Event>(message.Text, out selectedEvent))
            {
                EventCache.UpdateParticpantCount(selectedEvent.EventId);
                context.Done("");
            }
            else
            {
                await context.PostAsync("I'm sorry I don't understand your choice.  ");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private IList<Attachment> GetCardsAttachments(IEnumerable<Event> events)
        {
            var attachments = new List<Attachment>();

            foreach (var @event in events)
            {
                var canJoin = @event.ParticipantMax - @event.ParticipantCount > 0;
                var cardImages = new List<CardImage>()
                    {new CardImage(_cardImageUrls[@event.EventId])};
                    //{new CardImage(_cardImageUrls[@event.Activity.ActivityId])};

                var cardButtons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = "Join",
                        Type = ActionTypes.PostBack,
                        Value = JsonConvert.SerializeObject(@event)
                    }
                };

                var heroCard = new HeroCard()
                {
                    Title = $"{@event.Activity.Description}",
                    Subtitle = $"Where: {@event.Location.Description}  {Environment.NewLine}" +
                               $"Start: {@event.StartDate.ToShortTimeString()} - End {@event.EndDate.ToShortTimeString()}  {Environment.NewLine}" +
                               $"Players Needed: {@event.ParticipantMax - @event.ParticipantCount}",
                    Images = cardImages,
                    Buttons = canJoin ? cardButtons : new List<CardAction>()
                };

                attachments.Add(heroCard.ToAttachment());
            }

            return attachments;
        }
    }
}