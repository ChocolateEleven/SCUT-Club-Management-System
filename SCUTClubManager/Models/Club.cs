using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Club
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public Decimal Fund { get; set; }
        public DateTime FoundDate { get; set; }
        public IEnumerable<AssetAssignment> AssetAssignments { get; set; }
        public IEnumerable<LocationAssignment> LocationAssignments { get; set; }
        public IEnumerable<FundDetails> FundDetails { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<ClubMember> Members { get; set; }
        public IEnumerable<ClubBranch> Branches { get; set; }
        public int ClubInfoId { get; set; }
        public ClubInfo Info { get; set; }
    }
}