using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubApplication : Application
    {
        public int RoleId { get; set; }
        public bool IsFlexible { get; set; }
        public ClubRole Role { get; set; }
        public ClubApplicationDetails Details { get; set; }
        public IEnumerable<ClubApplicationInclination> Inclinations { get; set; }
    }
}