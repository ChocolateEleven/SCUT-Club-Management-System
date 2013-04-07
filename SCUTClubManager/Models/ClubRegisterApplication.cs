using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplication : Application
    {
        public virtual ICollection<ClubRegisterApplicant> Applicants { get; set; }
        public virtual ICollection<BranchModification> Branches { get; set; }

        //[Required]
        public int ClubSubInfoId { get; set; }
        //[Required]
        public int ClubMajorInfoId { get; set; }

        public virtual ClubMajorInfo MajorInfo { get; set; }
        public virtual ClubSubInfo SubInfo { get; set; }

        [MaxLength(256)]
        public string Material { get; set; }
    }
}