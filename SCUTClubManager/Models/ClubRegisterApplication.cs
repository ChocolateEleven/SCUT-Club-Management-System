using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplication : Application
    {
        [Display(Name = "申请人列表")]
        public virtual ICollection<ClubRegisterApplicant> Applicants { get; set; }

        [Display(Name = "社团架构")]
        public virtual ICollection<BranchModification> Branches { get; set; }

        //[Required]
        public int ClubSubInfoId { get; set; }
        //[Required]
        public int ClubMajorInfoId { get; set; }

        [Display(Name = "基本信息")]
        public virtual ClubMajorInfo MajorInfo { get; set; }

        [Display(Name = "次要信息")]
        public virtual ClubSubInfo SubInfo { get; set; }

        [MaxLength(256)]
        [Display(Name = "申请材料")]
        public string Material { get; set; }
    }
}