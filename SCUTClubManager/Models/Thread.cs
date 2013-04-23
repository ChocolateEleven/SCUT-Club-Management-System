using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class Thread
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }

        //[RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string AuthorUserName { get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        [Required]
        public DateTime LatestReplyDate { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }

        [ForeignKey("AuthorUserName")]
        public virtual User Author { get; set; }
    }
}