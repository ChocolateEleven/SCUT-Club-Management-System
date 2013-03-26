using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class LocationApplication
    {
        public int LocationId { get; set; }
        public int TimeId { get; set; }
        public int SubEventId { get; set; }
        public Location Location { get; set; }
        public Time Time { get; set; }
        public SubEvent SubEvent { get; set; }
    }
}