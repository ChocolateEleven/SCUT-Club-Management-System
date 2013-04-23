using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class Reply
    {
        public int Id { get; set; }

        [Required]
        public int ThreadId { get; set; }

        [Required(ErrorMessage = "请输入内容")]
        public string Content { get; set; }

        //[RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string AuthorUserName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Number { get; set; }
        public virtual Thread Thread { get; set; }

        [ForeignKey("AuthorUserName")]
        public virtual User Author { get; set; }
    }
}