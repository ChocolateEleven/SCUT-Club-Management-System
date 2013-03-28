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
        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9A-Z]$",
ErrorMessage = "密码只能是数字和字母的组合")]
        [MaxLength(15, ErrorMessage = "密码的长度不能超过15个字符")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,10}$")]
        [MaxLength(10)]
        public string Role { get; set; }

        //public virtual ICollection<Message> SentMessages { get; set; }
        //public virtual ICollection<Message> ReceivedMessages { get; set; }
    }
}