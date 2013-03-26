using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplication : Application
    {
        public IEnumerable<ClubRegisterApplicant> Applicants { get; set; }
        public IEnumerable<BranchModification> Branches { get; set; }
        public int ClubInfoId { get; set; }
        public ClubInfo Info { get; set; }
        public string Material { get; set; }
    }
}