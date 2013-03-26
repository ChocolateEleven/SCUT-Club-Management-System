using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubBranch
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ClubId { get; set; }

        [MaxLength(20)]
        public string BranchName { get; set; }
        public IEnumerable<ClubMember> Members { get; set; }

        [Required]
        public int MemberCount { get; set; }

        [Required]
        public int NewMemberCount { get; set; }
    }
}