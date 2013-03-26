using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubInfoModificationApplication : Application
    {
        public IEnumerable<BranchModification> ModificationBranches { get; set; }
        public int ClubInfoId { get; set; }
        public ClubInfo Info { get; set; }
    }
}