using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class UserPoll
    {
        public int Id { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string UserName { get; set; }

        public int PollId { get; set; }

        public virtual User User { get; set; }
        public virtual Poll Poll { get; set; }
    }
}