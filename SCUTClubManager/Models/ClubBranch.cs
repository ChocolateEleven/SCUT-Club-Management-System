using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubBranch
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string BranchName { get; set; }
        public IEnumerable<ClubMember> Members { get; set; }
        public int MemberCount { get; set; }
        public int NewMemberCount { get; set; }
    }
}