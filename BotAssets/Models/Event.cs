﻿using System;

namespace BotAssets.Models
{
    public class Event
    {
        public Int64 EventId { get; set; }
        public Participant Organnizer { get; set; }
        public Activity Activity { get; set; }
        public Location Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int16 ParticipantMin { get; set; }
        public Int16 ParticipantMax { get; set; }
        public int ParticipantCount { get; set; }
    }
}
