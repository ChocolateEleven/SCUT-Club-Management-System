using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required]
        public int Level { get; set; }
        public Decimal Fund { get; set; }
        public DateTime FoundDate { get; set; }
        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
        public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
        public virtual ICollection<FundDetails> FundDetails { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ClubMember> Members { get; set; }
        public virtual ICollection<ClubBranch> Branches { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        [Required]
        public int ClubInfoId { get; set; }

        public virtual ClubInfo Info { get; set; }
    }
}