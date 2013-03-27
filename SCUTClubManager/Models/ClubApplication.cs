using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubApplication : Application
    {
        [Required]
        public int RoleId { get; set; }
        public bool IsFlexible { get; set; }
        public virtual ClubRole Role { get; set; }
        public virtual ClubApplicationDetails Details { get; set; }
        public virtual ICollection<ClubApplicationInclination> Inclinations { get; set; }
    }
}