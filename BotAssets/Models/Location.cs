using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotAssets.Models
{
    public class Location
    {
        public Int16 LocationId { get; set; }
        public string Description { get; set; }
        public Branch Branch { get; set; }
    }
}
