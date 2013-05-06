using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class EventDescription
    {
        [Key]
        public int EventId { get; set; }

        [MaxLength(400)]
        [Display(Name = "活动描述")]
        public string Description { get; set; }
    }
}