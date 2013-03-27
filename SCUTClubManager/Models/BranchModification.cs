using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class BranchModification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [MaxLength(20)]
        [Required]
        public string BranchName { get; set; }

        [MaxLength(1)]
        public string Type { get; set; }
        public virtual ClubBranch OrigBranch { get; set; }
    }
}