using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Student : User
    {
        [Required]
        [RegularExpression(@"^\w$",ErrorMessage = "请输入正确的字符")]
        [MaxLength(10,ErrorMessage = "姓氏的长度不能超过10个字符")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\w$", ErrorMessage = "请输入正确的字符")]
        [MaxLength(10, ErrorMessage = "名字的长度不能超过10个字符")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        public string Department { get; set; }

        [Required]
        [MaxLength(20,ErrorMessage = "长度不能超过20个字符")]
        public string Major { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]$")]
        [MaxLength(4)]
        public string Grade { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]$",ErrorMessage = "学位由字母表示")]
        [MaxLength(1)]
        public string Degree { get; set; }


        [RegularExpression(@"^[a-zA-Z]$", ErrorMessage = "政治面貌由字母表示")]
        [MaxLength(1)]
        public string PoliticalId { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }
        public virtual IEnumerable<Application> Applications { get; set; }
        public virtual IEnumerable<ClubMember> MemberShips { get; set; }
        public virtual IEnumerable<Poll> Polls { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
        public virtual IEnumerable<Event> Events { get; set; }
    }
}