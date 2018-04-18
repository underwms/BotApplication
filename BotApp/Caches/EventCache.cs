using BotAssets.Models;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Caches
{
    public static class EventCache
    {
        private static ConcurrentDictionary<Int64, Event> Events = new ConcurrentDictionary<Int64, Event>();

        public static void InitCache()
        {
            var events = new List<Event>()
            { 
                new Event()
                { 
                    EventId = 1,
                    Organnizer = new Participant()
                    {
                        ParticipantId = 1,
                        FirstName = "Mike",
                        LastName = "Underwood",
                        Email = "munderwood@cardinalsolutions.com"
                    },
                    Activity = new BotAssets.Models.Activity() 
                    { 
                        ActivityId = 1,
                        Description = "Table Soccer" 
                    },
                    Location = new Location()
                    {
                        LocationId = 1,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 1,
                            City = "Cincinnati",
                            State = "OH"
                        }
                    },
                    ParticipantCount = 1,
                    ParticipantMax = 2,
                    ParticipantMin = 2,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(1)
                },
                new Event()
                { 
                    EventId = 2,
                    Organnizer = new Participant()
                    {
                        ParticipantId = 1,
                        FirstName = "Mike",
                        LastName = "Underwood",
                        Email = "munderwood@cardinalsolutions.com"
                    },
                    Activity = new BotAssets.Models.Activity() 
                    { 
                        ActivityId = 2,
                        Description = "Cards" 
                    },
                    Location = new Location()
                    {
                        LocationId = 1,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 1,
                            City = "Cincinnati",
                            State = "OH"
                        }
                    },
                    ParticipantCount = 1,
                    ParticipantMax = 2,
                    ParticipantMin = 2,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(1)
                },
                new Event()
                { 
                    EventId = 3,
                    Organnizer = new Participant()
                    {
                        ParticipantId = 1,
                        FirstName = "Mike",
                        LastName = "Underwood",
                        Email = "munderwood@cardinalsolutions.com"
                    },
                    Activity = new BotAssets.Models.Activity() 
                    { 
                        ActivityId = 3,
                        Description = "Mario Kart" 
                    },
                    Location = new Location()
                    {
                        LocationId = 1,
                        Description = "Breakroom",
                        Branch = new Branch()
                        {
                            BranchId = 1,
                            City = "Cincinnati",
                            State = "OH"
                        }
                    },
                    ParticipantCount = 1,
                    ParticipantMax = 4,
                    ParticipantMin = 2,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(1)
                }
            };
            
            foreach(var @event in events)
            { Events.TryAdd(@event.EventId, @event); }
        }
       
        public static IEnumerable<Event> GetAllEvents() =>
            Events.Values;

        public static void AddEvent(Event @event) =>
            Events.TryAdd(@event.EventId, @event);

        public static void UpdateParticpantCount(Int64 eventId)
        {
            var @event = Events[eventId];
            @event.ParticipantCount++;

            Events.TryUpdate(eventId, @event, Events[eventId]);
        }

    }
}