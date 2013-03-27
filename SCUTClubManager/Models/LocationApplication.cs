using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class LocationApplication : Application
    {
        [Required]
        public int LocationId { get; set; }

        [Required]
        public int TimeId { get; set; }

        public int SubEventId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Time Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}