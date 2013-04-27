using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class User
    {
        [Key]
        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",
      ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [RegularExpression(@"^[\u2E80-\u9FFFa-zA-Z]{1,10}$", ErrorMessage = "请输入正确的字符")]
        [MaxLength(10, ErrorMessage = "姓名的长度不能超过10个字符")]
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        public string Name { get; set; }

        [Required]
        //[RegularExpression(@"^[a-z0-9A-Z]{1,15}$",
        //ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(32, ErrorMessage = "密码的长度不能超过15个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public virtual UserRole Role { get; set; }

        public virtual ICollection<Poll> Polls { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
    }
}