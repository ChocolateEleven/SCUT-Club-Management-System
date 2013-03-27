using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class LocationAvailableTime
    {
        public int Id { get; set; }

        [Required]
        public int LocatoinId { get; set; }

        [Required]
        public int TimeId { get; set; }

        [Required]
        public int WeekDayId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Time Time { get; set; }
    }
}