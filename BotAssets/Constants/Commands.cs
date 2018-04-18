using System.Collections.Generic;

namespace BotAssets
{
    public class Commands
    {
        public const string Help = "Help";
        public const string ViewBranches = "View Branches";
        public const string ViewLocations = "View Locations";
        public const string ViewActivities = "View Activities";

        public const string ViewEvents = "View Events";
        public const string CreateEvent = "Create Event";

        public static List<string> AvailableCommands = new List<string>()
        {
            Help,
            ViewBranches,
            ViewLocations,
            ViewActivities,
            ViewEvents,
            CreateEvent
        };
    }
}
