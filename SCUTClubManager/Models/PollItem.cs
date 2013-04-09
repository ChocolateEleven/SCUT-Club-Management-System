using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
 
namespace SCUTClubManager.Models
{
    public class PollItem
    {
        public int Id { get; set; }
        public int PollId { get; set; }
         
        [MaxLength(50)]
        [Required(ErrorMessage="请输入选项内容")]
        public string Caption { get; set; }
        public int Count { get; set; }
        public virtual Poll Poll { get; set; }
    }
}