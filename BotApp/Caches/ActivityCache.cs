using BotAssets.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BotApp.Caches
{
    public static class ActivityCahce
    {
        private static ConcurrentDictionary<Int16, Activity> Activities = new ConcurrentDictionary<Int16, Activity>();

        public static void InitCache()
        {
            if (!Activities.Any())
            {
                var activities = new List<Activity>()
                {
                    new Activity() { ActivityId = 1, Description = "Table Soccer" },  
                    new Activity() { ActivityId = 2, Description = "Cards" },  
                    new Activity() { ActivityId = 3, Description = "Mario Kart" }    
                };

                foreach(var activity in activities)
                { Activities.TryAdd(activity.ActivityId, activity); }
            }
        }
       
        public static IEnumerable<Activity> GetAllActivities() =>
            Activities.Values;
    }
}