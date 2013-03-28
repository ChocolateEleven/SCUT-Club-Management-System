using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Thread
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string AuthorUserName { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
        public virtual User Author { get; set; }
    }
}