using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplication : Application
    {
        public virtual IEnumerable<ClubRegisterApplicant> Applicants { get; set; }
        public virtual IEnumerable<BranchModification> Branches { get; set; }
        public int ClubInfoId { get; set; }
        public virtual ClubInfo Info { get; set; }
        public string Material { get; set; }
    }
}