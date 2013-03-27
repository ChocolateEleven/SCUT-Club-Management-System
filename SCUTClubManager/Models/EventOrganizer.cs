using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class EventOrganizer
    {
        [Required]
        public int EventId { get; set; }

        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        public virtual Event Event { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}