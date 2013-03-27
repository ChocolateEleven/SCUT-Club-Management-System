using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class MessageContent
    {
        [Key]
        [Required]
        public int MessageId { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }
    }
}