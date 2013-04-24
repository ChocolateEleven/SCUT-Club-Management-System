using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Student : User
    {
        //[Required]
        [StringLength(1)]
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        //[Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        public string Department { get; set; }

        //[Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        public string Major { get; set; }

        //[Required]
        [RegularExpression(@"^[0-9]{1,4}$")]
        [MaxLength(4)]
        public string Grade { get; set; }

        //[Required]
        [RegularExpression(@"^[a-zA-Z]{1}$",ErrorMessage = "学位由字母表示")]
        [MaxLength(1)]
        //"b"表示本科 "m"表示硕士 "p"表示博士
        public string Degree { get; set; }

        [RegularExpression(@"^[a-zA-Z]{1}$", ErrorMessage = "政治面貌由字母表示")]
        [MaxLength(1)]
        //"q"表示群众 "d"表示共产党员 "y"表示预备党员 "t"表示共青团员 "o"表示其他党派
        public string PoliticalId { get; set; }

        //public string FullName
        //{
        //    get
        //    {
        //        return LastName  + FirstName;
        //    }
        //}

        public virtual ContactInfo ContactInfo { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<ClubMember> MemberShips { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<LocationAssignment> LocationAssignments { get; set; }
        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; }
    }
}