using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubApplicationInclination
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int BranchId { get; set; }
        public int Order { get; set; }
        public ClubBranch Branch { get; set; }
    }
}