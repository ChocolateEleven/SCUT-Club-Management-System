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

        //public virtual ICollection<AssetApplication> AssetApplications { get; set; }
        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
        public virtual ICollection<LocationApplication> LocationApplications { get; set; }
        public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
    }
}