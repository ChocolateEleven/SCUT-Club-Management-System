using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubInfoModificationApplication : Application
    {
        public virtual ICollection<BranchModification> ModificationBranches { get; set; }

        public int? ClubSubInfoId { get; set; }
        public int? ClubMajorInfoId { get; set; }

        public virtual ClubMajorInfo MajorInfo { get; set; }
        public virtual ClubSubInfo SubInfo { get; set; }
    }
}