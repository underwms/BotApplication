using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace BotApp.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var availableCommands = $"Here are all the commands available:  {Environment.NewLine}" +
                                    $"{string.Join(",  " + Environment.NewLine, BotAssets.Commands.AvailableCommands)}";

            await context.PostAsync(availableCommands);

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