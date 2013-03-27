using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class LocationApplication : Application
    {
        public int LocationId { get; set; }
        public int TimeId { get; set; }
        public int SubEventId { get; set; }
        public virtual Location Location { get; set; }
        public virtual Time Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}