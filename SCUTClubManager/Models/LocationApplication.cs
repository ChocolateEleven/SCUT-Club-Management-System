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

        public DateTime ApplicatedDate { get; set; }

        public int? AssignmentId { get; set; }

        public virtual LocationAssignment Assignment { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Time> Times { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}