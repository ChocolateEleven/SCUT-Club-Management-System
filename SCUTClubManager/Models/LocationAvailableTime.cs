using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class LocationAvailableTime
    {
        public int Id { get; set; }
        public int LocatoinId { get; set; }
        public int TimeId { get; set; }
        public int WeekDayId { get; set; }
        public virtual Location Location { get; set; }
        public virtual Time Time { get; set; }
    }
}