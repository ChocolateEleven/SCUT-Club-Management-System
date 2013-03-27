using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Title { get; set; }

        [Required]
        public int ClubId { get; set; }

        [Required]
        public int ChiefEventOrganizerId { get; set; }

        [Range(0,100)]
        public int Score { get; set; }

        [MaxLength(256)]
        public string PosterUrl { get; set; }

        [MaxLength(256)]
        public string PlanUrl { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }
        public virtual Club Club { get; set; }
        public virtual EventDescription Description { get; set; }
        public virtual ICollection<EventOrganizer> Organizers { get; set; }
        public virtual ICollection<SubEvent> SubEvents { get; set; }

        [ForeignKey("ChiefEventOrganizerId")]
        public virtual EventOrganizer ChiefEventOrganizer { get; set; }
        public virtual DateTime Date { get; set; }
    }
}