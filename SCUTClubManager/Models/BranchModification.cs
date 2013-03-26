using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class BranchModification
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int ApplicationId { get; set; }
        public string BranchName { get; set; }
        public string Type { get; set; }
        public ClubBranch OrigBranch { get; set; }
    }
}