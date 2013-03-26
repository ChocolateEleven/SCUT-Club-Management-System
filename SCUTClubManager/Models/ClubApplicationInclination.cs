using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubApplicationInclination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int BranchId { get; set; }
        public int Order { get; set; }
        public virtual ClubBranch Branch { get; set; }
    }
}