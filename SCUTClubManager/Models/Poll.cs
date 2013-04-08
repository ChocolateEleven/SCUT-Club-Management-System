using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Poll
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage="请输入标题")]
        public string Title { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage="请输入问题描述")]
        public string Question { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string AuthorUserName { get; set; }

        [Required(ErrorMessage="请输入投票开始时间")]
        public DateTime OpenDate { get; set; }

        [Required(ErrorMessage="请输入投票结束时间")]
        public DateTime CloseDate { get; set; }
        public bool IsMultiSelectable { get; set; }

        public virtual User Author { get; set; }
        public virtual ICollection<PollItem> Items { get; set; }
    }
}