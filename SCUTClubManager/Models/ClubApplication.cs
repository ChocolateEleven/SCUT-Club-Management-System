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
        [Display(Name = "角色")]
        public int RoleId { get; set; }
        [Required]
        [Display(Name = "可调剂？")]
        public bool IsFlexible { get; set; }
        public virtual ClubRole Role { get; set; }
        public virtual ClubApplicationDetails Details { get; set; }
        public virtual ICollection<ClubApplicationInclination> Inclinations { get; set; }
    }
}