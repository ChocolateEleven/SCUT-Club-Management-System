using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class EventOrganizer
    {
        [Required]
        public int EventId { get; set; }

        [Key]
        public int Id { get; set; }
        public virtual Event Event { get; set; }
        public virtual Student Student { get; set; }
    }
}