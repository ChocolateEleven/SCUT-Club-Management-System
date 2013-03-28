using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public abstract class Application
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }


        public virtual Club Club { get; set; }
        public virtual Student Applicant { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string ApplicantUserName { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationRejectReason RejectReason { get; set; }
    }
}