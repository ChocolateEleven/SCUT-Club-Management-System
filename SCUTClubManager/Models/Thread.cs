using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Thread
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public virtual IEnumerable<Reply> Replies { get; set; }
        public virtual User Author { get; set; }
    }
}