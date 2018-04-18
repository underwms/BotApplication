using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BotAssets.Models.SaveModels
{
    [XmlRoot(ElementName = "Event")]
    public class EventSave
    {
        public int ParticipantId { get; set; }
        public Int16 ActivityId { get; set; }
        public Int16 LocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int16 ParticipantMin { get; set; }
        public Int16 ParticipantMax { get; set; }
    }
}
