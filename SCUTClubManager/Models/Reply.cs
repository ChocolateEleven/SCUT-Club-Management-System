using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Reply
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ThreadId { get; set; }
        public string Content { get; set; }


        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string AuthorUserName { get; set; }

        public DateTime Date { get; set; }
        public int Number { get; set; }
        public virtual Thread Thread { get; set; }
        public virtual User Author { get; set; }
    }
}