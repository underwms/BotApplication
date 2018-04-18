using Autofac;
using Bot_Application1.Caches;
using Bot_Application1.Properties;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    var dialog = scope.Resolve<IDialog<object>>(TypedParameter.From(""));
                    await Conversation.SendAsync(activity, () => dialog);
                };
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.ConversationUpdate)
            {
                //check and see if the bot was added to trigger an automatic greeting
                if (message.MembersAdded.Any(member => member.Id == message.Recipient.Id))
                {

                    var reply = message.CreateReply($"Hello, {BotDialog.RootDialog_Intro}");
                    reply.Type = ActivityTypes.Message;
                    reply.TextFormat = TextFormatTypes.Plain;
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = LocationCache.GetAllBranches().Select(b => 
                        new CardAction() {
                            Title = $"{b.City}, {b.State}  ",
                            Type = ActionTypes.ImBack,
                            Value = $"{b.City}, {b.State}  "
                        }).ToList()
                    };

                    var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            //else if (message.Type == ActivityTypes.DeleteUserData)
            //{
            //    // Implement user deletion here
            //    // If we handle user deletion, return a real message
            //}
            //else if (message.Type == ActivityTypes.ContactRelationUpdate)
            //{
            //    // Handle add/remove from contact lists
            //    // Activity.From + Activity.Action represent what happened
            //}
            //else if (message.Type == ActivityTypes.Typing)
            //{
            //    // Handle knowing tha the user is typing
            //}
            //else if (message.Type == ActivityTypes.Ping)
            //{
            //}
        }

    }
}