using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models.View_Models
{
    public class ClubScopedAuthenticationModel
    {
        public int ClubId { get; set; }
        public int BranchId { get; set; }
        public string ClubRole { get; set; }
    }
}