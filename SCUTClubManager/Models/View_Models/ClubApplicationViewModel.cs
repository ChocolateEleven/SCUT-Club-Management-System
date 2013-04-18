using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models.View_Models
{
    public class ClubApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public string ApplicantUserName { get; set; }
        public string ApplicantName { get; set; }
        public int AppliedBranchId { get; set; }
        public string AppliedBranchName { get; set; }
        public int AppliedRoleId { get; set; }
        public string AppliedRoleName { get; set; }
    }
}