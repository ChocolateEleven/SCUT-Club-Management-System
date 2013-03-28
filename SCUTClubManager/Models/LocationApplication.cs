using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class LocationApplication : Application
    {
        public int? SubEventId { get; set; }

        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<Time> Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}