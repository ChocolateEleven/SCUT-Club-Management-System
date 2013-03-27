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
        public virtual IEnumerable<AssetAssignment> AssetAssignments { get; set; }
        public virtual IEnumerable<LocationAssignment> LocationAssignments { get; set; }
        public virtual IEnumerable<FundDetails> FundDetails { get; set; }
        public virtual IEnumerable<Event> Events { get; set; }
        public virtual IEnumerable<ClubMember> Members { get; set; }
        public virtual IEnumerable<ClubBranch> Branches { get; set; }

        [Required]
        public int ClubInfoId { get; set; }

        public virtual ClubInfo Info { get; set; }
    }
}