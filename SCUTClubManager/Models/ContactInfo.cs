using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class ContactInfo
    {
        [Key]
        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string UserName { get; set; }


        [RegularExpression(@"^[0-9]{1,13}$", ErrorMessage = "只能是数字的组合")]
        [MaxLength(13,ErrorMessage = "长度不能超过13个数字")]
        public string Phone { get; set; }


        [RegularExpression(@"^[0-9]{1,10}$",  ErrorMessage = "只能是数字的组合")]
        [MaxLength(10, ErrorMessage = "长度不能超过10个数字")]
        public string QQ { get; set; }

        [MaxLength(50, ErrorMessage = "长度不能超过50个字符")]
        public string EMailAddress { get; set; }

       [MaxLength(1)]
        //"s"表示社团内可见 "a"表示所有人可见
        public string Visibility { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z-]{1,10}$",ErrorMessage = "只能是数字和字母的组合")]
        [MaxLength(10, ErrorMessage = "宿舍号的长度不能超过10个字符")]
        public string Room { get; set; }
    }
}