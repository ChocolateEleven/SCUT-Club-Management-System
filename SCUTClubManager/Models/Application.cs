using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public abstract class Application
    {
        public const string NOT_VERIFIED = "n";
        public const string PASSED = "p";
        public const string FAILED = "f";
        public const int ALL = 0;
        public const int FLEXIBLE = -1;

        [Key]
        public int Id { get; set; }

        public int? ClubId { get; set; }

        [MaxLength(1)]
        [Display(Name = "申请状态")]
        //n,p,f 见上面常量
        public string Status { get; set; }

        [Display(Name = "申请社团")]
        public virtual Club Club { get; set; }

        [Display(Name = "申请人")]
        public virtual Student Applicant { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",
      ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string ApplicantUserName { get; set; }

        [Display(Name = "申请日期")]
        public DateTime Date { get; set; }

        [Display(Name = "驳回理由")]
        public virtual ApplicationRejectReason RejectReason { get; set; }
    }
}