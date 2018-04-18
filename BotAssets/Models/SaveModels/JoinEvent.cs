using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BotAssets.Models.SaveModels
{
    [XmlRoot(ElementName = "ROOT")]
    public class JoinEvent
    {
        public Int64 EventId { get; set; }
        public int ParticipantId { get; set; }
    }
}
