using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubMember
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string UserName { get; set; }

        [Required]
        public int ClubId { get; set; }

        public int BranchId { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public int ClubRoleId { get; set; }


        public virtual Student Student { get; set; }
        public virtual Club Club { get; set; }
        public virtual ClubRole ClubRole { get; set; }
        public virtual ClubBranch Branch { get; set; }
    }
}