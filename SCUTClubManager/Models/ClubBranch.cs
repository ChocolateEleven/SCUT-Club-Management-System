using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubBranch
    {
        public int Id { get; set; }

        [Required]
        public int ClubId { get; set; }

        [MaxLength(20)]
        public string BranchName { get; set; }

        public virtual ICollection<ClubMember> Members { get; set; }

        public virtual Club Club { get; set; }

        [Required]

        public int MemberCount { get; set; }

        [Required]
        public int NewMemberCount { get; set; }
    }
}