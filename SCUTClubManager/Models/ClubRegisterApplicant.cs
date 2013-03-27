using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplicant
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string ApplicantUserName { get; set; }

        [Required]
        public bool IsMainApplicant { get; set; }
        public virtual ClubRegisterApplicantDescription Description { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        public virtual Student Applicant { get; set; }

        public virtual ClubRegisterApplication Application { get; set; }
    }
}