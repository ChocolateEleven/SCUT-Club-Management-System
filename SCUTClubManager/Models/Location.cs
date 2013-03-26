using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string Name { get; set; }
        public virtual IEnumerable<LocationAvailableTime> AvailableTimes { get; set; }
    }
}