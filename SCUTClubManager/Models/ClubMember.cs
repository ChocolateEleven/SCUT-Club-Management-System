using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubMember
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int ClubId { get; set; }
        public int BranchId { get; set; }
        public DateTime JoinDate { get; set; }
        public int ClubRoleId { get; set; }
        public virtual Student Student { get; set; }
        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual ClubBranch Branch { get; set; }
    }
}