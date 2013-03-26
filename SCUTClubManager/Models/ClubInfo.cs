using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubInfo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Principle { get; set; }

        [MaxLength(100)]
        public string Purpose { get; set; }

        [MaxLength(100)]
        public string Range { get; set; }


        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string Instructor { get; set; }

        [MaxLength(40)]
        public string Address { get; set; }

        [MaxLength(256)]
        public string PosterUrl { get; set; }
        public virtual ClubInfoDetails Details { get; set; }
    }
}