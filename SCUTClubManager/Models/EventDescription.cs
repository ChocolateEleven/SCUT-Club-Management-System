using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class EventDescription
    {
        [Key]
        public int EventId { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
        public virtual Event Event { get; set; }
    }
}