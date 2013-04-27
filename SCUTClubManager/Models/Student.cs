using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Student : User
    {
        public const string MALE = "m";
        public const string FEMALE = "f";
        public const string BACHELOR = "b";
        public const string MASTER = "m";
        public const string PHD = "p";

        //[Required]
        [StringLength(1)]
        [Display(Name = "性别")]
        public string Gender { get; set; }

        [Display(Name = "出生日期")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        //[Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        [Display(Name = "学院")]
        public string Department { get; set; }

        //[Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        [Display(Name = "专业")]
        public string Major { get; set; }

        //[Required]
        [MaxLength(10)]
        [Display(Name = "年级")]
        public string Grade { get; set; }

        //[Required]
        [RegularExpression(@"^[a-zA-Z]{1}$",ErrorMessage = "学位由字母表示")]
        [MaxLength(1)]
        [Display(Name = "学位")]
        //"b"表示本科 "m"表示硕士 "p"表示博士
        public string Degree { get; set; }

        [RegularExpression(@"^[a-zA-Z]{1}$", ErrorMessage = "政治面貌由字母表示")]
        [MaxLength(1)]
        [Display(Name = "政治面目")]
        //"q"表示群众 "d"表示共产党员 "y"表示预备党员 "t"表示共青团员 "o"表示其他党派
        public string PoliticalId { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<ClubMember> MemberShips { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
    }
}