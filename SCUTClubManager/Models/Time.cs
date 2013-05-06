using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Time
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string TimeName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsCoveredBy(TimeSpan start_time, TimeSpan end_time)
        {
            return start_time >= StartTime && start_time <= EndTime || end_time >= StartTime && end_time <= EndTime || start_time <= StartTime && end_time >= EndTime;
        }

        //public virtual ICollection<AssetApplication> AssetApplications { get; set; }
        //public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
        //public virtual ICollection<LocationApplication> LocationApplications { get; set; }
        //public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
    }
}