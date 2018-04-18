using BotApp.Caches;
using BotAssets.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;

namespace BotApp.Forms
{
    [Serializable]
    public class EventForm
    {
        private Int16 _branchId;

        public EventForm(Int16 branchId) =>
            _branchId = branchId;

        [Prompt("Please enter your {&}")]
        public string FirstName { get; set; }

        [Prompt("Please enter your {&}")]
        public string LastName { get; set; }

        [Prompt("Please enter your {&}")]
        public string Email { get; set; }

        [Prompt("What Activity do you want to plan?  {||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public List<BotAssets.Models.Activity> Activities { get; set; }

        [Prompt("Where do you want to have it take place?  {||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public List<Location> Locations { get; set; }

        [Prompt("When does it start?")]
        public DateTime StartDate { get; set; }

        [Prompt("When does it end?")]
        public DateTime EndDate { get; set; }

        [Prompt("What the minimum number of people you need?")]
        [Numeric(1, int.MaxValue)]
        public short ParticipantMin { get; set; }

        [Prompt("What the maximum number of people you need?")]
        [Numeric(2, int.MaxValue)]
        public short ParticipantMax { get; set; }

        public static IForm<EventForm> BuildForm()
        {
            OnCompletionAsyncDelegate<EventForm> createEvent = async (context, state) =>
                await context.PostAsync("Ok, I am adding the event.");
            
            return new FormBuilder<EventForm>()
                .Message("Lets create an event!")
                .Field(nameof(FirstName))
                .Field(nameof(LastName))
                .Field(nameof(Email))
                .Field(new FieldReflector<EventForm>(nameof(Activities))
                    .SetType(null)
                    .SetDefine(async (state, field) => {
                        var activities = ActivityCahce.GetAllActivities();
                        foreach(var activity in activities)
                        {
                            field
                                .AddDescription(activity, activity.Description)
                                .AddTerms(activity, activity.Description);
                        }

                        return true;
                    })
                )
                .Field(new FieldReflector<EventForm>(nameof(Locations))
                    .SetType(null)
                    .SetDefine(async (state, field) => {
                        var locations = LocationCache.GetLocationsByBranch(state._branchId);
                        foreach (var location in locations)
                        {
                            field
                                .AddDescription(location, location.Description)
                                .AddTerms(location, location.Description);
                        }

                        return true;
                    })
                )
                .AddRemainingFields()
                .OnCompletion(createEvent)
                .Build();
        }
    }
}