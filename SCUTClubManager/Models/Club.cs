using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "社团等级")]
        public int Level { get; set; }

        [Display(Name = "社团资金")]
        public Decimal Fund { get; set; }

        [Display(Name = "成立日期")]
        public DateTime FoundDate { get; set; }

        [Display(Name = "成员人数")]
        public int MemberCount { get; set; }

        [Display(Name = "本学年新增人数")]
        public int NewMemberCount { get; set; }

        public ClubBranch MemberBranch
        {
            get
            {
                foreach (var branch in Branches)
                {
                    if (branch.BranchName == "会员部")
                    {
                        return branch;
                    }
                }

                return null;
            }
        }

        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
        public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
        public virtual ICollection<FundDetails> FundDetails { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ClubMember> Members { get; set; }

        [Display(Name = "部门")]
        public virtual ICollection<ClubBranch> Branches { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        //[Required]
        public int ClubSubInfoId { get; set; }
        //[Required]
        public int ClubMajorInfoId { get; set; }

        public virtual ClubMajorInfo MajorInfo { get; set; }
        public virtual ClubSubInfo SubInfo { get; set; }
    }
}