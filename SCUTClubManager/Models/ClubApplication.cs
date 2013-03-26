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
        public virtual ClubRole Role { get; set; }
        public virtual ClubApplicationDetails Details { get; set; }
        public virtual IEnumerable<ClubApplicationInclination> Inclinations { get; set; }
    }
}