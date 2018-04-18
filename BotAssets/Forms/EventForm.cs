using System;
using Microsoft.Bot.Builder.FormFlow;

namespace BotAssets.Forms
{
    [Serializable]
    public class EventForm
    {
        [Prompt]
        public string FirstName { get; set; }

        [Prompt]
        public string LastName { get; set; }
        public short ActivityId { get; set; }
        public short LocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short ParticipantMin { get; set; }
        public short ParticipantMax { get; set; }

        public static IForm<EventForm> BuildForm()
        {
            return new FormBuilder<EventForm>()
                    .Message("Welcome to the simple sandwich order bot!")
                    .Build();
        }
    }
}