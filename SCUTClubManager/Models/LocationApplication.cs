using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SCUTClubManager.Models
{
    public class LocationApplication : Application
    {
        public int? SubEventId { get; set; }

        public DateTime ApplicatedDate { get; set; }

        public int? AssignmentId { get; set; }

        [JsonIgnore]
        public virtual LocationAssignment Assignment { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        [JsonIgnore]
        public virtual ICollection<Time> Times { get; set; }

        [JsonIgnore]
        public virtual SubEvent SubEvent { get; set; }
    }
}