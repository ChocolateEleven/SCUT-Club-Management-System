using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ContactInfo
    {

        [Key]
        [Required]
        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string UserName { get; set; }


        [RegularExpression(@"^[0-9]$",
      ErrorMessage = "只能是数字的组合")]
        [MaxLength(13,ErrorMessage = "长度不能超过13个数字")]
        public string Phone { get; set; }


        [RegularExpression(@"^[0-9]$",
      ErrorMessage = "只能是数字的组合")]
        [MaxLength(13, ErrorMessage = "长度不能超过10个数字")]
        public string QQ { get; set; }

        [MaxLength(13, ErrorMessage = "长度不能超过50个字符")]
        public string EMailAddress { get; set; }

        [MaxLength(1)]
        public string Visibility { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过10个字符")]
        public string Room { get; set; }


        public virtual Student Student { get; set; }
    }
}